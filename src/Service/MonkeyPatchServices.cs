using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Quantum.MonkeyPatch.Service;

public class MonkeyPatchServices : IMonkeyPatchServices
{
    private readonly QuantumMonkeyPatchDbContext _context;

    public MonkeyPatchServices(QuantumMonkeyPatchDbContext context)
        => _context = context;

    public async Task CreateObject(CreateQuantumObjectCommand cmd)
    {
        if (await _context.Objects.AnyAsync(a => a.Name == cmd.Name))
            throw new QuantumObjectNameDuplicationException(cmd.Name);


        var builder = QuantumObject.Builder.Name(cmd.Name).WithID(Snowflake.SnowflakeIdGenerator.New().ToLong());

        foreach (var prop in cmd.Properties)
        {
            QuantumPropertyType quantumPropertyType = null;
            switch (prop.Type)
            {
                case QuantumPrimitiveTypes.Number:
                    quantumPropertyType = new QuantumNumberType(prop.Required);
                    break;
                case QuantumPrimitiveTypes.String:
                    quantumPropertyType = new QuantumStringType(prop.Required);
                    break;
                case QuantumPrimitiveTypes.Date:
                    quantumPropertyType = new QuantumDateType(prop.Required);
                    break;
                case QuantumPrimitiveTypes.Blob:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            builder.AddProperty(new QuantumProperty(quantumPropertyType, prop.Name, prop.DisplayName));
            var quantumObject = builder.Build();

            _context.Objects.Add(quantumObject);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteObject(long objectId)
    {
        var quantumObject = await _context.Objects.FindAsync(objectId);
        if (quantumObject is null)
            return;

        if (await _context.ObjectInstances.AnyAsync(a => a.QuantumObjectId == objectId))
            throw new ObjectHasBeenUsedDeletionException(quantumObject.Name);

        _context.Objects.Remove(quantumObject);
        await _context.SaveChangesAsync();
    }

    public async Task<List<QuantumObjectViewModel>> GetAllObjects()
    {
        var contextObjects = await _context.Objects.ToListAsync();
        return contextObjects.Select(o => new QuantumObjectViewModel(o.Id
        , o.Name, o.Properties.Select(a => new QuantumPropertyDto(
            a.Type.Type, a.Type.Required, a.Name, a.DisplayName)).ToList())).ToList();
    }

    public async Task CreateObjectInstance(long objectId, CreateNewInstanceCommand cmd)
    {
        var quantumObject = await _context.Objects.FindAsync(objectId);

        var quantumObjectInstance = quantumObject.CreateNewInstance(Snowflake.SnowflakeIdGenerator.New().ToLong()
            , cmd);

        _context.ObjectInstances.Add(quantumObjectInstance);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteObjectInstance(long id)
    {
        var instance = await _context.ObjectInstances.FindAsync(id);
        if (instance is null)
            return;

        _context.ObjectInstances.Remove(instance);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateObjectInstance(long id, CreateNewInstanceCommand cmd)
    {
        var instance = await _context.ObjectInstances.FindAsync(id);
        if (instance is null)
            return;

        var quantumObject = await _context.Objects.FindAsync(id);

        instance = quantumObject.CreateNewInstance(instance.Id, cmd);

        _context.ObjectInstances.Update(instance);
        await _context.SaveChangesAsync();
    }


    public async Task<List<QuantumObjectInstance>> GetAllObjectInstances(long objectId)
    {
        return await _context
            .ObjectInstances.Where(a => a.QuantumObjectId == objectId).ToListAsync();
    }
}
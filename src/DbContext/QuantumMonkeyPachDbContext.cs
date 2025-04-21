using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Quantum.MonkeyPatch.Service;

public class QuantumMonkeyPatchDbContext : DbContext
{
    public DbSet<QuantumObject> Objects { get; set; }
    public DbSet<QuantumObjectInstance> ObjectInstances { get; set; }

    public QuantumMonkeyPatchDbContext(DbContextOptions<QuantumMonkeyPatchDbContext> options)
    : base(options)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuantumObject>()
            .Property(p => p.Properties)
            .HasConversion(v => Newtonsoft.Json.JsonConvert.SerializeObject(v)
                , v => Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuantumProperty>>(v));

        modelBuilder.Entity<QuantumObjectInstance>()
            .Property(p => p.Properties)
            .HasConversion(v => Newtonsoft.Json.JsonConvert.SerializeObject(v)
                , v => Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, QuantumPropertyInstance>>(v));
        
        modelBuilder.Entity<QuantumObjectInstance>()
            .HasOne(p => p.QuantumObject);

        base.OnModelCreating(modelBuilder);
    }

    
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quantum.MonkeyPatch.Service;

public interface IMonkeyPatchServices
{
    Task CreateObject(CreateQuantumObjectCommand cmd);
    Task DeleteObject(long objectId);
    Task<List<QuantumObjectViewModel>> GetAllObjects();

    Task CreateObjectInstance(long objectId, CreateNewInstanceCommand cmd);
    Task<List<QuantumObjectInstance>> GetAllObjectInstances(long objectId);
    Task DeleteObjectInstance(long id);
    Task UpdateObjectInstance(long id, CreateNewInstanceCommand cmd);
}
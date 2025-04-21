using System.Collections.Generic;
using Quantum.MonkeyPatch.Service;

namespace Quantum.MonkeyPatch;

public record QuantumObjectInstance(long Id, QuantumObject QuantumObject, Dictionary<string, QuantumPropertyInstance> Properties)
{
    private QuantumObjectInstance() : this(0, null, null)
    { }

    public long QuantumObjectId { get; set; } = QuantumObject.Id;
    public QuantumPropertyInstance this[string name] => Properties[name];
    
}
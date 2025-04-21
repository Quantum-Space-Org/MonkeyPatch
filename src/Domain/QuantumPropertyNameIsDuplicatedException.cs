using System;

namespace Quantum.MonkeyPatch;

public class QuantumPropertyNameIsDuplicatedException : Exception
{
    public string TypeName { get; }

    public QuantumPropertyNameIsDuplicatedException(string typeName)
    {
        TypeName = typeName;
    }
}
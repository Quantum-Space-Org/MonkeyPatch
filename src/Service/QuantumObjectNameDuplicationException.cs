using System;

namespace Quantum.MonkeyPatch.Service;

public class QuantumObjectNameDuplicationException : Exception
{
    public string Name { get; }

    public QuantumObjectNameDuplicationException(string name)
    {
        Name = name;
    }
}
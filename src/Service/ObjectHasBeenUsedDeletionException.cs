using System;

namespace Quantum.MonkeyPatch.Service;

public class ObjectHasBeenUsedDeletionException : Exception
{
    public string Name { get; }

    public ObjectHasBeenUsedDeletionException(string name)
    {
        Name = name;
    }
}
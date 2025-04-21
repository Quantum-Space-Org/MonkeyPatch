namespace Quantum.MonkeyPatch;

public abstract record QuantumPropertyType(QuantumPrimitiveTypes Type, bool Required)
{
    public abstract QuantumPropertyInstance NewInstance(object value);
}

public record QuantumPropertyInstance(QuantumPrimitiveTypes Type);

public enum QuantumPrimitiveTypes
{
    Number,
    String,
    Date,
    Blob
}
namespace Quantum.MonkeyPatch;

public record QuantumStringType(bool Required) : QuantumPropertyType(QuantumPrimitiveTypes.String, Required)
{
    public override QuantumPropertyInstance NewInstance(object value)
    {
        return new QuantumStringInstance(value.ToString());
    }
}

public record QuantumStringInstance : QuantumPropertyInstance
{
    public string Value { get; set; }

    public QuantumStringInstance(string value) : base(QuantumPrimitiveTypes.String)
    {
        Value = value;
    }
}
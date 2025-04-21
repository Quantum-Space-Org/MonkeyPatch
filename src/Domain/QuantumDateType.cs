using System;

namespace Quantum.MonkeyPatch;

public record QuantumDateType (bool Required) : QuantumPropertyType(QuantumPrimitiveTypes.Date,Required)
{
    public override QuantumPropertyInstance NewInstance(object value)
    {
        return new QuantumDateInstance(DateTime.Parse(value.ToString()));
    }
}

public record QuantumDateInstance(DateTime Value) : QuantumPropertyInstance(QuantumPrimitiveTypes.Date)
{
    public DateTime Value { get; set; } = Value;
}
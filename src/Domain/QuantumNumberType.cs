using System;
using System.Globalization;

namespace Quantum.MonkeyPatch;

public record QuantumNumberType(bool Required) : QuantumPropertyType(QuantumPrimitiveTypes.Number, Required)
{
    public override QuantumPropertyInstance NewInstance(object value)
    {
        if (value == null)
            throw new ArgumentNullException("value");

        return new QuantumNumberInstance(Convert.ToDecimal(value, CultureInfo.InvariantCulture));
    }

    public QuantumNumberInstance NewInstance()
    {
        if (Required)
            throw new ArgumentNullException("value");

        return new QuantumNumberInstance(0);
    }
}

public record QuantumNumberInstance(decimal Value): QuantumPropertyInstance(QuantumPrimitiveTypes.Number);
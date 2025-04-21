namespace Quantum.MonkeyPatch;

public record QuantumPropertyDto(QuantumPrimitiveTypes Type, bool Required, string Name, string DisplayName = "")
{
}

public record QuantumProperty(QuantumPropertyType Type, string Name, string DisplayName = "")
{
    public string DisplayName { get; init; } = string.IsNullOrWhiteSpace(DisplayName) ? Name : DisplayName;


    public QuantumPropertyInstance CreateInstance(object value)
    {
     return   Type.NewInstance(value);
    }

    public override string ToString()
    {

        return
            DisplayName == Name
                ? $"{Name} "
                : $"{Name} - {DisplayName}";
    }

    
}
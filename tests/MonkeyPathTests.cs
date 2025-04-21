using FluentAssertions;

namespace Quantum.MonkeyPatch.Tests;

public class MonkeyPathTests
{
    [Fact]
    public void quantum_object_should_have_at_least_one_property()
    {
        Action action = () => QuantumBuilder("name").Build();

        action.Should()
            .Throw<Exception>();
    }

    [Fact]
    public void quantum_object_should_have_a_name()
    {
        Action action = () => QuantumBuilder("").Build();

        action.Should()
            .Throw<Exception>();
    }

    [Fact]
    public void createQuantumObjectSuccessfully()
    {
        QuantumPropertyType type = new QuantumNumberType(true);
        var property = Property(type, "number");

        var qObject = QuantumBuilder("name").AddProperty(property).Build();

        qObject.Should()
            .NotBe(null);
    }


    [Fact]
    public void when_creating_a_new_instance_of_a_quantum_object_required_props_must_be_initialized()
    {
        var qObject = QuantumBuilder("name").AddProperty(QuantumProperty(Number())).Build();

        var emptyPropertyValueCommand = new CreateNewInstanceCommand
        {
            Values = new Dictionary<string, object>()
        };

        Action action = () => qObject.CreateNewInstance(Snowflake.SnowflakeIdGenerator.New().ToLong(), emptyPropertyValueCommand);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void creating_a_new_instance_of_a_quantum_object()
    {
        var qObject = QuantumBuilder("name")
            .AddProperty(QuantumProperty(Number(), "number"))
            .AddProperty(new QuantumProperty(new QuantumStringType(true), "FirstName", "نام"))
            .AddProperty(new QuantumProperty(new QuantumDateType(true), "BirthDate", "تاریخ تولد"))
            .Build();

        var dateTime = DateTime.Now;

        var emptyPropertyValueCommand = new CreateNewInstanceCommand
        {
            Values = new Dictionary<string, object>()
            {
                {"number",20.22},
                {"FirstName","Masoud"},
                {"BirthDate",dateTime},
            }
        };

        var instance = qObject.CreateNewInstance(Snowflake.SnowflakeIdGenerator.New().ToLong(), emptyPropertyValueCommand);
        instance.Should().NotBeNull();

        instance["number"]
            .Should().Be(new QuantumNumberInstance(20.22M));

        instance["FirstName"]
            .Should().Be(new QuantumStringInstance("Masoud"));

        instance["BirthDate"]
            .Should().Be(new QuantumDateInstance(dateTime));
    }

    [Fact]
    public void initialize_a_new_instance_of_quantum_number()
    {
        var quantumNumberType = Number();

        var newInstance = quantumNumberType.NewInstance(20.2);
        newInstance.Should().Be(new QuantumNumberInstance(20.2M));

        newInstance = quantumNumberType.NewInstance("20.2");
        newInstance.Should().Be(new QuantumNumberInstance(20.2M));

        quantumNumberType = quantumNumberType with { Required = false };

        newInstance = quantumNumberType.NewInstance();
        newInstance.Should().Be(new QuantumNumberInstance(0M));
    }

    private static QuantumProperty QuantumProperty(QuantumPropertyType type, string name = "prop name", string displayName = "")
        => Property(type, name, displayName);

    private static QuantumNumberType Number()
    {
        return new QuantumNumberType(true)
        {
            Required = true
        };
    }


    private static QuantumProperty Property(QuantumPropertyType type, string name, string displayName = "") =>
        new(type, name, displayName);

    private static QuantumObject.Builder QuantumBuilder(string name)
        => QuantumObject.Builder.Name(name).WithID(Snowflake.SnowflakeIdGenerator.New().ToLong());
}
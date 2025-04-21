using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.MonkeyPatch;

public class QuantumObject
{
    public long Id { get; set; }
    public string Name { get; set; }
    public readonly List<QuantumProperty> Properties = new();

    private QuantumObject(){}
    private QuantumObject(long id, string name, List<QuantumProperty> quantumProperties)
    {
        Id = id;
        Name = name;
        Properties = quantumProperties;
    }

    public class Builder
    {
        private readonly string _name;
        private readonly List<QuantumProperty> _properties = new();
        private long _id = Snowflake.SnowflakeIdGenerator.New().ToLong();

        private Builder(string name)
        {
            _name = name;
        }

        public static Builder Name(string name)
        {
            return new Builder(name);
        }
        public Builder WithID(long id)
        {
            _id = id;
            return this;
        }

        public Builder AddProperty(QuantumProperty type)
        {
            if (_properties.Any(t => t.Name == type.Name))
                throw new QuantumPropertyNameIsDuplicatedException(type.Name);

            _properties.Add(type);
            return this;
        }

        public QuantumObject Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
                throw new ArgumentNullException(nameof(_name));

            if (_properties.Any() is false)
                throw new ArgumentNullException(nameof(_properties));

            return new QuantumObject(_id, _name, _properties);
        }
    }

    public void AddProperty(QuantumProperty type)
    {
        Properties.Add(type);
    }

    public QuantumObjectInstance CreateNewInstance(long id, CreateNewInstanceCommand cmd)
    {
        var properties = new Dictionary<string, QuantumPropertyInstance>();
        foreach (var property in Properties)
        {
            if (cmd.Values.TryGetValue(property.Name, out var p))
            {
                properties[property.Name] = property.CreateInstance(p);
            }
            else if (property.Type.Required)
            {
                throw new ArgumentNullException(property.ToString());
            }
        }

        return new QuantumObjectInstance(id, this, properties);

    }
}
using System.Collections.Generic;

namespace Quantum.MonkeyPatch.Service;

public record CreateQuantumObjectCommand(string Name, List<QuantumPropertyDto> Properties);
public record QuantumObjectViewModel(long id, string Name, List<QuantumPropertyDto> Properties);
using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidRsgVersionException(uint ver, List<uint> expected) : 
InvalidFileVersionException<uint>(ver, $"Unknown RSG Version: ({ver}). " +
$"Allowed Versions are: {string.Join(", ", expected)}", [.. expected] )
{
}

}
using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidRsbVersionException(uint ver, List<uint> expected) : 
InvalidFileVersionException<uint>(ver, $"Unknown RSB Version: {ver}. " +
$"Allowed Versions are: {string.Join(", ", expected)}", [.. expected] )
{
}

}
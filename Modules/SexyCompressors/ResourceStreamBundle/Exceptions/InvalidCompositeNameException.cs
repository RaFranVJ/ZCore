using System;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Exceptions
{
// Exception thrown when Invalid CompositeName is Read

public class InvalidCompositeNameException(string name, string expected) : 
Exception($"Invalid CompositeName: \"{name}\" - Expected: \"{expected}\"")
{
}

}
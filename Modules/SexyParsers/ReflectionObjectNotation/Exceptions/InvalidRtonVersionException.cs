namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidRtonVersionException(uint ver, uint expected) : 
InvalidFileVersionException<uint>(ver, $"This File is Encoded with an unknown Version of the RTON Algorithm\n" +
$"Version detected v{ver}, expected v{expected}", expected)
{
}

}
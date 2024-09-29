namespace ZCore.Modules.SexyCompressors.MarmaladeDZ.Exceptions
{
// Exception thrown when Invalid ver is Read

public class InvalidDzVersionException(byte ver, byte expected) : 
InvalidFileVersionException<byte>(ver, $"This File was Compressed with a unknown Version of the Dz SDK (v{ver})\n" +
$"Allowed Version is v{expected}", expected)
{
}

}
namespace ZCore.Modules.SexyCompressors.StandarMediaFile.Exceptions
{
// Exception thrown when Invalid header is Read

public class InvalidSmfException(long pos, uint header, uint expected) : 
GenericValueMismatchException<uint>(pos, header, expected, 
$"Invalid SMF Magic at {pos}.\nValue read: {header} - Expected: {expected}")
{
}

}
namespace ZCore.Modules.SexyCryptors.PopCapCipheredData.Exceptions
{
// Exception thrown when a CDAT File differs from expected Size

public class CdatSizeMismatchException(long sizeX, long sizeY) : FileSizeMismatchException(sizeX, sizeY)
{
}

}
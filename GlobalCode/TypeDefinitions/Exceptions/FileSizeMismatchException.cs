using System;

public class FileSizeMismatchException(long sizeX, long sizeY) : Exception( GetErrorMsg(sizeX, sizeY) )
{
public long FileSize{ get; } = sizeX;

public long SizeExpected{ get; } = sizeY;

// Get Error Msg

private static string GetErrorMsg(long sizeX, long sizeY)
{
string displaySizeX = InputHelper.GetDisplaySize(sizeX);
string displaySizeY = InputHelper.GetDisplaySize(sizeY);

return $"File Size ({sizeX} | {displaySizeX}) differs from Expected ({sizeY} | {displaySizeY})"; 
}

}
using System;
using System.IO;

namespace ZCore.Modules.TextureDrawer
{
/// <summary> Initializes some useful Tasks for Textures. </summary>

public static class TextureHelper
{
// Calculate Texture Size

public static int CalculateTextureSize(int width, int height, int factor = 0) => (width * height) << factor;

// Get Padded Width
	
public static int CalculatePaddedDimension(int x, int factor)
{
return (x % factor != 0) ? x / factor * factor + factor : x;
}

// Compare Stream Size with given Size

public static bool FileMatchSize(Stream targetStream, int originalSize) => targetStream.Length == originalSize;

/** <summary> Compares the Size of a Parsed Image (in bytes) and Displays an Alert in case an Anomally is Detected. </summary>

<returns> <b>true</b> if theres is a SizeMatch; otherwise, <b>false</b>. </returns> */

public static void CompareFileSize(Stream targetStream, int originalSize)
{

if(!FileMatchSize(targetStream, originalSize) )
throw new FileSizeMismatchException(targetStream.Length, originalSize);

}

// Compare Calculted TexSize with given Size

public static bool DimensionsMatchSize(int width, int height, int sizeToCompare, int factor = 0)
{
int originalSize = CalculateTextureSize(width, height, factor);

return sizeToCompare == originalSize;
}

/** <summary> Compares the Size of a Image (Dimensions) and Displays an Alert in case an Anomally is Detected. </summary>

<returns> <b>true</b> if theres is a SizeMatch; otherwise, <b>false</b>. </returns> */

public static void CompareTextureSize(int width, int height, int sizeToCompare, int factor = 0)
{

if(!DimensionsMatchSize(width, height, sizeToCompare, factor) )
throw new Exception($"Texture size ({sizeToCompare}) doesn't Match the Current dimensions ({width}x{height})");

}

}

}
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.Mobile;

namespace ZCore.Modules.TextureDrawer.Parsers.PopCapTexture.AndroidCN
{
/// <summary> Initializes Parsing Tasks for PTX Files (Android, China). </summary>

public static class PtxParser
{
// Get PTX Stream

public static BinaryStream EncodeStream(SexyBitmap input, PtxFormat_AndroidCN format, Endian endian = default, 
string outputPath = null, string pathToPtxInfo = null)
{
PtxParamsForRsb fileInfo = new(input.Width, input.Height, (uint)format);

PathHelper.ChangeExtension(ref outputPath, PtxUtils.FileExt);
BinaryStream ptxStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

// Encode Image

switch(format)
{
case PtxFormat_AndroidCN.RGBA4444:
fileInfo.TexturePitch = RGBA4444.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.RGB565:
fileInfo.TexturePitch = RGB565.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.RGBA5551:
fileInfo.TexturePitch = RGBA5551.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.RGBA4444_Block:
fileInfo.TextureWidth = TextureHelper.CalculatePaddedDimension(fileInfo.TextureWidth, PtxUtils.PaddingFactor);

fileInfo.TextureHeight = TextureHelper.CalculatePaddedDimension(fileInfo.TextureHeight, PtxUtils.PaddingFactor);
fileInfo.TexturePitch = RGBA4444_Block.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.RGB565_Block:
fileInfo.TextureWidth = TextureHelper.CalculatePaddedDimension(fileInfo.TextureWidth, PtxUtils.PaddingFactor);

fileInfo.TextureHeight = TextureHelper.CalculatePaddedDimension(fileInfo.TextureHeight, PtxUtils.PaddingFactor);
fileInfo.TexturePitch = RGB565_Block.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.RGBA5551_Block:
fileInfo.TextureWidth = TextureHelper.CalculatePaddedDimension(fileInfo.TextureWidth, PtxUtils.PaddingFactor);

fileInfo.TextureHeight = TextureHelper.CalculatePaddedDimension(fileInfo.TextureHeight, PtxUtils.PaddingFactor);
fileInfo.TexturePitch = RGBA5551_Block.Write(ptxStream, input, endian);
break;

case PtxFormat_AndroidCN.ETC1_RGB_A_Palette:
fileInfo.TexturePitch = ETC1_RGB_A_Palette.Write(ptxStream, input, out int a_Size, endian);

fileInfo.AlphaSize = a_Size;
fileInfo.AlphaChannel = PtxAlphaChannel.A_Palette;
break;

default:
fileInfo.TexturePitch = ABGR8888.Write(ptxStream, input, endian);
break;
}

PtxUtils.SaveInfo(fileInfo, pathToPtxInfo);

return ptxStream;
}

/** <summary> Encodes an Image as a PopCapTexture. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded PTX File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, PtxFormat_AndroidCN format,
Endian endian = default, string pathToInfoContainer = null)
{
pathToInfoContainer = string.IsNullOrEmpty(pathToInfoContainer) ? "PtxInfo" : pathToInfoContainer;

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToPtxInfo = PtxUtils.GetPathToPtxInfo(pathToInfoContainer, inputPath);

using BinaryStream outputFile = EncodeStream(inputFile, format, endian, outputPath, pathToPtxInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, PtxFormat_AndroidCN format, int width, int height,
Endian endian = default)
{

return format switch
{
PtxFormat_AndroidCN.RGBA4444 => RGBA4444.Read(input, width, height, endian),
PtxFormat_AndroidCN.RGB565 => RGB565.Read(input, width, height, endian),
PtxFormat_AndroidCN.RGBA5551 => RGBA5551.Read(input, width, height, endian),
PtxFormat_AndroidCN.RGBA4444_Block => RGBA4444_Block.Read(input, width, height, endian),
PtxFormat_AndroidCN.RGB565_Block => RGB565_Block.Read(input, width, height, endian),
PtxFormat_AndroidCN.RGBA5551_Block => RGBA5551_Block.Read(input, width, height, endian),
PtxFormat_AndroidCN.ETC1_RGB_A_Palette => ETC1_RGB_A_Palette.Read(input, width, height, endian),
_ => ABGR8888.Read(input, width, height, endian)
};

}
		
/** <summary> Decodes a PopCapTexture as an Image. </summary>

<param name = "inputPath"> The Path where the PTX to Decode is Located. </param>
<param name = "outputPath"> The Location where the Decoded Image will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */	

public static void DecodeFile(string inputPath, string outputPath, Endian endian = default, string pathToInfoContainer = null)
{
var fileInfo = PtxUtils.LoadInfo(inputPath, pathToInfoContainer);
PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);

using SexyBitmap image = DecodeStream(inputFile, (PtxFormat_AndroidCN)fileInfo.TextureFormat,
fileInfo.TextureWidth, fileInfo.TextureHeight, endian);

image.Save(outputPath);
}

}

}
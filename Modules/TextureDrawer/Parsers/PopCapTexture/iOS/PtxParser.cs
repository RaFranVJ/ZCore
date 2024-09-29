using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Ptx.Format.Mobile;

namespace ZCore.Modules.TextureDrawer.Parsers.PopCapTexture.iOS
{
/// <summary> Initializes Parsing Tasks for PTX Files (iOS). </summary>

public static class PtxParser
{
// Get PTX Stream

public static BinaryStream EncodeStream(SexyBitmap input, PtxFormat_iOS format, bool useArgbPaddingForRsb,
Endian endian = default, string outputPath = null, string pathToPtxInfo = null)
{
PtxParamsForRsb fileInfo = new(input.Width, input.Height, (uint)format);

PathHelper.ChangeExtension(ref outputPath, PtxUtils.FileExt);
BinaryStream ptxStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

// Encode Image

switch(format)
{
case PtxFormat_iOS.RGBA4444:
fileInfo.TexturePitch = RGBA4444.Write(ptxStream, input, endian);
break;

case PtxFormat_iOS.RGBA5551:
fileInfo.TexturePitch = RGBA5551.Write(ptxStream, input, endian);
break;

case PtxFormat_iOS.RGBA4444_Block:
fileInfo.TextureWidth = TextureHelper.CalculatePaddedDimension(fileInfo.TextureWidth, PtxUtils.PaddingFactor);

fileInfo.TextureHeight = TextureHelper.CalculatePaddedDimension(fileInfo.TextureHeight, PtxUtils.PaddingFactor);
fileInfo.TexturePitch = RGBA4444_Block.Write(ptxStream, input, endian);
break;

case PtxFormat_iOS.RGBA5551_Block:
fileInfo.TextureWidth = TextureHelper.CalculatePaddedDimension(fileInfo.TextureWidth, PtxUtils.PaddingFactor);

fileInfo.TextureHeight = TextureHelper.CalculatePaddedDimension(fileInfo.TextureHeight, PtxUtils.PaddingFactor);
fileInfo.TexturePitch = RGBA5551_Block.Write(ptxStream, input, endian);
break;

case PtxFormat_iOS.PVRTC_4BPP_RGBA:
fileInfo.TexturePitch = PVRTC_4BPP_RGBA.Write(ptxStream, input, endian);
break;

case PtxFormat_iOS.PVRTC_4BPP_RGB_A8:
fileInfo.TexturePitch = PVRTC_4BPP_RGB_A8.Write(ptxStream, input, endian);
break;

default:

if(useArgbPaddingForRsb)
{
int paddedWidth = TextureHelper.CalculatePaddedDimension(input.Width, PtxUtils.PaddingFactor * 2);

fileInfo.TexturePitch = ARGB8888_Padding.Write(ptxStream, input, paddedWidth << 2, endian);
}

else
fileInfo.TexturePitch = ARGB8888.Write(ptxStream, input, endian);

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

public static void EncodeFile(string inputPath, string outputPath, PtxFormat_iOS format,
Endian endian = default, string pathToInfoContainer = null)
{
pathToInfoContainer = string.IsNullOrEmpty(pathToInfoContainer) ? "PtxInfo" : pathToInfoContainer;

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToPtxInfo = PtxUtils.GetPathToPtxInfo(pathToInfoContainer, inputPath);

using BinaryStream outputFile = EncodeStream(inputFile, format, false, endian, outputPath, pathToPtxInfo);
}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, PtxFormat_iOS format, bool useArgbPaddingForRsb,
int width, int height, Endian endian = default)
{
SexyBitmap output;

switch(format)
{
case PtxFormat_iOS.RGBA4444:
output = RGBA4444.Read(input, width, height, endian);
break;

case PtxFormat_iOS.RGBA5551:
output = RGBA5551.Read(input, width, height, endian);
break;
        
case PtxFormat_iOS.RGBA4444_Block:
output = RGBA4444_Block.Read(input, width, height, endian);
break;
        
case PtxFormat_iOS.RGBA5551_Block:
output = RGBA5551_Block.Read(input, width, height, endian);
break;

case PtxFormat_iOS.PVRTC_4BPP_RGBA:
output = PVRTC_4BPP_RGBA.Read(input, width, height);
break;

case PtxFormat_iOS.PVRTC_4BPP_RGB_A8:
output = PVRTC_4BPP_RGB_A8.Read(input, width, height);
break;
        
default:
int paddedW = useArgbPaddingForRsb ? TextureHelper.CalculatePaddedDimension(width, PtxUtils.PaddingFactor * 2) : width;

output = useArgbPaddingForRsb ? ARGB8888_Padding.Read(input, width, height, paddedW << 2) : 
ARGB8888.Read(input, width, height, endian);
break;
}

return output;
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

using SexyBitmap image = DecodeStream(inputFile, (PtxFormat_iOS)fileInfo.TextureFormat, false, 
fileInfo.TextureWidth, fileInfo.TextureHeight, endian);

image.Save(outputPath);
}

}

}
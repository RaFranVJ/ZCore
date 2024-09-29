using System;
using System.IO;
using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions;
using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.VersionCheck;
using ZCore.Serializables.ArgumentsInfo.FileManager.Archive;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb.Integrity;

namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio
{
/// <summary> Initializes Parsing Tasks for XNB Files (from Microsoft). </summary>

public static class XnbParser
{
/** <summary> The Header of a XNB File. </summary>

<remarks> It Occupies 3 Bytes in the Stream, which are: <c>0x58 0x4E 0x42</c> </remarks> */

private const string XnbHeader = "XNB";

// Get XNB Stream

public static BinaryStream EncodeStream(SexyBitmap input, XnbPlatform platform, XnbFormat format,
Endian endian = default, FileVersionDetails<ushort> xnbVer = null, FileVersionDetails<uint> ddsVer = null,
string outputPath = null, string pathToXnbInfo = null)
{
xnbVer ??= new(XnbVersion.ExpectedVersions.MaxValue);
ddsVer ??= new();

XnbInfo fileInfo = string.IsNullOrEmpty(pathToXnbInfo) ? 
new(input.Width, input.Height, platform, format) :
new XnbInfo().ReadObject(pathToXnbInfo);

PathHelper.ChangeExtension(ref outputPath, ".xnb");
BinaryStream xnbStream = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

xnbStream.WriteString(XnbHeader, default, endian);
fileInfo.WriteBin(xnbStream, xnbVer, ddsVer, endian);

// Encode Image

switch(format)
{
case XnbFormat.RGBA4444:
RGBA4444.Write(xnbStream, input, endian);
break;

case XnbFormat.RGB565:
RGB565.Write(xnbStream, input, endian);
break;

case XnbFormat.RGBA5551:
RGBA5551.Write(xnbStream, input, endian);
break;
    
case XnbFormat.DXT1_RGB:
DXT1_RGB.Write(xnbStream, input, endian);
break;

case XnbFormat.DXT2_RGBA:
DXT2_RGBA.Write(xnbStream, input, endian);
break;
    
case XnbFormat.DXT3_RGBA:
DXT3_RGBA.Write(xnbStream, input, endian);
break;

case XnbFormat.DXT4_RGBA:
DXT4_RGBA.Write(xnbStream, input, endian);
break;
		
case XnbFormat.DXT5_RGBA:
DXT5_RGBA.Write(xnbStream, input, endian);
break;

default:
ABGR8888.Write(xnbStream, input, endian);
break;
}

xnbStream.Position = XnbHeader.Length;
fileInfo.WriteBin(xnbStream, xnbVer, ddsVer, endian); // Update Info (XnbSize)

return xnbStream;
}

/** <summary> Encodes an Image as a XNB File. </summary>

<param name = "inputPath"> The Path where the Image to Encode is Located. </param>
<param name = "outputPath"> The Location where the Encoded XNB File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeFile(string inputPath, string outputPath, XnbPlatform platform, XnbFormat format,
Endian endian = default, FileVersionDetails<ushort> xnbVer = null, FileVersionDetails<uint> ddsVer = null,
MetadataImportParams importCfg = null)
{
importCfg ??= new();

using SexyBitmap inputFile = SexyBitmap.CreateNew(inputPath);
string pathToXnbInfo = importCfg.ImportMetadataToFiles ? XnbInfo.ResolvePath(inputPath, importCfg) : null;

using BinaryStream outputFile = EncodeStream(inputFile, platform, format, endian,
xnbVer, ddsVer, outputPath, pathToXnbInfo);

}

// Get Plain Image

public static SexyBitmap DecodeStream(BinaryStream input, Endian endian = default, bool adaptVer_Xnb = true,
bool adaptVer_Dds = true, XnbIntegrityInfo integrityCfg = null, string pathToXnbInfo = null)
{
integrityCfg ??= new();

input.CompareString<InvalidXnbException>(XnbHeader, default, endian);

XnbInfo fileInfo = XnbInfo.ReadBin(input, endian, adaptVer_Xnb, adaptVer_Dds);

if(integrityCfg.CheckIntegrityOnDecoding)
XnbAnalisis.IntegrityCheck(input, fileInfo, integrityCfg.AnalisisType);

// Decode Image

SexyBitmap output = fileInfo.TextureFormat switch
{
XnbFormat.DXT5_RGBA => DXT5_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.RGBA4444 => RGBA4444.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.RGB565 => RGB565.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.RGBA5551 => RGBA5551.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.DXT1_RGB => DXT1_RGB.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.DXT2_RGBA => DXT2_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.DXT3_RGBA => DXT3_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
XnbFormat.DXT4_RGBA => DXT4_RGBA.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian),
_ => ABGR8888.Read(input, fileInfo.TextureWidth, fileInfo.TextureHeight, endian)
};

if(!string.IsNullOrEmpty(pathToXnbInfo) )
fileInfo.WriteObject(pathToXnbInfo);

return output;
}

/** <summary> Decodes a XNB File as an Image. </summary>

<param name = "inputPath"> The Path where the XNB File to Decode is Located. </param>
<param name = "outputPath"> The Location where the Decoded Image will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecodeFile(string inputPath, string outputPath, Endian endian = default, bool adaptVer_Xnb = true,
bool adaptVer_Dds = true, XnbIntegrityInfo integrityCfg = null, MetadataExportParams exportCfg = null)
{
exportCfg ??= new();

PathHelper.ChangeExtension(ref outputPath, ".png");

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToXnbInfo = exportCfg.ExportMetadataFromFiles ? XnbInfo.ResolvePath(inputPath, exportCfg) : null;

using SexyBitmap image = DecodeStream(inputFile, endian, adaptVer_Xnb, adaptVer_Dds, integrityCfg, pathToXnbInfo);
image.Save(outputPath);        
}

/** <summary> Checks the Integrity of a XNB File. </summary>
<param name = "targetPath"> The Path to the File to Analize. </param> */

public static void CheckFileIntegrity(string targetPath, IntegrityCheckType analisisType, Endian endian = default,
bool adaptVer_Xnb = true, bool adaptVer_Dds = true)
{
using BinaryStream inputFile = BinaryStream.Open(targetPath);

inputFile.CompareString<InvalidXnbException>(XnbHeader, default, endian);
XnbInfo fileInfo = XnbInfo.ReadBin(inputFile, endian, adaptVer_Xnb, adaptVer_Dds);

XnbAnalisis.IntegrityCheck(inputFile, fileInfo, analisisType);
}

}

}
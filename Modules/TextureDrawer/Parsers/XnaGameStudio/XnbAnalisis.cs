using System;
using System.IO;
using ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio.Exceptions;
using ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb.Integrity;

namespace ZCore.Modules.TextureDrawer.Parsers.XnaGameStudio
{
/// <summary> Performs Integrity Checks on XNB Files. </summary>

public static class XnbAnalisis
{
/** <summary> Displays the Result of a Integrity Check on a RSB File. </summary>

<param name = "filePath"> The Path that belongs to the Opened Stream (used for Loading Tags). </param>
<param name = "targetStream"> The Stream to Check. </param>
<param name = "fileInfo"> The SMF used to Compare the File Integrity. </param> */

public static void IntegrityCheck(Stream targetStream, XnbInfo fileInfo, IntegrityCheckType analisisType)
{

bool result = analisisType switch
{
IntegrityCheckType.TextureComparisson => TextureHelper.DimensionsMatchSize(fileInfo.TextureWidth,
fileInfo.TextureHeight, fileInfo.TextureSize, 2),

IntegrityCheckType.XnbComparisson => TextureHelper.FileMatchSize(targetStream, fileInfo.XnbSize),

IntegrityCheckType.FullScope => TextureHelper.DimensionsMatchSize(fileInfo.TextureWidth, fileInfo.TextureHeight,
fileInfo.TextureSize, 2) && TextureHelper.FileMatchSize(targetStream, fileInfo.XnbSize),

_ => true
};

if(!result)
throw new CorruptedXnbException();

}

}

}
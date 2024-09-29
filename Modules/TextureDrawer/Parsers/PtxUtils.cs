using System;
using System.IO;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.PopCapTexture;
using ZCore.Modules.TextureDrawer.Parsers.PopCapTexture;

namespace ZCore.Modules.TextureDrawer.Parsers
{
/// <summary> Initializes some useful Tasks for the PTX Parser. </summary>

public static class PtxUtils
{
/// <summary> The PTX File Extention. </summary>

public const string FileExt = ".ptx";

/// <summary> The PTX File Header. </summary>

public const string FileHeader = "ptx1";

/// <summary> Value used for Calculating the Padded Width of a Texture. </summary>

public const int PaddingFactor = 32;

// Get Path to PTX Info

public static string GetPathToPtxInfo(string containerPath, string filePath)
{
string fileName = Path.GetFileNameWithoutExtension(filePath);

return containerPath + Path.DirectorySeparatorChar + fileName + "_Info.json";
}

// Load Ptx Info (Mobile only)

public static PtxParamsForRsb LoadInfo(string filePath, string containerPath = null)
{
containerPath = string.IsNullOrEmpty(containerPath) ? "PtxInfo" : containerPath;

string pathToPtxInfo = GetPathToPtxInfo(containerPath, filePath);

if(!File.Exists(pathToPtxInfo) )
throw new MissingPtxInfoException(pathToPtxInfo);

var fileInfo = JsonSerializer.DeserializeObject<PtxParamsForRsb>( File.ReadAllText(pathToPtxInfo) ) ?? 
throw new MissingPtxInfoException(pathToPtxInfo);

return fileInfo;
}

// Save Ptx Info

public static void SaveInfo(PtxParamsForRsb sourceInfo, string targetPath)
{

if(string.IsNullOrEmpty(targetPath) || sourceInfo == null)
{
var paramName = sourceInfo == null ? nameof(sourceInfo) : nameof(targetPath);

throw new ArgumentNullException(paramName, "Must define a Path and a PTX Info to Save");
}

DirManager.CheckMissingFolder( Path.GetDirectoryName(targetPath) );

File.WriteAllText(targetPath, JsonSerializer.SerializeObject(sourceInfo) );
}

}

}
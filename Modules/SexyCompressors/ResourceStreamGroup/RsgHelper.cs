using System.IO;

namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup
{
/// <summary> Performs some useful Tasks for RSG Files </summary>

public static class RsgHelper
{
/// <summary> The RSG Extension </summary>

public const string FileExt = ".rsg";

// Build Path for RSG

public static string BuildFilePath(string baseDir, string groupName, StringCase strCaseForGroupName)
{
InputHelper.ApplyStringCase(ref groupName, strCaseForGroupName);

string packetDir = baseDir + Path.DirectorySeparatorChar + "Packets" + Path.DirectorySeparatorChar; 
string filePath = packetDir + groupName + FileExt;

DirManager.CheckMissingFolder(Path.GetDirectoryName(filePath) );

return filePath;
}

// Build Path for RSG Info

public static string BuildInfoPath(string baseDir, string groupName, StringCase strCaseForGroupName)
{
InputHelper.ApplyStringCase(ref groupName, strCaseForGroupName);

string infoDir = baseDir + Path.DirectorySeparatorChar + "PacketInfo" + Path.DirectorySeparatorChar; 
string filePath = infoDir + groupName + ".json";

DirManager.CheckMissingFolder(Path.GetDirectoryName(filePath) );

return filePath;
}

// Build Path for Res

public static string BuildResPath(string baseDir, string resName, bool useResDir, StringCase strCaseForResName)
{
string resDir = baseDir + Path.DirectorySeparatorChar;

if(useResDir)
resDir += "Resources" + Path.DirectorySeparatorChar;

InputHelper.ApplyStringCase(ref resName, strCaseForResName);
string resPath = resDir + resName;

DirManager.CheckMissingFolder(Path.GetDirectoryName(resPath) );

return resPath;
}

// Pad Length

public static int BeautifyLength(int oriLength, bool forFile = false)
{

if (oriLength % 4096 == 0)
return forFile ? 0 : 4096;

return 4096 - (oriLength % 4096);
}

}

}
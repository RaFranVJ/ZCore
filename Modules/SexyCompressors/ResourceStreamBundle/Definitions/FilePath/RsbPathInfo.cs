using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.FilePath
{
/// <summary> Represents some Info for a RSB Path </summary>

public class RsbPathInfo
{
/** <summary> Obtains or Creates a List of Group Names found in the RSB Stream. </summary>
<returns> The GroupNames </returns> */

public List<string> GroupNames{ get; set; }

/** <summary> Gets or Sets a Path to the extracted RSB Directory. </summary>
<returns> The PathToExtractedFolder </returns> */

public string PathToExtractedFolder{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbPathInfo</c> </summary>

public RsbPathInfo()
{
GroupNames = new();
PathToExtractedFolder = "Packet";
}

/// <summary> Creates a new Instance of the <RsbPathInfo</c> </summary>

public RsbPathInfo(List<string> names, string outPath)
{
GroupNames = names;
PathToExtractedFolder = outPath;
}

}

}
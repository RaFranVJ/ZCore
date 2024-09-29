namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.Resources
{
/// <summary> Represents Info of a Resource that was Extracted from a RSG Stream </summary>

public class ResInfo
{
/** <summary> Gets or Sets a Path to the Resource File. </summary>
<returns> The PathToResFile </returns> */

public string PathToResFile{ get; set; }

/** <summary> Gets or Sets some Info related to the PTX Image. </summary>
<returns> The ImageInfo or <c>null</c> if ResFile is not a Image </returns> */

public PtxInfoForRsg ImageInfo{ get; set; }

/// <summary> Creates a new Instance of the <c>ResInfo</c> </summary>

public ResInfo()
{
PathToResFile = "<MUST DEFINE A PATH TO RES FILE>";
}

/// <summary> Creates a new Instance of the <c>ResInfo</c> </summary>

public ResInfo(string resPath)
{
PathToResFile = resPath;
}

/// <summary> Creates a new Instance of the <c>ResInfo</c> </summary>

public ResInfo(string resPath, PtxInfoForRsg imageInfo)
{
PathToResFile = resPath;

ImageInfo = imageInfo;
}

}

}
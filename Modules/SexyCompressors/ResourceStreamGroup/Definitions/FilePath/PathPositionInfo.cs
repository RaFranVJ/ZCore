namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath
{
/// <summary> Stores Info of the Position of a Path inside a RSG Stream </summary>

public class PathPositionInfo
{
/** <summary> Gets or Sets the Position of the Path. </summary>
<returns> The PathPosition </returns> */

public uint PathPosition{ get; set; }

/** <summary> Gets or Sets the Offset of the Path. </summary>
<returns> The PathOffset. </returns> */

public uint PathOffset{ get; set; }

/// <summary> Creates a new Instance of the <c>PathPositionInfo</c> </summary>

public PathPositionInfo()
{
}

/// <summary> Creates a new Instance of the <c>PathPositionInfo</c> </summary>

public PathPositionInfo(uint position, uint offset)
{
PathPosition = position;
PathOffset = offset;
}

}

}
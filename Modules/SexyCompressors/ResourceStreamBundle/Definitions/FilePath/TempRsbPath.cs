using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.FilePath
{
/// <summary> Represents a Temporal RSB Path </summary>

public class TempRsbPath
{
/** <summary> Gets or Sets a Temporal Path that will be Split into an Array of Strings. </summary>
<returns> The PathToSlice </returns> */

public string PathToSlice{ get; set; }

/** <summary> Gets or Sets the Path Key </summary>
<returns> The Path Key </returns> */

public int Key{ get; set; }

/** <summary> Gets or Sets the Index of the Path in the StringsPool </summary>
<returns> The PoolIndex </returns> */

public int PoolIndex{ get; set; }

/** <summary> Obtains or Creates a List that Contains Info related to the Path Position. </summary>
<returns> The PosInfo </returns> */

public List<PathPositionInfo> PosInfo{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>TempRsbPath</c> </summary>

public TempRsbPath()
{
}

/// <summary> Creates a new Instance of the <c>TempRsbPath</c> </summary>

public TempRsbPath(string path, int key, int poolIndex)
{
PathToSlice = path;

Key = key;
PoolIndex = poolIndex;
}

// Write FileList

public void WriteList(BinaryStream targetStream, Endian endian)
{
long startOffset = targetStream.Position;

targetStream.WriteStringAsFourBytes(PathToSlice);
long backupOffset = targetStream.Position;

for(int i = 0; i < PosInfo.Count; i++)
{
targetStream.Position = startOffset + PosInfo[i].PathOffset * 4 + 1;

targetStream.WriteUTripleByte(PosInfo[i].PathPosition, endian);
}

targetStream.Position = backupOffset;

targetStream.WriteInt(PoolIndex, endian);
}

}

}
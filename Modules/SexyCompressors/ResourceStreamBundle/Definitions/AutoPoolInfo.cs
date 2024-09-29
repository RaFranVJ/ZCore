using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions
{
/// <summary> Represents some Info for an AutoPool inside a RSB Stream </summary>

public class AutoPoolInfo
{
/** <summary> Gets or Sets the Name of this Pool. </summary>
<returns> The PoolName </returns> */

public string PoolName{ get; set; }

/** <summary> Gets or Sets the Size of the Part0. </summary>
<returns> The Part0 Size </returns> */

public uint Part0_Size{ get; set; }

/** <summary> Gets or Sets the Size of the Part1. </summary>
<returns> The Part1 Size </returns> */

public uint Part1_Size{ get; set; }

/// <summary> Creates a new Instance of the <c>AutoPoolInfo</c> </summary>

public AutoPoolInfo()
{
PoolName = "AutoPool_";
}

/// <summary> Creates a new Instance of the <c>AutoPoolInfo</c> </summary>

public AutoPoolInfo(string name, uint part0_Size, uint part1_Size)
{
PoolName = name;

Part0_Size = part0_Size;
Part1_Size = part1_Size;
}

// Read AutoPool Info

public static List<AutoPoolInfo> Read(BinaryStream sourceStream, Endian endian, uint offset,
uint itemsCount, uint infoLength)
{
sourceStream.Position = offset;

List<AutoPoolInfo> autoPoolInfo = new();

for(int i = 0; i < itemsCount; i++)
{
long startOffset = sourceStream.Position;

string autoPoolName = sourceStream.ReadStringUntilZero(default, endian);

sourceStream.Position = startOffset + 128;

uint part0_Size = sourceStream.ReadUInt(endian);
uint part1_Size = sourceStream.ReadUInt(endian);

autoPoolInfo.Add( new(autoPoolName, part0_Size, part1_Size) );
				
sourceStream.Position = startOffset + infoLength;
}

uint endOffset = infoLength * itemsCount + offset;
RsbHelper.CheckEndOffset(sourceStream, endOffset);

return autoPoolInfo;
}

}

}
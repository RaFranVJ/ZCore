using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Description
{
/// <summary> Represents a SubGroup Description </summary>

public class SubGroupDescription
{
/** <summary> Gets or Sets the Ratio of this SubGroup. </summary>
<returns> The Ratio </returns> */

public uint Ratio{ get; set; }

/** <summary> Gets or Sets the Language of this SubGroup. </summary>
<returns> The Language </returns> */

public string Language{ get; set; }

/** <summary> Gets or Sets a Map to the Res Files inside the SubGroup. </summary>
<returns> The ResourcesMap </returns> */

public Dictionary<string, FileDescription> ResourcesMap{ get; set; } = new();

/// <summary> Creates a new Instance of the <c>SubGroupDescription</c> </summary>

public SubGroupDescription()
{
}

/// <summary> Creates a new Instance of the <c>SubGroupDescription</c> </summary>

public SubGroupDescription(uint ratio, string lang)
{
Ratio = ratio;
Language = lang;
}

// Write SubGroup

public void Write(BinaryStream part1_Res, BinaryStream part2_Res, BinaryStream part3_Res,
Endian endian, Dictionary<string, uint> stringPool, string groupKey)
{
part1_Res.WriteUInt(Ratio, endian);

if(string.IsNullOrEmpty(Language) )
part1_Res.WriteInt(0);

else
part1_Res.WriteString( (Language + "    ")[..4], default, endian);

uint idOffset = RsbHelper.ThrowInPool(part3_Res, stringPool, groupKey, endian);
part1_Res.WriteUInt(idOffset, endian);

var resKeys = ResourcesMap.Keys;
part1_Res.WriteUInt( (uint)resKeys.Count, endian);

// Process ResFiles

foreach(var key in resKeys)  
ResourcesMap[key].Write(part1_Res, part2_Res, part3_Res, endian, stringPool, key);

}

}

}
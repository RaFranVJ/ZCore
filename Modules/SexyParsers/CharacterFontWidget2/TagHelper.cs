using System.Collections.Generic;

namespace ZCore.Modules.SexyParsers.CharacterFontWidget2
{
/// <summary> Loads or Saves the Tags for a CFW2. </summary>

public static class TagHelper
{
// Get Tag List

public static List<string> LoadTags(BinaryStream bs, uint count, Endian endian = default)
{
List<string> tags = new();

for(uint i = 0; i < count; i++)
tags.Add(bs.ReadStringByIntLength(default, endian) );

return tags;
}

// Save Tag List

public static void SaveTags(BinaryStream bs, List<string> tags, uint count, Endian endian = default)
{

if(tags == null)
return;

count = count > tags.Count ? (uint)tags.Count : count;

for(int i = 0; i < count; i++)
bs.WriteStringByIntLength(tags[i], default, endian);

}

}

}
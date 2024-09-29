using System;
using System.Collections.Generic;

namespace ZCore.Modules.SexyCompressors.ArcVPackage.Definitions
{
/// <summary> Represents a Entry to a File inside a Directory that will be Compressed. </summary>

public class ArcvResEntry
{
/** <summary> Gets or Sets the Path to a Resource inside a Directory. </summary>
<returns> The Path to the File. </returns> */

public string PathToResource{ get; set; }

/** <summary> Gets or Sets a Value which Contains Info about the CRC32 Value of an ARCV Resource. </summary>
<returns> The CRC32 Value. </returns> */

public long CRC32{ get; set; }

/// <summary> Creates a new Instance of the <c>ArcvResEntry</c>. </summary>

public ArcvResEntry()
{
CRC32 = -1;
}

/** <summary> Creates a new Instance of the <c>ArcvResEntry</c> with the given Path. </summary>

<param name = "path"> The Path to the File. </param> */

public ArcvResEntry(string path)
{
PathToResource = path;
CRC32 = ArcvHelper.GetCRC32(path);
}

/** <summary> Creates a new Instance of the <c>ArcvResEntry</c> with the given Parameters. </summary>

<param name = "pos"> The File Position. </param>
<param name = "size"> The File Size. </param>
 */

public ArcvResEntry(string path, long checksum)
{
PathToResource = path;
CRC32 = checksum;
}

// Build ARCV Entries

public static List<ArcvResEntry> BuildEntries(params string[] entryNames)
{
List<ArcvResEntry> resEntries = new();

foreach(string name in entryNames)
resEntries.Add( new(name) );

resEntries.Sort( new ArcvResComparer() );

return resEntries;
}

}

}
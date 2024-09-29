using System.Collections.Generic;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.FilePath;
using ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Group;

namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions
{
/// <summary> Represents a MANIFEST for a RSB File that was Extracted by the Tool </summary>

public class ManifestInfo : SerializableClass<ManifestInfo>
{
/** <summary> Gets or Sets the Manifest Version. </summary>
<returns> The File Version </returns> */

public uint FileVersion{ get; set; }

/** <summary> Gets or Sets the Number of PTX Entries found in the RSB Stream. </summary>
<returns> The NumberOfPtxEntries </returns> */

public uint NumberOfPtxEntries{ get; set; }

/** <summary> Gets or Sets some Info related to the RSB Paths. </summary>
<returns> The RsbPathInfo </returns> */

public RsbPathInfo PathInfo{ get; set; }

/** <summary> Obtains or Creates a List of Entries for the Groups inside the RSB Stream. </summary>
<returns> The NumberOfPtxEntries </returns> */

public List<GroupInfo> GroupEntries{ get; set; }

/// <summary> Creates a new Instance of the <c>ManifestInfo</c> </summary>

public ManifestInfo()
{
GroupEntries = new();
}

/// <summary> Creates a new Instance of the <c>ManifestInfo</c> </summary>

public ManifestInfo(uint version, uint ptxCount, RsbPathInfo pathInfo, List<GroupInfo> groups)
{
FileVersion = version;
NumberOfPtxEntries = ptxCount;

PathInfo = pathInfo;
GroupEntries = groups;
}

}

}
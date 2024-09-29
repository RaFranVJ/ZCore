using System.IO;

namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf
{
/// <summary> Groups options related to the Smf Tag Files. </summary>

public class SmfTagInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines if Tags should be Generated on Compression. </summary>
<returns> <b>true</b> if a Tag should be Generated after Compressing Files; otherwise, <b>false</b>. </returns> */

public bool GenerateTagsOnCompression{ get; set; }

/** <summary> Gets or Sets a Value that Determines how Hashed String Case should be Handled. </summary>
<returns> The Hashed String Case. </returns> */

public StringCase HashedStringCase{ get; set; }

/** <summary> Gets or Sets the Path where SMF Tags should be Saved or Loaded. </summary>
<returns> The Container Path. </returns> */

public string TagContainerPath{ get; set; }

/** <summary> Gets or Sets the Path where the File Tag should be Saved or Loaded. </summary>
<returns> The File Path. </returns> */

public string PathToTagFile{ get; set; }

/// <summary> Creates a new Instance of the <c>Tagnfo</c>. </summary>

public SmfTagInfo()
{
GenerateTagsOnCompression = true;
HashedStringCase = StringCase.Upper;

TagContainerPath = GetDefaultContainerPath();
}

/// <summary> Checks each nullable Field of the <c>TarInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
SmfTagInfo defaultInfo = new();

#region ======== Set default Values to Null Fields ========

TagContainerPath ??= defaultInfo.TagContainerPath;

#endregion

PathHelper.CheckExistingPath(TagContainerPath, GenerateTagsOnCompression);
}

/** <summary> Gets the default Path to the CspContainer basing on the CurrentAppDirectory. </summary>
<returns> The default Container Path. </returns> */

private static string GetDefaultContainerPath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + "SmfTags";
}

}
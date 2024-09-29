using System.IO;
using ZCore.Modules.Other;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf;

namespace ZCore.Modules.SexyCompressors.StandarMediaFile
{
/// <summary> Allows the Creation of Tags for SMF Files. </summary>

public static class SmfTagCreator
{
/// <summary> The Tag Extension </summary>

private const string TagExt = ".tag.smf";

/// <summary> The Tag Padding </summary>

private const string TagPadding = "\x0D\x0A";

/** <summary> Gets a Path to the SMF Files. </summary>

<param name = "filePath"> The Path to the File where the SMF Info should be Saved. </param>

<returns> The Path to the SMF Tag. </returns> */

public static string ResolvePath(string basePath, SmfTagInfo tagInfo = default)
{
tagInfo ??= new();

if(!string.IsNullOrEmpty(tagInfo.PathToTagFile) )
return tagInfo.PathToTagFile;

return PathHelper.BuildPathFromDir(tagInfo.TagContainerPath, basePath, TagExt);
}

/** <summary> Generates a Tag by using SHA-1 Digest on the RSB File. </summary>

<param name = "sourceStream"> The Stream from which the Tag will be Created. </param>

<returns> The SMF Tag. </returns> */

public static string GenerateTag(Stream sourceStream, StringCase strCase)
{
return StringDigest.DigestData(sourceStream, false, "SHA1", strCase) + TagPadding;
}

/** <summary> Saves the SMF Tag from the given RSB Stream. </summary>

<param name = "sourceStream"> The Stream from which the Tag will be Created. </param>
<param name = "targetPath"> The Path where to Save the SMF Tag. </param> */

public static void SaveTag(Stream sourceStream, StringCase strCase, string targetPath)
{
File.WriteAllText(targetPath, GenerateTag(sourceStream, strCase) );
}

}

}
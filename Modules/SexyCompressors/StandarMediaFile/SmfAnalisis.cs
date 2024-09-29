using System;
using System.IO;
using System.Linq;
using System.Security;
using ZCore.Modules.Other;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;
using ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf.Integrity;

namespace ZCore.Modules.SexyCompressors.StandarMediaFile
{
/// <summary> Performs Integrity Checks on SMF Files. </summary>

public static class SmfAnalisis
{
/** <summary> Compares two Tags and Displays an Alert in case an Anomally is Detected. </summary>

<param name = "targetStream"> The Stream from which to Compare the Tags. </param>
<param name = "pathToSmfTag"> The Path to the Original SMF Tag. </param>

<returns> <b>true</b> if the Two Tags are Equals; otherwise, <b>false</b>. </returns> */

private static bool CompareTags(Stream targetStream, StringCase strCase, string pathToSmfTag)
{
string originalTag = File.ReadAllText(pathToSmfTag);
string tagToCompare = SmfTagCreator.GenerateTag(targetStream, strCase);

return tagToCompare.Equals(originalTag);
}

/** <summary> Compares the Size of a RSB Size and Displays an Alert in case an Anomally is Detected. </summary>

<param name = "sizeToCompare"> The Size of the RSB File to Compare. </param>
<param name = "hexBytes"> The Size Contained inside the SMF Metadata. </param>

<returns> <b>true</b> if the Two Tags are Equals; otherwise, <b>false</b>. </returns> */

private static bool CompareSize(Stream targetStream, byte[] hexBytes)
{
long originalSize = StandarMediaInfo.GetPlainSize(hexBytes);

return targetStream.Length.Equals(originalSize);
}

/** <summary> Compares two Adler32 Bytes Array and Displays an Alert in case an Anomally is Detected. </summary>

<param name = "targetStream"> The Stream from which to Compare the Tags. </param>
<param name = "pathToSmfTag"> The Path to the Original SMF Tag. </param>

<returns> <b>true</b> if the Two Tags are Equals; otherwise, </b>false</b>. </returns> */

private static bool CompareAdler32(Stream targetStream, byte[] adler32Bytes, Adler32BytesInfo adler32Cfg = null)
{
adler32Cfg ??= new();

byte[] checksumToCompare = Adler32BytesChecksum.GetAdler32Bytes(targetStream, adler32Cfg); 

return adler32Bytes.SequenceEqual(checksumToCompare);
}

/** <summary> Displays the Result of a Integrity Check on a RSB File. </summary>

<param name = "filePath"> The Path that belongs to the Opened Stream (used for Loading Tags). </param>
<param name = "targetStream"> The Stream to Check. </param>
<param name = "fileInfo"> The SMF used to Compare the File Integrity. </param> */

public static void IntegrityCheck(Stream targetStream, StandarMediaInfo fileInfo, IntegrityCheckType analisisType, 
StringCase? strCaseForTags = null, string pathToSmfTag = null, Adler32BytesInfo adler32Cfg = null)
{

bool result = analisisType switch
{
IntegrityCheckType.Tags => CompareTags(targetStream, (StringCase)strCaseForTags, pathToSmfTag),
IntegrityCheckType.HexSize => CompareSize(targetStream, fileInfo.HexSizeBeforeCompression),
IntegrityCheckType.Adler32 => CompareAdler32(targetStream, fileInfo.Adler32Bytes, adler32Cfg),

IntegrityCheckType.CompareTagsAndSize => CompareTags(targetStream, (StringCase)strCaseForTags, pathToSmfTag) &&
CompareSize(targetStream, fileInfo.HexSizeBeforeCompression),

IntegrityCheckType.CompareTagsAndAdler32 => CompareTags(targetStream, (StringCase)strCaseForTags, pathToSmfTag) &&
CompareAdler32(targetStream, fileInfo.Adler32Bytes, adler32Cfg),

IntegrityCheckType.CompareSizeAndAdler32 => CompareSize(targetStream, fileInfo.HexSizeBeforeCompression) &&
CompareAdler32(targetStream, fileInfo.Adler32Bytes, adler32Cfg),

IntegrityCheckType.FullScope => CompareTags(targetStream, (StringCase)strCaseForTags, pathToSmfTag) &&
CompareSize(targetStream, fileInfo.HexSizeBeforeCompression) &&
CompareAdler32(targetStream, fileInfo.Adler32Bytes, adler32Cfg),

_ => true
};

if(!result)
throw new SecurityException("The File is not Secure");

}

}

}
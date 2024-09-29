using System;
using System.IO;
using ZCore.Modules.Other;
using ZCore.Serializables.ArgumentsInfo.FileSecurity.Integrity;

namespace ZCore.Modules.FileSecurity
{
/// <summary> Initializes Adler32 Digest for Files. </summary>

public static class Adler32Checksum
{
/** <summary> Gets the Adler32 Checksum of a File. </summary>

<param name = "inputPath"> The Path where the File to Digest. </param>
<param name = "outputPath"> The Location where the Adler32 Checksum will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DigestFile(string inputPath, string outputPath, Adler32BytesInfo adler32Cfg)
{
using FileStream inputFile = File.OpenRead(inputPath);
 
PathHelper.ChangeExtension(ref outputPath, ".hash");
string hashedString = Adler32BytesChecksum.GetAdler32String(inputFile, adler32Cfg);

File.WriteAllText(outputPath, hashedString);
}

}

}
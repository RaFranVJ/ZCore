using System.IO;
using ZCore.Modules.Other;

namespace ZCore.Modules.FileSecurity
{
/// <summary> Initializes Digest Tasks for Files using the .NET Providers. </summary>

public static class DotNetHasher
{	
/** <summary> Hashes a File by using a Generic Digest. </summary>

<param name = "inputPath"> The Path where the File to be Hashed is Located. </param>
<param name = "outputPath"> The Path where the Hashed File will be Saved. </param>
<param name = "md5Info"> Specifies how Data should be Hashed. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "OutOfMemoryException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void HashFile(string inputPath, string outputPath, bool useHmac, string providerName,
StringCase strCase, byte[] authCode = null)
{
using FileStream inputFile = File.OpenRead(inputPath);
 
PathHelper.ChangeExtension(ref outputPath, ".hash");
string hashedString = StringDigest.DigestData(inputFile, useHmac, providerName, strCase, authCode);

File.WriteAllText(outputPath, hashedString);
}

}

}
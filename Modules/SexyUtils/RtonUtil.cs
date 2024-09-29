using System;
using System.IO;
using ZCore.Modules.SexyCryptors.CRton;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation;
using ZCore.Modules.SexyParsers.ReflectionObjectNotation.Parser;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.CRton;
using ZCore.Serializables.ArgumentsInfo.SexyParser.Rton;

namespace ZCore.Modules.SexyUtils
{
/// <summary> Initializes Parsing + Ciphering Tasks for RTON Files. </summary>

public static class RtonUtil
{
// Get C-RTON Stream from JSON or viceversa by using Default Config

public static Stream ProcessStream(BinaryStream input, bool operationMode, RtonSettings parseCfg = null,
CRtonSettings cryptoInfo = null, string outputPath = default, string pathToRefStrings = null)
{
parseCfg ??= new();
cryptoInfo ??= new();

if(operationMode)
{
using BinaryStream streamToEncrypt = RtonParser.EncodeStream(input, parseCfg.Endianness, 
parseCfg.ParserVersion, parseCfg.EncoderInfo, pathToRefStrings: pathToRefStrings);

streamToEncrypt.Position = 0;

if(outputPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase) )
PathHelper.ChangeExtension(ref outputPath, ".rton");

return RtonCryptor.EncryptStream(streamToEncrypt.ReadBytes(), cryptoInfo, outputPath);
}

using BinaryStream streamToDecode = RtonCryptor.DecryptStream(input, cryptoInfo);
streamToDecode.Position = 0;

return RtonParser.DecodeStream(streamToDecode, parseCfg.Endianness,
parseCfg.ParserVersion.AdaptCompatibilityBetweenVersions, parseCfg.DecoderInfo, 
outputPath, pathToRefStrings);

}

/** <summary> Encodes a JSON File as a RTON File, and then, Encrypts it by using Rijndael. </summary>

<param name = "inputPath"> The Path to the File to be Processed. </param>
<param name = "outputPath"> The Location where the Resulting File will be Saved. </param> 

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncodeAndEncryptFile(string inputPath, string outputPath,
RtonSettings parseCfg = null, CRtonSettings cryptoInfo = null)
{
parseCfg ??= new();

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToRefStrings = parseCfg.UseReferenceStrings ? ReferenceStringsHandler.GetRefFilePath(inputPath) : null;

using Stream outputFile = ProcessStream(inputFile, true, parseCfg, cryptoInfo, outputPath, pathToRefStrings);
}

/** <summary> Decrypts a RTON File by using Rijndael Ciphering, and then, Decodes it as a JSON File. </summary>

<param name = "inputPath"> The Path to the File to be Processed. </param>
<param name = "outputPath"> The Location where the Resulting File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecryptAndDecodeFile(string inputPath, string outputPath,
CRtonSettings cryptoInfo = null, RtonSettings parseCfg = null)
{
parseCfg ??= new();

using BinaryStream inputFile = BinaryStream.Open(inputPath);
string pathToRefStrings = parseCfg.UseReferenceStrings ? ReferenceStringsHandler.GetRefFilePath(inputPath) : null;

using Stream outputFile = ProcessStream(inputFile, false, parseCfg, cryptoInfo, outputPath, pathToRefStrings);
}

}

}
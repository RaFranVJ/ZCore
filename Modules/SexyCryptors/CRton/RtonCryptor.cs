using System;
using System.IO;
using System.Text;
using ZCore.Modules.Other;
using ZCore.Modules.SexyCryptors.CRton.Exceptions;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.CRton;

namespace ZCore.Modules.SexyCryptors.CRton
{
/// <summary> Initializes Ciphering Functions for RTON Files. </summary>

public class RtonCryptor : RijndaelCryptor
{
/** <summary> The Header of an Encrypted RTON File. </summary>
<remarks> It Occupies 2 Bytes in the Stream, which are: <c>0x10 0x00</c> </remarks> */

private const ushort CryptoHeader = 16;

/// <summary> Hashes the Cipher Key by using MD5. </summary>

private static byte[] HashKey(CRtonSettings cryptoInfo)
{
byte[] rtonKey = cryptoInfo.CipherKey;

if(cryptoInfo.DeriveKeys)
rtonKey = CryptoParams.CipherKeySchedule(cryptoInfo.CipherKey, GetKeySizeRange(), true,
cryptoInfo.SaltValue, cryptoInfo.HashType, cryptoInfo.IterationsCount);

string hashedKeyStr = StringDigest.DigestData(rtonKey, false, "MD5");

return Encoding.UTF8.GetBytes(hashedKeyStr);
}

/// <summary> Gets a IV from the given Cipher Key. </summary>

private static byte[] InitVector(byte[] cipherKey)
{
int vectorIndex = 4;

Limit<int> vectorSize = new(cipherKey.Length - vectorIndex * 2);

return CryptoParams.InitVector(cipherKey, vectorSize, 4);
}

// Get Crypto Stream

public static BinaryStream EncryptStream(byte[] inputBytes, CRtonSettings cryptoInfo = null, string outputPath = null)
{
cryptoInfo ??= new();

BinaryStream output = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

byte[] cipherKey = HashKey(cryptoInfo);
byte[] IV = InitVector(cipherKey);

byte[] encryptedData = CipherData(inputBytes, cipherKey, IV, true, cryptoInfo.BlockCipherName,
cryptoInfo.CipherPaddingIndex);

output.WriteUShort(CryptoHeader);
output.Write(encryptedData);

return output;
}	

/** <summary> Encrypts a RTON File by using Rijndael Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncryptFile(string inputPath, string outputPath, CRtonSettings cryptoInfo = null)
{
using BinaryStream outputFile = EncryptStream(File.ReadAllBytes(inputPath), cryptoInfo, outputPath);
}

// Get Plain Stream

public static BinaryStream DecryptStream(BinaryStream input, CRtonSettings cryptoInfo = null, string outputPath = null)
{
cryptoInfo ??= new();

input.CompareUShort<InvalidCRtonException>(CryptoHeader);

BinaryStream output = string.IsNullOrEmpty(outputPath) ? new() : BinaryStream.OpenWrite(outputPath);

byte[] cipherKey = HashKey(cryptoInfo);
byte[] IV = InitVector(cipherKey);

byte[] decryptedData = CipherData(input.ReadBytes(input.Length), cipherKey, IV,
false, cryptoInfo.BlockCipherName, cryptoInfo.CipherPaddingIndex);

output.Write(decryptedData);

return output;
}

/** <summary> Decrypts a RTON File by using Rijndael Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Decrypt is Located. </param>
<param name = "outputPath"> The Path where the Decrypt File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecryptFile(string inputPath, string outputPath, CRtonSettings cryptoInfo = null)
{
using BinaryStream inputFile = BinaryStream.Open(inputPath);

using Stream outputFile = DecryptStream(inputFile, cryptoInfo, outputPath);
}

}

}
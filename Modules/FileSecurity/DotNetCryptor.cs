using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ZCore.Serializables.ArgumentsInfo.Cryptor;

namespace ZCore.Modules.FileSecurity
{
/// <summary> Initializes Ciphering Tasks for Files by using the .NET Providers. </summary>

public static class DotNetCryptor
{
// Get Size Range for Symmetric Keys

private static Limit<int> GetKeySizeRange(SymmetricAlgorithm cipherAlg)
{
KeySizes[] keySizes = cipherAlg.LegalKeySizes;

int minSize = keySizes.Min(k => k.MinSize);
int maxSize = keySizes.Max(j => j.MaxSize);

return new(minSize, maxSize);
}

// Get Size Range for IV

private static Limit<int> GetIVLength(SymmetricAlgorithm cipherAlg) => new(cipherAlg.BlockSize / 8);

// Create ICryptoTransform by using a Symmetric Key, IV and Operation Mode

private static ICryptoTransform GetTransform(bool isForEncryption, SymmetricAlgorithm cipherAlg, byte[] cipherKey = null, byte[] IV = null)
{

if(isForEncryption)
return cipherAlg.CreateEncryptor(cipherKey, IV);

return cipherAlg.CreateDecryptor(cipherKey, IV);
}

/** <summary> Gets a Path to the exported Key. </summary>

<param name = "filePath"> The Path to the File where the Key should be Saved. </param>

<returns> The Path to the generated Key. </returns> */

private static string GetCryptoPath(string containerName, string filePath) => PathHelper.BuildPathFromDir(containerName, filePath, ".bin");

// Returns a new Key or the given Key if its Length is Valid

public static byte[] ValidateKey(SymmetricAlgorithm cipherAlg, bool isForEncryption, bool deriveKeys, string keyPath,
byte[] key = null, byte[] salt = null, string hashType = null, uint? iterations = null)
{
Limit<int> keySizeRange = GetKeySizeRange(cipherAlg);

if(key != null && deriveKeys)
key = CryptoParams.CipherKeySchedule(key, keySizeRange, deriveKeys, salt, hashType, iterations);

else if(key == null && isForEncryption)
{
cipherAlg.GenerateKey();
key = cipherAlg.Key;

File.WriteAllBytes(keyPath, key);
}

else if(key == null && !isForEncryption)
key = File.ReadAllBytes(keyPath);

CryptoParams.CheckKeySize(key, keySizeRange);

return key;
}

// Validate IV

public static byte[] ValidateIV(SymmetricAlgorithm cipherAlg, bool isForEncryption, bool randomIV,
byte[] key, string ivPath, byte[] IV = null)
{
Limit<int> ivLength = GetIVLength(cipherAlg);

if(IV == null && randomIV && isForEncryption)
{
cipherAlg.GenerateIV();

IV = cipherAlg.IV;

File.WriteAllBytes(ivPath, IV);
}

else if(IV == null && randomIV && !isForEncryption)
IV = File.ReadAllBytes(ivPath);

else if(IV == null)
CryptoParams.InitVector(key, ivLength);

CryptoParams.CheckIVLength(IV, ivLength);

return IV;
}

// Get Crypto Stream

public static CryptoStream CipherStream(Stream input, Stream output, bool isForEncryption,
SpecificCryptoInfo cfg = default, string keyPath = default, string ivPath = default)
{
cfg ??= new();

keyPath = string.IsNullOrEmpty(keyPath) ? cfg.KeyContainer + "MyCipherKey.bin" : keyPath;
ivPath = string.IsNullOrEmpty(ivPath) ? cfg.IVContainer + "MyIV.bin" : ivPath;

#pragma warning disable SYSLIB0045 // Type or member is obsolete
SymmetricAlgorithm cipherAlg = SymmetricAlgorithm.Create(cfg.ProviderName);

cipherAlg.Mode = cfg.CipheringMode;
cipherAlg.Padding = cfg.DataPadding;

byte[] key = ValidateKey(cipherAlg, cfg.DeriveKeys, isForEncryption, keyPath, 
cfg.CipherKey, cfg.SaltValue, cfg.HashType, cfg.IterationsCount);

byte[] iv = ValidateIV(cipherAlg, isForEncryption, cfg.RandomizeIVS, key, ivPath, cfg.IV);

ICryptoTransform transform = GetTransform(isForEncryption, cipherAlg, key, iv);
CryptoStream cipheredStream = new(output, transform, CryptoStreamMode.Write);

FileManager.ProcessBuffer(input, cipheredStream, cfg.BufferSizeForIOTasks);
cipheredStream.FlushFinalBlock();

return cipheredStream;
}

/** <summary> Performs Generic Task for Ciphering on Files. </summary>

<param name = "inputPath"> Th Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>
<param name = "isForEncryption"> Determines if the Files should be Encrypted or not. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception> */

private static void CipherFile(string inputPath, string outputPath, bool isForEncryption, SpecificCryptoInfo cfg)
{
using FileStream inputFile = File.OpenRead(inputPath);
using FileStream outputFile = File.OpenWrite(outputPath);

string keyPath = GetCryptoPath(inputPath, cfg.KeyContainer);
string ivPath = GetCryptoPath(inputPath, cfg.IVContainer);

using CryptoStream cipheredStream = CipherStream(inputFile, outputFile, isForEncryption, cfg, keyPath, ivPath);
}

/** <summary> Encrypts a File by using a Generic Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception> */

public static void Encrypt(string inputPath, string outputPath, SpecificCryptoInfo cfg = null) => CipherFile(inputPath, outputPath, true, cfg);

/** <summary> Decrypts a File by using a Generic Decryption. </summary>

<param name = "inputPath"> The Path where the File to be Decrypted is Located. </param>
<param name = "outputPath"> The Location where the Decrypted File will be Saved. </param>

<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception> */

public static void Decrypt(string inputPath, string outputPath, SpecificCryptoInfo cfg = null) => CipherFile(inputPath, outputPath, false, cfg);
}

}
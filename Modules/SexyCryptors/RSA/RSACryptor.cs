using RsaObj = System.Security.Cryptography.RSA;

using System;
using System.IO;
using System.Security.Cryptography;
using ZCore.Serializables.ArgumentsInfo.SexyCryptor.RSA;

namespace ZCore.Modules.SexyCryptors.RSA
{
/// <summary> Initializes RSA (Rivest, Shamir and Adleman) Ciphering Tasks for Files. </summary>

public static class RSACryptor
{
/** <summary> Gets the Path to the RSA Key File. </summary>

<param name = "basePath"> The Path to the File to be Encrypted. </param>

<returns> The RSA Path. </returns> */

private static string GetRsaKeyPath(string containerPath, string basePath, bool isPrivateKey)
{
string keySuffix = isPrivateKey ? "PrivateKey" : "PublicKey";

return PathHelper.BuildPathFromDir(containerPath, basePath, ".xml", keySuffix);
}

/** <summary> Saves the RSA Key from the given <c>RSACryptoServiceProvider</c>. </summary>

<param name = "targetPath"> The Path to the RSA Key. </param>
<param name = "serviceProvider"> The <c>RSACryptoServiceProvider</c> where the Keys will be Retrieved from. </param> */

private static void SaveRsaKey(string targetPath, bool isPrivateKey, RsaObj serviceProvider)
{
File.WriteAllText(targetPath, serviceProvider.ToXmlString(isPrivateKey) );
}

/** <summary> Loads the RSA Key to the given <c>RSACryptoServiceProvider</c>. </summary>

<param name = "targetPath"> The Path to the RSA Key. </param>
<param name = "serviceProvider"> The <c>RSACryptoServiceProvider</c> where the Keys will be Retrieved. </param> */

private static void LoadRsaKey(string targetPath, bool isPrivateKey, ref RsaObj serviceProvider)
{

if(!File.Exists(targetPath) )
throw new FileNotFoundException(string.Format("The Container for \"{0}\" is Empty or does not Exist.", Path.GetFileName(targetPath) ) );

serviceProvider.FromXmlString( File.ReadAllText(targetPath) );
}

/** <summary> Gets the Maximum Size in Bytes of a DataBlock by using the given ServiceProvider. </summary>
<param name = "serviceProvider"> The ServiceProvider to be use. </param>

<param name = "useOAEP"> A Boolean that Determines if OAEP should be Used when Encrypting the Data.<para>
</para> OAEP stands for Optimal Asynmetric Encryption Padding. </param>

<returns> The Block Size. </returns> */

private static int GetBlockSize(RsaObj serviceProvider, bool useOAEP)
{
int paddingSize = useOAEP ? 42 : 11;

return (serviceProvider.KeySize/8) - paddingSize;
}

// Get Service Provider according to User Platform

private static RsaObj GetServiceProvider<T>(T rsaCfg) where T : RSASettings
{

#if WINDOWS || WINDOWSCONSOLE

var windowsCfg = rsaCfg as RSASettings_Windows;

CspParameters serviceParams = new()
{
KeyContainerName = windowsCfg.CspKeyContainerName,
KeyNumber = windowsCfg.CspKeyNumber,
Flags = windowsCfg.PersistKeyInCsp ? CspProviderFlags.UseExistingKey : CspProviderFlags.NoFlags
};

return new RSACryptoServiceProvider(serviceParams)
{
PersistKeyInCsp = windowsCfg.PersistKeyInCsp,
KeySize = (int)windowsCfg.CipherKeySize
};

#endif

return RsaObj.Create( (int)rsaCfg.CipherKeySize);
}

// Get RSA Stream or Plain Stream

public static Stream CipherStream<T>(Stream input, bool cipherMode, T rsaCfg, string outputPath = default,
string keyContainerName = "My RSA Key", RsaObj serviceProvider = null) where T : RSASettings
{
string keyPath = GetRsaKeyPath(rsaCfg.PathToKeyContainer, keyContainerName, rsaCfg.UsePrivateKey);

PathHelper.CheckDuplicatedPath(ref keyPath);

if(serviceProvider == null && cipherMode)
serviceProvider = GetServiceProvider(rsaCfg);

else if(serviceProvider == null && !cipherMode)
LoadRsaKey(keyPath, rsaCfg.UsePrivateKey, ref serviceProvider);

Stream output = string.IsNullOrEmpty(outputPath) ? new MemoryStream() : File.OpenWrite(outputPath);
var padding = rsaCfg.UseOAEP ? RSAEncryptionPadding.OaepSHA1 : RSAEncryptionPadding.Pkcs1;

byte[] processFunc(byte[] bufferData) => cipherMode ?
serviceProvider.Encrypt(bufferData, padding) :
serviceProvider.Decrypt(bufferData, padding);

int bufferSize = GetBlockSize(serviceProvider, rsaCfg.UseOAEP);
FileManager.ProcessBuffer(input, output, processFunc, bufferSize);

if(cipherMode)
SaveRsaKey(keyPath, rsaCfg.UsePrivateKey, serviceProvider);

return output;
}

/** <summary> Encrypts a File by using RSA Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Encrypted is Located. </param>
<param name = "outputPath"> The Location where the Encrypted File will be Saved. </param>

<exception cref = "ArgumentNullException"></exception>
<exception cref = "CryptographicException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void EncryptFile<T>(string inputPath, string outputPath, T rsaCfg) where T : RSASettings
{
using FileStream inputFile = File.OpenRead(inputPath);

using Stream outputFile = CipherStream(inputFile, true, rsaCfg, outputPath, inputPath);
}

/** <summary> Decrypt a File by using RSA Ciphering. </summary>

<param name = "inputPath"> The Path where the File to be Decrypt is Located. </param>
<param name = "outputPath"> The Path where the Decrypt File will be Saved. </param>

<exception cref = "ArgumentException"></exception>
<exception cref = "ArgumentNullException"></exception>
<exception cref = "FileNotFoundException"></exception>
<exception cref = "IOException"></exception>
<exception cref = "NotSupportedException"></exception>
<exception cref = "OutOfMemoryException"></exception>
<exception cref = "SecurityException"></exception>
<exception cref = "UnauthorizedAccessException"></exception> */

public static void DecryptFile<T>(string inputPath, string outputPath, T rsaCfg) where T : RSASettings
{
using FileStream inputFile = File.OpenRead(inputPath);

using Stream outputFile = CipherStream(inputFile, false, rsaCfg, outputPath, inputPath);
}

}

}
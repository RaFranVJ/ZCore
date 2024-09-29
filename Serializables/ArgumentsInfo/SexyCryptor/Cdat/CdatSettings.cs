using System.Text;
using ZCore.Serializables.ArgumentsInfo.Cryptor;
using ZCore.Serializables.ArgumentsInfo.FileManager.Metadata;

namespace ZCore.Serializables.ArgumentsInfo.SexyCryptor.Cdat
{
/// <summary> Groups Info related to the PopCap Data Cryptor. </summary>

public class CdatSettings : GenericCryptoInfo
{
/** <summary> Gets or Sets the Endian Order used when Parsing Files. </summary>
<returns> The Endian Order. </returns> */

public Endian Endianness{ get; set; }

/** <summary> Gets or Sets the Number of Bytes to Encrypt or Decrypt. </summary>

<remarks> Setting this Field to a Value lower than 0, will Cause the Encryptor to Cipher the whole File. </remarks>

<returns> The Number of Bytes to Cipher. </returns> */

public int NumberOfBytesToCipher{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Info should be Used for Decrypting CDAT Files. </summary>
<returns> <b>true</b> if Size Info should be Used; otherwise, <b>false</b>. </returns> */

public bool UseSizeInfoForDecryption{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if CDAT Files should be Compared by Size. </summary>
<returns> <b>true</b> if Size Before Encrytion should be Compared; otherwise, <b>false</b>. </returns> */

public bool CompareSizeBeforeEncryption{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside CDAT Files should be Saved. </summary>
<returns> The CDAT Export Config. </returns> */

public MetadataImportParams MetadataImportConfig{ get; set; }

/** <summary> Gets or Sets some Info related to how the Info inside CDAT Files should be Saved. </summary>
<returns> The CDAT Export Config. </returns> */

public MetadataExportParams MetadataExportConfig{ get; set; }

/// <summary> Creates a new Instance of the <c>CdatInfo</c>. </summary>

public CdatSettings()
{
NumberOfBytesToCipher = 0x100;
UseSizeInfoForDecryption = true;

MetadataImportConfig = new();
MetadataExportConfig = new();

CipherKey = Encoding.UTF8.GetBytes("AS23DSREPLKL335KO4439032N8345NF");
DeriveKeys = false;

SaltValue = null;
HashType = null;
}

/// <summary> Checks each nullable Field of the <c>RijndaelInfo</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
CdatSettings defaultInfo = new();

#region ======== Set default Values to Null Fields ========

MetadataImportConfig ??= defaultInfo.MetadataImportConfig;
MetadataExportConfig ??= defaultInfo.MetadataExportConfig;
CipherKey ??= defaultInfo.CipherKey;

#endregion

BufferSizeForIOTasks = BufferSizeRange.CheckParamRange(BufferSizeForIOTasks);
}

}

}
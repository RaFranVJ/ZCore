using System;
using ZCore.Serializables.ArgumentsInfo.Cryptor;

namespace ZCore.Serializables.ArgumentsInfo.SexyCryptor.Rijndael
{
/// <summary> Groups Info related to specific Params used for Ciphering Data with Rijndael. </summary>

public class RijndaelSettings : GenericCryptoInfo_NoPadding
{
/** <summary> Gets or Sets the BlockCipherName used for Encrypting/Decrypting Data. </summary>
<returns> The BlockCipher Name. </returns> */

public string BlockCipherName{ get; set; }

/** <summary> Gets or Sets the CipherPadding Index used for Encrypting/Decrypting Data. </summary>
<returns> The CipherPadding Index. </returns> */

public int CipherPaddingIndex{ get; set; }

/// <summary> Creates a new Instance of the <c>RijndaelInfo</c>. </summary>

public RijndaelSettings()
{
BlockCipherName = "CBC";
}

/// <summary> Checks each nullable Field of the <c>RijndaelInfo</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RijndaelSettings defaultInfo = new();

#region ======== Set default Values to Null Fields ========

BlockCipherName ??= defaultInfo.BlockCipherName;

#endregion
}

}

}
using RsaObj = System.Security.Cryptography.RSA;

using System.IO;

namespace ZCore.Serializables.ArgumentsInfo.SexyCryptor.RSA
{
/// <summary> Groups Info related to specific Params used for Ciphering Data with RSA. </summary>

public class RSASettings : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines which Key should be used, Private or Public. </summary>
<returns> <b>true</b> if Private Keys should be Used; otherwise, <b>false</b>. </returns> */

public uint CipherKeySize{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines which Key should be used, Private or Public. </summary>
<returns> <b>true</b> if Private Keys should be Used; otherwise, <b>false</b>. </returns> */

public bool UsePrivateKey{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Files should be Ciphered with OAEP or not. </summary>
<returns> <b>true</b> if OAEP should be Used; otherwise, <b>false</b>. </returns> */

public bool UseOAEP{ get; set; }

/** <summary> Gets or Sets the Path where the Key Container should be Saved or Loaded. </summary>
<returns> The Container Path. </returns> */

public string PathToKeyContainer{ get; set; }

/// <summary> Creates a new Instance of the <c>RsaSettings</c>. </summary>

public RSASettings()
{
using RsaObj rsa = RsaObj.Create();

CipherKeySize = (uint)rsa.LegalKeySizes[0].MinSize;
UseOAEP = true;

PathToKeyContainer = GetDefaultContainerPath();
}

/// <summary> Checks each nullable Field of the <c>RsaInfo</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RSASettings defaultInfo = new();

#region ======== Set default Values to Null Fields ========

PathToKeyContainer ??= defaultInfo.PathToKeyContainer;

#endregion

PathHelper.CheckExistingPath(PathToKeyContainer, true);
}

/** <summary> Gets the default Path to the CspContainer basing on the CurrentAppDirectory. </summary>
<returns> The default Container Path. </returns> */

protected static string GetDefaultContainerPath() => LibInfo.CurrentDllDirectory + Path.DirectorySeparatorChar + typeof(RSASettings).Name;
}

}
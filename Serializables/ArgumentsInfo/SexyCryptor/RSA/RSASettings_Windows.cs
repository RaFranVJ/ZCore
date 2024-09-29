using System.Security.Cryptography;

namespace ZCore.Serializables.ArgumentsInfo.SexyCryptor.RSA
{
/// <summary> Groups Info related to the RSA Algorithm using Exclusive Features for Windows Users (like CSP). </summary>

public class RSASettings_Windows : RSASettings
{
/** <summary> Gets or Sets the CSP KeyNumber. </summary>
<returns> The Key Number. </returns> */

public int CspKeyNumber{ get; set; }

/** <summary> Gets or Sets the Name of the CSP Key Container. </summary>
<returns> The Container Name. </returns> */

public string CspKeyContainerName{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Persistent CSP Keys should be Generated. </summary>
<returns> <b>true</b> if Persistent Keys should be Used; otherwise, <b>false</b>. </returns> */

public bool PersistKeyInCsp{ get; set; }

/// <summary> Creates a new Instance of the <c>RsaSettings</c> for Windows. </summary>

public RSASettings_Windows()
{
CspKeyNumber = (int)KeyNumber.Exchange;
CspKeyContainerName = $"My CspKeyContainer #{CspKeyNumber}";
}

/// <summary> Checks each nullable Field of the <c>RsaInfo</c> instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RSASettings_Windows defaultInfo = new();

#region ======== Set default Values to Null Fields ========

CspKeyContainerName ??= defaultInfo.CspKeyContainerName;
PathToKeyContainer ??= defaultInfo.PathToKeyContainer;

#endregion

PathHelper.CheckExistingPath(PathToKeyContainer, true);
}

}

}
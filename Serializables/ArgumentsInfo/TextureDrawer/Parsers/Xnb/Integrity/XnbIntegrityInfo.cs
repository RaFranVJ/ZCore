namespace ZCore.Serializables.ArgumentsInfo.TextureDrawer.Parsers.Xnb.Integrity
{
/// <summary> Groups options related to the Integrity Check for XNB Files. </summary>

public class XnbIntegrityInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines if Integrity Check should be Done on Decoding. </summary>
<returns> <b>true</b> if Integrity Check should be Done when Decoding XNB Files; otherwise, <b>false</b>. </returns> */

public bool CheckIntegrityOnDecoding{ get; set; }

/** <summary> Gets or Sets a Value that Determines how the Integrity Check should be Done. </summary>
<returns> The Analisis Type. </returns> */

public IntegrityCheckType AnalisisType{ get; set; }

/// <summary> Creates a new Instance of the <c>IntegrityInfo</c>. </summary>

public XnbIntegrityInfo()
{
CheckIntegrityOnDecoding = true;
AnalisisType = IntegrityCheckType.TextureComparisson;
}

}

}
namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Smf.Integrity
{
/// <summary> Groups options related to the Integrity Check for SMF Files. </summary>

public class SmfIntegrityInfo : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines if Integrity Check should be Done on Decompression. </summary>
<returns> <b>true</b> if Integrity Check should be Done when Decompressing SMF Files; otherwise, <b>false</b>. </returns> */

public bool CheckIntegrityOnDecompression{ get; set; }

/** <summary> Gets or Sets a Value that Determines how the Integrity Check should be Done. </summary>
<returns> The Analisis Type. </returns> */

public IntegrityCheckType AnalisisType{ get; set; }

/// <summary> Creates a new Instance of the <c>IntegrityInfo</c>. </summary>

public SmfIntegrityInfo()
{
AnalisisType = IntegrityCheckType.Tags;
}

}

}
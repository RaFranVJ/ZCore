namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsg
{
/// <summary> Groups options related to the RSG Extractor. </summary>

public class RsgExtractParams : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines if Entries should be Extracted from <c>Part0</c>. </summary>
<returns> <b>true</b> if Entries should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractEntriesFromPart0{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Entries should be Extracted from <c>Part1</c>. </summary>
<returns> <b>true</b> if Entries should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractEntriesFromPart1{ get; set; }

/// <summary> Creates a new Instance of the <c>RsgExtractParams</c>. </summary>

public RsgExtractParams()
{
}

}

}
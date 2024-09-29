namespace ZCore.Serializables.ArgumentsInfo.SexyCompressor.Rsb
{
/// <summary> Groups options related to the RSB Extractor. </summary>

public class RsbExtractParams : ParamGroupInfo
{
/** <summary> Gets or Sets a Boolean that Determines if a <c>FileList</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>FileList</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractFileList{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if a <c>RsgList</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>RsgList</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractRsgList{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>CompositeInfo</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>CompositeInfo</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractCompositeInfo{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if a <c>CompositeList</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>CompositeList</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractCompositeList{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>RsgInfo</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>RsgInfo</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractRsgInfo{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>AutoPoolInfo</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>AutoPoolInfo</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractAutoPoolInfo{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>PtxInfo</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>PtxInfo</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractPtxInfo{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>RsgFiles</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>RsgFiles</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractRsgFiles{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Content from <c>RsgFiles</c> should be Extracted. </summary>
<returns> <b>true</b> if Content from <c>RsgFiles</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractContentFromRsgFiles{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if <c>RsgFiles</c> should be Extracted from RSB Files. </summary>
<returns> <b>true</b> if <c>RsgFiles</c> should be Extracted; otherwise, <b>false</b>. </returns> */

public bool ExtractInfoFromRsgPackets{ get; set; }

/// <summary> Creates a new Instance of the <c>RsbExtractParams</c>. </summary>

public RsbExtractParams()
{
ExtractRsgFiles = true;
}

}

}
using System.Text.Encodings.Web;

namespace ZCore.Serializables.ArgumentsInfo.SexyParser.Rton
{
/// <summary> Groups Info related to the RTON Decoder. </summary>

public class RtonDecoderInfo : ParamGroupInfo
{
/** <summary> The allowed Data Depth. </summary>
<remarks> Minimum Depth is <b>0</b>; Maximum is <b>1000</b>. </remarks> */

private static readonly Limit<int> DataDepthRange = new(0, 1000);

/** <summary> Gets or Sets an Instance of the <c>JavaScriptEncoder</c> used when Escaping strings. </summary>

<remarks> Set this Property to <c>null</c> in Order to Use the default Encoder. </remarks>

<returns> The JSON Encoder. </returns> */

public string JsonEncoder{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if the JSON Data should be Indented. </summary>

<remarks> An Indented Formatting means that new Lines will be Added for Separating each JSON Token.<para>
</para>Also, Whitespaces should be Added between Property Names and Values for Indented JSON Data. </remarks>

<returns> <b>true</b> if Indented Formating should be Used; otherwise, <b>false</b>. </returns> */

public bool FormattingIndented{ get; set; }

/** <summary> Gets or Sets the Maximum Depth when Writting JSON Data. </summary>
<returns> The Max Data Depth. </returns> */

public int MaxDataDepth{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if the JSON Validation should be Omitted or not. </summary>

<remarks> Skipping the Json Validation means allowing the User to Write JSON Data even if they are Errors on its Syntax. </remarks>

<returns> <b>true</b> if JSON Validation should be Omitted; otherwise, <b>false</b>. </returns> */

public bool SkipJsonValidation{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Length Mismatch should be Ignored when Reading Strings. </summary>
<returns> <b>true</b> if StrLength Mismatch should be Ignored; otherwise, <b>false</b>. </returns> */

public bool IgnoreStrLengthMismatch{ get; set; }

/// <summary> Creates a new Instance of the <c>RtonDecoderInfo</c>. </summary>

public RtonDecoderInfo()
{
JsonEncoder = "UnsafeRelaxedJsonEscaping";
FormattingIndented = true;

MaxDataDepth = DataDepthRange.MaxValue;
}

///<summary> Checks each nullable Field of the <c>RtonDecoderInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields()
{
RtonDecoderInfo defaultInfo = new();

#region ======== Set default Values to Null Fields ========

JsonEncoder ??= defaultInfo.JsonEncoder;

#endregion

MaxDataDepth = DataDepthRange.CheckParamRange(MaxDataDepth);
}

}

}
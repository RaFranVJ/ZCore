using System.Text.Json;

namespace ZCore.Serializables.ArgumentsInfo.SexyParser.Rton
{
/// <summary> Groups Info related to the RTON Encoder. </summary>

public class RtonEncoderInfo : ParamGroupInfo
{
/** <summary> The allowed Data Depth. </summary>
<remarks> Minimum Depth is <b>0</b>; Maximum is <b>64</b>. </remarks> */

private static readonly Limit<int> DataDepthRange = new(0, 64);

/** <summary> Gets or Sets a Boolean that Determines if Trailing Commas should be Ignored or not. <para>
</para> Extra Commas can be Found at the end of a JSON object or a JSON array. </summary>

<remarks> In case the Commas are Ignored, it means no Exception should be Raised when Encoding JSON Data (not Recomended). </remarks>

<returns> <b>true</b> if Commas should be Ignored/Allowed; otherwise, <b>false</b>. </returns> */

public bool IgnoreExtraCommas{ get; set; }

/** <summary> Gets or Sets a Value that Determines how JSON Comments should be Handled. </summary>
<returns> The JSON Comments Handling. </returns> */

public JsonCommentHandling CommentsHandling{ get; set; }

/** <summary> Gets or Sets the Maximum Depth when Encoding JSON Data. </summary>
<returns> The Max Data Depth. </returns> */

public int MaxDataDepth{ get; set; }

/** <summary> Gets or Sets a Boolean that Determines if Strings should be Encoded with UTF8. </summary>
<returns> <b>true</b> if Strings should be Encoded with UTF8; otherwise, <b>false</b>. </returns> */

public bool EncodeStringsWithUtf8{ get; set; }

/// <summary> Creates a new Instance of the <c>RtonEncoderInfo</c>. </summary>

public RtonEncoderInfo()
{
CommentsHandling = JsonCommentHandling.Skip;
MaxDataDepth = DataDepthRange.MaxValue;
}

/// <summary> Checks each nullable Field of the <c>RtonEncoderInfo</c> Instance and Validates it, in case it's <c>null</c>. </summary>

public override void CheckForNullFields() => MaxDataDepth = DataDepthRange.CheckParamRange(MaxDataDepth);
}

}
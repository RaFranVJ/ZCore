namespace ZCore.Modules.SexyCompressors.ResourceStreamBundle.Definitions.Resources
{
/// <summary> Represents a Resource Property with a Key and a Value </summary>

public class ResPropertiesInfo
{
/** <summary> Gets or Sets the PropertyName of a Resource. </summary>
<returns> The PropertyName (or Key) </returns> */

public string Key{ get; set; }

/** <summary> Gets or Sets the PropertyValue of a Resource. </summary>
<returns> The PathToResFile </returns> */

public string Value{ get; set; }

/// <summary> Creates a new Instance of the <c>ResPropertiesInfo</c> </summary>

public ResPropertiesInfo()
{
Key = "<ResKey>";
Value = "<ResValue>";
}

/// <summary> Creates a new Instance of the <c>ResPropertiesInfo</c> </summary>

public ResPropertiesInfo(string key, string value)
{
Key = key;
Value = value;
}

}

}
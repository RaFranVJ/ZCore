namespace ZCore.Modules.SexyCompressors.ResourceStreamGroup.Definitions.FilePath
{
/// <summary> Stores Info related to a PathName inside a RSG </summary>
	
public class NameDict
{
/** <summary> Gets or Sets the PathName. </summary>
<returns> The PathName </returns> */

public string PathName{ get; set; }

/** <summary> Gets or Sets the ByteOffset. </summary>
<returns> The ByteOffset </returns> */
		
public int ByteOffset{ get; set; }

/// <summary> Creates a new Instance of the <c>NameDict</c> </summary>

public NameDict()
{
}

/// <summary> Creates a new Instance of the <c>PathPositionInfo</c> </summary>

public NameDict(string pathName, int offset)
{
PathName = pathName;
ByteOffset = offset;
}

}

}
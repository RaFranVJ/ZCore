using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using ZCore.Modules;

/// <summary> Represents a Table of different Objects. </summary>

[DataContract]

public class SexyObjTable : SerializableClass<SexyObjTable>
{
/** <summary> Gets or Sets a Comment for this Table. </summary>
<returns> The Json Comment. </returns> */

[DataMember(Name="#comment") ]

public string Comment{ get; set; }

/** <summary> Gets or Sets the Version of the Table. </summary>
<returns> The Version (Default is 1). </returns> */

[DataMember(Name="version") ]

public uint Version{ get; set; } = 1;

/** <summary> Gets or Sets a List of Objects for this Table. </summary>
<returns> The List of Objects. </returns> */

[DataMember(Name="objects") ]

public List<SexyObj> Objects{ get; set; } = new();

// ctor

public SexyObjTable()
{
}

// ctor 2

public SexyObjTable(List<SexyObj> objs)
{
Objects = objs;
}

// ctor 3

public SexyObjTable(string comment, uint ver, List<SexyObj> objs)
{
Comment = comment;

Version = ver;
Objects = objs;
}

// Check for null Fields

protected override void CheckForNullFields() => Objects ??= new();

// Check current Instance

public void CheckObjs() => CheckForNullFields();

/** <summary> Reads a JSON Table. </summary>

<param name = "sourcePath"> The Path where to Read the Table (default is already Set). </param>

<returns> The Table Read. </returns> */

public override SexyObjTable ReadObject(string sourcePath = default)
{
sourcePath ??= FilePath;

DirManager.CheckMissingFolder(Path.GetDirectoryName(sourcePath) );

if(!File.Exists(sourcePath) || FileManager.FileIsEmpty(sourcePath) )
WriteObject(sourcePath);

var jObject = JObject.Parse(File.ReadAllText(sourcePath) ); // Alternative for Unk JSON Struct
SexyObjTable targetObj = new();

targetObj.Comment = (string)jObject["#comment"];
targetObj.Version = (uint?)jObject["version"] ?? 1;

targetObj.Objects = jObject["objects"]?.ToObject<List<SexyObj>>() ?? new();

return targetObj;
}

}

/// <summary> Represents a Table of Objects from the same Type. </summary>

[DataContract]

public abstract class SexyObjTable<T> : SerializableClass<SexyObjTable<T>> where T : class
{
/** <summary> Gets or Sets a Comment for this Table. </summary>
<returns> The Json Comment. </returns> */

[DataMember(Name="#comment") ]

public string Comment{ get; set; }

/** <summary> Gets or Sets the Version of the Table. </summary>
<returns> The Version (Default is 1). </returns> */

[DataMember(Name="version") ]

public uint Version{ get; set; } = 1;

/** <summary> Gets or Sets a List of Objects for this Table. </summary>
<returns> The List of Objects. </returns> */

[DataMember(Name="objects") ]

public List<T> Objects{ get; set; } = new();

// Check for null Fields

public abstract void CheckObjs();
}
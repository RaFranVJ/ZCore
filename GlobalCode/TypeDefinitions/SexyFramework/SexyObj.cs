using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using ZCore.Modules;


/// <summary> Represents a Generic Object in the SexyFramework. </summary>

[DataContract]

public class SexyObj : SerializableClass<SexyObj>
{
/** <summary> Gets or Sets a Comment for this Object. </summary>
<returns> The Json Comment. </returns> */

[DataMember(Name="#comment") ]

public string Comment{ get; set; }
    
/** <summary> Gets or Sets the Aliases for this Object. </summary>

<returns> The Obj Aliases or null if Obj has no Aliases. </returns> */

[DataMember(Name="aliases") ]

public List<string> Aliases{ get; set; }

/** <summary> Gets or Sets the Name of the Object. </summary>
<returns> The Obj ClassName. </returns> */

[DataMember(Name="objclass") ]

public string ObjClass{ get; set; }

/** <summary> Gets or Sets the Instance of the Object. </summary>
<returns> The Obj itself. </returns> */

[DataMember(Name="objdata") ]

public ExpandoObject ObjData{ get; set; } = new();

// ctor

public SexyObj()
{
ObjClass = "MyClassTemplate";
}

public SexyObj(string comment, List<string> aliases, string objClass)
{
Comment = comment;
Aliases = aliases;

ObjClass = objClass;
}

public SexyObj(string comment, List<string> aliases, string objClass, ExpandoObject objData)
{
Comment = comment;
Aliases = aliases;

ObjClass = objClass;
ObjData = objData;
}

/** <summary> Reads a JSON Object. </summary>

<param name = "sourcePath"> The Path where to Read the Obj (default is already Set). </param>

<returns> The Obj Read. </returns> */

public override SexyObj ReadObject(string sourcePath = default)
{
sourcePath ??= FilePath;

DirManager.CheckMissingFolder(Path.GetDirectoryName(sourcePath) );

if(!File.Exists(sourcePath) || FileManager.FileIsEmpty(sourcePath) )
WriteObject(sourcePath);

var jObject = JObject.Parse(File.ReadAllText(sourcePath) ); // Alternative for Unk JSON Struct
SexyObj targetObj = new();

targetObj.Comment = (string)jObject["#comment"];
targetObj.Aliases = jObject["aliases"]?.ToObject<List<string>>();

targetObj.ObjClass = jObject["objclass"]?.ToObject<string>() ?? "MyClassTemplate";
targetObj.ObjData = ExpandObjPlugin.ToExpandoObject(jObject["objdata"]?.ToObject<JObject>() );

return targetObj;
}

}

/// <summary> Represents an Object that uses Typing in the SexyFramework. </summary>

[DataContract]

public abstract class SexyObj<T> : SerializableClass<SexyObj<T>> where T : class
{
/** <summary> Gets or Sets a Comment for this Object. </summary>
<returns> The Json Comment. </returns> */

[DataMember(Name="#comment") ]

public string Comment{ get; set; }

/** <summary> Gets or Sets the Aliases for this Object. </summary>
<returns> The Obj Aliases. </returns> */

[DataMember(Name="aliases") ]

public List<string> Aliases{ get; set; } = new();

/** <summary> Gets or Sets the Name of the Object. </summary>
<returns> The Obj ClassName. </returns> */

[DataMember(Name="objclass") ]

public string ObjClass{ get; set; }

/** <summary> Gets or Sets the Instance of the Object. </summary>
<returns> The Obj itself. </returns> */

[DataMember(Name="objdata") ]

public T ObjData{ get; set; }
}
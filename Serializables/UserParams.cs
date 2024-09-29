using System;
using System.IO;
using System.Reflection;
using ZCore.Serializables.Arguments.ParamGroups;

namespace ZCore.Serializables
{
/// <summary> Stores groups of Params which the User can Set in order to Perform a specific Task. </summary>

public class UserParams : SerializableClass<UserParams>
{
/** <summary> Gets or Sets some Params for the <c>FileManager</c>. </summary>
<returns> The <c>FileManager</c> Params. </returns> */

public FileManagerArgs FileManagerParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>FileParsers</c>. </summary>
<returns> The <c>FileParsers</c> Params. </returns> */

public FileParsersArgs FileParsersParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>FileCompressors</c>. </summary>
<returns> The <c>FileCompressors</c> Params. </returns> */

public FileCompressorsArgs FileCompressorsParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>FileSecurity</c>. </summary>
<returns> The <c>FileSecurity</c> Params. </returns> */

public FileSecurityArgs FileSecurityParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>TextProcessor</c>. </summary>
<returns> The <c>TextProcessor</c> Params. </returns> */

public TextProcessorArgs TextProcessorParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>SexyParsers</c> </summary>
<returns> The <c>SexyParsers</c> Params. </returns> */

public SexyParsersArgs SexyParsersParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>SexyCompressors</c>. </summary>
<returns> The <c>SexyCompressors</c> Params. </returns> */

public SexyCompressorsArgs SexyCompressorsParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>SexyCryptors</c>. </summary>
<returns> The <c>SexyCryptors</c> Params. </returns> */

public SexyCryptorsArgs SexyCryptorsParams{ get; set; }

/** <summary> Gets or Sets some Params for the <c>TextureDrawer</c>. </summary>
<returns> The <c>TextureDrawer</c> Params. </returns> */

public TextureDrawerArgs TextureDrawerParams{ get; set; }

// Add more args here...

/** <summary> Gets or Sets some Params for the <c>SexyUtils</c>. </summary>
<returns> The <c>SexyUtils</c> Params. </returns> */

public SexyUtilsArgs SexyUtilsParams{ get; set; }

/// <summary> Creates a new Instance of the <c>UserParams</c> Class. </summary>

public UserParams()
{
FileManagerParams = new();
FileParsersParams = new();

FileCompressorsParams = new();
FileSecurityParams = new();

TextProcessorParams = new();
SexyParsersParams = new();

SexyCompressorsParams  = new();
SexyCryptorsParams = new();

TextureDrawerParams = new();
SexyUtilsParams = new();
}

/** <summary> Checks each nullable Field of the <c>UserParams</c> instance given and Validates it, in case it's <c>null</c>. </summary>
<param name = "targetParams"> The params to be Analized. </param> */

protected override void CheckForNullFields()
{
UserParams defaultParams = new();

#region ======== Set default Values to Null Fields ========

FileManagerParams ??= defaultParams.FileManagerParams;
FileParsersParams ??= defaultParams.FileParsersParams;
FileCompressorsParams ??= defaultParams.FileCompressorsParams;
FileSecurityParams ??= defaultParams.FileSecurityParams;
TextProcessorParams ??= defaultParams.TextProcessorParams;
SexyParsersParams ??= defaultParams.SexyParsersParams;
SexyCompressorsParams ??= defaultParams.SexyCompressorsParams;
SexyCryptorsParams ??= defaultParams.SexyCryptorsParams;
TextureDrawerParams ??= defaultParams.TextureDrawerParams;
SexyUtilsParams ??= defaultParams.SexyUtilsParams;

#endregion

FileManagerParams.CheckForNullFields();
FileParsersParams.CheckForNullFields();
FileCompressorsParams.CheckForNullFields();
FileSecurityParams.CheckForNullFields();
TextProcessorParams.CheckForNullFields();
SexyParsersParams.CheckForNullFields();
SexyCompressorsParams.CheckForNullFields();
SexyCryptorsParams.CheckForNullFields();
TextureDrawerParams.CheckForNullFields();
SexyUtilsParams.CheckForNullFields();
}

 // Gets a Value using a Property Patb

public object GetFieldValue(string[] propertyPath)
{

if(propertyPath == null || propertyPath == Array.Empty<string>() )
throw new ArgumentNullException(nameof(propertyPath), "Must define a Property Path");

Type propertyType = GetType();
object fieldValue = this;

foreach(string propertyName in propertyPath)
{
PropertyInfo propertyInfo = propertyType.GetProperty(propertyName);

if(propertyInfo != null)
{
fieldValue = propertyInfo.GetValue(fieldValue);
propertyType = propertyInfo.PropertyType;
}

}

return fieldValue;
}

}

}
using System.IO;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation
{
/// <summary> Handles the ReferenceStrings Cached on Parsing Tasks. </summary>

public static class ReferenceStringsHandler
{
/// <summary> Stores the Reference Strings found on Parsed Files. </summary>

public static ReferenceStrings StringsContainer{ get; set; } = new();

/** <summary> Adds a String to the Native Strings List. </summary> 

<param name = "targetStr"> The String to be Added. </param> */

public static void AddStringToNativeList(string targetStr) => StringsContainer.AddNativeString(targetStr);

/** <summary> Adds a String to the Unicode Strings List. </summary> 

<param name = "targetStr"> The String to be Added. </param> */

public static void AddStringToUnicodeList(string targetStr) => StringsContainer.AddUnicodeString(targetStr);

/// <summary> Removes all the Strings stored in the Container. </summary>

public static void ClearStrings()
{
StringsContainer.ClearNativeStrings();
StringsContainer.ClearUnicodeStrings();
}

/** <summary> Gets the Index of a String found in the Native Strings List. </summary>

<param name = "targetStr"> The String to Locate in the List. </param>

<returns> The Index of the String. </returns> */

public static int GetNativeStringIndex(string targetStr) => StringsContainer.GetIndexOfNativeString(targetStr);

/** <summary> Gets the Index of a String found in the Unicode Strings List. </summary>

<param name = "targetStr"> The String to Locate in the List. </param>

<returns> The Index of the String. </returns> */

public static int GetUnicodeStringIndex(string targetStr) => StringsContainer.GetIndexOfUnicodeString(targetStr);

/** <summary> Gets a String from the Native Strings List by using a Index. </summary>

<param name = "stringIndex"> The Index of the String to Obtain. </param>

<returns> The Native String Obtained. </returns> */

public static string GetStringFromNativeList(int stringIndex) => StringsContainer.GetNativeString(stringIndex);

/** <summary> Gets a String from the Unicode Strings List by using a Index. </summary>

<param name = "stringIndex"> The Index of the String to Obtain. </param>

<returns> The Unicode String Obtained. </returns> */

public static string GetStringFromUnicodeList(int stringIndex) => StringsContainer.GetUnicodeString(stringIndex);

/** <summary> Checks if a String is Contained in the Native Strings List. </summary> 

<param name = "targetStr"> The String to be Checked. </param>

<returns> <b>true</b> if the String exists in the List; otherwise, <b>false</b> </returns> */

public static bool ListHasNativeString(string targetStr) => StringsContainer.HasNativeString(targetStr);

/** <summary> Checks if a String is Contained in the Unicode Strings List. </summary> 

<param name = "targetStr"> The String to be Checked. </param>

<returns> <b>true</b> if the String exists in the List; otherwise, <b>false</b> </returns> */

public static bool ListHasUnicodeString(string targetStr) => StringsContainer.HasUnicodeString(targetStr);

/** <summary> Gets the Path of a Reference Strings File. </summary>

<param name = "targetPath"> The Location of the File that Contains the Strings. </param>

<returns> The Path to the Reference Strings. </returns> */

public static string GetRefFilePath(string targetPath)
{
string refContainerPath = DirManager.GetContainerPath(targetPath, typeof(ReferenceStrings).Name);
int refStringsCount = StringsContainer.GetNativeStringsCount() + StringsContainer.GetUnicodeStringsCount();

string contentSummary = $"{typeof(string).Name} x{refStringsCount}";

return refContainerPath + Path.DirectorySeparatorChar + contentSummary + ".json";
}

/** <summary> Reads and Deserializes the Contents of the <c>StringsContainer</c>. </summary>

<param name = "inputPath"> The Path where the Reference Strings will be Read from. </param> */

public static ReferenceStrings ReadStrings(string sourcePath) => StringsContainer.ReadObject(sourcePath);

/** <summary> Writes all the Reference Strings of the Container from this Instance. </summary> 

<param name = "outputPath"> The Path where the Reference Strings will be Saved. </param> */

public static void WriteStrings(string targetPath) => StringsContainer.WriteObject(targetPath );
}

}
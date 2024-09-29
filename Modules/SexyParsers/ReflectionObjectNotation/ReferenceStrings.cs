using System;
using System.Collections.Generic;

namespace ZCore.Modules.SexyParsers.ReflectionObjectNotation
{
/// <summary> Serves as a Reference Control for Cached Strings (either Native or Unicode Strings). </summary>

public class ReferenceStrings : SerializableClass<ReferenceStrings>
{
/** <summary> Creates or Obtains a List which Contains the Native Strings found in a File. </summary>
<returns> The native Strings. </returns> */

public List<string> NativeStrings{ get; set; }

/** <summary> Creates or Obtains a List which Contains the Unicode Strings found in a File. </summary>
<returns> The Unicode Strings. </returns> */

public List<string> UnicodeStrings{ get; set; }

/// <summary> Creates a new Instance of the <c>ReferenceStrings</c> Class. </summary>

public ReferenceStrings()
{
NativeStrings = new();
UnicodeStrings = new();
}

/** <summary> Creates a new Instance of the <c>ReferenceStrings</c> Class with the Specified storage Limit. </summary>
<param name = "storageLimit"> The Limit of Elements which the Lists can Store. </param> */

public ReferenceStrings(int storageLimit)
{
NativeStrings = new(storageLimit);
UnicodeStrings = new(storageLimit);
}

/** <summary> Adds a Native String to the List. </summary> 
<param name = "targetStr"> The String to be Added. </param> */

public void AddNativeString(string targetStr) => NativeStrings.Add(targetStr);

/** <summary> Adds a Unicode String to the List. </summary> 
<param name = "targetStr"> The String to be Added. </param> */

public void AddUnicodeString(string targetStr) => UnicodeStrings.Add(targetStr);

/// <summary> Removes all the Native Strings from the List. </summary>

public void ClearNativeStrings() => NativeStrings.Clear();

/// <summary> Removes all the Unicode Strings from the List. </summary>

public void ClearUnicodeStrings() => UnicodeStrings.Clear();

/** <summary> Gets the Index of a Native String found in the List. </summary>

<param name = "targetStr"> The String to Locate in the List. </param>

<returns> The Index of the String. </returns> */

public int GetIndexOfNativeString(string targetStr) => NativeStrings.IndexOf(targetStr);

/** <summary> Gets the Index of a Unicode String found in the List. </summary>

<param name = "targetStr"> The String to Locate in the List. </param>

<returns> The Index of the String. </returns> */

public int GetIndexOfUnicodeString(string targetStr) => UnicodeStrings.IndexOf(targetStr);

/** <summary> Gets a Native String from the List by using a Index. </summary>

<param name = "stringIndex"> The Index of the String to Obtain. </param>

<returns> The Native String Obtained. </returns> */

public string GetNativeString(int stringIndex) => NativeStrings[stringIndex];

/** <summary> Gets the Number of Native Strings contained in the List. </summary>
<returns> The Number of Strings. </returns> */

public int GetNativeStringsCount() => NativeStrings.Count;

/** <summary> Gets a Unicode String from the List by using a Index. </summary>

<param name = "stringIndex"> The Index of the String to Obtain. </param>

<returns> The Unicode String Obtained. </returns> */

public string GetUnicodeString(int stringIndex) => UnicodeStrings[stringIndex];

/** <summary> Gets the Number of Unicode Strings contained in the List. </summary>
<returns> The Number of Strings. </returns> */

public int GetUnicodeStringsCount() => UnicodeStrings.Count;

/** <summary> Checks if a Native String is Contained in the List. </summary>

<param name = "targetStr"> The String to be Checked. </param>

<returns> <b>true</b> if the String exists in the List; otherwise, <b>false</b> </returns> */

public bool HasNativeString(string targetStr) => NativeStrings.Contains(targetStr);

/** <summary> Checks if a String is Contained in the List. </summary> 

<param name = "targetStr"> The String to be Checked. </param>

<returns> <b>true</b> if the String exists in the List; otherwise, <b>false</b> </returns> */

public bool HasUnicodeString(string targetStr) => UnicodeStrings.Contains(targetStr);
}

}
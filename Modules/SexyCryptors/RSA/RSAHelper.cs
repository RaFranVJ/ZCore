using RsaObj = System.Security.Cryptography.RSA;

using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace ZCore.Modules.SexyCryptors.RSA
{
/// <summary> Provides Additional Functions for the <b>RSA</b> Object. </summary>

public static class RSAHelper
{
/** <summary> Converts the given RSA Object as a XML String. </summary>

<param name = "rsa"> The RSA Object. </param>

<returns> The XML String. </returns> */

public static string ToXmlString(this RsaObj rsa, bool includePrivateParams)
{
RSAParameters parameters = rsa.ExportParameters(includePrivateParams);

XElement xml = new("RSAKeyValue",
new XElement("Modulus", Convert.ToBase64String(parameters.Modulus) ),
new XElement("Exponent", Convert.ToBase64String(parameters.Exponent) ) );

if(includePrivateParams)
{

xml.Add(
new XElement("P", Convert.ToBase64String(parameters.P) ),
new XElement("Q", Convert.ToBase64String(parameters.Q)),
new XElement("DP", Convert.ToBase64String(parameters.DP)),
new XElement("DQ", Convert.ToBase64String(parameters.DQ)),
new XElement("InverseQ", Convert.ToBase64String(parameters.InverseQ)),
new XElement("D", Convert.ToBase64String(parameters.D) ) );
}

return xml.ToString();
}

/** <summary> Initializes a RSA Object with the given XML String. </summary>

<param name = "rsa"> The RSA Object. </param> */

public static void FromXmlString(this RsaObj rsa, string xmlString)
{
XElement xml = XElement.Parse(xmlString);

RSAParameters parameters = new RSAParameters
{
Modulus = Convert.FromBase64String(xml.Element("Modulus").Value),
Exponent = Convert.FromBase64String(xml.Element("Exponent").Value)
};

if(xml.Element("P") != null)
{
parameters.P = Convert.FromBase64String(xml.Element("P").Value);
parameters.Q = Convert.FromBase64String(xml.Element("Q").Value);
parameters.DP = Convert.FromBase64String(xml.Element("DP").Value);
parameters.DQ = Convert.FromBase64String(xml.Element("DQ").Value);
parameters.InverseQ = Convert.FromBase64String(xml.Element("InverseQ").Value);
parameters.D = Convert.FromBase64String(xml.Element("D").Value);
}

rsa.ImportParameters(parameters);
}

}

}
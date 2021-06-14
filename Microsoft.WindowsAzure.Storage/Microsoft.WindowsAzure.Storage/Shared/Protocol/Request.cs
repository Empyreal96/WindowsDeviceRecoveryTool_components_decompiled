using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Shared.Protocol
{
	// Token: 0x02000164 RID: 356
	internal static class Request
	{
		// Token: 0x0600152C RID: 5420 RVA: 0x000503AC File Offset: 0x0004E5AC
		internal static string ConvertDateTimeToSnapshotString(DateTimeOffset dateTime)
		{
			return dateTime.UtcDateTime.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'", CultureInfo.InvariantCulture);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x000503D4 File Offset: 0x0004E5D4
		internal static void WriteSharedAccessIdentifiers<T>(IDictionary<string, T> sharedAccessPolicies, Stream outputStream, Action<T, XmlWriter> writePolicyXml)
		{
			CommonUtility.AssertNotNull("sharedAccessPolicies", sharedAccessPolicies);
			CommonUtility.AssertNotNull("outputStream", outputStream);
			if (sharedAccessPolicies.Count > 5)
			{
				string message = string.Format(CultureInfo.CurrentCulture, "Too many '{0}' shared access policy identifiers provided. Server does not support setting more than '{1}' on a single container, queue, table, or share.", new object[]
				{
					sharedAccessPolicies.Count,
					5
				});
				throw new ArgumentOutOfRangeException("sharedAccessPolicies", message);
			}
			using (XmlWriter xmlWriter = XmlWriter.Create(outputStream, new XmlWriterSettings
			{
				Encoding = Encoding.UTF8
			}))
			{
				xmlWriter.WriteStartElement("SignedIdentifiers");
				foreach (string text in sharedAccessPolicies.Keys)
				{
					xmlWriter.WriteStartElement("SignedIdentifier");
					xmlWriter.WriteElementString("Id", text);
					xmlWriter.WriteStartElement("AccessPolicy");
					T arg = sharedAccessPolicies[text];
					writePolicyXml(arg, xmlWriter);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndDocument();
			}
		}
	}
}

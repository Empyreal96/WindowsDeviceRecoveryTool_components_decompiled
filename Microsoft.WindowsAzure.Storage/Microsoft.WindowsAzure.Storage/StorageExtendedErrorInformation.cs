using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using Microsoft.Data.OData;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007F RID: 127
	[Serializable]
	public sealed class StorageExtendedErrorInformation
	{
		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x000394D7 File Offset: 0x000376D7
		// (set) Token: 0x06000EFB RID: 3835 RVA: 0x000394DF File Offset: 0x000376DF
		public string ErrorCode { get; internal set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000EFC RID: 3836 RVA: 0x000394E8 File Offset: 0x000376E8
		// (set) Token: 0x06000EFD RID: 3837 RVA: 0x000394F0 File Offset: 0x000376F0
		public string ErrorMessage { get; internal set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x000394F9 File Offset: 0x000376F9
		// (set) Token: 0x06000EFF RID: 3839 RVA: 0x00039501 File Offset: 0x00037701
		public IDictionary<string, string> AdditionalDetails { get; internal set; }

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003950C File Offset: 0x0003770C
		public static StorageExtendedErrorInformation ReadFromStream(Stream inputStream)
		{
			CommonUtility.AssertNotNull("inputStream", inputStream);
			if (inputStream.CanSeek && inputStream.Length < 1L)
			{
				return null;
			}
			StorageExtendedErrorInformation storageExtendedErrorInformation = new StorageExtendedErrorInformation();
			StorageExtendedErrorInformation result;
			try
			{
				using (XmlReader xmlReader = XmlReader.Create(inputStream))
				{
					xmlReader.Read();
					storageExtendedErrorInformation.ReadXml(xmlReader);
				}
				result = storageExtendedErrorInformation;
			}
			catch (XmlException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00039588 File Offset: 0x00037788
		public static StorageExtendedErrorInformation ReadFromStreamUsingODataLib(Stream inputStream, HttpWebResponse response, string contentType)
		{
			CommonUtility.AssertNotNull("inputStream", inputStream);
			CommonUtility.AssertNotNull("response", response);
			if (inputStream.CanSeek && inputStream.Length <= 0L)
			{
				return null;
			}
			HttpResponseAdapterMessage responseMessage = new HttpResponseAdapterMessage(response, inputStream, contentType);
			return StorageExtendedErrorInformation.ReadAndParseExtendedError(responseMessage);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000395D0 File Offset: 0x000377D0
		public static StorageExtendedErrorInformation ReadDataServiceResponseFromStream(Stream inputStream, IDictionary<string, string> responseHeaders, string contentType)
		{
			CommonUtility.AssertNotNull("inputStream", inputStream);
			if (inputStream.CanSeek && inputStream.Length <= 0L)
			{
				return null;
			}
			DataServicesResponseAdapterMessage responseMessage = new DataServicesResponseAdapterMessage(responseHeaders, inputStream, contentType);
			return StorageExtendedErrorInformation.ReadAndParseExtendedError(responseMessage);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003960C File Offset: 0x0003780C
		public static StorageExtendedErrorInformation ReadAndParseExtendedError(IODataResponseMessage responseMessage)
		{
			StorageExtendedErrorInformation storageExtendedErrorInformation = null;
			using (ODataMessageReader odataMessageReader = new ODataMessageReader(responseMessage))
			{
				try
				{
					ODataError odataError = odataMessageReader.ReadError();
					if (odataError != null)
					{
						storageExtendedErrorInformation = new StorageExtendedErrorInformation();
						storageExtendedErrorInformation.AdditionalDetails = new Dictionary<string, string>();
						storageExtendedErrorInformation.ErrorCode = odataError.ErrorCode;
						storageExtendedErrorInformation.ErrorMessage = odataError.Message;
						if (odataError.InnerError != null)
						{
							storageExtendedErrorInformation.AdditionalDetails["ExceptionMessage"] = odataError.InnerError.Message;
							storageExtendedErrorInformation.AdditionalDetails["StackTrace"] = odataError.InnerError.StackTrace;
						}
						if (odataError.InstanceAnnotations.Count > 0)
						{
							foreach (ODataInstanceAnnotation odataInstanceAnnotation in odataError.InstanceAnnotations)
							{
								storageExtendedErrorInformation.AdditionalDetails[odataInstanceAnnotation.Name] = odataInstanceAnnotation.Value.GetAnnotation<string>();
							}
						}
					}
				}
				catch (Exception)
				{
					return null;
				}
			}
			return storageExtendedErrorInformation;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00039730 File Offset: 0x00037930
		public void ReadXml(XmlReader reader)
		{
			CommonUtility.AssertNotNull("reader", reader);
			this.AdditionalDetails = new Dictionary<string, string>();
			reader.ReadStartElement();
			while (reader.IsStartElement())
			{
				if (reader.IsEmptyElement)
				{
					reader.Skip();
				}
				else if (string.Compare(reader.LocalName, "Code", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(reader.LocalName, "code", StringComparison.Ordinal) == 0)
				{
					this.ErrorCode = reader.ReadElementContentAsString();
				}
				else if (string.Compare(reader.LocalName, "Message", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(reader.LocalName, "message", StringComparison.Ordinal) == 0)
				{
					this.ErrorMessage = reader.ReadElementContentAsString();
				}
				else if (string.Compare(reader.LocalName, "exceptiondetails", StringComparison.OrdinalIgnoreCase) == 0)
				{
					reader.ReadStartElement();
					while (reader.IsStartElement())
					{
						string localName;
						if ((localName = reader.LocalName) != null)
						{
							if (localName == "ExceptionMessage")
							{
								this.AdditionalDetails.Add("ExceptionMessage", reader.ReadElementContentAsString("ExceptionMessage", string.Empty));
								continue;
							}
							if (localName == "StackTrace")
							{
								this.AdditionalDetails.Add("StackTrace", reader.ReadElementContentAsString("StackTrace", string.Empty));
								continue;
							}
						}
						reader.Skip();
					}
					reader.ReadEndElement();
				}
				else
				{
					this.AdditionalDetails.Add(reader.LocalName, reader.ReadInnerXml());
				}
			}
			reader.ReadEndElement();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000398A8 File Offset: 0x00037AA8
		public void WriteXml(XmlWriter writer)
		{
			CommonUtility.AssertNotNull("writer", writer);
			writer.WriteStartElement("Error");
			writer.WriteElementString("Code", this.ErrorCode);
			writer.WriteElementString("Message", this.ErrorMessage);
			foreach (string text in this.AdditionalDetails.Keys)
			{
				writer.WriteElementString(text, this.AdditionalDetails[text]);
			}
			writer.WriteEndElement();
		}
	}
}

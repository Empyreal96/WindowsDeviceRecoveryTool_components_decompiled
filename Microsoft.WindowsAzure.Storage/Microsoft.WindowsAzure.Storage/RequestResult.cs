using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage
{
	// Token: 0x0200007C RID: 124
	[Serializable]
	public sealed class RequestResult
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00038BF5 File Offset: 0x00036DF5
		// (set) Token: 0x06000EC5 RID: 3781 RVA: 0x00038BFD File Offset: 0x00036DFD
		public int HttpStatusCode { get; set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x00038C06 File Offset: 0x00036E06
		// (set) Token: 0x06000EC7 RID: 3783 RVA: 0x00038C0E File Offset: 0x00036E0E
		public string HttpStatusMessage { get; internal set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00038C17 File Offset: 0x00036E17
		// (set) Token: 0x06000EC9 RID: 3785 RVA: 0x00038C1F File Offset: 0x00036E1F
		public string ServiceRequestID { get; internal set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000ECA RID: 3786 RVA: 0x00038C28 File Offset: 0x00036E28
		// (set) Token: 0x06000ECB RID: 3787 RVA: 0x00038C30 File Offset: 0x00036E30
		public string ContentMd5 { get; internal set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00038C39 File Offset: 0x00036E39
		// (set) Token: 0x06000ECD RID: 3789 RVA: 0x00038C41 File Offset: 0x00036E41
		public string Etag { get; internal set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000ECE RID: 3790 RVA: 0x00038C4A File Offset: 0x00036E4A
		// (set) Token: 0x06000ECF RID: 3791 RVA: 0x00038C52 File Offset: 0x00036E52
		public long IngressBytes { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00038C5B File Offset: 0x00036E5B
		// (set) Token: 0x06000ED1 RID: 3793 RVA: 0x00038C63 File Offset: 0x00036E63
		public long EgressBytes { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x00038C6C File Offset: 0x00036E6C
		// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x00038C74 File Offset: 0x00036E74
		public string RequestDate { get; internal set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00038C7D File Offset: 0x00036E7D
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x00038C85 File Offset: 0x00036E85
		public StorageLocation TargetLocation { get; internal set; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00038C8E File Offset: 0x00036E8E
		// (set) Token: 0x06000ED7 RID: 3799 RVA: 0x00038C96 File Offset: 0x00036E96
		public StorageExtendedErrorInformation ExtendedErrorInformation { get; internal set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x00038C9F File Offset: 0x00036E9F
		// (set) Token: 0x06000ED9 RID: 3801 RVA: 0x00038CA9 File Offset: 0x00036EA9
		public Exception Exception
		{
			get
			{
				return this.exception;
			}
			set
			{
				this.exception = value;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00038CB4 File Offset: 0x00036EB4
		// (set) Token: 0x06000EDB RID: 3803 RVA: 0x00038CBC File Offset: 0x00036EBC
		public DateTime StartTime { get; internal set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00038CC5 File Offset: 0x00036EC5
		// (set) Token: 0x06000EDD RID: 3805 RVA: 0x00038CCD File Offset: 0x00036ECD
		public DateTime EndTime { get; internal set; }

		// Token: 0x06000EDE RID: 3806 RVA: 0x00038CD8 File Offset: 0x00036ED8
		[Obsolete("This should be available only in Microsoft.WindowsAzure.Storage.WinMD and not in Microsoft.WindowsAzure.Storage.dll. Please use ReadXML to deserialize RequestResult when Microsoft.WindowsAzure.Storage.dll is used.")]
		public static RequestResult TranslateFromExceptionMessage(string message)
		{
			RequestResult requestResult = new RequestResult();
			using (XmlReader xmlReader = XmlReader.Create(new StringReader(message)))
			{
				requestResult.ReadXml(xmlReader);
			}
			return requestResult;
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00038D1C File Offset: 0x00036F1C
		internal string WriteAsXml()
		{
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			StringBuilder stringBuilder = new StringBuilder();
			string result;
			try
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
				{
					this.WriteXml(xmlWriter);
				}
				result = stringBuilder.ToString();
			}
			catch (XmlException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00038D84 File Offset: 0x00036F84
		public void ReadXml(XmlReader reader)
		{
			CommonUtility.AssertNotNull("reader", reader);
			reader.Read();
			if (reader.NodeType == XmlNodeType.Comment)
			{
				reader.Read();
			}
			reader.ReadStartElement("RequestResult");
			this.HttpStatusCode = int.Parse(CommonUtility.ReadElementAsString("HTTPStatusCode", reader), CultureInfo.InvariantCulture);
			this.HttpStatusMessage = CommonUtility.ReadElementAsString("HttpStatusMessage", reader);
			StorageLocation targetLocation;
			if (Enum.TryParse<StorageLocation>(CommonUtility.ReadElementAsString("TargetLocation", reader), out targetLocation))
			{
				this.TargetLocation = targetLocation;
			}
			this.ServiceRequestID = CommonUtility.ReadElementAsString("ServiceRequestID", reader);
			this.ContentMd5 = CommonUtility.ReadElementAsString("ContentMd5", reader);
			this.Etag = CommonUtility.ReadElementAsString("Etag", reader);
			this.RequestDate = CommonUtility.ReadElementAsString("RequestDate", reader);
			this.StartTime = DateTime.Parse(CommonUtility.ReadElementAsString("StartTime", reader), CultureInfo.InvariantCulture);
			this.EndTime = DateTime.Parse(CommonUtility.ReadElementAsString("EndTime", reader), CultureInfo.InvariantCulture);
			this.ExtendedErrorInformation = new StorageExtendedErrorInformation();
			this.ExtendedErrorInformation.ReadXml(reader);
			reader.ReadEndElement();
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00038E9C File Offset: 0x0003709C
		public void WriteXml(XmlWriter writer)
		{
			CommonUtility.AssertNotNull("writer", writer);
			writer.WriteComment("An exception has occurred. For more information please deserialize this message via RequestResult.TranslateFromExceptionMessage.");
			writer.WriteStartElement("RequestResult");
			writer.WriteElementString("HTTPStatusCode", Convert.ToString(this.HttpStatusCode, CultureInfo.InvariantCulture));
			writer.WriteElementString("HttpStatusMessage", this.HttpStatusMessage);
			writer.WriteElementString("TargetLocation", this.TargetLocation.ToString());
			writer.WriteElementString("ServiceRequestID", this.ServiceRequestID);
			writer.WriteElementString("ContentMd5", this.ContentMd5);
			writer.WriteElementString("Etag", this.Etag);
			writer.WriteElementString("RequestDate", this.RequestDate);
			writer.WriteElementString("StartTime", this.StartTime.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture));
			writer.WriteElementString("EndTime", this.EndTime.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture));
			if (this.ExtendedErrorInformation != null)
			{
				this.ExtendedErrorInformation.WriteXml(writer);
			}
			else
			{
				writer.WriteStartElement("Error");
				writer.WriteFullEndElement();
			}
			writer.WriteEndElement();
		}

		// Token: 0x0400025E RID: 606
		private volatile Exception exception;
	}
}

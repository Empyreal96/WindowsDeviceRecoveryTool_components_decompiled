using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000AA RID: 170
	[XmlRoot("ContinuationToken", IsNullable = false)]
	[Serializable]
	public sealed class BlobContinuationToken : IContinuationToken, IXmlSerializable
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0003E4F3 File Offset: 0x0003C6F3
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x0003E4FC File Offset: 0x0003C6FC
		private string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
				if (this.version != "2.0")
				{
					throw new XmlException(string.Format(CultureInfo.InvariantCulture, "Unexpected Element '{0}'", new object[]
					{
						this.version
					}));
				}
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x0003E548 File Offset: 0x0003C748
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x0003E550 File Offset: 0x0003C750
		private string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				if (this.type != "Blob")
				{
					throw new XmlException("Unexpected Continuation Type");
				}
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x0003E576 File Offset: 0x0003C776
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x0003E57E File Offset: 0x0003C77E
		public string NextMarker { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x0003E587 File Offset: 0x0003C787
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x0003E58F File Offset: 0x0003C78F
		public StorageLocation? TargetLocation { get; set; }

		// Token: 0x0600107E RID: 4222 RVA: 0x0003E598 File Offset: 0x0003C798
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x0003E59C File Offset: 0x0003C79C
		public void ReadXml(XmlReader reader)
		{
			CommonUtility.AssertNotNull("reader", reader);
			reader.MoveToContent();
			reader.ReadStartElement();
			reader.MoveToContent();
			if (reader.Name == "ContinuationToken")
			{
				reader.ReadStartElement();
			}
			while (reader.IsStartElement())
			{
				string name;
				if ((name = reader.Name) != null)
				{
					if (name == "Version")
					{
						this.Version = reader.ReadElementContentAsString();
						continue;
					}
					if (name == "NextMarker")
					{
						this.NextMarker = reader.ReadElementContentAsString();
						continue;
					}
					if (!(name == "TargetLocation"))
					{
						if (name == "Type")
						{
							this.Type = reader.ReadElementContentAsString();
							continue;
						}
					}
					else
					{
						string text = reader.ReadElementContentAsString();
						StorageLocation value;
						if (Enum.TryParse<StorageLocation>(text, out value))
						{
							this.TargetLocation = new StorageLocation?(value);
							continue;
						}
						throw new XmlException(string.Format(CultureInfo.InvariantCulture, "Unexpected Location '{0}'", new object[]
						{
							text
						}));
					}
				}
				throw new XmlException(string.Format(CultureInfo.InvariantCulture, "Unexpected Element '{0}'", new object[]
				{
					reader.Name
				}));
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0003E6CC File Offset: 0x0003C8CC
		public void WriteXml(XmlWriter writer)
		{
			CommonUtility.AssertNotNull("writer", writer);
			writer.WriteStartElement("ContinuationToken");
			writer.WriteElementString("Version", this.Version);
			writer.WriteElementString("Type", this.Type);
			if (this.NextMarker != null)
			{
				writer.WriteElementString("NextMarker", this.NextMarker);
			}
			writer.WriteElementString("TargetLocation", this.TargetLocation.ToString());
			writer.WriteEndElement();
		}

		// Token: 0x040003EC RID: 1004
		private string version = "2.0";

		// Token: 0x040003ED RID: 1005
		private string type = "Blob";
	}
}

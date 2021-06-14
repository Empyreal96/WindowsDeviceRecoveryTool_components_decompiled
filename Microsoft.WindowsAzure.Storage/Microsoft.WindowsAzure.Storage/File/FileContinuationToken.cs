using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000D6 RID: 214
	[XmlRoot("ContinuationToken", IsNullable = false)]
	[Serializable]
	public sealed class FileContinuationToken : IContinuationToken, IXmlSerializable
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x000419F0 File Offset: 0x0003FBF0
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x000419F8 File Offset: 0x0003FBF8
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

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x00041A44 File Offset: 0x0003FC44
		// (set) Token: 0x06001175 RID: 4469 RVA: 0x00041A4C File Offset: 0x0003FC4C
		private string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				if (this.type != "File")
				{
					throw new XmlException("Unexpected Continuation Type");
				}
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x00041A72 File Offset: 0x0003FC72
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x00041A7A File Offset: 0x0003FC7A
		public string NextMarker { get; set; }

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x00041A83 File Offset: 0x0003FC83
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x00041A8B File Offset: 0x0003FC8B
		public StorageLocation? TargetLocation { get; set; }

		// Token: 0x0600117A RID: 4474 RVA: 0x00041A94 File Offset: 0x0003FC94
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00041A98 File Offset: 0x0003FC98
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

		// Token: 0x0600117C RID: 4476 RVA: 0x00041BC8 File Offset: 0x0003FDC8
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

		// Token: 0x040004BE RID: 1214
		private string version = "2.0";

		// Token: 0x040004BF RID: 1215
		private string type = "File";
	}
}

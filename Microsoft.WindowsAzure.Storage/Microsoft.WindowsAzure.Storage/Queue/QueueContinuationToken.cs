using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000F4 RID: 244
	[XmlRoot("ContinuationToken", IsNullable = false)]
	[Serializable]
	public sealed class QueueContinuationToken : IContinuationToken, IXmlSerializable
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600122F RID: 4655 RVA: 0x00043B2E File Offset: 0x00041D2E
		// (set) Token: 0x06001230 RID: 4656 RVA: 0x00043B38 File Offset: 0x00041D38
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

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001231 RID: 4657 RVA: 0x00043B84 File Offset: 0x00041D84
		// (set) Token: 0x06001232 RID: 4658 RVA: 0x00043B8C File Offset: 0x00041D8C
		private string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				if (this.type != "Queue")
				{
					throw new XmlException("Unexpected Continuation Type");
				}
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00043BB2 File Offset: 0x00041DB2
		// (set) Token: 0x06001234 RID: 4660 RVA: 0x00043BBA File Offset: 0x00041DBA
		public string NextMarker { get; set; }

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001235 RID: 4661 RVA: 0x00043BC3 File Offset: 0x00041DC3
		// (set) Token: 0x06001236 RID: 4662 RVA: 0x00043BCB File Offset: 0x00041DCB
		public StorageLocation? TargetLocation { get; set; }

		// Token: 0x06001237 RID: 4663 RVA: 0x00043BD4 File Offset: 0x00041DD4
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00043BD8 File Offset: 0x00041DD8
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

		// Token: 0x06001239 RID: 4665 RVA: 0x00043D08 File Offset: 0x00041F08
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

		// Token: 0x04000526 RID: 1318
		private string version = "2.0";

		// Token: 0x04000527 RID: 1319
		private string type = "Queue";
	}
}

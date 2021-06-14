using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000145 RID: 325
	[XmlRoot("ContinuationToken", IsNullable = false)]
	[Serializable]
	public sealed class TableContinuationToken : IContinuationToken, IXmlSerializable
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0004F0FF File Offset: 0x0004D2FF
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x0004F108 File Offset: 0x0004D308
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

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060014AD RID: 5293 RVA: 0x0004F154 File Offset: 0x0004D354
		// (set) Token: 0x060014AE RID: 5294 RVA: 0x0004F15C File Offset: 0x0004D35C
		private string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				if (this.type != "Table")
				{
					throw new XmlException("Unexpected Continuation Type");
				}
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x0004F182 File Offset: 0x0004D382
		// (set) Token: 0x060014B0 RID: 5296 RVA: 0x0004F18A File Offset: 0x0004D38A
		public string NextPartitionKey { get; set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0004F193 File Offset: 0x0004D393
		// (set) Token: 0x060014B2 RID: 5298 RVA: 0x0004F19B File Offset: 0x0004D39B
		public string NextRowKey { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060014B3 RID: 5299 RVA: 0x0004F1A4 File Offset: 0x0004D3A4
		// (set) Token: 0x060014B4 RID: 5300 RVA: 0x0004F1AC File Offset: 0x0004D3AC
		public string NextTableName { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0004F1B5 File Offset: 0x0004D3B5
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0004F1BD File Offset: 0x0004D3BD
		public StorageLocation? TargetLocation { get; set; }

		// Token: 0x060014B7 RID: 5303 RVA: 0x0004F1C6 File Offset: 0x0004D3C6
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x0004F1CC File Offset: 0x0004D3CC
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
				switch (name = reader.Name)
				{
				case "Version":
					this.Version = reader.ReadElementContentAsString();
					continue;
				case "Type":
					this.Type = reader.ReadElementContentAsString();
					continue;
				case "TargetLocation":
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
				case "NextPartitionKey":
					this.NextPartitionKey = reader.ReadElementContentAsString();
					continue;
				case "NextRowKey":
					this.NextRowKey = reader.ReadElementContentAsString();
					continue;
				case "NextTableName":
					this.NextTableName = reader.ReadElementContentAsString();
					continue;
				}
				throw new XmlException(string.Format(CultureInfo.InvariantCulture, "Unexpected Element '{0}'", new object[]
				{
					reader.Name
				}));
			}
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0004F380 File Offset: 0x0004D580
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteStartElement("ContinuationToken");
			writer.WriteElementString("Version", this.Version);
			writer.WriteElementString("Type", this.Type);
			if (this.NextPartitionKey != null)
			{
				writer.WriteElementString("NextPartitionKey", this.NextPartitionKey);
			}
			if (this.NextRowKey != null)
			{
				writer.WriteElementString("NextRowKey", this.NextRowKey);
			}
			if (this.NextTableName != null)
			{
				writer.WriteElementString("NextTableName", this.NextTableName);
			}
			writer.WriteElementString("TargetLocation", this.TargetLocation.ToString());
			writer.WriteEndElement();
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0004F438 File Offset: 0x0004D638
		internal void ApplyToUriQueryBuilder(UriQueryBuilder builder)
		{
			if (!string.IsNullOrEmpty(this.NextPartitionKey))
			{
				builder.Add("NextPartitionKey", this.NextPartitionKey);
			}
			if (!string.IsNullOrEmpty(this.NextRowKey))
			{
				builder.Add("NextRowKey", this.NextRowKey);
			}
			if (!string.IsNullOrEmpty(this.NextTableName))
			{
				builder.Add("NextTableName", this.NextTableName);
			}
		}

		// Token: 0x04000804 RID: 2052
		private string version = "2.0";

		// Token: 0x04000805 RID: 2053
		private string type = "Table";
	}
}

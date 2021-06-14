using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000CC RID: 204
	public class GetBlockListResponse : ResponseParsingBase<ListBlockItem>
	{
		// Token: 0x0600113A RID: 4410 RVA: 0x00040104 File Offset: 0x0003E304
		public GetBlockListResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x0004010D File Offset: 0x0003E30D
		public IEnumerable<ListBlockItem> Blocks
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00040118 File Offset: 0x0003E318
		private ListBlockItem ParseBlockItem(bool committed)
		{
			ListBlockItem listBlockItem = new ListBlockItem
			{
				Committed = committed
			};
			this.reader.ReadStartElement();
			while (this.reader.IsStartElement())
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					string name;
					if ((name = this.reader.Name) != null)
					{
						if (name == "Size")
						{
							listBlockItem.Length = this.reader.ReadElementContentAsLong();
							continue;
						}
						if (name == "Name")
						{
							listBlockItem.Name = this.reader.ReadElementContentAsString();
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			return listBlockItem;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00040440 File Offset: 0x0003E640
		protected override IEnumerable<ListBlockItem> ParseXml()
		{
			if (this.reader.ReadToFollowing("BlockList"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					this.reader.ReadStartElement();
					while (this.reader.IsStartElement())
					{
						if (this.reader.IsEmptyElement)
						{
							this.reader.Skip();
						}
						else
						{
							string name;
							if ((name = this.reader.Name) != null)
							{
								if (name == "CommittedBlocks")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement("Block"))
									{
										yield return this.ParseBlockItem(true);
									}
									this.reader.ReadEndElement();
									continue;
								}
								if (name == "UncommittedBlocks")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement("Block"))
									{
										yield return this.ParseBlockItem(false);
									}
									this.reader.ReadEndElement();
									continue;
								}
							}
							this.reader.Skip();
						}
					}
					this.allObjectsParsed = true;
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000CD RID: 205
	public sealed class GetPageRangesResponse : ResponseParsingBase<PageRange>
	{
		// Token: 0x0600113E RID: 4414 RVA: 0x0004045D File Offset: 0x0003E65D
		public GetPageRangesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x00040466 File Offset: 0x0003E666
		public IEnumerable<PageRange> PageRanges
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00040470 File Offset: 0x0003E670
		private PageRange ParsePageRange()
		{
			long start = 0L;
			long end = 0L;
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
						if (name == "Start")
						{
							start = this.reader.ReadElementContentAsLong();
							continue;
						}
						if (name == "End")
						{
							end = this.reader.ReadElementContentAsLong();
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			return new PageRange(start, end);
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00040678 File Offset: 0x0003E878
		protected override IEnumerable<PageRange> ParseXml()
		{
			if (this.reader.ReadToFollowing("PageList"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					this.reader.ReadStartElement();
					while (this.reader.IsStartElement("PageRange"))
					{
						yield return this.ParsePageRange();
					}
					this.allObjectsParsed = true;
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000F0 RID: 240
	public sealed class ListRangesResponse : ResponseParsingBase<FileRange>
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x00043285 File Offset: 0x00041485
		public ListRangesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x0004328E File Offset: 0x0004148E
		public IEnumerable<FileRange> Ranges
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00043298 File Offset: 0x00041498
		private FileRange ParseRange()
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
			return new FileRange(start, end);
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x000434A0 File Offset: 0x000416A0
		protected override IEnumerable<FileRange> ParseXml()
		{
			if (this.reader.ReadToFollowing("Ranges"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					this.reader.ReadStartElement();
					while (this.reader.IsStartElement("Range"))
					{
						yield return this.ParseRange();
					}
					this.allObjectsParsed = true;
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}
	}
}

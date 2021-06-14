using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000F1 RID: 241
	public sealed class ListSharesResponse : ResponseParsingBase<FileShareEntry>
	{
		// Token: 0x06001222 RID: 4642 RVA: 0x000434BD File Offset: 0x000416BD
		public ListSharesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000434C8 File Offset: 0x000416C8
		public ListingContext ListingContext
		{
			get
			{
				return new ListingContext(this.Prefix, new int?(this.MaxResults))
				{
					Marker = this.NextMarker
				};
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x000434F9 File Offset: 0x000416F9
		public IEnumerable<FileShareEntry> Shares
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00043501 File Offset: 0x00041701
		public string Prefix
		{
			get
			{
				base.Variable(ref this.prefixConsumable);
				return this.prefix;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001226 RID: 4646 RVA: 0x00043515 File Offset: 0x00041715
		public string Marker
		{
			get
			{
				base.Variable(ref this.markerConsumable);
				return this.marker;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001227 RID: 4647 RVA: 0x00043529 File Offset: 0x00041729
		public int MaxResults
		{
			get
			{
				base.Variable(ref this.maxResultsConsumable);
				return this.maxResults;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001228 RID: 4648 RVA: 0x0004353D File Offset: 0x0004173D
		public string NextMarker
		{
			get
			{
				base.Variable(ref this.nextMarkerConsumable);
				return this.nextMarker;
			}
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00043554 File Offset: 0x00041754
		private FileShareEntry ParseShareEntry(Uri baseUri)
		{
			string text = null;
			IDictionary<string, string> dictionary = null;
			FileShareProperties fileShareProperties = new FileShareProperties();
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
						if (name == "Name")
						{
							text = this.reader.ReadElementContentAsString();
							continue;
						}
						if (name == "Properties")
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
									string name2;
									if ((name2 = this.reader.Name) != null)
									{
										if (name2 == "Last-Modified")
										{
											fileShareProperties.LastModified = new DateTimeOffset?(this.reader.ReadElementContentAsString().ToUTCTime());
											continue;
										}
										if (name2 == "Etag")
										{
											fileShareProperties.ETag = this.reader.ReadElementContentAsString();
											continue;
										}
										if (name2 == "Quota")
										{
											fileShareProperties.Quota = new int?(this.reader.ReadElementContentAsInt());
											continue;
										}
									}
									this.reader.Skip();
								}
							}
							this.reader.ReadEndElement();
							continue;
						}
						if (name == "Metadata")
						{
							dictionary = Response.ParseMetadata(this.reader);
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, string>();
			}
			return new FileShareEntry
			{
				Properties = fileShareProperties,
				Name = text,
				Uri = NavigationHelper.AppendPathToSingleUri(baseUri, text),
				Metadata = dictionary
			};
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00043AAC File Offset: 0x00041CAC
		protected override IEnumerable<FileShareEntry> ParseXml()
		{
			if (this.reader.ReadToFollowing("EnumerationResults"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					Uri baseUri = new Uri(this.reader.GetAttribute("ServiceEndpoint"));
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
								if (name == "Marker")
								{
									this.marker = this.reader.ReadElementContentAsString();
									this.markerConsumable = true;
									yield return null;
									continue;
								}
								if (name == "NextMarker")
								{
									this.nextMarker = this.reader.ReadElementContentAsString();
									this.nextMarkerConsumable = true;
									yield return null;
									continue;
								}
								if (name == "MaxResults")
								{
									this.maxResults = this.reader.ReadElementContentAsInt();
									this.maxResultsConsumable = true;
									yield return null;
									continue;
								}
								if (name == "Prefix")
								{
									this.prefix = this.reader.ReadElementContentAsString();
									this.prefixConsumable = true;
									yield return null;
									continue;
								}
								if (name == "Shares")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement("Share"))
									{
										yield return this.ParseShareEntry(baseUri);
									}
									this.reader.ReadEndElement();
									this.allObjectsParsed = true;
									continue;
								}
							}
							this.reader.Skip();
						}
					}
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}

		// Token: 0x04000518 RID: 1304
		private string prefix;

		// Token: 0x04000519 RID: 1305
		private bool prefixConsumable;

		// Token: 0x0400051A RID: 1306
		private string marker;

		// Token: 0x0400051B RID: 1307
		private bool markerConsumable;

		// Token: 0x0400051C RID: 1308
		private int maxResults;

		// Token: 0x0400051D RID: 1309
		private bool maxResultsConsumable;

		// Token: 0x0400051E RID: 1310
		private string nextMarker;

		// Token: 0x0400051F RID: 1311
		private bool nextMarkerConsumable;
	}
}

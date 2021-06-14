using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Queue.Protocol
{
	// Token: 0x020000FC RID: 252
	public sealed class ListQueuesResponse : ResponseParsingBase<QueueEntry>
	{
		// Token: 0x06001272 RID: 4722 RVA: 0x00044871 File Offset: 0x00042A71
		public ListQueuesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001273 RID: 4723 RVA: 0x0004487C File Offset: 0x00042A7C
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

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001274 RID: 4724 RVA: 0x000448AD File Offset: 0x00042AAD
		public IEnumerable<QueueEntry> Queues
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001275 RID: 4725 RVA: 0x000448B5 File Offset: 0x00042AB5
		public string Prefix
		{
			get
			{
				base.Variable(ref this.prefixConsumable);
				return this.prefix;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001276 RID: 4726 RVA: 0x000448C9 File Offset: 0x00042AC9
		public string Marker
		{
			get
			{
				base.Variable(ref this.markerConsumable);
				return this.marker;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001277 RID: 4727 RVA: 0x000448DD File Offset: 0x00042ADD
		public int MaxResults
		{
			get
			{
				base.Variable(ref this.maxResultsConsumable);
				return this.maxResults;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x000448F1 File Offset: 0x00042AF1
		public string NextMarker
		{
			get
			{
				base.Variable(ref this.nextMarkerConsumable);
				return this.nextMarker;
			}
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00044908 File Offset: 0x00042B08
		private QueueEntry ParseQueueEntry(Uri baseUri)
		{
			string text = null;
			IDictionary<string, string> dictionary = null;
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
			return new QueueEntry(text, NavigationHelper.AppendPathToSingleUri(baseUri, text), dictionary);
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x00044D38 File Offset: 0x00042F38
		protected override IEnumerable<QueueEntry> ParseXml()
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
								if (name == "Queues")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement())
									{
										yield return this.ParseQueueEntry(baseUri);
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

		// Token: 0x04000541 RID: 1345
		private string prefix;

		// Token: 0x04000542 RID: 1346
		private bool prefixConsumable;

		// Token: 0x04000543 RID: 1347
		private string marker;

		// Token: 0x04000544 RID: 1348
		private bool markerConsumable;

		// Token: 0x04000545 RID: 1349
		private int maxResults;

		// Token: 0x04000546 RID: 1350
		private bool maxResultsConsumable;

		// Token: 0x04000547 RID: 1351
		private string nextMarker;

		// Token: 0x04000548 RID: 1352
		private bool nextMarkerConsumable;
	}
}

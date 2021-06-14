using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000D2 RID: 210
	public sealed class ListContainersResponse : ResponseParsingBase<BlobContainerEntry>
	{
		// Token: 0x0600115A RID: 4442 RVA: 0x000412ED File Offset: 0x0003F4ED
		public ListContainersResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x000412F8 File Offset: 0x0003F4F8
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00041329 File Offset: 0x0003F529
		public IEnumerable<BlobContainerEntry> Containers
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00041331 File Offset: 0x0003F531
		public string Prefix
		{
			get
			{
				base.Variable(ref this.prefixConsumable);
				return this.prefix;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00041345 File Offset: 0x0003F545
		public string Marker
		{
			get
			{
				base.Variable(ref this.markerConsumable);
				return this.marker;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600115F RID: 4447 RVA: 0x00041359 File Offset: 0x0003F559
		public int MaxResults
		{
			get
			{
				base.Variable(ref this.maxResultsConsumable);
				return this.maxResults;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x0004136D File Offset: 0x0003F56D
		public string NextMarker
		{
			get
			{
				base.Variable(ref this.nextMarkerConsumable);
				return this.nextMarker;
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00041384 File Offset: 0x0003F584
		private BlobContainerEntry ParseContainerEntry(Uri baseUri)
		{
			string text = null;
			IDictionary<string, string> dictionary = null;
			BlobContainerProperties blobContainerProperties = new BlobContainerProperties();
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
											blobContainerProperties.LastModified = new DateTimeOffset?(this.reader.ReadElementContentAsString().ToUTCTime());
											continue;
										}
										if (name2 == "Etag")
										{
											blobContainerProperties.ETag = this.reader.ReadElementContentAsString();
											continue;
										}
										if (name2 == "LeaseStatus")
										{
											blobContainerProperties.LeaseStatus = BlobHttpResponseParsers.GetLeaseStatus(this.reader.ReadElementContentAsString());
											continue;
										}
										if (name2 == "LeaseState")
										{
											blobContainerProperties.LeaseState = BlobHttpResponseParsers.GetLeaseState(this.reader.ReadElementContentAsString());
											continue;
										}
										if (name2 == "LeaseDuration")
										{
											blobContainerProperties.LeaseDuration = BlobHttpResponseParsers.GetLeaseDuration(this.reader.ReadElementContentAsString());
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
			return new BlobContainerEntry
			{
				Properties = blobContainerProperties,
				Name = text,
				Uri = NavigationHelper.AppendPathToSingleUri(baseUri, text),
				Metadata = dictionary
			};
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x0004192C File Offset: 0x0003FB2C
		protected override IEnumerable<BlobContainerEntry> ParseXml()
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
								if (name == "Containers")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement("Container"))
									{
										yield return this.ParseContainerEntry(baseUri);
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

		// Token: 0x040004AD RID: 1197
		private string prefix;

		// Token: 0x040004AE RID: 1198
		private bool prefixConsumable;

		// Token: 0x040004AF RID: 1199
		private string marker;

		// Token: 0x040004B0 RID: 1200
		private bool markerConsumable;

		// Token: 0x040004B1 RID: 1201
		private int maxResults;

		// Token: 0x040004B2 RID: 1202
		private bool maxResultsConsumable;

		// Token: 0x040004B3 RID: 1203
		private string nextMarker;

		// Token: 0x040004B4 RID: 1204
		private bool nextMarkerConsumable;
	}
}

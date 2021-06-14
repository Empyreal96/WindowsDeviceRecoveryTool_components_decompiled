using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.Blob.Protocol
{
	// Token: 0x020000D1 RID: 209
	public sealed class ListBlobsResponse : ResponseParsingBase<IListBlobEntry>
	{
		// Token: 0x0600114F RID: 4431 RVA: 0x00040727 File Offset: 0x0003E927
		public ListBlobsResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x00040730 File Offset: 0x0003E930
		public BlobListingContext ListingContext
		{
			get
			{
				return new BlobListingContext(this.Prefix, new int?(this.MaxResults), this.Delimiter, BlobListingDetails.None)
				{
					Marker = this.NextMarker
				};
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x00040768 File Offset: 0x0003E968
		public IEnumerable<IListBlobEntry> Blobs
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00040770 File Offset: 0x0003E970
		public string Prefix
		{
			get
			{
				base.Variable(ref this.prefixConsumable);
				return this.prefix;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x00040784 File Offset: 0x0003E984
		public string Marker
		{
			get
			{
				base.Variable(ref this.markerConsumable);
				return this.marker;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00040798 File Offset: 0x0003E998
		public string Delimiter
		{
			get
			{
				base.Variable(ref this.delimiterConsumable);
				return this.delimiter;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x000407AC File Offset: 0x0003E9AC
		public int MaxResults
		{
			get
			{
				base.Variable(ref this.maxResultsConsumable);
				return this.maxResults;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x000407C0 File Offset: 0x0003E9C0
		public string NextMarker
		{
			get
			{
				base.Variable(ref this.nextMarkerConsumable);
				return this.nextMarker;
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x000407D4 File Offset: 0x0003E9D4
		private IListBlobEntry ParseBlobEntry(Uri baseUri)
		{
			BlobAttributes blobAttributes = new BlobAttributes();
			string text = null;
			string copyId = null;
			string text2 = null;
			string copyCompletionTimeString = null;
			string copyProgressString = null;
			string copySourceString = null;
			string copyStatusDescription = null;
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
						if (name == "Snapshot")
						{
							blobAttributes.SnapshotTime = new DateTimeOffset?(this.reader.ReadElementContentAsString().ToUTCTime());
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
									switch (name2 = this.reader.Name)
									{
									case "Last-Modified":
										blobAttributes.Properties.LastModified = new DateTimeOffset?(this.reader.ReadElementContentAsString().ToUTCTime());
										continue;
									case "Etag":
										blobAttributes.Properties.ETag = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", new object[]
										{
											this.reader.ReadElementContentAsString()
										});
										continue;
									case "Content-Length":
										blobAttributes.Properties.Length = this.reader.ReadElementContentAsLong();
										continue;
									case "Cache-Control":
										blobAttributes.Properties.CacheControl = this.reader.ReadElementContentAsString();
										continue;
									case "Content-Type":
										blobAttributes.Properties.ContentType = this.reader.ReadElementContentAsString();
										continue;
									case "Content-Disposition":
										blobAttributes.Properties.ContentDisposition = this.reader.ReadElementContentAsString();
										continue;
									case "Content-Encoding":
										blobAttributes.Properties.ContentEncoding = this.reader.ReadElementContentAsString();
										continue;
									case "Content-Language":
										blobAttributes.Properties.ContentLanguage = this.reader.ReadElementContentAsString();
										continue;
									case "Content-MD5":
										blobAttributes.Properties.ContentMD5 = this.reader.ReadElementContentAsString();
										continue;
									case "BlobType":
									{
										string text3 = this.reader.ReadElementContentAsString();
										string a;
										if ((a = text3) == null)
										{
											continue;
										}
										if (a == "BlockBlob")
										{
											blobAttributes.Properties.BlobType = BlobType.BlockBlob;
											continue;
										}
										if (a == "PageBlob")
										{
											blobAttributes.Properties.BlobType = BlobType.PageBlob;
											continue;
										}
										if (!(a == "AppendBlob"))
										{
											continue;
										}
										blobAttributes.Properties.BlobType = BlobType.AppendBlob;
										continue;
									}
									case "LeaseStatus":
										blobAttributes.Properties.LeaseStatus = BlobHttpResponseParsers.GetLeaseStatus(this.reader.ReadElementContentAsString());
										continue;
									case "LeaseState":
										blobAttributes.Properties.LeaseState = BlobHttpResponseParsers.GetLeaseState(this.reader.ReadElementContentAsString());
										continue;
									case "LeaseDuration":
										blobAttributes.Properties.LeaseDuration = BlobHttpResponseParsers.GetLeaseDuration(this.reader.ReadElementContentAsString());
										continue;
									case "CopyId":
										copyId = this.reader.ReadElementContentAsString();
										continue;
									case "CopyCompletionTime":
										copyCompletionTimeString = this.reader.ReadElementContentAsString();
										continue;
									case "CopyStatus":
										text2 = this.reader.ReadElementContentAsString();
										continue;
									case "CopyProgress":
										copyProgressString = this.reader.ReadElementContentAsString();
										continue;
									case "CopySource":
										copySourceString = this.reader.ReadElementContentAsString();
										continue;
									case "CopyStatusDescription":
										copyStatusDescription = this.reader.ReadElementContentAsString();
										continue;
									}
									this.reader.Skip();
								}
							}
							this.reader.ReadEndElement();
							continue;
						}
						if (name == "Metadata")
						{
							blobAttributes.Metadata = Response.ParseMetadata(this.reader);
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			Uri uri = NavigationHelper.AppendPathToSingleUri(baseUri, text);
			if (blobAttributes.SnapshotTime != null)
			{
				UriQueryBuilder uriQueryBuilder = new UriQueryBuilder();
				uriQueryBuilder.Add("snapshot", Request.ConvertDateTimeToSnapshotString(blobAttributes.SnapshotTime.Value));
				uri = uriQueryBuilder.AddToUri(uri);
			}
			blobAttributes.StorageUri = new StorageUri(uri);
			if (!string.IsNullOrEmpty(text2))
			{
				blobAttributes.CopyState = BlobHttpResponseParsers.GetCopyAttributes(text2, copyId, copySourceString, copyProgressString, copyCompletionTimeString, copyStatusDescription);
			}
			return new ListBlobEntry(text, blobAttributes);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00040D90 File Offset: 0x0003EF90
		private IListBlobEntry ParseBlobPrefixEntry()
		{
			ListBlobPrefixEntry listBlobPrefixEntry = new ListBlobPrefixEntry();
			this.reader.ReadStartElement();
			while (this.reader.IsStartElement())
			{
				string name;
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else if ((name = this.reader.Name) != null && name == "Name")
				{
					listBlobPrefixEntry.Name = this.reader.ReadElementContentAsString();
				}
				else
				{
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			return listBlobPrefixEntry;
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000412D0 File Offset: 0x0003F4D0
		protected override IEnumerable<IListBlobEntry> ParseXml()
		{
			if (this.reader.ReadToFollowing("EnumerationResults"))
			{
				if (this.reader.IsEmptyElement)
				{
					this.reader.Skip();
				}
				else
				{
					string serviceEndpoint = this.reader.GetAttribute("ServiceEndpoint");
					Uri baseUri;
					if (!string.IsNullOrEmpty(serviceEndpoint))
					{
						baseUri = NavigationHelper.AppendPathToSingleUri(new Uri(serviceEndpoint), this.reader.GetAttribute("ContainerName"));
					}
					else
					{
						baseUri = new Uri(this.reader.GetAttribute("ContainerName"));
					}
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
							switch (name = this.reader.Name)
							{
							case "Delimiter":
								this.delimiter = this.reader.ReadElementContentAsString();
								this.delimiterConsumable = true;
								yield return null;
								continue;
							case "Marker":
								this.marker = this.reader.ReadElementContentAsString();
								this.markerConsumable = true;
								yield return null;
								continue;
							case "NextMarker":
								this.nextMarker = this.reader.ReadElementContentAsString();
								this.nextMarkerConsumable = true;
								yield return null;
								continue;
							case "MaxResults":
								this.maxResults = this.reader.ReadElementContentAsInt();
								this.maxResultsConsumable = true;
								yield return null;
								continue;
							case "Prefix":
								this.prefix = this.reader.ReadElementContentAsString();
								this.prefixConsumable = true;
								yield return null;
								continue;
							case "Blobs":
								this.reader.ReadStartElement();
								while (this.reader.IsStartElement())
								{
									string name2;
									if ((name2 = this.reader.Name) != null)
									{
										if (!(name2 == "Blob"))
										{
											if (name2 == "BlobPrefix")
											{
												yield return this.ParseBlobPrefixEntry();
											}
										}
										else
										{
											yield return this.ParseBlobEntry(baseUri);
										}
									}
								}
								this.reader.ReadEndElement();
								this.allObjectsParsed = true;
								continue;
							}
							this.reader.Skip();
						}
					}
					this.reader.ReadEndElement();
				}
			}
			yield break;
		}

		// Token: 0x040004A3 RID: 1187
		private string prefix;

		// Token: 0x040004A4 RID: 1188
		private bool prefixConsumable;

		// Token: 0x040004A5 RID: 1189
		private string marker;

		// Token: 0x040004A6 RID: 1190
		private bool markerConsumable;

		// Token: 0x040004A7 RID: 1191
		private string delimiter;

		// Token: 0x040004A8 RID: 1192
		private bool delimiterConsumable;

		// Token: 0x040004A9 RID: 1193
		private int maxResults;

		// Token: 0x040004AA RID: 1194
		private bool maxResultsConsumable;

		// Token: 0x040004AB RID: 1195
		private string nextMarker;

		// Token: 0x040004AC RID: 1196
		private bool nextMarkerConsumable;
	}
}

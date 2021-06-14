using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Microsoft.WindowsAzure.Storage.File.Protocol
{
	// Token: 0x020000EF RID: 239
	public sealed class ListFilesAndDirectoriesResponse : ResponseParsingBase<IListFileEntry>
	{
		// Token: 0x06001215 RID: 4629 RVA: 0x00042B48 File Offset: 0x00040D48
		public ListFilesAndDirectoriesResponse(Stream stream) : base(stream)
		{
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001216 RID: 4630 RVA: 0x00042B54 File Offset: 0x00040D54
		public FileListingContext ListingContext
		{
			get
			{
				return new FileListingContext(new int?(this.MaxResults))
				{
					Marker = this.NextMarker
				};
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x00042B7F File Offset: 0x00040D7F
		public IEnumerable<IListFileEntry> Files
		{
			get
			{
				return base.ObjectsToParse;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001218 RID: 4632 RVA: 0x00042B87 File Offset: 0x00040D87
		public string Marker
		{
			get
			{
				base.Variable(ref this.markerConsumable);
				return this.marker;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001219 RID: 4633 RVA: 0x00042B9B File Offset: 0x00040D9B
		public int MaxResults
		{
			get
			{
				base.Variable(ref this.maxResultsConsumable);
				return this.maxResults;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600121A RID: 4634 RVA: 0x00042BAF File Offset: 0x00040DAF
		public string NextMarker
		{
			get
			{
				base.Variable(ref this.nextMarkerConsumable);
				return this.nextMarker;
			}
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00042BC4 File Offset: 0x00040DC4
		private IListFileEntry ParseFileEntry(Uri baseUri)
		{
			CloudFileAttributes cloudFileAttributes = new CloudFileAttributes();
			string text = null;
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
								string name2;
								if (this.reader.IsEmptyElement)
								{
									this.reader.Skip();
								}
								else if ((name2 = this.reader.Name) != null && name2 == "Content-Length")
								{
									cloudFileAttributes.Properties.Length = this.reader.ReadElementContentAsLong();
								}
								else
								{
									this.reader.Skip();
								}
							}
							this.reader.ReadEndElement();
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			Uri primaryUri = NavigationHelper.AppendPathToSingleUri(baseUri, text);
			cloudFileAttributes.StorageUri = new StorageUri(primaryUri);
			return new ListFileEntry(text, cloudFileAttributes);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00042D0C File Offset: 0x00040F0C
		private IListFileEntry ParseFileDirectoryEntry(Uri baseUri)
		{
			FileDirectoryProperties fileDirectoryProperties = new FileDirectoryProperties();
			string text = null;
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
											fileDirectoryProperties.LastModified = new DateTimeOffset?(this.reader.ReadElementContentAsString().ToUTCTime());
											continue;
										}
										if (name2 == "Etag")
										{
											fileDirectoryProperties.ETag = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", new object[]
											{
												this.reader.ReadElementContentAsString()
											});
											continue;
										}
									}
									this.reader.Skip();
								}
							}
							this.reader.ReadEndElement();
							continue;
						}
					}
					this.reader.Skip();
				}
			}
			this.reader.ReadEndElement();
			Uri uri = NavigationHelper.AppendPathToSingleUri(baseUri, text);
			return new ListFileDirectoryEntry(text, uri, fileDirectoryProperties);
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00043268 File Offset: 0x00041468
		protected override IEnumerable<IListFileEntry> ParseXml()
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
					baseUri = NavigationHelper.AppendPathToSingleUri(baseUri, this.reader.GetAttribute("ShareName"));
					baseUri = NavigationHelper.AppendPathToSingleUri(baseUri, this.reader.GetAttribute("DirectoryPath"));
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
								if (name == "Entries")
								{
									this.reader.ReadStartElement();
									while (this.reader.IsStartElement())
									{
										string name2;
										if ((name2 = this.reader.Name) != null)
										{
											if (!(name2 == "File"))
											{
												if (name2 == "Directory")
												{
													yield return this.ParseFileDirectoryEntry(baseUri);
												}
											}
											else
											{
												yield return this.ParseFileEntry(baseUri);
											}
										}
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

		// Token: 0x04000512 RID: 1298
		private string marker;

		// Token: 0x04000513 RID: 1299
		private bool markerConsumable;

		// Token: 0x04000514 RID: 1300
		private int maxResults;

		// Token: 0x04000515 RID: 1301
		private bool maxResultsConsumable;

		// Token: 0x04000516 RID: 1302
		private string nextMarker;

		// Token: 0x04000517 RID: 1303
		private bool nextMarkerConsumable;
	}
}

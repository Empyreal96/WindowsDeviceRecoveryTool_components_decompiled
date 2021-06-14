using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.OData
{
	// Token: 0x020001B1 RID: 433
	internal sealed class MediaTypeResolver
	{
		// Token: 0x06000D63 RID: 3427 RVA: 0x0002DE70 File Offset: 0x0002C070
		private MediaTypeResolver(ODataVersion version)
		{
			this.version = version;
			this.mediaTypesForPayloadKind = MediaTypeResolver.CloneDefaultMediaTypes();
			if (this.version < ODataVersion.V3)
			{
				MediaTypeWithFormat mediaTypeWithFormat = new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonMediaType
				};
				this.AddForJsonPayloadKinds(mediaTypeWithFormat);
				return;
			}
			this.AddJsonLightMediaTypes();
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0002DECA File Offset: 0x0002C0CA
		private MediaTypeResolver(ODataVersion version, ODataBehaviorKind formatBehaviorKind) : this(version)
		{
			if (this.version < ODataVersion.V3 && formatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
			{
				this.AddV2ClientMediaTypes();
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D65 RID: 3429 RVA: 0x0002DEE6 File Offset: 0x0002C0E6
		public static MediaTypeResolver DefaultMediaTypeResolver
		{
			get
			{
				return MediaTypeResolver.MediaTypeResolverCache[ODataVersion.V1];
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0002DEF3 File Offset: 0x0002C0F3
		internal static MediaTypeResolver GetWriterMediaTypeResolver(ODataVersion version)
		{
			return MediaTypeResolver.MediaTypeResolverCache[version];
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0002DF00 File Offset: 0x0002C100
		internal static MediaTypeResolver CreateReaderMediaTypeResolver(ODataVersion version, ODataBehaviorKind formatBehaviorKind)
		{
			return new MediaTypeResolver(version, formatBehaviorKind);
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0002DF09 File Offset: 0x0002C109
		internal IList<MediaTypeWithFormat> GetMediaTypesForPayloadKind(ODataPayloadKind payloadKind)
		{
			return this.mediaTypesForPayloadKind[(int)payloadKind];
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0002DF14 File Offset: 0x0002C114
		internal bool IsIllegalMediaType(MediaType mediaType)
		{
			return this.version < ODataVersion.V3 && HttpUtils.CompareMediaTypeNames(mediaType.SubTypeName, "json") && HttpUtils.CompareMediaTypeNames(mediaType.TypeName, "application") && (mediaType.MediaTypeHasParameterWithValue("odata", "minimalmetadata") || mediaType.MediaTypeHasParameterWithValue("odata", "fullmetadata") || mediaType.MediaTypeHasParameterWithValue("odata", "nometadata"));
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002DF88 File Offset: 0x0002C188
		private static IList<MediaTypeWithFormat>[] CloneDefaultMediaTypes()
		{
			IList<MediaTypeWithFormat>[] array = new IList<MediaTypeWithFormat>[MediaTypeResolver.defaultMediaTypes.Length];
			for (int i = 0; i < MediaTypeResolver.defaultMediaTypes.Length; i++)
			{
				array[i] = new List<MediaTypeWithFormat>(MediaTypeResolver.defaultMediaTypes[i]);
			}
			return array;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		private static void AddMediaTypeEntryOrdered(IList<MediaTypeWithFormat> mediaTypeList, MediaTypeWithFormat mediaTypeToInsert, ODataFormat beforeFormat)
		{
			for (int i = 0; i < mediaTypeList.Count; i++)
			{
				if (mediaTypeList[i].Format == beforeFormat)
				{
					mediaTypeList.Insert(i, mediaTypeToInsert);
					return;
				}
			}
			mediaTypeList.Add(mediaTypeToInsert);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0002E11C File Offset: 0x0002C31C
		private void AddJsonLightMediaTypes()
		{
			var array = new <>f__AnonymousType0<string, string[]>[]
			{
				new
				{
					ParameterName = "odata",
					Values = new string[]
					{
						"minimalmetadata",
						"fullmetadata",
						"nometadata"
					}
				},
				new
				{
					ParameterName = "streaming",
					Values = new string[]
					{
						"true",
						"false"
					}
				}
			};
			LinkedList<MediaType> linkedList = new LinkedList<MediaType>();
			linkedList.AddFirst(MediaTypeResolver.ApplicationJsonMediaType);
			var array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				var <>f__AnonymousType = array2[i];
				for (LinkedListNode<MediaType> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
				{
					MediaType value = linkedListNode.Value;
					foreach (string value2 in <>f__AnonymousType.Values)
					{
						List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>(value.Parameters ?? Enumerable.Empty<KeyValuePair<string, string>>())
						{
							new KeyValuePair<string, string>(<>f__AnonymousType.ParameterName, value2)
						};
						MediaType value3 = new MediaType(value.TypeName, value.SubTypeName, parameters);
						linkedList.AddBefore(linkedListNode, value3);
					}
				}
			}
			foreach (MediaType mediaType in linkedList)
			{
				MediaTypeWithFormat mediaTypeWithFormat = new MediaTypeWithFormat
				{
					Format = ODataFormat.Json,
					MediaType = mediaType
				};
				if (mediaType == MediaTypeResolver.ApplicationJsonMediaType)
				{
					this.AddForJsonPayloadKinds(mediaTypeWithFormat);
				}
				else
				{
					this.InsertForJsonPayloadKinds(mediaTypeWithFormat, ODataFormat.VerboseJson);
				}
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0002E2D4 File Offset: 0x0002C4D4
		private void AddForJsonPayloadKinds(MediaTypeWithFormat mediaTypeWithFormat)
		{
			foreach (ODataPayloadKind odataPayloadKind in MediaTypeResolver.JsonPayloadKinds)
			{
				this.mediaTypesForPayloadKind[(int)odataPayloadKind].Add(mediaTypeWithFormat);
			}
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0002E308 File Offset: 0x0002C508
		private void InsertForJsonPayloadKinds(MediaTypeWithFormat mediaTypeWithFormat, ODataFormat beforeFormat)
		{
			foreach (ODataPayloadKind odataPayloadKind in MediaTypeResolver.JsonPayloadKinds)
			{
				MediaTypeResolver.AddMediaTypeEntryOrdered(this.mediaTypesForPayloadKind[(int)odataPayloadKind], mediaTypeWithFormat, beforeFormat);
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0002E33C File Offset: 0x0002C53C
		private void AddV2ClientMediaTypes()
		{
			MediaTypeWithFormat item = new MediaTypeWithFormat
			{
				Format = ODataFormat.Atom,
				MediaType = MediaTypeResolver.ApplicationAtomXmlMediaType
			};
			MediaTypeWithFormat item2 = new MediaTypeWithFormat
			{
				Format = ODataFormat.Atom,
				MediaType = MediaTypeResolver.ApplicationXmlMediaType
			};
			IList<MediaTypeWithFormat> list = this.mediaTypesForPayloadKind[0];
			list.Add(new MediaTypeWithFormat
			{
				Format = ODataFormat.Atom,
				MediaType = new MediaType("application", "xml", new KeyValuePair<string, string>[]
				{
					new KeyValuePair<string, string>("type", "feed")
				})
			});
			list.Add(item2);
			IList<MediaTypeWithFormat> list2 = this.mediaTypesForPayloadKind[1];
			list2.Add(new MediaTypeWithFormat
			{
				Format = ODataFormat.Atom,
				MediaType = new MediaType("application", "xml", new KeyValuePair<string, string>[]
				{
					new KeyValuePair<string, string>("type", "entry")
				})
			});
			list2.Add(item2);
			this.mediaTypesForPayloadKind[2].Add(item);
			this.mediaTypesForPayloadKind[3].Add(item);
			this.mediaTypesForPayloadKind[4].Add(item);
			this.mediaTypesForPayloadKind[7].Add(item);
			this.mediaTypesForPayloadKind[8].Add(item);
			this.mediaTypesForPayloadKind[9].Add(item);
			this.mediaTypesForPayloadKind[10].Add(item);
		}

		// Token: 0x0400047D RID: 1149
		private static readonly MediaType ApplicationAtomXmlMediaType = new MediaType("application", "atom+xml");

		// Token: 0x0400047E RID: 1150
		private static readonly MediaType ApplicationXmlMediaType = new MediaType("application", "xml");

		// Token: 0x0400047F RID: 1151
		private static readonly MediaType TextXmlMediaType = new MediaType("text", "xml");

		// Token: 0x04000480 RID: 1152
		private static readonly MediaType ApplicationJsonMediaType = new MediaType("application", "json");

		// Token: 0x04000481 RID: 1153
		private static readonly MediaType ApplicationJsonVerboseMediaType = new MediaType("application", "json", new KeyValuePair<string, string>[]
		{
			new KeyValuePair<string, string>("odata", "verbose")
		});

		// Token: 0x04000482 RID: 1154
		private static readonly MediaTypeWithFormat[][] defaultMediaTypes = new MediaTypeWithFormat[][]
		{
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = new MediaType("application", "atom+xml", new KeyValuePair<string, string>[]
					{
						new KeyValuePair<string, string>("type", "feed")
					})
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationAtomXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = new MediaType("application", "atom+xml", new KeyValuePair<string, string>[]
					{
						new KeyValuePair<string, string>("type", "entry")
					})
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationAtomXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.TextXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.TextXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.TextXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.RawValue,
					MediaType = new MediaType("text", "plain")
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.RawValue,
					MediaType = new MediaType("application", "octet-stream")
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.TextXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = new MediaType("application", "atomsvc+xml")
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Metadata,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Atom,
					MediaType = MediaTypeResolver.ApplicationXmlMediaType
				},
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.Batch,
					MediaType = new MediaType("multipart", "mixed")
				}
			},
			new MediaTypeWithFormat[]
			{
				new MediaTypeWithFormat
				{
					Format = ODataFormat.VerboseJson,
					MediaType = MediaTypeResolver.ApplicationJsonVerboseMediaType
				}
			}
		};

		// Token: 0x04000483 RID: 1155
		private static readonly ODataVersionCache<MediaTypeResolver> MediaTypeResolverCache = new ODataVersionCache<MediaTypeResolver>((ODataVersion version) => new MediaTypeResolver(version));

		// Token: 0x04000484 RID: 1156
		private readonly ODataVersion version;

		// Token: 0x04000485 RID: 1157
		private readonly IList<MediaTypeWithFormat>[] mediaTypesForPayloadKind;

		// Token: 0x04000486 RID: 1158
		private static readonly ODataPayloadKind[] JsonPayloadKinds = new ODataPayloadKind[]
		{
			ODataPayloadKind.Feed,
			ODataPayloadKind.Entry,
			ODataPayloadKind.Property,
			ODataPayloadKind.EntityReferenceLink,
			ODataPayloadKind.EntityReferenceLinks,
			ODataPayloadKind.Collection,
			ODataPayloadKind.ServiceDocument,
			ODataPayloadKind.Error,
			ODataPayloadKind.Parameter
		};
	}
}

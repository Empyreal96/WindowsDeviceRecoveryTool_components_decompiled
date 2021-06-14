using System;
using System.Diagnostics;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001B6 RID: 438
	internal abstract class ODataVerboseJsonDeserializer : ODataDeserializer
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x0002EE89 File Offset: 0x0002D089
		protected ODataVerboseJsonDeserializer(ODataVerboseJsonInputContext jsonInputContext) : base(jsonInputContext)
		{
			this.jsonInputContext = jsonInputContext;
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x0002EE99 File Offset: 0x0002D099
		internal BufferingJsonReader JsonReader
		{
			get
			{
				return this.jsonInputContext.JsonReader;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0002EEA6 File Offset: 0x0002D0A6
		protected ODataVerboseJsonInputContext VerboseJsonInputContext
		{
			get
			{
				return this.jsonInputContext;
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0002EEAE File Offset: 0x0002D0AE
		internal void ReadPayloadStart(bool isReadingNestedPayload)
		{
			this.ReadPayloadStart(isReadingNestedPayload, true);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0002EEB8 File Offset: 0x0002D0B8
		internal void ReadPayloadStart(bool isReadingNestedPayload, bool expectResponseWrapper)
		{
			if (!isReadingNestedPayload)
			{
				this.JsonReader.Read();
			}
			if (base.ReadingResponse && expectResponseWrapper)
			{
				this.JsonReader.ReadStartObject();
				while (this.JsonReader.NodeType == JsonNodeType.Property)
				{
					string strB = this.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal("d", strB) == 0)
					{
						break;
					}
					this.JsonReader.SkipValue();
				}
				if (this.JsonReader.NodeType == JsonNodeType.EndObject)
				{
					throw new ODataException(Strings.ODataJsonDeserializer_DataWrapperPropertyNotFound);
				}
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0002EF37 File Offset: 0x0002D137
		internal void ReadPayloadEnd(bool isReadingNestedPayload)
		{
			this.ReadPayloadEnd(isReadingNestedPayload, true);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0002EF44 File Offset: 0x0002D144
		internal void ReadPayloadEnd(bool isReadingNestedPayload, bool expectResponseWrapper)
		{
			if (base.ReadingResponse && expectResponseWrapper)
			{
				while (this.JsonReader.NodeType == JsonNodeType.Property)
				{
					string strB = this.JsonReader.ReadPropertyName();
					if (string.CompareOrdinal("d", strB) == 0)
					{
						throw new ODataException(Strings.ODataJsonDeserializer_DataWrapperMultipleProperties);
					}
					this.JsonReader.SkipValue();
				}
				this.JsonReader.ReadEndObject();
			}
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0002EFA6 File Offset: 0x0002D1A6
		internal Uri ProcessUriFromPayload(string uriFromPayload)
		{
			return this.ProcessUriFromPayload(uriFromPayload, true);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0002EFB0 File Offset: 0x0002D1B0
		internal Uri ProcessUriFromPayload(string uriFromPayload, bool requireAbsoluteUri)
		{
			Uri uri = new Uri(uriFromPayload, UriKind.RelativeOrAbsolute);
			Uri uri2 = this.VerboseJsonInputContext.ResolveUri(base.MessageReaderSettings.BaseUri, uri);
			if (uri2 != null)
			{
				return uri2;
			}
			if (!uri.IsAbsoluteUri)
			{
				if (base.MessageReaderSettings.BaseUri != null)
				{
					uri = UriUtils.UriToAbsoluteUri(base.MessageReaderSettings.BaseUri, uri);
				}
				else if (requireAbsoluteUri)
				{
					throw new ODataException(Strings.ODataJsonDeserializer_RelativeUriUsedWithoutBaseUriSpecified(uriFromPayload));
				}
			}
			return uri;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0002F027 File Offset: 0x0002D227
		[Conditional("DEBUG")]
		internal void AssertJsonCondition(params JsonNodeType[] allowedNodeTypes)
		{
		}

		// Token: 0x0400048F RID: 1167
		private readonly ODataVerboseJsonInputContext jsonInputContext;
	}
}

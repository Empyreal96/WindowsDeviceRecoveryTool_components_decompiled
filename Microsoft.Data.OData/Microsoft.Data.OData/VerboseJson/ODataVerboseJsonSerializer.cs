using System;
using Microsoft.Data.OData.Json;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C1 RID: 449
	internal class ODataVerboseJsonSerializer : ODataSerializer
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x00030659 File Offset: 0x0002E859
		internal ODataVerboseJsonSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
			this.verboseJsonOutputContext = verboseJsonOutputContext;
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00030669 File Offset: 0x0002E869
		internal ODataVerboseJsonOutputContext VerboseJsonOutputContext
		{
			get
			{
				return this.verboseJsonOutputContext;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00030671 File Offset: 0x0002E871
		internal IJsonWriter JsonWriter
		{
			get
			{
				return this.verboseJsonOutputContext.JsonWriter;
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0003067E File Offset: 0x0002E87E
		internal void WritePayloadStart()
		{
			this.WritePayloadStart(false);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00030687 File Offset: 0x0002E887
		internal void WritePayloadStart(bool disableResponseWrapper)
		{
			ODataJsonWriterUtils.StartJsonPaddingIfRequired(this.JsonWriter, base.MessageWriterSettings);
			if (base.WritingResponse && !disableResponseWrapper)
			{
				this.JsonWriter.StartObjectScope();
				this.JsonWriter.WriteDataWrapper();
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000306BB File Offset: 0x0002E8BB
		internal void WritePayloadEnd()
		{
			this.WritePayloadEnd(false);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000306C4 File Offset: 0x0002E8C4
		internal void WritePayloadEnd(bool disableResponseWrapper)
		{
			if (base.WritingResponse && !disableResponseWrapper)
			{
				this.JsonWriter.EndObjectScope();
			}
			ODataJsonWriterUtils.EndJsonPaddingIfRequired(this.JsonWriter, base.MessageWriterSettings);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x000306ED File Offset: 0x0002E8ED
		internal void WriteTopLevelPayload(Action payloadWriterAction)
		{
			this.WriteTopLevelPayload(payloadWriterAction, false);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x000306F7 File Offset: 0x0002E8F7
		internal void WriteTopLevelPayload(Action payloadWriterAction, bool disableResponseWrapper)
		{
			this.WritePayloadStart(disableResponseWrapper);
			payloadWriterAction();
			this.WritePayloadEnd(disableResponseWrapper);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00030750 File Offset: 0x0002E950
		internal void WriteTopLevelError(ODataError error, bool includeDebugInformation)
		{
			this.WriteTopLevelPayload(delegate()
			{
				ODataJsonWriterUtils.WriteError(this.VerboseJsonOutputContext.JsonWriter, null, error, includeDebugInformation, this.MessageWriterSettings.MessageQuotas.MaxNestingDepth, false);
			}, true);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0003078B File Offset: 0x0002E98B
		internal string UriToAbsoluteUriString(Uri uri)
		{
			return this.UriToUriString(uri, true);
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00030795 File Offset: 0x0002E995
		internal string UriToUriString(Uri uri, bool makeAbsolute)
		{
			return ODataJsonWriterUtils.UriToUriString(this.verboseJsonOutputContext, uri, makeAbsolute);
		}

		// Token: 0x040004AA RID: 1194
		private readonly ODataVerboseJsonOutputContext verboseJsonOutputContext;
	}
}

using System;
using Microsoft.Data.OData;

namespace System.Data.Services.Client
{
	// Token: 0x0200008A RID: 138
	internal class ODataWriterWrapper
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x00013D27 File Offset: 0x00011F27
		private ODataWriterWrapper(ODataWriter odataWriter, DataServiceClientRequestPipelineConfiguration requestPipeline)
		{
			this.odataWriter = odataWriter;
			this.requestPipeline = requestPipeline;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00013D3D File Offset: 0x00011F3D
		internal static ODataWriterWrapper CreateForEntry(ODataMessageWriter messageWriter, DataServiceClientRequestPipelineConfiguration requestPipeline)
		{
			return new ODataWriterWrapper(messageWriter.CreateODataEntryWriter(), requestPipeline);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00013D4B File Offset: 0x00011F4B
		internal static ODataWriterWrapper CreateForEntryTest(ODataWriter writer, DataServiceClientRequestPipelineConfiguration requestPipeline)
		{
			return new ODataWriterWrapper(writer, requestPipeline);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00013D54 File Offset: 0x00011F54
		internal void WriteStart(ODataEntry entry, object entity)
		{
			this.requestPipeline.ExecuteOnEntryStartActions(entry, entity);
			this.odataWriter.WriteStart(entry);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00013D6F File Offset: 0x00011F6F
		internal void WriteEnd(ODataEntry entry, object entity)
		{
			this.requestPipeline.ExecuteOnEntryEndActions(entry, entity);
			this.odataWriter.WriteEnd();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00013D89 File Offset: 0x00011F89
		internal void WriteEnd(ODataNavigationLink navlink, object source, object target)
		{
			this.requestPipeline.ExecuteOnNavigationLinkEndActions(navlink, source, target);
			this.odataWriter.WriteEnd();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00013DA4 File Offset: 0x00011FA4
		internal void WriteNavigationLinkEnd(ODataNavigationLink navlink, object source, object target)
		{
			this.requestPipeline.ExecuteOnNavigationLinkEndActions(navlink, source, target);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00013DB4 File Offset: 0x00011FB4
		internal void WriteNavigationLinksEnd()
		{
			this.odataWriter.WriteEnd();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00013DC1 File Offset: 0x00011FC1
		internal void WriteStart(ODataNavigationLink navigationLink, object source, object target)
		{
			this.requestPipeline.ExecuteOnNavigationLinkStartActions(navigationLink, source, target);
			this.odataWriter.WriteStart(navigationLink);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00013DDD File Offset: 0x00011FDD
		internal void WriteNavigationLinkStart(ODataNavigationLink navigationLink, object source, object target)
		{
			this.requestPipeline.ExecuteOnNavigationLinkStartActions(navigationLink, source, target);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00013DED File Offset: 0x00011FED
		internal void WriteNavigationLinksStart(ODataNavigationLink navigationLink)
		{
			this.odataWriter.WriteStart(navigationLink);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00013DFB File Offset: 0x00011FFB
		internal void WriteEntityReferenceLink(ODataEntityReferenceLink referenceLink, object source, object target)
		{
			this.requestPipeline.ExecuteEntityReferenceLinkActions(referenceLink, source, target);
			this.odataWriter.WriteEntityReferenceLink(referenceLink);
		}

		// Token: 0x040002F6 RID: 758
		private readonly ODataWriter odataWriter;

		// Token: 0x040002F7 RID: 759
		private readonly DataServiceClientRequestPipelineConfiguration requestPipeline;
	}
}

using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C4 RID: 452
	internal sealed class ODataVerboseJsonEntityReferenceLinkSerializer : ODataVerboseJsonSerializer
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00030DFF File Offset: 0x0002EFFF
		internal ODataVerboseJsonEntityReferenceLinkSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00030E24 File Offset: 0x0002F024
		internal void WriteEntityReferenceLink(ODataEntityReferenceLink link)
		{
			base.WriteTopLevelPayload(delegate()
			{
				this.WriteEntityReferenceLinkImplementation(link);
			});
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00030E90 File Offset: 0x0002F090
		internal void WriteEntityReferenceLinks(ODataEntityReferenceLinks entityReferenceLinks)
		{
			base.WriteTopLevelPayload(delegate()
			{
				this.WriteEntityReferenceLinksImplementation(entityReferenceLinks, this.Version >= ODataVersion.V2 && this.WritingResponse);
			});
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00030EC4 File Offset: 0x0002F0C4
		private void WriteEntityReferenceLinkImplementation(ODataEntityReferenceLink entityReferenceLink)
		{
			WriterValidationUtils.ValidateEntityReferenceLink(entityReferenceLink);
			base.JsonWriter.StartObjectScope();
			base.JsonWriter.WriteName("uri");
			base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(entityReferenceLink.Url));
			base.JsonWriter.EndObjectScope();
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00030F14 File Offset: 0x0002F114
		private void WriteEntityReferenceLinksImplementation(ODataEntityReferenceLinks entityReferenceLinks, bool includeResultsWrapper)
		{
			if (includeResultsWrapper)
			{
				base.JsonWriter.StartObjectScope();
			}
			if (entityReferenceLinks.Count != null)
			{
				base.JsonWriter.WriteName("__count");
				base.JsonWriter.WriteValue(entityReferenceLinks.Count.Value);
			}
			if (includeResultsWrapper)
			{
				base.JsonWriter.WriteDataArrayName();
			}
			base.JsonWriter.StartArrayScope();
			IEnumerable<ODataEntityReferenceLink> links = entityReferenceLinks.Links;
			if (links != null)
			{
				foreach (ODataEntityReferenceLink entityReferenceLink in links)
				{
					WriterValidationUtils.ValidateEntityReferenceLinkNotNull(entityReferenceLink);
					this.WriteEntityReferenceLinkImplementation(entityReferenceLink);
				}
			}
			base.JsonWriter.EndArrayScope();
			if (entityReferenceLinks.NextPageLink != null)
			{
				base.JsonWriter.WriteName("__next");
				base.JsonWriter.WriteValue(base.UriToAbsoluteUriString(entityReferenceLinks.NextPageLink));
			}
			if (includeResultsWrapper)
			{
				base.JsonWriter.EndObjectScope();
			}
		}
	}
}

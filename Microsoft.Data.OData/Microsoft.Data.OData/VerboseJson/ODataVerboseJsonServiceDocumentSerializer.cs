using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.VerboseJson
{
	// Token: 0x020001C7 RID: 455
	internal sealed class ODataVerboseJsonServiceDocumentSerializer : ODataVerboseJsonSerializer
	{
		// Token: 0x06000E22 RID: 3618 RVA: 0x00031A47 File Offset: 0x0002FC47
		internal ODataVerboseJsonServiceDocumentSerializer(ODataVerboseJsonOutputContext verboseJsonOutputContext) : base(verboseJsonOutputContext)
		{
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00031B1C File Offset: 0x0002FD1C
		internal void WriteServiceDocument(ODataWorkspace defaultWorkspace)
		{
			IEnumerable<ODataResourceCollectionInfo> collections = defaultWorkspace.Collections;
			base.WriteTopLevelPayload(delegate()
			{
				this.JsonWriter.StartObjectScope();
				this.JsonWriter.WriteName("EntitySets");
				this.JsonWriter.StartArrayScope();
				if (collections != null)
				{
					foreach (ODataResourceCollectionInfo odataResourceCollectionInfo in collections)
					{
						ValidationUtils.ValidateResourceCollectionInfo(odataResourceCollectionInfo);
						this.JsonWriter.WriteValue(UriUtilsCommon.UriToString(odataResourceCollectionInfo.Url));
					}
				}
				this.JsonWriter.EndArrayScope();
				this.JsonWriter.EndObjectScope();
			});
		}
	}
}

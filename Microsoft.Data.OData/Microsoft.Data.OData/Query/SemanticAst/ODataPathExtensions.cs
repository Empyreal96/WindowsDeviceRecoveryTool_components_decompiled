using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200004C RID: 76
	internal static class ODataPathExtensions
	{
		// Token: 0x060001FA RID: 506 RVA: 0x00007D00 File Offset: 0x00005F00
		public static IEdmTypeReference EdmType(this ODataPath path)
		{
			return path.LastSegment.EdmType.ToTypeReference();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00007D12 File Offset: 0x00005F12
		public static IEdmEntitySet EntitySet(this ODataPath path)
		{
			return path.LastSegment.Translate<IEdmEntitySet>(new DetermineEntitySetTranslator());
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00007D24 File Offset: 0x00005F24
		public static bool IsCollection(this ODataPath path)
		{
			return path.LastSegment.Translate<bool>(new IsCollectionTranslator());
		}
	}
}

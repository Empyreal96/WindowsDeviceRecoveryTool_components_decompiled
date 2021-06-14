using System;
using System.Collections.Generic;
using Microsoft.Data.OData.Query.SemanticAst;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200004D RID: 77
	internal static class ODataPathFactory
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00007D38 File Offset: 0x00005F38
		internal static ODataPath BindPath(ICollection<string> segments, ODataUriParserConfiguration configuration)
		{
			ODataPathParser odataPathParser = new ODataPathParser(configuration);
			IList<ODataPathSegment> segments2 = odataPathParser.ParsePath(segments);
			return new ODataPath(segments2);
		}
	}
}

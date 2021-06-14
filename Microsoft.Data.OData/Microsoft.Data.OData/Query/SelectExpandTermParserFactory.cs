using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x0200005E RID: 94
	internal static class SelectExpandTermParserFactory
	{
		// Token: 0x06000265 RID: 613 RVA: 0x00009B9A File Offset: 0x00007D9A
		public static ISelectExpandTermParser Create(string clauseToParse, ODataUriParserSettings settings)
		{
			if (settings.SupportExpandOptions)
			{
				return new ExpandOptionSelectExpandTermParser(clauseToParse, settings.SelectExpandLimit);
			}
			return new NonOptionSelectExpandTermParser(clauseToParse, settings.SelectExpandLimit);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009BBD File Offset: 0x00007DBD
		public static ISelectExpandTermParser Create(string clauseToParse)
		{
			return new NonOptionSelectExpandTermParser(clauseToParse, 800);
		}
	}
}

using System;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SyntacticAst
{
	// Token: 0x02000022 RID: 34
	internal static class ExpandBinderFactory
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x0000448D File Offset: 0x0000268D
		public static ExpandBinder Create(IEdmEntityType elementType, IEdmEntitySet entitySet, ODataUriParserConfiguration configuration)
		{
			if (configuration.Settings.SupportExpandOptions)
			{
				return new ExpandOptionExpandBinder(configuration, elementType, entitySet);
			}
			return new NonOptionExpandBinder(configuration, elementType, entitySet);
		}
	}
}

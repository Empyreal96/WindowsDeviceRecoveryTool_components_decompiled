using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics;
using Microsoft.Data.Edm.Csdl.Internal.Parsing;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x020001B6 RID: 438
	public static class CsdlReader
	{
		// Token: 0x06000AA4 RID: 2724 RVA: 0x0001F418 File Offset: 0x0001D618
		public static bool TryParse(IEnumerable<XmlReader> readers, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			return CsdlReader.TryParse(readers, Enumerable.Empty<IEdmModel>(), out model, out errors);
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0001F428 File Offset: 0x0001D628
		public static bool TryParse(IEnumerable<XmlReader> readers, IEdmModel reference, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			return CsdlReader.TryParse(readers, new IEdmModel[]
			{
				reference
			}, out model, out errors);
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x0001F44C File Offset: 0x0001D64C
		public static bool TryParse(IEnumerable<XmlReader> readers, IEnumerable<IEdmModel> references, out IEdmModel model, out IEnumerable<EdmError> errors)
		{
			CsdlModel astModel;
			if (CsdlParser.TryParse(readers, out astModel, out errors))
			{
				model = new CsdlSemanticsModel(astModel, new CsdlSemanticsDirectValueAnnotationsManager(), references);
				return true;
			}
			model = null;
			return false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Validation.Internal;

namespace Microsoft.Data.Edm.Validation
{
	// Token: 0x020001EE RID: 494
	public static class EdmValidator
	{
		// Token: 0x06000BE1 RID: 3041 RVA: 0x0002292C File Offset: 0x00020B2C
		public static bool Validate(this IEdmModel root, out IEnumerable<EdmError> errors)
		{
			return root.Validate(root.GetEdmVersion() ?? EdmConstants.EdmVersionLatest, out errors);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00022944 File Offset: 0x00020B44
		public static bool Validate(this IEdmModel root, Version version, out IEnumerable<EdmError> errors)
		{
			return root.Validate(ValidationRuleSet.GetEdmModelRuleSet(version), out errors);
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00022953 File Offset: 0x00020B53
		public static bool Validate(this IEdmModel root, ValidationRuleSet ruleSet, out IEnumerable<EdmError> errors)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(root, "root");
			EdmUtil.CheckArgumentNull<ValidationRuleSet>(ruleSet, "ruleSet");
			errors = InterfaceValidator.ValidateModelStructureAndSemantics(root, ruleSet);
			return errors.FirstOrDefault<EdmError>() == null;
		}
	}
}

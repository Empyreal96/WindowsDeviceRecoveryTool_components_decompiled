using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x020000B8 RID: 184
	public class EdmEntitySetReferenceExpression : EdmElement, IEdmEntitySetReferenceExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x0600034F RID: 847 RVA: 0x00008A2B File Offset: 0x00006C2B
		public EdmEntitySetReferenceExpression(IEdmEntitySet referencedEntitySet)
		{
			EdmUtil.CheckArgumentNull<IEdmEntitySet>(referencedEntitySet, "referencedEntitySet");
			this.referencedEntitySet = referencedEntitySet;
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00008A46 File Offset: 0x00006C46
		public IEdmEntitySet ReferencedEntitySet
		{
			get
			{
				return this.referencedEntitySet;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000351 RID: 849 RVA: 0x00008A4E File Offset: 0x00006C4E
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EntitySetReference;
			}
		}

		// Token: 0x04000176 RID: 374
		private readonly IEdmEntitySet referencedEntitySet;
	}
}

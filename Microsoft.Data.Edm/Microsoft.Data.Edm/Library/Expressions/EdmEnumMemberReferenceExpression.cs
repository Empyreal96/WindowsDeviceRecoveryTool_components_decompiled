using System;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x02000184 RID: 388
	public class EdmEnumMemberReferenceExpression : EdmElement, IEdmEnumMemberReferenceExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x06000893 RID: 2195 RVA: 0x00017FD2 File Offset: 0x000161D2
		public EdmEnumMemberReferenceExpression(IEdmEnumMember referencedEnumMember)
		{
			EdmUtil.CheckArgumentNull<IEdmEnumMember>(referencedEnumMember, "referencedEnumMember");
			this.referencedEnumMember = referencedEnumMember;
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00017FED File Offset: 0x000161ED
		public IEdmEnumMember ReferencedEnumMember
		{
			get
			{
				return this.referencedEnumMember;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00017FF5 File Offset: 0x000161F5
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.EnumMemberReference;
			}
		}

		// Token: 0x04000442 RID: 1090
		private readonly IEdmEnumMember referencedEnumMember;
	}
}

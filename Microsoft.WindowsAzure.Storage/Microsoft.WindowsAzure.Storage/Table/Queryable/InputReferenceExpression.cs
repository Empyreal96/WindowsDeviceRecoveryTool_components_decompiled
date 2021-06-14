using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000119 RID: 281
	[DebuggerDisplay("InputReferenceExpression -> {Type}")]
	internal sealed class InputReferenceExpression : Expression
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x00048307 File Offset: 0x00046507
		internal InputReferenceExpression(ResourceExpression target) : base((ExpressionType)10007, target.ResourceType)
		{
			this.target = target;
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x00048321 File Offset: 0x00046521
		internal ResourceExpression Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x00048329 File Offset: 0x00046529
		internal void OverrideTarget(ResourceSetExpression newTarget)
		{
			this.target = newTarget;
		}

		// Token: 0x040005A3 RID: 1443
		private ResourceExpression target;
	}
}

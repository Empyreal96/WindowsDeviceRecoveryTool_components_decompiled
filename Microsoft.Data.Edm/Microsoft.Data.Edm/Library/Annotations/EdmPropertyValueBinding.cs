using System;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Annotations
{
	// Token: 0x0200017C RID: 380
	public class EdmPropertyValueBinding : EdmElement, IEdmPropertyValueBinding, IEdmElement
	{
		// Token: 0x0600086C RID: 2156 RVA: 0x00017D31 File Offset: 0x00015F31
		public EdmPropertyValueBinding(IEdmProperty boundProperty, IEdmExpression value)
		{
			EdmUtil.CheckArgumentNull<IEdmProperty>(boundProperty, "boundProperty");
			EdmUtil.CheckArgumentNull<IEdmExpression>(value, "value");
			this.boundProperty = boundProperty;
			this.value = value;
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x00017D5F File Offset: 0x00015F5F
		public IEdmProperty BoundProperty
		{
			get
			{
				return this.boundProperty;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00017D67 File Offset: 0x00015F67
		public IEdmExpression Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04000432 RID: 1074
		private readonly IEdmProperty boundProperty;

		// Token: 0x04000433 RID: 1075
		private readonly IEdmExpression value;
	}
}

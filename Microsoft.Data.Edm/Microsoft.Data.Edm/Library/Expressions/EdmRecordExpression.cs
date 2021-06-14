using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x02000197 RID: 407
	public class EdmRecordExpression : EdmElement, IEdmRecordExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x060008E7 RID: 2279 RVA: 0x00018465 File Offset: 0x00016665
		public EdmRecordExpression(params IEdmPropertyConstructor[] properties) : this((IEnumerable<IEdmPropertyConstructor>)properties)
		{
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00018473 File Offset: 0x00016673
		public EdmRecordExpression(IEdmStructuredTypeReference declaredType, params IEdmPropertyConstructor[] properties) : this(declaredType, (IEnumerable<IEdmPropertyConstructor>)properties)
		{
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00018482 File Offset: 0x00016682
		public EdmRecordExpression(IEnumerable<IEdmPropertyConstructor> properties) : this(null, properties)
		{
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001848C File Offset: 0x0001668C
		public EdmRecordExpression(IEdmStructuredTypeReference declaredType, IEnumerable<IEdmPropertyConstructor> properties)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<IEdmPropertyConstructor>>(properties, "properties");
			this.declaredType = declaredType;
			this.properties = properties;
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x000184AE File Offset: 0x000166AE
		public IEdmStructuredTypeReference DeclaredType
		{
			get
			{
				return this.declaredType;
			}
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x000184B6 File Offset: 0x000166B6
		public IEnumerable<IEdmPropertyConstructor> Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x000184BE File Offset: 0x000166BE
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Record;
			}
		}

		// Token: 0x0400045E RID: 1118
		private readonly IEdmStructuredTypeReference declaredType;

		// Token: 0x0400045F RID: 1119
		private readonly IEnumerable<IEdmPropertyConstructor> properties;
	}
}

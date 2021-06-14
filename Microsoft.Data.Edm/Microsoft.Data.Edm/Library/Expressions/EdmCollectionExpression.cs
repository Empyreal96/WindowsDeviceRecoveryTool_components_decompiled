using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x0200019F RID: 415
	public class EdmCollectionExpression : EdmElement, IEdmCollectionExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x000186BF File Offset: 0x000168BF
		public EdmCollectionExpression(params IEdmExpression[] elements) : this((IEnumerable<IEdmExpression>)elements)
		{
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x000186CD File Offset: 0x000168CD
		public EdmCollectionExpression(IEdmTypeReference declaredType, params IEdmExpression[] elements) : this(declaredType, (IEnumerable<IEdmExpression>)elements)
		{
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000186DC File Offset: 0x000168DC
		public EdmCollectionExpression(IEnumerable<IEdmExpression> elements) : this(null, elements)
		{
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000186E6 File Offset: 0x000168E6
		public EdmCollectionExpression(IEdmTypeReference declaredType, IEnumerable<IEdmExpression> elements)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<IEdmExpression>>(elements, "elements");
			this.declaredType = declaredType;
			this.elements = elements;
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00018708 File Offset: 0x00016908
		public IEdmTypeReference DeclaredType
		{
			get
			{
				return this.declaredType;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00018710 File Offset: 0x00016910
		public IEnumerable<IEdmExpression> Elements
		{
			get
			{
				return this.elements;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00018718 File Offset: 0x00016918
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Collection;
			}
		}

		// Token: 0x0400046E RID: 1134
		private readonly IEdmTypeReference declaredType;

		// Token: 0x0400046F RID: 1135
		private readonly IEnumerable<IEdmExpression> elements;
	}
}

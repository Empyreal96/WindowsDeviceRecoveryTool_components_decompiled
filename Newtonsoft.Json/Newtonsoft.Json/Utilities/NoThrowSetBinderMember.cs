using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000EA RID: 234
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x0002D5ED File Offset: 0x0002B7ED
		public NoThrowSetBinderMember(SetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0002D608 File Offset: 0x0002B808
		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[]
			{
				value
			});
			NoThrowExpressionVisitor noThrowExpressionVisitor = new NoThrowExpressionVisitor();
			Expression expression = noThrowExpressionVisitor.Visit(dynamicMetaObject.Expression);
			return new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
		}

		// Token: 0x04000407 RID: 1031
		private readonly SetMemberBinder _innerBinder;
	}
}

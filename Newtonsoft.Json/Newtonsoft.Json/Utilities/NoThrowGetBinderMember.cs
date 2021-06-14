using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000E9 RID: 233
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		// Token: 0x06000B31 RID: 2865 RVA: 0x0002D58F File Offset: 0x0002B78F
		public NoThrowGetBinderMember(GetMemberBinder innerBinder) : base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			this._innerBinder = innerBinder;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0002D5AC File Offset: 0x0002B7AC
		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = this._innerBinder.Bind(target, new DynamicMetaObject[0]);
			NoThrowExpressionVisitor noThrowExpressionVisitor = new NoThrowExpressionVisitor();
			Expression expression = noThrowExpressionVisitor.Visit(dynamicMetaObject.Expression);
			return new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
		}

		// Token: 0x04000406 RID: 1030
		private readonly GetMemberBinder _innerBinder;
	}
}

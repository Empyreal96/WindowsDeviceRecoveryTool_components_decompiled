using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000E2 RID: 226
	internal sealed class DynamicProxyMetaObject<T> : DynamicMetaObject
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002BE7D File Offset: 0x0002A07D
		internal DynamicProxyMetaObject(Expression expression, T value, DynamicProxy<T> proxy, bool dontFallbackFirst) : base(expression, BindingRestrictions.Empty, value)
		{
			this._proxy = proxy;
			this._dontFallbackFirst = dontFallbackFirst;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0002BEA0 File Offset: 0x0002A0A0
		private new T Value
		{
			get
			{
				return (T)((object)base.Value);
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0002BEAD File Offset: 0x0002A0AD
		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(this._proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0002BEE8 File Offset: 0x0002A0E8
		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!this.IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return this.CallMethodWithResult("TryGetMember", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackGetMember(this, e), null);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002BF68 File Offset: 0x0002A168
		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return this.CallMethodReturnLast("TrySetMember", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[]
			{
				value
			}), (DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0002C000 File Offset: 0x0002A200
		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!this.IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return this.CallMethodNoResult("TryDeleteMember", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0002C07C File Offset: 0x0002A27C
		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!this.IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return this.CallMethodWithResult("TryConvert", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackConvert(this, e), null);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0002C114 File Offset: 0x0002A314
		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			DynamicProxyMetaObject<T>.Fallback fallback = (DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			DynamicMetaObject dynamicMetaObject = this.BuildCallMethodWithResult("TryInvokeMember", binder, DynamicProxyMetaObject<T>.GetArgArray(args), this.BuildCallMethodWithResult("TryGetMember", new DynamicProxyMetaObject<T>.GetBinderAdapter(binder), DynamicProxyMetaObject<T>.NoArgs, fallback(null), (DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
			if (!this._dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0002C1E8 File Offset: 0x0002A3E8
		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return this.CallMethodWithResult("TryCreateInstance", binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e), null);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0002C27C File Offset: 0x0002A47C
		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!this.IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return this.CallMethodWithResult("TryInvoke", binder, DynamicProxyMetaObject<T>.GetArgArray(args), (DynamicMetaObject e) => binder.FallbackInvoke(this, args, e), null);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0002C310 File Offset: 0x0002A510
		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!this.IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return this.CallMethodWithResult("TryBinaryOperation", binder, DynamicProxyMetaObject<T>.GetArgs(new DynamicMetaObject[]
			{
				arg
			}), (DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e), null);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002C3A8 File Offset: 0x0002A5A8
		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!this.IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return this.CallMethodWithResult("TryUnaryOperation", binder, DynamicProxyMetaObject<T>.NoArgs, (DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e), null);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0002C428 File Offset: 0x0002A628
		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return this.CallMethodWithResult("TryGetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e), null);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002C4C4 File Offset: 0x0002A6C4
		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!this.IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return this.CallMethodReturnLast("TrySetIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes, value), (DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002C56C File Offset: 0x0002A76C
		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!this.IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return this.CallMethodNoResult("TryDeleteIndex", binder, DynamicProxyMetaObject<T>.GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002C5F3 File Offset: 0x0002A7F3
		private static Expression[] GetArgs(params DynamicMetaObject[] args)
		{
			return (from arg in args
			select Expression.Convert(arg.Expression, typeof(object))).ToArray<UnaryExpression>();
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0002C620 File Offset: 0x0002A820
		private static Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[]
			{
				Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args))
			};
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0002C650 File Offset: 0x0002A850
		private static Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			return new Expression[]
			{
				Expression.NewArrayInit(typeof(object), DynamicProxyMetaObject<T>.GetArgs(args)),
				Expression.Convert(value.Expression, typeof(object))
			};
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x0002C698 File Offset: 0x0002A898
		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return Expression.Constant(binder, type);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x0002C6C4 File Offset: 0x0002A8C4
		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicProxyMetaObject<T>.Fallback fallback, DynamicProxyMetaObject<T>.Fallback fallbackInvoke = null)
		{
			DynamicMetaObject fallbackResult = fallback(null);
			DynamicMetaObject dynamicMetaObject = this.BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
			if (!this._dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002C6FC File Offset: 0x0002A8FC
		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicMetaObject fallbackResult, DynamicProxyMetaObject<T>.Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			list.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				UnaryExpression expression = Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType);
				dynamicMetaObject = new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			return new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType)
			}), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0002C828 File Offset: 0x0002AA28
		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			list[args.Length + 1] = Expression.Assign(parameterExpression, list[args.Length + 1]);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(Expression.Block(new ParameterExpression[]
			{
				parameterExpression
			}, new Expression[]
			{
				Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), parameterExpression, dynamicMetaObject.Expression, typeof(object))
			}), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!this._dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0002C924 File Offset: 0x0002AB24
		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicProxyMetaObject<T>.Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<Expression> list = new List<Expression>();
			list.Add(Expression.Convert(base.Expression, typeof(T)));
			list.Add(DynamicProxyMetaObject<T>.Constant(binder));
			list.AddRange(args);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(Expression.Condition(Expression.Call(Expression.Constant(this._proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), this.GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!this._dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002C9D2 File Offset: 0x0002ABD2
		private BindingRestrictions GetRestrictions()
		{
			if (this.Value != null || !base.HasValue)
			{
				return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
			}
			return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0002CA07 File Offset: 0x0002AC07
		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return this._proxy.GetDynamicMemberNames(this.Value);
		}

		// Token: 0x040003F7 RID: 1015
		private readonly DynamicProxy<T> _proxy;

		// Token: 0x040003F8 RID: 1016
		private readonly bool _dontFallbackFirst;

		// Token: 0x040003F9 RID: 1017
		private static readonly Expression[] NoArgs = new Expression[0];

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x06000B0C RID: 2828
		private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

		// Token: 0x020000E4 RID: 228
		private sealed class GetBinderAdapter : GetMemberBinder
		{
			// Token: 0x06000B0F RID: 2831 RVA: 0x0002CA27 File Offset: 0x0002AC27
			internal GetBinderAdapter(InvokeMemberBinder binder) : base(binder.Name, binder.IgnoreCase)
			{
			}

			// Token: 0x06000B10 RID: 2832 RVA: 0x0002CA3B File Offset: 0x0002AC3B
			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}
	}
}

using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000B3 RID: 179
	internal static class OpenTypeMethods
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x0000E442 File Offset: 0x0000C642
		public static object GetValue(object value, string propertyName)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000E449 File Offset: 0x0000C649
		public static object Add(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000E450 File Offset: 0x0000C650
		public static object AndAlso(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000E457 File Offset: 0x0000C657
		public static object Divide(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000E45E File Offset: 0x0000C65E
		public static object Equal(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000E465 File Offset: 0x0000C665
		public static object GreaterThan(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000E46C File Offset: 0x0000C66C
		public static object GreaterThanOrEqual(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000E473 File Offset: 0x0000C673
		public static object LessThan(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000E47A File Offset: 0x0000C67A
		public static object LessThanOrEqual(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000E481 File Offset: 0x0000C681
		public static object Modulo(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000E488 File Offset: 0x0000C688
		public static object Multiply(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000E48F File Offset: 0x0000C68F
		public static object NotEqual(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000E496 File Offset: 0x0000C696
		public static object OrElse(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000E49D File Offset: 0x0000C69D
		public static object Subtract(object left, object right)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
		public static object Negate(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000E4AB File Offset: 0x0000C6AB
		public static object Not(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000E4B2 File Offset: 0x0000C6B2
		public static object Convert(object value, IEdmTypeReference typeReference)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000E4B9 File Offset: 0x0000C6B9
		public static object TypeIs(object value, IEdmTypeReference typeReference)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000E4C0 File Offset: 0x0000C6C0
		public static object Concat(object first, object second)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000E4C7 File Offset: 0x0000C6C7
		public static object EndsWith(object targetString, object substring)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000E4CE File Offset: 0x0000C6CE
		public static object IndexOf(object targetString, object substring)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000E4D5 File Offset: 0x0000C6D5
		public static object Length(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		public static object Replace(object targetString, object substring, object newString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000E4E3 File Offset: 0x0000C6E3
		public static object StartsWith(object targetString, object substring)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000E4EA File Offset: 0x0000C6EA
		public static object Substring(object targetString, object startIndex)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000E4F1 File Offset: 0x0000C6F1
		public static object Substring(object targetString, object startIndex, object length)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public static object SubstringOf(object substring, object targetString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000E4FF File Offset: 0x0000C6FF
		public static object ToLower(object targetString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000E506 File Offset: 0x0000C706
		public static object ToUpper(object targetString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000E50D File Offset: 0x0000C70D
		public static object Trim(object targetString)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000E514 File Offset: 0x0000C714
		public static object Year(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000E51B File Offset: 0x0000C71B
		public static object Month(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000E522 File Offset: 0x0000C722
		public static object Day(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000E529 File Offset: 0x0000C729
		public static object Hour(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000E530 File Offset: 0x0000C730
		public static object Minute(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000E537 File Offset: 0x0000C737
		public static object Second(object dateTime)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000E53E File Offset: 0x0000C73E
		public static object Ceiling(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000E545 File Offset: 0x0000C745
		public static object Floor(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000E54C File Offset: 0x0000C74C
		public static object Round(object value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000E553 File Offset: 0x0000C753
		internal static Expression AddExpression(Expression left, Expression right)
		{
			return Expression.Add(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), OpenTypeMethods.AddMethodInfo);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000E56B File Offset: 0x0000C76B
		internal static Expression AndAlsoExpression(Expression left, Expression right)
		{
			return Expression.Call(OpenTypeMethods.AndAlsoMethodInfo, OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right));
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000E583 File Offset: 0x0000C783
		internal static Expression DivideExpression(Expression left, Expression right)
		{
			return Expression.Divide(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), OpenTypeMethods.DivideMethodInfo);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000E59B File Offset: 0x0000C79B
		internal static Expression EqualExpression(Expression left, Expression right)
		{
			return Expression.Equal(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.EqualMethodInfo);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		internal static Expression GreaterThanExpression(Expression left, Expression right)
		{
			return Expression.GreaterThan(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.GreaterThanMethodInfo);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000E5CD File Offset: 0x0000C7CD
		internal static Expression GreaterThanOrEqualExpression(Expression left, Expression right)
		{
			return Expression.GreaterThanOrEqual(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.GreaterThanOrEqualMethodInfo);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000E5E6 File Offset: 0x0000C7E6
		internal static Expression LessThanExpression(Expression left, Expression right)
		{
			return Expression.LessThan(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.LessThanMethodInfo);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000E5FF File Offset: 0x0000C7FF
		internal static Expression LessThanOrEqualExpression(Expression left, Expression right)
		{
			return Expression.LessThanOrEqual(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.LessThanOrEqualMethodInfo);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000E618 File Offset: 0x0000C818
		internal static Expression ModuloExpression(Expression left, Expression right)
		{
			return Expression.Modulo(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), OpenTypeMethods.ModuloMethodInfo);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000E630 File Offset: 0x0000C830
		internal static Expression MultiplyExpression(Expression left, Expression right)
		{
			return Expression.Multiply(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), OpenTypeMethods.MultiplyMethodInfo);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000E648 File Offset: 0x0000C848
		internal static Expression OrElseExpression(Expression left, Expression right)
		{
			return Expression.Call(OpenTypeMethods.OrElseMethodInfo, OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right));
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000E660 File Offset: 0x0000C860
		internal static Expression NotEqualExpression(Expression left, Expression right)
		{
			return Expression.NotEqual(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), false, OpenTypeMethods.NotEqualMethodInfo);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000E679 File Offset: 0x0000C879
		internal static Expression SubtractExpression(Expression left, Expression right)
		{
			return Expression.Subtract(OpenTypeMethods.ExpressionAsObject(left), OpenTypeMethods.ExpressionAsObject(right), OpenTypeMethods.SubtractMethodInfo);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000E691 File Offset: 0x0000C891
		internal static Expression NegateExpression(Expression expression)
		{
			return Expression.Negate(OpenTypeMethods.ExpressionAsObject(expression), OpenTypeMethods.NegateMethodInfo);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000E6A3 File Offset: 0x0000C8A3
		internal static Expression NotExpression(Expression expression)
		{
			return Expression.Not(OpenTypeMethods.ExpressionAsObject(expression), OpenTypeMethods.NotMethodInfo);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000E6B5 File Offset: 0x0000C8B5
		private static Expression ExpressionAsObject(Expression expression)
		{
			if (!expression.Type.IsValueType())
			{
				return expression;
			}
			return Expression.Convert(expression, typeof(object));
		}

		// Token: 0x0400016B RID: 363
		internal static readonly MethodInfo AddMethodInfo = typeof(OpenTypeMethods).GetMethod("Add", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400016C RID: 364
		internal static readonly MethodInfo AndAlsoMethodInfo = typeof(OpenTypeMethods).GetMethod("AndAlso", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400016D RID: 365
		internal static readonly MethodInfo ConvertMethodInfo = typeof(OpenTypeMethods).GetMethod("Convert", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400016E RID: 366
		internal static readonly MethodInfo DivideMethodInfo = typeof(OpenTypeMethods).GetMethod("Divide", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400016F RID: 367
		internal static readonly MethodInfo EqualMethodInfo = typeof(OpenTypeMethods).GetMethod("Equal", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000170 RID: 368
		internal static readonly MethodInfo GreaterThanMethodInfo = typeof(OpenTypeMethods).GetMethod("GreaterThan", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000171 RID: 369
		internal static readonly MethodInfo GreaterThanOrEqualMethodInfo = typeof(OpenTypeMethods).GetMethod("GreaterThanOrEqual", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000172 RID: 370
		internal static readonly MethodInfo LessThanMethodInfo = typeof(OpenTypeMethods).GetMethod("LessThan", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000173 RID: 371
		internal static readonly MethodInfo LessThanOrEqualMethodInfo = typeof(OpenTypeMethods).GetMethod("LessThanOrEqual", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000174 RID: 372
		internal static readonly MethodInfo ModuloMethodInfo = typeof(OpenTypeMethods).GetMethod("Modulo", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000175 RID: 373
		internal static readonly MethodInfo MultiplyMethodInfo = typeof(OpenTypeMethods).GetMethod("Multiply", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000176 RID: 374
		internal static readonly MethodInfo NegateMethodInfo = typeof(OpenTypeMethods).GetMethod("Negate", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000177 RID: 375
		internal static readonly MethodInfo NotMethodInfo = typeof(OpenTypeMethods).GetMethod("Not", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000178 RID: 376
		internal static readonly MethodInfo NotEqualMethodInfo = typeof(OpenTypeMethods).GetMethod("NotEqual", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04000179 RID: 377
		internal static readonly MethodInfo OrElseMethodInfo = typeof(OpenTypeMethods).GetMethod("OrElse", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400017A RID: 378
		internal static readonly MethodInfo SubtractMethodInfo = typeof(OpenTypeMethods).GetMethod("Subtract", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400017B RID: 379
		internal static readonly MethodInfo TypeIsMethodInfo = typeof(OpenTypeMethods).GetMethod("TypeIs", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400017C RID: 380
		internal static readonly MethodInfo GetValueOpenPropertyMethodInfo = typeof(OpenTypeMethods).GetMethod("GetValue", new Type[]
		{
			typeof(object),
			typeof(string)
		}, true, true);
	}
}

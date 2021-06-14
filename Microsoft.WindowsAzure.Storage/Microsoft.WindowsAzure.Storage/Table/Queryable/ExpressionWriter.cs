using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000116 RID: 278
	internal class ExpressionWriter : DataServiceALinqExpressionVisitor
	{
		// Token: 0x0600131D RID: 4893 RVA: 0x000477C9 File Offset: 0x000459C9
		private ExpressionWriter()
		{
			this.builder = new StringBuilder();
			this.expressionStack = new Stack<Expression>();
			this.expressionStack.Push(null);
		}

		// Token: 0x0600131E RID: 4894 RVA: 0x000477F4 File Offset: 0x000459F4
		internal static string ExpressionToString(Expression e)
		{
			ExpressionWriter expressionWriter = new ExpressionWriter();
			string result = expressionWriter.Translate(e);
			if (expressionWriter.cantTranslateExpression)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression {0} is not supported.", new object[]
				{
					e.ToString()
				}));
			}
			return result;
		}

		// Token: 0x0600131F RID: 4895 RVA: 0x00047840 File Offset: 0x00045A40
		internal override Expression Visit(Expression exp)
		{
			this.parent = this.expressionStack.Peek();
			this.expressionStack.Push(exp);
			Expression result = base.Visit(exp);
			this.expressionStack.Pop();
			return result;
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x0004787F File Offset: 0x00045A7F
		internal override Expression VisitConditional(ConditionalExpression c)
		{
			this.cantTranslateExpression = true;
			return c;
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00047889 File Offset: 0x00045A89
		internal override Expression VisitLambda(LambdaExpression lambda)
		{
			this.cantTranslateExpression = true;
			return lambda;
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00047893 File Offset: 0x00045A93
		internal override NewExpression VisitNew(NewExpression nex)
		{
			this.cantTranslateExpression = true;
			return nex;
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0004789D File Offset: 0x00045A9D
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			this.cantTranslateExpression = true;
			return init;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000478A7 File Offset: 0x00045AA7
		internal override Expression VisitListInit(ListInitExpression init)
		{
			this.cantTranslateExpression = true;
			return init;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x000478B1 File Offset: 0x00045AB1
		internal override Expression VisitNewArray(NewArrayExpression na)
		{
			this.cantTranslateExpression = true;
			return na;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x000478BB File Offset: 0x00045ABB
		internal override Expression VisitInvocation(InvocationExpression iv)
		{
			this.cantTranslateExpression = true;
			return iv;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x000478C8 File Offset: 0x00045AC8
		internal override Expression VisitInputReferenceExpression(InputReferenceExpression ire)
		{
			if (this.parent == null || this.parent.NodeType != ExpressionType.MemberAccess)
			{
				string text = (this.parent != null) ? this.parent.ToString() : ire.ToString();
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression {0} is not supported.", new object[]
				{
					text
				}));
			}
			return ire;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0004792C File Offset: 0x00045B2C
		internal override Expression VisitMethodCall(MethodCallExpression m)
		{
			string text;
			if (TypeSystem.TryGetQueryOptionMethod(m.Method, out text))
			{
				this.builder.Append(text);
				this.builder.Append('(');
				if (text == "substringof")
				{
					this.Visit(m.Arguments[0]);
					this.builder.Append(',');
					this.Visit(m.Object);
				}
				else
				{
					if (m.Object != null)
					{
						this.Visit(m.Object);
					}
					if (m.Arguments.Count > 0)
					{
						if (m.Object != null)
						{
							this.builder.Append(',');
						}
						for (int i = 0; i < m.Arguments.Count; i++)
						{
							this.Visit(m.Arguments[i]);
							if (i < m.Arguments.Count - 1)
							{
								this.builder.Append(',');
							}
						}
					}
				}
				this.builder.Append(')');
			}
			else
			{
				this.cantTranslateExpression = true;
			}
			return m;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00047A40 File Offset: 0x00045C40
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m.Member is FieldInfo)
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Referencing public field '{0}' not supported in query option expression.  Use public property instead.", new object[]
				{
					m.Member.Name
				}));
			}
			if (m.Member.DeclaringType == typeof(EntityProperty))
			{
				MethodCallExpression methodCallExpression = m.Expression as MethodCallExpression;
				if (methodCallExpression != null && methodCallExpression.Arguments.Count == 1 && methodCallExpression.Method == ReflectionUtil.DictionaryGetItemMethodInfo)
				{
					MemberExpression memberExpression = methodCallExpression.Object as MemberExpression;
					if (memberExpression != null && memberExpression.Member.DeclaringType == typeof(DynamicTableEntity) && memberExpression.Member.Name == "Properties")
					{
						ConstantExpression constantExpression = methodCallExpression.Arguments[0] as ConstantExpression;
						if (constantExpression == null || !(constantExpression.Value is string))
						{
							throw new NotSupportedException("Accessing property dictionary of DynamicTableEntity requires a string constant for property name.");
						}
						this.builder.Append((string)constantExpression.Value);
						return constantExpression;
					}
				}
				throw new NotSupportedException("Referencing {0} on EntityProperty only supported with properties dictionary exposed via DynamicTableEntity.");
			}
			Expression expression = this.Visit(m.Expression);
			if (m.Member.Name == "Value" && m.Member.DeclaringType.IsGenericType && m.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return m;
			}
			if (!ExpressionWriter.IsInputReference(expression) && expression.NodeType != ExpressionType.Convert && expression.NodeType != ExpressionType.ConvertChecked)
			{
				this.builder.Append('/');
			}
			this.builder.Append(m.Member.Name);
			return m;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00047C10 File Offset: 0x00045E10
		internal override Expression VisitConstant(ConstantExpression c)
		{
			string value = null;
			if (c.Value == null)
			{
				this.builder.Append("null");
				return c;
			}
			if (!ClientConvert.TryKeyPrimitiveToString(c.Value, out value))
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Could not convert constant {0} expression to string.", new object[]
				{
					c.Value
				}));
			}
			this.builder.Append(value);
			return c;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x00047C80 File Offset: 0x00045E80
		internal override Expression VisitUnary(UnaryExpression u)
		{
			ExpressionType nodeType = u.NodeType;
			switch (nodeType)
			{
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
				break;
			default:
				switch (nodeType)
				{
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					this.builder.Append(' ');
					this.builder.Append("-");
					this.VisitOperand(u.Operand);
					return u;
				case ExpressionType.UnaryPlus:
					return u;
				case ExpressionType.Not:
					this.builder.Append("not");
					this.builder.Append(' ');
					this.VisitOperand(u.Operand);
					return u;
				}
				this.cantTranslateExpression = true;
				break;
			}
			return u;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00047D30 File Offset: 0x00045F30
		internal override Expression VisitBinary(BinaryExpression b)
		{
			this.VisitOperand(b.Left);
			this.builder.Append(' ');
			switch (b.NodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
				this.builder.Append("add");
				goto IL_1EC;
			case ExpressionType.And:
			case ExpressionType.AndAlso:
				this.builder.Append("and");
				goto IL_1EC;
			case ExpressionType.Divide:
				this.builder.Append("div");
				goto IL_1EC;
			case ExpressionType.Equal:
				this.builder.Append("eq");
				goto IL_1EC;
			case ExpressionType.GreaterThan:
				this.builder.Append("gt");
				goto IL_1EC;
			case ExpressionType.GreaterThanOrEqual:
				this.builder.Append("ge");
				goto IL_1EC;
			case ExpressionType.LessThan:
				this.builder.Append("lt");
				goto IL_1EC;
			case ExpressionType.LessThanOrEqual:
				this.builder.Append("le");
				goto IL_1EC;
			case ExpressionType.Modulo:
				this.builder.Append("mod");
				goto IL_1EC;
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
				this.builder.Append("mul");
				goto IL_1EC;
			case ExpressionType.NotEqual:
				this.builder.Append("ne");
				goto IL_1EC;
			case ExpressionType.Or:
			case ExpressionType.OrElse:
				this.builder.Append("or");
				goto IL_1EC;
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				this.builder.Append("sub");
				goto IL_1EC;
			}
			this.cantTranslateExpression = true;
			IL_1EC:
			this.builder.Append(' ');
			this.VisitOperand(b.Right);
			return b;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x00047F44 File Offset: 0x00046144
		internal override Expression VisitTypeIs(TypeBinaryExpression b)
		{
			this.builder.Append("isof");
			this.builder.Append('(');
			if (!ExpressionWriter.IsInputReference(b.Expression))
			{
				this.Visit(b.Expression);
				this.builder.Append(',');
				this.builder.Append(' ');
			}
			this.builder.Append('\'');
			this.builder.Append(this.TypeNameForUri(b.TypeOperand));
			this.builder.Append('\'');
			this.builder.Append(')');
			return b;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x00047FE9 File Offset: 0x000461E9
		internal override Expression VisitParameter(ParameterExpression p)
		{
			return p;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x00047FEC File Offset: 0x000461EC
		private static bool IsInputReference(Expression exp)
		{
			return exp is InputReferenceExpression || exp is ParameterExpression;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x00048004 File Offset: 0x00046204
		private string TypeNameForUri(Type type)
		{
			type = (Nullable.GetUnderlyingType(type) ?? type);
			if (!ClientConvert.IsKnownType(type))
			{
				return null;
			}
			if (ClientConvert.IsSupportedPrimitiveTypeForUri(type))
			{
				return ClientConvert.ToTypeName(type);
			}
			throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Can't cast to unsupported type '{0}'", new object[]
			{
				type.Name
			}));
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0004805C File Offset: 0x0004625C
		private void VisitOperand(Expression e)
		{
			if (e is BinaryExpression || e is UnaryExpression)
			{
				this.builder.Append('(');
				this.Visit(e);
				this.builder.Append(')');
				return;
			}
			this.Visit(e);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0004809B File Offset: 0x0004629B
		private string Translate(Expression e)
		{
			this.Visit(e);
			return this.builder.ToString();
		}

		// Token: 0x0400059A RID: 1434
		private readonly StringBuilder builder;

		// Token: 0x0400059B RID: 1435
		private readonly Stack<Expression> expressionStack;

		// Token: 0x0400059C RID: 1436
		private bool cantTranslateExpression;

		// Token: 0x0400059D RID: 1437
		private Expression parent;
	}
}

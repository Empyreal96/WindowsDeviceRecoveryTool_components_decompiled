using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x020000DD RID: 221
	internal class ExpressionWriter : DataServiceALinqExpressionVisitor
	{
		// Token: 0x0600070E RID: 1806 RVA: 0x0001DE1C File Offset: 0x0001C01C
		private ExpressionWriter(DataServiceContext context, bool inPath)
		{
			this.context = context;
			this.inPath = inPath;
			this.builder = new StringBuilder();
			this.expressionStack = new Stack<Expression>();
			this.expressionStack.Push(null);
			this.uriVersion = Util.DataServiceVersion1;
			this.scopeCount = 0;
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001DE71 File Offset: 0x0001C071
		private bool InSubScope
		{
			get
			{
				return this.scopeCount > 0;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001DE7C File Offset: 0x0001C07C
		internal static string ExpressionToString(DataServiceContext context, Expression e, bool inPath, ref Version uriVersion)
		{
			ExpressionWriter expressionWriter = new ExpressionWriter(context, inPath);
			string result = expressionWriter.Translate(e);
			WebUtil.RaiseVersion(ref uriVersion, expressionWriter.uriVersion);
			if (expressionWriter.cantTranslateExpression)
			{
				throw new NotSupportedException(Strings.ALinq_CantTranslateExpression(e.ToString()));
			}
			return result;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
		internal override Expression Visit(Expression exp)
		{
			this.parent = this.expressionStack.Peek();
			this.expressionStack.Push(exp);
			Expression result = base.Visit(exp);
			this.expressionStack.Pop();
			return result;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001DEFF File Offset: 0x0001C0FF
		internal override Expression VisitConditional(ConditionalExpression c)
		{
			this.cantTranslateExpression = true;
			return c;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001DF09 File Offset: 0x0001C109
		internal override Expression VisitLambda(LambdaExpression lambda)
		{
			this.cantTranslateExpression = true;
			return lambda;
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001DF13 File Offset: 0x0001C113
		internal override NewExpression VisitNew(NewExpression nex)
		{
			this.cantTranslateExpression = true;
			return nex;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001DF1D File Offset: 0x0001C11D
		internal override Expression VisitMemberInit(MemberInitExpression init)
		{
			this.cantTranslateExpression = true;
			return init;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001DF27 File Offset: 0x0001C127
		internal override Expression VisitListInit(ListInitExpression init)
		{
			this.cantTranslateExpression = true;
			return init;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001DF31 File Offset: 0x0001C131
		internal override Expression VisitNewArray(NewArrayExpression na)
		{
			this.cantTranslateExpression = true;
			return na;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001DF3B File Offset: 0x0001C13B
		internal override Expression VisitInvocation(InvocationExpression iv)
		{
			this.cantTranslateExpression = true;
			return iv;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001DF48 File Offset: 0x0001C148
		internal override Expression VisitInputReferenceExpression(InputReferenceExpression ire)
		{
			if (this.parent == null || (!this.InSubScope && this.parent.NodeType != ExpressionType.MemberAccess && this.parent.NodeType != ExpressionType.TypeAs))
			{
				string p = (this.parent != null) ? this.parent.ToString() : ire.ToString();
				throw new NotSupportedException(Strings.ALinq_CantTranslateExpression(p));
			}
			if (this.InSubScope)
			{
				this.builder.Append("$it");
			}
			return ire;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001DFC8 File Offset: 0x0001C1C8
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
				SequenceMethod sequenceMethod;
				if (ReflectionUtil.TryIdentifySequenceMethod(m.Method, out sequenceMethod))
				{
					if (ReflectionUtil.IsAnyAllMethod(sequenceMethod))
					{
						WebUtil.RaiseVersion(ref this.uriVersion, Util.DataServiceVersion3);
						this.Visit(m.Arguments[0]);
						this.builder.Append('/');
						if (sequenceMethod == SequenceMethod.All)
						{
							this.builder.Append("all");
						}
						else
						{
							this.builder.Append("any");
						}
						this.builder.Append('(');
						if (sequenceMethod != SequenceMethod.Any)
						{
							LambdaExpression lambdaExpression = (LambdaExpression)m.Arguments[1];
							string name = lambdaExpression.Parameters[0].Name;
							this.builder.Append(name);
							this.builder.Append(':');
							this.scopeCount++;
							this.Visit(lambdaExpression.Body);
							this.scopeCount--;
						}
						this.builder.Append(')');
						return m;
					}
					if (sequenceMethod == SequenceMethod.OfType && this.parent != null)
					{
						MethodCallExpression methodCallExpression = this.parent as MethodCallExpression;
						if (methodCallExpression != null && ReflectionUtil.TryIdentifySequenceMethod(methodCallExpression.Method, out sequenceMethod) && ReflectionUtil.IsAnyAllMethod(sequenceMethod))
						{
							Type type = methodCallExpression.Method.GetGenericArguments().SingleOrDefault<Type>();
							if (ClientTypeUtil.TypeOrElementTypeIsEntity(type))
							{
								this.Visit(m.Arguments[0]);
								this.builder.Append('/');
								UriHelper.AppendTypeSegment(this.builder, type, this.context, this.inPath, ref this.uriVersion);
								return m;
							}
						}
					}
				}
				this.cantTranslateExpression = true;
			}
			return m;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001E27C File Offset: 0x0001C47C
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (m.Member is FieldInfo)
			{
				throw new NotSupportedException(Strings.ALinq_CantReferToPublicField(m.Member.Name));
			}
			Expression exp = this.Visit(m.Expression);
			if (m.Member.Name == "Value" && m.Member.DeclaringType.IsGenericType() && m.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return m;
			}
			if (!this.IsImplicitInputReference(exp))
			{
				this.builder.Append('/');
			}
			this.builder.Append(m.Member.Name);
			return m;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001E334 File Offset: 0x0001C534
		internal override Expression VisitConstant(ConstantExpression c)
		{
			if (c.Value == null)
			{
				this.builder.Append("null");
				return c;
			}
			string value;
			try
			{
				value = LiteralFormatter.ForConstants.Format(c.Value);
			}
			catch (InvalidOperationException)
			{
				if (this.cantTranslateExpression)
				{
					return c;
				}
				throw new NotSupportedException(Strings.ALinq_CouldNotConvert(c.Value));
			}
			this.builder.Append(value);
			return c;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001E3B0 File Offset: 0x0001C5B0
		internal override Expression VisitUnary(UnaryExpression u)
		{
			ExpressionType nodeType = u.NodeType;
			switch (nodeType)
			{
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
				if (u.Type != typeof(object))
				{
					this.builder.Append("cast");
					this.builder.Append('(');
					if (!this.IsImplicitInputReference(u.Operand))
					{
						this.Visit(u.Operand);
						this.builder.Append(',');
					}
					this.builder.Append('\'');
					this.builder.Append(UriHelper.GetTypeNameForUri(u.Type, this.context));
					this.builder.Append('\'');
					this.builder.Append(')');
				}
				else if (!this.IsImplicitInputReference(u.Operand))
				{
					this.Visit(u.Operand);
				}
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
				case ExpressionType.New:
				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
					break;
				case ExpressionType.Not:
					this.builder.Append("not");
					this.builder.Append(' ');
					this.VisitOperand(u.Operand);
					return u;
				default:
					if (nodeType == ExpressionType.TypeAs)
					{
						if (u.Operand.NodeType == ExpressionType.TypeAs)
						{
							throw new NotSupportedException(Strings.ALinq_CannotUseTypeFiltersMultipleTimes);
						}
						this.Visit(u.Operand);
						if (!this.IsImplicitInputReference(u.Operand))
						{
							this.builder.Append('/');
						}
						UriHelper.AppendTypeSegment(this.builder, u.Type, this.context, this.inPath, ref this.uriVersion);
						return u;
					}
					break;
				}
				this.cantTranslateExpression = true;
				break;
			}
			return u;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001E5A4 File Offset: 0x0001C7A4
		internal override Expression VisitBinary(BinaryExpression b)
		{
			this.VisitOperand(b.Left, new ExpressionType?(b.NodeType), new ExpressionWriter.ChildDirection?(ExpressionWriter.ChildDirection.Left));
			this.builder.Append(' ');
			switch (b.NodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
				this.builder.Append("add");
				goto IL_1FD;
			case ExpressionType.And:
			case ExpressionType.AndAlso:
				this.builder.Append("and");
				goto IL_1FD;
			case ExpressionType.Divide:
				this.builder.Append("div");
				goto IL_1FD;
			case ExpressionType.Equal:
				this.builder.Append("eq");
				goto IL_1FD;
			case ExpressionType.GreaterThan:
				this.builder.Append("gt");
				goto IL_1FD;
			case ExpressionType.GreaterThanOrEqual:
				this.builder.Append("ge");
				goto IL_1FD;
			case ExpressionType.LessThan:
				this.builder.Append("lt");
				goto IL_1FD;
			case ExpressionType.LessThanOrEqual:
				this.builder.Append("le");
				goto IL_1FD;
			case ExpressionType.Modulo:
				this.builder.Append("mod");
				goto IL_1FD;
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
				this.builder.Append("mul");
				goto IL_1FD;
			case ExpressionType.NotEqual:
				this.builder.Append("ne");
				goto IL_1FD;
			case ExpressionType.Or:
			case ExpressionType.OrElse:
				this.builder.Append("or");
				goto IL_1FD;
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				this.builder.Append("sub");
				goto IL_1FD;
			}
			this.cantTranslateExpression = true;
			IL_1FD:
			this.builder.Append(' ');
			this.VisitOperand(b.Right, new ExpressionType?(b.NodeType), new ExpressionWriter.ChildDirection?(ExpressionWriter.ChildDirection.Right));
			return b;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001E7DC File Offset: 0x0001C9DC
		internal override Expression VisitTypeIs(TypeBinaryExpression b)
		{
			this.builder.Append("isof");
			this.builder.Append('(');
			if (!this.IsImplicitInputReference(b.Expression))
			{
				this.Visit(b.Expression);
				this.builder.Append(',');
				this.builder.Append(' ');
			}
			this.builder.Append('\'');
			this.builder.Append(UriHelper.GetTypeNameForUri(b.TypeOperand, this.context));
			this.builder.Append('\'');
			this.builder.Append(')');
			return b;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001E887 File Offset: 0x0001CA87
		internal override Expression VisitParameter(ParameterExpression p)
		{
			if (this.InSubScope)
			{
				this.builder.Append(p.Name);
			}
			return p;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		private static bool AreExpressionTypesCollapsible(ExpressionType type, ExpressionType parentType, ExpressionWriter.ChildDirection childDirection)
		{
			int num = ExpressionWriter.BinaryPrecedence(type);
			int num2 = ExpressionWriter.BinaryPrecedence(parentType);
			if (num >= 0 && num2 >= 0)
			{
				if (childDirection == ExpressionWriter.ChildDirection.Left)
				{
					if (num <= num2)
					{
						return true;
					}
				}
				else if (num < num2)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		private static int BinaryPrecedence(ExpressionType type)
		{
			switch (type)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
				break;
			case ExpressionType.And:
			case ExpressionType.AndAlso:
				return 3;
			default:
				switch (type)
				{
				case ExpressionType.Divide:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
					return 0;
				case ExpressionType.Equal:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					break;
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Invoke:
				case ExpressionType.Lambda:
				case ExpressionType.LeftShift:
				case ExpressionType.ListInit:
				case ExpressionType.MemberAccess:
				case ExpressionType.MemberInit:
					return -1;
				default:
					switch (type)
					{
					case ExpressionType.NotEqual:
						break;
					case ExpressionType.Or:
					case ExpressionType.OrElse:
						return 4;
					case ExpressionType.Parameter:
					case ExpressionType.Power:
					case ExpressionType.Quote:
					case ExpressionType.RightShift:
						return -1;
					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
						return 1;
					default:
						return -1;
					}
					break;
				}
				return 2;
			}
			return 1;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001E980 File Offset: 0x0001CB80
		private void VisitOperand(Expression e)
		{
			this.VisitOperand(e, null, null);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001E9A8 File Offset: 0x0001CBA8
		private void VisitOperand(Expression e, ExpressionType? parentType, ExpressionWriter.ChildDirection? childDirection)
		{
			if (e is BinaryExpression)
			{
				bool flag = parentType == null || !ExpressionWriter.AreExpressionTypesCollapsible(e.NodeType, parentType.Value, childDirection.Value);
				if (flag)
				{
					this.builder.Append('(');
				}
				this.Visit(e);
				if (flag)
				{
					this.builder.Append(')');
					return;
				}
			}
			else
			{
				this.Visit(e);
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001EA19 File Offset: 0x0001CC19
		private string Translate(Expression e)
		{
			this.Visit(e);
			return this.builder.ToString();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001EA2E File Offset: 0x0001CC2E
		private bool IsImplicitInputReference(Expression exp)
		{
			return !this.InSubScope && (exp is InputReferenceExpression || exp is ParameterExpression);
		}

		// Token: 0x04000475 RID: 1141
		private readonly StringBuilder builder;

		// Token: 0x04000476 RID: 1142
		private readonly DataServiceContext context;

		// Token: 0x04000477 RID: 1143
		private readonly Stack<Expression> expressionStack;

		// Token: 0x04000478 RID: 1144
		private readonly bool inPath;

		// Token: 0x04000479 RID: 1145
		private bool cantTranslateExpression;

		// Token: 0x0400047A RID: 1146
		private Expression parent;

		// Token: 0x0400047B RID: 1147
		private Version uriVersion;

		// Token: 0x0400047C RID: 1148
		private int scopeCount;

		// Token: 0x020000DE RID: 222
		private enum ChildDirection
		{
			// Token: 0x0400047E RID: 1150
			Left,
			// Token: 0x0400047F RID: 1151
			Right
		}
	}
}

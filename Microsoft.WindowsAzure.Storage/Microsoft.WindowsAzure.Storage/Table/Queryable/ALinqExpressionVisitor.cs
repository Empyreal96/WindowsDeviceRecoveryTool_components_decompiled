using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000105 RID: 261
	internal abstract class ALinqExpressionVisitor
	{
		// Token: 0x0600129E RID: 4766 RVA: 0x00045098 File Offset: 0x00043298
		internal virtual Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return exp;
			}
			switch (exp.NodeType)
			{
			case ExpressionType.Add:
			case ExpressionType.AddChecked:
			case ExpressionType.And:
			case ExpressionType.AndAlso:
			case ExpressionType.ArrayIndex:
			case ExpressionType.Coalesce:
			case ExpressionType.Divide:
			case ExpressionType.Equal:
			case ExpressionType.ExclusiveOr:
			case ExpressionType.GreaterThan:
			case ExpressionType.GreaterThanOrEqual:
			case ExpressionType.LeftShift:
			case ExpressionType.LessThan:
			case ExpressionType.LessThanOrEqual:
			case ExpressionType.Modulo:
			case ExpressionType.Multiply:
			case ExpressionType.MultiplyChecked:
			case ExpressionType.NotEqual:
			case ExpressionType.Or:
			case ExpressionType.OrElse:
			case ExpressionType.Power:
			case ExpressionType.RightShift:
			case ExpressionType.Subtract:
			case ExpressionType.SubtractChecked:
				return this.VisitBinary((BinaryExpression)exp);
			case ExpressionType.ArrayLength:
			case ExpressionType.Convert:
			case ExpressionType.ConvertChecked:
			case ExpressionType.Negate:
			case ExpressionType.UnaryPlus:
			case ExpressionType.NegateChecked:
			case ExpressionType.Not:
			case ExpressionType.Quote:
			case ExpressionType.TypeAs:
				return this.VisitUnary((UnaryExpression)exp);
			case ExpressionType.Call:
				return this.VisitMethodCall((MethodCallExpression)exp);
			case ExpressionType.Conditional:
				return this.VisitConditional((ConditionalExpression)exp);
			case ExpressionType.Constant:
				return this.VisitConstant((ConstantExpression)exp);
			case ExpressionType.Invoke:
				return this.VisitInvocation((InvocationExpression)exp);
			case ExpressionType.Lambda:
				return this.VisitLambda((LambdaExpression)exp);
			case ExpressionType.ListInit:
				return this.VisitListInit((ListInitExpression)exp);
			case ExpressionType.MemberAccess:
				return this.VisitMemberAccess((MemberExpression)exp);
			case ExpressionType.MemberInit:
				return this.VisitMemberInit((MemberInitExpression)exp);
			case ExpressionType.New:
				return this.VisitNew((NewExpression)exp);
			case ExpressionType.NewArrayInit:
			case ExpressionType.NewArrayBounds:
				return this.VisitNewArray((NewArrayExpression)exp);
			case ExpressionType.Parameter:
				return this.VisitParameter((ParameterExpression)exp);
			case ExpressionType.TypeIs:
				return this.VisitTypeIs((TypeBinaryExpression)exp);
			default:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression type {0} is not supported.", new object[]
				{
					exp.NodeType.ToString()
				}));
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0004525C File Offset: 0x0004345C
		internal virtual MemberBinding VisitBinding(MemberBinding binding)
		{
			switch (binding.BindingType)
			{
			case MemberBindingType.Assignment:
				return this.VisitMemberAssignment((MemberAssignment)binding);
			case MemberBindingType.MemberBinding:
				return this.VisitMemberMemberBinding((MemberMemberBinding)binding);
			case MemberBindingType.ListBinding:
				return this.VisitMemberListBinding((MemberListBinding)binding);
			default:
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "The expression type {0} is not supported.", new object[]
				{
					binding.BindingType.ToString()
				}));
			}
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000452DC File Offset: 0x000434DC
		internal virtual ElementInit VisitElementInitializer(ElementInit initializer)
		{
			ReadOnlyCollection<Expression> readOnlyCollection = this.VisitExpressionList(initializer.Arguments);
			if (readOnlyCollection != initializer.Arguments)
			{
				return Expression.ElementInit(initializer.AddMethod, readOnlyCollection);
			}
			return initializer;
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00045310 File Offset: 0x00043510
		internal virtual Expression VisitUnary(UnaryExpression u)
		{
			Expression expression = this.Visit(u.Operand);
			if (expression != u.Operand)
			{
				return Expression.MakeUnary(u.NodeType, expression, u.Type, u.Method);
			}
			return u;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x00045350 File Offset: 0x00043550
		internal virtual Expression VisitBinary(BinaryExpression b)
		{
			Expression expression = this.Visit(b.Left);
			Expression expression2 = this.Visit(b.Right);
			Expression expression3 = this.Visit(b.Conversion);
			if (expression == b.Left && expression2 == b.Right && expression3 == b.Conversion)
			{
				return b;
			}
			if (b.NodeType == ExpressionType.Coalesce && b.Conversion != null)
			{
				return Expression.Coalesce(expression, expression2, expression3 as LambdaExpression);
			}
			return Expression.MakeBinary(b.NodeType, expression, expression2, b.IsLiftedToNull, b.Method);
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x000453DC File Offset: 0x000435DC
		internal virtual Expression VisitTypeIs(TypeBinaryExpression b)
		{
			Expression expression = this.Visit(b.Expression);
			if (expression != b.Expression)
			{
				return Expression.TypeIs(expression, b.TypeOperand);
			}
			return b;
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0004540D File Offset: 0x0004360D
		internal virtual Expression VisitConstant(ConstantExpression c)
		{
			return c;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x00045410 File Offset: 0x00043610
		internal virtual Expression VisitConditional(ConditionalExpression c)
		{
			Expression expression = this.Visit(c.Test);
			Expression expression2 = this.Visit(c.IfTrue);
			Expression expression3 = this.Visit(c.IfFalse);
			if (expression != c.Test || expression2 != c.IfTrue || expression3 != c.IfFalse)
			{
				return Expression.Condition(expression, expression2, expression3);
			}
			return c;
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00045469 File Offset: 0x00043669
		internal virtual Expression VisitParameter(ParameterExpression p)
		{
			return p;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0004546C File Offset: 0x0004366C
		internal virtual Expression VisitMemberAccess(MemberExpression m)
		{
			Expression expression = this.Visit(m.Expression);
			if (expression != m.Expression)
			{
				return Expression.MakeMemberAccess(expression, m.Member);
			}
			return m;
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x000454A0 File Offset: 0x000436A0
		internal virtual Expression VisitMethodCall(MethodCallExpression m)
		{
			Expression expression = this.Visit(m.Object);
			IEnumerable<Expression> enumerable = this.VisitExpressionList(m.Arguments);
			if (expression != m.Object || enumerable != m.Arguments)
			{
				return Expression.Call(expression, m.Method, enumerable);
			}
			return m;
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x000454E8 File Offset: 0x000436E8
		internal virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
		{
			List<Expression> list = null;
			int i = 0;
			int count = original.Count;
			while (i < count)
			{
				Expression expression = this.Visit(original[i]);
				if (list != null)
				{
					list.Add(expression);
				}
				else if (expression != original[i])
				{
					list = new List<Expression>(count);
					for (int j = 0; j < i; j++)
					{
						list.Add(original[j]);
					}
					list.Add(expression);
				}
				i++;
			}
			if (list != null)
			{
				return new ReadOnlyCollection<Expression>(list);
			}
			return original;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00045568 File Offset: 0x00043768
		internal virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
		{
			Expression expression = this.Visit(assignment.Expression);
			if (expression != assignment.Expression)
			{
				return Expression.Bind(assignment.Member, expression);
			}
			return assignment;
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0004559C File Offset: 0x0004379C
		internal virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			IEnumerable<MemberBinding> enumerable = this.VisitBindingList(binding.Bindings);
			if (enumerable != binding.Bindings)
			{
				return Expression.MemberBind(binding.Member, enumerable);
			}
			return binding;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x000455D0 File Offset: 0x000437D0
		internal virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
		{
			IEnumerable<ElementInit> enumerable = this.VisitElementInitializerList(binding.Initializers);
			if (enumerable != binding.Initializers)
			{
				return Expression.ListBind(binding.Member, enumerable);
			}
			return binding;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00045604 File Offset: 0x00043804
		internal virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
		{
			List<MemberBinding> list = null;
			int i = 0;
			int count = original.Count;
			while (i < count)
			{
				MemberBinding memberBinding = this.VisitBinding(original[i]);
				if (list != null)
				{
					list.Add(memberBinding);
				}
				else if (memberBinding != original[i])
				{
					list = new List<MemberBinding>(count);
					for (int j = 0; j < i; j++)
					{
						list.Add(original[j]);
					}
					list.Add(memberBinding);
				}
				i++;
			}
			if (list != null)
			{
				return list;
			}
			return original;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0004567C File Offset: 0x0004387C
		internal virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
		{
			List<ElementInit> list = null;
			int i = 0;
			int count = original.Count;
			while (i < count)
			{
				ElementInit elementInit = this.VisitElementInitializer(original[i]);
				if (list != null)
				{
					list.Add(elementInit);
				}
				else if (elementInit != original[i])
				{
					list = new List<ElementInit>(count);
					for (int j = 0; j < i; j++)
					{
						list.Add(original[j]);
					}
					list.Add(elementInit);
				}
				i++;
			}
			if (list != null)
			{
				return list;
			}
			return original;
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x000456F4 File Offset: 0x000438F4
		internal virtual Expression VisitLambda(LambdaExpression lambda)
		{
			Expression expression = this.Visit(lambda.Body);
			if (expression != lambda.Body)
			{
				return Expression.Lambda(lambda.Type, expression, lambda.Parameters);
			}
			return lambda;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0004572C File Offset: 0x0004392C
		internal virtual NewExpression VisitNew(NewExpression nex)
		{
			IEnumerable<Expression> enumerable = this.VisitExpressionList(nex.Arguments);
			if (enumerable == nex.Arguments)
			{
				return nex;
			}
			if (nex.Members != null)
			{
				return Expression.New(nex.Constructor, enumerable, nex.Members);
			}
			return Expression.New(nex.Constructor, enumerable);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x00045778 File Offset: 0x00043978
		internal virtual Expression VisitMemberInit(MemberInitExpression init)
		{
			NewExpression newExpression = this.VisitNew(init.NewExpression);
			IEnumerable<MemberBinding> enumerable = this.VisitBindingList(init.Bindings);
			if (newExpression != init.NewExpression || enumerable != init.Bindings)
			{
				return Expression.MemberInit(newExpression, enumerable);
			}
			return init;
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000457BC File Offset: 0x000439BC
		internal virtual Expression VisitListInit(ListInitExpression init)
		{
			NewExpression newExpression = this.VisitNew(init.NewExpression);
			IEnumerable<ElementInit> enumerable = this.VisitElementInitializerList(init.Initializers);
			if (newExpression != init.NewExpression || enumerable != init.Initializers)
			{
				return Expression.ListInit(newExpression, enumerable);
			}
			return init;
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00045800 File Offset: 0x00043A00
		internal virtual Expression VisitNewArray(NewArrayExpression na)
		{
			IEnumerable<Expression> enumerable = this.VisitExpressionList(na.Expressions);
			if (enumerable == na.Expressions)
			{
				return na;
			}
			if (na.NodeType == ExpressionType.NewArrayInit)
			{
				return Expression.NewArrayInit(na.Type.GetElementType(), enumerable);
			}
			return Expression.NewArrayBounds(na.Type.GetElementType(), enumerable);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x00045854 File Offset: 0x00043A54
		internal virtual Expression VisitInvocation(InvocationExpression iv)
		{
			IEnumerable<Expression> enumerable = this.VisitExpressionList(iv.Arguments);
			Expression expression = this.Visit(iv.Expression);
			if (enumerable != iv.Arguments || expression != iv.Expression)
			{
				return Expression.Invoke(expression, enumerable);
			}
			return iv;
		}
	}
}

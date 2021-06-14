using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x020000CA RID: 202
	internal sealed class InputBinder : DataServiceALinqExpressionVisitor
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x0001986C File Offset: 0x00017A6C
		private InputBinder(ResourceExpression resource, ParameterExpression setReferenceParam)
		{
			this.input = resource;
			this.inputSet = (resource as ResourceSetExpression);
			this.inputParameter = setReferenceParam;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000198A0 File Offset: 0x00017AA0
		internal static Expression Bind(Expression e, ResourceExpression currentInput, ParameterExpression inputParameter, List<ResourceExpression> referencedInputs)
		{
			InputBinder inputBinder = new InputBinder(currentInput, inputParameter);
			Expression result = inputBinder.Visit(e);
			referencedInputs.AddRange(inputBinder.referencedInputs);
			return result;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000198CC File Offset: 0x00017ACC
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (this.inputSet == null || !this.inputSet.HasTransparentScope)
			{
				return base.VisitMemberAccess(m);
			}
			ParameterExpression parameterExpression = null;
			Stack<PropertyInfo> stack = new Stack<PropertyInfo>();
			MemberExpression memberExpression = m;
			while (memberExpression != null && PlatformHelper.IsProperty(memberExpression.Member) && memberExpression.Expression != null)
			{
				stack.Push((PropertyInfo)memberExpression.Member);
				if (memberExpression.Expression.NodeType == ExpressionType.Parameter)
				{
					parameterExpression = (ParameterExpression)memberExpression.Expression;
				}
				memberExpression = (memberExpression.Expression as MemberExpression);
			}
			if (parameterExpression != this.inputParameter || stack.Count == 0)
			{
				return m;
			}
			ResourceExpression resource = this.input;
			ResourceSetExpression resourceSetExpression = this.inputSet;
			bool flag = false;
			while (stack.Count > 0 && resourceSetExpression != null && resourceSetExpression.HasTransparentScope)
			{
				PropertyInfo propertyInfo = stack.Peek();
				if (propertyInfo.Name.Equals(resourceSetExpression.TransparentScope.Accessor, StringComparison.Ordinal))
				{
					resource = resourceSetExpression;
					stack.Pop();
					flag = true;
				}
				else
				{
					Expression expression;
					if (!resourceSetExpression.TransparentScope.SourceAccessors.TryGetValue(propertyInfo.Name, out expression))
					{
						break;
					}
					flag = true;
					stack.Pop();
					InputReferenceExpression inputReferenceExpression = expression as InputReferenceExpression;
					if (inputReferenceExpression == null)
					{
						resourceSetExpression = (expression as ResourceSetExpression);
						if (resourceSetExpression == null || !resourceSetExpression.HasTransparentScope)
						{
							resource = (ResourceExpression)expression;
						}
					}
					else
					{
						resourceSetExpression = (inputReferenceExpression.Target as ResourceSetExpression);
						resource = resourceSetExpression;
					}
				}
			}
			if (!flag)
			{
				return m;
			}
			Expression expression2 = this.CreateReference(resource);
			while (stack.Count > 0)
			{
				expression2 = Expression.Property(expression2, stack.Pop());
			}
			return expression2;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00019A5C File Offset: 0x00017C5C
		internal override Expression VisitParameter(ParameterExpression p)
		{
			if ((this.inputSet == null || !this.inputSet.HasTransparentScope) && p == this.inputParameter)
			{
				return this.CreateReference(this.input);
			}
			return base.VisitParameter(p);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00019A90 File Offset: 0x00017C90
		private Expression CreateReference(ResourceExpression resource)
		{
			this.referencedInputs.Add(resource);
			return resource.CreateReference();
		}

		// Token: 0x04000409 RID: 1033
		private readonly HashSet<ResourceExpression> referencedInputs = new HashSet<ResourceExpression>(EqualityComparer<ResourceExpression>.Default);

		// Token: 0x0400040A RID: 1034
		private readonly ResourceExpression input;

		// Token: 0x0400040B RID: 1035
		private readonly ResourceSetExpression inputSet;

		// Token: 0x0400040C RID: 1036
		private readonly ParameterExpression inputParameter;
	}
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000118 RID: 280
	internal sealed class InputBinder : DataServiceALinqExpressionVisitor
	{
		// Token: 0x06001335 RID: 4917 RVA: 0x000480CD File Offset: 0x000462CD
		private InputBinder(ResourceExpression resource, ParameterExpression setReferenceParam)
		{
			this.input = resource;
			this.inputSet = (resource as ResourceSetExpression);
			this.inputParameter = setReferenceParam;
		}

		// Token: 0x06001336 RID: 4918 RVA: 0x00048100 File Offset: 0x00046300
		internal static Expression Bind(Expression e, ResourceExpression currentInput, ParameterExpression inputParameter, List<ResourceExpression> referencedInputs)
		{
			InputBinder inputBinder = new InputBinder(currentInput, inputParameter);
			Expression result = inputBinder.Visit(e);
			referencedInputs.AddRange(inputBinder.referencedInputs);
			return result;
		}

		// Token: 0x06001337 RID: 4919 RVA: 0x0004812C File Offset: 0x0004632C
		internal override Expression VisitMemberAccess(MemberExpression m)
		{
			if (this.inputSet == null || !this.inputSet.HasTransparentScope)
			{
				return base.VisitMemberAccess(m);
			}
			ParameterExpression parameterExpression = null;
			Stack<PropertyInfo> stack = new Stack<PropertyInfo>();
			MemberExpression memberExpression = m;
			while (memberExpression != null && memberExpression.Member.MemberType == MemberTypes.Property && memberExpression.Expression != null)
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

		// Token: 0x06001338 RID: 4920 RVA: 0x000482BE File Offset: 0x000464BE
		internal override Expression VisitParameter(ParameterExpression p)
		{
			if ((this.inputSet == null || !this.inputSet.HasTransparentScope) && p == this.inputParameter)
			{
				return this.CreateReference(this.input);
			}
			return base.VisitParameter(p);
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x000482F2 File Offset: 0x000464F2
		private Expression CreateReference(ResourceExpression resource)
		{
			this.referencedInputs.Add(resource);
			return resource.CreateReference();
		}

		// Token: 0x0400059F RID: 1439
		private readonly HashSet<ResourceExpression> referencedInputs = new HashSet<ResourceExpression>(EqualityComparer<ResourceExpression>.Default);

		// Token: 0x040005A0 RID: 1440
		private readonly ResourceExpression input;

		// Token: 0x040005A1 RID: 1441
		private readonly ResourceSetExpression inputSet;

		// Token: 0x040005A2 RID: 1442
		private readonly ParameterExpression inputParameter;
	}
}

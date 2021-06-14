using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x0200007E RID: 126
	internal class ProjectionPathBuilder
	{
		// Token: 0x06000432 RID: 1074 RVA: 0x000117EC File Offset: 0x0000F9EC
		internal ProjectionPathBuilder()
		{
			this.entityInScope = new Stack<bool>();
			this.rewrites = new List<ProjectionPathBuilder.MemberInitRewrite>();
			this.parameterExpressions = new Stack<ParameterExpression>();
			this.parameterExpressionTypes = new Stack<Expression>();
			this.parameterEntries = new Stack<Expression>();
			this.parameterProjectionTypes = new Stack<Type>();
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x00011841 File Offset: 0x0000FA41
		internal bool CurrentIsEntity
		{
			get
			{
				return this.entityInScope.Peek();
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0001184E File Offset: 0x0000FA4E
		internal Expression ExpectedParamTypeInScope
		{
			get
			{
				return this.parameterExpressionTypes.Peek();
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001185B File Offset: 0x0000FA5B
		internal bool HasRewrites
		{
			get
			{
				return this.rewrites.Count > 0;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001186B File Offset: 0x0000FA6B
		internal Expression LambdaParameterInScope
		{
			get
			{
				return this.parameterExpressions.Peek();
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00011878 File Offset: 0x0000FA78
		internal Expression ParameterEntryInScope
		{
			get
			{
				return this.parameterEntries.Peek();
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00011888 File Offset: 0x0000FA88
		public override string ToString()
		{
			string text = "ProjectionPathBuilder: ";
			if (this.parameterExpressions.Count == 0)
			{
				text += "(empty)";
			}
			else
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"entity:",
					this.CurrentIsEntity,
					" param:",
					this.ParameterEntryInScope
				});
			}
			return text;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000118F4 File Offset: 0x0000FAF4
		internal void EnterLambdaScope(LambdaExpression lambda, Expression entry, Expression expectedType)
		{
			ParameterExpression item = lambda.Parameters[0];
			Type type = lambda.Body.Type;
			bool item2 = ClientTypeUtil.TypeOrElementTypeIsEntity(type);
			this.entityInScope.Push(item2);
			this.parameterExpressions.Push(item);
			this.parameterExpressionTypes.Push(expectedType);
			this.parameterEntries.Push(entry);
			this.parameterProjectionTypes.Push(type);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00011960 File Offset: 0x0000FB60
		internal void EnterMemberInit(MemberInitExpression init)
		{
			bool item = ClientTypeUtil.TypeOrElementTypeIsEntity(init.Type);
			this.entityInScope.Push(item);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00011988 File Offset: 0x0000FB88
		internal Expression GetRewrite(Expression expression)
		{
			List<string> list = new List<string>();
			expression = ResourceBinder.StripTo<Expression>(expression);
			while (expression.NodeType == ExpressionType.MemberAccess || expression.NodeType == ExpressionType.TypeAs)
			{
				if (expression.NodeType == ExpressionType.MemberAccess)
				{
					MemberExpression memberExpression = (MemberExpression)expression;
					list.Add(memberExpression.Member.Name);
					expression = ResourceBinder.StripTo<Expression>(memberExpression.Expression);
				}
				else
				{
					expression = ResourceBinder.StripTo<Expression>(((UnaryExpression)expression).Operand);
				}
			}
			Expression result = null;
			foreach (ProjectionPathBuilder.MemberInitRewrite memberInitRewrite in this.rewrites)
			{
				if (memberInitRewrite.Root == expression && list.Count == memberInitRewrite.MemberNames.Length)
				{
					bool flag = true;
					int num = 0;
					while (num < list.Count && num < memberInitRewrite.MemberNames.Length)
					{
						if (list[list.Count - num - 1] != memberInitRewrite.MemberNames[num])
						{
							flag = false;
							break;
						}
						num++;
					}
					if (flag)
					{
						result = memberInitRewrite.RewriteExpression;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00011AB4 File Offset: 0x0000FCB4
		internal void LeaveLambdaScope()
		{
			this.entityInScope.Pop();
			this.parameterExpressions.Pop();
			this.parameterExpressionTypes.Pop();
			this.parameterEntries.Pop();
			this.parameterProjectionTypes.Pop();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00011AF2 File Offset: 0x0000FCF2
		internal void LeaveMemberInit()
		{
			this.entityInScope.Pop();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00011B00 File Offset: 0x0000FD00
		internal void RegisterRewrite(Expression root, string[] names, Expression rewriteExpression)
		{
			this.rewrites.Add(new ProjectionPathBuilder.MemberInitRewrite
			{
				Root = root,
				MemberNames = names,
				RewriteExpression = rewriteExpression
			});
			this.parameterEntries.Push(rewriteExpression);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00011B40 File Offset: 0x0000FD40
		internal void RevokeRewrite(Expression root, string[] names)
		{
			for (int i = 0; i < this.rewrites.Count; i++)
			{
				if (this.rewrites[i].Root == root && names.Length == this.rewrites[i].MemberNames.Length)
				{
					bool flag = true;
					for (int j = 0; j < names.Length; j++)
					{
						if (names[j] != this.rewrites[i].MemberNames[j])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						this.rewrites.RemoveAt(i);
						this.parameterEntries.Pop();
						return;
					}
				}
			}
		}

		// Token: 0x040002CA RID: 714
		private readonly Stack<bool> entityInScope;

		// Token: 0x040002CB RID: 715
		private readonly List<ProjectionPathBuilder.MemberInitRewrite> rewrites;

		// Token: 0x040002CC RID: 716
		private readonly Stack<ParameterExpression> parameterExpressions;

		// Token: 0x040002CD RID: 717
		private readonly Stack<Expression> parameterExpressionTypes;

		// Token: 0x040002CE RID: 718
		private readonly Stack<Expression> parameterEntries;

		// Token: 0x040002CF RID: 719
		private readonly Stack<Type> parameterProjectionTypes;

		// Token: 0x0200007F RID: 127
		internal class MemberInitRewrite
		{
			// Token: 0x17000111 RID: 273
			// (get) Token: 0x06000440 RID: 1088 RVA: 0x00011BDF File Offset: 0x0000FDDF
			// (set) Token: 0x06000441 RID: 1089 RVA: 0x00011BE7 File Offset: 0x0000FDE7
			internal string[] MemberNames { get; set; }

			// Token: 0x17000112 RID: 274
			// (get) Token: 0x06000442 RID: 1090 RVA: 0x00011BF0 File Offset: 0x0000FDF0
			// (set) Token: 0x06000443 RID: 1091 RVA: 0x00011BF8 File Offset: 0x0000FDF8
			internal Expression Root { get; set; }

			// Token: 0x17000113 RID: 275
			// (get) Token: 0x06000444 RID: 1092 RVA: 0x00011C01 File Offset: 0x0000FE01
			// (set) Token: 0x06000445 RID: 1093 RVA: 0x00011C09 File Offset: 0x0000FE09
			internal Expression RewriteExpression { get; set; }
		}
	}
}

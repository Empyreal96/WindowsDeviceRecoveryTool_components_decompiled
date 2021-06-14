using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x0200007D RID: 125
	[DebuggerDisplay("{ToString()}")]
	internal class ProjectionPath : List<ProjectionPathSegment>
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x00011666 File Offset: 0x0000F866
		internal ProjectionPath()
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001166E File Offset: 0x0000F86E
		internal ProjectionPath(ParameterExpression root, Expression expectedRootType, Expression rootEntry)
		{
			this.Root = root;
			this.RootEntry = rootEntry;
			this.ExpectedRootType = expectedRootType;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001168C File Offset: 0x0000F88C
		internal ProjectionPath(ParameterExpression root, Expression expectedRootType, Expression rootEntry, IEnumerable<Expression> members) : this(root, expectedRootType, rootEntry)
		{
			foreach (Expression expression in members)
			{
				base.Add(new ProjectionPathSegment(this, (MemberExpression)expression));
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x000116EC File Offset: 0x0000F8EC
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x000116F4 File Offset: 0x0000F8F4
		internal ParameterExpression Root { get; private set; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x000116FD File Offset: 0x0000F8FD
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x00011705 File Offset: 0x0000F905
		internal Expression RootEntry { get; private set; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0001170E File Offset: 0x0000F90E
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x00011716 File Offset: 0x0000F916
		internal Expression ExpectedRootType { get; private set; }

		// Token: 0x06000431 RID: 1073 RVA: 0x00011720 File Offset: 0x0000F920
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Root.ToString());
			stringBuilder.Append("->");
			for (int i = 0; i < base.Count; i++)
			{
				if (base[i].SourceTypeAs != null)
				{
					stringBuilder.Insert(0, "(");
					stringBuilder.Append(" as " + base[i].SourceTypeAs.Name + ")");
				}
				if (i > 0)
				{
					stringBuilder.Append('.');
				}
				stringBuilder.Append((base[i].Member == null) ? "*" : base[i].Member);
			}
			return stringBuilder.ToString();
		}
	}
}

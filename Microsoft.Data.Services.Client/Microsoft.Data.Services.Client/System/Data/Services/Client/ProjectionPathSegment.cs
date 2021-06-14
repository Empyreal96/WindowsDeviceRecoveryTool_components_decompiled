using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x02000080 RID: 128
	[DebuggerDisplay("Segment {ProjectionType} {Member}")]
	internal class ProjectionPathSegment
	{
		// Token: 0x06000447 RID: 1095 RVA: 0x00011C1A File Offset: 0x0000FE1A
		internal ProjectionPathSegment(ProjectionPath startPath, string member, Type projectionType)
		{
			this.Member = member;
			this.StartPath = startPath;
			this.ProjectionType = projectionType;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00011C38 File Offset: 0x0000FE38
		internal ProjectionPathSegment(ProjectionPath startPath, MemberExpression memberExpression)
		{
			this.StartPath = startPath;
			Expression expression = ResourceBinder.StripTo<Expression>(memberExpression.Expression);
			this.Member = memberExpression.Member.Name;
			this.ProjectionType = memberExpression.Type;
			this.SourceTypeAs = ((expression.NodeType == ExpressionType.TypeAs) ? expression.Type : null);
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x00011C94 File Offset: 0x0000FE94
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x00011C9C File Offset: 0x0000FE9C
		internal string Member { get; private set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00011CA5 File Offset: 0x0000FEA5
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x00011CAD File Offset: 0x0000FEAD
		internal Type ProjectionType { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00011CB6 File Offset: 0x0000FEB6
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00011CBE File Offset: 0x0000FEBE
		internal Type SourceTypeAs { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00011CC7 File Offset: 0x0000FEC7
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00011CCF File Offset: 0x0000FECF
		internal ProjectionPath StartPath { get; private set; }
	}
}

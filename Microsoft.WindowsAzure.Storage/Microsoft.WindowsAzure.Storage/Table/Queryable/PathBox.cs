using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x0200011E RID: 286
	internal class PathBox
	{
		// Token: 0x06001367 RID: 4967 RVA: 0x000488C0 File Offset: 0x00046AC0
		internal PathBox()
		{
			this.projectionPaths.Add(new StringBuilder());
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x00048928 File Offset: 0x00046B28
		internal IEnumerable<string> ProjectionPaths
		{
			get
			{
				return (from s in this.projectionPaths
				where s.Length > 0
				select s.ToString()).Distinct<string>();
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001369 RID: 4969 RVA: 0x00048998 File Offset: 0x00046B98
		internal IEnumerable<string> ExpandPaths
		{
			get
			{
				return (from s in this.expandPaths
				where s.Length > 0
				select s.ToString()).Distinct<string>();
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x000489F4 File Offset: 0x00046BF4
		internal void PushParamExpression(ParameterExpression pe)
		{
			StringBuilder stringBuilder = this.projectionPaths.Last<StringBuilder>();
			this.basePaths.Add(pe, stringBuilder.ToString());
			this.projectionPaths.Remove(stringBuilder);
			this.parameterExpressions.Push(pe);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00048A38 File Offset: 0x00046C38
		internal void PopParamExpression()
		{
			this.parameterExpressions.Pop();
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x00048A46 File Offset: 0x00046C46
		internal ParameterExpression ParamExpressionInScope
		{
			get
			{
				return this.parameterExpressions.Peek();
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00048A54 File Offset: 0x00046C54
		internal void StartNewPath()
		{
			StringBuilder stringBuilder = new StringBuilder(this.basePaths[this.ParamExpressionInScope]);
			PathBox.RemoveEntireEntityMarkerIfPresent(stringBuilder);
			this.expandPaths.Add(new StringBuilder(stringBuilder.ToString()));
			PathBox.AddEntireEntityMarker(stringBuilder);
			this.projectionPaths.Add(stringBuilder);
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00048AA8 File Offset: 0x00046CA8
		internal void AppendToPath(PropertyInfo pi)
		{
			Type elementType = TypeSystem.GetElementType(pi.PropertyType);
			StringBuilder stringBuilder;
			if (CommonUtil.IsClientType(elementType))
			{
				stringBuilder = this.expandPaths.Last<StringBuilder>();
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('/');
				}
				stringBuilder.Append(pi.Name);
			}
			stringBuilder = this.projectionPaths.Last<StringBuilder>();
			PathBox.RemoveEntireEntityMarkerIfPresent(stringBuilder);
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('/');
			}
			stringBuilder.Append(pi.Name);
			if (CommonUtil.IsClientType(elementType))
			{
				PathBox.AddEntireEntityMarker(stringBuilder);
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00048B34 File Offset: 0x00046D34
		private static void RemoveEntireEntityMarkerIfPresent(StringBuilder sb)
		{
			if (sb.Length > 0 && sb[sb.Length - 1] == '*')
			{
				sb.Remove(sb.Length - 1, 1);
			}
			if (sb.Length > 0 && sb[sb.Length - 1] == '/')
			{
				sb.Remove(sb.Length - 1, 1);
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00048B97 File Offset: 0x00046D97
		private static void AddEntireEntityMarker(StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.Append('/');
			}
			sb.Append('*');
		}

		// Token: 0x040005B4 RID: 1460
		private const char EntireEntityMarker = '*';

		// Token: 0x040005B5 RID: 1461
		private readonly List<StringBuilder> projectionPaths = new List<StringBuilder>();

		// Token: 0x040005B6 RID: 1462
		private readonly List<StringBuilder> expandPaths = new List<StringBuilder>();

		// Token: 0x040005B7 RID: 1463
		private readonly Stack<ParameterExpression> parameterExpressions = new Stack<ParameterExpression>();

		// Token: 0x040005B8 RID: 1464
		private readonly Dictionary<ParameterExpression, string> basePaths = new Dictionary<ParameterExpression, string>(ReferenceEqualityComparer<ParameterExpression>.Instance);
	}
}

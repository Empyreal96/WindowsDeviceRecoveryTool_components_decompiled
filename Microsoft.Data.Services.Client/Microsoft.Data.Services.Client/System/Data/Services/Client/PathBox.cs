using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x020000B5 RID: 181
	internal class PathBox
	{
		// Token: 0x060005BC RID: 1468 RVA: 0x00015E20 File Offset: 0x00014020
		internal PathBox()
		{
			this.projectionPaths.Add(new StringBuilder());
			this.uriVersion = Util.DataServiceVersion1;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00015E94 File Offset: 0x00014094
		internal IEnumerable<string> ProjectionPaths
		{
			get
			{
				return (from s in this.projectionPaths
				where s.Length > 0
				select s.ToString()).Distinct<string>();
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x00015F04 File Offset: 0x00014104
		internal IEnumerable<string> ExpandPaths
		{
			get
			{
				return (from s in this.expandPaths
				where s.Length > 0
				select s.ToString()).Distinct<string>();
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00015F60 File Offset: 0x00014160
		internal Version UriVersion
		{
			get
			{
				return this.uriVersion;
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00015F68 File Offset: 0x00014168
		internal void PushParamExpression(ParameterExpression pe)
		{
			StringBuilder stringBuilder = this.projectionPaths.Last<StringBuilder>();
			this.basePaths.Add(pe, stringBuilder.ToString());
			this.projectionPaths.Remove(stringBuilder);
			this.parameterExpressions.Push(pe);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00015FAC File Offset: 0x000141AC
		internal void PopParamExpression()
		{
			this.parameterExpressions.Pop();
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00015FBA File Offset: 0x000141BA
		internal ParameterExpression ParamExpressionInScope
		{
			get
			{
				return this.parameterExpressions.Peek();
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015FC8 File Offset: 0x000141C8
		internal void StartNewPath()
		{
			StringBuilder stringBuilder = new StringBuilder(this.basePaths[this.ParamExpressionInScope]);
			PathBox.RemoveEntireEntityMarkerIfPresent(stringBuilder);
			this.expandPaths.Add(new StringBuilder(stringBuilder.ToString()));
			PathBox.AddEntireEntityMarker(stringBuilder);
			this.projectionPaths.Add(stringBuilder);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001601C File Offset: 0x0001421C
		internal void AppendPropertyToPath(PropertyInfo pi, Type convertedSourceType, DataServiceContext context)
		{
			bool flag = ClientTypeUtil.TypeOrElementTypeIsEntity(pi.PropertyType);
			string name = (convertedSourceType == null) ? null : UriHelper.GetEntityTypeNameForUriAndValidateMaxProtocolVersion(convertedSourceType, context, ref this.uriVersion);
			if (flag)
			{
				if (convertedSourceType != null)
				{
					this.AppendToExpandPath(name);
				}
				this.AppendToExpandPath(pi.Name);
			}
			if (convertedSourceType != null)
			{
				this.AppendToProjectionPath(name, false);
			}
			StringBuilder sb = this.AppendToProjectionPath(pi.Name, false);
			if (flag)
			{
				PathBox.AddEntireEntityMarker(sb);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0001609C File Offset: 0x0001429C
		private StringBuilder AppendToProjectionPath(string name, bool replaceEntityMarkerIfPresent)
		{
			StringBuilder stringBuilder = this.projectionPaths.Last<StringBuilder>();
			bool flag = PathBox.RemoveEntireEntityMarkerIfPresent(stringBuilder);
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('/');
			}
			stringBuilder.Append(name);
			if (flag && replaceEntityMarkerIfPresent)
			{
				PathBox.AddEntireEntityMarker(stringBuilder);
			}
			return stringBuilder;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000160E4 File Offset: 0x000142E4
		private void AppendToExpandPath(string name)
		{
			StringBuilder stringBuilder = this.expandPaths.Last<StringBuilder>();
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('/');
			}
			stringBuilder.Append(name);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00016118 File Offset: 0x00014318
		private static bool RemoveEntireEntityMarkerIfPresent(StringBuilder sb)
		{
			bool result = false;
			if (sb.Length > 0 && sb[sb.Length - 1] == '*')
			{
				sb.Remove(sb.Length - 1, 1);
				result = true;
			}
			if (sb.Length > 0 && sb[sb.Length - 1] == '/')
			{
				sb.Remove(sb.Length - 1, 1);
			}
			return result;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00016180 File Offset: 0x00014380
		private static void AddEntireEntityMarker(StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				sb.Append('/');
			}
			sb.Append('*');
		}

		// Token: 0x0400031E RID: 798
		private const char EntireEntityMarker = '*';

		// Token: 0x0400031F RID: 799
		private readonly List<StringBuilder> projectionPaths = new List<StringBuilder>();

		// Token: 0x04000320 RID: 800
		private readonly List<StringBuilder> expandPaths = new List<StringBuilder>();

		// Token: 0x04000321 RID: 801
		private readonly Stack<ParameterExpression> parameterExpressions = new Stack<ParameterExpression>();

		// Token: 0x04000322 RID: 802
		private readonly Dictionary<ParameterExpression, string> basePaths = new Dictionary<ParameterExpression, string>(ReferenceEqualityComparer<ParameterExpression>.Instance);

		// Token: 0x04000323 RID: 803
		private Version uriVersion;
	}
}

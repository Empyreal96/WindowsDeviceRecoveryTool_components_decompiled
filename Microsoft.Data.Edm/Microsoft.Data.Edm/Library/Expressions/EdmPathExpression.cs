using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Expressions;

namespace Microsoft.Data.Edm.Library.Expressions
{
	// Token: 0x020000BA RID: 186
	public class EdmPathExpression : EdmElement, IEdmPathExpression, IEdmExpression, IEdmElement
	{
		// Token: 0x06000355 RID: 853 RVA: 0x00008A6C File Offset: 0x00006C6C
		public EdmPathExpression(string path) : this(EdmUtil.CheckArgumentNull<string>(path, "path").Split(new char[]
		{
			'/'
		}))
		{
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00008A9C File Offset: 0x00006C9C
		public EdmPathExpression(params string[] path) : this((IEnumerable<string>)path)
		{
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00008AAC File Offset: 0x00006CAC
		public EdmPathExpression(IEnumerable<string> path)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<string>>(path, "path");
			foreach (string text in path)
			{
				if (text.Contains("/"))
				{
					throw new ArgumentException(Strings.PathSegmentMustNotContainSlash);
				}
			}
			this.path = path;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00008B20 File Offset: 0x00006D20
		public IEnumerable<string> Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00008B28 File Offset: 0x00006D28
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.Path;
			}
		}

		// Token: 0x04000178 RID: 376
		private readonly IEnumerable<string> path;
	}
}

using System;
using System.Globalization;

namespace Microsoft.Data.OData.Metadata
{
	// Token: 0x02000202 RID: 514
	internal sealed class EpmAttributeNameBuilder
	{
		// Token: 0x06000FBB RID: 4027 RVA: 0x000393A3 File Offset: 0x000375A3
		internal EpmAttributeNameBuilder()
		{
			this.suffix = string.Empty;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x000393B6 File Offset: 0x000375B6
		internal string EpmKeepInContent
		{
			get
			{
				return "FC_KeepInContent" + this.suffix;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x000393C8 File Offset: 0x000375C8
		internal string EpmSourcePath
		{
			get
			{
				return "FC_SourcePath" + this.suffix;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x000393DA File Offset: 0x000375DA
		internal string EpmTargetPath
		{
			get
			{
				return "FC_TargetPath" + this.suffix;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x000393EC File Offset: 0x000375EC
		internal string EpmContentKind
		{
			get
			{
				return "FC_ContentKind" + this.suffix;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x000393FE File Offset: 0x000375FE
		internal string EpmNsPrefix
		{
			get
			{
				return "FC_NsPrefix" + this.suffix;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x00039410 File Offset: 0x00037610
		internal string EpmNsUri
		{
			get
			{
				return "FC_NsUri" + this.suffix;
			}
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00039422 File Offset: 0x00037622
		internal void MoveNext()
		{
			this.index++;
			this.suffix = "_" + this.index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x040005B4 RID: 1460
		private const string Separator = "_";

		// Token: 0x040005B5 RID: 1461
		private int index;

		// Token: 0x040005B6 RID: 1462
		private string suffix;
	}
}

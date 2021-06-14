using System;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000019 RID: 25
	public sealed class ODataUriParserSettings
	{
		// Token: 0x06000090 RID: 144 RVA: 0x00003748 File Offset: 0x00001948
		public ODataUriParserSettings()
		{
			this.FilterLimit = 800;
			this.OrderByLimit = 800;
			this.PathLimit = 100;
			this.SelectExpandLimit = 800;
			this.MaximumExpansionDepth = int.MaxValue;
			this.MaximumExpansionCount = int.MaxValue;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000091 RID: 145 RVA: 0x0000379A File Offset: 0x0000199A
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000037A2 File Offset: 0x000019A2
		public int MaximumExpansionDepth
		{
			get
			{
				return this.maxExpandDepth;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.maxExpandDepth = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000037BA File Offset: 0x000019BA
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000037C2 File Offset: 0x000019C2
		public int MaximumExpansionCount
		{
			get
			{
				return this.maxExpandCount;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.maxExpandCount = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000037DA File Offset: 0x000019DA
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000037E2 File Offset: 0x000019E2
		internal int SelectExpandLimit
		{
			get
			{
				return this.selectExpandLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.selectExpandLimit = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000037FA File Offset: 0x000019FA
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00003802 File Offset: 0x00001A02
		internal bool UseWcfDataServicesServerBehavior
		{
			get
			{
				return this.useWcfDataServicesServerBehavior;
			}
			set
			{
				this.useWcfDataServicesServerBehavior = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000380B File Offset: 0x00001A0B
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00003813 File Offset: 0x00001A13
		internal bool SupportExpandOptions
		{
			get
			{
				return this.supportExpandOptions;
			}
			set
			{
				this.supportExpandOptions = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000381C File Offset: 0x00001A1C
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003824 File Offset: 0x00001A24
		internal int FilterLimit
		{
			get
			{
				return this.filterLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.filterLimit = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000383C File Offset: 0x00001A3C
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003844 File Offset: 0x00001A44
		internal int OrderByLimit
		{
			get
			{
				return this.orderByLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.orderByLimit = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000385C File Offset: 0x00001A5C
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003864 File Offset: 0x00001A64
		internal int PathLimit
		{
			get
			{
				return this.pathLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ODataException(Strings.UriParser_NegativeLimit);
				}
				this.pathLimit = value;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000387C File Offset: 0x00001A7C
		public void EnableWcfDataServicesServerBehavior()
		{
			this.UseWcfDataServicesServerBehavior = true;
			this.selectExpandLimit = int.MaxValue;
		}

		// Token: 0x04000038 RID: 56
		internal const int DefaultFilterLimit = 800;

		// Token: 0x04000039 RID: 57
		internal const int DefaultOrderByLimit = 800;

		// Token: 0x0400003A RID: 58
		internal const int DefaultSelectExpandLimit = 800;

		// Token: 0x0400003B RID: 59
		internal const int DefaultPathLimit = 100;

		// Token: 0x0400003C RID: 60
		private int filterLimit;

		// Token: 0x0400003D RID: 61
		private int orderByLimit;

		// Token: 0x0400003E RID: 62
		private int pathLimit;

		// Token: 0x0400003F RID: 63
		private int selectExpandLimit;

		// Token: 0x04000040 RID: 64
		private bool supportExpandOptions;

		// Token: 0x04000041 RID: 65
		private bool useWcfDataServicesServerBehavior;

		// Token: 0x04000042 RID: 66
		private int maxExpandDepth;

		// Token: 0x04000043 RID: 67
		private int maxExpandCount;
	}
}

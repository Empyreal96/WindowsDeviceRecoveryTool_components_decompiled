using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000118 RID: 280
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MediaEntryAttribute : Attribute
	{
		// Token: 0x06000927 RID: 2343 RVA: 0x00025481 File Offset: 0x00023681
		public MediaEntryAttribute(string mediaMemberName)
		{
			Util.CheckArgumentNull<string>(mediaMemberName, "mediaMemberName");
			this.mediaMemberName = mediaMemberName;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x0002549C File Offset: 0x0002369C
		public string MediaMemberName
		{
			get
			{
				return this.mediaMemberName;
			}
		}

		// Token: 0x0400055E RID: 1374
		private readonly string mediaMemberName;
	}
}

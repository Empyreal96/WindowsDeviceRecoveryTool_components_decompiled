using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200011A RID: 282
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class MimeTypePropertyAttribute : Attribute
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x000254A4 File Offset: 0x000236A4
		public MimeTypePropertyAttribute(string dataPropertyName, string mimeTypePropertyName)
		{
			this.dataPropertyName = dataPropertyName;
			this.mimeTypePropertyName = mimeTypePropertyName;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x000254BA File Offset: 0x000236BA
		public string DataPropertyName
		{
			get
			{
				return this.dataPropertyName;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600092B RID: 2347 RVA: 0x000254C2 File Offset: 0x000236C2
		public string MimeTypePropertyName
		{
			get
			{
				return this.mimeTypePropertyName;
			}
		}

		// Token: 0x04000564 RID: 1380
		private readonly string dataPropertyName;

		// Token: 0x04000565 RID: 1381
		private readonly string mimeTypePropertyName;
	}
}

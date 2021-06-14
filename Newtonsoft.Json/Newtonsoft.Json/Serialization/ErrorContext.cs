using System;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000AB RID: 171
	public class ErrorContext
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x00020784 File Offset: 0x0001E984
		internal ErrorContext(object originalObject, object member, string path, Exception error)
		{
			this.OriginalObject = originalObject;
			this.Member = member;
			this.Error = error;
			this.Path = path;
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x000207A9 File Offset: 0x0001E9A9
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x000207B1 File Offset: 0x0001E9B1
		internal bool Traced { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x000207BA File Offset: 0x0001E9BA
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x000207C2 File Offset: 0x0001E9C2
		public Exception Error { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x000207CB File Offset: 0x0001E9CB
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x000207D3 File Offset: 0x0001E9D3
		public object OriginalObject { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x000207DC File Offset: 0x0001E9DC
		// (set) Token: 0x06000868 RID: 2152 RVA: 0x000207E4 File Offset: 0x0001E9E4
		public object Member { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x000207ED File Offset: 0x0001E9ED
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x000207F5 File Offset: 0x0001E9F5
		public string Path { get; private set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x000207FE File Offset: 0x0001E9FE
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x00020806 File Offset: 0x0001EA06
		public bool Handled { get; set; }
	}
}

using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000EF RID: 239
	internal class ReflectionMember
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0002D9CB File Offset: 0x0002BBCB
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0002D9D3 File Offset: 0x0002BBD3
		public Type MemberType { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0002D9DC File Offset: 0x0002BBDC
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0002D9E4 File Offset: 0x0002BBE4
		public Func<object, object> Getter { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0002D9ED File Offset: 0x0002BBED
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0002D9F5 File Offset: 0x0002BBF5
		public Action<object, object> Setter { get; set; }
	}
}

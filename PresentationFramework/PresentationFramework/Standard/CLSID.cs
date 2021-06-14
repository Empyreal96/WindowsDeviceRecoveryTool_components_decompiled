using System;

namespace Standard
{
	// Token: 0x02000004 RID: 4
	internal static class CLSID
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002111 File Offset: 0x00000311
		public static T CoCreateInstance<T>(string clsid)
		{
			return (T)((object)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid(clsid))));
		}

		// Token: 0x0400001A RID: 26
		public const string TaskbarList = "56FDF344-FD6D-11d0-958A-006097C9A090";

		// Token: 0x0400001B RID: 27
		public const string EnumerableObjectCollection = "2d3468c1-36a7-43b6-ac24-d3f02fd9607a";

		// Token: 0x0400001C RID: 28
		public const string ShellLink = "00021401-0000-0000-C000-000000000046";

		// Token: 0x0400001D RID: 29
		public const string DestinationList = "77f10cf0-3db5-4966-b520-b7c54fd35ed6";

		// Token: 0x0400001E RID: 30
		public const string ApplicationDestinations = "86c14003-4d6b-4ef3-a7b4-0506663b2e68";

		// Token: 0x0400001F RID: 31
		public const string ApplicationDocumentLists = "86bec222-30f2-47e0-9f25-60d11cd75c28";
	}
}

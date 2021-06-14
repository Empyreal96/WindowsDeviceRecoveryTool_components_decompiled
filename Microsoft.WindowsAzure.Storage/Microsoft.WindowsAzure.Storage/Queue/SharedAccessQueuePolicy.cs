using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Queue
{
	// Token: 0x020000FA RID: 250
	public sealed class SharedAccessQueuePolicy
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001266 RID: 4710 RVA: 0x0004430D File Offset: 0x0004250D
		// (set) Token: 0x06001267 RID: 4711 RVA: 0x00044315 File Offset: 0x00042515
		public DateTimeOffset? SharedAccessStartTime { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001268 RID: 4712 RVA: 0x0004431E File Offset: 0x0004251E
		// (set) Token: 0x06001269 RID: 4713 RVA: 0x00044326 File Offset: 0x00042526
		public DateTimeOffset? SharedAccessExpiryTime { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600126A RID: 4714 RVA: 0x0004432F File Offset: 0x0004252F
		// (set) Token: 0x0600126B RID: 4715 RVA: 0x00044337 File Offset: 0x00042537
		public SharedAccessQueuePermissions Permissions { get; set; }

		// Token: 0x0600126C RID: 4716 RVA: 0x00044340 File Offset: 0x00042540
		public static string PermissionsToString(SharedAccessQueuePermissions permissions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((permissions & SharedAccessQueuePermissions.Read) == SharedAccessQueuePermissions.Read)
			{
				stringBuilder.Append("r");
			}
			if ((permissions & SharedAccessQueuePermissions.Add) == SharedAccessQueuePermissions.Add)
			{
				stringBuilder.Append("a");
			}
			if ((permissions & SharedAccessQueuePermissions.Update) == SharedAccessQueuePermissions.Update)
			{
				stringBuilder.Append("u");
			}
			if ((permissions & SharedAccessQueuePermissions.ProcessMessages) == SharedAccessQueuePermissions.ProcessMessages)
			{
				stringBuilder.Append("p");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x000443A4 File Offset: 0x000425A4
		public static SharedAccessQueuePermissions PermissionsFromString(string input)
		{
			CommonUtility.AssertNotNull("input", input);
			SharedAccessQueuePermissions sharedAccessQueuePermissions = SharedAccessQueuePermissions.None;
			foreach (char c in input)
			{
				char c2 = c;
				if (c2 != 'a')
				{
					switch (c2)
					{
					case 'p':
						sharedAccessQueuePermissions |= SharedAccessQueuePermissions.ProcessMessages;
						goto IL_65;
					case 'q':
						break;
					case 'r':
						sharedAccessQueuePermissions |= SharedAccessQueuePermissions.Read;
						goto IL_65;
					default:
						if (c2 == 'u')
						{
							sharedAccessQueuePermissions |= SharedAccessQueuePermissions.Update;
							goto IL_65;
						}
						break;
					}
					throw new ArgumentOutOfRangeException("input");
				}
				sharedAccessQueuePermissions |= SharedAccessQueuePermissions.Add;
				IL_65:;
			}
			if (sharedAccessQueuePermissions == SharedAccessQueuePermissions.None)
			{
				sharedAccessQueuePermissions = sharedAccessQueuePermissions;
			}
			return sharedAccessQueuePermissions;
		}
	}
}

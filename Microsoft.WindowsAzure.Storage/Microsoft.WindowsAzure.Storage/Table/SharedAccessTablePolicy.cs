using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000144 RID: 324
	public sealed class SharedAccessTablePolicy
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x0004EFE5 File Offset: 0x0004D1E5
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x0004EFED File Offset: 0x0004D1ED
		public DateTimeOffset? SharedAccessStartTime { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x0004EFF6 File Offset: 0x0004D1F6
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x0004EFFE File Offset: 0x0004D1FE
		public DateTimeOffset? SharedAccessExpiryTime { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x0004F007 File Offset: 0x0004D207
		// (set) Token: 0x060014A8 RID: 5288 RVA: 0x0004F00F File Offset: 0x0004D20F
		public SharedAccessTablePermissions Permissions { get; set; }

		// Token: 0x060014A9 RID: 5289 RVA: 0x0004F018 File Offset: 0x0004D218
		public static string PermissionsToString(SharedAccessTablePermissions permissions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((permissions & SharedAccessTablePermissions.Query) == SharedAccessTablePermissions.Query)
			{
				stringBuilder.Append("r");
			}
			if ((permissions & SharedAccessTablePermissions.Add) == SharedAccessTablePermissions.Add)
			{
				stringBuilder.Append("a");
			}
			if ((permissions & SharedAccessTablePermissions.Update) == SharedAccessTablePermissions.Update)
			{
				stringBuilder.Append("u");
			}
			if ((permissions & SharedAccessTablePermissions.Delete) == SharedAccessTablePermissions.Delete)
			{
				stringBuilder.Append("d");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0004F07C File Offset: 0x0004D27C
		public static SharedAccessTablePermissions PermissionsFromString(string input)
		{
			CommonUtility.AssertNotNull("input", input);
			SharedAccessTablePermissions sharedAccessTablePermissions = SharedAccessTablePermissions.None;
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				char c2 = c;
				if (c2 <= 'd')
				{
					if (c2 != 'a')
					{
						if (c2 != 'd')
						{
							goto IL_58;
						}
						sharedAccessTablePermissions |= SharedAccessTablePermissions.Delete;
					}
					else
					{
						sharedAccessTablePermissions |= SharedAccessTablePermissions.Add;
					}
				}
				else if (c2 != 'r')
				{
					if (c2 != 'u')
					{
						goto IL_58;
					}
					sharedAccessTablePermissions |= SharedAccessTablePermissions.Update;
				}
				else
				{
					sharedAccessTablePermissions |= SharedAccessTablePermissions.Query;
				}
				i++;
				continue;
				IL_58:
				throw new ArgumentOutOfRangeException("input");
			}
			if (sharedAccessTablePermissions == SharedAccessTablePermissions.None)
			{
				sharedAccessTablePermissions = sharedAccessTablePermissions;
			}
			return sharedAccessTablePermissions;
		}
	}
}

using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.File
{
	// Token: 0x020000E2 RID: 226
	public sealed class SharedAccessFilePolicy
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00042659 File Offset: 0x00040859
		// (set) Token: 0x060011E4 RID: 4580 RVA: 0x00042661 File Offset: 0x00040861
		public DateTimeOffset? SharedAccessStartTime { get; set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x0004266A File Offset: 0x0004086A
		// (set) Token: 0x060011E6 RID: 4582 RVA: 0x00042672 File Offset: 0x00040872
		public DateTimeOffset? SharedAccessExpiryTime { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0004267B File Offset: 0x0004087B
		// (set) Token: 0x060011E8 RID: 4584 RVA: 0x00042683 File Offset: 0x00040883
		public SharedAccessFilePermissions Permissions { get; set; }

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004268C File Offset: 0x0004088C
		public static string PermissionsToString(SharedAccessFilePermissions permissions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((permissions & SharedAccessFilePermissions.Read) == SharedAccessFilePermissions.Read)
			{
				stringBuilder.Append("r");
			}
			if ((permissions & SharedAccessFilePermissions.Write) == SharedAccessFilePermissions.Write)
			{
				stringBuilder.Append("w");
			}
			if ((permissions & SharedAccessFilePermissions.Delete) == SharedAccessFilePermissions.Delete)
			{
				stringBuilder.Append("d");
			}
			if ((permissions & SharedAccessFilePermissions.List) == SharedAccessFilePermissions.List)
			{
				stringBuilder.Append("l");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000426F0 File Offset: 0x000408F0
		public static SharedAccessFilePermissions PermissionsFromString(string input)
		{
			CommonUtility.AssertNotNull("input", input);
			SharedAccessFilePermissions sharedAccessFilePermissions = SharedAccessFilePermissions.None;
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				char c2 = c;
				if (c2 <= 'l')
				{
					if (c2 != 'd')
					{
						if (c2 != 'l')
						{
							goto IL_58;
						}
						sharedAccessFilePermissions |= SharedAccessFilePermissions.List;
					}
					else
					{
						sharedAccessFilePermissions |= SharedAccessFilePermissions.Delete;
					}
				}
				else if (c2 != 'r')
				{
					if (c2 != 'w')
					{
						goto IL_58;
					}
					sharedAccessFilePermissions |= SharedAccessFilePermissions.Write;
				}
				else
				{
					sharedAccessFilePermissions |= SharedAccessFilePermissions.Read;
				}
				i++;
				continue;
				IL_58:
				throw new ArgumentOutOfRangeException("input");
			}
			if (sharedAccessFilePermissions == SharedAccessFilePermissions.None)
			{
				sharedAccessFilePermissions = sharedAccessFilePermissions;
			}
			return sharedAccessFilePermissions;
		}
	}
}

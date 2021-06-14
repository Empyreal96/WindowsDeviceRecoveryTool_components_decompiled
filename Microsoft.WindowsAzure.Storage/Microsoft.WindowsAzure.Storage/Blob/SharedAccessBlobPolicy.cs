using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Blob
{
	// Token: 0x020000C3 RID: 195
	public sealed class SharedAccessBlobPolicy
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x0003F3C5 File Offset: 0x0003D5C5
		// (set) Token: 0x0600110D RID: 4365 RVA: 0x0003F3CD File Offset: 0x0003D5CD
		public DateTimeOffset? SharedAccessStartTime { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x0003F3D6 File Offset: 0x0003D5D6
		// (set) Token: 0x0600110F RID: 4367 RVA: 0x0003F3DE File Offset: 0x0003D5DE
		public DateTimeOffset? SharedAccessExpiryTime { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x0003F3E7 File Offset: 0x0003D5E7
		// (set) Token: 0x06001111 RID: 4369 RVA: 0x0003F3EF File Offset: 0x0003D5EF
		public SharedAccessBlobPermissions Permissions { get; set; }

		// Token: 0x06001112 RID: 4370 RVA: 0x0003F3F8 File Offset: 0x0003D5F8
		public static string PermissionsToString(SharedAccessBlobPermissions permissions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((permissions & SharedAccessBlobPermissions.Read) == SharedAccessBlobPermissions.Read)
			{
				stringBuilder.Append("r");
			}
			if ((permissions & SharedAccessBlobPermissions.Write) == SharedAccessBlobPermissions.Write)
			{
				stringBuilder.Append("w");
			}
			if ((permissions & SharedAccessBlobPermissions.Delete) == SharedAccessBlobPermissions.Delete)
			{
				stringBuilder.Append("d");
			}
			if ((permissions & SharedAccessBlobPermissions.List) == SharedAccessBlobPermissions.List)
			{
				stringBuilder.Append("l");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0003F45C File Offset: 0x0003D65C
		public static SharedAccessBlobPermissions PermissionsFromString(string input)
		{
			CommonUtility.AssertNotNull("input", input);
			SharedAccessBlobPermissions sharedAccessBlobPermissions = SharedAccessBlobPermissions.None;
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
						sharedAccessBlobPermissions |= SharedAccessBlobPermissions.List;
					}
					else
					{
						sharedAccessBlobPermissions |= SharedAccessBlobPermissions.Delete;
					}
				}
				else if (c2 != 'r')
				{
					if (c2 != 'w')
					{
						goto IL_58;
					}
					sharedAccessBlobPermissions |= SharedAccessBlobPermissions.Write;
				}
				else
				{
					sharedAccessBlobPermissions |= SharedAccessBlobPermissions.Read;
				}
				i++;
				continue;
				IL_58:
				throw new ArgumentOutOfRangeException("input");
			}
			if (sharedAccessBlobPermissions == SharedAccessBlobPermissions.None)
			{
				sharedAccessBlobPermissions = sharedAccessBlobPermissions;
			}
			return sharedAccessBlobPermissions;
		}
	}
}

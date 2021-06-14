using System;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200005C RID: 92
	public sealed class RegValueInfo
	{
		// Token: 0x06000239 RID: 569 RVA: 0x0000AC89 File Offset: 0x00008E89
		public RegValueInfo()
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000AC94 File Offset: 0x00008E94
		public RegValueInfo(RegValueInfo regValueInfo)
		{
			this.Type = regValueInfo.Type;
			this.KeyName = regValueInfo.KeyName;
			this.ValueName = regValueInfo.ValueName;
			this.Value = regValueInfo.Value;
			this.Partition = regValueInfo.Partition;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000ACE3 File Offset: 0x00008EE3
		public void SetRegValueType(string strType)
		{
			this.Type = RegUtil.RegValueTypeForString(strType);
		}

		// Token: 0x04000167 RID: 359
		public RegValueType Type;

		// Token: 0x04000168 RID: 360
		public string KeyName;

		// Token: 0x04000169 RID: 361
		public string ValueName;

		// Token: 0x0400016A RID: 362
		public string Value;

		// Token: 0x0400016B RID: 363
		public string Partition;
	}
}

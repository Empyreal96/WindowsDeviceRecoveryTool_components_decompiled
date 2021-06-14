using System;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Helpers
{
	// Token: 0x0200000F RID: 15
	[AttributeUsage(AttributeTargets.All)]
	public sealed class UriDescriptionAttribute : Attribute
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003256 File Offset: 0x00001456
		public UriDescriptionAttribute(string value)
		{
			this.value = value;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003268 File Offset: 0x00001468
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0400005E RID: 94
		private readonly string value;
	}
}

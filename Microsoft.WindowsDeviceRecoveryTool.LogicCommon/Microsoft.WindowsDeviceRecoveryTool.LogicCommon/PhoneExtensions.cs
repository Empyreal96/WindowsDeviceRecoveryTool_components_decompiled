using System;
using System.Linq;
using Microsoft.WindowsDeviceRecoveryTool.Model;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon
{
	// Token: 0x02000020 RID: 32
	public static class PhoneExtensions
	{
		// Token: 0x06000123 RID: 291 RVA: 0x000075F8 File Offset: 0x000057F8
		public static bool IsPhoneDeviceType(this Phone phone)
		{
			PhoneTypes[] source = new PhoneTypes[]
			{
				PhoneTypes.Alcatel,
				PhoneTypes.Blu,
				PhoneTypes.Htc,
				PhoneTypes.Lg,
				PhoneTypes.Lumia,
				PhoneTypes.Mcj
			};
			return source.Contains(phone.Type);
		}
	}
}

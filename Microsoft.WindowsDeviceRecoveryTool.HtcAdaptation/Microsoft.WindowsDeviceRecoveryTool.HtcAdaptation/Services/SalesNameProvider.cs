using System;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;

namespace Microsoft.WindowsDeviceRecoveryTool.HtcAdaptation.Services
{
	// Token: 0x02000009 RID: 9
	public class SalesNameProvider : BaseSalesNameProvider
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000039A0 File Offset: 0x00001BA0
		public override string NameForString(string text)
		{
			string result;
			if (text.Equals("USB BLDR"))
			{
				result = "HTC";
			}
			else
			{
				result = base.NameForString(text);
			}
			return result;
		}
	}
}

using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Model.Enums;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts
{
	// Token: 0x02000006 RID: 6
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ExportAdaptationAttribute : ExportAttribute
	{
		// Token: 0x06000041 RID: 65 RVA: 0x000027A1 File Offset: 0x000009A1
		public ExportAdaptationAttribute() : base(typeof(IAdaptation))
		{
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000027B8 File Offset: 0x000009B8
		// (set) Token: 0x06000043 RID: 67 RVA: 0x000027CF File Offset: 0x000009CF
		public PhoneTypes Type { get; set; }
	}
}

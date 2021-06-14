using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;

namespace Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services
{
	// Token: 0x02000039 RID: 57
	[Export]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class FlowConditionService
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000C790 File Offset: 0x0000A990
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000C7B7 File Offset: 0x0000A9B7
		public bool UseSignatureCheck
		{
			get
			{
				return !this.IsSignatureCheckChoiceAvailable || this.useSignatureCheck;
			}
			set
			{
				this.useSignatureCheck = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000C7C4 File Offset: 0x0000A9C4
		public bool IsSignatureCheckChoiceAvailable
		{
			get
			{
				return this.IsTestConfigFileAvailable;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000C7DC File Offset: 0x0000A9DC
		public bool IsManualSelectionAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000C7F0 File Offset: 0x0000A9F0
		public bool IsTestConfigFileAvailable
		{
			get
			{
				this.Initialize();
				return this.isTestConfigFileAvailable;
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C810 File Offset: 0x0000AA10
		private void Initialize()
		{
			if (!this.initialized)
			{
				try
				{
					this.isTestConfigFileAvailable = false;
					this.initialized = true;
				}
				catch (Exception error)
				{
					Tracer<FlowConditionService>.WriteWarning(error, "Could not be initialized", new object[0]);
				}
			}
		}

		// Token: 0x04000175 RID: 373
		private const string TestConfigFileName = "test.cfg";

		// Token: 0x04000176 RID: 374
		private bool initialized;

		// Token: 0x04000177 RID: 375
		private bool isTestConfigFileAvailable;

		// Token: 0x04000178 RID: 376
		private bool useSignatureCheck;
	}
}

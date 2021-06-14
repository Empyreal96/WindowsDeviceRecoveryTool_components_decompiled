using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.Framework;
using Microsoft.WindowsDeviceRecoveryTool.Messages;
using Microsoft.WindowsDeviceRecoveryTool.Model;

namespace Microsoft.WindowsDeviceRecoveryTool.Controllers
{
	// Token: 0x02000035 RID: 53
	[Export("Microsoft.WindowsDeviceRecoveryTool.Controllers.FfuController", typeof(IController))]
	public class FfuController : BaseController
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000D980 File Offset: 0x0000BB80
		[ImportingConstructor]
		public FfuController(ICommandRepository commandRepository, LogicContext logics, EventAggregator eventAggragator) : base(commandRepository, eventAggragator)
		{
			this.logics = logics;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000D994 File Offset: 0x0000BB94
		[CustomCommand(IsAsynchronous = true)]
		public void ReadFfuFilePlatformId(string ffuFilePath, CancellationToken token)
		{
			this.logics.FfuFileInfoService.ClearDataForFfuFile(ffuFilePath);
			PlatformId platformId = this.logics.FfuFileInfoService.ReadFfuFilePlatformId(ffuFilePath);
			string version = null;
			if (!this.logics.FfuFileInfoService.TryReadFfuSoftwareVersion(ffuFilePath, out version))
			{
				Tracer<FfuController>.WriteWarning("Could not read ffu software version: {0}", new object[]
				{
					ffuFilePath
				});
			}
			IEnumerable<PlatformId> allPlatformIds = new List<PlatformId>
			{
				platformId
			};
			if (!this.logics.FfuFileInfoService.TryReadAllFfuPlatformIds(ffuFilePath, out allPlatformIds))
			{
				Tracer<FfuController>.WriteWarning("Could not read ffu platform ids list: {0}", new object[]
				{
					ffuFilePath
				});
			}
			base.EventAggregator.Publish<FfuFilePlatformIdMessage>(new FfuFilePlatformIdMessage(platformId, version)
			{
				AllPlatformIds = allPlatformIds
			});
		}

		// Token: 0x040000D5 RID: 213
		private readonly LogicContext logics;
	}
}

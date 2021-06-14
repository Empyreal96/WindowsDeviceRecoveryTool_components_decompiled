using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Net;
using Microsoft.WindowsDeviceRecoveryTool.BusinessLogic.Services;
using Microsoft.WindowsDeviceRecoveryTool.Common.Tracing;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Contracts;
using Microsoft.WindowsDeviceRecoveryTool.LogicCommon.Services;
using Microsoft.WindowsDeviceRecoveryTool.LumiaAdaptation.Services;

namespace Microsoft.WindowsDeviceRecoveryTool.BusinessLogic
{
	// Token: 0x02000002 RID: 2
	[Export]
	public class LogicContext : IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[ImportingConstructor]
		public LogicContext(CompositionContainer container)
		{
			try
			{
				container.SatisfyImportsOnce(this);
				this.ffuFileInfoService = container.GetExportedValue<FfuFileInfoService>();
				this.lumiaAdaptation = container.GetExportedValue<LumiaAdaptation>();
				this.adaptationManager = container.GetExportedValue<AdaptationManager>();
				this.autoUpdateService = container.GetExportedValue<AutoUpdateService>();
				this.reportingService = container.GetExportedValue<ReportingService>();
				this.msrReportingService = container.GetExportedValue<MsrReportingService>();
				this.msrReportingService.ManufacturerDataProvider = this.AdaptationManager;
				this.thor2Service = container.GetExportedValue<Thor2Service>();
				this.InitializeAdaptationManager();
			}
			catch (Exception error)
			{
				Tracer<LogicContext>.WriteError(error);
				throw;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020FC File Offset: 0x000002FC
		~LogicContext()
		{
			this.Dispose(false);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002130 File Offset: 0x00000330
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002147 File Offset: 0x00000347
		[ImportMany]
		public IEnumerable<Lazy<IAdaptation, IExportAdaptationMetadata>> AdaptationsWithMetadata { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002150 File Offset: 0x00000350
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002167 File Offset: 0x00000367
		[ImportMany]
		private IEnumerable<Lazy<IUseProxy>> UsingProxyServices { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002170 File Offset: 0x00000370
		public FfuFileInfoService FfuFileInfoService
		{
			get
			{
				FfuFileInfoService result;
				lock (this.ffuFileInfoService)
				{
					result = this.ffuFileInfoService;
				}
				return result;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021BC File Offset: 0x000003BC
		public LumiaAdaptation LumiaAdaptation
		{
			get
			{
				LumiaAdaptation result;
				lock (this.lumiaAdaptation)
				{
					result = this.lumiaAdaptation;
				}
				return result;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002208 File Offset: 0x00000408
		public AdaptationManager AdaptationManager
		{
			get
			{
				AdaptationManager result;
				lock (this.adaptationManager)
				{
					result = this.adaptationManager;
				}
				return result;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002254 File Offset: 0x00000454
		public ReportingService ReportingService
		{
			get
			{
				ReportingService result;
				lock (this.reportingService)
				{
					result = this.reportingService;
				}
				return result;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000022A0 File Offset: 0x000004A0
		public MsrReportingService MsrReportingService
		{
			get
			{
				MsrReportingService result;
				lock (this.msrReportingService)
				{
					result = this.msrReportingService;
				}
				return result;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000022EC File Offset: 0x000004EC
		public AutoUpdateService AutoUpdateService
		{
			get
			{
				AutoUpdateService result;
				lock (this.autoUpdateService)
				{
					result = this.autoUpdateService;
				}
				return result;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002338 File Offset: 0x00000538
		public Thor2Service Thor2Service
		{
			get
			{
				Thor2Service result;
				lock (this.thor2Service)
				{
					result = this.thor2Service;
				}
				return result;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002384 File Offset: 0x00000584
		private void InitializeAdaptationManager()
		{
			this.AdaptationManager.AddAdaptation(this.lumiaAdaptation);
			foreach (Lazy<IAdaptation, IExportAdaptationMetadata> lazy in this.AdaptationsWithMetadata)
			{
				this.AdaptationManager.AddAdaptation((BaseAdaptation)lazy.Value);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002400 File Offset: 0x00000600
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002414 File Offset: 0x00000614
		public void SetProxy(IWebProxy proxy)
		{
			foreach (Lazy<IUseProxy> lazy in this.UsingProxyServices)
			{
				lazy.Value.SetProxy(proxy);
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002474 File Offset: 0x00000674
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.AdaptationManager.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly FfuFileInfoService ffuFileInfoService;

		// Token: 0x04000002 RID: 2
		private readonly LumiaAdaptation lumiaAdaptation;

		// Token: 0x04000003 RID: 3
		private readonly AdaptationManager adaptationManager;

		// Token: 0x04000004 RID: 4
		private readonly AutoUpdateService autoUpdateService;

		// Token: 0x04000005 RID: 5
		private readonly ReportingService reportingService;

		// Token: 0x04000006 RID: 6
		private readonly MsrReportingService msrReportingService;

		// Token: 0x04000007 RID: 7
		private readonly Thor2Service thor2Service;

		// Token: 0x04000008 RID: 8
		private bool disposed;
	}
}

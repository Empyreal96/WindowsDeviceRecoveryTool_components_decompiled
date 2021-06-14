using System;
using System.ComponentModel.Composition;
using Microsoft.WindowsDeviceRecoveryTool.Common;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x0200004D RID: 77
	[Export]
	public class BaseViewModel : NotificationObject, IDisposable, ICanHandle
	{
		// Token: 0x06000296 RID: 662 RVA: 0x0000F72F File Offset: 0x0000D92F
		protected BaseViewModel()
		{
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000F744 File Offset: 0x0000D944
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000F75B File Offset: 0x0000D95B
		public bool IsStarted { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000F764 File Offset: 0x0000D964
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000F77C File Offset: 0x0000D97C
		[Import]
		public ICommandRepository Commands
		{
			get
			{
				return this.commands;
			}
			private set
			{
				base.SetValue<ICommandRepository>(() => this.Commands, ref this.commands, value);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000F7CC File Offset: 0x0000D9CC
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000F7E4 File Offset: 0x0000D9E4
		[Import]
		public EventAggregator EventAggregator
		{
			get
			{
				return this.eventAggregator;
			}
			private set
			{
				this.eventAggregator = value;
				if (this.eventAggregator != null)
				{
					this.eventAggregator.Subscribe(this);
				}
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000F818 File Offset: 0x0000DA18
		public virtual string PreviousStateName
		{
			get
			{
				return "PreviousState";
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F82F File Offset: 0x0000DA2F
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000F844 File Offset: 0x0000DA44
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.ReleaseManagedObjects();
				}
				this.ReleaseUnmanagedObjects();
				this.disposed = true;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000F881 File Offset: 0x0000DA81
		public virtual void OnStarted()
		{
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000F884 File Offset: 0x0000DA84
		public virtual void OnStopped()
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000F887 File Offset: 0x0000DA87
		protected virtual void ReleaseManagedObjects()
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000F88A File Offset: 0x0000DA8A
		protected virtual void ReleaseUnmanagedObjects()
		{
		}

		// Token: 0x04000101 RID: 257
		private ICommandRepository commands;

		// Token: 0x04000102 RID: 258
		private EventAggregator eventAggregator;

		// Token: 0x04000103 RID: 259
		private bool disposed = false;
	}
}

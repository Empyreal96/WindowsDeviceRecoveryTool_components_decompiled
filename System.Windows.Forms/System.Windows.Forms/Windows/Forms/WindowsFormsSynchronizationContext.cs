using System;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Provides a synchronization context for the Windows Forms application model. </summary>
	// Token: 0x02000436 RID: 1078
	public sealed class WindowsFormsSynchronizationContext : SynchronizationContext, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.WindowsFormsSynchronizationContext" /> class. </summary>
		// Token: 0x06004B02 RID: 19202 RVA: 0x00135DC8 File Offset: 0x00133FC8
		public WindowsFormsSynchronizationContext()
		{
			this.DestinationThread = Thread.CurrentThread;
			Application.ThreadContext threadContext = Application.ThreadContext.FromCurrent();
			if (threadContext != null)
			{
				this.controlToSendTo = threadContext.MarshalingControl;
			}
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x00135DFB File Offset: 0x00133FFB
		private WindowsFormsSynchronizationContext(Control marshalingControl, Thread destinationThread)
		{
			this.controlToSendTo = marshalingControl;
			this.DestinationThread = destinationThread;
		}

		// Token: 0x1700124B RID: 4683
		// (get) Token: 0x06004B04 RID: 19204 RVA: 0x00135E11 File Offset: 0x00134011
		// (set) Token: 0x06004B05 RID: 19205 RVA: 0x00135E3A File Offset: 0x0013403A
		private Thread DestinationThread
		{
			get
			{
				if (this.destinationThreadRef != null && this.destinationThreadRef.IsAlive)
				{
					return this.destinationThreadRef.Target as Thread;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.destinationThreadRef = new WeakReference(value);
				}
			}
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Windows.Forms.WindowsFormsSynchronizationContext" />. </summary>
		// Token: 0x06004B06 RID: 19206 RVA: 0x00135E4B File Offset: 0x0013404B
		public void Dispose()
		{
			if (this.controlToSendTo != null)
			{
				if (!this.controlToSendTo.IsDisposed)
				{
					this.controlToSendTo.Dispose();
				}
				this.controlToSendTo = null;
			}
		}

		/// <summary>Dispatches a synchronous message to a synchronization context</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <exception cref="T:System.ComponentModel.InvalidAsynchronousStateException">The destination thread no longer exists.</exception>
		// Token: 0x06004B07 RID: 19207 RVA: 0x00135E74 File Offset: 0x00134074
		public override void Send(SendOrPostCallback d, object state)
		{
			Thread destinationThread = this.DestinationThread;
			if (destinationThread == null || !destinationThread.IsAlive)
			{
				throw new InvalidAsynchronousStateException(SR.GetString("ThreadNoLongerValid"));
			}
			if (this.controlToSendTo != null)
			{
				this.controlToSendTo.Invoke(d, new object[]
				{
					state
				});
			}
		}

		/// <summary>Dispatches an asynchronous message to a synchronization context.</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		// Token: 0x06004B08 RID: 19208 RVA: 0x00135EC2 File Offset: 0x001340C2
		public override void Post(SendOrPostCallback d, object state)
		{
			if (this.controlToSendTo != null)
			{
				this.controlToSendTo.BeginInvoke(d, new object[]
				{
					state
				});
			}
		}

		/// <summary>Copies the synchronization context.</summary>
		/// <returns>A copy of the synchronization context.</returns>
		// Token: 0x06004B09 RID: 19209 RVA: 0x00135EE3 File Offset: 0x001340E3
		public override SynchronizationContext CreateCopy()
		{
			return new WindowsFormsSynchronizationContext(this.controlToSendTo, this.DestinationThread);
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.WindowsFormsSynchronizationContext" /> is installed when a control is created.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.WindowsFormsSynchronizationContext" /> is installed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06004B0A RID: 19210 RVA: 0x00135EF6 File Offset: 0x001340F6
		// (set) Token: 0x06004B0B RID: 19211 RVA: 0x00135F00 File Offset: 0x00134100
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static bool AutoInstall
		{
			get
			{
				return !WindowsFormsSynchronizationContext.dontAutoInstall;
			}
			set
			{
				WindowsFormsSynchronizationContext.dontAutoInstall = !value;
			}
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x00135F0C File Offset: 0x0013410C
		internal static void InstallIfNeeded()
		{
			if (!WindowsFormsSynchronizationContext.AutoInstall || WindowsFormsSynchronizationContext.inSyncContextInstallation)
			{
				return;
			}
			if (SynchronizationContext.Current == null)
			{
				WindowsFormsSynchronizationContext.previousSyncContext = null;
			}
			if (WindowsFormsSynchronizationContext.previousSyncContext != null)
			{
				return;
			}
			WindowsFormsSynchronizationContext.inSyncContextInstallation = true;
			try
			{
				SynchronizationContext synchronizationContext = AsyncOperationManager.SynchronizationContext;
				if (synchronizationContext == null || synchronizationContext.GetType() == typeof(SynchronizationContext))
				{
					WindowsFormsSynchronizationContext.previousSyncContext = synchronizationContext;
					new PermissionSet(PermissionState.Unrestricted).Assert();
					try
					{
						AsyncOperationManager.SynchronizationContext = new WindowsFormsSynchronizationContext();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			finally
			{
				WindowsFormsSynchronizationContext.inSyncContextInstallation = false;
			}
		}

		/// <summary>Uninstalls the currently installed <see cref="T:System.Windows.Forms.WindowsFormsSynchronizationContext" /> and replaces it with the previously installed context.</summary>
		// Token: 0x06004B0D RID: 19213 RVA: 0x00135FAC File Offset: 0x001341AC
		public static void Uninstall()
		{
			WindowsFormsSynchronizationContext.Uninstall(true);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x00135FB4 File Offset: 0x001341B4
		internal static void Uninstall(bool turnOffAutoInstall)
		{
			if (WindowsFormsSynchronizationContext.AutoInstall)
			{
				WindowsFormsSynchronizationContext windowsFormsSynchronizationContext = AsyncOperationManager.SynchronizationContext as WindowsFormsSynchronizationContext;
				if (windowsFormsSynchronizationContext != null)
				{
					try
					{
						new PermissionSet(PermissionState.Unrestricted).Assert();
						if (WindowsFormsSynchronizationContext.previousSyncContext == null)
						{
							AsyncOperationManager.SynchronizationContext = new SynchronizationContext();
						}
						else
						{
							AsyncOperationManager.SynchronizationContext = WindowsFormsSynchronizationContext.previousSyncContext;
						}
					}
					finally
					{
						WindowsFormsSynchronizationContext.previousSyncContext = null;
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			if (turnOffAutoInstall)
			{
				WindowsFormsSynchronizationContext.AutoInstall = false;
			}
		}

		// Token: 0x04002756 RID: 10070
		private Control controlToSendTo;

		// Token: 0x04002757 RID: 10071
		private WeakReference destinationThreadRef;

		// Token: 0x04002758 RID: 10072
		[ThreadStatic]
		private static bool dontAutoInstall;

		// Token: 0x04002759 RID: 10073
		[ThreadStatic]
		private static bool inSyncContextInstallation;

		// Token: 0x0400275A RID: 10074
		[ThreadStatic]
		private static SynchronizationContext previousSyncContext;
	}
}

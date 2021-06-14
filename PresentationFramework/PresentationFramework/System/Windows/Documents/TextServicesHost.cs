using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Threading;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000417 RID: 1047
	internal class TextServicesHost : DispatcherObject
	{
		// Token: 0x06003C80 RID: 15488 RVA: 0x00117B41 File Offset: 0x00115D41
		internal TextServicesHost()
		{
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x00117B49 File Offset: 0x00115D49
		[SecurityCritical]
		internal void RegisterTextStore(TextStore textstore)
		{
			this._RegisterTextStore(textstore);
			this._thread = Thread.CurrentThread;
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x00117B5D File Offset: 0x00115D5D
		[SecurityCritical]
		internal void UnregisterTextStore(TextStore textstore, bool finalizer)
		{
			if (!finalizer)
			{
				this.OnUnregisterTextStore(textstore);
				return;
			}
			if (!this._isDispatcherShutdownFinished)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnUnregisterTextStore), textstore);
			}
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x00117B8E File Offset: 0x00115D8E
		[SecurityCritical]
		internal void RegisterWinEventSink(TextStore textstore)
		{
			if (this._winEvent == null)
			{
				this._winEvent = new MoveSizeWinEventHandler();
				this._winEvent.Start();
			}
			this._winEvent.RegisterTextStore(textstore);
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x00117BBA File Offset: 0x00115DBA
		[SecurityCritical]
		internal void UnregisterWinEventSink(TextStore textstore)
		{
			this._winEvent.UnregisterTextStore(textstore);
			if (this._winEvent.TextStoreCount == 0)
			{
				this._winEvent.Stop();
				this._winEvent.Clear();
				this._winEvent = null;
			}
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x00117BF4 File Offset: 0x00115DF4
		[SecurityCritical]
		internal static void StartTransitoryExtension(TextStore textstore)
		{
			UnsafeNativeMethods.ITfCompartmentMgr tfCompartmentMgr = textstore.DocumentManager as UnsafeNativeMethods.ITfCompartmentMgr;
			Guid guid = UnsafeNativeMethods.GUID_COMPARTMENT_TRANSITORYEXTENSION;
			UnsafeNativeMethods.ITfCompartment tfCompartment;
			tfCompartmentMgr.GetCompartment(ref guid, out tfCompartment);
			object obj = 2;
			tfCompartment.SetValue(0, ref obj);
			guid = UnsafeNativeMethods.IID_ITfTransitoryExtensionSink;
			UnsafeNativeMethods.ITfSource tfSource = textstore.DocumentManager as UnsafeNativeMethods.ITfSource;
			if (tfSource != null)
			{
				int transitoryExtensionSinkCookie;
				tfSource.AdviseSink(ref guid, textstore, out transitoryExtensionSinkCookie);
				textstore.TransitoryExtensionSinkCookie = transitoryExtensionSinkCookie;
			}
			Marshal.ReleaseComObject(tfCompartment);
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x00117C60 File Offset: 0x00115E60
		[SecurityCritical]
		internal static void StopTransitoryExtension(TextStore textstore)
		{
			UnsafeNativeMethods.ITfCompartmentMgr tfCompartmentMgr = textstore.DocumentManager as UnsafeNativeMethods.ITfCompartmentMgr;
			if (textstore.TransitoryExtensionSinkCookie != -1)
			{
				UnsafeNativeMethods.ITfSource tfSource = textstore.DocumentManager as UnsafeNativeMethods.ITfSource;
				if (tfSource != null)
				{
					tfSource.UnadviseSink(textstore.TransitoryExtensionSinkCookie);
				}
				textstore.TransitoryExtensionSinkCookie = -1;
			}
			Guid guid_COMPARTMENT_TRANSITORYEXTENSION = UnsafeNativeMethods.GUID_COMPARTMENT_TRANSITORYEXTENSION;
			UnsafeNativeMethods.ITfCompartment tfCompartment;
			tfCompartmentMgr.GetCompartment(ref guid_COMPARTMENT_TRANSITORYEXTENSION, out tfCompartment);
			object obj = 0;
			tfCompartment.SetValue(0, ref obj);
			Marshal.ReleaseComObject(tfCompartment);
		}

		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x06003C87 RID: 15495 RVA: 0x00117CD0 File Offset: 0x00115ED0
		internal static TextServicesHost Current
		{
			get
			{
				TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
				if (threadLocalStore.TextServicesHost == null)
				{
					threadLocalStore.TextServicesHost = new TextServicesHost();
				}
				return threadLocalStore.TextServicesHost;
			}
		}

		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x06003C88 RID: 15496 RVA: 0x00117CFC File Offset: 0x00115EFC
		internal UnsafeNativeMethods.ITfThreadMgr ThreadManager
		{
			[SecurityCritical]
			get
			{
				if (this._threadManager == null)
				{
					return null;
				}
				return this._threadManager.Value;
			}
		}

		// Token: 0x06003C89 RID: 15497 RVA: 0x00117D14 File Offset: 0x00115F14
		[SecurityCritical]
		private object OnUnregisterTextStore(object arg)
		{
			if (this._threadManager == null || this._threadManager.Value == null)
			{
				return null;
			}
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			TextStore textStore = (TextStore)arg;
			if (textStore.ThreadFocusCookie != -1)
			{
				UnsafeNativeMethods.ITfSource tfSource = this._threadManager.Value as UnsafeNativeMethods.ITfSource;
				tfSource.UnadviseSink(textStore.ThreadFocusCookie);
				textStore.ThreadFocusCookie = -1;
			}
			UnsafeNativeMethods.ITfContext tfContext;
			textStore.DocumentManager.GetBase(out tfContext);
			if (tfContext != null)
			{
				if (textStore.EditSinkCookie != -1)
				{
					UnsafeNativeMethods.ITfSource tfSource = tfContext as UnsafeNativeMethods.ITfSource;
					tfSource.UnadviseSink(textStore.EditSinkCookie);
					textStore.EditSinkCookie = -1;
				}
				Marshal.ReleaseComObject(tfContext);
			}
			securityPermission.Assert();
			try
			{
				textStore.DocumentManager.Pop(UnsafeNativeMethods.PopFlags.TF_POPF_ALL);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			Marshal.ReleaseComObject(textStore.DocumentManager);
			textStore.DocumentManager = null;
			this._registeredtextstorecount--;
			if (this._isDispatcherShutdownFinished && this._registeredtextstorecount == 0)
			{
				this.DeactivateThreadManager();
			}
			return null;
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x00117E10 File Offset: 0x00116010
		[SecurityCritical]
		private void OnDispatcherShutdownFinished(object sender, EventArgs args)
		{
			base.Dispatcher.ShutdownFinished -= this.OnDispatcherShutdownFinished;
			if (this._registeredtextstorecount == 0)
			{
				this.DeactivateThreadManager();
			}
			this._isDispatcherShutdownFinished = true;
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00117E40 File Offset: 0x00116040
		[SecurityCritical]
		private void _RegisterTextStore(TextStore textstore)
		{
			int editCookie = -1;
			int threadFocusCookie = -1;
			int editSinkCookie = -1;
			if (this._threadManager == null)
			{
				this._threadManager = new SecurityCriticalDataClass<UnsafeNativeMethods.ITfThreadMgr>(TextServicesLoader.Load());
				if (this._threadManager.Value == null)
				{
					this._threadManager = null;
					return;
				}
				int value;
				this._threadManager.Value.Activate(out value);
				this._clientId = new SecurityCriticalData<int>(value);
				base.Dispatcher.ShutdownFinished += this.OnDispatcherShutdownFinished;
			}
			UnsafeNativeMethods.ITfDocumentMgr tfDocumentMgr;
			this._threadManager.Value.CreateDocumentMgr(out tfDocumentMgr);
			UnsafeNativeMethods.ITfContext tfContext;
			tfDocumentMgr.CreateContext(this._clientId.Value, (UnsafeNativeMethods.CreateContextFlags)0, textstore, out tfContext, out editCookie);
			tfDocumentMgr.Push(tfContext);
			if (textstore != null)
			{
				Guid guid = UnsafeNativeMethods.IID_ITfThreadFocusSink;
				UnsafeNativeMethods.ITfSource tfSource = this._threadManager.Value as UnsafeNativeMethods.ITfSource;
				tfSource.AdviseSink(ref guid, textstore, out threadFocusCookie);
			}
			if (textstore != null)
			{
				Guid guid = UnsafeNativeMethods.IID_ITfTextEditSink;
				UnsafeNativeMethods.ITfSource tfSource = tfContext as UnsafeNativeMethods.ITfSource;
				tfSource.AdviseSink(ref guid, textstore, out editSinkCookie);
			}
			Marshal.ReleaseComObject(tfContext);
			textstore.DocumentManager = tfDocumentMgr;
			textstore.ThreadFocusCookie = threadFocusCookie;
			textstore.EditSinkCookie = editSinkCookie;
			textstore.EditCookie = editCookie;
			if (textstore.UiScope.IsKeyboardFocused)
			{
				textstore.OnGotFocus();
			}
			this._registeredtextstorecount++;
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x00117F70 File Offset: 0x00116170
		[SecurityCritical]
		private void DeactivateThreadManager()
		{
			if (this._threadManager != null)
			{
				if (this._threadManager.Value != null)
				{
					if (this._thread == Thread.CurrentThread || Environment.OSVersion.Version.Major >= 6)
					{
						this._threadManager.Value.Deactivate();
					}
					Marshal.ReleaseComObject(this._threadManager.Value);
				}
				this._threadManager = null;
			}
			TextEditorThreadLocalStore threadLocalStore = TextEditor._ThreadLocalStore;
			threadLocalStore.TextServicesHost = null;
		}

		// Token: 0x04002625 RID: 9765
		private int _registeredtextstorecount;

		// Token: 0x04002626 RID: 9766
		private SecurityCriticalData<int> _clientId;

		// Token: 0x04002627 RID: 9767
		[SecurityCritical]
		private SecurityCriticalDataClass<UnsafeNativeMethods.ITfThreadMgr> _threadManager;

		// Token: 0x04002628 RID: 9768
		private bool _isDispatcherShutdownFinished;

		// Token: 0x04002629 RID: 9769
		private MoveSizeWinEventHandler _winEvent;

		// Token: 0x0400262A RID: 9770
		private Thread _thread;
	}
}

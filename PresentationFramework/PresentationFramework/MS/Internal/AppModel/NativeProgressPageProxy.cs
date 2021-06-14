using System;
using System.Security;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x02000796 RID: 1942
	internal class NativeProgressPageProxy : IProgressPage2, IProgressPage
	{
		// Token: 0x060079CC RID: 31180 RVA: 0x002287E7 File Offset: 0x002269E7
		[SecurityCritical]
		internal NativeProgressPageProxy(INativeProgressPage npp)
		{
			this._npp = npp;
		}

		// Token: 0x060079CD RID: 31181 RVA: 0x002287F8 File Offset: 0x002269F8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void ShowProgressMessage(string message)
		{
			HRESULT hresult = this._npp.ShowProgressMessage(message);
		}

		// Token: 0x17001CBB RID: 7355
		// (get) Token: 0x060079CF RID: 31183 RVA: 0x0003E384 File Offset: 0x0003C584
		// (set) Token: 0x060079CE RID: 31182 RVA: 0x00002137 File Offset: 0x00000337
		public Uri DeploymentPath
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x17001CBC RID: 7356
		// (get) Token: 0x060079D1 RID: 31185 RVA: 0x0003E384 File Offset: 0x0003C584
		// (set) Token: 0x060079D0 RID: 31184 RVA: 0x00002137 File Offset: 0x00000337
		public DispatcherOperationCallback StopCallback
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
			}
		}

		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x060079D3 RID: 31187 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x060079D2 RID: 31186 RVA: 0x00002137 File Offset: 0x00000337
		public DispatcherOperationCallback RefreshCallback
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x060079D5 RID: 31189 RVA: 0x0003E384 File Offset: 0x0003C584
		// (set) Token: 0x060079D4 RID: 31188 RVA: 0x00228814 File Offset: 0x00226A14
		public string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				HRESULT hresult = this._npp.SetApplicationName(value);
			}
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x060079D7 RID: 31191 RVA: 0x0003E384 File Offset: 0x0003C584
		// (set) Token: 0x060079D6 RID: 31190 RVA: 0x00228830 File Offset: 0x00226A30
		public string PublisherName
		{
			get
			{
				throw new NotImplementedException();
			}
			[SecurityCritical]
			[SecurityTreatAsSafe]
			set
			{
				HRESULT hresult = this._npp.SetPublisherName(value);
			}
		}

		// Token: 0x060079D8 RID: 31192 RVA: 0x0022884C File Offset: 0x00226A4C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public void UpdateProgress(long bytesDownloaded, long bytesTotal)
		{
			HRESULT hresult = this._npp.OnDownloadProgress((ulong)bytesDownloaded, (ulong)bytesTotal);
		}

		// Token: 0x0400399F RID: 14751
		[SecurityCritical]
		private INativeProgressPage _npp;
	}
}

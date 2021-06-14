using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B4 RID: 1972
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
	[ComImport]
	internal interface IFileSaveDialog : IFileDialog, IModalWindow
	{
		// Token: 0x06007AF3 RID: 31475
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x06007AF4 RID: 31476
		void SetFileTypes(uint cFileTypes, [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x06007AF5 RID: 31477
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x06007AF6 RID: 31478
		uint GetFileTypeIndex();

		// Token: 0x06007AF7 RID: 31479
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x06007AF8 RID: 31480
		void Unadvise(uint dwCookie);

		// Token: 0x06007AF9 RID: 31481
		void SetOptions(FOS fos);

		// Token: 0x06007AFA RID: 31482
		FOS GetOptions();

		// Token: 0x06007AFB RID: 31483
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x06007AFC RID: 31484
		void SetFolder(IShellItem psi);

		// Token: 0x06007AFD RID: 31485
		IShellItem GetFolder();

		// Token: 0x06007AFE RID: 31486
		IShellItem GetCurrentSelection();

		// Token: 0x06007AFF RID: 31487
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06007B00 RID: 31488
		[return: MarshalAs(UnmanagedType.LPWStr)]
		void GetFileName();

		// Token: 0x06007B01 RID: 31489
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x06007B02 RID: 31490
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x06007B03 RID: 31491
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x06007B04 RID: 31492
		IShellItem GetResult();

		// Token: 0x06007B05 RID: 31493
		void AddPlace(IShellItem psi, FDAP fdcp);

		// Token: 0x06007B06 RID: 31494
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x06007B07 RID: 31495
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x06007B08 RID: 31496
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x06007B09 RID: 31497
		void ClearClientData();

		// Token: 0x06007B0A RID: 31498
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);

		// Token: 0x06007B0B RID: 31499
		void SetSaveAsItem(IShellItem psi);

		// Token: 0x06007B0C RID: 31500
		void SetProperties([MarshalAs(UnmanagedType.Interface)] [In] object pStore);

		// Token: 0x06007B0D RID: 31501
		void SetCollectedProperties([MarshalAs(UnmanagedType.Interface)] [In] object pList, [In] int fAppendDefault);

		// Token: 0x06007B0E RID: 31502
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetProperties();

		// Token: 0x06007B0F RID: 31503
		void ApplyProperties(IShellItem psi, [MarshalAs(UnmanagedType.Interface)] object pStore, [In] ref IntPtr hwnd, [MarshalAs(UnmanagedType.Interface)] object pSink);
	}
}

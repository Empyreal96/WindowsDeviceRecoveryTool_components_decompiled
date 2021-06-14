using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000241 RID: 577
	internal static class FileDialogNative
	{
		// Token: 0x020005D1 RID: 1489
		[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
		[CoClass(typeof(FileDialogNative.FileOpenDialogRCW))]
		[ComImport]
		internal interface NativeFileOpenDialog : FileDialogNative.IFileOpenDialog, FileDialogNative.IFileDialog
		{
		}

		// Token: 0x020005D2 RID: 1490
		[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
		[CoClass(typeof(FileDialogNative.FileSaveDialogRCW))]
		[ComImport]
		internal interface NativeFileSaveDialog : FileDialogNative.IFileSaveDialog, FileDialogNative.IFileDialog
		{
		}

		// Token: 0x020005D3 RID: 1491
		[ClassInterface(ClassInterfaceType.None)]
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
		[ComImport]
		internal class FileOpenDialogRCW
		{
			// Token: 0x06005A79 RID: 23161
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern FileOpenDialogRCW();
		}

		// Token: 0x020005D4 RID: 1492
		[ClassInterface(ClassInterfaceType.None)]
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[Guid("C0B4E2F3-BA21-4773-8DBA-335EC946EB8B")]
		[ComImport]
		internal class FileSaveDialogRCW
		{
			// Token: 0x06005A7A RID: 23162
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern FileSaveDialogRCW();
		}

		// Token: 0x020005D5 RID: 1493
		internal class IIDGuid
		{
			// Token: 0x06005A7B RID: 23163 RVA: 0x000027DB File Offset: 0x000009DB
			private IIDGuid()
			{
			}

			// Token: 0x0400396D RID: 14701
			internal const string IModalWindow = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802";

			// Token: 0x0400396E RID: 14702
			internal const string IFileDialog = "42f85136-db7e-439c-85f1-e4075d135fc8";

			// Token: 0x0400396F RID: 14703
			internal const string IFileOpenDialog = "d57c7288-d4ad-4768-be02-9d969532d960";

			// Token: 0x04003970 RID: 14704
			internal const string IFileSaveDialog = "84bccd23-5fde-4cdb-aea4-af64b83d78ab";

			// Token: 0x04003971 RID: 14705
			internal const string IFileDialogEvents = "973510DB-7D7F-452B-8975-74A85828D354";

			// Token: 0x04003972 RID: 14706
			internal const string IShellItem = "43826D1E-E718-42EE-BC55-A1E261C37BFE";

			// Token: 0x04003973 RID: 14707
			internal const string IShellItemArray = "B63EA76D-1F85-456F-A19C-48159EFA858B";
		}

		// Token: 0x020005D6 RID: 1494
		internal class CLSIDGuid
		{
			// Token: 0x06005A7C RID: 23164 RVA: 0x000027DB File Offset: 0x000009DB
			private CLSIDGuid()
			{
			}

			// Token: 0x04003974 RID: 14708
			internal const string FileOpenDialog = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7";

			// Token: 0x04003975 RID: 14709
			internal const string FileSaveDialog = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B";
		}

		// Token: 0x020005D7 RID: 1495
		[Guid("b4db1657-70d7-485e-8e3e-6fcb5a5c1802")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IModalWindow
		{
			// Token: 0x06005A7D RID: 23165
			[PreserveSig]
			int Show([In] IntPtr parent);
		}

		// Token: 0x020005D8 RID: 1496
		internal enum SIATTRIBFLAGS
		{
			// Token: 0x04003977 RID: 14711
			SIATTRIBFLAGS_AND = 1,
			// Token: 0x04003978 RID: 14712
			SIATTRIBFLAGS_OR,
			// Token: 0x04003979 RID: 14713
			SIATTRIBFLAGS_APPCOMPAT
		}

		// Token: 0x020005D9 RID: 1497
		[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IShellItemArray
		{
			// Token: 0x06005A7E RID: 23166
			void BindToHandler([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

			// Token: 0x06005A7F RID: 23167
			void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06005A80 RID: 23168
			void GetPropertyDescriptionList([In] ref FileDialogNative.PROPERTYKEY keyType, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06005A81 RID: 23169
			void GetAttributes([In] FileDialogNative.SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);

			// Token: 0x06005A82 RID: 23170
			void GetCount(out uint pdwNumItems);

			// Token: 0x06005A83 RID: 23171
			void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005A84 RID: 23172
			void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
		}

		// Token: 0x020005DA RID: 1498
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct PROPERTYKEY
		{
			// Token: 0x0400397A RID: 14714
			internal Guid fmtid;

			// Token: 0x0400397B RID: 14715
			internal uint pid;
		}

		// Token: 0x020005DB RID: 1499
		[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileDialog
		{
			// Token: 0x06005A85 RID: 23173
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06005A86 RID: 23174
			void SetFileTypes([In] uint cFileTypes, [MarshalAs(UnmanagedType.LPArray)] [In] FileDialogNative.COMDLG_FILTERSPEC[] rgFilterSpec);

			// Token: 0x06005A87 RID: 23175
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06005A88 RID: 23176
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06005A89 RID: 23177
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06005A8A RID: 23178
			void Unadvise([In] uint dwCookie);

			// Token: 0x06005A8B RID: 23179
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06005A8C RID: 23180
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06005A8D RID: 23181
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005A8E RID: 23182
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005A8F RID: 23183
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005A90 RID: 23184
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005A91 RID: 23185
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06005A92 RID: 23186
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06005A93 RID: 23187
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06005A94 RID: 23188
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06005A95 RID: 23189
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06005A96 RID: 23190
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005A97 RID: 23191
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, int alignment);

			// Token: 0x06005A98 RID: 23192
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06005A99 RID: 23193
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06005A9A RID: 23194
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06005A9B RID: 23195
			void ClearClientData();

			// Token: 0x06005A9C RID: 23196
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
		}

		// Token: 0x020005DC RID: 1500
		[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileOpenDialog : FileDialogNative.IFileDialog
		{
			// Token: 0x06005A9D RID: 23197
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06005A9E RID: 23198
			void SetFileTypes([In] uint cFileTypes, [In] ref FileDialogNative.COMDLG_FILTERSPEC rgFilterSpec);

			// Token: 0x06005A9F RID: 23199
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06005AA0 RID: 23200
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06005AA1 RID: 23201
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06005AA2 RID: 23202
			void Unadvise([In] uint dwCookie);

			// Token: 0x06005AA3 RID: 23203
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06005AA4 RID: 23204
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06005AA5 RID: 23205
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005AA6 RID: 23206
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005AA7 RID: 23207
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AA8 RID: 23208
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AA9 RID: 23209
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06005AAA RID: 23210
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06005AAB RID: 23211
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06005AAC RID: 23212
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06005AAD RID: 23213
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06005AAE RID: 23214
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AAF RID: 23215
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, FileDialogCustomPlace fdcp);

			// Token: 0x06005AB0 RID: 23216
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06005AB1 RID: 23217
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06005AB2 RID: 23218
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06005AB3 RID: 23219
			void ClearClientData();

			// Token: 0x06005AB4 RID: 23220
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

			// Token: 0x06005AB5 RID: 23221
			void GetResults([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItemArray ppenum);

			// Token: 0x06005AB6 RID: 23222
			void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItemArray ppsai);
		}

		// Token: 0x020005DD RID: 1501
		[Guid("84bccd23-5fde-4cdb-aea4-af64b83d78ab")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileSaveDialog : FileDialogNative.IFileDialog
		{
			// Token: 0x06005AB7 RID: 23223
			[PreserveSig]
			int Show([In] IntPtr parent);

			// Token: 0x06005AB8 RID: 23224
			void SetFileTypes([In] uint cFileTypes, [In] ref FileDialogNative.COMDLG_FILTERSPEC rgFilterSpec);

			// Token: 0x06005AB9 RID: 23225
			void SetFileTypeIndex([In] uint iFileType);

			// Token: 0x06005ABA RID: 23226
			void GetFileTypeIndex(out uint piFileType);

			// Token: 0x06005ABB RID: 23227
			void Advise([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialogEvents pfde, out uint pdwCookie);

			// Token: 0x06005ABC RID: 23228
			void Unadvise([In] uint dwCookie);

			// Token: 0x06005ABD RID: 23229
			void SetOptions([In] FileDialogNative.FOS fos);

			// Token: 0x06005ABE RID: 23230
			void GetOptions(out FileDialogNative.FOS pfos);

			// Token: 0x06005ABF RID: 23231
			void SetDefaultFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005AC0 RID: 23232
			void SetFolder([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005AC1 RID: 23233
			void GetFolder([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AC2 RID: 23234
			void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AC3 RID: 23235
			void SetFileName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszName);

			// Token: 0x06005AC4 RID: 23236
			void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

			// Token: 0x06005AC5 RID: 23237
			void SetTitle([MarshalAs(UnmanagedType.LPWStr)] [In] string pszTitle);

			// Token: 0x06005AC6 RID: 23238
			void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszText);

			// Token: 0x06005AC7 RID: 23239
			void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] [In] string pszLabel);

			// Token: 0x06005AC8 RID: 23240
			void GetResult([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005AC9 RID: 23241
			void AddPlace([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, FileDialogCustomPlace fdcp);

			// Token: 0x06005ACA RID: 23242
			void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] [In] string pszDefaultExtension);

			// Token: 0x06005ACB RID: 23243
			void Close([MarshalAs(UnmanagedType.Error)] int hr);

			// Token: 0x06005ACC RID: 23244
			void SetClientGuid([In] ref Guid guid);

			// Token: 0x06005ACD RID: 23245
			void ClearClientData();

			// Token: 0x06005ACE RID: 23246
			void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

			// Token: 0x06005ACF RID: 23247
			void SetSaveAsItem([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi);

			// Token: 0x06005AD0 RID: 23248
			void SetProperties([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pStore);

			// Token: 0x06005AD1 RID: 23249
			void SetCollectedProperties([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pList, [In] int fAppendDefault);

			// Token: 0x06005AD2 RID: 23250
			void GetProperties([MarshalAs(UnmanagedType.Interface)] out IntPtr ppStore);

			// Token: 0x06005AD3 RID: 23251
			void ApplyProperties([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, [MarshalAs(UnmanagedType.Interface)] [In] IntPtr pStore, [ComAliasName("ShellObjects.wireHWND")] [In] ref IntPtr hwnd, [MarshalAs(UnmanagedType.Interface)] [In] IntPtr pSink);
		}

		// Token: 0x020005DE RID: 1502
		[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IFileDialogEvents
		{
			// Token: 0x06005AD4 RID: 23252
			[PreserveSig]
			int OnFileOk([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06005AD5 RID: 23253
			[PreserveSig]
			int OnFolderChanging([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psiFolder);

			// Token: 0x06005AD6 RID: 23254
			void OnFolderChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06005AD7 RID: 23255
			void OnSelectionChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06005AD8 RID: 23256
			void OnShareViolation([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, out FileDialogNative.FDE_SHAREVIOLATION_RESPONSE pResponse);

			// Token: 0x06005AD9 RID: 23257
			void OnTypeChange([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd);

			// Token: 0x06005ADA RID: 23258
			void OnOverwrite([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IFileDialog pfd, [MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, out FileDialogNative.FDE_OVERWRITE_RESPONSE pResponse);
		}

		// Token: 0x020005DF RID: 1503
		[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IShellItem
		{
			// Token: 0x06005ADB RID: 23259
			void BindToHandler([MarshalAs(UnmanagedType.Interface)] [In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

			// Token: 0x06005ADC RID: 23260
			void GetParent([MarshalAs(UnmanagedType.Interface)] out FileDialogNative.IShellItem ppsi);

			// Token: 0x06005ADD RID: 23261
			void GetDisplayName([In] FileDialogNative.SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

			// Token: 0x06005ADE RID: 23262
			void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

			// Token: 0x06005ADF RID: 23263
			void Compare([MarshalAs(UnmanagedType.Interface)] [In] FileDialogNative.IShellItem psi, [In] uint hint, out int piOrder);
		}

		// Token: 0x020005E0 RID: 1504
		internal enum SIGDN : uint
		{
			// Token: 0x0400397D RID: 14717
			SIGDN_NORMALDISPLAY,
			// Token: 0x0400397E RID: 14718
			SIGDN_PARENTRELATIVEPARSING = 2147581953U,
			// Token: 0x0400397F RID: 14719
			SIGDN_DESKTOPABSOLUTEPARSING = 2147647488U,
			// Token: 0x04003980 RID: 14720
			SIGDN_PARENTRELATIVEEDITING = 2147684353U,
			// Token: 0x04003981 RID: 14721
			SIGDN_DESKTOPABSOLUTEEDITING = 2147794944U,
			// Token: 0x04003982 RID: 14722
			SIGDN_FILESYSPATH = 2147844096U,
			// Token: 0x04003983 RID: 14723
			SIGDN_URL = 2147909632U,
			// Token: 0x04003984 RID: 14724
			SIGDN_PARENTRELATIVEFORADDRESSBAR = 2147991553U,
			// Token: 0x04003985 RID: 14725
			SIGDN_PARENTRELATIVE = 2148007937U
		}

		// Token: 0x020005E1 RID: 1505
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal struct COMDLG_FILTERSPEC
		{
			// Token: 0x04003986 RID: 14726
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszName;

			// Token: 0x04003987 RID: 14727
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszSpec;
		}

		// Token: 0x020005E2 RID: 1506
		[Flags]
		internal enum FOS : uint
		{
			// Token: 0x04003989 RID: 14729
			FOS_OVERWRITEPROMPT = 2U,
			// Token: 0x0400398A RID: 14730
			FOS_STRICTFILETYPES = 4U,
			// Token: 0x0400398B RID: 14731
			FOS_NOCHANGEDIR = 8U,
			// Token: 0x0400398C RID: 14732
			FOS_PICKFOLDERS = 32U,
			// Token: 0x0400398D RID: 14733
			FOS_FORCEFILESYSTEM = 64U,
			// Token: 0x0400398E RID: 14734
			FOS_ALLNONSTORAGEITEMS = 128U,
			// Token: 0x0400398F RID: 14735
			FOS_NOVALIDATE = 256U,
			// Token: 0x04003990 RID: 14736
			FOS_ALLOWMULTISELECT = 512U,
			// Token: 0x04003991 RID: 14737
			FOS_PATHMUSTEXIST = 2048U,
			// Token: 0x04003992 RID: 14738
			FOS_FILEMUSTEXIST = 4096U,
			// Token: 0x04003993 RID: 14739
			FOS_CREATEPROMPT = 8192U,
			// Token: 0x04003994 RID: 14740
			FOS_SHAREAWARE = 16384U,
			// Token: 0x04003995 RID: 14741
			FOS_NOREADONLYRETURN = 32768U,
			// Token: 0x04003996 RID: 14742
			FOS_NOTESTFILECREATE = 65536U,
			// Token: 0x04003997 RID: 14743
			FOS_HIDEMRUPLACES = 131072U,
			// Token: 0x04003998 RID: 14744
			FOS_HIDEPINNEDPLACES = 262144U,
			// Token: 0x04003999 RID: 14745
			FOS_NODEREFERENCELINKS = 1048576U,
			// Token: 0x0400399A RID: 14746
			FOS_DONTADDTORECENT = 33554432U,
			// Token: 0x0400399B RID: 14747
			FOS_FORCESHOWHIDDEN = 268435456U,
			// Token: 0x0400399C RID: 14748
			FOS_DEFAULTNOMINIMODE = 536870912U
		}

		// Token: 0x020005E3 RID: 1507
		internal enum FDE_SHAREVIOLATION_RESPONSE
		{
			// Token: 0x0400399E RID: 14750
			FDESVR_DEFAULT,
			// Token: 0x0400399F RID: 14751
			FDESVR_ACCEPT,
			// Token: 0x040039A0 RID: 14752
			FDESVR_REFUSE
		}

		// Token: 0x020005E4 RID: 1508
		internal enum FDE_OVERWRITE_RESPONSE
		{
			// Token: 0x040039A2 RID: 14754
			FDEOR_DEFAULT,
			// Token: 0x040039A3 RID: 14755
			FDEOR_ACCEPT,
			// Token: 0x040039A4 RID: 14756
			FDEOR_REFUSE
		}
	}
}

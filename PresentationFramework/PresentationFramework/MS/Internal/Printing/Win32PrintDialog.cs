using System;
using System.Printing;
using System.Printing.Interop;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Printing
{
	// Token: 0x02000658 RID: 1624
	internal class Win32PrintDialog
	{
		// Token: 0x06006BD2 RID: 27602 RVA: 0x001F119C File Offset: 0x001EF39C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public Win32PrintDialog()
		{
			this._printTicket = null;
			this._printQueue = null;
			this._minPage = 1U;
			this._maxPage = 9999U;
			this._pageRangeSelection = PageRangeSelection.AllPages;
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x001F11CC File Offset: 0x001EF3CC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal uint ShowDialog()
		{
			uint result = 0U;
			IntPtr intPtr = IntPtr.Zero;
			if (Application.Current != null && Application.Current.MainWindow != null)
			{
				WindowInteropHelper windowInteropHelper = new WindowInteropHelper(Application.Current.MainWindow);
				intPtr = windowInteropHelper.CriticalHandle;
			}
			try
			{
				if (this._printQueue == null || this._printTicket == null)
				{
					this.ProbeForPrintingSupport();
				}
				using (Win32PrintDialog.PrintDlgExMarshaler printDlgExMarshaler = new Win32PrintDialog.PrintDlgExMarshaler(intPtr, this))
				{
					printDlgExMarshaler.SyncToStruct();
					if (UnsafeNativeMethods.PrintDlgEx(printDlgExMarshaler.UnmanagedPrintDlgEx) == 0)
					{
						result = printDlgExMarshaler.SyncFromStruct();
					}
				}
			}
			catch (Exception ex)
			{
				if (!string.Equals(ex.GetType().FullName, "System.Printing.PrintingNotSupportedException", StringComparison.Ordinal))
				{
					throw;
				}
				string text = SR.Get("PrintDialogInstallPrintSupportMessageBox");
				string text2 = SR.Get("PrintDialogInstallPrintSupportCaption");
				MessageBoxOptions messageBoxOptions = (text2 != null && text2.Length > 0 && text2[0] == '‏') ? MessageBoxOptions.RtlReading : MessageBoxOptions.None;
				int type = (int)((MessageBoxOptions)64 | messageBoxOptions);
				if (intPtr == IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.GetActiveWindow();
				}
				if (UnsafeNativeMethods.MessageBox(new HandleRef(null, intPtr), text, text2, type) != 0)
				{
					result = 0U;
				}
			}
			return result;
		}

		// Token: 0x170019C1 RID: 6593
		// (get) Token: 0x06006BD4 RID: 27604 RVA: 0x001F1310 File Offset: 0x001EF510
		// (set) Token: 0x06006BD5 RID: 27605 RVA: 0x001F1318 File Offset: 0x001EF518
		internal PrintTicket PrintTicket
		{
			[SecurityCritical]
			get
			{
				return this._printTicket;
			}
			[SecurityCritical]
			set
			{
				this._printTicket = value;
			}
		}

		// Token: 0x170019C2 RID: 6594
		// (get) Token: 0x06006BD6 RID: 27606 RVA: 0x001F1321 File Offset: 0x001EF521
		// (set) Token: 0x06006BD7 RID: 27607 RVA: 0x001F1329 File Offset: 0x001EF529
		internal PrintQueue PrintQueue
		{
			[SecurityCritical]
			get
			{
				return this._printQueue;
			}
			[SecurityCritical]
			set
			{
				this._printQueue = value;
			}
		}

		// Token: 0x170019C3 RID: 6595
		// (get) Token: 0x06006BD8 RID: 27608 RVA: 0x001F1332 File Offset: 0x001EF532
		// (set) Token: 0x06006BD9 RID: 27609 RVA: 0x001F133A File Offset: 0x001EF53A
		internal uint MinPage
		{
			get
			{
				return this._minPage;
			}
			set
			{
				this._minPage = value;
			}
		}

		// Token: 0x170019C4 RID: 6596
		// (get) Token: 0x06006BDA RID: 27610 RVA: 0x001F1343 File Offset: 0x001EF543
		// (set) Token: 0x06006BDB RID: 27611 RVA: 0x001F134B File Offset: 0x001EF54B
		internal uint MaxPage
		{
			get
			{
				return this._maxPage;
			}
			set
			{
				this._maxPage = value;
			}
		}

		// Token: 0x170019C5 RID: 6597
		// (get) Token: 0x06006BDC RID: 27612 RVA: 0x001F1354 File Offset: 0x001EF554
		// (set) Token: 0x06006BDD RID: 27613 RVA: 0x001F135C File Offset: 0x001EF55C
		internal PageRangeSelection PageRangeSelection
		{
			get
			{
				return this._pageRangeSelection;
			}
			set
			{
				this._pageRangeSelection = value;
			}
		}

		// Token: 0x170019C6 RID: 6598
		// (get) Token: 0x06006BDE RID: 27614 RVA: 0x001F1365 File Offset: 0x001EF565
		// (set) Token: 0x06006BDF RID: 27615 RVA: 0x001F136D File Offset: 0x001EF56D
		internal PageRange PageRange
		{
			get
			{
				return this._pageRange;
			}
			set
			{
				this._pageRange = value;
			}
		}

		// Token: 0x170019C7 RID: 6599
		// (get) Token: 0x06006BE0 RID: 27616 RVA: 0x001F1376 File Offset: 0x001EF576
		// (set) Token: 0x06006BE1 RID: 27617 RVA: 0x001F137E File Offset: 0x001EF57E
		internal bool PageRangeEnabled
		{
			get
			{
				return this._pageRangeEnabled;
			}
			set
			{
				this._pageRangeEnabled = value;
			}
		}

		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x06006BE2 RID: 27618 RVA: 0x001F1387 File Offset: 0x001EF587
		// (set) Token: 0x06006BE3 RID: 27619 RVA: 0x001F138F File Offset: 0x001EF58F
		internal bool SelectedPagesEnabled
		{
			get
			{
				return this._selectedPagesEnabled;
			}
			set
			{
				this._selectedPagesEnabled = value;
			}
		}

		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x06006BE4 RID: 27620 RVA: 0x001F1398 File Offset: 0x001EF598
		// (set) Token: 0x06006BE5 RID: 27621 RVA: 0x001F13A0 File Offset: 0x001EF5A0
		internal bool CurrentPageEnabled
		{
			get
			{
				return this._currentPageEnabled;
			}
			set
			{
				this._currentPageEnabled = value;
			}
		}

		// Token: 0x06006BE6 RID: 27622 RVA: 0x001F13AC File Offset: 0x001EF5AC
		[SecurityCritical]
		private void ProbeForPrintingSupport()
		{
			string deviceName = (this._printQueue != null) ? this._printQueue.FullName : string.Empty;
			SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
			try
			{
				using (new PrintTicketConverter(deviceName, 1))
				{
				}
			}
			catch (PrintQueueException)
			{
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x040034F4 RID: 13556
		[SecurityCritical]
		private PrintTicket _printTicket;

		// Token: 0x040034F5 RID: 13557
		[SecurityCritical]
		private PrintQueue _printQueue;

		// Token: 0x040034F6 RID: 13558
		private PageRangeSelection _pageRangeSelection;

		// Token: 0x040034F7 RID: 13559
		private PageRange _pageRange;

		// Token: 0x040034F8 RID: 13560
		private bool _pageRangeEnabled;

		// Token: 0x040034F9 RID: 13561
		private bool _selectedPagesEnabled;

		// Token: 0x040034FA RID: 13562
		private bool _currentPageEnabled;

		// Token: 0x040034FB RID: 13563
		private uint _minPage;

		// Token: 0x040034FC RID: 13564
		private uint _maxPage;

		// Token: 0x040034FD RID: 13565
		private const char RightToLeftMark = '‏';

		// Token: 0x02000B1A RID: 2842
		private sealed class PrintDlgExMarshaler : IDisposable
		{
			// Token: 0x06008D11 RID: 36113 RVA: 0x002581BE File Offset: 0x002563BE
			[SecurityCritical]
			internal PrintDlgExMarshaler(IntPtr owner, Win32PrintDialog dialog)
			{
				this._ownerHandle = owner;
				this._dialog = dialog;
				this._unmanagedPrintDlgEx = IntPtr.Zero;
			}

			// Token: 0x06008D12 RID: 36114 RVA: 0x002581E0 File Offset: 0x002563E0
			~PrintDlgExMarshaler()
			{
				this.Dispose(true);
			}

			// Token: 0x17001F6B RID: 8043
			// (get) Token: 0x06008D13 RID: 36115 RVA: 0x00258210 File Offset: 0x00256410
			internal IntPtr UnmanagedPrintDlgEx
			{
				[SecurityCritical]
				get
				{
					return this._unmanagedPrintDlgEx;
				}
			}

			// Token: 0x06008D14 RID: 36116 RVA: 0x00258218 File Offset: 0x00256418
			[SecurityCritical]
			internal uint SyncFromStruct()
			{
				if (this._unmanagedPrintDlgEx == IntPtr.Zero)
				{
					return 0U;
				}
				uint num = this.AcquireResultFromPrintDlgExStruct(this._unmanagedPrintDlgEx);
				if (num == 1U || num == 2U)
				{
					string text;
					uint num2;
					PageRange pageRange;
					IntPtr devModeHandle;
					this.ExtractPrintDataAndDevMode(this._unmanagedPrintDlgEx, out text, out num2, out pageRange, out devModeHandle);
					this._dialog.PrintQueue = this.AcquirePrintQueue(text);
					this._dialog.PrintTicket = this.AcquirePrintTicket(devModeHandle, text);
					if ((num2 & 2U) == 2U)
					{
						if (pageRange.PageFrom > pageRange.PageTo)
						{
							int pageTo = pageRange.PageTo;
							pageRange.PageTo = pageRange.PageFrom;
							pageRange.PageFrom = pageTo;
						}
						this._dialog.PageRangeSelection = PageRangeSelection.UserPages;
						this._dialog.PageRange = pageRange;
					}
					else if ((num2 & 1U) == 1U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.SelectedPages;
					}
					else if ((num2 & 4194304U) == 4194304U)
					{
						this._dialog.PageRangeSelection = PageRangeSelection.CurrentPage;
					}
					else
					{
						this._dialog.PageRangeSelection = PageRangeSelection.AllPages;
					}
				}
				return num;
			}

			// Token: 0x06008D15 RID: 36117 RVA: 0x00258318 File Offset: 0x00256518
			[SecurityCritical]
			internal void SyncToStruct()
			{
				if (this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
				}
				if (this._ownerHandle == IntPtr.Zero)
				{
					this._ownerHandle = UnsafeNativeMethods.GetDesktopWindow();
				}
				this._unmanagedPrintDlgEx = this.AllocateUnmanagedPrintDlgExStruct();
			}

			// Token: 0x06008D16 RID: 36118 RVA: 0x0025836C File Offset: 0x0025656C
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private void Dispose(bool disposing)
			{
				if (disposing && this._unmanagedPrintDlgEx != IntPtr.Zero)
				{
					this.FreeUnmanagedPrintDlgExStruct(this._unmanagedPrintDlgEx);
					this._unmanagedPrintDlgEx = IntPtr.Zero;
				}
			}

			// Token: 0x06008D17 RID: 36119 RVA: 0x0025839C File Offset: 0x0025659C
			[SecurityCritical]
			private void ExtractPrintDataAndDevMode(IntPtr unmanagedBuffer, out string printerName, out uint flags, out PageRange pageRange, out IntPtr devModeHandle)
			{
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					devModeHandle = printdlgex.hDevMode;
					intPtr = printdlgex.hDevNames;
					flags = printdlgex.Flags;
					intPtr2 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					devModeHandle = printdlgex2.hDevMode;
					intPtr = printdlgex2.hDevNames;
					flags = printdlgex2.Flags;
					intPtr2 = printdlgex2.lpPageRanges;
				}
				if ((flags & 2U) == 2U && intPtr2 != IntPtr.Zero)
				{
					NativeMethods.PRINTPAGERANGE printpagerange = (NativeMethods.PRINTPAGERANGE)Marshal.PtrToStructure(intPtr2, typeof(NativeMethods.PRINTPAGERANGE));
					pageRange = new PageRange((int)printpagerange.nFromPage, (int)printpagerange.nToPage);
				}
				else
				{
					pageRange = new PageRange(1);
				}
				if (intPtr != IntPtr.Zero)
				{
					IntPtr intPtr3 = IntPtr.Zero;
					try
					{
						intPtr3 = UnsafeNativeMethods.GlobalLock(intPtr);
						NativeMethods.DEVNAMES devnames = (NativeMethods.DEVNAMES)Marshal.PtrToStructure(intPtr3, typeof(NativeMethods.DEVNAMES));
						int offset = checked((int)devnames.wDeviceOffset * Marshal.SystemDefaultCharSize);
						printerName = Marshal.PtrToStringAuto(intPtr3 + offset);
						return;
					}
					finally
					{
						if (intPtr3 != IntPtr.Zero)
						{
							UnsafeNativeMethods.GlobalUnlock(intPtr);
						}
					}
				}
				printerName = string.Empty;
			}

			// Token: 0x06008D18 RID: 36120 RVA: 0x00258500 File Offset: 0x00256700
			[SecurityCritical]
			private PrintQueue AcquirePrintQueue(string printerName)
			{
				PrintQueue printQueue = null;
				EnumeratedPrintQueueTypes[] enumerationFlag = new EnumeratedPrintQueueTypes[]
				{
					EnumeratedPrintQueueTypes.Local,
					EnumeratedPrintQueueTypes.Connections
				};
				PrintQueueIndexedProperty[] propertiesFilter = new PrintQueueIndexedProperty[]
				{
					PrintQueueIndexedProperty.Name,
					PrintQueueIndexedProperty.QueueAttributes
				};
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (LocalPrintServer localPrintServer = new LocalPrintServer())
					{
						foreach (PrintQueue printQueue2 in localPrintServer.GetPrintQueues(propertiesFilter, enumerationFlag))
						{
							if (printerName.Equals(printQueue2.FullName, StringComparison.OrdinalIgnoreCase))
							{
								printQueue = printQueue2;
								break;
							}
						}
					}
					if (printQueue != null)
					{
						printQueue.InPartialTrust = true;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return printQueue;
			}

			// Token: 0x06008D19 RID: 36121 RVA: 0x002585C8 File Offset: 0x002567C8
			[SecurityCritical]
			private PrintTicket AcquirePrintTicket(IntPtr devModeHandle, string printQueueName)
			{
				PrintTicket result = null;
				byte[] array = null;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = UnsafeNativeMethods.GlobalLock(devModeHandle);
					NativeMethods.DEVMODE devmode = (NativeMethods.DEVMODE)Marshal.PtrToStructure(intPtr, typeof(NativeMethods.DEVMODE));
					array = new byte[(int)(devmode.dmSize + devmode.dmDriverExtra)];
					Marshal.Copy(intPtr, array, 0, array.Length);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						UnsafeNativeMethods.GlobalUnlock(devModeHandle);
					}
				}
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printQueueName, PrintTicketConverter.MaxPrintSchemaVersion))
					{
						result = printTicketConverter.ConvertDevModeToPrintTicket(array);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return result;
			}

			// Token: 0x06008D1A RID: 36122 RVA: 0x00258690 File Offset: 0x00256890
			[SecurityCritical]
			private uint AcquireResultFromPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				uint dwResultAction;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					dwResultAction = printdlgex.dwResultAction;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					dwResultAction = printdlgex2.dwResultAction;
				}
				return dwResultAction;
			}

			// Token: 0x06008D1B RID: 36123 RVA: 0x002586E4 File Offset: 0x002568E4
			[SecurityCritical]
			private IntPtr AllocateUnmanagedPrintDlgExStruct()
			{
				IntPtr intPtr = IntPtr.Zero;
				NativeMethods.PRINTPAGERANGE printpagerange;
				printpagerange.nToPage = (uint)this._dialog.PageRange.PageTo;
				printpagerange.nFromPage = (uint)this._dialog.PageRange.PageFrom;
				uint flags = 1835008U;
				try
				{
					if (!this.Is64Bit())
					{
						NativeMethods.PRINTDLGEX32 printdlgex = new NativeMethods.PRINTDLGEX32();
						printdlgex.hwndOwner = this._ownerHandle;
						printdlgex.nMinPage = this._dialog.MinPage;
						printdlgex.nMaxPage = this._dialog.MaxPage;
						printdlgex.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex.Flags |= 1U;
							}
						}
						else
						{
							printdlgex.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex.nPageRanges = 1U;
								Marshal.StructureToPtr(printpagerange, printdlgex.lpPageRanges, false);
								printdlgex.Flags |= 2U;
							}
							else
							{
								printdlgex.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex.lpPageRanges = IntPtr.Zero;
							printdlgex.nMaxPageRanges = 0U;
							printdlgex.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						int cb = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX32));
						intPtr = Marshal.AllocHGlobal(cb);
						Marshal.StructureToPtr(printdlgex, intPtr, false);
					}
					else
					{
						NativeMethods.PRINTDLGEX64 printdlgex2 = new NativeMethods.PRINTDLGEX64();
						printdlgex2.hwndOwner = this._ownerHandle;
						printdlgex2.nMinPage = this._dialog.MinPage;
						printdlgex2.nMaxPage = this._dialog.MaxPage;
						printdlgex2.Flags = flags;
						if (this._dialog.SelectedPagesEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.SelectedPages)
							{
								printdlgex2.Flags |= 1U;
							}
						}
						else
						{
							printdlgex2.Flags |= 4U;
						}
						if (this._dialog.CurrentPageEnabled)
						{
							if (this._dialog.PageRangeSelection == PageRangeSelection.CurrentPage)
							{
								printdlgex2.Flags |= 4194304U;
							}
						}
						else
						{
							printdlgex2.Flags |= 8388608U;
						}
						if (this._dialog.PageRangeEnabled)
						{
							printdlgex2.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeMethods.PRINTPAGERANGE)));
							printdlgex2.nMaxPageRanges = 1U;
							if (this._dialog.PageRangeSelection == PageRangeSelection.UserPages)
							{
								printdlgex2.nPageRanges = 1U;
								Marshal.StructureToPtr(printpagerange, printdlgex2.lpPageRanges, false);
								printdlgex2.Flags |= 2U;
							}
							else
							{
								printdlgex2.nPageRanges = 0U;
							}
						}
						else
						{
							printdlgex2.lpPageRanges = IntPtr.Zero;
							printdlgex2.nMaxPageRanges = 0U;
							printdlgex2.Flags |= 8U;
						}
						if (this._dialog.PrintQueue != null)
						{
							printdlgex2.hDevNames = this.AllocateAndInitializeDevNames(this._dialog.PrintQueue.FullName);
							if (this._dialog.PrintTicket != null)
							{
								printdlgex2.hDevMode = this.AllocateAndInitializeDevMode(this._dialog.PrintQueue.FullName, this._dialog.PrintTicket);
							}
						}
						int cb2 = Marshal.SizeOf(typeof(NativeMethods.PRINTDLGEX64));
						intPtr = Marshal.AllocHGlobal(cb2);
						Marshal.StructureToPtr(printdlgex2, intPtr, false);
					}
				}
				catch (Exception)
				{
					this.FreeUnmanagedPrintDlgExStruct(intPtr);
					intPtr = IntPtr.Zero;
					throw;
				}
				return intPtr;
			}

			// Token: 0x06008D1C RID: 36124 RVA: 0x00258B1C File Offset: 0x00256D1C
			[SecurityCritical]
			private void FreeUnmanagedPrintDlgExStruct(IntPtr unmanagedBuffer)
			{
				if (unmanagedBuffer == IntPtr.Zero)
				{
					return;
				}
				IntPtr intPtr = IntPtr.Zero;
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr intPtr3 = IntPtr.Zero;
				if (!this.Is64Bit())
				{
					NativeMethods.PRINTDLGEX32 printdlgex = (NativeMethods.PRINTDLGEX32)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX32));
					intPtr = printdlgex.hDevMode;
					intPtr2 = printdlgex.hDevNames;
					intPtr3 = printdlgex.lpPageRanges;
				}
				else
				{
					NativeMethods.PRINTDLGEX64 printdlgex2 = (NativeMethods.PRINTDLGEX64)Marshal.PtrToStructure(unmanagedBuffer, typeof(NativeMethods.PRINTDLGEX64));
					intPtr = printdlgex2.hDevMode;
					intPtr2 = printdlgex2.hDevNames;
					intPtr3 = printdlgex2.lpPageRanges;
				}
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr2);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					UnsafeNativeMethods.GlobalFree(intPtr3);
				}
				Marshal.FreeHGlobal(unmanagedBuffer);
			}

			// Token: 0x06008D1D RID: 36125 RVA: 0x00258BF0 File Offset: 0x00256DF0
			[SecurityCritical]
			[SecurityTreatAsSafe]
			private bool Is64Bit()
			{
				IntPtr zero = IntPtr.Zero;
				return Marshal.SizeOf(zero) == 8;
			}

			// Token: 0x06008D1E RID: 36126 RVA: 0x00258C14 File Offset: 0x00256E14
			[SecurityCritical]
			private IntPtr AllocateAndInitializeDevNames(string printerName)
			{
				IntPtr intPtr = IntPtr.Zero;
				char[] array = printerName.ToCharArray();
				int cb = checked((array.Length + 3) * Marshal.SystemDefaultCharSize + Marshal.SizeOf(typeof(NativeMethods.DEVNAMES)));
				intPtr = Marshal.AllocHGlobal(cb);
				ushort num = (ushort)Marshal.SizeOf(typeof(NativeMethods.DEVNAMES));
				NativeMethods.DEVNAMES devnames;
				devnames.wDeviceOffset = (ushort)((int)num / Marshal.SystemDefaultCharSize);
				IntPtr intPtr2;
				IntPtr destination;
				checked
				{
					devnames.wDriverOffset = (ushort)((int)devnames.wDeviceOffset + array.Length + 1);
					devnames.wOutputOffset = devnames.wDriverOffset + 1;
					devnames.wDefault = 0;
					Marshal.StructureToPtr(devnames, intPtr, false);
					intPtr2 = (IntPtr)((long)intPtr + (long)(unchecked((ulong)num)));
					destination = (IntPtr)((long)intPtr2 + unchecked((long)(checked(array.Length * Marshal.SystemDefaultCharSize))));
				}
				byte[] array2 = new byte[3 * Marshal.SystemDefaultCharSize];
				Array.Clear(array2, 0, array2.Length);
				Marshal.Copy(array, 0, intPtr2, array.Length);
				Marshal.Copy(array2, 0, destination, array2.Length);
				return intPtr;
			}

			// Token: 0x06008D1F RID: 36127 RVA: 0x00258D0C File Offset: 0x00256F0C
			[SecurityCritical]
			private IntPtr AllocateAndInitializeDevMode(string printerName, PrintTicket printTicket)
			{
				byte[] array = null;
				SystemDrawingHelper.NewDefaultPrintingPermission().Assert();
				try
				{
					using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printerName, PrintTicketConverter.MaxPrintSchemaVersion))
					{
						array = printTicketConverter.ConvertPrintTicketToDevMode(printTicket, BaseDevModeType.UserDefault);
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
				Marshal.Copy(array, 0, intPtr, array.Length);
				return intPtr;
			}

			// Token: 0x06008D20 RID: 36128 RVA: 0x00258D80 File Offset: 0x00256F80
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004A41 RID: 19009
			private Win32PrintDialog _dialog;

			// Token: 0x04004A42 RID: 19010
			[SecurityCritical]
			private IntPtr _unmanagedPrintDlgEx;

			// Token: 0x04004A43 RID: 19011
			[SecurityCritical]
			private IntPtr _ownerHandle;
		}
	}
}

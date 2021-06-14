using System;
using System.ComponentModel;
using System.Drawing.Internal;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Printing
{
	/// <summary>Specifies a print controller that sends information to a printer.</summary>
	// Token: 0x02000052 RID: 82
	public class StandardPrintController : PrintController
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x0001C579 File Offset: 0x0001A779
		private void CheckSecurity(PrintDocument document)
		{
			if (document.PrinterSettings.PrintDialogDisplayed)
			{
				IntSecurity.SafePrinting.Demand();
				return;
			}
			if (document.PrinterSettings.IsDefaultPrinter)
			{
				IntSecurity.DefaultPrinting.Demand();
				return;
			}
			IntSecurity.AllPrinting.Demand();
		}

		/// <summary>Begins the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains data about how to print the document. </param>
		/// <exception cref="T:System.Drawing.Printing.InvalidPrinterException">The printer settings are not valid. </exception>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The native Win32 Application Programming Interface (API) could not start a print job. </exception>
		// Token: 0x06000702 RID: 1794 RVA: 0x0001C5B8 File Offset: 0x0001A7B8
		public override void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity(document);
			base.OnStartPrint(document, e);
			if (!document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(document.PrinterSettings);
			}
			this.dc = document.PrinterSettings.CreateDeviceContext(this.modeHandle);
			SafeNativeMethods.DOCINFO docinfo = new SafeNativeMethods.DOCINFO();
			docinfo.lpszDocName = document.DocumentName;
			if (document.PrinterSettings.PrintToFile)
			{
				docinfo.lpszOutput = document.PrinterSettings.OutputPort;
			}
			else
			{
				docinfo.lpszOutput = null;
			}
			docinfo.lpszDatatype = null;
			docinfo.fwType = 0;
			int num = SafeNativeMethods.StartDoc(new HandleRef(this.dc, this.dc.Hdc), docinfo);
			if (num > 0)
			{
				return;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			if (lastWin32Error == 1223)
			{
				e.Cancel = true;
				return;
			}
			throw new Win32Exception(lastWin32Error);
		}

		/// <summary>Begins the control sequence that determines when and how to print a page in a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains data about how to print a page in the document. Initially, the <see cref="P:System.Drawing.Printing.PrintPageEventArgs.Graphics" /> property of this parameter will be <see langword="null" />. The value returned from the <see cref="M:System.Drawing.Printing.StandardPrintController.OnStartPage(System.Drawing.Printing.PrintDocument,System.Drawing.Printing.PrintPageEventArgs)" /> method will be used to set this property. </param>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> object that represents a page from a <see cref="T:System.Drawing.Printing.PrintDocument" />.</returns>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The native Win32 Application Programming Interface (API) could not prepare the printer driver to accept data.-or- The native Win32 API could not update the specified printer or plotter device context (DC) using the specified information. </exception>
		// Token: 0x06000703 RID: 1795 RVA: 0x0001C690 File Offset: 0x0001A890
		public override Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity(document);
			base.OnStartPage(document, e);
			try
			{
				IntSecurity.AllPrintingAndUnmanagedCode.Assert();
				e.PageSettings.CopyToHdevmode(this.modeHandle);
				IntPtr handle = SafeNativeMethods.GlobalLock(new HandleRef(this, this.modeHandle));
				try
				{
					IntPtr intPtr = SafeNativeMethods.ResetDC(new HandleRef(this.dc, this.dc.Hdc), new HandleRef(null, handle));
				}
				finally
				{
					SafeNativeMethods.GlobalUnlock(new HandleRef(this, this.modeHandle));
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.graphics = Graphics.FromHdcInternal(this.dc.Hdc);
			if (this.graphics != null && document.OriginAtMargins)
			{
				int deviceCaps = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 88);
				int deviceCaps2 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 90);
				int deviceCaps3 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 112);
				int deviceCaps4 = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(this.dc, this.dc.Hdc), 113);
				float num = (float)(deviceCaps3 * 100 / deviceCaps);
				float num2 = (float)(deviceCaps4 * 100 / deviceCaps2);
				this.graphics.TranslateTransform(-num, -num2);
				this.graphics.TranslateTransform((float)document.DefaultPageSettings.Margins.Left, (float)document.DefaultPageSettings.Margins.Top);
			}
			int num3 = SafeNativeMethods.StartPage(new HandleRef(this.dc, this.dc.Hdc));
			if (num3 <= 0)
			{
				throw new Win32Exception();
			}
			return this.graphics;
		}

		/// <summary>Completes the control sequence that determines when and how to print a page of a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains data about how to print a page in the document. </param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The native Win32 Application Programming Interface (API) could not finish writing to a page. </exception>
		// Token: 0x06000704 RID: 1796 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public override void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
			this.CheckSecurity(document);
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				int num = SafeNativeMethods.EndPage(new HandleRef(this.dc, this.dc.Hdc));
				if (num <= 0)
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				this.graphics.Dispose();
				this.graphics = null;
			}
			base.OnEndPage(document, e);
		}

		/// <summary>Completes the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains data about how to print the document. </param>
		/// <exception cref="T:System.ComponentModel.Win32Exception">The native Win32 Application Programming Interface (API) could not complete the print job.-or- The native Win32 API could not delete the specified device context (DC). </exception>
		// Token: 0x06000705 RID: 1797 RVA: 0x0001C8E4 File Offset: 0x0001AAE4
		public override void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			this.CheckSecurity(document);
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				if (this.dc != null)
				{
					try
					{
						int num = e.Cancel ? SafeNativeMethods.AbortDoc(new HandleRef(this.dc, this.dc.Hdc)) : SafeNativeMethods.EndDoc(new HandleRef(this.dc, this.dc.Hdc));
						if (num <= 0)
						{
							throw new Win32Exception();
						}
					}
					finally
					{
						this.dc.Dispose();
						this.dc = null;
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			base.OnEndPrint(document, e);
		}

		// Token: 0x04000603 RID: 1539
		private DeviceContext dc;

		// Token: 0x04000604 RID: 1540
		private Graphics graphics;
	}
}

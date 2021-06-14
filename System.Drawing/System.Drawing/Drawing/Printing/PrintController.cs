using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Printing
{
	/// <summary>Controls how a document is printed, when printing from a Windows Forms application.</summary>
	// Token: 0x02000060 RID: 96
	public abstract class PrintController
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.PrintController" /> class.</summary>
		// Token: 0x0600076D RID: 1901 RVA: 0x0001E362 File Offset: 0x0001C562
		protected PrintController()
		{
			IntSecurity.SafePrinting.Demand();
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Drawing.Printing.PrintController" /> is used for print preview.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001E374 File Offset: 0x0001C574
		public virtual bool IsPreview
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001E378 File Offset: 0x0001C578
		internal void Print(PrintDocument document)
		{
			IntSecurity.SafePrinting.Demand();
			PrintAction action;
			if (this.IsPreview)
			{
				action = PrintAction.PrintToPreview;
			}
			else
			{
				action = (document.PrinterSettings.PrintToFile ? PrintAction.PrintToFile : PrintAction.PrintToPrinter);
			}
			PrintEventArgs printEventArgs = new PrintEventArgs(action);
			document._OnBeginPrint(printEventArgs);
			if (printEventArgs.Cancel)
			{
				document._OnEndPrint(printEventArgs);
				return;
			}
			this.OnStartPrint(document, printEventArgs);
			if (printEventArgs.Cancel)
			{
				document._OnEndPrint(printEventArgs);
				this.OnEndPrint(document, printEventArgs);
				return;
			}
			bool flag = true;
			try
			{
				flag = (LocalAppContextSwitches.OptimizePrintPreview ? this.PrintLoopOptimized(document) : this.PrintLoop(document));
			}
			finally
			{
				try
				{
					try
					{
						document._OnEndPrint(printEventArgs);
						printEventArgs.Cancel = (flag | printEventArgs.Cancel);
					}
					finally
					{
						this.OnEndPrint(document, printEventArgs);
					}
				}
				finally
				{
					if (!IntSecurity.HasPermission(IntSecurity.AllPrinting))
					{
						IntSecurity.AllPrinting.Assert();
						document.PrinterSettings.PrintDialogDisplayed = false;
					}
				}
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001E474 File Offset: 0x0001C674
		private bool PrintLoop(PrintDocument document)
		{
			QueryPageSettingsEventArgs queryPageSettingsEventArgs = new QueryPageSettingsEventArgs((PageSettings)document.DefaultPageSettings.Clone());
			for (;;)
			{
				document._OnQueryPageSettings(queryPageSettingsEventArgs);
				if (queryPageSettingsEventArgs.Cancel)
				{
					break;
				}
				PrintPageEventArgs printPageEventArgs = this.CreatePrintPageEvent(queryPageSettingsEventArgs.PageSettings);
				Graphics graphics = this.OnStartPage(document, printPageEventArgs);
				printPageEventArgs.SetGraphics(graphics);
				try
				{
					document._OnPrintPage(printPageEventArgs);
					this.OnEndPage(document, printPageEventArgs);
				}
				finally
				{
					printPageEventArgs.Dispose();
				}
				if (printPageEventArgs.Cancel)
				{
					return true;
				}
				if (!printPageEventArgs.HasMorePages)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001E500 File Offset: 0x0001C700
		private bool PrintLoopOptimized(PrintDocument document)
		{
			PrintPageEventArgs printPageEventArgs = null;
			PageSettings pageSettings = (PageSettings)document.DefaultPageSettings.Clone();
			QueryPageSettingsEventArgs queryPageSettingsEventArgs = new QueryPageSettingsEventArgs(pageSettings);
			for (;;)
			{
				queryPageSettingsEventArgs.PageSettingsChanged = false;
				document._OnQueryPageSettings(queryPageSettingsEventArgs);
				if (queryPageSettingsEventArgs.Cancel)
				{
					break;
				}
				if (!queryPageSettingsEventArgs.PageSettingsChanged)
				{
					if (printPageEventArgs == null)
					{
						printPageEventArgs = this.CreatePrintPageEvent(queryPageSettingsEventArgs.PageSettings);
					}
					else
					{
						printPageEventArgs.CopySettingsToDevMode = false;
					}
					Graphics graphics = this.OnStartPage(document, printPageEventArgs);
					printPageEventArgs.SetGraphics(graphics);
				}
				else
				{
					printPageEventArgs = this.CreatePrintPageEvent(queryPageSettingsEventArgs.PageSettings);
					Graphics graphics2 = this.OnStartPage(document, printPageEventArgs);
					printPageEventArgs.SetGraphics(graphics2);
				}
				try
				{
					document._OnPrintPage(printPageEventArgs);
					this.OnEndPage(document, printPageEventArgs);
				}
				finally
				{
					printPageEventArgs.Graphics.Dispose();
					printPageEventArgs.SetGraphics(null);
				}
				if (printPageEventArgs.Cancel)
				{
					return true;
				}
				if (!printPageEventArgs.HasMorePages)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001E5DC File Offset: 0x0001C7DC
		private PrintPageEventArgs CreatePrintPageEvent(PageSettings pageSettings)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			Rectangle bounds = pageSettings.GetBounds(this.modeHandle);
			Rectangle marginBounds = new Rectangle(pageSettings.Margins.Left, pageSettings.Margins.Top, bounds.Width - (pageSettings.Margins.Left + pageSettings.Margins.Right), bounds.Height - (pageSettings.Margins.Top + pageSettings.Margins.Bottom));
			return new PrintPageEventArgs(null, marginBounds, bounds, pageSettings);
		}

		/// <summary>When overridden in a derived class, begins the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000773 RID: 1907 RVA: 0x0001E66B File Offset: 0x0001C86B
		public virtual void OnStartPrint(PrintDocument document, PrintEventArgs e)
		{
			IntSecurity.AllPrintingAndUnmanagedCode.Assert();
			this.modeHandle = (PrintController.SafeDeviceModeHandle)document.PrinterSettings.GetHdevmode(document.DefaultPageSettings);
		}

		/// <summary>When overridden in a derived class, begins the control sequence that determines when and how to print a page of a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data. </param>
		/// <returns>A <see cref="T:System.Drawing.Graphics" /> that represents a page from a <see cref="T:System.Drawing.Printing.PrintDocument" />.</returns>
		// Token: 0x06000774 RID: 1908 RVA: 0x0001C48C File Offset: 0x0001A68C
		public virtual Graphics OnStartPage(PrintDocument document, PrintPageEventArgs e)
		{
			return null;
		}

		/// <summary>When overridden in a derived class, completes the control sequence that determines when and how to print a page of a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintPageEventArgs" /> that contains the event data. </param>
		// Token: 0x06000775 RID: 1909 RVA: 0x00015255 File Offset: 0x00013455
		public virtual void OnEndPage(PrintDocument document, PrintPageEventArgs e)
		{
		}

		/// <summary>When overridden in a derived class, completes the control sequence that determines when and how to print a document.</summary>
		/// <param name="document">A <see cref="T:System.Drawing.Printing.PrintDocument" /> that represents the document currently being printed. </param>
		/// <param name="e">A <see cref="T:System.Drawing.Printing.PrintEventArgs" /> that contains the event data. </param>
		// Token: 0x06000776 RID: 1910 RVA: 0x0001E693 File Offset: 0x0001C893
		public virtual void OnEndPrint(PrintDocument document, PrintEventArgs e)
		{
			IntSecurity.UnmanagedCode.Assert();
			if (this.modeHandle != null)
			{
				this.modeHandle.Close();
			}
		}

		// Token: 0x040006BD RID: 1725
		internal PrintController.SafeDeviceModeHandle modeHandle;

		// Token: 0x0200011E RID: 286
		[SecurityCritical]
		internal sealed class SafeDeviceModeHandle : SafeHandle
		{
			// Token: 0x06000F4F RID: 3919 RVA: 0x0002DA2B File Offset: 0x0002BC2B
			private SafeDeviceModeHandle() : base(IntPtr.Zero, true)
			{
			}

			// Token: 0x06000F50 RID: 3920 RVA: 0x00029D35 File Offset: 0x00027F35
			internal SafeDeviceModeHandle(IntPtr handle) : base(IntPtr.Zero, true)
			{
				base.SetHandle(handle);
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00029DB0 File Offset: 0x00027FB0
			public override bool IsInvalid
			{
				get
				{
					return this.handle == IntPtr.Zero;
				}
			}

			// Token: 0x06000F52 RID: 3922 RVA: 0x0002DA39 File Offset: 0x0002BC39
			[SecurityCritical]
			protected override bool ReleaseHandle()
			{
				if (!this.IsInvalid)
				{
					SafeNativeMethods.GlobalFree(new HandleRef(this, this.handle));
				}
				this.handle = IntPtr.Zero;
				return true;
			}

			// Token: 0x06000F53 RID: 3923 RVA: 0x00029DC2 File Offset: 0x00027FC2
			public static implicit operator IntPtr(PrintController.SafeDeviceModeHandle handle)
			{
				if (handle != null)
				{
					return handle.handle;
				}
				return IntPtr.Zero;
			}

			// Token: 0x06000F54 RID: 3924 RVA: 0x0002DA61 File Offset: 0x0002BC61
			public static explicit operator PrintController.SafeDeviceModeHandle(IntPtr handle)
			{
				return new PrintController.SafeDeviceModeHandle(handle);
			}
		}
	}
}

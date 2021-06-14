using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents the raw preview part of print previewing from a Windows Forms application, without any dialog boxes or buttons. Most <see cref="T:System.Windows.Forms.PrintPreviewControl" /> objects are found on <see cref="T:System.Windows.Forms.PrintPreviewDialog" /> objects, but they do not have to be.</summary>
	// Token: 0x0200043E RID: 1086
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Document")]
	[SRDescription("DescriptionPrintPreviewControl")]
	public class PrintPreviewControl : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PrintPreviewControl" /> class.</summary>
		// Token: 0x06004B92 RID: 19346 RVA: 0x00137C08 File Offset: 0x00135E08
		public PrintPreviewControl()
		{
			this.ResetBackColor();
			this.ResetForeColor();
			base.Size = new Size(100, 100);
			base.SetStyle(ControlStyles.ResizeRedraw, false);
			base.SetStyle(ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
		}

		/// <summary>Gets or sets a value indicating whether printing uses the anti-aliasing features of the operating system.</summary>
		/// <returns>
		///     <see langword="true" /> if anti-aliasing is used; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700128A RID: 4746
		// (get) Token: 0x06004B93 RID: 19347 RVA: 0x00137C9F File Offset: 0x00135E9F
		// (set) Token: 0x06004B94 RID: 19348 RVA: 0x00137CA7 File Offset: 0x00135EA7
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("PrintPreviewAntiAliasDescr")]
		public bool UseAntiAlias
		{
			get
			{
				return this.antiAlias;
			}
			set
			{
				this.antiAlias = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether resizing the control or changing the number of pages shown automatically adjusts the <see cref="P:System.Windows.Forms.PrintPreviewControl.Zoom" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the changing the control size or number of pages adjusts the <see cref="P:System.Windows.Forms.PrintPreviewControl.Zoom" /> property; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700128B RID: 4747
		// (get) Token: 0x06004B95 RID: 19349 RVA: 0x00137CB0 File Offset: 0x00135EB0
		// (set) Token: 0x06004B96 RID: 19350 RVA: 0x00137CB8 File Offset: 0x00135EB8
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("PrintPreviewAutoZoomDescr")]
		public bool AutoZoom
		{
			get
			{
				return this.autoZoom;
			}
			set
			{
				if (this.autoZoom != value)
				{
					this.autoZoom = value;
					this.InvalidateLayout();
				}
			}
		}

		/// <summary>Gets or sets a value indicating the document to preview.</summary>
		/// <returns>The <see cref="T:System.Drawing.Printing.PrintDocument" /> representing the document to preview.</returns>
		// Token: 0x1700128C RID: 4748
		// (get) Token: 0x06004B97 RID: 19351 RVA: 0x00137CD0 File Offset: 0x00135ED0
		// (set) Token: 0x06004B98 RID: 19352 RVA: 0x00137CD8 File Offset: 0x00135ED8
		[SRCategory("CatBehavior")]
		[DefaultValue(null)]
		[SRDescription("PrintPreviewDocumentDescr")]
		public PrintDocument Document
		{
			get
			{
				return this.document;
			}
			set
			{
				this.document = value;
				this.InvalidatePreview();
			}
		}

		/// <summary>Gets or sets the number of pages displayed horizontally across the screen.</summary>
		/// <returns>The number of pages displayed horizontally across the screen. The default is 1.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than 1.</exception>
		// Token: 0x1700128D RID: 4749
		// (get) Token: 0x06004B99 RID: 19353 RVA: 0x00137CE7 File Offset: 0x00135EE7
		// (set) Token: 0x06004B9A RID: 19354 RVA: 0x00137CF0 File Offset: 0x00135EF0
		[DefaultValue(1)]
		[SRCategory("CatLayout")]
		[SRDescription("PrintPreviewColumnsDescr")]
		public int Columns
		{
			get
			{
				return this.columns;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("Columns", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Columns",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.columns = value;
				this.InvalidateLayout();
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x1700128E RID: 4750
		// (get) Token: 0x06004B9B RID: 19355 RVA: 0x00137D54 File Offset: 0x00135F54
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 1048576;
				createParams.Style |= 2097152;
				return createParams;
			}
		}

		// Token: 0x1700128F RID: 4751
		// (get) Token: 0x06004B9C RID: 19356 RVA: 0x00137D8D File Offset: 0x00135F8D
		// (set) Token: 0x06004B9D RID: 19357 RVA: 0x00137D95 File Offset: 0x00135F95
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWithScrollbarsPositionDescr")]
		private Point Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.SetPositionNoInvalidate(value);
			}
		}

		/// <summary>Gets or sets the number of pages displayed vertically down the screen.</summary>
		/// <returns>The number of pages displayed vertically down the screen. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than 1.</exception>
		// Token: 0x17001290 RID: 4752
		// (get) Token: 0x06004B9E RID: 19358 RVA: 0x00137D9E File Offset: 0x00135F9E
		// (set) Token: 0x06004B9F RID: 19359 RVA: 0x00137DA8 File Offset: 0x00135FA8
		[DefaultValue(1)]
		[SRDescription("PrintPreviewRowsDescr")]
		[SRCategory("CatBehavior")]
		public int Rows
		{
			get
			{
				return this.rows;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("Rows", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"Rows",
						value.ToString(CultureInfo.CurrentCulture),
						1.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.rows = value;
				this.InvalidateLayout();
			}
		}

		/// <summary>Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit" />.</returns>
		// Token: 0x17001291 RID: 4753
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x000DAB7B File Offset: 0x000D8D7B
		// (set) Token: 0x06004BA1 RID: 19361 RVA: 0x00137E09 File Offset: 0x00136009
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[AmbientValue(RightToLeft.Inherit)]
		[SRDescription("ControlRightToLeftDescr")]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
				this.InvalidatePreview();
			}
		}

		/// <summary>Gets or sets the text associated with this control.</summary>
		/// <returns>The text associated with this control.</returns>
		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06004BA3 RID: 19363 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PrintPreviewControl.Text" /> property changes.</summary>
		// Token: 0x140003EC RID: 1004
		// (add) Token: 0x06004BA4 RID: 19364 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06004BA5 RID: 19365 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Gets or sets the page number of the upper left page.</summary>
		/// <returns>The page number of the upper left page. The default is 0.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The set value is less than 0.</exception>
		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x00137E18 File Offset: 0x00136018
		// (set) Token: 0x06004BA7 RID: 19367 RVA: 0x00137E5C File Offset: 0x0013605C
		[DefaultValue(0)]
		[SRDescription("PrintPreviewStartPageDescr")]
		[SRCategory("CatBehavior")]
		public int StartPage
		{
			get
			{
				int val = this.startPage;
				if (this.pageInfo != null)
				{
					val = Math.Min(val, this.pageInfo.Length - this.rows * this.columns);
				}
				return Math.Max(val, 0);
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("StartPage", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"StartPage",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int num = this.StartPage;
				this.startPage = value;
				if (num != this.startPage)
				{
					this.InvalidateLayout();
					this.OnStartPageChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the start page changes.</summary>
		// Token: 0x140003ED RID: 1005
		// (add) Token: 0x06004BA8 RID: 19368 RVA: 0x00137ED8 File Offset: 0x001360D8
		// (remove) Token: 0x06004BA9 RID: 19369 RVA: 0x00137EEB File Offset: 0x001360EB
		[SRCategory("CatPropertyChanged")]
		[SRDescription("RadioButtonOnStartPageChangedDescr")]
		public event EventHandler StartPageChanged
		{
			add
			{
				base.Events.AddHandler(PrintPreviewControl.EVENT_STARTPAGECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(PrintPreviewControl.EVENT_STARTPAGECHANGED, value);
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06004BAA RID: 19370 RVA: 0x00137EFE File Offset: 0x001360FE
		// (set) Token: 0x06004BAB RID: 19371 RVA: 0x00137F06 File Offset: 0x00136106
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlWithScrollbarsVirtualSizeDescr")]
		private Size VirtualSize
		{
			get
			{
				return this.virtualSize;
			}
			set
			{
				this.SetVirtualSizeNoInvalidate(value);
				base.Invalidate();
			}
		}

		/// <summary>Gets or sets a value indicating how large the pages will appear.</summary>
		/// <returns>A value indicating how large the pages will appear. A value of 1.0 indicates full size.</returns>
		/// <exception cref="T:System.ArgumentException">The value is less than or equal to 0. </exception>
		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x00137F15 File Offset: 0x00136115
		// (set) Token: 0x06004BAD RID: 19373 RVA: 0x00137F1D File Offset: 0x0013611D
		[SRCategory("CatBehavior")]
		[SRDescription("PrintPreviewZoomDescr")]
		[DefaultValue(0.3)]
		public double Zoom
		{
			get
			{
				return this.zoom;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentException(SR.GetString("PrintPreviewControlZoomNegative"));
				}
				this.autoZoom = false;
				this.zoom = value;
				this.InvalidateLayout();
			}
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00137F50 File Offset: 0x00136150
		private int AdjustScroll(Message m, int pos, int maxPos, bool horizontal)
		{
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (pos > 5)
				{
					pos -= 5;
				}
				else
				{
					pos = 0;
				}
				break;
			case 1:
				if (pos < maxPos - 5)
				{
					pos += 5;
				}
				else
				{
					pos = maxPos;
				}
				break;
			case 2:
				if (pos > 100)
				{
					pos -= 100;
				}
				else
				{
					pos = 0;
				}
				break;
			case 3:
				if (pos < maxPos - 100)
				{
					pos += 100;
				}
				else
				{
					pos = maxPos;
				}
				break;
			case 4:
			case 5:
			{
				NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
				scrollinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.SCROLLINFO));
				scrollinfo.fMask = 16;
				int fnBar = horizontal ? 0 : 1;
				if (SafeNativeMethods.GetScrollInfo(new HandleRef(this, m.HWnd), fnBar, scrollinfo))
				{
					pos = scrollinfo.nTrackPos;
				}
				else
				{
					pos = NativeMethods.Util.HIWORD(m.WParam);
				}
				break;
			}
			}
			return pos;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x00138030 File Offset: 0x00136230
		private void ComputeLayout()
		{
			this.layoutOk = true;
			if (this.pageInfo.Length == 0)
			{
				base.ClientSize = base.Size;
				return;
			}
			Graphics graphics = base.CreateGraphicsInternal();
			IntPtr hdc = graphics.GetHdc();
			this.screendpi = new Point(UnsafeNativeMethods.GetDeviceCaps(new HandleRef(graphics, hdc), 88), UnsafeNativeMethods.GetDeviceCaps(new HandleRef(graphics, hdc), 90));
			graphics.ReleaseHdcInternal(hdc);
			graphics.Dispose();
			Size physicalSize = this.pageInfo[this.StartPage].PhysicalSize;
			Size size = new Size(PrintPreviewControl.PixelsToPhysical(new Point(base.Size), this.screendpi));
			if (this.autoZoom)
			{
				double val = ((double)size.Width - (double)(10 * (this.columns + 1))) / (double)(this.columns * physicalSize.Width);
				double val2 = ((double)size.Height - (double)(10 * (this.rows + 1))) / (double)(this.rows * physicalSize.Height);
				this.zoom = Math.Min(val, val2);
			}
			this.imageSize = new Size((int)(this.zoom * (double)physicalSize.Width), (int)(this.zoom * (double)physicalSize.Height));
			int x = this.imageSize.Width * this.columns + 10 * (this.columns + 1);
			int y = this.imageSize.Height * this.rows + 10 * (this.rows + 1);
			this.SetVirtualSizeNoInvalidate(new Size(PrintPreviewControl.PhysicalToPixels(new Point(x, y), this.screendpi)));
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x001381BC File Offset: 0x001363BC
		private void ComputePreview()
		{
			int num = this.StartPage;
			if (this.document == null)
			{
				this.pageInfo = new PreviewPageInfo[0];
			}
			else
			{
				IntSecurity.SafePrinting.Demand();
				PrintController printController = this.document.PrintController;
				PreviewPrintController previewPrintController = new PreviewPrintController();
				previewPrintController.UseAntiAlias = this.UseAntiAlias;
				this.document.PrintController = new PrintControllerWithStatusDialog(previewPrintController, SR.GetString("PrintControllerWithStatusDialog_DialogTitlePreview"));
				this.document.Print();
				this.pageInfo = previewPrintController.GetPreviewPageInfo();
				this.document.PrintController = printController;
			}
			if (num != this.StartPage)
			{
				this.OnStartPageChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x00138260 File Offset: 0x00136460
		private void InvalidateLayout()
		{
			this.layoutOk = false;
			base.Invalidate();
		}

		/// <summary>Refreshes the preview of the document.</summary>
		// Token: 0x06004BB2 RID: 19378 RVA: 0x0013826F File Offset: 0x0013646F
		public void InvalidatePreview()
		{
			this.pageInfo = null;
			this.InvalidateLayout();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06004BB3 RID: 19379 RVA: 0x0013827E File Offset: 0x0013647E
		protected override void OnResize(EventArgs eventargs)
		{
			this.InvalidateLayout();
			base.OnResize(eventargs);
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x00138290 File Offset: 0x00136490
		private void CalculatePageInfo()
		{
			if (this.pageInfoCalcPending)
			{
				return;
			}
			this.pageInfoCalcPending = true;
			try
			{
				if (this.pageInfo == null)
				{
					try
					{
						this.ComputePreview();
					}
					catch
					{
						this.exceptionPrinting = true;
						throw;
					}
					finally
					{
						base.Invalidate();
					}
				}
			}
			finally
			{
				this.pageInfoCalcPending = false;
			}
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.OnPaint(System.Windows.Forms.PaintEventArgs)" /> method.</summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06004BB5 RID: 19381 RVA: 0x00138300 File Offset: 0x00136500
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Brush brush = new SolidBrush(this.BackColor);
			try
			{
				if (this.pageInfo == null || this.pageInfo.Length == 0)
				{
					pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
					if (this.pageInfo != null || this.exceptionPrinting)
					{
						StringFormat stringFormat = new StringFormat();
						stringFormat.Alignment = ControlPaint.TranslateAlignment(ContentAlignment.MiddleCenter);
						stringFormat.LineAlignment = ControlPaint.TranslateLineAlignment(ContentAlignment.MiddleCenter);
						SolidBrush solidBrush = new SolidBrush(this.ForeColor);
						try
						{
							if (this.exceptionPrinting)
							{
								pevent.Graphics.DrawString(SR.GetString("PrintPreviewExceptionPrinting"), this.Font, solidBrush, base.ClientRectangle, stringFormat);
								goto IL_4E4;
							}
							pevent.Graphics.DrawString(SR.GetString("PrintPreviewNoPages"), this.Font, solidBrush, base.ClientRectangle, stringFormat);
							goto IL_4E4;
						}
						finally
						{
							solidBrush.Dispose();
							stringFormat.Dispose();
						}
					}
					base.BeginInvoke(new MethodInvoker(this.CalculatePageInfo));
				}
				else
				{
					if (!this.layoutOk)
					{
						this.ComputeLayout();
					}
					Size size = new Size(PrintPreviewControl.PixelsToPhysical(new Point(base.Size), this.screendpi));
					Point point = new Point(this.VirtualSize);
					Point point2 = new Point(Math.Max(0, (base.Size.Width - point.X) / 2), Math.Max(0, (base.Size.Height - point.Y) / 2));
					point2.X -= this.Position.X;
					point2.Y -= this.Position.Y;
					this.lastOffset = point2;
					int num = PrintPreviewControl.PhysicalToPixels(10, this.screendpi.X);
					int num2 = PrintPreviewControl.PhysicalToPixels(10, this.screendpi.Y);
					Region clip = pevent.Graphics.Clip;
					Rectangle[] array = new Rectangle[this.rows * this.columns];
					Point empty = Point.Empty;
					int num3 = 0;
					try
					{
						for (int i = 0; i < this.rows; i++)
						{
							empty.X = 0;
							empty.Y = num3 * i;
							for (int j = 0; j < this.columns; j++)
							{
								int num4 = this.StartPage + j + i * this.columns;
								if (num4 < this.pageInfo.Length)
								{
									Size physicalSize = this.pageInfo[num4].PhysicalSize;
									if (this.autoZoom)
									{
										double val = ((double)size.Width - (double)(10 * (this.columns + 1))) / (double)(this.columns * physicalSize.Width);
										double val2 = ((double)size.Height - (double)(10 * (this.rows + 1))) / (double)(this.rows * physicalSize.Height);
										this.zoom = Math.Min(val, val2);
									}
									this.imageSize = new Size((int)(this.zoom * (double)physicalSize.Width), (int)(this.zoom * (double)physicalSize.Height));
									Point point3 = PrintPreviewControl.PhysicalToPixels(new Point(this.imageSize), this.screendpi);
									int x = point2.X + num * (j + 1) + empty.X;
									int y = point2.Y + num2 * (i + 1) + empty.Y;
									empty.X += point3.X;
									num3 = Math.Max(num3, point3.Y);
									array[num4 - this.StartPage] = new Rectangle(x, y, point3.X, point3.Y);
									pevent.Graphics.ExcludeClip(array[num4 - this.StartPage]);
								}
							}
						}
						pevent.Graphics.FillRectangle(brush, base.ClientRectangle);
					}
					finally
					{
						pevent.Graphics.Clip = clip;
					}
					for (int k = 0; k < array.Length; k++)
					{
						if (k + this.StartPage < this.pageInfo.Length)
						{
							Rectangle rect = array[k];
							pevent.Graphics.DrawRectangle(Pens.Black, rect);
							using (SolidBrush solidBrush2 = new SolidBrush(this.ForeColor))
							{
								pevent.Graphics.FillRectangle(solidBrush2, rect);
							}
							rect.Inflate(-1, -1);
							if (this.pageInfo[k + this.StartPage].Image != null)
							{
								pevent.Graphics.DrawImage(this.pageInfo[k + this.StartPage].Image, rect);
							}
							int num5 = rect.Width;
							rect.Width = num5 - 1;
							num5 = rect.Height;
							rect.Height = num5 - 1;
							pevent.Graphics.DrawRectangle(Pens.Black, rect);
						}
					}
				}
			}
			finally
			{
				brush.Dispose();
			}
			IL_4E4:
			base.OnPaint(pevent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PrintPreviewControl.StartPageChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06004BB6 RID: 19382 RVA: 0x0013885C File Offset: 0x00136A5C
		protected virtual void OnStartPageChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[PrintPreviewControl.EVENT_STARTPAGECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x0013888A File Offset: 0x00136A8A
		private static int PhysicalToPixels(int physicalSize, int dpi)
		{
			return (int)((double)(physicalSize * dpi) / 100.0);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x0013889B File Offset: 0x00136A9B
		private static Point PhysicalToPixels(Point physical, Point dpi)
		{
			return new Point(PrintPreviewControl.PhysicalToPixels(physical.X, dpi.X), PrintPreviewControl.PhysicalToPixels(physical.Y, dpi.Y));
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x001388C8 File Offset: 0x00136AC8
		private static Size PhysicalToPixels(Size physicalSize, Point dpi)
		{
			return new Size(PrintPreviewControl.PhysicalToPixels(physicalSize.Width, dpi.X), PrintPreviewControl.PhysicalToPixels(physicalSize.Height, dpi.Y));
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x001388F5 File Offset: 0x00136AF5
		private static int PixelsToPhysical(int pixels, int dpi)
		{
			return (int)((double)pixels * 100.0 / (double)dpi);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x00138907 File Offset: 0x00136B07
		private static Point PixelsToPhysical(Point pixels, Point dpi)
		{
			return new Point(PrintPreviewControl.PixelsToPhysical(pixels.X, dpi.X), PrintPreviewControl.PixelsToPhysical(pixels.Y, dpi.Y));
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x00138934 File Offset: 0x00136B34
		private static Size PixelsToPhysical(Size pixels, Point dpi)
		{
			return new Size(PrintPreviewControl.PixelsToPhysical(pixels.Width, dpi.X), PrintPreviewControl.PixelsToPhysical(pixels.Height, dpi.Y));
		}

		/// <summary>Resets the <see cref="P:System.Windows.Forms.Control.BackColor" /> property to <see cref="P:System.Drawing.SystemColors.AppWorkspace" />, which is the default color.</summary>
		// Token: 0x06004BBD RID: 19389 RVA: 0x00138961 File Offset: 0x00136B61
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetBackColor()
		{
			this.BackColor = SystemColors.AppWorkspace;
		}

		/// <summary>Resets the foreground color of the <see cref="T:System.Windows.Forms.PrintPreviewControl" /> to <see cref="P:System.Drawing.Color.White" />, which is the default color.</summary>
		// Token: 0x06004BBE RID: 19390 RVA: 0x0013896E File Offset: 0x00136B6E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void ResetForeColor()
		{
			this.ForeColor = Color.White;
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x0013897C File Offset: 0x00136B7C
		private void WmHScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Point point = this.position;
			int x = point.X;
			int maxPos = Math.Max(base.Width, this.virtualSize.Width);
			point.X = this.AdjustScroll(m, x, maxPos, true);
			this.Position = point;
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x001389E8 File Offset: 0x00136BE8
		private void SetPositionNoInvalidate(Point value)
		{
			Point point = this.position;
			this.position = value;
			this.position.X = Math.Min(this.position.X, this.virtualSize.Width - base.Width);
			this.position.Y = Math.Min(this.position.Y, this.virtualSize.Height - base.Height);
			if (this.position.X < 0)
			{
				this.position.X = 0;
			}
			if (this.position.Y < 0)
			{
				this.position.Y = 0;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
			SafeNativeMethods.ScrollWindow(new HandleRef(this, base.Handle), point.X - this.position.X, point.Y - this.position.Y, ref rect, ref rect);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 0, this.position.X, true);
			UnsafeNativeMethods.SetScrollPos(new HandleRef(this, base.Handle), 1, this.position.Y, true);
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x00138B34 File Offset: 0x00136D34
		internal void SetVirtualSizeNoInvalidate(Size value)
		{
			this.virtualSize = value;
			this.SetPositionNoInvalidate(this.position);
			NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
			scrollinfo.fMask = 3;
			scrollinfo.nMin = 0;
			scrollinfo.nMax = Math.Max(base.Height, this.virtualSize.Height) - 1;
			scrollinfo.nPage = base.Height;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 1, scrollinfo, true);
			scrollinfo.fMask = 3;
			scrollinfo.nMin = 0;
			scrollinfo.nMax = Math.Max(base.Width, this.virtualSize.Width) - 1;
			scrollinfo.nPage = base.Width;
			UnsafeNativeMethods.SetScrollInfo(new HandleRef(this, base.Handle), 0, scrollinfo, true);
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00138BF4 File Offset: 0x00136DF4
		private void WmVScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Point point = this.Position;
			int y = point.Y;
			int maxPos = Math.Max(base.Height, this.virtualSize.Height);
			point.Y = this.AdjustScroll(m, y, maxPos, false);
			this.Position = point;
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00138C60 File Offset: 0x00136E60
		private void WmKeyDown(ref Message msg)
		{
			Keys keys = (Keys)((int)msg.WParam | (int)Control.ModifierKeys);
			Point point = this.Position;
			switch (keys & Keys.KeyCode)
			{
			case Keys.Prior:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					int num = point.X;
					if (num > 100)
					{
						num -= 100;
					}
					else
					{
						num = 0;
					}
					point.X = num;
					this.Position = point;
					return;
				}
				if (this.StartPage > 0)
				{
					int num2 = this.StartPage;
					this.StartPage = num2 - 1;
					return;
				}
				break;
			case Keys.Next:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					int num = point.X;
					int num3 = Math.Max(base.Width, this.virtualSize.Width);
					if (num < num3 - 100)
					{
						num += 100;
					}
					else
					{
						num = num3;
					}
					point.X = num;
					this.Position = point;
					return;
				}
				if (this.StartPage < this.pageInfo.Length)
				{
					int num2 = this.StartPage;
					this.StartPage = num2 + 1;
					return;
				}
				break;
			case Keys.End:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					this.StartPage = this.pageInfo.Length;
					return;
				}
				break;
			case Keys.Home:
				if ((keys & Keys.Modifiers) == Keys.Control)
				{
					this.StartPage = 0;
					return;
				}
				break;
			case Keys.Left:
			{
				int num = point.X;
				if (num > 5)
				{
					num -= 5;
				}
				else
				{
					num = 0;
				}
				point.X = num;
				this.Position = point;
				return;
			}
			case Keys.Up:
			{
				int num = point.Y;
				if (num > 5)
				{
					num -= 5;
				}
				else
				{
					num = 0;
				}
				point.Y = num;
				this.Position = point;
				return;
			}
			case Keys.Right:
			{
				int num = point.X;
				int num3 = Math.Max(base.Width, this.virtualSize.Width);
				if (num < num3 - 5)
				{
					num += 5;
				}
				else
				{
					num = num3;
				}
				point.X = num;
				this.Position = point;
				break;
			}
			case Keys.Down:
			{
				int num = point.Y;
				int num3 = Math.Max(base.Height, this.virtualSize.Height);
				if (num < num3 - 5)
				{
					num += 5;
				}
				else
				{
					num = num3;
				}
				point.Y = num;
				this.Position = point;
				return;
			}
			default:
				return;
			}
		}

		/// <summary>Overrides the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process. </param>
		// Token: 0x06004BC4 RID: 19396 RVA: 0x00138E80 File Offset: 0x00137080
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 256)
			{
				this.WmKeyDown(ref m);
				return;
			}
			if (msg == 276)
			{
				this.WmHScroll(ref m);
				return;
			}
			if (msg == 277)
			{
				this.WmVScroll(ref m);
				return;
			}
			base.WndProc(ref m);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x00138ECC File Offset: 0x001370CC
		internal override bool ShouldSerializeBackColor()
		{
			return !this.BackColor.Equals(SystemColors.AppWorkspace);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x00138EFC File Offset: 0x001370FC
		internal override bool ShouldSerializeForeColor()
		{
			return !this.ForeColor.Equals(Color.White);
		}

		// Token: 0x040027A1 RID: 10145
		private Size virtualSize = new Size(1, 1);

		// Token: 0x040027A2 RID: 10146
		private Point position = new Point(0, 0);

		// Token: 0x040027A3 RID: 10147
		private Point lastOffset;

		// Token: 0x040027A4 RID: 10148
		private bool antiAlias;

		// Token: 0x040027A5 RID: 10149
		private const int SCROLL_PAGE = 100;

		// Token: 0x040027A6 RID: 10150
		private const int SCROLL_LINE = 5;

		// Token: 0x040027A7 RID: 10151
		private const double DefaultZoom = 0.3;

		// Token: 0x040027A8 RID: 10152
		private const int border = 10;

		// Token: 0x040027A9 RID: 10153
		private PrintDocument document;

		// Token: 0x040027AA RID: 10154
		private PreviewPageInfo[] pageInfo;

		// Token: 0x040027AB RID: 10155
		private int startPage;

		// Token: 0x040027AC RID: 10156
		private int rows = 1;

		// Token: 0x040027AD RID: 10157
		private int columns = 1;

		// Token: 0x040027AE RID: 10158
		private bool autoZoom = true;

		// Token: 0x040027AF RID: 10159
		private bool layoutOk;

		// Token: 0x040027B0 RID: 10160
		private Size imageSize = Size.Empty;

		// Token: 0x040027B1 RID: 10161
		private Point screendpi = Point.Empty;

		// Token: 0x040027B2 RID: 10162
		private double zoom = 0.3;

		// Token: 0x040027B3 RID: 10163
		private bool pageInfoCalcPending;

		// Token: 0x040027B4 RID: 10164
		private bool exceptionPrinting;

		// Token: 0x040027B5 RID: 10165
		private static readonly object EVENT_STARTPAGECHANGED = new object();
	}
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Defines a base class for controls that support auto-scrolling behavior.</summary>
	// Token: 0x02000344 RID: 836
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.ScrollableControlDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ScrollableControl : Control, IArrangedElement, IComponent, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ScrollableControl" /> class.</summary>
		// Token: 0x06003447 RID: 13383 RVA: 0x000EF90C File Offset: 0x000EDB0C
		public ScrollableControl()
		{
			base.SetStyle(ControlStyles.ContainerControl, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetScrollState(1, false);
		}

		/// <summary>Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.</summary>
		/// <returns>
		///     <see langword="true" /> if the container enables auto-scrolling; otherwise, <see langword="false" />. The default value is <see langword="false" />. </returns>
		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06003448 RID: 13384 RVA: 0x000EF972 File Offset: 0x000EDB72
		// (set) Token: 0x06003449 RID: 13385 RVA: 0x000EF97B File Offset: 0x000EDB7B
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("FormAutoScrollDescr")]
		public virtual bool AutoScroll
		{
			get
			{
				return this.GetScrollState(1);
			}
			set
			{
				if (value)
				{
					this.UpdateFullDrag();
				}
				this.SetScrollState(1, value);
				LayoutTransaction.DoLayout(this, this, PropertyNames.AutoScroll);
			}
		}

		/// <summary>Gets or sets the size of the auto-scroll margin.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the height and width of the auto-scroll margin in pixels.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.Drawing.Size.Height" /> or <see cref="P:System.Drawing.Size.Width" /> value assigned is less than 0. </exception>
		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x0600344A RID: 13386 RVA: 0x000EF99A File Offset: 0x000EDB9A
		// (set) Token: 0x0600344B RID: 13387 RVA: 0x000EF9A4 File Offset: 0x000EDBA4
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FormAutoScrollMarginDescr")]
		public Size AutoScrollMargin
		{
			get
			{
				return this.requestedScrollMargin;
			}
			set
			{
				if (value.Width < 0 || value.Height < 0)
				{
					throw new ArgumentOutOfRangeException("AutoScrollMargin", SR.GetString("InvalidArgument", new object[]
					{
						"AutoScrollMargin",
						value.ToString()
					}));
				}
				this.SetAutoScrollMargin(value.Width, value.Height);
			}
		}

		/// <summary>Gets or sets the location of the auto-scroll position.</summary>
		/// <returns>A <see cref="T:System.Drawing.Point" /> that represents the auto-scroll position in pixels.</returns>
		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x0600344C RID: 13388 RVA: 0x000EFA0C File Offset: 0x000EDC0C
		// (set) Token: 0x0600344D RID: 13389 RVA: 0x000EFA33 File Offset: 0x000EDC33
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FormAutoScrollPositionDescr")]
		public Point AutoScrollPosition
		{
			get
			{
				Rectangle displayRectInternal = this.GetDisplayRectInternal();
				return new Point(displayRectInternal.X, displayRectInternal.Y);
			}
			set
			{
				if (base.Created)
				{
					this.SetDisplayRectLocation(-value.X, -value.Y);
					this.SyncScrollbars(true);
				}
				this.scrollPosition = value;
			}
		}

		/// <summary>Gets or sets the minimum size of the auto-scroll.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that determines the minimum size of the virtual area through which the user can scroll.</returns>
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x0600344E RID: 13390 RVA: 0x000EFA61 File Offset: 0x000EDC61
		// (set) Token: 0x0600344F RID: 13391 RVA: 0x000EFA69 File Offset: 0x000EDC69
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FormAutoScrollMinSizeDescr")]
		public Size AutoScrollMinSize
		{
			get
			{
				return this.userAutoScrollMinSize;
			}
			set
			{
				if (value != this.userAutoScrollMinSize)
				{
					this.userAutoScrollMinSize = value;
					this.AutoScroll = true;
					base.PerformLayout();
				}
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06003450 RID: 13392 RVA: 0x000EFA90 File Offset: 0x000EDC90
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (this.HScroll || this.HorizontalScroll.Visible)
				{
					createParams.Style |= 1048576;
				}
				else
				{
					createParams.Style &= -1048577;
				}
				if (this.VScroll || this.VerticalScroll.Visible)
				{
					createParams.Style |= 2097152;
				}
				else
				{
					createParams.Style &= -2097153;
				}
				return createParams;
			}
		}

		/// <summary>Gets the rectangle that represents the virtual display area of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the display area of the control.</returns>
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x000EFB1C File Offset: 0x000EDD1C
		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle clientRectangle = base.ClientRectangle;
				if (!this.displayRect.IsEmpty)
				{
					clientRectangle.X = this.displayRect.X;
					clientRectangle.Y = this.displayRect.Y;
					if (this.HScroll)
					{
						clientRectangle.Width = this.displayRect.Width;
					}
					if (this.VScroll)
					{
						clientRectangle.Height = this.displayRect.Height;
					}
				}
				return LayoutUtils.DeflateRect(clientRectangle, base.Padding);
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06003452 RID: 13394 RVA: 0x000EFBA4 File Offset: 0x000EDDA4
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				Rectangle displayRectangle = this.DisplayRectangle;
				if (this.AutoScrollMinSize.Width != 0 && this.AutoScrollMinSize.Height != 0)
				{
					displayRectangle.Width = Math.Max(displayRectangle.Width, this.AutoScrollMinSize.Width);
					displayRectangle.Height = Math.Max(displayRectangle.Height, this.AutoScrollMinSize.Height);
				}
				return displayRectangle;
			}
		}

		/// <summary>Gets or sets a value indicating whether the horizontal scroll bar is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the horizontal scroll bar is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x000EFC1B File Offset: 0x000EDE1B
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x000EFC24 File Offset: 0x000EDE24
		protected bool HScroll
		{
			get
			{
				return this.GetScrollState(2);
			}
			set
			{
				this.SetScrollState(2, value);
			}
		}

		/// <summary>Gets the characteristics associated with the horizontal scroll bar.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.HScrollProperties" /> that contains information about the horizontal scroll bar.</returns>
		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x000EFC2E File Offset: 0x000EDE2E
		[SRCategory("CatLayout")]
		[SRDescription("ScrollableControlHorizontalScrollDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public HScrollProperties HorizontalScroll
		{
			get
			{
				if (this.horizontalScroll == null)
				{
					this.horizontalScroll = new HScrollProperties(this);
				}
				return this.horizontalScroll;
			}
		}

		/// <summary>Gets or sets a value indicating whether the vertical scroll bar is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the vertical scroll bar is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x000EFC4A File Offset: 0x000EDE4A
		// (set) Token: 0x06003457 RID: 13399 RVA: 0x000EFC53 File Offset: 0x000EDE53
		protected bool VScroll
		{
			get
			{
				return this.GetScrollState(4);
			}
			set
			{
				this.SetScrollState(4, value);
			}
		}

		/// <summary>Gets the characteristics associated with the vertical scroll bar.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.VScrollProperties" /> that contains information about the vertical scroll bar.</returns>
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x000EFC5D File Offset: 0x000EDE5D
		[SRCategory("CatLayout")]
		[SRDescription("ScrollableControlVerticalScrollDescr")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public VScrollProperties VerticalScroll
		{
			get
			{
				if (this.verticalScroll == null)
				{
					this.verticalScroll = new VScrollProperties(this);
				}
				return this.verticalScroll;
			}
		}

		/// <summary>Gets the dock padding settings for all edges of the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> that represents the padding for all the edges of a docked control.</returns>
		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06003459 RID: 13401 RVA: 0x000EFC79 File Offset: 0x000EDE79
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				if (this.dockPadding == null)
				{
					this.dockPadding = new ScrollableControl.DockPaddingEdges(this);
				}
				return this.dockPadding;
			}
		}

		/// <summary>Adjusts the scroll bars on the container based on the current control positions and the control currently selected. </summary>
		/// <param name="displayScrollbars">
		///       <see langword="true" /> to show the scroll bars; otherwise, <see langword="false" />. </param>
		// Token: 0x0600345A RID: 13402 RVA: 0x000EFC98 File Offset: 0x000EDE98
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void AdjustFormScrollbars(bool displayScrollbars)
		{
			bool flag = false;
			Rectangle displayRectInternal = this.GetDisplayRectInternal();
			if (!displayScrollbars && (this.HScroll || this.VScroll))
			{
				flag = this.SetVisibleScrollbars(false, false);
			}
			if (!displayScrollbars)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				displayRectInternal.Width = clientRectangle.Width;
				displayRectInternal.Height = clientRectangle.Height;
			}
			else
			{
				flag |= this.ApplyScrollbarChanges(displayRectInternal);
			}
			if (flag)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.DisplayRectangle);
			}
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x000EFD0C File Offset: 0x000EDF0C
		private bool ApplyScrollbarChanges(Rectangle display)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = clientRectangle;
			Rectangle rectangle2 = rectangle;
			if (this.HScroll)
			{
				rectangle.Height += SystemInformation.HorizontalScrollBarHeight;
			}
			else
			{
				rectangle2.Height -= SystemInformation.HorizontalScrollBarHeight;
			}
			if (this.VScroll)
			{
				rectangle.Width += SystemInformation.VerticalScrollBarWidth;
			}
			else
			{
				rectangle2.Width -= SystemInformation.VerticalScrollBarWidth;
			}
			int num = rectangle2.Width;
			int num2 = rectangle2.Height;
			if (base.Controls.Count != 0)
			{
				this.scrollMargin = this.requestedScrollMargin;
				if (this.dockPadding != null)
				{
					this.scrollMargin.Height = this.scrollMargin.Height + base.Padding.Bottom;
					this.scrollMargin.Width = this.scrollMargin.Width + base.Padding.Right;
				}
				for (int i = 0; i < base.Controls.Count; i++)
				{
					Control control = base.Controls[i];
					if (control != null && control.GetState(2))
					{
						DockStyle dock = control.Dock;
						if (dock != DockStyle.Bottom)
						{
							if (dock == DockStyle.Right)
							{
								this.scrollMargin.Width = this.scrollMargin.Width + control.Size.Width;
							}
						}
						else
						{
							this.scrollMargin.Height = this.scrollMargin.Height + control.Size.Height;
						}
					}
				}
			}
			if (!this.userAutoScrollMinSize.IsEmpty)
			{
				num = this.userAutoScrollMinSize.Width + this.scrollMargin.Width;
				num2 = this.userAutoScrollMinSize.Height + this.scrollMargin.Height;
				flag2 = true;
				flag3 = true;
			}
			bool flag4 = this.LayoutEngine == DefaultLayout.Instance;
			if (!flag4 && CommonProperties.HasLayoutBounds(this))
			{
				Size layoutBounds = CommonProperties.GetLayoutBounds(this);
				if (layoutBounds.Width > num)
				{
					flag2 = true;
					num = layoutBounds.Width;
				}
				if (layoutBounds.Height > num2)
				{
					flag3 = true;
					num2 = layoutBounds.Height;
				}
			}
			else if (base.Controls.Count != 0)
			{
				for (int j = 0; j < base.Controls.Count; j++)
				{
					bool flag5 = true;
					bool flag6 = true;
					Control control2 = base.Controls[j];
					if (control2 != null && control2.GetState(2))
					{
						if (flag4)
						{
							Control control3 = control2;
							switch (control3.Dock)
							{
							case DockStyle.Top:
								flag5 = false;
								break;
							case DockStyle.Bottom:
							case DockStyle.Right:
							case DockStyle.Fill:
								flag5 = false;
								flag6 = false;
								break;
							case DockStyle.Left:
								flag6 = false;
								break;
							default:
							{
								AnchorStyles anchor = control3.Anchor;
								if ((anchor & AnchorStyles.Right) == AnchorStyles.Right)
								{
									flag5 = false;
								}
								if ((anchor & AnchorStyles.Left) != AnchorStyles.Left)
								{
									flag5 = false;
								}
								if ((anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom)
								{
									flag6 = false;
								}
								if ((anchor & AnchorStyles.Top) != AnchorStyles.Top)
								{
									flag6 = false;
								}
								break;
							}
							}
						}
						if (flag5 || flag6)
						{
							Rectangle bounds = control2.Bounds;
							int num3 = -display.X + bounds.X + bounds.Width + this.scrollMargin.Width;
							int num4 = -display.Y + bounds.Y + bounds.Height + this.scrollMargin.Height;
							if (!flag4)
							{
								num3 += control2.Margin.Right;
								num4 += control2.Margin.Bottom;
							}
							if (num3 > num && flag5)
							{
								flag2 = true;
								num = num3;
							}
							if (num4 > num2 && flag6)
							{
								flag3 = true;
								num2 = num4;
							}
						}
					}
				}
			}
			if (num <= rectangle.Width)
			{
				flag2 = false;
			}
			if (num2 <= rectangle.Height)
			{
				flag3 = false;
			}
			Rectangle rectangle3 = rectangle;
			if (flag2)
			{
				rectangle3.Height -= SystemInformation.HorizontalScrollBarHeight;
			}
			if (flag3)
			{
				rectangle3.Width -= SystemInformation.VerticalScrollBarWidth;
			}
			if (flag2 && num2 > rectangle3.Height)
			{
				flag3 = true;
			}
			if (flag3 && num > rectangle3.Width)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				num = rectangle3.Width;
			}
			if (!flag3)
			{
				num2 = rectangle3.Height;
			}
			flag = (this.SetVisibleScrollbars(flag2, flag3) || flag);
			if (this.HScroll || this.VScroll)
			{
				flag = (this.SetDisplayRectangleSize(num, num2) || flag);
			}
			else
			{
				this.SetDisplayRectangleSize(num, num2);
			}
			this.SyncScrollbars(true);
			return flag;
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000F0170 File Offset: 0x000EE370
		private Rectangle GetDisplayRectInternal()
		{
			if (this.displayRect.IsEmpty)
			{
				this.displayRect = base.ClientRectangle;
			}
			if (!this.AutoScroll && this.HorizontalScroll.visible)
			{
				this.displayRect = new Rectangle(this.displayRect.X, this.displayRect.Y, this.HorizontalScroll.Maximum, this.displayRect.Height);
			}
			if (!this.AutoScroll && this.VerticalScroll.visible)
			{
				this.displayRect = new Rectangle(this.displayRect.X, this.displayRect.Y, this.displayRect.Width, this.VerticalScroll.Maximum);
			}
			return this.displayRect;
		}

		/// <summary>Determines whether the specified flag has been set.</summary>
		/// <param name="bit">The flag to check.</param>
		/// <returns>
		///     <see langword="true" /> if the specified flag has been set; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600345D RID: 13405 RVA: 0x000F0234 File Offset: 0x000EE434
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected bool GetScrollState(int bit)
		{
			return (bit & this.scrollState) == bit;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="levent">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x0600345E RID: 13406 RVA: 0x000F0241 File Offset: 0x000EE441
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (levent.AffectedControl != null && this.AutoScroll)
			{
				base.OnLayout(levent);
			}
			this.AdjustFormScrollbars(this.AutoScroll);
			base.OnLayout(levent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		// Token: 0x0600345F RID: 13407 RVA: 0x000F0270 File Offset: 0x000EE470
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (this.VScroll)
			{
				Rectangle clientRectangle = base.ClientRectangle;
				int num = -this.displayRect.Y;
				int val = -(clientRectangle.Height - this.displayRect.Height);
				num = Math.Max(num - e.Delta, 0);
				num = Math.Min(num, val);
				this.SetDisplayRectLocation(this.displayRect.X, -num);
				this.SyncScrollbars(this.AutoScroll);
				if (e is HandledMouseEventArgs)
				{
					((HandledMouseEventArgs)e).Handled = true;
				}
			}
			else if (this.HScroll)
			{
				Rectangle clientRectangle2 = base.ClientRectangle;
				int num2 = -this.displayRect.X;
				int val2 = -(clientRectangle2.Width - this.displayRect.Width);
				num2 = Math.Max(num2 - e.Delta, 0);
				num2 = Math.Min(num2, val2);
				this.SetDisplayRectLocation(-num2, this.displayRect.Y);
				this.SyncScrollbars(this.AutoScroll);
				if (e is HandledMouseEventArgs)
				{
					((HandledMouseEventArgs)e).Handled = true;
				}
			}
			base.OnMouseWheel(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003460 RID: 13408 RVA: 0x000F038A File Offset: 0x000EE58A
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			this.resetRTLHScrollValue = true;
			LayoutTransaction.DoLayout(this, this, PropertyNames.RightToLeft);
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06003461 RID: 13409 RVA: 0x000F03A8 File Offset: 0x000EE5A8
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if ((this.HScroll || this.VScroll) && this.BackgroundImage != null && (this.BackgroundImageLayout == ImageLayout.Zoom || this.BackgroundImageLayout == ImageLayout.Stretch || this.BackgroundImageLayout == ImageLayout.Center))
			{
				if (ControlPaint.IsImageTransparent(this.BackgroundImage))
				{
					base.PaintTransparentBackground(e, this.displayRect);
				}
				ControlPaint.DrawBackgroundImage(e.Graphics, this.BackgroundImage, this.BackColor, this.BackgroundImageLayout, this.displayRect, this.displayRect, this.displayRect.Location);
				return;
			}
			base.OnPaintBackground(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06003462 RID: 13410 RVA: 0x000F0440 File Offset: 0x000EE640
		protected override void OnPaddingChanged(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[Control.EventPaddingChanged];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
		// Token: 0x06003463 RID: 13411 RVA: 0x000F046E File Offset: 0x000EE66E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnVisibleChanged(EventArgs e)
		{
			if (base.Visible)
			{
				LayoutTransaction.DoLayout(this, this, PropertyNames.Visible);
			}
			base.OnVisibleChanged(e);
		}

		// Token: 0x06003464 RID: 13412 RVA: 0x000F048B File Offset: 0x000EE68B
		internal void ScaleDockPadding(float dx, float dy)
		{
			if (this.dockPadding != null)
			{
				this.dockPadding.Scale(dx, dy);
			}
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="dx">The horizontal scaling factor.</param>
		/// <param name="dy">The vertical scaling factor.</param>
		// Token: 0x06003465 RID: 13413 RVA: 0x000F04A2 File Offset: 0x000EE6A2
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void ScaleCore(float dx, float dy)
		{
			this.ScaleDockPadding(dx, dy);
			base.ScaleCore(dx, dy);
		}

		/// <summary>Scales a control's location, size, padding and margin.</summary>
		/// <param name="factor">The factor by which the height and width of the control will be scaled.
		/// </param>
		/// <param name="specified">A <see cref="T:System.Windows.Forms.BoundsSpecified" /> value that specifies the bounds of the control to use when defining its size and position.</param>
		// Token: 0x06003466 RID: 13414 RVA: 0x000F04B4 File Offset: 0x000EE6B4
		protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
		{
			this.ScaleDockPadding(factor.Width, factor.Height);
			base.ScaleControl(factor, specified);
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x000F04D4 File Offset: 0x000EE6D4
		internal void SetDisplayFromScrollProps(int x, int y)
		{
			Rectangle displayRectInternal = this.GetDisplayRectInternal();
			this.ApplyScrollbarChanges(displayRectInternal);
			this.SetDisplayRectLocation(x, y);
		}

		/// <summary>Positions the display window to the specified value.</summary>
		/// <param name="x">The horizontal offset at which to position the <see cref="T:System.Windows.Forms.ScrollableControl" />.</param>
		/// <param name="y">The vertical offset at which to position the <see cref="T:System.Windows.Forms.ScrollableControl" />.</param>
		// Token: 0x06003468 RID: 13416 RVA: 0x000F04F8 File Offset: 0x000EE6F8
		protected void SetDisplayRectLocation(int x, int y)
		{
			int num = 0;
			int num2 = 0;
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = this.displayRect;
			int num3 = Math.Min(clientRectangle.Width - rectangle.Width, 0);
			int num4 = Math.Min(clientRectangle.Height - rectangle.Height, 0);
			if (x > 0)
			{
				x = 0;
			}
			if (y > 0)
			{
				y = 0;
			}
			if (x < num3)
			{
				x = num3;
			}
			if (y < num4)
			{
				y = num4;
			}
			if (rectangle.X != x)
			{
				num = x - rectangle.X;
			}
			if (rectangle.Y != y)
			{
				num2 = y - rectangle.Y;
			}
			this.displayRect.X = x;
			this.displayRect.Y = y;
			if (num != 0 || (num2 != 0 && base.IsHandleCreated))
			{
				Rectangle clientRectangle2 = base.ClientRectangle;
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(clientRectangle2.X, clientRectangle2.Y, clientRectangle2.Width, clientRectangle2.Height);
				NativeMethods.RECT rect2 = NativeMethods.RECT.FromXYWH(clientRectangle2.X, clientRectangle2.Y, clientRectangle2.Width, clientRectangle2.Height);
				SafeNativeMethods.ScrollWindowEx(new HandleRef(this, base.Handle), num, num2, null, ref rect, NativeMethods.NullHandleRef, ref rect2, 7);
			}
			for (int i = 0; i < base.Controls.Count; i++)
			{
				Control control = base.Controls[i];
				if (control != null && control.IsHandleCreated)
				{
					control.UpdateBounds();
				}
			}
		}

		/// <summary>Scrolls the specified child control into view on an auto-scroll enabled control.</summary>
		/// <param name="activeControl">The child control to scroll into view. </param>
		// Token: 0x06003469 RID: 13417 RVA: 0x000F0660 File Offset: 0x000EE860
		public void ScrollControlIntoView(Control activeControl)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			if (base.IsDescendant(activeControl) && this.AutoScroll && (this.HScroll || this.VScroll) && activeControl != null && clientRectangle.Width > 0 && clientRectangle.Height > 0)
			{
				Point point = this.ScrollToControl(activeControl);
				this.SetScrollState(8, false);
				this.SetDisplayRectLocation(point.X, point.Y);
				this.SyncScrollbars(true);
			}
		}

		/// <summary>Calculates the scroll offset to the specified child control. </summary>
		/// <param name="activeControl">The child control to scroll into view. </param>
		/// <returns>The upper-left hand <see cref="T:System.Drawing.Point" /> of the display area relative to the client area required to scroll the control into view.</returns>
		// Token: 0x0600346A RID: 13418 RVA: 0x000F06D8 File Offset: 0x000EE8D8
		protected virtual Point ScrollToControl(Control activeControl)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			int num = this.displayRect.X;
			int num2 = this.displayRect.Y;
			int width = this.scrollMargin.Width;
			int height = this.scrollMargin.Height;
			Rectangle r = activeControl.Bounds;
			if (activeControl.ParentInternal != this)
			{
				r = base.RectangleToClient(activeControl.ParentInternal.RectangleToScreen(r));
			}
			if (r.X < width)
			{
				num = this.displayRect.X + width - r.X;
			}
			else if (r.X + r.Width + width > clientRectangle.Width)
			{
				num = clientRectangle.Width - (r.X + r.Width + width - this.displayRect.X);
				if (r.X + num - this.displayRect.X < width)
				{
					num = this.displayRect.X + width - r.X;
				}
			}
			if (r.Y < height)
			{
				num2 = this.displayRect.Y + height - r.Y;
			}
			else if (r.Y + r.Height + height > clientRectangle.Height)
			{
				num2 = clientRectangle.Height - (r.Y + r.Height + height - this.displayRect.Y);
				if (r.Y + num2 - this.displayRect.Y < height)
				{
					num2 = this.displayRect.Y + height - r.Y;
				}
			}
			num += activeControl.AutoScrollOffset.X;
			num2 += activeControl.AutoScrollOffset.Y;
			return new Point(num, num2);
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x000F0894 File Offset: 0x000EEA94
		private int ScrollThumbPosition(int fnBar)
		{
			NativeMethods.SCROLLINFO scrollinfo = new NativeMethods.SCROLLINFO();
			scrollinfo.fMask = 16;
			SafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), fnBar, scrollinfo);
			return scrollinfo.nTrackPos;
		}

		/// <summary>Occurs when the user or code scrolls through the client area.</summary>
		// Token: 0x1400027B RID: 635
		// (add) Token: 0x0600346C RID: 13420 RVA: 0x000F08C9 File Offset: 0x000EEAC9
		// (remove) Token: 0x0600346D RID: 13421 RVA: 0x000F08DC File Offset: 0x000EEADC
		[SRCategory("CatAction")]
		[SRDescription("ScrollBarOnScrollDescr")]
		public event ScrollEventHandler Scroll
		{
			add
			{
				base.Events.AddHandler(ScrollableControl.EVENT_SCROLL, value);
			}
			remove
			{
				base.Events.RemoveHandler(ScrollableControl.EVENT_SCROLL, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ScrollableControl.Scroll" /> event.</summary>
		/// <param name="se">A <see cref="T:System.Windows.Forms.ScrollEventArgs" /> that contains the event data. </param>
		// Token: 0x0600346E RID: 13422 RVA: 0x000F08F0 File Offset: 0x000EEAF0
		protected virtual void OnScroll(ScrollEventArgs se)
		{
			ScrollEventHandler scrollEventHandler = (ScrollEventHandler)base.Events[ScrollableControl.EVENT_SCROLL];
			if (scrollEventHandler != null)
			{
				scrollEventHandler(this, se);
			}
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000F091E File Offset: 0x000EEB1E
		private void ResetAutoScrollMargin()
		{
			this.AutoScrollMargin = Size.Empty;
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000F092B File Offset: 0x000EEB2B
		private void ResetAutoScrollMinSize()
		{
			this.AutoScrollMinSize = Size.Empty;
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000F0938 File Offset: 0x000EEB38
		private void ResetScrollProperties(ScrollProperties scrollProperties)
		{
			scrollProperties.visible = false;
			scrollProperties.value = 0;
		}

		/// <summary>Sets the size of the auto-scroll margins.</summary>
		/// <param name="x">The <see cref="P:System.Drawing.Size.Width" /> value. </param>
		/// <param name="y">The <see cref="P:System.Drawing.Size.Height" /> value. </param>
		// Token: 0x06003472 RID: 13426 RVA: 0x000F0948 File Offset: 0x000EEB48
		public void SetAutoScrollMargin(int x, int y)
		{
			if (x < 0)
			{
				x = 0;
			}
			if (y < 0)
			{
				y = 0;
			}
			if (x != this.requestedScrollMargin.Width || y != this.requestedScrollMargin.Height)
			{
				this.requestedScrollMargin = new Size(x, y);
				if (this.AutoScroll)
				{
					base.PerformLayout();
				}
			}
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000F099C File Offset: 0x000EEB9C
		private bool SetVisibleScrollbars(bool horiz, bool vert)
		{
			bool flag = false;
			if ((!horiz && this.HScroll) || (horiz && !this.HScroll) || (!vert && this.VScroll) || (vert && !this.VScroll))
			{
				flag = true;
			}
			if (horiz && !this.HScroll && this.RightToLeft == RightToLeft.Yes)
			{
				this.resetRTLHScrollValue = true;
			}
			if (flag)
			{
				int x = this.displayRect.X;
				int y = this.displayRect.Y;
				if (!horiz)
				{
					x = 0;
				}
				if (!vert)
				{
					y = 0;
				}
				this.SetDisplayRectLocation(x, y);
				this.SetScrollState(8, false);
				this.HScroll = horiz;
				this.VScroll = vert;
				if (horiz)
				{
					this.HorizontalScroll.visible = true;
				}
				else
				{
					this.ResetScrollProperties(this.HorizontalScroll);
				}
				if (vert)
				{
					this.VerticalScroll.visible = true;
				}
				else
				{
					this.ResetScrollProperties(this.VerticalScroll);
				}
				base.UpdateStyles();
			}
			return flag;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000F0A7C File Offset: 0x000EEC7C
		private bool SetDisplayRectangleSize(int width, int height)
		{
			bool result = false;
			if (this.displayRect.Width != width || this.displayRect.Height != height)
			{
				this.displayRect.Width = width;
				this.displayRect.Height = height;
				result = true;
			}
			int num = base.ClientRectangle.Width - width;
			int num2 = base.ClientRectangle.Height - height;
			if (num > 0)
			{
				num = 0;
			}
			if (num2 > 0)
			{
				num2 = 0;
			}
			int num3 = this.displayRect.X;
			int num4 = this.displayRect.Y;
			if (!this.HScroll)
			{
				num3 = 0;
			}
			if (!this.VScroll)
			{
				num4 = 0;
			}
			if (num3 < num)
			{
				num3 = num;
			}
			if (num4 < num2)
			{
				num4 = num2;
			}
			this.SetDisplayRectLocation(num3, num4);
			return result;
		}

		/// <summary>Sets the specified scroll state flag.</summary>
		/// <param name="bit">The scroll state flag to set. </param>
		/// <param name="value">The value to set the flag. </param>
		// Token: 0x06003475 RID: 13429 RVA: 0x000F0B37 File Offset: 0x000EED37
		protected void SetScrollState(int bit, bool value)
		{
			if (value)
			{
				this.scrollState |= bit;
				return;
			}
			this.scrollState &= ~bit;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x000F0B5C File Offset: 0x000EED5C
		private bool ShouldSerializeAutoScrollPosition()
		{
			if (this.AutoScroll)
			{
				Point autoScrollPosition = this.AutoScrollPosition;
				if (autoScrollPosition.X != 0 || autoScrollPosition.Y != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x000F0B90 File Offset: 0x000EED90
		private bool ShouldSerializeAutoScrollMargin()
		{
			return !this.AutoScrollMargin.Equals(new Size(0, 0));
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x000F0BC0 File Offset: 0x000EEDC0
		private bool ShouldSerializeAutoScrollMinSize()
		{
			return !this.AutoScrollMinSize.Equals(new Size(0, 0));
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x000F0BF0 File Offset: 0x000EEDF0
		private void SyncScrollbars(bool autoScroll)
		{
			Rectangle rectangle = this.displayRect;
			if (autoScroll)
			{
				if (!base.IsHandleCreated)
				{
					return;
				}
				if (this.HScroll)
				{
					if (!this.HorizontalScroll.maximumSetExternally)
					{
						this.HorizontalScroll.maximum = rectangle.Width - 1;
					}
					if (!this.HorizontalScroll.largeChangeSetExternally)
					{
						this.HorizontalScroll.largeChange = base.ClientRectangle.Width;
					}
					if (!this.HorizontalScroll.smallChangeSetExternally)
					{
						this.HorizontalScroll.smallChange = 5;
					}
					if (this.resetRTLHScrollValue && !base.IsMirrored)
					{
						this.resetRTLHScrollValue = false;
						base.BeginInvoke(new EventHandler(this.OnSetScrollPosition));
					}
					else if (-rectangle.X >= this.HorizontalScroll.minimum && -rectangle.X < this.HorizontalScroll.maximum)
					{
						this.HorizontalScroll.value = -rectangle.X;
					}
					this.HorizontalScroll.UpdateScrollInfo();
				}
				if (this.VScroll)
				{
					if (!this.VerticalScroll.maximumSetExternally)
					{
						this.VerticalScroll.maximum = rectangle.Height - 1;
					}
					if (!this.VerticalScroll.largeChangeSetExternally)
					{
						this.VerticalScroll.largeChange = base.ClientRectangle.Height;
					}
					if (!this.VerticalScroll.smallChangeSetExternally)
					{
						this.VerticalScroll.smallChange = 5;
					}
					if (-rectangle.Y >= this.VerticalScroll.minimum && -rectangle.Y < this.VerticalScroll.maximum)
					{
						this.VerticalScroll.value = -rectangle.Y;
					}
					this.VerticalScroll.UpdateScrollInfo();
					return;
				}
			}
			else
			{
				if (this.HorizontalScroll.Visible)
				{
					this.HorizontalScroll.Value = -rectangle.X;
				}
				else
				{
					this.ResetScrollProperties(this.HorizontalScroll);
				}
				if (this.VerticalScroll.Visible)
				{
					this.VerticalScroll.Value = -rectangle.Y;
					return;
				}
				this.ResetScrollProperties(this.VerticalScroll);
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x000F0E01 File Offset: 0x000EF001
		private void OnSetScrollPosition(object sender, EventArgs e)
		{
			if (!base.IsMirrored)
			{
				base.SendMessage(276, NativeMethods.Util.MAKELPARAM((this.RightToLeft == RightToLeft.Yes) ? 7 : 6, 0), 0);
			}
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x000F0E2B File Offset: 0x000EF02B
		private void UpdateFullDrag()
		{
			this.SetScrollState(16, SystemInformation.DragFullWindows);
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x000F0E3C File Offset: 0x000EF03C
		private void WmVScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			bool flag = NativeMethods.Util.LOWORD(m.WParam) != 5;
			int num = -this.displayRect.Y;
			int oldValue = num;
			int num2 = -(clientRectangle.Height - this.displayRect.Height);
			if (!this.AutoScroll)
			{
				num2 = this.VerticalScroll.Maximum;
			}
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (num > 0)
				{
					num -= this.VerticalScroll.SmallChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 1:
				if (num < num2 - this.VerticalScroll.SmallChange)
				{
					num += this.VerticalScroll.SmallChange;
				}
				else
				{
					num = num2;
				}
				break;
			case 2:
				if (num > this.VerticalScroll.LargeChange)
				{
					num -= this.VerticalScroll.LargeChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 3:
				if (num < num2 - this.VerticalScroll.LargeChange)
				{
					num += this.VerticalScroll.LargeChange;
				}
				else
				{
					num = num2;
				}
				break;
			case 4:
			case 5:
				num = this.ScrollThumbPosition(1);
				break;
			case 6:
				num = 0;
				break;
			case 7:
				num = num2;
				break;
			}
			if (this.GetScrollState(16) || flag)
			{
				this.SetScrollState(8, true);
				this.SetDisplayRectLocation(this.displayRect.X, -num);
				this.SyncScrollbars(this.AutoScroll);
			}
			this.WmOnScroll(ref m, oldValue, num, ScrollOrientation.VerticalScroll);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x000F0FC8 File Offset: 0x000EF1C8
		private void WmHScroll(ref Message m)
		{
			if (m.LParam != IntPtr.Zero)
			{
				base.WndProc(ref m);
				return;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			int num = -this.displayRect.X;
			int oldValue = num;
			int num2 = -(clientRectangle.Width - this.displayRect.Width);
			if (!this.AutoScroll)
			{
				num2 = this.HorizontalScroll.Maximum;
			}
			switch (NativeMethods.Util.LOWORD(m.WParam))
			{
			case 0:
				if (num > this.HorizontalScroll.SmallChange)
				{
					num -= this.HorizontalScroll.SmallChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 1:
				if (num < num2 - this.HorizontalScroll.SmallChange)
				{
					num += this.HorizontalScroll.SmallChange;
				}
				else
				{
					num = num2;
				}
				break;
			case 2:
				if (num > this.HorizontalScroll.LargeChange)
				{
					num -= this.HorizontalScroll.LargeChange;
				}
				else
				{
					num = 0;
				}
				break;
			case 3:
				if (num < num2 - this.HorizontalScroll.LargeChange)
				{
					num += this.HorizontalScroll.LargeChange;
				}
				else
				{
					num = num2;
				}
				break;
			case 4:
			case 5:
				num = this.ScrollThumbPosition(0);
				break;
			case 6:
				num = 0;
				break;
			case 7:
				num = num2;
				break;
			}
			if (this.GetScrollState(16) || NativeMethods.Util.LOWORD(m.WParam) != 5)
			{
				this.SetScrollState(8, true);
				this.SetDisplayRectLocation(-num, this.displayRect.Y);
				this.SyncScrollbars(this.AutoScroll);
			}
			this.WmOnScroll(ref m, oldValue, num, ScrollOrientation.HorizontalScroll);
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x000F1150 File Offset: 0x000EF350
		private void WmOnScroll(ref Message m, int oldValue, int value, ScrollOrientation scrollOrientation)
		{
			ScrollEventType scrollEventType = (ScrollEventType)NativeMethods.Util.LOWORD(m.WParam);
			if (scrollEventType != ScrollEventType.EndScroll)
			{
				ScrollEventArgs se = new ScrollEventArgs(scrollEventType, oldValue, value, scrollOrientation);
				this.OnScroll(se);
			}
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x000F117F File Offset: 0x000EF37F
		private void WmSettingChange(ref Message m)
		{
			base.WndProc(ref m);
			this.UpdateFullDrag();
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06003480 RID: 13440 RVA: 0x000F1190 File Offset: 0x000EF390
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 26)
			{
				this.WmSettingChange(ref m);
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

		// Token: 0x0400204E RID: 8270
		internal static readonly TraceSwitch AutoScrolling;

		/// <summary>Determines the value of the <see cref="P:System.Windows.Forms.ScrollableControl.AutoScroll" /> property.</summary>
		// Token: 0x0400204F RID: 8271
		protected const int ScrollStateAutoScrolling = 1;

		/// <summary>Determines whether the value of the <see cref="P:System.Windows.Forms.ScrollableControl.HScroll" /> property is set to <see langword="true" />.</summary>
		// Token: 0x04002050 RID: 8272
		protected const int ScrollStateHScrollVisible = 2;

		/// <summary>Determines whether the value of the <see cref="P:System.Windows.Forms.ScrollableControl.VScroll" /> property is set to <see langword="true" />.</summary>
		// Token: 0x04002051 RID: 8273
		protected const int ScrollStateVScrollVisible = 4;

		/// <summary>Determines whether the user had scrolled through the <see cref="T:System.Windows.Forms.ScrollableControl" /> control.</summary>
		// Token: 0x04002052 RID: 8274
		protected const int ScrollStateUserHasScrolled = 8;

		/// <summary>Determines whether the user has enabled full window drag.</summary>
		// Token: 0x04002053 RID: 8275
		protected const int ScrollStateFullDrag = 16;

		// Token: 0x04002054 RID: 8276
		private Size userAutoScrollMinSize = Size.Empty;

		// Token: 0x04002055 RID: 8277
		private Rectangle displayRect = Rectangle.Empty;

		// Token: 0x04002056 RID: 8278
		private Size scrollMargin = Size.Empty;

		// Token: 0x04002057 RID: 8279
		private Size requestedScrollMargin = Size.Empty;

		// Token: 0x04002058 RID: 8280
		internal Point scrollPosition = Point.Empty;

		// Token: 0x04002059 RID: 8281
		private ScrollableControl.DockPaddingEdges dockPadding;

		// Token: 0x0400205A RID: 8282
		private int scrollState;

		// Token: 0x0400205B RID: 8283
		private VScrollProperties verticalScroll;

		// Token: 0x0400205C RID: 8284
		private HScrollProperties horizontalScroll;

		// Token: 0x0400205D RID: 8285
		private static readonly object EVENT_SCROLL = new object();

		// Token: 0x0400205E RID: 8286
		private bool resetRTLHScrollValue;

		/// <summary>Determines the border padding for docked controls.</summary>
		// Token: 0x02000713 RID: 1811
		[TypeConverter(typeof(ScrollableControl.DockPaddingEdgesConverter))]
		public class DockPaddingEdges : ICloneable
		{
			// Token: 0x06005FFE RID: 24574 RVA: 0x00189C4E File Offset: 0x00187E4E
			internal DockPaddingEdges(ScrollableControl owner)
			{
				this.owner = owner;
			}

			// Token: 0x06005FFF RID: 24575 RVA: 0x00189C5D File Offset: 0x00187E5D
			internal DockPaddingEdges(int left, int right, int top, int bottom)
			{
				this.left = left;
				this.right = right;
				this.top = top;
				this.bottom = bottom;
			}

			/// <summary>Gets or sets the padding width for all edges of a docked control.</summary>
			/// <returns>The padding width, in pixels.</returns>
			// Token: 0x170016F4 RID: 5876
			// (get) Token: 0x06006000 RID: 24576 RVA: 0x00189C84 File Offset: 0x00187E84
			// (set) Token: 0x06006001 RID: 24577 RVA: 0x00189D4F File Offset: 0x00187F4F
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingAllDescr")]
			public int All
			{
				get
				{
					if (this.owner == null)
					{
						if (this.left == this.right && this.top == this.bottom && this.left == this.top)
						{
							return this.left;
						}
						return 0;
					}
					else
					{
						if (this.owner.Padding.All == -1 && (this.owner.Padding.Left != -1 || this.owner.Padding.Top != -1 || this.owner.Padding.Right != -1 || this.owner.Padding.Bottom != -1))
						{
							return 0;
						}
						return this.owner.Padding.All;
					}
				}
				set
				{
					if (this.owner == null)
					{
						this.left = value;
						this.top = value;
						this.right = value;
						this.bottom = value;
						return;
					}
					this.owner.Padding = new Padding(value);
				}
			}

			/// <summary>Gets or sets the padding width for the bottom edge of a docked control.</summary>
			/// <returns>The padding width, in pixels.</returns>
			// Token: 0x170016F5 RID: 5877
			// (get) Token: 0x06006002 RID: 24578 RVA: 0x00189D88 File Offset: 0x00187F88
			// (set) Token: 0x06006003 RID: 24579 RVA: 0x00189DB8 File Offset: 0x00187FB8
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingBottomDescr")]
			public int Bottom
			{
				get
				{
					if (this.owner == null)
					{
						return this.bottom;
					}
					return this.owner.Padding.Bottom;
				}
				set
				{
					if (this.owner == null)
					{
						this.bottom = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Bottom = value;
					this.owner.Padding = padding;
				}
			}

			/// <summary>Gets or sets the padding width for the left edge of a docked control.</summary>
			/// <returns>The padding width, in pixels.</returns>
			// Token: 0x170016F6 RID: 5878
			// (get) Token: 0x06006004 RID: 24580 RVA: 0x00189DF8 File Offset: 0x00187FF8
			// (set) Token: 0x06006005 RID: 24581 RVA: 0x00189E28 File Offset: 0x00188028
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingLeftDescr")]
			public int Left
			{
				get
				{
					if (this.owner == null)
					{
						return this.left;
					}
					return this.owner.Padding.Left;
				}
				set
				{
					if (this.owner == null)
					{
						this.left = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Left = value;
					this.owner.Padding = padding;
				}
			}

			/// <summary>Gets or sets the padding width for the right edge of a docked control.</summary>
			/// <returns>The padding width, in pixels.</returns>
			// Token: 0x170016F7 RID: 5879
			// (get) Token: 0x06006006 RID: 24582 RVA: 0x00189E68 File Offset: 0x00188068
			// (set) Token: 0x06006007 RID: 24583 RVA: 0x00189E98 File Offset: 0x00188098
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingRightDescr")]
			public int Right
			{
				get
				{
					if (this.owner == null)
					{
						return this.right;
					}
					return this.owner.Padding.Right;
				}
				set
				{
					if (this.owner == null)
					{
						this.right = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Right = value;
					this.owner.Padding = padding;
				}
			}

			/// <summary>Gets or sets the padding width for the top edge of a docked control.</summary>
			/// <returns>The padding width, in pixels.</returns>
			// Token: 0x170016F8 RID: 5880
			// (get) Token: 0x06006008 RID: 24584 RVA: 0x00189ED8 File Offset: 0x001880D8
			// (set) Token: 0x06006009 RID: 24585 RVA: 0x00189F08 File Offset: 0x00188108
			[RefreshProperties(RefreshProperties.All)]
			[SRDescription("PaddingTopDescr")]
			public int Top
			{
				get
				{
					if (this.owner == null)
					{
						return this.bottom;
					}
					return this.owner.Padding.Top;
				}
				set
				{
					if (this.owner == null)
					{
						this.top = value;
						return;
					}
					Padding padding = this.owner.Padding;
					padding.Top = value;
					this.owner.Padding = padding;
				}
			}

			/// <summary>Determines whether the specified object is equal to the current <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> object.</summary>
			/// <param name="other">The object to compare with the current <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> object.</param>
			// Token: 0x0600600A RID: 24586 RVA: 0x00189F48 File Offset: 0x00188148
			public override bool Equals(object other)
			{
				ScrollableControl.DockPaddingEdges dockPaddingEdges = other as ScrollableControl.DockPaddingEdges;
				return dockPaddingEdges != null && this.owner.Padding.Equals(dockPaddingEdges.owner.Padding);
			}

			/// <summary>Serves as a hash function for a particular type.</summary>
			/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
			// Token: 0x0600600B RID: 24587 RVA: 0x001572D5 File Offset: 0x001554D5
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x0600600C RID: 24588 RVA: 0x00189F8A File Offset: 0x0018818A
			private void ResetAll()
			{
				this.All = 0;
			}

			// Token: 0x0600600D RID: 24589 RVA: 0x00189F93 File Offset: 0x00188193
			private void ResetBottom()
			{
				this.Bottom = 0;
			}

			// Token: 0x0600600E RID: 24590 RVA: 0x00189F9C File Offset: 0x0018819C
			private void ResetLeft()
			{
				this.Left = 0;
			}

			// Token: 0x0600600F RID: 24591 RVA: 0x00189FA5 File Offset: 0x001881A5
			private void ResetRight()
			{
				this.Right = 0;
			}

			// Token: 0x06006010 RID: 24592 RVA: 0x00189FAE File Offset: 0x001881AE
			private void ResetTop()
			{
				this.Top = 0;
			}

			// Token: 0x06006011 RID: 24593 RVA: 0x00189FB8 File Offset: 0x001881B8
			internal void Scale(float dx, float dy)
			{
				this.owner.Padding.Scale(dx, dy);
			}

			/// <summary>Returns an empty string.</summary>
			/// <returns>An empty string.</returns>
			// Token: 0x06006012 RID: 24594 RVA: 0x000E9114 File Offset: 0x000E7314
			public override string ToString()
			{
				return "";
			}

			/// <summary>Creates a new object that is a copy of the current instance.</summary>
			/// <returns>A new object that is a copy of the current instance.</returns>
			// Token: 0x06006013 RID: 24595 RVA: 0x00189FDC File Offset: 0x001881DC
			object ICloneable.Clone()
			{
				return new ScrollableControl.DockPaddingEdges(this.Left, this.Right, this.Top, this.Bottom);
			}

			// Token: 0x04004135 RID: 16693
			private ScrollableControl owner;

			// Token: 0x04004136 RID: 16694
			private int left;

			// Token: 0x04004137 RID: 16695
			private int right;

			// Token: 0x04004138 RID: 16696
			private int top;

			// Token: 0x04004139 RID: 16697
			private int bottom;
		}

		/// <summary>A <see cref="T:System.ComponentModel.TypeConverter" /> for the <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges" /> class.</summary>
		// Token: 0x02000714 RID: 1812
		public class DockPaddingEdgesConverter : TypeConverter
		{
			/// <summary>Returns a collection of properties for the type of array specified by the value parameter, using the specified context and attributes.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <param name="value">An object that specifies the type of array for which to get properties.</param>
			/// <param name="attributes">An array of type attribute that is used as a filter.</param>
			/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that are exposed for the <see cref="T:System.Windows.Forms.ScrollableControl" />.</returns>
			// Token: 0x06006014 RID: 24596 RVA: 0x0018A008 File Offset: 0x00188208
			public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ScrollableControl.DockPaddingEdges), attributes);
				return properties.Sort(new string[]
				{
					"All",
					"Left",
					"Top",
					"Right",
					"Bottom"
				});
			}

			/// <summary>Returns whether the current object supports properties, using the specified context.</summary>
			/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
			/// <returns>
			///     <see langword="true" /> in all cases.</returns>
			// Token: 0x06006015 RID: 24597 RVA: 0x0000E214 File Offset: 0x0000C414
			public override bool GetPropertiesSupported(ITypeDescriptorContext context)
			{
				return true;
			}
		}
	}
}

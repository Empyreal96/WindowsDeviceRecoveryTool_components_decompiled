using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies a component that creates an icon in the notification area. This class cannot be inherited.</summary>
	// Token: 0x020002FC RID: 764
	[DefaultProperty("Text")]
	[DefaultEvent("MouseDoubleClick")]
	[Designer("System.Windows.Forms.Design.NotifyIconDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItemFilter("System.Windows.Forms")]
	[SRDescription("DescriptionNotifyIcon")]
	public sealed class NotifyIcon : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NotifyIcon" /> class.</summary>
		// Token: 0x06002E15 RID: 11797 RVA: 0x000D700C File Offset: 0x000D520C
		public NotifyIcon()
		{
			this.id = ++NotifyIcon.nextId;
			this.window = new NotifyIcon.NotifyIconNativeWindow(this);
			this.UpdateIcon(this.visible);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NotifyIcon" /> class with the specified container.</summary>
		/// <param name="container">An <see cref="T:System.ComponentModel.IContainer" /> that represents the container for the <see cref="T:System.Windows.Forms.NotifyIcon" /> control. </param>
		// Token: 0x06002E16 RID: 11798 RVA: 0x000D7076 File Offset: 0x000D5276
		public NotifyIcon(IContainer container) : this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}
			container.Add(this);
		}

		/// <summary>Gets or sets the text to display on the balloon tip associated with the <see cref="T:System.Windows.Forms.NotifyIcon" />.</summary>
		/// <returns>The text to display on the balloon tip associated with the <see cref="T:System.Windows.Forms.NotifyIcon" />.</returns>
		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002E17 RID: 11799 RVA: 0x000D7093 File Offset: 0x000D5293
		// (set) Token: 0x06002E18 RID: 11800 RVA: 0x000D709B File Offset: 0x000D529B
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("NotifyIconBalloonTipTextDescr")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string BalloonTipText
		{
			get
			{
				return this.balloonTipText;
			}
			set
			{
				if (value != this.balloonTipText)
				{
					this.balloonTipText = value;
				}
			}
		}

		/// <summary>Gets or sets the icon to display on the balloon tip associated with the <see cref="T:System.Windows.Forms.NotifyIcon" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolTipIcon" /> to display on the balloon tip associated with the <see cref="T:System.Windows.Forms.NotifyIcon" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value is not a <see cref="T:System.Windows.Forms.ToolTipIcon" />.</exception>
		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06002E19 RID: 11801 RVA: 0x000D70B2 File Offset: 0x000D52B2
		// (set) Token: 0x06002E1A RID: 11802 RVA: 0x000D70BA File Offset: 0x000D52BA
		[SRCategory("CatAppearance")]
		[DefaultValue(ToolTipIcon.None)]
		[SRDescription("NotifyIconBalloonTipIconDescr")]
		public ToolTipIcon BalloonTipIcon
		{
			get
			{
				return this.balloonTipIcon;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolTipIcon));
				}
				if (value != this.balloonTipIcon)
				{
					this.balloonTipIcon = value;
				}
			}
		}

		/// <summary>Gets or sets the title of the balloon tip displayed on the <see cref="T:System.Windows.Forms.NotifyIcon" />.</summary>
		/// <returns>The text to display as the title of the balloon tip.</returns>
		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x000D70F2 File Offset: 0x000D52F2
		// (set) Token: 0x06002E1C RID: 11804 RVA: 0x000D70FA File Offset: 0x000D52FA
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("NotifyIconBalloonTipTitleDescr")]
		public string BalloonTipTitle
		{
			get
			{
				return this.balloonTipTitle;
			}
			set
			{
				if (value != this.balloonTipTitle)
				{
					this.balloonTipTitle = value;
				}
			}
		}

		/// <summary>Occurs when the balloon tip is clicked.</summary>
		// Token: 0x14000222 RID: 546
		// (add) Token: 0x06002E1D RID: 11805 RVA: 0x000D7111 File Offset: 0x000D5311
		// (remove) Token: 0x06002E1E RID: 11806 RVA: 0x000D7124 File Offset: 0x000D5324
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconOnBalloonTipClickedDescr")]
		public event EventHandler BalloonTipClicked
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPCLICKED, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPCLICKED, value);
			}
		}

		/// <summary>Occurs when the balloon tip is closed by the user.</summary>
		// Token: 0x14000223 RID: 547
		// (add) Token: 0x06002E1F RID: 11807 RVA: 0x000D7137 File Offset: 0x000D5337
		// (remove) Token: 0x06002E20 RID: 11808 RVA: 0x000D714A File Offset: 0x000D534A
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconOnBalloonTipClosedDescr")]
		public event EventHandler BalloonTipClosed
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPCLOSED, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPCLOSED, value);
			}
		}

		/// <summary>Occurs when the balloon tip is displayed on the screen.</summary>
		// Token: 0x14000224 RID: 548
		// (add) Token: 0x06002E21 RID: 11809 RVA: 0x000D715D File Offset: 0x000D535D
		// (remove) Token: 0x06002E22 RID: 11810 RVA: 0x000D7170 File Offset: 0x000D5370
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconOnBalloonTipShownDescr")]
		public event EventHandler BalloonTipShown
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_BALLOONTIPSHOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_BALLOONTIPSHOWN, value);
			}
		}

		/// <summary>Gets or sets the shortcut menu for the icon.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenu" /> for the icon. The default value is <see langword="null" />.</returns>
		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06002E23 RID: 11811 RVA: 0x000D7183 File Offset: 0x000D5383
		// (set) Token: 0x06002E24 RID: 11812 RVA: 0x000D718B File Offset: 0x000D538B
		[Browsable(false)]
		[DefaultValue(null)]
		[SRCategory("CatBehavior")]
		[SRDescription("NotifyIconMenuDescr")]
		public ContextMenu ContextMenu
		{
			get
			{
				return this.contextMenu;
			}
			set
			{
				this.contextMenu = value;
			}
		}

		/// <summary>Gets or sets the shortcut menu associated with the <see cref="T:System.Windows.Forms.NotifyIcon" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip" /> associated with the <see cref="T:System.Windows.Forms.NotifyIcon" /></returns>
		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x000D7194 File Offset: 0x000D5394
		// (set) Token: 0x06002E26 RID: 11814 RVA: 0x000D719C File Offset: 0x000D539C
		[DefaultValue(null)]
		[SRCategory("CatBehavior")]
		[SRDescription("NotifyIconMenuDescr")]
		public ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		/// <summary>Gets or sets the current icon.</summary>
		/// <returns>The <see cref="T:System.Drawing.Icon" /> displayed by the <see cref="T:System.Windows.Forms.NotifyIcon" /> component. The default value is <see langword="null" />.</returns>
		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002E27 RID: 11815 RVA: 0x000D71A5 File Offset: 0x000D53A5
		// (set) Token: 0x06002E28 RID: 11816 RVA: 0x000D71AD File Offset: 0x000D53AD
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue(null)]
		[SRDescription("NotifyIconIconDescr")]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (this.icon != value)
				{
					this.icon = value;
					this.UpdateIcon(this.visible);
				}
			}
		}

		/// <summary>Gets or sets the ToolTip text displayed when the mouse pointer rests on a notification area icon.</summary>
		/// <returns>The ToolTip text displayed when the mouse pointer rests on a notification area icon.</returns>
		/// <exception cref="T:System.ArgumentException">ToolTip text is more than 63 characters long.</exception>
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06002E29 RID: 11817 RVA: 0x000D71CB File Offset: 0x000D53CB
		// (set) Token: 0x06002E2A RID: 11818 RVA: 0x000D71D4 File Offset: 0x000D53D4
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("NotifyIconTextDescr")]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value != null && !value.Equals(this.text))
				{
					if (value != null && value.Length > 63)
					{
						throw new ArgumentOutOfRangeException("Text", value, SR.GetString("TrayIcon_TextTooLong"));
					}
					this.text = value;
					if (this.added)
					{
						this.UpdateIcon(true);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the icon is visible in the notification area of the taskbar.</summary>
		/// <returns>
		///     <see langword="true" /> if the icon is visible in the notification area; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06002E2B RID: 11819 RVA: 0x000D7235 File Offset: 0x000D5435
		// (set) Token: 0x06002E2C RID: 11820 RVA: 0x000D723D File Offset: 0x000D543D
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("NotifyIconVisDescr")]
		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				if (this.visible != value)
				{
					this.UpdateIcon(value);
					this.visible = value;
				}
			}
		}

		/// <summary>Gets or sets an object that contains data about the <see cref="T:System.Windows.Forms.NotifyIcon" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.NotifyIcon" />.</returns>
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x000D7256 File Offset: 0x000D5456
		// (set) Token: 0x06002E2E RID: 11822 RVA: 0x000D725E File Offset: 0x000D545E
		[SRCategory("CatData")]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		/// <summary>Occurs when the user clicks the icon in the notification area.</summary>
		// Token: 0x14000225 RID: 549
		// (add) Token: 0x06002E2F RID: 11823 RVA: 0x000D7267 File Offset: 0x000D5467
		// (remove) Token: 0x06002E30 RID: 11824 RVA: 0x000D727A File Offset: 0x000D547A
		[SRCategory("CatAction")]
		[SRDescription("ControlOnClickDescr")]
		public event EventHandler Click
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_CLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_CLICK, value);
			}
		}

		/// <summary>Occurs when the user double-clicks the icon in the notification area of the taskbar.</summary>
		// Token: 0x14000226 RID: 550
		// (add) Token: 0x06002E31 RID: 11825 RVA: 0x000D728D File Offset: 0x000D548D
		// (remove) Token: 0x06002E32 RID: 11826 RVA: 0x000D72A0 File Offset: 0x000D54A0
		[SRCategory("CatAction")]
		[SRDescription("ControlOnDoubleClickDescr")]
		public event EventHandler DoubleClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_DOUBLECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_DOUBLECLICK, value);
			}
		}

		/// <summary>Occurs when the user clicks a <see cref="T:System.Windows.Forms.NotifyIcon" /> with the mouse.</summary>
		// Token: 0x14000227 RID: 551
		// (add) Token: 0x06002E33 RID: 11827 RVA: 0x000D72B3 File Offset: 0x000D54B3
		// (remove) Token: 0x06002E34 RID: 11828 RVA: 0x000D72C6 File Offset: 0x000D54C6
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconMouseClickDescr")]
		public event MouseEventHandler MouseClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSECLICK, value);
			}
		}

		/// <summary>Occurs when the user double-clicks the <see cref="T:System.Windows.Forms.NotifyIcon" /> with the mouse.</summary>
		// Token: 0x14000228 RID: 552
		// (add) Token: 0x06002E35 RID: 11829 RVA: 0x000D72D9 File Offset: 0x000D54D9
		// (remove) Token: 0x06002E36 RID: 11830 RVA: 0x000D72EC File Offset: 0x000D54EC
		[SRCategory("CatAction")]
		[SRDescription("NotifyIconMouseDoubleClickDescr")]
		public event MouseEventHandler MouseDoubleClick
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEDOUBLECLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEDOUBLECLICK, value);
			}
		}

		/// <summary>Occurs when the user presses the mouse button while the pointer is over the icon in the notification area of the taskbar.</summary>
		// Token: 0x14000229 RID: 553
		// (add) Token: 0x06002E37 RID: 11831 RVA: 0x000D72FF File Offset: 0x000D54FF
		// (remove) Token: 0x06002E38 RID: 11832 RVA: 0x000D7312 File Offset: 0x000D5512
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseDownDescr")]
		public event MouseEventHandler MouseDown
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEDOWN, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEDOWN, value);
			}
		}

		/// <summary>Occurs when the user moves the mouse while the pointer is over the icon in the notification area of the taskbar.</summary>
		// Token: 0x1400022A RID: 554
		// (add) Token: 0x06002E39 RID: 11833 RVA: 0x000D7325 File Offset: 0x000D5525
		// (remove) Token: 0x06002E3A RID: 11834 RVA: 0x000D7338 File Offset: 0x000D5538
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseMoveDescr")]
		public event MouseEventHandler MouseMove
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEMOVE, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEMOVE, value);
			}
		}

		/// <summary>Occurs when the user releases the mouse button while the pointer is over the icon in the notification area of the taskbar.</summary>
		// Token: 0x1400022B RID: 555
		// (add) Token: 0x06002E3B RID: 11835 RVA: 0x000D734B File Offset: 0x000D554B
		// (remove) Token: 0x06002E3C RID: 11836 RVA: 0x000D735E File Offset: 0x000D555E
		[SRCategory("CatMouse")]
		[SRDescription("ControlOnMouseUpDescr")]
		public event MouseEventHandler MouseUp
		{
			add
			{
				base.Events.AddHandler(NotifyIcon.EVENT_MOUSEUP, value);
			}
			remove
			{
				base.Events.RemoveHandler(NotifyIcon.EVENT_MOUSEUP, value);
			}
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000D7374 File Offset: 0x000D5574
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.window != null)
				{
					this.icon = null;
					this.Text = string.Empty;
					this.UpdateIcon(false);
					this.window.DestroyHandle();
					this.window = null;
					this.contextMenu = null;
					this.contextMenuStrip = null;
				}
			}
			else if (this.window != null && this.window.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.PostMessage(new HandleRef(this.window, this.window.Handle), 16, 0, 0);
				this.window.ReleaseHandle();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000D741C File Offset: 0x000D561C
		private void OnBalloonTipClicked()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPCLICKED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000D7450 File Offset: 0x000D5650
		private void OnBalloonTipClosed()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPCLOSED];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000D7484 File Offset: 0x000D5684
		private void OnBalloonTipShown()
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_BALLOONTIPSHOWN];
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000D74B8 File Offset: 0x000D56B8
		private void OnClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_CLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000D74E8 File Offset: 0x000D56E8
		private void OnDoubleClick(EventArgs e)
		{
			EventHandler eventHandler = (EventHandler)base.Events[NotifyIcon.EVENT_DOUBLECLICK];
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000D7518 File Offset: 0x000D5718
		private void OnMouseClick(MouseEventArgs mea)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSECLICK];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, mea);
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000D7548 File Offset: 0x000D5748
		private void OnMouseDoubleClick(MouseEventArgs mea)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEDOUBLECLICK];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, mea);
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000D7578 File Offset: 0x000D5778
		private void OnMouseDown(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEDOWN];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000D75A8 File Offset: 0x000D57A8
		private void OnMouseMove(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEMOVE];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x000D75D8 File Offset: 0x000D57D8
		private void OnMouseUp(MouseEventArgs e)
		{
			MouseEventHandler mouseEventHandler = (MouseEventHandler)base.Events[NotifyIcon.EVENT_MOUSEUP];
			if (mouseEventHandler != null)
			{
				mouseEventHandler(this, e);
			}
		}

		/// <summary>Displays a balloon tip in the taskbar for the specified time period.</summary>
		/// <param name="timeout">The time period, in milliseconds, the balloon tip should display.This parameter is deprecated as of Windows Vista. Notification display times are now based on system accessibility settings.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="timeout" /> is less than 0.</exception>
		// Token: 0x06002E48 RID: 11848 RVA: 0x000D7606 File Offset: 0x000D5806
		public void ShowBalloonTip(int timeout)
		{
			this.ShowBalloonTip(timeout, this.balloonTipTitle, this.balloonTipText, this.balloonTipIcon);
		}

		/// <summary>Displays a balloon tip with the specified title, text, and icon in the taskbar for the specified time period.</summary>
		/// <param name="timeout">The time period, in milliseconds, the balloon tip should display.This parameter is deprecated as of Windows Vista. Notification display times are now based on system accessibility settings.</param>
		/// <param name="tipTitle">The title to display on the balloon tip.</param>
		/// <param name="tipText">The text to display on the balloon tip.</param>
		/// <param name="tipIcon">One of the <see cref="T:System.Windows.Forms.ToolTipIcon" /> values.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="timeout" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="tipText" /> is <see langword="null" /> or an empty string.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="tipIcon" /> is not a member of <see cref="T:System.Windows.Forms.ToolTipIcon" />.</exception>
		// Token: 0x06002E49 RID: 11849 RVA: 0x000D7624 File Offset: 0x000D5824
		public void ShowBalloonTip(int timeout, string tipTitle, string tipText, ToolTipIcon tipIcon)
		{
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout", SR.GetString("InvalidArgument", new object[]
				{
					"timeout",
					timeout.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (string.IsNullOrEmpty(tipText))
			{
				throw new ArgumentException(SR.GetString("NotifyIconEmptyOrNullTipText"));
			}
			if (!ClientUtils.IsEnumValid(tipIcon, (int)tipIcon, 0, 3))
			{
				throw new InvalidEnumArgumentException("tipIcon", (int)tipIcon, typeof(ToolTipIcon));
			}
			if (this.added)
			{
				if (base.DesignMode)
				{
					return;
				}
				IntSecurity.UnrestrictedWindows.Demand();
				NativeMethods.NOTIFYICONDATA notifyicondata = new NativeMethods.NOTIFYICONDATA();
				if (this.window.Handle == IntPtr.Zero)
				{
					this.window.CreateHandle(new CreateParams());
				}
				notifyicondata.hWnd = this.window.Handle;
				notifyicondata.uID = this.id;
				notifyicondata.uFlags = 16;
				notifyicondata.uTimeoutOrVersion = timeout;
				notifyicondata.szInfoTitle = tipTitle;
				notifyicondata.szInfo = tipText;
				switch (tipIcon)
				{
				case ToolTipIcon.None:
					notifyicondata.dwInfoFlags = 0;
					break;
				case ToolTipIcon.Info:
					notifyicondata.dwInfoFlags = 1;
					break;
				case ToolTipIcon.Warning:
					notifyicondata.dwInfoFlags = 2;
					break;
				case ToolTipIcon.Error:
					notifyicondata.dwInfoFlags = 3;
					break;
				}
				UnsafeNativeMethods.Shell_NotifyIcon(1, notifyicondata);
			}
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x000D7770 File Offset: 0x000D5970
		private void ShowContextMenu()
		{
			if (this.contextMenu != null || this.contextMenuStrip != null)
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				UnsafeNativeMethods.SetForegroundWindow(new HandleRef(this.window, this.window.Handle));
				if (this.contextMenu != null)
				{
					this.contextMenu.OnPopup(EventArgs.Empty);
					SafeNativeMethods.TrackPopupMenuEx(new HandleRef(this.contextMenu, this.contextMenu.Handle), 72, point.x, point.y, new HandleRef(this.window, this.window.Handle), null);
					UnsafeNativeMethods.PostMessage(new HandleRef(this.window, this.window.Handle), 0, IntPtr.Zero, IntPtr.Zero);
					return;
				}
				if (this.contextMenuStrip != null)
				{
					this.contextMenuStrip.ShowInTaskbar(point.x, point.y);
				}
			}
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x000D785C File Offset: 0x000D5A5C
		private void UpdateIcon(bool showIconInTray)
		{
			object obj = this.syncObj;
			lock (obj)
			{
				if (!base.DesignMode)
				{
					IntSecurity.UnrestrictedWindows.Demand();
					this.window.LockReference(showIconInTray);
					NativeMethods.NOTIFYICONDATA notifyicondata = new NativeMethods.NOTIFYICONDATA();
					notifyicondata.uCallbackMessage = 2048;
					notifyicondata.uFlags = 1;
					if (showIconInTray && this.window.Handle == IntPtr.Zero)
					{
						this.window.CreateHandle(new CreateParams());
					}
					notifyicondata.hWnd = this.window.Handle;
					notifyicondata.uID = this.id;
					notifyicondata.hIcon = IntPtr.Zero;
					notifyicondata.szTip = null;
					if (this.icon != null)
					{
						notifyicondata.uFlags |= 2;
						notifyicondata.hIcon = this.icon.Handle;
					}
					notifyicondata.uFlags |= 4;
					notifyicondata.szTip = this.text;
					if (showIconInTray && this.icon != null)
					{
						if (!this.added)
						{
							UnsafeNativeMethods.Shell_NotifyIcon(0, notifyicondata);
							this.added = true;
						}
						else
						{
							UnsafeNativeMethods.Shell_NotifyIcon(1, notifyicondata);
						}
					}
					else if (this.added)
					{
						UnsafeNativeMethods.Shell_NotifyIcon(2, notifyicondata);
						this.added = false;
					}
				}
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x000D79BC File Offset: 0x000D5BBC
		private void WmMouseDown(ref Message m, MouseButtons button, int clicks)
		{
			if (clicks == 2)
			{
				this.OnDoubleClick(new MouseEventArgs(button, 2, 0, 0, 0));
				this.OnMouseDoubleClick(new MouseEventArgs(button, 2, 0, 0, 0));
				this.doubleClick = true;
			}
			this.OnMouseDown(new MouseEventArgs(button, clicks, 0, 0, 0));
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x000D79F9 File Offset: 0x000D5BF9
		private void WmMouseMove(ref Message m)
		{
			this.OnMouseMove(new MouseEventArgs(Control.MouseButtons, 0, 0, 0, 0));
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x000D7A10 File Offset: 0x000D5C10
		private void WmMouseUp(ref Message m, MouseButtons button)
		{
			this.OnMouseUp(new MouseEventArgs(button, 0, 0, 0, 0));
			if (!this.doubleClick)
			{
				this.OnClick(new MouseEventArgs(button, 0, 0, 0, 0));
				this.OnMouseClick(new MouseEventArgs(button, 0, 0, 0, 0));
			}
			this.doubleClick = false;
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000D7A5C File Offset: 0x000D5C5C
		private void WmTaskbarCreated(ref Message m)
		{
			this.added = false;
			this.UpdateIcon(this.visible);
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000D7A74 File Offset: 0x000D5C74
		private void WndProc(ref Message msg)
		{
			int num = msg.Msg;
			if (num <= 44)
			{
				if (num == 2)
				{
					this.UpdateIcon(false);
					return;
				}
				if (num != 43)
				{
					if (num == 44)
					{
						if (msg.WParam == IntPtr.Zero)
						{
							this.WmMeasureMenuItem(ref msg);
							return;
						}
						return;
					}
				}
				else
				{
					if (msg.WParam == IntPtr.Zero)
					{
						this.WmDrawItemMenuItem(ref msg);
						return;
					}
					return;
				}
			}
			else if (num != 273)
			{
				if (num == 279)
				{
					this.WmInitMenuPopup(ref msg);
					return;
				}
				if (num == 2048)
				{
					num = (int)msg.LParam;
					switch (num)
					{
					case 512:
						this.WmMouseMove(ref msg);
						return;
					case 513:
						this.WmMouseDown(ref msg, MouseButtons.Left, 1);
						return;
					case 514:
						this.WmMouseUp(ref msg, MouseButtons.Left);
						return;
					case 515:
						this.WmMouseDown(ref msg, MouseButtons.Left, 2);
						return;
					case 516:
						this.WmMouseDown(ref msg, MouseButtons.Right, 1);
						return;
					case 517:
						if (this.contextMenu != null || this.contextMenuStrip != null)
						{
							this.ShowContextMenu();
						}
						this.WmMouseUp(ref msg, MouseButtons.Right);
						return;
					case 518:
						this.WmMouseDown(ref msg, MouseButtons.Right, 2);
						return;
					case 519:
						this.WmMouseDown(ref msg, MouseButtons.Middle, 1);
						return;
					case 520:
						this.WmMouseUp(ref msg, MouseButtons.Middle);
						return;
					case 521:
						this.WmMouseDown(ref msg, MouseButtons.Middle, 2);
						return;
					default:
						switch (num)
						{
						case 1026:
							this.OnBalloonTipShown();
							return;
						case 1027:
							this.OnBalloonTipClosed();
							return;
						case 1028:
							this.OnBalloonTipClosed();
							return;
						case 1029:
							this.OnBalloonTipClicked();
							return;
						default:
							return;
						}
						break;
					}
				}
			}
			else
			{
				if (IntPtr.Zero == msg.LParam)
				{
					Command.DispatchID((int)msg.WParam & 65535);
					return;
				}
				this.window.DefWndProc(ref msg);
				return;
			}
			if (msg.Msg == NotifyIcon.WM_TASKBARCREATED)
			{
				this.WmTaskbarCreated(ref msg);
			}
			this.window.DefWndProc(ref msg);
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000D7C77 File Offset: 0x000D5E77
		private void WmInitMenuPopup(ref Message m)
		{
			if (this.contextMenu != null && this.contextMenu.ProcessInitMenuPopup(m.WParam))
			{
				return;
			}
			this.window.DefWndProc(ref m);
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000D7CA4 File Offset: 0x000D5EA4
		private void WmMeasureMenuItem(ref Message m)
		{
			NativeMethods.MEASUREITEMSTRUCT measureitemstruct = (NativeMethods.MEASUREITEMSTRUCT)m.GetLParam(typeof(NativeMethods.MEASUREITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(measureitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmMeasureItem(ref m);
			}
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x000D7CE0 File Offset: 0x000D5EE0
		private void WmDrawItemMenuItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			MenuItem menuItemFromItemData = MenuItem.GetMenuItemFromItemData(drawitemstruct.itemData);
			if (menuItemFromItemData != null)
			{
				menuItemFromItemData.WmDrawItem(ref m);
			}
		}

		// Token: 0x04001D0A RID: 7434
		private static readonly object EVENT_MOUSEDOWN = new object();

		// Token: 0x04001D0B RID: 7435
		private static readonly object EVENT_MOUSEMOVE = new object();

		// Token: 0x04001D0C RID: 7436
		private static readonly object EVENT_MOUSEUP = new object();

		// Token: 0x04001D0D RID: 7437
		private static readonly object EVENT_CLICK = new object();

		// Token: 0x04001D0E RID: 7438
		private static readonly object EVENT_DOUBLECLICK = new object();

		// Token: 0x04001D0F RID: 7439
		private static readonly object EVENT_MOUSECLICK = new object();

		// Token: 0x04001D10 RID: 7440
		private static readonly object EVENT_MOUSEDOUBLECLICK = new object();

		// Token: 0x04001D11 RID: 7441
		private static readonly object EVENT_BALLOONTIPSHOWN = new object();

		// Token: 0x04001D12 RID: 7442
		private static readonly object EVENT_BALLOONTIPCLICKED = new object();

		// Token: 0x04001D13 RID: 7443
		private static readonly object EVENT_BALLOONTIPCLOSED = new object();

		// Token: 0x04001D14 RID: 7444
		private const int WM_TRAYMOUSEMESSAGE = 2048;

		// Token: 0x04001D15 RID: 7445
		private static int WM_TASKBARCREATED = SafeNativeMethods.RegisterWindowMessage("TaskbarCreated");

		// Token: 0x04001D16 RID: 7446
		private object syncObj = new object();

		// Token: 0x04001D17 RID: 7447
		private Icon icon;

		// Token: 0x04001D18 RID: 7448
		private string text = "";

		// Token: 0x04001D19 RID: 7449
		private int id;

		// Token: 0x04001D1A RID: 7450
		private bool added;

		// Token: 0x04001D1B RID: 7451
		private NotifyIcon.NotifyIconNativeWindow window;

		// Token: 0x04001D1C RID: 7452
		private ContextMenu contextMenu;

		// Token: 0x04001D1D RID: 7453
		private ContextMenuStrip contextMenuStrip;

		// Token: 0x04001D1E RID: 7454
		private ToolTipIcon balloonTipIcon;

		// Token: 0x04001D1F RID: 7455
		private string balloonTipText = "";

		// Token: 0x04001D20 RID: 7456
		private string balloonTipTitle = "";

		// Token: 0x04001D21 RID: 7457
		private static int nextId = 0;

		// Token: 0x04001D22 RID: 7458
		private object userData;

		// Token: 0x04001D23 RID: 7459
		private bool doubleClick;

		// Token: 0x04001D24 RID: 7460
		private bool visible;

		// Token: 0x020006FD RID: 1789
		private class NotifyIconNativeWindow : NativeWindow
		{
			// Token: 0x06005FA0 RID: 24480 RVA: 0x00189122 File Offset: 0x00187322
			internal NotifyIconNativeWindow(NotifyIcon component)
			{
				this.reference = component;
			}

			// Token: 0x06005FA1 RID: 24481 RVA: 0x00189134 File Offset: 0x00187334
			~NotifyIconNativeWindow()
			{
				if (base.Handle != IntPtr.Zero)
				{
					UnsafeNativeMethods.PostMessage(new HandleRef(this, base.Handle), 16, 0, 0);
				}
			}

			// Token: 0x06005FA2 RID: 24482 RVA: 0x00189184 File Offset: 0x00187384
			public void LockReference(bool locked)
			{
				if (locked)
				{
					if (!this.rootRef.IsAllocated)
					{
						this.rootRef = GCHandle.Alloc(this.reference, GCHandleType.Normal);
						return;
					}
				}
				else if (this.rootRef.IsAllocated)
				{
					this.rootRef.Free();
				}
			}

			// Token: 0x06005FA3 RID: 24483 RVA: 0x000337A1 File Offset: 0x000319A1
			protected override void OnThreadException(Exception e)
			{
				Application.OnThreadException(e);
			}

			// Token: 0x06005FA4 RID: 24484 RVA: 0x001891C1 File Offset: 0x001873C1
			protected override void WndProc(ref Message m)
			{
				this.reference.WndProc(ref m);
			}

			// Token: 0x0400403E RID: 16446
			internal NotifyIcon reference;

			// Token: 0x0400403F RID: 16447
			private GCHandle rootRef;
		}
	}
}

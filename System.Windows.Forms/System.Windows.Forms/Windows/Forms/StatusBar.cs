using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows status bar control. Although <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> replaces and adds functionality to the <see cref="T:System.Windows.Forms.StatusBar" /> control of previous versions, <see cref="T:System.Windows.Forms.StatusBar" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x02000363 RID: 867
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("PanelClick")]
	[DefaultProperty("Text")]
	[Designer("System.Windows.Forms.Design.StatusBarDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class StatusBar : Control
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusBar" /> class.</summary>
		// Token: 0x0600366F RID: 13935 RVA: 0x000F76A4 File Offset: 0x000F58A4
		public StatusBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Selectable, false);
			this.Dock = DockStyle.Bottom;
			this.TabStop = false;
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x000F76F0 File Offset: 0x000F58F0
		private static VisualStyleRenderer VisualStyleRenderer
		{
			get
			{
				if (VisualStyleRenderer.IsSupported)
				{
					if (StatusBar.renderer == null)
					{
						StatusBar.renderer = new VisualStyleRenderer(VisualStyleElement.ToolBar.Button.Normal);
					}
				}
				else
				{
					StatusBar.renderer = null;
				}
				return StatusBar.renderer;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x000F771C File Offset: 0x000F591C
		private int SizeGripWidth
		{
			get
			{
				if (this.sizeGripWidth == 0)
				{
					if (Application.RenderWithVisualStyles && StatusBar.VisualStyleRenderer != null)
					{
						VisualStyleRenderer visualStyleRenderer = StatusBar.VisualStyleRenderer;
						VisualStyleElement normal = VisualStyleElement.Status.GripperPane.Normal;
						visualStyleRenderer.SetParameters(normal);
						this.sizeGripWidth = visualStyleRenderer.GetPartSize(Graphics.FromHwndInternal(base.Handle), ThemeSizeType.True).Width;
						normal = VisualStyleElement.Status.Gripper.Normal;
						visualStyleRenderer.SetParameters(normal);
						Size partSize = visualStyleRenderer.GetPartSize(Graphics.FromHwndInternal(base.Handle), ThemeSizeType.True);
						this.sizeGripWidth += partSize.Width;
						this.sizeGripWidth = Math.Max(this.sizeGripWidth, 16);
					}
					else
					{
						this.sizeGripWidth = 16;
					}
				}
				return this.sizeGripWidth;
			}
		}

		/// <summary>Gets or sets the background color for the <see cref="T:System.Windows.Forms.StatusBar" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that is the background color of the control</returns>
		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x0002849B File Offset: 0x0002669B
		// (set) Token: 0x06003673 RID: 13939 RVA: 0x0000701A File Offset: 0x0000521A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return SystemColors.Control;
			}
			set
			{
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.StatusBar.BackColor" /> property changes.</summary>
		// Token: 0x140002AB RID: 683
		// (add) Token: 0x06003674 RID: 13940 RVA: 0x00050A7A File Offset: 0x0004EC7A
		// (remove) Token: 0x06003675 RID: 13941 RVA: 0x00050A83 File Offset: 0x0004EC83
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the background image for the <see cref="T:System.Windows.Forms.StatusBar" />.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> that is the background image for the <see cref="T:System.Windows.Forms.StatusBar" />.</returns>
		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06003677 RID: 13943 RVA: 0x00011FCA File Offset: 0x000101CA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.StatusBar.BackgroundImage" /> property is changed.</summary>
		// Token: 0x140002AC RID: 684
		// (add) Token: 0x06003678 RID: 13944 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x06003679 RID: 13945 RVA: 0x0001FD8A File Offset: 0x0001DF8A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>Gets or sets the layout of the background image of the <see cref="T:System.Windows.Forms.StatusBar" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600367B RID: 13947 RVA: 0x00011FDB File Offset: 0x000101DB
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.StatusBar.BackgroundImageLayout" /> property changes.</summary>
		// Token: 0x140002AD RID: 685
		// (add) Token: 0x0600367C RID: 13948 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x0600367D RID: 13949 RVA: 0x0001FD9C File Offset: 0x0001DF9C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.CreateParams" /> used to create the handle for this control.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.CreateParams" /> used to create the handle for this control.</returns>
		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x0600367E RID: 13950 RVA: 0x000F77D0 File Offset: 0x000F59D0
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "msctls_statusbar32";
				if (this.sizeGrip)
				{
					createParams.Style |= 256;
				}
				else
				{
					createParams.Style &= -257;
				}
				createParams.Style |= 12;
				return createParams;
			}
		}

		/// <summary>Gets the default Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Size" /> that represents the size of the control.</returns>
		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003680 RID: 13952 RVA: 0x000F782D File Offset: 0x000F5A2D
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 22);
			}
		}

		/// <summary>Gets or sets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker, however this property has no effect on the <see cref="T:System.Windows.Forms.StatusBar" /> control</summary>
		/// <returns>
		///     <see langword="true" /> if the control has a secondary buffer; otherwise, <see langword="false" />. </returns>
		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003681 RID: 13953 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06003682 RID: 13954 RVA: 0x000A2CBA File Offset: 0x000A0EBA
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}

		/// <summary>Gets or sets the docking behavior of the <see cref="T:System.Windows.Forms.StatusBar" /> control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle" /> values. The default is <see langword="Bottom" />.</returns>
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x000F3D46 File Offset: 0x000F1F46
		// (set) Token: 0x06003684 RID: 13956 RVA: 0x000F7576 File Offset: 0x000F5776
		[Localizable(true)]
		[DefaultValue(DockStyle.Bottom)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		/// <summary>Gets or sets the font the <see cref="T:System.Windows.Forms.StatusBar" /> control will use to display information.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> of the text. The default is the font of the container, unless you override it.</returns>
		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003685 RID: 13957 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06003686 RID: 13958 RVA: 0x000F7838 File Offset: 0x000F5A38
		[Localizable(true)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
				this.SetPanelContentsWidths(false);
			}
		}

		/// <summary>Gets or sets the forecolor for the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the forecolor of the control. The default is <see langword="Empty" />.</returns>
		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003687 RID: 13959 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06003688 RID: 13960 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.StatusBar.ForeColor" /> property changes.</summary>
		// Token: 0x140002AE RID: 686
		// (add) Token: 0x06003689 RID: 13961 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x0600368A RID: 13962 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x0600368B RID: 13963 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x0600368C RID: 13964 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.StatusBar.ImeMode" /> property changes.</summary>
		// Token: 0x140002AF RID: 687
		// (add) Token: 0x0600368D RID: 13965 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x0600368E RID: 13966 RVA: 0x0001BF35 File Offset: 0x0001A135
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Forms.StatusBar" /> panels contained within the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> containing the <see cref="T:System.Windows.Forms.StatusBarPanel" /> objects of the <see cref="T:System.Windows.Forms.StatusBar" /> control.</returns>
		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x000F7848 File Offset: 0x000F5A48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[SRDescription("StatusBarPanelsDescr")]
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[MergableProperty(false)]
		public StatusBar.StatusBarPanelCollection Panels
		{
			get
			{
				if (this.panelsCollection == null)
				{
					this.panelsCollection = new StatusBar.StatusBarPanelCollection(this);
				}
				return this.panelsCollection;
			}
		}

		/// <summary>Gets or sets the text associated with the <see cref="T:System.Windows.Forms.StatusBar" /> control.</summary>
		/// <returns>The text associated with the <see cref="T:System.Windows.Forms.StatusBar" /> control.</returns>
		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x000F7864 File Offset: 0x000F5A64
		// (set) Token: 0x06003691 RID: 13969 RVA: 0x000F787A File Offset: 0x000F5A7A
		[Localizable(true)]
		public override string Text
		{
			get
			{
				if (this.simpleText == null)
				{
					return "";
				}
				return this.simpleText;
			}
			set
			{
				this.SetSimpleText(value);
				if (this.simpleText != value)
				{
					this.simpleText = value;
					this.OnTextChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether any panels that have been added to the control are displayed.</summary>
		/// <returns>
		///     <see langword="true" /> if panels are displayed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003692 RID: 13970 RVA: 0x000F78A3 File Offset: 0x000F5AA3
		// (set) Token: 0x06003693 RID: 13971 RVA: 0x000F78AC File Offset: 0x000F5AAC
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("StatusBarShowPanelsDescr")]
		public bool ShowPanels
		{
			get
			{
				return this.showPanels;
			}
			set
			{
				if (this.showPanels != value)
				{
					this.showPanels = value;
					this.layoutDirty = true;
					if (base.IsHandleCreated)
					{
						int wparam = (!this.showPanels) ? 1 : 0;
						base.SendMessage(1033, wparam, 0);
						if (this.showPanels)
						{
							base.PerformLayout();
							this.RealizePanels();
						}
						else if (this.tooltips != null)
						{
							for (int i = 0; i < this.panels.Count; i++)
							{
								this.tooltips.SetTool(this.panels[i], null);
							}
						}
						this.SetSimpleText(this.simpleText);
					}
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a sizing grip is displayed in the lower-right corner of the control.</summary>
		/// <returns>
		///     <see langword="true" /> if a sizing grip is displayed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003694 RID: 13972 RVA: 0x000F794E File Offset: 0x000F5B4E
		// (set) Token: 0x06003695 RID: 13973 RVA: 0x000F7956 File Offset: 0x000F5B56
		[SRCategory("CatAppearance")]
		[DefaultValue(true)]
		[SRDescription("StatusBarSizingGripDescr")]
		public bool SizingGrip
		{
			get
			{
				return this.sizeGrip;
			}
			set
			{
				if (value != this.sizeGrip)
				{
					this.sizeGrip = value;
					base.RecreateHandle();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the user will be able to tab to the <see cref="T:System.Windows.Forms.StatusBar" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the tab key moves focus to the <see cref="T:System.Windows.Forms.StatusBar" />; otherwise <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06003696 RID: 13974 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06003697 RID: 13975 RVA: 0x000AA11D File Offset: 0x000A831D
		[DefaultValue(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06003698 RID: 13976 RVA: 0x000F796E File Offset: 0x000F5B6E
		internal bool ToolTipSet
		{
			get
			{
				return this.toolTipSet;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06003699 RID: 13977 RVA: 0x000F7976 File Offset: 0x000F5B76
		internal ToolTip MainToolTip
		{
			get
			{
				return this.mainToolTip;
			}
		}

		/// <summary>Occurs when a visual aspect of an owner-drawn status bar control changes.</summary>
		// Token: 0x140002B0 RID: 688
		// (add) Token: 0x0600369A RID: 13978 RVA: 0x000F797E File Offset: 0x000F5B7E
		// (remove) Token: 0x0600369B RID: 13979 RVA: 0x000F7991 File Offset: 0x000F5B91
		[SRCategory("CatBehavior")]
		[SRDescription("StatusBarDrawItem")]
		public event StatusBarDrawItemEventHandler DrawItem
		{
			add
			{
				base.Events.AddHandler(StatusBar.EVENT_SBDRAWITEM, value);
			}
			remove
			{
				base.Events.RemoveHandler(StatusBar.EVENT_SBDRAWITEM, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Forms.StatusBarPanel" /> object on a <see cref="T:System.Windows.Forms.StatusBar" /> control is clicked.</summary>
		// Token: 0x140002B1 RID: 689
		// (add) Token: 0x0600369C RID: 13980 RVA: 0x000F79A4 File Offset: 0x000F5BA4
		// (remove) Token: 0x0600369D RID: 13981 RVA: 0x000F79B7 File Offset: 0x000F5BB7
		[SRCategory("CatMouse")]
		[SRDescription("StatusBarOnPanelClickDescr")]
		public event StatusBarPanelClickEventHandler PanelClick
		{
			add
			{
				base.Events.AddHandler(StatusBar.EVENT_PANELCLICK, value);
			}
			remove
			{
				base.Events.RemoveHandler(StatusBar.EVENT_PANELCLICK, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Forms.StatusBar" /> control is redrawn.</summary>
		// Token: 0x140002B2 RID: 690
		// (add) Token: 0x0600369E RID: 13982 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x0600369F RID: 13983 RVA: 0x00020D40 File Offset: 0x0001EF40
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event PaintEventHandler Paint
		{
			add
			{
				base.Paint += value;
			}
			remove
			{
				base.Paint -= value;
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x000F79CA File Offset: 0x000F5BCA
		internal bool ArePanelsRealized()
		{
			return this.showPanels && base.IsHandleCreated;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000F79DC File Offset: 0x000F5BDC
		internal void DirtyLayout()
		{
			this.layoutDirty = true;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x000F79E8 File Offset: 0x000F5BE8
		private void ApplyPanelWidths()
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			int count = this.panels.Count;
			if (count == 0)
			{
				int[] array = new int[]
				{
					base.Size.Width
				};
				if (this.sizeGrip)
				{
					array[0] -= this.SizeGripWidth;
				}
				UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1028, 1, array);
				base.SendMessage(1039, 0, IntPtr.Zero);
				return;
			}
			int[] array2 = new int[count];
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				num += statusBarPanel.Width;
				array2[i] = num;
				statusBarPanel.Right = array2[i];
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1028, count, array2);
			for (int j = 0; j < count; j++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[j];
				this.UpdateTooltip(statusBarPanel);
			}
			this.layoutDirty = false;
		}

		/// <summary>Overrides <see cref="M:System.Windows.Forms.Control.CreateHandle" />.</summary>
		// Token: 0x060036A3 RID: 13987 RVA: 0x000F7B00 File Offset: 0x000F5D00
		protected override void CreateHandle()
		{
			if (!base.RecreatingHandle)
			{
				IntPtr userCookie = UnsafeNativeMethods.ThemingScope.Activate();
				try
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 4
					});
				}
				finally
				{
					UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
				}
			}
			base.CreateHandle();
		}

		/// <summary>Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.StatusBar" />.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060036A4 RID: 13988 RVA: 0x000F7B50 File Offset: 0x000F5D50
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.panelsCollection != null)
			{
				StatusBarPanel[] array = new StatusBarPanel[this.panelsCollection.Count];
				((ICollection)this.panelsCollection).CopyTo(array, 0);
				this.panelsCollection.Clear();
				foreach (StatusBarPanel statusBarPanel in array)
				{
					statusBarPanel.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x000F7BB2 File Offset: 0x000F5DB2
		private void ForcePanelUpdate()
		{
			if (this.ArePanelsRealized())
			{
				this.layoutDirty = true;
				this.SetPanelContentsWidths(true);
				base.PerformLayout();
				this.RealizePanels();
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060036A6 RID: 13990 RVA: 0x000F7BD8 File Offset: 0x000F5DD8
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (!base.DesignMode)
			{
				this.tooltips = new StatusBar.ControlToolTip(this);
			}
			if (!this.showPanels)
			{
				base.SendMessage(1033, 1, 0);
				this.SetSimpleText(this.simpleText);
				return;
			}
			this.ForcePanelUpdate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060036A7 RID: 13991 RVA: 0x000F7C29 File Offset: 0x000F5E29
		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if (this.tooltips != null)
			{
				this.tooltips.Dispose();
				this.tooltips = null;
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.StatusBar.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x060036A8 RID: 13992 RVA: 0x000F7C4C File Offset: 0x000F5E4C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.lastClick.X = e.X;
			this.lastClick.Y = e.Y;
			base.OnMouseDown(e);
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.StatusBar.OnPanelClick(System.Windows.Forms.StatusBarPanelClickEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.StatusBarPanelClickEventArgs" /> that contains the event data. </param>
		// Token: 0x060036A9 RID: 13993 RVA: 0x000F7C78 File Offset: 0x000F5E78
		protected virtual void OnPanelClick(StatusBarPanelClickEventArgs e)
		{
			StatusBarPanelClickEventHandler statusBarPanelClickEventHandler = (StatusBarPanelClickEventHandler)base.Events[StatusBar.EVENT_PANELCLICK];
			if (statusBarPanelClickEventHandler != null)
			{
				statusBarPanelClickEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see langword="Layout" /> event.</summary>
		/// <param name="levent">A <see langword="LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x060036AA RID: 13994 RVA: 0x000F7CA6 File Offset: 0x000F5EA6
		protected override void OnLayout(LayoutEventArgs levent)
		{
			if (this.showPanels)
			{
				this.LayoutPanels();
				if (base.IsHandleCreated && this.panelsRealized != this.panels.Count)
				{
					this.RealizePanels();
				}
			}
			base.OnLayout(levent);
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000F7CE0 File Offset: 0x000F5EE0
		internal void RealizePanels()
		{
			int count = this.panels.Count;
			int num = this.panelsRealized;
			this.panelsRealized = 0;
			if (count == 0)
			{
				base.SendMessage(NativeMethods.SB_SETTEXT, 0, "");
			}
			int i;
			for (i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				try
				{
					statusBarPanel.Realize();
					this.panelsRealized++;
				}
				catch
				{
				}
			}
			while (i < num)
			{
				base.SendMessage(NativeMethods.SB_SETTEXT, 0, null);
				i++;
			}
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000F7D80 File Offset: 0x000F5F80
		internal void RemoveAllPanelsWithoutUpdate()
		{
			int count = this.panels.Count;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				statusBarPanel.ParentInternal = null;
			}
			this.panels.Clear();
			if (this.showPanels)
			{
				this.ApplyPanelWidths();
				this.ForcePanelUpdate();
			}
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x000F7DE0 File Offset: 0x000F5FE0
		internal void SetPanelContentsWidths(bool newPanels)
		{
			int count = this.panels.Count;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				if (statusBarPanel.AutoSize == StatusBarPanelAutoSize.Contents)
				{
					int contentsWidth = statusBarPanel.GetContentsWidth(newPanels);
					if (statusBarPanel.Width != contentsWidth)
					{
						statusBarPanel.Width = contentsWidth;
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.DirtyLayout();
				base.PerformLayout();
			}
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x000F7E50 File Offset: 0x000F6050
		private void SetSimpleText(string simpleText)
		{
			if (!this.showPanels && base.IsHandleCreated)
			{
				int num = 511;
				if (this.RightToLeft == RightToLeft.Yes)
				{
					num |= 1024;
				}
				base.SendMessage(NativeMethods.SB_SETTEXT, num, simpleText);
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000F7E94 File Offset: 0x000F6094
		private void LayoutPanels()
		{
			int num = 0;
			int num2 = 0;
			StatusBarPanel[] array = new StatusBarPanel[this.panels.Count];
			bool flag = false;
			for (int i = 0; i < array.Length; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				if (statusBarPanel.AutoSize == StatusBarPanelAutoSize.Spring)
				{
					array[num2] = statusBarPanel;
					num2++;
				}
				else
				{
					num += statusBarPanel.Width;
				}
			}
			if (num2 > 0)
			{
				Rectangle bounds = base.Bounds;
				int j = num2;
				int num3 = bounds.Width - num;
				if (this.sizeGrip)
				{
					num3 -= this.SizeGripWidth;
				}
				int num4 = int.MinValue;
				while (j > 0)
				{
					int num5 = num3 / j;
					if (num3 == num4)
					{
						break;
					}
					num4 = num3;
					for (int k = 0; k < num2; k++)
					{
						StatusBarPanel statusBarPanel = array[k];
						if (statusBarPanel != null)
						{
							if (num5 < statusBarPanel.MinWidth)
							{
								if (statusBarPanel.Width != statusBarPanel.MinWidth)
								{
									flag = true;
								}
								statusBarPanel.Width = statusBarPanel.MinWidth;
								array[k] = null;
								j--;
								num3 -= statusBarPanel.MinWidth;
							}
							else
							{
								if (statusBarPanel.Width != num5)
								{
									flag = true;
								}
								statusBarPanel.Width = num5;
							}
						}
					}
				}
			}
			if (flag || this.layoutDirty)
			{
				this.ApplyPanelWidths();
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.StatusBar.OnDrawItem(System.Windows.Forms.StatusBarDrawItemEventArgs)" /> event.</summary>
		/// <param name="sbdievent">A <see cref="T:System.Windows.Forms.StatusBarDrawItemEventArgs" /> that contains the event data. </param>
		// Token: 0x060036B0 RID: 14000 RVA: 0x000F7FCC File Offset: 0x000F61CC
		protected virtual void OnDrawItem(StatusBarDrawItemEventArgs sbdievent)
		{
			StatusBarDrawItemEventHandler statusBarDrawItemEventHandler = (StatusBarDrawItemEventHandler)base.Events[StatusBar.EVENT_SBDRAWITEM];
			if (statusBarDrawItemEventHandler != null)
			{
				statusBarDrawItemEventHandler(this, sbdievent);
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.StatusBar.OnResize(System.EventArgs)" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060036B1 RID: 14001 RVA: 0x000F7FFA File Offset: 0x000F61FA
		protected override void OnResize(EventArgs e)
		{
			base.Invalidate();
			base.OnResize(e);
		}

		/// <summary>Returns a string representation for this control.</summary>
		/// <returns>String </returns>
		// Token: 0x060036B2 RID: 14002 RVA: 0x000F800C File Offset: 0x000F620C
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Panels != null)
			{
				text = text + ", Panels.Count: " + this.Panels.Count.ToString(CultureInfo.CurrentCulture);
				if (this.Panels.Count > 0)
				{
					text = text + ", Panels[0]: " + this.Panels[0].ToString();
				}
			}
			return text;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000F8078 File Offset: 0x000F6278
		internal void SetToolTip(ToolTip t)
		{
			this.mainToolTip = t;
			this.toolTipSet = true;
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x000F8088 File Offset: 0x000F6288
		internal void UpdateTooltip(StatusBarPanel panel)
		{
			if (this.tooltips == null)
			{
				if (!base.IsHandleCreated || base.DesignMode)
				{
					return;
				}
				this.tooltips = new StatusBar.ControlToolTip(this);
			}
			if (panel.Parent == this && panel.ToolTipText.Length > 0)
			{
				int width = SystemInformation.Border3DSize.Width;
				StatusBar.ControlToolTip.Tool tool = this.tooltips.GetTool(panel);
				if (tool == null)
				{
					tool = new StatusBar.ControlToolTip.Tool();
				}
				tool.text = panel.ToolTipText;
				tool.rect = new Rectangle(panel.Right - panel.Width + width, 0, panel.Width - width, base.Height);
				this.tooltips.SetTool(panel, tool);
				return;
			}
			this.tooltips.SetTool(panel, null);
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x000F8148 File Offset: 0x000F6348
		private void UpdatePanelIndex()
		{
			int count = this.panels.Count;
			for (int i = 0; i < count; i++)
			{
				((StatusBarPanel)this.panels[i]).Index = i;
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x000F8184 File Offset: 0x000F6384
		private void WmDrawItem(ref Message m)
		{
			NativeMethods.DRAWITEMSTRUCT drawitemstruct = (NativeMethods.DRAWITEMSTRUCT)m.GetLParam(typeof(NativeMethods.DRAWITEMSTRUCT));
			int count = this.panels.Count;
			if (drawitemstruct.itemID >= 0)
			{
				int itemID = drawitemstruct.itemID;
			}
			StatusBarPanel panel = (StatusBarPanel)this.panels[drawitemstruct.itemID];
			Graphics graphics = Graphics.FromHdcInternal(drawitemstruct.hDC);
			Rectangle r = Rectangle.FromLTRB(drawitemstruct.rcItem.left, drawitemstruct.rcItem.top, drawitemstruct.rcItem.right, drawitemstruct.rcItem.bottom);
			this.OnDrawItem(new StatusBarDrawItemEventArgs(graphics, this.Font, r, drawitemstruct.itemID, DrawItemState.None, panel, this.ForeColor, this.BackColor));
			graphics.Dispose();
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x000F824C File Offset: 0x000F644C
		private void WmNotifyNMClick(NativeMethods.NMHDR note)
		{
			if (!this.showPanels)
			{
				return;
			}
			int count = this.panels.Count;
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < count; i++)
			{
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.panels[i];
				num += statusBarPanel.Width;
				if (this.lastClick.X < num)
				{
					num2 = i;
					break;
				}
			}
			if (num2 != -1)
			{
				MouseButtons button = MouseButtons.Left;
				int clicks = 0;
				switch (note.code)
				{
				case -6:
					button = MouseButtons.Right;
					clicks = 2;
					break;
				case -5:
					button = MouseButtons.Right;
					clicks = 1;
					break;
				case -3:
					button = MouseButtons.Left;
					clicks = 2;
					break;
				case -2:
					button = MouseButtons.Left;
					clicks = 1;
					break;
				}
				Point point = this.lastClick;
				StatusBarPanel statusBarPanel2 = (StatusBarPanel)this.panels[num2];
				StatusBarPanelClickEventArgs e = new StatusBarPanelClickEventArgs(statusBarPanel2, button, clicks, point.X, point.Y);
				this.OnPanelClick(e);
			}
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x000F8354 File Offset: 0x000F6554
		private void WmNCHitTest(ref Message m)
		{
			int num = NativeMethods.Util.LOWORD(m.LParam);
			Rectangle bounds = base.Bounds;
			bool flag = true;
			if (num > bounds.X + bounds.Width - this.SizeGripWidth)
			{
				Control parentInternal = this.ParentInternal;
				if (parentInternal != null && parentInternal is Form)
				{
					FormBorderStyle formBorderStyle = ((Form)parentInternal).FormBorderStyle;
					if (formBorderStyle != FormBorderStyle.Sizable && formBorderStyle != FormBorderStyle.SizableToolWindow)
					{
						flag = false;
					}
					if (!((Form)parentInternal).TopLevel || this.Dock != DockStyle.Bottom)
					{
						flag = false;
					}
					if (flag)
					{
						Control.ControlCollection controls = parentInternal.Controls;
						int count = controls.Count;
						for (int i = 0; i < count; i++)
						{
							Control control = controls[i];
							if (control != this && control.Dock == DockStyle.Bottom && control.Top > base.Top)
							{
								flag = false;
								break;
							}
						}
					}
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				base.WndProc(ref m);
				return;
			}
			m.Result = (IntPtr)1;
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x060036B9 RID: 14009 RVA: 0x000F8448 File Offset: 0x000F6648
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int num = m.Msg;
			if (num <= 132)
			{
				if (num != 78)
				{
					if (num != 132)
					{
						goto IL_7B;
					}
					this.WmNCHitTest(ref m);
					return;
				}
			}
			else
			{
				if (num == 8235)
				{
					this.WmDrawItem(ref m);
					return;
				}
				if (num != 8270)
				{
					goto IL_7B;
				}
			}
			NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
			num = nmhdr.code;
			if (num - -6 <= 1 || num - -3 <= 1)
			{
				this.WmNotifyNMClick(nmhdr);
				return;
			}
			base.WndProc(ref m);
			return;
			IL_7B:
			base.WndProc(ref m);
		}

		// Token: 0x040021B4 RID: 8628
		private int sizeGripWidth;

		// Token: 0x040021B5 RID: 8629
		private const int SIMPLE_INDEX = 255;

		// Token: 0x040021B6 RID: 8630
		private static readonly object EVENT_PANELCLICK = new object();

		// Token: 0x040021B7 RID: 8631
		private static readonly object EVENT_SBDRAWITEM = new object();

		// Token: 0x040021B8 RID: 8632
		private bool showPanels;

		// Token: 0x040021B9 RID: 8633
		private bool layoutDirty;

		// Token: 0x040021BA RID: 8634
		private int panelsRealized;

		// Token: 0x040021BB RID: 8635
		private bool sizeGrip = true;

		// Token: 0x040021BC RID: 8636
		private string simpleText;

		// Token: 0x040021BD RID: 8637
		private Point lastClick = new Point(0, 0);

		// Token: 0x040021BE RID: 8638
		private IList panels = new ArrayList();

		// Token: 0x040021BF RID: 8639
		private StatusBar.StatusBarPanelCollection panelsCollection;

		// Token: 0x040021C0 RID: 8640
		private StatusBar.ControlToolTip tooltips;

		// Token: 0x040021C1 RID: 8641
		private ToolTip mainToolTip;

		// Token: 0x040021C2 RID: 8642
		private bool toolTipSet;

		// Token: 0x040021C3 RID: 8643
		private static VisualStyleRenderer renderer = null;

		/// <summary>Represents the collection of panels in a <see cref="T:System.Windows.Forms.StatusBar" /> control.</summary>
		// Token: 0x0200071E RID: 1822
		[ListBindable(false)]
		public class StatusBarPanelCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.StatusBar" /> control that contains this collection. </param>
			// Token: 0x06006026 RID: 24614 RVA: 0x0018A3C7 File Offset: 0x001885C7
			public StatusBarPanelCollection(StatusBar owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.StatusBarPanel" /> at the specified index.</summary>
			/// <param name="index">The index of the panel in the collection to get or set. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.StatusBarPanel" /> representing the panel located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.StatusBar.StatusBarPanelCollection.Count" /> property of the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> class. </exception>
			/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> assigned to the collection was <see langword="null" />. </exception>
			// Token: 0x170016F9 RID: 5881
			public virtual StatusBarPanel this[int index]
			{
				get
				{
					return (StatusBarPanel)this.owner.panels[index];
				}
				set
				{
					if (value == null)
					{
						throw new ArgumentNullException("StatusBarPanel");
					}
					this.owner.layoutDirty = true;
					if (value.Parent != null)
					{
						throw new ArgumentException(SR.GetString("ObjectHasParent"), "value");
					}
					int count = this.owner.panels.Count;
					if (index < 0 || index >= count)
					{
						throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					StatusBarPanel statusBarPanel = (StatusBarPanel)this.owner.panels[index];
					statusBarPanel.ParentInternal = null;
					value.ParentInternal = this.owner;
					if (value.AutoSize == StatusBarPanelAutoSize.Contents)
					{
						value.Width = value.GetContentsWidth(true);
					}
					this.owner.panels[index] = value;
					value.Index = index;
					if (this.owner.ArePanelsRealized())
					{
						this.owner.PerformLayout();
						value.Realize();
					}
				}
			}

			/// <summary>Gets or sets the element at the specified index.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The element at the specified index.</returns>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.StatusBarPanel" />.</exception>
			// Token: 0x170016FA RID: 5882
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is StatusBarPanel)
					{
						this[index] = (StatusBarPanel)value;
						return;
					}
					throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
				}
			}

			/// <summary>Gets an item with the specified key from the collection.</summary>
			/// <param name="key">The name of the item to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key.</returns>
			// Token: 0x170016FB RID: 5883
			public virtual StatusBarPanel this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Gets the number of items in the collection.</summary>
			/// <returns>The number of <see cref="T:System.Windows.Forms.StatusBarPanel" /> objects in the collection.</returns>
			// Token: 0x170016FC RID: 5884
			// (get) Token: 0x0600602C RID: 24620 RVA: 0x0018A565 File Offset: 0x00188765
			[Browsable(false)]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Count
			{
				get
				{
					return this.owner.panels.Count;
				}
			}

			/// <summary>Gets an object that can be used to synchronize access to the collection.</summary>
			/// <returns>The object used to synchronize access to the collection.</returns>
			// Token: 0x170016FD RID: 5885
			// (get) Token: 0x0600602D RID: 24621 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>Gets a value indicating whether access to the collection is synchronized (thread safe).</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x170016FE RID: 5886
			// (get) Token: 0x0600602E RID: 24622 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
			/// <returns>
			///     <see langword="false" /> in all cases.</returns>
			// Token: 0x170016FF RID: 5887
			// (get) Token: 0x0600602F RID: 24623 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether this collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if this collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001700 RID: 5888
			// (get) Token: 0x06006030 RID: 24624 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified text to the collection.</summary>
			/// <param name="text">The text for the <see cref="T:System.Windows.Forms.StatusBarPanel" /> that is being added. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel that was added to the collection.</returns>
			// Token: 0x06006031 RID: 24625 RVA: 0x0018A578 File Offset: 0x00188778
			public virtual StatusBarPanel Add(string text)
			{
				StatusBarPanel statusBarPanel = new StatusBarPanel();
				statusBarPanel.Text = text;
				this.Add(statusBarPanel);
				return statusBarPanel;
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.StatusBarPanel" /> to the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to add to the collection. </param>
			/// <returns>The zero-based index of the item in the collection.</returns>
			/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> being added to the collection was <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentException">The parent of the <see cref="T:System.Windows.Forms.StatusBarPanel" /> specified in the <paramref name="value" /> parameter is not <see langword="null" />. </exception>
			// Token: 0x06006032 RID: 24626 RVA: 0x0018A59C File Offset: 0x0018879C
			public virtual int Add(StatusBarPanel value)
			{
				int count = this.owner.panels.Count;
				this.Insert(count, value);
				return count;
			}

			/// <summary>Adds a <see cref="T:System.Windows.Forms.StatusBarPanel" /> to the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to add to the collection.</param>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.StatusBarPanel" />.-or-The parent of value is not <see langword="null" />.</exception>
			// Token: 0x06006033 RID: 24627 RVA: 0x0018A5C3 File Offset: 0x001887C3
			int IList.Add(object value)
			{
				if (value is StatusBarPanel)
				{
					return this.Add((StatusBarPanel)value);
				}
				throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
			}

			/// <summary>Adds an array of <see cref="T:System.Windows.Forms.StatusBarPanel" /> objects to the collection.</summary>
			/// <param name="panels">An array of <see cref="T:System.Windows.Forms.StatusBarPanel" /> objects to add. </param>
			/// <exception cref="T:System.ArgumentNullException">The array of <see cref="T:System.Windows.Forms.StatusBarPanel" /> objects being added to the collection was <see langword="null" />. </exception>
			// Token: 0x06006034 RID: 24628 RVA: 0x0018A5F0 File Offset: 0x001887F0
			public virtual void AddRange(StatusBarPanel[] panels)
			{
				if (panels == null)
				{
					throw new ArgumentNullException("panels");
				}
				foreach (StatusBarPanel value in panels)
				{
					this.Add(value);
				}
			}

			/// <summary>Determines whether the specified panel is located within the collection.</summary>
			/// <param name="panel">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to locate in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the panel is located within the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006035 RID: 24629 RVA: 0x0018A627 File Offset: 0x00188827
			public bool Contains(StatusBarPanel panel)
			{
				return this.IndexOf(panel) != -1;
			}

			/// <summary>Determines whether the specified panel is located within the collection.</summary>
			/// <param name="panel">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to locate in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if panel is a <see cref="T:System.Windows.Forms.StatusBarPanel" /> located within the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06006036 RID: 24630 RVA: 0x0018A636 File Offset: 0x00188836
			bool IList.Contains(object panel)
			{
				return panel is StatusBarPanel && this.Contains((StatusBarPanel)panel);
			}

			/// <summary>Determines whether the collection contains a <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key. </summary>
			/// <param name="key">The name of the item to find in the collection.</param>
			/// <returns>
			///     <see langword="true" /> to indicate the collection contains a <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key; otherwise, <see langword="false" />. </returns>
			// Token: 0x06006037 RID: 24631 RVA: 0x0018A64E File Offset: 0x0018884E
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>Returns the index within the collection of the specified panel.</summary>
			/// <param name="panel">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to locate in the collection. </param>
			/// <returns>The zero-based index where the panel is located within the collection; otherwise, negative one (-1).</returns>
			// Token: 0x06006038 RID: 24632 RVA: 0x0018A660 File Offset: 0x00188860
			public int IndexOf(StatusBarPanel panel)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this[i] == panel)
					{
						return i;
					}
				}
				return -1;
			}

			/// <summary>Returns the index of the specified panel within the collection.</summary>
			/// <param name="panel">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> to locate in the collection.</param>
			/// <returns>The zero-based index of panel, if found, within the entire collection; otherwise, -1.</returns>
			// Token: 0x06006039 RID: 24633 RVA: 0x0018A68B File Offset: 0x0018888B
			int IList.IndexOf(object panel)
			{
				if (panel is StatusBarPanel)
				{
					return this.IndexOf((StatusBarPanel)panel);
				}
				return -1;
			}

			/// <summary>Returns the index of the first occurrence of a <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.StatusBarPanel" /> to find in the collection.</param>
			/// <returns>The zero-based index of the first occurrence of the <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key, if found; otherwise, -1.</returns>
			// Token: 0x0600603A RID: 24634 RVA: 0x0018A6A4 File Offset: 0x001888A4
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			/// <summary>Inserts the specified <see cref="T:System.Windows.Forms.StatusBarPanel" /> into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the panel is inserted. </param>
			/// <param name="value">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> representing the panel to insert. </param>
			/// <exception cref="T:System.ArgumentNullException">The <paramref name="value" /> parameter is <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentException">The <paramref name="value" /> parameter's parent is not <see langword="null" />. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than the value of the <see cref="P:System.Windows.Forms.StatusBar.StatusBarPanelCollection.Count" /> property of the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> class. </exception>
			/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <see cref="P:System.Windows.Forms.StatusBarPanel.AutoSize" /> property of the <paramref name="value" /> parameter's panel is not a valid <see cref="T:System.Windows.Forms.StatusBarPanelAutoSize" /> value. </exception>
			// Token: 0x0600603B RID: 24635 RVA: 0x0018A724 File Offset: 0x00188924
			public virtual void Insert(int index, StatusBarPanel value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.owner.layoutDirty = true;
				if (value.Parent != this.owner && value.Parent != null)
				{
					throw new ArgumentException(SR.GetString("ObjectHasParent"), "value");
				}
				int count = this.owner.panels.Count;
				if (index < 0 || index > count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				value.ParentInternal = this.owner;
				StatusBarPanelAutoSize autoSize = value.AutoSize;
				if (autoSize - StatusBarPanelAutoSize.None > 1 && autoSize == StatusBarPanelAutoSize.Contents)
				{
					value.Width = value.GetContentsWidth(true);
				}
				this.owner.panels.Insert(index, value);
				this.owner.UpdatePanelIndex();
				this.owner.ForcePanelUpdate();
			}

			/// <summary>Inserts the specified <see cref="T:System.Windows.Forms.StatusBarPanel" /> into the collection at the specified index.</summary>
			/// <param name="index">The zero-based index location where the panel is inserted. </param>
			/// <param name="value">A <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to insert.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The index parameter is less than zero or greater than the value of the <see langword="Count" /> property.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="value" /> is not a <see cref="T:System.Windows.Forms.StatusBarPanel" />.-or-The parent of value is not <see langword="null" />.</exception>
			// Token: 0x0600603C RID: 24636 RVA: 0x0018A813 File Offset: 0x00188A13
			void IList.Insert(int index, object value)
			{
				if (value is StatusBarPanel)
				{
					this.Insert(index, (StatusBarPanel)value);
					return;
				}
				throw new ArgumentException(SR.GetString("StatusBarBadStatusBarPanel"), "value");
			}

			// Token: 0x0600603D RID: 24637 RVA: 0x0018A83F File Offset: 0x00188A3F
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Removes all items from the collection.</summary>
			// Token: 0x0600603E RID: 24638 RVA: 0x0018A850 File Offset: 0x00188A50
			public virtual void Clear()
			{
				this.owner.RemoveAllPanelsWithoutUpdate();
				this.owner.PerformLayout();
			}

			/// <summary>Removes the specified <see cref="T:System.Windows.Forms.StatusBarPanel" /> from the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> representing the panel to remove from the collection. </param>
			/// <exception cref="T:System.ArgumentNullException">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> assigned to the <paramref name="value" /> parameter is <see langword="null" />. </exception>
			// Token: 0x0600603F RID: 24639 RVA: 0x0018A868 File Offset: 0x00188A68
			public virtual void Remove(StatusBarPanel value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("StatusBarPanel");
				}
				if (value.Parent != this.owner)
				{
					return;
				}
				this.RemoveAt(value.Index);
			}

			/// <summary>Removes the specified <see cref="T:System.Windows.Forms.StatusBarPanel" /> from the collection.</summary>
			/// <param name="value">The <see cref="T:System.Windows.Forms.StatusBarPanel" /> that represents the panel to remove from the collection.</param>
			// Token: 0x06006040 RID: 24640 RVA: 0x0018A893 File Offset: 0x00188A93
			void IList.Remove(object value)
			{
				if (value is StatusBarPanel)
				{
					this.Remove((StatusBarPanel)value);
				}
			}

			/// <summary>Removes the <see cref="T:System.Windows.Forms.StatusBarPanel" /> located at the specified index within the collection.</summary>
			/// <param name="index">The zero-based index of the item to remove. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than zero or greater than or equal to the value of the <see cref="P:System.Windows.Forms.StatusBar.StatusBarPanelCollection.Count" /> property of the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> class. </exception>
			// Token: 0x06006041 RID: 24641 RVA: 0x0018A8AC File Offset: 0x00188AAC
			public virtual void RemoveAt(int index)
			{
				int count = this.Count;
				if (index < 0 || index >= count)
				{
					throw new ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				StatusBarPanel statusBarPanel = (StatusBarPanel)this.owner.panels[index];
				this.owner.panels.RemoveAt(index);
				statusBarPanel.ParentInternal = null;
				this.owner.UpdateTooltip(statusBarPanel);
				this.owner.UpdatePanelIndex();
				this.owner.ForcePanelUpdate();
			}

			/// <summary>Removes the <see cref="T:System.Windows.Forms.StatusBarPanel" /> with the specified key from the collection.</summary>
			/// <param name="key">The name of the <see cref="T:System.Windows.Forms.StatusBarPanel" /> to remove from the collection.</param>
			// Token: 0x06006042 RID: 24642 RVA: 0x0018A94C File Offset: 0x00188B4C
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>Copies the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
			/// <param name="dest">The one-dimensional array that is the destination of the elements copied from the collection. The array must have zero-based indexing.  </param>
			/// <param name="index">The zero-based index in the array at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">
			///         <paramref name="array" /> is <see langword="null" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///         <paramref name="index" /> is less than zero.</exception>
			/// <exception cref="T:System.ArgumentException">
			///         <paramref name="array" /> is multidimensional.-or-The number of elements in the <see cref="T:System.Windows.Forms.StatusBar.StatusBarPanelCollection" /> is greater than the available space from index to the end of <paramref name="array" />.</exception>
			/// <exception cref="T:System.InvalidCastException">The type in the collection cannot be cast automatically to the type of <paramref name="array" />.</exception>
			// Token: 0x06006043 RID: 24643 RVA: 0x0018A971 File Offset: 0x00188B71
			void ICollection.CopyTo(Array dest, int index)
			{
				this.owner.panels.CopyTo(dest, index);
			}

			/// <summary>Returns an enumerator to use to iterate through the item collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the item collection.</returns>
			// Token: 0x06006044 RID: 24644 RVA: 0x0018A985 File Offset: 0x00188B85
			public IEnumerator GetEnumerator()
			{
				if (this.owner.panels != null)
				{
					return this.owner.panels.GetEnumerator();
				}
				return new StatusBarPanel[0].GetEnumerator();
			}

			// Token: 0x0400414B RID: 16715
			private StatusBar owner;

			// Token: 0x0400414C RID: 16716
			private int lastAccessedIndex = -1;
		}

		// Token: 0x0200071F RID: 1823
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private class ControlToolTip
		{
			// Token: 0x06006045 RID: 24645 RVA: 0x0018A9B0 File Offset: 0x00188BB0
			public ControlToolTip(Control parent)
			{
				this.window = new StatusBar.ControlToolTip.ToolTipNativeWindow(this);
				this.parent = parent;
			}

			// Token: 0x17001701 RID: 5889
			// (get) Token: 0x06006046 RID: 24646 RVA: 0x0018A9D8 File Offset: 0x00188BD8
			protected CreateParams CreateParams
			{
				get
				{
					SafeNativeMethods.InitCommonControlsEx(new NativeMethods.INITCOMMONCONTROLSEX
					{
						dwICC = 8
					});
					CreateParams createParams = new CreateParams();
					createParams.Parent = IntPtr.Zero;
					createParams.ClassName = "tooltips_class32";
					createParams.Style |= 1;
					createParams.ExStyle = 0;
					createParams.Caption = null;
					return createParams;
				}
			}

			// Token: 0x17001702 RID: 5890
			// (get) Token: 0x06006047 RID: 24647 RVA: 0x0018AA32 File Offset: 0x00188C32
			public IntPtr Handle
			{
				get
				{
					if (this.window.Handle == IntPtr.Zero)
					{
						this.CreateHandle();
					}
					return this.window.Handle;
				}
			}

			// Token: 0x17001703 RID: 5891
			// (get) Token: 0x06006048 RID: 24648 RVA: 0x0018AA5C File Offset: 0x00188C5C
			private bool IsHandleCreated
			{
				get
				{
					return this.window.Handle != IntPtr.Zero;
				}
			}

			// Token: 0x06006049 RID: 24649 RVA: 0x0018AA73 File Offset: 0x00188C73
			private void AssignId(StatusBar.ControlToolTip.Tool tool)
			{
				tool.id = (IntPtr)this.nextId;
				this.nextId++;
			}

			// Token: 0x0600604A RID: 24650 RVA: 0x0018AA94 File Offset: 0x00188C94
			public void SetTool(object key, StatusBar.ControlToolTip.Tool tool)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				StatusBar.ControlToolTip.Tool tool2 = null;
				if (this.tools.ContainsKey(key))
				{
					tool2 = (StatusBar.ControlToolTip.Tool)this.tools[key];
				}
				if (tool2 != null)
				{
					flag = true;
				}
				if (tool != null)
				{
					flag2 = true;
				}
				if (tool != null && tool2 != null && tool.id == tool2.id)
				{
					flag3 = true;
				}
				if (flag3)
				{
					this.UpdateTool(tool);
				}
				else
				{
					if (flag)
					{
						this.RemoveTool(tool2);
					}
					if (flag2)
					{
						this.AddTool(tool);
					}
				}
				if (tool != null)
				{
					this.tools[key] = tool;
					return;
				}
				this.tools.Remove(key);
			}

			// Token: 0x0600604B RID: 24651 RVA: 0x0018AB2B File Offset: 0x00188D2B
			public StatusBar.ControlToolTip.Tool GetTool(object key)
			{
				return (StatusBar.ControlToolTip.Tool)this.tools[key];
			}

			// Token: 0x0600604C RID: 24652 RVA: 0x0018AB40 File Offset: 0x00188D40
			private void AddTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0)
				{
					StatusBar statusBar = (StatusBar)this.parent;
					int num;
					if (statusBar.ToolTipSet)
					{
						num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(statusBar.MainToolTip, statusBar.MainToolTip.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(tool));
					}
					else
					{
						num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(tool));
					}
					if (num == 0)
					{
						throw new InvalidOperationException(SR.GetString("StatusBarAddFailed"));
					}
				}
			}

			// Token: 0x0600604D RID: 24653 RVA: 0x0018ABE8 File Offset: 0x00188DE8
			private void RemoveTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0 && (int)tool.id >= 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetMinTOOLINFO(tool));
				}
			}

			// Token: 0x0600604E RID: 24654 RVA: 0x0018AC3C File Offset: 0x00188E3C
			private void UpdateTool(StatusBar.ControlToolTip.Tool tool)
			{
				if (tool != null && tool.text != null && tool.text.Length > 0 && (int)tool.id >= 0)
				{
					UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, this.GetTOOLINFO(tool));
				}
			}

			// Token: 0x0600604F RID: 24655 RVA: 0x0018AC90 File Offset: 0x00188E90
			protected void CreateHandle()
			{
				if (this.IsHandleCreated)
				{
					return;
				}
				this.window.CreateHandle(this.CreateParams);
				SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 19);
				UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 1048, 0, SystemInformation.MaxWindowTrackSize.Width);
			}

			// Token: 0x06006050 RID: 24656 RVA: 0x0018ACF9 File Offset: 0x00188EF9
			protected void DestroyHandle()
			{
				if (this.IsHandleCreated)
				{
					this.window.DestroyHandle();
					this.tools.Clear();
				}
			}

			// Token: 0x06006051 RID: 24657 RVA: 0x0018AD19 File Offset: 0x00188F19
			public void Dispose()
			{
				this.DestroyHandle();
			}

			// Token: 0x06006052 RID: 24658 RVA: 0x0018AD24 File Offset: 0x00188F24
			private NativeMethods.TOOLINFO_T GetMinTOOLINFO(StatusBar.ControlToolTip.Tool tool)
			{
				NativeMethods.TOOLINFO_T toolinfo_T = new NativeMethods.TOOLINFO_T();
				toolinfo_T.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				toolinfo_T.hwnd = this.parent.Handle;
				if ((int)tool.id < 0)
				{
					this.AssignId(tool);
				}
				StatusBar statusBar = (StatusBar)this.parent;
				if (statusBar != null && statusBar.ToolTipSet)
				{
					toolinfo_T.uId = this.parent.Handle;
				}
				else
				{
					toolinfo_T.uId = tool.id;
				}
				return toolinfo_T;
			}

			// Token: 0x06006053 RID: 24659 RVA: 0x0018ADAC File Offset: 0x00188FAC
			private NativeMethods.TOOLINFO_T GetTOOLINFO(StatusBar.ControlToolTip.Tool tool)
			{
				NativeMethods.TOOLINFO_T minTOOLINFO = this.GetMinTOOLINFO(tool);
				minTOOLINFO.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
				minTOOLINFO.uFlags |= 272;
				Control control = this.parent;
				if (control != null && control.RightToLeft == RightToLeft.Yes)
				{
					minTOOLINFO.uFlags |= 4;
				}
				minTOOLINFO.lpszText = tool.text;
				minTOOLINFO.rect = NativeMethods.RECT.FromXYWH(tool.rect.X, tool.rect.Y, tool.rect.Width, tool.rect.Height);
				return minTOOLINFO;
			}

			// Token: 0x06006054 RID: 24660 RVA: 0x0018AE50 File Offset: 0x00189050
			~ControlToolTip()
			{
				this.DestroyHandle();
			}

			// Token: 0x06006055 RID: 24661 RVA: 0x0018AE7C File Offset: 0x0018907C
			protected void WndProc(ref Message msg)
			{
				int msg2 = msg.Msg;
				if (msg2 == 7)
				{
					return;
				}
				this.window.DefWndProc(ref msg);
			}

			// Token: 0x0400414D RID: 16717
			private Hashtable tools = new Hashtable();

			// Token: 0x0400414E RID: 16718
			private StatusBar.ControlToolTip.ToolTipNativeWindow window;

			// Token: 0x0400414F RID: 16719
			private Control parent;

			// Token: 0x04004150 RID: 16720
			private int nextId;

			// Token: 0x0200089C RID: 2204
			public class Tool
			{
				// Token: 0x04004401 RID: 17409
				public Rectangle rect = Rectangle.Empty;

				// Token: 0x04004402 RID: 17410
				public string text;

				// Token: 0x04004403 RID: 17411
				internal IntPtr id = new IntPtr(-1);
			}

			// Token: 0x0200089D RID: 2205
			private class ToolTipNativeWindow : NativeWindow
			{
				// Token: 0x060070D2 RID: 28882 RVA: 0x0019C24C File Offset: 0x0019A44C
				internal ToolTipNativeWindow(StatusBar.ControlToolTip control)
				{
					this.control = control;
				}

				// Token: 0x060070D3 RID: 28883 RVA: 0x0019C25B File Offset: 0x0019A45B
				protected override void WndProc(ref Message m)
				{
					if (this.control != null)
					{
						this.control.WndProc(ref m);
					}
				}

				// Token: 0x04004404 RID: 17412
				private StatusBar.ControlToolTip control;
			}
		}
	}
}

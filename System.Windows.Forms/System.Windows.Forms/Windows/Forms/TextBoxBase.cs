using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Implements the basic functionality required by text controls.</summary>
	// Token: 0x02000391 RID: 913
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("TextChanged")]
	[DefaultBindingProperty("Text")]
	[Designer("System.Windows.Forms.Design.TextBoxBaseDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public abstract class TextBoxBase : Control
	{
		// Token: 0x06003979 RID: 14713 RVA: 0x000FFB60 File Offset: 0x000FDD60
		internal TextBoxBase()
		{
			base.SetState2(2048, true);
			this.textBoxFlags[TextBoxBase.autoSize | TextBoxBase.hideSelection | TextBoxBase.wordWrap | TextBoxBase.shortcutsEnabled] = true;
			base.SetStyle(ControlStyles.FixedHeight, this.textBoxFlags[TextBoxBase.autoSize]);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.StandardClick | ControlStyles.StandardDoubleClick | ControlStyles.UseTextForAccessibility, false);
			this.requestedHeight = base.Height;
		}

		/// <summary>Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.</summary>
		/// <returns>
		///     <see langword="true" /> if users can enter tabs in a multiline text box using the TAB key; <see langword="false" /> if pressing the TAB key moves the focus. The default is <see langword="false" />.</returns>
		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600397A RID: 14714 RVA: 0x000FFBE4 File Offset: 0x000FDDE4
		// (set) Token: 0x0600397B RID: 14715 RVA: 0x000FFBF6 File Offset: 0x000FDDF6
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("TextBoxAcceptsTabDescr")]
		public bool AcceptsTab
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.acceptsTab];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.acceptsTab] != value)
				{
					this.textBoxFlags[TextBoxBase.acceptsTab] = value;
					this.OnAcceptsTabChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.AcceptsTab" /> property has changed.</summary>
		// Token: 0x140002CB RID: 715
		// (add) Token: 0x0600397C RID: 14716 RVA: 0x000FFC27 File Offset: 0x000FDE27
		// (remove) Token: 0x0600397D RID: 14717 RVA: 0x000FFC3A File Offset: 0x000FDE3A
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnAcceptsTabChangedDescr")]
		public event EventHandler AcceptsTabChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_ACCEPTSTABCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_ACCEPTSTABCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the defined shortcuts are enabled.</summary>
		/// <returns>
		///     <see langword="true" /> to enable the shortcuts; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600397E RID: 14718 RVA: 0x000FFC4D File Offset: 0x000FDE4D
		// (set) Token: 0x0600397F RID: 14719 RVA: 0x000FFC5F File Offset: 0x000FDE5F
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxShortcutsEnabledDescr")]
		public virtual bool ShortcutsEnabled
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.shortcutsEnabled];
			}
			set
			{
				if (TextBoxBase.shortcutsToDisable == null)
				{
					TextBoxBase.shortcutsToDisable = new int[]
					{
						131162,
						131139,
						131160,
						131158,
						131137,
						131148,
						131154,
						131141,
						131161,
						131080,
						131118,
						65582,
						65581,
						131146
					};
				}
				this.textBoxFlags[TextBoxBase.shortcutsEnabled] = value;
			}
		}

		/// <summary>Processes a command key.</summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message" />, passed by reference that represents the window message to process. </param>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the shortcut key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the command key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003980 RID: 14720 RVA: 0x000FFC90 File Offset: 0x000FDE90
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool flag = base.ProcessCmdKey(ref msg, keyData);
			if (!this.ShortcutsEnabled)
			{
				foreach (int num in TextBoxBase.shortcutsToDisable)
				{
					if (keyData == (Keys)num || keyData == (Keys)(num | 65536))
					{
						return true;
					}
				}
			}
			return (this.textBoxFlags[TextBoxBase.readOnly] && (keyData == (Keys)131148 || keyData == (Keys)131154 || keyData == (Keys)131141 || keyData == (Keys)131146)) || flag;
		}

		/// <summary>Gets or sets a value indicating whether the height of the control automatically adjusts when the font assigned to the control is changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the height of the control automatically adjusts when the font is changed; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06003981 RID: 14721 RVA: 0x000FFD12 File Offset: 0x000FDF12
		// (set) Token: 0x06003982 RID: 14722 RVA: 0x000FFD24 File Offset: 0x000FDF24
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("TextBoxAutoSizeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoSize
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.autoSize];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.autoSize] != value)
				{
					this.textBoxFlags[TextBoxBase.autoSize] = value;
					if (!this.Multiline)
					{
						base.SetStyle(ControlStyles.FixedHeight, value);
						this.AdjustHeight(false);
					}
					this.OnAutoSizeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Gets or sets the background color of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the background of the control.</returns>
		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x000FFD78 File Offset: 0x000FDF78
		// (set) Token: 0x06003984 RID: 14724 RVA: 0x00011FB9 File Offset: 0x000101B9
		[SRCategory("CatAppearance")]
		[DispId(-501)]
		[SRDescription("ControlBackColorDescr")]
		public override Color BackColor
		{
			get
			{
				if (this.ShouldSerializeBackColor())
				{
					return base.BackColor;
				}
				if (this.ReadOnly)
				{
					return SystemColors.Control;
				}
				return SystemColors.Window;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>The background image for the object.</returns>
		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x06003985 RID: 14725 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x06003986 RID: 14726 RVA: 0x00011FCA File Offset: 0x000101CA
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

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x140002CC RID: 716
		// (add) Token: 0x06003987 RID: 14727 RVA: 0x0001BA2E File Offset: 0x00019C2E
		// (remove) Token: 0x06003988 RID: 14728 RVA: 0x0001BA37 File Offset: 0x00019C37
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.BackgroundImage" /> property changes. This event is not relevant for this class.</summary>
		// Token: 0x140002CD RID: 717
		// (add) Token: 0x06003989 RID: 14729 RVA: 0x0001FD81 File Offset: 0x0001DF81
		// (remove) Token: 0x0600398A RID: 14730 RVA: 0x0001FD8A File Offset: 0x0001DF8A
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

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImageLayout" /> values.</returns>
		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x00011FD3 File Offset: 0x000101D3
		// (set) Token: 0x0600398C RID: 14732 RVA: 0x00011FDB File Offset: 0x000101DB
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

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.BackgroundImageLayout" /> property changes. This event is not relevant for this class.</summary>
		// Token: 0x140002CE RID: 718
		// (add) Token: 0x0600398D RID: 14733 RVA: 0x0001FD93 File Offset: 0x0001DF93
		// (remove) Token: 0x0600398E RID: 14734 RVA: 0x0001FD9C File Offset: 0x0001DF9C
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

		/// <summary>Gets or sets the border type of the text box control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.BorderStyle" /> that represents the border type of the text box control. The default is <see langword="Fixed3D" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value that is not within the range of valid values for the enumeration was assigned to the property. </exception>
		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x0600398F RID: 14735 RVA: 0x000FFD9C File Offset: 0x000FDF9C
		// (set) Token: 0x06003990 RID: 14736 RVA: 0x000FFDA4 File Offset: 0x000FDFA4
		[SRCategory("CatAppearance")]
		[DefaultValue(BorderStyle.Fixed3D)]
		[DispId(-504)]
		[SRDescription("TextBoxBorderDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
					{
						throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
					}
					this.borderStyle = value;
					base.UpdateStyles();
					base.RecreateHandle();
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.BorderStyle))
					{
						this.OnBorderStyleChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.BorderStyle" /> property has changed.</summary>
		// Token: 0x140002CF RID: 719
		// (add) Token: 0x06003991 RID: 14737 RVA: 0x000FFE34 File Offset: 0x000FE034
		// (remove) Token: 0x06003992 RID: 14738 RVA: 0x000FFE47 File Offset: 0x000FE047
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnBorderStyleChangedDescr")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_BORDERSTYLECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_BORDERSTYLECHANGED, value);
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool CanRaiseTextChangedEvent
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="P:System.Windows.Forms.Control.ImeMode" /> property can be set to an active value, to enable IME support.</summary>
		/// <returns>
		///     <see langword="false" /> if the <see cref="P:System.Windows.Forms.TextBoxBase.ReadOnly" /> property is <see langword="true" /> or if this <see cref="T:System.Windows.Forms.TextBoxBase" /> class is set to use a password mask character; otherwise, <see langword="true" />.</returns>
		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06003994 RID: 14740 RVA: 0x000FFE5C File Offset: 0x000FE05C
		protected override bool CanEnableIme
		{
			get
			{
				return !this.ReadOnly && !this.PasswordProtect && base.CanEnableIme;
			}
		}

		/// <summary>Gets a value indicating whether the user can undo the previous operation in a text box control.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can undo the previous operation performed in a text box control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x000FFE84 File Offset: 0x000FE084
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxCanUndoDescr")]
		public bool CanUndo
		{
			get
			{
				return base.IsHandleCreated && (int)((long)base.SendMessage(198, 0, 0)) != 0;
			}
		}

		/// <summary>Gets the required creation parameters when the control handle is created.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x000FFEB4 File Offset: 0x000FE0B4
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassName = "EDIT";
				createParams.Style |= 192;
				if (!this.textBoxFlags[TextBoxBase.hideSelection])
				{
					createParams.Style |= 256;
				}
				if (this.textBoxFlags[TextBoxBase.readOnly])
				{
					createParams.Style |= 2048;
				}
				createParams.ExStyle &= -513;
				createParams.Style &= -8388609;
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.ExStyle |= 512;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				if (this.textBoxFlags[TextBoxBase.multiline])
				{
					createParams.Style |= 4;
					if (this.textBoxFlags[TextBoxBase.wordWrap])
					{
						createParams.Style &= -129;
					}
				}
				return createParams;
			}
		}

		/// <summary>Gets or sets a value indicating whether control drawing is done in a buffer before the control is displayed. This property is not relevant for this class.</summary>
		/// <returns>
		///     <see langword="true" /> to implement double buffering on the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x000A2CB2 File Offset: 0x000A0EB2
		// (set) Token: 0x06003998 RID: 14744 RVA: 0x000A2CBA File Offset: 0x000A0EBA
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

		/// <summary>Occurs when the text box is clicked.</summary>
		// Token: 0x140002D0 RID: 720
		// (add) Token: 0x06003999 RID: 14745 RVA: 0x000A2B72 File Offset: 0x000A0D72
		// (remove) Token: 0x0600399A RID: 14746 RVA: 0x000A2B7B File Offset: 0x000A0D7B
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler Click
		{
			add
			{
				base.Click += value;
			}
			remove
			{
				base.Click -= value;
			}
		}

		/// <summary>Occurs when the control is clicked by the mouse.</summary>
		// Token: 0x140002D1 RID: 721
		// (add) Token: 0x0600399B RID: 14747 RVA: 0x000A2FE9 File Offset: 0x000A11E9
		// (remove) Token: 0x0600399C RID: 14748 RVA: 0x000A2FF2 File Offset: 0x000A11F2
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event MouseEventHandler MouseClick
		{
			add
			{
				base.MouseClick += value;
			}
			remove
			{
				base.MouseClick -= value;
			}
		}

		/// <summary>Gets or sets the default cursor for the control.</summary>
		/// <returns>An object of type <see cref="T:System.Windows.Forms.Cursor" /> representing the current default cursor.</returns>
		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000FFFCB File Offset: 0x000FE1CB
		protected override Cursor DefaultCursor
		{
			get
			{
				return Cursors.IBeam;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600399E RID: 14750 RVA: 0x000FFFD2 File Offset: 0x000FE1D2
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, this.PreferredHeight);
			}
		}

		/// <summary>Gets or sets the foreground color of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the control's foreground color.</returns>
		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600399F RID: 14751 RVA: 0x000201D0 File Offset: 0x0001E3D0
		// (set) Token: 0x060039A0 RID: 14752 RVA: 0x0001208A File Offset: 0x0001028A
		[SRCategory("CatAppearance")]
		[DispId(-513)]
		[SRDescription("ControlForeColorDescr")]
		public override Color ForeColor
		{
			get
			{
				if (this.ShouldSerializeForeColor())
				{
					return base.ForeColor;
				}
				return SystemColors.WindowText;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.</summary>
		/// <returns>
		///     <see langword="true" /> if the selected text does not appear highlighted when the text box control loses focus; <see langword="false" />, if the selected text remains highlighted when the text box control loses focus. The default is <see langword="true" />.</returns>
		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x060039A1 RID: 14753 RVA: 0x000FFFE1 File Offset: 0x000FE1E1
		// (set) Token: 0x060039A2 RID: 14754 RVA: 0x000FFFF3 File Offset: 0x000FE1F3
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		[SRDescription("TextBoxHideSelectionDescr")]
		public bool HideSelection
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.hideSelection];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.hideSelection] != value)
				{
					this.textBoxFlags[TextBoxBase.hideSelection] = value;
					base.RecreateHandle();
					this.OnHideSelectionChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.HideSelection" /> property has changed.</summary>
		// Token: 0x140002D2 RID: 722
		// (add) Token: 0x060039A3 RID: 14755 RVA: 0x0010002A File Offset: 0x000FE22A
		// (remove) Token: 0x060039A4 RID: 14756 RVA: 0x0010003D File Offset: 0x000FE23D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnHideSelectionChangedDescr")]
		public event EventHandler HideSelectionChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_HIDESELECTIONCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_HIDESELECTIONCHANGED, value);
			}
		}

		/// <summary>Gets or sets the Input Method Editor (IME) mode of a control.</summary>
		/// <returns>The IME mode of the control.</returns>
		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060039A5 RID: 14757 RVA: 0x00100050 File Offset: 0x000FE250
		// (set) Token: 0x060039A6 RID: 14758 RVA: 0x0010007F File Offset: 0x000FE27F
		protected override ImeMode ImeModeBase
		{
			get
			{
				if (base.DesignMode)
				{
					return base.ImeModeBase;
				}
				return this.CanEnableIme ? base.ImeModeBase : ImeMode.Disable;
			}
			set
			{
				base.ImeModeBase = value;
			}
		}

		/// <summary>Gets or sets the lines of text in a text box control.</summary>
		/// <returns>An array of strings that contains the text in a text box control.</returns>
		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060039A7 RID: 14759 RVA: 0x00100088 File Offset: 0x000FE288
		// (set) Token: 0x060039A8 RID: 14760 RVA: 0x0010016C File Offset: 0x000FE36C
		[SRCategory("CatAppearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MergableProperty(false)]
		[Localizable(true)]
		[SRDescription("TextBoxLinesDescr")]
		[Editor("System.Windows.Forms.Design.StringArrayEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string[] Lines
		{
			get
			{
				string text = this.Text;
				ArrayList arrayList = new ArrayList();
				int j;
				for (int i = 0; i < text.Length; i = j)
				{
					for (j = i; j < text.Length; j++)
					{
						char c = text[j];
						if (c == '\r' || c == '\n')
						{
							break;
						}
					}
					string value = text.Substring(i, j - i);
					arrayList.Add(value);
					if (j < text.Length && text[j] == '\r')
					{
						j++;
					}
					if (j < text.Length && text[j] == '\n')
					{
						j++;
					}
				}
				if (text.Length > 0 && (text[text.Length - 1] == '\r' || text[text.Length - 1] == '\n'))
				{
					arrayList.Add("");
				}
				return (string[])arrayList.ToArray(typeof(string));
			}
			set
			{
				if (value != null && value.Length != 0)
				{
					StringBuilder stringBuilder = new StringBuilder(value[0]);
					for (int i = 1; i < value.Length; i++)
					{
						stringBuilder.Append("\r\n");
						stringBuilder.Append(value[i]);
					}
					this.Text = stringBuilder.ToString();
					return;
				}
				this.Text = "";
			}
		}

		/// <summary>Gets or sets the maximum number of characters the user can type or paste into the text box control.</summary>
		/// <returns>The number of characters that can be entered into the control. The default is 32767.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value assigned to the property is less than 0. </exception>
		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x060039A9 RID: 14761 RVA: 0x001001C5 File Offset: 0x000FE3C5
		// (set) Token: 0x060039AA RID: 14762 RVA: 0x001001D0 File Offset: 0x000FE3D0
		[SRCategory("CatBehavior")]
		[DefaultValue(32767)]
		[Localizable(true)]
		[SRDescription("TextBoxMaxLengthDescr")]
		public virtual int MaxLength
		{
			get
			{
				return this.maxLength;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MaxLength", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MaxLength",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.maxLength != value)
				{
					this.maxLength = value;
					this.UpdateMaxLength();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates that the text box control has been modified by the user since the control was created or its contents were last set.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's contents have been modified; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x0010023C File Offset: 0x000FE43C
		// (set) Token: 0x060039AC RID: 14764 RVA: 0x001002AC File Offset: 0x000FE4AC
		[SRCategory("CatBehavior")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxModifiedDescr")]
		public bool Modified
		{
			get
			{
				if (base.IsHandleCreated)
				{
					bool flag = (int)((long)base.SendMessage(184, 0, 0)) != 0;
					if (this.textBoxFlags[TextBoxBase.modified] != flag)
					{
						this.textBoxFlags[TextBoxBase.modified] = flag;
						this.OnModifiedChanged(EventArgs.Empty);
					}
					return flag;
				}
				return this.textBoxFlags[TextBoxBase.modified];
			}
			set
			{
				if (this.Modified != value)
				{
					if (base.IsHandleCreated)
					{
						base.SendMessage(185, value ? 1 : 0, 0);
					}
					this.textBoxFlags[TextBoxBase.modified] = value;
					this.OnModifiedChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.Modified" /> property has changed.</summary>
		// Token: 0x140002D3 RID: 723
		// (add) Token: 0x060039AD RID: 14765 RVA: 0x001002FA File Offset: 0x000FE4FA
		// (remove) Token: 0x060039AE RID: 14766 RVA: 0x0010030D File Offset: 0x000FE50D
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnModifiedChangedDescr")]
		public event EventHandler ModifiedChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_MODIFIEDCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_MODIFIEDCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating whether this is a multiline text box control.</summary>
		/// <returns>
		///     <see langword="true" /> if the control is a multiline text box control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x060039AF RID: 14767 RVA: 0x00100320 File Offset: 0x000FE520
		// (set) Token: 0x060039B0 RID: 14768 RVA: 0x00100334 File Offset: 0x000FE534
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[Localizable(true)]
		[SRDescription("TextBoxMultilineDescr")]
		[RefreshProperties(RefreshProperties.All)]
		public virtual bool Multiline
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.multiline];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.multiline] != value)
				{
					using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.Multiline))
					{
						this.textBoxFlags[TextBoxBase.multiline] = value;
						if (value)
						{
							base.SetStyle(ControlStyles.FixedHeight, false);
						}
						else
						{
							base.SetStyle(ControlStyles.FixedHeight, this.AutoSize);
						}
						base.RecreateHandle();
						this.AdjustHeight(false);
						this.OnMultilineChanged(EventArgs.Empty);
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.Multiline" /> property has changed.</summary>
		// Token: 0x140002D4 RID: 724
		// (add) Token: 0x060039B1 RID: 14769 RVA: 0x001003D0 File Offset: 0x000FE5D0
		// (remove) Token: 0x060039B2 RID: 14770 RVA: 0x001003E3 File Offset: 0x000FE5E3
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnMultilineChangedDescr")]
		public event EventHandler MultilineChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_MULTILINECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_MULTILINECHANGED, value);
			}
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value.</returns>
		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x060039B3 RID: 14771 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x060039B4 RID: 14772 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>This event is not relevant for this class.</summary>
		// Token: 0x140002D5 RID: 725
		// (add) Token: 0x060039B5 RID: 14773 RVA: 0x000204AB File Offset: 0x0001E6AB
		// (remove) Token: 0x060039B6 RID: 14774 RVA: 0x000204B4 File Offset: 0x0001E6B4
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRCategory("CatLayout")]
		[SRDescription("ControlOnPaddingChangedDescr")]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x060039B7 RID: 14775 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal virtual bool PasswordProtect
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the preferred height for a text box.</summary>
		/// <returns>The preferred height of a text box.</returns>
		// Token: 0x17000E72 RID: 3698
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x001003F8 File Offset: 0x000FE5F8
		[SRCategory("CatLayout")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxPreferredHeightDescr")]
		public int PreferredHeight
		{
			get
			{
				int num = base.FontHeight;
				if (this.borderStyle != BorderStyle.None)
				{
					num += SystemInformation.GetBorderSizeForDpi(this.deviceDpi).Height * 4 + 3;
				}
				return num;
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00100430 File Offset: 0x000FE630
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			Size size = this.SizeFromClientSize(Size.Empty) + this.Padding.Size;
			if (this.BorderStyle != BorderStyle.None)
			{
				size += new Size(0, 3);
			}
			if (this.BorderStyle == BorderStyle.FixedSingle)
			{
				size.Width += 2;
				size.Height += 2;
			}
			proposedConstraints -= size;
			TextFormatFlags textFormatFlags = TextFormatFlags.NoPrefix;
			if (!this.Multiline)
			{
				textFormatFlags |= TextFormatFlags.SingleLine;
			}
			else if (this.WordWrap)
			{
				textFormatFlags |= TextFormatFlags.WordBreak;
			}
			Size sz = TextRenderer.MeasureText(this.Text, this.Font, proposedConstraints, textFormatFlags);
			sz.Height = Math.Max(sz.Height, base.FontHeight);
			return sz + size;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001004FC File Offset: 0x000FE6FC
		internal void GetSelectionStartAndLength(out int start, out int length)
		{
			int num = 0;
			if (!base.IsHandleCreated)
			{
				this.AdjustSelectionStartAndEnd(this.selectionStart, this.selectionLength, out start, out num, -1);
				length = num - start;
				return;
			}
			start = 0;
			UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 176, ref start, ref num);
			start = Math.Max(0, start);
			num = Math.Max(0, num);
			if (this.SelectionUsesDbcsOffsetsInWin9x && Marshal.SystemDefaultCharSize == 1)
			{
				TextBoxBase.ToUnicodeOffsets(this.WindowText, ref start, ref num);
			}
			length = num - start;
		}

		/// <summary>Gets or sets a value indicating whether text in the text box is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the text box is read-only; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000E73 RID: 3699
		// (get) Token: 0x060039BB RID: 14779 RVA: 0x00100583 File Offset: 0x000FE783
		// (set) Token: 0x060039BC RID: 14780 RVA: 0x00100598 File Offset: 0x000FE798
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("TextBoxReadOnlyDescr")]
		public bool ReadOnly
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.readOnly];
			}
			set
			{
				if (this.textBoxFlags[TextBoxBase.readOnly] != value)
				{
					this.textBoxFlags[TextBoxBase.readOnly] = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(207, value ? -1 : 0, 0);
					}
					this.OnReadOnlyChanged(EventArgs.Empty);
					base.VerifyImeRestrictedModeChanged();
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.TextBoxBase.ReadOnly" /> property has changed.</summary>
		// Token: 0x140002D6 RID: 726
		// (add) Token: 0x060039BD RID: 14781 RVA: 0x001005F6 File Offset: 0x000FE7F6
		// (remove) Token: 0x060039BE RID: 14782 RVA: 0x00100609 File Offset: 0x000FE809
		[SRCategory("CatPropertyChanged")]
		[SRDescription("TextBoxBaseOnReadOnlyChangedDescr")]
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(TextBoxBase.EVENT_READONLYCHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(TextBoxBase.EVENT_READONLYCHANGED, value);
			}
		}

		/// <summary>Gets or sets a value indicating the currently selected text in the control.</summary>
		/// <returns>A string that represents the currently selected text in the text box.</returns>
		// Token: 0x17000E74 RID: 3700
		// (get) Token: 0x060039BF RID: 14783 RVA: 0x0010061C File Offset: 0x000FE81C
		// (set) Token: 0x060039C0 RID: 14784 RVA: 0x000CBC1C File Offset: 0x000C9E1C
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectedTextDescr")]
		public virtual string SelectedText
		{
			get
			{
				int startIndex;
				int length;
				this.GetSelectionStartAndLength(out startIndex, out length);
				return this.Text.Substring(startIndex, length);
			}
			set
			{
				this.SetSelectedTextInternal(value, true);
			}
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x00100640 File Offset: 0x000FE840
		internal virtual void SetSelectedTextInternal(string text, bool clearUndo)
		{
			if (!base.IsHandleCreated)
			{
				this.CreateHandle();
			}
			if (text == null)
			{
				text = "";
			}
			base.SendMessage(197, 0, 0);
			if (clearUndo)
			{
				base.SendMessage(194, 0, text);
				base.SendMessage(185, 0, 0);
				this.ClearUndo();
			}
			else
			{
				base.SendMessage(194, -1, text);
			}
			base.SendMessage(197, this.maxLength, 0);
		}

		/// <summary>Gets or sets the number of characters selected in the text box.</summary>
		/// <returns>The number of characters selected in the text box.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than zero.</exception>
		// Token: 0x17000E75 RID: 3701
		// (get) Token: 0x060039C2 RID: 14786 RVA: 0x001006BC File Offset: 0x000FE8BC
		// (set) Token: 0x060039C3 RID: 14787 RVA: 0x001006D4 File Offset: 0x000FE8D4
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionLengthDescr")]
		public virtual int SelectionLength
		{
			get
			{
				int num;
				int result;
				this.GetSelectionStartAndLength(out num, out result);
				return result;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionLength", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionLength",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				int start;
				int num;
				this.GetSelectionStartAndLength(out start, out num);
				if (value != num)
				{
					this.Select(start, value);
				}
			}
		}

		/// <summary>Gets or sets the starting point of text selected in the text box.</summary>
		/// <returns>The starting position of text selected in the text box.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than zero.</exception>
		// Token: 0x17000E76 RID: 3702
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x00100730 File Offset: 0x000FE930
		// (set) Token: 0x060039C5 RID: 14789 RVA: 0x00100748 File Offset: 0x000FE948
		[SRCategory("CatAppearance")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("TextBoxSelectionStartDescr")]
		public int SelectionStart
		{
			get
			{
				int result;
				int num;
				this.GetSelectionStartAndLength(out result, out num);
				return result;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("SelectionStart", SR.GetString("InvalidArgument", new object[]
					{
						"SelectionStart",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.Select(value, this.SelectionLength);
			}
		}

		// Token: 0x17000E77 RID: 3703
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool SetSelectionInCreateHandle
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the current text in the text box.</summary>
		/// <returns>The text displayed in the control.</returns>
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x060039C7 RID: 14791 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x060039C8 RID: 14792 RVA: 0x00100798 File Offset: 0x000FE998
		[Localizable(true)]
		[Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				if (value != base.Text)
				{
					base.Text = value;
					if (base.IsHandleCreated)
					{
						base.SendMessage(185, 0, 0);
					}
				}
			}
		}

		/// <summary>Gets the length of text in the control.</summary>
		/// <returns>The number of characters contained in the text of the control.</returns>
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x060039C9 RID: 14793 RVA: 0x001007C5 File Offset: 0x000FE9C5
		[Browsable(false)]
		public virtual int TextLength
		{
			get
			{
				if (base.IsHandleCreated && Marshal.SystemDefaultCharSize == 2)
				{
					return SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
				}
				return this.Text.Length;
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x060039CA RID: 14794 RVA: 0x0000E214 File Offset: 0x0000C414
		internal virtual bool SelectionUsesDbcsOffsetsInWin9x
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x060039CB RID: 14795 RVA: 0x001007F4 File Offset: 0x000FE9F4
		// (set) Token: 0x060039CC RID: 14796 RVA: 0x001007FC File Offset: 0x000FE9FC
		internal override string WindowText
		{
			get
			{
				return base.WindowText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.WindowText.Equals(value))
				{
					this.textBoxFlags[TextBoxBase.codeUpdateText] = true;
					try
					{
						base.WindowText = value;
					}
					finally
					{
						this.textBoxFlags[TextBoxBase.codeUpdateText] = false;
					}
				}
			}
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00100860 File Offset: 0x000FEA60
		internal void ForceWindowText(string value)
		{
			if (value == null)
			{
				value = "";
			}
			this.textBoxFlags[TextBoxBase.codeUpdateText] = true;
			try
			{
				if (base.IsHandleCreated)
				{
					UnsafeNativeMethods.SetWindowText(new HandleRef(this, base.Handle), value);
				}
				else if (value.Length == 0)
				{
					this.Text = null;
				}
				else
				{
					this.Text = value;
				}
			}
			finally
			{
				this.textBoxFlags[TextBoxBase.codeUpdateText] = false;
			}
		}

		/// <summary>Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.</summary>
		/// <returns>
		///     <see langword="true" /> if the multiline text box control wraps words; <see langword="false" /> if the text box control automatically scrolls horizontally when the user types past the right edge of the control. The default is <see langword="true" />.</returns>
		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x060039CE RID: 14798 RVA: 0x001008E4 File Offset: 0x000FEAE4
		// (set) Token: 0x060039CF RID: 14799 RVA: 0x001008F8 File Offset: 0x000FEAF8
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(true)]
		[SRDescription("TextBoxWordWrapDescr")]
		public bool WordWrap
		{
			get
			{
				return this.textBoxFlags[TextBoxBase.wordWrap];
			}
			set
			{
				using (LayoutTransaction.CreateTransactionIf(this.AutoSize, this.ParentInternal, this, PropertyNames.WordWrap))
				{
					if (this.textBoxFlags[TextBoxBase.wordWrap] != value)
					{
						this.textBoxFlags[TextBoxBase.wordWrap] = value;
						base.RecreateHandle();
					}
				}
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x00100964 File Offset: 0x000FEB64
		private void AdjustHeight(bool returnIfAnchored)
		{
			if (returnIfAnchored && (this.Anchor & (AnchorStyles.Top | AnchorStyles.Bottom)) == (AnchorStyles.Top | AnchorStyles.Bottom))
			{
				return;
			}
			int num = this.requestedHeight;
			try
			{
				if (this.textBoxFlags[TextBoxBase.autoSize] && !this.textBoxFlags[TextBoxBase.multiline])
				{
					base.Height = this.PreferredHeight;
				}
				else
				{
					int height = base.Height;
					if (this.textBoxFlags[TextBoxBase.multiline])
					{
						base.Height = Math.Max(num, this.PreferredHeight + 2);
					}
					this.integralHeightAdjust = true;
					try
					{
						base.Height = num;
					}
					finally
					{
						this.integralHeightAdjust = false;
					}
				}
			}
			finally
			{
				this.requestedHeight = num;
			}
		}

		/// <summary>Appends text to the current text of a text box.</summary>
		/// <param name="text">The text to append to the current contents of the text box. </param>
		// Token: 0x060039D1 RID: 14801 RVA: 0x00100A24 File Offset: 0x000FEC24
		public void AppendText(string text)
		{
			if (text.Length > 0)
			{
				int start;
				int length;
				this.GetSelectionStartAndLength(out start, out length);
				try
				{
					int endPosition = this.GetEndPosition();
					this.SelectInternal(endPosition, endPosition, endPosition);
					this.SelectedText = text;
				}
				finally
				{
					if (base.Width == 0 || base.Height == 0)
					{
						this.Select(start, length);
					}
				}
			}
		}

		/// <summary>Clears all text from the text box control.</summary>
		// Token: 0x060039D2 RID: 14802 RVA: 0x00100A88 File Offset: 0x000FEC88
		public void Clear()
		{
			this.Text = null;
		}

		/// <summary>Clears information about the most recent operation from the undo buffer of the text box.</summary>
		// Token: 0x060039D3 RID: 14803 RVA: 0x00100A91 File Offset: 0x000FEC91
		public void ClearUndo()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(205, 0, 0);
			}
		}

		/// <summary>Copies the current selection in the text box to the Clipboard.</summary>
		// Token: 0x060039D4 RID: 14804 RVA: 0x00100AA9 File Offset: 0x000FECA9
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public void Copy()
		{
			base.SendMessage(769, 0, 0);
		}

		/// <summary>Creates a handle for the control.</summary>
		// Token: 0x060039D5 RID: 14805 RVA: 0x00100ABC File Offset: 0x000FECBC
		protected override void CreateHandle()
		{
			this.textBoxFlags[TextBoxBase.creatingHandle] = true;
			try
			{
				base.CreateHandle();
				if (this.SetSelectionInCreateHandle)
				{
					this.SetSelectionOnHandle();
				}
			}
			finally
			{
				this.textBoxFlags[TextBoxBase.creatingHandle] = false;
			}
		}

		/// <summary>Moves the current selection in the text box to the Clipboard.</summary>
		// Token: 0x060039D6 RID: 14806 RVA: 0x00100B14 File Offset: 0x000FED14
		public void Cut()
		{
			base.SendMessage(768, 0, 0);
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x00100B24 File Offset: 0x000FED24
		internal virtual int GetEndPosition()
		{
			if (!base.IsHandleCreated)
			{
				return this.TextLength;
			}
			return this.TextLength + 1;
		}

		/// <summary>Determines whether the specified key is an input key or a special key that requires preprocessing.</summary>
		/// <param name="keyData">One of the Keys value.</param>
		/// <returns>
		///     <see langword="true" /> if the specified key is an input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x060039D8 RID: 14808 RVA: 0x00100B40 File Offset: 0x000FED40
		protected override bool IsInputKey(Keys keyData)
		{
			if ((keyData & Keys.Alt) != Keys.Alt)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys <= Keys.Tab)
				{
					if (keys != Keys.Back)
					{
						if (keys == Keys.Tab)
						{
							return this.Multiline && this.textBoxFlags[TextBoxBase.acceptsTab] && (keyData & Keys.Control) == Keys.None;
						}
					}
					else if (!this.ReadOnly)
					{
						return true;
					}
				}
				else if (keys != Keys.Escape)
				{
					if (keys - Keys.Prior <= 3)
					{
						return true;
					}
				}
				else if (this.Multiline)
				{
					return false;
				}
			}
			return base.IsInputKey(keyData);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060039D9 RID: 14809 RVA: 0x00100BC8 File Offset: 0x000FEDC8
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			CommonProperties.xClearPreferredSizeCache(this);
			this.AdjustHeight(true);
			this.UpdateMaxLength();
			if (this.textBoxFlags[TextBoxBase.modified])
			{
				base.SendMessage(185, 1, 0);
			}
			if (this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated])
			{
				this.ScrollToCaret();
				this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated] = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060039DA RID: 14810 RVA: 0x00100C38 File Offset: 0x000FEE38
		protected override void OnHandleDestroyed(EventArgs e)
		{
			this.textBoxFlags[TextBoxBase.modified] = this.Modified;
			this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = true;
			this.GetSelectionStartAndLength(out this.selectionStart, out this.selectionLength);
			base.OnHandleDestroyed(e);
		}

		/// <summary>Replaces the current selection in the text box with the contents of the Clipboard.</summary>
		// Token: 0x060039DB RID: 14811 RVA: 0x00100C85 File Offset: 0x000FEE85
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public void Paste()
		{
			IntSecurity.ClipboardRead.Demand();
			base.SendMessage(770, 0, 0);
		}

		/// <summary>Processes a dialog key.</summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values that represents the key to process. </param>
		/// <returns>
		///     <see langword="true" /> if the key was processed by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060039DC RID: 14812 RVA: 0x00100CA0 File Offset: 0x000FEEA0
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = keyData & Keys.KeyCode;
			if (keys == Keys.Tab && this.AcceptsTab && (keyData & Keys.Control) != Keys.None)
			{
				keyData &= ~Keys.Control;
			}
			return base.ProcessDialogKey(keyData);
		}

		/// <summary>Occurs when the control is redrawn. This event is not relevant for this class.</summary>
		// Token: 0x140002D7 RID: 727
		// (add) Token: 0x060039DD RID: 14813 RVA: 0x00020D37 File Offset: 0x0001EF37
		// (remove) Token: 0x060039DE RID: 14814 RVA: 0x00020D40 File Offset: 0x0001EF40
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

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBoxBase.AcceptsTabChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039DF RID: 14815 RVA: 0x00100CDC File Offset: 0x000FEEDC
		protected virtual void OnAcceptsTabChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_ACCEPTSTABCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBoxBase.BorderStyleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E0 RID: 14816 RVA: 0x00100D0C File Offset: 0x000FEF0C
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_BORDERSTYLECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E1 RID: 14817 RVA: 0x00100D3A File Offset: 0x000FEF3A
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustHeight(false);
		}

		/// <summary>Raise the <see cref="E:System.Windows.Forms.TextBoxBase.HideSelectionChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E2 RID: 14818 RVA: 0x00100D4C File Offset: 0x000FEF4C
		protected virtual void OnHideSelectionChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_HIDESELECTIONCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBoxBase.ModifiedChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E3 RID: 14819 RVA: 0x00100D7C File Offset: 0x000FEF7C
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_MODIFIEDCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.</summary>
		/// <param name="mevent">The event data.</param>
		// Token: 0x060039E4 RID: 14820 RVA: 0x00100DAC File Offset: 0x000FEFAC
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			Point point = base.PointToScreen(mevent.Location);
			if (mevent.Button == MouseButtons.Left)
			{
				if (!base.ValidationCancelled && UnsafeNativeMethods.WindowFromPoint(point.X, point.Y) == base.Handle)
				{
					if (!this.doubleClickFired)
					{
						this.OnClick(mevent);
						this.OnMouseClick(mevent);
					}
					else
					{
						this.doubleClickFired = false;
						this.OnDoubleClick(mevent);
						this.OnMouseDoubleClick(mevent);
					}
				}
				this.doubleClickFired = false;
			}
			base.OnMouseUp(mevent);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBoxBase.MultilineChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E5 RID: 14821 RVA: 0x00100E38 File Offset: 0x000FF038
		protected virtual void OnMultilineChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_MULTILINECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>This method is not relevant for this class.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060039E6 RID: 14822 RVA: 0x00100E66 File Offset: 0x000FF066
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.AdjustHeight(false);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.TextBoxBase.ReadOnlyChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060039E7 RID: 14823 RVA: 0x00100E78 File Offset: 0x000FF078
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[TextBoxBase.EVENT_READONLYCHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060039E8 RID: 14824 RVA: 0x00100EA6 File Offset: 0x000FF0A6
		protected override void OnTextChanged(EventArgs e)
		{
			CommonProperties.xClearPreferredSizeCache(this);
			base.OnTextChanged(e);
		}

		/// <summary>Retrieves the character that is closest to the specified location within the control.</summary>
		/// <param name="pt">The location from which to seek the nearest character. </param>
		/// <returns>The character at the specified location.</returns>
		// Token: 0x060039E9 RID: 14825 RVA: 0x00100EB8 File Offset: 0x000FF0B8
		public virtual char GetCharFromPosition(Point pt)
		{
			string text = this.Text;
			int charIndexFromPosition = this.GetCharIndexFromPosition(pt);
			if (charIndexFromPosition >= 0 && charIndexFromPosition < text.Length)
			{
				return text[charIndexFromPosition];
			}
			return '\0';
		}

		/// <summary>Retrieves the index of the character nearest to the specified location.</summary>
		/// <param name="pt">The location to search. </param>
		/// <returns>The zero-based character index at the specified location.</returns>
		// Token: 0x060039EA RID: 14826 RVA: 0x00100EEC File Offset: 0x000FF0EC
		public virtual int GetCharIndexFromPosition(Point pt)
		{
			int lParam = NativeMethods.Util.MAKELONG(pt.X, pt.Y);
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 215, 0, lParam);
			num = NativeMethods.Util.LOWORD(num);
			if (num < 0)
			{
				num = 0;
			}
			else
			{
				string text = this.Text;
				if (num >= text.Length)
				{
					num = Math.Max(text.Length - 1, 0);
				}
			}
			return num;
		}

		/// <summary>Retrieves the line number from the specified character position within the text of the control.</summary>
		/// <param name="index">The character index position to search. </param>
		/// <returns>The zero-based line number in which the character index is located.</returns>
		// Token: 0x060039EB RID: 14827 RVA: 0x00100F5A File Offset: 0x000FF15A
		public virtual int GetLineFromCharIndex(int index)
		{
			return (int)((long)base.SendMessage(201, index, 0));
		}

		/// <summary>Retrieves the location within the control at the specified character index.</summary>
		/// <param name="index">The index of the character for which to retrieve the location. </param>
		/// <returns>The location of the specified character within the client rectangle of the control.</returns>
		// Token: 0x060039EC RID: 14828 RVA: 0x00100F70 File Offset: 0x000FF170
		public virtual Point GetPositionFromCharIndex(int index)
		{
			if (index < 0 || index >= this.Text.Length)
			{
				return Point.Empty;
			}
			int n = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 214, index, 0);
			return new Point(NativeMethods.Util.SignedLOWORD(n), NativeMethods.Util.SignedHIWORD(n));
		}

		/// <summary>Retrieves the index of the first character of a given line.</summary>
		/// <param name="lineNumber">The line for which to get the index of its first character. </param>
		/// <returns>The zero-based index of the first character in the specified line.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="lineNumber" /> parameter is less than zero.</exception>
		// Token: 0x060039ED RID: 14829 RVA: 0x00100FC4 File Offset: 0x000FF1C4
		public int GetFirstCharIndexFromLine(int lineNumber)
		{
			if (lineNumber < 0)
			{
				throw new ArgumentOutOfRangeException("lineNumber", SR.GetString("InvalidArgument", new object[]
				{
					"lineNumber",
					lineNumber.ToString(CultureInfo.CurrentCulture)
				}));
			}
			return (int)((long)base.SendMessage(187, lineNumber, 0));
		}

		/// <summary>Retrieves the index of the first character of the current line.</summary>
		/// <returns>The zero-based character index in the current line.</returns>
		// Token: 0x060039EE RID: 14830 RVA: 0x0010101A File Offset: 0x000FF21A
		public int GetFirstCharIndexOfCurrentLine()
		{
			return (int)((long)base.SendMessage(187, -1, 0));
		}

		/// <summary>Scrolls the contents of the control to the current caret position.</summary>
		// Token: 0x060039EF RID: 14831 RVA: 0x00101030 File Offset: 0x000FF230
		public void ScrollToCaret()
		{
			if (base.IsHandleCreated)
			{
				if (string.IsNullOrEmpty(this.WindowText))
				{
					return;
				}
				bool flag = false;
				object o = null;
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					if (UnsafeNativeMethods.SendMessage(new HandleRef(this, base.Handle), 1084, 0, out o) != 0)
					{
						intPtr = Marshal.GetIUnknownForObject(o);
						if (intPtr != IntPtr.Zero)
						{
							IntPtr zero = IntPtr.Zero;
							Guid guid = typeof(UnsafeNativeMethods.ITextDocument).GUID;
							try
							{
								Marshal.QueryInterface(intPtr, ref guid, out zero);
								UnsafeNativeMethods.ITextDocument textDocument = Marshal.GetObjectForIUnknown(zero) as UnsafeNativeMethods.ITextDocument;
								if (textDocument != null)
								{
									int num;
									int num2;
									this.GetSelectionStartAndLength(out num, out num2);
									int lineFromCharIndex = this.GetLineFromCharIndex(num);
									UnsafeNativeMethods.ITextRange textRange = textDocument.Range(this.WindowText.Length - 1, this.WindowText.Length - 1);
									textRange.ScrollIntoView(0);
									int num3 = (int)((long)base.SendMessage(206, 0, 0));
									if (num3 > lineFromCharIndex)
									{
										textRange = textDocument.Range(num, num + num2);
										textRange.ScrollIntoView(32);
									}
									flag = true;
								}
							}
							finally
							{
								if (zero != IntPtr.Zero)
								{
									Marshal.Release(zero);
								}
							}
						}
					}
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						Marshal.Release(intPtr);
					}
				}
				if (!flag)
				{
					base.SendMessage(183, 0, 0);
					return;
				}
			}
			else
			{
				this.textBoxFlags[TextBoxBase.scrollToCaretOnHandleCreated] = true;
			}
		}

		/// <summary>Specifies that the value of the <see cref="P:System.Windows.Forms.TextBoxBase.SelectionLength" /> property is zero so that no characters are selected in the control.</summary>
		// Token: 0x060039F0 RID: 14832 RVA: 0x001011AC File Offset: 0x000FF3AC
		public void DeselectAll()
		{
			this.SelectionLength = 0;
		}

		/// <summary>Selects a range of text in the text box.</summary>
		/// <param name="start">The position of the first character in the current text selection within the text box. </param>
		/// <param name="length">The number of characters to select. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <paramref name="start" /> parameter is less than zero.</exception>
		// Token: 0x060039F1 RID: 14833 RVA: 0x001011B8 File Offset: 0x000FF3B8
		public void Select(int start, int length)
		{
			if (start < 0)
			{
				throw new ArgumentOutOfRangeException("start", SR.GetString("InvalidArgument", new object[]
				{
					"start",
					start.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int textLength = this.TextLength;
			if (start > textLength)
			{
				long num = Math.Min(0L, (long)length + (long)start - (long)textLength);
				if (num < -2147483648L)
				{
					length = int.MinValue;
				}
				else
				{
					length = (int)num;
				}
				start = textLength;
			}
			this.SelectInternal(start, length, textLength);
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x00101238 File Offset: 0x000FF438
		internal virtual void SelectInternal(int start, int length, int textLen)
		{
			if (base.IsHandleCreated)
			{
				int wparam;
				int lparam;
				this.AdjustSelectionStartAndEnd(start, length, out wparam, out lparam, textLen);
				base.SendMessage(177, wparam, lparam);
				return;
			}
			this.selectionStart = start;
			this.selectionLength = length;
			this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = true;
		}

		/// <summary>Selects all text in the text box.</summary>
		// Token: 0x060039F3 RID: 14835 RVA: 0x00101288 File Offset: 0x000FF488
		public void SelectAll()
		{
			int textLength = this.TextLength;
			this.SelectInternal(0, textLength, textLength);
		}

		/// <summary>Sets the specified bounds of the <see cref="T:System.Windows.Forms.TextBoxBase" /> control.</summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left" /> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top" /> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width" /> property value of the control.</param>
		/// <param name="height">Not used.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified" /> values.</param>
		// Token: 0x060039F4 RID: 14836 RVA: 0x001012A8 File Offset: 0x000FF4A8
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (!this.integralHeightAdjust && height != base.Height)
			{
				this.requestedHeight = height;
			}
			if (this.textBoxFlags[TextBoxBase.autoSize] && !this.textBoxFlags[TextBoxBase.multiline])
			{
				height = this.PreferredHeight;
			}
			base.SetBoundsCore(x, y, width, height, specified);
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x00101308 File Offset: 0x000FF508
		private static void Swap(ref int n1, ref int n2)
		{
			int num = n2;
			n2 = n1;
			n1 = num;
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x00101320 File Offset: 0x000FF520
		internal void AdjustSelectionStartAndEnd(int selStart, int selLength, out int start, out int end, int textLen)
		{
			start = selStart;
			end = 0;
			if (start <= -1)
			{
				start = -1;
				return;
			}
			int num;
			if (textLen >= 0)
			{
				num = textLen;
			}
			else
			{
				num = this.TextLength;
			}
			if (start > num)
			{
				start = num;
			}
			try
			{
				end = checked(start + selLength);
			}
			catch (OverflowException)
			{
				end = ((start > 0) ? int.MaxValue : int.MinValue);
			}
			if (end < 0)
			{
				end = 0;
			}
			else if (end > num)
			{
				end = num;
			}
			if (this.SelectionUsesDbcsOffsetsInWin9x && Marshal.SystemDefaultCharSize == 1)
			{
				TextBoxBase.ToDbcsOffsets(this.WindowText, ref start, ref end);
			}
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x001013BC File Offset: 0x000FF5BC
		internal void SetSelectionOnHandle()
		{
			if (this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated])
			{
				this.textBoxFlags[TextBoxBase.setSelectionOnHandleCreated] = false;
				int wparam;
				int lparam;
				this.AdjustSelectionStartAndEnd(this.selectionStart, this.selectionLength, out wparam, out lparam, -1);
				base.SendMessage(177, wparam, lparam);
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00101414 File Offset: 0x000FF614
		private static void ToUnicodeOffsets(string str, ref int start, ref int end)
		{
			Encoding @default = Encoding.Default;
			byte[] bytes = @default.GetBytes(str);
			bool flag = start > end;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
			if (start < 0)
			{
				start = 0;
			}
			if (start > bytes.Length)
			{
				start = bytes.Length;
			}
			if (end > bytes.Length)
			{
				end = bytes.Length;
			}
			int num = (start == 0) ? 0 : @default.GetCharCount(bytes, 0, start);
			end = num + @default.GetCharCount(bytes, start, end - start);
			start = num;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00101494 File Offset: 0x000FF694
		internal static void ToDbcsOffsets(string str, ref int start, ref int end)
		{
			Encoding @default = Encoding.Default;
			bool flag = start > end;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
			if (start < 0)
			{
				start = 0;
			}
			if (start > str.Length)
			{
				start = str.Length;
			}
			if (end < start)
			{
				end = start;
			}
			if (end > str.Length)
			{
				end = str.Length;
			}
			int num = (start == 0) ? 0 : @default.GetByteCount(str.Substring(0, start));
			end = num + @default.GetByteCount(str.Substring(start, end - start));
			start = num;
			if (flag)
			{
				TextBoxBase.Swap(ref start, ref end);
			}
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.TextBoxBase" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.TextBoxBase" />. The string includes the type and the <see cref="T:System.Windows.Forms.TextBoxBase" /> property of the control.</returns>
		// Token: 0x060039FA RID: 14842 RVA: 0x0010152C File Offset: 0x000FF72C
		public override string ToString()
		{
			string str = base.ToString();
			string text = this.Text;
			if (text.Length > 40)
			{
				text = text.Substring(0, 40) + "...";
			}
			return str + ", Text: " + text.ToString();
		}

		/// <summary>Undoes the last edit operation in the text box.</summary>
		// Token: 0x060039FB RID: 14843 RVA: 0x00101576 File Offset: 0x000FF776
		public void Undo()
		{
			base.SendMessage(199, 0, 0);
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00101586 File Offset: 0x000FF786
		internal virtual void UpdateMaxLength()
		{
			if (base.IsHandleCreated)
			{
				base.SendMessage(197, this.maxLength, 0);
			}
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x001015A3 File Offset: 0x000FF7A3
		internal override IntPtr InitializeDCForWmCtlColor(IntPtr dc, int msg)
		{
			if (msg == 312 && !this.ShouldSerializeBackColor())
			{
				return IntPtr.Zero;
			}
			return base.InitializeDCForWmCtlColor(dc, msg);
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x001015C4 File Offset: 0x000FF7C4
		private void WmReflectCommand(ref Message m)
		{
			if (!this.textBoxFlags[TextBoxBase.codeUpdateText] && !this.textBoxFlags[TextBoxBase.creatingHandle])
			{
				if (NativeMethods.Util.HIWORD(m.WParam) == 768 && this.CanRaiseTextChangedEvent)
				{
					this.OnTextChanged(EventArgs.Empty);
					return;
				}
				if (NativeMethods.Util.HIWORD(m.WParam) == 1024)
				{
					bool flag = this.Modified;
				}
			}
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x00101634 File Offset: 0x000FF834
		private void WmSetFont(ref Message m)
		{
			base.WndProc(ref m);
			if (!this.textBoxFlags[TextBoxBase.multiline])
			{
				base.SendMessage(211, 3, 0);
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00101660 File Offset: 0x000FF860
		private void WmGetDlgCode(ref Message m)
		{
			base.WndProc(ref m);
			if (this.AcceptsTab)
			{
				m.Result = (IntPtr)((int)((long)m.Result) | 2);
				return;
			}
			m.Result = (IntPtr)((int)((long)m.Result) & -7);
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x001016B0 File Offset: 0x000FF8B0
		private void WmTextBoxContextMenu(ref Message m)
		{
			if (this.ContextMenu != null || this.ContextMenuStrip != null)
			{
				int x = NativeMethods.Util.SignedLOWORD(m.LParam);
				int y = NativeMethods.Util.SignedHIWORD(m.LParam);
				bool isKeyboardActivated = false;
				Point point;
				if ((int)((long)m.LParam) == -1)
				{
					isKeyboardActivated = true;
					point = new Point(base.Width / 2, base.Height / 2);
				}
				else
				{
					point = base.PointToClientInternal(new Point(x, y));
				}
				if (base.ClientRectangle.Contains(point))
				{
					if (this.ContextMenu != null)
					{
						this.ContextMenu.Show(this, point);
						return;
					}
					if (this.ContextMenuStrip != null)
					{
						this.ContextMenuStrip.ShowInternal(this, point, isKeyboardActivated);
						return;
					}
					this.DefWndProc(ref m);
				}
			}
		}

		/// <summary>Processes Windows messages.</summary>
		/// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
		// Token: 0x06003A02 RID: 14850 RVA: 0x00101768 File Offset: 0x000FF968
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg <= 123)
			{
				if (msg == 48)
				{
					this.WmSetFont(ref m);
					return;
				}
				if (msg == 123)
				{
					if (this.ShortcutsEnabled)
					{
						base.WndProc(ref m);
						return;
					}
					this.WmTextBoxContextMenu(ref m);
					return;
				}
			}
			else
			{
				if (msg == 135)
				{
					this.WmGetDlgCode(ref m);
					return;
				}
				if (msg == 515)
				{
					this.doubleClickFired = true;
					base.WndProc(ref m);
					return;
				}
				if (msg == 8465)
				{
					this.WmReflectCommand(ref m);
					return;
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x04002293 RID: 8851
		private static readonly int autoSize = BitVector32.CreateMask();

		// Token: 0x04002294 RID: 8852
		private static readonly int hideSelection = BitVector32.CreateMask(TextBoxBase.autoSize);

		// Token: 0x04002295 RID: 8853
		private static readonly int multiline = BitVector32.CreateMask(TextBoxBase.hideSelection);

		// Token: 0x04002296 RID: 8854
		private static readonly int modified = BitVector32.CreateMask(TextBoxBase.multiline);

		// Token: 0x04002297 RID: 8855
		private static readonly int readOnly = BitVector32.CreateMask(TextBoxBase.modified);

		// Token: 0x04002298 RID: 8856
		private static readonly int acceptsTab = BitVector32.CreateMask(TextBoxBase.readOnly);

		// Token: 0x04002299 RID: 8857
		private static readonly int wordWrap = BitVector32.CreateMask(TextBoxBase.acceptsTab);

		// Token: 0x0400229A RID: 8858
		private static readonly int creatingHandle = BitVector32.CreateMask(TextBoxBase.wordWrap);

		// Token: 0x0400229B RID: 8859
		private static readonly int codeUpdateText = BitVector32.CreateMask(TextBoxBase.creatingHandle);

		// Token: 0x0400229C RID: 8860
		private static readonly int shortcutsEnabled = BitVector32.CreateMask(TextBoxBase.codeUpdateText);

		// Token: 0x0400229D RID: 8861
		private static readonly int scrollToCaretOnHandleCreated = BitVector32.CreateMask(TextBoxBase.shortcutsEnabled);

		// Token: 0x0400229E RID: 8862
		private static readonly int setSelectionOnHandleCreated = BitVector32.CreateMask(TextBoxBase.scrollToCaretOnHandleCreated);

		// Token: 0x0400229F RID: 8863
		private static readonly object EVENT_ACCEPTSTABCHANGED = new object();

		// Token: 0x040022A0 RID: 8864
		private static readonly object EVENT_BORDERSTYLECHANGED = new object();

		// Token: 0x040022A1 RID: 8865
		private static readonly object EVENT_HIDESELECTIONCHANGED = new object();

		// Token: 0x040022A2 RID: 8866
		private static readonly object EVENT_MODIFIEDCHANGED = new object();

		// Token: 0x040022A3 RID: 8867
		private static readonly object EVENT_MULTILINECHANGED = new object();

		// Token: 0x040022A4 RID: 8868
		private static readonly object EVENT_READONLYCHANGED = new object();

		// Token: 0x040022A5 RID: 8869
		private BorderStyle borderStyle = BorderStyle.Fixed3D;

		// Token: 0x040022A6 RID: 8870
		private int maxLength = 32767;

		// Token: 0x040022A7 RID: 8871
		private int requestedHeight;

		// Token: 0x040022A8 RID: 8872
		private bool integralHeightAdjust;

		// Token: 0x040022A9 RID: 8873
		private int selectionStart;

		// Token: 0x040022AA RID: 8874
		private int selectionLength;

		// Token: 0x040022AB RID: 8875
		private bool doubleClickFired;

		// Token: 0x040022AC RID: 8876
		private static int[] shortcutsToDisable;

		// Token: 0x040022AD RID: 8877
		private BitVector32 textBoxFlags;
	}
}

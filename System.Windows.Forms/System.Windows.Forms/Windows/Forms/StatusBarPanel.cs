using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Represents a panel in a <see cref="T:System.Windows.Forms.StatusBar" /> control. Although the <see cref="T:System.Windows.Forms.StatusStrip" /> control replaces and adds functionality to the <see cref="T:System.Windows.Forms.StatusBar" /> control of previous versions, <see cref="T:System.Windows.Forms.StatusBar" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x02000366 RID: 870
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("Text")]
	public class StatusBarPanel : Component, ISupportInitialize
	{
		/// <summary>Gets or sets the alignment of text and icons within the status bar panel.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> values. The default is <see cref="F:System.Windows.Forms.HorizontalAlignment.Left" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a member of the <see cref="T:System.Windows.Forms.HorizontalAlignment" /> enumeration. </exception>
		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x060036C3 RID: 14019 RVA: 0x000F8589 File Offset: 0x000F6789
		// (set) Token: 0x060036C4 RID: 14020 RVA: 0x000F8591 File Offset: 0x000F6791
		[SRCategory("CatAppearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		[Localizable(true)]
		[SRDescription("StatusBarPanelAlignmentDescr")]
		public HorizontalAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					this.Realize();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether the status bar panel is automatically resized.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.StatusBarPanelAutoSize" /> values. The default is <see cref="F:System.Windows.Forms.StatusBarPanelAutoSize.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a member of the <see cref="T:System.Windows.Forms.StatusBarPanelAutoSize" /> enumeration. </exception>
		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x060036C5 RID: 14021 RVA: 0x000F85CF File Offset: 0x000F67CF
		// (set) Token: 0x060036C6 RID: 14022 RVA: 0x000F85D7 File Offset: 0x000F67D7
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelAutoSize.None)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("StatusBarPanelAutoSizeDescr")]
		public StatusBarPanelAutoSize AutoSize
		{
			get
			{
				return this.autoSize;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelAutoSize));
				}
				if (this.autoSize != value)
				{
					this.autoSize = value;
					this.UpdateSize();
				}
			}
		}

		/// <summary>Gets or sets the border style of the status bar panel.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.StatusBarPanelBorderStyle" /> values. The default is <see cref="F:System.Windows.Forms.StatusBarPanelBorderStyle.Sunken" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a member of the <see cref="T:System.Windows.Forms.StatusBarPanelBorderStyle" /> enumeration. </exception>
		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x060036C7 RID: 14023 RVA: 0x000F8615 File Offset: 0x000F6815
		// (set) Token: 0x060036C8 RID: 14024 RVA: 0x000F8620 File Offset: 0x000F6820
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelBorderStyle.Sunken)]
		[DispId(-504)]
		[SRDescription("StatusBarPanelBorderStyleDescr")]
		public StatusBarPanelBorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelBorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					this.Realize();
					if (this.Created)
					{
						this.parent.Invalidate();
					}
				}
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060036C9 RID: 14025 RVA: 0x000F867C File Offset: 0x000F687C
		internal bool Created
		{
			get
			{
				return this.parent != null && this.parent.ArePanelsRealized();
			}
		}

		/// <summary>Gets or sets the icon to display within the status bar panel.</summary>
		/// <returns>An <see cref="T:System.Drawing.Icon" /> that represents the icon to display in the panel.</returns>
		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x060036CA RID: 14026 RVA: 0x000F8693 File Offset: 0x000F6893
		// (set) Token: 0x060036CB RID: 14027 RVA: 0x000F869C File Offset: 0x000F689C
		[SRCategory("CatAppearance")]
		[DefaultValue(null)]
		[Localizable(true)]
		[SRDescription("StatusBarPanelIconDescr")]
		public Icon Icon
		{
			get
			{
				return this.icon;
			}
			set
			{
				if (value != null && (value.Height > SystemInformation.SmallIconSize.Height || value.Width > SystemInformation.SmallIconSize.Width))
				{
					this.icon = new Icon(value, SystemInformation.SmallIconSize);
				}
				else
				{
					this.icon = value;
				}
				if (this.Created)
				{
					IntPtr lparam = (this.icon == null) ? IntPtr.Zero : this.icon.Handle;
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), lparam);
				}
				this.UpdateSize();
				if (this.Created)
				{
					this.parent.Invalidate();
				}
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x060036CC RID: 14028 RVA: 0x000F8749 File Offset: 0x000F6949
		// (set) Token: 0x060036CD RID: 14029 RVA: 0x000F8751 File Offset: 0x000F6951
		internal int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		/// <summary>Gets or sets the minimum allowed width of the status bar panel within the <see cref="T:System.Windows.Forms.StatusBar" /> control.</summary>
		/// <returns>The minimum width, in pixels, of the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</returns>
		/// <exception cref="T:System.ArgumentException">A value less than 0 is assigned to the property. </exception>
		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x000F875A File Offset: 0x000F695A
		// (set) Token: 0x060036CF RID: 14031 RVA: 0x000F8764 File Offset: 0x000F6964
		[SRCategory("CatBehavior")]
		[DefaultValue(10)]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("StatusBarPanelMinWidthDescr")]
		public int MinWidth
		{
			get
			{
				return this.minWidth;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("MinWidth", SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"MinWidth",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.minWidth)
				{
					this.minWidth = value;
					this.UpdateSize();
					if (this.minWidth > this.Width)
					{
						this.Width = value;
					}
				}
			}
		}

		/// <summary>Gets or sets the name to apply to the <see cref="T:System.Windows.Forms.StatusBarPanel" />. </summary>
		/// <returns>The name of the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</returns>
		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060036D0 RID: 14032 RVA: 0x000F87E3 File Offset: 0x000F69E3
		// (set) Token: 0x060036D1 RID: 14033 RVA: 0x000F87F1 File Offset: 0x000F69F1
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[SRDescription("StatusBarPanelNameDescr")]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				this.name = value;
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.StatusBar" /> control that hosts the status bar panel.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.StatusBar" /> that contains the panel.</returns>
		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x000F8813 File Offset: 0x000F6A13
		[Browsable(false)]
		public StatusBar Parent
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (set) Token: 0x060036D3 RID: 14035 RVA: 0x000F881B File Offset: 0x000F6A1B
		internal StatusBar ParentInternal
		{
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060036D4 RID: 14036 RVA: 0x000F8824 File Offset: 0x000F6A24
		// (set) Token: 0x060036D5 RID: 14037 RVA: 0x000F882C File Offset: 0x000F6A2C
		internal int Right
		{
			get
			{
				return this.right;
			}
			set
			{
				this.right = value;
			}
		}

		/// <summary>Gets or sets the style of the status bar panel.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.StatusBarPanelStyle" /> values. The default is <see cref="F:System.Windows.Forms.StatusBarPanelStyle.Text" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned to the property is not a member of the <see cref="T:System.Windows.Forms.StatusBarPanelStyle" /> enumeration. </exception>
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060036D6 RID: 14038 RVA: 0x000F8835 File Offset: 0x000F6A35
		// (set) Token: 0x060036D7 RID: 14039 RVA: 0x000F8840 File Offset: 0x000F6A40
		[SRCategory("CatAppearance")]
		[DefaultValue(StatusBarPanelStyle.Text)]
		[SRDescription("StatusBarPanelStyleDescr")]
		public StatusBarPanelStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(StatusBarPanelStyle));
				}
				if (this.style != value)
				{
					this.style = value;
					this.Realize();
					if (this.Created)
					{
						this.parent.Invalidate();
					}
				}
			}
		}

		/// <summary>Gets or sets an object that contains data about the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> that contains data about the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</returns>
		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060036D8 RID: 14040 RVA: 0x000F889C File Offset: 0x000F6A9C
		// (set) Token: 0x060036D9 RID: 14041 RVA: 0x000F88A4 File Offset: 0x000F6AA4
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

		/// <summary>Gets or sets the text of the status bar panel.</summary>
		/// <returns>The text displayed in the panel.</returns>
		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x060036DA RID: 14042 RVA: 0x000F88AD File Offset: 0x000F6AAD
		// (set) Token: 0x060036DB RID: 14043 RVA: 0x000F88C3 File Offset: 0x000F6AC3
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("StatusBarPanelTextDescr")]
		public string Text
		{
			get
			{
				if (this.text == null)
				{
					return "";
				}
				return this.text;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.Text.Equals(value))
				{
					if (value.Length == 0)
					{
						this.text = null;
					}
					else
					{
						this.text = value;
					}
					this.Realize();
					this.UpdateSize();
				}
			}
		}

		/// <summary>Gets or sets ToolTip text associated with the status bar panel.</summary>
		/// <returns>The ToolTip text for the panel.</returns>
		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x060036DC RID: 14044 RVA: 0x000F8901 File Offset: 0x000F6B01
		// (set) Token: 0x060036DD RID: 14045 RVA: 0x000F8918 File Offset: 0x000F6B18
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("StatusBarPanelToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				if (this.toolTipText == null)
				{
					return "";
				}
				return this.toolTipText;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (!this.ToolTipText.Equals(value))
				{
					if (value.Length == 0)
					{
						this.toolTipText = null;
					}
					else
					{
						this.toolTipText = value;
					}
					if (this.Created)
					{
						this.parent.UpdateTooltip(this);
					}
				}
			}
		}

		/// <summary>Gets or sets the width of the status bar panel within the <see cref="T:System.Windows.Forms.StatusBar" /> control.</summary>
		/// <returns>The width, in pixels, of the <see cref="T:System.Windows.Forms.StatusBarPanel" />.</returns>
		/// <exception cref="T:System.ArgumentException">The width specified is less than the value of the <see cref="P:System.Windows.Forms.StatusBarPanel.MinWidth" /> property. </exception>
		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x060036DE RID: 14046 RVA: 0x000F8969 File Offset: 0x000F6B69
		// (set) Token: 0x060036DF RID: 14047 RVA: 0x000F8971 File Offset: 0x000F6B71
		[Localizable(true)]
		[SRCategory("CatAppearance")]
		[DefaultValue(100)]
		[SRDescription("StatusBarPanelWidthDescr")]
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (!this.initializing && value < this.minWidth)
				{
					throw new ArgumentOutOfRangeException("Width", SR.GetString("WidthGreaterThanMinWidth"));
				}
				this.width = value;
				this.UpdateSize();
			}
		}

		/// <summary>Begins the initialization of a <see cref="T:System.Windows.Forms.StatusBarPanel" />.</summary>
		// Token: 0x060036E0 RID: 14048 RVA: 0x000F89A6 File Offset: 0x000F6BA6
		public void BeginInit()
		{
			this.initializing = true;
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.StatusBarPanel" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060036E1 RID: 14049 RVA: 0x000F89B0 File Offset: 0x000F6BB0
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.parent != null)
			{
				int num = this.GetIndex();
				if (num != -1)
				{
					this.parent.Panels.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Ends the initialization of a <see cref="T:System.Windows.Forms.StatusBarPanel" />.</summary>
		// Token: 0x060036E2 RID: 14050 RVA: 0x000F89EB File Offset: 0x000F6BEB
		public void EndInit()
		{
			this.initializing = false;
			if (this.Width < this.MinWidth)
			{
				this.Width = this.MinWidth;
			}
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x000F8A10 File Offset: 0x000F6C10
		internal int GetContentsWidth(bool newPanel)
		{
			string text;
			if (newPanel)
			{
				if (this.text == null)
				{
					text = "";
				}
				else
				{
					text = this.text;
				}
			}
			else
			{
				text = this.Text;
			}
			Graphics graphics = this.parent.CreateGraphicsInternal();
			Size size = Size.Ceiling(graphics.MeasureString(text, this.parent.Font));
			if (this.icon != null)
			{
				size.Width += this.icon.Size.Width + 5;
			}
			graphics.Dispose();
			int val = size.Width + SystemInformation.BorderSize.Width * 2 + 6 + 2;
			return Math.Max(val, this.minWidth);
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000F8749 File Offset: 0x000F6949
		private int GetIndex()
		{
			return this.index;
		}

		// Token: 0x060036E5 RID: 14053 RVA: 0x000F8AC0 File Offset: 0x000F6CC0
		internal void Realize()
		{
			if (this.Created)
			{
				int num = 0;
				string text;
				if (this.text == null)
				{
					text = "";
				}
				else
				{
					text = this.text;
				}
				HorizontalAlignment horizontalAlignment = this.alignment;
				if (this.parent.RightToLeft == RightToLeft.Yes)
				{
					if (horizontalAlignment != HorizontalAlignment.Left)
					{
						if (horizontalAlignment == HorizontalAlignment.Right)
						{
							horizontalAlignment = HorizontalAlignment.Left;
						}
					}
					else
					{
						horizontalAlignment = HorizontalAlignment.Right;
					}
				}
				string lParam;
				if (horizontalAlignment != HorizontalAlignment.Right)
				{
					if (horizontalAlignment == HorizontalAlignment.Center)
					{
						lParam = "\t" + text;
					}
					else
					{
						lParam = text;
					}
				}
				else
				{
					lParam = "\t\t" + text;
				}
				switch (this.borderStyle)
				{
				case StatusBarPanelBorderStyle.None:
					num |= 256;
					break;
				case StatusBarPanelBorderStyle.Raised:
					num |= 512;
					break;
				}
				StatusBarPanelStyle statusBarPanelStyle = this.style;
				if (statusBarPanelStyle != StatusBarPanelStyle.Text && statusBarPanelStyle == StatusBarPanelStyle.OwnerDraw)
				{
					num |= 4096;
				}
				int num2 = this.GetIndex() | num;
				if (this.parent.RightToLeft == RightToLeft.Yes)
				{
					num2 |= 1024;
				}
				if ((int)UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), NativeMethods.SB_SETTEXT, (IntPtr)num2, lParam) == 0)
				{
					throw new InvalidOperationException(SR.GetString("UnableToSetPanelText"));
				}
				if (this.icon != null && this.style != StatusBarPanelStyle.OwnerDraw)
				{
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), this.icon.Handle);
				}
				else
				{
					this.parent.SendMessage(1039, (IntPtr)this.GetIndex(), IntPtr.Zero);
				}
				if (this.style == StatusBarPanelStyle.OwnerDraw)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					int num3 = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), 1034, (IntPtr)this.GetIndex(), ref rect);
					if (num3 != 0)
					{
						this.parent.Invalidate(Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom));
					}
				}
			}
		}

		// Token: 0x060036E6 RID: 14054 RVA: 0x000F8CBB File Offset: 0x000F6EBB
		private void UpdateSize()
		{
			if (this.autoSize == StatusBarPanelAutoSize.Contents)
			{
				this.ApplyContentSizing();
				return;
			}
			if (this.Created)
			{
				this.parent.DirtyLayout();
				this.parent.PerformLayout();
			}
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x000F8CEC File Offset: 0x000F6EEC
		private void ApplyContentSizing()
		{
			if (this.autoSize == StatusBarPanelAutoSize.Contents && this.parent != null)
			{
				int contentsWidth = this.GetContentsWidth(false);
				if (contentsWidth != this.Width)
				{
					this.Width = contentsWidth;
					if (this.Created)
					{
						this.parent.DirtyLayout();
						this.parent.PerformLayout();
					}
				}
			}
		}

		/// <summary>Retrieves a string that contains information about the panel.</summary>
		/// <returns>Returns a string that contains the class name for the control and the text it contains.</returns>
		// Token: 0x060036E8 RID: 14056 RVA: 0x000F8D40 File Offset: 0x000F6F40
		public override string ToString()
		{
			return "StatusBarPanel: {" + this.Text + "}";
		}

		// Token: 0x040021C5 RID: 8645
		private const int DEFAULTWIDTH = 100;

		// Token: 0x040021C6 RID: 8646
		private const int DEFAULTMINWIDTH = 10;

		// Token: 0x040021C7 RID: 8647
		private const int PANELTEXTINSET = 3;

		// Token: 0x040021C8 RID: 8648
		private const int PANELGAP = 2;

		// Token: 0x040021C9 RID: 8649
		private string text = "";

		// Token: 0x040021CA RID: 8650
		private string name = "";

		// Token: 0x040021CB RID: 8651
		private string toolTipText = "";

		// Token: 0x040021CC RID: 8652
		private Icon icon;

		// Token: 0x040021CD RID: 8653
		private HorizontalAlignment alignment;

		// Token: 0x040021CE RID: 8654
		private StatusBarPanelBorderStyle borderStyle = StatusBarPanelBorderStyle.Sunken;

		// Token: 0x040021CF RID: 8655
		private StatusBarPanelStyle style = StatusBarPanelStyle.Text;

		// Token: 0x040021D0 RID: 8656
		private StatusBar parent;

		// Token: 0x040021D1 RID: 8657
		private int width = 100;

		// Token: 0x040021D2 RID: 8658
		private int right;

		// Token: 0x040021D3 RID: 8659
		private int minWidth = 10;

		// Token: 0x040021D4 RID: 8660
		private int index;

		// Token: 0x040021D5 RID: 8661
		private StatusBarPanelAutoSize autoSize = StatusBarPanelAutoSize.None;

		// Token: 0x040021D6 RID: 8662
		private bool initializing;

		// Token: 0x040021D7 RID: 8663
		private object userData;
	}
}

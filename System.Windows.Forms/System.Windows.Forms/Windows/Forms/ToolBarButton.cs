using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows toolbar button. Although <see cref="T:System.Windows.Forms.ToolStripButton" /> replaces and extends the <see cref="T:System.Windows.Forms.ToolBarButton" /> control of previous versions, <see cref="T:System.Windows.Forms.ToolBarButton" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x0200039A RID: 922
	[Designer("System.Windows.Forms.Design.ToolBarButtonDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	public class ToolBarButton : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolBarButton" /> class.</summary>
		// Token: 0x06003A8D RID: 14989 RVA: 0x00104159 File Offset: 0x00102359
		public ToolBarButton()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolBarButton" /> class and displays the assigned text on the button.</summary>
		/// <param name="text">The text to display on the new <see cref="T:System.Windows.Forms.ToolBarButton" />. </param>
		// Token: 0x06003A8E RID: 14990 RVA: 0x00104189 File Offset: 0x00102389
		public ToolBarButton(string text)
		{
			this.Text = text;
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06003A8F RID: 14991 RVA: 0x001041C0 File Offset: 0x001023C0
		internal ToolBarButton.ToolBarButtonImageIndexer ImageIndexer
		{
			get
			{
				if (this.imageIndexer == null)
				{
					this.imageIndexer = new ToolBarButton.ToolBarButtonImageIndexer(this);
				}
				return this.imageIndexer;
			}
		}

		/// <summary>Gets or sets the menu to be displayed in the drop-down toolbar button.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenu" /> to be displayed in the drop-down toolbar button. The default is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned object is not a <see cref="T:System.Windows.Forms.ContextMenu" />. </exception>
		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x001041DC File Offset: 0x001023DC
		// (set) Token: 0x06003A91 RID: 14993 RVA: 0x001041E4 File Offset: 0x001023E4
		[DefaultValue(null)]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRDescription("ToolBarButtonMenuDescr")]
		public Menu DropDownMenu
		{
			get
			{
				return this.dropDownMenu;
			}
			set
			{
				if (value != null && !(value is ContextMenu))
				{
					throw new ArgumentException(SR.GetString("ToolBarButtonInvalidDropDownMenuType"));
				}
				this.dropDownMenu = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the button is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the button is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x00104208 File Offset: 0x00102408
		// (set) Token: 0x06003A93 RID: 14995 RVA: 0x00104210 File Offset: 0x00102410
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonEnabledDescr")]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled != value)
				{
					this.enabled = value;
					if (this.parent != null && this.parent.IsHandleCreated)
					{
						this.parent.SendMessage(1025, this.FindButtonIndex(), this.enabled ? 1 : 0);
					}
				}
			}
		}

		/// <summary>Gets or sets the index value of the image assigned to the button.</summary>
		/// <returns>The index value of the <see cref="T:System.Drawing.Image" /> assigned to the toolbar button. The default is -1.</returns>
		/// <exception cref="T:System.ArgumentException">The assigned value is less than -1. </exception>
		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x00104265 File Offset: 0x00102465
		// (set) Token: 0x06003A95 RID: 14997 RVA: 0x00104274 File Offset: 0x00102474
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue(-1)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonImageIndexDescr")]
		public int ImageIndex
		{
			get
			{
				return this.ImageIndexer.Index;
			}
			set
			{
				if (this.ImageIndexer.Index != value)
				{
					if (value < -1)
					{
						throw new ArgumentOutOfRangeException("ImageIndex", SR.GetString("InvalidLowBoundArgumentEx", new object[]
						{
							"ImageIndex",
							value.ToString(CultureInfo.CurrentCulture),
							-1
						}));
					}
					this.ImageIndexer.Index = value;
					this.UpdateButton(false);
				}
			}
		}

		/// <summary>Gets or sets the name of the image assigned to the button.</summary>
		/// <returns>The name of the <see cref="T:System.Drawing.Image" /> assigned to the toolbar button. </returns>
		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06003A96 RID: 14998 RVA: 0x001042E1 File Offset: 0x001024E1
		// (set) Token: 0x06003A97 RID: 14999 RVA: 0x001042EE File Offset: 0x001024EE
		[TypeConverter(typeof(ImageKeyConverter))]
		[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRDescription("ToolBarButtonImageIndexDescr")]
		public string ImageKey
		{
			get
			{
				return this.ImageIndexer.Key;
			}
			set
			{
				if (this.ImageIndexer.Key != value)
				{
					this.ImageIndexer.Key = value;
					this.UpdateButton(false);
				}
			}
		}

		/// <summary>The name of the button.</summary>
		/// <returns>The name of the button.</returns>
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06003A98 RID: 15000 RVA: 0x00104316 File Offset: 0x00102516
		// (set) Token: 0x06003A99 RID: 15001 RVA: 0x00104324 File Offset: 0x00102524
		[Browsable(false)]
		public string Name
		{
			get
			{
				return WindowsFormsUtils.GetComponentName(this, this.name);
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.name = null;
				}
				else
				{
					this.name = value;
				}
				if (this.Site != null)
				{
					this.Site.Name = this.name;
				}
			}
		}

		/// <summary>Gets the toolbar control that the toolbar button is assigned to.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolBar" /> control that the <see cref="T:System.Windows.Forms.ToolBarButton" /> is assigned to.</returns>
		// Token: 0x17000EA3 RID: 3747
		// (get) Token: 0x06003A9A RID: 15002 RVA: 0x0010435A File Offset: 0x0010255A
		[Browsable(false)]
		public ToolBar Parent
		{
			get
			{
				return this.parent;
			}
		}

		/// <summary>Gets or sets a value indicating whether a toggle-style toolbar button is partially pushed.</summary>
		/// <returns>
		///     <see langword="true" /> if a toggle-style toolbar button is partially pushed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000EA4 RID: 3748
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x00104364 File Offset: 0x00102564
		// (set) Token: 0x06003A9C RID: 15004 RVA: 0x001043C1 File Offset: 0x001025C1
		[DefaultValue(false)]
		[SRDescription("ToolBarButtonPartialPushDescr")]
		public bool PartialPush
		{
			get
			{
				if (this.parent == null || !this.parent.IsHandleCreated)
				{
					return this.partialPush;
				}
				if ((int)this.parent.SendMessage(1037, this.FindButtonIndex(), 0) != 0)
				{
					this.partialPush = true;
				}
				else
				{
					this.partialPush = false;
				}
				return this.partialPush;
			}
			set
			{
				if (this.partialPush != value)
				{
					this.partialPush = value;
					this.UpdateButton(false);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a toggle-style toolbar button is currently in the pushed state.</summary>
		/// <returns>
		///     <see langword="true" /> if a toggle-style toolbar button is currently in the pushed state; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000EA5 RID: 3749
		// (get) Token: 0x06003A9D RID: 15005 RVA: 0x001043DA File Offset: 0x001025DA
		// (set) Token: 0x06003A9E RID: 15006 RVA: 0x001043FE File Offset: 0x001025FE
		[DefaultValue(false)]
		[SRDescription("ToolBarButtonPushedDescr")]
		public bool Pushed
		{
			get
			{
				if (this.parent == null || !this.parent.IsHandleCreated)
				{
					return this.pushed;
				}
				return this.GetPushedState();
			}
			set
			{
				if (value != this.Pushed)
				{
					this.pushed = value;
					this.UpdateButton(false, false, false);
				}
			}
		}

		/// <summary>Gets the bounding rectangle for a toolbar button.</summary>
		/// <returns>The bounding <see cref="T:System.Drawing.Rectangle" /> for a toolbar button.</returns>
		// Token: 0x17000EA6 RID: 3750
		// (get) Token: 0x06003A9F RID: 15007 RVA: 0x0010441C File Offset: 0x0010261C
		public Rectangle Rectangle
		{
			get
			{
				if (this.parent != null)
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					UnsafeNativeMethods.SendMessage(new HandleRef(this.parent, this.parent.Handle), 1075, this.FindButtonIndex(), ref rect);
					return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
				}
				return Rectangle.Empty;
			}
		}

		/// <summary>Gets or sets the style of the toolbar button.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolBarButtonStyle" /> values. The default is <see langword="ToolBarButtonStyle.PushButton" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ToolBarButtonStyle" /> values. </exception>
		// Token: 0x17000EA7 RID: 3751
		// (get) Token: 0x06003AA0 RID: 15008 RVA: 0x00104485 File Offset: 0x00102685
		// (set) Token: 0x06003AA1 RID: 15009 RVA: 0x0010448D File Offset: 0x0010268D
		[DefaultValue(ToolBarButtonStyle.PushButton)]
		[SRDescription("ToolBarButtonStyleDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public ToolBarButtonStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 1, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ToolBarButtonStyle));
				}
				if (this.style == value)
				{
					return;
				}
				this.style = value;
				this.UpdateButton(true);
			}
		}

		/// <summary>Gets or sets the object that contains data about the toolbar button.</summary>
		/// <returns>An <see cref="T:System.Object" /> that contains data about the toolbar button. The default is <see langword="null" />.</returns>
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06003AA2 RID: 15010 RVA: 0x001044CD File Offset: 0x001026CD
		// (set) Token: 0x06003AA3 RID: 15011 RVA: 0x001044D5 File Offset: 0x001026D5
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

		/// <summary>Gets or sets the text displayed on the toolbar button.</summary>
		/// <returns>The text displayed on the toolbar button. The default is an empty string ("").</returns>
		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06003AA4 RID: 15012 RVA: 0x001044DE File Offset: 0x001026DE
		// (set) Token: 0x06003AA5 RID: 15013 RVA: 0x001044F4 File Offset: 0x001026F4
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("ToolBarButtonTextDescr")]
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				return "";
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = null;
				}
				if ((value == null && this.text != null) || (value != null && (this.text == null || !this.text.Equals(value))))
				{
					this.text = value;
					this.UpdateButton(WindowsFormsUtils.ContainsMnemonic(this.text), true, true);
				}
			}
		}

		/// <summary>Gets or sets the text that appears as a ToolTip for the button.</summary>
		/// <returns>The text that is displayed when the mouse pointer moves over the toolbar button. The default is an empty string ("").</returns>
		// Token: 0x17000EAA RID: 3754
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x0010454A File Offset: 0x0010274A
		// (set) Token: 0x06003AA7 RID: 15015 RVA: 0x00104560 File Offset: 0x00102760
		[Localizable(true)]
		[DefaultValue("")]
		[SRDescription("ToolBarButtonToolTipTextDescr")]
		public string ToolTipText
		{
			get
			{
				if (this.tooltipText != null)
				{
					return this.tooltipText;
				}
				return "";
			}
			set
			{
				this.tooltipText = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the toolbar button is visible.</summary>
		/// <returns>
		///     <see langword="true" /> if the toolbar button is visible; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000EAB RID: 3755
		// (get) Token: 0x06003AA8 RID: 15016 RVA: 0x00104569 File Offset: 0x00102769
		// (set) Token: 0x06003AA9 RID: 15017 RVA: 0x00104571 File Offset: 0x00102771
		[DefaultValue(true)]
		[Localizable(true)]
		[SRDescription("ToolBarButtonVisibleDescr")]
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
					this.visible = value;
					this.UpdateButton(false);
				}
			}
		}

		// Token: 0x17000EAC RID: 3756
		// (get) Token: 0x06003AAA RID: 15018 RVA: 0x0010458C File Offset: 0x0010278C
		internal short Width
		{
			get
			{
				int num = 0;
				ToolBarButtonStyle toolBarButtonStyle = this.Style;
				Size border3DSize = SystemInformation.Border3DSize;
				if (toolBarButtonStyle != ToolBarButtonStyle.Separator)
				{
					using (Graphics graphics = this.parent.CreateGraphicsInternal())
					{
						Size buttonSize = this.parent.buttonSize;
						if (!buttonSize.IsEmpty)
						{
							num = buttonSize.Width;
							goto IL_14D;
						}
						if (this.parent.ImageList != null || !string.IsNullOrEmpty(this.Text))
						{
							Size imageSize = this.parent.ImageSize;
							Size size = Size.Ceiling(graphics.MeasureString(this.Text, this.parent.Font));
							if (this.parent.TextAlign == ToolBarTextAlign.Right)
							{
								if (size.Width == 0)
								{
									num = imageSize.Width + border3DSize.Width * 4;
								}
								else
								{
									num = imageSize.Width + size.Width + border3DSize.Width * 6;
								}
							}
							else if (imageSize.Width > size.Width)
							{
								num = imageSize.Width + border3DSize.Width * 4;
							}
							else
							{
								num = size.Width + border3DSize.Width * 4;
							}
							if (toolBarButtonStyle == ToolBarButtonStyle.DropDownButton && this.parent.DropDownArrows)
							{
								num += 15;
							}
						}
						else
						{
							num = this.parent.ButtonSize.Width;
						}
						goto IL_14D;
					}
				}
				num = border3DSize.Width * 2;
				IL_14D:
				return (short)num;
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolBarButton" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06003AAB RID: 15019 RVA: 0x00104704 File Offset: 0x00102904
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.parent != null)
			{
				int num = this.FindButtonIndex();
				if (num != -1)
				{
					this.parent.Buttons.RemoveAt(num);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00104740 File Offset: 0x00102940
		private int FindButtonIndex()
		{
			for (int i = 0; i < this.parent.Buttons.Count; i++)
			{
				if (this.parent.Buttons[i] == this)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00104780 File Offset: 0x00102980
		internal int GetButtonWidth()
		{
			int result = this.Parent.ButtonSize.Width;
			NativeMethods.TBBUTTONINFO tbbuttoninfo = default(NativeMethods.TBBUTTONINFO);
			tbbuttoninfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO));
			tbbuttoninfo.dwMask = 64;
			int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this.Parent, this.Parent.Handle), NativeMethods.TB_GETBUTTONINFO, this.commandId, ref tbbuttoninfo);
			if (num != -1)
			{
				result = (int)tbbuttoninfo.cx;
			}
			return result;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00104802 File Offset: 0x00102A02
		private bool GetPushedState()
		{
			if ((int)this.parent.SendMessage(1034, this.FindButtonIndex(), 0) != 0)
			{
				this.pushed = true;
			}
			else
			{
				this.pushed = false;
			}
			return this.pushed;
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00104838 File Offset: 0x00102A38
		internal NativeMethods.TBBUTTON GetTBBUTTON(int commandId)
		{
			NativeMethods.TBBUTTON result = default(NativeMethods.TBBUTTON);
			result.iBitmap = this.ImageIndexer.ActualIndex;
			result.fsState = 0;
			if (this.enabled)
			{
				result.fsState |= 4;
			}
			if (this.partialPush && this.style == ToolBarButtonStyle.ToggleButton)
			{
				result.fsState |= 16;
			}
			if (this.pushed)
			{
				result.fsState |= 1;
			}
			if (!this.visible)
			{
				result.fsState |= 8;
			}
			switch (this.style)
			{
			case ToolBarButtonStyle.PushButton:
				result.fsStyle = 0;
				break;
			case ToolBarButtonStyle.ToggleButton:
				result.fsStyle = 2;
				break;
			case ToolBarButtonStyle.Separator:
				result.fsStyle = 1;
				break;
			case ToolBarButtonStyle.DropDownButton:
				result.fsStyle = 8;
				break;
			}
			result.dwData = (IntPtr)0;
			result.iString = this.stringIndex;
			this.commandId = commandId;
			result.idCommand = commandId;
			return result;
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00104938 File Offset: 0x00102B38
		internal NativeMethods.TBBUTTONINFO GetTBBUTTONINFO(bool updateText, int newCommandId)
		{
			NativeMethods.TBBUTTONINFO result = default(NativeMethods.TBBUTTONINFO);
			result.cbSize = Marshal.SizeOf(typeof(NativeMethods.TBBUTTONINFO));
			result.dwMask = 13;
			if (updateText)
			{
				result.dwMask |= 2;
			}
			result.iImage = this.ImageIndexer.ActualIndex;
			if (newCommandId != this.commandId)
			{
				this.commandId = newCommandId;
				result.idCommand = newCommandId;
				result.dwMask |= 32;
			}
			result.fsState = 0;
			if (this.enabled)
			{
				result.fsState |= 4;
			}
			if (this.partialPush && this.style == ToolBarButtonStyle.ToggleButton)
			{
				result.fsState |= 16;
			}
			if (this.pushed)
			{
				result.fsState |= 1;
			}
			if (!this.visible)
			{
				result.fsState |= 8;
			}
			switch (this.style)
			{
			case ToolBarButtonStyle.PushButton:
				result.fsStyle = 0;
				break;
			case ToolBarButtonStyle.ToggleButton:
				result.fsStyle = 2;
				break;
			case ToolBarButtonStyle.Separator:
				result.fsStyle = 1;
				break;
			}
			if (this.text == null)
			{
				result.pszText = Marshal.StringToHGlobalAuto("\0\0");
			}
			else
			{
				string s = this.text;
				this.PrefixAmpersands(ref s);
				result.pszText = Marshal.StringToHGlobalAuto(s);
			}
			return result;
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00104A8C File Offset: 0x00102C8C
		private void PrefixAmpersands(ref string value)
		{
			if (value == null || value.Length == 0)
			{
				return;
			}
			if (value.IndexOf('&') < 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == '&')
				{
					if (i < value.Length - 1 && value[i + 1] == '&')
					{
						i++;
					}
					stringBuilder.Append("&&");
				}
				else
				{
					stringBuilder.Append(value[i]);
				}
			}
			value = stringBuilder.ToString();
		}

		/// <summary>Returns a string that represents the <see cref="T:System.Windows.Forms.ToolBarButton" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.ToolBarButton" />.</returns>
		// Token: 0x06003AB2 RID: 15026 RVA: 0x00104B1B File Offset: 0x00102D1B
		public override string ToString()
		{
			return "ToolBarButton: " + this.Text + ", Style: " + this.Style.ToString("G");
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00104B47 File Offset: 0x00102D47
		internal void UpdateButton(bool recreate)
		{
			this.UpdateButton(recreate, false, true);
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00104B54 File Offset: 0x00102D54
		private void UpdateButton(bool recreate, bool updateText, bool updatePushedState)
		{
			if (this.style == ToolBarButtonStyle.DropDownButton && this.parent != null && this.parent.DropDownArrows)
			{
				recreate = true;
			}
			if (updatePushedState && this.parent != null && this.parent.IsHandleCreated)
			{
				this.GetPushedState();
			}
			if (this.parent != null)
			{
				int num = this.FindButtonIndex();
				if (num != -1)
				{
					this.parent.InternalSetButton(num, this, recreate, updateText);
				}
			}
		}

		// Token: 0x04002313 RID: 8979
		private string text;

		// Token: 0x04002314 RID: 8980
		private string name;

		// Token: 0x04002315 RID: 8981
		private string tooltipText;

		// Token: 0x04002316 RID: 8982
		private bool enabled = true;

		// Token: 0x04002317 RID: 8983
		private bool visible = true;

		// Token: 0x04002318 RID: 8984
		private bool pushed;

		// Token: 0x04002319 RID: 8985
		private bool partialPush;

		// Token: 0x0400231A RID: 8986
		private int commandId = -1;

		// Token: 0x0400231B RID: 8987
		private ToolBarButton.ToolBarButtonImageIndexer imageIndexer;

		// Token: 0x0400231C RID: 8988
		private ToolBarButtonStyle style = ToolBarButtonStyle.PushButton;

		// Token: 0x0400231D RID: 8989
		private object userData;

		// Token: 0x0400231E RID: 8990
		internal IntPtr stringIndex = (IntPtr)(-1);

		// Token: 0x0400231F RID: 8991
		internal ToolBar parent;

		// Token: 0x04002320 RID: 8992
		internal Menu dropDownMenu;

		// Token: 0x0200072A RID: 1834
		internal class ToolBarButtonImageIndexer : ImageList.Indexer
		{
			// Token: 0x060060CC RID: 24780 RVA: 0x0018C42F File Offset: 0x0018A62F
			public ToolBarButtonImageIndexer(ToolBarButton button)
			{
				this.owner = button;
			}

			// Token: 0x1700171A RID: 5914
			// (get) Token: 0x060060CD RID: 24781 RVA: 0x0018C43E File Offset: 0x0018A63E
			// (set) Token: 0x060060CE RID: 24782 RVA: 0x0000701A File Offset: 0x0000521A
			public override ImageList ImageList
			{
				get
				{
					if (this.owner != null && this.owner.parent != null)
					{
						return this.owner.parent.ImageList;
					}
					return null;
				}
				set
				{
				}
			}

			// Token: 0x04004165 RID: 16741
			private ToolBarButton owner;
		}
	}
}

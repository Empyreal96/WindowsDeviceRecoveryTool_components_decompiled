using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides basic functionality for the <see cref="T:System.Windows.Forms.ContextMenuStrip" /> control. Although <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> and <see cref="T:System.Windows.Forms.ToolStripDropDown" /> replace and add functionality to the <see cref="T:System.Windows.Forms.Menu" /> control of previous versions, <see cref="T:System.Windows.Forms.Menu" /> is retained for both backward compatibility and future use if you choose.</summary>
	// Token: 0x020003B2 RID: 946
	[Designer("System.Windows.Forms.Design.ToolStripDropDownDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ToolStripDropDownMenu : ToolStripDropDown
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" /> class. </summary>
		// Token: 0x06003E4C RID: 15948 RVA: 0x0010F32C File Offset: 0x0010D52C
		public ToolStripDropDownMenu()
		{
		}

		// Token: 0x06003E4D RID: 15949 RVA: 0x0010F3EC File Offset: 0x0010D5EC
		internal ToolStripDropDownMenu(ToolStripItem ownerItem, bool isAutoGenerated) : base(ownerItem, isAutoGenerated)
		{
			if (DpiHelper.IsScalingRequired)
			{
				this.scaledDefaultImageSize = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.DefaultImageSize, 0);
				this.scaledDefaultImageMarginWidth = DpiHelper.LogicalToDeviceUnitsX(ToolStripDropDownMenu.DefaultImageMarginWidth) + 1;
				this.scaledDefaultImageAndCheckMarginWidth = DpiHelper.LogicalToDeviceUnitsX(ToolStripDropDownMenu.DefaultImageAndCheckMarginWidth) + 1;
				if (DpiHelper.EnableToolStripHighDpiImprovements)
				{
					this.scaledImagePadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.ImagePadding, 0);
					this.scaledTextPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.TextPadding, 0);
					this.scaledCheckPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.CheckPadding, 0);
					this.scaledArrowPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.ArrowPadding, 0);
					this.scaledArrowSize = DpiHelper.LogicalToDeviceUnitsX(ToolStripDropDownMenu.ArrowSize);
				}
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0010F547 File Offset: 0x0010D747
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x0010F552 File Offset: 0x0010D752
		internal override bool AllItemsVisible
		{
			get
			{
				return !this.RequiresScrollButtons;
			}
			set
			{
				this.RequiresScrollButtons = !value;
			}
		}

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x0010F55E File Offset: 0x0010D75E
		internal Rectangle ArrowRectangle
		{
			get
			{
				return this.arrowRectangle;
			}
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x0010F566 File Offset: 0x0010D766
		internal Rectangle CheckRectangle
		{
			get
			{
				return this.checkRectangle;
			}
		}

		/// <summary>Gets the internal spacing, in pixels, of the control.</summary>
		/// <returns>A <see langword="Padding" /> object representing the spacing.</returns>
		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0010F570 File Offset: 0x0010D770
		protected override Padding DefaultPadding
		{
			get
			{
				RightToLeft rightToLeft = this.RightToLeft;
				int num = (rightToLeft == RightToLeft.Yes) ? this.scaledTextPadding.Right : this.scaledTextPadding.Left;
				int num2 = (this.ShowCheckMargin || this.ShowImageMargin) ? (num + this.ImageMargin.Width) : num;
				if (rightToLeft == RightToLeft.Yes)
				{
					return new Padding(1, 2, num2, 2);
				}
				return new Padding(num2, 2, 1, 2);
			}
		}

		/// <summary>Gets the rectangle that represents the display area of the <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> that represents the display area.</returns>
		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x0010F5DC File Offset: 0x0010D7DC
		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle rectangle = base.DisplayRectangle;
				if (base.GetToolStripState(32))
				{
					rectangle.Y += this.UpScrollButton.Height + this.UpScrollButton.Margin.Vertical;
					rectangle.Height -= this.UpScrollButton.Height + this.UpScrollButton.Margin.Vertical + this.DownScrollButton.Height + this.DownScrollButton.Margin.Vertical;
					rectangle = LayoutUtils.InflateRect(rectangle, new Padding(0, base.Padding.Top, 0, base.Padding.Bottom));
				}
				return rectangle;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x0010F6A2 File Offset: 0x0010D8A2
		private ToolStripScrollButton DownScrollButton
		{
			get
			{
				if (this.downScrollButton == null)
				{
					this.downScrollButton = new ToolStripScrollButton(false);
					this.downScrollButton.ParentInternal = this;
				}
				return this.downScrollButton;
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x06003E55 RID: 15957 RVA: 0x0010F6CA File Offset: 0x0010D8CA
		internal Rectangle ImageRectangle
		{
			get
			{
				return this.imageRectangle;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x0010F6D2 File Offset: 0x0010D8D2
		// (set) Token: 0x06003E57 RID: 15959 RVA: 0x0010F6DA File Offset: 0x0010D8DA
		internal int PaddingToTrim
		{
			get
			{
				return this.paddingToTrim;
			}
			set
			{
				if (this.paddingToTrim != value)
				{
					this.paddingToTrim = value;
					base.AdjustSize();
				}
			}
		}

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06003E58 RID: 15960 RVA: 0x0010F6F2 File Offset: 0x0010D8F2
		internal Rectangle ImageMargin
		{
			get
			{
				this.imageMarginBounds.Height = base.Height;
				return this.imageMarginBounds;
			}
		}

		/// <summary>Passes a reference to the cached <see cref="P:System.Windows.Forms.Control.LayoutEngine" /> returned by the layout engine interface.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> that represents the cached layout engine returned by the layout engine interface.</returns>
		// Token: 0x17000F8F RID: 3983
		// (get) Token: 0x06003E59 RID: 15961 RVA: 0x0010F70B File Offset: 0x0010D90B
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return ToolStripDropDownMenu.ToolStripDropDownLayoutEngine.LayoutInstance;
			}
		}

		/// <summary>Gets or sets a value indicating how the items of <see cref="T:System.Windows.Forms.ContextMenuStrip" /> are displayed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripLayoutStyle" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripLayoutStyle.Flow" />.</returns>
		// Token: 0x17000F90 RID: 3984
		// (get) Token: 0x06003E5A RID: 15962 RVA: 0x000F8E3E File Offset: 0x000F703E
		// (set) Token: 0x06003E5B RID: 15963 RVA: 0x000F8E46 File Offset: 0x000F7046
		[DefaultValue(ToolStripLayoutStyle.Flow)]
		public new ToolStripLayoutStyle LayoutStyle
		{
			get
			{
				return base.LayoutStyle;
			}
			set
			{
				base.LayoutStyle = value;
			}
		}

		/// <summary>Gets the maximum height and width, in pixels, of the <see cref="T:System.Windows.Forms.ContextMenuStrip" />.</summary>
		/// <returns>A <see langword="Size" /> object representing the height and width of the control, in pixels.</returns>
		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x06003E5C RID: 15964 RVA: 0x0010F712 File Offset: 0x0010D912
		protected internal override Size MaxItemSize
		{
			get
			{
				if (!this.state[ToolStripDropDownMenu.stateMaxItemSizeValid])
				{
					this.CalculateInternalLayoutMetrics();
				}
				return this.maxItemSize;
			}
		}

		/// <summary>Gets or sets a value indicating whether space for an image is shown on the left edge of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the image margin is shown; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x06003E5D RID: 15965 RVA: 0x0010F732 File Offset: 0x0010D932
		// (set) Token: 0x06003E5E RID: 15966 RVA: 0x0010F744 File Offset: 0x0010D944
		[DefaultValue(true)]
		[SRDescription("ToolStripDropDownMenuShowImageMarginDescr")]
		[SRCategory("CatAppearance")]
		public bool ShowImageMargin
		{
			get
			{
				return this.state[ToolStripDropDownMenu.stateShowImageMargin];
			}
			set
			{
				if (value != this.state[ToolStripDropDownMenu.stateShowImageMargin])
				{
					this.state[ToolStripDropDownMenu.stateShowImageMargin] = value;
					LayoutTransaction.DoLayout(this, this, PropertyNames.ShowImageMargin);
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether space for a check mark is shown on the left edge of the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. </summary>
		/// <returns>
		///     <see langword="true" /> if the check margin is shown; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x06003E5F RID: 15967 RVA: 0x0010F776 File Offset: 0x0010D976
		// (set) Token: 0x06003E60 RID: 15968 RVA: 0x0010F788 File Offset: 0x0010D988
		[DefaultValue(false)]
		[SRDescription("ToolStripDropDownMenuShowCheckMarginDescr")]
		[SRCategory("CatAppearance")]
		public bool ShowCheckMargin
		{
			get
			{
				return this.state[ToolStripDropDownMenu.stateShowCheckMargin];
			}
			set
			{
				if (value != this.state[ToolStripDropDownMenu.stateShowCheckMargin])
				{
					this.state[ToolStripDropDownMenu.stateShowCheckMargin] = value;
					LayoutTransaction.DoLayout(this, this, PropertyNames.ShowCheckMargin);
				}
			}
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0010F7BA File Offset: 0x0010D9BA
		internal Rectangle TextRectangle
		{
			get
			{
				return this.textRectangle;
			}
		}

		// Token: 0x17000F95 RID: 3989
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0010F7C2 File Offset: 0x0010D9C2
		private ToolStripScrollButton UpScrollButton
		{
			get
			{
				if (this.upScrollButton == null)
				{
					this.upScrollButton = new ToolStripScrollButton(true);
					this.upScrollButton.ParentInternal = this;
				}
				return this.upScrollButton;
			}
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x0010F7EC File Offset: 0x0010D9EC
		internal static ToolStripDropDownMenu FromHMenu(IntPtr hmenu, IWin32Window targetWindow)
		{
			ToolStripDropDownMenu toolStripDropDownMenu = new ToolStripDropDownMenu();
			toolStripDropDownMenu.SuspendLayout();
			HandleRef hMenu = new HandleRef(null, hmenu);
			int menuItemCount = UnsafeNativeMethods.GetMenuItemCount(hMenu);
			for (int i = 0; i < menuItemCount; i++)
			{
				NativeMethods.MENUITEMINFO_T_RW menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
				menuiteminfo_T_RW.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));
				menuiteminfo_T_RW.fMask = 256;
				menuiteminfo_T_RW.fType = 256;
				UnsafeNativeMethods.GetMenuItemInfo(hMenu, i, true, menuiteminfo_T_RW);
				ToolStripItem toolStripItem;
				if (menuiteminfo_T_RW.fType == 2048)
				{
					toolStripItem = new ToolStripSeparator();
				}
				else
				{
					menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
					menuiteminfo_T_RW.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));
					menuiteminfo_T_RW.fMask = 2;
					menuiteminfo_T_RW.fType = 2;
					UnsafeNativeMethods.GetMenuItemInfo(hMenu, i, true, menuiteminfo_T_RW);
					toolStripItem = new ToolStripMenuItem(hmenu, menuiteminfo_T_RW.wID, targetWindow);
					menuiteminfo_T_RW = new NativeMethods.MENUITEMINFO_T_RW();
					menuiteminfo_T_RW.cbSize = Marshal.SizeOf(typeof(NativeMethods.MENUITEMINFO_T_RW));
					menuiteminfo_T_RW.fMask = 4;
					menuiteminfo_T_RW.fType = 4;
					UnsafeNativeMethods.GetMenuItemInfo(hMenu, i, true, menuiteminfo_T_RW);
					if (menuiteminfo_T_RW.hSubMenu != IntPtr.Zero)
					{
						((ToolStripMenuItem)toolStripItem).DropDown = ToolStripDropDownMenu.FromHMenu(menuiteminfo_T_RW.hSubMenu, targetWindow);
					}
				}
				toolStripDropDownMenu.Items.Add(toolStripItem);
			}
			toolStripDropDownMenu.ResumeLayout();
			return toolStripDropDownMenu;
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x0010F948 File Offset: 0x0010DB48
		private void CalculateInternalLayoutMetrics()
		{
			Size empty = Size.Empty;
			Size empty2 = Size.Empty;
			Size size = this.scaledDefaultImageSize;
			Size empty3 = Size.Empty;
			Size empty4 = Size.Empty;
			for (int i = 0; i < this.Items.Count; i++)
			{
				ToolStripItem toolStripItem = this.Items[i];
				ToolStripMenuItem toolStripMenuItem = toolStripItem as ToolStripMenuItem;
				if (toolStripMenuItem != null)
				{
					Size textSize = toolStripMenuItem.GetTextSize();
					if (toolStripMenuItem.ShowShortcutKeys)
					{
						Size shortcutTextSize = toolStripMenuItem.GetShortcutTextSize();
						if (this.tabWidth == -1)
						{
							this.tabWidth = TextRenderer.MeasureText("\t", this.Font).Width;
						}
						textSize.Width += this.tabWidth + shortcutTextSize.Width;
						textSize.Height = Math.Max(textSize.Height, shortcutTextSize.Height);
					}
					empty.Width = Math.Max(empty.Width, textSize.Width);
					empty.Height = Math.Max(empty.Height, textSize.Height);
					Size size2 = Size.Empty;
					if (toolStripMenuItem.Image != null)
					{
						size2 = ((toolStripMenuItem.ImageScaling == ToolStripItemImageScaling.SizeToFit) ? base.ImageScalingSize : toolStripMenuItem.Image.Size);
					}
					empty2.Width = Math.Max(empty2.Width, size2.Width);
					empty2.Height = Math.Max(empty2.Height, size2.Height);
					if (toolStripMenuItem.CheckedImage != null)
					{
						Size size3 = toolStripMenuItem.CheckedImage.Size;
						size.Width = Math.Max(size3.Width, size.Width);
						size.Height = Math.Max(size3.Height, size.Height);
					}
				}
				else if (!(toolStripItem is ToolStripSeparator))
				{
					empty4.Height = Math.Max(toolStripItem.Bounds.Height, empty4.Height);
					empty4.Width = Math.Max(toolStripItem.Bounds.Width, empty4.Width);
				}
			}
			this.maxItemSize.Height = Math.Max(empty.Height + this.scaledTextPadding.Vertical, Math.Max(size.Height + this.scaledCheckPadding.Vertical, empty3.Height + this.scaledArrowPadding.Vertical));
			if (this.ShowImageMargin)
			{
				this.maxItemSize.Height = Math.Max(empty2.Height + this.scaledImagePadding.Vertical, this.maxItemSize.Height);
			}
			bool flag = this.ShowCheckMargin && size.Width == 0;
			bool flag2 = this.ShowImageMargin && empty2.Width == 0;
			empty3 = new Size(this.scaledArrowSize, this.maxItemSize.Height);
			empty.Height = this.maxItemSize.Height - this.scaledTextPadding.Vertical;
			empty2.Height = this.maxItemSize.Height - this.scaledImagePadding.Vertical;
			size.Height = this.maxItemSize.Height - this.scaledCheckPadding.Vertical;
			empty.Width = Math.Max(empty.Width, empty4.Width);
			Point empty5 = Point.Empty;
			int num = Math.Max(0, empty2.Width - this.scaledDefaultImageSize.Width);
			int num2;
			if (this.ShowCheckMargin && this.ShowImageMargin)
			{
				num2 = this.scaledDefaultImageAndCheckMarginWidth;
				num2 += num;
				empty5 = new Point(this.scaledCheckPadding.Left, this.scaledCheckPadding.Top);
				this.checkRectangle = LayoutUtils.Align(size, new Rectangle(empty5.X, empty5.Y, size.Width, this.maxItemSize.Height), ContentAlignment.MiddleCenter);
				empty5.X = this.checkRectangle.Right + this.scaledCheckPadding.Right + this.scaledImagePadding.Left;
				empty5.Y = this.scaledImagePadding.Top;
				this.imageRectangle = LayoutUtils.Align(empty2, new Rectangle(empty5.X, empty5.Y, empty2.Width, this.maxItemSize.Height), ContentAlignment.MiddleCenter);
			}
			else if (this.ShowCheckMargin)
			{
				num2 = this.scaledDefaultImageMarginWidth;
				empty5 = new Point(1, this.scaledCheckPadding.Top);
				this.checkRectangle = LayoutUtils.Align(size, new Rectangle(empty5.X, empty5.Y, num2, this.maxItemSize.Height), ContentAlignment.MiddleCenter);
				this.imageRectangle = Rectangle.Empty;
			}
			else if (this.ShowImageMargin)
			{
				num2 = this.scaledDefaultImageMarginWidth;
				num2 += num;
				empty5 = new Point(1, this.scaledCheckPadding.Top);
				this.checkRectangle = LayoutUtils.Align(LayoutUtils.UnionSizes(size, empty2), new Rectangle(empty5.X, empty5.Y, num2 - 1, this.maxItemSize.Height), ContentAlignment.MiddleCenter);
				this.imageRectangle = this.checkRectangle;
			}
			else
			{
				num2 = 0;
			}
			empty5.X = num2 + 1;
			this.imageMarginBounds = new Rectangle(0, 0, num2, base.Height);
			empty5.X = this.imageMarginBounds.Right + this.scaledTextPadding.Left;
			empty5.Y = this.scaledTextPadding.Top;
			this.textRectangle = new Rectangle(empty5, empty);
			empty5.X = this.textRectangle.Right + this.scaledTextPadding.Right + this.scaledArrowPadding.Left;
			empty5.Y = this.scaledArrowPadding.Top;
			this.arrowRectangle = new Rectangle(empty5, empty3);
			this.maxItemSize.Width = this.arrowRectangle.Right + this.scaledArrowPadding.Right - this.imageMarginBounds.Left;
			base.Padding = this.DefaultPadding;
			int num3 = this.imageMarginBounds.Width;
			if (this.RightToLeft == RightToLeft.Yes)
			{
				num3 += this.scaledTextPadding.Right;
				int width = this.maxItemSize.Width;
				this.checkRectangle.X = width - this.checkRectangle.Right;
				this.imageRectangle.X = width - this.imageRectangle.Right;
				this.textRectangle.X = width - this.textRectangle.Right;
				this.arrowRectangle.X = width - this.arrowRectangle.Right;
				this.imageMarginBounds.X = width - this.imageMarginBounds.Right;
			}
			else
			{
				num3 += this.scaledTextPadding.Left;
			}
			this.maxItemSize.Height = this.maxItemSize.Height + this.maxItemSize.Height % 2;
			this.textRectangle.Y = LayoutUtils.VAlign(this.textRectangle.Size, new Rectangle(Point.Empty, this.maxItemSize), ContentAlignment.MiddleCenter).Y;
			this.textRectangle.Y = this.textRectangle.Y + this.textRectangle.Height % 2;
			this.state[ToolStripDropDownMenu.stateMaxItemSizeValid] = true;
			this.PaddingToTrim = num3;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x001100B4 File Offset: 0x0010E2B4
		internal override void ChangeSelection(ToolStripItem nextItem)
		{
			if (nextItem != null)
			{
				Rectangle displayRectangle = this.DisplayRectangle;
				if (!displayRectangle.Contains(displayRectangle.X, nextItem.Bounds.Top) || !displayRectangle.Contains(displayRectangle.X, nextItem.Bounds.Bottom))
				{
					int num;
					if (displayRectangle.Y > nextItem.Bounds.Top)
					{
						num = nextItem.Bounds.Top - displayRectangle.Y;
					}
					else
					{
						num = nextItem.Bounds.Bottom - (displayRectangle.Y + displayRectangle.Height);
						int num2 = this.Items.IndexOf(nextItem);
						while (num2 >= 0 && ((this.Items[num2].Visible && displayRectangle.Contains(displayRectangle.X, this.Items[num2].Bounds.Top - num)) || !this.Items[num2].Visible))
						{
							num2--;
						}
						if (num2 >= 0 && displayRectangle.Contains(displayRectangle.X, this.Items[num2].Bounds.Bottom - num))
						{
							num += this.Items[num2].Bounds.Bottom - num - displayRectangle.Top;
						}
					}
					this.ScrollInternal(num);
					this.UpdateScrollButtonStatus();
				}
			}
			base.ChangeSelection(nextItem);
		}

		/// <summary>Creates a default <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> with the specified text, image, and event handler on a new <see cref="T:System.Windows.Forms.ToolStripDropDownMenu" />.</summary>
		/// <param name="text">The text to use for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />. If the <paramref name="text" /> parameter is a hyphen (-), this method creates a <see cref="T:System.Windows.Forms.ToolStripSeparator" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.</param>
		/// <param name="onClick">An event handler that raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event when the <see cref="T:System.Windows.Forms.ToolStripMenuItem" /> is clicked.</param>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripMenuItem" />, or a <see cref="T:System.Windows.Forms.ToolStripSeparator" /> if the <paramref name="text" /> parameter is a hyphen (-).</returns>
		// Token: 0x06003E66 RID: 15974 RVA: 0x000D0B2D File Offset: 0x000CED2D
		protected internal override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
		{
			if (text == "-")
			{
				return new ToolStripSeparator();
			}
			return new ToolStripMenuItem(text, image, onClick);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00110230 File Offset: 0x0010E430
		internal override ToolStripItem GetNextItem(ToolStripItem start, ArrowDirection direction, bool rtlAware)
		{
			return this.GetNextItem(start, direction);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0011023C File Offset: 0x0010E43C
		internal override void Initialize()
		{
			base.Initialize();
			base.Padding = this.DefaultPadding;
			FlowLayoutSettings flowLayoutSettings = FlowLayout.CreateSettings(this);
			flowLayoutSettings.FlowDirection = FlowDirection.TopDown;
			this.state[ToolStripDropDownMenu.stateShowImageMargin] = true;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data. </param>
		// Token: 0x06003E69 RID: 15977 RVA: 0x0011027A File Offset: 0x0010E47A
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (!base.IsDisposed)
			{
				this.RequiresScrollButtons = false;
				this.CalculateInternalLayoutMetrics();
				base.OnLayout(e);
				if (!this.RequiresScrollButtons)
				{
					this.ResetScrollPosition();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripDropDown.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06003E6A RID: 15978 RVA: 0x001102A6 File Offset: 0x0010E4A6
		protected override void OnFontChanged(EventArgs e)
		{
			this.tabWidth = -1;
			base.OnFontChanged(e);
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06003E6B RID: 15979 RVA: 0x001102B6 File Offset: 0x0010E4B6
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			if (this.ShowCheckMargin || this.ShowImageMargin)
			{
				base.Renderer.DrawImageMargin(new ToolStripRenderEventArgs(e.Graphics, this, this.ImageMargin, SystemColors.Control));
			}
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x001102F4 File Offset: 0x0010E4F4
		internal override void ResetScaling(int newDpi)
		{
			base.ResetScaling(newDpi);
			CommonProperties.xClearPreferredSizeCache(this);
			this.scaledDefaultImageSize = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.DefaultImageSize, newDpi);
			this.scaledDefaultImageMarginWidth = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.DefaultImageMarginWidth, newDpi) + 1;
			this.scaledDefaultImageAndCheckMarginWidth = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.DefaultImageAndCheckMarginWidth, newDpi) + 1;
			this.scaledImagePadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.ImagePadding, newDpi);
			this.scaledTextPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.TextPadding, newDpi);
			this.scaledCheckPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.CheckPadding, newDpi);
			this.scaledArrowPadding = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.ArrowPadding, newDpi);
			this.scaledArrowSize = DpiHelper.LogicalToDeviceUnits(ToolStripDropDownMenu.ArrowSize, newDpi);
		}

		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0011039A File Offset: 0x0010E59A
		// (set) Token: 0x06003E6E RID: 15982 RVA: 0x001103A4 File Offset: 0x0010E5A4
		internal override bool RequiresScrollButtons
		{
			get
			{
				return base.GetToolStripState(32);
			}
			set
			{
				bool flag = this.RequiresScrollButtons != value;
				base.SetToolStripState(32, value);
				if (flag)
				{
					this.UpdateScrollButtonLocations();
					if (this.Items.Count > 0)
					{
						int num = this.Items[0].Bounds.Top - this.DisplayRectangle.Top;
						this.ScrollInternal(num);
						this.scrollAmount -= num;
						if (value)
						{
							this.RestoreScrollPosition();
							return;
						}
					}
					else
					{
						this.scrollAmount = 0;
					}
				}
			}
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x0011042D File Offset: 0x0010E62D
		internal void ResetScrollPosition()
		{
			this.scrollAmount = 0;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00110438 File Offset: 0x0010E638
		internal void RestoreScrollPosition()
		{
			if (!this.RequiresScrollButtons || this.Items.Count == 0)
			{
				return;
			}
			Rectangle displayRectangle = this.DisplayRectangle;
			int num = displayRectangle.Top - this.Items[0].Bounds.Top;
			int num2 = this.scrollAmount - num;
			int num3 = 0;
			if (num2 > 0)
			{
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (num3 >= num2)
					{
						break;
					}
					if (this.Items[i].Available)
					{
						Rectangle bounds = this.Items[this.Items.Count - 1].Bounds;
						bounds.Y -= num3;
						if (displayRectangle.Contains(displayRectangle.X, bounds.Top) && displayRectangle.Contains(displayRectangle.X, bounds.Bottom))
						{
							break;
						}
						if (i < this.Items.Count - 1)
						{
							num3 += this.Items[i + 1].Bounds.Top - this.Items[i].Bounds.Top;
						}
						else
						{
							num3 += this.Items[i].Bounds.Height;
						}
					}
				}
			}
			else
			{
				int num4 = this.Items.Count - 1;
				while (num4 >= 0 && num3 > num2)
				{
					if (this.Items[num4].Available)
					{
						Rectangle bounds2 = this.Items[0].Bounds;
						bounds2.Y -= num3;
						if (displayRectangle.Contains(displayRectangle.X, bounds2.Top) && displayRectangle.Contains(displayRectangle.X, bounds2.Bottom))
						{
							break;
						}
						if (num4 > 0)
						{
							num3 -= this.Items[num4].Bounds.Top - this.Items[num4 - 1].Bounds.Top;
						}
						else
						{
							num3 -= this.Items[num4].Bounds.Height;
						}
					}
					num4--;
				}
			}
			this.ScrollInternal(num3);
			this.scrollAmount = this.DisplayRectangle.Top - this.Items[0].Bounds.Top;
			this.UpdateScrollButtonLocations();
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x001106D1 File Offset: 0x0010E8D1
		internal override void ScrollInternal(int delta)
		{
			base.ScrollInternal(delta);
			this.scrollAmount += delta;
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x001106E8 File Offset: 0x0010E8E8
		internal void ScrollInternal(bool up)
		{
			this.UpdateScrollButtonStatus();
			int delta;
			if (this.indexOfFirstDisplayedItem == -1 || this.indexOfFirstDisplayedItem >= this.Items.Count)
			{
				int menuHeight = SystemInformation.MenuHeight;
				delta = (up ? (-menuHeight) : menuHeight);
			}
			else if (up)
			{
				if (this.indexOfFirstDisplayedItem == 0)
				{
					delta = 0;
				}
				else
				{
					ToolStripItem toolStripItem = this.Items[this.indexOfFirstDisplayedItem - 1];
					ToolStripItem toolStripItem2 = this.Items[this.indexOfFirstDisplayedItem];
					delta = toolStripItem.Bounds.Top - toolStripItem2.Bounds.Top;
				}
			}
			else
			{
				if (this.indexOfFirstDisplayedItem == this.Items.Count - 1)
				{
				}
				ToolStripItem toolStripItem3 = this.Items[this.indexOfFirstDisplayedItem];
				ToolStripItem toolStripItem4 = this.Items[this.indexOfFirstDisplayedItem + 1];
				delta = toolStripItem4.Bounds.Top - toolStripItem3.Bounds.Top;
			}
			this.ScrollInternal(delta);
			this.UpdateScrollButtonLocations();
		}

		/// <summary>Resets the collection of displayed and overflow items after a layout is done.</summary>
		// Token: 0x06003E73 RID: 15987 RVA: 0x001107F4 File Offset: 0x0010E9F4
		protected override void SetDisplayedItems()
		{
			base.SetDisplayedItems();
			if (this.RequiresScrollButtons)
			{
				this.DisplayedItems.Add(this.UpScrollButton);
				this.DisplayedItems.Add(this.DownScrollButton);
				this.UpdateScrollButtonLocations();
				this.UpScrollButton.Visible = true;
				this.DownScrollButton.Visible = true;
				return;
			}
			this.UpScrollButton.Visible = false;
			this.DownScrollButton.Visible = false;
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x0011086C File Offset: 0x0010EA6C
		private void UpdateScrollButtonLocations()
		{
			if (base.GetToolStripState(32))
			{
				Size preferredSize = this.UpScrollButton.GetPreferredSize(Size.Empty);
				Point location = new Point(1, 0);
				this.UpScrollButton.SetBounds(new Rectangle(location, preferredSize));
				Size preferredSize2 = this.DownScrollButton.GetPreferredSize(Size.Empty);
				int height = base.GetDropDownBounds(base.Bounds).Height;
				Point location2 = new Point(1, height - preferredSize2.Height);
				this.DownScrollButton.SetBounds(new Rectangle(location2, preferredSize2));
				this.UpdateScrollButtonStatus();
			}
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00110900 File Offset: 0x0010EB00
		private void UpdateScrollButtonStatus()
		{
			Rectangle displayRectangle = this.DisplayRectangle;
			this.indexOfFirstDisplayedItem = -1;
			int num = int.MaxValue;
			int num2 = 0;
			for (int i = 0; i < this.Items.Count; i++)
			{
				ToolStripItem toolStripItem = this.Items[i];
				if (this.UpScrollButton != toolStripItem && this.DownScrollButton != toolStripItem && toolStripItem.Available)
				{
					if (this.indexOfFirstDisplayedItem == -1 && displayRectangle.Contains(displayRectangle.X, toolStripItem.Bounds.Top))
					{
						this.indexOfFirstDisplayedItem = i;
					}
					num = Math.Min(num, toolStripItem.Bounds.Top);
					num2 = Math.Max(num2, toolStripItem.Bounds.Bottom);
				}
			}
			this.UpScrollButton.Enabled = !displayRectangle.Contains(displayRectangle.X, num);
			this.DownScrollButton.Enabled = !displayRectangle.Contains(displayRectangle.X, num2);
		}

		// Token: 0x040023E9 RID: 9193
		private static readonly Padding ImagePadding = new Padding(2);

		// Token: 0x040023EA RID: 9194
		private static readonly Padding TextPadding = new Padding(8, 1, 9, 1);

		// Token: 0x040023EB RID: 9195
		private static readonly Padding CheckPadding = new Padding(5, 2, 2, 2);

		// Token: 0x040023EC RID: 9196
		private static readonly Padding ArrowPadding = new Padding(0, 0, 8, 0);

		// Token: 0x040023ED RID: 9197
		private static int DefaultImageMarginWidth = 24;

		// Token: 0x040023EE RID: 9198
		private static int DefaultImageAndCheckMarginWidth = 46;

		// Token: 0x040023EF RID: 9199
		private static int ArrowSize = 10;

		// Token: 0x040023F0 RID: 9200
		private Size maxItemSize = Size.Empty;

		// Token: 0x040023F1 RID: 9201
		private Rectangle checkRectangle = Rectangle.Empty;

		// Token: 0x040023F2 RID: 9202
		private Rectangle imageRectangle = Rectangle.Empty;

		// Token: 0x040023F3 RID: 9203
		private Rectangle arrowRectangle = Rectangle.Empty;

		// Token: 0x040023F4 RID: 9204
		private Rectangle textRectangle = Rectangle.Empty;

		// Token: 0x040023F5 RID: 9205
		private Rectangle imageMarginBounds = Rectangle.Empty;

		// Token: 0x040023F6 RID: 9206
		private int paddingToTrim;

		// Token: 0x040023F7 RID: 9207
		private int tabWidth = -1;

		// Token: 0x040023F8 RID: 9208
		private ToolStripScrollButton upScrollButton;

		// Token: 0x040023F9 RID: 9209
		private ToolStripScrollButton downScrollButton;

		// Token: 0x040023FA RID: 9210
		private int scrollAmount;

		// Token: 0x040023FB RID: 9211
		private int indexOfFirstDisplayedItem = -1;

		// Token: 0x040023FC RID: 9212
		private BitVector32 state;

		// Token: 0x040023FD RID: 9213
		private static readonly int stateShowImageMargin = BitVector32.CreateMask();

		// Token: 0x040023FE RID: 9214
		private static readonly int stateShowCheckMargin = BitVector32.CreateMask(ToolStripDropDownMenu.stateShowImageMargin);

		// Token: 0x040023FF RID: 9215
		private static readonly int stateMaxItemSizeValid = BitVector32.CreateMask(ToolStripDropDownMenu.stateShowCheckMargin);

		// Token: 0x04002400 RID: 9216
		private static readonly Size DefaultImageSize = new Size(16, 16);

		// Token: 0x04002401 RID: 9217
		private Size scaledDefaultImageSize = ToolStripDropDownMenu.DefaultImageSize;

		// Token: 0x04002402 RID: 9218
		private int scaledDefaultImageMarginWidth = ToolStripDropDownMenu.DefaultImageMarginWidth + 1;

		// Token: 0x04002403 RID: 9219
		private int scaledDefaultImageAndCheckMarginWidth = ToolStripDropDownMenu.DefaultImageAndCheckMarginWidth + 1;

		// Token: 0x04002404 RID: 9220
		private Padding scaledImagePadding = ToolStripDropDownMenu.ImagePadding;

		// Token: 0x04002405 RID: 9221
		private Padding scaledTextPadding = ToolStripDropDownMenu.TextPadding;

		// Token: 0x04002406 RID: 9222
		private Padding scaledCheckPadding = ToolStripDropDownMenu.CheckPadding;

		// Token: 0x04002407 RID: 9223
		private Padding scaledArrowPadding = ToolStripDropDownMenu.ArrowPadding;

		// Token: 0x04002408 RID: 9224
		private int scaledArrowSize = ToolStripDropDownMenu.ArrowSize;

		// Token: 0x02000737 RID: 1847
		internal sealed class ToolStripDropDownLayoutEngine : FlowLayout
		{
			// Token: 0x0600610F RID: 24847 RVA: 0x0018D408 File Offset: 0x0018B608
			internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
			{
				Size preferredSize = base.GetPreferredSize(container, proposedConstraints);
				ToolStripDropDownMenu toolStripDropDownMenu = container as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					preferredSize.Width = toolStripDropDownMenu.MaxItemSize.Width - toolStripDropDownMenu.PaddingToTrim;
				}
				return preferredSize;
			}

			// Token: 0x0400417C RID: 16764
			public static ToolStripDropDownMenu.ToolStripDropDownLayoutEngine LayoutInstance = new ToolStripDropDownMenu.ToolStripDropDownLayoutEngine();
		}
	}
}

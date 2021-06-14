using System;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003D8 RID: 984
	internal class ToolStripMenuItemInternalLayout : ToolStripItemInternalLayout
	{
		// Token: 0x06004155 RID: 16725 RVA: 0x00119F0A File Offset: 0x0011810A
		public ToolStripMenuItemInternalLayout(ToolStripMenuItem ownerItem) : base(ownerItem)
		{
			this.ownerItem = ownerItem;
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06004156 RID: 16726 RVA: 0x00119F1C File Offset: 0x0011811C
		public bool ShowCheckMargin
		{
			get
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				return toolStripDropDownMenu != null && toolStripDropDownMenu.ShowCheckMargin;
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06004157 RID: 16727 RVA: 0x00119F48 File Offset: 0x00118148
		public bool ShowImageMargin
		{
			get
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				return toolStripDropDownMenu != null && toolStripDropDownMenu.ShowImageMargin;
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x00119F71 File Offset: 0x00118171
		public bool PaintCheck
		{
			get
			{
				return this.ShowCheckMargin || this.ShowImageMargin;
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x00119F83 File Offset: 0x00118183
		public bool PaintImage
		{
			get
			{
				return this.ShowImageMargin;
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x00119F8C File Offset: 0x0011818C
		public Rectangle ArrowRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle arrowRectangle = toolStripDropDownMenu.ArrowRectangle;
						arrowRectangle.Y = LayoutUtils.VAlign(arrowRectangle.Size, this.ownerItem.ClientBounds, ContentAlignment.MiddleCenter).Y;
						return arrowRectangle;
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x0600415B RID: 16731 RVA: 0x00119FEC File Offset: 0x001181EC
		public Rectangle CheckRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle checkRectangle = toolStripDropDownMenu.CheckRectangle;
						if (this.ownerItem.CheckedImage != null)
						{
							int height = this.ownerItem.CheckedImage.Height;
							checkRectangle.Y += (checkRectangle.Height - height) / 2;
							checkRectangle.Height = height;
							return checkRectangle;
						}
					}
				}
				return Rectangle.Empty;
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x0600415C RID: 16732 RVA: 0x0011A064 File Offset: 0x00118264
		public override Rectangle ImageRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						Rectangle imageRectangle = toolStripDropDownMenu.ImageRectangle;
						if (this.ownerItem.ImageScaling == ToolStripItemImageScaling.SizeToFit)
						{
							imageRectangle.Size = toolStripDropDownMenu.ImageScalingSize;
						}
						else
						{
							Image image = this.ownerItem.Image ?? this.ownerItem.CheckedImage;
							imageRectangle.Size = image.Size;
						}
						imageRectangle.Y = LayoutUtils.VAlign(imageRectangle.Size, this.ownerItem.ClientBounds, ContentAlignment.MiddleCenter).Y;
						return imageRectangle;
					}
				}
				return base.ImageRectangle;
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x0600415D RID: 16733 RVA: 0x0011A10C File Offset: 0x0011830C
		public override Rectangle TextRectangle
		{
			get
			{
				if (this.UseMenuLayout)
				{
					ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
					if (toolStripDropDownMenu != null)
					{
						return toolStripDropDownMenu.TextRectangle;
					}
				}
				return base.TextRectangle;
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x0011A142 File Offset: 0x00118342
		public bool UseMenuLayout
		{
			get
			{
				return this.ownerItem.Owner is ToolStripDropDownMenu;
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0011A158 File Offset: 0x00118358
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.UseMenuLayout)
			{
				ToolStripDropDownMenu toolStripDropDownMenu = this.ownerItem.Owner as ToolStripDropDownMenu;
				if (toolStripDropDownMenu != null)
				{
					return toolStripDropDownMenu.MaxItemSize;
				}
			}
			return base.GetPreferredSize(constrainingSize);
		}

		// Token: 0x04002513 RID: 9491
		private ToolStripMenuItem ownerItem;
	}
}

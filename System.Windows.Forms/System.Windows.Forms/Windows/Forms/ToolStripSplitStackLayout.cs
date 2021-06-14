using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003F4 RID: 1012
	internal class ToolStripSplitStackLayout : LayoutEngine
	{
		// Token: 0x0600443B RID: 17467 RVA: 0x001234EC File Offset: 0x001216EC
		internal ToolStripSplitStackLayout(ToolStrip owner)
		{
			this.toolStrip = owner;
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x0600443C RID: 17468 RVA: 0x00123506 File Offset: 0x00121706
		// (set) Token: 0x0600443D RID: 17469 RVA: 0x0012350E File Offset: 0x0012170E
		protected int BackwardsWalkingIndex
		{
			get
			{
				return this.backwardsWalkingIndex;
			}
			set
			{
				this.backwardsWalkingIndex = value;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x0600443E RID: 17470 RVA: 0x00123517 File Offset: 0x00121717
		// (set) Token: 0x0600443F RID: 17471 RVA: 0x0012351F File Offset: 0x0012171F
		protected int ForwardsWalkingIndex
		{
			get
			{
				return this.forwardsWalkingIndex;
			}
			set
			{
				this.forwardsWalkingIndex = value;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x06004440 RID: 17472 RVA: 0x00123528 File Offset: 0x00121728
		private Size OverflowButtonSize
		{
			get
			{
				ToolStrip toolStrip = this.ToolStrip;
				if (!toolStrip.CanOverflow)
				{
					return Size.Empty;
				}
				Size sz = toolStrip.OverflowButton.AutoSize ? toolStrip.OverflowButton.GetPreferredSize(this.displayRectangle.Size) : toolStrip.OverflowButton.Size;
				return sz + toolStrip.OverflowButton.Margin.Size;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x06004441 RID: 17473 RVA: 0x00123594 File Offset: 0x00121794
		// (set) Token: 0x06004442 RID: 17474 RVA: 0x0012359C File Offset: 0x0012179C
		private int OverflowSpace
		{
			get
			{
				return this.overflowSpace;
			}
			set
			{
				this.overflowSpace = value;
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x06004443 RID: 17475 RVA: 0x001235A5 File Offset: 0x001217A5
		// (set) Token: 0x06004444 RID: 17476 RVA: 0x001235AD File Offset: 0x001217AD
		private bool OverflowRequired
		{
			get
			{
				return this.overflowRequired;
			}
			set
			{
				this.overflowRequired = value;
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x06004445 RID: 17477 RVA: 0x001235B6 File Offset: 0x001217B6
		public ToolStrip ToolStrip
		{
			get
			{
				return this.toolStrip;
			}
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x001235C0 File Offset: 0x001217C0
		private void CalculatePlacementsHorizontal()
		{
			this.ResetItemPlacements();
			ToolStrip toolStrip = this.ToolStrip;
			int num = 0;
			if (this.ToolStrip.CanOverflow)
			{
				this.ForwardsWalkingIndex = 0;
				while (this.ForwardsWalkingIndex < toolStrip.Items.Count)
				{
					ToolStripItem toolStripItem = toolStrip.Items[this.ForwardsWalkingIndex];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						if (toolStripItem.Overflow == ToolStripItemOverflow.Always)
						{
							this.OverflowRequired = true;
						}
						if (toolStripItem.Overflow != ToolStripItemOverflow.Always && toolStripItem.Placement == ToolStripItemPlacement.None)
						{
							num += (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size).Width + toolStripItem.Margin.Horizontal;
							int num2 = this.OverflowRequired ? this.OverflowButtonSize.Width : 0;
							if (num > this.displayRectangle.Width - num2)
							{
								int num3 = this.SendNextItemToOverflow(num + num2 - this.displayRectangle.Width, true);
								num -= num3;
							}
						}
					}
					int num4 = this.ForwardsWalkingIndex;
					this.ForwardsWalkingIndex = num4 + 1;
				}
			}
			this.PlaceItems();
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x001236F4 File Offset: 0x001218F4
		private void CalculatePlacementsVertical()
		{
			this.ResetItemPlacements();
			ToolStrip toolStrip = this.ToolStrip;
			int num = 0;
			if (this.ToolStrip.CanOverflow)
			{
				this.ForwardsWalkingIndex = 0;
				while (this.ForwardsWalkingIndex < this.ToolStrip.Items.Count)
				{
					ToolStripItem toolStripItem = toolStrip.Items[this.ForwardsWalkingIndex];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						if (toolStripItem.Overflow == ToolStripItemOverflow.Always)
						{
							this.OverflowRequired = true;
						}
						if (toolStripItem.Overflow != ToolStripItemOverflow.Always && toolStripItem.Placement == ToolStripItemPlacement.None)
						{
							Size size = toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size;
							int num2 = this.OverflowRequired ? this.OverflowButtonSize.Height : 0;
							num += size.Height + toolStripItem.Margin.Vertical;
							if (num > this.displayRectangle.Height - num2)
							{
								int num3 = this.SendNextItemToOverflow(num - this.displayRectangle.Height, false);
								num -= num3;
							}
						}
					}
					int num4 = this.ForwardsWalkingIndex;
					this.ForwardsWalkingIndex = num4 + 1;
				}
			}
			this.PlaceItems();
		}

		// Token: 0x06004448 RID: 17480 RVA: 0x00123828 File Offset: 0x00121A28
		internal override Size GetPreferredSize(IArrangedElement container, Size proposedConstraints)
		{
			if (!(container is ToolStrip))
			{
				throw new NotSupportedException(SR.GetString("ToolStripSplitStackLayoutContainerMustBeAToolStrip"));
			}
			if (this.toolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow)
			{
				return ToolStrip.GetPreferredSizeHorizontal(container, proposedConstraints);
			}
			return ToolStrip.GetPreferredSizeVertical(container, proposedConstraints);
		}

		// Token: 0x06004449 RID: 17481 RVA: 0x0012385F File Offset: 0x00121A5F
		private void InvalidateLayout()
		{
			this.forwardsWalkingIndex = 0;
			this.backwardsWalkingIndex = -1;
			this.overflowSpace = 0;
			this.overflowRequired = false;
			this.displayRectangle = Rectangle.Empty;
		}

		// Token: 0x0600444A RID: 17482 RVA: 0x00123888 File Offset: 0x00121A88
		internal override bool LayoutCore(IArrangedElement container, LayoutEventArgs layoutEventArgs)
		{
			if (!(container is ToolStrip))
			{
				throw new NotSupportedException(SR.GetString("ToolStripSplitStackLayoutContainerMustBeAToolStrip"));
			}
			this.InvalidateLayout();
			this.displayRectangle = this.toolStrip.DisplayRectangle;
			this.noMansLand = this.displayRectangle.Location;
			this.noMansLand.X = this.noMansLand.X + (this.toolStrip.ClientSize.Width + 1);
			this.noMansLand.Y = this.noMansLand.Y + (this.toolStrip.ClientSize.Height + 1);
			if (this.toolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow)
			{
				this.LayoutHorizontal();
			}
			else
			{
				this.LayoutVertical();
			}
			return CommonProperties.GetAutoSize(container);
		}

		// Token: 0x0600444B RID: 17483 RVA: 0x00123948 File Offset: 0x00121B48
		private bool LayoutHorizontal()
		{
			ToolStrip toolStrip = this.ToolStrip;
			Rectangle clientRectangle = toolStrip.ClientRectangle;
			int num = this.displayRectangle.Right;
			int num2 = this.displayRectangle.Left;
			bool result = false;
			Size itemSize = Size.Empty;
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = Rectangle.Empty;
			this.CalculatePlacementsHorizontal();
			bool flag = toolStrip.CanOverflow && (this.OverflowRequired || this.OverflowSpace >= this.OverflowButtonSize.Width);
			toolStrip.OverflowButton.Visible = flag;
			if (flag)
			{
				if (toolStrip.RightToLeft == RightToLeft.No)
				{
					num = clientRectangle.Right;
				}
				else
				{
					num2 = clientRectangle.Left;
				}
			}
			int i = -1;
			while (i < toolStrip.Items.Count)
			{
				ToolStripItem toolStripItem;
				if (i == -1)
				{
					if (flag)
					{
						toolStripItem = toolStrip.OverflowButton;
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
						itemSize = this.OverflowButtonSize;
						goto IL_11F;
					}
					toolStripItem = toolStrip.OverflowButton;
					toolStripItem.SetPlacement(ToolStripItemPlacement.None);
				}
				else
				{
					toolStripItem = toolStrip.Items[i];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						itemSize = (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(Size.Empty) : toolStripItem.Size);
						goto IL_11F;
					}
				}
				IL_356:
				i++;
				continue;
				IL_11F:
				if (!flag && toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
				}
				if (toolStripItem != null && toolStripItem.Placement == ToolStripItemPlacement.Main)
				{
					int num3 = this.displayRectangle.Left;
					int num4 = this.displayRectangle.Top;
					Padding margin = toolStripItem.Margin;
					if ((toolStripItem.Alignment == ToolStripItemAlignment.Right && toolStrip.RightToLeft == RightToLeft.No) || (toolStripItem.Alignment == ToolStripItemAlignment.Left && toolStrip.RightToLeft == RightToLeft.Yes))
					{
						num3 = num - (margin.Right + itemSize.Width);
						num4 += margin.Top;
						num = num3 - margin.Left;
						rectangle2 = ((rectangle2 == Rectangle.Empty) ? new Rectangle(num3, num4, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle2, new Rectangle(num3, num4, itemSize.Width, itemSize.Height)));
					}
					else
					{
						num3 = num2 + margin.Left;
						num4 += margin.Top;
						num2 = num3 + itemSize.Width + margin.Right;
						rectangle = ((rectangle == Rectangle.Empty) ? new Rectangle(num3, num4, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle, new Rectangle(num3, num4, itemSize.Width, itemSize.Height)));
					}
					toolStripItem.ParentInternal = this.ToolStrip;
					Point itemLocation = new Point(num3, num4);
					if (!clientRectangle.Contains(num3, num4))
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (rectangle2.Width > 0 && rectangle.Width > 0 && rectangle2.IntersectsWith(rectangle))
					{
						itemLocation = this.noMansLand;
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					if (toolStripItem.AutoSize)
					{
						itemSize.Height = Math.Max(this.displayRectangle.Height - margin.Vertical, 0);
					}
					else
					{
						itemLocation.Y = LayoutUtils.VAlign(toolStripItem.Size, this.displayRectangle, AnchorStyles.None).Y;
					}
					this.SetItemLocation(toolStripItem, itemLocation, itemSize);
					goto IL_356;
				}
				toolStripItem.ParentInternal = ((toolStripItem.Placement == ToolStripItemPlacement.Overflow) ? toolStrip.OverflowButton.DropDown : null);
				goto IL_356;
			}
			return result;
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x00123CC8 File Offset: 0x00121EC8
		private bool LayoutVertical()
		{
			ToolStrip toolStrip = this.ToolStrip;
			Rectangle clientRectangle = toolStrip.ClientRectangle;
			int num = this.displayRectangle.Bottom;
			int num2 = this.displayRectangle.Top;
			bool result = false;
			Size itemSize = Size.Empty;
			Rectangle rectangle = Rectangle.Empty;
			Rectangle rectangle2 = Rectangle.Empty;
			Size size = this.displayRectangle.Size;
			DockStyle dock = toolStrip.Dock;
			if (toolStrip.AutoSize && ((!toolStrip.IsInToolStripPanel && dock == DockStyle.Left) || dock == DockStyle.Right))
			{
				size = ToolStrip.GetPreferredSizeVertical(toolStrip, Size.Empty) - toolStrip.Padding.Size;
			}
			this.CalculatePlacementsVertical();
			bool flag = toolStrip.CanOverflow && (this.OverflowRequired || this.OverflowSpace >= this.OverflowButtonSize.Height);
			toolStrip.OverflowButton.Visible = flag;
			int i = -1;
			while (i < this.ToolStrip.Items.Count)
			{
				ToolStripItem toolStripItem;
				if (i == -1)
				{
					if (flag)
					{
						toolStripItem = toolStrip.OverflowButton;
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
						itemSize = this.OverflowButtonSize;
						goto IL_153;
					}
					toolStripItem = toolStrip.OverflowButton;
					toolStripItem.SetPlacement(ToolStripItemPlacement.None);
				}
				else
				{
					toolStripItem = toolStrip.Items[i];
					if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
					{
						itemSize = (toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(Size.Empty) : toolStripItem.Size);
						goto IL_153;
					}
				}
				IL_366:
				i++;
				continue;
				IL_153:
				if (!flag && toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement == ToolStripItemPlacement.Overflow)
				{
					toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
				}
				if (toolStripItem != null && toolStripItem.Placement == ToolStripItemPlacement.Main)
				{
					Padding margin = toolStripItem.Margin;
					int x = this.displayRectangle.Left + margin.Left;
					int num3 = this.displayRectangle.Top;
					ToolStripItemAlignment alignment = toolStripItem.Alignment;
					if (alignment != ToolStripItemAlignment.Left && alignment == ToolStripItemAlignment.Right)
					{
						num3 = num - (margin.Bottom + itemSize.Height);
						num = num3 - margin.Top;
						rectangle2 = ((rectangle2 == Rectangle.Empty) ? new Rectangle(x, num3, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle2, new Rectangle(x, num3, itemSize.Width, itemSize.Height)));
					}
					else
					{
						num3 = num2 + margin.Top;
						num2 = num3 + itemSize.Height + margin.Bottom;
						rectangle = ((rectangle == Rectangle.Empty) ? new Rectangle(x, num3, itemSize.Width, itemSize.Height) : Rectangle.Union(rectangle, new Rectangle(x, num3, itemSize.Width, itemSize.Height)));
					}
					toolStripItem.ParentInternal = this.ToolStrip;
					Point itemLocation = new Point(x, num3);
					if (!clientRectangle.Contains(x, num3))
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					else if (rectangle2.Width > 0 && rectangle.Width > 0 && rectangle2.IntersectsWith(rectangle))
					{
						itemLocation = this.noMansLand;
						toolStripItem.SetPlacement(ToolStripItemPlacement.None);
					}
					if (toolStripItem.AutoSize)
					{
						itemSize.Width = Math.Max(size.Width - margin.Horizontal - 1, 0);
					}
					else
					{
						itemLocation.X = LayoutUtils.HAlign(toolStripItem.Size, this.displayRectangle, AnchorStyles.None).X;
					}
					this.SetItemLocation(toolStripItem, itemLocation, itemSize);
					goto IL_366;
				}
				toolStripItem.ParentInternal = ((toolStripItem.Placement == ToolStripItemPlacement.Overflow) ? toolStrip.OverflowButton.DropDown : null);
				goto IL_366;
			}
			return result;
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0012405C File Offset: 0x0012225C
		private void SetItemLocation(ToolStripItem item, Point itemLocation, Size itemSize)
		{
			if (item.Placement == ToolStripItemPlacement.Main && !(item is ToolStripOverflowButton))
			{
				bool flag = this.ToolStrip.LayoutStyle == ToolStripLayoutStyle.HorizontalStackWithOverflow;
				Rectangle rectangle = this.displayRectangle;
				Rectangle rectangle2 = new Rectangle(itemLocation, itemSize);
				if (flag)
				{
					if (rectangle2.Right > this.displayRectangle.Right || rectangle2.Left < this.displayRectangle.Left)
					{
						itemLocation = this.noMansLand;
						item.SetPlacement(ToolStripItemPlacement.None);
					}
				}
				else if (rectangle2.Bottom > this.displayRectangle.Bottom || rectangle2.Top < this.displayRectangle.Top)
				{
					itemLocation = this.noMansLand;
					item.SetPlacement(ToolStripItemPlacement.None);
				}
			}
			item.SetBounds(new Rectangle(itemLocation, itemSize));
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x00124120 File Offset: 0x00122320
		private void PlaceItems()
		{
			ToolStrip toolStrip = this.ToolStrip;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				ToolStripItem toolStripItem = toolStrip.Items[i];
				if (toolStripItem.Placement == ToolStripItemPlacement.None)
				{
					if (toolStripItem.Overflow != ToolStripItemOverflow.Always)
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.Main);
					}
					else
					{
						toolStripItem.SetPlacement(ToolStripItemPlacement.Overflow);
					}
				}
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0012417C File Offset: 0x0012237C
		private void ResetItemPlacements()
		{
			ToolStrip toolStrip = this.ToolStrip;
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				if (toolStrip.Items[i].Placement == ToolStripItemPlacement.Overflow)
				{
					toolStrip.Items[i].ParentInternal = null;
				}
				toolStrip.Items[i].SetPlacement(ToolStripItemPlacement.None);
			}
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x001241E0 File Offset: 0x001223E0
		private int SendNextItemToOverflow(int spaceNeeded, bool horizontal)
		{
			int num = 0;
			int num2 = this.BackwardsWalkingIndex;
			this.BackwardsWalkingIndex = ((num2 == -1) ? (this.ToolStrip.Items.Count - 1) : (num2 - 1));
			while (this.BackwardsWalkingIndex >= 0)
			{
				ToolStripItem toolStripItem = this.ToolStrip.Items[this.BackwardsWalkingIndex];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					Padding margin = toolStripItem.Margin;
					if (toolStripItem.Overflow == ToolStripItemOverflow.AsNeeded && toolStripItem.Placement != ToolStripItemPlacement.Overflow)
					{
						Size size = toolStripItem.AutoSize ? toolStripItem.GetPreferredSize(this.displayRectangle.Size) : toolStripItem.Size;
						if (this.BackwardsWalkingIndex <= this.ForwardsWalkingIndex)
						{
							num += (horizontal ? (size.Width + margin.Horizontal) : (size.Height + margin.Vertical));
						}
						toolStripItem.SetPlacement(ToolStripItemPlacement.Overflow);
						if (!this.OverflowRequired)
						{
							spaceNeeded += (horizontal ? this.OverflowButtonSize.Width : this.OverflowButtonSize.Height);
							this.OverflowRequired = true;
						}
						this.OverflowSpace += (horizontal ? (size.Width + margin.Horizontal) : (size.Height + margin.Vertical));
					}
					if (num > spaceNeeded)
					{
						break;
					}
				}
				int num3 = this.BackwardsWalkingIndex;
				this.BackwardsWalkingIndex = num3 - 1;
			}
			return num;
		}

		// Token: 0x040025C4 RID: 9668
		private int backwardsWalkingIndex;

		// Token: 0x040025C5 RID: 9669
		private int forwardsWalkingIndex;

		// Token: 0x040025C6 RID: 9670
		private ToolStrip toolStrip;

		// Token: 0x040025C7 RID: 9671
		private int overflowSpace;

		// Token: 0x040025C8 RID: 9672
		private bool overflowRequired;

		// Token: 0x040025C9 RID: 9673
		private Point noMansLand;

		// Token: 0x040025CA RID: 9674
		private Rectangle displayRectangle = Rectangle.Empty;

		// Token: 0x040025CB RID: 9675
		internal static readonly TraceSwitch DebugLayoutTraceSwitch;
	}
}

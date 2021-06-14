using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020003A2 RID: 930
	internal sealed class ToolStripSplitStackDragDropHandler : IDropTarget, ISupportOleDropSource
	{
		// Token: 0x06003BF5 RID: 15349 RVA: 0x00109C9A File Offset: 0x00107E9A
		public ToolStripSplitStackDragDropHandler(ToolStrip owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this.owner = owner;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x00109CB8 File Offset: 0x00107EB8
		public void OnDragEnter(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				e.Effect = DragDropEffects.Move;
				this.ShowItemDropPoint(this.owner.PointToClient(new Point(e.X, e.Y)));
			}
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00109D06 File Offset: 0x00107F06
		public void OnDragLeave(EventArgs e)
		{
			this.owner.ClearInsertionMark();
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00109D14 File Offset: 0x00107F14
		public void OnDragDrop(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				ToolStripItem droppedItem = (ToolStripItem)e.Data.GetData(typeof(ToolStripItem));
				this.OnDropItem(droppedItem, this.owner.PointToClient(new Point(e.X, e.Y)));
			}
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00109D78 File Offset: 0x00107F78
		public void OnDragOver(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(ToolStripItem)))
			{
				if (this.ShowItemDropPoint(this.owner.PointToClient(new Point(e.X, e.Y))))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
				if (this.owner != null)
				{
					this.owner.ClearInsertionMark();
				}
				e.Effect = DragDropEffects.None;
			}
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x0000701A File Offset: 0x0000521A
		public void OnGiveFeedback(GiveFeedbackEventArgs e)
		{
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x0000701A File Offset: 0x0000521A
		public void OnQueryContinueDrag(QueryContinueDragEventArgs e)
		{
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00109DE4 File Offset: 0x00107FE4
		private void OnDropItem(ToolStripItem droppedItem, Point ownerClientAreaRelativeDropPoint)
		{
			Point empty = Point.Empty;
			int itemInsertionIndex = this.GetItemInsertionIndex(ownerClientAreaRelativeDropPoint);
			if (itemInsertionIndex < 0)
			{
				if (itemInsertionIndex == -1 && this.owner.Items.Count == 0)
				{
					this.owner.Items.Add(droppedItem);
					this.owner.ClearInsertionMark();
				}
				return;
			}
			ToolStripItem toolStripItem = this.owner.Items[itemInsertionIndex];
			if (toolStripItem == droppedItem)
			{
				this.owner.ClearInsertionMark();
				return;
			}
			ToolStripSplitStackDragDropHandler.RelativeLocation relativeLocation = this.ComparePositions(toolStripItem.Bounds, ownerClientAreaRelativeDropPoint);
			droppedItem.Alignment = toolStripItem.Alignment;
			int num = Math.Max(0, itemInsertionIndex);
			if (relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Above)
			{
				num = ((toolStripItem.Alignment == ToolStripItemAlignment.Left) ? num : (num + 1));
			}
			else if (relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Below)
			{
				num = ((toolStripItem.Alignment == ToolStripItemAlignment.Left) ? num : (num - 1));
			}
			else if ((toolStripItem.Alignment == ToolStripItemAlignment.Left && relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Left) || (toolStripItem.Alignment == ToolStripItemAlignment.Right && relativeLocation == ToolStripSplitStackDragDropHandler.RelativeLocation.Right))
			{
				num = Math.Max(0, (this.owner.RightToLeft == RightToLeft.Yes) ? (num + 1) : num);
			}
			else
			{
				num = Math.Max(0, (this.owner.RightToLeft == RightToLeft.No) ? (num + 1) : num);
			}
			if (this.owner.Items.IndexOf(droppedItem) < num)
			{
				num--;
			}
			this.owner.Items.MoveItem(Math.Max(0, num), droppedItem);
			this.owner.ClearInsertionMark();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00109F44 File Offset: 0x00108144
		private bool ShowItemDropPoint(Point ownerClientAreaRelativeDropPoint)
		{
			int itemInsertionIndex = this.GetItemInsertionIndex(ownerClientAreaRelativeDropPoint);
			if (itemInsertionIndex >= 0)
			{
				ToolStripItem toolStripItem = this.owner.Items[itemInsertionIndex];
				ToolStripSplitStackDragDropHandler.RelativeLocation relativeLocation = this.ComparePositions(toolStripItem.Bounds, ownerClientAreaRelativeDropPoint);
				Rectangle empty = Rectangle.Empty;
				switch (relativeLocation)
				{
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Above:
					empty = new Rectangle(this.owner.Margin.Left, toolStripItem.Bounds.Top, this.owner.Width - this.owner.Margin.Horizontal - 1, ToolStrip.insertionBeamWidth);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Below:
					empty = new Rectangle(this.owner.Margin.Left, toolStripItem.Bounds.Bottom, this.owner.Width - this.owner.Margin.Horizontal - 1, ToolStrip.insertionBeamWidth);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Right:
					empty = new Rectangle(toolStripItem.Bounds.Right, this.owner.Margin.Top, ToolStrip.insertionBeamWidth, this.owner.Height - this.owner.Margin.Vertical - 1);
					break;
				case ToolStripSplitStackDragDropHandler.RelativeLocation.Left:
					empty = new Rectangle(toolStripItem.Bounds.Left, this.owner.Margin.Top, ToolStrip.insertionBeamWidth, this.owner.Height - this.owner.Margin.Vertical - 1);
					break;
				}
				this.owner.PaintInsertionMark(empty);
				return true;
			}
			if (this.owner.Items.Count == 0)
			{
				Rectangle displayRectangle = this.owner.DisplayRectangle;
				displayRectangle.Width = ToolStrip.insertionBeamWidth;
				this.owner.PaintInsertionMark(displayRectangle);
				return true;
			}
			return false;
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x0010A13C File Offset: 0x0010833C
		private int GetItemInsertionIndex(Point ownerClientAreaRelativeDropPoint)
		{
			for (int i = 0; i < this.owner.DisplayedItems.Count; i++)
			{
				Rectangle bounds = this.owner.DisplayedItems[i].Bounds;
				bounds.Inflate(this.owner.DisplayedItems[i].Margin.Size);
				if (bounds.Contains(ownerClientAreaRelativeDropPoint))
				{
					return this.owner.Items.IndexOf(this.owner.DisplayedItems[i]);
				}
			}
			if (this.owner.DisplayedItems.Count > 0)
			{
				int j = 0;
				while (j < this.owner.DisplayedItems.Count)
				{
					if (this.owner.DisplayedItems[j].Alignment == ToolStripItemAlignment.Right)
					{
						if (j > 0)
						{
							return this.owner.Items.IndexOf(this.owner.DisplayedItems[j - 1]);
						}
						return this.owner.Items.IndexOf(this.owner.DisplayedItems[j]);
					}
					else
					{
						j++;
					}
				}
				return this.owner.Items.IndexOf(this.owner.DisplayedItems[this.owner.DisplayedItems.Count - 1]);
			}
			return -1;
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x0010A294 File Offset: 0x00108494
		private ToolStripSplitStackDragDropHandler.RelativeLocation ComparePositions(Rectangle orig, Point check)
		{
			if (this.owner.Orientation == Orientation.Horizontal)
			{
				int num = orig.Width / 2;
				if (orig.Left + num >= check.X)
				{
					return ToolStripSplitStackDragDropHandler.RelativeLocation.Left;
				}
				if (orig.Right - num <= check.X)
				{
					return ToolStripSplitStackDragDropHandler.RelativeLocation.Right;
				}
			}
			if (this.owner.Orientation == Orientation.Vertical)
			{
				int num2 = orig.Height / 2;
				return (check.Y <= orig.Top + num2) ? ToolStripSplitStackDragDropHandler.RelativeLocation.Above : ToolStripSplitStackDragDropHandler.RelativeLocation.Below;
			}
			return ToolStripSplitStackDragDropHandler.RelativeLocation.Left;
		}

		// Token: 0x0400238B RID: 9099
		private ToolStrip owner;

		// Token: 0x0200072F RID: 1839
		private enum RelativeLocation
		{
			// Token: 0x04004169 RID: 16745
			Above,
			// Token: 0x0400416A RID: 16746
			Below,
			// Token: 0x0400416B RID: 16747
			Right,
			// Token: 0x0400416C RID: 16748
			Left
		}
	}
}

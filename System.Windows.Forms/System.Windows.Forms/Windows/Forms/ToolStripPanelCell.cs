using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003DE RID: 990
	internal class ToolStripPanelCell : ArrangedElement
	{
		// Token: 0x0600424A RID: 16970 RVA: 0x0011BE8D File Offset: 0x0011A08D
		public ToolStripPanelCell(Control control) : this(null, control)
		{
		}

		// Token: 0x0600424B RID: 16971 RVA: 0x0011BE98 File Offset: 0x0011A098
		public ToolStripPanelCell(ToolStripPanelRow parent, Control control)
		{
			this.ToolStripPanelRow = parent;
			this._wrappedToolStrip = (control as ToolStrip);
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (this._wrappedToolStrip == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
				{
					typeof(ToolStrip).Name
				}), new object[0]), control.GetType().Name);
			}
			CommonProperties.SetAutoSize(this, true);
			this._wrappedToolStrip.LocationChanging += this.OnToolStripLocationChanging;
			this._wrappedToolStrip.VisibleChanged += this.OnToolStripVisibleChanged;
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x0011BF62 File Offset: 0x0011A162
		// (set) Token: 0x0600424D RID: 16973 RVA: 0x0011BF6A File Offset: 0x0011A16A
		public Rectangle CachedBounds
		{
			get
			{
				return this.cachedBounds;
			}
			set
			{
				this.cachedBounds = value;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x0011BF73 File Offset: 0x0011A173
		public Control Control
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x0011BF7B File Offset: 0x0011A17B
		public bool ControlInDesignMode
		{
			get
			{
				return this._wrappedToolStrip != null && this._wrappedToolStrip.IsInDesignMode;
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x0011BF73 File Offset: 0x0011A173
		public IArrangedElement InnerElement
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06004251 RID: 16977 RVA: 0x0011BF73 File Offset: 0x0011A173
		public ISupportToolStripPanel DraggedControl
		{
			get
			{
				return this._wrappedToolStrip;
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x0011BF92 File Offset: 0x0011A192
		// (set) Token: 0x06004253 RID: 16979 RVA: 0x0011BF9A File Offset: 0x0011A19A
		public ToolStripPanelRow ToolStripPanelRow
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (this.parent != value)
				{
					if (this.parent != null)
					{
						((IList)this.parent.Cells).Remove(this);
					}
					this.parent = value;
					base.Margin = Padding.Empty;
				}
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x0011BFD0 File Offset: 0x0011A1D0
		// (set) Token: 0x06004255 RID: 16981 RVA: 0x0011BFFF File Offset: 0x0011A1FF
		public override bool Visible
		{
			get
			{
				return this.Control != null && this.Control.ParentInternal == this.ToolStripPanelRow.ToolStripPanel && this.InnerElement.ParticipatesInLayout;
			}
			set
			{
				this.Control.Visible = value;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06004256 RID: 16982 RVA: 0x0011C00D File Offset: 0x0011A20D
		public Size MaximumSize
		{
			get
			{
				return this.maxSize;
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06004257 RID: 16983 RVA: 0x00027639 File Offset: 0x00025839
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return DefaultLayout.Instance;
			}
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x0011BF92 File Offset: 0x0011A192
		protected override IArrangedElement GetContainer()
		{
			return this.parent;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x0011C015 File Offset: 0x0011A215
		public int Grow(int growBy)
		{
			if (this.ToolStripPanelRow.Orientation == Orientation.Vertical)
			{
				return this.GrowVertical(growBy);
			}
			return this.GrowHorizontal(growBy);
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0011C034 File Offset: 0x0011A234
		private int GrowVertical(int growBy)
		{
			if (this.MaximumSize.Height >= this.Control.PreferredSize.Height)
			{
				return 0;
			}
			if (this.MaximumSize.Height + growBy >= this.Control.PreferredSize.Height)
			{
				int result = this.Control.PreferredSize.Height - this.MaximumSize.Height;
				this.maxSize = LayoutUtils.MaxSize;
				return result;
			}
			if (this.MaximumSize.Height + growBy < this.Control.PreferredSize.Height)
			{
				this.maxSize.Height = this.maxSize.Height + growBy;
				return growBy;
			}
			return 0;
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x0011C0F8 File Offset: 0x0011A2F8
		private int GrowHorizontal(int growBy)
		{
			if (this.MaximumSize.Width >= this.Control.PreferredSize.Width)
			{
				return 0;
			}
			if (this.MaximumSize.Width + growBy >= this.Control.PreferredSize.Width)
			{
				int result = this.Control.PreferredSize.Width - this.MaximumSize.Width;
				this.maxSize = LayoutUtils.MaxSize;
				return result;
			}
			if (this.MaximumSize.Width + growBy < this.Control.PreferredSize.Width)
			{
				this.maxSize.Width = this.maxSize.Width + growBy;
				return growBy;
			}
			return 0;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0011C1BC File Offset: 0x0011A3BC
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._wrappedToolStrip != null)
					{
						this._wrappedToolStrip.LocationChanging -= this.OnToolStripLocationChanging;
						this._wrappedToolStrip.VisibleChanged -= this.OnToolStripVisibleChanged;
					}
					this._wrappedToolStrip = null;
					if (this.parent != null)
					{
						((IList)this.parent.Cells).Remove(this);
					}
					this.parent = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0011C244 File Offset: 0x0011A444
		protected override ArrangedElementCollection GetChildren()
		{
			return ArrangedElementCollection.Empty;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x0011C24C File Offset: 0x0011A44C
		public override Size GetPreferredSize(Size constrainingSize)
		{
			ISupportToolStripPanel draggedControl = this.DraggedControl;
			Size result = Size.Empty;
			if (draggedControl.Stretch)
			{
				if (this.ToolStripPanelRow.Orientation == Orientation.Horizontal)
				{
					constrainingSize.Width = this.ToolStripPanelRow.Bounds.Width;
					result = this._wrappedToolStrip.GetPreferredSize(constrainingSize);
					result.Width = constrainingSize.Width;
				}
				else
				{
					constrainingSize.Height = this.ToolStripPanelRow.Bounds.Height;
					result = this._wrappedToolStrip.GetPreferredSize(constrainingSize);
					result.Height = constrainingSize.Height;
				}
			}
			else
			{
				result = ((!this._wrappedToolStrip.AutoSize) ? this._wrappedToolStrip.Size : this._wrappedToolStrip.GetPreferredSize(constrainingSize));
			}
			return result;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x0011C314 File Offset: 0x0011A514
		protected override void SetBoundsCore(Rectangle bounds, BoundsSpecified specified)
		{
			this.currentlySizing = true;
			this.CachedBounds = bounds;
			try
			{
				if (this.DraggedControl.IsCurrentlyDragging)
				{
					if (this.ToolStripPanelRow.Cells[this.ToolStripPanelRow.Cells.Count - 1] == this)
					{
						Rectangle displayRectangle = this.ToolStripPanelRow.DisplayRectangle;
						if (this.ToolStripPanelRow.Orientation == Orientation.Horizontal)
						{
							int num = bounds.Right - displayRectangle.Right;
							if (num > 0 && bounds.Width > num)
							{
								bounds.Width -= num;
							}
						}
						else
						{
							int num2 = bounds.Bottom - displayRectangle.Bottom;
							if (num2 > 0 && bounds.Height > num2)
							{
								bounds.Height -= num2;
							}
						}
					}
					base.SetBoundsCore(bounds, specified);
					this.InnerElement.SetBounds(bounds, specified);
				}
				else if (!this.ToolStripPanelRow.CachedBoundsMode)
				{
					base.SetBoundsCore(bounds, specified);
					this.InnerElement.SetBounds(bounds, specified);
				}
			}
			finally
			{
				this.currentlySizing = false;
			}
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0011C42C File Offset: 0x0011A62C
		public int Shrink(int shrinkBy)
		{
			if (this.ToolStripPanelRow.Orientation == Orientation.Vertical)
			{
				return this.ShrinkVertical(shrinkBy);
			}
			return this.ShrinkHorizontal(shrinkBy);
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		private int ShrinkHorizontal(int shrinkBy)
		{
			return 0;
		}

		// Token: 0x06004262 RID: 16994 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		private int ShrinkVertical(int shrinkBy)
		{
			return 0;
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x0011C44C File Offset: 0x0011A64C
		private void OnToolStripLocationChanging(object sender, ToolStripLocationCancelEventArgs e)
		{
			if (this.ToolStripPanelRow == null)
			{
				return;
			}
			if (!this.currentlySizing && !this.currentlyDragging)
			{
				try
				{
					this.currentlyDragging = true;
					Point newLocation = e.NewLocation;
					if (this.ToolStripPanelRow != null && this.ToolStripPanelRow.Bounds == Rectangle.Empty)
					{
						this.ToolStripPanelRow.ToolStripPanel.PerformUpdate(true);
					}
					if (this._wrappedToolStrip != null)
					{
						this.ToolStripPanelRow.ToolStripPanel.Join(this._wrappedToolStrip, newLocation);
					}
				}
				finally
				{
					this.currentlyDragging = false;
					e.Cancel = true;
				}
			}
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0011C4F0 File Offset: 0x0011A6F0
		private void OnToolStripVisibleChanged(object sender, EventArgs e)
		{
			if (this._wrappedToolStrip != null && !this._wrappedToolStrip.IsInDesignMode && !this._wrappedToolStrip.IsCurrentlyDragging && !this._wrappedToolStrip.IsDisposed && !this._wrappedToolStrip.Disposing)
			{
				if (!this.Control.Visible)
				{
					this.restoreOnVisibleChanged = (this.ToolStripPanelRow != null && ((IList)this.ToolStripPanelRow.Cells).Contains(this));
					return;
				}
				if (this.restoreOnVisibleChanged)
				{
					try
					{
						if (this.ToolStripPanelRow != null && ((IList)this.ToolStripPanelRow.Cells).Contains(this))
						{
							this.ToolStripPanelRow.ToolStripPanel.Join(this._wrappedToolStrip, this._wrappedToolStrip.Location);
						}
					}
					finally
					{
						this.restoreOnVisibleChanged = false;
					}
				}
			}
		}

		// Token: 0x0400253C RID: 9532
		private ToolStrip _wrappedToolStrip;

		// Token: 0x0400253D RID: 9533
		private ToolStripPanelRow parent;

		// Token: 0x0400253E RID: 9534
		private Size maxSize = LayoutUtils.MaxSize;

		// Token: 0x0400253F RID: 9535
		private bool currentlySizing;

		// Token: 0x04002540 RID: 9536
		private bool currentlyDragging;

		// Token: 0x04002541 RID: 9537
		private bool restoreOnVisibleChanged;

		// Token: 0x04002542 RID: 9538
		private Rectangle cachedBounds = Rectangle.Empty;
	}
}

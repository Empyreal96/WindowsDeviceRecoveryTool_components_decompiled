using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a row of a <see cref="T:System.Windows.Forms.ToolStripPanel" /> that can contain controls.</summary>
	// Token: 0x020003E3 RID: 995
	[ToolboxItem(false)]
	public class ToolStripPanelRow : Component, IArrangedElement, IComponent, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> class, specifying the containing <see cref="T:System.Windows.Forms.ToolStripPanel" />. </summary>
		/// <param name="parent">The containing <see cref="T:System.Windows.Forms.ToolStripPanel" />.</param>
		// Token: 0x06004277 RID: 17015 RVA: 0x0011C642 File Offset: 0x0011A842
		public ToolStripPanelRow(ToolStripPanel parent) : this(parent, true)
		{
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x0011C64C File Offset: 0x0011A84C
		internal ToolStripPanelRow(ToolStripPanel parent, bool visible)
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.minAllowedWidth = DpiHelper.LogicalToDeviceUnitsX(50);
			}
			this.parent = parent;
			this.state[ToolStripPanelRow.stateVisible] = visible;
			this.state[ToolStripPanelRow.stateDisposing | ToolStripPanelRow.stateLocked | ToolStripPanelRow.stateInitialized] = false;
			using (new LayoutTransaction(parent, this, null))
			{
				this.Margin = this.DefaultMargin;
				CommonProperties.SetAutoSize(this, true);
			}
		}

		/// <summary>Gets the size and location of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />, including its nonclient elements, in pixels, relative to the parent control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the size and location.</returns>
		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06004279 RID: 17017 RVA: 0x0011C700 File Offset: 0x0011A900
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		/// <summary>Gets the controls in the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</summary>
		/// <returns>An array of controls.</returns>
		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x0600427A RID: 17018 RVA: 0x0011C708 File Offset: 0x0011A908
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlControlsDescr")]
		public Control[] Controls
		{
			get
			{
				Control[] array = new Control[this.ControlsInternal.Count];
				this.ControlsInternal.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x0600427B RID: 17019 RVA: 0x0011C734 File Offset: 0x0011A934
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("ControlControlsDescr")]
		internal ToolStripPanelRow.ToolStripPanelRowControlCollection ControlsInternal
		{
			get
			{
				ToolStripPanelRow.ToolStripPanelRowControlCollection toolStripPanelRowControlCollection = (ToolStripPanelRow.ToolStripPanelRowControlCollection)this.Properties.GetObject(ToolStripPanelRow.PropControlsCollection);
				if (toolStripPanelRowControlCollection == null)
				{
					toolStripPanelRowControlCollection = this.CreateControlsInstance();
					this.Properties.SetObject(ToolStripPanelRow.PropControlsCollection, toolStripPanelRowControlCollection);
				}
				return toolStripPanelRowControlCollection;
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x0600427C RID: 17020 RVA: 0x0011C773 File Offset: 0x0011A973
		internal ArrangedElementCollection Cells
		{
			get
			{
				return this.ControlsInternal.Cells;
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x0600427D RID: 17021 RVA: 0x0011C780 File Offset: 0x0011A980
		// (set) Token: 0x0600427E RID: 17022 RVA: 0x0011C792 File Offset: 0x0011A992
		internal bool CachedBoundsMode
		{
			get
			{
				return this.state[ToolStripPanelRow.stateCachedBoundsMode];
			}
			set
			{
				this.state[ToolStripPanelRow.stateCachedBoundsMode] = value;
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x0600427F RID: 17023 RVA: 0x0011C7A5 File Offset: 0x0011A9A5
		private ToolStripPanelRow.ToolStripPanelRowManager RowManager
		{
			get
			{
				if (this.rowManager == null)
				{
					this.rowManager = ((this.Orientation == Orientation.Horizontal) ? new ToolStripPanelRow.HorizontalRowManager(this) : new ToolStripPanelRow.VerticalRowManager(this));
					this.Initialized = true;
				}
				return this.rowManager;
			}
		}

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the default space between controls.</returns>
		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06004280 RID: 17024 RVA: 0x0011C7D8 File Offset: 0x0011A9D8
		protected virtual Padding DefaultMargin
		{
			get
			{
				ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(0, true);
				if (nextVisibleCell != null && nextVisibleCell.DraggedControl != null && nextVisibleCell.DraggedControl.Stretch)
				{
					Padding rowMargin = this.ToolStripPanel.RowMargin;
					if (this.Orientation == Orientation.Horizontal)
					{
						rowMargin.Left = 0;
						rowMargin.Right = 0;
					}
					else
					{
						rowMargin.Top = 0;
						rowMargin.Bottom = 0;
					}
					return rowMargin;
				}
				return this.ToolStripPanel.RowMargin;
			}
		}

		/// <summary>Gets the internal spacing, in pixels, of the contents of a control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> that represents the internal spacing of the contents of a control.</returns>
		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06004281 RID: 17025 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected virtual Padding DefaultPadding
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets the display area of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" /> representing the size and location.</returns>
		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x0011C84E File Offset: 0x0011AA4E
		public Rectangle DisplayRectangle
		{
			get
			{
				return this.RowManager.DisplayRectangle;
			}
		}

		/// <summary>Gets an instance of the control's layout engine.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> for the control's contents.</returns>
		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06004283 RID: 17027 RVA: 0x000A76F4 File Offset: 0x000A58F4
		public LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x0011C85B File Offset: 0x0011AA5B
		internal bool Locked
		{
			get
			{
				return this.state[ToolStripPanelRow.stateLocked];
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06004285 RID: 17029 RVA: 0x0011C86D File Offset: 0x0011AA6D
		// (set) Token: 0x06004286 RID: 17030 RVA: 0x0011C87F File Offset: 0x0011AA7F
		private bool Initialized
		{
			get
			{
				return this.state[ToolStripPanelRow.stateInitialized];
			}
			set
			{
				this.state[ToolStripPanelRow.stateInitialized] = value;
			}
		}

		/// <summary>Gets or sets the space between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the space between controls.</returns>
		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06004287 RID: 17031 RVA: 0x000119E5 File Offset: 0x0000FBE5
		// (set) Token: 0x06004288 RID: 17032 RVA: 0x0011C892 File Offset: 0x0011AA92
		public Padding Margin
		{
			get
			{
				return CommonProperties.GetMargin(this);
			}
			set
			{
				if (this.Margin != value)
				{
					CommonProperties.SetMargin(this, value);
				}
			}
		}

		/// <summary>Gets or sets padding within the control.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the control's internal spacing characteristics.</returns>
		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06004289 RID: 17033 RVA: 0x0011C8A9 File Offset: 0x0011AAA9
		// (set) Token: 0x0600428A RID: 17034 RVA: 0x0011C8B7 File Offset: 0x0011AAB7
		public virtual Padding Padding
		{
			get
			{
				return CommonProperties.GetPadding(this, this.DefaultPadding);
			}
			set
			{
				if (this.Padding != value)
				{
					CommonProperties.SetPadding(this, value);
				}
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x0600428B RID: 17035 RVA: 0x0011C8CE File Offset: 0x0011AACE
		internal Control ParentInternal
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x0600428C RID: 17036 RVA: 0x0011C8D6 File Offset: 0x0011AAD6
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.ToolStripPanel" /> that contains the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolStripPanel" /> that contains the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</returns>
		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x0600428D RID: 17037 RVA: 0x0011C8CE File Offset: 0x0011AACE
		public ToolStripPanel ToolStripPanel
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x0600428E RID: 17038 RVA: 0x0011C8DE File Offset: 0x0011AADE
		internal bool Visible
		{
			get
			{
				return this.state[ToolStripPanelRow.stateVisible];
			}
		}

		/// <summary>Gets the layout direction of the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> relative to its containing <see cref="T:System.Windows.Forms.ToolStripPanel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Orientation" /> values.</returns>
		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x0600428F RID: 17039 RVA: 0x0011C8F0 File Offset: 0x0011AAF0
		public Orientation Orientation
		{
			get
			{
				return this.ToolStripPanel.Orientation;
			}
		}

		/// <summary>Gets or sets a value indicating whether a <see cref="T:System.Windows.Forms.ToolStrip" /> can be dragged and dropped into a <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</summary>
		/// <param name="toolStripToDrag">The <see cref="T:System.Windows.Forms.ToolStrip" /> to be dragged and dropped into the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</param>
		/// <returns>
		///     <see langword="true" /> if there is enough space in the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> to receive the <see cref="T:System.Windows.Forms.ToolStrip" />; otherwise, <see langword="false" />. </returns>
		// Token: 0x06004290 RID: 17040 RVA: 0x0011C8FD File Offset: 0x0011AAFD
		public bool CanMove(ToolStrip toolStripToDrag)
		{
			return !this.ToolStripPanel.Locked && !this.Locked && this.RowManager.CanMove(toolStripToDrag);
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x0011C922 File Offset: 0x0011AB22
		private ToolStripPanelRow.ToolStripPanelRowControlCollection CreateControlsInstance()
		{
			return new ToolStripPanelRow.ToolStripPanelRowControlCollection(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ToolStripPanelRow" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x06004292 RID: 17042 RVA: 0x0011C92C File Offset: 0x0011AB2C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.state[ToolStripPanelRow.stateDisposing] = true;
					this.ControlsInternal.Clear();
				}
			}
			finally
			{
				this.state[ToolStripPanelRow.stateDisposing] = false;
				base.Dispose(disposing);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded" /> event.</summary>
		/// <param name="control">The control that was added to the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</param>
		/// <param name="index">The zero-based index representing the position of the added control.</param>
		// Token: 0x06004293 RID: 17043 RVA: 0x0011C984 File Offset: 0x0011AB84
		protected internal virtual void OnControlAdded(Control control, int index)
		{
			ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
			if (supportToolStripPanel != null)
			{
				supportToolStripPanel.ToolStripPanelRow = this;
			}
			this.RowManager.OnControlAdded(control, index);
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.ToolStripPanelRow.Orientation" /> property changes.</summary>
		// Token: 0x06004294 RID: 17044 RVA: 0x0011C9AF File Offset: 0x0011ABAF
		protected internal virtual void OnOrientationChanged()
		{
			this.rowManager = null;
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Forms.ToolStripPanelRow.Bounds" /> property changes.</summary>
		/// <param name="oldBounds">The original value of the <see cref="P:System.Windows.Forms.ToolStripPanelRow.Bounds" /> property.</param>
		/// <param name="newBounds">The new value of the <see cref="P:System.Windows.Forms.ToolStripPanelRow.Bounds" /> property.</param>
		// Token: 0x06004295 RID: 17045 RVA: 0x0011C9B8 File Offset: 0x0011ABB8
		protected void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
		{
			((IArrangedElement)this).PerformLayout(this, PropertyNames.Size);
			this.RowManager.OnBoundsChanged(oldBounds, newBounds);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved" /> event.</summary>
		/// <param name="control">The control that was removed from the <see cref="T:System.Windows.Forms.ToolStripPanelRow" />.</param>
		/// <param name="index">The zero-based index representing the position of the removed control.</param>
		// Token: 0x06004296 RID: 17046 RVA: 0x0011C9D4 File Offset: 0x0011ABD4
		protected internal virtual void OnControlRemoved(Control control, int index)
		{
			if (!this.state[ToolStripPanelRow.stateDisposing])
			{
				this.SuspendLayout();
				this.RowManager.OnControlRemoved(control, index);
				ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
				if (supportToolStripPanel != null && supportToolStripPanel.ToolStripPanelRow == this)
				{
					supportToolStripPanel.ToolStripPanelRow = null;
				}
				this.ResumeLayout(true);
				if (this.ControlsInternal.Count <= 0)
				{
					this.ToolStripPanel.RowsInternal.Remove(this);
					base.Dispose();
				}
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x0011CA4C File Offset: 0x0011AC4C
		internal Size GetMinimumSize(ToolStrip toolStrip)
		{
			if (toolStrip.MinimumSize == Size.Empty)
			{
				return new Size(this.minAllowedWidth, this.minAllowedWidth);
			}
			return toolStrip.MinimumSize;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0011CA78 File Offset: 0x0011AC78
		private void ApplyCachedBounds()
		{
			for (int i = 0; i < this.Cells.Count; i++)
			{
				IArrangedElement arrangedElement = this.Cells[i];
				if (arrangedElement.ParticipatesInLayout)
				{
					ToolStripPanelCell toolStripPanelCell = arrangedElement as ToolStripPanelCell;
					arrangedElement.SetBounds(toolStripPanelCell.CachedBounds, BoundsSpecified.None);
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x06004299 RID: 17049 RVA: 0x0011CAC4 File Offset: 0x0011ACC4
		protected virtual void OnLayout(LayoutEventArgs e)
		{
			if (this.Initialized && !this.state[ToolStripPanelRow.stateInLayout])
			{
				this.state[ToolStripPanelRow.stateInLayout] = true;
				try
				{
					this.Margin = this.DefaultMargin;
					this.CachedBoundsMode = true;
					try
					{
						bool flag = this.LayoutEngine.Layout(this, e);
					}
					finally
					{
						this.CachedBoundsMode = false;
					}
					if (this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false) == null)
					{
						this.ApplyCachedBounds();
					}
					else if (this.Orientation == Orientation.Horizontal)
					{
						this.OnLayoutHorizontalPostFix();
					}
					else
					{
						this.OnLayoutVerticalPostFix();
					}
				}
				finally
				{
					this.state[ToolStripPanelRow.stateInLayout] = false;
				}
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0011CB98 File Offset: 0x0011AD98
		private void OnLayoutHorizontalPostFix()
		{
			ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false);
			if (nextVisibleCell == null)
			{
				this.ApplyCachedBounds();
				return;
			}
			int num = nextVisibleCell.CachedBounds.Right - this.RowManager.DisplayRectangle.Right;
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array = new int[this.Cells.Count];
			for (int i = 0; i < this.Cells.Count; i++)
			{
				ToolStripPanelCell toolStripPanelCell = this.Cells[i] as ToolStripPanelCell;
				array[i] = toolStripPanelCell.Margin.Left;
			}
			num -= this.RowManager.FreeSpaceFromRow(num);
			for (int j = 0; j < this.Cells.Count; j++)
			{
				ToolStripPanelCell toolStripPanelCell2 = this.Cells[j] as ToolStripPanelCell;
				Rectangle cachedBounds = toolStripPanelCell2.CachedBounds;
				cachedBounds.X -= Math.Max(0, array[j] - toolStripPanelCell2.Margin.Left);
				toolStripPanelCell2.CachedBounds = cachedBounds;
			}
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array2 = null;
			for (int k = this.Cells.Count - 1; k >= 0; k--)
			{
				ToolStripPanelCell toolStripPanelCell3 = this.Cells[k] as ToolStripPanelCell;
				if (toolStripPanelCell3.Visible)
				{
					Size minimumSize = this.GetMinimumSize(toolStripPanelCell3.Control as ToolStrip);
					Rectangle cachedBounds2 = toolStripPanelCell3.CachedBounds;
					if (cachedBounds2.Width > minimumSize.Width)
					{
						num -= cachedBounds2.Width - minimumSize.Width;
						cachedBounds2.Width = ((num < 0) ? (minimumSize.Width + -num) : minimumSize.Width);
						for (int l = k + 1; l < this.Cells.Count; l++)
						{
							if (array2 == null)
							{
								array2 = new int[this.Cells.Count];
							}
							array2[l] += Math.Max(0, toolStripPanelCell3.CachedBounds.Width - cachedBounds2.Width);
						}
						toolStripPanelCell3.CachedBounds = cachedBounds2;
					}
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (array2 != null)
			{
				for (int m = 0; m < this.Cells.Count; m++)
				{
					ToolStripPanelCell toolStripPanelCell4 = this.Cells[m] as ToolStripPanelCell;
					Rectangle cachedBounds3 = toolStripPanelCell4.CachedBounds;
					cachedBounds3.X -= array2[m];
					toolStripPanelCell4.CachedBounds = cachedBounds3;
				}
			}
			this.ApplyCachedBounds();
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x0011CE38 File Offset: 0x0011B038
		private void OnLayoutVerticalPostFix()
		{
			ToolStripPanelCell nextVisibleCell = this.RowManager.GetNextVisibleCell(this.Cells.Count - 1, false);
			int num = nextVisibleCell.CachedBounds.Bottom - this.RowManager.DisplayRectangle.Bottom;
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array = new int[this.Cells.Count];
			for (int i = 0; i < this.Cells.Count; i++)
			{
				ToolStripPanelCell toolStripPanelCell = this.Cells[i] as ToolStripPanelCell;
				array[i] = toolStripPanelCell.Margin.Top;
			}
			num -= this.RowManager.FreeSpaceFromRow(num);
			for (int j = 0; j < this.Cells.Count; j++)
			{
				ToolStripPanelCell toolStripPanelCell2 = this.Cells[j] as ToolStripPanelCell;
				Rectangle cachedBounds = toolStripPanelCell2.CachedBounds;
				cachedBounds.X = Math.Max(0, cachedBounds.X - array[j] - toolStripPanelCell2.Margin.Top);
				toolStripPanelCell2.CachedBounds = cachedBounds;
			}
			if (num <= 0)
			{
				this.ApplyCachedBounds();
				return;
			}
			int[] array2 = null;
			for (int k = this.Cells.Count - 1; k >= 0; k--)
			{
				ToolStripPanelCell toolStripPanelCell3 = this.Cells[k] as ToolStripPanelCell;
				if (toolStripPanelCell3.Visible)
				{
					Size minimumSize = this.GetMinimumSize(toolStripPanelCell3.Control as ToolStrip);
					Rectangle cachedBounds2 = toolStripPanelCell3.CachedBounds;
					if (cachedBounds2.Height > minimumSize.Height)
					{
						num -= cachedBounds2.Height - minimumSize.Height;
						cachedBounds2.Height = ((num < 0) ? (minimumSize.Height + -num) : minimumSize.Height);
						for (int l = k + 1; l < this.Cells.Count; l++)
						{
							if (array2 == null)
							{
								array2 = new int[this.Cells.Count];
							}
							array2[l] += Math.Max(0, toolStripPanelCell3.CachedBounds.Height - cachedBounds2.Height);
						}
						toolStripPanelCell3.CachedBounds = cachedBounds2;
					}
				}
				if (num <= 0)
				{
					break;
				}
			}
			if (array2 != null)
			{
				for (int m = 0; m < this.Cells.Count; m++)
				{
					ToolStripPanelCell toolStripPanelCell4 = this.Cells[m] as ToolStripPanelCell;
					Rectangle cachedBounds3 = toolStripPanelCell4.CachedBounds;
					cachedBounds3.Y -= array2[m];
					toolStripPanelCell4.CachedBounds = cachedBounds3;
				}
			}
			this.ApplyCachedBounds();
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x0011D0D0 File Offset: 0x0011B2D0
		private void SetBounds(Rectangle bounds)
		{
			if (bounds != this.bounds)
			{
				Rectangle oldBounds = this.bounds;
				this.bounds = bounds;
				this.OnBoundsChanged(oldBounds, bounds);
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x0011D101 File Offset: 0x0011B301
		private void SuspendLayout()
		{
			this.suspendCount++;
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x0011D111 File Offset: 0x0011B311
		private void ResumeLayout(bool performLayout)
		{
			this.suspendCount--;
			if (performLayout)
			{
				((IArrangedElement)this).PerformLayout(this, null);
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x0600429F RID: 17055 RVA: 0x0011D12C File Offset: 0x0011B32C
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.Cells;
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x060042A0 RID: 17056 RVA: 0x0011D134 File Offset: 0x0011B334
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ToolStripPanel;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x0011D13C File Offset: 0x0011B33C
		Rectangle IArrangedElement.DisplayRectangle
		{
			get
			{
				return this.Bounds;
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x060042A2 RID: 17058 RVA: 0x0011D151 File Offset: 0x0011B351
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return this.Visible;
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x0011D159 File Offset: 0x0011B359
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return this.Properties;
			}
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x0011D164 File Offset: 0x0011B364
		Size IArrangedElement.GetPreferredSize(Size constrainingSize)
		{
			Size result = this.LayoutEngine.GetPreferredSize(this, constrainingSize - this.Padding.Size) + this.Padding.Size;
			if (this.Orientation == Orientation.Horizontal && this.ParentInternal != null)
			{
				result.Width = this.DisplayRectangle.Width;
			}
			else
			{
				result.Height = this.DisplayRectangle.Height;
			}
			return result;
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x0011D1E2 File Offset: 0x0011B3E2
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBounds(bounds);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x0011D1EB File Offset: 0x0011B3EB
		void IArrangedElement.PerformLayout(IArrangedElement container, string propertyName)
		{
			if (this.suspendCount <= 0)
			{
				this.OnLayout(new LayoutEventArgs(container, propertyName));
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x0011D203 File Offset: 0x0011B403
		internal Rectangle DragBounds
		{
			get
			{
				return this.RowManager.DragBounds;
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x0011D210 File Offset: 0x0011B410
		internal void MoveControl(ToolStrip movingControl, Point startClientLocation, Point endClientLocation)
		{
			this.RowManager.MoveControl(movingControl, startClientLocation, endClientLocation);
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x0011D220 File Offset: 0x0011B420
		internal void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
		{
			this.RowManager.JoinRow(toolStripToDrag, locationToDrag);
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x0011D22F File Offset: 0x0011B42F
		internal void LeaveRow(ToolStrip toolStripToDrag)
		{
			this.RowManager.LeaveRow(toolStripToDrag);
			if (this.ControlsInternal.Count == 0)
			{
				this.ToolStripPanel.RowsInternal.Remove(this);
				base.Dispose();
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x0000701A File Offset: 0x0000521A
		[Conditional("DEBUG")]
		private void PrintPlacements(int index)
		{
		}

		// Token: 0x04002549 RID: 9545
		private Rectangle bounds = Rectangle.Empty;

		// Token: 0x0400254A RID: 9546
		private ToolStripPanel parent;

		// Token: 0x0400254B RID: 9547
		private BitVector32 state;

		// Token: 0x0400254C RID: 9548
		private PropertyStore propertyStore = new PropertyStore();

		// Token: 0x0400254D RID: 9549
		private int suspendCount;

		// Token: 0x0400254E RID: 9550
		private ToolStripPanelRow.ToolStripPanelRowManager rowManager;

		// Token: 0x0400254F RID: 9551
		private const int MINALLOWEDWIDTH = 50;

		// Token: 0x04002550 RID: 9552
		private int minAllowedWidth = 50;

		// Token: 0x04002551 RID: 9553
		private static readonly int stateVisible = BitVector32.CreateMask();

		// Token: 0x04002552 RID: 9554
		private static readonly int stateDisposing = BitVector32.CreateMask(ToolStripPanelRow.stateVisible);

		// Token: 0x04002553 RID: 9555
		private static readonly int stateLocked = BitVector32.CreateMask(ToolStripPanelRow.stateDisposing);

		// Token: 0x04002554 RID: 9556
		private static readonly int stateInitialized = BitVector32.CreateMask(ToolStripPanelRow.stateLocked);

		// Token: 0x04002555 RID: 9557
		private static readonly int stateCachedBoundsMode = BitVector32.CreateMask(ToolStripPanelRow.stateInitialized);

		// Token: 0x04002556 RID: 9558
		private static readonly int stateInLayout = BitVector32.CreateMask(ToolStripPanelRow.stateCachedBoundsMode);

		// Token: 0x04002557 RID: 9559
		private static readonly int PropControlsCollection = PropertyStore.CreateKey();

		// Token: 0x04002558 RID: 9560
		internal static TraceSwitch ToolStripPanelRowCreationDebug;

		// Token: 0x04002559 RID: 9561
		internal static readonly TraceSwitch ToolStripPanelMouseDebug;

		// Token: 0x02000746 RID: 1862
		private abstract class ToolStripPanelRowManager
		{
			// Token: 0x06006193 RID: 24979 RVA: 0x0018F048 File Offset: 0x0018D248
			public ToolStripPanelRowManager(ToolStripPanelRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x06006194 RID: 24980 RVA: 0x0018F058 File Offset: 0x0018D258
			public virtual bool CanMove(ToolStrip toolStripToDrag)
			{
				if (toolStripToDrag != null && ((ISupportToolStripPanel)toolStripToDrag).Stretch)
				{
					return false;
				}
				foreach (object obj in this.Row.ControlsInternal)
				{
					Control control = (Control)obj;
					ISupportToolStripPanel supportToolStripPanel = control as ISupportToolStripPanel;
					if (supportToolStripPanel != null && supportToolStripPanel.Stretch)
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x1700174C RID: 5964
			// (get) Token: 0x06006195 RID: 24981 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
			public virtual Rectangle DragBounds
			{
				get
				{
					return Rectangle.Empty;
				}
			}

			// Token: 0x1700174D RID: 5965
			// (get) Token: 0x06006196 RID: 24982 RVA: 0x0004BFF9 File Offset: 0x0004A1F9
			public virtual Rectangle DisplayRectangle
			{
				get
				{
					return Rectangle.Empty;
				}
			}

			// Token: 0x1700174E RID: 5966
			// (get) Token: 0x06006197 RID: 24983 RVA: 0x0018F0DC File Offset: 0x0018D2DC
			public ToolStripPanel ToolStripPanel
			{
				get
				{
					return this.owner.ToolStripPanel;
				}
			}

			// Token: 0x1700174F RID: 5967
			// (get) Token: 0x06006198 RID: 24984 RVA: 0x0018F0E9 File Offset: 0x0018D2E9
			public ToolStripPanelRow Row
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x17001750 RID: 5968
			// (get) Token: 0x06006199 RID: 24985 RVA: 0x0018F0F1 File Offset: 0x0018D2F1
			public FlowLayoutSettings FlowLayoutSettings
			{
				get
				{
					if (this.flowLayoutSettings == null)
					{
						this.flowLayoutSettings = new FlowLayoutSettings(this.owner);
					}
					return this.flowLayoutSettings;
				}
			}

			// Token: 0x0600619A RID: 24986 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			protected internal virtual int FreeSpaceFromRow(int spaceToFree)
			{
				return 0;
			}

			// Token: 0x0600619B RID: 24987 RVA: 0x0018F114 File Offset: 0x0018D314
			protected virtual int Grow(int index, int growBy)
			{
				int result = 0;
				if (index >= 0 && index < this.Row.ControlsInternal.Count - 1)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)this.Row.Cells[index];
					if (toolStripPanelCell.Visible)
					{
						result = toolStripPanelCell.Grow(growBy);
					}
				}
				return result;
			}

			// Token: 0x0600619C RID: 24988 RVA: 0x0018F164 File Offset: 0x0018D364
			public ToolStripPanelCell GetNextVisibleCell(int index, bool forward)
			{
				if (forward)
				{
					for (int i = index; i < this.Row.Cells.Count; i++)
					{
						ToolStripPanelCell toolStripPanelCell = this.Row.Cells[i] as ToolStripPanelCell;
						if ((toolStripPanelCell.Visible || (this.owner.parent.Visible && toolStripPanelCell.ControlInDesignMode)) && toolStripPanelCell.ToolStripPanelRow == this.owner)
						{
							return toolStripPanelCell;
						}
					}
				}
				else
				{
					for (int j = index; j >= 0; j--)
					{
						ToolStripPanelCell toolStripPanelCell2 = this.Row.Cells[j] as ToolStripPanelCell;
						if ((toolStripPanelCell2.Visible || (this.owner.parent.Visible && toolStripPanelCell2.ControlInDesignMode)) && toolStripPanelCell2.ToolStripPanelRow == this.owner)
						{
							return toolStripPanelCell2;
						}
					}
				}
				return null;
			}

			// Token: 0x0600619D RID: 24989 RVA: 0x0018F230 File Offset: 0x0018D430
			protected virtual int GrowControlsAfter(int index, int growBy)
			{
				if (growBy < 0)
				{
					return 0;
				}
				int num = growBy;
				for (int i = index + 1; i < this.Row.ControlsInternal.Count; i++)
				{
					int num2 = this.Grow(i, num);
					if (num2 >= 0)
					{
						num -= num2;
						if (num <= 0)
						{
							return growBy;
						}
					}
				}
				return growBy - num;
			}

			// Token: 0x0600619E RID: 24990 RVA: 0x0018F27C File Offset: 0x0018D47C
			protected virtual int GrowControlsBefore(int index, int growBy)
			{
				if (growBy < 0)
				{
					return 0;
				}
				int num = growBy;
				for (int i = index - 1; i >= 0; i--)
				{
					num -= this.Grow(i, num);
					if (num <= 0)
					{
						return growBy;
					}
				}
				return growBy - num;
			}

			// Token: 0x0600619F RID: 24991 RVA: 0x0000701A File Offset: 0x0000521A
			public virtual void MoveControl(ToolStrip movingControl, Point startClientLocation, Point endClientLocation)
			{
			}

			// Token: 0x060061A0 RID: 24992 RVA: 0x0000701A File Offset: 0x0000521A
			public virtual void LeaveRow(ToolStrip toolStripToDrag)
			{
			}

			// Token: 0x060061A1 RID: 24993 RVA: 0x0000701A File Offset: 0x0000521A
			public virtual void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
			}

			// Token: 0x060061A2 RID: 24994 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal virtual void OnControlAdded(Control c, int index)
			{
			}

			// Token: 0x060061A3 RID: 24995 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal virtual void OnControlRemoved(Control c, int index)
			{
			}

			// Token: 0x060061A4 RID: 24996 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal virtual void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
			}

			// Token: 0x0400419E RID: 16798
			private FlowLayoutSettings flowLayoutSettings;

			// Token: 0x0400419F RID: 16799
			private ToolStripPanelRow owner;
		}

		// Token: 0x02000747 RID: 1863
		private class HorizontalRowManager : ToolStripPanelRow.ToolStripPanelRowManager
		{
			// Token: 0x060061A5 RID: 24997 RVA: 0x0018F2B3 File Offset: 0x0018D4B3
			public HorizontalRowManager(ToolStripPanelRow owner) : base(owner)
			{
				owner.SuspendLayout();
				base.FlowLayoutSettings.WrapContents = false;
				base.FlowLayoutSettings.FlowDirection = FlowDirection.LeftToRight;
				owner.ResumeLayout(false);
			}

			// Token: 0x17001751 RID: 5969
			// (get) Token: 0x060061A6 RID: 24998 RVA: 0x0018F2E4 File Offset: 0x0018D4E4
			public override Rectangle DisplayRectangle
			{
				get
				{
					Rectangle displayRectangle = ((IArrangedElement)base.Row).DisplayRectangle;
					if (base.ToolStripPanel != null)
					{
						Rectangle displayRectangle2 = base.ToolStripPanel.DisplayRectangle;
						if ((!base.ToolStripPanel.Visible || LayoutUtils.IsZeroWidthOrHeight(displayRectangle2)) && base.ToolStripPanel.ParentInternal != null)
						{
							displayRectangle.Width = base.ToolStripPanel.ParentInternal.DisplayRectangle.Width - (base.ToolStripPanel.Margin.Horizontal + base.ToolStripPanel.Padding.Horizontal) - base.Row.Margin.Horizontal;
						}
						else
						{
							displayRectangle.Width = displayRectangle2.Width - base.Row.Margin.Horizontal;
						}
					}
					return displayRectangle;
				}
			}

			// Token: 0x17001752 RID: 5970
			// (get) Token: 0x060061A7 RID: 24999 RVA: 0x0018F3B8 File Offset: 0x0018D5B8
			public override Rectangle DragBounds
			{
				get
				{
					Rectangle bounds = base.Row.Bounds;
					int num = base.ToolStripPanel.RowsInternal.IndexOf(base.Row);
					if (num > 0)
					{
						Rectangle bounds2 = base.ToolStripPanel.RowsInternal[num - 1].Bounds;
						int num2 = bounds2.Y + bounds2.Height - (bounds2.Height >> 2);
						bounds.Height += bounds.Y - num2;
						bounds.Y = num2;
					}
					if (num < base.ToolStripPanel.RowsInternal.Count - 1)
					{
						Rectangle bounds3 = base.ToolStripPanel.RowsInternal[num + 1].Bounds;
						bounds.Height += (bounds3.Height >> 2) + base.Row.Margin.Bottom + base.ToolStripPanel.RowsInternal[num + 1].Margin.Top;
					}
					bounds.Width += base.Row.Margin.Horizontal + base.ToolStripPanel.Padding.Horizontal + 5;
					bounds.X -= base.Row.Margin.Left + base.ToolStripPanel.Padding.Left + 4;
					return bounds;
				}
			}

			// Token: 0x060061A8 RID: 25000 RVA: 0x0018F530 File Offset: 0x0018D730
			public override bool CanMove(ToolStrip toolStripToDrag)
			{
				if (base.CanMove(toolStripToDrag))
				{
					Size sz = Size.Empty;
					for (int i = 0; i < base.Row.ControlsInternal.Count; i++)
					{
						sz += base.Row.GetMinimumSize(base.Row.ControlsInternal[i] as ToolStrip);
					}
					return (sz + base.Row.GetMinimumSize(toolStripToDrag)).Width < this.DisplayRectangle.Width;
				}
				return false;
			}

			// Token: 0x060061A9 RID: 25001 RVA: 0x0018F5BC File Offset: 0x0018D7BC
			protected internal override int FreeSpaceFromRow(int spaceToFree)
			{
				int num = spaceToFree;
				if (spaceToFree > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					if (nextVisibleCell == null)
					{
						return 0;
					}
					Padding margin = nextVisibleCell.Margin;
					if (margin.Left >= spaceToFree)
					{
						margin.Left -= spaceToFree;
						margin.Right = 0;
						spaceToFree = 0;
					}
					else
					{
						spaceToFree -= nextVisibleCell.Margin.Left;
						margin.Left = 0;
						margin.Right = 0;
					}
					nextVisibleCell.Margin = margin;
					spaceToFree -= this.MoveLeft(base.Row.Cells.Count - 1, spaceToFree);
					if (spaceToFree > 0)
					{
						spaceToFree -= nextVisibleCell.Shrink(spaceToFree);
					}
				}
				return num - Math.Max(0, spaceToFree);
			}

			// Token: 0x060061AA RID: 25002 RVA: 0x0018F67C File Offset: 0x0018D87C
			public override void MoveControl(ToolStrip movingControl, Point clientStartLocation, Point clientEndLocation)
			{
				if (base.Row.Locked)
				{
					return;
				}
				if (!this.DragBounds.Contains(clientEndLocation))
				{
					base.MoveControl(movingControl, clientStartLocation, clientEndLocation);
					return;
				}
				int index = base.Row.ControlsInternal.IndexOf(movingControl);
				int num = clientEndLocation.X - clientStartLocation.X;
				if (num < 0)
				{
					this.MoveLeft(index, num * -1);
					return;
				}
				this.MoveRight(index, num);
			}

			// Token: 0x060061AB RID: 25003 RVA: 0x0018F6F0 File Offset: 0x0018D8F0
			private int MoveLeft(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0)
					{
						return 0;
					}
					for (int i = index; i >= 0; i--)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Horizontal >= num2)
							{
								num += num2;
								margin.Left -= num2;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
							}
							else
							{
								num += toolStripPanelCell.Margin.Horizontal;
								margin.Left = 0;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
							}
							if (num >= spaceToFree)
							{
								if (index + 1 < base.Row.Cells.Count)
								{
									toolStripPanelCell = base.GetNextVisibleCell(index + 1, true);
									if (toolStripPanelCell != null)
									{
										margin = toolStripPanelCell.Margin;
										margin.Left += spaceToFree;
										toolStripPanelCell.Margin = margin;
									}
								}
								return spaceToFree;
							}
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x060061AC RID: 25004 RVA: 0x0018F82C File Offset: 0x0018DA2C
			private int MoveRight(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0 || index >= base.Row.ControlsInternal.Count)
					{
						return 0;
					}
					int i = index + 1;
					while (i < base.Row.Cells.Count)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Horizontal >= num2)
							{
								num += num2;
								margin.Left -= num2;
								margin.Right = 0;
								toolStripPanelCell.Margin = margin;
								break;
							}
							num += toolStripPanelCell.Margin.Horizontal;
							margin.Left = 0;
							margin.Right = 0;
							toolStripPanelCell.Margin = margin;
							break;
						}
						else
						{
							i++;
						}
					}
					if (base.Row.Cells.Count > 0 && spaceToFree > num)
					{
						ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						if (nextVisibleCell != null)
						{
							num += this.DisplayRectangle.Right - nextVisibleCell.Bounds.Right;
						}
						else
						{
							num += this.DisplayRectangle.Width;
						}
					}
					if (spaceToFree <= num)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell == null)
						{
							toolStripPanelCell = (base.Row.Cells[index] as ToolStripPanelCell);
						}
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Left += spaceToFree;
							toolStripPanelCell.Margin = margin;
						}
						return spaceToFree;
					}
					for (int j = index + 1; j < base.Row.Cells.Count; j++)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[j];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int shrinkBy = spaceToFree - num;
							num += toolStripPanelCell.Shrink(shrinkBy);
							if (spaceToFree >= num)
							{
								base.Row.ResumeLayout(true);
								return spaceToFree;
							}
						}
					}
					if (base.Row.Cells.Count == 1)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Left += num;
							toolStripPanelCell.Margin = margin;
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x060061AD RID: 25005 RVA: 0x0018FAB0 File Offset: 0x0018DCB0
			public override void LeaveRow(ToolStrip toolStripToDrag)
			{
				base.Row.SuspendLayout();
				int num = base.Row.ControlsInternal.IndexOf(toolStripToDrag);
				if (num >= 0)
				{
					if (num < base.Row.ControlsInternal.Count - 1)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[num];
						if (toolStripPanelCell.Visible)
						{
							int num2 = toolStripPanelCell.Margin.Horizontal + toolStripPanelCell.Bounds.Width;
							ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(num + 1, true);
							if (nextVisibleCell != null)
							{
								Padding margin = nextVisibleCell.Margin;
								margin.Left += num2;
								nextVisibleCell.Margin = margin;
							}
						}
					}
					((IList)base.Row.Cells).RemoveAt(num);
				}
				base.Row.ResumeLayout(true);
			}

			// Token: 0x060061AE RID: 25006 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal override void OnControlAdded(Control control, int index)
			{
			}

			// Token: 0x060061AF RID: 25007 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal override void OnControlRemoved(Control control, int index)
			{
			}

			// Token: 0x060061B0 RID: 25008 RVA: 0x0018FB80 File Offset: 0x0018DD80
			public override void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
				if (!base.Row.ControlsInternal.Contains(toolStripToDrag))
				{
					base.Row.SuspendLayout();
					try
					{
						if (base.Row.ControlsInternal.Count > 0)
						{
							int i;
							for (i = 0; i < base.Row.Cells.Count; i++)
							{
								ToolStripPanelCell toolStripPanelCell = base.Row.Cells[i] as ToolStripPanelCell;
								if ((toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode) && (base.Row.Cells[i].Bounds.Contains(locationToDrag) || base.Row.Cells[i].Bounds.X >= locationToDrag.X))
								{
									break;
								}
							}
							Control control = base.Row.ControlsInternal[i];
							if (i < base.Row.ControlsInternal.Count)
							{
								base.Row.ControlsInternal.Insert(i, toolStripToDrag);
							}
							else
							{
								base.Row.ControlsInternal.Add(toolStripToDrag);
							}
							int num = toolStripToDrag.AutoSize ? toolStripToDrag.PreferredSize.Width : toolStripToDrag.Width;
							int num2 = num;
							if (i == 0)
							{
								num2 += locationToDrag.X;
							}
							int num3 = 0;
							if (i < base.Row.ControlsInternal.Count - 1)
							{
								ToolStripPanelCell toolStripPanelCell2 = (ToolStripPanelCell)base.Row.Cells[i + 1];
								Padding margin = toolStripPanelCell2.Margin;
								if (margin.Left > num2)
								{
									margin.Left -= num2;
									toolStripPanelCell2.Margin = margin;
									num3 = num2;
								}
								else
								{
									num3 = this.MoveRight(i + 1, num2 - num3);
									if (num3 > 0)
									{
										margin = toolStripPanelCell2.Margin;
										margin.Left = Math.Max(0, margin.Left - num3);
										toolStripPanelCell2.Margin = margin;
									}
								}
							}
							else
							{
								ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 2, false);
								ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell != null && nextVisibleCell2 != null)
								{
									Padding margin2 = nextVisibleCell2.Margin;
									margin2.Left = Math.Max(0, locationToDrag.X - nextVisibleCell.Bounds.Right);
									nextVisibleCell2.Margin = margin2;
									num3 = num2;
								}
							}
							if (num3 < num2 && i > 0)
							{
								num3 = this.MoveLeft(i - 1, num2 - num3);
							}
							if (i == 0 && num3 - num > 0)
							{
								ToolStripPanelCell toolStripPanelCell3 = base.Row.Cells[i] as ToolStripPanelCell;
								Padding margin3 = toolStripPanelCell3.Margin;
								margin3.Left = num3 - num;
								toolStripPanelCell3.Margin = margin3;
							}
						}
						else
						{
							base.Row.ControlsInternal.Add(toolStripToDrag);
							if (base.Row.Cells.Count > 0 || toolStripToDrag.IsInDesignMode)
							{
								ToolStripPanelCell toolStripPanelCell4 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (toolStripPanelCell4 == null && toolStripToDrag.IsInDesignMode)
								{
									toolStripPanelCell4 = (ToolStripPanelCell)base.Row.Cells[base.Row.Cells.Count - 1];
								}
								if (toolStripPanelCell4 != null)
								{
									Padding margin4 = toolStripPanelCell4.Margin;
									margin4.Left = Math.Max(0, locationToDrag.X - base.Row.Margin.Left);
									toolStripPanelCell4.Margin = margin4;
								}
							}
						}
					}
					finally
					{
						base.Row.ResumeLayout(true);
					}
				}
			}

			// Token: 0x060061B1 RID: 25009 RVA: 0x0018FF40 File Offset: 0x0018E140
			protected internal override void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
				base.OnBoundsChanged(oldBounds, newBounds);
			}

			// Token: 0x040041A0 RID: 16800
			private const int DRAG_BOUNDS_INFLATE = 4;
		}

		// Token: 0x02000748 RID: 1864
		private class VerticalRowManager : ToolStripPanelRow.ToolStripPanelRowManager
		{
			// Token: 0x060061B2 RID: 25010 RVA: 0x0018FF4A File Offset: 0x0018E14A
			public VerticalRowManager(ToolStripPanelRow owner) : base(owner)
			{
				owner.SuspendLayout();
				base.FlowLayoutSettings.WrapContents = false;
				base.FlowLayoutSettings.FlowDirection = FlowDirection.TopDown;
				owner.ResumeLayout(false);
			}

			// Token: 0x17001753 RID: 5971
			// (get) Token: 0x060061B3 RID: 25011 RVA: 0x0018FF78 File Offset: 0x0018E178
			public override Rectangle DisplayRectangle
			{
				get
				{
					Rectangle displayRectangle = ((IArrangedElement)base.Row).DisplayRectangle;
					if (base.ToolStripPanel != null)
					{
						Rectangle displayRectangle2 = base.ToolStripPanel.DisplayRectangle;
						if ((!base.ToolStripPanel.Visible || LayoutUtils.IsZeroWidthOrHeight(displayRectangle2)) && base.ToolStripPanel.ParentInternal != null)
						{
							displayRectangle.Height = base.ToolStripPanel.ParentInternal.DisplayRectangle.Height - (base.ToolStripPanel.Margin.Vertical + base.ToolStripPanel.Padding.Vertical) - base.Row.Margin.Vertical;
						}
						else
						{
							displayRectangle.Height = displayRectangle2.Height - base.Row.Margin.Vertical;
						}
					}
					return displayRectangle;
				}
			}

			// Token: 0x17001754 RID: 5972
			// (get) Token: 0x060061B4 RID: 25012 RVA: 0x0019004C File Offset: 0x0018E24C
			public override Rectangle DragBounds
			{
				get
				{
					Rectangle bounds = base.Row.Bounds;
					int num = base.ToolStripPanel.RowsInternal.IndexOf(base.Row);
					if (num > 0)
					{
						Rectangle bounds2 = base.ToolStripPanel.RowsInternal[num - 1].Bounds;
						int num2 = bounds2.X + bounds2.Width - (bounds2.Width >> 2);
						bounds.Width += bounds.X - num2;
						bounds.X = num2;
					}
					if (num < base.ToolStripPanel.RowsInternal.Count - 1)
					{
						Rectangle bounds3 = base.ToolStripPanel.RowsInternal[num + 1].Bounds;
						bounds.Width += (bounds3.Width >> 2) + base.Row.Margin.Right + base.ToolStripPanel.RowsInternal[num + 1].Margin.Left;
					}
					bounds.Height += base.Row.Margin.Vertical + base.ToolStripPanel.Padding.Vertical + 5;
					bounds.Y -= base.Row.Margin.Top + base.ToolStripPanel.Padding.Top + 4;
					return bounds;
				}
			}

			// Token: 0x060061B5 RID: 25013 RVA: 0x001901C4 File Offset: 0x0018E3C4
			public override bool CanMove(ToolStrip toolStripToDrag)
			{
				if (base.CanMove(toolStripToDrag))
				{
					Size sz = Size.Empty;
					for (int i = 0; i < base.Row.ControlsInternal.Count; i++)
					{
						sz += base.Row.GetMinimumSize(base.Row.ControlsInternal[i] as ToolStrip);
					}
					return (sz + base.Row.GetMinimumSize(toolStripToDrag)).Height < this.DisplayRectangle.Height;
				}
				return false;
			}

			// Token: 0x060061B6 RID: 25014 RVA: 0x00190250 File Offset: 0x0018E450
			protected internal override int FreeSpaceFromRow(int spaceToFree)
			{
				int num = spaceToFree;
				if (spaceToFree > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					if (nextVisibleCell == null)
					{
						return 0;
					}
					Padding margin = nextVisibleCell.Margin;
					if (margin.Top >= spaceToFree)
					{
						margin.Top -= spaceToFree;
						margin.Bottom = 0;
						spaceToFree = 0;
					}
					else
					{
						spaceToFree -= nextVisibleCell.Margin.Top;
						margin.Top = 0;
						margin.Bottom = 0;
					}
					nextVisibleCell.Margin = margin;
					spaceToFree -= this.MoveUp(base.Row.Cells.Count - 1, spaceToFree);
					if (spaceToFree > 0)
					{
						spaceToFree -= nextVisibleCell.Shrink(spaceToFree);
					}
				}
				return num - Math.Max(0, spaceToFree);
			}

			// Token: 0x060061B7 RID: 25015 RVA: 0x00190310 File Offset: 0x0018E510
			public override void MoveControl(ToolStrip movingControl, Point clientStartLocation, Point clientEndLocation)
			{
				if (base.Row.Locked)
				{
					return;
				}
				if (!this.DragBounds.Contains(clientEndLocation))
				{
					base.MoveControl(movingControl, clientStartLocation, clientEndLocation);
					return;
				}
				int index = base.Row.ControlsInternal.IndexOf(movingControl);
				int num = clientEndLocation.Y - clientStartLocation.Y;
				if (num < 0)
				{
					this.MoveUp(index, num * -1);
					return;
				}
				this.MoveDown(index, num);
			}

			// Token: 0x060061B8 RID: 25016 RVA: 0x00190384 File Offset: 0x0018E584
			private int MoveUp(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0)
					{
						return 0;
					}
					for (int i = index; i >= 0; i--)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Vertical >= num2)
							{
								num += num2;
								margin.Top -= num2;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
							}
							else
							{
								num += toolStripPanelCell.Margin.Vertical;
								margin.Top = 0;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
							}
							if (num >= spaceToFree)
							{
								if (index + 1 < base.Row.Cells.Count)
								{
									toolStripPanelCell = base.GetNextVisibleCell(index + 1, true);
									if (toolStripPanelCell != null)
									{
										margin = toolStripPanelCell.Margin;
										margin.Top += spaceToFree;
										toolStripPanelCell.Margin = margin;
									}
								}
								return spaceToFree;
							}
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return num;
			}

			// Token: 0x060061B9 RID: 25017 RVA: 0x001904C0 File Offset: 0x0018E6C0
			private int MoveDown(int index, int spaceToFree)
			{
				int num = 0;
				base.Row.SuspendLayout();
				try
				{
					if (spaceToFree == 0 || index < 0 || index >= base.Row.ControlsInternal.Count)
					{
						return 0;
					}
					int i = index + 1;
					while (i < base.Row.Cells.Count)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[i];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int num2 = spaceToFree - num;
							Padding margin = toolStripPanelCell.Margin;
							if (margin.Vertical >= num2)
							{
								num += num2;
								margin.Top -= num2;
								margin.Bottom = 0;
								toolStripPanelCell.Margin = margin;
								break;
							}
							num += toolStripPanelCell.Margin.Vertical;
							margin.Top = 0;
							margin.Bottom = 0;
							toolStripPanelCell.Margin = margin;
							break;
						}
						else
						{
							i++;
						}
					}
					if (base.Row.Cells.Count > 0 && spaceToFree > num)
					{
						ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						if (nextVisibleCell != null)
						{
							num += this.DisplayRectangle.Bottom - nextVisibleCell.Bounds.Bottom;
						}
						else
						{
							num += this.DisplayRectangle.Height;
						}
					}
					if (spaceToFree <= num)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[index];
						Padding margin = toolStripPanelCell.Margin;
						margin.Top += spaceToFree;
						toolStripPanelCell.Margin = margin;
						return spaceToFree;
					}
					for (int j = index + 1; j < base.Row.Cells.Count; j++)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[j];
						if (toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode)
						{
							int shrinkBy = spaceToFree - num;
							num += toolStripPanelCell.Shrink(shrinkBy);
							if (spaceToFree >= num)
							{
								base.Row.ResumeLayout(true);
								return spaceToFree;
							}
						}
					}
					if (base.Row.Cells.Count == 1)
					{
						ToolStripPanelCell toolStripPanelCell = base.GetNextVisibleCell(index, true);
						if (toolStripPanelCell != null)
						{
							Padding margin = toolStripPanelCell.Margin;
							margin.Top += num;
							toolStripPanelCell.Margin = margin;
						}
					}
				}
				finally
				{
					base.Row.ResumeLayout(true);
				}
				return spaceToFree - num;
			}

			// Token: 0x060061BA RID: 25018 RVA: 0x00190740 File Offset: 0x0018E940
			protected internal override void OnBoundsChanged(Rectangle oldBounds, Rectangle newBounds)
			{
				base.OnBoundsChanged(oldBounds, newBounds);
				if (base.Row.Cells.Count > 0)
				{
					ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
					int num = (nextVisibleCell != null) ? (nextVisibleCell.Bounds.Bottom - newBounds.Height) : 0;
					if (num > 0)
					{
						ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
						Padding margin = nextVisibleCell2.Margin;
						if (margin.Top >= num)
						{
							margin.Top -= num;
							margin.Bottom = 0;
							nextVisibleCell2.Margin = margin;
							num = 0;
						}
						else
						{
							num -= nextVisibleCell2.Margin.Top;
							margin.Top = 0;
							margin.Bottom = 0;
							nextVisibleCell2.Margin = margin;
						}
						num -= nextVisibleCell2.Shrink(num);
						this.MoveUp(base.Row.Cells.Count - 1, num);
					}
				}
			}

			// Token: 0x060061BB RID: 25019 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal override void OnControlRemoved(Control c, int index)
			{
			}

			// Token: 0x060061BC RID: 25020 RVA: 0x0000701A File Offset: 0x0000521A
			protected internal override void OnControlAdded(Control control, int index)
			{
			}

			// Token: 0x060061BD RID: 25021 RVA: 0x00190848 File Offset: 0x0018EA48
			public override void JoinRow(ToolStrip toolStripToDrag, Point locationToDrag)
			{
				if (!base.Row.ControlsInternal.Contains(toolStripToDrag))
				{
					base.Row.SuspendLayout();
					try
					{
						if (base.Row.ControlsInternal.Count > 0)
						{
							int i;
							for (i = 0; i < base.Row.Cells.Count; i++)
							{
								ToolStripPanelCell toolStripPanelCell = base.Row.Cells[i] as ToolStripPanelCell;
								if ((toolStripPanelCell.Visible || toolStripPanelCell.ControlInDesignMode) && (toolStripPanelCell.Bounds.Contains(locationToDrag) || toolStripPanelCell.Bounds.Y >= locationToDrag.Y))
								{
									break;
								}
							}
							Control control = base.Row.ControlsInternal[i];
							if (i < base.Row.ControlsInternal.Count)
							{
								base.Row.ControlsInternal.Insert(i, toolStripToDrag);
							}
							else
							{
								base.Row.ControlsInternal.Add(toolStripToDrag);
							}
							int num = toolStripToDrag.AutoSize ? toolStripToDrag.PreferredSize.Height : toolStripToDrag.Height;
							int num2 = num;
							if (i == 0)
							{
								num2 += locationToDrag.Y;
							}
							int num3 = 0;
							if (i < base.Row.ControlsInternal.Count - 1)
							{
								ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(i + 1, true);
								if (nextVisibleCell != null)
								{
									Padding margin = nextVisibleCell.Margin;
									if (margin.Top > num2)
									{
										margin.Top -= num2;
										nextVisibleCell.Margin = margin;
										num3 = num2;
									}
									else
									{
										num3 = this.MoveDown(i + 1, num2 - num3);
										if (num3 > 0)
										{
											margin = nextVisibleCell.Margin;
											margin.Top -= num3;
											nextVisibleCell.Margin = margin;
										}
									}
								}
							}
							else
							{
								ToolStripPanelCell nextVisibleCell2 = base.GetNextVisibleCell(base.Row.Cells.Count - 2, false);
								ToolStripPanelCell nextVisibleCell3 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell2 != null && nextVisibleCell3 != null)
								{
									Padding margin2 = nextVisibleCell3.Margin;
									margin2.Top = Math.Max(0, locationToDrag.Y - nextVisibleCell2.Bounds.Bottom);
									nextVisibleCell3.Margin = margin2;
									num3 = num2;
								}
							}
							if (num3 < num2 && i > 0)
							{
								num3 = this.MoveUp(i - 1, num2 - num3);
							}
							if (i == 0 && num3 - num > 0)
							{
								ToolStripPanelCell toolStripPanelCell2 = base.Row.Cells[i] as ToolStripPanelCell;
								Padding margin3 = toolStripPanelCell2.Margin;
								margin3.Top = num3 - num;
								toolStripPanelCell2.Margin = margin3;
							}
						}
						else
						{
							base.Row.ControlsInternal.Add(toolStripToDrag);
							if (base.Row.Cells.Count > 0)
							{
								ToolStripPanelCell nextVisibleCell4 = base.GetNextVisibleCell(base.Row.Cells.Count - 1, false);
								if (nextVisibleCell4 != null)
								{
									Padding margin4 = nextVisibleCell4.Margin;
									margin4.Top = Math.Max(0, locationToDrag.Y - base.Row.Margin.Top);
									nextVisibleCell4.Margin = margin4;
								}
							}
						}
					}
					finally
					{
						base.Row.ResumeLayout(true);
					}
				}
			}

			// Token: 0x060061BE RID: 25022 RVA: 0x00190B98 File Offset: 0x0018ED98
			public override void LeaveRow(ToolStrip toolStripToDrag)
			{
				base.Row.SuspendLayout();
				int num = base.Row.ControlsInternal.IndexOf(toolStripToDrag);
				if (num >= 0)
				{
					if (num < base.Row.ControlsInternal.Count - 1)
					{
						ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.Row.Cells[num];
						if (toolStripPanelCell.Visible)
						{
							int num2 = toolStripPanelCell.Margin.Vertical + toolStripPanelCell.Bounds.Height;
							ToolStripPanelCell nextVisibleCell = base.GetNextVisibleCell(num + 1, true);
							if (nextVisibleCell != null)
							{
								Padding margin = nextVisibleCell.Margin;
								margin.Top += num2;
								nextVisibleCell.Margin = margin;
							}
						}
					}
					((IList)base.Row.Cells).RemoveAt(num);
				}
				base.Row.ResumeLayout(true);
			}

			// Token: 0x040041A1 RID: 16801
			private const int DRAG_BOUNDS_INFLATE = 4;
		}

		// Token: 0x02000749 RID: 1865
		internal class ToolStripPanelRowControlCollection : ArrangedElementCollection, IList, ICollection, IEnumerable
		{
			// Token: 0x060061BF RID: 25023 RVA: 0x00190C67 File Offset: 0x0018EE67
			public ToolStripPanelRowControlCollection(ToolStripPanelRow owner)
			{
				this.owner = owner;
			}

			// Token: 0x060061C0 RID: 25024 RVA: 0x00190C76 File Offset: 0x0018EE76
			public ToolStripPanelRowControlCollection(ToolStripPanelRow owner, Control[] value)
			{
				this.owner = owner;
				this.AddRange(value);
			}

			// Token: 0x17001755 RID: 5973
			public virtual Control this[int index]
			{
				get
				{
					return this.GetControl(index);
				}
			}

			// Token: 0x17001756 RID: 5974
			// (get) Token: 0x060061C2 RID: 25026 RVA: 0x00190C95 File Offset: 0x0018EE95
			public ArrangedElementCollection Cells
			{
				get
				{
					if (this.cellCollection == null)
					{
						this.cellCollection = new ArrangedElementCollection(base.InnerList);
					}
					return this.cellCollection;
				}
			}

			// Token: 0x17001757 RID: 5975
			// (get) Token: 0x060061C3 RID: 25027 RVA: 0x00190CB6 File Offset: 0x0018EEB6
			public ToolStripPanel ToolStripPanel
			{
				get
				{
					return this.owner.ToolStripPanel;
				}
			}

			// Token: 0x060061C4 RID: 25028 RVA: 0x00190CC4 File Offset: 0x0018EEC4
			[EditorBrowsable(EditorBrowsableState.Never)]
			public int Add(Control value)
			{
				ISupportToolStripPanel supportToolStripPanel = value as ISupportToolStripPanel;
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (supportToolStripPanel == null)
				{
					throw new NotSupportedException(SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						typeof(ToolStrip).Name
					}));
				}
				int num = base.InnerList.Add(supportToolStripPanel.ToolStripPanelCell);
				this.OnAdd(supportToolStripPanel, num);
				return num;
			}

			// Token: 0x060061C5 RID: 25029 RVA: 0x00190D2C File Offset: 0x0018EF2C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void AddRange(Control[] value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ToolStripPanel toolStripPanel = this.ToolStripPanel;
				if (toolStripPanel != null)
				{
					toolStripPanel.SuspendLayout();
				}
				try
				{
					for (int i = 0; i < value.Length; i++)
					{
						this.Add(value[i]);
					}
				}
				finally
				{
					if (toolStripPanel != null)
					{
						toolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x060061C6 RID: 25030 RVA: 0x00190D8C File Offset: 0x0018EF8C
			public bool Contains(Control value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.GetControl(i) == value)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x060061C7 RID: 25031 RVA: 0x00190DB8 File Offset: 0x0018EFB8
			public virtual void Clear()
			{
				if (this.owner != null)
				{
					this.ToolStripPanel.SuspendLayout();
				}
				try
				{
					while (this.Count != 0)
					{
						this.RemoveAt(this.Count - 1);
					}
				}
				finally
				{
					if (this.owner != null)
					{
						this.ToolStripPanel.ResumeLayout();
					}
				}
			}

			// Token: 0x060061C8 RID: 25032 RVA: 0x00190E18 File Offset: 0x0018F018
			public override IEnumerator GetEnumerator()
			{
				return new ToolStripPanelRow.ToolStripPanelRowControlCollection.ToolStripPanelCellToControlEnumerator(base.InnerList);
			}

			// Token: 0x060061C9 RID: 25033 RVA: 0x00190E28 File Offset: 0x0018F028
			private Control GetControl(int index)
			{
				Control result = null;
				if (index < this.Count && index >= 0)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.InnerList[index];
					result = ((toolStripPanelCell != null) ? toolStripPanelCell.Control : null);
				}
				return result;
			}

			// Token: 0x060061CA RID: 25034 RVA: 0x00190E68 File Offset: 0x0018F068
			private int IndexOfControl(Control c)
			{
				for (int i = 0; i < this.Count; i++)
				{
					ToolStripPanelCell toolStripPanelCell = (ToolStripPanelCell)base.InnerList[i];
					if (toolStripPanelCell.Control == c)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060061CB RID: 25035 RVA: 0x00190EA4 File Offset: 0x0018F0A4
			void IList.Clear()
			{
				this.Clear();
			}

			// Token: 0x17001758 RID: 5976
			// (get) Token: 0x060061CC RID: 25036 RVA: 0x00115FFC File Offset: 0x001141FC
			bool IList.IsFixedSize
			{
				get
				{
					return base.InnerList.IsFixedSize;
				}
			}

			// Token: 0x060061CD RID: 25037 RVA: 0x00115D88 File Offset: 0x00113F88
			bool IList.Contains(object value)
			{
				return base.InnerList.Contains(value);
			}

			// Token: 0x17001759 RID: 5977
			// (get) Token: 0x060061CE RID: 25038 RVA: 0x001573CB File Offset: 0x001555CB
			bool IList.IsReadOnly
			{
				get
				{
					return base.InnerList.IsReadOnly;
				}
			}

			// Token: 0x060061CF RID: 25039 RVA: 0x00190EAC File Offset: 0x0018F0AC
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			// Token: 0x060061D0 RID: 25040 RVA: 0x00190EB5 File Offset: 0x0018F0B5
			void IList.Remove(object value)
			{
				this.Remove(value as Control);
			}

			// Token: 0x060061D1 RID: 25041 RVA: 0x00190EC3 File Offset: 0x0018F0C3
			int IList.Add(object value)
			{
				return this.Add(value as Control);
			}

			// Token: 0x060061D2 RID: 25042 RVA: 0x00190ED1 File Offset: 0x0018F0D1
			int IList.IndexOf(object value)
			{
				return this.IndexOf(value as Control);
			}

			// Token: 0x060061D3 RID: 25043 RVA: 0x00190EDF File Offset: 0x0018F0DF
			void IList.Insert(int index, object value)
			{
				this.Insert(index, value as Control);
			}

			// Token: 0x060061D4 RID: 25044 RVA: 0x00190EF0 File Offset: 0x0018F0F0
			public int IndexOf(Control value)
			{
				for (int i = 0; i < this.Count; i++)
				{
					if (this.GetControl(i) == value)
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x060061D5 RID: 25045 RVA: 0x00190F1C File Offset: 0x0018F11C
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Insert(int index, Control value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				ISupportToolStripPanel supportToolStripPanel = value as ISupportToolStripPanel;
				if (supportToolStripPanel == null)
				{
					throw new NotSupportedException(SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						typeof(ToolStrip).Name
					}));
				}
				base.InnerList.Insert(index, supportToolStripPanel.ToolStripPanelCell);
				this.OnAdd(supportToolStripPanel, index);
			}

			// Token: 0x060061D6 RID: 25046 RVA: 0x00190F84 File Offset: 0x0018F184
			private void OnAfterRemove(Control control, int index)
			{
				if (this.owner != null)
				{
					using (new LayoutTransaction(this.ToolStripPanel, control, PropertyNames.Parent))
					{
						this.owner.ToolStripPanel.Controls.Remove(control);
						this.owner.OnControlRemoved(control, index);
					}
				}
			}

			// Token: 0x060061D7 RID: 25047 RVA: 0x00190FEC File Offset: 0x0018F1EC
			private void OnAdd(ISupportToolStripPanel controlToBeDragged, int index)
			{
				if (this.owner != null)
				{
					LayoutTransaction layoutTransaction = null;
					if (this.ToolStripPanel != null && this.ToolStripPanel.ParentInternal != null)
					{
						layoutTransaction = new LayoutTransaction(this.ToolStripPanel, this.ToolStripPanel.ParentInternal, PropertyNames.Parent);
					}
					try
					{
						if (controlToBeDragged != null)
						{
							controlToBeDragged.ToolStripPanelRow = this.owner;
							Control control = controlToBeDragged as Control;
							if (control != null)
							{
								control.ParentInternal = this.owner.ToolStripPanel;
								this.owner.OnControlAdded(control, index);
							}
						}
					}
					finally
					{
						if (layoutTransaction != null)
						{
							layoutTransaction.Dispose();
						}
					}
				}
			}

			// Token: 0x060061D8 RID: 25048 RVA: 0x00191088 File Offset: 0x0018F288
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void Remove(Control value)
			{
				int index = this.IndexOfControl(value);
				this.RemoveAt(index);
			}

			// Token: 0x060061D9 RID: 25049 RVA: 0x001910A4 File Offset: 0x0018F2A4
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void RemoveAt(int index)
			{
				if (index >= 0 && index < this.Count)
				{
					Control control = this.GetControl(index);
					ToolStripPanelCell toolStripPanelCell = base.InnerList[index] as ToolStripPanelCell;
					base.InnerList.RemoveAt(index);
					this.OnAfterRemove(control, index);
				}
			}

			// Token: 0x060061DA RID: 25050 RVA: 0x001910EC File Offset: 0x0018F2EC
			[EditorBrowsable(EditorBrowsableState.Never)]
			public void CopyTo(Control[] array, int index)
			{
				if (array == null)
				{
					throw new ArgumentNullException("array");
				}
				if (index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				if (index >= array.Length || base.InnerList.Count > array.Length - index)
				{
					throw new ArgumentException(SR.GetString("ToolStripPanelRowControlCollectionIncorrectIndexLength"));
				}
				for (int i = 0; i < base.InnerList.Count; i++)
				{
					array[index++] = this.GetControl(i);
				}
			}

			// Token: 0x040041A2 RID: 16802
			private ToolStripPanelRow owner;

			// Token: 0x040041A3 RID: 16803
			private ArrangedElementCollection cellCollection;

			// Token: 0x020008A4 RID: 2212
			private class ToolStripPanelCellToControlEnumerator : IEnumerator, ICloneable
			{
				// Token: 0x060070F0 RID: 28912 RVA: 0x0019CA68 File Offset: 0x0019AC68
				internal ToolStripPanelCellToControlEnumerator(ArrayList list)
				{
					this.arrayListEnumerator = ((IEnumerable)list).GetEnumerator();
				}

				// Token: 0x17001884 RID: 6276
				// (get) Token: 0x060070F1 RID: 28913 RVA: 0x0019CA7C File Offset: 0x0019AC7C
				public virtual object Current
				{
					get
					{
						ToolStripPanelCell toolStripPanelCell = this.arrayListEnumerator.Current as ToolStripPanelCell;
						if (toolStripPanelCell != null)
						{
							return toolStripPanelCell.Control;
						}
						return null;
					}
				}

				// Token: 0x060070F2 RID: 28914 RVA: 0x0019CAA5 File Offset: 0x0019ACA5
				public object Clone()
				{
					return base.MemberwiseClone();
				}

				// Token: 0x060070F3 RID: 28915 RVA: 0x0019CAAD File Offset: 0x0019ACAD
				public virtual bool MoveNext()
				{
					return this.arrayListEnumerator.MoveNext();
				}

				// Token: 0x060070F4 RID: 28916 RVA: 0x0019CABA File Offset: 0x0019ACBA
				public virtual void Reset()
				{
					this.arrayListEnumerator.Reset();
				}

				// Token: 0x0400440B RID: 17419
				private IEnumerator arrayListEnumerator;
			}
		}
	}
}

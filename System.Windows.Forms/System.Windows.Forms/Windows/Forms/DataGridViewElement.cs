using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Provides the base class for elements of a <see cref="T:System.Windows.Forms.DataGridView" /> control.</summary>
	// Token: 0x020001C4 RID: 452
	public class DataGridViewElement
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridViewElement" /> class.</summary>
		// Token: 0x06001D28 RID: 7464 RVA: 0x0009368F File Offset: 0x0009188F
		public DataGridViewElement()
		{
			this.state = DataGridViewElementStates.Visible;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0009369F File Offset: 0x0009189F
		internal DataGridViewElement(DataGridViewElement dgveTemplate)
		{
			this.state = (dgveTemplate.State & (DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.ResizableSet | DataGridViewElementStates.Visible));
		}

		/// <summary>Gets the user interface (UI) state of the element.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewElementStates" /> values representing the state.</returns>
		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x000936B6 File Offset: 0x000918B6
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewElementStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x000936BE File Offset: 0x000918BE
		internal DataGridViewElementStates StateInternal
		{
			set
			{
				this.state = value;
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000936C7 File Offset: 0x000918C7
		internal bool StateIncludes(DataGridViewElementStates elementState)
		{
			return (this.State & elementState) == elementState;
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000936D4 File Offset: 0x000918D4
		internal bool StateExcludes(DataGridViewElementStates elementState)
		{
			return (this.State & elementState) == DataGridViewElementStates.None;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.DataGridView" /> control associated with this element.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.DataGridView" /> control that contains this element. The default is <see langword="null" />.</returns>
		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x000936E1 File Offset: 0x000918E1
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (set) Token: 0x06001D2F RID: 7471 RVA: 0x000936E9 File Offset: 0x000918E9
		internal DataGridView DataGridViewInternal
		{
			set
			{
				if (this.DataGridView != value)
				{
					this.dataGridView = value;
					this.OnDataGridViewChanged();
				}
			}
		}

		/// <summary>Called when the element is associated with a different <see cref="T:System.Windows.Forms.DataGridView" />.</summary>
		// Token: 0x06001D30 RID: 7472 RVA: 0x0000701A File Offset: 0x0000521A
		protected virtual void OnDataGridViewChanged()
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridView.CellClick" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D31 RID: 7473 RVA: 0x00093701 File Offset: 0x00091901
		protected void RaiseCellClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellClickInternal(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridView.CellContentClick" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D32 RID: 7474 RVA: 0x00093717 File Offset: 0x00091917
		protected void RaiseCellContentClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellContentClickInternal(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridView.CellContentDoubleClick" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D33 RID: 7475 RVA: 0x0009372D File Offset: 0x0009192D
		protected void RaiseCellContentDoubleClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellContentDoubleClickInternal(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridView.CellValueChanged" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D34 RID: 7476 RVA: 0x00093743 File Offset: 0x00091943
		protected void RaiseCellValueChanged(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellValueChangedInternal(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.DataGridView.DataError" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewDataErrorEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D35 RID: 7477 RVA: 0x00093759 File Offset: 0x00091959
		protected void RaiseDataError(DataGridViewDataErrorEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnDataErrorInternal(e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x06001D36 RID: 7478 RVA: 0x0009376F File Offset: 0x0009196F
		protected void RaiseMouseWheel(MouseEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnMouseWheelInternal(e);
			}
		}

		// Token: 0x04000D01 RID: 3329
		private DataGridViewElementStates state;

		// Token: 0x04000D02 RID: 3330
		private DataGridView dataGridView;
	}
}

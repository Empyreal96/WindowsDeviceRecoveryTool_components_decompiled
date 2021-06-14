using System;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the <see cref="P:System.Windows.Controls.DataGridColumn.Header" /> of a <see cref="T:System.Windows.Controls.DataGridColumn" /> that is in a <see cref="T:System.Windows.Controls.DataGrid" /> to UI Automation.</summary>
	// Token: 0x020002A2 RID: 674
	public class DataGridColumnHeaderItemAutomationPeer : ItemAutomationPeer, IInvokeProvider, IScrollItemProvider, ITransformProvider, IVirtualizedItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" /> class. </summary>
		/// <param name="item">The <see cref="P:System.Windows.Controls.DataGridColumn.Header" /> in the <see cref="T:System.Windows.Controls.DataGridColumn" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" />.</param>
		/// <param name="column">The column that is associated with the <see cref="P:System.Windows.Controls.DataGridColumn.Header" />.</param>
		/// <param name="peer">The automation peer for the <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeadersPresenter" /> that is associated with the <see cref="T:System.Windows.Controls.DataGrid" />.</param>
		// Token: 0x060025C2 RID: 9666 RVA: 0x000B52FD File Offset: 0x000B34FD
		public DataGridColumnHeaderItemAutomationPeer(object item, DataGridColumn column, DataGridColumnHeadersPresenterAutomationPeer peer) : base(item, peer)
		{
			this._column = column;
		}

		/// <summary>Gets the control type for the header that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.HeaderItem" /> enumeration value.</returns>
		// Token: 0x060025C3 RID: 9667 RVA: 0x00094F9F File Offset: 0x0009319F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		/// <summary>Gets a name that differentiates the header that is represented by this <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>The type name of the <see cref="P:System.Windows.Controls.DataGridColumn.Header" /> property that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" />.</returns>
		// Token: 0x060025C4 RID: 9668 RVA: 0x000B5310 File Offset: 0x000B3510
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			base.ThrowElementNotAvailableException();
			return string.Empty;
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration value that specifies the control pattern.</param>
		/// <returns>The current <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" /> object, if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. For more information, see Remarks.</returns>
		// Token: 0x060025C5 RID: 9669 RVA: 0x000B533C File Offset: 0x000B353C
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface <= PatternInterface.ScrollItem)
			{
				if (patternInterface != PatternInterface.Invoke)
				{
					if (patternInterface == PatternInterface.ScrollItem)
					{
						if (this.Column != null)
						{
							return this;
						}
					}
				}
				else if (this.Column != null && this.Column.CanUserSort)
				{
					return this;
				}
			}
			else if (patternInterface != PatternInterface.Transform)
			{
				if (patternInterface == PatternInterface.VirtualizedItem)
				{
					if (this.Column != null)
					{
						return this;
					}
				}
			}
			else if (this.Column != null && this.Column.CanUserResize)
			{
				return this;
			}
			return null;
		}

		/// <summary>Gets a value that indicates whether the element that is associated with this automation peer contains data that is presented to the user.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x060025C6 RID: 9670 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}

		/// <summary>Sends a request to activate a control and initiate its single, unambiguous action.</summary>
		// Token: 0x060025C7 RID: 9671 RVA: 0x000B53A8 File Offset: 0x000B35A8
		void IInvokeProvider.Invoke()
		{
			UIElementAutomationPeer uielementAutomationPeer = this.GetWrapperPeer() as UIElementAutomationPeer;
			if (uielementAutomationPeer != null)
			{
				((DataGridColumnHeader)uielementAutomationPeer.Owner).Invoke();
				return;
			}
			base.ThrowElementNotAvailableException();
		}

		/// <summary>Scrolls the header and column that is associated with the <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" /> into view.</summary>
		// Token: 0x060025C8 RID: 9672 RVA: 0x000B53DB File Offset: 0x000B35DB
		void IScrollItemProvider.ScrollIntoView()
		{
			if (this.Column != null && this.OwningDataGrid != null)
			{
				this.OwningDataGrid.ScrollIntoView(null, this.Column);
			}
		}

		/// <summary>Gets a value that specifies whether the column can be moved.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanMove
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value that specifies whether the column can be resized.</summary>
		/// <returns>
		///     <see langword="true" /> if the element can be resized; otherwise <see langword="false" />. </returns>
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060025CA RID: 9674 RVA: 0x000B53FF File Offset: 0x000B35FF
		bool ITransformProvider.CanResize
		{
			get
			{
				return this.Column != null && this.Column.CanUserResize;
			}
		}

		/// <summary>Gets a value that specifies whether the control can be rotated.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ITransformProvider.CanRotate
		{
			get
			{
				return false;
			}
		}

		/// <summary>Throws an exception in all cases.</summary>
		/// <param name="x">This parameter is not used.</param>
		/// <param name="y">This parameter is not used.</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		// Token: 0x060025CC RID: 9676 RVA: 0x000B5416 File Offset: 0x000B3616
		void ITransformProvider.Move(double x, double y)
		{
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unsupported"));
		}

		/// <summary>Resizes the width of the column that is associated with the <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" />. </summary>
		/// <param name="width">The new width of the column, in pixels.</param>
		/// <param name="height">This parameter is not used.</param>
		/// <exception cref="T:System.InvalidOperationException">The column that is associated with this <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderItemAutomationPeer" /> cannot be resized.</exception>
		// Token: 0x060025CD RID: 9677 RVA: 0x000B5427 File Offset: 0x000B3627
		void ITransformProvider.Resize(double width, double height)
		{
			if (this.OwningDataGrid != null && this.Column.CanUserResize)
			{
				this.Column.Width = new DataGridLength(width);
				return;
			}
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unresizable"));
		}

		/// <summary>Throws an exception in all cases.</summary>
		/// <param name="degrees">This parameter is not used.</param>
		/// <exception cref="T:System.InvalidOperationException">In all cases.</exception>
		// Token: 0x060025CE RID: 9678 RVA: 0x000B5416 File Offset: 0x000B3616
		void ITransformProvider.Rotate(double degrees)
		{
			throw new InvalidOperationException(SR.Get("DataGridColumnHeaderItemAutomationPeer_Unsupported"));
		}

		/// <summary>Makes the virtual column fully accessible as a UI Automation element.</summary>
		// Token: 0x060025CF RID: 9679 RVA: 0x000B545F File Offset: 0x000B365F
		void IVirtualizedItemProvider.Realize()
		{
			if (this.OwningDataGrid != null)
			{
				this.OwningDataGrid.ScrollIntoView(null, this.Column);
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060025D0 RID: 9680 RVA: 0x000B547B File Offset: 0x000B367B
		// (set) Token: 0x060025D1 RID: 9681 RVA: 0x000B5484 File Offset: 0x000B3684
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer owningColumnHeaderPeer = this.OwningColumnHeaderPeer;
				if (owningColumnHeaderPeer != null)
				{
					owningColumnHeaderPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060025D2 RID: 9682 RVA: 0x000B54AD File Offset: 0x000B36AD
		internal DataGridColumnHeader OwningHeader
		{
			get
			{
				return base.GetWrapper() as DataGridColumnHeader;
			}
		}

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x000B54BA File Offset: 0x000B36BA
		internal DataGrid OwningDataGrid
		{
			get
			{
				return this.Column.DataGridOwner;
			}
		}

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x060025D4 RID: 9684 RVA: 0x000B54C7 File Offset: 0x000B36C7
		internal DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x060025D5 RID: 9685 RVA: 0x000B54CF File Offset: 0x000B36CF
		internal DataGridColumnHeaderAutomationPeer OwningColumnHeaderPeer
		{
			get
			{
				return this.GetWrapperPeer() as DataGridColumnHeaderAutomationPeer;
			}
		}

		// Token: 0x04001B6A RID: 7018
		private DataGridColumn _column;
	}
}

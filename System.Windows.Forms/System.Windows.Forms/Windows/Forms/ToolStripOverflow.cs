using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Manages the overflow behavior of a <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020003D9 RID: 985
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class ToolStripOverflow : ToolStripDropDown, IArrangedElement, IComponent, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripOverflow" /> class derived from a base <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <param name="parentItem">The <see cref="T:System.Windows.Forms.ToolStripItem" /> from which to derive this <see cref="T:System.Windows.Forms.ToolStripOverflow" /> instance. </param>
		// Token: 0x06004160 RID: 16736 RVA: 0x0011A18F File Offset: 0x0011838F
		public ToolStripOverflow(ToolStripItem parentItem) : base(parentItem)
		{
			if (parentItem == null)
			{
				throw new ArgumentNullException("parentItem");
			}
			this.ownerItem = (parentItem as ToolStripOverflowButton);
		}

		/// <summary>Gets all of the items that are currently being displayed on the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItemCollection" /> that includes all items on this <see cref="T:System.Windows.Forms.ToolStrip" />.</returns>
		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06004161 RID: 16737 RVA: 0x0011A1B4 File Offset: 0x001183B4
		protected internal override ToolStripItemCollection DisplayedItems
		{
			get
			{
				if (this.ParentToolStrip != null)
				{
					return this.ParentToolStrip.OverflowItems;
				}
				return new ToolStripItemCollection(null, false);
			}
		}

		/// <summary>Gets all of the items on the <see cref="T:System.Windows.Forms.ToolStrip" />, whether they are currently being displayed or not.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripItemCollection" /> containing all of the items.</returns>
		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06004162 RID: 16738 RVA: 0x0011A1DE File Offset: 0x001183DE
		public override ToolStripItemCollection Items
		{
			get
			{
				return new ToolStripItemCollection(null, false, true);
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06004163 RID: 16739 RVA: 0x0011A1E8 File Offset: 0x001183E8
		private ToolStrip ParentToolStrip
		{
			get
			{
				if (this.ownerItem != null)
				{
					return this.ownerItem.ParentToolStrip;
				}
				return null;
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06004164 RID: 16740 RVA: 0x0011A1FF File Offset: 0x001183FF
		ArrangedElementCollection IArrangedElement.Children
		{
			get
			{
				return this.DisplayedItems;
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06004165 RID: 16741 RVA: 0x000337D9 File Offset: 0x000319D9
		IArrangedElement IArrangedElement.Container
		{
			get
			{
				return this.ParentInternal;
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x000337E1 File Offset: 0x000319E1
		bool IArrangedElement.ParticipatesInLayout
		{
			get
			{
				return base.GetState(2);
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06004167 RID: 16743 RVA: 0x000337F9 File Offset: 0x000319F9
		PropertyStore IArrangedElement.Properties
		{
			get
			{
				return base.Properties;
			}
		}

		// Token: 0x06004168 RID: 16744 RVA: 0x00109833 File Offset: 0x00107A33
		void IArrangedElement.SetBounds(Rectangle bounds, BoundsSpecified specified)
		{
			this.SetBoundsCore(bounds.X, bounds.Y, bounds.Width, bounds.Height, specified);
		}

		/// <summary>Passes a reference to the cached <see cref="P:System.Windows.Forms.Control.LayoutEngine" /> returned by the layout engine interface.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Layout.LayoutEngine" /> that represents the cached layout engine returned by the layout engine interface.</returns>
		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06004169 RID: 16745 RVA: 0x000A76F4 File Offset: 0x000A58F4
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x0600416A RID: 16746 RVA: 0x0011A207 File Offset: 0x00118407
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripOverflow.ToolStripOverflowAccessibleObject(this);
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x0600416B RID: 16747 RVA: 0x0011A20F File Offset: 0x0011840F
		public override Size GetPreferredSize(Size constrainingSize)
		{
			constrainingSize.Width = 200;
			return base.GetPreferredSize(constrainingSize);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Layout" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LayoutEventArgs" /> that contains the event data.</param>
		// Token: 0x0600416C RID: 16748 RVA: 0x0011A224 File Offset: 0x00118424
		protected override void OnLayout(LayoutEventArgs e)
		{
			if (this.ParentToolStrip != null && this.ParentToolStrip.IsInDesignMode)
			{
				if (FlowLayout.GetFlowDirection(this) != FlowDirection.TopDown)
				{
					FlowLayout.SetFlowDirection(this, FlowDirection.TopDown);
				}
				if (FlowLayout.GetWrapContents(this))
				{
					FlowLayout.SetWrapContents(this, false);
				}
			}
			else
			{
				if (FlowLayout.GetFlowDirection(this) != FlowDirection.LeftToRight)
				{
					FlowLayout.SetFlowDirection(this, FlowDirection.LeftToRight);
				}
				if (!FlowLayout.GetWrapContents(this))
				{
					FlowLayout.SetWrapContents(this, true);
				}
			}
			base.OnLayout(e);
		}

		/// <summary>Resets the collection of displayed and overflow items after a layout is done.</summary>
		// Token: 0x0600416D RID: 16749 RVA: 0x0011A28C File Offset: 0x0011848C
		protected override void SetDisplayedItems()
		{
			Size size = Size.Empty;
			for (int i = 0; i < this.DisplayedItems.Count; i++)
			{
				ToolStripItem toolStripItem = this.DisplayedItems[i];
				if (((IArrangedElement)toolStripItem).ParticipatesInLayout)
				{
					base.HasVisibleItems = true;
					size = LayoutUtils.UnionSizes(size, toolStripItem.Bounds.Size);
				}
			}
			base.SetLargestItemSize(size);
		}

		// Token: 0x04002514 RID: 9492
		internal static readonly TraceSwitch PopupLayoutDebug;

		// Token: 0x04002515 RID: 9493
		private ToolStripOverflowButton ownerItem;

		// Token: 0x02000740 RID: 1856
		internal class ToolStripOverflowAccessibleObject : ToolStrip.ToolStripAccessibleObject
		{
			// Token: 0x06006163 RID: 24931 RVA: 0x001868E4 File Offset: 0x00184AE4
			public ToolStripOverflowAccessibleObject(ToolStripOverflow owner) : base(owner)
			{
			}

			// Token: 0x06006164 RID: 24932 RVA: 0x0018EA61 File Offset: 0x0018CC61
			public override AccessibleObject GetChild(int index)
			{
				return ((ToolStripOverflow)base.Owner).DisplayedItems[index].AccessibilityObject;
			}

			// Token: 0x06006165 RID: 24933 RVA: 0x0018EA7E File Offset: 0x0018CC7E
			public override int GetChildCount()
			{
				return ((ToolStripOverflow)base.Owner).DisplayedItems.Count;
			}
		}
	}
}

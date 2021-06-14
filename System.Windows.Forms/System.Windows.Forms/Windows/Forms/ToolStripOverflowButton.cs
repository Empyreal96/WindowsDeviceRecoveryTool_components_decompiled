using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Hosts a <see cref="T:System.Windows.Forms.ToolStripDropDown" /> that displays items that overflow the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
	// Token: 0x020003DA RID: 986
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.None)]
	public class ToolStripOverflowButton : ToolStripDropDownButton
	{
		// Token: 0x0600416E RID: 16750 RVA: 0x0011A2F0 File Offset: 0x001184F0
		internal ToolStripOverflowButton(ToolStrip parentToolStrip)
		{
			if (!ToolStripOverflowButton.isScalingInitialized)
			{
				if (DpiHelper.IsScalingRequired)
				{
					ToolStripOverflowButton.maxWidth = DpiHelper.LogicalToDeviceUnitsX(16);
					ToolStripOverflowButton.maxHeight = DpiHelper.LogicalToDeviceUnitsY(16);
				}
				ToolStripOverflowButton.isScalingInitialized = true;
			}
			base.SupportsItemClick = false;
			this.parentToolStrip = parentToolStrip;
		}

		/// <summary>Called by the <see cref="M:System.ComponentModel.Component.Dispose(System.Boolean)" /> and <see cref="M:System.ComponentModel.Component.Finalize" /> methods to release the managed and unmanaged resources used by the current instance of the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> class. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x0600416F RID: 16751 RVA: 0x0011A33D File Offset: 0x0011853D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.HasDropDownItems)
			{
				base.DropDown.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>Gets the space, in pixels, that is specified by default between controls.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> value representing the space between controls.</returns>
		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x000119C9 File Offset: 0x0000FBC9
		protected internal override Padding DefaultMargin
		{
			get
			{
				return Padding.Empty;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> has items that overflow the <see cref="T:System.Windows.Forms.ToolStrip" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" /> has overflow items; otherwise, <see langword="false" />. </returns>
		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0011A35C File Offset: 0x0011855C
		public override bool HasDropDownItems
		{
			get
			{
				return base.ParentInternal.OverflowItems.Count > 0;
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool OppositeDropDownAlign
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x0011A371 File Offset: 0x00118571
		internal ToolStrip ParentToolStrip
		{
			get
			{
				return this.parentToolStrip;
			}
		}

		/// <summary>This property is not relevant to this class.</summary>
		/// <returns>
		///     <see langword="true" /> to enable automatic mirroring; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x0010B22C File Offset: 0x0010942C
		// (set) Token: 0x06004175 RID: 16757 RVA: 0x0010B234 File Offset: 0x00109434
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool RightToLeftAutoMirrorImage
		{
			get
			{
				return base.RightToLeftAutoMirrorImage;
			}
			set
			{
				base.RightToLeftAutoMirrorImage = value;
			}
		}

		/// <summary>Creates a new accessibility object for the control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06004176 RID: 16758 RVA: 0x0011A379 File Offset: 0x00118579
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripOverflowButton.ToolStripOverflowButtonAccessibleObject(this);
		}

		/// <summary>Creates an empty <see cref="T:System.Windows.Forms.ToolStripDropDown" /> that can be dropped down and to which events can be attached.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.ToolStripDropDown" /> control.</returns>
		// Token: 0x06004177 RID: 16759 RVA: 0x0011A381 File Offset: 0x00118581
		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			return new ToolStripOverflow(this);
		}

		/// <summary>Retrieves the size of a rectangular area into which a control can fit.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control. </param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" /> representing the width and height of a rectangle.</returns>
		// Token: 0x06004178 RID: 16760 RVA: 0x0011A38C File Offset: 0x0011858C
		public override Size GetPreferredSize(Size constrainingSize)
		{
			Size sz = constrainingSize;
			if (base.ParentInternal != null)
			{
				if (base.ParentInternal.Orientation == Orientation.Horizontal)
				{
					sz.Width = Math.Min(constrainingSize.Width, ToolStripOverflowButton.maxWidth);
				}
				else
				{
					sz.Height = Math.Min(constrainingSize.Height, ToolStripOverflowButton.maxHeight);
				}
			}
			return sz + this.Padding.Size;
		}

		/// <summary>Sets the size and location of the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</summary>
		/// <param name="bounds">A <see cref="T:System.Drawing.Rectangle" /> representing the size and location of the <see cref="T:System.Windows.Forms.ToolStripOverflowButton" />.</param>
		// Token: 0x06004179 RID: 16761 RVA: 0x0011A3F8 File Offset: 0x001185F8
		protected internal override void SetBounds(Rectangle bounds)
		{
			if (base.ParentInternal != null && base.ParentInternal.LayoutEngine is ToolStripSplitStackLayout)
			{
				if (base.ParentInternal.Orientation == Orientation.Horizontal)
				{
					bounds.Height = base.ParentInternal.Height;
					bounds.Y = 0;
				}
				else
				{
					bounds.Width = base.ParentInternal.Width;
					bounds.X = 0;
				}
			}
			base.SetBounds(bounds);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600417A RID: 16762 RVA: 0x0011A46C File Offset: 0x0011866C
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.ParentInternal != null)
			{
				ToolStripRenderer renderer = base.ParentInternal.Renderer;
				renderer.DrawOverflowButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
			}
		}

		// Token: 0x04002516 RID: 9494
		private ToolStrip parentToolStrip;

		// Token: 0x04002517 RID: 9495
		private static bool isScalingInitialized = false;

		// Token: 0x04002518 RID: 9496
		private const int MAX_WIDTH = 16;

		// Token: 0x04002519 RID: 9497
		private const int MAX_HEIGHT = 16;

		// Token: 0x0400251A RID: 9498
		private static int maxWidth = 16;

		// Token: 0x0400251B RID: 9499
		private static int maxHeight = 16;

		// Token: 0x02000741 RID: 1857
		internal class ToolStripOverflowButtonAccessibleObject : ToolStripDropDownItemAccessibleObject
		{
			// Token: 0x06006166 RID: 24934 RVA: 0x0018EA95 File Offset: 0x0018CC95
			public ToolStripOverflowButtonAccessibleObject(ToolStripOverflowButton owner) : base(owner)
			{
			}

			// Token: 0x17001746 RID: 5958
			// (get) Token: 0x06006167 RID: 24935 RVA: 0x0018EAA0 File Offset: 0x0018CCA0
			// (set) Token: 0x06006168 RID: 24936 RVA: 0x0018D4A5 File Offset: 0x0018B6A5
			public override string Name
			{
				get
				{
					string accessibleName = base.Owner.AccessibleName;
					if (accessibleName != null)
					{
						return accessibleName;
					}
					if (string.IsNullOrEmpty(this.stockName))
					{
						this.stockName = SR.GetString("ToolStripOptions");
					}
					return this.stockName;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x06006169 RID: 24937 RVA: 0x0018EAE1 File Offset: 0x0018CCE1
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3 && propertyID == 30003)
				{
					return 50011;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x04004197 RID: 16791
			private string stockName;
		}
	}
}

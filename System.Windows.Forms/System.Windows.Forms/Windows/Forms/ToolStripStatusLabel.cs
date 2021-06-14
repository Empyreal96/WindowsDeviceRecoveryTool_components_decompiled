using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.Automation;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a panel in a <see cref="T:System.Windows.Forms.StatusStrip" /> control. </summary>
	// Token: 0x020003F5 RID: 1013
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
	public class ToolStripStatusLabel : ToolStripLabel, IAutomationLiveRegion
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class. </summary>
		// Token: 0x06004451 RID: 17489 RVA: 0x00124344 File Offset: 0x00122544
		public ToolStripStatusLabel()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class that displays the specified text.</summary>
		/// <param name="text">A <see cref="T:System.String" /> representing the text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		// Token: 0x06004452 RID: 17490 RVA: 0x00124368 File Offset: 0x00122568
		public ToolStripStatusLabel(string text) : base(text, null, false, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class that displays the specified image. </summary>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that is displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		// Token: 0x06004453 RID: 17491 RVA: 0x00124390 File Offset: 0x00122590
		public ToolStripStatusLabel(Image image) : base(null, image, false, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class that displays the specified image and text.</summary>
		/// <param name="text">A <see cref="T:System.String" /> representing the text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that is displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		// Token: 0x06004454 RID: 17492 RVA: 0x001243B8 File Offset: 0x001225B8
		public ToolStripStatusLabel(string text, Image image) : base(text, image, false, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class that displays the specified image and text, and that carries out the specified action when the user clicks the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</summary>
		/// <param name="text">A <see cref="T:System.String" /> representing the text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that is displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="onClick">Specifies the action to carry out when the control is clicked.</param>
		// Token: 0x06004455 RID: 17493 RVA: 0x001243E0 File Offset: 0x001225E0
		public ToolStripStatusLabel(string text, Image image, EventHandler onClick) : base(text, image, false, onClick, null)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> class with the specified name that displays the specified image and text, and that carries out the specified action when the user clicks the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</summary>
		/// <param name="text">A <see cref="T:System.String" /> representing the text to be displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="image">An <see cref="T:System.Drawing.Image" /> that is displayed on the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		/// <param name="onClick">Specifies the action to carry out when the control is clicked.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</param>
		// Token: 0x06004456 RID: 17494 RVA: 0x00124409 File Offset: 0x00122609
		public ToolStripStatusLabel(string text, Image image, EventHandler onClick, string name) : base(text, image, false, onClick, name)
		{
			this.Initialize();
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x00124433 File Offset: 0x00122633
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			if (AccessibilityImprovements.Level3)
			{
				return new ToolStripStatusLabel.ToolStripStatusLabelAccessibleObject(this);
			}
			return base.CreateAccessibilityInstance();
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x00124449 File Offset: 0x00122649
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripStatusLabel.ToolStripStatusLabelLayout(this);
		}

		/// <summary>Gets or sets a value that determines where the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> is aligned on the <see cref="T:System.Windows.Forms.StatusStrip" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripItemAlignment" /> values.</returns>
		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06004459 RID: 17497 RVA: 0x00124451 File Offset: 0x00122651
		// (set) Token: 0x0600445A RID: 17498 RVA: 0x00124459 File Offset: 0x00122659
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public new ToolStripItemAlignment Alignment
		{
			get
			{
				return base.Alignment;
			}
			set
			{
				base.Alignment = value;
			}
		}

		/// <summary>Gets or sets the border style of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Border3DStyle" /> values. The default is <see cref="F:System.Windows.Forms.Border3DStyle.Flat" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value of <see cref="P:System.Windows.Forms.ToolStripStatusLabel.BorderStyle" /> is not one of the <see cref="T:System.Windows.Forms.Border3DStyle" /> values.</exception>
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x0600445B RID: 17499 RVA: 0x00124462 File Offset: 0x00122662
		// (set) Token: 0x0600445C RID: 17500 RVA: 0x0012446C File Offset: 0x0012266C
		[DefaultValue(Border3DStyle.Flat)]
		[SRDescription("ToolStripStatusLabelBorderStyleDescr")]
		[SRCategory("CatAppearance")]
		public Border3DStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[]
				{
					8192,
					9,
					6,
					16394,
					5,
					4,
					1,
					10,
					8,
					2
				}))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Border3DStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates which sides of the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> show borders.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripStatusLabelBorderSides" /> values. The default is <see cref="F:System.Windows.Forms.ToolStripStatusLabelBorderSides.None" />.</returns>
		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x0600445D RID: 17501 RVA: 0x001244C5 File Offset: 0x001226C5
		// (set) Token: 0x0600445E RID: 17502 RVA: 0x001244CD File Offset: 0x001226CD
		[DefaultValue(ToolStripStatusLabelBorderSides.None)]
		[SRDescription("ToolStripStatusLabelBorderSidesDescr")]
		[SRCategory("CatAppearance")]
		public ToolStripStatusLabelBorderSides BorderSides
		{
			get
			{
				return this.borderSides;
			}
			set
			{
				if (this.borderSides != value)
				{
					this.borderSides = value;
					LayoutTransaction.DoLayout(base.Owner, this, PropertyNames.BorderStyle);
					base.Invalidate();
				}
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x001244F6 File Offset: 0x001226F6
		private void Initialize()
		{
			if (DpiHelper.EnableToolStripHighDpiImprovements)
			{
				this.scaledDefaultMargin = DpiHelper.LogicalToDeviceUnits(ToolStripStatusLabel.defaultMargin, 0);
			}
		}

		/// <summary>Gets the default margin of an item.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding" /> representing the margin.</returns>
		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x00124510 File Offset: 0x00122710
		protected internal override Padding DefaultMargin
		{
			get
			{
				return this.scaledDefaultMargin;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> automatically fills the available space on the <see cref="T:System.Windows.Forms.StatusStrip" /> as the form is resized. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> automatically fills the available space on the <see cref="T:System.Windows.Forms.StatusStrip" /> as the form is resized; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x06004461 RID: 17505 RVA: 0x00124518 File Offset: 0x00122718
		// (set) Token: 0x06004462 RID: 17506 RVA: 0x00124520 File Offset: 0x00122720
		[DefaultValue(false)]
		[SRDescription("ToolStripStatusLabelSpringDescr")]
		[SRCategory("CatAppearance")]
		public bool Spring
		{
			get
			{
				return this.spring;
			}
			set
			{
				if (this.spring != value)
				{
					this.spring = value;
					if (base.ParentInternal != null)
					{
						LayoutTransaction.DoLayout(base.ParentInternal, this, PropertyNames.Spring);
					}
				}
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x06004463 RID: 17507 RVA: 0x0012454B File Offset: 0x0012274B
		// (set) Token: 0x06004464 RID: 17508 RVA: 0x00124553 File Offset: 0x00122753
		[SRCategory("CatAccessibility")]
		[DefaultValue(AutomationLiveSetting.Off)]
		[SRDescription("LiveRegionAutomationLiveSettingDescr")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public AutomationLiveSetting LiveSetting
		{
			get
			{
				return this.liveSetting;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AutomationLiveSetting));
				}
				this.liveSetting = value;
			}
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x00124582 File Offset: 0x00122782
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			if (AccessibilityImprovements.Level3 && this.LiveSetting != AutomationLiveSetting.Off)
			{
				base.AccessibilityObject.RaiseLiveRegionChanged();
			}
		}

		/// <summary>Retrieves the size of a rectangular area into which a <see cref="T:System.Windows.Forms.ToolStripStatusLabel" /> can be fitted.</summary>
		/// <param name="constrainingSize">The custom-sized area for a control.</param>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size" />, representing the width and height of a rectangle.</returns>
		// Token: 0x06004466 RID: 17510 RVA: 0x001245A6 File Offset: 0x001227A6
		public override Size GetPreferredSize(Size constrainingSize)
		{
			if (this.BorderSides != ToolStripStatusLabelBorderSides.None)
			{
				return base.GetPreferredSize(constrainingSize) + new Size(4, 4);
			}
			return base.GetPreferredSize(constrainingSize);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06004467 RID: 17511 RVA: 0x001245CC File Offset: 0x001227CC
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawToolStripStatusLabelBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle));
				}
				base.PaintText(e.Graphics);
			}
		}

		// Token: 0x040025CC RID: 9676
		private static readonly Padding defaultMargin = new Padding(0, 3, 0, 2);

		// Token: 0x040025CD RID: 9677
		private Padding scaledDefaultMargin = ToolStripStatusLabel.defaultMargin;

		// Token: 0x040025CE RID: 9678
		private Border3DStyle borderStyle = Border3DStyle.Flat;

		// Token: 0x040025CF RID: 9679
		private ToolStripStatusLabelBorderSides borderSides;

		// Token: 0x040025D0 RID: 9680
		private bool spring;

		// Token: 0x040025D1 RID: 9681
		private AutomationLiveSetting liveSetting;

		// Token: 0x02000755 RID: 1877
		[ComVisible(true)]
		internal class ToolStripStatusLabelAccessibleObject : ToolStripLabel.ToolStripLabelAccessibleObject
		{
			// Token: 0x06006216 RID: 25110 RVA: 0x00191742 File Offset: 0x0018F942
			public ToolStripStatusLabelAccessibleObject(ToolStripStatusLabel ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x06006217 RID: 25111 RVA: 0x00191752 File Offset: 0x0018F952
			public override bool RaiseLiveRegionChanged()
			{
				return base.RaiseAutomationEvent(20024);
			}

			// Token: 0x06006218 RID: 25112 RVA: 0x0019175F File Offset: 0x0018F95F
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30003)
				{
					return 50020;
				}
				if (propertyID == 30135)
				{
					return this.ownerItem.LiveSetting;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x040041B4 RID: 16820
			private ToolStripStatusLabel ownerItem;
		}

		// Token: 0x02000756 RID: 1878
		private class ToolStripStatusLabelLayout : ToolStripItemInternalLayout
		{
			// Token: 0x06006219 RID: 25113 RVA: 0x00191794 File Offset: 0x0018F994
			public ToolStripStatusLabelLayout(ToolStripStatusLabel owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x0600621A RID: 25114 RVA: 0x001917A4 File Offset: 0x0018F9A4
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				toolStripItemLayoutOptions.borderSize = 0;
				return toolStripItemLayoutOptions;
			}

			// Token: 0x040041B5 RID: 16821
			private ToolStripStatusLabel owner;
		}
	}
}

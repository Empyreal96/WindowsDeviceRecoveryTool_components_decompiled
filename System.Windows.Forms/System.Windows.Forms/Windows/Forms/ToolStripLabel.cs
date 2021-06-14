using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Design;

namespace System.Windows.Forms
{
	/// <summary>Represents a nonselectable <see cref="T:System.Windows.Forms.ToolStripItem" /> that renders text and images and can display hyperlinks.</summary>
	// Token: 0x020003CF RID: 975
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripLabel : ToolStripItem
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class.</summary>
		// Token: 0x06004088 RID: 16520 RVA: 0x00116671 File Offset: 0x00114871
		public ToolStripLabel()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the text to display.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		// Token: 0x06004089 RID: 16521 RVA: 0x0011669A File Offset: 0x0011489A
		public ToolStripLabel(string text) : base(text, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the image to display.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		// Token: 0x0600408A RID: 16522 RVA: 0x001166C6 File Offset: 0x001148C6
		public ToolStripLabel(Image image) : base(null, image, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the text and image to display.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		// Token: 0x0600408B RID: 16523 RVA: 0x001166F2 File Offset: 0x001148F2
		public ToolStripLabel(string text, Image image) : base(text, image, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the text and image to display and whether the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="isLink">
		///       <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link; otherwise, <see langword="false" />. </param>
		// Token: 0x0600408C RID: 16524 RVA: 0x0011671E File Offset: 0x0011491E
		public ToolStripLabel(string text, Image image, bool isLink) : this(text, image, isLink, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the text and image to display, whether the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link, and providing a <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event handler.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="isLink">
		///       <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link; otherwise, <see langword="false" />. </param>
		/// <param name="onClick">A <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event handler.</param>
		// Token: 0x0600408D RID: 16525 RVA: 0x0011672A File Offset: 0x0011492A
		public ToolStripLabel(string text, Image image, bool isLink, EventHandler onClick) : this(text, image, isLink, onClick, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripLabel" /> class, specifying the text and image to display, whether the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link, and providing a <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event handler and name for the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</summary>
		/// <param name="text">The text to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		/// <param name="isLink">
		///       <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripLabel" /> acts as a link; otherwise, <see langword="false" />. </param>
		/// <param name="onClick">A <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event handler.</param>
		/// <param name="name">The name of the <see cref="T:System.Windows.Forms.ToolStripLabel" />.</param>
		// Token: 0x0600408E RID: 16526 RVA: 0x00116738 File Offset: 0x00114938
		public ToolStripLabel(string text, Image image, bool isLink, EventHandler onClick, string name) : base(text, image, onClick, name)
		{
			this.IsLink = isLink;
		}

		/// <summary>Gets a value indicating the selectable state of a <see cref="T:System.Windows.Forms.ToolStripLabel" />.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x0011676E File Offset: 0x0011496E
		public override bool CanSelect
		{
			get
			{
				return this.IsLink || base.DesignMode;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.ToolStripLabel" /> is a hyperlink. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Forms.ToolStripLabel" /> is a hyperlink; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x00116780 File Offset: 0x00114980
		// (set) Token: 0x06004091 RID: 16529 RVA: 0x00116788 File Offset: 0x00114988
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripLabelIsLinkDescr")]
		public bool IsLink
		{
			get
			{
				return this.isLink;
			}
			set
			{
				if (this.isLink != value)
				{
					this.isLink = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color used to display an active link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color to display an active link. The default color is specified by the system. Typically, this color is <see langword="Color.Red" />.</returns>
		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06004092 RID: 16530 RVA: 0x001167A0 File Offset: 0x001149A0
		// (set) Token: 0x06004093 RID: 16531 RVA: 0x001167BC File Offset: 0x001149BC
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelActiveLinkColorDescr")]
		public Color ActiveLinkColor
		{
			get
			{
				if (this.activeLinkColor.IsEmpty)
				{
					return this.IEActiveLinkColor;
				}
				return this.activeLinkColor;
			}
			set
			{
				if (this.activeLinkColor != value)
				{
					this.activeLinkColor = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06004094 RID: 16532 RVA: 0x000BA3AD File Offset: 0x000B85AD
		private Color IELinkColor
		{
			get
			{
				return LinkUtilities.IELinkColor;
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x000BA3B4 File Offset: 0x000B85B4
		private Color IEActiveLinkColor
		{
			get
			{
				return LinkUtilities.IEActiveLinkColor;
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06004096 RID: 16534 RVA: 0x000BA3BB File Offset: 0x000B85BB
		private Color IEVisitedLinkColor
		{
			get
			{
				return LinkUtilities.IEVisitedLinkColor;
			}
		}

		/// <summary>Gets or sets a value that represents the behavior of a link.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LinkBehavior" /> values. The default is <see langword="LinkBehavior.SystemDefault" />.</returns>
		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x001167D9 File Offset: 0x001149D9
		// (set) Token: 0x06004098 RID: 16536 RVA: 0x001167E4 File Offset: 0x001149E4
		[DefaultValue(LinkBehavior.SystemDefault)]
		[SRCategory("CatBehavior")]
		[SRDescription("ToolStripLabelLinkBehaviorDescr")]
		public LinkBehavior LinkBehavior
		{
			get
			{
				return this.linkBehavior;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("LinkBehavior", (int)value, typeof(LinkBehavior));
				}
				if (this.linkBehavior != value)
				{
					this.linkBehavior = value;
					this.InvalidateLinkFonts();
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color used when displaying a normal link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to displaying a normal link. The default color is specified by the system. Typically, this color is <see langword="Color.Blue" />.</returns>
		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06004099 RID: 16537 RVA: 0x00116833 File Offset: 0x00114A33
		// (set) Token: 0x0600409A RID: 16538 RVA: 0x0011684F File Offset: 0x00114A4F
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelLinkColorDescr")]
		public Color LinkColor
		{
			get
			{
				if (this.linkColor.IsEmpty)
				{
					return this.IELinkColor;
				}
				return this.linkColor;
			}
			set
			{
				if (this.linkColor != value)
				{
					this.linkColor = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether a link should be displayed as though it were visited.</summary>
		/// <returns>
		///     <see langword="true" /> if links should display as though they were visited; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x0011686C File Offset: 0x00114A6C
		// (set) Token: 0x0600409C RID: 16540 RVA: 0x00116874 File Offset: 0x00114A74
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelLinkVisitedDescr")]
		public bool LinkVisited
		{
			get
			{
				return this.linkVisited;
			}
			set
			{
				if (this.linkVisited != value)
				{
					this.linkVisited = value;
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color used when displaying a link that that has been previously visited.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display links that have been visited. The default color is specified by the system. Typically, this color is <see langword="Color.Purple" />.</returns>
		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x0600409D RID: 16541 RVA: 0x0011688C File Offset: 0x00114A8C
		// (set) Token: 0x0600409E RID: 16542 RVA: 0x001168A8 File Offset: 0x00114AA8
		[SRCategory("CatAppearance")]
		[SRDescription("ToolStripLabelVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (this.visitedLinkColor.IsEmpty)
				{
					return this.IEVisitedLinkColor;
				}
				return this.visitedLinkColor;
			}
			set
			{
				if (this.visitedLinkColor != value)
				{
					this.visitedLinkColor = value;
					base.Invalidate();
				}
			}
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x001168C8 File Offset: 0x00114AC8
		private void InvalidateLinkFonts()
		{
			if (this.linkFont != null)
			{
				this.linkFont.Dispose();
			}
			if (this.hoverLinkFont != null && this.hoverLinkFont != this.linkFont)
			{
				this.hoverLinkFont.Dispose();
			}
			this.linkFont = null;
			this.hoverLinkFont = null;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060040A0 RID: 16544 RVA: 0x00116917 File Offset: 0x00114B17
		protected override void OnFontChanged(EventArgs e)
		{
			this.InvalidateLinkFonts();
			base.OnFontChanged(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseEnter" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060040A1 RID: 16545 RVA: 0x00116928 File Offset: 0x00114B28
		protected override void OnMouseEnter(EventArgs e)
		{
			if (this.IsLink)
			{
				ToolStrip parent = base.Parent;
				if (parent != null)
				{
					this.lastCursor = parent.Cursor;
					parent.Cursor = Cursors.Hand;
				}
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseLeave" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x060040A2 RID: 16546 RVA: 0x00116968 File Offset: 0x00114B68
		protected override void OnMouseLeave(EventArgs e)
		{
			if (this.IsLink)
			{
				ToolStrip parent = base.Parent;
				if (parent != null)
				{
					parent.Cursor = this.lastCursor;
				}
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0011699A File Offset: 0x00114B9A
		private void ResetActiveLinkColor()
		{
			this.ActiveLinkColor = this.IEActiveLinkColor;
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x001169A8 File Offset: 0x00114BA8
		private void ResetLinkColor()
		{
			this.LinkColor = this.IELinkColor;
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x001169B6 File Offset: 0x00114BB6
		private void ResetVisitedLinkColor()
		{
			this.VisitedLinkColor = this.IEVisitedLinkColor;
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x001169C4 File Offset: 0x00114BC4
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeActiveLinkColor()
		{
			return !this.activeLinkColor.IsEmpty;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x001169D4 File Offset: 0x00114BD4
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeLinkColor()
		{
			return !this.linkColor.IsEmpty;
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x001169E4 File Offset: 0x00114BE4
		[EditorBrowsable(EditorBrowsableState.Never)]
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.visitedLinkColor.IsEmpty;
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x001169F4 File Offset: 0x00114BF4
		internal override ToolStripItemInternalLayout CreateInternalLayout()
		{
			return new ToolStripLabel.ToolStripLabelLayout(this);
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x060040AA RID: 16554 RVA: 0x001169FC File Offset: 0x00114BFC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new ToolStripLabel.ToolStripLabelAccessibleObject(this);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Paint" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x060040AB RID: 16555 RVA: 0x00116A04 File Offset: 0x00114C04
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Owner != null)
			{
				ToolStripRenderer renderer = base.Renderer;
				renderer.DrawLabelBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
				if ((this.DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image)
				{
					renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(e.Graphics, this, base.InternalLayout.ImageRectangle));
				}
				this.PaintText(e.Graphics);
			}
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x00116A68 File Offset: 0x00114C68
		internal void PaintText(Graphics g)
		{
			ToolStripRenderer renderer = base.Renderer;
			if ((this.DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text)
			{
				Font font = this.Font;
				Color textColor = this.ForeColor;
				if (this.IsLink)
				{
					LinkUtilities.EnsureLinkFonts(font, this.LinkBehavior, ref this.linkFont, ref this.hoverLinkFont);
					if (this.Pressed)
					{
						font = this.hoverLinkFont;
						textColor = this.ActiveLinkColor;
					}
					else if (this.Selected)
					{
						font = this.hoverLinkFont;
						textColor = (this.LinkVisited ? this.VisitedLinkColor : this.LinkColor);
					}
					else
					{
						font = this.linkFont;
						textColor = (this.LinkVisited ? this.VisitedLinkColor : this.LinkColor);
					}
				}
				Rectangle textRectangle = base.InternalLayout.TextRectangle;
				renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(g, this, this.Text, textRectangle, textColor, font, base.InternalLayout.TextFormat));
			}
		}

		/// <summary>Processes a mnemonic character.</summary>
		/// <param name="charCode">The character to process. </param>
		/// <returns>
		///     <see langword="true" /> if the character was processed as a mnemonic by the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x060040AD RID: 16557 RVA: 0x00116B43 File Offset: 0x00114D43
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected internal override bool ProcessMnemonic(char charCode)
		{
			if (base.ParentInternal != null)
			{
				if (!this.CanSelect)
				{
					base.ParentInternal.SetFocusUnsafe();
					base.ParentInternal.SelectNextToolStripItem(this, true);
				}
				else
				{
					base.FireEvent(ToolStripItemEventType.Click);
				}
				return true;
			}
			return false;
		}

		// Token: 0x040024CB RID: 9419
		private LinkBehavior linkBehavior;

		// Token: 0x040024CC RID: 9420
		private bool isLink;

		// Token: 0x040024CD RID: 9421
		private bool linkVisited;

		// Token: 0x040024CE RID: 9422
		private Color linkColor = Color.Empty;

		// Token: 0x040024CF RID: 9423
		private Color activeLinkColor = Color.Empty;

		// Token: 0x040024D0 RID: 9424
		private Color visitedLinkColor = Color.Empty;

		// Token: 0x040024D1 RID: 9425
		private Font hoverLinkFont;

		// Token: 0x040024D2 RID: 9426
		private Font linkFont;

		// Token: 0x040024D3 RID: 9427
		private Cursor lastCursor;

		// Token: 0x0200073C RID: 1852
		[ComVisible(true)]
		internal class ToolStripLabelAccessibleObject : ToolStripItem.ToolStripItemAccessibleObject
		{
			// Token: 0x06006136 RID: 24886 RVA: 0x0018DE54 File Offset: 0x0018C054
			public ToolStripLabelAccessibleObject(ToolStripLabel ownerItem) : base(ownerItem)
			{
				this.ownerItem = ownerItem;
			}

			// Token: 0x1700173B RID: 5947
			// (get) Token: 0x06006137 RID: 24887 RVA: 0x0018DE64 File Offset: 0x0018C064
			public override string DefaultAction
			{
				get
				{
					if (this.ownerItem.IsLink)
					{
						return SR.GetString("AccessibleActionClick");
					}
					return string.Empty;
				}
			}

			// Token: 0x06006138 RID: 24888 RVA: 0x0018DE83 File Offset: 0x0018C083
			public override void DoDefaultAction()
			{
				if (this.ownerItem.IsLink)
				{
					base.DoDefaultAction();
				}
			}

			// Token: 0x06006139 RID: 24889 RVA: 0x0018DE98 File Offset: 0x0018C098
			internal override object GetPropertyValue(int propertyID)
			{
				if (AccessibilityImprovements.Level3)
				{
					if (propertyID == 30003)
					{
						return 50020;
					}
					if (propertyID == 30096)
					{
						return this.State;
					}
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x1700173C RID: 5948
			// (get) Token: 0x0600613A RID: 24890 RVA: 0x0018DED0 File Offset: 0x0018C0D0
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					if (!this.ownerItem.IsLink)
					{
						return AccessibleRole.StaticText;
					}
					return AccessibleRole.Link;
				}
			}

			// Token: 0x1700173D RID: 5949
			// (get) Token: 0x0600613B RID: 24891 RVA: 0x0018DF01 File Offset: 0x0018C101
			public override AccessibleStates State
			{
				get
				{
					return base.State | AccessibleStates.ReadOnly;
				}
			}

			// Token: 0x04004186 RID: 16774
			private ToolStripLabel ownerItem;
		}

		// Token: 0x0200073D RID: 1853
		private class ToolStripLabelLayout : ToolStripItemInternalLayout
		{
			// Token: 0x0600613C RID: 24892 RVA: 0x0018DF0C File Offset: 0x0018C10C
			public ToolStripLabelLayout(ToolStripLabel owner) : base(owner)
			{
				this.owner = owner;
			}

			// Token: 0x0600613D RID: 24893 RVA: 0x0018DF1C File Offset: 0x0018C11C
			protected override ToolStripItemInternalLayout.ToolStripItemLayoutOptions CommonLayoutOptions()
			{
				ToolStripItemInternalLayout.ToolStripItemLayoutOptions toolStripItemLayoutOptions = base.CommonLayoutOptions();
				toolStripItemLayoutOptions.borderSize = 0;
				return toolStripItemLayoutOptions;
			}

			// Token: 0x04004187 RID: 16775
			private ToolStripLabel owner;
		}
	}
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.Internal;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows label control that can display hyperlinks.</summary>
	// Token: 0x020002B5 RID: 693
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultEvent("LinkClicked")]
	[ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionLinkLabel")]
	public class LinkLabel : Label, IButtonControl
	{
		/// <summary>Initializes a new default instance of the <see cref="T:System.Windows.Forms.LinkLabel" /> class.</summary>
		// Token: 0x060027EF RID: 10223 RVA: 0x000BA280 File Offset: 0x000B8480
		public LinkLabel()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.StandardClick | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.ResetLinkArea();
		}

		/// <summary>Gets or sets the color used to display an active link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color to display an active link. The default color is specified by the system, typically this color is <see langword="Color.Red" />.</returns>
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x000BA2DD File Offset: 0x000B84DD
		// (set) Token: 0x060027F1 RID: 10225 RVA: 0x000BA2F9 File Offset: 0x000B84F9
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelActiveLinkColorDescr")]
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
					this.InvalidateLink(null);
				}
			}
		}

		/// <summary>Gets or sets the color used when displaying a disabled link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color when displaying a disabled link. The default is <see langword="Empty" />.</returns>
		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x000BA317 File Offset: 0x000B8517
		// (set) Token: 0x060027F3 RID: 10227 RVA: 0x000BA333 File Offset: 0x000B8533
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelDisabledLinkColorDescr")]
		public Color DisabledLinkColor
		{
			get
			{
				if (this.disabledLinkColor.IsEmpty)
				{
					return this.IEDisabledLinkColor;
				}
				return this.disabledLinkColor;
			}
			set
			{
				if (this.disabledLinkColor != value)
				{
					this.disabledLinkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000BA351 File Offset: 0x000B8551
		// (set) Token: 0x060027F5 RID: 10229 RVA: 0x000BA35C File Offset: 0x000B855C
		private LinkLabel.Link FocusLink
		{
			get
			{
				return this.focusLink;
			}
			set
			{
				if (this.focusLink != value)
				{
					if (this.focusLink != null)
					{
						this.InvalidateLink(this.focusLink);
					}
					this.focusLink = value;
					if (this.focusLink != null)
					{
						this.InvalidateLink(this.focusLink);
						this.UpdateAccessibilityLink(this.focusLink);
					}
				}
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000BA3AD File Offset: 0x000B85AD
		private Color IELinkColor
		{
			get
			{
				return LinkUtilities.IELinkColor;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000BA3B4 File Offset: 0x000B85B4
		private Color IEActiveLinkColor
		{
			get
			{
				return LinkUtilities.IEActiveLinkColor;
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000BA3BB File Offset: 0x000B85BB
		private Color IEVisitedLinkColor
		{
			get
			{
				return LinkUtilities.IEVisitedLinkColor;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000BA3C2 File Offset: 0x000B85C2
		private Color IEDisabledLinkColor
		{
			get
			{
				if (LinkLabel.iedisabledLinkColor.IsEmpty)
				{
					LinkLabel.iedisabledLinkColor = ControlPaint.Dark(base.DisabledColor);
				}
				return LinkLabel.iedisabledLinkColor;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x000BA3E5 File Offset: 0x000B85E5
		private Rectangle ClientRectWithPadding
		{
			get
			{
				return LayoutUtils.DeflateRect(base.ClientRectangle, this.Padding);
			}
		}

		/// <summary>Gets or sets the flat style appearance of the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle" /> values.</returns>
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060027FB RID: 10235 RVA: 0x000BA3F8 File Offset: 0x000B85F8
		// (set) Token: 0x060027FC RID: 10236 RVA: 0x000BA400 File Offset: 0x000B8600
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = value;
			}
		}

		/// <summary>Gets or sets the range in the text to treat as a link.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.LinkArea" /> that represents the area treated as a link.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.Windows.Forms.LinkArea.Start" /> property of the <see cref="T:System.Windows.Forms.LinkArea" /> object is less than zero.-or- The <see cref="P:System.Windows.Forms.LinkArea.Length" /> property of the <see cref="T:System.Windows.Forms.LinkArea" /> object is less than -1. </exception>
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060027FD RID: 10237 RVA: 0x000BA40C File Offset: 0x000B860C
		// (set) Token: 0x060027FE RID: 10238 RVA: 0x000BA460 File Offset: 0x000B8660
		[Editor("System.Windows.Forms.Design.LinkAreaEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("LinkLabelLinkAreaDescr")]
		public LinkArea LinkArea
		{
			get
			{
				if (this.links.Count == 0)
				{
					return new LinkArea(0, 0);
				}
				return new LinkArea(((LinkLabel.Link)this.links[0]).Start, ((LinkLabel.Link)this.links[0]).Length);
			}
			set
			{
				LinkArea linkArea = this.LinkArea;
				this.links.Clear();
				if (!value.IsEmpty)
				{
					if (value.Start < 0)
					{
						throw new ArgumentOutOfRangeException("LinkArea", value, SR.GetString("LinkLabelAreaStart"));
					}
					if (value.Length < -1)
					{
						throw new ArgumentOutOfRangeException("LinkArea", value, SR.GetString("LinkLabelAreaLength"));
					}
					if (value.Start != 0 || value.Length != 0)
					{
						this.Links.Add(new LinkLabel.Link(this));
						((LinkLabel.Link)this.links[0]).Start = value.Start;
						((LinkLabel.Link)this.links[0]).Length = value.Length;
					}
				}
				this.UpdateSelectability();
				if (!linkArea.Equals(this.LinkArea))
				{
					this.InvalidateTextLayout();
					LayoutTransaction.DoLayout(this.ParentInternal, this, PropertyNames.LinkArea);
					base.AdjustSize();
					base.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets a value that represents the behavior of a link.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.LinkBehavior" /> values. The default is <see langword="LinkBehavior.SystemDefault" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">A value is assigned that is not one of the <see cref="T:System.Windows.Forms.LinkBehavior" /> values.</exception>
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x060027FF RID: 10239 RVA: 0x000BA576 File Offset: 0x000B8776
		// (set) Token: 0x06002800 RID: 10240 RVA: 0x000BA580 File Offset: 0x000B8780
		[DefaultValue(LinkBehavior.SystemDefault)]
		[SRCategory("CatBehavior")]
		[SRDescription("LinkLabelLinkBehaviorDescr")]
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
				if (value != this.linkBehavior)
				{
					this.linkBehavior = value;
					this.InvalidateLinkFonts();
					this.InvalidateLink(null);
				}
			}
		}

		/// <summary>Gets or sets the color used when displaying a normal link.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to displaying a normal link. The default color is specified by the system, typically this color is <see langword="Color.Blue" />.</returns>
		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06002801 RID: 10241 RVA: 0x000BA5D0 File Offset: 0x000B87D0
		// (set) Token: 0x06002802 RID: 10242 RVA: 0x000BA5F9 File Offset: 0x000B87F9
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelLinkColorDescr")]
		public Color LinkColor
		{
			get
			{
				if (!this.linkColor.IsEmpty)
				{
					return this.linkColor;
				}
				if (SystemInformation.HighContrast)
				{
					return SystemColors.HotTrack;
				}
				return this.IELinkColor;
			}
			set
			{
				if (this.linkColor != value)
				{
					this.linkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		/// <summary>Gets the collection of links contained within the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.LinkLabel.LinkCollection" /> that represents the links contained within the <see cref="T:System.Windows.Forms.LinkLabel" /> control.</returns>
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06002803 RID: 10243 RVA: 0x000BA617 File Offset: 0x000B8817
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public LinkLabel.LinkCollection Links
		{
			get
			{
				if (this.linkCollection == null)
				{
					this.linkCollection = new LinkLabel.LinkCollection(this);
				}
				return this.linkCollection;
			}
		}

		/// <summary>Gets or sets a value indicating whether a link should be displayed as though it were visited.</summary>
		/// <returns>
		///     <see langword="true" /> if links should display as though they were visited; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x000BA633 File Offset: 0x000B8833
		// (set) Token: 0x06002805 RID: 10245 RVA: 0x000BA65C File Offset: 0x000B885C
		[DefaultValue(false)]
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelLinkVisitedDescr")]
		public bool LinkVisited
		{
			get
			{
				return this.links.Count != 0 && ((LinkLabel.Link)this.links[0]).Visited;
			}
			set
			{
				if (value != this.LinkVisited)
				{
					if (this.links.Count == 0)
					{
						this.Links.Add(new LinkLabel.Link(this));
					}
					((LinkLabel.Link)this.links[0]).Visited = value;
				}
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x0000E214 File Offset: 0x0000C414
		internal override bool OwnerDraw
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the mouse pointer to use when the mouse pointer is within the bounds of the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> to use when the mouse pointer is within the <see cref="T:System.Windows.Forms.LinkLabel" /> bounds.</returns>
		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x000BA6A8 File Offset: 0x000B88A8
		// (set) Token: 0x06002808 RID: 10248 RVA: 0x000BA6B0 File Offset: 0x000B88B0
		protected Cursor OverrideCursor
		{
			get
			{
				return this.overrideCursor;
			}
			set
			{
				if (this.overrideCursor != value)
				{
					this.overrideCursor = value;
					if (base.IsHandleCreated)
					{
						NativeMethods.POINT point = new NativeMethods.POINT();
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						UnsafeNativeMethods.GetCursorPos(point);
						UnsafeNativeMethods.GetWindowRect(new HandleRef(this, base.Handle), ref rect);
						if ((rect.left <= point.x && point.x < rect.right && rect.top <= point.y && point.y < rect.bottom) || UnsafeNativeMethods.GetCapture() == base.Handle)
						{
							base.SendMessage(32, base.Handle, 1);
						}
					}
				}
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.Label.TabStop" /> property changes.</summary>
		// Token: 0x140001D9 RID: 473
		// (add) Token: 0x06002809 RID: 10249 RVA: 0x000BA763 File Offset: 0x000B8963
		// (remove) Token: 0x0600280A RID: 10250 RVA: 0x000BA76C File Offset: 0x000B896C
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can tab to the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can tab to the <see cref="T:System.Windows.Forms.LinkLabel" />;otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x0600280B RID: 10251 RVA: 0x000BA775 File Offset: 0x000B8975
		// (set) Token: 0x0600280C RID: 10252 RVA: 0x000BA77D File Offset: 0x000B897D
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>Gets or sets the text displayed by the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
		/// <returns>The text displayed by the <see cref="T:System.Windows.Forms.LinkLabel" />.</returns>
		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x0600280D RID: 10253 RVA: 0x000BA786 File Offset: 0x000B8986
		// (set) Token: 0x0600280E RID: 10254 RVA: 0x000BA78E File Offset: 0x000B898E
		[RefreshProperties(RefreshProperties.Repaint)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Gets or sets the interior spacing, in pixels, between the edges of a <see cref="T:System.Windows.Forms.LinkLabel" /> and its contents.</summary>
		/// <returns>
		///     <see cref="T:System.Windows.Forms.Padding" /> values representing the interior spacing, in pixels.</returns>
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600280F RID: 10255 RVA: 0x0002049A File Offset: 0x0001E69A
		// (set) Token: 0x06002810 RID: 10256 RVA: 0x000204A2 File Offset: 0x0001E6A2
		[RefreshProperties(RefreshProperties.Repaint)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		/// <summary>Gets or sets the color used when displaying a link that that has been previously visited.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color used to display links that have been visited. The default color is specified by the system, typically this color is <see langword="Color.Purple" />.</returns>
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000BA797 File Offset: 0x000B8997
		// (set) Token: 0x06002812 RID: 10258 RVA: 0x000BA7C0 File Offset: 0x000B89C0
		[SRCategory("CatAppearance")]
		[SRDescription("LinkLabelVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (!this.visitedLinkColor.IsEmpty)
				{
					return this.visitedLinkColor;
				}
				if (SystemInformation.HighContrast)
				{
					return LinkUtilities.GetVisitedLinkColor();
				}
				return this.IEVisitedLinkColor;
			}
			set
			{
				if (this.visitedLinkColor != value)
				{
					this.visitedLinkColor = value;
					this.InvalidateLink(null);
				}
			}
		}

		/// <summary>Occurs when a link is clicked within the control.</summary>
		// Token: 0x140001DA RID: 474
		// (add) Token: 0x06002813 RID: 10259 RVA: 0x000BA7DE File Offset: 0x000B89DE
		// (remove) Token: 0x06002814 RID: 10260 RVA: 0x000BA7F1 File Offset: 0x000B89F1
		[WinCategory("Action")]
		[SRDescription("LinkLabelLinkClickedDescr")]
		public event LinkLabelLinkClickedEventHandler LinkClicked
		{
			add
			{
				base.Events.AddHandler(LinkLabel.EventLinkClicked, value);
			}
			remove
			{
				base.Events.RemoveHandler(LinkLabel.EventLinkClicked, value);
			}
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000BA804 File Offset: 0x000B8A04
		internal static Rectangle CalcTextRenderBounds(Rectangle textRect, Rectangle clientRect, ContentAlignment align)
		{
			int x;
			if ((align & WindowsFormsUtils.AnyRightAlign) != (ContentAlignment)0)
			{
				x = clientRect.Right - textRect.Width;
			}
			else if ((align & WindowsFormsUtils.AnyCenterAlign) != (ContentAlignment)0)
			{
				x = (clientRect.Width - textRect.Width) / 2;
			}
			else
			{
				x = clientRect.X;
			}
			int y;
			if ((align & WindowsFormsUtils.AnyBottomAlign) != (ContentAlignment)0)
			{
				y = clientRect.Bottom - textRect.Height;
			}
			else if ((align & WindowsFormsUtils.AnyMiddleAlign) != (ContentAlignment)0)
			{
				y = (clientRect.Height - textRect.Height) / 2;
			}
			else
			{
				y = clientRect.Y;
			}
			int width;
			if (textRect.Width > clientRect.Width)
			{
				x = clientRect.X;
				width = clientRect.Width;
			}
			else
			{
				width = textRect.Width;
			}
			int height;
			if (textRect.Height > clientRect.Height)
			{
				y = clientRect.Y;
				height = clientRect.Height;
			}
			else
			{
				height = textRect.Height;
			}
			return new Rectangle(x, y, width, height);
		}

		/// <summary>Creates a new accessibility object for the <see cref="T:System.Windows.Forms.LinkLabel" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Forms.AccessibleObject" /> for the control.</returns>
		// Token: 0x06002816 RID: 10262 RVA: 0x000BA8EE File Offset: 0x000B8AEE
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new LinkLabel.LinkLabelAccessibleObject(this);
		}

		/// <summary>Creates a handle for this control. This method is called by the .NET Framework, this should not be called. Inheriting classes should always call <see langword="base.createHandle" /> when overriding this method.</summary>
		// Token: 0x06002817 RID: 10263 RVA: 0x000BA8F6 File Offset: 0x000B8AF6
		protected override void CreateHandle()
		{
			base.CreateHandle();
			this.InvalidateTextLayout();
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x000BA904 File Offset: 0x000B8B04
		internal override bool CanUseTextRenderer
		{
			get
			{
				StringInfo stringInfo = new StringInfo(this.Text);
				return this.LinkArea.Start == 0 && (this.LinkArea.Length == 0 || this.LinkArea.Length == stringInfo.LengthInTextElements);
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000BA957 File Offset: 0x000B8B57
		internal override bool UseGDIMeasuring()
		{
			return !this.UseCompatibleTextRendering;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000BA964 File Offset: 0x000B8B64
		private static int ConvertToCharIndex(int index, string text)
		{
			if (index <= 0)
			{
				return 0;
			}
			if (string.IsNullOrEmpty(text))
			{
				return index;
			}
			StringInfo stringInfo = new StringInfo(text);
			int lengthInTextElements = stringInfo.LengthInTextElements;
			if (index > lengthInTextElements)
			{
				return index - lengthInTextElements + text.Length;
			}
			string text2 = stringInfo.SubstringByTextElements(0, index);
			return text2.Length;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000BA9B0 File Offset: 0x000B8BB0
		private void EnsureRun(Graphics g)
		{
			if (this.textLayoutValid)
			{
				return;
			}
			if (this.textRegion != null)
			{
				this.textRegion.Dispose();
				this.textRegion = null;
			}
			if (this.Text.Length == 0)
			{
				this.Links.Clear();
				this.Links.Add(new LinkLabel.Link(0, -1));
				this.textLayoutValid = true;
				return;
			}
			StringFormat stringFormat = this.CreateStringFormat();
			string text = this.Text;
			try
			{
				Font font = new Font(this.Font, this.Font.Style | FontStyle.Underline);
				Graphics graphics = null;
				try
				{
					if (g == null)
					{
						graphics = (g = base.CreateGraphicsInternal());
					}
					if (this.UseCompatibleTextRendering)
					{
						Region[] array = g.MeasureCharacterRanges(text, font, this.ClientRectWithPadding, stringFormat);
						int num = 0;
						for (int i = 0; i < this.Links.Count; i++)
						{
							LinkLabel.Link link = this.Links[i];
							int num2 = LinkLabel.ConvertToCharIndex(link.Start, text);
							int num3 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
							if (this.LinkInText(num2, num3 - num2))
							{
								this.Links[i].VisualRegion = array[num];
								num++;
							}
						}
						this.textRegion = array[array.Length - 1];
					}
					else
					{
						Rectangle clientRectWithPadding = this.ClientRectWithPadding;
						Size size = new Size(clientRectWithPadding.Width, clientRectWithPadding.Height);
						TextFormatFlags textFormatFlags = this.CreateTextFormatFlags(size);
						Size size2 = TextRenderer.MeasureText(text, font, size, textFormatFlags);
						int iLeftMargin;
						int iRightMargin;
						using (WindowsGraphics windowsGraphics = WindowsGraphics.FromGraphics(g))
						{
							if ((textFormatFlags & TextFormatFlags.NoPadding) == TextFormatFlags.NoPadding)
							{
								windowsGraphics.TextPadding = TextPaddingOptions.NoPadding;
							}
							else if ((textFormatFlags & TextFormatFlags.LeftAndRightPadding) == TextFormatFlags.LeftAndRightPadding)
							{
								windowsGraphics.TextPadding = TextPaddingOptions.LeftAndRightPadding;
							}
							using (WindowsFont windowsFont = WindowsGraphicsCacheManager.GetWindowsFont(this.Font))
							{
								IntNativeMethods.DRAWTEXTPARAMS textMargins = windowsGraphics.GetTextMargins(windowsFont);
								iLeftMargin = textMargins.iLeftMargin;
								iRightMargin = textMargins.iRightMargin;
							}
						}
						Rectangle rectangle = new Rectangle(clientRectWithPadding.X + iLeftMargin, clientRectWithPadding.Y, size2.Width - iRightMargin - iLeftMargin, size2.Height);
						rectangle = LinkLabel.CalcTextRenderBounds(rectangle, clientRectWithPadding, base.RtlTranslateContent(this.TextAlign));
						Region visualRegion = new Region(rectangle);
						if (this.links != null && this.links.Count == 1)
						{
							this.Links[0].VisualRegion = visualRegion;
						}
						this.textRegion = visualRegion;
					}
				}
				finally
				{
					font.Dispose();
					font = null;
					if (graphics != null)
					{
						graphics.Dispose();
						graphics = null;
					}
				}
				this.textLayoutValid = true;
			}
			finally
			{
				stringFormat.Dispose();
			}
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000BACB4 File Offset: 0x000B8EB4
		internal override StringFormat CreateStringFormat()
		{
			StringFormat stringFormat = base.CreateStringFormat();
			if (string.IsNullOrEmpty(this.Text))
			{
				return stringFormat;
			}
			CharacterRange[] measurableCharacterRanges = this.AdjustCharacterRangesForSurrogateChars();
			stringFormat.SetMeasurableCharacterRanges(measurableCharacterRanges);
			return stringFormat;
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000BACE8 File Offset: 0x000B8EE8
		private CharacterRange[] AdjustCharacterRangesForSurrogateChars()
		{
			string text = this.Text;
			if (string.IsNullOrEmpty(text))
			{
				return new CharacterRange[0];
			}
			StringInfo stringInfo = new StringInfo(text);
			int lengthInTextElements = stringInfo.LengthInTextElements;
			ArrayList arrayList = new ArrayList(this.Links.Count);
			foreach (object obj in this.Links)
			{
				LinkLabel.Link link = (LinkLabel.Link)obj;
				int num = LinkLabel.ConvertToCharIndex(link.Start, text);
				int num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
				if (this.LinkInText(num, num2 - num))
				{
					int num3 = Math.Min(link.Length, lengthInTextElements - link.Start);
					arrayList.Add(new CharacterRange(num, LinkLabel.ConvertToCharIndex(link.Start + num3, text) - num));
				}
			}
			CharacterRange[] array = new CharacterRange[arrayList.Count + 1];
			arrayList.CopyTo(array, 0);
			array[array.Length - 1] = new CharacterRange(0, text.Length);
			return array;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000BAE24 File Offset: 0x000B9024
		private bool IsOneLink()
		{
			if (this.links == null || this.links.Count != 1 || this.Text == null)
			{
				return false;
			}
			StringInfo stringInfo = new StringInfo(this.Text);
			return this.LinkArea.Start == 0 && this.LinkArea.Length == stringInfo.LengthInTextElements;
		}

		/// <summary>Gets the link located at the specified client coordinates.</summary>
		/// <param name="x">The horizontal coordinate of the point to search for a link. </param>
		/// <param name="y">The vertical coordinate of the point to search for a link. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link located at the specified coordinates. If the point does not contain a link, <see langword="null" /> is returned.</returns>
		// Token: 0x0600281F RID: 10271 RVA: 0x000BAE88 File Offset: 0x000B9088
		protected LinkLabel.Link PointInLink(int x, int y)
		{
			Graphics graphics = base.CreateGraphicsInternal();
			LinkLabel.Link result = null;
			try
			{
				this.EnsureRun(graphics);
				foreach (object obj in this.links)
				{
					LinkLabel.Link link = (LinkLabel.Link)obj;
					if (link.VisualRegion != null && link.VisualRegion.IsVisible(x, y, graphics))
					{
						result = link;
						break;
					}
				}
			}
			finally
			{
				graphics.Dispose();
				graphics = null;
			}
			return result;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000BAF20 File Offset: 0x000B9120
		private void InvalidateLink(LinkLabel.Link link)
		{
			if (base.IsHandleCreated)
			{
				if (link == null || link.VisualRegion == null || this.IsOneLink())
				{
					base.Invalidate();
					return;
				}
				base.Invalidate(link.VisualRegion);
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000BAF50 File Offset: 0x000B9150
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

		// Token: 0x06002822 RID: 10274 RVA: 0x000BAF9F File Offset: 0x000B919F
		private void InvalidateTextLayout()
		{
			this.textLayoutValid = false;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000BAFA8 File Offset: 0x000B91A8
		private bool LinkInText(int start, int length)
		{
			return 0 <= start && start < this.Text.Length && 0 < length;
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Forms.IButtonControl.DialogResult" />.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06002824 RID: 10276 RVA: 0x000BAFC2 File Offset: 0x000B91C2
		// (set) Token: 0x06002825 RID: 10277 RVA: 0x000BAFCA File Offset: 0x000B91CA
		DialogResult IButtonControl.DialogResult
		{
			get
			{
				return this.dialogResult;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 7))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DialogResult));
				}
				this.dialogResult = value;
			}
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Forms.LinkLabel" /> control that it is the default button.</summary>
		/// <param name="value">
		///       <see langword="true" /> if the control should behave as a default button; otherwise, <see langword="false" />.</param>
		// Token: 0x06002826 RID: 10278 RVA: 0x0000701A File Offset: 0x0000521A
		void IButtonControl.NotifyDefault(bool value)
		{
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.GotFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002827 RID: 10279 RVA: 0x000BAFFC File Offset: 0x000B91FC
		protected override void OnGotFocus(EventArgs e)
		{
			if (!this.processingOnGotFocus)
			{
				base.OnGotFocus(e);
				this.processingOnGotFocus = true;
			}
			try
			{
				LinkLabel.Link link = this.FocusLink;
				if (link == null)
				{
					IntSecurity.ModifyFocus.Assert();
					this.Select(true, true);
				}
				else
				{
					this.InvalidateLink(link);
					this.UpdateAccessibilityLink(link);
				}
			}
			finally
			{
				if (this.processingOnGotFocus)
				{
					this.processingOnGotFocus = false;
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.LostFocus" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002828 RID: 10280 RVA: 0x000BB070 File Offset: 0x000B9270
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.FocusLink != null)
			{
				this.InvalidateLink(this.FocusLink);
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnKeyDown(System.Windows.Forms.KeyEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x06002829 RID: 10281 RVA: 0x000BB08D File Offset: 0x000B928D
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Return && this.FocusLink != null && this.FocusLink.Enabled)
			{
				this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.FocusLink));
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseLeave(System.EventArgs)" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x0600282A RID: 10282 RVA: 0x000BB0C8 File Offset: 0x000B92C8
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.Enabled)
			{
				return;
			}
			foreach (object obj in this.links)
			{
				LinkLabel.Link link = (LinkLabel.Link)obj;
				if ((link.State & LinkState.Hover) == LinkState.Hover || (link.State & LinkState.Active) == LinkState.Active)
				{
					bool flag = (link.State & LinkState.Active) == LinkState.Active;
					link.State &= (LinkState)(-4);
					if (flag || this.hoverLinkFont != this.linkFont)
					{
						this.InvalidateLink(link);
					}
					this.OverrideCursor = null;
				}
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseDown(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600282B RID: 10283 RVA: 0x000BB17C File Offset: 0x000B937C
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.Enabled || e.Clicks > 1)
			{
				this.receivedDoubleClick = true;
				return;
			}
			for (int i = 0; i < this.links.Count; i++)
			{
				if ((((LinkLabel.Link)this.links[i]).State & LinkState.Hover) == LinkState.Hover)
				{
					((LinkLabel.Link)this.links[i]).State |= LinkState.Active;
					this.FocusInternal();
					if (((LinkLabel.Link)this.links[i]).Enabled)
					{
						this.FocusLink = (LinkLabel.Link)this.links[i];
						this.InvalidateLink(this.FocusLink);
					}
					base.CaptureInternal = true;
					return;
				}
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseUp(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600282C RID: 10284 RVA: 0x000BB248 File Offset: 0x000B9448
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (!base.Enabled || e.Clicks > 1 || this.receivedDoubleClick)
			{
				this.receivedDoubleClick = false;
				return;
			}
			for (int i = 0; i < this.links.Count; i++)
			{
				if ((((LinkLabel.Link)this.links[i]).State & LinkState.Active) == LinkState.Active)
				{
					((LinkLabel.Link)this.links[i]).State &= (LinkState)(-3);
					this.InvalidateLink((LinkLabel.Link)this.links[i]);
					base.CaptureInternal = false;
					LinkLabel.Link link = this.PointInLink(e.X, e.Y);
					if (link != null && link == this.FocusLink && link.Enabled)
					{
						this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(link, e.Button));
					}
				}
			}
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x0600282D RID: 10285 RVA: 0x000BB33C File Offset: 0x000B953C
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!base.Enabled)
			{
				return;
			}
			LinkLabel.Link link = null;
			foreach (object obj in this.links)
			{
				LinkLabel.Link link2 = (LinkLabel.Link)obj;
				if ((link2.State & LinkState.Hover) == LinkState.Hover)
				{
					link = link2;
					break;
				}
			}
			LinkLabel.Link link3 = this.PointInLink(e.X, e.Y);
			if (link3 != link)
			{
				if (link != null)
				{
					link.State &= (LinkState)(-2);
				}
				if (link3 != null)
				{
					link3.State |= LinkState.Hover;
					if (link3.Enabled)
					{
						this.OverrideCursor = Cursors.Hand;
					}
				}
				else
				{
					this.OverrideCursor = null;
				}
				if (this.hoverLinkFont != this.linkFont)
				{
					if (link != null)
					{
						this.InvalidateLink(link);
					}
					if (link3 != null)
					{
						this.InvalidateLink(link3);
					}
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.LinkLabel.LinkClicked" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.LinkLabelLinkClickedEventArgs" /> that contains the event data. </param>
		// Token: 0x0600282E RID: 10286 RVA: 0x000BB42C File Offset: 0x000B962C
		protected virtual void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
		{
			LinkLabelLinkClickedEventHandler linkLabelLinkClickedEventHandler = (LinkLabelLinkClickedEventHandler)base.Events[LinkLabel.EventLinkClicked];
			if (linkLabelLinkClickedEventHandler != null)
			{
				linkLabelLinkClickedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.PaddingChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x0600282F RID: 10287 RVA: 0x000BB45A File Offset: 0x000B965A
		protected override void OnPaddingChanged(EventArgs e)
		{
			base.OnPaddingChanged(e);
			this.InvalidateTextLayout();
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06002830 RID: 10288 RVA: 0x000BB46C File Offset: 0x000B966C
		protected override void OnPaint(PaintEventArgs e)
		{
			RectangleF rectangleF = RectangleF.Empty;
			base.Animate();
			ImageAnimator.UpdateFrames(base.Image);
			this.EnsureRun(e.Graphics);
			if (this.Text.Length == 0)
			{
				this.PaintLinkBackground(e.Graphics);
			}
			else
			{
				if (base.AutoEllipsis)
				{
					Rectangle clientRectWithPadding = this.ClientRectWithPadding;
					Size preferredSize = this.GetPreferredSize(new Size(clientRectWithPadding.Width, clientRectWithPadding.Height));
					this.showToolTip = (clientRectWithPadding.Width < preferredSize.Width || clientRectWithPadding.Height < preferredSize.Height);
				}
				else
				{
					this.showToolTip = false;
				}
				if (base.Enabled)
				{
					bool flag = !base.GetStyle(ControlStyles.OptimizedDoubleBuffer);
					SolidBrush solidBrush = new SolidBrush(this.ForeColor);
					SolidBrush solidBrush2 = new SolidBrush(this.LinkColor);
					try
					{
						if (!flag)
						{
							this.PaintLinkBackground(e.Graphics);
						}
						LinkUtilities.EnsureLinkFonts(this.Font, this.LinkBehavior, ref this.linkFont, ref this.hoverLinkFont);
						Region clip = e.Graphics.Clip;
						try
						{
							if (this.IsOneLink())
							{
								e.Graphics.Clip = clip;
								RectangleF[] regionScans = ((LinkLabel.Link)this.links[0]).VisualRegion.GetRegionScans(e.Graphics.Transform);
								if (regionScans == null || regionScans.Length == 0)
								{
									goto IL_2B6;
								}
								if (this.UseCompatibleTextRendering)
								{
									rectangleF = new RectangleF(regionScans[0].Location, SizeF.Empty);
									foreach (RectangleF b in regionScans)
									{
										rectangleF = RectangleF.Union(rectangleF, b);
									}
								}
								else
								{
									rectangleF = this.ClientRectWithPadding;
									Size size = rectangleF.Size.ToSize();
									Size textSize = base.MeasureTextCache.GetTextSize(this.Text, this.Font, size, this.CreateTextFormatFlags(size));
									rectangleF.Width = (float)textSize.Width;
									if ((float)textSize.Height < rectangleF.Height)
									{
										rectangleF.Height = (float)textSize.Height;
									}
									rectangleF = LinkLabel.CalcTextRenderBounds(Rectangle.Round(rectangleF), this.ClientRectWithPadding, base.RtlTranslateContent(this.TextAlign));
								}
								using (Region region = new Region(rectangleF))
								{
									e.Graphics.ExcludeClip(region);
									goto IL_2B6;
								}
							}
							foreach (object obj in this.links)
							{
								LinkLabel.Link link = (LinkLabel.Link)obj;
								if (link.VisualRegion != null)
								{
									e.Graphics.ExcludeClip(link.VisualRegion);
								}
							}
							IL_2B6:
							if (!this.IsOneLink())
							{
								this.PaintLink(e.Graphics, null, solidBrush, solidBrush2, flag, rectangleF);
							}
							foreach (object obj2 in this.links)
							{
								LinkLabel.Link link2 = (LinkLabel.Link)obj2;
								this.PaintLink(e.Graphics, link2, solidBrush, solidBrush2, flag, rectangleF);
							}
							if (flag)
							{
								e.Graphics.Clip = clip;
								e.Graphics.ExcludeClip(this.textRegion);
								this.PaintLinkBackground(e.Graphics);
							}
							goto IL_456;
						}
						finally
						{
							e.Graphics.Clip = clip;
						}
					}
					finally
					{
						solidBrush.Dispose();
						solidBrush2.Dispose();
					}
				}
				Region clip2 = e.Graphics.Clip;
				try
				{
					this.PaintLinkBackground(e.Graphics);
					e.Graphics.IntersectClip(this.textRegion);
					if (this.UseCompatibleTextRendering)
					{
						StringFormat format = this.CreateStringFormat();
						ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, base.DisabledColor, this.ClientRectWithPadding, format);
					}
					else
					{
						IntPtr hdc = e.Graphics.GetHdc();
						Color nearestColor;
						try
						{
							using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
							{
								nearestColor = windowsGraphics.GetNearestColor(base.DisabledColor);
							}
						}
						finally
						{
							e.Graphics.ReleaseHdc();
						}
						Rectangle clientRectWithPadding2 = this.ClientRectWithPadding;
						ControlPaint.DrawStringDisabled(e.Graphics, this.Text, this.Font, nearestColor, clientRectWithPadding2, this.CreateTextFormatFlags(clientRectWithPadding2.Size));
					}
				}
				finally
				{
					e.Graphics.Clip = clip2;
				}
			}
			IL_456:
			base.RaisePaintEvent(this, e);
		}

		/// <summary>Paints the background of the control.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		// Token: 0x06002831 RID: 10289 RVA: 0x000BB99C File Offset: 0x000B9B9C
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Image image = base.Image;
			if (image != null)
			{
				Region clip = e.Graphics.Clip;
				Rectangle rect = base.CalcImageRenderBounds(image, base.ClientRectangle, base.RtlTranslateAlignment(base.ImageAlign));
				e.Graphics.ExcludeClip(rect);
				try
				{
					base.OnPaintBackground(e);
				}
				finally
				{
					e.Graphics.Clip = clip;
				}
				e.Graphics.IntersectClip(rect);
				try
				{
					base.OnPaintBackground(e);
					base.DrawImage(e.Graphics, image, base.ClientRectangle, base.RtlTranslateAlignment(base.ImageAlign));
					return;
				}
				finally
				{
					e.Graphics.Clip = clip;
				}
			}
			base.OnPaintBackground(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002832 RID: 10290 RVA: 0x000BBA64 File Offset: 0x000B9C64
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.InvalidateTextLayout();
			this.InvalidateLinkFonts();
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Label.AutoSizeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002833 RID: 10291 RVA: 0x000BBA7F File Offset: 0x000B9C7F
		protected override void OnAutoSizeChanged(EventArgs e)
		{
			base.OnAutoSizeChanged(e);
			this.InvalidateTextLayout();
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000BBA8E File Offset: 0x000B9C8E
		internal override void OnAutoEllipsisChanged()
		{
			base.OnAutoEllipsisChanged();
			this.InvalidateTextLayout();
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002835 RID: 10293 RVA: 0x000BBA9C File Offset: 0x000B9C9C
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			if (!base.Enabled)
			{
				for (int i = 0; i < this.links.Count; i++)
				{
					((LinkLabel.Link)this.links[i]).State &= (LinkState)(-4);
				}
				this.OverrideCursor = null;
			}
			this.InvalidateTextLayout();
			base.Invalidate();
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Forms.Control.TextChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002836 RID: 10294 RVA: 0x000BBB00 File Offset: 0x000B9D00
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.InvalidateTextLayout();
			this.UpdateSelectability();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Label.TextAlignChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002837 RID: 10295 RVA: 0x000BBB15 File Offset: 0x000B9D15
		protected override void OnTextAlignChanged(EventArgs e)
		{
			base.OnTextAlignChanged(e);
			this.InvalidateTextLayout();
			this.UpdateSelectability();
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000BBB2C File Offset: 0x000B9D2C
		private void PaintLink(Graphics g, LinkLabel.Link link, SolidBrush foreBrush, SolidBrush linkBrush, bool optimizeBackgroundRendering, RectangleF finalrect)
		{
			Font font = this.Font;
			if (link != null)
			{
				if (link.VisualRegion != null)
				{
					Color color = Color.Empty;
					LinkState state = link.State;
					if ((state & LinkState.Hover) == LinkState.Hover)
					{
						font = this.hoverLinkFont;
					}
					else
					{
						font = this.linkFont;
					}
					if (link.Enabled)
					{
						if ((state & LinkState.Active) == LinkState.Active)
						{
							color = this.ActiveLinkColor;
						}
						else if ((state & LinkState.Visited) == LinkState.Visited)
						{
							color = this.VisitedLinkColor;
						}
					}
					else
					{
						color = this.DisabledLinkColor;
					}
					if (this.IsOneLink())
					{
						g.Clip = new Region(finalrect);
					}
					else
					{
						g.Clip = link.VisualRegion;
					}
					if (optimizeBackgroundRendering)
					{
						this.PaintLinkBackground(g);
					}
					if (this.UseCompatibleTextRendering)
					{
						SolidBrush solidBrush = (color == Color.Empty) ? linkBrush : new SolidBrush(color);
						StringFormat format = this.CreateStringFormat();
						g.DrawString(this.Text, font, solidBrush, this.ClientRectWithPadding, format);
						if (solidBrush != linkBrush)
						{
							solidBrush.Dispose();
						}
					}
					else
					{
						if (color == Color.Empty)
						{
							color = linkBrush.Color;
						}
						IntPtr hdc = g.GetHdc();
						try
						{
							using (WindowsGraphics windowsGraphics = WindowsGraphics.FromHdc(hdc))
							{
								color = windowsGraphics.GetNearestColor(color);
							}
						}
						finally
						{
							g.ReleaseHdc();
						}
						Rectangle clientRectWithPadding = this.ClientRectWithPadding;
						TextRenderer.DrawText(g, this.Text, font, clientRectWithPadding, color, this.CreateTextFormatFlags(clientRectWithPadding.Size));
					}
					if (this.Focused && this.ShowFocusCues && this.FocusLink == link)
					{
						RectangleF[] regionScans = link.VisualRegion.GetRegionScans(g.Transform);
						if (regionScans != null && regionScans.Length != 0)
						{
							if (this.IsOneLink())
							{
								Rectangle rectangle = Rectangle.Ceiling(finalrect);
								ControlPaint.DrawFocusRectangle(g, rectangle, this.ForeColor, this.BackColor);
								return;
							}
							foreach (RectangleF value in regionScans)
							{
								ControlPaint.DrawFocusRectangle(g, Rectangle.Ceiling(value), this.ForeColor, this.BackColor);
							}
							return;
						}
					}
				}
			}
			else
			{
				g.IntersectClip(this.textRegion);
				if (optimizeBackgroundRendering)
				{
					this.PaintLinkBackground(g);
				}
				if (this.UseCompatibleTextRendering)
				{
					StringFormat format2 = this.CreateStringFormat();
					g.DrawString(this.Text, font, foreBrush, this.ClientRectWithPadding, format2);
					return;
				}
				IntPtr hdc2 = g.GetHdc();
				Color nearestColor;
				try
				{
					using (WindowsGraphics windowsGraphics2 = WindowsGraphics.FromHdc(hdc2))
					{
						nearestColor = windowsGraphics2.GetNearestColor(foreBrush.Color);
					}
				}
				finally
				{
					g.ReleaseHdc();
				}
				Rectangle clientRectWithPadding2 = this.ClientRectWithPadding;
				TextRenderer.DrawText(g, this.Text, font, clientRectWithPadding2, nearestColor, this.CreateTextFormatFlags(clientRectWithPadding2.Size));
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000BBE00 File Offset: 0x000BA000
		private void PaintLinkBackground(Graphics g)
		{
			using (PaintEventArgs paintEventArgs = new PaintEventArgs(g, base.ClientRectangle))
			{
				base.InvokePaintBackground(this, paintEventArgs);
			}
		}

		/// <summary>Generates a <see cref="E:System.Windows.Forms.Control.Click" /> event for the <see cref="T:System.Windows.Forms.LinkLabel" /> control.</summary>
		// Token: 0x0600283A RID: 10298 RVA: 0x000BBE40 File Offset: 0x000BA040
		void IButtonControl.PerformClick()
		{
			if (this.FocusLink == null && this.Links.Count > 0)
			{
				string text = this.Text;
				foreach (object obj in this.Links)
				{
					LinkLabel.Link link = (LinkLabel.Link)obj;
					int num = LinkLabel.ConvertToCharIndex(link.Start, text);
					int num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					if (link.Enabled && this.LinkInText(num, num2 - num))
					{
						this.FocusLink = link;
						break;
					}
				}
			}
			if (this.FocusLink != null)
			{
				this.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.FocusLink));
			}
		}

		/// <summary>Processes a dialog key. </summary>
		/// <param name="keyData">Key code and modifier flags. </param>
		/// <returns>
		///     <see langword="true" /> to consume the key; <see langword="false" /> to allow further processing.</returns>
		// Token: 0x0600283B RID: 10299 RVA: 0x000BBF10 File Offset: 0x000BA110
		[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Control | Keys.Alt)) != Keys.Alt)
			{
				Keys keys = keyData & Keys.KeyCode;
				if (keys != Keys.Tab)
				{
					if (keys - Keys.Left > 1)
					{
						if (keys - Keys.Right <= 1)
						{
							if (this.FocusNextLink(true))
							{
								return true;
							}
						}
					}
					else if (this.FocusNextLink(false))
					{
						return true;
					}
				}
				else if (this.TabStop)
				{
					bool forward = (keyData & Keys.Shift) != Keys.Shift;
					if (this.FocusNextLink(forward))
					{
						return true;
					}
				}
			}
			return base.ProcessDialogKey(keyData);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000BBF8C File Offset: 0x000BA18C
		private bool FocusNextLink(bool forward)
		{
			int num = -1;
			if (this.focusLink != null)
			{
				for (int i = 0; i < this.links.Count; i++)
				{
					if (this.links[i] == this.focusLink)
					{
						num = i;
						break;
					}
				}
			}
			num = this.GetNextLinkIndex(num, forward);
			if (num != -1)
			{
				this.FocusLink = this.Links[num];
				return true;
			}
			this.FocusLink = null;
			return false;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000BBFFC File Offset: 0x000BA1FC
		private int GetNextLinkIndex(int focusIndex, bool forward)
		{
			string text = this.Text;
			int num = 0;
			int num2 = 0;
			if (forward)
			{
				do
				{
					focusIndex++;
					LinkLabel.Link link;
					if (focusIndex < this.Links.Count)
					{
						link = this.Links[focusIndex];
						num = LinkLabel.ConvertToCharIndex(link.Start, text);
						num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					}
					else
					{
						link = null;
					}
					if (link == null || link.Enabled)
					{
						break;
					}
				}
				while (this.LinkInText(num, num2 - num));
			}
			else
			{
				LinkLabel.Link link;
				do
				{
					focusIndex--;
					if (focusIndex >= 0)
					{
						link = this.Links[focusIndex];
						num = LinkLabel.ConvertToCharIndex(link.Start, text);
						num2 = LinkLabel.ConvertToCharIndex(link.Start + link.Length, text);
					}
					else
					{
						link = null;
					}
				}
				while (link != null && !link.Enabled && this.LinkInText(num, num2 - num));
			}
			if (focusIndex < 0 || focusIndex >= this.links.Count)
			{
				return -1;
			}
			return focusIndex;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000BC0DC File Offset: 0x000BA2DC
		private void ResetLinkArea()
		{
			this.LinkArea = new LinkArea(0, -1);
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000BC0EB File Offset: 0x000BA2EB
		internal void ResetActiveLinkColor()
		{
			this.activeLinkColor = Color.Empty;
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000BC0F8 File Offset: 0x000BA2F8
		internal void ResetDisabledLinkColor()
		{
			this.disabledLinkColor = Color.Empty;
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000BC105 File Offset: 0x000BA305
		internal void ResetLinkColor()
		{
			this.linkColor = Color.Empty;
			this.InvalidateLink(null);
		}

		// Token: 0x06002842 RID: 10306 RVA: 0x000BC119 File Offset: 0x000BA319
		private void ResetVisitedLinkColor()
		{
			this.visitedLinkColor = Color.Empty;
		}

		/// <summary>Performs the work of setting the bounds of this control. </summary>
		/// <param name="x">New left of the control. </param>
		/// <param name="y">New right of the control. </param>
		/// <param name="width">New width of the control. </param>
		/// <param name="height">New height of the control. </param>
		/// <param name="specified">Which values were specified. This parameter reflects user intent, not which values have changed. </param>
		// Token: 0x06002843 RID: 10307 RVA: 0x000BC126 File Offset: 0x000BA326
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			this.InvalidateTextLayout();
			base.Invalidate();
			base.SetBoundsCore(x, y, width, height, specified);
		}

		/// <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
		/// <param name="directed">
		///   <see langword="true" /> to specify the direction of the control to select; otherwise, <see langword="false" />.</param>
		/// <param name="forward">
		///   <see langword="true" /> to move forward in the tab order; <see langword="false" /> to move backward in the tab order.</param>
		// Token: 0x06002844 RID: 10308 RVA: 0x000BC144 File Offset: 0x000BA344
		protected override void Select(bool directed, bool forward)
		{
			if (directed && this.links.Count > 0)
			{
				int focusIndex = -1;
				if (this.FocusLink != null)
				{
					focusIndex = this.links.IndexOf(this.FocusLink);
				}
				this.FocusLink = null;
				int nextLinkIndex = this.GetNextLinkIndex(focusIndex, forward);
				if (nextLinkIndex == -1)
				{
					if (forward)
					{
						nextLinkIndex = this.GetNextLinkIndex(-1, forward);
					}
					else
					{
						nextLinkIndex = this.GetNextLinkIndex(this.links.Count, forward);
					}
				}
				if (nextLinkIndex != -1)
				{
					this.FocusLink = (LinkLabel.Link)this.links[nextLinkIndex];
				}
			}
			base.Select(directed, forward);
		}

		// Token: 0x06002845 RID: 10309 RVA: 0x000BC1D6 File Offset: 0x000BA3D6
		internal bool ShouldSerializeActiveLinkColor()
		{
			return !this.activeLinkColor.IsEmpty;
		}

		// Token: 0x06002846 RID: 10310 RVA: 0x000BC1E6 File Offset: 0x000BA3E6
		internal bool ShouldSerializeDisabledLinkColor()
		{
			return !this.disabledLinkColor.IsEmpty;
		}

		// Token: 0x06002847 RID: 10311 RVA: 0x000BC1F6 File Offset: 0x000BA3F6
		private bool ShouldSerializeLinkArea()
		{
			return this.links.Count != 1 || this.Links[0].Start != 0 || this.Links[0].length != -1;
		}

		// Token: 0x06002848 RID: 10312 RVA: 0x000BC234 File Offset: 0x000BA434
		internal bool ShouldSerializeLinkColor()
		{
			return !this.linkColor.IsEmpty;
		}

		// Token: 0x06002849 RID: 10313 RVA: 0x000BC244 File Offset: 0x000BA444
		private bool ShouldSerializeUseCompatibleTextRendering()
		{
			return !this.CanUseTextRenderer || this.UseCompatibleTextRendering != Control.UseCompatibleTextRenderingDefault;
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000BC260 File Offset: 0x000BA460
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.visitedLinkColor.IsEmpty;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000BC270 File Offset: 0x000BA470
		private void UpdateAccessibilityLink(LinkLabel.Link focusLink)
		{
			if (!base.IsHandleCreated)
			{
				return;
			}
			int childID = -1;
			for (int i = 0; i < this.links.Count; i++)
			{
				if (this.links[i] == focusLink)
				{
					childID = i;
				}
			}
			base.AccessibilityNotifyClients(AccessibleEvents.Focus, childID);
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000BC2BC File Offset: 0x000BA4BC
		private void ValidateNoOverlappingLinks()
		{
			for (int i = 0; i < this.links.Count; i++)
			{
				LinkLabel.Link link = (LinkLabel.Link)this.links[i];
				if (link.Length < 0)
				{
					throw new InvalidOperationException(SR.GetString("LinkLabelOverlap"));
				}
				for (int j = i; j < this.links.Count; j++)
				{
					if (i != j)
					{
						LinkLabel.Link link2 = (LinkLabel.Link)this.links[j];
						int num = Math.Max(link.Start, link2.Start);
						int num2 = Math.Min(link.Start + link.Length, link2.Start + link2.Length);
						if (num < num2)
						{
							throw new InvalidOperationException(SR.GetString("LinkLabelOverlap"));
						}
					}
				}
			}
		}

		// Token: 0x0600284D RID: 10317 RVA: 0x000BC388 File Offset: 0x000BA588
		private void UpdateSelectability()
		{
			LinkArea linkArea = this.LinkArea;
			bool flag = false;
			string text = this.Text;
			int num = LinkLabel.ConvertToCharIndex(linkArea.Start, text);
			int num2 = LinkLabel.ConvertToCharIndex(linkArea.Start + linkArea.Length, text);
			if (this.LinkInText(num, num2 - num))
			{
				flag = true;
			}
			else if (this.FocusLink != null)
			{
				this.FocusLink = null;
			}
			this.OverrideCursor = null;
			this.TabStop = flag;
			base.SetStyle(ControlStyles.Selectable, flag);
		}

		/// <summary>Gets or sets a value that determines whether to use the <see cref="T:System.Drawing.Graphics" /> class (GDI+) or the <see cref="T:System.Windows.Forms.TextRenderer" /> class (GDI) to render text.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Drawing.Graphics" /> class should be used to perform text rendering for compatibility with versions 1.0 and 1.1. of the .NET Framework; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x000BC404 File Offset: 0x000BA604
		// (set) Token: 0x0600284F RID: 10319 RVA: 0x000BC40C File Offset: 0x000BA60C
		[RefreshProperties(RefreshProperties.Repaint)]
		[SRCategory("CatBehavior")]
		[SRDescription("UseCompatibleTextRenderingDescr")]
		public new bool UseCompatibleTextRendering
		{
			get
			{
				return base.UseCompatibleTextRendering;
			}
			set
			{
				if (base.UseCompatibleTextRendering != value)
				{
					base.UseCompatibleTextRendering = value;
					this.InvalidateTextLayout();
				}
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06002850 RID: 10320 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
		internal override bool SupportsUiaProviders
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x000BC424 File Offset: 0x000BA624
		private void WmSetCursor(ref Message m)
		{
			if (!(m.WParam == base.InternalHandle) || NativeMethods.Util.LOWORD(m.LParam) != 1)
			{
				this.DefWndProc(ref m);
				return;
			}
			if (this.OverrideCursor != null)
			{
				Cursor.CurrentInternal = this.OverrideCursor;
				return;
			}
			Cursor.CurrentInternal = this.Cursor;
		}

		/// <summary>Processes the specified Windows message.</summary>
		/// <param name="msg">The message to process.</param>
		// Token: 0x06002852 RID: 10322 RVA: 0x000BC480 File Offset: 0x000BA680
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message msg)
		{
			int msg2 = msg.Msg;
			if (msg2 == 32)
			{
				this.WmSetCursor(ref msg);
				return;
			}
			base.WndProc(ref msg);
		}

		// Token: 0x0400117B RID: 4475
		private static readonly object EventLinkClicked = new object();

		// Token: 0x0400117C RID: 4476
		private static Color iedisabledLinkColor = Color.Empty;

		// Token: 0x0400117D RID: 4477
		private static LinkLabel.LinkComparer linkComparer = new LinkLabel.LinkComparer();

		// Token: 0x0400117E RID: 4478
		private DialogResult dialogResult;

		// Token: 0x0400117F RID: 4479
		private Color linkColor = Color.Empty;

		// Token: 0x04001180 RID: 4480
		private Color activeLinkColor = Color.Empty;

		// Token: 0x04001181 RID: 4481
		private Color visitedLinkColor = Color.Empty;

		// Token: 0x04001182 RID: 4482
		private Color disabledLinkColor = Color.Empty;

		// Token: 0x04001183 RID: 4483
		private Font linkFont;

		// Token: 0x04001184 RID: 4484
		private Font hoverLinkFont;

		// Token: 0x04001185 RID: 4485
		private bool textLayoutValid;

		// Token: 0x04001186 RID: 4486
		private bool receivedDoubleClick;

		// Token: 0x04001187 RID: 4487
		private ArrayList links = new ArrayList(2);

		// Token: 0x04001188 RID: 4488
		private LinkLabel.Link focusLink;

		// Token: 0x04001189 RID: 4489
		private LinkLabel.LinkCollection linkCollection;

		// Token: 0x0400118A RID: 4490
		private Region textRegion;

		// Token: 0x0400118B RID: 4491
		private Cursor overrideCursor;

		// Token: 0x0400118C RID: 4492
		private bool processingOnGotFocus;

		// Token: 0x0400118D RID: 4493
		private LinkBehavior linkBehavior;

		/// <summary>Represents the collection of links within a <see cref="T:System.Windows.Forms.LinkLabel" /> control.</summary>
		// Token: 0x020005FF RID: 1535
		public class LinkCollection : IList, ICollection, IEnumerable
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabel.LinkCollection" /> class.</summary>
			/// <param name="owner">The <see cref="T:System.Windows.Forms.LinkLabel" /> control that owns the collection. </param>
			// Token: 0x06005BEE RID: 23534 RVA: 0x0018007A File Offset: 0x0017E27A
			public LinkCollection(LinkLabel owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}
				this.owner = owner;
			}

			/// <summary>Gets and sets the link at the specified index within the collection.</summary>
			/// <param name="index">The index of the link in the collection to get. </param>
			/// <returns>An object representing the link located at the specified index within the collection.</returns>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="index" /> is a negative value or greater than the number of items in the collection. </exception>
			// Token: 0x17001600 RID: 5632
			public virtual LinkLabel.Link this[int index]
			{
				get
				{
					return (LinkLabel.Link)this.owner.links[index];
				}
				set
				{
					this.owner.links[index] = value;
					this.owner.links.Sort(LinkLabel.linkComparer);
					this.owner.InvalidateTextLayout();
					this.owner.Invalidate();
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.IList.Item(System.Int32)" />.</summary>
			/// <param name="index">The zero-based index of the element to get or set.</param>
			/// <returns>The element at the specified index.</returns>
			// Token: 0x17001601 RID: 5633
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is LinkLabel.Link)
					{
						this[index] = (LinkLabel.Link)value;
						return;
					}
					throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
				}
			}

			/// <summary>Gets a link with the specified key from the collection.</summary>
			/// <param name="key">The name of the link to retrieve from the collection.</param>
			/// <returns>The <see cref="T:System.Windows.Forms.LinkLabel.Link" /> with the specified key within the collection.</returns>
			// Token: 0x17001602 RID: 5634
			public virtual LinkLabel.Link this[string key]
			{
				get
				{
					if (string.IsNullOrEmpty(key))
					{
						return null;
					}
					int index = this.IndexOfKey(key);
					if (this.IsValidIndex(index))
					{
						return this[index];
					}
					return null;
				}
			}

			/// <summary>Gets the number of links in the collection.</summary>
			/// <returns>The number of links in the collection.</returns>
			// Token: 0x17001603 RID: 5635
			// (get) Token: 0x06005BF4 RID: 23540 RVA: 0x0018015D File Offset: 0x0017E35D
			[Browsable(false)]
			public int Count
			{
				get
				{
					return this.owner.links.Count;
				}
			}

			/// <summary>Gets a value indicating whether links have been added to the <see cref="T:System.Windows.Forms.LinkLabel.LinkCollection" />. </summary>
			/// <returns>
			///     <see langword="true" /> if links have been added to the <see cref="T:System.Windows.Forms.LinkLabel.LinkCollection" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001604 RID: 5636
			// (get) Token: 0x06005BF5 RID: 23541 RVA: 0x0018016F File Offset: 0x0017E36F
			public bool LinksAdded
			{
				get
				{
					return this.linksAdded;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.SyncRoot" />.</summary>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
			// Token: 0x17001605 RID: 5637
			// (get) Token: 0x06005BF6 RID: 23542 RVA: 0x000069BD File Offset: 0x00004BBD
			object ICollection.SyncRoot
			{
				get
				{
					return this;
				}
			}

			/// <summary>For a description of this member, see <see cref="P:System.Collections.ICollection.IsSynchronized" />.</summary>
			/// <returns>
			///     <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
			// Token: 0x17001606 RID: 5638
			// (get) Token: 0x06005BF7 RID: 23543 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/// <summary>For a description of this member, see .<see cref="P:System.Collections.IList.IsFixedSize" />.</summary>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001607 RID: 5639
			// (get) Token: 0x06005BF8 RID: 23544 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/// <summary>Gets a value indicating whether this collection is read-only.</summary>
			/// <returns>
			///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001608 RID: 5640
			// (get) Token: 0x06005BF9 RID: 23545 RVA: 0x0000E0A4 File Offset: 0x0000C2A4
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>Adds a link to the collection.</summary>
			/// <param name="start">The starting character within the text of the label where the link is created. </param>
			/// <param name="length">The number of characters after the starting character to include in the link text. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link that was created and added to the collection.</returns>
			// Token: 0x06005BFA RID: 23546 RVA: 0x00180177 File Offset: 0x0017E377
			public LinkLabel.Link Add(int start, int length)
			{
				if (length != 0)
				{
					this.linksAdded = true;
				}
				return this.Add(start, length, null);
			}

			/// <summary>Adds a link to the collection with information to associate with the link.</summary>
			/// <param name="start">The starting character within the text of the label where the link is created. </param>
			/// <param name="length">The number of characters after the starting character to include in the link text. </param>
			/// <param name="linkData">The object containing the information to associate with the link. </param>
			/// <returns>A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link that was created and added to the collection.</returns>
			// Token: 0x06005BFB RID: 23547 RVA: 0x0018018C File Offset: 0x0017E38C
			public LinkLabel.Link Add(int start, int length, object linkData)
			{
				if (length != 0)
				{
					this.linksAdded = true;
				}
				if (this.owner.links.Count == 1 && this[0].Start == 0 && this[0].length == -1)
				{
					this.owner.links.Clear();
					this.owner.FocusLink = null;
				}
				LinkLabel.Link link = new LinkLabel.Link(this.owner);
				link.Start = start;
				link.Length = length;
				link.LinkData = linkData;
				this.Add(link);
				return link;
			}

			/// <summary>Adds a link with the specified value to the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link to add.</param>
			/// <returns>The zero-based index where the link specified by the <paramref name="value" /> parameter is located in the collection.</returns>
			// Token: 0x06005BFC RID: 23548 RVA: 0x0018021C File Offset: 0x0017E41C
			public int Add(LinkLabel.Link value)
			{
				if (value != null && value.Length != 0)
				{
					this.linksAdded = true;
				}
				if (this.owner.links.Count == 1 && this[0].Start == 0 && this[0].length == -1)
				{
					this.owner.links.Clear();
					this.owner.FocusLink = null;
				}
				value.Owner = this.owner;
				this.owner.links.Add(value);
				if (this.owner.AutoSize)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				if (this.owner.Links.Count > 1)
				{
					this.owner.links.Sort(LinkLabel.linkComparer);
				}
				this.owner.ValidateNoOverlappingLinks();
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
				if (this.owner.Links.Count > 1)
				{
					return this.IndexOf(value);
				}
				return 0;
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Add(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
			/// <returns>The position into which the new element was inserted.</returns>
			// Token: 0x06005BFD RID: 23549 RVA: 0x00180353 File Offset: 0x0017E553
			int IList.Add(object value)
			{
				if (value is LinkLabel.Link)
				{
					return this.Add((LinkLabel.Link)value);
				}
				throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Insert(System.Int32,System.Object)" />.</summary>
			/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
			/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Collections.IList" />.</param>
			// Token: 0x06005BFE RID: 23550 RVA: 0x0018037E File Offset: 0x0017E57E
			void IList.Insert(int index, object value)
			{
				if (value is LinkLabel.Link)
				{
					this.Add((LinkLabel.Link)value);
					return;
				}
				throw new ArgumentException(SR.GetString("LinkLabelBadLink"), "value");
			}

			/// <summary>Determines whether the specified link is within the collection.</summary>
			/// <param name="link">A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link to search for in the collection. </param>
			/// <returns>
			///     <see langword="true" /> if the specified link is within the collection; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005BFF RID: 23551 RVA: 0x001803AA File Offset: 0x0017E5AA
			public bool Contains(LinkLabel.Link link)
			{
				return this.owner.links.Contains(link);
			}

			/// <summary>Returns a value indicating whether the collection contains a link with the specified key.</summary>
			/// <param name="key">The link to search for in the collection.</param>
			/// <returns>
			///     <see langword="true" /> if the collection contains an item with the specified key; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005C00 RID: 23552 RVA: 0x001803BD File Offset: 0x0017E5BD
			public virtual bool ContainsKey(string key)
			{
				return this.IsValidIndex(this.IndexOfKey(key));
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Contains(System.Object)" />.</summary>
			/// <param name="link">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
			/// <returns>
			///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
			// Token: 0x06005C01 RID: 23553 RVA: 0x001803CC File Offset: 0x0017E5CC
			bool IList.Contains(object link)
			{
				return link is LinkLabel.Link && this.Contains((LinkLabel.Link)link);
			}

			/// <summary>Returns the index of the specified link within the collection.</summary>
			/// <param name="link">A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> representing the link to search for in the collection. </param>
			/// <returns>The zero-based index where the link is located within the collection; otherwise, negative one (-1).</returns>
			// Token: 0x06005C02 RID: 23554 RVA: 0x001803E4 File Offset: 0x0017E5E4
			public int IndexOf(LinkLabel.Link link)
			{
				return this.owner.links.IndexOf(link);
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.IndexOf(System.Object)" />.</summary>
			/// <param name="link">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
			/// <returns>The index of the <paramref name="link" /> parameter, if found in the list; otherwise, -1.</returns>
			// Token: 0x06005C03 RID: 23555 RVA: 0x001803F7 File Offset: 0x0017E5F7
			int IList.IndexOf(object link)
			{
				if (link is LinkLabel.Link)
				{
					return this.IndexOf((LinkLabel.Link)link);
				}
				return -1;
			}

			/// <summary>Retrieves the zero-based index of the first occurrence of the specified key within the entire collection.</summary>
			/// <param name="key">The key to search the collection for.</param>
			/// <returns>The zero-based index of the first occurrence of value within the entire collection, if found; otherwise, -1.</returns>
			// Token: 0x06005C04 RID: 23556 RVA: 0x00180410 File Offset: 0x0017E610
			public virtual int IndexOfKey(string key)
			{
				if (string.IsNullOrEmpty(key))
				{
					return -1;
				}
				if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
				{
					return this.lastAccessedIndex;
				}
				for (int i = 0; i < this.Count; i++)
				{
					if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
					{
						this.lastAccessedIndex = i;
						return i;
					}
				}
				this.lastAccessedIndex = -1;
				return -1;
			}

			// Token: 0x06005C05 RID: 23557 RVA: 0x0018048D File Offset: 0x0017E68D
			private bool IsValidIndex(int index)
			{
				return index >= 0 && index < this.Count;
			}

			/// <summary>Clears all links from the collection.</summary>
			// Token: 0x06005C06 RID: 23558 RVA: 0x001804A0 File Offset: 0x0017E6A0
			public virtual void Clear()
			{
				bool flag = this.owner.links.Count > 0 && this.owner.AutoSize;
				this.owner.links.Clear();
				if (flag)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.ICollection.CopyTo(System.Array,System.Int32)" />.</summary>
			/// <param name="dest">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
			// Token: 0x06005C07 RID: 23559 RVA: 0x00180534 File Offset: 0x0017E734
			void ICollection.CopyTo(Array dest, int index)
			{
				this.owner.links.CopyTo(dest, index);
			}

			/// <summary>Returns an enumerator to use to iterate through the link collection.</summary>
			/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that represents the link collection.</returns>
			// Token: 0x06005C08 RID: 23560 RVA: 0x00180548 File Offset: 0x0017E748
			public IEnumerator GetEnumerator()
			{
				if (this.owner.links != null)
				{
					return this.owner.links.GetEnumerator();
				}
				return new LinkLabel.Link[0].GetEnumerator();
			}

			/// <summary>Removes the specified link from the collection.</summary>
			/// <param name="value">A <see cref="T:System.Windows.Forms.LinkLabel.Link" /> that represents the link to remove from the collection. </param>
			// Token: 0x06005C09 RID: 23561 RVA: 0x00180574 File Offset: 0x0017E774
			public void Remove(LinkLabel.Link value)
			{
				if (value.Owner != this.owner)
				{
					return;
				}
				this.owner.links.Remove(value);
				if (this.owner.AutoSize)
				{
					LayoutTransaction.DoLayout(this.owner.ParentInternal, this.owner, PropertyNames.Links);
					this.owner.AdjustSize();
					this.owner.Invalidate();
				}
				this.owner.links.Sort(LinkLabel.linkComparer);
				this.owner.ValidateNoOverlappingLinks();
				this.owner.UpdateSelectability();
				this.owner.InvalidateTextLayout();
				this.owner.Invalidate();
				if (this.owner.FocusLink == null && this.owner.links.Count > 0)
				{
					this.owner.FocusLink = (LinkLabel.Link)this.owner.links[0];
				}
			}

			/// <summary>Removes a link at a specified location within the collection.</summary>
			/// <param name="index">The zero-based index of the item to remove from the collection. </param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The value of <paramref name="index" /> is a negative value or greater than the number of items in the collection. </exception>
			// Token: 0x06005C0A RID: 23562 RVA: 0x00180661 File Offset: 0x0017E861
			public void RemoveAt(int index)
			{
				this.Remove(this[index]);
			}

			/// <summary>Removes the link with the specified key. </summary>
			/// <param name="key">The key of the link to remove.</param>
			// Token: 0x06005C0B RID: 23563 RVA: 0x00180670 File Offset: 0x0017E870
			public virtual void RemoveByKey(string key)
			{
				int index = this.IndexOfKey(key);
				if (this.IsValidIndex(index))
				{
					this.RemoveAt(index);
				}
			}

			/// <summary>For a description of this member, see <see cref="M:System.Collections.IList.Remove(System.Object)" />.</summary>
			/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Collections.IList" />.</param>
			// Token: 0x06005C0C RID: 23564 RVA: 0x00180695 File Offset: 0x0017E895
			void IList.Remove(object value)
			{
				if (value is LinkLabel.Link)
				{
					this.Remove((LinkLabel.Link)value);
				}
			}

			// Token: 0x040039E1 RID: 14817
			private LinkLabel owner;

			// Token: 0x040039E2 RID: 14818
			private bool linksAdded;

			// Token: 0x040039E3 RID: 14819
			private int lastAccessedIndex = -1;
		}

		/// <summary>Represents a link within a <see cref="T:System.Windows.Forms.LinkLabel" /> control.</summary>
		// Token: 0x02000600 RID: 1536
		[TypeConverter(typeof(LinkConverter))]
		public class Link
		{
			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabel.Link" /> class. </summary>
			// Token: 0x06005C0D RID: 23565 RVA: 0x001806AB File Offset: 0x0017E8AB
			public Link()
			{
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabel.Link" /> class with the specified starting location and number of characters after the starting location within the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
			/// <param name="start">The zero-based starting location of the link area within the text of the <see cref="T:System.Windows.Forms.LinkLabel" />.</param>
			/// <param name="length">The number of characters, after the starting character, to include in the link area.</param>
			// Token: 0x06005C0E RID: 23566 RVA: 0x001806BA File Offset: 0x0017E8BA
			public Link(int start, int length)
			{
				this.start = start;
				this.length = length;
			}

			/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.LinkLabel.Link" /> class with the specified starting location, number of characters after the starting location within the <see cref="T:System.Windows.Forms.LinkLabel" />, and the data associated with the link.</summary>
			/// <param name="start">The zero-based starting location of the link area within the text of the <see cref="T:System.Windows.Forms.LinkLabel" />.</param>
			/// <param name="length">The number of characters, after the starting character, to include in the link area.</param>
			/// <param name="linkData">The data associated with the link.</param>
			// Token: 0x06005C0F RID: 23567 RVA: 0x001806D7 File Offset: 0x0017E8D7
			public Link(int start, int length, object linkData)
			{
				this.start = start;
				this.length = length;
				this.linkData = linkData;
			}

			// Token: 0x06005C10 RID: 23568 RVA: 0x001806FB File Offset: 0x0017E8FB
			internal Link(LinkLabel owner)
			{
				this.owner = owner;
			}

			/// <summary>Gets or sets a text description of the link.</summary>
			/// <returns>A <see cref="T:System.String" /> representing a text description of the link.</returns>
			// Token: 0x17001609 RID: 5641
			// (get) Token: 0x06005C11 RID: 23569 RVA: 0x00180711 File Offset: 0x0017E911
			// (set) Token: 0x06005C12 RID: 23570 RVA: 0x00180719 File Offset: 0x0017E919
			public string Description
			{
				get
				{
					return this.description;
				}
				set
				{
					this.description = value;
				}
			}

			/// <summary>Gets or sets a value indicating whether the link is enabled.</summary>
			/// <returns>
			///     <see langword="true" /> if the link is enabled; otherwise, <see langword="false" />.</returns>
			// Token: 0x1700160A RID: 5642
			// (get) Token: 0x06005C13 RID: 23571 RVA: 0x00180722 File Offset: 0x0017E922
			// (set) Token: 0x06005C14 RID: 23572 RVA: 0x0018072C File Offset: 0x0017E92C
			[DefaultValue(true)]
			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
				set
				{
					if (this.enabled != value)
					{
						this.enabled = value;
						if ((this.state & (LinkState)3) != LinkState.Normal)
						{
							this.state &= (LinkState)(-4);
							if (this.owner != null)
							{
								this.owner.OverrideCursor = null;
							}
						}
						if (this.owner != null)
						{
							this.owner.InvalidateLink(this);
						}
					}
				}
			}

			/// <summary>Gets or sets the number of characters in the link text.</summary>
			/// <returns>The number of characters, including spaces, in the link text.</returns>
			// Token: 0x1700160B RID: 5643
			// (get) Token: 0x06005C15 RID: 23573 RVA: 0x0018078C File Offset: 0x0017E98C
			// (set) Token: 0x06005C16 RID: 23574 RVA: 0x001807E3 File Offset: 0x0017E9E3
			public int Length
			{
				get
				{
					if (this.length != -1)
					{
						return this.length;
					}
					if (this.owner != null && !string.IsNullOrEmpty(this.owner.Text))
					{
						StringInfo stringInfo = new StringInfo(this.owner.Text);
						return stringInfo.LengthInTextElements - this.Start;
					}
					return 0;
				}
				set
				{
					if (this.length != value)
					{
						this.length = value;
						if (this.owner != null)
						{
							this.owner.InvalidateTextLayout();
							this.owner.Invalidate();
						}
					}
				}
			}

			/// <summary>Gets or sets the data associated with the link.</summary>
			/// <returns>An <see cref="T:System.Object" /> representing the data associated with the link.</returns>
			// Token: 0x1700160C RID: 5644
			// (get) Token: 0x06005C17 RID: 23575 RVA: 0x00180813 File Offset: 0x0017EA13
			// (set) Token: 0x06005C18 RID: 23576 RVA: 0x0018081B File Offset: 0x0017EA1B
			[DefaultValue(null)]
			public object LinkData
			{
				get
				{
					return this.linkData;
				}
				set
				{
					this.linkData = value;
				}
			}

			// Token: 0x1700160D RID: 5645
			// (get) Token: 0x06005C19 RID: 23577 RVA: 0x00180824 File Offset: 0x0017EA24
			// (set) Token: 0x06005C1A RID: 23578 RVA: 0x0018082C File Offset: 0x0017EA2C
			internal LinkLabel Owner
			{
				get
				{
					return this.owner;
				}
				set
				{
					this.owner = value;
				}
			}

			// Token: 0x1700160E RID: 5646
			// (get) Token: 0x06005C1B RID: 23579 RVA: 0x00180835 File Offset: 0x0017EA35
			// (set) Token: 0x06005C1C RID: 23580 RVA: 0x0018083D File Offset: 0x0017EA3D
			internal LinkState State
			{
				get
				{
					return this.state;
				}
				set
				{
					this.state = value;
				}
			}

			/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Forms.LinkLabel.Link" />.</summary>
			/// <returns>A <see cref="T:System.String" /> representing the name of the <see cref="T:System.Windows.Forms.LinkLabel.Link" />. The default value is the empty string ("").</returns>
			// Token: 0x1700160F RID: 5647
			// (get) Token: 0x06005C1D RID: 23581 RVA: 0x00180846 File Offset: 0x0017EA46
			// (set) Token: 0x06005C1E RID: 23582 RVA: 0x0018085C File Offset: 0x0017EA5C
			[DefaultValue("")]
			[SRCategory("CatAppearance")]
			[SRDescription("TreeNodeNodeNameDescr")]
			public string Name
			{
				get
				{
					if (this.name != null)
					{
						return this.name;
					}
					return "";
				}
				set
				{
					this.name = value;
				}
			}

			/// <summary>Gets or sets the starting location of the link within the text of the <see cref="T:System.Windows.Forms.LinkLabel" />.</summary>
			/// <returns>The location within the text of the <see cref="T:System.Windows.Forms.LinkLabel" /> control where the link starts.</returns>
			// Token: 0x17001610 RID: 5648
			// (get) Token: 0x06005C1F RID: 23583 RVA: 0x00180865 File Offset: 0x0017EA65
			// (set) Token: 0x06005C20 RID: 23584 RVA: 0x00180870 File Offset: 0x0017EA70
			public int Start
			{
				get
				{
					return this.start;
				}
				set
				{
					if (this.start != value)
					{
						this.start = value;
						if (this.owner != null)
						{
							this.owner.links.Sort(LinkLabel.linkComparer);
							this.owner.InvalidateTextLayout();
							this.owner.Invalidate();
						}
					}
				}
			}

			/// <summary>Gets or sets the object that contains data about the <see cref="T:System.Windows.Forms.LinkLabel.Link" />.</summary>
			/// <returns>An <see cref="T:System.Object" /> that contains data about the control. The default is <see langword="null" />.</returns>
			// Token: 0x17001611 RID: 5649
			// (get) Token: 0x06005C21 RID: 23585 RVA: 0x001808C0 File Offset: 0x0017EAC0
			// (set) Token: 0x06005C22 RID: 23586 RVA: 0x001808C8 File Offset: 0x0017EAC8
			[SRCategory("CatData")]
			[Localizable(false)]
			[Bindable(true)]
			[SRDescription("ControlTagDescr")]
			[DefaultValue(null)]
			[TypeConverter(typeof(StringConverter))]
			public object Tag
			{
				get
				{
					return this.userData;
				}
				set
				{
					this.userData = value;
				}
			}

			/// <summary>Gets or sets a value indicating whether the user has visited the link.</summary>
			/// <returns>
			///     <see langword="true" /> if the link has been visited; otherwise, <see langword="false" />.</returns>
			// Token: 0x17001612 RID: 5650
			// (get) Token: 0x06005C23 RID: 23587 RVA: 0x001808D1 File Offset: 0x0017EAD1
			// (set) Token: 0x06005C24 RID: 23588 RVA: 0x001808E0 File Offset: 0x0017EAE0
			[DefaultValue(false)]
			public bool Visited
			{
				get
				{
					return (this.State & LinkState.Visited) == LinkState.Visited;
				}
				set
				{
					bool visited = this.Visited;
					if (value)
					{
						this.State |= LinkState.Visited;
					}
					else
					{
						this.State &= (LinkState)(-5);
					}
					if (visited != this.Visited && this.owner != null)
					{
						this.owner.InvalidateLink(this);
					}
				}
			}

			// Token: 0x17001613 RID: 5651
			// (get) Token: 0x06005C25 RID: 23589 RVA: 0x00180933 File Offset: 0x0017EB33
			// (set) Token: 0x06005C26 RID: 23590 RVA: 0x0018093B File Offset: 0x0017EB3B
			internal Region VisualRegion
			{
				get
				{
					return this.visualRegion;
				}
				set
				{
					this.visualRegion = value;
				}
			}

			// Token: 0x040039E4 RID: 14820
			private int start;

			// Token: 0x040039E5 RID: 14821
			private object linkData;

			// Token: 0x040039E6 RID: 14822
			private LinkState state;

			// Token: 0x040039E7 RID: 14823
			private bool enabled = true;

			// Token: 0x040039E8 RID: 14824
			private Region visualRegion;

			// Token: 0x040039E9 RID: 14825
			internal int length;

			// Token: 0x040039EA RID: 14826
			private LinkLabel owner;

			// Token: 0x040039EB RID: 14827
			private string name;

			// Token: 0x040039EC RID: 14828
			private string description;

			// Token: 0x040039ED RID: 14829
			private object userData;
		}

		// Token: 0x02000601 RID: 1537
		private class LinkComparer : IComparer
		{
			// Token: 0x06005C27 RID: 23591 RVA: 0x00180944 File Offset: 0x0017EB44
			int IComparer.Compare(object link1, object link2)
			{
				int start = ((LinkLabel.Link)link1).Start;
				int start2 = ((LinkLabel.Link)link2).Start;
				return start - start2;
			}
		}

		// Token: 0x02000602 RID: 1538
		[ComVisible(true)]
		internal class LinkLabelAccessibleObject : Label.LabelAccessibleObject
		{
			// Token: 0x06005C29 RID: 23593 RVA: 0x0018096C File Offset: 0x0017EB6C
			public LinkLabelAccessibleObject(LinkLabel owner) : base(owner)
			{
			}

			// Token: 0x06005C2A RID: 23594 RVA: 0x00180975 File Offset: 0x0017EB75
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005C2B RID: 23595 RVA: 0x00180986 File Offset: 0x0017EB86
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < ((LinkLabel)base.Owner).Links.Count)
				{
					return new LinkLabel.LinkAccessibleObject(((LinkLabel)base.Owner).Links[index]);
				}
				return null;
			}

			// Token: 0x06005C2C RID: 23596 RVA: 0x001809C1 File Offset: 0x0017EBC1
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30010 && !base.Owner.Enabled)
				{
					return false;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x06005C2D RID: 23597 RVA: 0x001809E8 File Offset: 0x0017EBE8
			public override AccessibleObject HitTest(int x, int y)
			{
				Point point = base.Owner.PointToClient(new Point(x, y));
				LinkLabel.Link link = ((LinkLabel)base.Owner).PointInLink(point.X, point.Y);
				if (link != null)
				{
					return new LinkLabel.LinkAccessibleObject(link);
				}
				if (this.Bounds.Contains(x, y))
				{
					return this;
				}
				return null;
			}

			// Token: 0x06005C2E RID: 23598 RVA: 0x00180A46 File Offset: 0x0017EC46
			public override int GetChildCount()
			{
				return ((LinkLabel)base.Owner).Links.Count;
			}
		}

		// Token: 0x02000603 RID: 1539
		[ComVisible(true)]
		internal class LinkAccessibleObject : AccessibleObject
		{
			// Token: 0x06005C2F RID: 23599 RVA: 0x00180A5D File Offset: 0x0017EC5D
			public LinkAccessibleObject(LinkLabel.Link link)
			{
				this.link = link;
			}

			// Token: 0x17001614 RID: 5652
			// (get) Token: 0x06005C30 RID: 23600 RVA: 0x00180A6C File Offset: 0x0017EC6C
			public override Rectangle Bounds
			{
				get
				{
					Region visualRegion = this.link.VisualRegion;
					Graphics graphics = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						graphics = Graphics.FromHwnd(this.link.Owner.Handle);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (visualRegion == null)
					{
						this.link.Owner.EnsureRun(graphics);
						visualRegion = this.link.VisualRegion;
						if (visualRegion == null)
						{
							graphics.Dispose();
							return Rectangle.Empty;
						}
					}
					Rectangle r;
					try
					{
						r = Rectangle.Ceiling(visualRegion.GetBounds(graphics));
					}
					finally
					{
						graphics.Dispose();
					}
					return this.link.Owner.RectangleToScreen(r);
				}
			}

			// Token: 0x17001615 RID: 5653
			// (get) Token: 0x06005C31 RID: 23601 RVA: 0x00180B24 File Offset: 0x0017ED24
			public override string DefaultAction
			{
				get
				{
					return SR.GetString("AccessibleActionClick");
				}
			}

			// Token: 0x17001616 RID: 5654
			// (get) Token: 0x06005C32 RID: 23602 RVA: 0x00180B30 File Offset: 0x0017ED30
			public override string Description
			{
				get
				{
					return this.link.Description;
				}
			}

			// Token: 0x17001617 RID: 5655
			// (get) Token: 0x06005C33 RID: 23603 RVA: 0x00180B40 File Offset: 0x0017ED40
			// (set) Token: 0x06005C34 RID: 23604 RVA: 0x0016B02C File Offset: 0x0016922C
			public override string Name
			{
				get
				{
					string text = this.link.Owner.Text;
					string text2;
					if (AccessibilityImprovements.Level3)
					{
						text2 = text;
						if (this.link.Owner.UseMnemonic)
						{
							text2 = WindowsFormsUtils.TextWithoutMnemonics(text2);
						}
					}
					else
					{
						int num = LinkLabel.ConvertToCharIndex(this.link.Start, text);
						int num2 = LinkLabel.ConvertToCharIndex(this.link.Start + this.link.Length, text);
						text2 = text.Substring(num, num2 - num);
						if (AccessibilityImprovements.Level1 && this.link.Owner.UseMnemonic)
						{
							text2 = WindowsFormsUtils.TextWithoutMnemonics(text2);
						}
					}
					return text2;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x17001618 RID: 5656
			// (get) Token: 0x06005C35 RID: 23605 RVA: 0x00180BDE File Offset: 0x0017EDDE
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.link.Owner.AccessibilityObject;
				}
			}

			// Token: 0x17001619 RID: 5657
			// (get) Token: 0x06005C36 RID: 23606 RVA: 0x00172BB7 File Offset: 0x00170DB7
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.Link;
				}
			}

			// Token: 0x1700161A RID: 5658
			// (get) Token: 0x06005C37 RID: 23607 RVA: 0x00180BF0 File Offset: 0x0017EDF0
			public override AccessibleStates State
			{
				get
				{
					AccessibleStates accessibleStates = AccessibleStates.Focusable;
					if (this.link.Owner.FocusLink == this.link)
					{
						accessibleStates |= AccessibleStates.Focused;
					}
					return accessibleStates;
				}
			}

			// Token: 0x1700161B RID: 5659
			// (get) Token: 0x06005C38 RID: 23608 RVA: 0x00180C20 File Offset: 0x0017EE20
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					if (AccessibilityImprovements.Level1)
					{
						return string.Empty;
					}
					return this.Name;
				}
			}

			// Token: 0x06005C39 RID: 23609 RVA: 0x00180C35 File Offset: 0x0017EE35
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override void DoDefaultAction()
			{
				this.link.Owner.OnLinkClicked(new LinkLabelLinkClickedEventArgs(this.link));
			}

			// Token: 0x06005C3A RID: 23610 RVA: 0x00180C52 File Offset: 0x0017EE52
			internal override bool IsIAccessibleExSupported()
			{
				return AccessibilityImprovements.Level3 || base.IsIAccessibleExSupported();
			}

			// Token: 0x06005C3B RID: 23611 RVA: 0x00180C63 File Offset: 0x0017EE63
			internal override object GetPropertyValue(int propertyID)
			{
				if (propertyID == 30010 && !this.link.Owner.Enabled)
				{
					return false;
				}
				return base.GetPropertyValue(propertyID);
			}

			// Token: 0x040039EE RID: 14830
			private LinkLabel.Link link;
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Provides properties that specify the appearance of <see cref="T:System.Windows.Forms.Button" /> controls whose <see cref="T:System.Windows.Forms.FlatStyle" /> is <see cref="F:System.Windows.Forms.FlatStyle.Flat" />.</summary>
	// Token: 0x02000243 RID: 579
	[TypeConverter(typeof(FlatButtonAppearanceConverter))]
	public class FlatButtonAppearance
	{
		// Token: 0x06002257 RID: 8791 RVA: 0x000A7490 File Offset: 0x000A5690
		internal FlatButtonAppearance(ButtonBase owner)
		{
			this.owner = owner;
		}

		/// <summary>Gets or sets a value that specifies the size, in pixels, of the border around the button.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the size, in pixels, of the border around the button.</returns>
		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x000A74DD File Offset: 0x000A56DD
		// (set) Token: 0x06002259 RID: 8793 RVA: 0x000A74E8 File Offset: 0x000A56E8
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonBorderSizeDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(1)]
		public int BorderSize
		{
			get
			{
				return this.borderSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BorderSize", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"BorderSize",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.borderSize != value)
				{
					this.borderSize = value;
					if (this.owner != null && this.owner.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.owner.AutoSize, this.owner.ParentInternal, this.owner, PropertyNames.FlatAppearanceBorderSize);
					}
					this.owner.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color of the border around the button.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the color of the border around the button.</returns>
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x000A7598 File Offset: 0x000A5798
		// (set) Token: 0x0600225B RID: 8795 RVA: 0x000A75A0 File Offset: 0x000A57A0
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonBorderColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new NotSupportedException(SR.GetString("ButtonFlatAppearanceInvalidBorderColor"));
				}
				if (this.borderColor != value)
				{
					this.borderColor = value;
					this.owner.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color of the client area of the button when the button is checked and the mouse pointer is outside the bounds of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the color of the client area of the button.</returns>
		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x000A75F6 File Offset: 0x000A57F6
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x000A75FE File Offset: 0x000A57FE
		[Browsable(true)]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonCheckedBackColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		public Color CheckedBackColor
		{
			get
			{
				return this.checkedBackColor;
			}
			set
			{
				if (this.checkedBackColor != value)
				{
					this.checkedBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color of the client area of the button when the mouse is pressed within the bounds of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the color of the client area of the button.</returns>
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x000A7620 File Offset: 0x000A5820
		// (set) Token: 0x0600225F RID: 8799 RVA: 0x000A7628 File Offset: 0x000A5828
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonMouseDownBackColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		public Color MouseDownBackColor
		{
			get
			{
				return this.mouseDownBackColor;
			}
			set
			{
				if (this.mouseDownBackColor != value)
				{
					this.mouseDownBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		/// <summary>Gets or sets the color of the client area of the button when the mouse pointer is within the bounds of the control.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> structure representing the color of the client area of the button.</returns>
		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002260 RID: 8800 RVA: 0x000A764A File Offset: 0x000A584A
		// (set) Token: 0x06002261 RID: 8801 RVA: 0x000A7652 File Offset: 0x000A5852
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonMouseOverBackColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		public Color MouseOverBackColor
		{
			get
			{
				return this.mouseOverBackColor;
			}
			set
			{
				if (this.mouseOverBackColor != value)
				{
					this.mouseOverBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x04000EED RID: 3821
		private ButtonBase owner;

		// Token: 0x04000EEE RID: 3822
		private int borderSize = 1;

		// Token: 0x04000EEF RID: 3823
		private Color borderColor = Color.Empty;

		// Token: 0x04000EF0 RID: 3824
		private Color checkedBackColor = Color.Empty;

		// Token: 0x04000EF1 RID: 3825
		private Color mouseDownBackColor = Color.Empty;

		// Token: 0x04000EF2 RID: 3826
		private Color mouseOverBackColor = Color.Empty;
	}
}

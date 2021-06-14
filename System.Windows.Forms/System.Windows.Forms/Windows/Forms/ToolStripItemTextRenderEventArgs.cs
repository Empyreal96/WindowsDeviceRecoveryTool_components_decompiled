using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolStripRenderer.RenderItemText" /> event.</summary>
	// Token: 0x020003CD RID: 973
	public class ToolStripItemTextRenderEventArgs : ToolStripItemRenderEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemTextRenderEventArgs" /> class with the specified text and text properties format.</summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text.</param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to draw the text.</param>
		/// <param name="text">The text to be drawn.</param>
		/// <param name="textRectangle">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds to draw the text in.</param>
		/// <param name="textColor">The <see cref="T:System.Drawing.Color" /> used to draw the text.</param>
		/// <param name="textFont">The <see cref="T:System.Drawing.Font" /> used to draw the text.</param>
		/// <param name="format">The display and layout information for text strings.</param>
		// Token: 0x06004074 RID: 16500 RVA: 0x001164C8 File Offset: 0x001146C8
		public ToolStripItemTextRenderEventArgs(Graphics g, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, TextFormatFlags format) : base(g, item)
		{
			this.text = text;
			this.textRectangle = textRectangle;
			this.defaultTextColor = textColor;
			this.textFont = textFont;
			this.textAlignment = item.TextAlign;
			this.textFormat = format;
			this.textDirection = item.TextDirection;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolStripItemTextRenderEventArgs" /> class with the specified text and text properties. </summary>
		/// <param name="g">The <see cref="T:System.Drawing.Graphics" /> used to draw the text.</param>
		/// <param name="item">The <see cref="T:System.Windows.Forms.ToolStripItem" /> on which to draw the text.</param>
		/// <param name="text">The text to be drawn.</param>
		/// <param name="textRectangle">The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds to draw the text in.</param>
		/// <param name="textColor">The <see cref="T:System.Drawing.Color" /> used to draw the text.</param>
		/// <param name="textFont">The <see cref="T:System.Drawing.Font" /> used to draw the text.</param>
		/// <param name="textAlign">The <see cref="T:System.Drawing.ContentAlignment" /> that specifies the vertical and horizontal alignment of the text in the bounding area.</param>
		// Token: 0x06004075 RID: 16501 RVA: 0x00116544 File Offset: 0x00114744
		public ToolStripItemTextRenderEventArgs(Graphics g, ToolStripItem item, string text, Rectangle textRectangle, Color textColor, Font textFont, ContentAlignment textAlign) : base(g, item)
		{
			this.text = text;
			this.textRectangle = textRectangle;
			this.defaultTextColor = textColor;
			this.textFont = textFont;
			this.textFormat = ToolStripItemInternalLayout.ContentAlignToTextFormat(textAlign, item.RightToLeft == RightToLeft.Yes);
			this.textFormat = (item.ShowKeyboardCues ? this.textFormat : (this.textFormat | TextFormatFlags.HidePrefix));
			this.textDirection = item.TextDirection;
		}

		/// <summary>Gets or sets the text to be drawn on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A string that represents the text to be painted on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06004076 RID: 16502 RVA: 0x001165E4 File Offset: 0x001147E4
		// (set) Token: 0x06004077 RID: 16503 RVA: 0x001165EC File Offset: 0x001147EC
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		/// <summary>Gets or sets the color of the <see cref="T:System.Windows.Forms.ToolStripItem" /> text. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that represents the color of the <see cref="T:System.Windows.Forms.ToolStripItem" /> text.</returns>
		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06004078 RID: 16504 RVA: 0x001165F5 File Offset: 0x001147F5
		// (set) Token: 0x06004079 RID: 16505 RVA: 0x0011660C File Offset: 0x0011480C
		public Color TextColor
		{
			get
			{
				if (this.textColorChanged)
				{
					return this.textColor;
				}
				return this.DefaultTextColor;
			}
			set
			{
				this.textColor = value;
				this.textColorChanged = true;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x0600407A RID: 16506 RVA: 0x0011661C File Offset: 0x0011481C
		// (set) Token: 0x0600407B RID: 16507 RVA: 0x00116624 File Offset: 0x00114824
		internal Color DefaultTextColor
		{
			get
			{
				return this.defaultTextColor;
			}
			set
			{
				this.defaultTextColor = value;
			}
		}

		/// <summary>Gets or sets the font of the text drawn on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> of the text drawn on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</returns>
		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x0011662D File Offset: 0x0011482D
		// (set) Token: 0x0600407D RID: 16509 RVA: 0x00116635 File Offset: 0x00114835
		public Font TextFont
		{
			get
			{
				return this.textFont;
			}
			set
			{
				this.textFont = value;
			}
		}

		/// <summary>Gets or sets the rectangle that represents the bounds to draw the text in.</summary>
		/// <returns>The <see cref="T:System.Drawing.Rectangle" /> that represents the bounds to draw the text in.</returns>
		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x0600407E RID: 16510 RVA: 0x0011663E File Offset: 0x0011483E
		// (set) Token: 0x0600407F RID: 16511 RVA: 0x00116646 File Offset: 0x00114846
		public Rectangle TextRectangle
		{
			get
			{
				return this.textRectangle;
			}
			set
			{
				this.textRectangle = value;
			}
		}

		/// <summary>Gets or sets the display and layout information of the text drawn on the <see cref="T:System.Windows.Forms.ToolStripItem" />.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.TextFormatFlags" /> values that specify the display and layout information of the drawn text. </returns>
		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06004080 RID: 16512 RVA: 0x0011664F File Offset: 0x0011484F
		// (set) Token: 0x06004081 RID: 16513 RVA: 0x00116657 File Offset: 0x00114857
		public TextFormatFlags TextFormat
		{
			get
			{
				return this.textFormat;
			}
			set
			{
				this.textFormat = value;
			}
		}

		/// <summary>Gets or sets whether the text is drawn vertically or horizontally.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ToolStripTextDirection" /> values. </returns>
		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x00116660 File Offset: 0x00114860
		// (set) Token: 0x06004083 RID: 16515 RVA: 0x00116668 File Offset: 0x00114868
		public ToolStripTextDirection TextDirection
		{
			get
			{
				return this.textDirection;
			}
			set
			{
				this.textDirection = value;
			}
		}

		// Token: 0x040024C2 RID: 9410
		private string text;

		// Token: 0x040024C3 RID: 9411
		private Rectangle textRectangle = Rectangle.Empty;

		// Token: 0x040024C4 RID: 9412
		private Color textColor = SystemColors.ControlText;

		// Token: 0x040024C5 RID: 9413
		private Font textFont;

		// Token: 0x040024C6 RID: 9414
		private ContentAlignment textAlignment;

		// Token: 0x040024C7 RID: 9415
		private ToolStripTextDirection textDirection = ToolStripTextDirection.Horizontal;

		// Token: 0x040024C8 RID: 9416
		private TextFormatFlags textFormat;

		// Token: 0x040024C9 RID: 9417
		private Color defaultTextColor = SystemColors.ControlText;

		// Token: 0x040024CA RID: 9418
		private bool textColorChanged;
	}
}

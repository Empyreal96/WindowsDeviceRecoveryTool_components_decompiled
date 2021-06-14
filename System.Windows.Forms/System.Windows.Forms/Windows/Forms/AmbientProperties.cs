using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides ambient property values to top-level controls.</summary>
	// Token: 0x0200010E RID: 270
	public sealed class AmbientProperties
	{
		/// <summary>Gets or sets the ambient background color of an object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value that represents the background color of an object.</returns>
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001012F File Offset: 0x0000E32F
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00010137 File Offset: 0x0000E337
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
			}
		}

		/// <summary>Gets or sets the ambient cursor of an object.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor of an object.</returns>
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00010140 File Offset: 0x0000E340
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00010148 File Offset: 0x0000E348
		public Cursor Cursor
		{
			get
			{
				return this.cursor;
			}
			set
			{
				this.cursor = value;
			}
		}

		/// <summary>Gets or sets the ambient font of an object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Font" /> that represents the font used when displaying text within an object.</returns>
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00010151 File Offset: 0x0000E351
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00010159 File Offset: 0x0000E359
		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}

		/// <summary>Gets or sets the ambient foreground color of an object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value that represents the foreground color of an object.</returns>
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00010162 File Offset: 0x0000E362
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0001016A File Offset: 0x0000E36A
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				this.foreColor = value;
			}
		}

		// Token: 0x04000511 RID: 1297
		private Color backColor;

		// Token: 0x04000512 RID: 1298
		private Color foreColor;

		// Token: 0x04000513 RID: 1299
		private Cursor cursor;

		// Token: 0x04000514 RID: 1300
		private Font font;
	}
}

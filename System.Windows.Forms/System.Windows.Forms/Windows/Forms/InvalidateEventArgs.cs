using System;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.Invalidated" /> event.</summary>
	// Token: 0x02000290 RID: 656
	public class InvalidateEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.InvalidateEventArgs" /> class.</summary>
		/// <param name="invalidRect">The <see cref="T:System.Drawing.Rectangle" /> that contains the invalidated window area. </param>
		// Token: 0x060026D4 RID: 9940 RVA: 0x000B7687 File Offset: 0x000B5887
		public InvalidateEventArgs(Rectangle invalidRect)
		{
			this.invalidRect = invalidRect;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.Rectangle" /> that contains the invalidated window area.</summary>
		/// <returns>The invalidated window area.</returns>
		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000B7696 File Offset: 0x000B5896
		public Rectangle InvalidRect
		{
			get
			{
				return this.invalidRect;
			}
		}

		// Token: 0x04001067 RID: 4199
		private readonly Rectangle invalidRect;
	}
}

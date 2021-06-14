using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the DPIChanged events of a form or control.</summary>
	// Token: 0x02000223 RID: 547
	public sealed class DpiChangedEventArgs : CancelEventArgs
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x000A45A0 File Offset: 0x000A27A0
		internal DpiChangedEventArgs(int old, Message m)
		{
			this.DeviceDpiOld = old;
			this.DeviceDpiNew = NativeMethods.Util.SignedLOWORD(m.WParam);
			NativeMethods.RECT rect = (NativeMethods.RECT)UnsafeNativeMethods.PtrToStructure(m.LParam, typeof(NativeMethods.RECT));
			this.SuggestedRectangle = Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
		}

		/// <summary>Gets the DPI value for the display device where the control or form was previously displayed.</summary>
		/// <returns>A DPI value.</returns>
		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x000A460B File Offset: 0x000A280B
		// (set) Token: 0x06002142 RID: 8514 RVA: 0x000A4613 File Offset: 0x000A2813
		public int DeviceDpiOld { get; private set; }

		/// <summary>Gets the DPI value for the new display device where the control or form is currently being displayed.</summary>
		/// <returns>The DPI value.</returns>
		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000A461C File Offset: 0x000A281C
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x000A4624 File Offset: 0x000A2824
		public int DeviceDpiNew { get; private set; }

		/// <summary>Gets a <see cref="T:System.Drawing.Rectangle" /> that represents the new bounding rectangle for the control or form based on the DPI of the display device where it's displayed.</summary>
		/// <returns>A <see cref="T:System.Drawing.Rectangle" />.</returns>
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000A462D File Offset: 0x000A282D
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x000A4635 File Offset: 0x000A2835
		public Rectangle SuggestedRectangle { get; private set; }

		/// <summary>Creates and returns a string representation of the current <see cref="T:System.Windows.Forms.DpiChangedEventArgs" />.</summary>
		/// <returns>A string.</returns>
		// Token: 0x06002147 RID: 8519 RVA: 0x000A463E File Offset: 0x000A283E
		public override string ToString()
		{
			return string.Format("was: {0}, now: {1}", this.DeviceDpiOld, this.DeviceDpiNew);
		}
	}
}

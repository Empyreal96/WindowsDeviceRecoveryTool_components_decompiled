using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.DataGrid.Navigate" /> event.</summary>
	// Token: 0x020002F7 RID: 759
	[ComVisible(true)]
	public class NavigateEventArgs : EventArgs
	{
		/// <summary>Gets a value indicating whether to navigate in a forward direction.</summary>
		/// <returns>
		///     <see langword="true" /> if the navigation is in a forward direction; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x000D6F98 File Offset: 0x000D5198
		public bool Forward
		{
			get
			{
				return this.isForward;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.NavigateEventArgs" /> class.</summary>
		/// <param name="isForward">
		///       <see langword="true" /> to navigate in a forward direction; otherwise, <see langword="false" />. </param>
		// Token: 0x06002E04 RID: 11780 RVA: 0x000D6FA0 File Offset: 0x000D51A0
		public NavigateEventArgs(bool isForward)
		{
			this.isForward = isForward;
		}

		// Token: 0x04001D06 RID: 7430
		private bool isForward = true;
	}
}

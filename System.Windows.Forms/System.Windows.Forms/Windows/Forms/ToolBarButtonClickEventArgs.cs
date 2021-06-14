using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolBar.ButtonClick" /> event.</summary>
	// Token: 0x0200039B RID: 923
	public class ToolBarButtonClickEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.ToolBarButtonClickEventArgs" /> class.</summary>
		/// <param name="button">The <see cref="T:System.Windows.Forms.ToolBarButton" /> that was clicked. </param>
		// Token: 0x06003AB5 RID: 15029 RVA: 0x00104BC3 File Offset: 0x00102DC3
		public ToolBarButtonClickEventArgs(ToolBarButton button)
		{
			this.button = button;
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Forms.ToolBarButton" /> that was clicked.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.ToolBarButton" /> that was clicked.</returns>
		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06003AB6 RID: 15030 RVA: 0x00104BD2 File Offset: 0x00102DD2
		// (set) Token: 0x06003AB7 RID: 15031 RVA: 0x00104BDA File Offset: 0x00102DDA
		public ToolBarButton Button
		{
			get
			{
				return this.button;
			}
			set
			{
				this.button = value;
			}
		}

		// Token: 0x04002321 RID: 8993
		private ToolBarButton button;
	}
}

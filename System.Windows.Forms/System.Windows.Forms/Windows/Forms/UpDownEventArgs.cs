using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for controls that derive from the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</summary>
	// Token: 0x0200041C RID: 1052
	public class UpDownEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.UpDownEventArgs" /> class</summary>
		/// <param name="buttonPushed">The button that was clicked on the <see cref="T:System.Windows.Forms.UpDownBase" /> control.</param>
		// Token: 0x06004939 RID: 18745 RVA: 0x00132242 File Offset: 0x00130442
		public UpDownEventArgs(int buttonPushed)
		{
			this.buttonID = buttonPushed;
		}

		/// <summary>Gets a value that represents which button the user clicked.</summary>
		/// <returns>A value that represents which button the user clicked.</returns>
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x0600493A RID: 18746 RVA: 0x00132251 File Offset: 0x00130451
		public int ButtonID
		{
			get
			{
				return this.buttonID;
			}
		}

		// Token: 0x040026DC RID: 9948
		private int buttonID;
	}
}

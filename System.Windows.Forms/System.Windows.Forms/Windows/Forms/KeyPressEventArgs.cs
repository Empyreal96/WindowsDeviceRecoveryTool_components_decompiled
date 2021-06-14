using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
	// Token: 0x020002A4 RID: 676
	[ComVisible(true)]
	public class KeyPressEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> class.</summary>
		/// <param name="keyChar">The ASCII character corresponding to the key the user pressed. </param>
		// Token: 0x06002735 RID: 10037 RVA: 0x000B7E45 File Offset: 0x000B6045
		public KeyPressEventArgs(char keyChar)
		{
			this.keyChar = keyChar;
		}

		/// <summary>Gets or sets the character corresponding to the key pressed.</summary>
		/// <returns>The ASCII character that is composed. For example, if the user presses SHIFT + K, this property returns an uppercase K.</returns>
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x000B7E54 File Offset: 0x000B6054
		// (set) Token: 0x06002737 RID: 10039 RVA: 0x000B7E5C File Offset: 0x000B605C
		public char KeyChar
		{
			get
			{
				return this.keyChar;
			}
			set
			{
				this.keyChar = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event was handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event is handled; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x000B7E65 File Offset: 0x000B6065
		// (set) Token: 0x06002739 RID: 10041 RVA: 0x000B7E6D File Offset: 0x000B606D
		public bool Handled
		{
			get
			{
				return this.handled;
			}
			set
			{
				this.handled = value;
			}
		}

		// Token: 0x04001083 RID: 4227
		private char keyChar;

		// Token: 0x04001084 RID: 4228
		private bool handled;
	}
}

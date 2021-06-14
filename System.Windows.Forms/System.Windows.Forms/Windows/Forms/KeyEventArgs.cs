using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
	// Token: 0x020002A2 RID: 674
	[ComVisible(true)]
	public class KeyEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.KeyEventArgs" /> class.</summary>
		/// <param name="keyData">A <see cref="T:System.Windows.Forms.Keys" /> representing the key that was pressed, combined with any modifier flags that indicate which CTRL, SHIFT, and ALT keys were pressed at the same time. Possible values are obtained be applying the bitwise OR (|) operator to constants from the <see cref="T:System.Windows.Forms.Keys" /> enumeration. </param>
		// Token: 0x06002725 RID: 10021 RVA: 0x000B7D73 File Offset: 0x000B5F73
		public KeyEventArgs(Keys keyData)
		{
			this.keyData = keyData;
		}

		/// <summary>Gets a value indicating whether the ALT key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the ALT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002726 RID: 10022 RVA: 0x000B7D82 File Offset: 0x000B5F82
		public virtual bool Alt
		{
			get
			{
				return (this.keyData & Keys.Alt) == Keys.Alt;
			}
		}

		/// <summary>Gets a value indicating whether the CTRL key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the CTRL key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002727 RID: 10023 RVA: 0x000B7D97 File Offset: 0x000B5F97
		public bool Control
		{
			get
			{
				return (this.keyData & Keys.Control) == Keys.Control;
			}
		}

		/// <summary>Gets or sets a value indicating whether the event was handled.</summary>
		/// <returns>
		///     <see langword="true" /> to bypass the control's default handling; otherwise, <see langword="false" /> to also pass the event along to the default control handler.</returns>
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002728 RID: 10024 RVA: 0x000B7DAC File Offset: 0x000B5FAC
		// (set) Token: 0x06002729 RID: 10025 RVA: 0x000B7DB4 File Offset: 0x000B5FB4
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

		/// <summary>Gets the keyboard code for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Keys" /> value that is the key code for the event.</returns>
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x0600272A RID: 10026 RVA: 0x000B7DC0 File Offset: 0x000B5FC0
		public Keys KeyCode
		{
			get
			{
				Keys keys = this.keyData & Keys.KeyCode;
				if (!Enum.IsDefined(typeof(Keys), (int)keys))
				{
					return Keys.None;
				}
				return keys;
			}
		}

		/// <summary>Gets the keyboard value for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>The integer representation of the <see cref="P:System.Windows.Forms.KeyEventArgs.KeyCode" /> property.</returns>
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x000B7DF4 File Offset: 0x000B5FF4
		public int KeyValue
		{
			get
			{
				return (int)(this.keyData & Keys.KeyCode);
			}
		}

		/// <summary>Gets the key data for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Keys" /> representing the key code for the key that was pressed, combined with modifier flags that indicate which combination of CTRL, SHIFT, and ALT keys was pressed at the same time.</returns>
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000B7E02 File Offset: 0x000B6002
		public Keys KeyData
		{
			get
			{
				return this.keyData;
			}
		}

		/// <summary>Gets the modifier flags for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event. The flags indicate which combination of CTRL, SHIFT, and ALT keys was pressed.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.Keys" /> value representing one or more modifier flags.</returns>
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x000B7E0A File Offset: 0x000B600A
		public Keys Modifiers
		{
			get
			{
				return this.keyData & Keys.Modifiers;
			}
		}

		/// <summary>Gets a value indicating whether the SHIFT key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the SHIFT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x000B7E18 File Offset: 0x000B6018
		public virtual bool Shift
		{
			get
			{
				return (this.keyData & Keys.Shift) == Keys.Shift;
			}
		}

		/// <summary>Gets or sets a value indicating whether the key event should be passed on to the underlying control.</summary>
		/// <returns>
		///     <see langword="true" /> if the key event should not be sent to the control; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x000B7E2D File Offset: 0x000B602D
		// (set) Token: 0x06002730 RID: 10032 RVA: 0x000B7E35 File Offset: 0x000B6035
		public bool SuppressKeyPress
		{
			get
			{
				return this.suppressKeyPress;
			}
			set
			{
				this.suppressKeyPress = value;
				this.handled = value;
			}
		}

		// Token: 0x04001080 RID: 4224
		private readonly Keys keyData;

		// Token: 0x04001081 RID: 4225
		private bool handled;

		// Token: 0x04001082 RID: 4226
		private bool suppressKeyPress;
	}
}

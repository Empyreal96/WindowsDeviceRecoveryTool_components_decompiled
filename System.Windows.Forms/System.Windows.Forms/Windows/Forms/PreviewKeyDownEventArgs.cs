using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.PreviewKeyDown" /> event.</summary>
	// Token: 0x02000313 RID: 787
	public class PreviewKeyDownEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PreviewKeyDownEventArgs" /> class with the specified key. </summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys" /> values.</param>
		// Token: 0x06002F9F RID: 12191 RVA: 0x000DB286 File Offset: 0x000D9486
		public PreviewKeyDownEventArgs(Keys keyData)
		{
			this._keyData = keyData;
		}

		/// <summary>Gets a value indicating whether the ALT key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the ALT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000DB295 File Offset: 0x000D9495
		public bool Alt
		{
			get
			{
				return (this._keyData & Keys.Alt) == Keys.Alt;
			}
		}

		/// <summary>Gets a value indicating whether the CTRL key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the CTRL key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000DB2AA File Offset: 0x000D94AA
		public bool Control
		{
			get
			{
				return (this._keyData & Keys.Control) == Keys.Control;
			}
		}

		/// <summary>Gets the keyboard code for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Keys" /> values.</returns>
		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06002FA2 RID: 12194 RVA: 0x000DB2C0 File Offset: 0x000D94C0
		public Keys KeyCode
		{
			get
			{
				Keys keys = this._keyData & Keys.KeyCode;
				if (!Enum.IsDefined(typeof(Keys), (int)keys))
				{
					return Keys.None;
				}
				return keys;
			}
		}

		/// <summary>Gets the keyboard value for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>An <see cref="T:System.Int32" /> representing the keyboard value.</returns>
		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000DB2F4 File Offset: 0x000D94F4
		public int KeyValue
		{
			get
			{
				return (int)(this._keyData & Keys.KeyCode);
			}
		}

		/// <summary>Gets the key data for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Keys" /> values.</returns>
		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x000DB302 File Offset: 0x000D9502
		public Keys KeyData
		{
			get
			{
				return this._keyData;
			}
		}

		/// <summary>Gets the modifier flags for a <see cref="E:System.Windows.Forms.Control.KeyDown" /> or <see cref="E:System.Windows.Forms.Control.KeyUp" /> event.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.Keys" /> values.</returns>
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000DB30A File Offset: 0x000D950A
		public Keys Modifiers
		{
			get
			{
				return this._keyData & Keys.Modifiers;
			}
		}

		/// <summary>Gets a value indicating whether the SHIFT key was pressed.</summary>
		/// <returns>
		///     <see langword="true" /> if the SHIFT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06002FA6 RID: 12198 RVA: 0x000DB318 File Offset: 0x000D9518
		public bool Shift
		{
			get
			{
				return (this._keyData & Keys.Shift) == Keys.Shift;
			}
		}

		/// <summary>Gets or sets a value indicating whether a key is a regular input key.</summary>
		/// <returns>
		///     <see langword="true" /> if the key is a regular input key; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x000DB32D File Offset: 0x000D952D
		// (set) Token: 0x06002FA8 RID: 12200 RVA: 0x000DB335 File Offset: 0x000D9535
		public bool IsInputKey
		{
			get
			{
				return this._isInputKey;
			}
			set
			{
				this._isInputKey = value;
			}
		}

		// Token: 0x04001D99 RID: 7577
		private readonly Keys _keyData;

		// Token: 0x04001D9A RID: 7578
		private bool _isInputKey;
	}
}

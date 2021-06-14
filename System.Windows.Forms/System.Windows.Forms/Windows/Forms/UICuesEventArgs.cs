using System;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.Control.ChangeUICues" /> event.</summary>
	// Token: 0x02000418 RID: 1048
	public class UICuesEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.UICuesEventArgs" /> class with the specified <see cref="T:System.Windows.Forms.UICues" />.</summary>
		/// <param name="uicues">A bitwise combination of the <see cref="T:System.Windows.Forms.UICues" /> values. </param>
		// Token: 0x0600476B RID: 18283 RVA: 0x00130E8D File Offset: 0x0012F08D
		public UICuesEventArgs(UICues uicues)
		{
			this.uicues = uicues;
		}

		/// <summary>Gets a value indicating whether focus rectangles are shown after the change.</summary>
		/// <returns>
		///     <see langword="true" /> if focus rectangles are shown after the change; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x0600476C RID: 18284 RVA: 0x00130E9C File Offset: 0x0012F09C
		public bool ShowFocus
		{
			get
			{
				return (this.uicues & UICues.ShowFocus) > UICues.None;
			}
		}

		/// <summary>Gets a value indicating whether keyboard cues are underlined after the change.</summary>
		/// <returns>
		///     <see langword="true" /> if keyboard cues are underlined after the change; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x0600476D RID: 18285 RVA: 0x00130EA9 File Offset: 0x0012F0A9
		public bool ShowKeyboard
		{
			get
			{
				return (this.uicues & UICues.ShowKeyboard) > UICues.None;
			}
		}

		/// <summary>Gets a value indicating whether the state of the focus cues has changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the state of the focus cues has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x0600476E RID: 18286 RVA: 0x00130EB6 File Offset: 0x0012F0B6
		public bool ChangeFocus
		{
			get
			{
				return (this.uicues & UICues.ChangeFocus) > UICues.None;
			}
		}

		/// <summary>Gets a value indicating whether the state of the keyboard cues has changed.</summary>
		/// <returns>
		///     <see langword="true" /> if the state of the keyboard cues has changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x0600476F RID: 18287 RVA: 0x00130EC3 File Offset: 0x0012F0C3
		public bool ChangeKeyboard
		{
			get
			{
				return (this.uicues & UICues.ChangeKeyboard) > UICues.None;
			}
		}

		/// <summary>Gets the bitwise combination of the <see cref="T:System.Windows.Forms.UICues" /> values.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.UICues" /> values. The default is <see cref="F:System.Windows.Forms.UICues.Changed" />.</returns>
		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x06004770 RID: 18288 RVA: 0x00130ED0 File Offset: 0x0012F0D0
		public UICues Changed
		{
			get
			{
				return this.uicues & UICues.Changed;
			}
		}

		// Token: 0x040026C3 RID: 9923
		private readonly UICues uicues;
	}
}

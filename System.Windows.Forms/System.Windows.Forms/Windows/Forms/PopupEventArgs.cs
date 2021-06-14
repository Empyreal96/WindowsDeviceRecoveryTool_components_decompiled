using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Forms.ToolTip.Popup" /> event.</summary>
	// Token: 0x0200030C RID: 780
	public class PopupEventArgs : CancelEventArgs
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Forms.PopupEventArgs" /> class.</summary>
		/// <param name="associatedWindow">The <see cref="T:System.Windows.Forms.IWin32Window" /> that the ToolTip is bound to.</param>
		/// <param name="associatedControl">The <see cref="T:System.Windows.Forms.Control" /> that the ToolTip is being created for.</param>
		/// <param name="isBalloon">
		///       <see langword="true" /> to indicate that the associated ToolTip window has a balloon-style appearance; otherwise, <see langword="false" /> to indicate that the ToolTip window has a standard rectangular appearance.</param>
		/// <param name="size">The <see cref="T:System.Drawing.Size" /> of the ToolTip.</param>
		// Token: 0x06002F8E RID: 12174 RVA: 0x000DB1A6 File Offset: 0x000D93A6
		public PopupEventArgs(IWin32Window associatedWindow, Control associatedControl, bool isBalloon, Size size)
		{
			this.associatedWindow = associatedWindow;
			this.size = size;
			this.associatedControl = associatedControl;
			this.isBalloon = isBalloon;
		}

		/// <summary>Gets the window to which this <see cref="T:System.Windows.Forms.ToolTip" /> is bound.</summary>
		/// <returns>The window which owns the <see cref="T:System.Windows.Forms.ToolTip" />.</returns>
		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06002F8F RID: 12175 RVA: 0x000DB1CB File Offset: 0x000D93CB
		public IWin32Window AssociatedWindow
		{
			get
			{
				return this.associatedWindow;
			}
		}

		/// <summary>Gets the control for which the <see cref="T:System.Windows.Forms.ToolTip" /> is being drawn.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Control" /> that is associated with the <see cref="T:System.Windows.Forms.ToolTip" />, or <see langword="null" /> if the ToolTip is not associated with a control.</returns>
		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002F90 RID: 12176 RVA: 0x000DB1D3 File Offset: 0x000D93D3
		public Control AssociatedControl
		{
			get
			{
				return this.associatedControl;
			}
		}

		/// <summary>Gets a value indicating whether the ToolTip is displayed as a standard rectangular or a balloon window.</summary>
		/// <returns>
		///     <see langword="true" /> if the ToolTip is displayed in a balloon window; otherwise, <see langword="false" /> if a standard rectangular window is used.</returns>
		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002F91 RID: 12177 RVA: 0x000DB1DB File Offset: 0x000D93DB
		public bool IsBalloon
		{
			get
			{
				return this.isBalloon;
			}
		}

		/// <summary>Gets or sets the size of the ToolTip.</summary>
		/// <returns>The <see cref="T:System.Drawing.Size" /> of the <see cref="T:System.Windows.Forms.ToolTip" /> window.</returns>
		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002F92 RID: 12178 RVA: 0x000DB1E3 File Offset: 0x000D93E3
		// (set) Token: 0x06002F93 RID: 12179 RVA: 0x000DB1EB File Offset: 0x000D93EB
		public Size ToolTipSize
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x04001D82 RID: 7554
		private IWin32Window associatedWindow;

		// Token: 0x04001D83 RID: 7555
		private Size size;

		// Token: 0x04001D84 RID: 7556
		private Control associatedControl;

		// Token: 0x04001D85 RID: 7557
		private bool isBalloon;
	}
}

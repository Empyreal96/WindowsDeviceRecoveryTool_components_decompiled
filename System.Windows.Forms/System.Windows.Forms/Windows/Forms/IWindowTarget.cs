using System;

namespace System.Windows.Forms
{
	/// <summary>Defines the communication layer between a control and the Win32 API.</summary>
	// Token: 0x020002A0 RID: 672
	public interface IWindowTarget
	{
		/// <summary>Sets the handle of the <see cref="T:System.Windows.Forms.IWindowTarget" /> to the specified handle.</summary>
		/// <param name="newHandle">The new handle of the <see cref="T:System.Windows.Forms.IWindowTarget" />.</param>
		// Token: 0x06002706 RID: 9990
		void OnHandleChange(IntPtr newHandle);

		/// <summary>Processes the Windows messages.</summary>
		/// <param name="m">The Windows message to process. </param>
		// Token: 0x06002707 RID: 9991
		void OnMessage(ref Message m);
	}
}

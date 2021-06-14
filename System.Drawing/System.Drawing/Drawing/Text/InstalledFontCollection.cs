using System;

namespace System.Drawing.Text
{
	/// <summary>Represents the fonts installed on the system. This class cannot be inherited. </summary>
	// Token: 0x02000088 RID: 136
	public sealed class InstalledFontCollection : FontCollection
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Text.InstalledFontCollection" /> class. </summary>
		// Token: 0x060008CB RID: 2251 RVA: 0x00022100 File Offset: 0x00020300
		public InstalledFontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipNewInstalledFontCollection(out this.nativeFontCollection);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}
	}
}

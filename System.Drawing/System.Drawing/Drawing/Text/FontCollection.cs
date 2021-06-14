using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Text
{
	/// <summary>Provides a base class for installed and private font collections. </summary>
	// Token: 0x02000085 RID: 133
	public abstract class FontCollection : IDisposable
	{
		// Token: 0x060008C6 RID: 2246 RVA: 0x0002201D File Offset: 0x0002021D
		internal FontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
		}

		/// <summary>Releases all resources used by this <see cref="T:System.Drawing.Text.FontCollection" />.</summary>
		// Token: 0x060008C7 RID: 2247 RVA: 0x00022030 File Offset: 0x00020230
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Drawing.Text.FontCollection" /> and optionally releases the managed resources. </summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources. </param>
		// Token: 0x060008C8 RID: 2248 RVA: 0x00015255 File Offset: 0x00013455
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>Gets the array of <see cref="T:System.Drawing.FontFamily" /> objects associated with this <see cref="T:System.Drawing.Text.FontCollection" />. </summary>
		/// <returns>An array of <see cref="T:System.Drawing.FontFamily" /> objects.</returns>
		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00022040 File Offset: 0x00020240
		public FontFamily[] Families
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetFontCollectionFamilyCount(new HandleRef(this, this.nativeFontCollection), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				IntPtr[] array = new IntPtr[num];
				int num3 = 0;
				num2 = SafeNativeMethods.Gdip.GdipGetFontCollectionFamilyList(new HandleRef(this, this.nativeFontCollection), num, array, out num3);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				FontFamily[] array2 = new FontFamily[num3];
				for (int i = 0; i < num3; i++)
				{
					IntPtr family;
					SafeNativeMethods.Gdip.GdipCloneFontFamily(new HandleRef(null, array[i]), out family);
					array2[i] = new FontFamily(family);
				}
				return array2;
			}
		}

		/// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
		// Token: 0x060008CA RID: 2250 RVA: 0x000220D0 File Offset: 0x000202D0
		~FontCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x0400071E RID: 1822
		internal IntPtr nativeFontCollection;
	}
}

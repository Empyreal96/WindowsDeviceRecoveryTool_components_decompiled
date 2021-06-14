using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Drawing.Text
{
	/// <summary>Provides a collection of font families built from font files that are provided by the client application.</summary>
	// Token: 0x02000089 RID: 137
	public sealed class PrivateFontCollection : FontCollection
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Text.PrivateFontCollection" /> class. </summary>
		// Token: 0x060008CC RID: 2252 RVA: 0x00022134 File Offset: 0x00020334
		public PrivateFontCollection()
		{
			this.nativeFontCollection = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipNewPrivateFontCollection(out this.nativeFontCollection);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (!LocalAppContextSwitches.DoNotRemoveGdiFontsResourcesFromFontCollection)
			{
				this.gdiFonts = new List<string>();
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002217C File Offset: 0x0002037C
		protected override void Dispose(bool disposing)
		{
			if (this.nativeFontCollection != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePrivateFontCollection(out this.nativeFontCollection);
					if (this.gdiFonts != null)
					{
						foreach (string fileName in this.gdiFonts)
						{
							SafeNativeMethods.RemoveFontFile(fileName);
						}
						this.gdiFonts.Clear();
						this.gdiFonts = null;
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativeFontCollection = IntPtr.Zero;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Adds a font from the specified file to this <see cref="T:System.Drawing.Text.PrivateFontCollection" />. </summary>
		/// <param name="filename">A <see cref="T:System.String" /> that contains the file name of the font to add. </param>
		/// <exception cref="T:System.IO.FileNotFoundException">The specified font is not supported or the font file cannot be found.</exception>
		// Token: 0x060008CE RID: 2254 RVA: 0x00022244 File Offset: 0x00020444
		public void AddFontFile(string filename)
		{
			IntSecurity.DemandReadFileIO(filename);
			int num = SafeNativeMethods.Gdip.GdipPrivateAddFontFile(new HandleRef(this, this.nativeFontCollection), filename);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			if (SafeNativeMethods.AddFontFile(filename) != 0 && this.gdiFonts != null)
			{
				this.gdiFonts.Add(filename);
			}
		}

		/// <summary>Adds a font contained in system memory to this <see cref="T:System.Drawing.Text.PrivateFontCollection" />.</summary>
		/// <param name="memory">The memory address of the font to add. </param>
		/// <param name="length">The memory length of the font to add. </param>
		// Token: 0x060008CF RID: 2255 RVA: 0x00022290 File Offset: 0x00020490
		public void AddMemoryFont(IntPtr memory, int length)
		{
			IntSecurity.ObjectFromWin32Handle.Demand();
			int num = SafeNativeMethods.Gdip.GdipPrivateAddMemoryFont(new HandleRef(this, this.nativeFontCollection), new HandleRef(null, memory), length);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x04000727 RID: 1831
		private List<string> gdiFonts;
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	/// <summary>Defines an array of colors that make up a color palette. The colors are 32-bit ARGB colors. Not inheritable.</summary>
	// Token: 0x02000094 RID: 148
	public sealed class ColorPalette
	{
		/// <summary>Gets a value that specifies how to interpret the color information in the array of colors.</summary>
		/// <returns>The following flag values are valid: 0x00000001The color values in the array contain alpha information. 0x00000002The colors in the array are grayscale values. 0x00000004The colors in the array are halftone values. </returns>
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x00022B09 File Offset: 0x00020D09
		public int Flags
		{
			get
			{
				return this.flags;
			}
		}

		/// <summary>Gets an array of <see cref="T:System.Drawing.Color" /> structures.</summary>
		/// <returns>The array of <see cref="T:System.Drawing.Color" /> structure that make up this <see cref="T:System.Drawing.Imaging.ColorPalette" />.</returns>
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000922 RID: 2338 RVA: 0x00022B11 File Offset: 0x00020D11
		public Color[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00022B19 File Offset: 0x00020D19
		internal ColorPalette(int count)
		{
			this.entries = new Color[count];
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00022B2D File Offset: 0x00020D2D
		internal ColorPalette()
		{
			this.entries = new Color[1];
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00022B44 File Offset: 0x00020D44
		internal void ConvertFromMemory(IntPtr memory)
		{
			this.flags = Marshal.ReadInt32(memory);
			int num = Marshal.ReadInt32((IntPtr)((long)memory + 4L));
			this.entries = new Color[num];
			for (int i = 0; i < num; i++)
			{
				int argb = Marshal.ReadInt32((IntPtr)((long)memory + 8L + (long)(i * 4)));
				this.entries[i] = Color.FromArgb(argb);
			}
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00022BB4 File Offset: 0x00020DB4
		internal IntPtr ConvertToMemory()
		{
			int num = this.entries.Length;
			IntPtr intPtr;
			checked
			{
				intPtr = Marshal.AllocHGlobal(4 * (2 + num));
				Marshal.WriteInt32(intPtr, 0, this.flags);
				Marshal.WriteInt32((IntPtr)((long)intPtr + 4L), 0, num);
			}
			for (int i = 0; i < num; i++)
			{
				Marshal.WriteInt32((IntPtr)((long)intPtr + (long)(4 * (i + 2))), 0, this.entries[i].ToArgb());
			}
			return intPtr;
		}

		// Token: 0x0400076C RID: 1900
		private int flags;

		// Token: 0x0400076D RID: 1901
		private Color[] entries;
	}
}

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms
{
	// Token: 0x0200036F RID: 879
	internal class StringSource : IEnumString
	{
		// Token: 0x0600372C RID: 14124 RVA: 0x000F9BF4 File Offset: 0x000F7DF4
		public StringSource(string[] strings)
		{
			Array.Clear(strings, 0, this.size);
			if (strings != null)
			{
				this.strings = strings;
			}
			this.current = 0;
			this.size = ((strings == null) ? 0 : strings.Length);
			Guid guid = typeof(UnsafeNativeMethods.IAutoComplete2).GUID;
			object obj = UnsafeNativeMethods.CoCreateInstance(ref StringSource.autoCompleteClsid, null, 1, ref guid);
			this.autoCompleteObject2 = (UnsafeNativeMethods.IAutoComplete2)obj;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000F9C60 File Offset: 0x000F7E60
		public bool Bind(HandleRef edit, int options)
		{
			bool result = false;
			if (this.autoCompleteObject2 != null)
			{
				try
				{
					this.autoCompleteObject2.SetOptions(options);
					this.autoCompleteObject2.Init(edit, this, null, null);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x000F9CB0 File Offset: 0x000F7EB0
		public void ReleaseAutoComplete()
		{
			if (this.autoCompleteObject2 != null)
			{
				Marshal.ReleaseComObject(this.autoCompleteObject2);
				this.autoCompleteObject2 = null;
			}
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x000F9CD0 File Offset: 0x000F7ED0
		public void RefreshList(string[] newSource)
		{
			Array.Clear(this.strings, 0, this.size);
			if (this.strings != null)
			{
				this.strings = newSource;
			}
			this.current = 0;
			this.size = ((this.strings == null) ? 0 : this.strings.Length);
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x000F9D1E File Offset: 0x000F7F1E
		void IEnumString.Clone(out IEnumString ppenum)
		{
			ppenum = new StringSource(this.strings);
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x000F9D30 File Offset: 0x000F7F30
		int IEnumString.Next(int celt, string[] rgelt, IntPtr pceltFetched)
		{
			if (celt < 0)
			{
				return -2147024809;
			}
			int num = 0;
			while (this.current < this.size && celt > 0)
			{
				rgelt[num] = this.strings[this.current];
				this.current++;
				num++;
				celt--;
			}
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, num);
			}
			if (celt != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000F9D9E File Offset: 0x000F7F9E
		void IEnumString.Reset()
		{
			this.current = 0;
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x000F9DA7 File Offset: 0x000F7FA7
		int IEnumString.Skip(int celt)
		{
			this.current += celt;
			if (this.current >= this.size)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x040021FE RID: 8702
		private string[] strings;

		// Token: 0x040021FF RID: 8703
		private int current;

		// Token: 0x04002200 RID: 8704
		private int size;

		// Token: 0x04002201 RID: 8705
		private UnsafeNativeMethods.IAutoComplete2 autoCompleteObject2;

		// Token: 0x04002202 RID: 8706
		private static Guid autoCompleteClsid = new Guid("{00BB2763-6A77-11D0-A535-00C04FD7D062}");
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x0200008B RID: 139
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class PropertyItemInternal : IDisposable
	{
		// Token: 0x060008D0 RID: 2256 RVA: 0x000222CB File Offset: 0x000204CB
		internal PropertyItemInternal()
		{
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000222E0 File Offset: 0x000204E0
		~PropertyItemInternal()
		{
			this.Dispose(false);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00022310 File Offset: 0x00020510
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00022319 File Offset: 0x00020519
		private void Dispose(bool disposing)
		{
			if (this.value != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.value);
				this.value = IntPtr.Zero;
			}
			if (disposing)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002234C File Offset: 0x0002054C
		internal static PropertyItemInternal ConvertFromPropertyItem(PropertyItem propItem)
		{
			PropertyItemInternal propertyItemInternal = new PropertyItemInternal();
			propertyItemInternal.id = propItem.Id;
			propertyItemInternal.len = 0;
			propertyItemInternal.type = propItem.Type;
			byte[] array = propItem.Value;
			if (array != null)
			{
				int num = array.Length;
				propertyItemInternal.len = num;
				propertyItemInternal.value = Marshal.AllocHGlobal(num);
				Marshal.Copy(array, 0, propertyItemInternal.value, num);
			}
			return propertyItemInternal;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000223B0 File Offset: 0x000205B0
		internal static PropertyItem[] ConvertFromMemory(IntPtr propdata, int count)
		{
			PropertyItem[] array = new PropertyItem[count];
			for (int i = 0; i < count; i++)
			{
				PropertyItemInternal propertyItemInternal = null;
				try
				{
					propertyItemInternal = (PropertyItemInternal)UnsafeNativeMethods.PtrToStructure(propdata, typeof(PropertyItemInternal));
					array[i] = new PropertyItem();
					array[i].Id = propertyItemInternal.id;
					array[i].Len = propertyItemInternal.len;
					array[i].Type = propertyItemInternal.type;
					array[i].Value = propertyItemInternal.Value;
					propertyItemInternal.value = IntPtr.Zero;
				}
				finally
				{
					if (propertyItemInternal != null)
					{
						propertyItemInternal.Dispose();
					}
				}
				propdata = (IntPtr)((long)propdata + (long)Marshal.SizeOf(typeof(PropertyItemInternal)));
			}
			return array;
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00022474 File Offset: 0x00020674
		public byte[] Value
		{
			get
			{
				if (this.len == 0)
				{
					return null;
				}
				byte[] array = new byte[this.len];
				Marshal.Copy(this.value, array, 0, this.len);
				return array;
			}
		}

		// Token: 0x0400072F RID: 1839
		public int id;

		// Token: 0x04000730 RID: 1840
		public int len;

		// Token: 0x04000731 RID: 1841
		public short type;

		// Token: 0x04000732 RID: 1842
		public IntPtr value = IntPtr.Zero;
	}
}

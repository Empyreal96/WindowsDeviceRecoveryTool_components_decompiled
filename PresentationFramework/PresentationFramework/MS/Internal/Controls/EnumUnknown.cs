using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000755 RID: 1877
	internal class EnumUnknown : UnsafeNativeMethods.IEnumUnknown
	{
		// Token: 0x0600779A RID: 30618 RVA: 0x002223B8 File Offset: 0x002205B8
		internal EnumUnknown(object[] arr)
		{
			this.arr = arr;
			this.loc = 0;
			this.size = ((arr == null) ? 0 : arr.Length);
		}

		// Token: 0x0600779B RID: 30619 RVA: 0x002223DD File Offset: 0x002205DD
		private EnumUnknown(object[] arr, int loc) : this(arr)
		{
			this.loc = loc;
		}

		// Token: 0x0600779C RID: 30620 RVA: 0x002223F0 File Offset: 0x002205F0
		[SecurityCritical]
		int UnsafeNativeMethods.IEnumUnknown.Next(int celt, IntPtr rgelt, IntPtr pceltFetched)
		{
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, 0);
			}
			if (celt < 0)
			{
				return -2147024809;
			}
			int num = 0;
			if (this.loc >= this.size)
			{
				num = 0;
			}
			else
			{
				while (this.loc < this.size && num < celt)
				{
					if (this.arr[this.loc] != null)
					{
						Marshal.WriteIntPtr(rgelt, Marshal.GetIUnknownForObject(this.arr[this.loc]));
						rgelt = (IntPtr)((long)rgelt + (long)sizeof(IntPtr));
						num++;
					}
					this.loc++;
				}
			}
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, num);
			}
			if (num != celt)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600779D RID: 30621 RVA: 0x002224AC File Offset: 0x002206AC
		[SecurityCritical]
		int UnsafeNativeMethods.IEnumUnknown.Skip(int celt)
		{
			this.loc += celt;
			if (this.loc >= this.size)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600779E RID: 30622 RVA: 0x002224CD File Offset: 0x002206CD
		[SecurityCritical]
		void UnsafeNativeMethods.IEnumUnknown.Reset()
		{
			this.loc = 0;
		}

		// Token: 0x0600779F RID: 30623 RVA: 0x002224D6 File Offset: 0x002206D6
		[SecurityCritical]
		void UnsafeNativeMethods.IEnumUnknown.Clone(out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = new EnumUnknown(this.arr, this.loc);
		}

		// Token: 0x040038C8 RID: 14536
		private object[] arr;

		// Token: 0x040038C9 RID: 14537
		private int loc;

		// Token: 0x040038CA RID: 14538
		private int size;
	}
}

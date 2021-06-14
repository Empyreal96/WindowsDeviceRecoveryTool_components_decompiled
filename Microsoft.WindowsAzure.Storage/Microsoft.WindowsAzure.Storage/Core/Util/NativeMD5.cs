using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Microsoft.WindowsAzure.Storage.Core.Util
{
	// Token: 0x02000063 RID: 99
	internal sealed class NativeMD5 : MD5
	{
		// Token: 0x06000DA8 RID: 3496 RVA: 0x00032A67 File Offset: 0x00030C67
		public NativeMD5()
		{
			NativeMD5.ValidateReturnCode(NativeMethods.CryptAcquireContextW(out this.hashProv, null, null, 1U, 4026531840U));
			this.Initialize();
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00032A90 File Offset: 0x00030C90
		~NativeMD5()
		{
			this.Dispose(false);
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00032AC0 File Offset: 0x00030CC0
		public override void Initialize()
		{
			if (this.hashHandle != IntPtr.Zero)
			{
				NativeMethods.CryptDestroyHash(this.hashHandle);
				this.hashHandle = IntPtr.Zero;
			}
			NativeMD5.ValidateReturnCode(NativeMethods.CryptCreateHash(this.hashProv, 32771U, IntPtr.Zero, 0U, out this.hashHandle));
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x00032B18 File Offset: 0x00030D18
		protected override void HashCore(byte[] array, int offset, int dataLen)
		{
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				if (offset != 0)
				{
					intPtr = IntPtr.Add(intPtr, offset);
				}
				NativeMD5.ValidateReturnCode(NativeMethods.CryptHashData(this.hashHandle, intPtr, dataLen, 0U));
			}
			finally
			{
				gchandle.Free();
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00032B70 File Offset: 0x00030D70
		protected override byte[] HashFinal()
		{
			byte[] array = new byte[16];
			int num = array.Length;
			NativeMD5.ValidateReturnCode(NativeMethods.CryptGetHashParam(this.hashHandle, 2U, array, ref num, 0U));
			return array;
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00032BA0 File Offset: 0x00030DA0
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (this.hashHandle != IntPtr.Zero)
				{
					NativeMethods.CryptDestroyHash(this.hashHandle);
					this.hashHandle = IntPtr.Zero;
				}
				if (this.hashProv != IntPtr.Zero)
				{
					NativeMethods.CryptReleaseContext(this.hashProv, 0);
					this.hashProv = IntPtr.Zero;
				}
				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00032C18 File Offset: 0x00030E18
		private static void ValidateReturnCode(bool status)
		{
			if (!status)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Crypto function failed with error code '{0}'", new object[]
				{
					lastWin32Error
				}));
			}
		}

		// Token: 0x040001D6 RID: 470
		private const uint ProvRsaFull = 1U;

		// Token: 0x040001D7 RID: 471
		private const uint CryptVerifyContext = 4026531840U;

		// Token: 0x040001D8 RID: 472
		private const uint CalgMD5 = 32771U;

		// Token: 0x040001D9 RID: 473
		private const uint HashVal = 2U;

		// Token: 0x040001DA RID: 474
		private IntPtr hashHandle;

		// Token: 0x040001DB RID: 475
		private IntPtr hashProv;

		// Token: 0x040001DC RID: 476
		private bool disposed;
	}
}

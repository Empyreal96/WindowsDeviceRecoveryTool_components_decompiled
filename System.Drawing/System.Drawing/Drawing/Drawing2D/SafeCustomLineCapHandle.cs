using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000D4 RID: 212
	[SecurityCritical]
	internal class SafeCustomLineCapHandle : SafeHandle
	{
		// Token: 0x06000B6A RID: 2922 RVA: 0x00029D35 File Offset: 0x00027F35
		internal SafeCustomLineCapHandle(IntPtr h) : base(IntPtr.Zero, true)
		{
			base.SetHandle(h);
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00029D4C File Offset: 0x00027F4C
		[SecurityCritical]
		protected override bool ReleaseHandle()
		{
			int num = 0;
			if (!this.IsInvalid)
			{
				try
				{
					num = SafeNativeMethods.Gdip.GdipDeleteCustomLineCap(new HandleRef(this, this.handle));
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
					this.handle = IntPtr.Zero;
				}
			}
			return num == 0;
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00029DB0 File Offset: 0x00027FB0
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00029DC2 File Offset: 0x00027FC2
		public static implicit operator IntPtr(SafeCustomLineCapHandle handle)
		{
			if (handle != null)
			{
				return handle.handle;
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00029DD3 File Offset: 0x00027FD3
		public static explicit operator SafeCustomLineCapHandle(IntPtr handle)
		{
			return new SafeCustomLineCapHandle(handle);
		}
	}
}

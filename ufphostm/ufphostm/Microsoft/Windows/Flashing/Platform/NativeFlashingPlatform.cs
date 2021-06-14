using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FlashingPlatform;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x0200003F RID: 63
	internal class NativeFlashingPlatform
	{
		// Token: 0x06000117 RID: 279
		[DllImport("ufphost.dll")]
		[MethodImpl(MethodImplOptions.ForwardRef)]
		public unsafe static extern int GetFlashingPlatformVersion(uint* A_0, uint* A_1);

		// Token: 0x06000118 RID: 280
		[DllImport("ufphost.dll")]
		[MethodImpl(MethodImplOptions.ForwardRef)]
		public unsafe static extern int CreateFlashingPlatform(ushort* A_0, IFlashingPlatform** A_1);
	}
}

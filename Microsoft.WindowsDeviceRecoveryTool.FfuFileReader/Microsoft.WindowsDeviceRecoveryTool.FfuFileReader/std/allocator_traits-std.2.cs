using System;
using System.Runtime.CompilerServices;

namespace std
{
	// Token: 0x0200019D RID: 413
	[NativeCppClass]
	internal struct allocator_traits<std::allocator<wchar_t>\u0020>
	{
		// Token: 0x0200019E RID: 414
		[NativeCppClass]
		[CLSCompliant(false)]
		public struct rebind_alloc<wchar_t>
		{
		}

		// Token: 0x0200019F RID: 415
		[CLSCompliant(false)]
		[NativeCppClass]
		public struct rebind_alloc<wchar_t\u0020*>
		{
		}
	}
}

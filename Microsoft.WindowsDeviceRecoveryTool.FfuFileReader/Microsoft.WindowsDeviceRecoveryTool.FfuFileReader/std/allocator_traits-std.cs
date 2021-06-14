using System;
using System.Runtime.CompilerServices;

namespace std
{
	// Token: 0x02000198 RID: 408
	[NativeCppClass]
	internal struct allocator_traits<std::allocator<char>\u0020>
	{
		// Token: 0x02000199 RID: 409
		[CLSCompliant(false)]
		[NativeCppClass]
		public struct rebind_alloc<char>
		{
		}

		// Token: 0x0200019A RID: 410
		[NativeCppClass]
		[CLSCompliant(false)]
		public struct rebind_alloc<char\u0020*>
		{
		}
	}
}

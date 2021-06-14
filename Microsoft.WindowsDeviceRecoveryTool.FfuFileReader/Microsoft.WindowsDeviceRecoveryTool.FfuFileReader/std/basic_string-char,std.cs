using System;
using System.Runtime.CompilerServices;

namespace std
{
	// Token: 0x0200012F RID: 303
	[UnsafeValueType]
	[NativeCppClass]
	internal struct basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>
	{
		// Token: 0x060002AD RID: 685 RVA: 0x000025D8 File Offset: 0x000019D8
		public unsafe static void <MarshalCopy>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1)
		{
			*(int*)(A_0 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			*(int*)(A_0 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, A_1, 0U, uint.MaxValue);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000023C4 File Offset: 0x000017C4
		public unsafe static void <MarshalDestroy>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
		}
	}
}

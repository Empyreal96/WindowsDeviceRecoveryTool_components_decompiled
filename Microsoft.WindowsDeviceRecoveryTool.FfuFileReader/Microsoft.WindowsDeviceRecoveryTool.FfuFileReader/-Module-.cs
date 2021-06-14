using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using <CppImplementationDetails>;
using <CrtImplementationDetails>;
using msclr.interop.details;
using std;
using std.io_errc;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x00001000 File Offset: 0x00000400
	internal unsafe static exception_ptr* {ctor}(exception_ptr* A_0, exception_ptr* _Rhs)
	{
		<Module>.__ExceptionPtrCopy(A_0, _Rhs);
		return A_0;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000101C File Offset: 0x0000041C
	internal unsafe static void* @new(uint __unnamed000, void* _Where)
	{
		return _Where;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000102C File Offset: 0x0000042C
	internal unsafe static void delete(void* A_0, void* A_1)
	{
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002C4C File Offset: 0x0000204C
	internal unsafe static uint length(sbyte* _First)
	{
		uint result;
		if (*(sbyte*)_First == 0)
		{
			result = 0U;
		}
		else
		{
			sbyte* ptr = _First;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			result = (uint)(ptr - _First);
		}
		return result;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000103C File Offset: 0x0000043C
	internal unsafe static sbyte* copy(sbyte* _First1, sbyte* _First2, uint _Count)
	{
		sbyte* result;
		if (_Count == 0U)
		{
			result = _First1;
		}
		else
		{
			cpblk(_First1, _First2, _Count);
			result = _First1;
		}
		return result;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00001058 File Offset: 0x00000458
	internal unsafe static sbyte* move(sbyte* _First1, sbyte* _First2, uint _Count)
	{
		return (_Count != 0U) ? <Module>.memmove((void*)_First1, (void*)_First2, _Count) : _First1;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00001074 File Offset: 0x00000474
	internal unsafe static sbyte* assign(sbyte* _First, uint _Count, sbyte _Ch)
	{
		initblk(_First, _Ch, _Count);
		return _First;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0000108C File Offset: 0x0000048C
	internal unsafe static void assign(sbyte* _Left, sbyte* _Right)
	{
		*_Left = (byte)(*_Right);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00001F6C File Offset: 0x0000136C
	internal unsafe static _Iterator_base12* {ctor}(_Iterator_base12* A_0, _Iterator_base12* _Right)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		<Module>.std._Iterator_base12.=(A_0, _Right);
		return A_0;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00001D58 File Offset: 0x00001158
	internal unsafe static _Iterator_base12* =(_Iterator_base12* A_0, _Iterator_base12* _Right)
	{
		uint num = (uint)(*_Right);
		if (*A_0 != (int)num)
		{
			if (num != 0U)
			{
				_Container_base12* ptr = *num;
				if (ptr == null)
				{
					_Lockit lockit;
					<Module>.std._Lockit.{ctor}(ref lockit, 3);
					<Module>.std._Lockit.{dtor}(ref lockit);
				}
				else
				{
					*A_0 = *(int*)ptr;
				}
			}
			else
			{
				_Lockit lockit2;
				<Module>.std._Lockit.{ctor}(ref lockit2, 3);
				<Module>.std._Lockit.{dtor}(ref lockit2);
			}
		}
		return A_0;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00001878 File Offset: 0x00000C78
	internal unsafe static void _Adopt(_Iterator_base12* A_0, _Container_base12* _Parent)
	{
		if (_Parent == null)
		{
			_Lockit lockit;
			<Module>.std._Lockit.{ctor}(ref lockit, 3);
			<Module>.std._Lockit.{dtor}(ref lockit);
		}
		else
		{
			*A_0 = *(int*)_Parent;
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000010A0 File Offset: 0x000004A0
	internal unsafe static void _Orphan_me(_Iterator_base12* A_0)
	{
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000010B0 File Offset: 0x000004B0
	internal unsafe static allocator<void>* {ctor}(allocator<void>* A_0, allocator<void>* A_0)
	{
		return A_0;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002950 File Offset: 0x00001D50
	internal unsafe static void* __vecDelDtor(runtime_error* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			runtime_error* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 12U, *ptr, ldftn(std.runtime_error.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.std.exception.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000010C4 File Offset: 0x000004C4
	internal unsafe static void {dtor}(runtime_error* A_0)
	{
		<Module>.std.exception.{dtor}(A_0);
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000010D8 File Offset: 0x000004D8
	internal unsafe static runtime_error* {ctor}(runtime_error* A_0, runtime_error* A_0)
	{
		<Module>.std.exception.{ctor}(A_0, A_0);
		try
		{
			*A_0 = ref <Module>.??_7runtime_error@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.exception.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00001128 File Offset: 0x00000528
	internal unsafe static locale* {ctor}(locale* A_0, locale* _Right)
	{
		int num = *_Right;
		*A_0 = num;
		int num2 = num;
		calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 4));
		return A_0;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00001150 File Offset: 0x00000550
	internal unsafe static error_category* {ctor}(error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		return A_0;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00001168 File Offset: 0x00000568
	internal unsafe static void {dtor}(error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002A48 File Offset: 0x00001E48
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(error_category* A_0, error_category* _Right)
	{
		return (A_0 == _Right) ? 1 : 0;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000029A4 File Offset: 0x00001DA4
	internal unsafe static void* __vecDelDtor(error_category* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			error_category* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 4U, *ptr, ldftn(std.error_category.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002A5C File Offset: 0x00001E5C
	internal unsafe static int value(error_code* A_0)
	{
		return *A_0;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002A6C File Offset: 0x00001E6C
	internal unsafe static error_category* category(error_code* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002A08 File Offset: 0x00001E08
	internal unsafe static error_condition* {ctor}(error_condition* A_0, int _Val, error_category* _Cat)
	{
		*A_0 = _Val;
		*(A_0 + 4) = _Cat;
		return A_0;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002AD0 File Offset: 0x00001ED0
	internal unsafe static int value(error_condition* A_0)
	{
		return *A_0;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002AE0 File Offset: 0x00001EE0
	internal unsafe static error_category* category(error_condition* A_0)
	{
		return *(A_0 + 4);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002AA8 File Offset: 0x00001EA8
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool ==(error_condition* A_0, error_condition* _Right)
	{
		int num;
		if (((*(A_0 + 4) == *(_Right + 4)) ? 1 : 0) != 0 && *A_0 == *_Right)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000029F0 File Offset: 0x00001DF0
	internal unsafe static error_condition* default_error_condition(error_category* A_0, error_condition* A_1, int _Errval)
	{
		*(int*)A_1 = _Errval;
		*(int*)(A_1 + 4 / sizeof(error_condition)) = A_0;
		return A_1;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002A84 File Offset: 0x00001E84
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool equivalent(error_category* A_0, int _Errval, error_condition* _Cond)
	{
		error_condition error_condition;
		return <Module>.std.error_condition.==(calli(std.error_condition* modreq(System.Runtime.CompilerServices.IsUdtReturn) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,std.error_condition*,System.Int32), A_0, ref error_condition, _Errval, *(*A_0 + 12)), _Cond);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002A24 File Offset: 0x00001E24
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool equivalent(error_category* A_0, error_code* _Code, int _Errval)
	{
		int num;
		if (((A_0 == *(_Code + 4)) ? 1 : 0) != 0 && *_Code == _Errval)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002FA8 File Offset: 0x000023A8
	internal unsafe static void* __vecDelDtor(system_error* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			system_error* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 20U, *ptr, ldftn(std.system_error.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.std.exception.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0000117C File Offset: 0x0000057C
	internal unsafe static void {dtor}(system_error* A_0)
	{
		<Module>.std.exception.{dtor}(A_0);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00001190 File Offset: 0x00000590
	internal unsafe static _Generic_error_category* {ctor}(_Generic_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		try
		{
			*A_0 = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002B5C File Offset: 0x00001F5C
	internal unsafe static sbyte* name(_Generic_error_category* A_0)
	{
		return ref <Module>.??_C@_07DCLBNMLN@generic?$AA@;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002B74 File Offset: 0x00001F74
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* message(_Generic_error_category* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1, int _Errcode)
	{
		uint num = 0U;
		sbyte* ptr = <Module>.std._Syserror_map(_Errcode);
		sbyte* ptr2 = (ptr != null) ? ptr : ((sbyte*)(&<Module>.??_C@_0O@BFJCFAAK@unknown?5error?$AA@));
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1, ptr2);
		try
		{
			num = 1U;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002B0C File Offset: 0x00001F0C
	internal unsafe static void* __vecDelDtor(_Generic_error_category* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			_Generic_error_category* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 4U, *ptr, ldftn(std._Generic_error_category.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000011D8 File Offset: 0x000005D8
	internal unsafe static void {dtor}(_Generic_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000011EC File Offset: 0x000005EC
	internal unsafe static _Iostream_error_category* {ctor}(_Iostream_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		try
		{
			*A_0 = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), A_0);
			throw;
		}
		try
		{
			*A_0 = ref <Module>.??_7_Iostream_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002DA8 File Offset: 0x000021A8
	internal unsafe static sbyte* name(_Iostream_error_category* A_0)
	{
		return ref <Module>.??_C@_08LLGCOLLL@iostream?$AA@;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002DC0 File Offset: 0x000021C0
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* message(_Iostream_error_category* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1, int _Errcode)
	{
		uint num = 0U;
		if (_Errcode == 1)
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1, (sbyte*)(&<Module>.??_C@_0BG@PADBLCHM@iostream?5stream?5error?$AA@));
			try
			{
				num = 1U;
				return A_1;
			}
			catch
			{
				if ((num & 1U) != 0U)
				{
					num &= 4294967294U;
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
				}
				throw;
			}
		}
		<Module>.std._Generic_error_category.message(A_0, A_1, _Errcode);
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* result;
		try
		{
			num = 1U;
			result = A_1;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
			}
			throw;
		}
		return result;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002D58 File Offset: 0x00002158
	internal unsafe static void* __vecDelDtor(_Iostream_error_category* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			_Iostream_error_category* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 4U, *ptr, ldftn(std._Iostream_error_category.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00001264 File Offset: 0x00000664
	internal unsafe static void {dtor}(_Iostream_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00001278 File Offset: 0x00000678
	internal unsafe static _System_error_category* {ctor}(_System_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		try
		{
			*A_0 = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), A_0);
			throw;
		}
		try
		{
			*A_0 = ref <Module>.??_7_System_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002EBC File Offset: 0x000022BC
	internal unsafe static sbyte* name(_System_error_category* A_0)
	{
		return ref <Module>.??_C@_06FHFOAHML@system?$AA@;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002ED4 File Offset: 0x000022D4
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* message(_System_error_category* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1, int _Errcode)
	{
		uint num = 0U;
		sbyte* ptr = <Module>.std._Winerror_map(_Errcode);
		sbyte* ptr2 = (ptr != null) ? ptr : ((sbyte*)(&<Module>.??_C@_0O@BFJCFAAK@unknown?5error?$AA@));
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1, ptr2);
		try
		{
			num = 1U;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002F44 File Offset: 0x00002344
	internal unsafe static error_condition* default_error_condition(_System_error_category* A_0, error_condition* A_1, int _Errval)
	{
		if (<Module>.std._Syserror_map(_Errval) != null)
		{
			*(int*)A_1 = _Errval;
			*(int*)(A_1 + 4 / sizeof(error_condition)) = ref <Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A;
			return A_1;
		}
		*(int*)A_1 = _Errval;
		*(int*)(A_1 + 4 / sizeof(error_condition)) = ref <Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A;
		return A_1;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002E6C File Offset: 0x0000226C
	internal unsafe static void* __vecDelDtor(_System_error_category* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			_System_error_category* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 4U, *ptr, ldftn(std._System_error_category.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000012F0 File Offset: 0x000006F0
	internal unsafe static void {dtor}(_System_error_category* A_0)
	{
		*A_0 = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002F74 File Offset: 0x00002374
	internal unsafe static error_category* generic_category()
	{
		return ref <Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002F88 File Offset: 0x00002388
	internal unsafe static error_category* system_category()
	{
		return ref <Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000030A0 File Offset: 0x000024A0
	internal unsafe static void* __vecDelDtor(ios_base.failure* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			ios_base.failure* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 20U, *ptr, ldftn(std.ios_base.failure.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.std.exception.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x000030E8 File Offset: 0x000024E8
	internal unsafe static void {dtor}(ios_base.failure* A_0)
	{
		<Module>.std.exception.{dtor}(A_0);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000018A0 File Offset: 0x00000CA0
	internal unsafe static ios_base.failure* {ctor}(ios_base.failure* A_0, ios_base.failure* A_0)
	{
		<Module>.std.exception.{ctor}(A_0, A_0);
		try
		{
			*A_0 = ref <Module>.??_7runtime_error@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.exception.{dtor}), A_0);
			throw;
		}
		try
		{
			*A_0 = ref <Module>.??_7system_error@std@@6B@;
			cpblk(A_0 + 12, A_0 + 12, 8);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.runtime_error.{dtor}), A_0);
			throw;
		}
		try
		{
			*A_0 = ref <Module>.??_7failure@ios_base@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.system_error.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00001304 File Offset: 0x00000704
	internal unsafe static system_error* {ctor}(system_error* A_0, system_error* A_0)
	{
		<Module>.std.exception.{ctor}(A_0, A_0);
		try
		{
			*A_0 = ref <Module>.??_7runtime_error@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.exception.{dtor}), A_0);
			throw;
		}
		try
		{
			*A_0 = ref <Module>.??_7system_error@std@@6B@;
			cpblk(A_0 + 12, A_0 + 12, 8);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.runtime_error.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00001390 File Offset: 0x00000790
	internal static ref char PtrToStringChars(string s)
	{
		ref byte ptr = s;
		if (ref ptr != null)
		{
			ptr = RuntimeHelpers.OffsetToStringData + ref ptr;
		}
		return ref ptr;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000013AC File Offset: 0x000007AC
	internal static uint GetAnsiStringSize(string _str)
	{
		ref byte ptr = _str;
		if (ref ptr != null)
		{
			ptr = RuntimeHelpers.OffsetToStringData + ref ptr;
		}
		ref char char_u0020modopt(IsConst)& = ref ptr;
		uint num = <Module>.WideCharToMultiByte(3U, 1024, ref char_u0020modopt(IsConst)&, _str.Length, null, 0, null, null);
		if (num == 0U && _str.Length != 0)
		{
			throw new ArgumentException("Conversion from WideChar to MultiByte failed.  Please check the content of the string and/or locale settings.");
		}
		return num + 1U;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000013F8 File Offset: 0x000007F8
	internal unsafe static void WriteAnsiString(sbyte* _buf, uint _size, string _str)
	{
		ref byte ptr = _str;
		if (ref ptr != null)
		{
			ptr = RuntimeHelpers.OffsetToStringData + ref ptr;
		}
		ref char char_u0020modopt(IsConst)& = ref ptr;
		if (_size > 2147483647U)
		{
			throw new ArgumentOutOfRangeException("Size of string exceeds INT_MAX.");
		}
		uint num = <Module>.WideCharToMultiByte(3U, 1024, ref char_u0020modopt(IsConst)&, _str.Length, _buf, (int)_size, null, null);
		if (num < _size && (num != 0U || _size == 1U))
		{
			*(byte*)(num / (uint)sizeof(sbyte) + _buf) = 0;
			return;
		}
		throw new ArgumentException("Conversion from WideChar to MultiByte failed.  Please check the content of the string and/or locale settings.");
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000145C File Offset: 0x0000085C
	internal unsafe static uint GetUnicodeStringSize(sbyte* _str, uint _count)
	{
		if (_count > 2147483647U)
		{
			throw new ArgumentOutOfRangeException("Size of string exceeds INT_MAX.");
		}
		uint num = <Module>.MultiByteToWideChar(3U, 0, _str, (int)_count, null, 0);
		if (num == 0U && _count != 0U)
		{
			throw new ArgumentException("Conversion from MultiByte to WideChar failed.  Please check the content of the string and/or locale settings.");
		}
		return num + 1U;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000149C File Offset: 0x0000089C
	internal unsafe static void WriteUnicodeString(char* _dest, uint _size, sbyte* _src, uint _count)
	{
		if (_size > 2147483647U || _count > 2147483647U)
		{
			throw new ArgumentOutOfRangeException("Size of string exceeds INT_MAX.");
		}
		uint num = <Module>.MultiByteToWideChar(3U, 0, _src, (int)_count, _dest, (int)_size);
		if (num < _size && (num != 0U || _size == 1U))
		{
			num[_dest] = '\0';
			return;
		}
		throw new ArgumentException("Conversion from MultiByte to WideChar failed.  Please check the content of the string and/or locale settings.");
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000195C File Offset: 0x00000D5C
	internal unsafe static string InternalAnsiToStringHelper(sbyte* _src, uint _count)
	{
		uint num = <Module>.msclr.interop.details.GetUnicodeStringSize(_src, _count);
		if (num - 1U <= 2147483646U)
		{
			char_buffer<wchar_t> char_buffer<wchar_t> = <Module>.new[](num << 1);
			string result;
			try
			{
				if (char_buffer<wchar_t> == null)
				{
					throw new InsufficientMemoryException();
				}
				<Module>.msclr.interop.details.WriteUnicodeString(char_buffer<wchar_t>, num, _src, _count);
				result = new string(char_buffer<wchar_t>, 0, (int)(num - 1U));
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(msclr.interop.details.char_buffer<wchar_t>.{dtor}), (void*)(&char_buffer<wchar_t>));
				throw;
			}
			<Module>.delete[](char_buffer<wchar_t>);
			return result;
		}
		throw new ArgumentOutOfRangeException("Size of string exceeds INT_MAX.");
		try
		{
		}
		catch
		{
			char_buffer<wchar_t> char_buffer<wchar_t>;
			<Module>.___CxxCallUnwindDtor(ldftn(msclr.interop.details.char_buffer<wchar_t>.{dtor}), (void*)(&char_buffer<wchar_t>));
			throw;
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00001F90 File Offset: 0x00001390
	internal unsafe static string marshal_as<class\u0020System::String\u0020^,class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _from_obj)
	{
		uint count = (uint)(*(_from_obj + 16));
		return <Module>.msclr.interop.details.InternalAnsiToStringHelper((16 > *(_from_obj + 20)) ? _from_obj : (*_from_obj), count);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002648 File Offset: 0x00001A48
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* marshal_as<class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>,class\u0020System::String\u0020^>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, string* _from_obj)
	{
		try
		{
			uint num = 0U;
			if (*_from_obj == null)
			{
				throw new ArgumentNullException("NULLPTR is not supported for this conversion.");
			}
			*(int*)(A_0 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>);
			*(int*)ptr = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
			num = 1U;
			uint num2 = <Module>.msclr.interop.details.GetAnsiStringSize(*_from_obj);
			if (num2 > 1U)
			{
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.resize(A_0, num2 - 1U);
				<Module>.msclr.interop.details.WriteAnsiString((sbyte*)((16 > *(int*)ptr) ? A_0 : (*(int*)A_0)), num2, *_from_obj);
			}
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_0);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000023E0 File Offset: 0x000017E0
	internal unsafe static void {dtor}(FfuReaderResult* A_0)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 4, true, 0U);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002568 File Offset: 0x00001968
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Right, 0U, uint.MaxValue);
		return A_0;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002294 File Offset: 0x00001694
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		return A_0;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00002BDC File Offset: 0x00001FDC
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Ptr, count);
		return A_0;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000023F8 File Offset: 0x000017F8
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_0, _Right);
		return A_0;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000022B8 File Offset: 0x000016B8
	internal unsafe static void {dtor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00001DD8 File Offset: 0x000011D8
	internal unsafe static sbyte* [](basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Off)
	{
		return ((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00001DF8 File Offset: 0x000011F8
	internal unsafe static sbyte* c_str(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		return (16 > *(A_0 + 20)) ? A_0 : (*A_0);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00001654 File Offset: 0x00000A54
	internal unsafe static uint length(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		return *(A_0 + 16);
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0000260C File Offset: 0x00001A0C
	internal unsafe static void resize(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newsize)
	{
		uint num = (uint)(*(A_0 + 16));
		if (_Newsize <= num)
		{
			*(A_0 + 16) = (int)_Newsize;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Newsize) = 0;
		}
		else
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Newsize - num, 0);
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00001668 File Offset: 0x00000A68
	internal unsafe static char_buffer<wchar_t>* {ctor}(char_buffer<wchar_t>* A_0, uint _size)
	{
		uint num;
		if (_size <= 2147483647U)
		{
			num = _size << 1;
		}
		else
		{
			num = uint.MaxValue;
		}
		*A_0 = <Module>.new[](num);
		return A_0;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00001690 File Offset: 0x00000A90
	internal unsafe static void {dtor}(char_buffer<wchar_t>* A_0)
	{
		<Module>.delete[](*A_0);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000016A4 File Offset: 0x00000AA4
	internal unsafe static char* get(char_buffer<wchar_t>* A_0)
	{
		return *A_0;
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000016B4 File Offset: 0x00000AB4
	internal unsafe static char_buffer<char>* {ctor}(char_buffer<char>* A_0, uint _size)
	{
		*A_0 = <Module>.new[](_size);
		return A_0;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000016CC File Offset: 0x00000ACC
	internal unsafe static void {dtor}(char_buffer<char>* A_0)
	{
		<Module>.delete[](*A_0);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000016E0 File Offset: 0x00000AE0
	internal unsafe static sbyte* get(char_buffer<char>* A_0)
	{
		return *A_0;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x000016F0 File Offset: 0x00000AF0
	internal unsafe static sbyte* release(char_buffer<char>* A_0)
	{
		sbyte* result = *A_0;
		*A_0 = 0;
		return result;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000022D0 File Offset: 0x000016D0
	internal unsafe static void _Assign_rv(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		if (*(_Right + 20) < 16)
		{
			uint num = (uint)(*(_Right + 16) + 1);
			if (num != 0U)
			{
				<Module>.memmove(A_0, _Right, num);
			}
		}
		else
		{
			try
			{
				if (A_0 != null)
				{
					*A_0 = *_Right;
				}
			}
			catch
			{
				<Module>.delete(A_0, A_0);
				throw;
			}
			*_Right = 0;
		}
		*(A_0 + 16) = *(_Right + 16);
		*(A_0 + 20) = *(_Right + 20);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(_Right, false, 0U);
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00002424 File Offset: 0x00001824
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right, uint _Roff, uint _Count)
	{
		uint num = (uint)(*(_Right + 16));
		if (num < _Roff)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
		}
		uint num2 = num - _Roff;
		num2 = ((_Count < num2) ? _Count : num2);
		if (A_0 == _Right)
		{
			if (*(A_0 + 16) < (int)(_Roff + num2))
			{
				<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
			}
			*(A_0 + 16) = (int)(_Roff + num2);
			sbyte* ptr;
			if (16 <= *(A_0 + 20))
			{
				ptr = *A_0;
			}
			else
			{
				ptr = A_0;
			}
			*(byte*)((_Roff + num2) / (uint)sizeof(sbyte) + ptr) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.erase(A_0, 0U, _Roff);
		}
		else if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, num2, false) != null)
		{
			sbyte* ptr2;
			if (16 <= *(_Right + 20))
			{
				ptr2 = *_Right;
			}
			else
			{
				ptr2 = _Right;
			}
			sbyte* ptr3;
			if (16 <= *(A_0 + 20))
			{
				ptr3 = *A_0;
			}
			else
			{
				ptr3 = A_0;
			}
			if (num2 != 0U)
			{
				cpblk(ptr3, ptr2 + _Roff / (uint)sizeof(sbyte), num2);
			}
			*(A_0 + 16) = (int)num2;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + num2) = 0;
		}
		return A_0;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00002C20 File Offset: 0x00002020
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Ptr, count);
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00001704 File Offset: 0x00000B04
	internal unsafe static uint size(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		return *(A_0 + 16);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0000259C File Offset: 0x0000199C
	internal unsafe static void resize(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newsize, sbyte _Ch)
	{
		uint num = (uint)(*(A_0 + 16));
		if (_Newsize <= num)
		{
			*(A_0 + 16) = (int)_Newsize;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Newsize) = 0;
		}
		else
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Newsize - num, _Ch);
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00001FB8 File Offset: 0x000013B8
	internal unsafe static void _Tidy(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, [MarshalAs(UnmanagedType.U1)] bool _Built, uint _Newsize)
	{
		if (_Built && 16 <= *(A_0 + 20))
		{
			sbyte* ptr = *A_0;
			if (0U < _Newsize && _Newsize != 0U)
			{
				cpblk(A_0, ptr, _Newsize);
			}
			<Module>.delete((void*)ptr);
		}
		*(A_0 + 20) = 15;
		*(A_0 + 16) = (int)_Newsize;
		*(_Newsize + A_0) = 0;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00001718 File Offset: 0x00000B18
	internal unsafe static allocator<char>* {ctor}(allocator<char>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00001BD4 File Offset: 0x00000FD4
	internal unsafe static _String_alloc<0,std::_String_base_types<char,std::allocator<char>\u0020>\u0020>* {ctor}(_String_alloc<0,std::_String_base_types<char,std::allocator<char>\u0020>\u0020>* A_0, allocator<char>* A_0)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		return A_0;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00001BF0 File Offset: 0x00000FF0
	internal unsafe static _Wrap_alloc<std::allocator<char>\u0020>* _Getal(_String_alloc<0,std::_String_base_types<char,std::allocator<char>\u0020>\u0020>* A_0, _Wrap_alloc<std::allocator<char>\u0020>* A_1)
	{
		return A_1;
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00001FF8 File Offset: 0x000013F8
	internal unsafe static _Wrap_alloc<std::allocator<char>\u0020>* select_on_container_copy_construction(_Wrap_alloc<std::allocator<char>\u0020>* A_0, _Wrap_alloc<std::allocator<char>\u0020>* A_1)
	{
		return A_1;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00001C00 File Offset: 0x00001000
	internal unsafe static sbyte* _Myptr(_String_val<std::_Simple_types<char>\u0020>* A_0)
	{
		return (16 > *(A_0 + 20)) ? A_0 : (*A_0);
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00001C1C File Offset: 0x0000101C
	internal unsafe static sbyte* _Myptr(_String_val<std::_Simple_types<char>\u0020>* A_0)
	{
		return (16 > *(A_0 + 20)) ? A_0 : (*A_0);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x000024E4 File Offset: 0x000018E4
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* append(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Count, sbyte _Ch)
	{
		uint num = (uint)(*(A_0 + 16));
		if (4294967295U - num <= _Count)
		{
			<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
		}
		if (0U < _Count)
		{
			uint num2 = num + _Count;
			if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, num2, false) != null)
			{
				uint num3 = (uint)(*(A_0 + 16));
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr;
				if (_Count == 1U)
				{
					ptr = A_0 + 20;
					*(((16 > *ptr) ? A_0 : (*A_0)) + num3) = _Ch;
				}
				else
				{
					ptr = A_0 + 20;
					initblk(((16 > *ptr) ? A_0 : (*A_0)) + num3, _Ch, _Count);
				}
				*(A_0 + 16) = (int)num2;
				*(((16 > *ptr) ? A_0 : (*A_0)) + num2) = 0;
			}
		}
		return A_0;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00002C70 File Offset: 0x00002070
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr, uint _Count)
	{
		if (_Ptr != null)
		{
			uint num = (uint)(*(A_0 + 20));
			sbyte* ptr;
			if (16U <= num)
			{
				ptr = *A_0;
			}
			else
			{
				ptr = A_0;
			}
			if (_Ptr >= (sbyte*)ptr)
			{
				sbyte* ptr2;
				if (16U <= num)
				{
					ptr2 = *A_0;
				}
				else
				{
					ptr2 = A_0;
				}
				if (*(A_0 + 16) / sizeof(sbyte) + ptr2 != (sbyte*)_Ptr)
				{
					sbyte* ptr3;
					if (16U <= num)
					{
						ptr3 = *A_0;
					}
					else
					{
						ptr3 = A_0;
					}
					return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, A_0, (uint)(_Ptr - ptr3), _Count);
				}
			}
		}
		if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, _Count, false) != null)
		{
			sbyte* ptr4;
			if (16 <= *(A_0 + 20))
			{
				ptr4 = *A_0;
			}
			else
			{
				ptr4 = A_0;
			}
			if (_Count != 0U)
			{
				cpblk(ptr4, _Ptr, _Count);
			}
			*(A_0 + 16) = (int)_Count;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Count) = 0;
		}
		return A_0;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00001E14 File Offset: 0x00001214
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* erase(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Off)
	{
		if (*(A_0 + 16) < (int)_Off)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
		}
		*(A_0 + 16) = (int)_Off;
		*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off) = 0;
		return A_0;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00001E4C File Offset: 0x0000124C
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* erase(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Off, uint _Count)
	{
		uint num = (uint)(*(A_0 + 16));
		if (num < _Off)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
		}
		if (num - _Off <= _Count)
		{
			*(A_0 + 16) = (int)_Off;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off) = 0;
		}
		else if (0U < _Count)
		{
			sbyte* ptr = ((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off / (uint)sizeof(sbyte);
			uint num2 = num - _Count;
			uint num3 = num2 - _Off;
			if (num3 != 0U)
			{
				sbyte* ptr2 = ptr;
				<Module>.memmove((void*)ptr2, (void*)(ptr2 + _Count / (uint)sizeof(sbyte)), num3);
			}
			*(A_0 + 16) = (int)num2;
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + num2) = 0;
		}
		return A_0;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00001C38 File Offset: 0x00001038
	internal unsafe static void _Eos(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newsize)
	{
		*(A_0 + 16) = (int)_Newsize;
		*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Newsize) = 0;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000234C File Offset: 0x0000174C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Grow(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newsize, [MarshalAs(UnmanagedType.U1)] bool _Trim)
	{
		if (4294967294U < _Newsize)
		{
			<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
		}
		uint num = (uint)(*(A_0 + 20));
		if (num < _Newsize)
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Copy(A_0, _Newsize, (uint)(*(A_0 + 16)));
		}
		else if (_Trim && _Newsize < 16U)
		{
			uint num2 = (uint)(*(A_0 + 16));
			uint newsize = (_Newsize < num2) ? _Newsize : num2;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, newsize);
		}
		else if (_Newsize == 0U)
		{
			*(A_0 + 16) = 0;
			*((16U > num) ? A_0 : (*A_0)) = 0;
			goto IL_67;
		}
		int num3;
		if (0U < _Newsize)
		{
			num3 = 1;
			goto IL_69;
		}
		IL_67:
		num3 = 0;
		IL_69:
		return (byte)num3;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00001728 File Offset: 0x00000B28
	internal unsafe static void _Xran(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00001740 File Offset: 0x00000B40
	internal unsafe static _Wrap_alloc<std::allocator<char>\u0020>* {ctor}(_Wrap_alloc<std::allocator<char>\u0020>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00001C60 File Offset: 0x00001060
	internal unsafe static _Wrap_alloc<std::allocator<char>\u0020>* {ctor}(_Wrap_alloc<std::allocator<char>\u0020>* A_0, allocator<char>* _Right)
	{
		return A_0;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00001C70 File Offset: 0x00001070
	internal unsafe static void deallocate(_Wrap_alloc<std::allocator<char>\u0020>* A_0, sbyte* _Ptr, uint _Count)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00001ED4 File Offset: 0x000012D4
	internal unsafe static allocator<char>* select_on_container_copy_construction(allocator<char>* A_0, allocator<char>* _Al)
	{
		return A_0;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00001750 File Offset: 0x00000B50
	internal unsafe static _String_val<std::_Simple_types<char>\u0020>* {ctor}(_String_val<std::_Simple_types<char>\u0020>* A_0)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		return A_0;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00002008 File Offset: 0x00001408
	internal unsafe static uint max_size(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		return -2;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00001C84 File Offset: 0x00001084
	internal unsafe static void _Chassign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Off, uint _Count, sbyte _Ch)
	{
		if (_Count == 1U)
		{
			*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off) = _Ch;
		}
		else
		{
			initblk(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + _Off, _Ch, _Count);
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00002018 File Offset: 0x00001418
	internal unsafe static void _Copy(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newsize, uint _Oldlen)
	{
		uint num = <Module>.__CxxQueryExceptionSize();
		int num2 = (int)stackalloc byte[num << 1];
		uint num3 = _Newsize | 15U;
		if (4294967294U < num3)
		{
			num3 = _Newsize;
		}
		else
		{
			uint num4 = (uint)(*(A_0 + 20));
			uint num5 = num4 >> 1;
			if (num5 > num3 / 3U)
			{
				if (num4 <= 4294967294U - num5)
				{
					num3 = num5 + num4;
				}
				else
				{
					num3 = 4294967294U;
				}
			}
		}
		sbyte* ptr2;
		uint exceptionCode;
		try
		{
			int num6 = (int)(num + (uint)num2);
			uint num7 = num3 + 1U;
			void* ptr = null;
			if (num7 != 0U)
			{
				if (4294967295U >= num7)
				{
					ptr = <Module>.@new(num7);
					if (ptr != null)
					{
						goto IL_68;
					}
				}
				<Module>.std._Xbad_alloc();
				goto IL_169;
			}
			IL_68:
			ptr2 = (sbyte*)ptr;
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num8 = 0U;
			int num6;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num6);
			try
			{
				try
				{
					num3 = _Newsize;
					uint exceptionCode2;
					try
					{
						uint num9 = _Newsize + 1U;
						void* ptr3 = null;
						if (num9 != 0U)
						{
							if (4294967295U >= num9)
							{
								ptr3 = <Module>.@new(num9);
								if (ptr3 != null)
								{
									goto IL_CE;
								}
							}
							<Module>.std._Xbad_alloc();
							goto IL_13F;
						}
						IL_CE:
						ptr2 = (sbyte*)ptr3;
					}
					catch when (delegate
					{
						// Failed to create a 'catch-when' expression
						exceptionCode2 = (uint)Marshal.GetExceptionCode();
						endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
					})
					{
						uint num10 = 0U;
						<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num2);
						try
						{
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
								<Module>._CxxThrowException(null, null);
							}
							catch when (delegate
							{
								// Failed to create a 'catch-when' expression
								num10 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
								endfilter(num10 != 0U);
							})
							{
							}
							if (num10 != 0U)
							{
								throw;
							}
						}
						finally
						{
							<Module>.__CxxUnregisterExceptionObject(num2, (int)num10);
						}
					}
					IL_13F:
					goto IL_169;
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num8 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num8 != 0U);
				})
				{
				}
				if (num8 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num6, (int)num8);
			}
		}
		IL_169:
		if (0U < _Oldlen)
		{
			sbyte* ptr4;
			if (16 <= *(A_0 + 20))
			{
				ptr4 = *A_0;
			}
			else
			{
				ptr4 = A_0;
			}
			if (_Oldlen != 0U)
			{
				cpblk(ptr2, ptr4, _Oldlen);
			}
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
		try
		{
			if (A_0 != null)
			{
				*A_0 = ptr2;
			}
		}
		catch
		{
			<Module>.delete(A_0, A_0);
			throw;
		}
		*(A_0 + 20) = (int)num3;
		*(A_0 + 16) = (int)_Oldlen;
		*(((16U > num3) ? A_0 : (*A_0)) + _Oldlen) = 0;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00002D04 File Offset: 0x00002104
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Inside(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		if (_Ptr != null)
		{
			uint num = (uint)(*(A_0 + 20));
			sbyte* ptr;
			if (16U <= num)
			{
				ptr = *A_0;
			}
			else
			{
				ptr = A_0;
			}
			if (_Ptr >= (sbyte*)ptr)
			{
				sbyte* ptr2;
				if (16U <= num)
				{
					ptr2 = *A_0;
				}
				else
				{
					ptr2 = A_0;
				}
				if (*(A_0 + 16) / sizeof(sbyte) + ptr2 != (sbyte*)_Ptr)
				{
					return 1;
				}
			}
		}
		return 0;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000176C File Offset: 0x00000B6C
	internal unsafe static void _Xlen(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00001CC0 File Offset: 0x000010C0
	internal unsafe static allocator<char>* select_on_container_copy_construction(allocator<char>* A_0, allocator<char>* A_1)
	{
		return A_1;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00001784 File Offset: 0x00000B84
	internal unsafe static allocator<char>* {ctor}(allocator<char>* A_0, allocator<char>* A_0)
	{
		return A_0;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00001798 File Offset: 0x00000B98
	internal unsafe static void deallocate(allocator<char>* A_0, sbyte* _Ptr, uint __unnamed001)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x000017AC File Offset: 0x00000BAC
	internal unsafe static allocator<wchar_t>* {ctor}(allocator<wchar_t>* A_0, allocator<wchar_t>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00001EE4 File Offset: 0x000012E4
	internal unsafe static sbyte* allocate(_Wrap_alloc<std::allocator<char>\u0020>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (4294967295U >= _Count)
			{
				ptr = <Module>.@new(_Count);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00001F0C File Offset: 0x0000130C
	internal unsafe static uint max_size(_Wrap_alloc<std::allocator<char>\u0020>* A_0)
	{
		return -1;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00001CD0 File Offset: 0x000010D0
	internal unsafe static sbyte* allocate(allocator<char>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (4294967295U >= _Count)
			{
				ptr = <Module>.@new(_Count);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00001CF8 File Offset: 0x000010F8
	internal unsafe static uint max_size(allocator<char>* _Al)
	{
		return -1;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000017C0 File Offset: 0x00000BC0
	internal unsafe static uint max_size(allocator<char>* A_0)
	{
		return -1;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00011AA0 File Offset: 0x00010EA0
	internal unsafe static void ??__E?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0x8c080b19.??__F?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00011AF4 File Offset: 0x00010EF4
	internal unsafe static void ??__E?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A));
			throw;
		}
		try
		{
			<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7_Iostream_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), (void*)(&<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0x8c080b19.??__F?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00011B80 File Offset: 0x00010F80
	internal unsafe static void ??__E?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A));
			throw;
		}
		try
		{
			<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7_System_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), (void*)(&<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0x8c080b19.??__F?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000017D0 File Offset: 0x00000BD0
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* forward<class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Arg)
	{
		return _Arg;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00001F1C File Offset: 0x0000131C
	internal unsafe static void construct<char\u0020*,char\u0020*\u0020&>(_Wrap_alloc<std::allocator<char>\u0020>* A_0, sbyte** _Ptr, sbyte** _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				*(int*)_Ptr = *_V0;
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00001F5C File Offset: 0x0000135C
	internal unsafe static void destroy<char\u0020*>(_Wrap_alloc<std::allocator<char>\u0020>* A_0, sbyte** _Ptr)
	{
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000017E0 File Offset: 0x00000BE0
	internal unsafe static sbyte* addressof<char>(sbyte* _Val)
	{
		return _Val;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000017F0 File Offset: 0x00000BF0
	internal unsafe static sbyte* _Allocate<char>(uint _Count, sbyte* __unnamed001)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (4294967295U >= _Count)
			{
				ptr = <Module>.@new(_Count);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00011F40 File Offset: 0x00011340
	internal static void ??__F?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ()
	{
		<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00011F60 File Offset: 0x00011360
	internal static void ??__F?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ()
	{
		<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00011F80 File Offset: 0x00011380
	internal static void ??__F?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ()
	{
		<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00011C0C File Offset: 0x0001100C
	internal static void ??__E?id@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2V0locale@2@A@@YMXXZ()
	{
		<Module>.std.locale.id.{ctor}(ref <Module>.?id@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2V0locale@2@A, 0U);
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00011C28 File Offset: 0x00011028
	internal static void ??__E?id@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2V0locale@2@A@@YMXXZ()
	{
		<Module>.std.locale.id.{ctor}(ref <Module>.?id@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2V0locale@2@A, 0U);
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00001818 File Offset: 0x00000C18
	internal unsafe static sbyte** forward<char\u0020*\u0020&>(sbyte** _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00001D08 File Offset: 0x00001108
	internal unsafe static void construct<char\u0020*,char\u0020*\u0020&>(allocator<char>* _Al, sbyte** _Ptr, sbyte** _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				*(int*)_Ptr = *_V0;
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00001D48 File Offset: 0x00001148
	internal unsafe static void destroy<char\u0020*>(allocator<char>* _Al, sbyte** _Ptr)
	{
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00001828 File Offset: 0x00000C28
	internal unsafe static void construct<char\u0020*,char\u0020*\u0020&>(allocator<char>* A_0, sbyte** _Ptr, sbyte** _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				*(int*)_Ptr = *_V0;
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00001868 File Offset: 0x00000C68
	internal unsafe static void destroy<char\u0020*>(allocator<char>* A_0, sbyte** _Ptr)
	{
	}

	// Token: 0x06000088 RID: 136 RVA: 0x0000324C File Offset: 0x0000264C
	internal unsafe static int compare(sbyte* _First1, sbyte* _First2, uint _Count)
	{
		int result;
		if (_Count == 0U)
		{
			result = 0;
		}
		else
		{
			uint num = _Count;
			sbyte* ptr = _First2;
			int num2 = 0;
			byte b = *(byte*)_First1;
			byte b2 = *(byte*)_First2;
			if (b >= b2)
			{
				int num3 = (int)(_First1 - _First2);
				while (b <= b2)
				{
					if (num == 1U)
					{
						goto IL_44;
					}
					num -= 1U;
					ptr += 1 / sizeof(sbyte);
					b = *(byte*)(num3 / sizeof(sbyte) + ptr);
					b2 = *(byte*)ptr;
					if (b < b2)
					{
						goto IL_3E;
					}
				}
				num2 = 1;
				goto IL_44;
			}
			IL_3E:
			num2 = -1;
			IL_44:
			result = num2;
		}
		return result;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x0000D50C File Offset: 0x0000C90C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool eq(sbyte* _Left, sbyte* _Right)
	{
		return (*_Left == *_Right) ? 1 : 0;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0000D474 File Offset: 0x0000C874
	internal unsafe static sbyte to_char_type(int* _Meta)
	{
		return (sbyte)(*_Meta);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0000D5BC File Offset: 0x0000C9BC
	internal unsafe static int to_int_type(sbyte* _Ch)
	{
		return (byte)(*_Ch);
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000032A4 File Offset: 0x000026A4
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool eq_int_type(int* _Left, int* _Right)
	{
		return (*_Left == *_Right) ? 1 : 0;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0000D484 File Offset: 0x0000C884
	internal unsafe static int not_eof(int* _Meta)
	{
		int num = *_Meta;
		return (num != -1) ? num : 0;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000032B8 File Offset: 0x000026B8
	internal static int eof()
	{
		return -1;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x000032C8 File Offset: 0x000026C8
	internal unsafe static void _Adopt(_Iterator_base0* A_0, void* A_0)
	{
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000032D8 File Offset: 0x000026D8
	internal unsafe static void {dtor}(locale* A_0)
	{
		uint num = (uint)(*A_0);
		if (num != 0U)
		{
			uint num2 = num;
			_Facet_base* ptr = calli(std._Facet_base* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num2, *(*num2 + 8));
			if (ptr != null)
			{
				object obj = calli(System.Void* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt32), ptr, 1, *(*(int*)ptr));
			}
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00003308 File Offset: 0x00002708
	internal unsafe static locale.facet* _Getfacet(locale* A_0, uint _Id)
	{
		int num = *A_0;
		locale.facet* ptr;
		if (_Id < (uint)(*(num + 12)))
		{
			ptr = *(_Id * 4U + (uint)(*(num + 8)));
			if (ptr != null)
			{
				return ptr;
			}
		}
		else
		{
			ptr = null;
		}
		if (*(num + 20) != 0)
		{
			locale._Locimp* ptr2 = <Module>.std.locale._Getgloballocale();
			return (_Id >= (uint)(*(int*)(ptr2 + 12 / sizeof(locale._Locimp)))) ? 0 : (*(_Id * 4U + (uint)(*(int*)(ptr2 + 8 / sizeof(locale._Locimp)))));
		}
		return ptr;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00003358 File Offset: 0x00002758
	internal unsafe static ios_base* hex(ios_base* _Iosbase)
	{
		<Module>.std.ios_base.setf(_Iosbase, 2048, 3584);
		return _Iosbase;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x0000DF00 File Offset: 0x0000D300
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Fgetc<char>(sbyte* _Byte, _iobuf* _File)
	{
		int num = <Module>.fgetc(_File);
		if (num == -1)
		{
			return 0;
		}
		*_Byte = (byte)num;
		return 1;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0000DB78 File Offset: 0x0000CF78
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Fputc<char>(sbyte _Byte, _iobuf* _File)
	{
		return (<Module>.fputc(_Byte, _File) != -1) ? 1 : 0;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0000DC30 File Offset: 0x0000D030
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Ungetc<char>(sbyte* _Byte, _iobuf* _File)
	{
		return (<Module>.ungetc((int)((byte)(*_Byte)), _File) != -1) ? 1 : 0;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00003378 File Offset: 0x00002778
	internal unsafe static long time(long* _Time)
	{
		return <Module>._time64(_Time);
	}

	// Token: 0x06000097 RID: 151 RVA: 0x000056C0 File Offset: 0x00004AC0
	internal unsafe static void {dtor}(IFfuReader* A_0)
	{
		*A_0 = ref <Module>.??_7IFfuReader@@6B@;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 4, true, 0U);
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0000E414 File Offset: 0x0000D814
	internal unsafe static void* __vecDelDtor(IFfuReader* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			IFfuReader* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 28U, *ptr, ldftn(IFfuReader.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7IFfuReader@@6B@;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 4, true, 0U);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0000338C File Offset: 0x0000278C
	internal unsafe static FfuReader.WriteRequest* {ctor}(FfuReader.WriteRequest* A_0)
	{
		*A_0 = 0L;
		*(A_0 + 8) = 0;
		*(A_0 + 16) = 0L;
		*(A_0 + 24) = 0;
		*(A_0 + 32) = 0L;
		return A_0;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x000033B8 File Offset: 0x000027B8
	internal unsafe static FfuReader.BlockDataEntry* {ctor}(FfuReader.BlockDataEntry* A_0)
	{
		*A_0 = 0L;
		*(A_0 + 12) = 0;
		*(A_0 + 16) = 0L;
		return A_0;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x000056E0 File Offset: 0x00004AE0
	internal unsafe static FfuReader.partition* {ctor}(FfuReader.partition* A_0)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		try
		{
			*(A_0 + 56) = 0;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00011C60 File Offset: 0x00011060
	internal unsafe static void ??__E?A0xd5e524cb@IMAGE_HEADER_SIGNATURE@@YMXXZ()
	{
		sbyte* ptr = ref <Module>.??_C@_0N@EFJOMLHH@ImageFlash?5?5?$AA@;
		do
		{
			ptr++;
		}
		while (*ptr != 0);
		int count = ptr - ref <Module>.??_C@_0N@EFJOMLHH@ImageFlash?5?5?$AA@;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref <Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE, (sbyte*)(&<Module>.??_C@_0N@EFJOMLHH@ImageFlash?5?5?$AA@), (uint)count);
		<Module>._atexit_m(ldftn(?A0xd5e524cb.??__F?A0xd5e524cb@IMAGE_HEADER_SIGNATURE@@YMXXZ));
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00012000 File Offset: 0x00011400
	internal unsafe static void ??__F?A0xd5e524cb@IMAGE_HEADER_SIGNATURE@@YMXXZ()
	{
		if (16 <= *(ref <Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE + 20))
		{
			void* ptr = <Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE;
			_Wrap_alloc<std::allocator<char>\u0020> wrap_alloc<std::allocator<char>_u0020>;
			<Module>.std._Wrap_alloc<std::allocator<char>\u0020>.destroy<char\u0020*>(ref wrap_alloc<std::allocator<char>_u0020>, (sbyte**)(&<Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE));
			<Module>.delete(ptr);
		}
		*(ref <Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE + 20) = 15;
		*(ref <Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE + 16) = 0;
		<Module>.?A0xd5e524cb.IMAGE_HEADER_SIGNATURE = 0;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00005FF4 File Offset: 0x000053F4
	internal unsafe static void stringToLower(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* s)
	{
		sbyte* ptr = <Module>.new[]((uint)(*(s + 16) + 1));
		sbyte* ptr2;
		if (16 <= *(s + 20))
		{
			ptr2 = *s;
		}
		else
		{
			ptr2 = s;
		}
		sbyte* ptr3 = ptr2;
		int num = (int)(ptr - ptr2);
		sbyte b;
		do
		{
			b = *(sbyte*)ptr3;
			*(byte*)(num / sizeof(sbyte) + ptr3) = (byte)b;
			ptr3 += 1 / sizeof(sbyte);
		}
		while (b != 0);
		uint num2 = 0U;
		if (0 < *(s + 16))
		{
			do
			{
				*(byte*)(ptr + num2 / (uint)sizeof(sbyte)) = <Module>.tolower((int)(*(sbyte*)(ptr + num2 / (uint)sizeof(sbyte))));
				num2 += 1U;
			}
			while (num2 < (uint)(*(s + 16)));
		}
		uint count;
		if (*(sbyte*)ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr4 = ptr;
			do
			{
				ptr4 += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr4 != 0);
			count = (uint)(ptr4 - ptr);
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(s, (sbyte*)ptr, count);
		<Module>.delete[]((void*)ptr);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000A1FC File Offset: 0x000095FC
	internal unsafe static FfuReader* {ctor}(FfuReader* A_0)
	{
		*A_0 = ref <Module>.??_7IFfuReader@@6B@;
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 4;
		*(ptr + 16) = 0;
		*(ptr + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, false, 0U);
		try
		{
			*A_0 = ref <Module>.??_7FfuReader@@6B@;
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = A_0 + 28;
			*(ptr2 + 16) = 0;
			*(ptr2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2, false, 0U);
			try
			{
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = A_0 + 52;
				*(ptr3 + 16) = 0;
				*(ptr3 + 20) = 0;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr3, false, 0U);
				try
				{
					*(A_0 + 80) = 2013103101L;
					vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* ptr4 = A_0 + 92;
					*ptr4 = 0;
					*(ptr4 + 4) = 0;
					*(ptr4 + 8) = 0;
					try
					{
						vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* ptr5 = A_0 + 104;
						*ptr5 = 0;
						*(ptr5 + 4) = 0;
						*(ptr5 + 8) = 0;
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr6 = A_0 + 17012;
							*(ptr6 + 16) = 0;
							*(ptr6 + 20) = 0;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr6, false, 0U);
							try
							{
								*(A_0 + 17036) = 0;
								*(A_0 + 17232) = 0;
								*(A_0 + 17236) = 0;
								*(A_0 + 17240) = 0L;
								*(A_0 + 17248) = 0L;
								vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* ptr7 = A_0 + 17268;
								*ptr7 = 0;
								*(ptr7 + 4) = 0;
								*(ptr7 + 8) = 0;
								try
								{
									*(A_0 + 17280) = 0;
									*(A_0 + 18340) = 0;
									*(A_0 + 17264) = 0;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>.{dtor}), A_0 + 17268);
									throw;
								}
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 17012);
								throw;
							}
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>.{dtor}), A_0 + 104);
							throw;
						}
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>.{dtor}), A_0 + 92);
						throw;
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 52);
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 28);
				throw;
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(IFfuReader.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000E6A0 File Offset: 0x0000DAA0
	internal unsafe static void* __vecDelDtor(FfuReader* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			FfuReader* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 18344U, *ptr, ldftn(FfuReader.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.FfuReader.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00005734 File Offset: 0x00004B34
	internal unsafe static IFfuReader* {ctor}(IFfuReader* A_0)
	{
		*A_0 = ref <Module>.??_7IFfuReader@@6B@;
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 4;
		*(ptr + 16) = 0;
		*(ptr + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, false, 0U);
		return A_0;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x0000A450 File Offset: 0x00009850
	internal unsafe static void {dtor}(FfuReader* A_0)
	{
		*A_0 = ref <Module>.??_7FfuReader@@6B@;
		try
		{
			try
			{
				try
				{
					try
					{
						try
						{
							try
							{
								<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Tidy(A_0 + 17268);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 17012);
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 17012, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>.{dtor}), A_0 + 104);
							throw;
						}
						<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>._Tidy(A_0 + 104);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>.{dtor}), A_0 + 92);
						throw;
					}
					<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>._Tidy(A_0 + 92);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 52);
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 52, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0 + 28);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 28, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(IFfuReader.{dtor}), A_0);
			throw;
		}
		*A_0 = ref <Module>.??_7IFfuReader@@6B@;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0 + 4, true, 0U);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000033D8 File Offset: 0x000027D8
	internal unsafe static void setProgress(FfuReader* A_0, IFfuReaderProgress* progress)
	{
		*(A_0 + 17280) = progress;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000033F4 File Offset: 0x000027F4
	internal unsafe static void setDiagnostic(FfuReader* A_0, IDiagnostic* pIdiagnostic)
	{
		*(A_0 + 18340) = pIdiagnostic;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00003410 File Offset: 0x00002810
	internal unsafe static long SkipChunk(FfuReader* A_0, long currentPos, long chunkSize)
	{
		long num = currentPos % chunkSize;
		if (num != 0L)
		{
			currentPos += chunkSize - num;
		}
		return currentPos;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00008CB0 File Offset: 0x000080B0
	internal unsafe static FfuReaderResult* readFfuPlatformId(FfuReader* A_0, FfuReaderResult* A_1, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename)
	{
		uint num = 0U;
		FfuReaderResult* result;
		try
		{
			try
			{
				FfuReaderResult ffuReaderResult;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult + 4);
				try
				{
					uint num2 = (uint)(*(A_0 + 18340));
					if (0U != num2)
					{
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0BB@MKPNLBEM@readFfu?$CI?$CJ?5Start?4?$AA@, *(*num2 + 4));
					}
					initblk(A_0 + 628, 0, 16384);
					*(A_0 + 17036) = 0;
					*(A_0 + 17040) = 0;
					*(A_0 + 17232) = 0;
					*(A_0 + 17236) = 0;
					*(A_0 + 17240) = 0L;
					*(A_0 + 17248) = 0L;
					_iobuf* ptr = <Module>.fopen((sbyte*)((16 > *(int*)(filename + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>))) ? filename : (*(int*)filename)), (sbyte*)(&<Module>.??_C@_02JDPG@rb?$AA@));
					if (ptr != null)
					{
						goto IL_116;
					}
					ffuReaderResult = 2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_0CD@GAPHJKMF@Could?5not?5open?5FFU?5file?0?5filenam@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_116:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					FfuReaderResult ffuReaderResult2;
					FfuReaderResult* ptr2 = <Module>.FfuReader.readSecurityHdrAndCheckValidity(A_0, &ffuReaderResult2, ptr);
					try
					{
						ffuReaderResult = *(int*)ptr2;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(ref ffuReaderResult + 4, ptr2 + 4 / sizeof(FfuReaderResult));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult2));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult2 + 4, true, 0U);
					if (0 == ffuReaderResult)
					{
						goto IL_1AD;
					}
					<Module>.fclose(ptr);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1AD:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					uint num3 = (uint)(*(A_0 + 17300) * 1024);
					*(A_0 + 17036) = (int)num3;
					_iobuf* ptr;
					*(A_0 + 17256) = <Module>._ftelli64(ptr) + (ulong)(*(A_0 + 17312)) + (ulong)(*(A_0 + 17308));
					long num4 = *(A_0 + 17256);
					long num5 = (long)((ulong)num3);
					long num6 = num4 % num5;
					if (num6 != 0L)
					{
						*(A_0 + 17256) = num5 - num6 + num4;
					}
					if (0 == <Module>._fseeki64(ptr, *(A_0 + 17256), 0))
					{
						goto IL_2B9;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&<Module>.??_C@_0DA@OLEAJDFH@Corrupted?5FFU?0?5incorrect?5headerS@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_2B9:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					*(A_0 + 17232) = 0;
					_iobuf* ptr;
					ImageHeader imageHeader;
					if (1 == <Module>.fread((void*)(&imageHeader), 24U, 1U, ptr))
					{
						goto IL_362;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_0DG@LJEFNDMN@Corrupted?5FFU?0?5could?5not?5read?5im@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_362:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					ImageHeader imageHeader;
					if (imageHeader == 24)
					{
						goto IL_3FB;
					}
					_iobuf* ptr;
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right4 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, (sbyte*)(&<Module>.??_C@_0DI@OMEKDBC@Corrupted?5FFU?0?5size?5of?5image?5hea@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right4);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_3FB:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					ImageHeader imageHeader;
					if (0 == <Module>._fseeki64(ptr, (long)((ulong)(*(ref imageHeader + 16))), 1))
					{
						goto IL_49E;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, (sbyte*)(&<Module>.??_C@_0CP@NOMNLDJO@Corrupted?5FFU?0?5cannot?5skip?5manif@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right5);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_49E:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					long num7 = (long)((ulong)(*(A_0 + 17300) * 1024));
					_iobuf* ptr;
					long num8 = <Module>._ftelli64(ptr);
					long num9 = num8 % num7;
					if (num9 != 0L)
					{
						num8 += num7 - num9;
					}
					if (0 == <Module>._fseeki64(ptr, num8, 0))
					{
						goto IL_56C;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, (sbyte*)(&<Module>.??_C@_0CP@JCLBHCCP@Corrupted?5FFU?0?5cannot?5skip?5paddd@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right6);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_56C:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					StoreHeader storeHeader;
					if (<Module>.fread((void*)(&storeHeader), 248U, 1U, ptr) == 1)
					{
						goto IL_66C;
					}
					FfuReaderResult ffuReaderResult3;
					*(ref ffuReaderResult3 + 20) = 0;
					*(ref ffuReaderResult3 + 24) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult3 + 4, false, 0U);
					try
					{
						<Module>.fclose(ptr);
						ffuReaderResult3 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* left = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right7 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8, left, (sbyte*)(&<Module>.??_C@_0DF@NIFEHAIP@fread?$CI?$CGstoreHeader?0?5sizeof?$CIStore@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult3 + 4, right7);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7, true, 0U);
						*(int*)A_1 = ffuReaderResult3;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult3 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult3));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult3 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_66C:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					StoreHeader storeHeader;
					if (*(ref storeHeader + 8) != 2)
					{
						goto IL_810;
					}
					if (*(ref storeHeader + 10) != 0)
					{
						goto IL_810;
					}
					if (storeHeader == null)
					{
						goto IL_71D;
					}
					_iobuf* ptr;
					<Module>.fclose(ptr);
					ffuReaderResult = 7;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right8 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9, (sbyte*)(&<Module>.??_C@_0EL@FHOOFPLA@The?5FFU?5is?5not?5a?5full?5flash?5FFU?4@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right8);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_71D:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					StoreHeader storeHeader;
					long num10 = <Module>._ftelli64(ptr) + (ulong)(*(ref storeHeader + 212));
					long num11 = (long)((ulong)(*(ref storeHeader + 204)));
					long num12 = num10 % num11;
					if (num12 != 0L)
					{
						num10 += num11 - num12;
					}
					cpblk(A_0 + 17040, ref storeHeader + 12, 192);
					cpblk(A_0 + 88, ref storeHeader + 232, 4);
					*(A_0 + 17236) = (int)num10 - *(A_0 + 17232);
					*(A_0 + 17240) = num10;
					<Module>.fclose(ptr);
					ffuReaderResult = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, (sbyte*)(&<Module>.??_C@_00CNPNBAHC@?$AA@), 0U);
					uint num2 = (uint)(*(A_0 + 18340));
					if (0U != num2)
					{
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0P@FGKADOIP@readFfu?$CI?$CJ?5End?4?$AA@, *(*num2 + 4));
					}
					<Module>.FfuReaderResult.{ctor}(A_1, ref ffuReaderResult);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_810:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>;
					<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, 3, 1);
					try
					{
						StoreHeader storeHeader;
						<Module>.std.operator<<<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 16, (sbyte*)(&<Module>.??_C@_0DN@EDLJMDMN@The?5FFU?5has?5wrong?5version?0?5must?5@)), *(ref storeHeader + 8)), (sbyte*)(&<Module>.??_C@_01LFCBOECM@?4?$AA@)), *(ref storeHeader + 10)), (sbyte*)(&<Module>.??_C@_0N@LPLCCEEL@?0?5filename?3?5?$AA@)), filename);
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult = 3;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right9 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10);
						try
						{
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right9);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10, true, 0U);
						*(int*)A_1 = ffuReaderResult;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>));
						throw;
					}
					<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
					<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			result = A_1;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return result;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00005764 File Offset: 0x00004B64
	internal unsafe static FfuReaderResult* {ctor}(FfuReaderResult* A_0)
	{
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 4;
		*(ptr + 16) = 0;
		*(ptr + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, false, 0U);
		return A_0;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00006088 File Offset: 0x00005488
	internal unsafe static FfuReaderResult* {ctor}(FfuReaderResult* A_0, FfuReaderResult* A_0)
	{
		*A_0 = *A_0;
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 4;
		*(ptr + 16) = 0;
		*(ptr + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, A_0 + 4, 0U, uint.MaxValue);
		return A_0;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00006CEC File Offset: 0x000060EC
	internal unsafe static FfuReaderResult* =(FfuReaderResult* A_0, FfuReaderResult* A_0)
	{
		*A_0 = *A_0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(A_0 + 4, A_0 + 4);
		return A_0;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00004FBC File Offset: 0x000043BC
	internal unsafe static void __vbaseDtor(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 104;
		<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ptr);
		<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ptr);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000A924 File Offset: 0x00009D24
	internal unsafe static FfuReaderResult* readFfu(FfuReader* A_0, FfuReaderResult* A_1, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename, uint maxBlockSizeInBytes, [MarshalAs(UnmanagedType.U1)] bool secureFFU, [MarshalAs(UnmanagedType.U1)] bool dumpPartitions, [MarshalAs(UnmanagedType.U1)] bool dumpGpt)
	{
		uint num = 0U;
		FfuReaderResult* ptr2;
		try
		{
			try
			{
				uint num2 = 0U;
				FfuReaderResult ffuReaderResult;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult + 4);
				try
				{
					uint num3 = (uint)(*(A_0 + 18340));
					if (0U != num3)
					{
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num3, ref <Module>.??_C@_0BB@MKPNLBEM@readFfu?$CI?$CJ?5Start?4?$AA@, *(*num3 + 4));
					}
					initblk(A_0 + 628, 0, 16384);
					*(A_0 + 17036) = 0;
					*(A_0 + 17040) = 0;
					*(A_0 + 17232) = 0;
					*(A_0 + 17236) = 0;
					*(A_0 + 17240) = 0L;
					*(A_0 + 17248) = 0L;
					_iobuf* ptr = <Module>.fopen((sbyte*)((16 > *(int*)(filename + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>))) ? filename : (*(int*)filename)), (sbyte*)(&<Module>.??_C@_02JDPG@rb?$AA@));
					if (ptr != null)
					{
						goto IL_11D;
					}
					ffuReaderResult = 2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_0CD@GAPHJKMF@Could?5not?5open?5FFU?5file?0?5filenam@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_11D:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					FfuReaderResult ffuReaderResult2;
					ptr2 = <Module>.FfuReader.readSecurityHdrAndCheckValidity(A_0, &ffuReaderResult2, ptr);
					try
					{
						ffuReaderResult = *(int*)ptr2;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(ref ffuReaderResult + 4, ptr2 + 4 / sizeof(FfuReaderResult));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult2));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult2 + 4, true, 0U);
					if (0 == ffuReaderResult)
					{
						goto IL_1B1;
					}
					<Module>.fclose(ptr);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1B1:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					uint num4 = (uint)(*(A_0 + 17300) * 1024);
					*(A_0 + 17036) = (int)num4;
					_iobuf* ptr;
					*(A_0 + 17256) = <Module>._ftelli64(ptr) + (ulong)(*(A_0 + 17308)) + (ulong)(*(A_0 + 17312));
					long num5 = *(A_0 + 17256);
					long num6 = (long)((ulong)num4);
					long num7 = num5 % num6;
					if (num7 != 0L)
					{
						*(A_0 + 17256) = num6 - num7 + num5;
					}
					if (0 == <Module>._fseeki64(ptr, *(A_0 + 17256), 0))
					{
						goto IL_2BD;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&<Module>.??_C@_0DA@OLEAJDFH@Corrupted?5FFU?0?5incorrect?5headerS@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_2BD:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					*(A_0 + 17232) = 0;
					_iobuf* ptr;
					ImageHeader imageHeader;
					if (1 == <Module>.fread((void*)(&imageHeader), 24U, 1U, ptr))
					{
						goto IL_366;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_0DG@LJEFNDMN@Corrupted?5FFU?0?5could?5not?5read?5im@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_366:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					ImageHeader imageHeader;
					if (imageHeader == 24)
					{
						goto IL_3FF;
					}
					_iobuf* ptr;
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, (sbyte*)(&<Module>.??_C@_0DI@OMEKDBC@Corrupted?5FFU?0?5size?5of?5image?5hea@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_3FF:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					ImageHeader imageHeader;
					if (0 == <Module>._fseeki64(ptr, (long)((ulong)(*(ref imageHeader + 16))), 1))
					{
						goto IL_4A2;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, (sbyte*)(&<Module>.??_C@_0CP@NOMNLDJO@Corrupted?5FFU?0?5cannot?5skip?5manif@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_4A2:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					long num8 = (long)((ulong)(*(A_0 + 17300) * 1024));
					_iobuf* ptr;
					long num9 = <Module>._ftelli64(ptr);
					long num10 = num9 % num8;
					if (num10 != 0L)
					{
						num9 += num8 - num10;
					}
					if (0 == <Module>._fseeki64(ptr, num9, 0))
					{
						goto IL_570;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 4;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, (sbyte*)(&<Module>.??_C@_0CP@JCLBHCCP@Corrupted?5FFU?0?5cannot?5skip?5paddd@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_570:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					StoreHeader storeHeader;
					if (<Module>.fread((void*)(&storeHeader), 248U, 1U, ptr) == 1)
					{
						goto IL_661;
					}
					FfuReaderResult ffuReaderResult3;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult3 + 4);
					try
					{
						<Module>.fclose(ptr);
						ffuReaderResult3 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8;
							ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8, ptr3, (sbyte*)(&<Module>.??_C@_0DF@NIFEHAIP@fread?$CI?$CGstoreHeader?0?5sizeof?$CIStore@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult3 + 4, ptr3);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>8, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>7, true, 0U);
						*(int*)A_1 = ffuReaderResult3;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult3 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult3));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult3 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_661:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					StoreHeader storeHeader;
					if (*(ref storeHeader + 8) != 2)
					{
						goto IL_1B93;
					}
					if (*(ref storeHeader + 10) != 0)
					{
						goto IL_1B93;
					}
					if (storeHeader == null)
					{
						goto IL_712;
					}
					_iobuf* ptr;
					<Module>.fclose(ptr);
					ffuReaderResult = 7;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9, (sbyte*)(&<Module>.??_C@_0EL@FHOOFPLA@The?5FFU?5is?5not?5a?5full?5flash?5FFU?4@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr3);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>9, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_712:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					StoreHeader storeHeader;
					long num11 = <Module>._ftelli64(ptr) + (ulong)(*(ref storeHeader + 212));
					long num12 = (long)((ulong)(*(ref storeHeader + 204)));
					long num13 = num11 % num12;
					if (num13 != 0L)
					{
						num11 += num12 - num13;
					}
					cpblk(A_0 + 17040, ref storeHeader + 12, 192);
					cpblk(A_0 + 88, ref storeHeader + 232, 4);
					*(A_0 + 17236) = (int)num11 - *(A_0 + 17232);
					*(A_0 + 17240) = num11;
					if (*(ref storeHeader + 204) <= (int)maxBlockSizeInBytes)
					{
						goto IL_826;
					}
					<Module>.fclose(ptr);
					ffuReaderResult = 6;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr4 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10, (sbyte*)(&<Module>.??_C@_0CJ@EDDAJGLL@Too?5small?5maximum?5block?5size?0?5fi@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, ptr4);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>10, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_826:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					StoreHeader storeHeader;
					if (*(ref storeHeader + 204) == (int)maxBlockSizeInBytes)
					{
						goto IL_936;
					}
					_iobuf* ptr;
					long num11;
					if (0 != <Module>.FfuReader.readDescriptors(A_0, ptr, num11, maxBlockSizeInBytes, (uint)(*(ref storeHeader + 204)), (uint)(*(ref storeHeader + 208))))
					{
						goto IL_CBA;
					}
					FfuReaderResult ffuReaderResult4;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult4 + 4);
					try
					{
						<Module>.fclose(ptr);
						ffuReaderResult4 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>11;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr4 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>11, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>12;
							ptr4 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>12, ptr4, (sbyte*)(&<Module>.??_C@_0IB@KAMICEBB@0?5?$CB?$DN?5readDescriptors?$CIfp?0?5payload@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult4 + 4, ptr4);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>12));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>12, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>11));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>11, true, 0U);
						*(int*)A_1 = ffuReaderResult4;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult4 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult4));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult4 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_936:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					uint num14 = 0U;
					StoreHeader storeHeader;
					if (0 >= *(ref storeHeader + 208))
					{
						goto IL_CBA;
					}
					for (;;)
					{
						_iobuf* ptr;
						long num15 = <Module>._ftelli64(ptr);
						DataBlock dataBlock;
						if (<Module>.fread((void*)(&dataBlock), 8U, 1U, ptr) != 1)
						{
							goto IL_A2A;
						}
						if (dataBlock <= 0)
						{
							break;
						}
						if (*(ref dataBlock + 4) == 0)
						{
							break;
						}
						num14 += 1U;
						long num11;
						do
						{
							dataBlock--;
							BlockLocation blockLocation;
							if (1 != <Module>.fread((void*)(&blockLocation), 8U, 1U, ptr))
							{
								goto Block_144;
							}
							FfuReader.AccessMethod accessMethod = (blockLocation == 0) ? ((FfuReader.AccessMethod)0) : ((FfuReader.AccessMethod)2);
							FfuReader.WriteRequest writeRequest;
							*(ref writeRequest + 24) = (int)accessMethod;
							writeRequest = num11;
							uint num16 = (uint)(*(ref storeHeader + 204)) >> 9;
							*(ref writeRequest + 16) = (long)((ulong)(num16 * (uint)(*(ref blockLocation + 4))));
							*(ref writeRequest + 8) = (int)(num16 * (uint)(*(ref dataBlock + 4)));
							*(ref writeRequest + 32) = num15;
							<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>.push_back(A_0 + 92, ref writeRequest);
						}
						while (dataBlock > 0);
						dataBlock--;
						num11 += (long)((ulong)(*(ref dataBlock + 4) * *(ref storeHeader + 204)));
						if (num14 >= (uint)(*(ref storeHeader + 208)))
						{
							goto Block_147;
						}
					}
					goto IL_BDF;
					Block_144:
					goto IL_B04;
					Block_147:
					goto IL_CBA;
					IL_A2A:
					FfuReaderResult ffuReaderResult5;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult5 + 4);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult5 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>13;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>13, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>14;
							ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>14, ptr5, (sbyte*)(&<Module>.??_C@_0CN@KKAJNADJ@fread?$CI?$CGblock?0?5sizeof?$CIDataBlock?$CJ?0@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult5 + 4, ptr5);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>14));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>14, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>13));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>13, true, 0U);
						*(int*)A_1 = ffuReaderResult5;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult5 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult5));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult5 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_B04:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					FfuReaderResult ffuReaderResult6;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult6 + 4);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult6 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>15;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>15, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>16;
							ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>16, ptr5, (sbyte*)(&<Module>.??_C@_0DE@OEBOFPPN@1?5?$DN?$DN?5fread?$CI?$CGlocation?0?5sizeof?$CIBlo@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult6 + 4, ptr5);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>16));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>16, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>15));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>15, true, 0U);
						*(int*)A_1 = ffuReaderResult6;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult6 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult6));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult6 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_BDF:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					FfuReaderResult ffuReaderResult7;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult7 + 4);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult7 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>17;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>17, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>18;
							ptr5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>18, ptr5, (sbyte*)(&<Module>.??_C@_0DJ@NAHGMBCO@block?4blockLocationCount?5?$DO?50?5?$CG?$CG?5@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult7 + 4, ptr5);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>18));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>18, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>17));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>17, true, 0U);
						*(int*)A_1 = ffuReaderResult7;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult7 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult7));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult7 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_CBA:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					if (true != secureFFU)
					{
						goto IL_11C7;
					}
					long num17 = (long)((ulong)(*(A_0 + 17300) * 1024));
					long num18 = num17;
					long num19 = (long)((ulong)(*(A_0 + 17312) + *(A_0 + 17308) + 32));
					long num20 = num19 % num18;
					if (num20 != 0L)
					{
						num19 += num18 - num20;
					}
					num18 = num17;
					ImageHeader imageHeader;
					long num21 = (long)((ulong)(*(ref imageHeader + 16) + 24));
					num17 = num21 % num18;
					if (num17 != 0L)
					{
						num21 += num18 - num17;
					}
					_iobuf* ptr;
					StoreHeader storeHeader;
					<Module>._fseeki64(ptr, (long)((ulong)(*(ref storeHeader + 220) + (int)num19 + (int)num21 + 248)), 0);
					uint num22 = 0U;
					if (0 >= *(ref storeHeader + 208))
					{
						goto IL_11C7;
					}
					for (;;)
					{
						num21 = <Module>._ftelli64(ptr);
						DataBlock dataBlock2;
						if (<Module>.fread((void*)(&dataBlock2), 8U, 1U, ptr) != 1)
						{
							goto IL_E3E;
						}
						if (dataBlock2 <= 0)
						{
							break;
						}
						if (*(ref dataBlock2 + 4) == 0)
						{
							break;
						}
						BlockLocation blockLocation2;
						if (1 != <Module>.fread((void*)(&blockLocation2), 8U, 1U, ptr))
						{
							goto Block_194;
						}
						long num11;
						FfuReader.BlockDataEntry blockDataEntry = num11;
						*(ref blockDataEntry + 8) = dataBlock2;
						*(ref blockDataEntry + 12) = *(ref dataBlock2 + 4) * *(ref storeHeader + 204);
						*(ref blockDataEntry + 16) = num21;
						<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>.push_back(A_0 + 104, ref blockDataEntry);
						dataBlock2--;
						if (dataBlock2 != null)
						{
							do
							{
								dataBlock2--;
								if (1 != <Module>.fread((void*)(&blockLocation2), 8U, 1U, ptr))
								{
									goto Block_195;
								}
							}
							while (dataBlock2 != null);
						}
						dataBlock2--;
						num11 += (long)((ulong)(*(ref dataBlock2 + 4) * *(ref storeHeader + 204)));
						num22 += 1U;
						if (num22 >= (uint)(*(ref storeHeader + 208)))
						{
							goto Block_196;
						}
					}
					goto IL_10EC;
					Block_194:
					goto IL_F18;
					Block_195:
					goto IL_1002;
					Block_196:
					goto IL_11C7;
					IL_E3E:
					FfuReaderResult ffuReaderResult8;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult8 + 4);
					try
					{
						<Module>.fclose(ptr);
						ffuReaderResult8 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>19;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>19, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>20;
							ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>20, ptr6, (sbyte*)(&<Module>.??_C@_0CN@KKAJNADJ@fread?$CI?$CGblock?0?5sizeof?$CIDataBlock?$CJ?0@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult8 + 4, ptr6);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>20));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>20, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>19));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>19, true, 0U);
						*(int*)A_1 = ffuReaderResult8;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult8 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult8));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult8 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_F18:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					FfuReaderResult ffuReaderResult9;
					*(ref ffuReaderResult9 + 20) = 0;
					*(ref ffuReaderResult9 + 24) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult9 + 4, false, 0U);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult9 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>21;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>21, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>22;
							ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>22, ptr6, (sbyte*)(&<Module>.??_C@_0DE@OEBOFPPN@1?5?$DN?$DN?5fread?$CI?$CGlocation?0?5sizeof?$CIBlo@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult9 + 4, ptr6);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>22));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>22, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>21));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>21, true, 0U);
						*(int*)A_1 = ffuReaderResult9;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult9 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult9));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult9 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1002:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					FfuReaderResult ffuReaderResult10;
					*(ref ffuReaderResult10 + 20) = 0;
					*(ref ffuReaderResult10 + 24) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult10 + 4, false, 0U);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult10 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>23;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>23, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>24;
							ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>24, ptr6, (sbyte*)(&<Module>.??_C@_0DE@OEBOFPPN@1?5?$DN?$DN?5fread?$CI?$CGlocation?0?5sizeof?$CIBlo@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult10 + 4, ptr6);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>24));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>24, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>23));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>23, true, 0U);
						*(int*)A_1 = ffuReaderResult10;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult10 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult10));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult10 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_10EC:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					FfuReaderResult ffuReaderResult11;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult11 + 4);
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult11 = 5;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>25;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>25, filename, (sbyte*)(&<Module>.??_C@_02KEGNLNML@?0?5?$AA@));
						try
						{
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>26;
							ptr6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>26, ptr6, (sbyte*)(&<Module>.??_C@_0DJ@NAHGMBCO@block?4blockLocationCount?5?$DO?50?5?$CG?$CG?5@));
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult11 + 4, ptr6);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>26));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>26, true, 0U);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>25));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>25, true, 0U);
						*(int*)A_1 = ffuReaderResult11;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult11 + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult11));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult11 + 4, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_11C7:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					<Module>._fseeki64(ptr, 0L, 2);
					*(A_0 + 17248) = <Module>._ftelli64(ptr) - *(A_0 + 17240);
					<Module>._fseeki64(ptr, 0L, 0);
					uint num23 = 0U;
					vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* ptr7 = A_0 + 92;
					if (0 >= (*(ptr7 + 4) - *ptr7) / 40)
					{
						goto IL_1964;
					}
					int num24 = 0;
					for (;;)
					{
						int num25 = *(A_0 + 92) + num24;
						if (*(num25 + 16) == 0L)
						{
							FfuReader.WriteRequest* ptr8 = num25;
							if (0 != <Module>._fseeki64(ptr, *ptr8 + 512L, 0))
							{
								goto IL_13FE;
							}
							EFI_PARTITION_TABLE_HEADER efi_PARTITION_TABLE_HEADER;
							if (1 != <Module>.fread((void*)(&efi_PARTITION_TABLE_HEADER), 512U, 1U, ptr))
							{
								break;
							}
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, (sbyte*)(&<Module>.??_C@_08BOGKMBPC@EFI?5PART?$AA@));
							try
							{
								sbyte* ptr9 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27 + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27 : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
								sbyte* ptr10 = ptr9;
								EFI_PARTITION_TABLE_HEADER* ptr11 = &efi_PARTITION_TABLE_HEADER;
								sbyte b = efi_PARTITION_TABLE_HEADER;
								sbyte b2 = *(sbyte*)ptr9;
								if (efi_PARTITION_TABLE_HEADER >= b2)
								{
									while (b <= b2)
									{
										if (b != 0)
										{
											ptr11 += 1 / sizeof(EFI_PARTITION_TABLE_HEADER);
											ptr10 += 1 / sizeof(sbyte);
											b = *(sbyte*)ptr11;
											b2 = *(sbyte*)ptr10;
											if (b < b2)
											{
												break;
											}
										}
										else
										{
											if (128 != *(ref efi_PARTITION_TABLE_HEADER + 80))
											{
												goto IL_1519;
											}
											if (128 != *(ref efi_PARTITION_TABLE_HEADER + 84))
											{
												goto IL_15C0;
											}
											$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@ $ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@;
											if (128 != <Module>.fread((void*)(&$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@), 128U, 128U, ptr))
											{
												goto IL_1667;
											}
											uint num26 = 0U;
											EFI_PARTITION_TABLE_HEADER efi_PARTITION_TABLE_HEADER2;
											if (0 < *(ref efi_PARTITION_TABLE_HEADER + 80))
											{
												num25 = ref $ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@ + 56;
												while (*num25 != 0)
												{
													num26 += 1U;
													num25 += 128;
													if (num26 >= (uint)(*(ref efi_PARTITION_TABLE_HEADER + 80)))
													{
														break;
													}
												}
												uint num2;
												if (num2 < num26)
												{
													num2 = num26;
													cpblk(A_0 + 116, ref efi_PARTITION_TABLE_HEADER, 512);
													efi_PARTITION_TABLE_HEADER2 = efi_PARTITION_TABLE_HEADER;
													cpblk(A_0 + 628, ref $ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@, 16384);
												}
											}
											*(ref efi_PARTITION_TABLE_HEADER2 + 16) = 0;
											num26 = <Module>.FfuReader.calculate_crc32(A_0, (void*)(&efi_PARTITION_TABLE_HEADER2), (uint)(*(ref efi_PARTITION_TABLE_HEADER2 + 12)));
											if (*(ref efi_PARTITION_TABLE_HEADER + 16) != (int)num26)
											{
												goto IL_170E;
											}
											num26 = <Module>.FfuReader.calculate_crc32(A_0, (void*)(&$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@), (uint)(*(ref efi_PARTITION_TABLE_HEADER + 84) * *(ref efi_PARTITION_TABLE_HEADER + 80)));
											if (*(ref efi_PARTITION_TABLE_HEADER + 88) != (int)num26)
											{
												goto IL_1839;
											}
											break;
										}
									}
								}
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
						}
						num23 += 1U;
						num24 += 40;
						ptr7 = A_0 + 92;
						if (num23 >= (uint)((*(ptr7 + 4) - *ptr7) / 40))
						{
							goto Block_254;
						}
					}
					goto IL_148B;
					Block_254:
					goto IL_1964;
					IL_13FE:
					<Module>.fclose(ptr);
					ffuReaderResult = 10;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>28;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>28, (sbyte*)(&<Module>.??_C@_0DE@KMDNEMNG@Seek?5to?5the?5GPT?5header?5?$CL?5MBR?5siz@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>28));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>28, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_148B:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					<Module>.fclose(ptr);
					ffuReaderResult = 10;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>29;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>29, (sbyte*)(&<Module>.??_C@_0CE@CGHHDHDD@Read?5table?5header?5failed?0?5filena@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>29));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>29, true, 0U);
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1519:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult = 10;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>30;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>30, (sbyte*)(&<Module>.??_C@_0DB@NLCGKNEK@Wrong?5number?5of?5GPT?5partition?5en@), filename);
						try
						{
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>30));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>30, true, 0U);
						*(int*)A_1 = ffuReaderResult;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_15C0:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult = 10;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>31;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>31, (sbyte*)(&<Module>.??_C@_0CN@BBLDFMCJ@Wrong?5size?5of?5GPT?5partition?5entr@), filename);
						try
						{
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>31));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>31, true, 0U);
						*(int*)A_1 = ffuReaderResult;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1667:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
					try
					{
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult = 10;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>32;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>32, (sbyte*)(&<Module>.??_C@_0CN@EGEGKNHO@Cannot?5read?5GPT?5partition?5entrie@), filename);
						try
						{
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>32));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>32, true, 0U);
						*(int*)A_1 = ffuReaderResult;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_170E:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
					try
					{
						basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>;
						<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, 3, 1);
						try
						{
							int _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z = <Module>.__unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z;
							EFI_PARTITION_TABLE_HEADER efi_PARTITION_TABLE_HEADER;
							uint num26;
							<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 16, (sbyte*)(&<Module>.??_C@_0DE@NAEAEFBF@CRC32?5mismatch?5of?5GPT?5header?$CB?5CR@)), _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z), (uint)(*(ref efi_PARTITION_TABLE_HEADER + 16))), (sbyte*)(&<Module>.??_C@_0CA@MGBAEBFD@?5Calculated?5CRC32?5of?5header?3?50x?$AA@)), _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z), num26), (sbyte*)(&<Module>.??_C@_0N@LPLCCEEL@?0?5filename?3?5?$AA@)), filename), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
							_iobuf* ptr;
							<Module>.fclose(ptr);
							ffuReaderResult = 8;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>33;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>33);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>33));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>33, true, 0U);
							*(int*)A_1 = ffuReaderResult;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
							num = 1U;
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>));
							throw;
						}
						<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
						<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1839:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27;
					try
					{
						basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2;
						<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2, 3, 1);
						try
						{
							int _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z = <Module>.__unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z;
							EFI_PARTITION_TABLE_HEADER efi_PARTITION_TABLE_HEADER;
							uint num26;
							<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 16, (sbyte*)(&<Module>.??_C@_0ED@DFFBNCAK@CRC32?5mismatch?5of?5GPT?5partition?5@)), _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z), (uint)(*(ref efi_PARTITION_TABLE_HEADER + 88))), (sbyte*)(&<Module>.??_C@_0CA@MGBAEBFD@?5Calculated?5CRC32?5of?5header?3?50x?$AA@)), _unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z), num26), (sbyte*)(&<Module>.??_C@_0N@LPLCCEEL@?0?5filename?3?5?$AA@)), filename), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
							_iobuf* ptr;
							<Module>.fclose(ptr);
							ffuReaderResult = 8;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>34;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>34);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right2);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>34));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>34, true, 0U);
							*(int*)A_1 = ffuReaderResult;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
							num = 1U;
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2));
							throw;
						}
						<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 104);
						<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 104);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>27, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1964:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					_iobuf* ptr;
					<Module>._fseeki64(ptr, 0L, 0);
					<Module>.FfuReader.readGpt(A_0);
					byte* ptr12 = <Module>.new[](10485760U);
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>35;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr13 = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>35, (sbyte*)(&<Module>.??_C@_04MIKOFOFJ@SBL1?$AA@));
					<Module>.FfuReader.readRkh(A_0, ptr, maxBlockSizeInBytes, ptr13, ptr12);
					uint num27 = (uint)(*(int*)(ptr12 + 10240 + 44) - *(int*)(ptr12 + 10240 + 24) + 10240);
					if (*(int*)(ptr12 + 10240 + 4) == 1943474228 && *(int*)(ptr12 + 10240 + 40) == 256)
					{
						ptr13 = A_0 + 28;
						*(ptr13 + 16) = 0;
						*((16 > *(ptr13 + 20)) ? ptr13 : (*ptr13)) = 0;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0 + 28, (sbyte*)(ptr12 + num27 + 305), 64U);
					}
					else
					{
						num27 = (uint)(*(int*)(ptr12 + 44) - *(int*)(ptr12 + 24));
						if (*(int*)(ptr12 + 4) == 1943474228 && *(int*)(ptr12 + 40) == 256)
						{
							ptr13 = A_0 + 28;
							*(ptr13 + 16) = 0;
							*((16 > *(ptr13 + 20)) ? ptr13 : (*ptr13)) = 0;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0 + 28, (sbyte*)(ptr12 + num27 + 305), 64U);
						}
					}
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>36;
					ptr13 = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>36, (sbyte*)(&<Module>.??_C@_04DAEIOFGI@UEFI?$AA@));
					<Module>.FfuReader.readRkh(A_0, ptr, maxBlockSizeInBytes, ptr13, ptr12);
					num27 = (uint)(*(int*)(ptr12 + 12) - *(int*)(ptr12 + 32) - 256 + 517);
					if (*(int*)(ptr12 + 24) == 256)
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Eos(A_0 + 52, 0U);
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0 + 52, (sbyte*)(num27 + ptr12), 64U);
					}
					<Module>.delete[]((void*)ptr12);
					if (true == dumpGpt)
					{
						<Module>._fseeki64(ptr, 0L, 0);
						<Module>.FfuReader.DumpGpt(A_0, ptr);
					}
					if (true == dumpPartitions)
					{
						<Module>._fseeki64(ptr, 0L, 0);
						<Module>.FfuReader.DumpPartitions(A_0, ptr, maxBlockSizeInBytes);
					}
					<Module>.FfuReader.checkDppPartition(A_0);
					<Module>.fclose(ptr);
					ffuReaderResult = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, (sbyte*)(&<Module>.??_C@_00CNPNBAHC@?$AA@), 0U);
					uint num3 = (uint)(*(A_0 + 18340));
					if (0U != num3)
					{
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num3, ref <Module>.??_C@_0P@FGKADOIP@readFfu?$CI?$CJ?5End?4?$AA@, *(*num3 + 4));
					}
					*(int*)A_1 = ffuReaderResult;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			IL_1B93:
			try
			{
				FfuReaderResult ffuReaderResult;
				try
				{
					basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3;
					<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3, 3, 1);
					try
					{
						StoreHeader storeHeader;
						<Module>.std.operator<<<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 16, (sbyte*)(&<Module>.??_C@_0DN@EDLJMDMN@The?5FFU?5has?5wrong?5version?0?5must?5@)), *(ref storeHeader + 8)), (sbyte*)(&<Module>.??_C@_01LFCBOECM@?4?$AA@)), *(ref storeHeader + 10)), (sbyte*)(&<Module>.??_C@_0N@LPLCCEEL@?0?5filename?3?5?$AA@)), filename);
						_iobuf* ptr;
						<Module>.fclose(ptr);
						ffuReaderResult = 3;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>37;
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right3 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>37);
						try
						{
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref ffuReaderResult + 4, right3);
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>37));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>37, true, 0U);
						*(int*)A_1 = ffuReaderResult;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult + 4);
						num = 1U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3));
						throw;
					}
					<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 104);
					<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 104);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			ptr2 = A_1;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return ptr2;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x000073DC File Offset: 0x000067DC
	internal unsafe static FfuReaderResult* readPartImage(FfuReader* A_0, FfuReaderResult* A_1, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename, uint maxBlockSizeInBytes)
	{
		try
		{
			uint num = 0U;
			try
			{
				FfuReaderResult* ptr = A_1 + 4 / sizeof(FfuReaderResult);
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = ptr;
				*(ptr2 + 16) = 0;
				*(ptr2 + 20) = 0;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2, false, 0U);
				num = 1U;
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, filename, (sbyte*)(&<Module>.??_C@_0BM@GBJGKMED@?5image?5parsed?5successfully?4?$AA@));
				try
				{
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
				*(int*)A_1 = 0;
				_iobuf* ptr3 = <Module>.fopen((sbyte*)((16 > *(int*)(filename + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>))) ? filename : (*(int*)filename)), (sbyte*)(&<Module>.??_C@_02JDPG@rb?$AA@));
				if (ptr3 == null)
				{
					*(int*)A_1 = 2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&<Module>.??_C@_0CA@JFGCLPEA@Could?5not?5open?5file?0?5filename?3?5?$AA@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right2);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
				}
				else
				{
					uint num2 = 0U;
					FfuReaderResult ffuReaderResult;
					FfuReaderResult* ptr4 = <Module>.FfuReader.readImageId(A_0, &ffuReaderResult, ptr3, ref num2);
					try
					{
						*(int*)A_1 = *(int*)ptr4;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(ptr, ptr4 + 4 / sizeof(FfuReaderResult));
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
					if (*(int*)A_1 == 0)
					{
						if (<Module>.FfuReader.isValidBootImage(A_0, ref num2) == null)
						{
							*(int*)A_1 = 12;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_0CN@CDANHBKI@The?5file?5is?5not?5valid?5boot?5image@), filename);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right3);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
						}
						else
						{
							byte* ptr5 = <Module>.new[](10485760U);
							if (2219564241U == num2)
							{
								uint num3 = <Module>.fread((void*)ptr5, 1U, 10320U, ptr3);
								uint num4 = (uint)(*(int*)(ptr5 + 10240 + 44) - *(int*)(ptr5 + 10240 + 24) + 10240);
								if (*(int*)(ptr5 + 10240 + 4) == 1943474228 && *(int*)(ptr5 + 10240 + 40) == 256)
								{
									<Module>.fread((void*)(num3 + ptr5), 1U, num4 - num3 + 369U, ptr3);
									*(A_0 + 28 + 16) = 0;
									*((16 > *(A_0 + 28 + 20)) ? (A_0 + 28) : (*(A_0 + 28))) = 0;
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0 + 28, (sbyte*)(ptr5 + num4 + 305), 64U);
								}
								else
								{
									uint num5 = (uint)(*(int*)(ptr5 + 44) - *(int*)(ptr5 + 24));
									if (*(int*)(ptr5 + 4) == 1943474228 && *(int*)(ptr5 + 40) == 256)
									{
										<Module>.fread((void*)(num3 + ptr5), 1U, num5 - num3 + 369U, ptr3);
										*(A_0 + 28 + 16) = 0;
										*((16 > *(A_0 + 28 + 20)) ? (A_0 + 28) : (*(A_0 + 28))) = 0;
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0 + 28, (sbyte*)(ptr5 + num5 + 305), 64U);
									}
								}
							}
							else
							{
								uint num6 = <Module>.fread((void*)ptr5, 1U, 36U, ptr3);
								uint num7 = (uint)(*(int*)(ptr5 + 12) - *(int*)(ptr5 + 32) - 256 + 517);
								if (*(int*)(ptr5 + 24) == 256)
								{
									<Module>.fread((void*)(num6 + ptr5), 1U, num7 - num6 + 64U, ptr3);
									FfuReader* ptr6 = A_0 + 28;
									*(ptr6 + 16) = 0;
									*((16 > *(ptr6 + 20)) ? ptr6 : (*ptr6)) = 0;
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr6, (sbyte*)(num7 + ptr5), 64U);
								}
							}
							if (ptr5 != null)
							{
								<Module>.delete[]((void*)ptr5);
							}
						}
					}
					<Module>.fclose(ptr3);
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000A69C File Offset: 0x00009A9C
	internal unsafe static void readGpt(FfuReader* A_0)
	{
		FfuReader.partition partition;
		<Module>.FfuReader.partition.{ctor}(ref partition);
		try
		{
			uint num = (uint)(*(A_0 + 18340));
			if (0U != num)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BB@DFMAPPBF@readGpt?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
			}
			uint num2 = 0U;
			if (0 < *(A_0 + 196))
			{
				int num3 = 344;
				FfuReader* ptr = A_0 + 668;
				FfuReader* ptr2 = A_0 + 684;
				while (*(ptr + 16) != 0)
				{
					int num4 = 0;
					FfuReader* ptr3 = ptr2;
					$ArrayType$$$BY0EI@D $ArrayType$$$BY0EI@D;
					do
					{
						short num5 = *ptr3;
						if (num5 == 0)
						{
							break;
						}
						*(num4 + ref $ArrayType$$$BY0EI@D) = (byte)num5;
						short num6 = *((num3 + num4) * 2 + A_0 - 2);
						if (num6 == 0)
						{
							goto IL_122;
						}
						*(num4 + (ref $ArrayType$$$BY0EI@D + 1)) = (byte)num6;
						short num7 = *((num3 + num4) * 2 + A_0);
						if (num7 == 0)
						{
							goto IL_128;
						}
						*(num4 + (ref $ArrayType$$$BY0EI@D + 2)) = (byte)num7;
						short num8 = *((num3 + num4) * 2 + A_0 + 2);
						if (num8 == 0)
						{
							goto IL_12E;
						}
						*(num4 + (ref $ArrayType$$$BY0EI@D + 3)) = (byte)num8;
						short num9 = *((num3 + num4) * 2 + A_0 + 4);
						if (num9 == 0)
						{
							goto IL_134;
						}
						*(num4 + (ref $ArrayType$$$BY0EI@D + 4)) = (byte)num9;
						short num10 = *((num3 + num4) * 2 + A_0 + 6);
						if (num10 == 0)
						{
							goto IL_13A;
						}
						*(num4 + (ref $ArrayType$$$BY0EI@D + 5)) = (byte)num10;
						num4 += 6;
						ptr3 += 12;
					}
					while (num4 < 36);
					IL_13E:
					*(num4 + ref $ArrayType$$$BY0EI@D) = 0;
					uint count;
					if ($ArrayType$$$BY0EI@D == null)
					{
						count = 0U;
					}
					else
					{
						sbyte* ptr4 = ref $ArrayType$$$BY0EI@D;
						do
						{
							ptr4++;
						}
						while (*ptr4 != 0);
						count = ptr4 - ref $ArrayType$$$BY0EI@D;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref partition, (sbyte*)(&$ArrayType$$$BY0EI@D), count);
					ulong num11 = (ulong)(*(ptr - 8));
					*(ref partition + 24) = (long)num11;
					ulong num12 = (ulong)(*ptr);
					*(ref partition + 32) = (long)num12;
					*(ref partition + 40) = (long)(num12 - num11 + 1UL);
					*(ref partition + 48) = *(ptr + 8);
					<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>.push_back(A_0 + 17268, ref partition);
					num2 += 1U;
					ptr2 += 128;
					num3 += 64;
					ptr += 128;
					if (num2 >= (uint)(*(A_0 + 196)))
					{
						break;
					}
					continue;
					goto IL_13E;
					IL_122:
					num4++;
					goto IL_13E;
					IL_128:
					num4 += 2;
					goto IL_13E;
					IL_12E:
					num4 += 3;
					goto IL_13E;
					IL_134:
					num4 += 4;
					goto IL_13E;
					IL_13A:
					num4 += 5;
					goto IL_13E;
				}
			}
			sbyte* ptr5 = ref <Module>.??_C@_08KJPBNJGC@Overflow?$AA@;
			do
			{
				ptr5++;
			}
			while (*ptr5 != 0);
			int count2 = ptr5 - ref <Module>.??_C@_08KJPBNJGC@Overflow?$AA@;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref partition, (sbyte*)(&<Module>.??_C@_08KJPBNJGC@Overflow?$AA@), (uint)count2);
			<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>.push_back(A_0 + 17268, ref partition);
			uint num13 = (uint)(*(A_0 + 18340));
			if (0U != num13)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num13, ref <Module>.??_C@_0P@KKOFAEKD@readGpt?$CI?$CJ?5End?4?$AA@, *(*num13 + 4));
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(FfuReader.partition.{dtor}), (void*)(&partition));
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref partition, true, 0U);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000578C File Offset: 0x00004B8C
	internal unsafe static void {dtor}(FfuReader.partition* A_0)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000077A4 File Offset: 0x00006BA4
	internal unsafe static uint readDescriptors(FfuReader* A_0, _iobuf* fp, long fileOffset, uint maxBlockSizeInBytes, uint dwBlockSizeInBytes, uint writeDescriptorCount)
	{
		uint num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BJ@EAKABFJM@readDescriptors?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
		}
		FfuReader.WriteRequest writeRequest = 0L;
		*(ref writeRequest + 8) = 0;
		*(ref writeRequest + 16) = 0L;
		*(ref writeRequest + 24) = 0;
		*(ref writeRequest + 32) = 0L;
		uint num2 = 0U;
		if (0U < writeDescriptorCount)
		{
			for (;;)
			{
				IL_49:
				long num3 = <Module>._ftelli64(fp);
				DataBlock dataBlock;
				if (<Module>.fread((void*)(&dataBlock), 8U, 1U, fp) != 1)
				{
					return 0;
				}
				if (dataBlock > 0 && *(ref dataBlock + 4) != 0)
				{
					num2 += 1U;
					BlockLocation blockLocation;
					while (1 == <Module>.fread((void*)(&blockLocation), 8U, 1U, fp))
					{
						if (*(ref writeRequest + 8) == 0)
						{
							FfuReader.AccessMethod accessMethod = (blockLocation == 0) ? ((FfuReader.AccessMethod)0) : ((FfuReader.AccessMethod)2);
							*(ref writeRequest + 24) = (int)accessMethod;
							writeRequest = fileOffset;
							uint num4 = dwBlockSizeInBytes >> 9;
							*(ref writeRequest + 16) = (long)((ulong)(num4 * (uint)(*(ref blockLocation + 4))));
							*(ref writeRequest + 8) = (int)(num4 * (uint)(*(ref dataBlock + 4)));
							*(ref writeRequest + 32) = num3;
							dataBlock--;
						}
						else
						{
							uint num4 = dwBlockSizeInBytes >> 9;
							uint num5 = (uint)(*(ref writeRequest + 8) + (int)(num4 * (uint)(*(ref dataBlock + 4))));
							if (num5 <= maxBlockSizeInBytes >> 9)
							{
								FfuReader.AccessMethod accessMethod2 = (blockLocation == 0) ? ((FfuReader.AccessMethod)0) : ((FfuReader.AccessMethod)2);
								if (*(ref writeRequest + 24) == (int)accessMethod2 && (ulong)(*(ref writeRequest + 8)) + (ulong)(*(ref writeRequest + 16)) == (ulong)(num4 * (uint)(*(ref blockLocation + 4))))
								{
									*(ref writeRequest + 8) = (int)num5;
									dataBlock--;
									goto IL_15B;
								}
							}
							<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>.push_back(A_0 + 92, ref writeRequest);
							*(ref writeRequest + 8) = 0;
							if (0 != <Module>._fseeki64(fp, -8L, 1))
							{
								return 0;
							}
						}
						IL_15B:
						if (dataBlock <= 0)
						{
							fileOffset += (long)((ulong)(*(ref dataBlock + 4) * (int)dwBlockSizeInBytes));
							if (num2 >= writeDescriptorCount)
							{
								goto Block_13;
							}
							goto IL_49;
						}
					}
					return 0;
				}
				return 0;
			}
			Block_13:
			if (*(ref writeRequest + 8) != 0)
			{
				<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>.push_back(A_0 + 92, ref writeRequest);
			}
		}
		num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BH@ODFOOBCN@readDescriptors?$CI?$CJ?5End?4?$AA@, *(*num + 4));
		}
		return 1;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00003434 File Offset: 0x00002834
	internal unsafe static int calculateSectorIndexInPartition(FfuReader* A_0, EFI_PARTITION_ENTRY* partitionEntries, FfuReader.WriteRequest* writeRequest, uint iSectorInBlock, uint* indexSectorInPartition)
	{
		ulong num = (ulong)iSectorInBlock;
		ulong num2 = (ulong)(*(writeRequest + 16));
		ulong num3 = num2 + num;
		if (num3 < (ulong)(*(partitionEntries + 32)))
		{
			*indexSectorInPartition = (int)iSectorInBlock;
			return -1;
		}
		uint num4 = 0U;
		EFI_PARTITION_ENTRY* ptr = partitionEntries + 32;
		while (*(ptr + 24) != 0)
		{
			if (num3 >= (ulong)(*ptr) && num3 <= (ulong)(*(ptr + 8)))
			{
				*indexSectorInPartition = (int)((uint)(num2 - (ulong)(*(partitionEntries + num4 * 128U + 32)) + num));
				return num4;
			}
			num4 += 1U;
			ptr += 128;
			if (num4 >= 128U)
			{
				return 1;
			}
		}
		*indexSectorInPartition = (int)((uint)(num2 - (ulong)(*(partitionEntries + num4 * 128U - 88)) + num + ulong.MaxValue));
		return num4;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x000060C8 File Offset: 0x000054C8
	internal unsafe static uint readRkh(FfuReader* A_0, _iobuf* fp, uint maxBlockSizeInBytes, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* name, byte* pDumpPartition)
	{
		try
		{
			uint num = (uint)(*(A_0 + 18340));
			if (0U != num)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BB@EFIOPFCC@readRkh?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
			}
			byte* ptr = <Module>.new[](maxBlockSizeInBytes);
			int i = 0;
			uint num2 = 0U;
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
			try
			{
				uint num3 = 0U;
				int num4 = *(A_0 + 92);
				if (0 >= (*(A_0 + 92 + 4) - num4) / 40)
				{
					goto IL_265;
				}
				int num5 = 0;
				while (i < 1)
				{
					bool flag = false;
					uint num6 = 0U;
					int num7 = num4 + num5;
					if (0 < *(num7 + 8))
					{
						FfuReader* partitionEntries = A_0 + 628;
						byte* ptr2 = ptr;
						for (;;)
						{
							num2 = 0U;
							FfuReader.WriteRequest* writeRequest = num7;
							int num8 = <Module>.FfuReader.calculateSectorIndexInPartition(A_0, partitionEntries, writeRequest, num6, ref num2);
							sbyte* ptr3;
							if (num8 == -1)
							{
								ptr3 = (sbyte*)(&<Module>.??_C@_03GKBDPGIH@GPT?$AA@);
								goto IL_E5;
							}
							FfuReader.partition* ptr4 = num8 * 64 + *(A_0 + 17268);
							sbyte* ptr5;
							if (16 <= *(ptr4 + 20))
							{
								ptr5 = *ptr4;
							}
							else
							{
								ptr5 = ptr4;
							}
							ptr3 = ptr5;
							if (*(sbyte*)ptr5 != 0)
							{
								goto IL_E5;
							}
							uint count = 0U;
							IL_FD:
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, ptr3, count);
							if (flag)
							{
								goto IL_164;
							}
							if (((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, name) == 0) ? 1 : 0) != 0)
							{
								FfuReader.WriteRequest* ptr6 = *(A_0 + 92) + num5;
								if (0 != <Module>._fseeki64(fp, *ptr6, 0))
								{
									goto IL_1F4;
								}
								FfuReader.WriteRequest* ptr7 = *(A_0 + 92) + num5;
								if (<Module>.fread((void*)ptr, 512U, (uint)(*(ptr7 + 8)), fp) != *(num5 + *(A_0 + 92) + 8))
								{
									break;
								}
								flag = true;
								goto IL_164;
							}
							IL_1A7:
							num6 += 1U;
							ptr2 += 512;
							num7 = num5 + *(A_0 + 92);
							if (num6 >= (uint)(*(num7 + 8)))
							{
								goto IL_1CC;
							}
							continue;
							IL_164:
							if (((((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_08KJPBNJGC@Overflow?$AA@)) == 0) ? 1 : 0) == 0) ? 1 : 0) == 0)
							{
								goto IL_1A7;
							}
							if (((<Module>.std.operator==<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, name) == 0) ? 1 : 0) != 0)
							{
								flag = false;
								i++;
								goto IL_1A7;
							}
							cpblk(num2 * 512U + pDumpPartition, ptr2, 512);
							goto IL_1A7;
							IL_E5:
							sbyte* ptr8 = ptr3;
							if (*(sbyte*)ptr3 != 0)
							{
								do
								{
									ptr8 += 1 / sizeof(sbyte);
								}
								while (*(sbyte*)ptr8 != 0);
							}
							count = (uint)(ptr8 - ptr3);
							goto IL_FD;
						}
						goto IL_22C;
						IL_1F4:
						<Module>.delete[]((void*)ptr);
						goto IL_20A;
					}
					IL_1CC:
					num3 += 1U;
					num5 += 40;
					num4 = *(A_0 + 92);
					if (num3 >= (uint)((*(A_0 + 92 + 4) - num4) / 40))
					{
						break;
					}
				}
				goto IL_265;
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			IL_20A:
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)name);
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(name, true, 0U);
		return 1;
		IL_22C:
		try
		{
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			try
			{
				byte* ptr;
				<Module>.delete[]((void*)ptr);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)name);
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(name, true, 0U);
		return 1;
		IL_265:
		try
		{
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			try
			{
				byte* ptr;
				<Module>.delete[]((void*)ptr);
				uint num = (uint)(*(A_0 + 18340));
				if (0U != num)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0P@OAPOKHFA@readRkh?$CI?$CJ?5End?4?$AA@, *(*num + 4));
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)name);
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(name, true, 0U);
		return 0;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000796C File Offset: 0x00006D6C
	internal unsafe static FfuReaderResult* readSecurityHdrAndCheckValidity(FfuReader* A_0, FfuReaderResult* A_1, _iobuf* fp)
	{
		try
		{
			uint num = 0U;
			FfuReaderResult* ptr = A_1 + 4 / sizeof(FfuReaderResult);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = ptr;
			*(ptr2 + 16) = 0;
			*(ptr2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2, false, 0U);
			num = 1U;
			*(int*)A_1 = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_00CNPNBAHC@?$AA@), 0U);
			uint num2 = (uint)(*(A_0 + 18340));
			if (0U != num2)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0CJ@KHIPEPBA@readSecurityHdrAndCheckValidity?$CI@, *(*num2 + 4));
			}
			if (1 != <Module>.fread(A_0 + 17284, 32U, 1U, fp))
			{
				*(int*)A_1 = 4;
				sbyte* ptr3 = ref <Module>.??_C@_0CO@NPKMMIFF@Corrupted?5FFU?0?5could?5not?5read?5se@;
				do
				{
					ptr3++;
				}
				while (*ptr3 != 0);
				int count = ptr3 - ref <Module>.??_C@_0CO@NPKMMIFF@Corrupted?5FFU?0?5could?5not?5read?5se@;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_0CO@NPKMMIFF@Corrupted?5FFU?0?5could?5not?5read?5se@), (uint)count);
			}
			else
			{
				uint num3 = 12U;
				sbyte* ptr4 = ref <Module>.??_C@_0N@LJGGPJIJ@SignedImage?5?$AA@;
				FfuReader* ptr5 = A_0 + 17288;
				byte b = *ptr5;
				byte b2 = 83;
				if (b >= 83)
				{
					while (b <= b2)
					{
						if (num3 != 1U)
						{
							num3 -= 1U;
							ptr5++;
							ptr4++;
							b = *ptr5;
							b2 = *ptr4;
							if (b < b2)
							{
								break;
							}
						}
						else
						{
							if (32 != *(A_0 + 17284))
							{
								basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>;
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, 3, 1);
								try
								{
									<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 16, (sbyte*)(&<Module>.??_C@_0BG@MMHJALCK@Invalid?5struct?5size?3?5?$AA@)), (uint)(*(A_0 + 17284))), (sbyte*)(&<Module>.??_C@_0BC@LJPEICOC@?4?5Expected?5size?3?5?$AA@)), 32U), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>);
									try
									{
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
									*(int*)A_1 = 4;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>));
									throw;
								}
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
								<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020> + 104);
								goto IL_421;
							}
							if (32780 != *(A_0 + 17304))
							{
								basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2;
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2, 3, 1);
								try
								{
									<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 16, (sbyte*)(&<Module>.??_C@_0BI@NPNBLBND@Unsupported?5algorithm?3?5?$AA@)), (uint)(*(A_0 + 17304))), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2);
									try
									{
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right2);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
									*(int*)A_1 = 4;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2));
									throw;
								}
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 104);
								<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 104);
								goto IL_421;
							}
							if (0 == *(A_0 + 17300))
							{
								basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3;
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3, 3, 1);
								try
								{
									<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 16, (sbyte*)(&<Module>.??_C@_0BF@CHEBECHJ@Invalid?5chunk?5size?3?5?$AA@)), (uint)(*(A_0 + 17300))), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right3 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3);
									try
									{
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right3);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
									*(int*)A_1 = 4;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3));
									throw;
								}
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 104);
								<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 104);
								goto IL_421;
							}
							if (0 == *(A_0 + 17308))
							{
								basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4;
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4, 3, 1);
								try
								{
									<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4 + 16, (sbyte*)(&<Module>.??_C@_0BH@EMBEMOOJ@Invalid?5catalog?5size?3?5?$AA@)), (uint)(*(A_0 + 17308))), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4;
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right4 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4);
									try
									{
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right4);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, true, 0U);
									*(int*)A_1 = 4;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4));
									throw;
								}
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4 + 104);
								<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>4 + 104);
								goto IL_421;
							}
							uint num4 = (uint)(*(A_0 + 17312));
							if (0U == num4 || (num4 & 31U) != 0U)
							{
								basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020> basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5;
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5, 3, 1);
								try
								{
									<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.<<(<Module>.std.operator<<<struct\u0020std::char_traits<char>\u0020>(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5 + 16, (sbyte*)(&<Module>.??_C@_0BK@HOBHKCKM@Invalid?5hash?5table?5size?3?5?$AA@)), (uint)(*(A_0 + 17312))), <Module>.__unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z);
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5;
									basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right5 = <Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.str(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5, &basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5);
									try
									{
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right5);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, true, 0U);
									*(int*)A_1 = 4;
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor), (void*)(&basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5));
									throw;
								}
								<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5 + 104);
								<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_stringstream<char,std::char_traits<char>,std::allocator<char>_u0020>5 + 104);
								goto IL_421;
							}
							goto IL_421;
						}
					}
				}
				*(int*)A_1 = 4;
				sbyte* ptr6 = ref <Module>.??_C@_0CP@MDFGNDNN@Corrupted?5FFU?0?5image?5header?5sign@;
				do
				{
					ptr6++;
				}
				while (*ptr6 != 0);
				int count2 = ptr6 - ref <Module>.??_C@_0CP@MDFGNDNN@Corrupted?5FFU?0?5image?5header?5sign@;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_0CP@MDFGNDNN@Corrupted?5FFU?0?5image?5header?5sign@), (uint)count2);
			}
			IL_421:
			<Module>.FfuReader.trace(A_0, (sbyte*)(&<Module>.??_C@_0CH@EGAMGHCG@readSecurityHdrAndCheckValidity?$CI@));
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000034E4 File Offset: 0x000028E4
	internal unsafe static void crc32_gentab(FfuReader* A_0)
	{
		uint num = 0U;
		FfuReader* ptr = A_0 + 17316;
		do
		{
			uint num2 = num;
			uint num3 = 8U;
			do
			{
				if ((num2 & 1U) != 0U)
				{
					num2 = (num2 >> 1 ^ 3988292384U);
				}
				else
				{
					num2 >>= 1;
				}
				num3 -= 1U;
			}
			while (num3 > 0U);
			*ptr = (int)num2;
			num += 1U;
			ptr += 4;
		}
		while (num < 256U);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00003538 File Offset: 0x00002938
	internal unsafe static uint calculate_crc32(FfuReader* A_0, void* data, uint size)
	{
		<Module>.FfuReader.crc32_gentab(A_0);
		uint num = uint.MaxValue;
		uint num2 = 0U;
		if (0U < size)
		{
			do
			{
				num = (uint)(*((((uint)(*(byte*)(num2 / (uint)sizeof(void) + data)) ^ num) & 255U) * 4U + A_0 + 17316) ^ (int)(num >> 8));
				num2 += 1U;
			}
			while (num2 < size);
		}
		return ~num;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00009944 File Offset: 0x00008D44
	internal unsafe static uint DumpPartitions(FfuReader* A_0, _iobuf* fp, uint maxBlockSizeInBytes)
	{
		uint num = 0U;
		uint num2 = (uint)(*(A_0 + 18340));
		if (0U != num2)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0BI@FFBKAKPK@DumpPartitions?$CI?$CJ?5Start?4?$AA@, *(*num2 + 4));
		}
		byte* ptr = <Module>.new[](maxBlockSizeInBytes);
		int num3 = 0;
		uint num4 = 0U;
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
		uint result;
		try
		{
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 16) = 0;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, false, 0U);
			try
			{
				basic_ofstream<char,std::char_traits<char>\u0020> basic_ofstream<char,std::char_traits<char>_u0020>;
				<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{ctor}(ref basic_ofstream<char,std::char_traits<char>_u0020>, 1);
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
					*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 16) = 0;
					*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 20) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, false, 0U);
					try
					{
						uint num5 = 0U;
						int num6 = *(A_0 + 92);
						if (0 < (*(A_0 + 92 + 4) - num6) / 40)
						{
							int num7 = 0;
							do
							{
								bool flag = false;
								uint num8 = 0U;
								int num9 = num7 + num6;
								if (0 < *(num9 + 8))
								{
									FfuReader* partitionEntries = A_0 + 628;
									byte* ptr2 = ptr;
									for (;;)
									{
										num4 = 0U;
										FfuReader.WriteRequest* writeRequest = num9;
										num3 = <Module>.FfuReader.calculateSectorIndexInPartition(A_0, partitionEntries, writeRequest, num8, ref num4);
										basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4;
										basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right;
										if (num3 == -1)
										{
											right = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, (sbyte*)(&<Module>.??_C@_03GKBDPGIH@GPT?$AA@));
											try
											{
												num |= 1U;
												goto IL_157;
											}
											catch
											{
												if ((num & 1U) != 0U)
												{
													num &= 4294967294U;
													<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
												}
												throw;
											}
											goto IL_105;
										}
										goto IL_105;
										IL_157:
										basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5;
										try
										{
											try
											{
												<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, right);
											}
											catch
											{
												if ((num & 2U) != 0U)
												{
													num &= 4294967293U;
													<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
												}
												throw;
											}
											if ((num & 2U) != 0U)
											{
												num &= 4294967293U;
												<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, true, 0U);
											}
										}
										catch
										{
											if ((num & 1U) != 0U)
											{
												num &= 4294967294U;
												<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
											}
											throw;
										}
										if ((num & 1U) != 0U)
										{
											num &= 4294967294U;
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, true, 0U);
										}
										int num10 = (*(ref basic_ofstream<char,std::char_traits<char>_u0020> + 84) != 0) ? 1 : 0;
										if (0 == (byte)num10)
										{
											sbyte* ptr3 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
											uint count;
											if (*(sbyte*)ptr3 == 0)
											{
												count = 0U;
											}
											else
											{
												sbyte* ptr4 = ptr3;
												do
												{
													ptr4 += 1 / sizeof(sbyte);
												}
												while (*(sbyte*)ptr4 != 0);
												count = (uint)(ptr4 - ptr3);
											}
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, ptr3, count);
											sbyte* ptr5 = ref <Module>.??_C@_04GKHLBAIJ@?4bin?$AA@;
											do
											{
												ptr5++;
											}
											while (*ptr5 != 0);
											int count2 = ptr5 - ref <Module>.??_C@_04GKHLBAIJ@?4bin?$AA@;
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_04GKHLBAIJ@?4bin?$AA@), (uint)count2);
											sbyte* filename = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
											<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.open(ref basic_ofstream<char,std::char_traits<char>_u0020>, filename, 34, 64);
											sbyte* ptr6 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
											uint count3;
											if (*(sbyte*)ptr6 == 0)
											{
												count3 = 0U;
											}
											else
											{
												sbyte* ptr7 = ptr6;
												do
												{
													ptr7 += 1 / sizeof(sbyte);
												}
												while (*(sbyte*)ptr7 != 0);
												count3 = (uint)(ptr7 - ptr6);
											}
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ptr6, count3);
										}
										if (flag)
										{
											goto IL_302;
										}
										if (((((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_08KJPBNJGC@Overflow?$AA@)) == 0) ? 1 : 0) == 0) ? 1 : 0) != 0)
										{
											FfuReader.WriteRequest* ptr8 = *(A_0 + 92) + num7;
											if (0 != <Module>._fseeki64(fp, *ptr8, 0))
											{
												goto IL_48B;
											}
											FfuReader.WriteRequest* ptr9 = *(A_0 + 92) + num7;
											if (<Module>.fread((void*)ptr, 512U, (uint)(*(ptr9 + 8)), fp) == *(num7 + *(A_0 + 92) + 8))
											{
												flag = true;
												goto IL_302;
											}
											goto IL_494;
										}
										IL_43A:
										num8 += 1U;
										ptr2 += 512;
										num9 = num7 + *(A_0 + 92);
										if (num8 >= (uint)(*(num9 + 8)))
										{
											break;
										}
										continue;
										IL_302:
										if (((((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_08KJPBNJGC@Overflow?$AA@)) == 0) ? 1 : 0) == 0) ? 1 : 0) == 0)
										{
											goto IL_43A;
										}
										if (((((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2) == 0) ? 1 : 0) == 0) ? 1 : 0) != 0)
										{
											flag = false;
											if (<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.close(ref basic_ofstream<char,std::char_traits<char>_u0020> + 4) == null)
											{
												<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(basic_ofstream<char,std::char_traits<char>_u0020> + 4) + ref basic_ofstream<char,std::char_traits<char>_u0020>, 2, false);
											}
											sbyte* ptr10 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
											uint count4;
											if (*(sbyte*)ptr10 == 0)
											{
												count4 = 0U;
											}
											else
											{
												sbyte* ptr11 = ptr10;
												do
												{
													ptr11 += 1 / sizeof(sbyte);
												}
												while (*(sbyte*)ptr11 != 0);
												count4 = (uint)(ptr11 - ptr10);
											}
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, ptr10, count4);
											sbyte* ptr12 = ref <Module>.??_C@_04GKHLBAIJ@?4bin?$AA@;
											do
											{
												ptr12++;
											}
											while (*ptr12 != 0);
											int count5 = ptr12 - ref <Module>.??_C@_04GKHLBAIJ@?4bin?$AA@;
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_04GKHLBAIJ@?4bin?$AA@), (uint)count5);
											sbyte* filename2 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
											<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.open(ref basic_ofstream<char,std::char_traits<char>_u0020>, filename2, 34, 64);
											sbyte* ptr13 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
											uint count6;
											if (*(sbyte*)ptr13 == 0)
											{
												count6 = 0U;
											}
											else
											{
												sbyte* ptr14 = ptr13;
												do
												{
													ptr14 += 1 / sizeof(sbyte);
												}
												while (*(sbyte*)ptr14 != 0);
												count6 = (uint)(ptr14 - ptr13);
											}
											<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ptr13, count6);
											<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.write(ref basic_ofstream<char,std::char_traits<char>_u0020>, (sbyte*)ptr2, 512L);
											goto IL_43A;
										}
										<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.write(ref basic_ofstream<char,std::char_traits<char>_u0020>, (sbyte*)ptr2, 512L);
										goto IL_43A;
										IL_105:
										FfuReader.partition* right2 = num3 * 64 + *(A_0 + 17268);
										right = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, right2);
										try
										{
											try
											{
												num |= 2U;
											}
											catch
											{
												if ((num & 2U) != 0U)
												{
													num &= 4294967293U;
													<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
												}
												throw;
											}
										}
										catch
										{
											if ((num & 1U) != 0U)
											{
												num &= 4294967294U;
												<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
											}
											throw;
										}
										goto IL_157;
									}
								}
								num5 += 1U;
								num7 += 40;
								num3++;
								num6 = *(A_0 + 92);
							}
							while (num5 < (uint)((*(A_0 + 92 + 4) - num6) / 40));
							goto IL_4A0;
							IL_48B:
							<Module>.delete[]((void*)ptr);
							goto IL_49B;
							IL_494:
							<Module>.delete[]((void*)ptr);
							IL_49B:
							result = 1U;
							goto IL_4D7;
						}
						IL_4A0:
						<Module>.delete[]((void*)ptr);
						num2 = (uint)(*(A_0 + 18340));
						if (0U != num2)
						{
							calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0BG@GBPPGJLC@DumpPartitions?$CI?$CJ?5End?4?$AA@, *(*num2 + 4));
						}
						result = 0U;
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
						throw;
					}
					IL_4D7:
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ofstream<char,std::char_traits<char>\u0020>.__vbaseDtor), (void*)(&basic_ofstream<char,std::char_traits<char>_u0020>));
					throw;
				}
				<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{dtor}(ref basic_ofstream<char,std::char_traits<char>_u0020> + 96);
				<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_ofstream<char,std::char_traits<char>_u0020> + 96);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		return result;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007EDC File Offset: 0x000072DC
	internal unsafe static void __vbaseDtor(basic_ofstream<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_ofstream<char,std::char_traits<char>\u0020>* ptr = A_0 + 96;
		<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{dtor}(ptr);
		<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ptr);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00007EFC File Offset: 0x000072FC
	internal unsafe static uint DumpGpt(FfuReader* A_0, _iobuf* fp)
	{
		uint num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BB@HHMKMHJH@DumpGpt?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
		}
		int num2 = 0;
		uint num3 = 0U;
		int num4 = *(A_0 + 92);
		if (0 < (*(A_0 + 92 + 4) - num4) / 40)
		{
			int num5 = 0;
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
			do
			{
				int num6 = num4 + num5;
				if (*(num6 + 16) == 0L)
				{
					$ArrayType$$$BY0CAA@E $ArrayType$$$BY0CAA@E;
					initblk(ref $ArrayType$$$BY0CAA@E, 0, 512);
					FfuReader.WriteRequest* ptr = num6;
					if (0 != <Module>._fseeki64(fp, *ptr, 0))
					{
						goto IL_292;
					}
					if (1 != <Module>.fread((void*)(&$ArrayType$$$BY0CAA@E), 512U, 1U, fp))
					{
						goto IL_29C;
					}
					EFI_PARTITION_TABLE_HEADER efi_PARTITION_TABLE_HEADER;
					if (1 != <Module>.fread((void*)(&efi_PARTITION_TABLE_HEADER), 512U, 1U, fp))
					{
						goto IL_2A6;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_08BOGKMBPC@EFI?5PART?$AA@));
					try
					{
						sbyte* ptr2 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
						sbyte* ptr3 = ptr2;
						EFI_PARTITION_TABLE_HEADER* ptr4 = &efi_PARTITION_TABLE_HEADER;
						sbyte b = efi_PARTITION_TABLE_HEADER;
						sbyte b2 = *(sbyte*)ptr2;
						if (efi_PARTITION_TABLE_HEADER >= b2)
						{
							while (b <= b2)
							{
								if (b == 0)
								{
									$ArrayType$$$BY0BJ@D $ArrayType$$$BY0BJ@D = 0;
									initblk(ref $ArrayType$$$BY0BJ@D + 1, 0, 24);
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&<Module>.??_C@_03GKBDPGIH@GPT?$AA@));
									try
									{
										<Module>.sprintf((sbyte*)(&$ArrayType$$$BY0BJ@D), (sbyte*)(&<Module>.??_C@_06DPNFGDJN@?$CFd?4bin?$AA@), num2);
										uint count;
										if ($ArrayType$$$BY0BJ@D == null)
										{
											count = 0U;
										}
										else
										{
											sbyte* ptr5 = ref $ArrayType$$$BY0BJ@D;
											do
											{
												ptr5++;
											}
											while (*ptr5 != 0);
											count = ptr5 - ref $ArrayType$$$BY0BJ@D;
										}
										<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&$ArrayType$$$BY0BJ@D), count);
										num2++;
										if (128 != *(ref efi_PARTITION_TABLE_HEADER + 80))
										{
											goto IL_2B0;
										}
										if (128 != *(ref efi_PARTITION_TABLE_HEADER + 84))
										{
											goto IL_2DA;
										}
										$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@ $ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@;
										if (128 != <Module>.fread((void*)(&$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@), 128U, 128U, fp))
										{
											goto IL_304;
										}
										basic_ofstream<char,std::char_traits<char>\u0020> basic_ofstream<char,std::char_traits<char>_u0020>;
										<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{ctor}(ref basic_ofstream<char,std::char_traits<char>_u0020>, 1);
										try
										{
											sbyte* filename = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
											<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.open(ref basic_ofstream<char,std::char_traits<char>_u0020>, filename, 34, 64);
											<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.write(ref basic_ofstream<char,std::char_traits<char>_u0020>, (sbyte*)(&$ArrayType$$$BY0CAA@E), 512L);
											<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.write(ref basic_ofstream<char,std::char_traits<char>_u0020>, (sbyte*)(&efi_PARTITION_TABLE_HEADER), 512L);
											<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.write(ref basic_ofstream<char,std::char_traits<char>_u0020>, (sbyte*)(&$ArrayType$$$BY0IA@UEFI_PARTITION_ENTRY@@), 16384L);
											if (<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.close(ref basic_ofstream<char,std::char_traits<char>_u0020> + 4) == null)
											{
												<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(basic_ofstream<char,std::char_traits<char>_u0020> + 4) + ref basic_ofstream<char,std::char_traits<char>_u0020>, 2, false);
											}
										}
										catch
										{
											<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ofstream<char,std::char_traits<char>\u0020>.__vbaseDtor), (void*)(&basic_ofstream<char,std::char_traits<char>_u0020>));
											throw;
										}
										<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{dtor}(ref basic_ofstream<char,std::char_traits<char>_u0020> + 96);
										<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ref basic_ofstream<char,std::char_traits<char>_u0020> + 96);
									}
									catch
									{
										<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
										throw;
									}
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
									break;
								}
								ptr4 += 1 / sizeof(EFI_PARTITION_TABLE_HEADER);
								ptr3 += 1 / sizeof(sbyte);
								b = *(sbyte*)ptr4;
								b2 = *(sbyte*)ptr3;
								if (b < b2)
								{
									break;
								}
							}
						}
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
				}
				num3 += 1U;
				num5 += 40;
				num4 = *(A_0 + 92);
			}
			while (num3 < (uint)((*(A_0 + 92 + 4) - num4) / 40));
			goto IL_352;
			IL_292:
			<Module>.fclose(fp);
			return 10;
			IL_29C:
			<Module>.fclose(fp);
			return 10;
			IL_2A6:
			<Module>.fclose(fp);
			return 10;
			IL_2B0:
			try
			{
				try
				{
					<Module>.fclose(fp);
					goto IL_32C;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			IL_2DA:
			try
			{
				try
				{
					<Module>.fclose(fp);
					goto IL_32C;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			IL_304:
			try
			{
				try
				{
					<Module>.fclose(fp);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			IL_32C:
			try
			{
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
			return 10;
		}
		IL_352:
		num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0P@LHKOCCJE@DumpGpt?$CI?$CJ?5End?4?$AA@, *(*num + 4));
		}
		return 0;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00003580 File Offset: 0x00002980
	internal unsafe static sbyte* ffu_get_hash_by_index(FfuReader* A_0, sbyte* hashTable, uint dwHashIndex)
	{
		sbyte* result = null;
		if (0 != A_0 + 17284 && dwHashIndex < (uint)(*(A_0 + 17312)) >> 5)
		{
			result = dwHashIndex * 32U / (uint)sizeof(sbyte) + hashTable;
		}
		return result;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00008378 File Offset: 0x00007778
	internal unsafe static FfuReaderResult* integrityCheck(FfuReader* A_0, FfuReaderResult* A_1, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename, bool* terminate)
	{
		try
		{
			uint num = 0U;
			int num2 = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
			try
			{
				uint num3 = (uint)(*(A_0 + 18340));
				if (0U != num3)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num3, ref <Module>.??_C@_0BI@MDMCKOFE@integrityCheck?$CI?$CJ?5Start?4?$AA@, *(*num3 + 4));
				}
				FfuReaderResult* ptr = A_1 + 4 / sizeof(FfuReaderResult);
				basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = ptr;
				*(ptr2 + 16) = 0;
				*(ptr2 + 20) = 0;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2, false, 0U);
				num = 1U;
				uint num4 = 0U;
				sbyte* ptr3 = null;
				sbyte* ptr4 = null;
				ulong num5 = 0UL;
				bool flag = false;
				if (null != terminate)
				{
					flag = (*(byte*)terminate != 0);
				}
				*(int*)A_1 = 0;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_00CNPNBAHC@?$AA@), 0U);
				_iobuf* ptr5 = <Module>.fopen((sbyte*)((16 > *(int*)(filename + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>))) ? filename : (*(int*)filename)), (sbyte*)(&<Module>.??_C@_02JDPG@rb?$AA@));
				if (null != ptr5)
				{
					FfuReaderResult ffuReaderResult8;
					try
					{
						FfuReaderResult ffuReaderResult;
						FfuReaderResult* ptr6 = <Module>.FfuReader.readSecurityHdrAndCheckValidity(A_0, &ffuReaderResult, ptr5);
						try
						{
							*(int*)A_1 = *(int*)ptr6;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(ptr, ptr6 + 4 / sizeof(FfuReaderResult));
						}
						catch
						{
							<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult));
							throw;
						}
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult + 4, true, 0U);
						int num6 = *(int*)A_1;
						if (0 != num6)
						{
							FfuReaderResult ffuReaderResult2 = num6;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult2 + 4, ptr);
							<Module>._CxxThrowException((void*)(&ffuReaderResult2), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
						}
						long num7 = <Module>.?A0xd5e524cb.time(null);
						uint num8 = (uint)(*(A_0 + 17300) * 1024);
						*(A_0 + 17036) = (int)num8;
						*(A_0 + 17256) = <Module>._ftelli64(ptr5) + (ulong)(*(A_0 + 17312)) + (ulong)(*(A_0 + 17308));
						long num9 = *(A_0 + 17256);
						long num10 = (long)((ulong)num8);
						long num11 = num9 % num10;
						if (num11 != 0L)
						{
							*(A_0 + 17256) = num10 - num11 + num9;
						}
						<Module>._fseeki64(ptr5, 0L, 2);
						ulong num12 = <Module>._ftelli64(ptr5);
						num12 -= (ulong)(*(A_0 + 17256));
						ulong num13 = num12;
						if (0 != <Module>._fseeki64(ptr5, (long)((ulong)(*(A_0 + 17284) + *(A_0 + 17308))), 0))
						{
							*(int*)A_1 = 4;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_0EB@NNMJIEIB@Corrupted?5FFU?0?5cannot?5seek?5to?5be@), filename);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
							FfuReaderResult ffuReaderResult3 = *(int*)A_1;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult3 + 4, ptr);
							<Module>._CxxThrowException((void*)(&ffuReaderResult3), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
						}
						sbyte* ptr7 = <Module>.new[]((uint)(*(A_0 + 17312)));
						ptr4 = ptr7;
						if (1 != <Module>.fread((void*)ptr7, (uint)(*(A_0 + 17312)), 1U, ptr5))
						{
							*(int*)A_1 = 4;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right2 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, (sbyte*)(&<Module>.??_C@_0DC@DBIHALHK@Corrupted?5FFU?0?5could?5not?5read?5FF@), filename);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right2);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
							FfuReaderResult ffuReaderResult4 = *(int*)A_1;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult4 + 4, ptr);
							<Module>._CxxThrowException((void*)(&ffuReaderResult4), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
						}
						if (0 != <Module>._fseeki64(ptr5, *(A_0 + 17256), 0))
						{
							*(int*)A_1 = 4;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
							basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right3 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, (sbyte*)(&<Module>.??_C@_0DJ@BLCILKMJ@Corrupted?5FFU?0?5cannot?5seek?5to?5st@), filename);
							try
							{
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right3);
							}
							catch
							{
								<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
								throw;
							}
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
							FfuReaderResult ffuReaderResult5 = *(int*)A_1;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult5 + 4, ptr);
							<Module>._CxxThrowException((void*)(&ffuReaderResult5), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
						}
						NC_SHA256_CTX nc_SHA256_CTX;
						<Module>.sha256_init(&nc_SHA256_CTX);
						uint num14;
						uint num15;
						if (52428800UL < num12)
						{
							num14 = (uint)(*(A_0 + 17036));
							num15 = 52428800U / num14;
						}
						else
						{
							num14 = (uint)(*(A_0 + 17036));
							num15 = (uint)num12 / num14;
						}
						num15 = num14 * num15;
						ptr3 = <Module>.new[](num15);
						while (num12 > 0UL && false == flag)
						{
							if (num12 < (ulong)num15)
							{
								num15 = (uint)num12;
							}
							if (1 != <Module>.fread((void*)ptr3, num15, 1U, ptr5))
							{
								*(int*)A_1 = 4;
								basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4;
								basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right4 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, (sbyte*)(&<Module>.??_C@_0DC@DBIHALHK@Corrupted?5FFU?0?5could?5not?5read?5FF@), filename);
								try
								{
									ptr = A_1 + 4 / sizeof(FfuReaderResult);
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right4);
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4));
									throw;
								}
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>4, true, 0U);
								FfuReaderResult ffuReaderResult6 = *(int*)A_1;
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref ffuReaderResult6 + 4, ptr);
								<Module>._CxxThrowException((void*)(&ffuReaderResult6), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
							}
							uint num16 = 0U;
							while (num16 < num15)
							{
								<Module>.sha256_update(&nc_SHA256_CTX, (sbyte*)(num16 / (uint)sizeof(sbyte) + ptr3), (uint)(*(A_0 + 17036)));
								$ArrayType$$$BY07I $ArrayType$$$BY07I;
								<Module>.sha256_final(&nc_SHA256_CTX, ref $ArrayType$$$BY07I);
								uint num17 = num4;
								num4 += 1U;
								sbyte* ptr8 = null;
								uint num18 = (uint)(*(A_0 + 17312)) >> 5;
								if (0 != A_0 + 17284 && num17 < num18)
								{
									ptr8 = num17 * 32U / (uint)sizeof(sbyte) + ptr4;
								}
								uint num19 = 32U;
								sbyte* ptr9 = ptr8;
								$ArrayType$$$BY07I* ptr10 = &$ArrayType$$$BY07I;
								int num20 = 0;
								for (;;)
								{
									byte b = *(byte*)ptr10;
									byte b2 = *(byte*)ptr9;
									if (b < b2 || b > b2)
									{
										goto IL_461;
									}
									if (num19 == 1U)
									{
										goto IL_45C;
									}
									num19 -= 1U;
									ptr10 += 1 / sizeof($ArrayType$$$BY07I);
									ptr9 += 1 / sizeof(sbyte);
								}
								IL_4AC:
								num14 = (uint)(*(A_0 + 17036));
								num16 = num14 + num16;
								uint num21 = (uint)(*(A_0 + 17280));
								if (num21 != 0U)
								{
									num5 += (ulong)num14;
									calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt64,System.UInt64), num21, num13, num5, *(*num21 + 4));
								}
								continue;
								IL_45C:
								if (0 == num20)
								{
									goto IL_4AC;
								}
								IL_461:
								*(int*)A_1 = 4;
								basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5;
								basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right5 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, (sbyte*)(&<Module>.??_C@_0CI@CGNAALBM@Corrupted?5FFU?0?5hash?5mismatch?0?5fi@), filename);
								try
								{
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_1 + 4 / sizeof(FfuReaderResult), right5);
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5));
									throw;
								}
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>5, true, 0U);
								FfuReaderResult ffuReaderResult7;
								<Module>.FfuReaderResult.{ctor}(ref ffuReaderResult7, A_1);
								<Module>._CxxThrowException((void*)(&ffuReaderResult7), (_s__ThrowInfo*)(&<Module>._TI1?AUFfuReaderResult@@));
								goto IL_4AC;
							}
							num12 -= (ulong)num15;
							if (null != terminate)
							{
								flag = (*(byte*)terminate != 0);
							}
						}
						if (true == flag)
						{
							*(int*)A_1 = 11;
							sbyte* ptr11 = ref <Module>.??_C@_0CI@PNDJCJNO@Calculation?5of?5hash?5terminated?5b@;
							while (*ptr11 != 0)
							{
								ptr11++;
							}
							int count = ptr11 - ref <Module>.??_C@_0CI@PNDJCJNO@Calculation?5of?5hash?5terminated?5b@;
							<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_1 + 4 / sizeof(FfuReaderResult), (sbyte*)(&<Module>.??_C@_0CI@PNDJCJNO@Calculation?5of?5hash?5terminated?5b@), (uint)count);
						}
					}
					catch when (endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), (void*)(&<Module>.??_R0?AUFfuReaderResult@@@8), 0, (void*)(&ffuReaderResult8)) != null))
					{
						uint num22 = 0U;
						<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num2);
						try
						{
							try
							{
								try
								{
									*(int*)A_1 = ffuReaderResult8;
									<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.=(A_1 + 4 / sizeof(FfuReaderResult), ref ffuReaderResult8 + 4);
								}
								catch
								{
									<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)(&ffuReaderResult8));
									throw;
								}
								<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref ffuReaderResult8 + 4, true, 0U);
								goto IL_5BA;
							}
							catch when (delegate
							{
								// Failed to create a 'catch-when' expression
								num22 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
								endfilter(num22 != 0U);
							})
							{
							}
							if (num22 != 0U)
							{
								throw;
							}
						}
						finally
						{
							<Module>.__CxxUnregisterExceptionObject(num2, (int)num22);
						}
					}
					IL_5BA:
					if (null != ptr5)
					{
						<Module>.fclose(ptr5);
					}
					if (null != ptr3)
					{
						<Module>.delete[]((void*)ptr3);
					}
					if (null != ptr4)
					{
						<Module>.delete[]((void*)ptr4);
					}
				}
				else
				{
					*(int*)A_1 = 2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right6 = <Module>.std.operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, (sbyte*)(&<Module>.??_C@_0CD@GAPHJKMF@Could?5not?5open?5FFU?5file?0?5filenam@), filename);
					try
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, right6);
					}
					catch
					{
						<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6));
						throw;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>6, true, 0U);
				}
				num3 = (uint)(*(A_0 + 18340));
				if (0U != num3)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num3, ref <Module>.??_C@_0BG@NGLINCBG@integrityCheck?$CI?$CJ?5End?4?$AA@, *(*num3 + 4));
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(filename, true, 0U);
			return A_1;
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)filename);
				throw;
			}
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00006D0C File Offset: 0x0000610C
	internal unsafe static void checkDppPartition(FfuReader* A_0)
	{
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, (sbyte*)(&<Module>.??_C@_03BBDBFBBB@dpp?$AA@));
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
		try
		{
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 16) = 0;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, false, 0U);
			try
			{
				uint num = (uint)(*(A_0 + 18340));
				if (0U != num)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BL@EMPFHFAH@checkDppPartition?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
				}
				uint num2 = 0U;
				if (0 < *(A_0 + 196))
				{
					int num3 = 0;
					while (((((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(num3 + *(A_0 + 17268), (sbyte*)(&<Module>.??_C@_08KJPBNJGC@Overflow?$AA@)) == 0) ? 1 : 0) == 0) ? 1 : 0) != 0)
					{
						basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = num3 + *(A_0 + 17268);
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, right, 0U, uint.MaxValue);
						<Module>.stringToLower(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2);
						if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>) == null)
						{
							uint num4 = 0U;
							int num5 = *(A_0 + 92);
							int i = (*(A_0 + 92 + 4) - num5) / 40;
							if (0 < i)
							{
								int num6 = 0;
								while (i > (int)num4)
								{
									FfuReader.WriteRequest* ptr = num5 + num6;
									int num7 = *(A_0 + 17268);
									if (*(A_0 + 17268 + 4) - num7 >> 6 <= (int)num2)
									{
										IL_177:
										<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
										goto IL_181;
									}
									FfuReader.partition* ptr2 = num7 + num3;
									if (*(ptr + 16) >= *(ptr2 + 24))
									{
										int num8 = num5 + num6;
										FfuReader.partition* ptr3 = num7 + num3;
										if (*(num8 + 16) <= *(ptr3 + 32))
										{
											*(A_0 + 17264) = 1;
										}
									}
									num4 += 1U;
									num6 += 40;
									num5 = *(A_0 + 92);
									i = (*(A_0 + 92 + 4) - num5) / 40;
									if (num4 >= (uint)i)
									{
										goto IL_14E;
									}
								}
								<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
								goto IL_177;
							}
						}
						IL_14E:
						num2 += 1U;
						num3 += 64;
						if (num2 >= (uint)(*(A_0 + 196)))
						{
							break;
						}
					}
				}
				IL_181:
				num = (uint)(*(A_0 + 18340));
				if (0U != num)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BJ@PLPGDIKF@checkDppPartition?$CI?$CJ?5End?4?$AA@, *(*num + 4));
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		try
		{
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
				throw;
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x000035B4 File Offset: 0x000029B4
	internal unsafe static void trace(FfuReader* A_0, sbyte* msg)
	{
		uint num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, msg, *(*num + 4));
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x000035E0 File Offset: 0x000029E0
	internal unsafe static void traceError(FfuReader* A_0, sbyte* msg)
	{
		uint num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, msg, *(*num + 8));
		}
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00006430 File Offset: 0x00005830
	internal unsafe static FfuReaderResult* readImageId(FfuReader* A_0, FfuReaderResult* A_1, _iobuf* fp, uint* imageId)
	{
		try
		{
			uint num = 0U;
			FfuReaderResult* ptr = A_1 + 4 / sizeof(FfuReaderResult);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = ptr;
			*(ptr2 + 16) = 0;
			*(ptr2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2, false, 0U);
			num = 1U;
			*(int*)A_1 = 0;
			sbyte* ptr3 = ref <Module>.??_C@_0BB@CMIDDDDM@ImageId?5read?5ok?4?$AA@;
			do
			{
				ptr3++;
			}
			while (*ptr3 != 0);
			int count = ptr3 - ref <Module>.??_C@_0BB@CMIDDDDM@ImageId?5read?5ok?4?$AA@;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_0BB@CMIDDDDM@ImageId?5read?5ok?4?$AA@), (uint)count);
			uint num2 = (uint)(*(A_0 + 18340));
			if (0U != num2)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0BF@FCPPKGEC@readImageId?$CI?$CJ?5Start?4?$AA@, *(*num2 + 4));
			}
			$ArrayType$$$BY07E $ArrayType$$$BY07E = 0;
			*(ref $ArrayType$$$BY07E + 1) = 0;
			*(ref $ArrayType$$$BY07E + 2) = 0;
			*(ref $ArrayType$$$BY07E + 3) = 0;
			*(ref $ArrayType$$$BY07E + 4) = 0;
			*(ref $ArrayType$$$BY07E + 5) = 0;
			*(ref $ArrayType$$$BY07E + 6) = 0;
			*(ref $ArrayType$$$BY07E + 7) = 0;
			if (<Module>.fread((void*)(&$ArrayType$$$BY07E), 1U, 8U, fp) != 8)
			{
				num2 = (uint)(*(A_0 + 18340));
				if (0U != num2)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0CI@FEMMPJOB@readImageId?0?5Unable?5to?5read?5file@, *(*num2 + 8));
				}
				*(int*)A_1 = 10;
				sbyte* ptr4 = ref <Module>.??_C@_0CI@FEMMPJOB@readImageId?0?5Unable?5to?5read?5file@;
				do
				{
					ptr4++;
				}
				while (*ptr4 != 0);
				int count2 = ptr4 - ref <Module>.??_C@_0CI@FEMMPJOB@readImageId?0?5Unable?5to?5read?5file@;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_0CI@FEMMPJOB@readImageId?0?5Unable?5to?5read?5file@), (uint)count2);
			}
			else
			{
				uint num3 = (uint)(((int)(*(ref $ArrayType$$$BY07E + 3)) << 8 | (int)(*(ref $ArrayType$$$BY07E + 2))) << 8 | (int)(*(ref $ArrayType$$$BY07E + 1))) << 8 | $ArrayType$$$BY07E;
				if (((ulong)((((int)(*(ref $ArrayType$$$BY07E + 7)) << 8 | (int)(*(ref $ArrayType$$$BY07E + 6))) << 8 | (int)(*(ref $ArrayType$$$BY07E + 5))) << 8 | (int)(*(ref $ArrayType$$$BY07E + 4))) << 32 | (ulong)num3) == 7452728838522632820UL)
				{
					num2 = (uint)(*(A_0 + 18340));
					if (0U != num2)
					{
						calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num2, ref <Module>.??_C@_0BO@EIIEAOIB@Encrypted?5image?5not?5supported?$AA@, *(*num2 + 8));
					}
					*(int*)A_1 = 13;
					sbyte* ptr5 = ref <Module>.??_C@_0CC@DMHIICGH@Encrypted?5image?5is?5not?5supported@;
					do
					{
						ptr5++;
					}
					while (*ptr5 != 0);
					int count3 = ptr5 - ref <Module>.??_C@_0CC@DMHIICGH@Encrypted?5image?5is?5not?5supported@;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ptr, (sbyte*)(&<Module>.??_C@_0CC@DMHIICGH@Encrypted?5image?5is?5not?5supported@), (uint)count3);
				}
				else
				{
					*imageId = (int)num3;
				}
			}
			<Module>._fseeki64(fp, 0L, 0);
			uint num4 = (uint)(*(A_0 + 18340));
			if (0U != num4)
			{
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num4, ref <Module>.??_C@_0BD@BAEILGK@readImageId?$CI?$CJ?5End?4?$AA@, *(*num4 + 4));
			}
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(FfuReaderResult.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000360C File Offset: 0x00002A0C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool isValidBootImage(FfuReader* A_0, uint* imageId)
	{
		bool result = false;
		uint num = (uint)(*(A_0 + 18340));
		if (0U != num)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num, ref <Module>.??_C@_0BK@CONNOIJG@isValidBootImage?$CI?$CJ?5Start?4?$AA@, *(*num + 4));
		}
		uint num2 = (uint)(*imageId);
		if ((num2 > 0U && num2 < 28U) || num2 == 2219564241U)
		{
			result = true;
		}
		uint num3 = (uint)(*(A_0 + 18340));
		if (0U != num3)
		{
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.SByte modopt(System.Runtime.CompilerServices.IsSignUnspecifiedByte) modopt(System.Runtime.CompilerServices.IsConst)*), num3, ref <Module>.??_C@_0BI@HEHMDDPC@isValidBootImage?$CI?$CJ?5End?4?$AA@, *(*num3 + 4));
		}
		return result;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00006F78 File Offset: 0x00006378
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		if (A_0 != _Right)
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_0, _Right);
		}
		return A_0;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000663C File Offset: 0x00005A3C
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* =(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		if (A_0 != _Right)
		{
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Right, 0U, uint.MaxValue);
		}
		return A_0;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00006658 File Offset: 0x00005A58
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* =(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Ptr, count);
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00005A80 File Offset: 0x00004E80
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* append(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Ptr, count);
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00005AAC File Offset: 0x00004EAC
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Right, 0U, uint.MaxValue);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004FDC File Offset: 0x000043DC
	internal unsafe static void clear(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		*(A_0 + 16) = 0;
		*((16 > *(A_0 + 20)) ? A_0 : (*A_0)) = 0;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00004890 File Offset: 0x00003C90
	internal unsafe static sbyte* data(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		return (16 > *(A_0 + 20)) ? A_0 : (*A_0);
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00005000 File Offset: 0x00004400
	internal unsafe static int compare(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		uint count = (uint)(*(_Right + 16));
		sbyte* ptr;
		if (16 <= *(_Right + 20))
		{
			ptr = *_Right;
		}
		else
		{
			ptr = _Right;
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(A_0, 0U, (uint)(*(A_0 + 16)), ptr, count);
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005034 File Offset: 0x00004434
	internal unsafe static int compare(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(A_0, 0U, (uint)(*(A_0 + 16)), _Ptr, count);
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000048AC File Offset: 0x00003CAC
	internal unsafe static vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* {ctor}(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00006684 File Offset: 0x00005A84
	internal unsafe static void {dtor}(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>._Tidy(A_0);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00003674 File Offset: 0x00002A74
	internal unsafe static uint size(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return (*(A_0 + 4) - *A_0) / 40;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00003FD4 File Offset: 0x000033D4
	internal unsafe static FfuReader.WriteRequest* at(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Pos)
	{
		int num = *A_0;
		if ((*(A_0 + 4) - num) / 40 <= (int)_Pos)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
		}
		return _Pos * 40U + (uint)num;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000368C File Offset: 0x00002A8C
	internal unsafe static FfuReader.WriteRequest* [](vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Pos)
	{
		return _Pos * 40U + (uint)(*A_0);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00006F9C File Offset: 0x0000639C
	internal unsafe static void push_back(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Val)
	{
		uint num = (uint)(*(A_0 + 4));
		int num2;
		if (_Val < num && *A_0 <= _Val)
		{
			num2 = 1;
		}
		else
		{
			num2 = 0;
		}
		if ((byte)num2 != 0)
		{
			FfuReader.WriteRequest* ptr = (_Val - *A_0) / 40;
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.WriteRequest* ptr2 = ptr * 40 + *A_0;
			FfuReader.WriteRequest* ptr3 = *(A_0 + 4);
			FfuReader.WriteRequest* a_ = ptr3;
			try
			{
				if (ptr3 != null)
				{
					cpblk(ptr3, ptr2, 40);
				}
			}
			catch
			{
				<Module>.delete((void*)a_, (void*)ptr3);
				throw;
			}
			*(A_0 + 4) = *(A_0 + 4) + 40;
		}
		else
		{
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.WriteRequest* ptr4 = *(A_0 + 4);
			if (ptr4 != null)
			{
				cpblk(ptr4, _Val, 40);
			}
			*(A_0 + 4) = *(A_0 + 4) + 40;
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000048C8 File Offset: 0x00003CC8
	internal unsafe static vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* {ctor}(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00006698 File Offset: 0x00005A98
	internal unsafe static void {dtor}(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>._Tidy(A_0);
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00007050 File Offset: 0x00006450
	internal unsafe static void push_back(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Val)
	{
		uint num = (uint)(*(A_0 + 4));
		int num2;
		if (_Val < num && *A_0 <= _Val)
		{
			num2 = 1;
		}
		else
		{
			num2 = 0;
		}
		if ((byte)num2 != 0)
		{
			FfuReader.BlockDataEntry* ptr = (_Val - *A_0) / 24;
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.BlockDataEntry* ptr2 = ptr * 24 + *A_0;
			FfuReader.BlockDataEntry* ptr3 = *(A_0 + 4);
			FfuReader.BlockDataEntry* a_ = ptr3;
			try
			{
				if (ptr3 != null)
				{
					cpblk(ptr3, ptr2, 24);
				}
			}
			catch
			{
				<Module>.delete((void*)a_, (void*)ptr3);
				throw;
			}
			*(A_0 + 4) = *(A_0 + 4) + 24;
		}
		else
		{
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.BlockDataEntry* ptr4 = *(A_0 + 4);
			if (ptr4 != null)
			{
				cpblk(ptr4, _Val, 24);
			}
			*(A_0 + 4) = *(A_0 + 4) + 24;
		}
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000048E4 File Offset: 0x00003CE4
	internal unsafe static vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* {ctor}(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x0000A168 File Offset: 0x00009568
	internal unsafe static void {dtor}(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Tidy(A_0);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00004000 File Offset: 0x00003400
	internal unsafe static FfuReader.partition* at(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, uint _Pos)
	{
		int num = *A_0;
		if (*(A_0 + 4) - num >> 6 <= (int)_Pos)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
		}
		return _Pos * 64U + (uint)num;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000036A0 File Offset: 0x00002AA0
	internal unsafe static FfuReader.partition* [](vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, uint _Pos)
	{
		return _Pos * 64U + (uint)(*A_0);
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000A5C0 File Offset: 0x000099C0
	internal unsafe static void push_back(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Val)
	{
		uint num = (uint)(*(A_0 + 4));
		int num2;
		if (_Val < num && *A_0 <= _Val)
		{
			num2 = 1;
		}
		else
		{
			num2 = 0;
		}
		if ((byte)num2 != 0)
		{
			FfuReader.partition* ptr = _Val - *A_0 >> 6;
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.partition* a_ = ptr * 64 + *A_0;
			FfuReader.partition* ptr2 = *(A_0 + 4);
			FfuReader.partition* a_2 = ptr2;
			try
			{
				if (ptr2 != null)
				{
					<Module>.FfuReader.partition.{ctor}(ptr2, a_);
				}
			}
			catch
			{
				<Module>.delete((void*)a_2, (void*)ptr2);
				throw;
			}
			*(A_0 + 4) = *(A_0 + 4) + 64;
		}
		else
		{
			if (num == (uint)(*(A_0 + 8)))
			{
				<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Reserve(A_0, 1U);
			}
			FfuReader.partition* ptr3 = *(A_0 + 4);
			FfuReader.partition* a_3 = ptr3;
			try
			{
				if (ptr3 != null)
				{
					<Module>.FfuReader.partition.{ctor}(ptr3, _Val);
				}
			}
			catch
			{
				<Module>.delete((void*)a_3, (void*)ptr3);
				throw;
			}
			*(A_0 + 4) = *(A_0 + 4) + 64;
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00005068 File Offset: 0x00004468
	internal unsafe static basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, int _Mode, int A_2)
	{
		uint num = 0U;
		if (A_2 != 0)
		{
			*A_0 = ref <Module>.??_8?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@7B?$basic_istream@DU?$char_traits@D@std@@@1@@;
			*(A_0 + 16) = ref <Module>.??_8?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@7B?$basic_ostream@DU?$char_traits@D@std@@@1@@;
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{ctor}(A_0 + 104);
			try
			{
				num = 1U;
			}
			catch
			{
				if ((num & 1U) != 0U)
				{
					num &= 4294967294U;
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 104);
				}
				throw;
			}
		}
		try
		{
			basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 + 24;
			<Module>.std.basic_iostream<char,std::char_traits<char>\u0020>.{ctor}(A_0, ptr, 0);
			try
			{
				*(*(*A_0 + 4) + A_0) = ref <Module>.??_7?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
				int num2 = *(*A_0 + 4);
				*(A_0 + num2 - 4) = num2 - 104;
				<Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ptr, _Mode);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_iostream<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 32);
				throw;
			}
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 104);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00004900 File Offset: 0x00003D00
	internal unsafe static void {dtor}(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 - 104;
		*(A_0 + *(*ptr + 4) - 104) = ref <Module>.??_7?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
		int num = *(*ptr + 4);
		*(A_0 + num - 108) = num - 104;
		try
		{
			basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2 = A_0 - 80;
			*ptr2 = ref <Module>.??_7?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
			try
			{
				<Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr2);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), ptr2);
				throw;
			}
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}(ptr2);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_iostream<char,std::char_traits<char>\u0020>.{dtor}), A_0 - 104 + 32);
			throw;
		}
		<Module>.std.basic_iostream<char,std::char_traits<char>\u0020>.{dtor}(A_0 - 72);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00007104 File Offset: 0x00006504
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* str(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1)
	{
		uint num = 0U;
		<Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>.str(A_0 + 24, A_1);
		try
		{
			num = 1U;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
			}
			throw;
		}
		return A_1;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000402C File Offset: 0x0000342C
	internal unsafe static void {dtor}(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		*A_0 = ref <Module>.??_7?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
		try
		{
			<Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), A_0);
			throw;
		}
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}(A_0);
	}

	// Token: 0x060000DA RID: 218 RVA: 0x0000D2BC File Offset: 0x0000C6BC
	internal unsafe static int overflow(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, int _Meta)
	{
		int num = *(A_0 + 60);
		if ((num & 2) != 0)
		{
			return -1;
		}
		if (((-1 == _Meta) ? 1 : 0) != 0)
		{
			return (_Meta != -1) ? _Meta : 0;
		}
		if ((num & 8) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) < *(A_0 + 56))
		{
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0), *(A_0 + 56), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0));
		}
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0))
		{
			*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Pninc(A_0) = (byte)_Meta;
			return _Meta;
		}
		int num2;
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) == null)
		{
			num2 = 0;
		}
		else
		{
			num2 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
		}
		uint num3 = (uint)num2 >> 1;
		uint num4 = (num3 < 32U) ? 32U : num3;
		uint num5 = num4;
		if (0U < num4)
		{
			while (2147483647U - num5 < (uint)num2)
			{
				num5 >>= 1;
				if (0U >= num5)
				{
					break;
				}
			}
		}
		if (num5 == 0U)
		{
			return -1;
		}
		uint num6 = num5 + (uint)num2;
		void* ptr = null;
		if (num6 != 0U)
		{
			if (4294967295U >= num6)
			{
				ptr = <Module>.@new(num6);
				if (ptr != null)
				{
					goto IL_DA;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		IL_DA:
		sbyte* ptr2 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
		if (0 < num2)
		{
			if (num2 == 0)
			{
				goto IL_F2;
			}
			cpblk(ptr, ptr2, num2);
		}
		if (num2 != 0)
		{
			*(A_0 + 56) = (int)((IntPtr)(*(A_0 + 56)) + (ptr - ptr2));
			int num7 = (int)(ptr - ptr2);
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0) + num7, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) + num7, (sbyte*)((byte*)ptr + num6));
			if ((*(A_0 + 60) & 4) != 0)
			{
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr, null, (sbyte*)ptr);
				goto IL_17F;
			}
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) + num7, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) + 1);
			goto IL_17F;
		}
		IL_F2:
		*(A_0 + 56) = ptr;
		void* ptr3 = ptr;
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, (sbyte*)ptr3, (sbyte*)((byte*)ptr3 + num6));
		if ((*(A_0 + 60) & 4) != 0)
		{
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr, null, (sbyte*)ptr);
		}
		else
		{
			void* ptr4 = ptr;
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr4, (sbyte*)ptr4, (sbyte*)((byte*)ptr + 1));
		}
		IL_17F:
		if ((*(A_0 + 60) & 1) != 0)
		{
			<Module>.delete((void*)ptr2);
		}
		*(A_0 + 60) = (*(A_0 + 60) | 1);
		*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Pninc(A_0) = (byte)_Meta;
		return _Meta;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x0000D4A4 File Offset: 0x0000C8A4
	internal unsafe static int pbackfail(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, int _Meta)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0))
		{
			if (((-1 == _Meta) ? 1 : 0) == 0)
			{
				sbyte* ptr = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) - 1;
				if ((((sbyte)_Meta == *(sbyte*)ptr) ? 1 : 0) == 0 && (*(A_0 + 60) & 2) != 0)
				{
					return -1;
				}
			}
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gbump(A_0, -1);
			if (((-1 == _Meta) ? 1 : 0) == 0)
			{
				*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) = (byte)_Meta;
			}
			return (_Meta != -1) ? _Meta : 0;
		}
		return -1;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x0000D528 File Offset: 0x0000C928
	internal unsafe static int underflow(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) == null)
		{
			return -1;
		}
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0))
		{
			return (byte)(*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0));
		}
		if ((*(A_0 + 60) & 4) == 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) || *(A_0 + 56) > <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0)))
		{
			if (*(A_0 + 56) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0))
			{
				*(A_0 + 56) = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0);
			}
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0), *(A_0 + 56));
			return (byte)(*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0));
		}
		return -1;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000D5D4 File Offset: 0x0000C9D4
	internal unsafe static fpos<int>* seekoff(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, fpos<int>* A_1, long _Off, int _Way, int _Which)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && *(A_0 + 56) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0))
		{
			*(A_0 + 56) = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0);
		}
		if ((_Which & 1) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null)
		{
			if (_Way == 2)
			{
				_Off += *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			}
			else
			{
				if (_Way == 1)
				{
					if ((_Which & 2) == 0)
					{
						_Off += <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
						goto IL_75;
					}
				}
				else if (_Way == 0)
				{
					goto IL_75;
				}
				_Off = *(long*)<Module>.__imp_std._BADOFF;
			}
			IL_75:
			if (0L <= _Off && _Off <= *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0))
			{
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gbump(A_0, (int)(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) + _Off));
				if ((_Which & 2) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
				{
					<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0));
				}
			}
			else
			{
				_Off = *(long*)<Module>.__imp_std._BADOFF;
			}
		}
		else if ((_Which & 2) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
		{
			if (_Way == 2)
			{
				_Off += *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			}
			else if (_Way == 1)
			{
				_Off += <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			}
			else if (_Way != 0)
			{
				_Off = *(long*)<Module>.__imp_std._BADOFF;
			}
			if (0L <= _Off && _Off <= *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0))
			{
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbump(A_0, (int)(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) + _Off));
			}
			else
			{
				_Off = *(long*)<Module>.__imp_std._BADOFF;
			}
		}
		else if (_Off != 0L)
		{
			_Off = *(long*)<Module>.__imp_std._BADOFF;
		}
		*(long*)A_1 = _Off;
		*(long*)(A_1 + 8 / sizeof(fpos<int>)) = 0L;
		*(int*)(A_1 + 16 / sizeof(fpos<int>)) = 0;
		return A_1;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000D78C File Offset: 0x0000CB8C
	internal unsafe static fpos<int>* seekpos(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, fpos<int>* A_1, fpos<int> _Ptr, int _Mode)
	{
		long num = *(ref _Ptr + 8) + _Ptr;
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && *(A_0 + 56) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0))
		{
			*(A_0 + 56) = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0);
		}
		if (num != *(long*)<Module>.__imp_std._BADOFF)
		{
			if ((_Mode & 1) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null)
			{
				if (0L <= num && num <= *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0))
				{
					<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gbump(A_0, (int)(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) + num));
					if ((_Mode & 2) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
					{
						<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0), <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0));
					}
				}
				else
				{
					num = *(long*)<Module>.__imp_std._BADOFF;
				}
			}
			else if ((_Mode & 2) != 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
			{
				if (0L <= num && num <= *(A_0 + 56) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0))
				{
					<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbump(A_0, (int)(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) + num));
				}
				else
				{
					num = *(long*)<Module>.__imp_std._BADOFF;
				}
			}
			else
			{
				num = *(long*)<Module>.__imp_std._BADOFF;
			}
		}
		*(long*)A_1 = num;
		*(long*)(A_1 + 8 / sizeof(fpos<int>)) = 0L;
		*(int*)(A_1 + 16 / sizeof(fpos<int>)) = 0;
		return A_1;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00007158 File Offset: 0x00006558
	internal unsafe static basic_ofstream<char,std::char_traits<char>\u0020>* {ctor}(basic_ofstream<char,std::char_traits<char>\u0020>* A_0, int A_1)
	{
		uint num = 0U;
		if (A_1 != 0)
		{
			*A_0 = ref <Module>.??_8?$basic_ofstream@DU?$char_traits@D@std@@@std@@7B@;
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{ctor}(A_0 + 96);
			try
			{
				num = 1U;
			}
			catch
			{
				if ((num & 1U) != 0U)
				{
					num &= 4294967294U;
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 96);
				}
				throw;
			}
		}
		try
		{
			basic_ofstream<char,std::char_traits<char>\u0020>* ptr = A_0 + 4;
			<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.{ctor}(A_0, ptr, false, 0);
			try
			{
				*(*(*A_0 + 4) + A_0) = ref <Module>.??_7?$basic_ofstream@DU?$char_traits@D@std@@@std@@6B@;
				int num2 = *(*A_0 + 4);
				*(A_0 + num2 - 4) = num2 - 96;
				basic_filebuf<char,std::char_traits<char>\u0020>* ptr2 = ptr;
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{ctor}(ptr2);
				try
				{
					*ptr2 = ref <Module>.??_7?$basic_filebuf@DU?$char_traits@D@std@@@std@@6B@;
					<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Init(ptr2, null, (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)0);
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), ptr2);
					throw;
				}
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 8);
				throw;
			}
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}), A_0 + 96);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00007280 File Offset: 0x00006680
	internal unsafe static void {dtor}(basic_ofstream<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_ofstream<char,std::char_traits<char>\u0020>* ptr = A_0 - 96;
		*(A_0 + *(*ptr + 4) - 96) = ref <Module>.??_7?$basic_ofstream@DU?$char_traits@D@std@@@std@@6B@;
		int num = *(*ptr + 4);
		*(A_0 + num - 100) = num - 96;
		try
		{
			<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.{dtor}(A_0 - 92);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>.{dtor}), A_0 - 96 + 8);
			throw;
		}
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.{dtor}(A_0 - 88);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00004078 File Offset: 0x00003478
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool is_open(basic_ofstream<char,std::char_traits<char>\u0020>* A_0)
	{
		return (*(A_0 + 84) != 0) ? 1 : 0;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x000049AC File Offset: 0x00003DAC
	internal unsafe static void open(basic_ofstream<char,std::char_traits<char>\u0020>* A_0, sbyte* _Filename, int _Mode, int _Prot)
	{
		if (<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.open(A_0 + 4, _Filename, _Mode | 2, _Prot) == null)
		{
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*A_0 + 4) + A_0, 2, false);
		}
		else
		{
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.clear(*(*A_0 + 4) + A_0, 0, false);
		}
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000066AC File Offset: 0x00005AAC
	internal unsafe static void close(basic_ofstream<char,std::char_traits<char>\u0020>* A_0)
	{
		if (<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.close(A_0 + 4) == null)
		{
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*A_0 + 4) + A_0, 2, false);
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000066D4 File Offset: 0x00005AD4
	internal unsafe static void {dtor}(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		*A_0 = ref <Module>.??_7?$basic_filebuf@DU?$char_traits@D@std@@@std@@6B@;
		try
		{
			if (*(A_0 + 80) != 0)
			{
				<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Reset_back(A_0);
			}
			if (*(A_0 + 76) != 0)
			{
				<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.close(A_0);
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), A_0);
			throw;
		}
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}(A_0);
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000D938 File Offset: 0x0000CD38
	internal unsafe static void _Lock(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		uint num = (uint)(*(A_0 + 80));
		if (num != 0U)
		{
			<Module>._lock_file(num);
		}
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000D95C File Offset: 0x0000CD5C
	internal unsafe static void _Unlock(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		uint num = (uint)(*(A_0 + 80));
		if (num != 0U)
		{
			<Module>._unlock_file(num);
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000D980 File Offset: 0x0000CD80
	internal unsafe static int overflow(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, int _Meta)
	{
		if (((-1 == _Meta) ? 1 : 0) != 0)
		{
			return (_Meta != -1) ? _Meta : 0;
		}
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0))
		{
			*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Pninc(A_0) = (byte)_Meta;
			return _Meta;
		}
		if (*(A_0 + 80) == 0)
		{
			return -1;
		}
		<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Reset_back(A_0);
		if (*(A_0 + 64) == 0)
		{
			_iobuf* ptr = *(A_0 + 80);
			return (((<Module>.fputc((int)((sbyte)_Meta), ptr) != -1) ? 1 : 0) != 0) ? _Meta : -1;
		}
		sbyte b = (sbyte)_Meta;
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 8U, 0);
		int result;
		try
		{
			basic_filebuf<char,std::char_traits<char>\u0020>* ptr2 = A_0 + 72;
			int num2;
			for (;;)
			{
				sbyte* parg = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020> string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>;
				<Module>.std._String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>.{ctor}(ref string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>, (sbyte*)parg, (_Container_base0*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				sbyte* ptr3 = string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>;
				uint num = (uint)(*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16));
				sbyte* parg2 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020> string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2;
				<Module>.std._String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>.{ctor}(ref string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2, (sbyte*)parg2, (_Container_base0*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				sbyte* ptr4;
				sbyte* ptr5;
				num2 = <Module>.std.codecvt<char,char,int>.out(*(A_0 + 64), ptr2, &b, ref b + 1, ref ptr4, string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2, ptr3 + num, ref ptr5);
				if (num2 < 0)
				{
					goto IL_1B1;
				}
				if (num2 > 1)
				{
					break;
				}
				sbyte* ptr6 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				uint num3 = (uint)(ptr5 - ptr6);
				if (0U < num3)
				{
					sbyte* ptr7 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					if (num3 != <Module>.fwrite((void*)ptr7, 1U, num3, *(A_0 + 80)))
					{
						goto IL_19A;
					}
				}
				*(A_0 + 69) = 1;
				if (ptr4 != &b)
				{
					goto IL_19F;
				}
				if (0U >= num3)
				{
					if (*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) >= 32)
					{
						goto IL_1A7;
					}
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 8U, 0);
				}
			}
			int num4;
			if (num2 == 3)
			{
				_iobuf* ptr8 = *(A_0 + 80);
				num4 = ((((<Module>.fputc(b, ptr8) != -1) ? 1 : 0) != 0) ? _Meta : -1);
				goto IL_1AC;
			}
			goto IL_1B1;
			IL_19A:
			int num5 = -1;
			goto IL_1A2;
			IL_19F:
			num5 = _Meta;
			IL_1A2:
			int num6 = num5;
			goto IL_1A9;
			IL_1A7:
			num6 = -1;
			IL_1A9:
			num4 = num6;
			IL_1AC:
			result = num4;
			goto IL_1C3;
			IL_1B1:
			result = -1;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
		IL_1C3:
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		return result;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000DB9C File Offset: 0x0000CF9C
	internal unsafe static int pbackfail(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, int _Meta)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) && (((-1 == _Meta) ? 1 : 0) != 0 || (((int)((byte)(*(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) - 1))) == _Meta) ? 1 : 0) != 0))
		{
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Gndec(A_0);
			return (_Meta != -1) ? _Meta : 0;
		}
		uint num = (uint)(*(A_0 + 80));
		if (num == 0U || ((-1 == _Meta) ? 1 : 0) != 0)
		{
			return -1;
		}
		if (*(A_0 + 64) == 0)
		{
			_iobuf* ptr = num;
			if (((<Module>.ungetc((int)((byte)_Meta), ptr) != -1) ? 1 : 0) != 0)
			{
				return _Meta;
			}
		}
		basic_filebuf<char,std::char_traits<char>\u0020>* ptr2 = A_0 + 68;
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != ptr2)
		{
			*ptr2 = (byte)_Meta;
			<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Set_back(A_0);
			return _Meta;
		}
		return -1;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000DC94 File Offset: 0x0000D094
	internal unsafe static int underflow(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0))
		{
			return (byte)(*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0));
		}
		int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), A_0, *(*A_0 + 28));
		if (((-1 == num) ? 1 : 0) != 0)
		{
			return num;
		}
		object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Int32), A_0, num, *(*A_0 + 16));
		return num;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000DCEC File Offset: 0x0000D0EC
	internal unsafe static int uflow(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) < <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0))
		{
			return (byte)(*<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Gninc(A_0));
		}
		if (*(A_0 + 80) == 0)
		{
			return -1;
		}
		<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Reset_back(A_0);
		if (*(A_0 + 64) == 0)
		{
			int num = <Module>.fgetc(*(A_0 + 80));
			int result;
			if (num != -1)
			{
				sbyte b = (sbyte)num;
				result = b;
			}
			else
			{
				result = -1;
			}
			return result;
		}
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
		int result2;
		try
		{
			int num2 = <Module>.fgetc(*(A_0 + 80));
			int num5;
			int num7;
			if (num2 != -1)
			{
				basic_filebuf<char,std::char_traits<char>\u0020>* ptr = A_0 + 72;
				sbyte* ptr3;
				sbyte b2;
				for (;;)
				{
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 1U, (sbyte)num2);
					sbyte* parg = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020> string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>;
					<Module>.std._String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>.{ctor}(ref string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>, (sbyte*)parg, (_Container_base0*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
					sbyte* ptr2 = string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>;
					uint num3 = (uint)(*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16));
					sbyte* parg2 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020> string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2;
					<Module>.std._String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>.{ctor}(ref string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2, (sbyte*)parg2, (_Container_base0*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
					sbyte* ptr4;
					int num4 = <Module>.std.codecvt<char,char,int>.in(*(A_0 + 64), ptr, string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>2, ptr2 + num3, ref ptr3, &b2, ref b2 + 1, ref ptr4);
					if (num4 < 0)
					{
						goto IL_1CC;
					}
					if (num4 > 1)
					{
						if (num4 != 3)
						{
							goto IL_1CC;
						}
						if (*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) >= 1)
						{
							break;
						}
					}
					else
					{
						if (ptr4 != &b2)
						{
							goto IL_176;
						}
						sbyte* ptr5 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.erase(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 0U, (uint)(ptr3 - ptr5));
					}
					num2 = <Module>.fgetc(*(A_0 + 80));
					if (num2 == -1)
					{
						goto IL_14B;
					}
				}
				sbyte* ptr6 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				<Module>.memcpy_s((void*)(&b2), 1U, (void*)ptr6, 1U);
				num5 = (int)b2;
				goto IL_1C6;
				IL_176:
				sbyte* parg3 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020> string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>3;
				<Module>.std._String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>.{ctor}(ref string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>3, (sbyte*)parg3, (_Container_base0*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				int num6 = *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) - ptr3 + string_iterator<std::_String_val<std::_Simple_types<char>_u0020>_u0020>3 / sizeof(sbyte);
				if (0 < num6)
				{
					do
					{
						num6--;
						<Module>.ungetc((int)(*(sbyte*)(num6 / sizeof(sbyte) + ptr3)), *(A_0 + 80));
					}
					while (num6 > 0);
				}
				num7 = (int)b2;
				goto IL_1C2;
				IL_1CC:
				result2 = -1;
				goto IL_1DF;
			}
			IL_14B:
			num7 = -1;
			IL_1C2:
			num5 = num7;
			IL_1C6:
			result2 = num5;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
		IL_1DF:
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		return result2;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000DF24 File Offset: 0x0000D324
	internal unsafe static fpos<int>* seekoff(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, fpos<int>* A_1, long _Off, int _Way, int __unnamed002)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) == A_0 + 68 && _Way == 1 && *(A_0 + 64) == 0)
		{
			_Off += -1L;
		}
		long num;
		if (*(A_0 + 80) != 0 && <Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Endwrite(A_0) != null && ((_Off == 0L && _Way == 1) || <Module>._fseeki64(*(A_0 + 80), _Off, _Way) == null) && <Module>.fgetpos(*(A_0 + 80), &num) == null)
		{
			<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Reset_back(A_0);
			*(long*)A_1 = 0L;
			*(long*)(A_1 + 8 / sizeof(fpos<int>)) = num;
			*(int*)(A_1 + 16 / sizeof(fpos<int>)) = *(A_0 + 72);
			return A_1;
		}
		*(long*)A_1 = *(long*)<Module>.__imp_std._BADOFF;
		*(long*)(A_1 + 8 / sizeof(fpos<int>)) = 0L;
		*(int*)(A_1 + 16 / sizeof(fpos<int>)) = 0;
		return A_1;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000DFE0 File Offset: 0x0000D3E0
	internal unsafe static fpos<int>* seekpos(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, fpos<int>* A_1, fpos<int> _Pos, int __unnamed001)
	{
		long num = *(ref _Pos + 8);
		long num2 = _Pos;
		if (*(A_0 + 80) != 0 && <Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Endwrite(A_0) != null && <Module>.fsetpos(*(A_0 + 80), (long*)(&num)) == null && (num2 == 0L || <Module>._fseeki64(*(A_0 + 80), num2, 1) == null) && <Module>.fgetpos(*(A_0 + 80), &num) == null)
		{
			*(A_0 + 72) = *(ref _Pos + 16);
			<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Reset_back(A_0);
			*(long*)A_1 = 0L;
			*(long*)(A_1 + 8 / sizeof(fpos<int>)) = num;
			*(int*)(A_1 + 16 / sizeof(fpos<int>)) = *(A_0 + 72);
			return A_1;
		}
		*(long*)A_1 = *(long*)<Module>.__imp_std._BADOFF;
		*(long*)(A_1 + 8 / sizeof(fpos<int>)) = 0L;
		*(int*)(A_1 + 16 / sizeof(fpos<int>)) = 0;
		return A_1;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000E0A4 File Offset: 0x0000D4A4
	internal unsafe static basic_streambuf<char,std::char_traits<char>\u0020>* setbuf(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, sbyte* _Buffer, long _Count)
	{
		uint num = (uint)(*(A_0 + 80));
		if (num != 0U)
		{
			int num2;
			if (_Buffer == null && _Count == 0L)
			{
				num2 = 4;
			}
			else
			{
				num2 = 0;
			}
			if (<Module>.setvbuf(num, _Buffer, num2, (uint)((int)_Count)) == null)
			{
				<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Init(A_0, *(A_0 + 80), (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)1);
				return A_0;
			}
		}
		return 0;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000E0EC File Offset: 0x0000D4EC
	internal unsafe static int sync(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		int result;
		if (*(A_0 + 80) != 0 && ((-1 == calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Int32), A_0, -1, *(*A_0 + 12))) ? 1 : 0) == 0 && 0 > <Module>.fflush(*(A_0 + 80)))
		{
			result = -1;
		}
		else
		{
			result = 0;
		}
		return result;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000E130 File Offset: 0x0000D530
	internal unsafe static void imbue(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, locale* _Loc)
	{
		codecvt<char,char,int>* ptr = <Module>.std.use_facet<class\u0020std::codecvt<char,char,int>\u0020>(_Loc);
		if (<Module>.std.codecvt_base.always_noconv(ptr) != null)
		{
			*(A_0 + 64) = 0;
		}
		else
		{
			*(A_0 + 64) = ptr;
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Init(A_0);
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000E244 File Offset: 0x0000D644
	internal unsafe static void* __vecDelDtor(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint A_0)
	{
		basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr2;
		if ((A_0 & 2U) != 0U)
		{
			basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 - 108;
			ptr2 = A_0 - 104;
			<Module>.__ehvec_dtor(ptr2, 176U, *ptr, ldftn(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vbaseDtor));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		ptr2 = A_0 - 104;
		basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr3 = ptr2 + 104;
		<Module>.std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}(ptr3);
		<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ptr3);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(ptr2);
		}
		return ptr2;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000D234 File Offset: 0x0000C634
	internal unsafe static void* __vecDelDtor(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 68U, *ptr, ldftn(std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		*A_0 = ref <Module>.??_7?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
		try
		{
			<Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), A_0);
			throw;
		}
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0000E5AC File Offset: 0x0000D9AC
	internal unsafe static void* __vecDelDtor(basic_ofstream<char,std::char_traits<char>\u0020>* A_0, uint A_0)
	{
		basic_ofstream<char,std::char_traits<char>\u0020>* ptr2;
		if ((A_0 & 2U) != 0U)
		{
			basic_ofstream<char,std::char_traits<char>\u0020>* ptr = A_0 - 100;
			ptr2 = A_0 - 96;
			<Module>.__ehvec_dtor(ptr2, 168U, *ptr, ldftn(std.basic_ofstream<char,std::char_traits<char>\u0020>.__vbaseDtor));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		ptr2 = A_0 - 96;
		basic_ofstream<char,std::char_traits<char>\u0020>* ptr3 = ptr2 + 96;
		<Module>.std.basic_ofstream<char,std::char_traits<char>\u0020>.{dtor}(ptr3);
		<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.{dtor}(ptr3);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(ptr2);
		}
		return ptr2;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0000D8EC File Offset: 0x0000CCEC
	internal unsafe static void* __vecDelDtor(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, uint A_0)
	{
		if ((A_0 & 2U) != 0U)
		{
			basic_filebuf<char,std::char_traits<char>\u0020>* ptr = A_0 - 4;
			<Module>.__ehvec_dtor(A_0, 84U, *ptr, ldftn(std.basic_filebuf<char,std::char_traits<char>\u0020>.{dtor}));
			if ((A_0 & 1U) != 0U)
			{
				<Module>.delete[](ptr);
			}
			return ptr;
		}
		<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000057A4 File Offset: 0x00004BA4
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Count, sbyte _Ch)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Count, _Ch);
		return A_0;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00005AC4 File Offset: 0x00004EC4
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* +=(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Right, 0U, uint.MaxValue);
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000057D0 File Offset: 0x00004BD0
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* append(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr, uint _Count)
	{
		if (_Ptr != null)
		{
			uint num = (uint)(*(A_0 + 20));
			sbyte* ptr;
			if (16U <= num)
			{
				ptr = *A_0;
			}
			else
			{
				ptr = A_0;
			}
			if (_Ptr >= (sbyte*)ptr)
			{
				sbyte* ptr2;
				if (16U <= num)
				{
					ptr2 = *A_0;
				}
				else
				{
					ptr2 = A_0;
				}
				if (*(A_0 + 16) / sizeof(sbyte) + ptr2 != (sbyte*)_Ptr)
				{
					sbyte* ptr3;
					if (16U <= num)
					{
						ptr3 = *A_0;
					}
					else
					{
						ptr3 = A_0;
					}
					return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, A_0, (uint)(_Ptr - ptr3), _Count);
				}
			}
		}
		uint num2 = (uint)(*(A_0 + 16));
		if (4294967295U - num2 <= _Count)
		{
			<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
		}
		if (0U < _Count)
		{
			uint num3 = num2 + _Count;
			if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, num3, false) != null)
			{
				sbyte* ptr4;
				if (16 <= *(A_0 + 20))
				{
					ptr4 = *A_0;
				}
				else
				{
					ptr4 = A_0;
				}
				if (_Count != 0U)
				{
					cpblk(ptr4 + *(A_0 + 16) / sizeof(sbyte), _Ptr, _Count);
				}
				*(A_0 + 16) = (int)num3;
				*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + num3) = 0;
			}
		}
		return A_0;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000049E8 File Offset: 0x00003DE8
	internal unsafe static _String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* begin(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, _String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* A_1)
	{
		sbyte* ptr;
		if (16 <= *(A_0 + 20))
		{
			ptr = *A_0;
		}
		else
		{
			ptr = A_0;
		}
		*(int*)A_1 = ptr;
		return A_1;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00004A0C File Offset: 0x00003E0C
	internal unsafe static int compare(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Off, uint _N0, sbyte* _Ptr, uint _Count)
	{
		uint num = (uint)(*(A_0 + 16));
		if (num < _Off)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
		}
		uint num2 = num - _Off;
		_N0 = ((num2 < _N0) ? num2 : _N0);
		uint num3 = (_N0 < _Count) ? _N0 : _Count;
		sbyte* ptr;
		if (16 <= *(A_0 + 20))
		{
			ptr = *A_0;
		}
		else
		{
			ptr = A_0;
		}
		if (num3 != 0U)
		{
			uint num4 = num3;
			sbyte* ptr2 = _Ptr;
			int num5 = _Off / (uint)sizeof(sbyte) + ptr;
			ref byte ptr3 = num5;
			int num6 = 0;
			byte b = ptr3;
			byte b2 = *(byte*)_Ptr;
			if (b >= b2)
			{
				sbyte* ptr4 = num5 - _Ptr;
				while (b <= b2)
				{
					if (num4 == 1U)
					{
						goto IL_8F;
					}
					num4 -= 1U;
					ptr2 += 1 / sizeof(sbyte);
					b = *(byte*)(ptr4 + ptr2 / sizeof(sbyte));
					b2 = *(byte*)ptr2;
					if (b < b2)
					{
						goto IL_89;
					}
				}
				num6 = 1;
				goto IL_8F;
			}
			IL_89:
			num6 = -1;
			IL_8F:
			if (num6 != 0)
			{
				return num6;
			}
		}
		int result;
		if (_N0 < _Count)
		{
			result = -1;
		}
		else
		{
			result = ((_N0 == _Count) ? 0 : 1);
		}
		return result;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00004ACC File Offset: 0x00003ECC
	internal unsafe static allocator<char>* get_allocator(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, allocator<char>* A_1)
	{
		return A_1;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x000036B4 File Offset: 0x00002AB4
	internal unsafe static void _Change_alloc(_String_alloc<0,std::_String_base_types<char,std::allocator<char>\u0020>\u0020>* A_0, _Wrap_alloc<std::allocator<char>\u0020>* A_0)
	{
	}

	// Token: 0x060000FB RID: 251 RVA: 0x000036C4 File Offset: 0x00002AC4
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Inside(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr)
	{
		int num;
		if (_Ptr < *(A_0 + 4) && *A_0 <= _Ptr)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006738 File Offset: 0x00005B38
	internal unsafe static void _Reserve(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Count)
	{
		int num = *(A_0 + 8);
		int num2 = *(A_0 + 4);
		if ((num - num2) / 40 < (int)_Count)
		{
			int num3 = *A_0;
			int num4 = (num2 - num3) / 40;
			if (107374182 - num4 < (int)_Count)
			{
				<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
			}
			uint num5 = (uint)(num4 + (int)_Count);
			uint num6 = (uint)((num - num3) / 40);
			uint num7 = num6 >> 1;
			uint num8;
			if (107374182U - num7 < num6)
			{
				num8 = 0U;
			}
			else
			{
				num8 = num7 + num6;
			}
			uint num9 = num8;
			num9 = ((num8 < num5) ? num5 : num9);
			<Module>.std.vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>._Reallocate(A_0, num9);
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00005ADC File Offset: 0x00004EDC
	internal unsafe static void _Tidy(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		if (*A_0 != 0)
		{
			<Module>.std._Container_base0._Orphan_all(A_0);
			<Module>.delete(*A_0);
			*A_0 = 0;
			*(A_0 + 4) = 0;
			*(A_0 + 8) = 0;
		}
	}

	// Token: 0x060000FE RID: 254 RVA: 0x000036E8 File Offset: 0x00002AE8
	internal unsafe static void _Xran(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00003700 File Offset: 0x00002B00
	internal unsafe static void _Orphan_range(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* A_0, FfuReader.WriteRequest* A_1)
	{
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00003710 File Offset: 0x00002B10
	internal unsafe static allocator<FfuReader::WriteRequest>* {ctor}(allocator<FfuReader::WriteRequest>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00004094 File Offset: 0x00003494
	internal unsafe static _Vector_alloc<0,std::_Vec_base_types<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>\u0020>* {ctor}(_Vector_alloc<0,std::_Vec_base_types<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>\u0020>* A_0, allocator<FfuReader::WriteRequest>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x000040B0 File Offset: 0x000034B0
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* _Getal(_Vector_alloc<0,std::_Vec_base_types<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>\u0020>* A_0, _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_1)
	{
		return A_1;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00003720 File Offset: 0x00002B20
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Inside(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr)
	{
		int num;
		if (_Ptr < *(A_0 + 4) && *A_0 <= _Ptr)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000067B8 File Offset: 0x00005BB8
	internal unsafe static void _Reserve(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, uint _Count)
	{
		int num = *(A_0 + 8);
		int num2 = *(A_0 + 4);
		if ((num - num2) / 24 < (int)_Count)
		{
			int num3 = *A_0;
			int num4 = (num2 - num3) / 24;
			if (178956970 - num4 < (int)_Count)
			{
				<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
			}
			uint num5 = (uint)(num4 + (int)_Count);
			uint num6 = (uint)((num - num3) / 24);
			uint num7 = num6 >> 1;
			uint num8;
			if (178956970U - num7 < num6)
			{
				num8 = 0U;
			}
			else
			{
				num8 = num7 + num6;
			}
			uint num9 = num8;
			num9 = ((num8 < num5) ? num5 : num9);
			<Module>.std.vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>._Reallocate(A_0, num9);
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00005B08 File Offset: 0x00004F08
	internal unsafe static void _Tidy(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		if (*A_0 != 0)
		{
			<Module>.std._Container_base0._Orphan_all(A_0);
			<Module>.delete(*A_0);
			*A_0 = 0;
			*(A_0 + 4) = 0;
			*(A_0 + 8) = 0;
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00003744 File Offset: 0x00002B44
	internal unsafe static void _Orphan_range(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* A_0, FfuReader.BlockDataEntry* A_1)
	{
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00003754 File Offset: 0x00002B54
	internal unsafe static allocator<FfuReader::BlockDataEntry>* {ctor}(allocator<FfuReader::BlockDataEntry>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000040C0 File Offset: 0x000034C0
	internal unsafe static _Vector_alloc<0,std::_Vec_base_types<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>\u0020>* {ctor}(_Vector_alloc<0,std::_Vec_base_types<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>\u0020>* A_0, allocator<FfuReader::BlockDataEntry>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x000040DC File Offset: 0x000034DC
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* _Getal(_Vector_alloc<0,std::_Vec_base_types<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>\u0020>* A_0, _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_1)
	{
		return A_1;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00003764 File Offset: 0x00002B64
	internal unsafe static uint size(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return *(A_0 + 4) - *A_0 >> 6;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000377C File Offset: 0x00002B7C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Inside(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr)
	{
		int num;
		if (_Ptr < *(A_0 + 4) && *A_0 <= _Ptr)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x0000A17C File Offset: 0x0000957C
	internal unsafe static void _Reserve(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, uint _Count)
	{
		int num = *(A_0 + 8);
		int num2 = *(A_0 + 4);
		if (num - num2 >> 6 < (int)_Count)
		{
			int num3 = *A_0;
			int num4 = num2 - num3 >> 6;
			if (67108863 - num4 < (int)_Count)
			{
				<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
			}
			uint num5 = (uint)(num4 + (int)_Count);
			uint num6 = (uint)(num - num3 >> 6);
			uint num7 = num6 >> 1;
			uint num8;
			if (67108863U - num7 < num6)
			{
				num8 = 0U;
			}
			else
			{
				num8 = num7 + num6;
			}
			uint num9 = num8;
			num9 = ((num8 < num5) ? num5 : num9);
			<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Reallocate(A_0, num9);
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00009FF0 File Offset: 0x000093F0
	internal unsafe static void _Tidy(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		if (*A_0 != 0)
		{
			<Module>.std._Container_base0._Orphan_all(A_0);
			<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Destroy(A_0, *A_0, *(A_0 + 4));
			<Module>.delete(*A_0);
			*A_0 = 0;
			*(A_0 + 4) = 0;
			*(A_0 + 8) = 0;
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x000037A0 File Offset: 0x00002BA0
	internal unsafe static void _Xran(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@));
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000037B8 File Offset: 0x00002BB8
	internal unsafe static void _Orphan_range(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* A_0, FfuReader.partition* A_1)
	{
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000037C8 File Offset: 0x00002BC8
	internal unsafe static allocator<FfuReader::partition>* {ctor}(allocator<FfuReader::partition>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x000040EC File Offset: 0x000034EC
	internal unsafe static _Vector_alloc<0,std::_Vec_base_types<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>\u0020>* {ctor}(_Vector_alloc<0,std::_Vec_base_types<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>\u0020>* A_0, allocator<FfuReader::partition>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00004108 File Offset: 0x00003508
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* _Getal(_Vector_alloc<0,std::_Vec_base_types<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>\u0020>* A_0, _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_1)
	{
		return A_1;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00004ADC File Offset: 0x00003EDC
	internal unsafe static basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, int _Mode)
	{
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{ctor}(A_0);
		try
		{
			*A_0 = ref <Module>.??_7?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;
			int num = <Module>.std.basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>._Getstate(A_0, _Mode);
			*(A_0 + 56) = 0;
			*(A_0 + 60) = num;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00006838 File Offset: 0x00005C38
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* str(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_1)
	{
		uint num = 0U;
		if ((*(A_0 + 60) & 2) == 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
		{
			uint num2 = (uint)(*(A_0 + 56));
			int count = ((num2 >= <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0)) ? num2 : <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0)) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0);
			sbyte* ptr = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pbase(A_0);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, ptr, (uint)count);
			try
			{
				try
				{
					*(int*)(A_1 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
					*(int*)(A_1 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_1, false, 0U);
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_1, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
				return A_1;
			}
			catch
			{
				if ((num & 1U) != 0U)
				{
					num &= 4294967294U;
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
				}
				throw;
			}
		}
		if ((*(A_0 + 60) & 4) == 0 && <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) != null)
		{
			int count2 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0) - <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			sbyte* ptr2 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 16) = 0;
			*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2 + 20) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, false, 0U);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ptr2, (uint)count2);
			try
			{
				try
				{
					*(int*)(A_1 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
					*(int*)(A_1 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_1, false, 0U);
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_1, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2);
					num = 1U;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2));
					throw;
				}
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, true, 0U);
				return A_1;
			}
			catch
			{
				if ((num & 1U) != 0U)
				{
					num &= 4294967294U;
					<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
				}
				throw;
			}
		}
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 16) = 0;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, false, 0U);
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* result;
		try
		{
			try
			{
				*(int*)(A_1 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
				*(int*)(A_1 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_1, false, 0U);
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_1, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3);
				num = 1U;
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>3, true, 0U);
			result = A_1;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_1);
			}
			throw;
		}
		return result;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000037D8 File Offset: 0x00002BD8
	internal unsafe static void _Tidy(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0)
	{
		if ((*(A_0 + 60) & 1) != 0)
		{
			if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.pptr(A_0) != null)
			{
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.epptr(A_0);
			}
			else
			{
				<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0);
			}
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			<Module>.delete(<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0));
		}
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, null, null, null);
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, null, null);
		*(A_0 + 56) = 0;
		*(A_0 + 60) = (*(A_0 + 60) & -2);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00004118 File Offset: 0x00003518
	internal unsafe static basic_filebuf<char,std::char_traits<char>\u0020>* {ctor}(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, _iobuf* _File)
	{
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.{ctor}(A_0);
		try
		{
			*A_0 = ref <Module>.??_7?$basic_filebuf@DU?$char_traits@D@std@@@std@@6B@;
			<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Init(A_0, _File, (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)0);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_streambuf<char,std::char_traits<char>\u0020>.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x0000383C File Offset: 0x00002C3C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool is_open(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		return (*(A_0 + 80) != 0) ? 1 : 0;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00004168 File Offset: 0x00003568
	internal unsafe static basic_filebuf<char,std::char_traits<char>\u0020>* open(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, sbyte* _Filename, int _Mode, int _Prot)
	{
		if (*(A_0 + 80) == 0)
		{
			_iobuf* ptr = <Module>.std._Fiopen(_Filename, _Mode, _Prot);
			if (ptr != null)
			{
				<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Init(A_0, ptr, (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)1);
				locale locale;
				locale* loc = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.getloc(A_0, &locale);
				try
				{
					codecvt<char,char,int>* ptr2 = <Module>.std.use_facet<class\u0020std::codecvt<char,char,int>\u0020>(loc);
					if (<Module>.std.codecvt_base.always_noconv(ptr2) != null)
					{
						*(A_0 + 64) = 0;
					}
					else
					{
						*(A_0 + 64) = ptr2;
						<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Init(A_0);
					}
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(std.locale.{dtor}), (void*)(&locale));
					throw;
				}
				<Module>.std.locale.{dtor}(ref locale);
				return A_0;
			}
		}
		return 0;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00005B34 File Offset: 0x00004F34
	internal unsafe static basic_filebuf<char,std::char_traits<char>\u0020>* close(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_filebuf<char,std::char_traits<char>\u0020>* ptr;
		if (*(A_0 + 80) == 0)
		{
			ptr = null;
		}
		else
		{
			ptr = ((<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Endwrite(A_0) == 0) ? null : A_0);
			ptr = ((<Module>.fclose(*(A_0 + 80)) != 0) ? null : ptr);
		}
		<Module>.std.basic_filebuf<char,std::char_traits<char>\u0020>._Init(A_0, null, (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)2);
		return ptr;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00003858 File Offset: 0x00002C58
	internal unsafe static void _Init(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, _iobuf* _File, basic_filebuf<char,std::char_traits<char>\u0020>._Initfl _Which)
	{
		int num = (_Which == (basic_filebuf<char,std::char_traits<char>\u0020>._Initfl)1) ? 1 : 0;
		*(A_0 + 76) = (byte)num;
		*(A_0 + 69) = 0;
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Init(A_0);
		if (_File != null)
		{
			sbyte** ptr = (sbyte**)(_File + 8 / sizeof(_iobuf));
			_iobuf* ptr2 = _File + 4 / sizeof(_iobuf);
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Init(A_0, ptr, (sbyte**)_File, (int*)ptr2, ptr, (sbyte**)_File, (int*)ptr2);
		}
		*(A_0 + 80) = _File;
		*(A_0 + 72) = <Module>.?_Stinit@?1??_Init@?$basic_filebuf@DU?$char_traits@D@std@@@std@@IAEXPAU_iobuf@@W4_Initfl@23@@Z@4HA;
		*(A_0 + 64) = 0;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000588C File Offset: 0x00004C8C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool _Endwrite(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		if (*(A_0 + 64) == 0 || *(A_0 + 69) == 0)
		{
			return 1;
		}
		int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.Int32), A_0, -1, *(*A_0 + 12));
		if (((-1 == num) ? 1 : 0) != 0)
		{
			return 0;
		}
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) = 0;
		*(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 8U, 0);
		bool result;
		try
		{
			basic_filebuf<char,std::char_traits<char>\u0020>* ptr = A_0 + 72;
			int num2;
			for (;;)
			{
				sbyte* ptr2 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				sbyte* ptr3 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				sbyte* ptr4;
				num2 = <Module>.std.codecvt<char,char,int>.unshift(*(A_0 + 64), ptr, ptr3, *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 16) / sizeof(sbyte) + ptr2, ref ptr4);
				if (num2 != 0)
				{
					if (num2 != 1)
					{
						break;
					}
				}
				else
				{
					*(A_0 + 69) = 0;
				}
				sbyte* ptr5 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
				uint num3 = (uint)(ptr4 - ptr5);
				if (0U < num3)
				{
					sbyte* ptr6 = (16 <= *(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> + 20)) ? basic_string<char,std::char_traits<char>,std::allocator<char>_u0020> : ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
					if (num3 != <Module>.fwrite((void*)ptr6, 1U, num3, *(A_0 + 80)))
					{
						goto IL_10D;
					}
				}
				if (*(A_0 + 69) == 0)
				{
					goto IL_111;
				}
				if (num3 == 0U)
				{
					<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, 8U, 0);
				}
			}
			if (num2 != 3)
			{
				result = false;
				goto IL_128;
			}
			bool flag = true;
			goto IL_115;
			IL_10D:
			bool flag2 = false;
			goto IL_113;
			IL_111:
			flag2 = true;
			IL_113:
			flag = flag2;
			IL_115:
			result = flag;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
			throw;
		}
		IL_128:
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
		return result;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000038AC File Offset: 0x00002CAC
	internal unsafe static void _Initcvt(basic_filebuf<char,std::char_traits<char>\u0020>* A_0, codecvt<char,char,int>* _Newpcvt)
	{
		if (<Module>.std.codecvt_base.always_noconv(_Newpcvt) != null)
		{
			*(A_0 + 64) = 0;
		}
		else
		{
			*(A_0 + 64) = _Newpcvt;
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>._Init(A_0);
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000038D8 File Offset: 0x00002CD8
	internal unsafe static void _Reset_back(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) == A_0 + 68)
		{
			int num = *(A_0 + 60);
			sbyte* ptr = *(A_0 + 56);
			int num2 = num;
			<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, ptr, num2, num2);
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000DC50 File Offset: 0x0000D050
	internal unsafe static void _Set_back(basic_filebuf<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_filebuf<char,std::char_traits<char>\u0020>* ptr = A_0 + 68;
		if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0) != ptr)
		{
			*(A_0 + 56) = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.eback(A_0);
			*(A_0 + 60) = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.egptr(A_0);
		}
		basic_filebuf<char,std::char_traits<char>\u0020>* ptr2 = ptr;
		<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, ptr2, ptr2, A_0 + 69);
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000D768 File Offset: 0x0000CB68
	internal unsafe static fpos<int>* {ctor}(fpos<int>* A_0, long _Off)
	{
		*A_0 = _Off;
		*(A_0 + 8) = 0L;
		*(A_0 + 16) = 0;
		return A_0;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000DFBC File Offset: 0x0000D3BC
	internal unsafe static fpos<int>* {ctor}(fpos<int>* A_0, int _State, long _Fileposition)
	{
		*A_0 = 0L;
		*(A_0 + 8) = _Fileposition;
		*(A_0 + 16) = _State;
		return A_0;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000E078 File Offset: 0x0000D478
	internal unsafe static int state(fpos<int>* A_0)
	{
		return *(A_0 + 16);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000E08C File Offset: 0x0000D48C
	internal unsafe static long seekpos(fpos<int>* A_0)
	{
		return *(A_0 + 8);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000D8A0 File Offset: 0x0000CCA0
	internal unsafe static long _J(fpos<int>* A_0)
	{
		return *(A_0 + 8) + *A_0;
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000041F4 File Offset: 0x000035F4
	internal unsafe static basic_ostream<char,std::char_traits<char>\u0020>.sentry* {ctor}(basic_ostream<char,std::char_traits<char>\u0020>.sentry* A_0, basic_ostream<char,std::char_traits<char>\u0020>* _Ostr)
	{
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{ctor}(A_0, _Ostr);
		try
		{
			if (<Module>.std.ios_base.good(*(*_Ostr + 4) + _Ostr) != null && <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.tie(*(*_Ostr + 4) + _Ostr) != null)
			{
				<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.flush(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.tie(*(*_Ostr + 4) + _Ostr));
			}
			*(A_0 + 4) = <Module>.std.ios_base.good(*(*_Ostr + 4) + _Ostr);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00004274 File Offset: 0x00003674
	internal unsafe static void {dtor}(basic_ostream<char,std::char_traits<char>\u0020>.sentry* A_0)
	{
		try
		{
			if (<Module>.std.uncaught_exception() == null)
			{
				<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Osfx(*A_0);
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}), A_0);
			throw;
		}
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}(A_0);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x000042C4 File Offset: 0x000036C4
	internal unsafe static method P6AXAAU?$_Bool_struct@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@std@@@Z(basic_ostream<char,std::char_traits<char>\u0020>.sentry* A_0)
	{
		return (*(A_0 + 4) == 0) ? null : <Module>.__unep@??$_Bool_function@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@std@@$$FYAXAAU?$_Bool_struct@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@0@@Z;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x000042E0 File Offset: 0x000036E0
	internal unsafe static sbyte* *(_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* A_0)
	{
		return *A_0;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00005B78 File Offset: 0x00004F78
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* {ctor}(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr, uint _Count)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, _Ptr, _Count);
		return A_0;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000059EC File Offset: 0x00004DEC
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* append(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Right, 0U, uint.MaxValue);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x000054FC File Offset: 0x000048FC
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* append(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right, uint _Roff, uint _Count)
	{
		uint num = (uint)(*(_Right + 16));
		if (num < _Roff)
		{
			<Module>.std._Xout_of_range((sbyte*)(&<Module>.??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@));
		}
		uint num2 = num - _Roff;
		_Count = ((num2 < _Count) ? num2 : _Count);
		uint num3 = (uint)(*(A_0 + 16));
		if (4294967295U - num3 <= _Count)
		{
			<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
		}
		if (0U < _Count)
		{
			num2 = num3 + _Count;
			if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, num2, false) != null)
			{
				sbyte* ptr;
				if (16 <= *(_Right + 20))
				{
					ptr = *_Right;
				}
				else
				{
					ptr = _Right;
				}
				sbyte* ptr2;
				if (16 <= *(A_0 + 20))
				{
					ptr2 = *A_0;
				}
				else
				{
					ptr2 = A_0;
				}
				if (_Count != 0U)
				{
					cpblk(*(A_0 + 16) / sizeof(sbyte) + ptr2, ptr + _Roff / (uint)sizeof(sbyte), _Count);
				}
				*(A_0 + 16) = (int)num2;
				*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + num2) = 0;
			}
		}
		return A_0;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000055A0 File Offset: 0x000049A0
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* assign(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Count, sbyte _Ch)
	{
		if (_Count == 4294967295U)
		{
			<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@));
		}
		if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, _Count, false) != null)
		{
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr;
			if (_Count == 1U)
			{
				ptr = A_0 + 20;
				*((16 > *ptr) ? A_0 : (*A_0)) = _Ch;
			}
			else
			{
				ptr = A_0 + 20;
				initblk((16 > *ptr) ? A_0 : (*A_0), _Ch, _Count);
			}
			*(A_0 + 16) = (int)_Count;
			*(((16 > *ptr) ? A_0 : (*A_0)) + _Count) = 0;
		}
		return A_0;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00003904 File Offset: 0x00002D04
	internal unsafe static uint _Unused_capacity(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return (*(A_0 + 8) - *(A_0 + 4)) / 40;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00005164 File Offset: 0x00004564
	internal unsafe static uint max_size(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return 107374182;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00005A04 File Offset: 0x00004E04
	internal unsafe static void _Destroy(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last)
	{
	}

	// Token: 0x0600012F RID: 303 RVA: 0x00005178 File Offset: 0x00004578
	internal unsafe static uint _Grow_to(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Count)
	{
		uint num = (uint)((*(A_0 + 8) - *A_0) / 40);
		uint num2 = num >> 1;
		uint num3;
		if (107374182U - num2 < num)
		{
			num3 = 0U;
		}
		else
		{
			num3 = num2 + num;
		}
		uint num4 = num3;
		return (num3 < _Count) ? _Count : num4;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00005BA4 File Offset: 0x00004FA4
	internal unsafe static void _Reallocate(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Count)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		void* ptr = null;
		if (_Count != 0U)
		{
			if (107374182U >= _Count)
			{
				ptr = <Module>.@new(_Count * 40U);
				if (ptr != null)
				{
					goto IL_28;
				}
			}
			<Module>.std._Xbad_alloc();
			return;
		}
		IL_28:
		uint exceptionCode;
		try
		{
			FfuReader.WriteRequest* last = *(A_0 + 4);
			FfuReader.WriteRequest* first = *A_0;
			_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
			_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
			_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020> wrap_alloc<std::allocator<FfuReader::WriteRequest>_u0020>;
			<Module>.std._Uninit_move<struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020FfuReader::WriteRequest\u0020*,class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>,struct\u0020FfuReader::WriteRequest>(first, last, (FfuReader.WriteRequest*)ptr, ref wrap_alloc<std::allocator<FfuReader::WriteRequest>_u0020>, null, _unnamed);
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					<Module>.delete(ptr);
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		int num3 = *A_0;
		uint num4 = (uint)((*(A_0 + 4) - num3) / 40);
		if (num3 != 0)
		{
			<Module>.delete(num3);
		}
		<Module>.std._Container_base0._Orphan_all(A_0);
		*(A_0 + 8) = _Count * 40U + (byte*)ptr;
		*(A_0 + 4) = num4 * 40U + (byte*)ptr;
		*A_0 = ptr;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x00003920 File Offset: 0x00002D20
	internal unsafe static void _Xlen(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
	}

	// Token: 0x06000132 RID: 306 RVA: 0x00003938 File Offset: 0x00002D38
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* {ctor}(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x000042F0 File Offset: 0x000036F0
	internal unsafe static void deallocate(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr, uint _Count)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00003948 File Offset: 0x00002D48
	internal unsafe static _Vector_val<std::_Simple_types<FfuReader::WriteRequest>\u0020>* {ctor}(_Vector_val<std::_Simple_types<FfuReader::WriteRequest>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x00003964 File Offset: 0x00002D64
	internal unsafe static uint _Unused_capacity(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return (*(A_0 + 8) - *(A_0 + 4)) / 24;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00003980 File Offset: 0x00002D80
	internal unsafe static uint size(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return (*(A_0 + 4) - *A_0) / 24;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x000051B4 File Offset: 0x000045B4
	internal unsafe static uint max_size(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return 178956970;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00005A14 File Offset: 0x00004E14
	internal unsafe static void _Destroy(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last)
	{
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000051C8 File Offset: 0x000045C8
	internal unsafe static uint _Grow_to(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, uint _Count)
	{
		uint num = (uint)((*(A_0 + 8) - *A_0) / 24);
		uint num2 = num >> 1;
		uint num3;
		if (178956970U - num2 < num)
		{
			num3 = 0U;
		}
		else
		{
			num3 = num2 + num;
		}
		uint num4 = num3;
		return (num3 < _Count) ? _Count : num4;
	}

	// Token: 0x0600013A RID: 314 RVA: 0x00005CD8 File Offset: 0x000050D8
	internal unsafe static void _Reallocate(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, uint _Count)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		void* ptr = null;
		if (_Count != 0U)
		{
			if (178956970U >= _Count)
			{
				ptr = <Module>.@new(_Count * 24U);
				if (ptr != null)
				{
					goto IL_28;
				}
			}
			<Module>.std._Xbad_alloc();
			return;
		}
		IL_28:
		uint exceptionCode;
		try
		{
			FfuReader.BlockDataEntry* last = *(A_0 + 4);
			FfuReader.BlockDataEntry* first = *A_0;
			_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
			_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
			_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020> wrap_alloc<std::allocator<FfuReader::BlockDataEntry>_u0020>;
			<Module>.std._Uninit_move<struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020FfuReader::BlockDataEntry\u0020*,class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>,struct\u0020FfuReader::BlockDataEntry>(first, last, (FfuReader.BlockDataEntry*)ptr, ref wrap_alloc<std::allocator<FfuReader::BlockDataEntry>_u0020>, null, _unnamed);
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					<Module>.delete(ptr);
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		int num3 = *A_0;
		uint num4 = (uint)((*(A_0 + 4) - num3) / 24);
		if (num3 != 0)
		{
			<Module>.delete(num3);
		}
		<Module>.std._Container_base0._Orphan_all(A_0);
		*(A_0 + 8) = _Count * 24U + (byte*)ptr;
		*(A_0 + 4) = num4 * 24U + (byte*)ptr;
		*A_0 = ptr;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x00003998 File Offset: 0x00002D98
	internal unsafe static void _Xlen(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
	}

	// Token: 0x0600013C RID: 316 RVA: 0x000039B0 File Offset: 0x00002DB0
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* {ctor}(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return A_0;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00004304 File Offset: 0x00003704
	internal unsafe static void deallocate(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr, uint _Count)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000039C0 File Offset: 0x00002DC0
	internal unsafe static _Vector_val<std::_Simple_types<FfuReader::BlockDataEntry>\u0020>* {ctor}(_Vector_val<std::_Simple_types<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000039DC File Offset: 0x00002DDC
	internal unsafe static uint _Unused_capacity(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return *(A_0 + 8) - *(A_0 + 4) >> 6;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00005204 File Offset: 0x00004604
	internal unsafe static uint max_size(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return 67108863;
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009FB0 File Offset: 0x000093B0
	internal unsafe static void _Destroy(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _First, FfuReader.partition* _Last)
	{
		FfuReader.partition* ptr = _First;
		if (_First != _Last)
		{
			do
			{
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, true, 0U);
				ptr += 64 / sizeof(FfuReader.partition);
			}
			while (ptr != _Last);
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00005218 File Offset: 0x00004618
	internal unsafe static uint _Grow_to(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, uint _Count)
	{
		uint num = (uint)(*(A_0 + 8) - *A_0 >> 6);
		uint num2 = num >> 1;
		uint num3;
		if (67108863U - num2 < num)
		{
			num3 = 0U;
		}
		else
		{
			num3 = num2 + num;
		}
		uint num4 = num3;
		return (num3 < _Count) ? _Count : num4;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000A028 File Offset: 0x00009428
	internal unsafe static void _Reallocate(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, uint _Count)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		void* ptr = null;
		if (_Count != 0U)
		{
			if (67108863U >= _Count)
			{
				ptr = <Module>.@new(_Count * 64U);
				if (ptr != null)
				{
					goto IL_29;
				}
			}
			<Module>.std._Xbad_alloc();
			return;
		}
		IL_29:
		uint exceptionCode;
		try
		{
			FfuReader.partition* last = *(A_0 + 4);
			FfuReader.partition* first = *A_0;
			_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
			_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
			_Wrap_alloc<std::allocator<FfuReader::partition>\u0020> wrap_alloc<std::allocator<FfuReader::partition>_u0020>;
			<Module>.std._Uninit_move<struct\u0020FfuReader::partition\u0020*,struct\u0020FfuReader::partition\u0020*,class\u0020std::allocator<struct\u0020FfuReader::partition>,struct\u0020FfuReader::partition>(first, last, (FfuReader.partition*)ptr, ref wrap_alloc<std::allocator<FfuReader::partition>_u0020>, null, _unnamed);
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					<Module>.delete(ptr);
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		int num3 = *(A_0 + 4);
		int num4 = *A_0;
		uint num5 = (uint)(num3 - num4 >> 6);
		if (num4 != 0)
		{
			<Module>.std.vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>._Destroy(A_0, num4, num3);
			<Module>.delete(*A_0);
		}
		<Module>.std._Container_base0._Orphan_all(A_0);
		*(A_0 + 8) = _Count * 64U + (byte*)ptr;
		*(A_0 + 4) = num5 * 64U + (byte*)ptr;
		*A_0 = ptr;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x000039F4 File Offset: 0x00002DF4
	internal unsafe static void _Xlen(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		<Module>.std._Xlength_error((sbyte*)(&<Module>.??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@));
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00003A0C File Offset: 0x00002E0C
	internal unsafe static _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* {ctor}(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return A_0;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00004318 File Offset: 0x00003718
	internal unsafe static void deallocate(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr, uint _Count)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00003A1C File Offset: 0x00002E1C
	internal unsafe static _Vector_val<std::_Simple_types<FfuReader::partition>\u0020>* {ctor}(_Vector_val<std::_Simple_types<FfuReader::partition>\u0020>* A_0)
	{
		*A_0 = 0;
		*(A_0 + 4) = 0;
		*(A_0 + 8) = 0;
		return A_0;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000432C File Offset: 0x0000372C
	internal unsafe static void _Init(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr, uint _Count, int _State)
	{
		*(A_0 + 56) = 0;
		*(A_0 + 60) = _State;
		if (_Count != 0U && (_State & 6) != 6)
		{
			if (4294967295U >= _Count)
			{
				void* ptr = <Module>.@new(_Count);
				if (ptr != null)
				{
					cpblk(ptr, _Ptr, _Count);
					int num = _Count + (byte*)ptr;
					*(A_0 + 56) = num;
					if ((*(A_0 + 60) & 4) == 0)
					{
						void* ptr2 = ptr;
						<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr2, (sbyte*)ptr2, num);
					}
					int num2 = *(A_0 + 60);
					if ((num2 & 2) == 0)
					{
						sbyte* ptr3 = (sbyte*)(((num2 & 16) != 0) ? num : ptr);
						<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setp(A_0, (sbyte*)ptr, ptr3, num);
						if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.gptr(A_0) == null)
						{
							<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.setg(A_0, (sbyte*)ptr, null, (sbyte*)ptr);
						}
					}
					*(A_0 + 60) = (*(A_0 + 60) | 1);
					return;
				}
			}
			<Module>.std._Xbad_alloc();
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00003A38 File Offset: 0x00002E38
	internal unsafe static int _Getstate(basic_stringbuf<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, int _Mode)
	{
		int num = 0;
		num = (((_Mode & 1) == 0) ? 4 : num);
		if ((_Mode & 2) == 0)
		{
			num |= 2;
		}
		if ((_Mode & 8) != 0)
		{
			num |= 8;
		}
		if ((_Mode & 4) != 0)
		{
			num |= 16;
		}
		return num;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00003A70 File Offset: 0x00002E70
	internal unsafe static sbyte* *(_String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* A_0)
	{
		return *A_0;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00003A80 File Offset: 0x00002E80
	internal unsafe static basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base* {ctor}(basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base* A_0, basic_ostream<char,std::char_traits<char>\u0020>* _Ostr)
	{
		*A_0 = _Ostr;
		if (<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr) != null)
		{
			int num = *A_0;
			basic_streambuf<char,std::char_traits<char>\u0020>* ptr = <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*num + 4) + num);
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*ptr + 4));
		}
		return A_0;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00003AB8 File Offset: 0x00002EB8
	internal unsafe static void {dtor}(basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base* A_0)
	{
		int num = *A_0;
		if (<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*num + 4) + num) != null)
		{
			num = *A_0;
			basic_streambuf<char,std::char_traits<char>\u0020>* ptr = <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*num + 4) + num);
			calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*ptr + 8));
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000043C4 File Offset: 0x000037C4
	internal unsafe static _String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* {ctor}(_String_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* A_0, sbyte* _Parg, _Container_base0* _Pstring)
	{
		*A_0 = _Parg;
		return A_0;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00003AF0 File Offset: 0x00002EF0
	internal unsafe static uint capacity(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return (*(A_0 + 8) - *A_0) / 40;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00003B08 File Offset: 0x00002F08
	internal unsafe static void deallocate(allocator<FfuReader::WriteRequest>* A_0, FfuReader.WriteRequest* _Ptr, uint __unnamed001)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00004B38 File Offset: 0x00003F38
	internal unsafe static FfuReader.WriteRequest* allocate(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (107374182U >= _Count)
			{
				ptr = <Module>.@new(_Count * 40U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00004B68 File Offset: 0x00003F68
	internal unsafe static uint max_size(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0)
	{
		return 107374182;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00003B1C File Offset: 0x00002F1C
	internal unsafe static uint capacity(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return (*(A_0 + 8) - *A_0) / 24;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00003B34 File Offset: 0x00002F34
	internal unsafe static void deallocate(allocator<FfuReader::BlockDataEntry>* A_0, FfuReader.BlockDataEntry* _Ptr, uint __unnamed001)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00004B7C File Offset: 0x00003F7C
	internal unsafe static FfuReader.BlockDataEntry* allocate(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (178956970U >= _Count)
			{
				ptr = <Module>.@new(_Count * 24U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00004BAC File Offset: 0x00003FAC
	internal unsafe static uint max_size(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0)
	{
		return 178956970;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00003B48 File Offset: 0x00002F48
	internal unsafe static uint capacity(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return *(A_0 + 8) - *A_0 >> 6;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00003B60 File Offset: 0x00002F60
	internal unsafe static void deallocate(allocator<FfuReader::partition>* A_0, FfuReader.partition* _Ptr, uint __unnamed001)
	{
		<Module>.delete((void*)_Ptr);
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00004BC0 File Offset: 0x00003FC0
	internal unsafe static FfuReader.partition* allocate(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (67108863U >= _Count)
			{
				ptr = <Module>.@new(_Count * 64U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00004BF0 File Offset: 0x00003FF0
	internal unsafe static uint max_size(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0)
	{
		return 67108863;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00003B74 File Offset: 0x00002F74
	internal unsafe static _String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* {ctor}(_String_const_iterator<std::_String_val<std::_Simple_types<char>\u0020>\u0020>* A_0, sbyte* _Parg, _Container_base0* _Pstring)
	{
		*A_0 = _Parg;
		return A_0;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000043D8 File Offset: 0x000037D8
	internal unsafe static FfuReader.WriteRequest* allocate(allocator<FfuReader::WriteRequest>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (107374182U >= _Count)
			{
				ptr = <Module>.@new(_Count * 40U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00004408 File Offset: 0x00003808
	internal unsafe static uint max_size(allocator<FfuReader::WriteRequest>* _Al)
	{
		return 107374182;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000441C File Offset: 0x0000381C
	internal unsafe static FfuReader.BlockDataEntry* allocate(allocator<FfuReader::BlockDataEntry>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (178956970U >= _Count)
			{
				ptr = <Module>.@new(_Count * 24U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000444C File Offset: 0x0000384C
	internal unsafe static uint max_size(allocator<FfuReader::BlockDataEntry>* _Al)
	{
		return 178956970;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00004460 File Offset: 0x00003860
	internal unsafe static FfuReader.partition* allocate(allocator<FfuReader::partition>* A_0, uint _Count)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (67108863U >= _Count)
			{
				ptr = <Module>.@new(_Count * 64U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00004490 File Offset: 0x00003890
	internal unsafe static uint max_size(allocator<FfuReader::partition>* _Al)
	{
		return 67108863;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00003B88 File Offset: 0x00002F88
	internal unsafe static uint max_size(allocator<FfuReader::WriteRequest>* A_0)
	{
		return 107374182;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00003B9C File Offset: 0x00002F9C
	internal unsafe static uint max_size(allocator<FfuReader::BlockDataEntry>* A_0)
	{
		return 178956970;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00003BB0 File Offset: 0x00002FB0
	internal unsafe static uint max_size(allocator<FfuReader::partition>* A_0)
	{
		return 67108863;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00011CA0 File Offset: 0x000110A0
	internal unsafe static void ??__E?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0xd5e524cb.??__F?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00011CF4 File Offset: 0x000110F4
	internal unsafe static void ??__E?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A));
			throw;
		}
		try
		{
			<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7_Iostream_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), (void*)(&<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0xd5e524cb.??__F?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00011D80 File Offset: 0x00011180
	internal unsafe static void ??__E?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ()
	{
		try
		{
			<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7_Generic_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.error_category.{dtor}), (void*)(&<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A));
			throw;
		}
		try
		{
			<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7_System_error_category@std@@6B@;
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std._Generic_error_category.{dtor}), (void*)(&<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A));
			throw;
		}
		<Module>._atexit_m(ldftn(?A0xd5e524cb.??__F?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ));
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00006AA4 File Offset: 0x00005EA4
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Left, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		try
		{
			uint num = 0U;
			*(int*)(A_0 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			*(int*)(A_0 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
			num = 1U;
			uint num2;
			if (*(sbyte*)_Left == 0)
			{
				num2 = 0U;
			}
			else
			{
				sbyte* ptr = _Left;
				do
				{
					ptr += 1 / sizeof(sbyte);
				}
				while (*(sbyte*)ptr != 0);
				num2 = (uint)(ptr - _Left);
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.reserve(A_0, (uint)(*(_Right + 16) + (int)num2));
			uint count;
			if (*(sbyte*)_Left == 0)
			{
				count = 0U;
			}
			else
			{
				sbyte* ptr2 = _Left;
				do
				{
					ptr2 += 1 / sizeof(sbyte);
				}
				while (*(sbyte*)ptr2 != 0);
				count = (uint)(ptr2 - _Left);
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Left, count);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Right, 0U, uint.MaxValue);
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_0);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00006B54 File Offset: 0x00005F54
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, sbyte* _Right)
	{
		try
		{
			uint num = 0U;
			*(int*)(A_0 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			*(int*)(A_0 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
			num = 1U;
			uint num2 = (uint)(*(_Left + 16));
			uint num3;
			if (*(sbyte*)_Right == 0)
			{
				num3 = 0U;
			}
			else
			{
				sbyte* ptr = _Right;
				do
				{
					ptr += 1 / sizeof(sbyte);
				}
				while (*(sbyte*)ptr != 0);
				num3 = (uint)(ptr - _Right);
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.reserve(A_0, num3 + num2);
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Left, 0U, uint.MaxValue);
			uint count;
			if (*(sbyte*)_Right == 0)
			{
				count = 0U;
			}
			else
			{
				sbyte* ptr2 = _Right;
				do
				{
					ptr2 += 1 / sizeof(sbyte);
				}
				while (*(sbyte*)ptr2 != 0);
				count = (uint)(ptr2 - _Right);
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Right, count);
		}
		catch
		{
			uint num;
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_0);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00005E0C File Offset: 0x0000520C
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* operator+<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, sbyte* _Right)
	{
		uint num = 0U;
		uint count;
		if (*(sbyte*)_Right == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Right;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Right);
		}
		basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* right = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(_Left, _Right, count);
		*(int*)(A_0 + 16 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
		*(int*)(A_0 + 20 / sizeof(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>)) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Assign_rv(A_0, right);
		try
		{
			num = 1U;
		}
		catch
		{
			if ((num & 1U) != 0U)
			{
				num &= 4294967294U;
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)A_0);
			}
			throw;
		}
		return A_0;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x000044A4 File Offset: 0x000038A4
	internal unsafe static basic_ostream<char,std::char_traits<char>\u0020>* operator<<<struct\u0020std::char_traits<char>\u0020>(basic_ostream<char,std::char_traits<char>\u0020>* _Ostr, sbyte* _Val)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		int num2 = 0;
		uint num3;
		if (*(sbyte*)_Val == 0)
		{
			num3 = 0U;
		}
		else
		{
			sbyte* ptr = _Val;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			num3 = (uint)(ptr - _Val);
		}
		long num4 = (long)((ulong)num3);
		long num5;
		if (<Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) > 0L && <Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) > num4)
		{
			num5 = <Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) - num4;
		}
		else
		{
			num5 = 0L;
		}
		long num6 = num5;
		basic_ostream<char,std::char_traits<char>\u0020>.sentry sentry;
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.sentry.{ctor}(ref sentry, _Ostr);
		try
		{
			if (*(ref sentry + 4) != 0 && <Module>.__unep@??$_Bool_function@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@std@@$$FYAXAAU?$_Bool_struct@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@0@@Z != null)
			{
				uint exceptionCode;
				try
				{
					if ((<Module>.std.ios_base.flags(*(*_Ostr + 4) + _Ostr) & 448) != 64)
					{
						while (0L < num6)
						{
							int num7 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputc(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.fill(*(*_Ostr + 4) + _Ostr));
							if (((-1 == num7) ? 1 : 0) != 0)
							{
								num2 |= 4;
								break;
							}
							num6 += -1L;
						}
						if (num2 != 0)
						{
							goto IL_13E;
						}
					}
					if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputn(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), _Val, num4) != num4)
					{
						num2 = 4;
					}
					else
					{
						while (0L < num6)
						{
							int num8 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputc(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.fill(*(*_Ostr + 4) + _Ostr));
							if (((-1 == num8) ? 1 : 0) != 0)
							{
								num2 |= 4;
								break;
							}
							num6 += -1L;
						}
					}
					IL_13E:
					<Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr, 0L);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					exceptionCode = (uint)Marshal.GetExceptionCode();
					endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
				})
				{
					uint num9 = 0U;
					<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
					try
					{
						try
						{
							<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*_Ostr + 4) + _Ostr, 4, true);
							goto IL_1AA;
						}
						catch when (delegate
						{
							// Failed to create a 'catch-when' expression
							num9 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
							endfilter(num9 != 0U);
						})
						{
						}
						if (num9 != 0U)
						{
							throw;
						}
					}
					finally
					{
						<Module>.__CxxUnregisterExceptionObject(num, (int)num9);
					}
				}
			}
			else
			{
				num2 = 4;
			}
			IL_1AA:
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*_Ostr + 4) + _Ostr, num2, false);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>.sentry.{dtor}), (void*)(&sentry));
			throw;
		}
		try
		{
			if (<Module>.std.uncaught_exception() == null)
			{
				<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Osfx(sentry);
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}), (void*)(&sentry));
			throw;
		}
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}(ref sentry);
		return _Ostr;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x00004C04 File Offset: 0x00004004
	internal unsafe static basic_ostream<char,std::char_traits<char>\u0020>* operator<<<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_ostream<char,std::char_traits<char>\u0020>* _Ostr, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Str)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		int num2 = 0;
		uint num3 = (uint)(*(_Str + 16));
		uint num4;
		if (<Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) > 0L && <Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) > (int)num3)
		{
			num4 = (uint)(<Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr) - (int)num3);
		}
		else
		{
			num4 = 0U;
		}
		uint num5 = num4;
		basic_ostream<char,std::char_traits<char>\u0020>.sentry sentry;
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>.sentry.{ctor}(ref sentry, _Ostr);
		try
		{
			if (*(ref sentry + 4) != 0 && <Module>.__unep@??$_Bool_function@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@std@@$$FYAXAAU?$_Bool_struct@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@0@@Z != null)
			{
				uint exceptionCode;
				try
				{
					if ((<Module>.std.ios_base.flags(*(*_Ostr + 4) + _Ostr) & 448) == 64)
					{
						goto IL_BE;
					}
					while (0U < num5)
					{
						int num6 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputc(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.fill(*(*_Ostr + 4) + _Ostr));
						if (((-1 == num6) ? 1 : 0) != 0)
						{
							num2 |= 4;
							break;
						}
						num5 -= 1U;
					}
					if (num2 == 0)
					{
						goto IL_BE;
					}
					IL_F1:
					while (0U < num5)
					{
						int num7 = <Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputc(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), <Module>.std.basic_ios<char,std::char_traits<char>\u0020>.fill(*(*_Ostr + 4) + _Ostr));
						if (((-1 == num7) ? 1 : 0) != 0)
						{
							num2 |= 4;
							break;
						}
						num5 -= 1U;
					}
					goto IL_12B;
					IL_BE:
					sbyte* ptr;
					if (16 <= *(_Str + 20))
					{
						ptr = *_Str;
					}
					else
					{
						ptr = _Str;
					}
					long num8 = (long)((ulong)num3);
					if (<Module>.std.basic_streambuf<char,std::char_traits<char>\u0020>.sputn(<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.rdbuf(*(*_Ostr + 4) + _Ostr), ptr, num8) == num8)
					{
						goto IL_F1;
					}
					num2 = 4;
					IL_12B:
					<Module>.std.ios_base.width(*(*_Ostr + 4) + _Ostr, 0L);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					exceptionCode = (uint)Marshal.GetExceptionCode();
					endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
				})
				{
					uint num9 = 0U;
					<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
					try
					{
						try
						{
							<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*_Ostr + 4) + _Ostr, 4, true);
							goto IL_197;
						}
						catch when (delegate
						{
							// Failed to create a 'catch-when' expression
							num9 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
							endfilter(num9 != 0U);
						})
						{
						}
						if (num9 != 0U)
						{
							throw;
						}
					}
					finally
					{
						<Module>.__CxxUnregisterExceptionObject(num, (int)num9);
					}
				}
			}
			else
			{
				num2 = 4;
			}
			IL_197:
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*_Ostr + 4) + _Ostr, num2, false);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>.sentry.{dtor}), (void*)(&sentry));
			throw;
		}
		try
		{
			if (<Module>.std.uncaught_exception() == null)
			{
				<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Osfx(sentry);
			}
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}), (void*)(&sentry));
			throw;
		}
		<Module>.std.basic_ostream<char,std::char_traits<char>\u0020>._Sentry_base.{dtor}(ref sentry);
		return _Ostr;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x00005254 File Offset: 0x00004654
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator==<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		uint count = (uint)(*(_Right + 16));
		sbyte* ptr;
		if (16 <= *(_Right + 20))
		{
			ptr = *_Right;
		}
		else
		{
			ptr = _Right;
		}
		return (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(_Left, 0U, (uint)(*(_Left + 16)), ptr, count) == 0) ? 1 : 0;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x00005608 File Offset: 0x00004A08
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator!=<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, sbyte* _Right)
	{
		return (((<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(_Left, _Right) == 0) ? 1 : 0) == 0) ? 1 : 0;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000528C File Offset: 0x0000468C
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator!=<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Right)
	{
		return (<Module>.std.operator==<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(_Left, _Right) == 0) ? 1 : 0;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00004720 File Offset: 0x00003B20
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator!=<char,char>(allocator<char>* _Left, allocator<char>* _Right)
	{
		return 0;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x00004730 File Offset: 0x00003B30
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator!=<class\u0020std::allocator<char>,class\u0020std::allocator<char>\u0020>(_Wrap_alloc<std::allocator<char>\u0020>* _Left, _Wrap_alloc<std::allocator<char>\u0020>* _Right)
	{
		return 0;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00003BC4 File Offset: 0x00002FC4
	internal unsafe static FfuReader.WriteRequest* addressof<struct\u0020FfuReader::WriteRequest\u0020const\u0020>(FfuReader.WriteRequest* _Val)
	{
		return _Val;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00004E6C File Offset: 0x0000426C
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest\u0020&>(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x00004EAC File Offset: 0x000042AC
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest\u0020const\u0020&>(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _V0, 40);
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x00003BD4 File Offset: 0x00002FD4
	internal unsafe static FfuReader.BlockDataEntry* addressof<struct\u0020FfuReader::BlockDataEntry\u0020const\u0020>(FfuReader.BlockDataEntry* _Val)
	{
		return _Val;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x00004EC4 File Offset: 0x000042C4
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry\u0020&>(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000176 RID: 374 RVA: 0x00004F04 File Offset: 0x00004304
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry\u0020const\u0020&>(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _V0, 24);
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00003BE4 File Offset: 0x00002FE4
	internal unsafe static FfuReader.partition* addressof<struct\u0020FfuReader::partition\u0020const\u0020>(FfuReader.partition* _Val)
	{
		return _Val;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x000072F8 File Offset: 0x000066F8
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition\u0020&>(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00008B30 File Offset: 0x00007F30
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition\u0020const\u0020&>(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00003BF4 File Offset: 0x00002FF4
	internal unsafe static codecvt<char,char,int>* use_facet<class\u0020std::codecvt<char,char,int>\u0020>(locale* _Loc)
	{
		bool flag = false;
		int num = 0;
		RuntimeHelpers.PrepareConstrainedRegions();
		codecvt<char,char,int>* result;
		try
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				<Module>.std._Lockit._Lockit_ctor(num);
				flag = true;
			}
			locale.facet* ptr = <Module>.?_Psave@?$_Facetptr@V?$codecvt@DDH@std@@@std@@2PBVfacet@locale@2@B;
			uint id = <Module>.std.locale.id..I(<Module>.__imp_?id@?$codecvt@DDH@std@@2V0locale@2@A);
			locale.facet* ptr2 = <Module>.std.locale._Getfacet(_Loc, id);
			if (ptr2 == null)
			{
				if (ptr != null)
				{
					ptr2 = ptr;
				}
				else
				{
					if (<Module>.std.codecvt<char,char,int>._Getcat(&ptr, _Loc) == -1)
					{
						bad_cast bad_cast;
						<Module>.std.bad_cast.{ctor}(ref bad_cast, (sbyte*)(&<Module>.??_C@_08EPJLHIJG@bad?5cast?$AA@));
						<Module>._CxxThrowException((void*)(&bad_cast), (_s__ThrowInfo*)(&<Module>._TI2?AVbad_cast@std@@));
					}
					ptr2 = ptr;
					<Module>.?_Psave@?$_Facetptr@V?$codecvt@DDH@std@@@std@@2PBVfacet@locale@2@B = ptr;
					_Facet_base* @this = (_Facet_base*)ptr;
					locale.facet* ptr3 = ptr;
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr3, *(*(int*)ptr3 + 4));
					<Module>.std._Facet_Register_m(@this);
				}
			}
			result = ptr2;
		}
		finally
		{
			if (flag)
			{
				<Module>.std._Lockit._Lockit_dtor(num);
			}
		}
		return result;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00003CC4 File Offset: 0x000030C4
	internal unsafe static void _Bool_function<class\u0020std::basic_ostream<char,struct\u0020std::char_traits<char>\u0020>\u0020>(_Bool_struct<std::basic_ostream<char,std::char_traits<char>\u0020>\u0020>* A_0)
	{
	}

	// Token: 0x0600017C RID: 380 RVA: 0x00005624 File Offset: 0x00004A24
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>\u0020>\u0020>(FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last, _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* _Al)
	{
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00005A24 File Offset: 0x00004E24
	internal unsafe static FfuReader.WriteRequest* _Umove<struct\u0020FfuReader::WriteRequest\u0020*>(vector<FfuReader::WriteRequest,std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last, FfuReader.WriteRequest* _Ptr)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020> wrap_alloc<std::allocator<FfuReader::WriteRequest>_u0020>;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020FfuReader::WriteRequest\u0020*,class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>,struct\u0020FfuReader::WriteRequest>(_First, _Last, _Ptr, ref wrap_alloc<std::allocator<FfuReader::WriteRequest>_u0020>, null, _unnamed);
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00005634 File Offset: 0x00004A34
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>\u0020>\u0020>(FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last, _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* _Al)
	{
	}

	// Token: 0x0600017F RID: 383 RVA: 0x00005A40 File Offset: 0x00004E40
	internal unsafe static FfuReader.BlockDataEntry* _Umove<struct\u0020FfuReader::BlockDataEntry\u0020*>(vector<FfuReader::BlockDataEntry,std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last, FfuReader.BlockDataEntry* _Ptr)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020> wrap_alloc<std::allocator<FfuReader::BlockDataEntry>_u0020>;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020FfuReader::BlockDataEntry\u0020*,class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>,struct\u0020FfuReader::BlockDataEntry>(_First, _Last, _Ptr, ref wrap_alloc<std::allocator<FfuReader::BlockDataEntry>_u0020>, null, _unnamed);
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00009F70 File Offset: 0x00009370
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::partition>\u0020>\u0020>(FfuReader.partition* _First, FfuReader.partition* _Last, _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* _Al)
	{
		FfuReader.partition* ptr = _First;
		if (_First != _Last)
		{
			do
			{
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, true, 0U);
				ptr += 64 / sizeof(FfuReader.partition);
			}
			while (ptr != _Last);
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00009FD4 File Offset: 0x000093D4
	internal unsafe static FfuReader.partition* _Umove<struct\u0020FfuReader::partition\u0020*>(vector<FfuReader::partition,std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _First, FfuReader.partition* _Last, FfuReader.partition* _Ptr)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		_Wrap_alloc<std::allocator<FfuReader::partition>\u0020> wrap_alloc<std::allocator<FfuReader::partition>_u0020>;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::partition\u0020*,struct\u0020FfuReader::partition\u0020*,class\u0020std::allocator<struct\u0020FfuReader::partition>,struct\u0020FfuReader::partition>(_First, _Last, _Ptr, ref wrap_alloc<std::allocator<FfuReader::partition>_u0020>, null, _unnamed);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00003CD4 File Offset: 0x000030D4
	internal unsafe static FfuReader.WriteRequest* _Allocate<struct\u0020FfuReader::WriteRequest>(uint _Count, FfuReader.WriteRequest* __unnamed001)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (107374182U >= _Count)
			{
				ptr = <Module>.@new(_Count * 40U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x00003D04 File Offset: 0x00003104
	internal unsafe static FfuReader.BlockDataEntry* _Allocate<struct\u0020FfuReader::BlockDataEntry>(uint _Count, FfuReader.BlockDataEntry* __unnamed001)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (178956970U >= _Count)
			{
				ptr = <Module>.@new(_Count * 24U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00003D34 File Offset: 0x00003134
	internal unsafe static FfuReader.partition* _Allocate<struct\u0020FfuReader::partition>(uint _Count, FfuReader.partition* __unnamed001)
	{
		void* ptr = null;
		if (_Count != 0U)
		{
			if (67108863U >= _Count)
			{
				ptr = <Module>.@new(_Count * 64U);
				if (ptr != null)
				{
					return ptr;
				}
			}
			<Module>.std._Xbad_alloc();
			return 0;
		}
		return ptr;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00011FA0 File Offset: 0x000113A0
	internal static void ??__F?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A@@YMXXZ()
	{
		<Module>.?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00011FC0 File Offset: 0x000113C0
	internal static void ??__F?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A@@YMXXZ()
	{
		<Module>.?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00011FE0 File Offset: 0x000113E0
	internal static void ??__F?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A@@YMXXZ()
	{
		<Module>.?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A = ref <Module>.??_7error_category@std@@6B@;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00005E94 File Offset: 0x00005294
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* +=(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, sbyte* _Ptr)
	{
		uint count;
		if (*(sbyte*)_Ptr == 0)
		{
			count = 0U;
		}
		else
		{
			sbyte* ptr = _Ptr;
			do
			{
				ptr += 1 / sizeof(sbyte);
			}
			while (*(sbyte*)ptr != 0);
			count = (uint)(ptr - _Ptr);
		}
		return <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.append(A_0, _Ptr, count);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00005644 File Offset: 0x00004A44
	internal unsafe static void reserve(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint _Newcap)
	{
		uint num = (uint)(*(A_0 + 16));
		if (num <= _Newcap && *(A_0 + 20) != (int)_Newcap)
		{
			uint num2 = num;
			if (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Grow(A_0, _Newcap, true) != null)
			{
				*(A_0 + 16) = (int)num2;
				*(((16 > *(A_0 + 20)) ? A_0 : (*A_0)) + num2) = 0;
			}
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x00011E0C File Offset: 0x0001120C
	internal static void ??__E?id@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2V0locale@2@A@@YMXXZ()
	{
		<Module>.std.locale.id.{ctor}(ref <Module>.?id@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2V0locale@2@A, 0U);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00011E28 File Offset: 0x00011228
	internal static void ??__E?id@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2V0locale@2@A@@YMXXZ()
	{
		<Module>.std.locale.id.{ctor}(ref <Module>.?id@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2V0locale@2@A, 0U);
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00003D64 File Offset: 0x00003164
	internal unsafe static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* move<class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>\u0020&>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Arg)
	{
		return _Arg;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x000052A4 File Offset: 0x000046A4
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator==<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>(basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* _Left, sbyte* _Right)
	{
		return (<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.compare(_Left, _Right) == 0) ? 1 : 0;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00003D74 File Offset: 0x00003174
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator==<char,char>(allocator<char>* A_0, allocator<char>* A_1)
	{
		return 1;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00003D84 File Offset: 0x00003184
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool operator==<class\u0020std::allocator<char>,class\u0020std::allocator<char>\u0020>(_Wrap_alloc<std::allocator<char>\u0020>* _Left, _Wrap_alloc<std::allocator<char>\u0020>* _Right)
	{
		return 1;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00003D94 File Offset: 0x00003194
	internal unsafe static FfuReader.WriteRequest* forward<struct\u0020FfuReader::WriteRequest\u0020&>(FfuReader.WriteRequest* _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x00004740 File Offset: 0x00003B40
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest\u0020&>(allocator<FfuReader::WriteRequest>* _Al, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x00003DA4 File Offset: 0x000031A4
	internal unsafe static FfuReader.WriteRequest* forward<struct\u0020FfuReader::WriteRequest\u0020const\u0020&>(FfuReader.WriteRequest* _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x00004780 File Offset: 0x00003B80
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest\u0020const\u0020&>(allocator<FfuReader::WriteRequest>* _Al, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _V0, 40);
		}
	}

	// Token: 0x06000194 RID: 404 RVA: 0x00003DB4 File Offset: 0x000031B4
	internal unsafe static FfuReader.BlockDataEntry* forward<struct\u0020FfuReader::BlockDataEntry\u0020&>(FfuReader.BlockDataEntry* _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x00004798 File Offset: 0x00003B98
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry\u0020&>(allocator<FfuReader::BlockDataEntry>* _Al, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x06000196 RID: 406 RVA: 0x00003DC4 File Offset: 0x000031C4
	internal unsafe static FfuReader.BlockDataEntry* forward<struct\u0020FfuReader::BlockDataEntry\u0020const\u0020&>(FfuReader.BlockDataEntry* _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x000047D8 File Offset: 0x00003BD8
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry\u0020const\u0020&>(allocator<FfuReader::BlockDataEntry>* _Al, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _V0, 24);
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00003DD4 File Offset: 0x000031D4
	internal unsafe static FfuReader.partition* forward<struct\u0020FfuReader::partition\u0020&>(FfuReader.partition* _Arg)
	{
		return _Arg;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x00006C08 File Offset: 0x00006008
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition\u0020&>(allocator<FfuReader::partition>* _Al, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00003DE4 File Offset: 0x000031E4
	internal unsafe static FfuReader.partition* forward<struct\u0020FfuReader::partition\u0020const\u0020&>(FfuReader.partition* _Arg)
	{
		return _Arg;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000733C File Offset: 0x0000673C
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition\u0020const\u0020&>(allocator<FfuReader::partition>* _Al, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00003DF4 File Offset: 0x000031F4
	internal unsafe static _Nonscalar_ptr_iterator_tag _Ptr_cat<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest>(FfuReader.WriteRequest* A_0, FfuReader.WriteRequest* A_1)
	{
		_Nonscalar_ptr_iterator_tag result;
		return result;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x000052BC File Offset: 0x000046BC
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>\u0020>\u0020>(FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last, _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* _Al, _Nonscalar_ptr_iterator_tag __unnamed003)
	{
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00005688 File Offset: 0x00004A88
	internal unsafe static FfuReader.WriteRequest* _Uninitialized_move<struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>\u0020>\u0020>(FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last, FfuReader.WriteRequest* _Dest, _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* _Al)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020FfuReader::WriteRequest\u0020*,class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>,struct\u0020FfuReader::WriteRequest>(_First, _Last, _Dest, _Al, null, _unnamed);
	}

	// Token: 0x0600019F RID: 415 RVA: 0x00003E04 File Offset: 0x00003204
	internal unsafe static _Nonscalar_ptr_iterator_tag _Ptr_cat<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry>(FfuReader.BlockDataEntry* A_0, FfuReader.BlockDataEntry* A_1)
	{
		_Nonscalar_ptr_iterator_tag result;
		return result;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000052CC File Offset: 0x000046CC
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>\u0020>\u0020>(FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last, _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* _Al, _Nonscalar_ptr_iterator_tag __unnamed003)
	{
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x000056A4 File Offset: 0x00004AA4
	internal unsafe static FfuReader.BlockDataEntry* _Uninitialized_move<struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>\u0020>\u0020>(FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last, FfuReader.BlockDataEntry* _Dest, _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* _Al)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020FfuReader::BlockDataEntry\u0020*,class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>,struct\u0020FfuReader::BlockDataEntry>(_First, _Last, _Dest, _Al, null, _unnamed);
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00003E14 File Offset: 0x00003214
	internal unsafe static _Nonscalar_ptr_iterator_tag _Ptr_cat<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition>(FfuReader.partition* A_0, FfuReader.partition* A_1)
	{
		_Nonscalar_ptr_iterator_tag result;
		return result;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00008B74 File Offset: 0x00007F74
	internal unsafe static void _Destroy_range<struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::partition>\u0020>\u0020>(FfuReader.partition* _First, FfuReader.partition* _Last, _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* _Al, _Nonscalar_ptr_iterator_tag __unnamed003)
	{
		if (_First != _Last)
		{
			do
			{
				<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(_First, true, 0U);
				_First += 64 / sizeof(FfuReader.partition);
			}
			while (_First != _Last);
		}
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00009F94 File Offset: 0x00009394
	internal unsafe static FfuReader.partition* _Uninitialized_move<struct\u0020FfuReader::partition\u0020*,struct\u0020FfuReader::partition\u0020*,struct\u0020std::_Wrap_alloc<class\u0020std::allocator<struct\u0020FfuReader::partition>\u0020>\u0020>(FfuReader.partition* _First, FfuReader.partition* _Last, FfuReader.partition* _Dest, _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* _Al)
	{
		_Nonscalar_ptr_iterator_tag nonscalar_ptr_iterator_tag;
		_Nonscalar_ptr_iterator_tag _unnamed = nonscalar_ptr_iterator_tag;
		return <Module>.std._Uninit_move<struct\u0020FfuReader::partition\u0020*,struct\u0020FfuReader::partition\u0020*,class\u0020std::allocator<struct\u0020FfuReader::partition>,struct\u0020FfuReader::partition>(_First, _Last, _Dest, _Al, null, _unnamed);
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00003E24 File Offset: 0x00003224
	internal unsafe static void construct(allocator<FfuReader::WriteRequest>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _Val)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _Val, 40);
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00003E3C File Offset: 0x0000323C
	internal unsafe static void construct(allocator<FfuReader::BlockDataEntry>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _Val)
	{
		if (_Ptr != null)
		{
			cpblk(_Ptr, _Val, 24);
		}
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00006C4C File Offset: 0x0000604C
	internal unsafe static void construct(allocator<FfuReader::partition>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _Val)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _Val);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00005EC0 File Offset: 0x000052C0
	internal unsafe static FfuReader.partition* {ctor}(FfuReader.partition* A_0, FfuReader.partition* A_0)
	{
		*(A_0 + 16) = 0;
		*(A_0 + 20) = 0;
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, false, 0U);
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.assign(A_0, A_0, 0U, uint.MaxValue);
		try
		{
			*(A_0 + 24) = *(A_0 + 24);
			*(A_0 + 32) = *(A_0 + 32);
			*(A_0 + 40) = *(A_0 + 40);
			*(A_0 + 48) = *(A_0 + 48);
			*(A_0 + 56) = *(A_0 + 56);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), A_0);
			throw;
		}
		return A_0;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00003E54 File Offset: 0x00003254
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest\u0020&>(allocator<FfuReader::WriteRequest>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00003E94 File Offset: 0x00003294
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry\u0020&>(allocator<FfuReader::BlockDataEntry>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00005F54 File Offset: 0x00005354
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition\u0020&>(allocator<FfuReader::partition>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00004F1C File Offset: 0x0000431C
	internal unsafe static void destroy<struct\u0020FfuReader::WriteRequest>(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr)
	{
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00003ED4 File Offset: 0x000032D4
	internal unsafe static FfuReader.WriteRequest* _Val_type<struct\u0020FfuReader::WriteRequest\u0020*>(FfuReader.WriteRequest* A_0)
	{
		return 0;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x000052DC File Offset: 0x000046DC
	internal unsafe static FfuReader.WriteRequest* _Uninit_move<struct\u0020FfuReader::WriteRequest\u0020*,struct\u0020FfuReader::WriteRequest\u0020*,class\u0020std::allocator<struct\u0020FfuReader::WriteRequest>,struct\u0020FfuReader::WriteRequest>(FfuReader.WriteRequest* _First, FfuReader.WriteRequest* _Last, FfuReader.WriteRequest* _Dest, _Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* _Al, FfuReader.WriteRequest* __unnamed004, _Nonscalar_ptr_iterator_tag __unnamed005)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		FfuReader.WriteRequest* ptr = _Dest;
		uint exceptionCode;
		try
		{
			while (_First != _Last)
			{
				FfuReader.WriteRequest* a_ = _Dest;
				try
				{
					if (_Dest != null)
					{
						cpblk(_Dest, _First, 40);
					}
				}
				catch
				{
					<Module>.delete((void*)a_, (void*)_Dest);
					throw;
				}
				_Dest += 40 / sizeof(FfuReader.WriteRequest);
				_First += 40 / sizeof(FfuReader.WriteRequest);
			}
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					while (ptr != _Dest)
					{
						ptr += 40 / sizeof(FfuReader.WriteRequest);
					}
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		return _Dest;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00004F2C File Offset: 0x0000432C
	internal unsafe static void destroy<struct\u0020FfuReader::BlockDataEntry>(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr)
	{
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00003EE4 File Offset: 0x000032E4
	internal unsafe static FfuReader.BlockDataEntry* _Val_type<struct\u0020FfuReader::BlockDataEntry\u0020*>(FfuReader.BlockDataEntry* A_0)
	{
		return 0;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x000053EC File Offset: 0x000047EC
	internal unsafe static FfuReader.BlockDataEntry* _Uninit_move<struct\u0020FfuReader::BlockDataEntry\u0020*,struct\u0020FfuReader::BlockDataEntry\u0020*,class\u0020std::allocator<struct\u0020FfuReader::BlockDataEntry>,struct\u0020FfuReader::BlockDataEntry>(FfuReader.BlockDataEntry* _First, FfuReader.BlockDataEntry* _Last, FfuReader.BlockDataEntry* _Dest, _Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* _Al, FfuReader.BlockDataEntry* __unnamed004, _Nonscalar_ptr_iterator_tag __unnamed005)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		FfuReader.BlockDataEntry* ptr = _Dest;
		uint exceptionCode;
		try
		{
			while (_First != _Last)
			{
				FfuReader.BlockDataEntry* a_ = _Dest;
				try
				{
					if (_Dest != null)
					{
						cpblk(_Dest, _First, 24);
					}
				}
				catch
				{
					<Module>.delete((void*)a_, (void*)_Dest);
					throw;
				}
				_Dest += 24 / sizeof(FfuReader.BlockDataEntry);
				_First += 24 / sizeof(FfuReader.BlockDataEntry);
			}
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					while (ptr != _Dest)
					{
						ptr += 24 / sizeof(FfuReader.BlockDataEntry);
					}
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		return _Dest;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00007380 File Offset: 0x00006780
	internal unsafe static void destroy<struct\u0020FfuReader::partition>(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(_Ptr, true, 0U);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00003EF4 File Offset: 0x000032F4
	internal unsafe static FfuReader.partition* _Val_type<struct\u0020FfuReader::partition\u0020*>(FfuReader.partition* A_0)
	{
		return 0;
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00008B98 File Offset: 0x00007F98
	internal unsafe static FfuReader.partition* _Uninit_move<struct\u0020FfuReader::partition\u0020*,struct\u0020FfuReader::partition\u0020*,class\u0020std::allocator<struct\u0020FfuReader::partition>,struct\u0020FfuReader::partition>(FfuReader.partition* _First, FfuReader.partition* _Last, FfuReader.partition* _Dest, _Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* _Al, FfuReader.partition* __unnamed004, _Nonscalar_ptr_iterator_tag __unnamed005)
	{
		int num = (int)stackalloc byte[<Module>.__CxxQueryExceptionSize()];
		FfuReader.partition* ptr = _Dest;
		uint exceptionCode;
		try
		{
			while (_First != _Last)
			{
				FfuReader.partition* a_ = _Dest;
				try
				{
					if (_Dest != null)
					{
						FfuReader.partition* ptr2 = <Module>.FfuReader.partition.{ctor}(_Dest, _First);
					}
				}
				catch
				{
					<Module>.delete((void*)a_, (void*)_Dest);
					throw;
				}
				_Dest += 64 / sizeof(FfuReader.partition);
				_First += 64 / sizeof(FfuReader.partition);
			}
		}
		catch when (delegate
		{
			// Failed to create a 'catch-when' expression
			exceptionCode = (uint)Marshal.GetExceptionCode();
			endfilter(<Module>.__CxxExceptionFilter(Marshal.GetExceptionPointers(), null, 0, null) != null);
		})
		{
			uint num2 = 0U;
			<Module>.__CxxRegisterExceptionObject(Marshal.GetExceptionPointers(), num);
			try
			{
				try
				{
					while (ptr != _Dest)
					{
						<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ptr, true, 0U);
						ptr += 64 / sizeof(FfuReader.partition);
					}
					<Module>._CxxThrowException(null, null);
				}
				catch when (delegate
				{
					// Failed to create a 'catch-when' expression
					num2 = <Module>.__CxxDetectRethrow(Marshal.GetExceptionPointers());
					endfilter(num2 != 0U);
				})
				{
				}
				if (num2 != 0U)
				{
					throw;
				}
			}
			finally
			{
				<Module>.__CxxUnregisterExceptionObject(num, (int)num2);
			}
		}
		return _Dest;
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x000047F0 File Offset: 0x00003BF0
	internal unsafe static void destroy<struct\u0020FfuReader::WriteRequest>(allocator<FfuReader::WriteRequest>* _Al, FfuReader.WriteRequest* _Ptr)
	{
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00004F3C File Offset: 0x0000433C
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest>(_Wrap_alloc<std::allocator<FfuReader::WriteRequest>\u0020>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00004800 File Offset: 0x00003C00
	internal unsafe static void destroy<struct\u0020FfuReader::BlockDataEntry>(allocator<FfuReader::BlockDataEntry>* _Al, FfuReader.BlockDataEntry* _Ptr)
	{
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00004F7C File Offset: 0x0000437C
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry>(_Wrap_alloc<std::allocator<FfuReader::BlockDataEntry>\u0020>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00006C90 File Offset: 0x00006090
	internal unsafe static void destroy<struct\u0020FfuReader::partition>(allocator<FfuReader::partition>* _Al, FfuReader.partition* _Ptr)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(_Ptr, true, 0U);
	}

	// Token: 0x060001BA RID: 442 RVA: 0x00007398 File Offset: 0x00006798
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition>(_Wrap_alloc<std::allocator<FfuReader::partition>\u0020>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00003F04 File Offset: 0x00003304
	internal unsafe static void destroy<struct\u0020FfuReader::WriteRequest>(allocator<FfuReader::WriteRequest>* A_0, FfuReader.WriteRequest* _Ptr)
	{
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00003F14 File Offset: 0x00003314
	internal unsafe static FfuReader.WriteRequest* forward<struct\u0020FfuReader::WriteRequest>(FfuReader.WriteRequest* _Arg)
	{
		return _Arg;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00004810 File Offset: 0x00003C10
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest>(allocator<FfuReader::WriteRequest>* _Al, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00003F24 File Offset: 0x00003324
	internal unsafe static void destroy<struct\u0020FfuReader::BlockDataEntry>(allocator<FfuReader::BlockDataEntry>* A_0, FfuReader.BlockDataEntry* _Ptr)
	{
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00003F34 File Offset: 0x00003334
	internal unsafe static FfuReader.BlockDataEntry* forward<struct\u0020FfuReader::BlockDataEntry>(FfuReader.BlockDataEntry* _Arg)
	{
		return _Arg;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00004850 File Offset: 0x00003C50
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry>(allocator<FfuReader::BlockDataEntry>* _Al, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00005F98 File Offset: 0x00005398
	internal unsafe static void destroy<struct\u0020FfuReader::partition>(allocator<FfuReader::partition>* A_0, FfuReader.partition* _Ptr)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(_Ptr, true, 0U);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00003F44 File Offset: 0x00003344
	internal unsafe static FfuReader.partition* forward<struct\u0020FfuReader::partition>(FfuReader.partition* _Arg)
	{
		return _Arg;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00006CA8 File Offset: 0x000060A8
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition>(allocator<FfuReader::partition>* _Al, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00005A5C File Offset: 0x00004E5C
	internal unsafe static void* __delDtor(FfuReader.partition* A_0, uint A_0)
	{
		<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(A_0, true, 0U);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00003F54 File Offset: 0x00003354
	internal unsafe static void construct<struct\u0020FfuReader::WriteRequest,struct\u0020FfuReader::WriteRequest>(allocator<FfuReader::WriteRequest>* A_0, FfuReader.WriteRequest* _Ptr, FfuReader.WriteRequest* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 40);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00003F94 File Offset: 0x00003394
	internal unsafe static void construct<struct\u0020FfuReader::BlockDataEntry,struct\u0020FfuReader::BlockDataEntry>(allocator<FfuReader::BlockDataEntry>* A_0, FfuReader.BlockDataEntry* _Ptr, FfuReader.BlockDataEntry* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				cpblk(_Ptr, _V0, 24);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00005FB0 File Offset: 0x000053B0
	internal unsafe static void construct<struct\u0020FfuReader::partition,struct\u0020FfuReader::partition>(allocator<FfuReader::partition>* A_0, FfuReader.partition* _Ptr, FfuReader.partition* _V0)
	{
		try
		{
			if (_Ptr != null)
			{
				<Module>.FfuReader.partition.{ctor}(_Ptr, _V0);
			}
		}
		catch
		{
			<Module>.delete((void*)_Ptr, (void*)_Ptr);
			throw;
		}
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0000E610 File Offset: 0x0000DA10
	internal unsafe static void ?_Add_vtordisp2@?$basic_ostream@DU?$char_traits@D@std@@@std@@$$F$4PPPPPPPM@FI@AEXXZ(basic_ostream<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_ostream<char,std::char_traits<char>\u0020>* ptr = A_0;
		A_0 = ptr - *(ptr + -4);
		A_0 -= 88;
		jmp(std.basic_ostream<char,std::char_traits<char>\u0020>._Add_vtordisp2());
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0000E228 File Offset: 0x0000D628
	internal unsafe static void* ??_E?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@$$F$4PPPPPPPM@A@AEPAXI@Z(basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* A_0, uint A_0)
	{
		basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>* ptr = A_0;
		A_0 = ptr - *(ptr + -4);
		jmp(std.basic_stringstream<char,std::char_traits<char>,std::allocator<char>\u0020>.__vecDelDtor());
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000E590 File Offset: 0x0000D990
	internal unsafe static void* ??_E?$basic_ofstream@DU?$char_traits@D@std@@@std@@$$F$4PPPPPPPM@A@AEPAXI@Z(basic_ofstream<char,std::char_traits<char>\u0020>* A_0, uint A_0)
	{
		basic_ofstream<char,std::char_traits<char>\u0020>* ptr = A_0;
		A_0 = ptr - *(ptr + -4);
		jmp(std.basic_ofstream<char,std::char_traits<char>\u0020>.__vecDelDtor());
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000E2D0 File Offset: 0x0000D6D0
	internal unsafe static void ?_Add_vtordisp2@?$basic_ostream@DU?$char_traits@D@std@@@std@@$$F$4PPPPPPPM@FA@AEXXZ(basic_ostream<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_ostream<char,std::char_traits<char>\u0020>* ptr = A_0;
		A_0 = ptr - *(ptr + -4);
		A_0 -= 80;
		jmp(std.basic_ostream<char,std::char_traits<char>\u0020>._Add_vtordisp2());
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0000E2A8 File Offset: 0x0000D6A8
	internal unsafe static void ?_Add_vtordisp1@?$basic_istream@DU?$char_traits@D@std@@@std@@$$F$4PPPPPPPM@FA@AEXXZ(basic_istream<char,std::char_traits<char>\u0020>* A_0)
	{
		basic_istream<char,std::char_traits<char>\u0020>* ptr = A_0;
		A_0 = ptr - *(ptr + -4);
		A_0 -= 80;
		jmp(std.basic_istream<char,std::char_traits<char>\u0020>._Add_vtordisp1());
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0000FF28 File Offset: 0x0000F328
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInDllMain()
	{
		return (<Module>.__native_dllmain_reason != uint.MaxValue) ? 1 : 0;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0000FF44 File Offset: 0x0000F344
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInProcessAttach()
	{
		return (<Module>.__native_dllmain_reason == 1U) ? 1 : 0;
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000FF5C File Offset: 0x0000F35C
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsInProcessDetach()
	{
		return (<Module>.__native_dllmain_reason == 0U) ? 1 : 0;
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0000FF74 File Offset: 0x0000F374
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.NativeDll.IsSafeForManagedCode()
	{
		if (((<Module>.__native_dllmain_reason != 4294967295U) ? 1 : 0) == 0)
		{
			return 1;
		}
		int num;
		if (((<Module>.__native_dllmain_reason == 1U) ? 1 : 0) == 0 && ((<Module>.__native_dllmain_reason == 0U) ? 1 : 0) == 0)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0001078C File Offset: 0x0000FB8C
	internal static void <CrtImplementationDetails>.ThrowNestedModuleLoadException(System.Exception innerException, System.Exception nestedException)
	{
		throw new ModuleLoadExceptionHandlerException("A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n", innerException, nestedException);
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00010174 File Offset: 0x0000F574
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage)
	{
		throw new ModuleLoadException(errorMessage);
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00010188 File Offset: 0x0000F588
	internal static void <CrtImplementationDetails>.ThrowModuleLoadException(string errorMessage, System.Exception innerException)
	{
		throw new ModuleLoadException(errorMessage, innerException);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000102A4 File Offset: 0x0000F6A4
	internal static void <CrtImplementationDetails>.RegisterModuleUninitializer(EventHandler handler)
	{
		ModuleUninitializer._ModuleUninitializer.AddHandler(handler);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000102BC File Offset: 0x0000F6BC
	[SecuritySafeCritical]
	internal unsafe static Guid <CrtImplementationDetails>.FromGUID(_GUID* guid)
	{
		Guid result = new Guid((uint)(*guid), *(guid + 4), *(guid + 6), *(guid + 8), *(guid + 9), *(guid + 10), *(guid + 11), *(guid + 12), *(guid + 13), *(guid + 14), *(guid + 15));
		return result;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00010304 File Offset: 0x0000F704
	[SecurityCritical]
	internal unsafe static int __get_default_appdomain(IUnknown** ppUnk)
	{
		ICorRuntimeHost* ptr = null;
		int num;
		try
		{
			Guid riid = <Module>.<CrtImplementationDetails>.FromGUID(ref <Module>._GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e);
			Guid clsid = <Module>.<CrtImplementationDetails>.FromGUID(ref <Module>._GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e);
			ptr = (ICorRuntimeHost*)RuntimeEnvironment.GetRuntimeInterfaceAsIntPtr(clsid, riid).ToPointer();
			goto IL_3D;
		}
		catch (System.Exception e)
		{
			num = Marshal.GetHRForException(e);
		}
		if (num < 0)
		{
			return num;
		}
		IL_3D:
		num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,IUnknown**), ptr, ppUnk, *(*(int*)ptr + 52));
		ICorRuntimeHost* ptr2 = ptr;
		object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, *(*(int*)ptr2 + 8));
		return num;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00010388 File Offset: 0x0000F788
	internal unsafe static void __release_appdomain(IUnknown* ppUnk)
	{
		object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ppUnk, *(*(int*)ppUnk + 8));
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x000103A4 File Offset: 0x0000F7A4
	[SecurityCritical]
	internal unsafe static AppDomain <CrtImplementationDetails>.GetDefaultDomain()
	{
		IUnknown* ptr = null;
		int num = <Module>.__get_default_appdomain(&ptr);
		if (num >= 0)
		{
			try
			{
				IntPtr pUnk = new IntPtr((void*)ptr);
				return (AppDomain)Marshal.GetObjectForIUnknown(pUnk);
			}
			finally
			{
				<Module>.__release_appdomain(ptr);
			}
		}
		Marshal.ThrowExceptionForHR(num);
		return null;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00010404 File Offset: 0x0000F804
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.DoCallBackInDefaultDomain(method function, void* cookie)
	{
		Guid riid = <Module>.<CrtImplementationDetails>.FromGUID(ref <Module>._GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02);
		ICLRRuntimeHost* ptr = (ICLRRuntimeHost*)RuntimeEnvironment.GetRuntimeInterfaceAsIntPtr(<Module>.<CrtImplementationDetails>.FromGUID(ref <Module>._GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02), riid).ToPointer();
		try
		{
			AppDomain appDomain = <Module>.<CrtImplementationDetails>.GetDefaultDomain();
			int num = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr,System.UInt32 modopt(System.Runtime.CompilerServices.IsLong),System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall) (System.Void*),System.Void*), ptr, appDomain.Id, function, cookie, *(*(int*)ptr + 32));
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}
		finally
		{
			ICLRRuntimeHost* ptr2 = ptr;
			object obj = calli(System.UInt32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvStdcall)(System.IntPtr), ptr2, *(*(int*)ptr2 + 8));
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x000104BC File Offset: 0x0000F8BC
	[SecuritySafeCritical]
	internal unsafe static int <CrtImplementationDetails>.DefaultDomain.DoNothing(void* cookie)
	{
		GC.KeepAlive(int.MaxValue);
		return 0;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000104DC File Offset: 0x0000F8DC
	[SecuritySafeCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasPerProcess()
	{
		if (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.?A0x357a6285.__xc_mp_a);
			if (ref <Module>.?A0x357a6285.__xc_mp_a < ref <Module>.?A0x357a6285.__xc_mp_z)
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.?A0x357a6285.__xc_mp_z))
					{
						goto IL_34;
					}
				}
				<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_34:
			<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00010530 File Offset: 0x0000F930
	[SecuritySafeCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static bool <CrtImplementationDetails>.DefaultDomain.HasNative()
	{
		if (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)2)
		{
			void** ptr = (void**)(&<Module>.__xi_a);
			if (ref <Module>.__xi_a < ref <Module>.__xi_z)
			{
				while (*(int*)ptr == 0)
				{
					ptr += 4 / sizeof(void*);
					if (ptr >= (void**)(&<Module>.__xi_z))
					{
						goto IL_34;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_34:
			void** ptr2 = (void**)(&<Module>.__xc_a);
			if (ref <Module>.__xc_a < ref <Module>.__xc_z)
			{
				while (*(int*)ptr2 == 0)
				{
					ptr2 += 4 / sizeof(void*);
					if (ptr2 >= (void**)(&<Module>.__xc_z))
					{
						goto IL_60;
					}
				}
				<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)(-1);
				return 1;
			}
			IL_60:
			<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A = (TriBool.State)0;
			return 0;
		}
		return (<Module>.?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A == (TriBool.State)(-1)) ? 1 : 0;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000105B0 File Offset: 0x0000F9B0
	[SecuritySafeCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.DefaultDomain.NeedsInitialization()
	{
		int num;
		if ((<Module>.<CrtImplementationDetails>.DefaultDomain.HasPerProcess() != null && !<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA) || (<Module>.<CrtImplementationDetails>.DefaultDomain.HasNative() != null && !<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA && <Module>.__native_startup_state == (__enative_startup_state)0))
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		return (byte)num;
	}

	// Token: 0x060001DE RID: 478 RVA: 0x000105EC File Offset: 0x0000F9EC
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.DefaultDomain.NeedsUninitialization()
	{
		return <Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x00010600 File Offset: 0x0000FA00
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.DefaultDomain.Initialize()
	{
		<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00011E68 File Offset: 0x00011268
	internal static void ??__E?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00011E7C File Offset: 0x0001127C
	internal static void ??__E?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA@@YMXXZ()
	{
		<Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 0;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00011E90 File Offset: 0x00011290
	internal static void ??__E?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA@@YMXXZ()
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = false;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00011EA4 File Offset: 0x000112A4
	internal static void ??__E?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00011EB8 File Offset: 0x000112B8
	internal static void ??__E?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00011ECC File Offset: 0x000112CC
	internal static void ??__E?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00011EE0 File Offset: 0x000112E0
	internal static void ??__E?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A@@YMXXZ()
	{
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)0;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x000107E0 File Offset: 0x0000FBE0
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeVtables(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during vtable initialization.\n");
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initterm_m((method*)(&<Module>.?A0x357a6285.__xi_vt_a), (method*)(&<Module>.?A0x357a6285.__xi_vt_z));
		<Module>.?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00010814 File Offset: 0x0000FC14
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load while attempting to initialize the default appdomain.\n");
		<Module>.<CrtImplementationDetails>.DefaultDomain.Initialize();
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00010834 File Offset: 0x0000FC34
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeNative(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during native initialization.\n");
		<Module>.__security_init_cookie();
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		if (<Module>.<CrtImplementationDetails>.NativeDll.IsSafeForManagedCode() == null)
		{
			<Module>._amsg_exit(33);
		}
		if (<Module>.__native_startup_state == (__enative_startup_state)1)
		{
			<Module>._amsg_exit(33);
		}
		else if (<Module>.__native_startup_state == (__enative_startup_state)0)
		{
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
			<Module>.__native_startup_state = (__enative_startup_state)1;
			if (<Module>._initterm_e((method*)(&<Module>.__xi_a), (method*)(&<Module>.__xi_z)) != 0)
			{
				<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0));
			}
			<Module>._initterm((method*)(&<Module>.__xc_a), (method*)(&<Module>.__xc_z));
			<Module>.__native_startup_state = (__enative_startup_state)2;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
			<Module>.?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x000108D0 File Offset: 0x0000FCD0
	[SecurityCritical]
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerProcess(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during process initialization.\n");
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_m();
		<Module>._initterm_m((method*)(&<Module>.?A0x357a6285.__xc_mp_a), (method*)(&<Module>.?A0x357a6285.__xc_mp_z));
		<Module>.?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00010910 File Offset: 0x0000FD10
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during appdomain initialization.\n");
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)1;
		<Module>._initatexit_app_domain();
		<Module>._initterm_m((method*)(&<Module>.?A0x357a6285.__xc_ma_a), (method*)(&<Module>.?A0x357a6285.__xc_ma_z));
		<Module>.?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A = (Progress.State)2;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0001094C File Offset: 0x0000FD4C
	[SecurityCritical]
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load during registration for the unload events.\n");
		<Module>.<CrtImplementationDetails>.RegisterModuleUninitializer(new EventHandler(<Module>.<CrtImplementationDetails>.LanguageSupport.DomainUnload));
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00010978 File Offset: 0x0000FD78
	[SecurityCritical]
	[DebuggerStepThrough]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport._Initialize(LanguageSupport* A_0)
	{
		<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA = AppDomain.CurrentDomain.IsDefaultAppDomain();
		if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
		{
			<Module>.?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA = true;
		}
		void* ptr = <Module>._getFiberPtrId();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			while (num2 == 0)
			{
				try
				{
				}
				finally
				{
					IntPtr comparand = (IntPtr)0;
					IntPtr value = (IntPtr)ptr;
					IntPtr value2 = Interlocked.CompareExchange(ref <Module>.__native_startup_lock, value, comparand);
					void* ptr2 = (void*)value2;
					if (ptr2 == null)
					{
						num2 = 1;
					}
					else if (ptr2 == ptr)
					{
						num = 1;
						num2 = 1;
					}
				}
				if (num2 == 0)
				{
					<Module>.Sleep(1000);
				}
			}
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeVtables(A_0);
			if (<Module>.?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeNative(A_0);
				<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerProcess(A_0);
			}
			else if (<Module>.<CrtImplementationDetails>.DefaultDomain.NeedsInitialization() != null)
			{
				num3 = 1;
			}
		}
		finally
		{
			if (num == 0)
			{
				IntPtr value3 = (IntPtr)0;
				Interlocked.Exchange(ref <Module>.__native_startup_lock, value3);
			}
		}
		if (num3 != 0)
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeDefaultAppDomain(A_0);
		}
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializePerAppDomain(A_0);
		<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA = 1;
		<Module>.<CrtImplementationDetails>.LanguageSupport.InitializeUninitializer(A_0);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00010618 File Offset: 0x0000FA18
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain()
	{
		<Module>._app_exit_callback();
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0001062C File Offset: 0x0000FA2C
	[SecurityCritical]
	internal unsafe static int <CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(void* cookie)
	{
		<Module>._exit_callback();
		<Module>.?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		if (<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA)
		{
			<Module>._cexit();
			<Module>.__native_startup_state = (__enative_startup_state)0;
			<Module>.?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		}
		<Module>.?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA = false;
		return 0;
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00010668 File Offset: 0x0000FA68
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain()
	{
		if (<Module>.<CrtImplementationDetails>.DefaultDomain.NeedsUninitialization() != null)
		{
			if (AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport._UninitializeDefaultDomain(null);
			}
			else
			{
				<Module>.<CrtImplementationDetails>.DoCallBackInDefaultDomain(<Module>.__unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z, null);
			}
		}
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0001069C File Offset: 0x0000FA9C
	[PrePrepareMethod]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.LanguageSupport.DomainUnload(object source, EventArgs arguments)
	{
		if (<Module>.?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA != 0 && Interlocked.Exchange(ref <Module>.?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA, 1) == 0)
		{
			byte b = (Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0;
			<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain();
			if (b != 0)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00010A9C File Offset: 0x0000FE9C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SecurityCritical]
	[DebuggerStepThrough]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Cleanup(LanguageSupport* A_0, System.Exception innerException)
	{
		try
		{
			bool flag = ((Interlocked.Decrement(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA) == 0) ? 1 : 0) != 0;
			<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeAppDomain();
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.UninitializeDefaultDomain();
			}
		}
		catch (System.Exception nestedException)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, nestedException);
		}
		catch (object obj)
		{
			<Module>.<CrtImplementationDetails>.ThrowNestedModuleLoadException(innerException, null);
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00010B10 File Offset: 0x0000FF10
	[SecurityCritical]
	internal unsafe static LanguageSupport* <CrtImplementationDetails>.LanguageSupport.{ctor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{ctor}(A_0);
		return A_0;
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00010B28 File Offset: 0x0000FF28
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.{dtor}(LanguageSupport* A_0)
	{
		<Module>.gcroot<System::String\u0020^>.{dtor}(A_0);
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00010B3C File Offset: 0x0000FF3C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static void <CrtImplementationDetails>.LanguageSupport.Initialize(LanguageSupport* A_0)
	{
		bool flag = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			<Module>.gcroot<System::String\u0020^>.=(A_0, "The C++ module failed to load.\n");
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Interlocked.Increment(ref <Module>.?Count@AllDomains@<CrtImplementationDetails>@@2HA);
				flag = true;
			}
			<Module>.<CrtImplementationDetails>.LanguageSupport._Initialize(A_0);
		}
		catch (System.Exception innerException)
		{
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, innerException);
			}
			<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0), innerException);
		}
		catch (object obj)
		{
			if (flag)
			{
				<Module>.<CrtImplementationDetails>.LanguageSupport.Cleanup(A_0, null);
			}
			<Module>.<CrtImplementationDetails>.ThrowModuleLoadException(<Module>.gcroot<System::String\u0020^>..P$AAVString@System@@(A_0), null);
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00010BF8 File Offset: 0x0000FFF8
	[DebuggerStepThrough]
	[SecurityCritical]
	static unsafe <Module>()
	{
		LanguageSupport languageSupport;
		<Module>.<CrtImplementationDetails>.LanguageSupport.{ctor}(ref languageSupport);
		try
		{
			<Module>.<CrtImplementationDetails>.LanguageSupport.Initialize(ref languageSupport);
		}
		catch
		{
			<Module>.___CxxCallUnwindDtor(ldftn(<CrtImplementationDetails>.LanguageSupport.{dtor}), (void*)(&languageSupport));
			throw;
		}
		<Module>.<CrtImplementationDetails>.LanguageSupport.{dtor}(ref languageSupport);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x000106D8 File Offset: 0x0000FAD8
	[SecuritySafeCritical]
	[DebuggerStepThrough]
	internal unsafe static gcroot<System::String\u0020^>* {ctor}(gcroot<System::String\u0020^>* A_0)
	{
		*A_0 = ((IntPtr)GCHandle.Alloc(null)).ToPointer();
		return A_0;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x000106FC File Offset: 0x0000FAFC
	[SecurityCritical]
	[DebuggerStepThrough]
	internal unsafe static void {dtor}(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Free();
		*A_0 = 0;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00010724 File Offset: 0x0000FB24
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static gcroot<System::String\u0020^>* =(gcroot<System::String\u0020^>* A_0, string t)
	{
		IntPtr value = new IntPtr(*A_0);
		((GCHandle)value).Target = t;
		return A_0;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0001074C File Offset: 0x0000FB4C
	[SecuritySafeCritical]
	internal unsafe static string P$AAVString@System@@(gcroot<System::String\u0020^>* A_0)
	{
		IntPtr value = new IntPtr(*A_0);
		return ((GCHandle)value).Target;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00010D58 File Offset: 0x00010158
	[HandleProcessCorruptedStateExceptions]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static void ___CxxCallUnwindDtor(method pDtor, void* pThis)
	{
		try
		{
			calli(System.Void(System.Void*), pThis, pDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00010DA4 File Offset: 0x000101A4
	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[HandleProcessCorruptedStateExceptions]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static void ___CxxCallUnwindDelDtor(method pDtor, void* pThis)
	{
		try
		{
			calli(System.Void(System.Void*), pThis, pDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00010DF0 File Offset: 0x000101F0
	[HandleProcessCorruptedStateExceptions]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static void ___CxxCallUnwindVecDtor(method pVecDtor, void* ptr, uint size, int count, method pDtor)
	{
		try
		{
			calli(System.Void(System.Void*,System.UInt32,System.Int32,System.Void (System.Void*)), ptr, size, count, pDtor, pVecDtor);
		}
		catch when (endfilter(<Module>.__FrameUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00010EBC File Offset: 0x000102BC
	[HandleProcessCorruptedStateExceptions]
	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal unsafe static void __ehvec_dtor(void* ptr, uint size, int count, method pDtor)
	{
		int num = 0;
		ptr = (void*)(size * (uint)count + (byte*)ptr);
		try
		{
			for (;;)
			{
				count--;
				if (count < 0)
				{
					break;
				}
				ptr = (void*)((byte*)ptr - size);
				calli(System.Void(System.Void*), ptr, pDtor);
			}
			num = 1;
		}
		finally
		{
			if (num == 0)
			{
				<Module>.__ArrayUnwind(ptr, size, count, pDtor);
			}
		}
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00010E40 File Offset: 0x00010240
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static int ArrayUnwindFilter(_EXCEPTION_POINTERS* pExPtrs)
	{
		if (*(*(int*)pExPtrs) != -529697949)
		{
			return 0;
		}
		<Module>.terminate();
		return 0;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00010E60 File Offset: 0x00010260
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[HandleProcessCorruptedStateExceptions]
	[SecurityCritical]
	internal unsafe static void __ArrayUnwind(void* ptr, uint size, int count, method pDtor)
	{
		try
		{
			for (;;)
			{
				count--;
				if (count < 0)
				{
					break;
				}
				ptr = (void*)((byte*)ptr - size);
				calli(System.Void(System.Void*), ptr, pDtor);
			}
		}
		catch when (endfilter(<Module>.?A0x2a616a52.ArrayUnwindFilter(Marshal.GetExceptionPointers()) != null))
		{
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00010F24 File Offset: 0x00010324
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static ValueType <CrtImplementationDetails>.AtExitLock._handle()
	{
		if (<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA != null)
		{
			IntPtr value = new IntPtr(<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA);
			return GCHandle.FromIntPtr(value);
		}
		return null;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00011428 File Offset: 0x00010828
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Construct(object value)
	{
		<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
		<Module>.<CrtImplementationDetails>.AtExitLock._lock_Set(value);
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00010F54 File Offset: 0x00010354
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Set(object value)
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType == null)
		{
			valueType = GCHandle.Alloc(value);
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = GCHandle.ToIntPtr((GCHandle)valueType).ToPointer();
		}
		else
		{
			((GCHandle)valueType).Target = value;
		}
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00010FA4 File Offset: 0x000103A4
	[DebuggerStepThrough]
	[SecurityCritical]
	internal static object <CrtImplementationDetails>.AtExitLock._lock_Get()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			return ((GCHandle)valueType).Target;
		}
		return null;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00010FC8 File Offset: 0x000103C8
	[DebuggerStepThrough]
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.AtExitLock._lock_Destruct()
	{
		ValueType valueType = <Module>.<CrtImplementationDetails>.AtExitLock._handle();
		if (valueType != null)
		{
			((GCHandle)valueType).Free();
			<Module>.?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA = null;
		}
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00010FF0 File Offset: 0x000103F0
	[DebuggerStepThrough]
	[SecuritySafeCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool <CrtImplementationDetails>.AtExitLock.IsInitialized()
	{
		return (<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get() != null) ? 1 : 0;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00011444 File Offset: 0x00010844
	[DebuggerStepThrough]
	[SecurityCritical]
	internal static void <CrtImplementationDetails>.AtExitLock.AddRef()
	{
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() == null)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Construct(new object());
			<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA = 0;
		}
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA++;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0001100C File Offset: 0x0001040C
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.RemoveRef()
	{
		<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA--;
		if (<Module>.?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA == 0)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock._lock_Destruct();
		}
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00011034 File Offset: 0x00010434
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.Enter()
	{
		Monitor.Enter(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0001104C File Offset: 0x0001044C
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void <CrtImplementationDetails>.AtExitLock.Exit()
	{
		Monitor.Exit(<Module>.<CrtImplementationDetails>.AtExitLock._lock_Get());
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00011064 File Offset: 0x00010464
	[DebuggerStepThrough]
	[SecurityCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __global_lock()
	{
		bool result = false;
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() != null)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock.Enter();
			result = true;
		}
		return result;
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00011084 File Offset: 0x00010484
	[SecurityCritical]
	[DebuggerStepThrough]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __global_unlock()
	{
		bool result = false;
		if (<Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized() != null)
		{
			<Module>.<CrtImplementationDetails>.AtExitLock.Exit();
			result = true;
		}
		return result;
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00011474 File Offset: 0x00010874
	[DebuggerStepThrough]
	[SecurityCritical]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static bool __alloc_global_lock()
	{
		<Module>.<CrtImplementationDetails>.AtExitLock.AddRef();
		return <Module>.<CrtImplementationDetails>.AtExitLock.IsInitialized();
	}

	// Token: 0x0600020E RID: 526 RVA: 0x000110A4 File Offset: 0x000104A4
	[SecurityCritical]
	[DebuggerStepThrough]
	internal static void __dealloc_global_lock()
	{
		<Module>.<CrtImplementationDetails>.AtExitLock.RemoveRef();
	}

	// Token: 0x0600020F RID: 527 RVA: 0x000110B8 File Offset: 0x000104B8
	[SecurityCritical]
	internal unsafe static int _atexit_helper(method func, uint* __pexit_list_size, method** __ponexitend_e, method** __ponexitbegin_e)
	{
		method system.Void_u0020() = 0;
		if (func == null)
		{
			return -1;
		}
		if (<Module>.?A0x0107eea2.__global_lock() == 1)
		{
			try
			{
				method* ptr = (method*)<Module>.DecodePointer(*(int*)__ponexitbegin_e);
				method* ptr2 = (method*)<Module>.DecodePointer(*(int*)__ponexitend_e);
				int num = (int)(ptr2 - ptr);
				if (*__pexit_list_size - 1U < (uint)num >> 2)
				{
					try
					{
						uint num2 = *__pexit_list_size * 4U;
						uint num3;
						if (num2 < 2048U)
						{
							num3 = num2;
						}
						else
						{
							num3 = 2048U;
						}
						IntPtr cb = new IntPtr((int)(num2 + num3));
						IntPtr pv = new IntPtr((void*)ptr);
						IntPtr intPtr = Marshal.ReAllocHGlobal(pv, cb);
						IntPtr intPtr2 = intPtr;
						ptr2 = (method*)((byte*)intPtr2.ToPointer() + num);
						ptr = (method*)intPtr2.ToPointer();
						uint num4 = *__pexit_list_size;
						uint num5;
						if (512U < num4)
						{
							num5 = 512U;
						}
						else
						{
							num5 = num4;
						}
						*__pexit_list_size = num4 + num5;
					}
					catch (OutOfMemoryException)
					{
						IntPtr cb2 = new IntPtr((int)(*__pexit_list_size * 4U + 8U));
						IntPtr pv2 = new IntPtr((void*)ptr);
						IntPtr intPtr3 = Marshal.ReAllocHGlobal(pv2, cb2);
						IntPtr intPtr4 = intPtr3;
						ptr2 = (intPtr4.ToPointer() - ptr) / (IntPtr)sizeof(method) + ptr2;
						ptr = (method*)intPtr4.ToPointer();
						*__pexit_list_size += 4U;
					}
				}
				*(int*)ptr2 = func;
				ptr2 += 4 / sizeof(method);
				system.Void_u0020() = func;
				*(int*)__ponexitbegin_e = <Module>.EncodePointer((void*)ptr);
				*(int*)__ponexitend_e = <Module>.EncodePointer((void*)ptr2);
			}
			catch (OutOfMemoryException)
			{
			}
			finally
			{
				<Module>.?A0x0107eea2.__global_unlock();
			}
			if (system.Void_u0020() != null)
			{
				return 0;
			}
		}
		return -1;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0001123C File Offset: 0x0001063C
	[SecurityCritical]
	internal unsafe static void _exit_callback()
	{
		if (<Module>.?A0x0107eea2.__exit_list_size != 0U)
		{
			method* ptr = (method*)<Module>.DecodePointer((void*)<Module>.?A0x0107eea2.__onexitbegin_m);
			method* ptr2 = (method*)<Module>.DecodePointer((void*)<Module>.?A0x0107eea2.__onexitend_m);
			if (ptr != -1 && ptr != null && ptr2 != null)
			{
				method* ptr3 = ptr;
				method* ptr4 = ptr2;
				for (;;)
				{
					ptr2 -= 4 / sizeof(method);
					if (ptr2 < ptr)
					{
						break;
					}
					if (*(int*)ptr2 != <Module>.EncodePointer(null))
					{
						void* ptr5 = <Module>.DecodePointer(*(int*)ptr2);
						*(int*)ptr2 = <Module>.EncodePointer(null);
						calli(System.Void(), ptr5);
						method* ptr6 = (method*)<Module>.DecodePointer((void*)<Module>.?A0x0107eea2.__onexitbegin_m);
						method* ptr7 = (method*)<Module>.DecodePointer((void*)<Module>.?A0x0107eea2.__onexitend_m);
						if (ptr3 != ptr6 || ptr4 != ptr7)
						{
							ptr3 = ptr6;
							ptr = ptr6;
							ptr4 = ptr7;
							ptr2 = ptr7;
						}
					}
				}
				IntPtr hglobal = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(hglobal);
			}
			<Module>.?A0x0107eea2.__dealloc_global_lock();
		}
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0001148C File Offset: 0x0001088C
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static int _initatexit_m()
	{
		int result = 0;
		if (<Module>.?A0x0107eea2.__alloc_global_lock() == 1)
		{
			<Module>.?A0x0107eea2.__onexitbegin_m = (method*)<Module>.EncodePointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.?A0x0107eea2.__onexitend_m = <Module>.?A0x0107eea2.__onexitbegin_m;
			<Module>.?A0x0107eea2.__exit_list_size = 32U;
			result = 1;
		}
		return result;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x000114D8 File Offset: 0x000108D8
	internal static method _onexit_m(method _Function)
	{
		return (<Module>._atexit_m(_Function) == -1) ? 0 : _Function;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x000112E8 File Offset: 0x000106E8
	[SecurityCritical]
	internal unsafe static int _atexit_m(method func)
	{
		return <Module>._atexit_helper(<Module>.EncodePointer(func), &<Module>.?A0x0107eea2.__exit_list_size, &<Module>.?A0x0107eea2.__onexitend_m, &<Module>.?A0x0107eea2.__onexitbegin_m);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x000114F8 File Offset: 0x000108F8
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static int _initatexit_app_domain()
	{
		if (<Module>.?A0x0107eea2.__alloc_global_lock() == 1)
		{
			<Module>.__onexitbegin_app_domain = (method*)<Module>.EncodePointer(Marshal.AllocHGlobal(128).ToPointer());
			<Module>.__onexitend_app_domain = <Module>.__onexitbegin_app_domain;
			<Module>.__exit_list_size_app_domain = 32U;
		}
		return 1;
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00011318 File Offset: 0x00010718
	[SecurityCritical]
	[HandleProcessCorruptedStateExceptions]
	internal unsafe static void _app_exit_callback()
	{
		if (<Module>.__exit_list_size_app_domain != 0U)
		{
			method* ptr = (method*)<Module>.DecodePointer((void*)<Module>.__onexitbegin_app_domain);
			method* ptr2 = (method*)<Module>.DecodePointer((void*)<Module>.__onexitend_app_domain);
			try
			{
				if (ptr != -1 && ptr != null && ptr2 != null)
				{
					method* ptr3 = ptr;
					method* ptr4 = ptr2;
					for (;;)
					{
						do
						{
							ptr2 -= 4 / sizeof(method);
						}
						while (ptr2 >= ptr && *(int*)ptr2 == <Module>.EncodePointer(null));
						if (ptr2 < ptr)
						{
							break;
						}
						method system.Void_u0020() = <Module>.DecodePointer(*(int*)ptr2);
						*(int*)ptr2 = <Module>.EncodePointer(null);
						calli(System.Void(), system.Void_u0020());
						method* ptr5 = (method*)<Module>.DecodePointer((void*)<Module>.__onexitbegin_app_domain);
						method* ptr6 = (method*)<Module>.DecodePointer((void*)<Module>.__onexitend_app_domain);
						if (ptr3 != ptr5 || ptr4 != ptr6)
						{
							ptr3 = ptr5;
							ptr = ptr5;
							ptr4 = ptr6;
							ptr2 = ptr6;
						}
					}
				}
			}
			finally
			{
				IntPtr hglobal = new IntPtr((void*)ptr);
				Marshal.FreeHGlobal(hglobal);
				<Module>.?A0x0107eea2.__dealloc_global_lock();
			}
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x00011544 File Offset: 0x00010944
	[SecurityCritical]
	internal static method _onexit_m_appdomain(method _Function)
	{
		return (<Module>._atexit_m_appdomain(_Function) == -1) ? 0 : _Function;
	}

	// Token: 0x06000217 RID: 535 RVA: 0x000113F8 File Offset: 0x000107F8
	[SecurityCritical]
	[DebuggerStepThrough]
	internal unsafe static int _atexit_m_appdomain(method func)
	{
		return <Module>._atexit_helper(<Module>.EncodePointer(func), &<Module>.__exit_list_size_app_domain, &<Module>.__onexitend_app_domain, &<Module>.__onexitbegin_app_domain);
	}

	// Token: 0x06000218 RID: 536
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[DllImport("KERNEL32.dll")]
	public unsafe static extern void* DecodePointer(void* Ptr);

	// Token: 0x06000219 RID: 537
	[SuppressUnmanagedCodeSecurity]
	[SecurityCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	[DllImport("KERNEL32.dll")]
	public unsafe static extern void* EncodePointer(void* Ptr);

	// Token: 0x0600021A RID: 538 RVA: 0x000115B8 File Offset: 0x000109B8
	internal unsafe static _Fac_node* {ctor}(_Fac_node* A_0, _Fac_node* _Nextarg, _Facet_base* _Facptrarg)
	{
		*A_0 = _Nextarg;
		*(A_0 + 4) = _Facptrarg;
		return A_0;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x000115D0 File Offset: 0x000109D0
	internal unsafe static void {dtor}(_Fac_node* A_0)
	{
		int num = *(A_0 + 4);
		_Facet_base* ptr = calli(std._Facet_base* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), num, *(*num + 8));
		if (ptr != null)
		{
			object obj = calli(System.Void* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,System.UInt32), ptr, 1, *(*(int*)ptr));
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00011678 File Offset: 0x00010A78
	internal unsafe static void {dtor}(_Fac_tidy_reg_t* A_0)
	{
		if (<Module>.std.?A0x7c6ddd5a._Fac_head != null)
		{
			do
			{
				_Fac_node* ptr = <Module>.std.?A0x7c6ddd5a._Fac_head;
				<Module>.std.?A0x7c6ddd5a._Fac_head = *(int*)<Module>.std.?A0x7c6ddd5a._Fac_head;
				<Module>.std._Fac_node.{dtor}(ptr);
				<Module>.delete((void*)ptr);
			}
			while (<Module>.std.?A0x7c6ddd5a._Fac_head != null);
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x000115FC File Offset: 0x000109FC
	internal unsafe static void* __delDtor(_Fac_node* A_0, uint A_0)
	{
		<Module>.std._Fac_node.{dtor}(A_0);
		if ((A_0 & 1U) != 0U)
		{
			<Module>.delete(A_0);
		}
		return A_0;
	}

	// Token: 0x0600021E RID: 542 RVA: 0x00011F20 File Offset: 0x00011320
	internal static void ??__E?A0x7c6ddd5a@_Fac_tidy_reg@std@@YMXXZ()
	{
		<Module>._atexit_m(ldftn(?A0x7c6ddd5a.??__F?A0x7c6ddd5a@_Fac_tidy_reg@std@@YMXXZ));
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00012054 File Offset: 0x00011454
	internal static void ??__F?A0x7c6ddd5a@_Fac_tidy_reg@std@@YMXXZ()
	{
		<Module>.std._Fac_tidy_reg_t.{dtor}(ref <Module>.std.?A0x7c6ddd5a._Fac_tidy_reg);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0001161C File Offset: 0x00010A1C
	internal unsafe static void _Facet_Register_m(_Facet_base* _This)
	{
		_Fac_node* ptr = <Module>.@new(8U);
		_Fac_node* ptr2;
		try
		{
			if (ptr != null)
			{
				*(int*)ptr = <Module>.std.?A0x7c6ddd5a._Fac_head;
				*(int*)(ptr + 4 / sizeof(_Fac_node)) = _This;
				ptr2 = ptr;
			}
			else
			{
				ptr2 = 0;
			}
		}
		catch
		{
			<Module>.delete((void*)ptr);
			throw;
		}
		<Module>.std.?A0x7c6ddd5a._Fac_head = ptr2;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x000116C8 File Offset: 0x00010AC8
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static int _initterm_e(method* pfbegin, method* pfend)
	{
		int num = 0;
		if (pfbegin < pfend)
		{
			while (num == 0)
			{
				uint num2 = (uint)(*(int*)pfbegin);
				if (num2 != 0U)
				{
					num = calli(System.Int32 modopt(System.Runtime.CompilerServices.CallConvCdecl)(), num2);
				}
				pfbegin += 4 / sizeof(method);
				if (pfbegin >= pfend)
				{
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x000116FC File Offset: 0x00010AFC
	[DebuggerStepThrough]
	[SecurityCritical]
	internal unsafe static void _initterm(method* pfbegin, method* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					calli(System.Void modopt(System.Runtime.CompilerServices.CallConvCdecl)(), num);
				}
				pfbegin += 4 / sizeof(method);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x00011728 File Offset: 0x00010B28
	[DebuggerStepThrough]
	internal static ModuleHandle <CrtImplementationDetails>.ThisModule.Handle()
	{
		return typeof(ThisModule).Module.ModuleHandle;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00011778 File Offset: 0x00010B78
	[DebuggerStepThrough]
	[SecurityCritical]
	[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
	internal unsafe static void _initterm_m(method* pfbegin, method* pfend)
	{
		if (pfbegin < pfend)
		{
			do
			{
				uint num = (uint)(*(int*)pfbegin);
				if (num != 0U)
				{
					object obj = calli(System.Void modopt(System.Runtime.CompilerServices.IsConst)*(), <Module>.<CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(num));
				}
				pfbegin += 4 / sizeof(method);
			}
			while (pfbegin < pfend);
		}
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0001174C File Offset: 0x00010B4C
	[DebuggerStepThrough]
	[SecurityCritical]
	internal static method <CrtImplementationDetails>.ThisModule.ResolveMethod<void\u0020const\u0020*\u0020__clrcall(void)>(method methodToken)
	{
		return <Module>.<CrtImplementationDetails>.ThisModule.Handle().ResolveMethodHandle(methodToken).GetFunctionPointer().ToPointer();
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0001192A File Offset: 0x00010D2A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern locale.id* {ctor}(locale.id*, uint);

	// Token: 0x06000227 RID: 551 RVA: 0x00011918 File Offset: 0x00010D18
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void setstate(basic_ios<char,std::char_traits<char>\u0020>*, int, [MarshalAs(UnmanagedType.U1)] bool);

	// Token: 0x06000228 RID: 552 RVA: 0x00011906 File Offset: 0x00010D06
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _Xbad_alloc();

	// Token: 0x06000229 RID: 553 RVA: 0x00011900 File Offset: 0x00010D00
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Xlength_error(sbyte*);

	// Token: 0x0600022A RID: 554 RVA: 0x00011A8C File Offset: 0x00010E8C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int MultiByteToWideChar(uint, uint, sbyte*, int, char*, int);

	// Token: 0x0600022B RID: 555 RVA: 0x00011A86 File Offset: 0x00010E86
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int WideCharToMultiByte(uint, uint, char*, int, sbyte*, int, sbyte*, int*);

	// Token: 0x0600022C RID: 556 RVA: 0x000118FA File Offset: 0x00010CFA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Xout_of_range(sbyte*);

	// Token: 0x0600022D RID: 557 RVA: 0x00011924 File Offset: 0x00010D24
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _Winerror_map(int);

	// Token: 0x0600022E RID: 558 RVA: 0x0001191E File Offset: 0x00010D1E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _Syserror_map(int);

	// Token: 0x0600022F RID: 559 RVA: 0x0001184C File Offset: 0x00010C4C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void* @new(uint);

	// Token: 0x06000230 RID: 560 RVA: 0x00011846 File Offset: 0x00010C46
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void* new[](uint);

	// Token: 0x06000231 RID: 561 RVA: 0x00011870 File Offset: 0x00010C70
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void __CxxUnregisterExceptionObject(void*, int);

	// Token: 0x06000232 RID: 562 RVA: 0x00011852 File Offset: 0x00010C52
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern int __CxxQueryExceptionSize();

	// Token: 0x06000233 RID: 563 RVA: 0x0001186A File Offset: 0x00010C6A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int __CxxDetectRethrow(void*);

	// Token: 0x06000234 RID: 564 RVA: 0x0001185E File Offset: 0x00010C5E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int __CxxRegisterExceptionObject(void*, void*);

	// Token: 0x06000235 RID: 565 RVA: 0x00011858 File Offset: 0x00010C58
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int __CxxExceptionFilter(void*, void*, int, void*);

	// Token: 0x06000236 RID: 566 RVA: 0x0000F32E File Offset: 0x0000E72E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern exception* {ctor}(exception*, exception*);

	// Token: 0x06000237 RID: 567 RVA: 0x00011864 File Offset: 0x00010C64
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _CxxThrowException(void*, _s__ThrowInfo*);

	// Token: 0x06000238 RID: 568 RVA: 0x00011840 File Offset: 0x00010C40
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void delete[](void*);

	// Token: 0x06000239 RID: 569 RVA: 0x00011912 File Offset: 0x00010D12
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(_Lockit*);

	// Token: 0x0600023A RID: 570 RVA: 0x0001190C File Offset: 0x00010D0C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern _Lockit* {ctor}(_Lockit*, int);

	// Token: 0x0600023B RID: 571 RVA: 0x00011834 File Offset: 0x00010C34
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void* memmove(void*, void*, uint);

	// Token: 0x0600023C RID: 572 RVA: 0x0001182E File Offset: 0x00010C2E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void __ExceptionPtrCopy(void*, void*);

	// Token: 0x0600023D RID: 573 RVA: 0x0000F7CE File Offset: 0x0000EBCE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void delete(void*);

	// Token: 0x0600023E RID: 574 RVA: 0x0001183A File Offset: 0x00010C3A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(exception*);

	// Token: 0x0600023F RID: 575 RVA: 0x00011A1A File Offset: 0x00010E1A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Orphan_all(_Container_base0*);

	// Token: 0x06000240 RID: 576 RVA: 0x0001197E File Offset: 0x00010D7E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern uint I(locale.id*);

	// Token: 0x06000241 RID: 577 RVA: 0x0001196C File Offset: 0x00010D6C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static extern bool always_noconv(codecvt_base*);

	// Token: 0x06000242 RID: 578 RVA: 0x000119A8 File Offset: 0x00010DA8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal unsafe static extern bool good(ios_base*);

	// Token: 0x06000243 RID: 579 RVA: 0x000119D8 File Offset: 0x00010DD8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int flags(ios_base*);

	// Token: 0x06000244 RID: 580 RVA: 0x00011936 File Offset: 0x00010D36
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int setf(ios_base*, int, int);

	// Token: 0x06000245 RID: 581 RVA: 0x000119D2 File Offset: 0x00010DD2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern long width(ios_base*);

	// Token: 0x06000246 RID: 582 RVA: 0x000119F0 File Offset: 0x00010DF0
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern long width(ios_base*, long);

	// Token: 0x06000247 RID: 583 RVA: 0x00011990 File Offset: 0x00010D90
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000248 RID: 584 RVA: 0x00011A02 File Offset: 0x00010E02
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(basic_ios<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000249 RID: 585 RVA: 0x00011972 File Offset: 0x00010D72
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_streambuf<char,std::char_traits<char>\u0020>* rdbuf(basic_ios<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600024A RID: 586 RVA: 0x00011A26 File Offset: 0x00010E26
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* {ctor}(basic_ostream<char,std::char_traits<char>\u0020>*, basic_streambuf<char,std::char_traits<char>\u0020>*, [MarshalAs(UnmanagedType.U1)] bool, int);

	// Token: 0x0600024B RID: 587 RVA: 0x00011A2C File Offset: 0x00010E2C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(basic_ostream<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600024C RID: 588 RVA: 0x00011A80 File Offset: 0x00010E80
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Add_vtordisp2(basic_ostream<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600024D RID: 589 RVA: 0x00011A38 File Offset: 0x00010E38
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* <<(basic_ostream<char,std::char_traits<char>\u0020>*, method);

	// Token: 0x0600024E RID: 590 RVA: 0x00011A4A File Offset: 0x00010E4A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* <<(basic_ostream<char,std::char_traits<char>\u0020>*, method);

	// Token: 0x0600024F RID: 591 RVA: 0x00011A44 File Offset: 0x00010E44
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* <<(basic_ostream<char,std::char_traits<char>\u0020>*, ushort);

	// Token: 0x06000250 RID: 592 RVA: 0x00011A32 File Offset: 0x00010E32
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* <<(basic_ostream<char,std::char_traits<char>\u0020>*, uint);

	// Token: 0x06000251 RID: 593 RVA: 0x00011A3E File Offset: 0x00010E3E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* write(basic_ostream<char,std::char_traits<char>\u0020>*, sbyte*, long);

	// Token: 0x06000252 RID: 594 RVA: 0x000119B4 File Offset: 0x00010DB4
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* flush(basic_ostream<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000253 RID: 595 RVA: 0x00011A7A File Offset: 0x00010E7A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Add_vtordisp1(basic_istream<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000254 RID: 596 RVA: 0x000119F6 File Offset: 0x00010DF6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void {dtor}(basic_iostream<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000255 RID: 597 RVA: 0x00011A74 File Offset: 0x00010E74
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int @in(codecvt<char,char,int>*, int*, sbyte*, sbyte*, sbyte**, sbyte*, sbyte*, sbyte**);

	// Token: 0x06000256 RID: 598 RVA: 0x00011A62 File Offset: 0x00010E62
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int @out(codecvt<char,char,int>*, int*, sbyte*, sbyte*, sbyte**, sbyte*, sbyte*, sbyte**);

	// Token: 0x06000257 RID: 599 RVA: 0x000119E4 File Offset: 0x00010DE4
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int sputc(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte);

	// Token: 0x06000258 RID: 600 RVA: 0x000119EA File Offset: 0x00010DEA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern long sputn(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte*, long);

	// Token: 0x06000259 RID: 601 RVA: 0x0001194E File Offset: 0x00010D4E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* eback(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600025A RID: 602 RVA: 0x000119CC File Offset: 0x00010DCC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* gptr(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600025B RID: 603 RVA: 0x00011A20 File Offset: 0x00010E20
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* pbase(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600025C RID: 604 RVA: 0x0001193C File Offset: 0x00010D3C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* pptr(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600025D RID: 605 RVA: 0x00011948 File Offset: 0x00010D48
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* egptr(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600025E RID: 606 RVA: 0x00011A56 File Offset: 0x00010E56
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void gbump(basic_streambuf<char,std::char_traits<char>\u0020>*, int);

	// Token: 0x0600025F RID: 607 RVA: 0x00011954 File Offset: 0x00010D54
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void setg(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte*, sbyte*, sbyte*);

	// Token: 0x06000260 RID: 608 RVA: 0x00011942 File Offset: 0x00010D42
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* epptr(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000261 RID: 609 RVA: 0x00011A68 File Offset: 0x00010E68
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _Gndec(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000262 RID: 610 RVA: 0x00011A6E File Offset: 0x00010E6E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _Gninc(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000263 RID: 611 RVA: 0x00011A5C File Offset: 0x00010E5C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void pbump(basic_streambuf<char,std::char_traits<char>\u0020>*, int);

	// Token: 0x06000264 RID: 612 RVA: 0x0001195A File Offset: 0x00010D5A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void setp(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte*, sbyte*);

	// Token: 0x06000265 RID: 613 RVA: 0x000119C6 File Offset: 0x00010DC6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void setp(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte*, sbyte*, sbyte*);

	// Token: 0x06000266 RID: 614 RVA: 0x00011A50 File Offset: 0x00010E50
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte* _Pninc(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000267 RID: 615 RVA: 0x000119FC File Offset: 0x00010DFC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void clear(basic_ios<char,std::char_traits<char>\u0020>*, int, [MarshalAs(UnmanagedType.U1)] bool);

	// Token: 0x06000268 RID: 616 RVA: 0x000119DE File Offset: 0x00010DDE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern sbyte fill(basic_ios<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000269 RID: 617 RVA: 0x00011A08 File Offset: 0x00010E08
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ios<char,std::char_traits<char>\u0020>* {ctor}(basic_ios<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600026A RID: 618 RVA: 0x00011A0E File Offset: 0x00010E0E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_iostream<char,std::char_traits<char>\u0020>* {ctor}(basic_iostream<char,std::char_traits<char>\u0020>*, basic_streambuf<char,std::char_traits<char>\u0020>*, int);

	// Token: 0x0600026B RID: 619 RVA: 0x00011A14 File Offset: 0x00010E14
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int unshift(codecvt<char,char,int>*, int*, sbyte*, sbyte*, sbyte**);

	// Token: 0x0600026C RID: 620 RVA: 0x00011996 File Offset: 0x00010D96
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_streambuf<char,std::char_traits<char>\u0020>* {ctor}(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600026D RID: 621 RVA: 0x000119A2 File Offset: 0x00010DA2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern locale* getloc(basic_streambuf<char,std::char_traits<char>\u0020>*, locale*);

	// Token: 0x0600026E RID: 622 RVA: 0x00011960 File Offset: 0x00010D60
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Init(basic_streambuf<char,std::char_traits<char>\u0020>*);

	// Token: 0x0600026F RID: 623 RVA: 0x00011966 File Offset: 0x00010D66
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Init(basic_streambuf<char,std::char_traits<char>\u0020>*, sbyte**, sbyte**, int*, sbyte**, sbyte**, int*);

	// Token: 0x06000270 RID: 624 RVA: 0x000119AE File Offset: 0x00010DAE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern basic_ostream<char,std::char_traits<char>\u0020>* tie(basic_ios<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000271 RID: 625 RVA: 0x000119C0 File Offset: 0x00010DC0
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _Osfx(basic_ostream<char,std::char_traits<char>\u0020>*);

	// Token: 0x06000272 RID: 626 RVA: 0x00011984 File Offset: 0x00010D84
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern uint _Getcat(locale.facet**, locale*);

	// Token: 0x06000273 RID: 627 RVA: 0x00011876 File Offset: 0x00010C76
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern long _time64(long*);

	// Token: 0x06000274 RID: 628 RVA: 0x00011888 File Offset: 0x00010C88
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fclose(_iobuf*);

	// Token: 0x06000275 RID: 629 RVA: 0x00011882 File Offset: 0x00010C82
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern uint fwrite(void*, uint, uint, _iobuf*);

	// Token: 0x06000276 RID: 630 RVA: 0x0000F0B0 File Offset: 0x0000E4B0
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void sha256_final(NC_SHA256_CTX*, uint*);

	// Token: 0x06000277 RID: 631 RVA: 0x000118D0 File Offset: 0x00010CD0
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int memcpy_s(void*, uint, void*, uint);

	// Token: 0x06000278 RID: 632 RVA: 0x0000F260 File Offset: 0x0000E660
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void sha256_update(NC_SHA256_CTX*, sbyte*, uint);

	// Token: 0x06000279 RID: 633 RVA: 0x0001189A File Offset: 0x00010C9A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern uint fread(void*, uint, uint, _iobuf*);

	// Token: 0x0600027A RID: 634 RVA: 0x0000F200 File Offset: 0x0000E600
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void sha256_init(NC_SHA256_CTX*);

	// Token: 0x0600027B RID: 635 RVA: 0x000118B2 File Offset: 0x00010CB2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _lock_file(_iobuf*);

	// Token: 0x0600027C RID: 636 RVA: 0x000118E2 File Offset: 0x00010CE2
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int setvbuf(_iobuf*, sbyte*, int, uint);

	// Token: 0x0600027D RID: 637 RVA: 0x000118A0 File Offset: 0x00010CA0
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern _iobuf* fopen(sbyte*, sbyte*);

	// Token: 0x0600027E RID: 638 RVA: 0x000118DC File Offset: 0x00010CDC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fsetpos(_iobuf*, long*);

	// Token: 0x0600027F RID: 639 RVA: 0x0001188E File Offset: 0x00010C8E
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern int tolower(int);

	// Token: 0x06000280 RID: 640 RVA: 0x000118CA File Offset: 0x00010CCA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fgetc(_iobuf*);

	// Token: 0x06000281 RID: 641 RVA: 0x000118E8 File Offset: 0x00010CE8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fflush(_iobuf*);

	// Token: 0x06000282 RID: 642 RVA: 0x00011894 File Offset: 0x00010C94
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int _fseeki64(_iobuf*, long, int);

	// Token: 0x06000283 RID: 643 RVA: 0x000118D6 File Offset: 0x00010CD6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fgetpos(_iobuf*, long*);

	// Token: 0x06000284 RID: 644 RVA: 0x000118C4 File Offset: 0x00010CC4
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int ungetc(int, _iobuf*);

	// Token: 0x06000285 RID: 645 RVA: 0x000118B8 File Offset: 0x00010CB8
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern void _unlock_file(_iobuf*);

	// Token: 0x06000286 RID: 646 RVA: 0x000118AC File Offset: 0x00010CAC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int sprintf(sbyte*, sbyte*, __arglist);

	// Token: 0x06000287 RID: 647 RVA: 0x0001199C File Offset: 0x00010D9C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern _iobuf* _Fiopen(sbyte*, int, int);

	// Token: 0x06000288 RID: 648 RVA: 0x00011930 File Offset: 0x00010D30
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern locale._Locimp* _Getgloballocale();

	// Token: 0x06000289 RID: 649 RVA: 0x000119BA File Offset: 0x00010DBA
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	[return: MarshalAs(UnmanagedType.U1)]
	internal static extern bool uncaught_exception();

	// Token: 0x0600028A RID: 650 RVA: 0x00011978 File Offset: 0x00010D78
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _Lockit_ctor(int);

	// Token: 0x0600028B RID: 651 RVA: 0x0001198A File Offset: 0x00010D8A
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _Lockit_dtor(int);

	// Token: 0x0600028C RID: 652 RVA: 0x0001187C File Offset: 0x00010C7C
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.ThisCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern bad_cast* {ctor}(bad_cast*, sbyte*);

	// Token: 0x0600028D RID: 653 RVA: 0x000118BE File Offset: 0x00010CBE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int fputc(int, _iobuf*);

	// Token: 0x0600028E RID: 654 RVA: 0x000118A6 File Offset: 0x00010CA6
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern long _ftelli64(_iobuf*);

	// Token: 0x0600028F RID: 655 RVA: 0x000104B1 File Offset: 0x0000F8B1
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal unsafe static extern void* _getFiberPtrId();

	// Token: 0x06000290 RID: 656 RVA: 0x0000FB04 File Offset: 0x0000EF04
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _amsg_exit(int);

	// Token: 0x06000291 RID: 657 RVA: 0x0000FD4E File Offset: 0x0000F14E
	[SuppressUnmanagedCodeSecurity]
	[MethodImpl(MethodImplOptions.Unmanaged | MethodImplOptions.PreserveSig)]
	internal static extern void __security_init_cookie();

	// Token: 0x06000292 RID: 658 RVA: 0x00011A92 File Offset: 0x00010E92
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void Sleep(uint);

	// Token: 0x06000293 RID: 659 RVA: 0x000118EE File Offset: 0x00010CEE
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void _cexit();

	// Token: 0x06000294 RID: 660 RVA: 0x000118F4 File Offset: 0x00010CF4
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal unsafe static extern int __FrameUnwindFilter(_EXCEPTION_POINTERS*);

	// Token: 0x06000295 RID: 661 RVA: 0x0000FEEC File Offset: 0x0000F2EC
	[SuppressUnmanagedCodeSecurity]
	[DllImport("", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
	[MethodImpl(MethodImplOptions.Unmanaged)]
	internal static extern void terminate();

	// Token: 0x04000001 RID: 1 RVA: 0x0001359C File Offset: 0x00011B9C
	internal static $ArrayType$$$BY07$$CBD ??_C@_07DCLBNMLN@generic?$AA@;

	// Token: 0x04000002 RID: 2 RVA: 0x000135A4 File Offset: 0x00011BA4
	internal static $ArrayType$$$BY0O@$$CBD ??_C@_0O@BFJCFAAK@unknown?5error?$AA@;

	// Token: 0x04000003 RID: 3 RVA: 0x000135B4 File Offset: 0x00011BB4
	internal static $ArrayType$$$BY08$$CBD ??_C@_08LLGCOLLL@iostream?$AA@;

	// Token: 0x04000004 RID: 4 RVA: 0x000135C0 File Offset: 0x00011BC0
	internal static $ArrayType$$$BY0BG@$$CBD ??_C@_0BG@PADBLCHM@iostream?5stream?5error?$AA@;

	// Token: 0x04000005 RID: 5 RVA: 0x000135D8 File Offset: 0x00011BD8
	internal static $ArrayType$$$BY06$$CBD ??_C@_06FHFOAHML@system?$AA@;

	// Token: 0x04000006 RID: 6 RVA: 0x00013574 File Offset: 0x00011B74
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@CFPLBAOH@invalid?5string?5position?$AA@;

	// Token: 0x04000007 RID: 7 RVA: 0x0001358C File Offset: 0x00011B8C
	internal static $ArrayType$$$BY0BA@$$CBD ??_C@_0BA@JFNIOLAK@string?5too?5long?$AA@;

	// Token: 0x04000008 RID: 8 RVA: 0x00039D68 File Offset: 0x00038368
	internal static $_s__RTTIBaseClassArray$_extraBytes_16 ??_R2failure@ios_base@std@@8;

	// Token: 0x04000009 RID: 9 RVA: 0x00039CC8 File Offset: 0x000382C8
	internal static $_s__RTTIBaseClassArray$_extraBytes_12 ??_R2_System_error_category@std@@8;

	// Token: 0x0400000A RID: 10 RVA: 0x00039C78 File Offset: 0x00038278
	internal static $_s__RTTIBaseClassArray$_extraBytes_12 ??_R2_Iostream_error_category@std@@8;

	// Token: 0x0400000B RID: 11 RVA: 0x00039C2C File Offset: 0x0003822C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2_Generic_error_category@std@@8;

	// Token: 0x0400000C RID: 12 RVA: 0x00039D18 File Offset: 0x00038318
	internal static $_s__RTTIBaseClassArray$_extraBytes_12 ??_R2system_error@std@@8;

	// Token: 0x0400000D RID: 13 RVA: 0x00039BE4 File Offset: 0x000381E4
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2error_category@std@@8;

	// Token: 0x0400000E RID: 14 RVA: 0x00039B98 File Offset: 0x00038198
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2runtime_error@std@@8;

	// Token: 0x0400000F RID: 15 RVA: 0x00039B64 File Offset: 0x00038164
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2exception@std@@8;

	// Token: 0x04000010 RID: 16 RVA: 0x00039D4C File Offset: 0x0003834C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@failure@ios_base@std@@8;

	// Token: 0x04000011 RID: 17 RVA: 0x00039CAC File Offset: 0x000382AC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@_System_error_category@std@@8;

	// Token: 0x04000012 RID: 18 RVA: 0x00039C5C File Offset: 0x0003825C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@_Iostream_error_category@std@@8;

	// Token: 0x04000013 RID: 19 RVA: 0x00039C10 File Offset: 0x00038210
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@_Generic_error_category@std@@8;

	// Token: 0x04000014 RID: 20 RVA: 0x00039CFC File Offset: 0x000382FC
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@system_error@std@@8;

	// Token: 0x04000015 RID: 21 RVA: 0x00039BC8 File Offset: 0x000381C8
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@error_category@std@@8;

	// Token: 0x04000016 RID: 22 RVA: 0x00039B48 File Offset: 0x00038148
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@runtime_error@std@@8;

	// Token: 0x04000017 RID: 23 RVA: 0x00039B7C File Offset: 0x0003817C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@exception@std@@8;

	// Token: 0x04000018 RID: 24 RVA: 0x00039D7C File Offset: 0x0003837C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3failure@ios_base@std@@8;

	// Token: 0x04000019 RID: 25 RVA: 0x00039CD8 File Offset: 0x000382D8
	internal static _s__RTTIClassHierarchyDescriptor ??_R3_System_error_category@std@@8;

	// Token: 0x0400001A RID: 26 RVA: 0x0003C128 File Offset: 0x0003A528
	internal static $_TypeDescriptor$_extraBytes_33 ??_R0?AV_System_error_category@std@@@8;

	// Token: 0x0400001B RID: 27 RVA: 0x00039C88 File Offset: 0x00038288
	internal static _s__RTTIClassHierarchyDescriptor ??_R3_Iostream_error_category@std@@8;

	// Token: 0x0400001C RID: 28 RVA: 0x0003C0E0 File Offset: 0x0003A4E0
	internal static $_TypeDescriptor$_extraBytes_35 ??_R0?AV_Iostream_error_category@std@@@8;

	// Token: 0x0400001D RID: 29 RVA: 0x00039C38 File Offset: 0x00038238
	internal static _s__RTTIClassHierarchyDescriptor ??_R3_Generic_error_category@std@@8;

	// Token: 0x0400001E RID: 30 RVA: 0x0003C098 File Offset: 0x0003A498
	internal static $_TypeDescriptor$_extraBytes_34 ??_R0?AV_Generic_error_category@std@@@8;

	// Token: 0x0400001F RID: 31 RVA: 0x00039D28 File Offset: 0x00038328
	internal static _s__RTTIClassHierarchyDescriptor ??_R3system_error@std@@8;

	// Token: 0x04000020 RID: 32 RVA: 0x00039BEC File Offset: 0x000381EC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3error_category@std@@8;

	// Token: 0x04000021 RID: 33 RVA: 0x0003C058 File Offset: 0x0003A458
	internal static $_TypeDescriptor$_extraBytes_25 ??_R0?AVerror_category@std@@@8;

	// Token: 0x04000022 RID: 34 RVA: 0x00039BA4 File Offset: 0x000381A4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3runtime_error@std@@8;

	// Token: 0x04000023 RID: 35 RVA: 0x00039B6C File Offset: 0x0003816C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3exception@std@@8;

	// Token: 0x04000024 RID: 36 RVA: 0x00039D8C File Offset: 0x0003838C
	internal static _s__RTTICompleteObjectLocator ??_R4failure@ios_base@std@@6B@;

	// Token: 0x04000025 RID: 37 RVA: 0x00039CE8 File Offset: 0x000382E8
	internal static _s__RTTICompleteObjectLocator ??_R4_System_error_category@std@@6B@;

	// Token: 0x04000026 RID: 38 RVA: 0x00039C98 File Offset: 0x00038298
	internal static _s__RTTICompleteObjectLocator ??_R4_Iostream_error_category@std@@6B@;

	// Token: 0x04000027 RID: 39 RVA: 0x00039C48 File Offset: 0x00038248
	internal static _s__RTTICompleteObjectLocator ??_R4_Generic_error_category@std@@6B@;

	// Token: 0x04000028 RID: 40 RVA: 0x00039D38 File Offset: 0x00038338
	internal static _s__RTTICompleteObjectLocator ??_R4system_error@std@@6B@;

	// Token: 0x04000029 RID: 41 RVA: 0x00039BFC File Offset: 0x000381FC
	internal static _s__RTTICompleteObjectLocator ??_R4error_category@std@@6B@;

	// Token: 0x0400002A RID: 42 RVA: 0x00039BB4 File Offset: 0x000381B4
	internal static _s__RTTICompleteObjectLocator ??_R4runtime_error@std@@6B@;

	// Token: 0x0400002B RID: 43 RVA: 0x0003CFE8 File Offset: 0x0003B3E8
	internal static locale.id ?id@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2V0locale@2@A;

	// Token: 0x0400002C RID: 44 RVA: 0x000132C4 File Offset: 0x000118C4
	internal static method ?id$initializer$@?$num_put@_WV?$back_insert_iterator@V?$basic_string@_WU?$char_traits@_W@std@@V?$allocator@_W@2@@std@@@std@@@std@@2P6MXXZA;

	// Token: 0x0400002D RID: 45 RVA: 0x0003CFE4 File Offset: 0x0003B3E4
	internal static locale.id ?id@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2V0locale@2@A;

	// Token: 0x0400002E RID: 46 RVA: 0x000132C0 File Offset: 0x000118C0
	internal static method ?id$initializer$@?$num_put@DV?$back_insert_iterator@V?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@std@@@std@@2P6MXXZA;

	// Token: 0x0400002F RID: 47 RVA: 0x0003C3C8 File Offset: 0x0003A7C8
	internal static _System_error_category ?_System_object@?$_Error_objects@H@std@@2V_System_error_category@2@A;

	// Token: 0x04000030 RID: 48 RVA: 0x000132BC File Offset: 0x000118BC
	internal static method ?_System_object$initializer$@?$_Error_objects@H@std@@2P6MXXZA;

	// Token: 0x04000031 RID: 49 RVA: 0x0003C3C4 File Offset: 0x0003A7C4
	internal static _Iostream_error_category ?_Iostream_object@?$_Error_objects@H@std@@2V_Iostream_error_category@2@A;

	// Token: 0x04000032 RID: 50 RVA: 0x000132B8 File Offset: 0x000118B8
	internal static method ?_Iostream_object$initializer$@?$_Error_objects@H@std@@2P6MXXZA;

	// Token: 0x04000033 RID: 51 RVA: 0x0003C3C0 File Offset: 0x0003A7C0
	internal static _Generic_error_category ?_Generic_object@?$_Error_objects@H@std@@2V_Generic_error_category@2@A;

	// Token: 0x04000034 RID: 52 RVA: 0x000132B4 File Offset: 0x000118B4
	internal static method ?_Generic_object$initializer$@?$_Error_objects@H@std@@2P6MXXZA;

	// Token: 0x04000035 RID: 53 RVA: 0x0003C170 File Offset: 0x0003A570
	internal static $_TypeDescriptor$_extraBytes_23 ??_R0?AVsystem_error@std@@@8;

	// Token: 0x04000036 RID: 54 RVA: 0x0003C1A8 File Offset: 0x0003A5A8
	internal static $_TypeDescriptor$_extraBytes_27 ??_R0?AVfailure@ios_base@std@@@8;

	// Token: 0x04000037 RID: 55 RVA: 0x0003C1D0 File Offset: 0x0003A5D0
	internal static $ArrayType$$$BY02Q6AXXZ ??_7failure@ios_base@std@@6B@;

	// Token: 0x04000038 RID: 56 RVA: 0x0001343C File Offset: 0x00011A3C
	internal static io_errc ?_Stream_err@failure@ios_base@std@@0W4io_errc@43@B;

	// Token: 0x04000039 RID: 57 RVA: 0x0001342C File Offset: 0x00011A2C
	internal static _Iosb<int>._Seekdir ?end@?$_Iosb@H@std@@2W4_Seekdir@12@B;

	// Token: 0x0400003A RID: 58 RVA: 0x00013428 File Offset: 0x00011A28
	internal static _Iosb<int>._Seekdir ?cur@?$_Iosb@H@std@@2W4_Seekdir@12@B;

	// Token: 0x0400003B RID: 59 RVA: 0x00013424 File Offset: 0x00011A24
	internal static _Iosb<int>._Seekdir ?beg@?$_Iosb@H@std@@2W4_Seekdir@12@B;

	// Token: 0x0400003C RID: 60 RVA: 0x00013420 File Offset: 0x00011A20
	internal static _Iosb<int>._Openmode ?binary@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x0400003D RID: 61 RVA: 0x0001341C File Offset: 0x00011A1C
	internal static _Iosb<int>._Openmode ?_Noreplace@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x0400003E RID: 62 RVA: 0x00013418 File Offset: 0x00011A18
	internal static _Iosb<int>._Openmode ?_Nocreate@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x0400003F RID: 63 RVA: 0x00013410 File Offset: 0x00011A10
	internal static _Iosb<int>._Openmode ?trunc@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x04000040 RID: 64 RVA: 0x0001340C File Offset: 0x00011A0C
	internal static _Iosb<int>._Openmode ?app@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x04000041 RID: 65 RVA: 0x00013404 File Offset: 0x00011A04
	internal static _Iosb<int>._Openmode ?ate@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x04000042 RID: 66 RVA: 0x000133FC File Offset: 0x000119FC
	internal static _Iosb<int>._Openmode ?out@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x04000043 RID: 67 RVA: 0x000133F8 File Offset: 0x000119F8
	internal static _Iosb<int>._Openmode ?in@?$_Iosb@H@std@@2W4_Openmode@12@B;

	// Token: 0x04000044 RID: 68 RVA: 0x000133F0 File Offset: 0x000119F0
	internal static _Iosb<int>._Iostate ?_Hardfail@?$_Iosb@H@std@@2W4_Iostate@12@B;

	// Token: 0x04000045 RID: 69 RVA: 0x000133EC File Offset: 0x000119EC
	internal static _Iosb<int>._Iostate ?badbit@?$_Iosb@H@std@@2W4_Iostate@12@B;

	// Token: 0x04000046 RID: 70 RVA: 0x000133E8 File Offset: 0x000119E8
	internal static _Iosb<int>._Iostate ?failbit@?$_Iosb@H@std@@2W4_Iostate@12@B;

	// Token: 0x04000047 RID: 71 RVA: 0x000133E4 File Offset: 0x000119E4
	internal static _Iosb<int>._Iostate ?eofbit@?$_Iosb@H@std@@2W4_Iostate@12@B;

	// Token: 0x04000048 RID: 72 RVA: 0x000133E0 File Offset: 0x000119E0
	internal static _Iosb<int>._Iostate ?goodbit@?$_Iosb@H@std@@2W4_Iostate@12@B;

	// Token: 0x04000049 RID: 73 RVA: 0x000133DC File Offset: 0x000119DC
	internal static _Iosb<int>._Fmtflags ?floatfield@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004A RID: 74 RVA: 0x000133D8 File Offset: 0x000119D8
	internal static _Iosb<int>._Fmtflags ?basefield@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004B RID: 75 RVA: 0x000133D0 File Offset: 0x000119D0
	internal static _Iosb<int>._Fmtflags ?adjustfield@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004C RID: 76 RVA: 0x000133C8 File Offset: 0x000119C8
	internal static _Iosb<int>._Fmtflags ?_Stdio@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004D RID: 77 RVA: 0x000133C0 File Offset: 0x000119C0
	internal static _Iosb<int>._Fmtflags ?boolalpha@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004E RID: 78 RVA: 0x000133BC File Offset: 0x000119BC
	internal static _Iosb<int>._Fmtflags ?hexfloat@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400004F RID: 79 RVA: 0x000133B8 File Offset: 0x000119B8
	internal static _Iosb<int>._Fmtflags ?fixed@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000050 RID: 80 RVA: 0x000133B4 File Offset: 0x000119B4
	internal static _Iosb<int>._Fmtflags ?scientific@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000051 RID: 81 RVA: 0x000133B0 File Offset: 0x000119B0
	internal static _Iosb<int>._Fmtflags ?hex@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000052 RID: 82 RVA: 0x000133AC File Offset: 0x000119AC
	internal static _Iosb<int>._Fmtflags ?oct@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000053 RID: 83 RVA: 0x000133A8 File Offset: 0x000119A8
	internal static _Iosb<int>._Fmtflags ?dec@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000054 RID: 84 RVA: 0x000133A4 File Offset: 0x000119A4
	internal static _Iosb<int>._Fmtflags ?internal@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000055 RID: 85 RVA: 0x000133A0 File Offset: 0x000119A0
	internal static _Iosb<int>._Fmtflags ?right@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000056 RID: 86 RVA: 0x0001339C File Offset: 0x0001199C
	internal static _Iosb<int>._Fmtflags ?left@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000057 RID: 87 RVA: 0x00013394 File Offset: 0x00011994
	internal static _Iosb<int>._Fmtflags ?showpos@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000058 RID: 88 RVA: 0x00013390 File Offset: 0x00011990
	internal static _Iosb<int>._Fmtflags ?showpoint@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x04000059 RID: 89 RVA: 0x00013388 File Offset: 0x00011988
	internal static _Iosb<int>._Fmtflags ?showbase@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400005A RID: 90 RVA: 0x00013380 File Offset: 0x00011980
	internal static _Iosb<int>._Fmtflags ?uppercase@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400005B RID: 91 RVA: 0x0001337C File Offset: 0x0001197C
	internal static _Iosb<int>._Fmtflags ?unitbuf@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400005C RID: 92 RVA: 0x00013378 File Offset: 0x00011978
	internal static _Iosb<int>._Fmtflags ?skipws@?$_Iosb@H@std@@2W4_Fmtflags@12@B;

	// Token: 0x0400005D RID: 93 RVA: 0x0003C158 File Offset: 0x0003A558
	internal static $ArrayType$$$BY06Q6AXXZ ??_7_System_error_category@std@@6B@;

	// Token: 0x0400005E RID: 94 RVA: 0x0003C110 File Offset: 0x0003A510
	internal static $ArrayType$$$BY06Q6AXXZ ??_7_Iostream_error_category@std@@6B@;

	// Token: 0x0400005F RID: 95 RVA: 0x0003C0C8 File Offset: 0x0003A4C8
	internal static $ArrayType$$$BY06Q6AXXZ ??_7_Generic_error_category@std@@6B@;

	// Token: 0x04000060 RID: 96 RVA: 0x0003C194 File Offset: 0x0003A594
	internal static $ArrayType$$$BY02Q6AXXZ ??_7system_error@std@@6B@;

	// Token: 0x04000061 RID: 97 RVA: 0x0003C080 File Offset: 0x0003A480
	internal static $ArrayType$$$BY06Q6AXXZ ??_7error_category@std@@6B@;

	// Token: 0x04000062 RID: 98 RVA: 0x00013548 File Offset: 0x00011B48
	internal static int ?none@?$_Locbase@H@std@@2HB;

	// Token: 0x04000063 RID: 99 RVA: 0x00013544 File Offset: 0x00011B44
	internal static int ?all@?$_Locbase@H@std@@2HB;

	// Token: 0x04000064 RID: 100 RVA: 0x0001353C File Offset: 0x00011B3C
	internal static int ?messages@?$_Locbase@H@std@@2HB;

	// Token: 0x04000065 RID: 101 RVA: 0x00013534 File Offset: 0x00011B34
	internal static int ?time@?$_Locbase@H@std@@2HB;

	// Token: 0x04000066 RID: 102 RVA: 0x00013530 File Offset: 0x00011B30
	internal static int ?numeric@?$_Locbase@H@std@@2HB;

	// Token: 0x04000067 RID: 103 RVA: 0x00013528 File Offset: 0x00011B28
	internal static int ?monetary@?$_Locbase@H@std@@2HB;

	// Token: 0x04000068 RID: 104 RVA: 0x00013524 File Offset: 0x00011B24
	internal static int ?ctype@?$_Locbase@H@std@@2HB;

	// Token: 0x04000069 RID: 105 RVA: 0x00013520 File Offset: 0x00011B20
	internal static int ?collate@?$_Locbase@H@std@@2HB;

	// Token: 0x0400006A RID: 106 RVA: 0x0003C028 File Offset: 0x0003A428
	internal static $_TypeDescriptor$_extraBytes_20 ??_R0?AVexception@std@@@8;

	// Token: 0x0400006B RID: 107 RVA: 0x0003C008 File Offset: 0x0003A408
	internal static $_TypeDescriptor$_extraBytes_24 ??_R0?AVruntime_error@std@@@8;

	// Token: 0x0400006C RID: 108 RVA: 0x0003C048 File Offset: 0x0003A448
	internal static $ArrayType$$$BY02Q6AXXZ ??_7runtime_error@std@@6B@;

	// Token: 0x0400006D RID: 109 RVA: 0x0003CFE1 File Offset: 0x0003B3E1
	internal static allocator_arg_t allocator_arg;

	// Token: 0x0400006E RID: 110 RVA: 0x0003CFE0 File Offset: 0x0003B3E0
	internal static piecewise_construct_t piecewise_construct;

	// Token: 0x0400006F RID: 111 RVA: 0x000133F4 File Offset: 0x000119F4
	internal static uint ?value@?$integral_constant@I$0A@@std@@2IB;

	// Token: 0x04000070 RID: 112 RVA: 0x0001354C File Offset: 0x00011B4C
	internal static uint ?value@?$_Sizeof@HU_Nil@std@@U12@U12@U12@U12@U12@U12@@std@@2IB;

	// Token: 0x04000071 RID: 113 RVA: 0x000134B8 File Offset: 0x00011AB8
	internal static int ?min_exponent10@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000072 RID: 114 RVA: 0x000134B4 File Offset: 0x00011AB4
	internal static int ?min_exponent@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000073 RID: 115 RVA: 0x000134B0 File Offset: 0x00011AB0
	internal static int ?max_exponent10@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000074 RID: 116 RVA: 0x000134AC File Offset: 0x00011AAC
	internal static int ?max_exponent@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000075 RID: 117 RVA: 0x000134A8 File Offset: 0x00011AA8
	internal static int ?max_digits10@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000076 RID: 118 RVA: 0x000134A4 File Offset: 0x00011AA4
	internal static int ?digits10@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000077 RID: 119 RVA: 0x000134A0 File Offset: 0x00011AA0
	internal static int ?digits@?$numeric_limits@O@std@@2HB;

	// Token: 0x04000078 RID: 120 RVA: 0x0001349C File Offset: 0x00011A9C
	internal static int ?min_exponent10@?$numeric_limits@N@std@@2HB;

	// Token: 0x04000079 RID: 121 RVA: 0x00013498 File Offset: 0x00011A98
	internal static int ?min_exponent@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007A RID: 122 RVA: 0x00013494 File Offset: 0x00011A94
	internal static int ?max_exponent10@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007B RID: 123 RVA: 0x00013490 File Offset: 0x00011A90
	internal static int ?max_exponent@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007C RID: 124 RVA: 0x0001348C File Offset: 0x00011A8C
	internal static int ?max_digits10@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007D RID: 125 RVA: 0x00013488 File Offset: 0x00011A88
	internal static int ?digits10@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007E RID: 126 RVA: 0x00013484 File Offset: 0x00011A84
	internal static int ?digits@?$numeric_limits@N@std@@2HB;

	// Token: 0x0400007F RID: 127 RVA: 0x0001347C File Offset: 0x00011A7C
	internal static int ?min_exponent10@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000080 RID: 128 RVA: 0x00013478 File Offset: 0x00011A78
	internal static int ?min_exponent@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000081 RID: 129 RVA: 0x00013474 File Offset: 0x00011A74
	internal static int ?max_exponent10@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000082 RID: 130 RVA: 0x00013470 File Offset: 0x00011A70
	internal static int ?max_exponent@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000083 RID: 131 RVA: 0x0001346C File Offset: 0x00011A6C
	internal static int ?max_digits10@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000084 RID: 132 RVA: 0x00013468 File Offset: 0x00011A68
	internal static int ?digits10@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000085 RID: 133 RVA: 0x00013464 File Offset: 0x00011A64
	internal static int ?digits@?$numeric_limits@M@std@@2HB;

	// Token: 0x04000086 RID: 134 RVA: 0x00013460 File Offset: 0x00011A60
	internal static int ?digits10@?$numeric_limits@_K@std@@2HB;

	// Token: 0x04000087 RID: 135 RVA: 0x0001345C File Offset: 0x00011A5C
	internal static int ?digits@?$numeric_limits@_K@std@@2HB;

	// Token: 0x04000088 RID: 136 RVA: 0x00013458 File Offset: 0x00011A58
	internal static bool ?is_signed@?$numeric_limits@_K@std@@2_NB;

	// Token: 0x04000089 RID: 137 RVA: 0x00013454 File Offset: 0x00011A54
	internal static int ?digits10@?$numeric_limits@_J@std@@2HB;

	// Token: 0x0400008A RID: 138 RVA: 0x00013450 File Offset: 0x00011A50
	internal static int ?digits@?$numeric_limits@_J@std@@2HB;

	// Token: 0x0400008B RID: 139 RVA: 0x0001344C File Offset: 0x00011A4C
	internal static bool ?is_signed@?$numeric_limits@_J@std@@2_NB;

	// Token: 0x0400008C RID: 140 RVA: 0x00013448 File Offset: 0x00011A48
	internal static int ?digits10@?$numeric_limits@K@std@@2HB;

	// Token: 0x0400008D RID: 141 RVA: 0x00013444 File Offset: 0x00011A44
	internal static int ?digits@?$numeric_limits@K@std@@2HB;

	// Token: 0x0400008E RID: 142 RVA: 0x00013440 File Offset: 0x00011A40
	internal static bool ?is_signed@?$numeric_limits@K@std@@2_NB;

	// Token: 0x0400008F RID: 143 RVA: 0x00013438 File Offset: 0x00011A38
	internal static int ?digits10@?$numeric_limits@J@std@@2HB;

	// Token: 0x04000090 RID: 144 RVA: 0x00013434 File Offset: 0x00011A34
	internal static int ?digits@?$numeric_limits@J@std@@2HB;

	// Token: 0x04000091 RID: 145 RVA: 0x00013430 File Offset: 0x00011A30
	internal static bool ?is_signed@?$numeric_limits@J@std@@2_NB;

	// Token: 0x04000092 RID: 146 RVA: 0x00013414 File Offset: 0x00011A14
	internal static int ?digits10@?$numeric_limits@I@std@@2HB;

	// Token: 0x04000093 RID: 147 RVA: 0x00013408 File Offset: 0x00011A08
	internal static int ?digits@?$numeric_limits@I@std@@2HB;

	// Token: 0x04000094 RID: 148 RVA: 0x00013400 File Offset: 0x00011A00
	internal static bool ?is_signed@?$numeric_limits@I@std@@2_NB;

	// Token: 0x04000095 RID: 149 RVA: 0x000133D4 File Offset: 0x000119D4
	internal static int ?digits10@?$numeric_limits@H@std@@2HB;

	// Token: 0x04000096 RID: 150 RVA: 0x000133CC File Offset: 0x000119CC
	internal static int ?digits@?$numeric_limits@H@std@@2HB;

	// Token: 0x04000097 RID: 151 RVA: 0x000133C4 File Offset: 0x000119C4
	internal static bool ?is_signed@?$numeric_limits@H@std@@2_NB;

	// Token: 0x04000098 RID: 152 RVA: 0x00013398 File Offset: 0x00011998
	internal static int ?digits10@?$numeric_limits@G@std@@2HB;

	// Token: 0x04000099 RID: 153 RVA: 0x0001338C File Offset: 0x0001198C
	internal static int ?digits@?$numeric_limits@G@std@@2HB;

	// Token: 0x0400009A RID: 154 RVA: 0x00013384 File Offset: 0x00011984
	internal static bool ?is_signed@?$numeric_limits@G@std@@2_NB;

	// Token: 0x0400009B RID: 155 RVA: 0x00013374 File Offset: 0x00011974
	internal static int ?digits10@?$numeric_limits@F@std@@2HB;

	// Token: 0x0400009C RID: 156 RVA: 0x00013370 File Offset: 0x00011970
	internal static int ?digits@?$numeric_limits@F@std@@2HB;

	// Token: 0x0400009D RID: 157 RVA: 0x0001336C File Offset: 0x0001196C
	internal static bool ?is_signed@?$numeric_limits@F@std@@2_NB;

	// Token: 0x0400009E RID: 158 RVA: 0x00013368 File Offset: 0x00011968
	internal static int ?digits10@?$numeric_limits@E@std@@2HB;

	// Token: 0x0400009F RID: 159 RVA: 0x00013364 File Offset: 0x00011964
	internal static int ?digits@?$numeric_limits@E@std@@2HB;

	// Token: 0x040000A0 RID: 160 RVA: 0x00013360 File Offset: 0x00011960
	internal static bool ?is_signed@?$numeric_limits@E@std@@2_NB;

	// Token: 0x040000A1 RID: 161 RVA: 0x00013570 File Offset: 0x00011B70
	internal static int ?digits10@?$numeric_limits@C@std@@2HB;

	// Token: 0x040000A2 RID: 162 RVA: 0x0001356C File Offset: 0x00011B6C
	internal static int ?digits@?$numeric_limits@C@std@@2HB;

	// Token: 0x040000A3 RID: 163 RVA: 0x00013568 File Offset: 0x00011B68
	internal static bool ?is_signed@?$numeric_limits@C@std@@2_NB;

	// Token: 0x040000A4 RID: 164 RVA: 0x00013564 File Offset: 0x00011B64
	internal static int ?digits10@?$numeric_limits@_N@std@@2HB;

	// Token: 0x040000A5 RID: 165 RVA: 0x00013560 File Offset: 0x00011B60
	internal static int ?digits@?$numeric_limits@_N@std@@2HB;

	// Token: 0x040000A6 RID: 166 RVA: 0x0001355D File Offset: 0x00011B5D
	internal static bool ?is_signed@?$numeric_limits@_N@std@@2_NB;

	// Token: 0x040000A7 RID: 167 RVA: 0x0001355C File Offset: 0x00011B5C
	internal static bool ?is_modulo@?$numeric_limits@_N@std@@2_NB;

	// Token: 0x040000A8 RID: 168 RVA: 0x00013558 File Offset: 0x00011B58
	internal static int ?digits10@?$numeric_limits@_W@std@@2HB;

	// Token: 0x040000A9 RID: 169 RVA: 0x00013554 File Offset: 0x00011B54
	internal static int ?digits@?$numeric_limits@_W@std@@2HB;

	// Token: 0x040000AA RID: 170 RVA: 0x00013550 File Offset: 0x00011B50
	internal static bool ?is_signed@?$numeric_limits@_W@std@@2_NB;

	// Token: 0x040000AB RID: 171 RVA: 0x00013540 File Offset: 0x00011B40
	internal static int ?digits10@?$numeric_limits@D@std@@2HB;

	// Token: 0x040000AC RID: 172 RVA: 0x00013538 File Offset: 0x00011B38
	internal static int ?digits@?$numeric_limits@D@std@@2HB;

	// Token: 0x040000AD RID: 173 RVA: 0x0001352C File Offset: 0x00011B2C
	internal static bool ?is_signed@?$numeric_limits@D@std@@2_NB;

	// Token: 0x040000AE RID: 174 RVA: 0x0001351C File Offset: 0x00011B1C
	internal static int ?radix@_Num_float_base@std@@2HB;

	// Token: 0x040000AF RID: 175 RVA: 0x00013518 File Offset: 0x00011B18
	internal static float_round_style ?round_style@_Num_float_base@std@@2W4float_round_style@2@B;

	// Token: 0x040000B0 RID: 176 RVA: 0x00013514 File Offset: 0x00011B14
	internal static bool ?traps@_Num_float_base@std@@2_NB;

	// Token: 0x040000B1 RID: 177 RVA: 0x00013513 File Offset: 0x00011B13
	internal static bool ?tinyness_before@_Num_float_base@std@@2_NB;

	// Token: 0x040000B2 RID: 178 RVA: 0x00013512 File Offset: 0x00011B12
	internal static bool ?is_specialized@_Num_float_base@std@@2_NB;

	// Token: 0x040000B3 RID: 179 RVA: 0x00013511 File Offset: 0x00011B11
	internal static bool ?is_signed@_Num_float_base@std@@2_NB;

	// Token: 0x040000B4 RID: 180 RVA: 0x00013510 File Offset: 0x00011B10
	internal static bool ?is_modulo@_Num_float_base@std@@2_NB;

	// Token: 0x040000B5 RID: 181 RVA: 0x0001350F File Offset: 0x00011B0F
	internal static bool ?is_integer@_Num_float_base@std@@2_NB;

	// Token: 0x040000B6 RID: 182 RVA: 0x0001350E File Offset: 0x00011B0E
	internal static bool ?is_iec559@_Num_float_base@std@@2_NB;

	// Token: 0x040000B7 RID: 183 RVA: 0x0001350D File Offset: 0x00011B0D
	internal static bool ?is_exact@_Num_float_base@std@@2_NB;

	// Token: 0x040000B8 RID: 184 RVA: 0x0001350C File Offset: 0x00011B0C
	internal static bool ?is_bounded@_Num_float_base@std@@2_NB;

	// Token: 0x040000B9 RID: 185 RVA: 0x0001350B File Offset: 0x00011B0B
	internal static bool ?has_signaling_NaN@_Num_float_base@std@@2_NB;

	// Token: 0x040000BA RID: 186 RVA: 0x0001350A File Offset: 0x00011B0A
	internal static bool ?has_quiet_NaN@_Num_float_base@std@@2_NB;

	// Token: 0x040000BB RID: 187 RVA: 0x00013509 File Offset: 0x00011B09
	internal static bool ?has_infinity@_Num_float_base@std@@2_NB;

	// Token: 0x040000BC RID: 188 RVA: 0x00013508 File Offset: 0x00011B08
	internal static bool ?has_denorm_loss@_Num_float_base@std@@2_NB;

	// Token: 0x040000BD RID: 189 RVA: 0x00013504 File Offset: 0x00011B04
	internal static float_denorm_style ?has_denorm@_Num_float_base@std@@2W4float_denorm_style@2@B;

	// Token: 0x040000BE RID: 190 RVA: 0x00013500 File Offset: 0x00011B00
	internal static int ?radix@_Num_int_base@std@@2HB;

	// Token: 0x040000BF RID: 191 RVA: 0x000134FC File Offset: 0x00011AFC
	internal static bool ?is_specialized@_Num_int_base@std@@2_NB;

	// Token: 0x040000C0 RID: 192 RVA: 0x000134FB File Offset: 0x00011AFB
	internal static bool ?is_modulo@_Num_int_base@std@@2_NB;

	// Token: 0x040000C1 RID: 193 RVA: 0x000134FA File Offset: 0x00011AFA
	internal static bool ?is_integer@_Num_int_base@std@@2_NB;

	// Token: 0x040000C2 RID: 194 RVA: 0x000134F9 File Offset: 0x00011AF9
	internal static bool ?is_exact@_Num_int_base@std@@2_NB;

	// Token: 0x040000C3 RID: 195 RVA: 0x000134F8 File Offset: 0x00011AF8
	internal static bool ?is_bounded@_Num_int_base@std@@2_NB;

	// Token: 0x040000C4 RID: 196 RVA: 0x000134F4 File Offset: 0x00011AF4
	internal static int ?radix@_Num_base@std@@2HB;

	// Token: 0x040000C5 RID: 197 RVA: 0x000134F0 File Offset: 0x00011AF0
	internal static int ?min_exponent10@_Num_base@std@@2HB;

	// Token: 0x040000C6 RID: 198 RVA: 0x000134EC File Offset: 0x00011AEC
	internal static int ?min_exponent@_Num_base@std@@2HB;

	// Token: 0x040000C7 RID: 199 RVA: 0x000134E8 File Offset: 0x00011AE8
	internal static int ?max_exponent10@_Num_base@std@@2HB;

	// Token: 0x040000C8 RID: 200 RVA: 0x000134E4 File Offset: 0x00011AE4
	internal static int ?max_exponent@_Num_base@std@@2HB;

	// Token: 0x040000C9 RID: 201 RVA: 0x000134E0 File Offset: 0x00011AE0
	internal static int ?max_digits10@_Num_base@std@@2HB;

	// Token: 0x040000CA RID: 202 RVA: 0x000134DC File Offset: 0x00011ADC
	internal static int ?digits10@_Num_base@std@@2HB;

	// Token: 0x040000CB RID: 203 RVA: 0x000134D8 File Offset: 0x00011AD8
	internal static int ?digits@_Num_base@std@@2HB;

	// Token: 0x040000CC RID: 204 RVA: 0x000134D4 File Offset: 0x00011AD4
	internal static float_round_style ?round_style@_Num_base@std@@2W4float_round_style@2@B;

	// Token: 0x040000CD RID: 205 RVA: 0x000134D0 File Offset: 0x00011AD0
	internal static bool ?traps@_Num_base@std@@2_NB;

	// Token: 0x040000CE RID: 206 RVA: 0x000134CF File Offset: 0x00011ACF
	internal static bool ?tinyness_before@_Num_base@std@@2_NB;

	// Token: 0x040000CF RID: 207 RVA: 0x000134CE File Offset: 0x00011ACE
	internal static bool ?is_specialized@_Num_base@std@@2_NB;

	// Token: 0x040000D0 RID: 208 RVA: 0x000134CD File Offset: 0x00011ACD
	internal static bool ?is_signed@_Num_base@std@@2_NB;

	// Token: 0x040000D1 RID: 209 RVA: 0x000134CC File Offset: 0x00011ACC
	internal static bool ?is_modulo@_Num_base@std@@2_NB;

	// Token: 0x040000D2 RID: 210 RVA: 0x000134CB File Offset: 0x00011ACB
	internal static bool ?is_integer@_Num_base@std@@2_NB;

	// Token: 0x040000D3 RID: 211 RVA: 0x000134CA File Offset: 0x00011ACA
	internal static bool ?is_iec559@_Num_base@std@@2_NB;

	// Token: 0x040000D4 RID: 212 RVA: 0x000134C9 File Offset: 0x00011AC9
	internal static bool ?is_exact@_Num_base@std@@2_NB;

	// Token: 0x040000D5 RID: 213 RVA: 0x000134C8 File Offset: 0x00011AC8
	internal static bool ?is_bounded@_Num_base@std@@2_NB;

	// Token: 0x040000D6 RID: 214 RVA: 0x000134C7 File Offset: 0x00011AC7
	internal static bool ?has_signaling_NaN@_Num_base@std@@2_NB;

	// Token: 0x040000D7 RID: 215 RVA: 0x000134C6 File Offset: 0x00011AC6
	internal static bool ?has_quiet_NaN@_Num_base@std@@2_NB;

	// Token: 0x040000D8 RID: 216 RVA: 0x000134C5 File Offset: 0x00011AC5
	internal static bool ?has_infinity@_Num_base@std@@2_NB;

	// Token: 0x040000D9 RID: 217 RVA: 0x000134C4 File Offset: 0x00011AC4
	internal static bool ?has_denorm_loss@_Num_base@std@@2_NB;

	// Token: 0x040000DA RID: 218 RVA: 0x000134C0 File Offset: 0x00011AC0
	internal static float_denorm_style ?has_denorm@_Num_base@std@@2W4float_denorm_style@2@B;

	// Token: 0x040000DB RID: 219 RVA: 0x00013480 File Offset: 0x00011A80
	internal static uint ?value@?$_Sizeof@U_Nil@std@@U12@U12@U12@U12@U12@U12@U12@@std@@2IB;

	// Token: 0x040000DC RID: 220 RVA: 0x000134BD File Offset: 0x00011ABD
	internal static bool ?value@?$integral_constant@_N$00@std@@2_NB;

	// Token: 0x040000DD RID: 221 RVA: 0x000134BC File Offset: 0x00011ABC
	internal static bool ?value@?$integral_constant@_N$0A@@std@@2_NB;

	// Token: 0x040000DE RID: 222 RVA: 0x0001382B File Offset: 0x00011E2B
	internal static $ArrayType$$$BY00$$CBD ??_C@_00CNPNBAHC@?$AA@;

	// Token: 0x040000DF RID: 223 RVA: 0x0001405C File Offset: 0x0001265C
	internal static $ArrayType$$$BY0N@$$CBD ??_C@_0N@EFJOMLHH@ImageFlash?5?5?$AA@;

	// Token: 0x040000E0 RID: 224 RVA: 0x00013B14 File Offset: 0x00012114
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@MKPNLBEM@readFfu?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x040000E1 RID: 225 RVA: 0x000137A4 File Offset: 0x00011DA4
	internal static $ArrayType$$$BY02$$CBD ??_C@_02JDPG@rb?$AA@;

	// Token: 0x040000E2 RID: 226 RVA: 0x000139D4 File Offset: 0x00011FD4
	internal static $ArrayType$$$BY0CD@$$CBD ??_C@_0CD@GAPHJKMF@Could?5not?5open?5FFU?5file?0?5filenam@;

	// Token: 0x040000E3 RID: 227 RVA: 0x00013B28 File Offset: 0x00012128
	internal static $ArrayType$$$BY0DA@$$CBD ??_C@_0DA@OLEAJDFH@Corrupted?5FFU?0?5incorrect?5headerS@;

	// Token: 0x040000E4 RID: 228 RVA: 0x00013B58 File Offset: 0x00012158
	internal static $ArrayType$$$BY0DG@$$CBD ??_C@_0DG@LJEFNDMN@Corrupted?5FFU?0?5could?5not?5read?5im@;

	// Token: 0x040000E5 RID: 229 RVA: 0x00013B90 File Offset: 0x00012190
	internal static $ArrayType$$$BY0DI@$$CBD ??_C@_0DI@OMEKDBC@Corrupted?5FFU?0?5size?5of?5image?5hea@;

	// Token: 0x040000E6 RID: 230 RVA: 0x00013BC8 File Offset: 0x000121C8
	internal static $ArrayType$$$BY0CP@$$CBD ??_C@_0CP@NOMNLDJO@Corrupted?5FFU?0?5cannot?5skip?5manif@;

	// Token: 0x040000E7 RID: 231 RVA: 0x00013BF8 File Offset: 0x000121F8
	internal static $ArrayType$$$BY0CP@$$CBD ??_C@_0CP@JCLBHCCP@Corrupted?5FFU?0?5cannot?5skip?5paddd@;

	// Token: 0x040000E8 RID: 232 RVA: 0x00013C2C File Offset: 0x0001222C
	internal static $ArrayType$$$BY0DF@$$CBD ??_C@_0DF@NIFEHAIP@fread?$CI?$CGstoreHeader?0?5sizeof?$CIStore@;

	// Token: 0x040000E9 RID: 233 RVA: 0x00013C28 File Offset: 0x00012228
	internal static $ArrayType$$$BY02$$CBD ??_C@_02KEGNLNML@?0?5?$AA@;

	// Token: 0x040000EA RID: 234 RVA: 0x00013D08 File Offset: 0x00012308
	internal static $ArrayType$$$BY0N@$$CBD ??_C@_0N@LPLCCEEL@?0?5filename?3?5?$AA@;

	// Token: 0x040000EB RID: 235 RVA: 0x00013D04 File Offset: 0x00012304
	internal static $ArrayType$$$BY01$$CBD ??_C@_01LFCBOECM@?4?$AA@;

	// Token: 0x040000EC RID: 236 RVA: 0x00013CC4 File Offset: 0x000122C4
	internal static $ArrayType$$$BY0DN@$$CBD ??_C@_0DN@EDLJMDMN@The?5FFU?5has?5wrong?5version?0?5must?5@;

	// Token: 0x040000ED RID: 237 RVA: 0x00013C68 File Offset: 0x00012268
	internal static $ArrayType$$$BY0EL@$$CBD ??_C@_0EL@FHOOFPLA@The?5FFU?5is?5not?5a?5full?5flash?5FFU?4@;

	// Token: 0x040000EE RID: 238 RVA: 0x00013CB4 File Offset: 0x000122B4
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@FGKADOIP@readFfu?$CI?$CJ?5End?4?$AA@;

	// Token: 0x040000EF RID: 239 RVA: 0x00013D74 File Offset: 0x00012374
	internal static $ArrayType$$$BY0CJ@$$CBD ??_C@_0CJ@EDDAJGLL@Too?5small?5maximum?5block?5size?0?5fi@;

	// Token: 0x040000F0 RID: 240 RVA: 0x00013DA0 File Offset: 0x000123A0
	internal static $ArrayType$$$BY0IB@$$CBD ??_C@_0IB@KAMICEBB@0?5?$CB?$DN?5readDescriptors?$CIfp?0?5payload@;

	// Token: 0x040000F1 RID: 241 RVA: 0x00013E24 File Offset: 0x00012424
	internal static $ArrayType$$$BY0CN@$$CBD ??_C@_0CN@KKAJNADJ@fread?$CI?$CGblock?0?5sizeof?$CIDataBlock?$CJ?0@;

	// Token: 0x040000F2 RID: 242 RVA: 0x00013E88 File Offset: 0x00012488
	internal static $ArrayType$$$BY0DJ@$$CBD ??_C@_0DJ@NAHGMBCO@block?4blockLocationCount?5?$DO?50?5?$CG?$CG?5@;

	// Token: 0x040000F3 RID: 243 RVA: 0x00013E54 File Offset: 0x00012454
	internal static $ArrayType$$$BY0DE@$$CBD ??_C@_0DE@OEBOFPPN@1?5?$DN?$DN?5fread?$CI?$CGlocation?0?5sizeof?$CIBlo@;

	// Token: 0x040000F4 RID: 244 RVA: 0x00013EC4 File Offset: 0x000124C4
	internal static $ArrayType$$$BY0DE@$$CBD ??_C@_0DE@KMDNEMNG@Seek?5to?5the?5GPT?5header?5?$CL?5MBR?5siz@;

	// Token: 0x040000F5 RID: 245 RVA: 0x00013EF8 File Offset: 0x000124F8
	internal static $ArrayType$$$BY0CE@$$CBD ??_C@_0CE@CGHHDHDD@Read?5table?5header?5failed?0?5filena@;

	// Token: 0x040000F6 RID: 246 RVA: 0x00013998 File Offset: 0x00011F98
	internal static $ArrayType$$$BY08$$CBD ??_C@_08BOGKMBPC@EFI?5PART?$AA@;

	// Token: 0x040000F7 RID: 247 RVA: 0x00013F1C File Offset: 0x0001251C
	internal static $ArrayType$$$BY0DB@$$CBD ??_C@_0DB@NLCGKNEK@Wrong?5number?5of?5GPT?5partition?5en@;

	// Token: 0x040000F8 RID: 248 RVA: 0x00013F50 File Offset: 0x00012550
	internal static $ArrayType$$$BY0CN@$$CBD ??_C@_0CN@BBLDFMCJ@Wrong?5size?5of?5GPT?5partition?5entr@;

	// Token: 0x040000F9 RID: 249 RVA: 0x00013F80 File Offset: 0x00012580
	internal static $ArrayType$$$BY0CN@$$CBD ??_C@_0CN@EGEGKNHO@Cannot?5read?5GPT?5partition?5entrie@;

	// Token: 0x040000FA RID: 250 RVA: 0x00013FE8 File Offset: 0x000125E8
	internal static $ArrayType$$$BY0CA@$$CBD ??_C@_0CA@MGBAEBFD@?5Calculated?5CRC32?5of?5header?3?50x?$AA@;

	// Token: 0x040000FB RID: 251 RVA: 0x00013FB4 File Offset: 0x000125B4
	internal static $ArrayType$$$BY0DE@$$CBD ??_C@_0DE@NAEAEFBF@CRC32?5mismatch?5of?5GPT?5header?$CB?5CR@;

	// Token: 0x040000FC RID: 252 RVA: 0x00014008 File Offset: 0x00012608
	internal static $ArrayType$$$BY0ED@$$CBD ??_C@_0ED@DFFBNCAK@CRC32?5mismatch?5of?5GPT?5partition?5@;

	// Token: 0x040000FD RID: 253 RVA: 0x0001404C File Offset: 0x0001264C
	internal static $ArrayType$$$BY04$$CBD ??_C@_04MIKOFOFJ@SBL1?$AA@;

	// Token: 0x040000FE RID: 254 RVA: 0x00014054 File Offset: 0x00012654
	internal static $ArrayType$$$BY04$$CBD ??_C@_04DAEIOFGI@UEFI?$AA@;

	// Token: 0x040000FF RID: 255 RVA: 0x00013788 File Offset: 0x00011D88
	internal static $ArrayType$$$BY0BM@$$CBD ??_C@_0BM@GBJGKMED@?5image?5parsed?5successfully?4?$AA@;

	// Token: 0x04000100 RID: 256 RVA: 0x000137A8 File Offset: 0x00011DA8
	internal static $ArrayType$$$BY0CA@$$CBD ??_C@_0CA@JFGCLPEA@Could?5not?5open?5file?0?5filename?3?5?$AA@;

	// Token: 0x04000101 RID: 257 RVA: 0x000137C8 File Offset: 0x00011DC8
	internal static $ArrayType$$$BY0CN@$$CBD ??_C@_0CN@CDANHBKI@The?5file?5is?5not?5valid?5boot?5image@;

	// Token: 0x04000102 RID: 258 RVA: 0x00013D50 File Offset: 0x00012350
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@DFMAPPBF@readGpt?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000103 RID: 259 RVA: 0x0001367C File Offset: 0x00011C7C
	internal static $ArrayType$$$BY08$$CBD ??_C@_08KJPBNJGC@Overflow?$AA@;

	// Token: 0x04000104 RID: 260 RVA: 0x00013D64 File Offset: 0x00012364
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@KKOFAEKD@readGpt?$CI?$CJ?5End?4?$AA@;

	// Token: 0x04000105 RID: 261 RVA: 0x000137F8 File Offset: 0x00011DF8
	internal static $ArrayType$$$BY0BJ@$$CBD ??_C@_0BJ@EAKABFJM@readDescriptors?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000106 RID: 262 RVA: 0x00013814 File Offset: 0x00011E14
	internal static $ArrayType$$$BY0BH@$$CBD ??_C@_0BH@ODFOOBCN@readDescriptors?$CI?$CJ?5End?4?$AA@;

	// Token: 0x04000107 RID: 263 RVA: 0x00013664 File Offset: 0x00011C64
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@EFIOPFCC@readRkh?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000108 RID: 264 RVA: 0x00013678 File Offset: 0x00011C78
	internal static $ArrayType$$$BY03$$CBD ??_C@_03GKBDPGIH@GPT?$AA@;

	// Token: 0x04000109 RID: 265 RVA: 0x00013688 File Offset: 0x00011C88
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@OAPOKHFA@readRkh?$CI?$CJ?5End?4?$AA@;

	// Token: 0x0400010A RID: 266 RVA: 0x0001382C File Offset: 0x00011E2C
	internal static $ArrayType$$$BY0CJ@$$CBD ??_C@_0CJ@KHIPEPBA@readSecurityHdrAndCheckValidity?$CI@;

	// Token: 0x0400010B RID: 267 RVA: 0x00013858 File Offset: 0x00011E58
	internal static $ArrayType$$$BY0CO@$$CBD ??_C@_0CO@NPKMMIFF@Corrupted?5FFU?0?5could?5not?5read?5se@;

	// Token: 0x0400010C RID: 268 RVA: 0x00013888 File Offset: 0x00011E88
	internal static $ArrayType$$$BY0N@$$CBD ??_C@_0N@LJGGPJIJ@SignedImage?5?$AA@;

	// Token: 0x0400010D RID: 269 RVA: 0x00013898 File Offset: 0x00011E98
	internal static $ArrayType$$$BY0CP@$$CBD ??_C@_0CP@MDFGNDNN@Corrupted?5FFU?0?5image?5header?5sign@;

	// Token: 0x0400010E RID: 270 RVA: 0x000138E0 File Offset: 0x00011EE0
	internal static $ArrayType$$$BY0BC@$$CBD ??_C@_0BC@LJPEICOC@?4?5Expected?5size?3?5?$AA@;

	// Token: 0x0400010F RID: 271 RVA: 0x000138C8 File Offset: 0x00011EC8
	internal static $ArrayType$$$BY0BG@$$CBD ??_C@_0BG@MMHJALCK@Invalid?5struct?5size?3?5?$AA@;

	// Token: 0x04000110 RID: 272 RVA: 0x000138F8 File Offset: 0x00011EF8
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@NPNBLBND@Unsupported?5algorithm?3?5?$AA@;

	// Token: 0x04000111 RID: 273 RVA: 0x00013910 File Offset: 0x00011F10
	internal static $ArrayType$$$BY0BF@$$CBD ??_C@_0BF@CHEBECHJ@Invalid?5chunk?5size?3?5?$AA@;

	// Token: 0x04000112 RID: 274 RVA: 0x00013928 File Offset: 0x00011F28
	internal static $ArrayType$$$BY0BH@$$CBD ??_C@_0BH@EMBEMOOJ@Invalid?5catalog?5size?3?5?$AA@;

	// Token: 0x04000113 RID: 275 RVA: 0x00013940 File Offset: 0x00011F40
	internal static $ArrayType$$$BY0BK@$$CBD ??_C@_0BK@HOBHKCKM@Invalid?5hash?5table?5size?3?5?$AA@;

	// Token: 0x04000114 RID: 276 RVA: 0x0001395C File Offset: 0x00011F5C
	internal static $ArrayType$$$BY0CH@$$CBD ??_C@_0CH@EGAMGHCG@readSecurityHdrAndCheckValidity?$CI@;

	// Token: 0x04000115 RID: 277 RVA: 0x00013D18 File Offset: 0x00012318
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@FFBKAKPK@DumpPartitions?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000116 RID: 278 RVA: 0x00013D30 File Offset: 0x00012330
	internal static $ArrayType$$$BY04$$CBD ??_C@_04GKHLBAIJ@?4bin?$AA@;

	// Token: 0x04000117 RID: 279 RVA: 0x00013D38 File Offset: 0x00012338
	internal static $ArrayType$$$BY0BG@$$CBD ??_C@_0BG@GBPPGJLC@DumpPartitions?$CI?$CJ?5End?4?$AA@;

	// Token: 0x04000118 RID: 280 RVA: 0x00013984 File Offset: 0x00011F84
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@HHMKMHJH@DumpGpt?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000119 RID: 281 RVA: 0x000139A4 File Offset: 0x00011FA4
	internal static $ArrayType$$$BY06$$CBD ??_C@_06DPNFGDJN@?$CFd?4bin?$AA@;

	// Token: 0x0400011A RID: 282 RVA: 0x000139AC File Offset: 0x00011FAC
	internal static $ArrayType$$$BY0P@$$CBD ??_C@_0P@LHKOCCJE@DumpGpt?$CI?$CJ?5End?4?$AA@;

	// Token: 0x0400011B RID: 283 RVA: 0x000139BC File Offset: 0x00011FBC
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@MDMCKOFE@integrityCheck?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x0400011C RID: 284 RVA: 0x000139F8 File Offset: 0x00011FF8
	internal static $ArrayType$$$BY0EB@$$CBD ??_C@_0EB@NNMJIEIB@Corrupted?5FFU?0?5cannot?5seek?5to?5be@;

	// Token: 0x0400011D RID: 285 RVA: 0x00013A3C File Offset: 0x0001203C
	internal static $ArrayType$$$BY0DC@$$CBD ??_C@_0DC@DBIHALHK@Corrupted?5FFU?0?5could?5not?5read?5FF@;

	// Token: 0x0400011E RID: 286 RVA: 0x00013A70 File Offset: 0x00012070
	internal static $ArrayType$$$BY0DJ@$$CBD ??_C@_0DJ@BLCILKMJ@Corrupted?5FFU?0?5cannot?5seek?5to?5st@;

	// Token: 0x0400011F RID: 287 RVA: 0x00013AAC File Offset: 0x000120AC
	internal static $ArrayType$$$BY0CI@$$CBD ??_C@_0CI@CGNAALBM@Corrupted?5FFU?0?5hash?5mismatch?0?5fi@;

	// Token: 0x04000120 RID: 288 RVA: 0x00013AD4 File Offset: 0x000120D4
	internal static $ArrayType$$$BY0CI@$$CBD ??_C@_0CI@PNDJCJNO@Calculation?5of?5hash?5terminated?5b@;

	// Token: 0x04000121 RID: 289 RVA: 0x00013AFC File Offset: 0x000120FC
	internal static $ArrayType$$$BY0BG@$$CBD ??_C@_0BG@NGLINCBG@integrityCheck?$CI?$CJ?5End?4?$AA@;

	// Token: 0x04000122 RID: 290 RVA: 0x00013744 File Offset: 0x00011D44
	internal static $ArrayType$$$BY03$$CBD ??_C@_03BBDBFBBB@dpp?$AA@;

	// Token: 0x04000123 RID: 291 RVA: 0x00013748 File Offset: 0x00011D48
	internal static $ArrayType$$$BY0BL@$$CBD ??_C@_0BL@EMPFHFAH@checkDppPartition?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000124 RID: 292 RVA: 0x00013764 File Offset: 0x00011D64
	internal static $ArrayType$$$BY0BJ@$$CBD ??_C@_0BJ@PLPGDIKF@checkDppPartition?$CI?$CJ?5End?4?$AA@;

	// Token: 0x04000125 RID: 293 RVA: 0x00013698 File Offset: 0x00011C98
	internal static $ArrayType$$$BY0BB@$$CBD ??_C@_0BB@CMIDDDDM@ImageId?5read?5ok?4?$AA@;

	// Token: 0x04000126 RID: 294 RVA: 0x000136AC File Offset: 0x00011CAC
	internal static $ArrayType$$$BY0BF@$$CBD ??_C@_0BF@FCPPKGEC@readImageId?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x04000127 RID: 295 RVA: 0x000136C4 File Offset: 0x00011CC4
	internal static $ArrayType$$$BY0CI@$$CBD ??_C@_0CI@FEMMPJOB@readImageId?0?5Unable?5to?5read?5file@;

	// Token: 0x04000128 RID: 296 RVA: 0x000136EC File Offset: 0x00011CEC
	internal static $ArrayType$$$BY0BO@$$CBD ??_C@_0BO@EIIEAOIB@Encrypted?5image?5not?5supported?$AA@;

	// Token: 0x04000129 RID: 297 RVA: 0x0001370C File Offset: 0x00011D0C
	internal static $ArrayType$$$BY0CC@$$CBD ??_C@_0CC@DMHIICGH@Encrypted?5image?5is?5not?5supported@;

	// Token: 0x0400012A RID: 298 RVA: 0x00013730 File Offset: 0x00011D30
	internal static $ArrayType$$$BY0BD@$$CBD ??_C@_0BD@BAEILGK@readImageId?$CI?$CJ?5End?4?$AA@;

	// Token: 0x0400012B RID: 299 RVA: 0x000135E0 File Offset: 0x00011BE0
	internal static $ArrayType$$$BY0BK@$$CBD ??_C@_0BK@CONNOIJG@isValidBootImage?$CI?$CJ?5Start?4?$AA@;

	// Token: 0x0400012C RID: 300 RVA: 0x000135FC File Offset: 0x00011BFC
	internal static $ArrayType$$$BY0BI@$$CBD ??_C@_0BI@HEHMDDPC@isValidBootImage?$CI?$CJ?5End?4?$AA@;

	// Token: 0x0400012D RID: 301 RVA: 0x00013614 File Offset: 0x00011C14
	internal static $ArrayType$$$BY0BM@$$CBD ??_C@_0BM@NMJKDPPO@invalid?5vector?$DMT?$DO?5subscript?$AA@;

	// Token: 0x0400012E RID: 302 RVA: 0x00013630 File Offset: 0x00011C30
	internal static $ArrayType$$$BY0BD@$$CBD ??_C@_0BD@OLBABOEK@vector?$DMT?$DO?5too?5long?$AA@;

	// Token: 0x0400012F RID: 303 RVA: 0x00013644 File Offset: 0x00011C44
	internal static $ArrayType$$$BY08$$CBD ??_C@_08EPJLHIJG@bad?5cast?$AA@;

	// Token: 0x04000130 RID: 304 RVA: 0x00039ED8 File Offset: 0x000384D8
	internal static _s__RTTIBaseClassDescriptor2 ??_R17?0A@EA@?$_Iosb@H@std@@8;

	// Token: 0x04000131 RID: 305 RVA: 0x0003CC90 File Offset: 0x0003B090
	internal static basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> IMAGE_HEADER_SIGNATURE;

	// Token: 0x04000132 RID: 306 RVA: 0x000132C8 File Offset: 0x000118C8
	internal static method IMAGE_HEADER_SIGNATURE$initializer$;

	// Token: 0x04000133 RID: 307 RVA: 0x00013780 File Offset: 0x00011D80
	internal static $ArrayType$$$BY01$$CBH ??_8?$basic_ofstream@DU?$char_traits@D@std@@@std@@7B@;

	// Token: 0x04000134 RID: 308 RVA: 0x00039E0C File Offset: 0x0003840C
	internal static _s__RTTICompleteObjectLocator ??_R4?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;

	// Token: 0x04000135 RID: 309 RVA: 0x0003A27C File Offset: 0x0003887C
	internal static _s__ThrowInfo _TI2?AVbad_cast@std@@;

	// Token: 0x04000136 RID: 310 RVA: 0x00039DC4 File Offset: 0x000383C4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_streambuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000137 RID: 311 RVA: 0x0003C5D0 File Offset: 0x0003A9D0
	internal static $_TypeDescriptor$_extraBytes_51 ??_R0?AV?$basic_iostream@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000138 RID: 312 RVA: 0x0003C414 File Offset: 0x0003A814
	internal static $_TypeDescriptor$_extraBytes_19 ??_R0?AVbad_cast@std@@@8;

	// Token: 0x04000139 RID: 313 RVA: 0x00039F00 File Offset: 0x00038500
	internal static _s__RTTIClassHierarchyDescriptor ??_R3ios_base@std@@8;

	// Token: 0x0400013A RID: 314 RVA: 0x00039DD4 File Offset: 0x000383D4
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_streambuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400013B RID: 315 RVA: 0x00039F4C File Offset: 0x0003854C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@A@3FA@?$basic_ios@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400013C RID: 316 RVA: 0x0003A010 File Offset: 0x00038610
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_ostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400013D RID: 317 RVA: 0x00039FC4 File Offset: 0x000385C4
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_istream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400013E RID: 318 RVA: 0x00039FFC File Offset: 0x000385FC
	internal static $_s__RTTIBaseClassArray$_extraBytes_16 ??_R2?$basic_ostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400013F RID: 319 RVA: 0x0003A144 File Offset: 0x00038744
	internal static $_s__RTTIBaseClassArray$_extraBytes_20 ??_R2?$basic_ofstream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000140 RID: 320 RVA: 0x00039E48 File Offset: 0x00038448
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_filebuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000141 RID: 321 RVA: 0x0003A15C File Offset: 0x0003875C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_ofstream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000142 RID: 322 RVA: 0x00039E6C File Offset: 0x0003846C
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x04000143 RID: 323 RVA: 0x0003C4C4 File Offset: 0x0003A8C4
	internal static $ArrayType$$$BY0BA@Q6AXXZ ??_7?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;

	// Token: 0x04000144 RID: 324 RVA: 0x0003C580 File Offset: 0x0003A980
	internal static $_TypeDescriptor$_extraBytes_72 ??_R0?AV?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@8;

	// Token: 0x04000145 RID: 325 RVA: 0x00039E20 File Offset: 0x00038420
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_filebuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000146 RID: 326 RVA: 0x00039F68 File Offset: 0x00038568
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@A@3EA@ios_base@std@@8;

	// Token: 0x04000147 RID: 327 RVA: 0x0003D301 File Offset: 0x0003B701
	internal static piecewise_construct_t piecewise_construct;

	// Token: 0x04000148 RID: 328 RVA: 0x0003C7A4 File Offset: 0x0003ABA4
	internal static $_TypeDescriptor$_extraBytes_22 ??_R0?AUFfuReaderResult@@@8;

	// Token: 0x04000149 RID: 329 RVA: 0x0003A16C File Offset: 0x0003876C
	internal static _s__RTTICompleteObjectLocator ??_R4?$basic_ofstream@DU?$char_traits@D@std@@@std@@6B@;

	// Token: 0x0400014A RID: 330 RVA: 0x0003C60C File Offset: 0x0003AA0C
	internal static $_TypeDescriptor$_extraBytes_50 ??_R0?AV?$basic_istream@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x0400014B RID: 331 RVA: 0x0003A2A8 File Offset: 0x000388A8
	internal static $_s__CatchableTypeArray$_extraBytes_4 _CTA1?AUFfuReaderResult@@;

	// Token: 0x0400014C RID: 332 RVA: 0x0003C680 File Offset: 0x0003AA80
	internal static $_TypeDescriptor$_extraBytes_19 ??_R0?AVios_base@std@@@8;

	// Token: 0x0400014D RID: 333 RVA: 0x00039E58 File Offset: 0x00038458
	internal static _s__RTTICompleteObjectLocator ??_R4?$basic_filebuf@DU?$char_traits@D@std@@@std@@6B@;

	// Token: 0x0400014E RID: 334 RVA: 0x00039EA4 File Offset: 0x000384A4
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$_Iosb@H@std@@8;

	// Token: 0x0400014F RID: 335 RVA: 0x0003A238 File Offset: 0x00038838
	internal static _s__CatchableType _CT??_R0?AVbad_cast@std@@@8??0bad_cast@std@@$$FQAE@ABV01@@Z12;

	// Token: 0x04000150 RID: 336 RVA: 0x0003A0BC File Offset: 0x000386BC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x04000151 RID: 337 RVA: 0x00039EC8 File Offset: 0x000384C8
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$_Iosb@H@std@@8;

	// Token: 0x04000152 RID: 338 RVA: 0x0003CFEC File Offset: 0x0003B3EC
	internal static int ?_Stinit@?1??_Init@?$basic_filebuf@DU?$char_traits@D@std@@@std@@IAEXPAU_iobuf@@W4_Initfl@23@@Z@4HA;

	// Token: 0x04000153 RID: 339 RVA: 0x00039DBC File Offset: 0x000383BC
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$basic_streambuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000154 RID: 340 RVA: 0x0003A0E0 File Offset: 0x000386E0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@IFfuReader@@8;

	// Token: 0x04000155 RID: 341 RVA: 0x0003A1B8 File Offset: 0x000387B8
	internal static _s__RTTICompleteObjectLocator ??_R4FfuReader@@6B@;

	// Token: 0x04000156 RID: 342 RVA: 0x00039FE0 File Offset: 0x000385E0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_ostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000157 RID: 343 RVA: 0x0003A020 File Offset: 0x00038620
	internal static _s__RTTIBaseClassDescriptor2 ??_R1BA@?0A@EA@?$basic_ostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000158 RID: 344 RVA: 0x0003A0CC File Offset: 0x000386CC
	internal static _s__RTTICompleteObjectLocator ??_R4?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;

	// Token: 0x04000159 RID: 345 RVA: 0x0003A2B0 File Offset: 0x000388B0
	internal static _s__ThrowInfo _TI1?AUFfuReaderResult@@;

	// Token: 0x0400015A RID: 346 RVA: 0x0003D300 File Offset: 0x0003B700
	internal static allocator_arg_t allocator_arg;

	// Token: 0x0400015B RID: 347 RVA: 0x0003A19C File Offset: 0x0003879C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2FfuReader@@8;

	// Token: 0x0400015C RID: 348 RVA: 0x0003A1A8 File Offset: 0x000387A8
	internal static _s__RTTIClassHierarchyDescriptor ??_R3FfuReader@@8;

	// Token: 0x0400015D RID: 349 RVA: 0x0003C7EC File Offset: 0x0003ABEC
	internal static $ArrayType$$$BY04Q6AXXZ ??_7FfuReader@@6B@;

	// Token: 0x0400015E RID: 350 RVA: 0x0003C6F8 File Offset: 0x0003AAF8
	internal static $ArrayType$$$BY03Q6AXXZ ??_7?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@6B@;

	// Token: 0x0400015F RID: 351 RVA: 0x0003C500 File Offset: 0x0003A900
	internal static $_TypeDescriptor$_extraBytes_50 ??_R0?AV?$basic_filebuf@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000160 RID: 352 RVA: 0x00039EF4 File Offset: 0x000384F4
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2ios_base@std@@8;

	// Token: 0x04000161 RID: 353 RVA: 0x00039E88 File Offset: 0x00038488
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_ios@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000162 RID: 354 RVA: 0x00039DF0 File Offset: 0x000383F0
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x04000163 RID: 355 RVA: 0x0003C648 File Offset: 0x0003AA48
	internal static $_TypeDescriptor$_extraBytes_46 ??_R0?AV?$basic_ios@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000164 RID: 356 RVA: 0x00039FB4 File Offset: 0x000385B4
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_istream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000165 RID: 357 RVA: 0x0003A0FC File Offset: 0x000386FC
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2IFfuReader@@8;

	// Token: 0x04000166 RID: 358 RVA: 0x00039EC0 File Offset: 0x000384C0
	internal static $_s__RTTIBaseClassArray$_extraBytes_4 ??_R2?$_Iosb@H@std@@8;

	// Token: 0x04000167 RID: 359 RVA: 0x0003C430 File Offset: 0x0003A830
	internal static $_TypeDescriptor$_extraBytes_69 ??_R0?AV?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@8;

	// Token: 0x04000168 RID: 360 RVA: 0x0003A28C File Offset: 0x0003888C
	internal static _s__CatchableType _CT??_R0?AUFfuReaderResult@@@8??0FfuReaderResult@@$$FQAE@ABU0@@Z28;

	// Token: 0x04000169 RID: 361 RVA: 0x0003C69C File Offset: 0x0003AA9C
	internal static $_TypeDescriptor$_extraBytes_20 ??_R0?AV?$_Iosb@H@std@@@8;

	// Token: 0x0400016A RID: 362 RVA: 0x00013654 File Offset: 0x00011C54
	internal static $ArrayType$$$BY01$$CBH ??_8?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@7B?$basic_istream@DU?$char_traits@D@std@@@1@@;

	// Token: 0x0400016B RID: 363 RVA: 0x00039E3C File Offset: 0x0003843C
	internal static $_s__RTTIBaseClassArray$_extraBytes_8 ??_R2?$basic_filebuf@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400016C RID: 364 RVA: 0x0003C7D0 File Offset: 0x0003ABD0
	internal static $_TypeDescriptor$_extraBytes_16 ??_R0?AVFfuReader@@@8;

	// Token: 0x0400016D RID: 365 RVA: 0x0003A03C File Offset: 0x0003863C
	internal static $_s__RTTIBaseClassArray$_extraBytes_36 ??_R2?$basic_iostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400016E RID: 366 RVA: 0x0003A104 File Offset: 0x00038704
	internal static _s__RTTIClassHierarchyDescriptor ??_R3IFfuReader@@8;

	// Token: 0x0400016F RID: 367 RVA: 0x0003A074 File Offset: 0x00038674
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_iostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000170 RID: 368 RVA: 0x0003A180 File Offset: 0x00038780
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@FfuReader@@8;

	// Token: 0x04000171 RID: 369 RVA: 0x00039F3C File Offset: 0x0003853C
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_ios@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000172 RID: 370 RVA: 0x0003C704 File Offset: 0x0003AB04
	internal static $_TypeDescriptor$_extraBytes_17 ??_R0?AVIFfuReader@@@8;

	// Token: 0x04000173 RID: 371 RVA: 0x0001365C File Offset: 0x00011C5C
	internal static $ArrayType$$$BY01$$CBH ??_8?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@7B?$basic_ostream@DU?$char_traits@D@std@@@1@@;

	// Token: 0x04000174 RID: 372 RVA: 0x0003C788 File Offset: 0x0003AB88
	internal static $ArrayType$$$BY03Q6AXXZ ??_7?$basic_ofstream@DU?$char_traits@D@std@@@std@@6B@;

	// Token: 0x04000175 RID: 373 RVA: 0x00039F84 File Offset: 0x00038584
	internal static _s__RTTIBaseClassDescriptor2 ??_R17A@3EA@?$_Iosb@H@std@@8;

	// Token: 0x04000176 RID: 374 RVA: 0x0003A090 File Offset: 0x00038690
	internal static $_s__RTTIBaseClassArray$_extraBytes_40 ??_R2?$basic_stringstream@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x04000177 RID: 375 RVA: 0x0003A254 File Offset: 0x00038854
	internal static _s__CatchableType _CT??_R0?AVexception@std@@@8??0exception@std@@$$FQAE@ABV01@@Z12;

	// Token: 0x04000178 RID: 376 RVA: 0x0003C748 File Offset: 0x0003AB48
	internal static $_TypeDescriptor$_extraBytes_51 ??_R0?AV?$basic_ofstream@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000179 RID: 377 RVA: 0x0003A064 File Offset: 0x00038664
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_iostream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400017A RID: 378 RVA: 0x00039DFC File Offset: 0x000383FC
	internal static _s__RTTIClassHierarchyDescriptor ??_R3?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x0400017B RID: 379 RVA: 0x00039F10 File Offset: 0x00038510
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@ios_base@std@@8;

	// Token: 0x0400017C RID: 380 RVA: 0x00039F2C File Offset: 0x0003852C
	internal static $_s__RTTIBaseClassArray$_extraBytes_12 ??_R2?$basic_ios@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400017D RID: 381 RVA: 0x0003A128 File Offset: 0x00038728
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_ofstream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x0400017E RID: 382 RVA: 0x0003C724 File Offset: 0x0003AB24
	internal static $ArrayType$$$BY04Q6AXXZ ??_7IFfuReader@@6B@;

	// Token: 0x0400017F RID: 383 RVA: 0x0003C6B8 File Offset: 0x0003AAB8
	internal static $_TypeDescriptor$_extraBytes_50 ??_R0?AV?$basic_ostream@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000180 RID: 384 RVA: 0x0003A114 File Offset: 0x00038714
	internal static _s__RTTICompleteObjectLocator ??_R4IFfuReader@@6B@;

	// Token: 0x04000181 RID: 385 RVA: 0x0003A270 File Offset: 0x00038870
	internal static $_s__CatchableTypeArray$_extraBytes_8 _CTA2?AVbad_cast@std@@;

	// Token: 0x04000182 RID: 386 RVA: 0x0003CFF0 File Offset: 0x0003B3F0
	internal unsafe static locale.facet* ?_Psave@?$_Facetptr@V?$codecvt@DDH@std@@@std@@2PBVfacet@locale@2@B;

	// Token: 0x04000183 RID: 387 RVA: 0x00039FA0 File Offset: 0x000385A0
	internal static $_s__RTTIBaseClassArray$_extraBytes_16 ??_R2?$basic_istream@DU?$char_traits@D@std@@@std@@8;

	// Token: 0x04000184 RID: 388 RVA: 0x0003C480 File Offset: 0x0003A880
	internal static $_TypeDescriptor$_extraBytes_52 ??_R0?AV?$basic_streambuf@DU?$char_traits@D@std@@@std@@@8;

	// Token: 0x04000185 RID: 389 RVA: 0x0003C544 File Offset: 0x0003A944
	internal static $ArrayType$$$BY0BA@Q6AXXZ ??_7?$basic_filebuf@DU?$char_traits@D@std@@@std@@6B@;

	// Token: 0x04000186 RID: 390 RVA: 0x00039DA0 File Offset: 0x000383A0
	internal static _s__RTTIBaseClassDescriptor2 ??_R1A@?0A@EA@?$basic_stringbuf@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@8;

	// Token: 0x04000187 RID: 391 RVA: 0x00013FB0 File Offset: 0x000125B0
	public unsafe static int** __unep@?hex@std@@$$FYAAAVios_base@1@AAV21@@Z;

	// Token: 0x04000188 RID: 392 RVA: 0x00013650 File Offset: 0x00011C50
	public unsafe static int** __unep@??$_Bool_function@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@std@@$$FYAXAAU?$_Bool_struct@V?$basic_ostream@DU?$char_traits@D@std@@@std@@@0@@Z;

	// Token: 0x04000189 RID: 393 RVA: 0x000138F4 File Offset: 0x00011EF4
	unsafe static int** __unep@?endl@std@@$$FYAAAV?$basic_ostream@DU?$char_traits@D@std@@@1@AAV21@@Z;

	// Token: 0x0400018A RID: 394 RVA: 0x0003D304 File Offset: 0x0003B704
	internal static int __@@_PchSym_@00@UzUBUhUhlfixvUuufurovivzwviUivovzhvUhgwzucOlyq@2EEC82BF08E14368;

	// Token: 0x0400018B RID: 395 RVA: 0x000141B8 File Offset: 0x000127B8
	internal static __s_GUID _GUID_90f1a06e_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x0400018C RID: 396 RVA: 0x000141A8 File Offset: 0x000127A8
	internal static __s_GUID _GUID_cb2f6722_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400018D RID: 397 RVA: 0x000132CC File Offset: 0x000118CC
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_mp_z;

	// Token: 0x0400018E RID: 398
	[FixedAddressValueType]
	internal static int ?Uninitialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x0400018F RID: 399 RVA: 0x00013290 File Offset: 0x00011890
	internal static method ?Uninitialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000190 RID: 400 RVA: 0x000132D0 File Offset: 0x000118D0
	internal static $ArrayType$$$BY00Q6MPBXXZ __xi_vt_a;

	// Token: 0x04000191 RID: 401
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerAppDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000192 RID: 402 RVA: 0x000132A4 File Offset: 0x000118A4
	internal static method ?InitializedPerAppDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000193 RID: 403
	[FixedAddressValueType]
	internal static bool ?IsDefaultDomain@CurrentDomain@<CrtImplementationDetails>@@$$Q2_NA;

	// Token: 0x04000194 RID: 404 RVA: 0x00013294 File Offset: 0x00011894
	internal static method ?IsDefaultDomain$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000195 RID: 405 RVA: 0x00013288 File Offset: 0x00011888
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_ma_a;

	// Token: 0x04000196 RID: 406
	[FixedAddressValueType]
	internal static Progress.State ?InitializedNative@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x04000197 RID: 407 RVA: 0x0001329C File Offset: 0x0001189C
	internal static method ?InitializedNative$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x04000198 RID: 408
	[FixedAddressValueType]
	internal static int ?Initialized@CurrentDomain@<CrtImplementationDetails>@@$$Q2HA;

	// Token: 0x04000199 RID: 409 RVA: 0x0001328C File Offset: 0x0001188C
	internal static method ?Initialized$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400019A RID: 410 RVA: 0x000132A8 File Offset: 0x000118A8
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_ma_z;

	// Token: 0x0400019B RID: 411
	[FixedAddressValueType]
	internal static Progress.State ?InitializedVtables@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x0400019C RID: 412 RVA: 0x00013298 File Offset: 0x00011898
	internal static method ?InitializedVtables$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x0400019D RID: 413 RVA: 0x000132D4 File Offset: 0x000118D4
	internal static $ArrayType$$$BY00Q6MPBXXZ __xi_vt_z;

	// Token: 0x0400019E RID: 414 RVA: 0x00014198 File Offset: 0x00012798
	internal static __s_GUID _GUID_cb2f6723_ab3a_11d2_9c40_00c04fa30a3e;

	// Token: 0x0400019F RID: 415
	[FixedAddressValueType]
	internal static Progress.State ?InitializedPerProcess@CurrentDomain@<CrtImplementationDetails>@@$$Q2W4State@Progress@2@A;

	// Token: 0x040001A0 RID: 416 RVA: 0x000132A0 File Offset: 0x000118A0
	internal static method ?InitializedPerProcess$initializer$@CurrentDomain@<CrtImplementationDetails>@@$$Q2P6MXXZA;

	// Token: 0x040001A1 RID: 417 RVA: 0x0003D63F File Offset: 0x0003BA3F
	internal static bool ?InitializedPerProcess@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040001A2 RID: 418 RVA: 0x0003D63C File Offset: 0x0003BA3C
	internal static bool ?Entered@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040001A3 RID: 419 RVA: 0x0003D63D File Offset: 0x0003BA3D
	internal static bool ?InitializedNative@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040001A4 RID: 420 RVA: 0x0003D638 File Offset: 0x0003BA38
	internal static int ?Count@AllDomains@<CrtImplementationDetails>@@2HA;

	// Token: 0x040001A5 RID: 421 RVA: 0x00014188 File Offset: 0x00012788
	internal static uint ?ProcessAttach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x040001A6 RID: 422 RVA: 0x0001418C File Offset: 0x0001278C
	internal static uint ?ThreadAttach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x040001A7 RID: 423 RVA: 0x0003CCF8 File Offset: 0x0003B0F8
	internal static TriBool.State ?hasNative@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x040001A8 RID: 424 RVA: 0x00014184 File Offset: 0x00012784
	internal static uint ?ProcessDetach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x040001A9 RID: 425 RVA: 0x00014190 File Offset: 0x00012790
	internal static uint ?ThreadDetach@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x040001AA RID: 426 RVA: 0x00014194 File Offset: 0x00012794
	internal static uint ?ProcessVerifier@NativeDll@<CrtImplementationDetails>@@0IB;

	// Token: 0x040001AB RID: 427 RVA: 0x0003CCF4 File Offset: 0x0003B0F4
	internal static TriBool.State ?hasPerProcess@DefaultDomain@<CrtImplementationDetails>@@0W4State@TriBool@2@A;

	// Token: 0x040001AC RID: 428 RVA: 0x0003D63E File Offset: 0x0003BA3E
	internal static bool ?InitializedNativeFromCCTOR@DefaultDomain@<CrtImplementationDetails>@@2_NA;

	// Token: 0x040001AD RID: 429 RVA: 0x000132AC File Offset: 0x000118AC
	internal static $ArrayType$$$BY00Q6MPBXXZ __xc_mp_a;

	// Token: 0x040001AE RID: 430 RVA: 0x000141C8 File Offset: 0x000127C8
	internal static __s_GUID _GUID_90f1a06c_7712_4762_86b5_7a5eba6bdb02;

	// Token: 0x040001AF RID: 431 RVA: 0x000141D8 File Offset: 0x000127D8
	public unsafe static int** __unep@?DoNothing@DefaultDomain@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x040001B0 RID: 432 RVA: 0x000141DC File Offset: 0x000127DC
	public unsafe static int** __unep@?_UninitializeDefaultDomain@LanguageSupport@<CrtImplementationDetails>@@$$FCGJPAX@Z;

	// Token: 0x040001B1 RID: 433
	[FixedAddressValueType]
	internal static uint __exit_list_size_app_domain;

	// Token: 0x040001B2 RID: 434
	[FixedAddressValueType]
	internal unsafe static method* __onexitbegin_app_domain;

	// Token: 0x040001B3 RID: 435 RVA: 0x0003D74C File Offset: 0x0003BB4C
	internal static uint __exit_list_size;

	// Token: 0x040001B4 RID: 436
	[FixedAddressValueType]
	internal unsafe static method* __onexitend_app_domain;

	// Token: 0x040001B5 RID: 437 RVA: 0x0003D744 File Offset: 0x0003BB44
	internal unsafe static method* __onexitbegin_m;

	// Token: 0x040001B6 RID: 438 RVA: 0x0003D748 File Offset: 0x0003BB48
	internal unsafe static method* __onexitend_m;

	// Token: 0x040001B7 RID: 439
	[FixedAddressValueType]
	internal unsafe static void* ?_lock@AtExitLock@<CrtImplementationDetails>@@$$Q0PAXA;

	// Token: 0x040001B8 RID: 440
	[FixedAddressValueType]
	internal static int ?_ref_count@AtExitLock@<CrtImplementationDetails>@@$$Q0HA;

	// Token: 0x040001B9 RID: 441 RVA: 0x0003D798 File Offset: 0x0003BB98
	internal static _Fac_tidy_reg_t _Fac_tidy_reg;

	// Token: 0x040001BA RID: 442 RVA: 0x000132B0 File Offset: 0x000118B0
	internal static method _Fac_tidy_reg$initializer$;

	// Token: 0x040001BB RID: 443 RVA: 0x0003D77C File Offset: 0x0003BB7C
	internal unsafe static _Fac_node* _Fac_head;

	// Token: 0x040001BC RID: 444 RVA: 0x00013068 File Offset: 0x00011668
	internal unsafe static basic_ostream<char,std::char_traits<char>\u0020>* cout;

	// Token: 0x040001BD RID: 445 RVA: 0x00014174 File Offset: 0x00012774
	internal static $ArrayType$$$BY01Q6AXXZ ??_7type_info@@6B@;

	// Token: 0x040001BE RID: 446 RVA: 0x0001306C File Offset: 0x0001166C
	internal unsafe static locale.id* __imp_?id@?$codecvt@DDH@std@@2V0locale@2@A;

	// Token: 0x040001BF RID: 447 RVA: 0x0001309C File Offset: 0x0001169C
	internal unsafe static long* _BADOFF;

	// Token: 0x040001C0 RID: 448 RVA: 0x00013274 File Offset: 0x00011874
	internal static $ArrayType$$$BY0A@P6AXXZ __xc_z;

	// Token: 0x040001C1 RID: 449 RVA: 0x00013270 File Offset: 0x00011870
	internal static $ArrayType$$$BY0A@P6AXXZ __xc_a;

	// Token: 0x040001C2 RID: 450 RVA: 0x00013278 File Offset: 0x00011878
	internal static $ArrayType$$$BY0A@P6AHXZ __xi_a;

	// Token: 0x040001C3 RID: 451 RVA: 0x0003D7A0 File Offset: 0x0003BBA0
	internal static volatile __enative_startup_state __native_startup_state;

	// Token: 0x040001C4 RID: 452 RVA: 0x00013284 File Offset: 0x00011884
	internal static $ArrayType$$$BY0A@P6AHXZ __xi_z;

	// Token: 0x040001C5 RID: 453 RVA: 0x0003D79C File Offset: 0x0003BB9C
	internal unsafe static volatile void* __native_startup_lock;

	// Token: 0x040001C6 RID: 454 RVA: 0x0003CCF0 File Offset: 0x0003B0F0
	internal static volatile uint __native_dllmain_reason;
}

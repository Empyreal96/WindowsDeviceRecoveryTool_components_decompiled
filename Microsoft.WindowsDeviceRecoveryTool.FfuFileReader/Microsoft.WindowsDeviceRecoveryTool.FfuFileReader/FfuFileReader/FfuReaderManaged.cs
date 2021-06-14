using System;
using System.Runtime.InteropServices;
using std;

namespace FfuFileReader
{
	// Token: 0x02000006 RID: 6
	public class FfuReaderManaged
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000026E8 File Offset: 0x00001AE8
		public unsafe int Read(string path)
		{
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*(int*)<Module>.__imp_std.cout + 4) + <Module>.__imp_std.cout, 4, false);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			<Module>.msclr.interop.marshal_as<class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>,class\u0020System::String\u0020^>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, ref path);
			int result;
			try
			{
				FfuReader ffuReader;
				<Module>.FfuReader.{ctor}(ref ffuReader);
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>);
					FfuReaderResult ffuReaderResult;
					<Module>.FfuReader.readFfu(ref ffuReader, &ffuReaderResult, filename, 131072U, true, false, false);
					try
					{
						if (ffuReaderResult != null)
						{
							result = ffuReaderResult;
						}
						else
						{
							this.rootKeyHash = <Module>.msclr.interop.marshal_as<class\u0020System::String\u0020^,class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>\u0020>(ref ffuReader + 28);
							IntPtr ptr = new IntPtr(ref ffuReader + 17040);
							this.platformId = Marshal.PtrToStringAnsi(ptr);
							result = 0;
						}
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
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReader.{dtor}), (void*)(&ffuReader));
					throw;
				}
				<Module>.FfuReader.{dtor}(ref ffuReader);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
			return result;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00002810 File Offset: 0x00001C10
		public unsafe int ReadPlatformId(string path)
		{
			<Module>.std.basic_ios<char,std::char_traits<char>\u0020>.setstate(*(*(int*)<Module>.__imp_std.cout + 4) + <Module>.__imp_std.cout, 4, false);
			basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>;
			<Module>.msclr.interop.marshal_as<class\u0020std::basic_string<char,struct\u0020std::char_traits<char>,class\u0020std::allocator<char>\u0020>,class\u0020System::String\u0020^>(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, ref path);
			int result;
			try
			{
				FfuReader ffuReader;
				<Module>.FfuReader.{ctor}(ref ffuReader);
				try
				{
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020> basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2;
					basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>* filename = <Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{ctor}(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>2, ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>);
					FfuReaderResult ffuReaderResult;
					<Module>.FfuReader.readFfuPlatformId(ref ffuReader, &ffuReaderResult, filename);
					try
					{
						if (ffuReaderResult != null)
						{
							result = ffuReaderResult;
						}
						else
						{
							IntPtr ptr = new IntPtr(ref ffuReader + 17040);
							this.platformId = Marshal.PtrToStringAnsi(ptr);
							result = 0;
						}
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
					<Module>.___CxxCallUnwindDtor(ldftn(FfuReader.{dtor}), (void*)(&ffuReader));
					throw;
				}
				<Module>.FfuReader.{dtor}(ref ffuReader);
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>.{dtor}), (void*)(&basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>));
				throw;
			}
			<Module>.std.basic_string<char,std::char_traits<char>,std::allocator<char>\u0020>._Tidy(ref basic_string<char,std::char_traits<char>,std::allocator<char>_u0020>, true, 0U);
			return result;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00001618 File Offset: 0x00000A18
		public string PlatformId
		{
			get
			{
				return this.platformId;
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x060002AB RID: 683 RVA: 0x0000162C File Offset: 0x00000A2C
		public string RootKeyHash
		{
			get
			{
				return this.rootKeyHash;
			}
		}

		// Token: 0x040001CB RID: 459
		private string rootKeyHash;

		// Token: 0x040001CC RID: 460
		private string platformId;
	}
}

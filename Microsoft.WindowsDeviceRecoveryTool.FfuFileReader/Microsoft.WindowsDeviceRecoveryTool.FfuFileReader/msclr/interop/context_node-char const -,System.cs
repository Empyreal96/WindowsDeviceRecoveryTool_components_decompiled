using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using msclr.interop.details;

namespace msclr.interop
{
	// Token: 0x02000004 RID: 4
	internal class context_node<char\u0020const\u0020*,System::String\u0020^> : context_node_base, IDisposable
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00001A34 File Offset: 0x00000E34
		public unsafe context_node<char\u0020const\u0020*,System::String\u0020^>(sbyte** _to_object, string _from_object)
		{
			this._ptr = null;
			char_buffer<char> char_buffer<char>;
			if (_from_object == null)
			{
				*_to_object = 0;
			}
			else
			{
				uint num = <Module>.msclr.interop.details.GetAnsiStringSize(_from_object);
				char_buffer<char> = <Module>.new[](num);
				try
				{
					if (char_buffer<char> == null)
					{
						throw new InsufficientMemoryException();
					}
					<Module>.msclr.interop.details.WriteAnsiString(char_buffer<char>, num, _from_object);
					sbyte* ptr = char_buffer<char>;
					char_buffer<char> = 0;
					this._ptr = ptr;
					*_to_object = ptr;
				}
				catch
				{
					<Module>.___CxxCallUnwindDtor(ldftn(msclr.interop.details.char_buffer<char>.{dtor}), (void*)(&char_buffer<char>));
					throw;
				}
				<Module>.delete[](null);
			}
			try
			{
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(msclr.interop.details.char_buffer<char>.{dtor}), (void*)(&char_buffer<char>));
				throw;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00001AF0 File Offset: 0x00000EF0
		private unsafe void ~context_node<char\u0020const\u0020*,System::String\u0020^>()
		{
			<Module>.delete[]((void*)this._ptr);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000159C File Offset: 0x0000099C
		private unsafe void !context_node<char\u0020const\u0020*,System::String\u0020^>()
		{
			<Module>.delete[]((void*)this._ptr);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00001B08 File Offset: 0x00000F08
		[HandleProcessCorruptedStateExceptions]
		protected unsafe virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				<Module>.delete[]((void*)this._ptr);
			}
			else
			{
				try
				{
					this.!context_node<char\u0020const\u0020*,System::String\u0020^>();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00001DA0 File Offset: 0x000011A0
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00001B54 File Offset: 0x00000F54
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x040001C9 RID: 457
		private unsafe sbyte* _ptr;
	}
}

using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace msclr.interop
{
	// Token: 0x02000005 RID: 5
	internal class context_node<wchar_t\u0020const\u0020*,System::String\u0020^> : context_node_base, IDisposable
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x000015B4 File Offset: 0x000009B4
		public unsafe context_node<wchar_t\u0020const\u0020*,System::String\u0020^>(char** _to_object, string _from_object)
		{
			IntPtr ip = Marshal.StringToHGlobalUni(_from_object);
			this._ip = ip;
			*_to_object = this._ip.ToPointer();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00001B68 File Offset: 0x00000F68
		private void ~context_node<wchar_t\u0020const\u0020*,System::String\u0020^>()
		{
			this.!context_node<wchar_t\u0020const\u0020*,System::String\u0020^>();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000015E4 File Offset: 0x000009E4
		private void !context_node<wchar_t\u0020const\u0020*,System::String\u0020^>()
		{
			if (this._ip != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this._ip);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00001B7C File Offset: 0x00000F7C
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.!context_node<wchar_t\u0020const\u0020*,System::String\u0020^>();
			}
			else
			{
				try
				{
					this.!context_node<wchar_t\u0020const\u0020*,System::String\u0020^>();
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00001DBC File Offset: 0x000011BC
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00001BC0 File Offset: 0x00000FC0
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x040001CA RID: 458
		private IntPtr _ip;
	}
}

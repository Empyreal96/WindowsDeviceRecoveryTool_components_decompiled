using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using FlashingPlatform;
using RAII;

namespace Microsoft.Windows.Flashing.Platform
{
	// Token: 0x0200003C RID: 60
	public class Logger : IDisposable
	{
		// Token: 0x06000109 RID: 265 RVA: 0x000126F4 File Offset: 0x00011AF4
		internal unsafe Logger([In] FlashingPlatform Platform)
		{
			IFlashingPlatform* native = Platform.Native;
			IFlashingPlatform* ptr = native;
			this.m_Logger = calli(FlashingPlatform.ILogger* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ptr, *(*(int*)ptr));
			this.m_Platform = Platform;
			base..ctor();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00012748 File Offset: 0x00011B48
		internal unsafe ILogger* Native
		{
			get
			{
				return this.m_Logger;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0001272C File Offset: 0x00011B2C
		private void ~Logger()
		{
			this.m_Logger = null;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0001272C File Offset: 0x00011B2C
		private void !Logger()
		{
			this.m_Logger = null;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00012760 File Offset: 0x00011B60
		public unsafe void SetLogLevel(LogLevel Level)
		{
			ILogger* logger = this.m_Logger;
			object obj = calli(System.Int32 modopt(System.Runtime.CompilerServices.IsLong) modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel), logger, Level, *(*(int*)logger));
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000128A4 File Offset: 0x00011CA4
		public unsafe void Log(LogLevel Level, [In] string Message)
		{
			ushort* ptr;
			if (Message != null)
			{
				ptr = (ushort*)Marshal.StringToCoTaskMemUni(Message).ToPointer();
			}
			else
			{
				ptr = null;
			}
			CAutoComFree<unsigned\u0020short\u0020const\u0020*> cautoComFree<unsigned_u0020short_u0020const_u0020*>;
			*(ref cautoComFree<unsigned_u0020short_u0020const_u0020*> + 4) = ptr;
			cautoComFree<unsigned_u0020short_u0020const_u0020*> = ref <Module>.??_7?$CAutoComFree@PBG@RAII@@6B@;
			try
			{
				ILogger* logger = this.m_Logger;
				calli(System.Void modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr,FlashingPlatform.LogLevel,System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)*), logger, Level, calli(System.UInt16 modopt(System.Runtime.CompilerServices.IsConst)* modopt(System.Runtime.CompilerServices.CallConvThiscall)(System.IntPtr), ref cautoComFree<unsigned_u0020short_u0020const_u0020*>, *(cautoComFree<unsigned_u0020short_u0020const_u0020*> + 16)), *(*(int*)logger + 4));
			}
			catch
			{
				<Module>.___CxxCallUnwindDtor(ldftn(RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}), (void*)(&cautoComFree<unsigned_u0020short_u0020const_u0020*>));
				throw;
			}
			<Module>.RAII.CAutoComFree<unsigned\u0020short\u0020const\u0020*>.{dtor}(ref cautoComFree<unsigned_u0020short_u0020const_u0020*>);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00012838 File Offset: 0x00011C38
		[HandleProcessCorruptedStateExceptions]
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.m_Logger = null;
			}
			else
			{
				try
				{
					this.m_Logger = null;
				}
				finally
				{
					base.Finalize();
				}
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00012934 File Offset: 0x00011D34
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00012888 File Offset: 0x00011C88
		protected override void Finalize()
		{
			this.Dispose(false);
		}

		// Token: 0x04000112 RID: 274
		private unsafe ILogger* m_Logger;

		// Token: 0x04000113 RID: 275
		internal FlashingPlatform m_Platform;
	}
}

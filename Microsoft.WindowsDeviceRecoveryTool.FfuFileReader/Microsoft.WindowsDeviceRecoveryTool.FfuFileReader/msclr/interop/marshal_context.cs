using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace msclr.interop
{
	// Token: 0x02000003 RID: 3
	internal class marshal_context : IDisposable
	{
		// Token: 0x06000299 RID: 665 RVA: 0x00001538 File Offset: 0x00000938
		private void ~marshal_context()
		{
			LinkedList<object>.Enumerator enumerator = this._clean_up_list.GetEnumerator();
			if (enumerator.MoveNext())
			{
				do
				{
					IDisposable disposable = enumerator.Current as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				while (enumerator.MoveNext());
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000157C File Offset: 0x0000097C
		protected virtual void Dispose([MarshalAs(UnmanagedType.U1)] bool A_0)
		{
			if (A_0)
			{
				this.~marshal_context();
			}
			else
			{
				base.Finalize();
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00001A18 File Offset: 0x00000E18
		public sealed void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x040001C8 RID: 456
		internal readonly LinkedList<object> _clean_up_list = new LinkedList<object>();
	}
}

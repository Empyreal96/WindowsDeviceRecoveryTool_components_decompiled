using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004C6 RID: 1222
	internal class OleStrCAMarshaler : BaseCAMarshaler
	{
		// Token: 0x0600516B RID: 20843 RVA: 0x00151E11 File Offset: 0x00150011
		public OleStrCAMarshaler(NativeMethods.CA_STRUCT caAddr) : base(caAddr)
		{
		}

		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x0600516C RID: 20844 RVA: 0x0014C98B File Offset: 0x0014AB8B
		public override Type ItemType
		{
			get
			{
				return typeof(string);
			}
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x00151E41 File Offset: 0x00150041
		protected override Array CreateArray()
		{
			return new string[base.Count];
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x00151E50 File Offset: 0x00150050
		protected override object GetItemFromAddress(IntPtr addr)
		{
			string result = Marshal.PtrToStringUni(addr);
			Marshal.FreeCoTaskMem(addr);
			return result;
		}
	}
}

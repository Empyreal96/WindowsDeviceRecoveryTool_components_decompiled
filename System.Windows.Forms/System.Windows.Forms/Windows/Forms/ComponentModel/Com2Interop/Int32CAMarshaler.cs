using System;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004C5 RID: 1221
	internal class Int32CAMarshaler : BaseCAMarshaler
	{
		// Token: 0x06005167 RID: 20839 RVA: 0x00151E11 File Offset: 0x00150011
		public Int32CAMarshaler(NativeMethods.CA_STRUCT caStruct) : base(caStruct)
		{
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x00151E1A File Offset: 0x0015001A
		public override Type ItemType
		{
			get
			{
				return typeof(int);
			}
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x00151E26 File Offset: 0x00150026
		protected override Array CreateArray()
		{
			return new int[base.Count];
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x00151E33 File Offset: 0x00150033
		protected override object GetItemFromAddress(IntPtr addr)
		{
			return addr.ToInt32();
		}
	}
}

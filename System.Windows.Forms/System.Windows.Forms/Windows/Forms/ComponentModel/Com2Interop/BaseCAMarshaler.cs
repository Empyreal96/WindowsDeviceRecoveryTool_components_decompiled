using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200049F RID: 1183
	internal abstract class BaseCAMarshaler
	{
		// Token: 0x06005032 RID: 20530 RVA: 0x0014C790 File Offset: 0x0014A990
		protected BaseCAMarshaler(NativeMethods.CA_STRUCT caStruct)
		{
			if (caStruct == null)
			{
				this.count = 0;
			}
			this.count = caStruct.cElems;
			this.caArrayAddress = caStruct.pElems;
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x0014C7BC File Offset: 0x0014A9BC
		protected override void Finalize()
		{
			try
			{
				if (this.itemArray == null && this.caArrayAddress != IntPtr.Zero)
				{
					object[] items = this.Items;
				}
			}
			catch
			{
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x06005034 RID: 20532
		protected abstract Array CreateArray();

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06005035 RID: 20533
		public abstract Type ItemType { get; }

		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06005036 RID: 20534 RVA: 0x0014C814 File Offset: 0x0014AA14
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06005037 RID: 20535 RVA: 0x0014C81C File Offset: 0x0014AA1C
		public virtual object[] Items
		{
			get
			{
				try
				{
					if (this.itemArray == null)
					{
						this.itemArray = this.Get_Items();
					}
				}
				catch (Exception ex)
				{
				}
				return this.itemArray;
			}
		}

		// Token: 0x06005038 RID: 20536
		protected abstract object GetItemFromAddress(IntPtr addr);

		// Token: 0x06005039 RID: 20537 RVA: 0x0014C858 File Offset: 0x0014AA58
		private object[] Get_Items()
		{
			Array array = new object[this.Count];
			for (int i = 0; i < this.count; i++)
			{
				try
				{
					IntPtr addr = Marshal.ReadIntPtr(this.caArrayAddress, i * IntPtr.Size);
					object itemFromAddress = this.GetItemFromAddress(addr);
					if (itemFromAddress != null && this.ItemType.IsInstanceOfType(itemFromAddress))
					{
						array.SetValue(itemFromAddress, i);
					}
				}
				catch (Exception ex)
				{
				}
			}
			Marshal.FreeCoTaskMem(this.caArrayAddress);
			this.caArrayAddress = IntPtr.Zero;
			return (object[])array;
		}

		// Token: 0x04003406 RID: 13318
		private static TraceSwitch CAMarshalSwitch = new TraceSwitch("CAMarshal", "BaseCAMarshaler: Debug CA* struct marshaling");

		// Token: 0x04003407 RID: 13319
		private IntPtr caArrayAddress;

		// Token: 0x04003408 RID: 13320
		private int count;

		// Token: 0x04003409 RID: 13321
		private object[] itemArray;
	}
}

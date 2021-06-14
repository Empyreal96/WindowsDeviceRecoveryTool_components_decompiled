using System;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B3 RID: 179
	internal class ComThreadingInfo
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x000227C8 File Offset: 0x000209C8
		private ComThreadingInfo()
		{
			ComThreadingInfo.IComThreadingInfo comThreadingInfo = (ComThreadingInfo.IComThreadingInfo)ComThreadingInfo.CoGetObjectContext(ref this.IID_IUnknown);
			this.apartmentType = comThreadingInfo.GetCurrentApartmentType();
			this.threadType = comThreadingInfo.GetCurrentThreadType();
			this.logicalThreadId = comThreadingInfo.GetCurrentLogicalThreadId();
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00022820 File Offset: 0x00020A20
		public static ComThreadingInfo Current
		{
			get
			{
				return new ComThreadingInfo();
			}
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00022827 File Offset: 0x00020A27
		public override string ToString()
		{
			return string.Format("{{{0}}} - {1} - {2}", this.LogicalThreadId, this.ApartmentType, this.ThreadType);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00022854 File Offset: 0x00020A54
		public ComThreadingInfo.APTTYPE ApartmentType
		{
			get
			{
				return this.apartmentType;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0002285C File Offset: 0x00020A5C
		public ComThreadingInfo.THDTYPE ThreadType
		{
			get
			{
				return this.threadType;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00022864 File Offset: 0x00020A64
		public Guid LogicalThreadId
		{
			get
			{
				return this.logicalThreadId;
			}
		}

		// Token: 0x060004B6 RID: 1206
		[DllImport("ole32.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		private static extern object CoGetObjectContext([In] ref Guid riid);

		// Token: 0x040004EF RID: 1263
		private Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x040004F0 RID: 1264
		private ComThreadingInfo.APTTYPE apartmentType;

		// Token: 0x040004F1 RID: 1265
		private ComThreadingInfo.THDTYPE threadType;

		// Token: 0x040004F2 RID: 1266
		private Guid logicalThreadId;

		// Token: 0x02000107 RID: 263
		public enum APTTYPE
		{
			// Token: 0x04000566 RID: 1382
			APTTYPE_CURRENT = -1,
			// Token: 0x04000567 RID: 1383
			APTTYPE_STA,
			// Token: 0x04000568 RID: 1384
			APTTYPE_MTA,
			// Token: 0x04000569 RID: 1385
			APTTYPE_NA,
			// Token: 0x0400056A RID: 1386
			APTTYPE_MAINSTA
		}

		// Token: 0x02000108 RID: 264
		public enum THDTYPE
		{
			// Token: 0x0400056C RID: 1388
			THDTYPE_BLOCKMESSAGES,
			// Token: 0x0400056D RID: 1389
			THDTYPE_PROCESSMESSAGES
		}

		// Token: 0x02000109 RID: 265
		[Guid("000001ce-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface IComThreadingInfo
		{
			// Token: 0x06000671 RID: 1649
			ComThreadingInfo.APTTYPE GetCurrentApartmentType();

			// Token: 0x06000672 RID: 1650
			ComThreadingInfo.THDTYPE GetCurrentThreadType();

			// Token: 0x06000673 RID: 1651
			Guid GetCurrentLogicalThreadId();

			// Token: 0x06000674 RID: 1652
			void SetCurrentLogicalThreadId([In] Guid rguid);
		}
	}
}

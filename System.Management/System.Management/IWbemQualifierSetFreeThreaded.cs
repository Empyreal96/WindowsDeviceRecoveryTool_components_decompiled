using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000056 RID: 86
	internal sealed class IWbemQualifierSetFreeThreaded : IDisposable
	{
		// Token: 0x06000379 RID: 889 RVA: 0x00021583 File Offset: 0x0001F783
		public IWbemQualifierSetFreeThreaded(IntPtr pWbemQualifierSet)
		{
			this.pWbemQualifierSet = pWbemQualifierSet;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0002159D File Offset: 0x0001F79D
		public void Dispose()
		{
			this.Dispose_(false);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000215A6 File Offset: 0x0001F7A6
		private void Dispose_(bool finalization)
		{
			if (this.pWbemQualifierSet != IntPtr.Zero)
			{
				Marshal.Release(this.pWbemQualifierSet);
				this.pWbemQualifierSet = IntPtr.Zero;
			}
			if (!finalization)
			{
				GC.KeepAlive(this);
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000215E0 File Offset: 0x0001F7E0
		~IWbemQualifierSetFreeThreaded()
		{
			this.Dispose_(true);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00021610 File Offset: 0x0001F810
		public int Get_(string wszName, int lFlags, ref object pVal, ref int plFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierGet_f(3, this.pWbemQualifierSet, wszName, lFlags, ref pVal, ref plFlavor);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00021658 File Offset: 0x0001F858
		public int Put_(string wszName, ref object pVal, int lFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierPut_f(4, this.pWbemQualifierSet, wszName, ref pVal, lFlavor);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000216A0 File Offset: 0x0001F8A0
		public int Delete_(string wszName)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierDelete_f(5, this.pWbemQualifierSet, wszName);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000216E4 File Offset: 0x0001F8E4
		public int GetNames_(int lFlags, out string[] pNames)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierGetNames_f(6, this.pWbemQualifierSet, lFlags, out pNames);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0002172C File Offset: 0x0001F92C
		public int BeginEnumeration_(int lFlags)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierBeginEnumeration_f(7, this.pWbemQualifierSet, lFlags);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00021770 File Offset: 0x0001F970
		public int Next_(int lFlags, out string pstrName, out object pVal, out int plFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierNext_f(8, this.pWbemQualifierSet, lFlags, out pstrName, out pVal, out plFlavor);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000217B8 File Offset: 0x0001F9B8
		public int EndEnumeration_()
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int result = WmiNetUtilsHelper.QualifierEndEnumeration_f(9, this.pWbemQualifierSet);
			GC.KeepAlive(this);
			return result;
		}

		// Token: 0x04000248 RID: 584
		private const string SerializationBlobName = "flatWbemClassObject";

		// Token: 0x04000249 RID: 585
		private static readonly string name = typeof(IWbemQualifierSetFreeThreaded).FullName;

		// Token: 0x0400024A RID: 586
		public static Guid IID_IWbemClassObject = new Guid("DC12A681-737F-11CF-884D-00AA004B2E24");

		// Token: 0x0400024B RID: 587
		private IntPtr pWbemQualifierSet = IntPtr.Zero;
	}
}

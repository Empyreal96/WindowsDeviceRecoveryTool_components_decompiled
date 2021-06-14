using System;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066D RID: 1645
	internal class EncryptedPackageFilter : IFilter
	{
		// Token: 0x06006CC8 RID: 27848 RVA: 0x001F4E35 File Offset: 0x001F3035
		internal EncryptedPackageFilter(EncryptedPackageEnvelope encryptedPackage)
		{
			if (encryptedPackage == null)
			{
				throw new ArgumentNullException("encryptedPackage");
			}
			this._filter = new IndexingFilterMarshaler(new CorePropertiesFilter(encryptedPackage.PackageProperties));
		}

		// Token: 0x06006CC9 RID: 27849 RVA: 0x001F4E61 File Offset: 0x001F3061
		public IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06006CCA RID: 27850 RVA: 0x001F4E71 File Offset: 0x001F3071
		public STAT_CHUNK GetChunk()
		{
			return this._filter.GetChunk();
		}

		// Token: 0x06006CCB RID: 27851 RVA: 0x001F4E7E File Offset: 0x001F307E
		public void GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06006CCC RID: 27852 RVA: 0x001F4E94 File Offset: 0x001F3094
		public IntPtr GetValue()
		{
			return this._filter.GetValue();
		}

		// Token: 0x06006CCD RID: 27853 RVA: 0x001F22DB File Offset: 0x001F04DB
		public IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x0400356E RID: 13678
		private IFilter _filter;
	}
}

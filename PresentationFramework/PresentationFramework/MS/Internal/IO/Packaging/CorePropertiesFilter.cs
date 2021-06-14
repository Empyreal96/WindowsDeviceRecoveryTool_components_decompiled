using System;
using System.Globalization;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066E RID: 1646
	internal class CorePropertiesFilter : IManagedFilter
	{
		// Token: 0x06006CCE RID: 27854 RVA: 0x001F4EA1 File Offset: 0x001F30A1
		internal CorePropertiesFilter(PackageProperties coreProperties)
		{
			if (coreProperties == null)
			{
				throw new ArgumentNullException("coreProperties");
			}
			this._coreProperties = coreProperties;
		}

		// Token: 0x06006CCF RID: 27855 RVA: 0x001F4EBE File Offset: 0x001F30BE
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes)
		{
			this._grfFlags = grfFlags;
			this._aAttributes = aAttributes;
			this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
			return IFILTER_FLAGS.IFILTER_FLAGS_NONE;
		}

		// Token: 0x06006CD0 RID: 27856 RVA: 0x001F4EEC File Offset: 0x001F30EC
		public ManagedChunk GetChunk()
		{
			this._pendingGetValue = false;
			if (!this.CorePropertyEnumerator.MoveNext())
			{
				return null;
			}
			ManagedChunk result = new CorePropertiesFilter.PropertyChunk(this.AllocateChunkID(), this.CorePropertyEnumerator.CurrentGuid, this.CorePropertyEnumerator.CurrentPropId);
			this._pendingGetValue = true;
			return result;
		}

		// Token: 0x06006CD1 RID: 27857 RVA: 0x001F4E7E File Offset: 0x001F307E
		public string GetText(int bufferCharacterCount)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06006CD2 RID: 27858 RVA: 0x001F4F39 File Offset: 0x001F3139
		public object GetValue()
		{
			if (!this._pendingGetValue)
			{
				throw new COMException(SR.Get("FilterGetValueAlreadyCalledOnCurrentChunk"), -2147215614);
			}
			this._pendingGetValue = false;
			return this.CorePropertyEnumerator.CurrentValue;
		}

		// Token: 0x06006CD3 RID: 27859 RVA: 0x001F4F6A File Offset: 0x001F316A
		private uint AllocateChunkID()
		{
			if (this._chunkID == 4294967295U)
			{
				this._chunkID = 1U;
			}
			else
			{
				this._chunkID += 1U;
			}
			return this._chunkID;
		}

		// Token: 0x170019FF RID: 6655
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x001F4F92 File Offset: 0x001F3192
		private CorePropertyEnumerator CorePropertyEnumerator
		{
			get
			{
				if (this._corePropertyEnumerator == null)
				{
					this._corePropertyEnumerator = new CorePropertyEnumerator(this._coreProperties, this._grfFlags, this._aAttributes);
				}
				return this._corePropertyEnumerator;
			}
		}

		// Token: 0x0400356F RID: 13679
		private IFILTER_INIT _grfFlags;

		// Token: 0x04003570 RID: 13680
		private ManagedFullPropSpec[] _aAttributes;

		// Token: 0x04003571 RID: 13681
		private uint _chunkID;

		// Token: 0x04003572 RID: 13682
		private bool _pendingGetValue;

		// Token: 0x04003573 RID: 13683
		private CorePropertyEnumerator _corePropertyEnumerator;

		// Token: 0x04003574 RID: 13684
		private PackageProperties _coreProperties;

		// Token: 0x02000B20 RID: 2848
		private class PropertyChunk : ManagedChunk
		{
			// Token: 0x06008D2E RID: 36142 RVA: 0x00258DBE File Offset: 0x00256FBE
			internal PropertyChunk(uint chunkId, Guid guid, uint propId) : base(chunkId, CHUNK_BREAKTYPE.CHUNK_EOS, new ManagedFullPropSpec(guid, propId), (uint)CultureInfo.InvariantCulture.LCID, CHUNKSTATE.CHUNK_VALUE)
			{
			}
		}
	}
}

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000661 RID: 1633
	internal class IndexingFilterMarshaler : IFilter
	{
		// Token: 0x06006C32 RID: 27698 RVA: 0x001F1F23 File Offset: 0x001F0123
		internal IndexingFilterMarshaler(IManagedFilter managedFilter)
		{
			if (managedFilter == null)
			{
				throw new ArgumentNullException("managedFilter");
			}
			this._implementation = managedFilter;
		}

		// Token: 0x06006C33 RID: 27699 RVA: 0x001F1F48 File Offset: 0x001F0148
		internal static ManagedFullPropSpec[] MarshalFullPropSpecArray(uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			if (cAttributes > 0U)
			{
				Invariant.Assert(aAttributes != null);
				ManagedFullPropSpec[] array = new ManagedFullPropSpec[checked((int)cAttributes)];
				int num = 0;
				while ((long)num < (long)((ulong)cAttributes))
				{
					array[num] = new ManagedFullPropSpec(aAttributes[num]);
					num++;
				}
				return array;
			}
			return null;
		}

		// Token: 0x06006C34 RID: 27700 RVA: 0x001F1F8C File Offset: 0x001F018C
		[SecurityCritical]
		internal static void MarshalStringToPtr(string s, ref uint bufCharacterCount, IntPtr p)
		{
			Invariant.Assert(bufCharacterCount > 0U);
			if (s.Length > (int)(bufCharacterCount - 1U))
			{
				throw new InvalidOperationException(SR.Get("FilterGetTextBufferOverflow"));
			}
			bufCharacterCount = (uint)(s.Length + 1);
			Marshal.Copy(s.ToCharArray(), 0, p, s.Length);
			Marshal.WriteInt16(p, s.Length * 2, 0);
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x001F1FEC File Offset: 0x001F01EC
		[SecurityCritical]
		internal static void MarshalPropSpec(ManagedPropSpec propSpec, ref PROPSPEC native)
		{
			native.propType = (uint)propSpec.PropType;
			PropSpecType propType = propSpec.PropType;
			if (propType == PropSpecType.Name)
			{
				native.union.name = Marshal.StringToCoTaskMemUni(propSpec.PropName);
				return;
			}
			if (propType == PropSpecType.Id)
			{
				native.union.propId = propSpec.PropId;
				return;
			}
			Invariant.Assert(false);
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x001F2042 File Offset: 0x001F0242
		[SecurityCritical]
		internal static void MarshalFullPropSpec(ManagedFullPropSpec fullPropSpec, ref FULLPROPSPEC native)
		{
			native.guid = fullPropSpec.Guid;
			IndexingFilterMarshaler.MarshalPropSpec(fullPropSpec.Property, ref native.property);
		}

		// Token: 0x06006C37 RID: 27703 RVA: 0x001F2064 File Offset: 0x001F0264
		[SecurityCritical]
		internal static STAT_CHUNK MarshalChunk(ManagedChunk chunk)
		{
			STAT_CHUNK result = default(STAT_CHUNK);
			result.idChunk = chunk.ID;
			Invariant.Assert(chunk.BreakType >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && chunk.BreakType <= CHUNK_BREAKTYPE.CHUNK_EOC);
			result.breakType = chunk.BreakType;
			Invariant.Assert(chunk.Flags >= (CHUNKSTATE)0 && chunk.Flags <= (CHUNKSTATE.CHUNK_TEXT | CHUNKSTATE.CHUNK_VALUE | CHUNKSTATE.CHUNK_FILTER_OWNED_VALUE));
			result.flags = chunk.Flags;
			result.locale = chunk.Locale;
			result.idChunkSource = chunk.ChunkSource;
			result.cwcStartSource = chunk.StartSource;
			result.cwcLenSource = chunk.LenSource;
			IndexingFilterMarshaler.MarshalFullPropSpec(chunk.Attribute, ref result.attribute);
			return result;
		}

		// Token: 0x06006C38 RID: 27704 RVA: 0x001F2124 File Offset: 0x001F0324
		[SecurityCritical]
		internal static IntPtr MarshalPropVariant(object obj)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				PROPVARIANT propvariant;
				if (obj is string)
				{
					intPtr = Marshal.StringToCoTaskMemAnsi((string)obj);
					propvariant = default(PROPVARIANT);
					propvariant.vt = VARTYPE.VT_LPSTR;
					propvariant.union.pszVal = intPtr;
				}
				else
				{
					if (!(obj is DateTime))
					{
						throw new InvalidOperationException(SR.Get("FilterGetValueMustBeStringOrDateTime"));
					}
					propvariant = default(PROPVARIANT);
					propvariant.vt = VARTYPE.VT_FILETIME;
					long num = ((DateTime)obj).ToFileTime();
					propvariant.union.filetime.dwLowDateTime = (int)num;
					propvariant.union.filetime.dwHighDateTime = (int)(num >> 32 & (long)((ulong)-1));
				}
				intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(PROPVARIANT)));
				Invariant.Assert(true);
				Marshal.StructureToPtr(propvariant, intPtr2, false);
			}
			catch
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(intPtr2);
				}
				throw;
			}
			return intPtr2;
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x001F223C File Offset: 0x001F043C
		public IFILTER_FLAGS Init(IFILTER_INIT grfFlags, uint cAttributes, FULLPROPSPEC[] aAttributes)
		{
			ManagedFullPropSpec[] aAttributes2 = IndexingFilterMarshaler.MarshalFullPropSpecArray(cAttributes, aAttributes);
			return this._implementation.Init(grfFlags, aAttributes2);
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x001F2260 File Offset: 0x001F0460
		[SecurityCritical]
		public STAT_CHUNK GetChunk()
		{
			ManagedChunk chunk = this._implementation.GetChunk();
			if (chunk != null)
			{
				return IndexingFilterMarshaler.MarshalChunk(chunk);
			}
			if (this.ThrowOnEndOfChunks)
			{
				throw new COMException(SR.Get("FilterEndOfChunks"), -2147215616);
			}
			return new STAT_CHUNK
			{
				idChunk = 0U
			};
		}

		// Token: 0x06006C3B RID: 27707 RVA: 0x001F22B1 File Offset: 0x001F04B1
		[SecurityCritical]
		public void GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			IndexingFilterMarshaler.MarshalStringToPtr(this._implementation.GetText((int)(bufCharacterCount - 1U)), ref bufCharacterCount, pBuffer);
		}

		// Token: 0x06006C3C RID: 27708 RVA: 0x001F22C9 File Offset: 0x001F04C9
		[SecurityCritical]
		public IntPtr GetValue()
		{
			return IndexingFilterMarshaler.MarshalPropVariant(this._implementation.GetValue());
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x001F22DB File Offset: 0x001F04DB
		public IntPtr BindRegion(FILTERREGION origPos, ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x170019E1 RID: 6625
		// (get) Token: 0x06006C3E RID: 27710 RVA: 0x001F22EC File Offset: 0x001F04EC
		// (set) Token: 0x06006C3F RID: 27711 RVA: 0x001F22F4 File Offset: 0x001F04F4
		internal bool ThrowOnEndOfChunks
		{
			get
			{
				return this._throwOnEndOfChunks;
			}
			set
			{
				this._throwOnEndOfChunks = value;
			}
		}

		// Token: 0x0400351A RID: 13594
		internal static Guid PSGUID_STORAGE = new Guid(3072717104U, 18415, 4122, 165, 241, 2, 96, 140, 158, 235, 172);

		// Token: 0x0400351B RID: 13595
		internal const int _int16Size = 2;

		// Token: 0x0400351C RID: 13596
		private IManagedFilter _implementation;

		// Token: 0x0400351D RID: 13597
		private bool _throwOnEndOfChunks = true;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Media.TextFormatting;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000398 RID: 920
	internal class NaturalLanguageHyphenator : TextLexicalService, IDisposable
	{
		// Token: 0x060031DC RID: 12764 RVA: 0x000DC1B8 File Offset: 0x000DA3B8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal NaturalLanguageHyphenator()
		{
			try
			{
				this._hyphenatorResource = NaturalLanguageHyphenator.UnsafeNativeMethods.NlCreateHyphenator();
			}
			catch (DllNotFoundException)
			{
			}
			catch (EntryPointNotFoundException)
			{
			}
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000DC1FC File Offset: 0x000DA3FC
		~NaturalLanguageHyphenator()
		{
			this.CleanupInternal(true);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000DC22C File Offset: 0x000DA42C
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.CleanupInternal(false);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000DC23B File Offset: 0x000DA43B
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void CleanupInternal(bool finalizing)
		{
			if (!this._disposed && this._hyphenatorResource != IntPtr.Zero)
			{
				NaturalLanguageHyphenator.UnsafeNativeMethods.NlDestroyHyphenator(ref this._hyphenatorResource);
				this._disposed = true;
			}
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsCultureSupported(CultureInfo culture)
		{
			return true;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000DC26C File Offset: 0x000DA46C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override TextLexicalBreaks AnalyzeText(char[] characterSource, int length, CultureInfo textCulture)
		{
			Invariant.Assert(characterSource != null && characterSource.Length != 0 && length > 0 && length <= characterSource.Length);
			if (this._hyphenatorResource == IntPtr.Zero)
			{
				return null;
			}
			if (this._disposed)
			{
				throw new ObjectDisposedException(SR.Get("HyphenatorDisposed"));
			}
			byte[] array = new byte[(length + 7) / 8];
			NaturalLanguageHyphenator.UnsafeNativeMethods.NlHyphenate(this._hyphenatorResource, characterSource, length, (textCulture != null && textCulture != CultureInfo.InvariantCulture) ? textCulture.LCID : 0, array, array.Length);
			return new NaturalLanguageHyphenator.HyphenBreaks(array, length);
		}

		// Token: 0x04001EA6 RID: 7846
		[SecurityCritical]
		private IntPtr _hyphenatorResource;

		// Token: 0x04001EA7 RID: 7847
		private bool _disposed;

		// Token: 0x020008D8 RID: 2264
		private class HyphenBreaks : TextLexicalBreaks
		{
			// Token: 0x060084A4 RID: 33956 RVA: 0x0024947B File Offset: 0x0024767B
			internal HyphenBreaks(byte[] isHyphenPositions, int numPositions)
			{
				this._isHyphenPositions = isHyphenPositions;
				this._numPositions = numPositions;
			}

			// Token: 0x17001E07 RID: 7687
			private bool this[int index]
			{
				get
				{
					return ((int)this._isHyphenPositions[index / 8] & 1 << index % 8) != 0;
				}
			}

			// Token: 0x17001E08 RID: 7688
			// (get) Token: 0x060084A6 RID: 33958 RVA: 0x002494A9 File Offset: 0x002476A9
			public override int Length
			{
				get
				{
					return this._numPositions;
				}
			}

			// Token: 0x060084A7 RID: 33959 RVA: 0x002494B4 File Offset: 0x002476B4
			public override int GetNextBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex >= 0)
				{
					int num = currentIndex + 1;
					while (num < this._numPositions && !this[num])
					{
						num++;
					}
					if (num < this._numPositions)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x060084A8 RID: 33960 RVA: 0x002494F8 File Offset: 0x002476F8
			public override int GetPreviousBreak(int currentIndex)
			{
				if (this._isHyphenPositions != null && currentIndex < this._numPositions)
				{
					int num = currentIndex;
					while (num > 0 && !this[num])
					{
						num--;
					}
					if (num > 0)
					{
						return num;
					}
				}
				return -1;
			}

			// Token: 0x04004289 RID: 17033
			private byte[] _isHyphenPositions;

			// Token: 0x0400428A RID: 17034
			private int _numPositions;
		}

		// Token: 0x020008D9 RID: 2265
		private static class UnsafeNativeMethods
		{
			// Token: 0x060084A9 RID: 33961
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern IntPtr NlCreateHyphenator();

			// Token: 0x060084AA RID: 33962
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll")]
			internal static extern void NlDestroyHyphenator(ref IntPtr hyphenator);

			// Token: 0x060084AB RID: 33963
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern void NlHyphenate(IntPtr hyphenator, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U2, SizeParamIndex = 2)] [In] char[] inputText, int textLength, int localeID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)] [In] byte[] hyphenBreaks, int numPositions);
		}
	}
}

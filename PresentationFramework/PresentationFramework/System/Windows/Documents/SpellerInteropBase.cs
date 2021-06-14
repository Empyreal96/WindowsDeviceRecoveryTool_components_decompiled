using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x020003DC RID: 988
	internal abstract class SpellerInteropBase : IDisposable
	{
		// Token: 0x06003580 RID: 13696
		public abstract void Dispose();

		// Token: 0x06003581 RID: 13697
		protected abstract void Dispose(bool disposing);

		// Token: 0x06003582 RID: 13698 RVA: 0x000F302C File Offset: 0x000F122C
		[SecuritySafeCritical]
		public static SpellerInteropBase CreateInstance()
		{
			SpellerInteropBase result = null;
			bool flag = false;
			try
			{
				result = new WinRTSpellerInterop();
				flag = true;
			}
			catch (PlatformNotSupportedException)
			{
				flag = false;
			}
			catch (NotSupportedException)
			{
				flag = true;
			}
			if (!flag)
			{
				try
				{
					result = new NLGSpellerInterop();
				}
				catch (Exception ex) when (ex is DllNotFoundException || ex is EntryPointNotFoundException)
				{
					return null;
				}
				return result;
			}
			return result;
		}

		// Token: 0x06003583 RID: 13699
		[SecurityCritical]
		internal abstract void SetLocale(CultureInfo culture);

		// Token: 0x06003584 RID: 13700
		[SecurityCritical]
		internal abstract int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data);

		// Token: 0x06003585 RID: 13701
		[SecurityCritical]
		internal abstract void UnloadDictionary(object dictionary);

		// Token: 0x06003586 RID: 13702
		[SecurityCritical]
		internal abstract object LoadDictionary(string lexiconFilePath);

		// Token: 0x06003587 RID: 13703
		[SecurityCritical]
		internal abstract object LoadDictionary(Uri item, string trustedFolder);

		// Token: 0x06003588 RID: 13704
		[SecurityCritical]
		internal abstract void ReleaseAllLexicons();

		// Token: 0x17000DB3 RID: 3507
		// (set) Token: 0x06003589 RID: 13705
		internal abstract SpellerInteropBase.SpellerMode Mode { [SecuritySafeCritical] set; }

		// Token: 0x17000DB4 RID: 3508
		// (set) Token: 0x0600358A RID: 13706
		internal abstract bool MultiWordMode { [SecurityCritical] set; }

		// Token: 0x0600358B RID: 13707
		[SecurityCritical]
		internal abstract void SetReformMode(CultureInfo culture, SpellingReform spellingReform);

		// Token: 0x0600358C RID: 13708
		internal abstract bool CanSpellCheck(CultureInfo culture);

		// Token: 0x020008E4 RID: 2276
		// (Invoke) Token: 0x060084C7 RID: 33991
		internal delegate bool EnumSentencesCallback(SpellerInteropBase.ISpellerSentence sentence, object data);

		// Token: 0x020008E5 RID: 2277
		// (Invoke) Token: 0x060084CB RID: 33995
		internal delegate bool EnumTextSegmentsCallback(SpellerInteropBase.ISpellerSegment textSegment, object data);

		// Token: 0x020008E6 RID: 2278
		internal interface ITextRange
		{
			// Token: 0x17001E17 RID: 7703
			// (get) Token: 0x060084CE RID: 33998
			int Start { get; }

			// Token: 0x17001E18 RID: 7704
			// (get) Token: 0x060084CF RID: 33999
			int Length { get; }
		}

		// Token: 0x020008E7 RID: 2279
		internal interface ISpellerSegment
		{
			// Token: 0x17001E19 RID: 7705
			// (get) Token: 0x060084D0 RID: 34000
			string SourceString { get; }

			// Token: 0x17001E1A RID: 7706
			// (get) Token: 0x060084D1 RID: 34001
			IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments { get; }

			// Token: 0x17001E1B RID: 7707
			// (get) Token: 0x060084D2 RID: 34002
			SpellerInteropBase.ITextRange TextRange { get; }

			// Token: 0x17001E1C RID: 7708
			// (get) Token: 0x060084D3 RID: 34003
			string Text { get; }

			// Token: 0x17001E1D RID: 7709
			// (get) Token: 0x060084D4 RID: 34004
			IReadOnlyList<string> Suggestions { get; }

			// Token: 0x17001E1E RID: 7710
			// (get) Token: 0x060084D5 RID: 34005
			bool IsClean { get; }

			// Token: 0x060084D6 RID: 34006
			void EnumSubSegments(SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data);
		}

		// Token: 0x020008E8 RID: 2280
		internal interface ISpellerSentence
		{
			// Token: 0x17001E1F RID: 7711
			// (get) Token: 0x060084D7 RID: 34007
			IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments { get; }

			// Token: 0x17001E20 RID: 7712
			// (get) Token: 0x060084D8 RID: 34008
			int EndOffset { get; }
		}

		// Token: 0x020008E9 RID: 2281
		[Flags]
		internal enum SpellerMode
		{
			// Token: 0x040042AF RID: 17071
			None = 0,
			// Token: 0x040042B0 RID: 17072
			WordBreaking = 1,
			// Token: 0x040042B1 RID: 17073
			SpellingErrors = 2,
			// Token: 0x040042B2 RID: 17074
			Suggestions = 4,
			// Token: 0x040042B3 RID: 17075
			SpellingErrorsWithSuggestions = 6,
			// Token: 0x040042B4 RID: 17076
			All = 7
		}
	}
}

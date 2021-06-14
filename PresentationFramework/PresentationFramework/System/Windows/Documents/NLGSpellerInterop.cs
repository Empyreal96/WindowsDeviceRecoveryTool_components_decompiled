using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Controls;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020003DD RID: 989
	internal class NLGSpellerInterop : SpellerInteropBase
	{
		// Token: 0x0600358E RID: 13710 RVA: 0x000F30B4 File Offset: 0x000F12B4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal NLGSpellerInterop()
		{
			NLGSpellerInterop.UnsafeNlMethods.NlLoad();
			bool flag = true;
			try
			{
				this._textChunk = NLGSpellerInterop.CreateTextChunk();
				NLGSpellerInterop.ITextContext textContext = NLGSpellerInterop.CreateTextContext();
				try
				{
					this._textChunk.put_Context(textContext);
				}
				finally
				{
					Marshal.ReleaseComObject(textContext);
				}
				this._textChunk.put_ReuseObjects(true);
				this.Mode = SpellerInteropBase.SpellerMode.None;
				this.MultiWordMode = false;
				flag = false;
			}
			finally
			{
				if (flag)
				{
					if (this._textChunk != null)
					{
						Marshal.ReleaseComObject(this._textChunk);
						this._textChunk = null;
					}
					NLGSpellerInterop.UnsafeNlMethods.NlUnload();
				}
			}
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000F3154 File Offset: 0x000F1354
		~NLGSpellerInterop()
		{
			this.Dispose(false);
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000F3184 File Offset: 0x000F1384
		public override void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000F3194 File Offset: 0x000F1394
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void Dispose(bool disposing)
		{
			if (this._isDisposed)
			{
				throw new ObjectDisposedException(SR.Get("TextEditorSpellerInteropHasBeenDisposed"));
			}
			if (this._textChunk != null)
			{
				Marshal.ReleaseComObject(this._textChunk);
				this._textChunk = null;
			}
			NLGSpellerInterop.UnsafeNlMethods.NlUnload();
			this._isDisposed = true;
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x000F31E0 File Offset: 0x000F13E0
		[SecurityCritical]
		internal override void SetLocale(CultureInfo culture)
		{
			this._textChunk.put_Locale(culture.LCID);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x000F31F4 File Offset: 0x000F13F4
		[SecurityCritical]
		private void SetContextOption(string option, object value)
		{
			NLGSpellerInterop.ITextContext textContext;
			this._textChunk.get_Context(out textContext);
			if (textContext != null)
			{
				try
				{
					NLGSpellerInterop.IProcessingOptions processingOptions;
					textContext.get_Options(out processingOptions);
					if (processingOptions != null)
					{
						try
						{
							processingOptions.put_Item(option, value);
						}
						finally
						{
							Marshal.ReleaseComObject(processingOptions);
						}
					}
				}
				finally
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x000F3254 File Offset: 0x000F1454
		[SecurityCritical]
		internal override int EnumTextSegments(char[] text, int count, SpellerInteropBase.EnumSentencesCallback sentenceCallback, SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
		{
			int num = 0;
			IntPtr intPtr = Marshal.AllocHGlobal(count * 2);
			try
			{
				Marshal.Copy(text, 0, intPtr, count);
				this._textChunk.SetInputArray(intPtr, count);
				UnsafeNativeMethods.IEnumVariant enumVariant;
				this._textChunk.GetEnumerator(out enumVariant);
				try
				{
					NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
					int[] array = new int[1];
					bool flag = true;
					enumVariant.Reset();
					do
					{
						variant.Clear();
						if (NLGSpellerInterop.EnumVariantNext(enumVariant, variant, array) != 0 || array[0] == 0)
						{
							break;
						}
						using (NLGSpellerInterop.SpellerSentence spellerSentence = new NLGSpellerInterop.SpellerSentence((NLGSpellerInterop.ISentence)variant.ToObject()))
						{
							num += spellerSentence.Segments.Count;
							if (segmentCallback != null)
							{
								int num2 = 0;
								while (flag && num2 < spellerSentence.Segments.Count)
								{
									flag = segmentCallback(spellerSentence.Segments[num2], data);
									num2++;
								}
							}
							if (sentenceCallback != null)
							{
								flag = sentenceCallback(spellerSentence, data);
							}
						}
					}
					while (flag);
					variant.Clear();
				}
				finally
				{
					Marshal.ReleaseComObject(enumVariant);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return num;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x000F3388 File Offset: 0x000F1588
		[SecurityCritical]
		internal override void UnloadDictionary(object dictionary)
		{
			NLGSpellerInterop.ILexicon lexicon = dictionary as NLGSpellerInterop.ILexicon;
			Invariant.Assert(lexicon != null);
			NLGSpellerInterop.ITextContext textContext = null;
			try
			{
				this._textChunk.get_Context(out textContext);
				textContext.RemoveLexicon(lexicon);
			}
			finally
			{
				Marshal.ReleaseComObject(lexicon);
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x000F33E0 File Offset: 0x000F15E0
		[SecurityCritical]
		internal override object LoadDictionary(string lexiconFilePath)
		{
			return this.AddLexicon(lexiconFilePath);
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x000F33EC File Offset: 0x000F15EC
		[SecurityCritical]
		internal override object LoadDictionary(Uri item, string trustedFolder)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, trustedFolder).Assert();
			object result;
			try
			{
				result = this.LoadDictionary(item.LocalPath);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x000F342C File Offset: 0x000F162C
		[SecurityCritical]
		internal override void ReleaseAllLexicons()
		{
			NLGSpellerInterop.ITextContext textContext = null;
			try
			{
				this._textChunk.get_Context(out textContext);
				int i = 0;
				textContext.get_LexiconCount(out i);
				while (i > 0)
				{
					NLGSpellerInterop.ILexicon lexicon = null;
					textContext.get_Lexicon(0, out lexicon);
					textContext.RemoveLexicon(lexicon);
					Marshal.ReleaseComObject(lexicon);
					i--;
				}
			}
			finally
			{
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
		}

		// Token: 0x17000DB5 RID: 3509
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x000F3494 File Offset: 0x000F1694
		internal override SpellerInteropBase.SpellerMode Mode
		{
			[SecurityCritical]
			set
			{
				this._mode = value;
				if (!this._mode.HasFlag(SpellerInteropBase.SpellerMode.SpellingErrors))
				{
					if (this._mode.HasFlag(SpellerInteropBase.SpellerMode.WordBreaking))
					{
						this.SetContextOption("IsSpellChecking", false);
					}
					return;
				}
				this.SetContextOption("IsSpellChecking", true);
				if (this._mode.HasFlag(SpellerInteropBase.SpellerMode.Suggestions))
				{
					this.SetContextOption("IsSpellVerifyOnly", false);
					return;
				}
				this.SetContextOption("IsSpellVerifyOnly", true);
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (set) Token: 0x0600359A RID: 13722 RVA: 0x000F3536 File Offset: 0x000F1736
		internal override bool MultiWordMode
		{
			[SecurityCritical]
			set
			{
				this._multiWordMode = value;
				this.SetContextOption("IsSpellSuggestingMWEs", this._multiWordMode);
			}
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x000F3558 File Offset: 0x000F1758
		[SecurityCritical]
		internal override void SetReformMode(CultureInfo culture, SpellingReform spellingReform)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			string text;
			if (!(twoLetterISOLanguageName == "de"))
			{
				if (!(twoLetterISOLanguageName == "fr"))
				{
					text = null;
				}
				else
				{
					text = "FrenchReform";
				}
			}
			else
			{
				text = "GermanReform";
			}
			if (text != null)
			{
				switch (spellingReform)
				{
				case SpellingReform.PreAndPostreform:
					if (text == "GermanReform")
					{
						this.SetContextOption(text, 2);
						return;
					}
					this.SetContextOption(text, 0);
					break;
				case SpellingReform.Prereform:
					this.SetContextOption(text, 1);
					return;
				case SpellingReform.Postreform:
					this.SetContextOption(text, 2);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000F35F4 File Offset: 0x000F17F4
		internal override bool CanSpellCheck(CultureInfo culture)
		{
			string twoLetterISOLanguageName = culture.TwoLetterISOLanguageName;
			return twoLetterISOLanguageName == "en" || twoLetterISOLanguageName == "de" || twoLetterISOLanguageName == "fr" || twoLetterISOLanguageName == "es";
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x000F3644 File Offset: 0x000F1844
		[SecurityCritical]
		private NLGSpellerInterop.ILexicon AddLexicon(string lexiconFilePath)
		{
			NLGSpellerInterop.ITextContext textContext = null;
			NLGSpellerInterop.ILexicon lexicon = null;
			bool flag = true;
			bool flag2 = false;
			try
			{
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, lexiconFilePath);
				fileIOPermission.Demand();
				flag2 = true;
				lexicon = NLGSpellerInterop.CreateLexicon();
				lexicon.ReadFrom(lexiconFilePath);
				this._textChunk.get_Context(out textContext);
				textContext.AddLexicon(lexicon);
				flag = false;
			}
			catch (Exception innerException)
			{
				if (flag2)
				{
					throw new ArgumentException(SR.Get("CustomDictionaryFailedToLoadDictionaryUri", new object[]
					{
						lexiconFilePath
					}), innerException);
				}
				throw;
			}
			finally
			{
				if (flag && lexicon != null)
				{
					Marshal.ReleaseComObject(lexicon);
				}
				if (textContext != null)
				{
					Marshal.ReleaseComObject(textContext);
				}
			}
			return lexicon;
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000F36E8 File Offset: 0x000F18E8
		[SecurityCritical]
		private static object CreateInstance(Guid clsid, Guid iid)
		{
			object result;
			NLGSpellerInterop.UnsafeNlMethods.NlGetClassObject(ref clsid, ref iid, out result);
			return result;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000F3701 File Offset: 0x000F1901
		[SecurityCritical]
		private static NLGSpellerInterop.ITextContext CreateTextContext()
		{
			return (NLGSpellerInterop.ITextContext)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_ITextContext, NLGSpellerInterop.IID_ITextContext);
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000F3717 File Offset: 0x000F1917
		[SecurityCritical]
		private static NLGSpellerInterop.ITextChunk CreateTextChunk()
		{
			return (NLGSpellerInterop.ITextChunk)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_ITextChunk, NLGSpellerInterop.IID_ITextChunk);
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000F372D File Offset: 0x000F192D
		[SecurityCritical]
		private static NLGSpellerInterop.ILexicon CreateLexicon()
		{
			return (NLGSpellerInterop.ILexicon)NLGSpellerInterop.CreateInstance(NLGSpellerInterop.CLSID_Lexicon, NLGSpellerInterop.IID_ILexicon);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000F3744 File Offset: 0x000F1944
		[SecurityCritical]
		private unsafe static int EnumVariantNext(UnsafeNativeMethods.IEnumVariant variantEnumerator, NativeMethods.VARIANT variant, int[] fetched)
		{
			int result;
			fixed (IntPtr* ptr = (IntPtr*)(&variant.vt))
			{
				result = variantEnumerator.Next(1, (IntPtr)((void*)ptr), fetched);
			}
			return result;
		}

		// Token: 0x0400251D RID: 9501
		[SecurityCritical]
		private NLGSpellerInterop.ITextChunk _textChunk;

		// Token: 0x0400251E RID: 9502
		private bool _isDisposed;

		// Token: 0x0400251F RID: 9503
		private SpellerInteropBase.SpellerMode _mode;

		// Token: 0x04002520 RID: 9504
		private bool _multiWordMode;

		// Token: 0x04002521 RID: 9505
		private static readonly Guid CLSID_ITextContext = new Guid(859728164, 17235, 18740, 167, 190, 95, 181, 189, 221, 178, 214);

		// Token: 0x04002522 RID: 9506
		private static readonly Guid IID_ITextContext = new Guid(3061415104U, 4526, 16455, 164, 56, 38, 192, 201, 22, 235, 141);

		// Token: 0x04002523 RID: 9507
		private static readonly Guid CLSID_ITextChunk = new Guid(2313837402U, 53276, 17760, 168, 116, 159, 201, 42, 251, 14, 250);

		// Token: 0x04002524 RID: 9508
		private static readonly Guid IID_ITextChunk = new Guid(1419745662, 3779, 17364, 180, 67, 43, 248, 2, 16, 16, 207);

		// Token: 0x04002525 RID: 9509
		private static readonly Guid CLSID_Lexicon = new Guid("D385FDAD-D394-4812-9CEC-C6575C0B2B38");

		// Token: 0x04002526 RID: 9510
		private static readonly Guid IID_ILexicon = new Guid("004CD7E2-8B63-4ef9-8D46-080CDBBE47AF");

		// Token: 0x020008EA RID: 2282
		private struct STextRange : SpellerInteropBase.ITextRange
		{
			// Token: 0x17001E21 RID: 7713
			// (get) Token: 0x060084D9 RID: 34009 RVA: 0x00249B33 File Offset: 0x00247D33
			public int Start
			{
				get
				{
					return this._start;
				}
			}

			// Token: 0x17001E22 RID: 7714
			// (get) Token: 0x060084DA RID: 34010 RVA: 0x00249B3B File Offset: 0x00247D3B
			public int Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x040042B5 RID: 17077
			private readonly int _start;

			// Token: 0x040042B6 RID: 17078
			private readonly int _length;
		}

		// Token: 0x020008EB RID: 2283
		private enum RangeRole
		{
			// Token: 0x040042B8 RID: 17080
			ecrrSimpleSegment,
			// Token: 0x040042B9 RID: 17081
			ecrrAlternativeForm,
			// Token: 0x040042BA RID: 17082
			ecrrIncorrect,
			// Token: 0x040042BB RID: 17083
			ecrrAutoReplaceForm,
			// Token: 0x040042BC RID: 17084
			ecrrCorrectForm,
			// Token: 0x040042BD RID: 17085
			ecrrPreferredForm,
			// Token: 0x040042BE RID: 17086
			ecrrNormalizedForm,
			// Token: 0x040042BF RID: 17087
			ecrrCompoundSegment,
			// Token: 0x040042C0 RID: 17088
			ecrrPhraseSegment,
			// Token: 0x040042C1 RID: 17089
			ecrrNamedEntity,
			// Token: 0x040042C2 RID: 17090
			ecrrCompoundWord,
			// Token: 0x040042C3 RID: 17091
			ecrrPhrase,
			// Token: 0x040042C4 RID: 17092
			ecrrUnknownWord,
			// Token: 0x040042C5 RID: 17093
			ecrrContraction,
			// Token: 0x040042C6 RID: 17094
			ecrrHyphenatedWord,
			// Token: 0x040042C7 RID: 17095
			ecrrContractionSegment,
			// Token: 0x040042C8 RID: 17096
			ecrrHyphenatedSegment,
			// Token: 0x040042C9 RID: 17097
			ecrrCapitalization,
			// Token: 0x040042CA RID: 17098
			ecrrAccent,
			// Token: 0x040042CB RID: 17099
			ecrrRepeated,
			// Token: 0x040042CC RID: 17100
			ecrrDefinition,
			// Token: 0x040042CD RID: 17101
			ecrrOutOfContext
		}

		// Token: 0x020008EC RID: 2284
		private class SpellerSegment : SpellerInteropBase.ISpellerSegment, IDisposable
		{
			// Token: 0x060084DB RID: 34011 RVA: 0x00249B43 File Offset: 0x00247D43
			public SpellerSegment(NLGSpellerInterop.ITextSegment textSegment)
			{
				this._textSegment = textSegment;
			}

			// Token: 0x060084DC RID: 34012 RVA: 0x00249B54 File Offset: 0x00247D54
			[SecurityCritical]
			private void EnumerateSuggestions()
			{
				List<string> list = new List<string>();
				UnsafeNativeMethods.IEnumVariant enumVariant;
				this._textSegment.get_Suggestions(out enumVariant);
				if (enumVariant == null)
				{
					this._suggestions = list.AsReadOnly();
					return;
				}
				try
				{
					NativeMethods.VARIANT variant = new NativeMethods.VARIANT();
					int[] array = new int[1];
					for (;;)
					{
						variant.Clear();
						if (NLGSpellerInterop.EnumVariantNext(enumVariant, variant, array) != 0)
						{
							break;
						}
						if (array[0] == 0)
						{
							break;
						}
						list.Add(Marshal.PtrToStringUni(variant.data1.Value));
					}
				}
				finally
				{
					Marshal.ReleaseComObject(enumVariant);
				}
				this._suggestions = list.AsReadOnly();
			}

			// Token: 0x060084DD RID: 34013 RVA: 0x00249BEC File Offset: 0x00247DEC
			[SecurityCritical]
			private void EnumerateSubSegments()
			{
				this._textSegment.get_Count(out this._subSegmentCount);
				List<SpellerInteropBase.ISpellerSegment> list = new List<SpellerInteropBase.ISpellerSegment>();
				for (int i = 0; i < this._subSegmentCount; i++)
				{
					NLGSpellerInterop.ITextSegment textSegment;
					this._textSegment.get_Item(i, out textSegment);
					list.Add(new NLGSpellerInterop.SpellerSegment(textSegment));
				}
				this._subSegments = list.AsReadOnly();
			}

			// Token: 0x17001E23 RID: 7715
			// (get) Token: 0x060084DE RID: 34014 RVA: 0x00249C47 File Offset: 0x00247E47
			public string SourceString { get; }

			// Token: 0x17001E24 RID: 7716
			// (get) Token: 0x060084DF RID: 34015 RVA: 0x00249C4F File Offset: 0x00247E4F
			public string Text
			{
				get
				{
					string sourceString = this.SourceString;
					if (sourceString == null)
					{
						return null;
					}
					return sourceString.Substring(this.TextRange.Start, this.TextRange.Length);
				}
			}

			// Token: 0x17001E25 RID: 7717
			// (get) Token: 0x060084E0 RID: 34016 RVA: 0x00249C78 File Offset: 0x00247E78
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> SubSegments
			{
				[SecuritySafeCritical]
				get
				{
					if (this._subSegments == null)
					{
						this.EnumerateSubSegments();
					}
					return this._subSegments;
				}
			}

			// Token: 0x17001E26 RID: 7718
			// (get) Token: 0x060084E1 RID: 34017 RVA: 0x00249C90 File Offset: 0x00247E90
			public SpellerInteropBase.ITextRange TextRange
			{
				[SecuritySafeCritical]
				get
				{
					if (this._sTextRange == null)
					{
						NLGSpellerInterop.STextRange value;
						this._textSegment.get_Range(out value);
						this._sTextRange = new NLGSpellerInterop.STextRange?(value);
					}
					return this._sTextRange.Value;
				}
			}

			// Token: 0x17001E27 RID: 7719
			// (get) Token: 0x060084E2 RID: 34018 RVA: 0x00249CD3 File Offset: 0x00247ED3
			public IReadOnlyList<string> Suggestions
			{
				[SecuritySafeCritical]
				get
				{
					if (this._suggestions == null)
					{
						this.EnumerateSuggestions();
					}
					return this._suggestions;
				}
			}

			// Token: 0x17001E28 RID: 7720
			// (get) Token: 0x060084E3 RID: 34019 RVA: 0x00249CE9 File Offset: 0x00247EE9
			public bool IsClean
			{
				[SecuritySafeCritical]
				get
				{
					return this.RangeRole != NLGSpellerInterop.RangeRole.ecrrIncorrect;
				}
			}

			// Token: 0x060084E4 RID: 34020 RVA: 0x00249CF8 File Offset: 0x00247EF8
			[SecuritySafeCritical]
			public void EnumSubSegments(SpellerInteropBase.EnumTextSegmentsCallback segmentCallback, object data)
			{
				bool flag = true;
				int num = 0;
				while (flag && num < this.SubSegments.Count)
				{
					flag = segmentCallback(this.SubSegments[num], data);
					num++;
				}
			}

			// Token: 0x060084E5 RID: 34021 RVA: 0x00249D34 File Offset: 0x00247F34
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060084E6 RID: 34022 RVA: 0x00249D44 File Offset: 0x00247F44
			[SecuritySafeCritical]
			protected virtual void Dispose(bool disposing)
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("NLGSpellerInterop.SpellerSegment");
				}
				if (this._subSegments != null)
				{
					foreach (SpellerInteropBase.ISpellerSegment spellerSegment in this._subSegments)
					{
						NLGSpellerInterop.SpellerSegment spellerSegment2 = (NLGSpellerInterop.SpellerSegment)spellerSegment;
						spellerSegment2.Dispose();
					}
					this._subSegments = null;
				}
				if (this._textSegment != null)
				{
					Marshal.ReleaseComObject(this._textSegment);
					this._textSegment = null;
				}
				this._disposed = true;
			}

			// Token: 0x060084E7 RID: 34023 RVA: 0x00249DDC File Offset: 0x00247FDC
			~SpellerSegment()
			{
				this.Dispose(false);
			}

			// Token: 0x17001E29 RID: 7721
			// (get) Token: 0x060084E8 RID: 34024 RVA: 0x00249E0C File Offset: 0x0024800C
			public NLGSpellerInterop.RangeRole RangeRole
			{
				[SecurityCritical]
				get
				{
					if (this._rangeRole == null)
					{
						NLGSpellerInterop.RangeRole value;
						this._textSegment.get_Role(out value);
						this._rangeRole = new NLGSpellerInterop.RangeRole?(value);
					}
					return this._rangeRole.Value;
				}
			}

			// Token: 0x040042CF RID: 17103
			private NLGSpellerInterop.STextRange? _sTextRange;

			// Token: 0x040042D0 RID: 17104
			private int _subSegmentCount;

			// Token: 0x040042D1 RID: 17105
			private IReadOnlyList<SpellerInteropBase.ISpellerSegment> _subSegments;

			// Token: 0x040042D2 RID: 17106
			private IReadOnlyList<string> _suggestions;

			// Token: 0x040042D3 RID: 17107
			private NLGSpellerInterop.RangeRole? _rangeRole;

			// Token: 0x040042D4 RID: 17108
			private NLGSpellerInterop.ITextSegment _textSegment;

			// Token: 0x040042D5 RID: 17109
			private bool _disposed;
		}

		// Token: 0x020008ED RID: 2285
		private class SpellerSentence : SpellerInteropBase.ISpellerSentence, IDisposable
		{
			// Token: 0x060084E9 RID: 34025 RVA: 0x00249E4C File Offset: 0x0024804C
			[SecurityCritical]
			public SpellerSentence(NLGSpellerInterop.ISentence sentence)
			{
				this._disposed = false;
				try
				{
					int num;
					sentence.get_Count(out num);
					List<SpellerInteropBase.ISpellerSegment> list = new List<SpellerInteropBase.ISpellerSegment>();
					for (int i = 0; i < num; i++)
					{
						NLGSpellerInterop.ITextSegment textSegment;
						sentence.get_Item(i, out textSegment);
						list.Add(new NLGSpellerInterop.SpellerSegment(textSegment));
					}
					this._segments = list.AsReadOnly();
					Invariant.Assert(this._segments.Count == num);
				}
				finally
				{
					Marshal.ReleaseComObject(sentence);
				}
			}

			// Token: 0x17001E2A RID: 7722
			// (get) Token: 0x060084EA RID: 34026 RVA: 0x00249ED0 File Offset: 0x002480D0
			public IReadOnlyList<SpellerInteropBase.ISpellerSegment> Segments
			{
				get
				{
					return this._segments;
				}
			}

			// Token: 0x17001E2B RID: 7723
			// (get) Token: 0x060084EB RID: 34027 RVA: 0x00249ED8 File Offset: 0x002480D8
			public int EndOffset
			{
				get
				{
					int result = -1;
					if (this.Segments.Count > 0)
					{
						SpellerInteropBase.ITextRange textRange = this.Segments[this.Segments.Count - 1].TextRange;
						result = textRange.Start + textRange.Length;
					}
					return result;
				}
			}

			// Token: 0x060084EC RID: 34028 RVA: 0x00249F22 File Offset: 0x00248122
			[SecurityCritical]
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x060084ED RID: 34029 RVA: 0x00249F34 File Offset: 0x00248134
			[SecurityCritical]
			protected virtual void Dispose(bool disposing)
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("NLGSpellerInterop.SpellerSentence");
				}
				if (this._segments != null)
				{
					foreach (SpellerInteropBase.ISpellerSegment spellerSegment in this._segments)
					{
						NLGSpellerInterop.SpellerSegment spellerSegment2 = (NLGSpellerInterop.SpellerSegment)spellerSegment;
						spellerSegment2.Dispose();
					}
					this._segments = null;
				}
				this._disposed = true;
			}

			// Token: 0x060084EE RID: 34030 RVA: 0x00249FB0 File Offset: 0x002481B0
			[SecurityCritical]
			~SpellerSentence()
			{
				this.Dispose(false);
			}

			// Token: 0x040042D6 RID: 17110
			private IReadOnlyList<SpellerInteropBase.ISpellerSegment> _segments;

			// Token: 0x040042D7 RID: 17111
			private bool _disposed;
		}

		// Token: 0x020008EE RID: 2286
		private static class UnsafeNlMethods
		{
			// Token: 0x060084EF RID: 34031
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern void NlLoad();

			// Token: 0x060084F0 RID: 34032
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll")]
			internal static extern void NlUnload();

			// Token: 0x060084F1 RID: 34033
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			[DllImport("PresentationNative_v0400.dll", PreserveSig = false)]
			internal static extern void NlGetClassObject(ref Guid clsid, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object classObject);
		}

		// Token: 0x020008EF RID: 2287
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("004CD7E2-8B63-4ef9-8D46-080CDBBE47AF")]
		[ComImport]
		internal interface ILexicon
		{
			// Token: 0x060084F2 RID: 34034
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void ReadFrom([MarshalAs(UnmanagedType.BStr)] string fileName);

			// Token: 0x060084F3 RID: 34035
			void stub_WriteTo();

			// Token: 0x060084F4 RID: 34036
			void stub_GetEnumerator();

			// Token: 0x060084F5 RID: 34037
			void stub_IndexOf();

			// Token: 0x060084F6 RID: 34038
			void stub_TagFor();

			// Token: 0x060084F7 RID: 34039
			void stub_ContainsPrefix();

			// Token: 0x060084F8 RID: 34040
			void stub_Add();

			// Token: 0x060084F9 RID: 34041
			void stub_Remove();

			// Token: 0x060084FA RID: 34042
			void stub_Version();

			// Token: 0x060084FB RID: 34043
			void stub_Count();

			// Token: 0x060084FC RID: 34044
			void stub__NewEnum();

			// Token: 0x060084FD RID: 34045
			void stub_get_Item();

			// Token: 0x060084FE RID: 34046
			void stub_set_Item();

			// Token: 0x060084FF RID: 34047
			void stub_get_ItemByName();

			// Token: 0x06008500 RID: 34048
			void stub_set_ItemByName();

			// Token: 0x06008501 RID: 34049
			void stub_get0_PropertyCount();

			// Token: 0x06008502 RID: 34050
			void stub_get1_Property();

			// Token: 0x06008503 RID: 34051
			void stub_set_Property();

			// Token: 0x06008504 RID: 34052
			void stub_get_IsSealed();

			// Token: 0x06008505 RID: 34053
			void stub_get_IsReadOnly();
		}

		// Token: 0x020008F0 RID: 2288
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B6797CC0-11AE-4047-A438-26C0C916EB8D")]
		[ComImport]
		private interface ITextContext
		{
			// Token: 0x06008506 RID: 34054
			void stub_get_PropertyCount();

			// Token: 0x06008507 RID: 34055
			void stub_get_Property();

			// Token: 0x06008508 RID: 34056
			void stub_put_Property();

			// Token: 0x06008509 RID: 34057
			void stub_get_DefaultDialectCount();

			// Token: 0x0600850A RID: 34058
			void stub_get_DefaultDialect();

			// Token: 0x0600850B RID: 34059
			void stub_AddDefaultDialect();

			// Token: 0x0600850C RID: 34060
			void stub_RemoveDefaultDialect();

			// Token: 0x0600850D RID: 34061
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_LexiconCount([MarshalAs(UnmanagedType.I4)] out int lexiconCount);

			// Token: 0x0600850E RID: 34062
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Lexicon(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x0600850F RID: 34063
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void AddLexicon([MarshalAs(UnmanagedType.Interface)] [In] NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x06008510 RID: 34064
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void RemoveLexicon([MarshalAs(UnmanagedType.Interface)] [In] NLGSpellerInterop.ILexicon lexicon);

			// Token: 0x06008511 RID: 34065
			void stub_get_Version();

			// Token: 0x06008512 RID: 34066
			void stub_get_ResourceLoader();

			// Token: 0x06008513 RID: 34067
			void stub_put_ResourceLoader();

			// Token: 0x06008514 RID: 34068
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Options([MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.IProcessingOptions val);

			// Token: 0x06008515 RID: 34069
			void get_Capabilities(int locale, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.IProcessingOptions val);

			// Token: 0x06008516 RID: 34070
			void stub_get_Lexicons();

			// Token: 0x06008517 RID: 34071
			void stub_put_Lexicons();

			// Token: 0x06008518 RID: 34072
			void stub_get_MaxSentences();

			// Token: 0x06008519 RID: 34073
			void stub_put_MaxSentences();

			// Token: 0x0600851A RID: 34074
			void stub_get_IsSingleLanguage();

			// Token: 0x0600851B RID: 34075
			void stub_put_IsSingleLanguage();

			// Token: 0x0600851C RID: 34076
			void stub_get_IsSimpleWordBreaking();

			// Token: 0x0600851D RID: 34077
			void stub_put_IsSimpleWordBreaking();

			// Token: 0x0600851E RID: 34078
			void stub_get_UseRelativeTimes();

			// Token: 0x0600851F RID: 34079
			void stub_put_UseRelativeTimes();

			// Token: 0x06008520 RID: 34080
			void stub_get_IgnorePunctuation();

			// Token: 0x06008521 RID: 34081
			void stub_put_IgnorePunctuation();

			// Token: 0x06008522 RID: 34082
			void stub_get_IsCaching();

			// Token: 0x06008523 RID: 34083
			void stub_put_IsCaching();

			// Token: 0x06008524 RID: 34084
			void stub_get_IsShowingGaps();

			// Token: 0x06008525 RID: 34085
			void stub_put_IsShowingGaps();

			// Token: 0x06008526 RID: 34086
			void stub_get_IsShowingCharacterNormalizations();

			// Token: 0x06008527 RID: 34087
			void stub_put_IsShowingCharacterNormalizations();

			// Token: 0x06008528 RID: 34088
			void stub_get_IsShowingWordNormalizations();

			// Token: 0x06008529 RID: 34089
			void stub_put_IsShowingWordNormalizations();

			// Token: 0x0600852A RID: 34090
			void stub_get_IsComputingCompounds();

			// Token: 0x0600852B RID: 34091
			void stub_put_IsComputingCompounds();

			// Token: 0x0600852C RID: 34092
			void stub_get_IsComputingInflections();

			// Token: 0x0600852D RID: 34093
			void stub_put_IsComputingInflections();

			// Token: 0x0600852E RID: 34094
			void stub_get_IsComputingLemmas();

			// Token: 0x0600852F RID: 34095
			void stub_put_IsComputingLemmas();

			// Token: 0x06008530 RID: 34096
			void stub_get_IsComputingExpansions();

			// Token: 0x06008531 RID: 34097
			void stub_put_IsComputingExpansions();

			// Token: 0x06008532 RID: 34098
			void stub_get_IsComputingBases();

			// Token: 0x06008533 RID: 34099
			void stub_put_IsComputingBases();

			// Token: 0x06008534 RID: 34100
			void stub_get_IsComputingPartOfSpeechTags();

			// Token: 0x06008535 RID: 34101
			void stub_put_IsComputingPartOfSpeechTags();

			// Token: 0x06008536 RID: 34102
			void stub_get_IsFindingDefinitions();

			// Token: 0x06008537 RID: 34103
			void stub_put_IsFindingDefinitions();

			// Token: 0x06008538 RID: 34104
			void stub_get_IsFindingDateTimeMeasures();

			// Token: 0x06008539 RID: 34105
			void stub_put_IsFindingDateTimeMeasures();

			// Token: 0x0600853A RID: 34106
			void stub_get_IsFindingPersons();

			// Token: 0x0600853B RID: 34107
			void stub_put_IsFindingPersons();

			// Token: 0x0600853C RID: 34108
			void stub_get_IsFindingLocations();

			// Token: 0x0600853D RID: 34109
			void stub_put_IsFindingLocations();

			// Token: 0x0600853E RID: 34110
			void stub_get_IsFindingOrganizations();

			// Token: 0x0600853F RID: 34111
			void stub_put_IsFindingOrganizations();

			// Token: 0x06008540 RID: 34112
			void stub_get_IsFindingPhrases();

			// Token: 0x06008541 RID: 34113
			void stub_put_IsFindingPhrases();
		}

		// Token: 0x020008F1 RID: 2289
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("549F997E-0EC3-43d4-B443-2BF8021010CF")]
		[ComImport]
		private interface ITextChunk
		{
			// Token: 0x06008542 RID: 34114
			void stub_get_InputText();

			// Token: 0x06008543 RID: 34115
			void stub_put_InputText();

			// Token: 0x06008544 RID: 34116
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void SetInputArray([In] IntPtr inputArray, int size);

			// Token: 0x06008545 RID: 34117
			void stub_RegisterEngine();

			// Token: 0x06008546 RID: 34118
			void stub_UnregisterEngine();

			// Token: 0x06008547 RID: 34119
			void stub_get_InputArray();

			// Token: 0x06008548 RID: 34120
			void stub_get_InputArrayRange();

			// Token: 0x06008549 RID: 34121
			void stub_put_InputArrayRange();

			// Token: 0x0600854A RID: 34122
			void get_Count(out int val);

			// Token: 0x0600854B RID: 34123
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ISentence val);

			// Token: 0x0600854C RID: 34124
			void stub_get__NewEnum();

			// Token: 0x0600854D RID: 34125
			[SecurityCritical]
			void get_Sentences([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x0600854E RID: 34126
			void stub_get_PropertyCount();

			// Token: 0x0600854F RID: 34127
			void stub_get_Property();

			// Token: 0x06008550 RID: 34128
			void stub_put_Property();

			// Token: 0x06008551 RID: 34129
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Context([MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextContext val);

			// Token: 0x06008552 RID: 34130
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void put_Context([MarshalAs(UnmanagedType.Interface)] NLGSpellerInterop.ITextContext val);

			// Token: 0x06008553 RID: 34131
			void stub_get_Locale();

			// Token: 0x06008554 RID: 34132
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void put_Locale(int val);

			// Token: 0x06008555 RID: 34133
			void stub_get_IsLocaleReliable();

			// Token: 0x06008556 RID: 34134
			void stub_put_IsLocaleReliable();

			// Token: 0x06008557 RID: 34135
			void stub_get_IsEndOfDocument();

			// Token: 0x06008558 RID: 34136
			void stub_put_IsEndOfDocument();

			// Token: 0x06008559 RID: 34137
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void GetEnumerator([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x0600855A RID: 34138
			void stub_ToString();

			// Token: 0x0600855B RID: 34139
			void stub_ProcessStream();

			// Token: 0x0600855C RID: 34140
			void get_ReuseObjects(out bool val);

			// Token: 0x0600855D RID: 34141
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void put_ReuseObjects(bool val);
		}

		// Token: 0x020008F2 RID: 2290
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("F0C13A7A-199B-44be-8492-F91EAA50F943")]
		[ComImport]
		private interface ISentence
		{
			// Token: 0x0600855E RID: 34142
			void stub_get_PropertyCount();

			// Token: 0x0600855F RID: 34143
			void stub_get_Property();

			// Token: 0x06008560 RID: 34144
			void stub_put_Property();

			// Token: 0x06008561 RID: 34145
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Count(out int val);

			// Token: 0x06008562 RID: 34146
			void stub_get_Parent();

			// Token: 0x06008563 RID: 34147
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextSegment val);

			// Token: 0x06008564 RID: 34148
			void stub_get__NewEnum();

			// Token: 0x06008565 RID: 34149
			void stub_get_Segments();

			// Token: 0x06008566 RID: 34150
			void stub_GetEnumerator();

			// Token: 0x06008567 RID: 34151
			void stub_get_IsEndOfParagraph();

			// Token: 0x06008568 RID: 34152
			void stub_get_IsUnfinished();

			// Token: 0x06008569 RID: 34153
			void stub_get_IsUnfinishedAtEnd();

			// Token: 0x0600856A RID: 34154
			void stub_get_Locale();

			// Token: 0x0600856B RID: 34155
			void stub_get_IsLocaleReliable();

			// Token: 0x0600856C RID: 34156
			void stub_get_Range();

			// Token: 0x0600856D RID: 34157
			void stub_get_RequiresNormalization();

			// Token: 0x0600856E RID: 34158
			void stub_ToString();

			// Token: 0x0600856F RID: 34159
			void stub_CopyToString();
		}

		// Token: 0x020008F3 RID: 2291
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("AF4656B8-5E5E-4fb2-A2D8-1E977E549A56")]
		[ComImport]
		private interface ITextSegment
		{
			// Token: 0x06008570 RID: 34160
			void stub_get_IsSurfaceString();

			// Token: 0x06008571 RID: 34161
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Range([MarshalAs(UnmanagedType.Struct)] out NLGSpellerInterop.STextRange val);

			// Token: 0x06008572 RID: 34162
			void stub_get_Identifier();

			// Token: 0x06008573 RID: 34163
			void stub_get_Unit();

			// Token: 0x06008574 RID: 34164
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Count(out int val);

			// Token: 0x06008575 RID: 34165
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Item(int index, [MarshalAs(UnmanagedType.Interface)] out NLGSpellerInterop.ITextSegment val);

			// Token: 0x06008576 RID: 34166
			void stub_get_Expansions();

			// Token: 0x06008577 RID: 34167
			void stub_get_Bases();

			// Token: 0x06008578 RID: 34168
			void stub_get_SuggestionScores();

			// Token: 0x06008579 RID: 34169
			void stub_get_PropertyCount();

			// Token: 0x0600857A RID: 34170
			void stub_get_Property();

			// Token: 0x0600857B RID: 34171
			void stub_put_Property();

			// Token: 0x0600857C RID: 34172
			void stub_CopyToString();

			// Token: 0x0600857D RID: 34173
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Role(out NLGSpellerInterop.RangeRole val);

			// Token: 0x0600857E RID: 34174
			void stub_get_PrimaryType();

			// Token: 0x0600857F RID: 34175
			void stub_get_SecondaryType();

			// Token: 0x06008580 RID: 34176
			void stub_get_SpellingVariations();

			// Token: 0x06008581 RID: 34177
			void stub_get_CharacterNormalizations();

			// Token: 0x06008582 RID: 34178
			void stub_get_Representations();

			// Token: 0x06008583 RID: 34179
			void stub_get_Inflections();

			// Token: 0x06008584 RID: 34180
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void get_Suggestions([MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IEnumVariant val);

			// Token: 0x06008585 RID: 34181
			void stub_get_Lemmas();

			// Token: 0x06008586 RID: 34182
			void stub_get_SubSegments();

			// Token: 0x06008587 RID: 34183
			void stub_get_Alternatives();

			// Token: 0x06008588 RID: 34184
			void stub_ToString();

			// Token: 0x06008589 RID: 34185
			void stub_get_IsPossiblePhraseStart();

			// Token: 0x0600858A RID: 34186
			void stub_get_SpellingScore();

			// Token: 0x0600858B RID: 34187
			void stub_get_IsPunctuation();

			// Token: 0x0600858C RID: 34188
			void stub_get_IsEndPunctuation();

			// Token: 0x0600858D RID: 34189
			void stub_get_IsSpace();

			// Token: 0x0600858E RID: 34190
			void stub_get_IsAbbreviation();

			// Token: 0x0600858F RID: 34191
			void stub_get_IsSmiley();
		}

		// Token: 0x020008F4 RID: 2292
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("C090356B-A6A5-442a-A204-CFD5415B5902")]
		[ComImport]
		private interface IProcessingOptions
		{
			// Token: 0x06008590 RID: 34192
			void stub_get__NewEnum();

			// Token: 0x06008591 RID: 34193
			void stub_GetEnumerator();

			// Token: 0x06008592 RID: 34194
			void stub_get_Locale();

			// Token: 0x06008593 RID: 34195
			void stub_get_Count();

			// Token: 0x06008594 RID: 34196
			void stub_get_Name();

			// Token: 0x06008595 RID: 34197
			void stub_get_Item();

			// Token: 0x06008596 RID: 34198
			[SecurityCritical]
			[SuppressUnmanagedCodeSecurity]
			void put_Item(object index, object val);

			// Token: 0x06008597 RID: 34199
			void stub_get_IsReadOnly();
		}
	}
}

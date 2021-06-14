using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045A RID: 1114
	internal class SpellChecker : IDisposable
	{
		// Token: 0x0600404C RID: 16460 RVA: 0x00126214 File Offset: 0x00124414
		public SpellChecker(string languageTag)
		{
			this._speller = new ChangeNotifyWrapper<RCW.ISpellChecker>(null, false);
			this._languageTag = languageTag;
			this._spellCheckerChangedEventHandler = new SpellChecker.SpellCheckerChangedEventHandler(this);
			if (this.Init(false))
			{
				this._speller.PropertyChanged += this.SpellerInstanceChanged;
			}
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x00126267 File Offset: 0x00124467
		[SecuritySafeCritical]
		private bool Init(bool shouldSuppressCOMExceptions = true)
		{
			this._speller.Value = SpellCheckerFactory.CreateSpellChecker(this._languageTag, shouldSuppressCOMExceptions);
			return this._speller.Value != null;
		}

		// Token: 0x0600404E RID: 16462 RVA: 0x0012628E File Offset: 0x0012448E
		public string GetLanguageTag()
		{
			if (!this._disposed)
			{
				return this._languageTag;
			}
			return null;
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x001262A0 File Offset: 0x001244A0
		[SecuritySafeCritical]
		public List<string> SuggestImpl(string word)
		{
			RCW.IEnumString enumString = this._speller.Value.Suggest(word);
			if (enumString == null)
			{
				return null;
			}
			return enumString.ToList(false, true);
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x001262CC File Offset: 0x001244CC
		public List<string> SuggestImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(() => this.SuggestImpl(word), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x0012632E File Offset: 0x0012452E
		public List<string> Suggest(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.SuggestImplWithRetries(word, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004052 RID: 16466 RVA: 0x00126342 File Offset: 0x00124542
		[SecuritySafeCritical]
		private void AddImpl(string word)
		{
			this._speller.Value.Add(word);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x00126358 File Offset: 0x00124558
		private void AddImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AddImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x001263B0 File Offset: 0x001245B0
		public void Add(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.AddImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x001263C3 File Offset: 0x001245C3
		[SecuritySafeCritical]
		private void IgnoreImpl(string word)
		{
			this._speller.Value.Ignore(word);
		}

		// Token: 0x06004056 RID: 16470 RVA: 0x001263D8 File Offset: 0x001245D8
		public void IgnoreImplWithRetries(string word, bool shouldSuppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.IgnoreImpl(word);
			}, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false);
		}

		// Token: 0x06004057 RID: 16471 RVA: 0x00126430 File Offset: 0x00124630
		public void Ignore(string word, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.IgnoreImplWithRetries(word, shouldSuppressCOMExceptions);
		}

		// Token: 0x06004058 RID: 16472 RVA: 0x00126443 File Offset: 0x00124643
		[SecuritySafeCritical]
		private void AutoCorrectImpl(string from, string to)
		{
			this._speller.Value.AutoCorrect(from, to);
		}

		// Token: 0x06004059 RID: 16473 RVA: 0x00126458 File Offset: 0x00124658
		private void AutoCorrectImplWithRetries(string from, string to, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.AutoCorrectImpl(from, to);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x0600405A RID: 16474 RVA: 0x001264B7 File Offset: 0x001246B7
		public void AutoCorrect(string from, string to, bool suppressCOMExceptions = true)
		{
			this.AutoCorrectImplWithRetries(from, to, suppressCOMExceptions);
		}

		// Token: 0x0600405B RID: 16475 RVA: 0x001264C2 File Offset: 0x001246C2
		[SecuritySafeCritical]
		private byte GetOptionValueImpl(string optionId)
		{
			return this._speller.Value.GetOptionValue(optionId);
		}

		// Token: 0x0600405C RID: 16476 RVA: 0x001264D8 File Offset: 0x001246D8
		private byte GetOptionValueImplWithRetries(string optionId, bool suppressCOMExceptions = true)
		{
			byte result;
			if (!RetryHelper.TryExecuteFunction<byte>(() => this.GetOptionValueImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return 0;
			}
			return result;
		}

		// Token: 0x0600405D RID: 16477 RVA: 0x00126538 File Offset: 0x00124738
		public byte GetOptionValue(string optionId, bool suppressCOMExceptions = true)
		{
			return this.GetOptionValueImplWithRetries(optionId, suppressCOMExceptions);
		}

		// Token: 0x0600405E RID: 16478 RVA: 0x00126544 File Offset: 0x00124744
		[SecuritySafeCritical]
		private List<string> GetOptionIdsImpl()
		{
			RCW.IEnumString optionIds = this._speller.Value.OptionIds;
			if (optionIds == null)
			{
				return null;
			}
			return optionIds.ToList(false, true);
		}

		// Token: 0x0600405F RID: 16479 RVA: 0x00126570 File Offset: 0x00124770
		private List<string> GetOptionIdsImplWithRetries(bool suppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.GetOptionIdsImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x001265CB File Offset: 0x001247CB
		public List<string> GetOptionIds(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionIdsImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x001265DE File Offset: 0x001247DE
		[SecuritySafeCritical]
		private string GetIdImpl()
		{
			return this._speller.Value.Id;
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x001265F0 File Offset: 0x001247F0
		private string GetIdImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetIdImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x0012664B File Offset: 0x0012484B
		private string GetId(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetIdImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004064 RID: 16484 RVA: 0x0012665E File Offset: 0x0012485E
		[SecuritySafeCritical]
		private string GetLocalizedNameImpl()
		{
			return this._speller.Value.LocalizedName;
		}

		// Token: 0x06004065 RID: 16485 RVA: 0x00126670 File Offset: 0x00124870
		private string GetLocalizedNameImplWithRetries(bool suppressCOMExceptions)
		{
			string result = null;
			if (!RetryHelper.TryExecuteFunction<string>(new Func<string>(this.GetLocalizedNameImpl), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x001266CB File Offset: 0x001248CB
		public string GetLocalizedName(bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetLocalizedNameImplWithRetries(suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004067 RID: 16487 RVA: 0x001266E0 File Offset: 0x001248E0
		[SecuritySafeCritical]
		private SpellChecker.OptionDescription GetOptionDescriptionImpl(string optionId)
		{
			RCW.IOptionDescription optionDescription = this._speller.Value.GetOptionDescription(optionId);
			if (optionDescription == null)
			{
				return null;
			}
			return SpellChecker.OptionDescription.Create(optionDescription, false, true);
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x0012670C File Offset: 0x0012490C
		private SpellChecker.OptionDescription GetOptionDescriptionImplWithRetries(string optionId, bool suppressCOMExceptions)
		{
			SpellChecker.OptionDescription result = null;
			if (!RetryHelper.TryExecuteFunction<SpellChecker.OptionDescription>(() => this.GetOptionDescriptionImpl(optionId), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x0012676E File Offset: 0x0012496E
		public SpellChecker.OptionDescription GetOptionDescription(string optionId, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.GetOptionDescriptionImplWithRetries(optionId, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x0600406A RID: 16490 RVA: 0x00126784 File Offset: 0x00124984
		[SecuritySafeCritical]
		private List<SpellChecker.SpellingError> CheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.Check(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x001267B4 File Offset: 0x001249B4
		private List<SpellChecker.SpellingError> CheckImplWithRetries(string text, bool suppressCOMExceptions)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.CheckImpl(text), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600406C RID: 16492 RVA: 0x00126816 File Offset: 0x00124A16
		public List<SpellChecker.SpellingError> Check(string text, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.CheckImplWithRetries(text, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x0012682C File Offset: 0x00124A2C
		[SecuritySafeCritical]
		public List<SpellChecker.SpellingError> ComprehensiveCheckImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.ComprehensiveCheck(text);
			if (enumSpellingError == null)
			{
				return null;
			}
			return enumSpellingError.ToList(this, text, false, true);
		}

		// Token: 0x0600406E RID: 16494 RVA: 0x0012685C File Offset: 0x00124A5C
		public List<SpellChecker.SpellingError> ComprehensiveCheckImplWithRetries(string text, bool shouldSuppressCOMExceptions = true)
		{
			List<SpellChecker.SpellingError> result = null;
			if (!RetryHelper.TryExecuteFunction<List<SpellChecker.SpellingError>>(() => this.ComprehensiveCheckImpl(text), out result, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600406F RID: 16495 RVA: 0x001268BE File Offset: 0x00124ABE
		public List<SpellChecker.SpellingError> ComprehensiveCheck(string text, bool shouldSuppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.ComprehensiveCheckImplWithRetries(text, shouldSuppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004070 RID: 16496 RVA: 0x001268D4 File Offset: 0x00124AD4
		[SecuritySafeCritical]
		public bool HasErrorsImpl(string text)
		{
			RCW.IEnumSpellingError enumSpellingError = this._speller.Value.ComprehensiveCheck(text);
			return enumSpellingError != null && enumSpellingError.HasErrors(false, true);
		}

		// Token: 0x06004071 RID: 16497 RVA: 0x00126900 File Offset: 0x00124B00
		public bool HasErrorsImplWithRetries(string text, bool shouldSuppressCOMExceptions = true)
		{
			bool flag = false;
			return RetryHelper.TryExecuteFunction<bool>(() => this.HasErrorsImpl(text), out flag, () => this.Init(shouldSuppressCOMExceptions), SpellChecker.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false) && flag;
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x00126964 File Offset: 0x00124B64
		public bool HasErrors(string text, bool shouldSuppressCOMExceptions = true)
		{
			if (this._disposed || string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			List<SpellChecker.HasErrorsResult> list = this._hasErrorsCache;
			int num = (list != null) ? list.Count : 0;
			int i = 0;
			while (i < num && !(text == list[i].Text))
			{
				i++;
			}
			SpellChecker.HasErrorsResult hasErrorsResult;
			if (i < num)
			{
				hasErrorsResult = list[i];
			}
			else
			{
				hasErrorsResult = new SpellChecker.HasErrorsResult(text, this.HasErrorsImplWithRetries(text, shouldSuppressCOMExceptions));
				if (list == null)
				{
					list = new List<SpellChecker.HasErrorsResult>(10);
					this._hasErrorsCache = list;
				}
				if (num < 10)
				{
					list.Add(hasErrorsResult);
				}
				else
				{
					i = 9;
				}
			}
			while (i > 0)
			{
				list[i] = list[i - 1];
				i--;
			}
			list[0] = hasErrorsResult;
			return hasErrorsResult.HasErrors;
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x00126A20 File Offset: 0x00124C20
		[SecuritySafeCritical]
		private uint? add_SpellCheckerChangedImpl(RCW.ISpellCheckerChangedEventHandler handler)
		{
			if (handler == null)
			{
				return new uint?(this._speller.Value.add_SpellCheckerChanged(handler));
			}
			return null;
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x00126A50 File Offset: 0x00124C50
		private uint? addSpellCheckerChangedImplWithRetries(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions)
		{
			uint? result;
			if (!RetryHelper.TryExecuteFunction<uint?>(() => this.add_SpellCheckerChangedImpl(handler), out result, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004075 RID: 16501 RVA: 0x00126AB8 File Offset: 0x00124CB8
		private uint? add_SpellCheckerChanged(RCW.ISpellCheckerChangedEventHandler handler, bool suppressCOMExceptions = true)
		{
			if (!this._disposed)
			{
				return this.addSpellCheckerChangedImplWithRetries(handler, suppressCOMExceptions);
			}
			return null;
		}

		// Token: 0x06004076 RID: 16502 RVA: 0x00126ADF File Offset: 0x00124CDF
		[SecuritySafeCritical]
		private void remove_SpellCheckerChangedImpl(uint eventCookie)
		{
			this._speller.Value.remove_SpellCheckerChanged(eventCookie);
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x00126AF4 File Offset: 0x00124CF4
		private void remove_SpellCheckerChangedImplWithRetries(uint eventCookie, bool suppressCOMExceptions = true)
		{
			RetryHelper.TryCallAction(delegate()
			{
				this.remove_SpellCheckerChangedImpl(eventCookie);
			}, () => this.Init(suppressCOMExceptions), SpellChecker.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x00126B4C File Offset: 0x00124D4C
		private void remove_SpellCheckerChanged(uint eventCookie, bool suppressCOMExceptions = true)
		{
			if (this._disposed)
			{
				return;
			}
			this.remove_SpellCheckerChangedImplWithRetries(eventCookie, suppressCOMExceptions);
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x00126B60 File Offset: 0x00124D60
		private void SpellerInstanceChanged(object sender, PropertyChangedEventArgs args)
		{
			this._hasErrorsCache = null;
			if (this._changed != null)
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed != null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
				}
			}
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x00126BC4 File Offset: 0x00124DC4
		internal virtual void OnChanged(SpellChecker.SpellCheckerChangedEventArgs e)
		{
			this._hasErrorsCache = null;
			EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
			if (changed == null)
			{
				return;
			}
			changed(this, e);
		}

		// Token: 0x14000097 RID: 151
		// (add) Token: 0x0600407B RID: 16507 RVA: 0x00126BE0 File Offset: 0x00124DE0
		// (remove) Token: 0x0600407C RID: 16508 RVA: 0x00126C3C File Offset: 0x00124E3C
		public event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> Changed
		{
			add
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					if (this._changed == null)
					{
						this._eventCookie = this.add_SpellCheckerChanged(this._spellCheckerChangedEventHandler, true);
					}
					this._changed += value;
				}
			}
			remove
			{
				EventHandler<SpellChecker.SpellCheckerChangedEventArgs> changed = this._changed;
				lock (changed)
				{
					this._changed -= value;
					if (this._changed == null && this._eventCookie != null)
					{
						this.remove_SpellCheckerChanged(this._eventCookie.Value, true);
						this._eventCookie = null;
					}
				}
			}
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x00126CB0 File Offset: 0x00124EB0
		[SecuritySafeCritical]
		protected virtual void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			ChangeNotifyWrapper<RCW.ISpellChecker> speller = this._speller;
			if (((speller != null) ? speller.Value : null) != null)
			{
				try
				{
					Marshal.ReleaseComObject(this._speller.Value);
				}
				catch
				{
				}
				this._speller = null;
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x00126D10 File Offset: 0x00124F10
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x00126D20 File Offset: 0x00124F20
		~SpellChecker()
		{
			this.Dispose(false);
		}

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06004080 RID: 16512 RVA: 0x00126D50 File Offset: 0x00124F50
		// (remove) Token: 0x06004081 RID: 16513 RVA: 0x00126D88 File Offset: 0x00124F88
		private event EventHandler<SpellChecker.SpellCheckerChangedEventArgs> _changed;

		// Token: 0x04002763 RID: 10083
		private static readonly Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
		{
			{
				false,
				new List<Type>()
			},
			{
				true,
				new List<Type>
				{
					typeof(COMException),
					typeof(UnauthorizedAccessException)
				}
			}
		};

		// Token: 0x04002764 RID: 10084
		private ChangeNotifyWrapper<RCW.ISpellChecker> _speller;

		// Token: 0x04002765 RID: 10085
		private string _languageTag;

		// Token: 0x04002766 RID: 10086
		private SpellChecker.SpellCheckerChangedEventHandler _spellCheckerChangedEventHandler;

		// Token: 0x04002767 RID: 10087
		private uint? _eventCookie;

		// Token: 0x04002769 RID: 10089
		private List<SpellChecker.HasErrorsResult> _hasErrorsCache;

		// Token: 0x0400276A RID: 10090
		private const int HasErrorsCacheCapacity = 10;

		// Token: 0x0400276B RID: 10091
		private bool _disposed;

		// Token: 0x02000937 RID: 2359
		internal class OptionDescription
		{
			// Token: 0x17001E79 RID: 7801
			// (get) Token: 0x060086B6 RID: 34486 RVA: 0x0024E8F4 File Offset: 0x0024CAF4
			// (set) Token: 0x060086B7 RID: 34487 RVA: 0x0024E8FC File Offset: 0x0024CAFC
			internal string Id { get; private set; }

			// Token: 0x17001E7A RID: 7802
			// (get) Token: 0x060086B8 RID: 34488 RVA: 0x0024E905 File Offset: 0x0024CB05
			// (set) Token: 0x060086B9 RID: 34489 RVA: 0x0024E90D File Offset: 0x0024CB0D
			internal string Heading { get; private set; }

			// Token: 0x17001E7B RID: 7803
			// (get) Token: 0x060086BA RID: 34490 RVA: 0x0024E916 File Offset: 0x0024CB16
			// (set) Token: 0x060086BB RID: 34491 RVA: 0x0024E91E File Offset: 0x0024CB1E
			internal string Description { get; private set; }

			// Token: 0x17001E7C RID: 7804
			// (get) Token: 0x060086BC RID: 34492 RVA: 0x0024E927 File Offset: 0x0024CB27
			internal IReadOnlyCollection<string> Labels
			{
				get
				{
					return this._labels.AsReadOnly();
				}
			}

			// Token: 0x060086BD RID: 34493 RVA: 0x0024E934 File Offset: 0x0024CB34
			private OptionDescription(string id, string heading, string description, List<string> labels = null)
			{
				this.Id = id;
				this.Heading = heading;
				this.Description = description;
				this._labels = (labels ?? new List<string>());
			}

			// Token: 0x060086BE RID: 34494 RVA: 0x0024E964 File Offset: 0x0024CB64
			[SecuritySafeCritical]
			internal static SpellChecker.OptionDescription Create(RCW.IOptionDescription optionDescription, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (optionDescription == null)
				{
					throw new ArgumentNullException("optionDescription");
				}
				SpellChecker.OptionDescription optionDescription2 = new SpellChecker.OptionDescription(optionDescription.Id, optionDescription.Heading, optionDescription.Description, null);
				try
				{
					optionDescription2._labels = optionDescription.Labels.ToList(true, true);
				}
				catch (COMException obj) when (shouldSuppressCOMExceptions)
				{
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(optionDescription);
					}
				}
				return optionDescription2;
			}

			// Token: 0x040043A1 RID: 17313
			private List<string> _labels;
		}

		// Token: 0x02000938 RID: 2360
		private class HasErrorsResult : Tuple<string, bool>
		{
			// Token: 0x060086BF RID: 34495 RVA: 0x0024E9EC File Offset: 0x0024CBEC
			public HasErrorsResult(string text, bool hasErrors) : base(text, hasErrors)
			{
			}

			// Token: 0x17001E7D RID: 7805
			// (get) Token: 0x060086C0 RID: 34496 RVA: 0x0024E9F6 File Offset: 0x0024CBF6
			public string Text
			{
				get
				{
					return base.Item1;
				}
			}

			// Token: 0x17001E7E RID: 7806
			// (get) Token: 0x060086C1 RID: 34497 RVA: 0x0024E9FE File Offset: 0x0024CBFE
			public bool HasErrors
			{
				get
				{
					return base.Item2;
				}
			}
		}

		// Token: 0x02000939 RID: 2361
		internal class SpellCheckerChangedEventArgs : EventArgs
		{
			// Token: 0x060086C2 RID: 34498 RVA: 0x0024EA06 File Offset: 0x0024CC06
			internal SpellCheckerChangedEventArgs(SpellChecker spellChecker)
			{
				this.SpellChecker = spellChecker;
			}

			// Token: 0x17001E7F RID: 7807
			// (get) Token: 0x060086C3 RID: 34499 RVA: 0x0024EA15 File Offset: 0x0024CC15
			// (set) Token: 0x060086C4 RID: 34500 RVA: 0x0024EA1D File Offset: 0x0024CC1D
			internal SpellChecker SpellChecker { get; private set; }
		}

		// Token: 0x0200093A RID: 2362
		private class SpellCheckerChangedEventHandler : RCW.ISpellCheckerChangedEventHandler
		{
			// Token: 0x060086C5 RID: 34501 RVA: 0x0024EA26 File Offset: 0x0024CC26
			internal SpellCheckerChangedEventHandler(SpellChecker spellChecker)
			{
				this._spellChecker = spellChecker;
				this._eventArgs = new SpellChecker.SpellCheckerChangedEventArgs(this._spellChecker);
			}

			// Token: 0x060086C6 RID: 34502 RVA: 0x0024EA46 File Offset: 0x0024CC46
			public void Invoke(RCW.ISpellChecker sender)
			{
				SpellChecker spellChecker = this._spellChecker;
				RCW.ISpellChecker spellChecker2;
				if (spellChecker == null)
				{
					spellChecker2 = null;
				}
				else
				{
					ChangeNotifyWrapper<RCW.ISpellChecker> speller = spellChecker._speller;
					spellChecker2 = ((speller != null) ? speller.Value : null);
				}
				if (sender == spellChecker2)
				{
					SpellChecker spellChecker3 = this._spellChecker;
					if (spellChecker3 == null)
					{
						return;
					}
					spellChecker3.OnChanged(this._eventArgs);
				}
			}

			// Token: 0x040043A3 RID: 17315
			private SpellChecker.SpellCheckerChangedEventArgs _eventArgs;

			// Token: 0x040043A4 RID: 17316
			private SpellChecker _spellChecker;
		}

		// Token: 0x0200093B RID: 2363
		internal enum CorrectiveAction
		{
			// Token: 0x040043A6 RID: 17318
			None,
			// Token: 0x040043A7 RID: 17319
			GetSuggestions,
			// Token: 0x040043A8 RID: 17320
			Replace,
			// Token: 0x040043A9 RID: 17321
			Delete
		}

		// Token: 0x0200093C RID: 2364
		internal class SpellingError
		{
			// Token: 0x17001E80 RID: 7808
			// (get) Token: 0x060086C7 RID: 34503 RVA: 0x0024EA7F File Offset: 0x0024CC7F
			internal uint StartIndex { get; }

			// Token: 0x17001E81 RID: 7809
			// (get) Token: 0x060086C8 RID: 34504 RVA: 0x0024EA87 File Offset: 0x0024CC87
			internal uint Length { get; }

			// Token: 0x17001E82 RID: 7810
			// (get) Token: 0x060086C9 RID: 34505 RVA: 0x0024EA8F File Offset: 0x0024CC8F
			internal SpellChecker.CorrectiveAction CorrectiveAction { get; }

			// Token: 0x17001E83 RID: 7811
			// (get) Token: 0x060086CA RID: 34506 RVA: 0x0024EA97 File Offset: 0x0024CC97
			internal string Replacement { get; }

			// Token: 0x17001E84 RID: 7812
			// (get) Token: 0x060086CB RID: 34507 RVA: 0x0024EA9F File Offset: 0x0024CC9F
			internal IReadOnlyCollection<string> Suggestions
			{
				get
				{
					return this._suggestions.AsReadOnly();
				}
			}

			// Token: 0x060086CC RID: 34508 RVA: 0x0024EAAC File Offset: 0x0024CCAC
			[SecuritySafeCritical]
			internal SpellingError(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
			{
				if (error == null)
				{
					throw new ArgumentNullException("error");
				}
				this.StartIndex = error.StartIndex;
				this.Length = error.Length;
				this.CorrectiveAction = error.CorrectiveAction;
				this.Replacement = error.Replacement;
				this.PopulateSuggestions(error, spellChecker, text, shouldSuppressCOMExceptions, shouldReleaseCOMObject);
			}

			// Token: 0x060086CD RID: 34509 RVA: 0x0024EB0C File Offset: 0x0024CD0C
			[SecuritySafeCritical]
			private void PopulateSuggestions(RCW.ISpellingError error, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions, bool shouldReleaseCOMObject)
			{
				try
				{
					this._suggestions = new List<string>();
					if (this.CorrectiveAction == SpellChecker.CorrectiveAction.GetSuggestions)
					{
						List<string> collection = spellChecker.Suggest(text, shouldSuppressCOMExceptions);
						this._suggestions.AddRange(collection);
					}
					else if (this.CorrectiveAction == SpellChecker.CorrectiveAction.Replace)
					{
						this._suggestions.Add(this.Replacement);
					}
				}
				finally
				{
					if (shouldReleaseCOMObject)
					{
						Marshal.ReleaseComObject(error);
					}
				}
			}

			// Token: 0x040043AE RID: 17326
			private List<string> _suggestions;
		}
	}
}

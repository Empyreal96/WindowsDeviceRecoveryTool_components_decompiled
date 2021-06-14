using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045B RID: 1115
	internal class SpellCheckerFactory
	{
		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06004083 RID: 16515 RVA: 0x00126E13 File Offset: 0x00125013
		// (set) Token: 0x06004084 RID: 16516 RVA: 0x00126E1B File Offset: 0x0012501B
		internal RCW.ISpellCheckerFactory ComFactory { get; private set; }

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06004085 RID: 16517 RVA: 0x00126E24 File Offset: 0x00125024
		// (set) Token: 0x06004086 RID: 16518 RVA: 0x00126E2B File Offset: 0x0012502B
		internal static SpellCheckerFactory Singleton { get; private set; } = new SpellCheckerFactory();

		// Token: 0x06004087 RID: 16519 RVA: 0x00126E34 File Offset: 0x00125034
		public static SpellCheckerFactory Create(bool shouldSuppressCOMExceptions = false)
		{
			SpellCheckerFactory result = null;
			bool flag = false;
			if (SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), shouldSuppressCOMExceptions, false, out flag) && flag)
			{
				result = SpellCheckerFactory.Singleton;
			}
			return result;
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0000326D File Offset: 0x0000146D
		private SpellCheckerFactory()
		{
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x00126E6C File Offset: 0x0012506C
		static SpellCheckerFactory()
		{
			bool flag = false;
			SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), true, true, out flag);
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x00126EF4 File Offset: 0x001250F4
		private static bool Reinitalize()
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<bool, bool, bool>(new Func<bool, bool, bool>(SpellCheckerFactory.CreateLockFree), false, false, out flag) && flag;
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x00126F20 File Offset: 0x00125120
		[SecuritySafeCritical]
		private static bool CreateLockFree(bool suppressCOMExceptions = true, bool suppressOtherExceptions = true)
		{
			if (SpellCheckerFactory.Singleton.ComFactory != null)
			{
				try
				{
					Marshal.ReleaseComObject(SpellCheckerFactory.Singleton.ComFactory);
				}
				catch
				{
				}
				SpellCheckerFactory.Singleton.ComFactory = null;
			}
			bool flag = false;
			RCW.ISpellCheckerFactory comFactory = null;
			try
			{
				comFactory = new RCW.SpellCheckerFactoryCoClass();
				flag = true;
			}
			catch (COMException obj) when (suppressCOMExceptions)
			{
			}
			catch (UnauthorizedAccessException inner)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner);
				}
			}
			catch (InvalidCastException inner2)
			{
				if (!suppressCOMExceptions)
				{
					throw new COMException(string.Empty, inner2);
				}
			}
			catch (Exception ex) when (suppressOtherExceptions && !(ex is COMException) && !(ex is UnauthorizedAccessException))
			{
			}
			if (flag)
			{
				SpellCheckerFactory.Singleton.ComFactory = comFactory;
			}
			return flag;
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x00127024 File Offset: 0x00125224
		[SecuritySafeCritical]
		private List<string> SupportedLanguagesImpl()
		{
			RCW.ISpellCheckerFactory comFactory = this.ComFactory;
			RCW.IEnumString enumString = (comFactory != null) ? comFactory.SupportedLanguages : null;
			List<string> result = null;
			if (enumString != null)
			{
				result = enumString.ToList(true, true);
			}
			return result;
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x00127054 File Offset: 0x00125254
		private List<string> SupportedLanguagesImplWithRetries(bool shouldSuppressCOMExceptions)
		{
			List<string> result = null;
			if (!RetryHelper.TryExecuteFunction<List<string>>(new Func<List<string>>(this.SupportedLanguagesImpl), out result, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[shouldSuppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600408E RID: 16526 RVA: 0x001270AC File Offset: 0x001252AC
		private List<string> GetSupportedLanguagesPrivate(bool shouldSuppressCOMExceptions = true)
		{
			List<string> result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<bool, List<string>>(new Func<bool, List<string>>(this.SupportedLanguagesImplWithRetries), shouldSuppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600408F RID: 16527 RVA: 0x001270DB File Offset: 0x001252DB
		internal static List<string> GetSupportedLanguages(bool shouldSuppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.GetSupportedLanguagesPrivate(shouldSuppressCOMExceptions);
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x001270EE File Offset: 0x001252EE
		[SecuritySafeCritical]
		private bool IsSupportedImpl(string languageTag)
		{
			return this.ComFactory != null && this.ComFactory.IsSupported(languageTag) != 0;
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x0012710C File Offset: 0x0012530C
		private bool IsSupportedImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			bool flag = false;
			return RetryHelper.TryExecuteFunction<bool>(() => this.IsSupportedImpl(languageTag), out flag, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false) && flag;
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00127178 File Offset: 0x00125378
		private bool IsSupportedPrivate(string languageTag, bool suppressCOMExceptons = true)
		{
			bool flag = false;
			return SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, bool>(new Func<string, bool, bool>(this.IsSupportedImplWithRetries), languageTag, suppressCOMExceptons, out flag) && flag;
		}

		// Token: 0x06004093 RID: 16531 RVA: 0x001271A8 File Offset: 0x001253A8
		internal static bool IsSupported(string languageTag, bool suppressCOMExceptons = true)
		{
			return SpellCheckerFactory.Singleton != null && SpellCheckerFactory.Singleton.IsSupportedPrivate(languageTag, suppressCOMExceptons);
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x001271BF File Offset: 0x001253BF
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerImpl(string languageTag)
		{
			return SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker();
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x001271CC File Offset: 0x001253CC
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerImplWithRetries(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!RetryHelper.TryExecuteFunction<RCW.ISpellChecker>(new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellChecker), out result, new RetryHelper.RetryFunctionPreamble<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(languageTag).CreateSpellCheckerRetryPreamble), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x00127218 File Offset: 0x00125418
		[SecurityCritical]
		private RCW.ISpellChecker CreateSpellCheckerPrivate(string languageTag, bool suppressCOMExceptions = true)
		{
			RCW.ISpellChecker result = null;
			if (!SpellCheckerFactory._factoryLock.WithWriteLock<string, bool, RCW.ISpellChecker>(new Func<string, bool, RCW.ISpellChecker>(this.CreateSpellCheckerImplWithRetries), languageTag, suppressCOMExceptions, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x00127248 File Offset: 0x00125448
		[SecurityCritical]
		internal static RCW.ISpellChecker CreateSpellChecker(string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return null;
			}
			return singleton.CreateSpellCheckerPrivate(languageTag, suppressCOMExceptions);
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0012725C File Offset: 0x0012545C
		[SecuritySafeCritical]
		private void RegisterUserDicionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar != null)
			{
				userDictionariesRegistrar.RegisterUserDictionary(dictionaryPath, languageTag);
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x00127280 File Offset: 0x00125480
		private void RegisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.RegisterUserDicionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x0012730C File Offset: 0x0012550C
		private void RegisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.RegisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x0600409B RID: 16539 RVA: 0x00127352 File Offset: 0x00125552
		internal static void RegisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.RegisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x0600409C RID: 16540 RVA: 0x00127368 File Offset: 0x00125568
		[SecuritySafeCritical]
		private void UnregisterUserDictionaryImpl(string dictionaryPath, string languageTag)
		{
			RCW.IUserDictionariesRegistrar userDictionariesRegistrar = (RCW.IUserDictionariesRegistrar)this.ComFactory;
			if (userDictionariesRegistrar != null)
			{
				userDictionariesRegistrar.UnregisterUserDictionary(dictionaryPath, languageTag);
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x0012738C File Offset: 0x0012558C
		private void UnregisterUserDictionaryImplWithRetries(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			if (dictionaryPath == null)
			{
				throw new ArgumentNullException("dictionaryPath");
			}
			if (languageTag == null)
			{
				throw new ArgumentNullException("languageTag");
			}
			RetryHelper.TryCallAction(delegate()
			{
				this.UnregisterUserDictionaryImpl(dictionaryPath, languageTag);
			}, () => SpellCheckerFactory.Reinitalize(), SpellCheckerFactory.SuppressedExceptions[suppressCOMExceptions], 3, false);
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x00127418 File Offset: 0x00125618
		private void UnregisterUserDictionaryPrivate(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory._factoryLock.WithWriteLock(delegate()
			{
				this.UnregisterUserDictionaryImplWithRetries(dictionaryPath, languageTag, suppressCOMExceptions);
			});
		}

		// Token: 0x0600409F RID: 16543 RVA: 0x0012745E File Offset: 0x0012565E
		internal static void UnregisterUserDictionary(string dictionaryPath, string languageTag, bool suppressCOMExceptions = true)
		{
			SpellCheckerFactory singleton = SpellCheckerFactory.Singleton;
			if (singleton == null)
			{
				return;
			}
			singleton.UnregisterUserDictionaryPrivate(dictionaryPath, languageTag, suppressCOMExceptions);
		}

		// Token: 0x0400276D RID: 10093
		private static ReaderWriterLockSlimWrapper _factoryLock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.SupportsRecursion, true);

		// Token: 0x0400276F RID: 10095
		private static Dictionary<bool, List<Type>> SuppressedExceptions = new Dictionary<bool, List<Type>>
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

		// Token: 0x0200094B RID: 2379
		private class SpellCheckerCreationHelper
		{
			// Token: 0x17001E85 RID: 7813
			// (get) Token: 0x060086F5 RID: 34549 RVA: 0x0024ED61 File Offset: 0x0024CF61
			public static RCW.ISpellCheckerFactory ComFactory
			{
				[SecurityCritical]
				get
				{
					return SpellCheckerFactory.Singleton.ComFactory;
				}
			}

			// Token: 0x060086F6 RID: 34550 RVA: 0x0024ED6D File Offset: 0x0024CF6D
			private SpellCheckerCreationHelper(string language)
			{
				this._language = language;
			}

			// Token: 0x060086F7 RID: 34551 RVA: 0x0024ED7C File Offset: 0x0024CF7C
			private static void CreateForLanguage(string language)
			{
				SpellCheckerFactory.SpellCheckerCreationHelper._instances[language] = new SpellCheckerFactory.SpellCheckerCreationHelper(language);
			}

			// Token: 0x060086F8 RID: 34552 RVA: 0x0024ED8F File Offset: 0x0024CF8F
			public static SpellCheckerFactory.SpellCheckerCreationHelper Helper(string language)
			{
				if (!SpellCheckerFactory.SpellCheckerCreationHelper._instances.ContainsKey(language))
				{
					SpellCheckerFactory.SpellCheckerCreationHelper._lock.WithWriteLock<string>(new Action<string>(SpellCheckerFactory.SpellCheckerCreationHelper.CreateForLanguage), language);
				}
				return SpellCheckerFactory.SpellCheckerCreationHelper._instances[language];
			}

			// Token: 0x060086F9 RID: 34553 RVA: 0x0024EDC1 File Offset: 0x0024CFC1
			[SecurityCritical]
			public RCW.ISpellChecker CreateSpellChecker()
			{
				RCW.ISpellCheckerFactory comFactory = SpellCheckerFactory.SpellCheckerCreationHelper.ComFactory;
				if (comFactory == null)
				{
					return null;
				}
				return comFactory.CreateSpellChecker(this._language);
			}

			// Token: 0x060086FA RID: 34554 RVA: 0x0024EDDC File Offset: 0x0024CFDC
			[SecurityCritical]
			public bool CreateSpellCheckerRetryPreamble(out Func<RCW.ISpellChecker> func)
			{
				func = null;
				bool result;
				if (result = SpellCheckerFactory.Reinitalize())
				{
					func = new Func<RCW.ISpellChecker>(SpellCheckerFactory.SpellCheckerCreationHelper.Helper(this._language).CreateSpellChecker);
				}
				return result;
			}

			// Token: 0x040043D7 RID: 17367
			private static Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper> _instances = new Dictionary<string, SpellCheckerFactory.SpellCheckerCreationHelper>();

			// Token: 0x040043D8 RID: 17368
			private static ReaderWriterLockSlimWrapper _lock = new ReaderWriterLockSlimWrapper(LockRecursionPolicy.NoRecursion, true);

			// Token: 0x040043D9 RID: 17369
			private string _language;
		}
	}
}

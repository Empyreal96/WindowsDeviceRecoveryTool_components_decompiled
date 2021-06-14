using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x02000459 RID: 1113
	internal class RCW
	{
		// Token: 0x0200092B RID: 2347
		internal enum WORDLIST_TYPE
		{
			// Token: 0x04004395 RID: 17301
			WORDLIST_TYPE_IGNORE,
			// Token: 0x04004396 RID: 17302
			WORDLIST_TYPE_ADD,
			// Token: 0x04004397 RID: 17303
			WORDLIST_TYPE_EXCLUDE,
			// Token: 0x04004398 RID: 17304
			WORDLIST_TYPE_AUTOCORRECT
		}

		// Token: 0x0200092C RID: 2348
		internal enum CORRECTIVE_ACTION
		{
			// Token: 0x0400439A RID: 17306
			CORRECTIVE_ACTION_NONE,
			// Token: 0x0400439B RID: 17307
			CORRECTIVE_ACTION_GET_SUGGESTIONS,
			// Token: 0x0400439C RID: 17308
			CORRECTIVE_ACTION_REPLACE,
			// Token: 0x0400439D RID: 17309
			CORRECTIVE_ACTION_DELETE
		}

		// Token: 0x0200092D RID: 2349
		[Guid("B7C82D61-FBE8-4B47-9B27-6C0D2E0DE0A3")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellingError
		{
			// Token: 0x17001E6B RID: 7787
			// (get) Token: 0x0600868F RID: 34447
			uint StartIndex { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001E6C RID: 7788
			// (get) Token: 0x06008690 RID: 34448
			uint Length { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001E6D RID: 7789
			// (get) Token: 0x06008691 RID: 34449
			RCW.CORRECTIVE_ACTION CorrectiveAction { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] get; }

			// Token: 0x17001E6E RID: 7790
			// (get) Token: 0x06008692 RID: 34450
			string Replacement { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }
		}

		// Token: 0x0200092E RID: 2350
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("803E3BD4-2828-4410-8290-418D1D73C762")]
		[ComImport]
		internal interface IEnumSpellingError
		{
			// Token: 0x06008693 RID: 34451
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.ISpellingError Next();
		}

		// Token: 0x0200092F RID: 2351
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("00000101-0000-0000-C000-000000000046")]
		[ComImport]
		internal interface IEnumString
		{
			// Token: 0x06008694 RID: 34452
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RemoteNext([In] uint celt, [MarshalAs(UnmanagedType.LPWStr)] out string rgelt, out uint pceltFetched);

			// Token: 0x06008695 RID: 34453
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Skip([In] uint celt);

			// Token: 0x06008696 RID: 34454
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Reset();

			// Token: 0x06008697 RID: 34455
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Clone([MarshalAs(UnmanagedType.Interface)] out RCW.IEnumString ppenum);
		}

		// Token: 0x02000930 RID: 2352
		[Guid("432E5F85-35CF-4606-A801-6F70277E1D7A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IOptionDescription
		{
			// Token: 0x17001E6F RID: 7791
			// (get) Token: 0x06008698 RID: 34456
			string Id { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001E70 RID: 7792
			// (get) Token: 0x06008699 RID: 34457
			string Heading { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001E71 RID: 7793
			// (get) Token: 0x0600869A RID: 34458
			string Description { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001E72 RID: 7794
			// (get) Token: 0x0600869B RID: 34459
			RCW.IEnumString Labels { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }
		}

		// Token: 0x02000931 RID: 2353
		[Guid("0B83A5B0-792F-4EAB-9799-ACF52C5ED08A")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellCheckerChangedEventHandler
		{
			// Token: 0x0600869C RID: 34460
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Invoke([MarshalAs(UnmanagedType.Interface)] [In] RCW.ISpellChecker sender);
		}

		// Token: 0x02000932 RID: 2354
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("B6FD0B71-E2BC-4653-8D05-F197E412770B")]
		[ComImport]
		internal interface ISpellChecker
		{
			// Token: 0x17001E73 RID: 7795
			// (get) Token: 0x0600869D RID: 34461
			string languageTag { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x0600869E RID: 34462
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumSpellingError Check([MarshalAs(UnmanagedType.LPWStr)] [In] string text);

			// Token: 0x0600869F RID: 34463
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumString Suggest([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x060086A0 RID: 34464
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Add([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x060086A1 RID: 34465
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void Ignore([MarshalAs(UnmanagedType.LPWStr)] [In] string word);

			// Token: 0x060086A2 RID: 34466
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void AutoCorrect([MarshalAs(UnmanagedType.LPWStr)] [In] string from, [MarshalAs(UnmanagedType.LPWStr)] [In] string to);

			// Token: 0x060086A3 RID: 34467
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			byte GetOptionValue([MarshalAs(UnmanagedType.LPWStr)] [In] string optionId);

			// Token: 0x17001E74 RID: 7796
			// (get) Token: 0x060086A4 RID: 34468
			RCW.IEnumString OptionIds { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x17001E75 RID: 7797
			// (get) Token: 0x060086A5 RID: 34469
			string Id { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x17001E76 RID: 7798
			// (get) Token: 0x060086A6 RID: 34470
			string LocalizedName { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.LPWStr)] get; }

			// Token: 0x060086A7 RID: 34471
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			uint add_SpellCheckerChanged([MarshalAs(UnmanagedType.Interface)] [In] RCW.ISpellCheckerChangedEventHandler handler);

			// Token: 0x060086A8 RID: 34472
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void remove_SpellCheckerChanged([In] uint eventCookie);

			// Token: 0x060086A9 RID: 34473
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IOptionDescription GetOptionDescription([MarshalAs(UnmanagedType.LPWStr)] [In] string optionId);

			// Token: 0x060086AA RID: 34474
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.IEnumSpellingError ComprehensiveCheck([MarshalAs(UnmanagedType.LPWStr)] [In] string text);
		}

		// Token: 0x02000933 RID: 2355
		[Guid("8E018A9D-2415-4677-BF08-794EA61F94BB")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ISpellCheckerFactory
		{
			// Token: 0x17001E77 RID: 7799
			// (get) Token: 0x060086AB RID: 34475
			RCW.IEnumString SupportedLanguages { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x060086AC RID: 34476
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			int IsSupported([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x060086AD RID: 34477
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			RCW.ISpellChecker CreateSpellChecker([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);
		}

		// Token: 0x02000934 RID: 2356
		[Guid("AA176B85-0E12-4844-8E1A-EEF1DA77F586")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IUserDictionariesRegistrar
		{
			// Token: 0x060086AE RID: 34478
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void RegisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x060086AF RID: 34479
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			void UnregisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);
		}

		// Token: 0x02000935 RID: 2357
		[Guid("7AB36653-1796-484B-BDFA-E74F1DB7C1DC")]
		[TypeLibType(TypeLibTypeFlags.FCanCreate)]
		[ClassInterface(ClassInterfaceType.None)]
		[ComImport]
		internal class SpellCheckerFactoryCoClass : RCW.ISpellCheckerFactory, RCW.SpellCheckerFactoryClass, RCW.IUserDictionariesRegistrar
		{
			// Token: 0x060086B0 RID: 34480
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[return: MarshalAs(UnmanagedType.Interface)]
			public virtual extern RCW.ISpellChecker CreateSpellChecker([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x060086B1 RID: 34481
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern int IsSupported([MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x060086B2 RID: 34482
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern void RegisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x060086B3 RID: 34483
			[SuppressUnmanagedCodeSecurity]
			[MethodImpl(MethodImplOptions.InternalCall)]
			public virtual extern void UnregisterUserDictionary([MarshalAs(UnmanagedType.LPWStr)] [In] string dictionaryPath, [MarshalAs(UnmanagedType.LPWStr)] [In] string languageTag);

			// Token: 0x17001E78 RID: 7800
			// (get) Token: 0x060086B4 RID: 34484
			public virtual extern RCW.IEnumString SupportedLanguages { [SuppressUnmanagedCodeSecurity] [MethodImpl(MethodImplOptions.InternalCall)] [return: MarshalAs(UnmanagedType.Interface)] get; }

			// Token: 0x060086B5 RID: 34485
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern SpellCheckerFactoryCoClass();
		}

		// Token: 0x02000936 RID: 2358
		[CoClass(typeof(RCW.SpellCheckerFactoryCoClass))]
		[Guid("8E018A9D-2415-4677-BF08-794EA61F94BB")]
		[ComImport]
		internal interface SpellCheckerFactoryClass : RCW.ISpellCheckerFactory
		{
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MS.Internal.WindowsRuntime.Windows.Globalization
{
	// Token: 0x020007F7 RID: 2039
	internal class Language
	{
		// Token: 0x06007D73 RID: 32115 RVA: 0x00233FA4 File Offset: 0x002321A4
		static Language()
		{
			ConstructorInfo left = null;
			try
			{
				Language.s_WinRTType = Type.GetType(Language.s_TypeName);
				if (Language.s_WinRTType != null)
				{
					left = Language.s_WinRTType.GetConstructor(new Type[]
					{
						typeof(string)
					});
				}
			}
			catch
			{
				Language.s_WinRTType = null;
			}
			Language.s_Supported = (Language.s_WinRTType != null && left != null);
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x00234030 File Offset: 0x00232230
		public Language(string languageTag)
		{
			if (!Language.s_Supported)
			{
				throw new PlatformNotSupportedException();
			}
			try
			{
				this._language = Language.s_WinRTType.ReflectionNew(languageTag);
			}
			catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException)
			{
				throw new ArgumentException(string.Empty, "languageTag", ex);
			}
			if (this._language == null)
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06007D75 RID: 32117 RVA: 0x002340B4 File Offset: 0x002322B4
		public IReadOnlyList<string> GetExtensionSubtags(string singleton)
		{
			object obj = this._language.ReflectionCall("GetExtensionSubtags", singleton);
			List<string> list = new List<string>();
			foreach (object obj2 in ((IEnumerable)obj))
			{
				if (obj2 != null)
				{
					list.Add((string)obj2);
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x00234134 File Offset: 0x00232334
		public static bool IsWellFormed(string languageTag)
		{
			if (!Language.s_Supported)
			{
				throw new PlatformNotSupportedException();
			}
			return Language.s_WinRTType.ReflectionStaticCall("IsWellFormed", languageTag);
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x00234153 File Offset: 0x00232353
		public static bool TrySetInputMethodLanguageTag(string languageTag)
		{
			if (!Language.s_Supported)
			{
				throw new PlatformNotSupportedException();
			}
			return Language.s_WinRTType.ReflectionStaticCall("TrySetInputMethodLanguageTag", languageTag);
		}

		// Token: 0x17001D27 RID: 7463
		// (get) Token: 0x06007D78 RID: 32120 RVA: 0x00234172 File Offset: 0x00232372
		public static string CurrentInputMethodLanguageTag
		{
			get
			{
				if (!Language.s_Supported)
				{
					throw new PlatformNotSupportedException();
				}
				return Language.s_WinRTType.ReflectionStaticGetProperty("CurrentInputMethodLanguageTag");
			}
		}

		// Token: 0x17001D28 RID: 7464
		// (get) Token: 0x06007D79 RID: 32121 RVA: 0x00234190 File Offset: 0x00232390
		public string DisplayName
		{
			get
			{
				return this._language.ReflectionGetProperty("DisplayName");
			}
		}

		// Token: 0x17001D29 RID: 7465
		// (get) Token: 0x06007D7A RID: 32122 RVA: 0x002341A2 File Offset: 0x002323A2
		public string LanguageTag
		{
			get
			{
				return this._language.ReflectionGetProperty("LanguageTag");
			}
		}

		// Token: 0x17001D2A RID: 7466
		// (get) Token: 0x06007D7B RID: 32123 RVA: 0x002341B4 File Offset: 0x002323B4
		public string NativeName
		{
			get
			{
				return this._language.ReflectionGetProperty("NativeName");
			}
		}

		// Token: 0x17001D2B RID: 7467
		// (get) Token: 0x06007D7C RID: 32124 RVA: 0x002341C6 File Offset: 0x002323C6
		public string Script
		{
			get
			{
				return this._language.ReflectionGetProperty("Script");
			}
		}

		// Token: 0x04003B0E RID: 15118
		private static Type s_WinRTType;

		// Token: 0x04003B0F RID: 15119
		private static readonly bool s_Supported;

		// Token: 0x04003B10 RID: 15120
		private static readonly string s_TypeName = "Windows.Globalization.Language, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003B11 RID: 15121
		private object _language;
	}
}

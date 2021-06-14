using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045F RID: 1119
	internal static class Extensions
	{
		// Token: 0x060040AB RID: 16555 RVA: 0x00127588 File Offset: 0x00125788
		[SecuritySafeCritical]
		internal static List<string> ToList(this RCW.IEnumString enumString, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			List<string> list = new List<string>();
			if (enumString == null)
			{
				throw new ArgumentNullException("enumString");
			}
			try
			{
				uint num = 0U;
				string empty = string.Empty;
				do
				{
					enumString.RemoteNext(1U, out empty, out num);
					if (num > 0U)
					{
						list.Add(empty);
					}
				}
				while (num > 0U);
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(enumString);
				}
			}
			return list;
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0012760C File Offset: 0x0012580C
		[SecuritySafeCritical]
		internal static List<SpellChecker.SpellingError> ToList(this RCW.IEnumSpellingError spellingErrors, SpellChecker spellChecker, string text, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			if (spellingErrors == null)
			{
				throw new ArgumentNullException("spellingErrors");
			}
			List<SpellChecker.SpellingError> list = new List<SpellChecker.SpellingError>();
			try
			{
				for (;;)
				{
					RCW.ISpellingError spellingError = spellingErrors.Next();
					if (spellingError == null)
					{
						break;
					}
					SpellChecker.SpellingError item = new SpellChecker.SpellingError(spellingError, spellChecker, text, shouldSuppressCOMExceptions, true);
					list.Add(item);
				}
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(spellingErrors);
				}
			}
			return list;
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0012768C File Offset: 0x0012588C
		internal static bool IsClean(this List<SpellChecker.SpellingError> errors)
		{
			if (errors == null)
			{
				throw new ArgumentNullException("errors");
			}
			bool result = true;
			foreach (SpellChecker.SpellingError spellingError in errors)
			{
				if (spellingError.CorrectiveAction != SpellChecker.CorrectiveAction.None)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x001276F0 File Offset: 0x001258F0
		[SecuritySafeCritical]
		internal static bool HasErrors(this RCW.IEnumSpellingError spellingErrors, bool shouldSuppressCOMExceptions = true, bool shouldReleaseCOMObject = true)
		{
			if (spellingErrors == null)
			{
				throw new ArgumentNullException("spellingErrors");
			}
			bool flag = false;
			try
			{
				while (!flag)
				{
					RCW.ISpellingError spellingError = spellingErrors.Next();
					if (spellingError == null)
					{
						break;
					}
					if (spellingError.CorrectiveAction != RCW.CORRECTIVE_ACTION.CORRECTIVE_ACTION_NONE)
					{
						flag = true;
					}
					Marshal.ReleaseComObject(spellingError);
				}
			}
			catch (COMException obj) when (shouldSuppressCOMExceptions)
			{
			}
			finally
			{
				if (shouldReleaseCOMObject)
				{
					Marshal.ReleaseComObject(spellingErrors);
				}
			}
			return flag;
		}
	}
}

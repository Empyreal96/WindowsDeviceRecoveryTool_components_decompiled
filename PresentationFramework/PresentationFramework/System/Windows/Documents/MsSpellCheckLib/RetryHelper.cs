using System;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x02000460 RID: 1120
	internal static class RetryHelper
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x00127770 File Offset: 0x00125970
		internal static bool TryCallAction(Action action, RetryHelper.RetryActionPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out action);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x001277EC File Offset: 0x001259EC
		internal static bool TryCallAction(Action action, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (action != null)
					{
						action();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x00127868 File Offset: 0x00125A68
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryFunctionPreamble<TResult> preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble(out func);
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x001278F4 File Offset: 0x00125AF4
		internal static bool TryExecuteFunction<TResult>(Func<TResult> func, out TResult result, RetryHelper.RetryPreamble preamble, List<Type> ignoredExceptions, int retries = 3, bool throwOnFailure = false)
		{
			RetryHelper.ValidateExceptionTypeList(ignoredExceptions);
			result = default(TResult);
			int num = retries;
			bool flag = false;
			bool flag2 = true;
			do
			{
				try
				{
					if (func != null)
					{
						result = func();
					}
					flag = true;
					break;
				}
				catch (Exception exception) when (RetryHelper.MatchException(exception, ignoredExceptions))
				{
				}
				num--;
				if (num > 0)
				{
					flag2 = preamble();
				}
			}
			while (num > 0 && flag2);
			if (!flag && throwOnFailure)
			{
				throw new RetriesExhaustedException();
			}
			return flag;
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0012797C File Offset: 0x00125B7C
		private static bool MatchException(Exception exception, List<Type> exceptions)
		{
			if (exception == null)
			{
				throw new ArgumentNullException("exception");
			}
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Type exceptionType = exception.GetType();
			Type left = exceptions.Find((Type e) => e.IsAssignableFrom(exceptionType));
			return left != null;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x001279D1 File Offset: 0x00125BD1
		private static void ValidateExceptionTypeList(List<Type> exceptions)
		{
			if (exceptions == null)
			{
				throw new ArgumentNullException("exceptions");
			}
			Invariant.Assert(exceptions.TrueForAll((Type t) => typeof(Exception).IsAssignableFrom(t)));
		}

		// Token: 0x02000952 RID: 2386
		// (Invoke) Token: 0x0600870D RID: 34573
		internal delegate bool RetryPreamble();

		// Token: 0x02000953 RID: 2387
		// (Invoke) Token: 0x06008711 RID: 34577
		internal delegate bool RetryActionPreamble(out Action action);

		// Token: 0x02000954 RID: 2388
		// (Invoke) Token: 0x06008715 RID: 34581
		internal delegate bool RetryFunctionPreamble<TResult>(out Func<TResult> func);
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Data.OData
{
	// Token: 0x020002A5 RID: 677
	internal static class ExceptionUtils
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x000529A4 File Offset: 0x00050BA4
		internal static bool IsCatchableExceptionType(Exception e)
		{
			Type type = e.GetType();
			return type != ExceptionUtils.ThreadAbortType && type != ExceptionUtils.StackOverflowType && type != ExceptionUtils.OutOfMemoryType;
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x000529DF File Offset: 0x00050BDF
		internal static void CheckArgumentNotNull<T>([ExceptionUtils.ValidatedNotNullAttribute] T value, string parameterName) where T : class
		{
			if (value == null)
			{
				throw Error.ArgumentNull(parameterName);
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x000529F0 File Offset: 0x00050BF0
		internal static void CheckArgumentStringNotEmpty(string value, string parameterName)
		{
			if (value != null && value.Length == 0)
			{
				throw new ArgumentException(Strings.ExceptionUtils_ArgumentStringEmpty, parameterName);
			}
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00052A09 File Offset: 0x00050C09
		internal static void CheckArgumentStringNotNullOrEmpty([ExceptionUtils.ValidatedNotNullAttribute] string value, string parameterName)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(parameterName, Strings.ExceptionUtils_ArgumentStringNullOrEmpty);
			}
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00052A1F File Offset: 0x00050C1F
		internal static void CheckIntegerNotNegative(int value, string parameterName)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException(parameterName, Strings.ExceptionUtils_CheckIntegerNotNegative(value));
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00052A37 File Offset: 0x00050C37
		internal static void CheckIntegerPositive(int value, string parameterName)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException(parameterName, Strings.ExceptionUtils_CheckIntegerPositive(value));
			}
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00052A4F File Offset: 0x00050C4F
		internal static void CheckLongPositive(long value, string parameterName)
		{
			if (value <= 0L)
			{
				throw new ArgumentOutOfRangeException(parameterName, Strings.ExceptionUtils_CheckLongPositive(value));
			}
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00052A68 File Offset: 0x00050C68
		internal static void CheckArgumentCollectionNotNullOrEmpty<T>(ICollection<T> value, string parameterName)
		{
			if (value == null)
			{
				throw Error.ArgumentNull(parameterName);
			}
			if (value.Count == 0)
			{
				throw new ArgumentException(Strings.ExceptionUtils_ArgumentStringEmpty, parameterName);
			}
		}

		// Token: 0x04000971 RID: 2417
		private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

		// Token: 0x04000972 RID: 2418
		private static readonly Type StackOverflowType = typeof(StackOverflowException);

		// Token: 0x04000973 RID: 2419
		private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

		// Token: 0x020002A6 RID: 678
		private sealed class ValidatedNotNullAttribute : Attribute
		{
		}
	}
}

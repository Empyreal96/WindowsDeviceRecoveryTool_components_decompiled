using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Standard
{
	// Token: 0x02000005 RID: 5
	internal static class Assert
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002128 File Offset: 0x00000328
		private static void _Break()
		{
			Debugger.Break();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000212F File Offset: 0x0000032F
		[Conditional("DEBUG")]
		public static void Evaluate(Assert.EvaluateFunction argument)
		{
			argument();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002137 File Offset: 0x00000337
		[Obsolete("Use Assert.AreEqual instead of Assert.Equals", false)]
		[Conditional("DEBUG")]
		public static void Equals<T>(T expected, T actual)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000213C File Offset: 0x0000033C
		[Conditional("DEBUG")]
		public static void AreEqual<T>(T expected, T actual)
		{
			if (expected == null)
			{
				if (actual != null && !actual.Equals(expected))
				{
					Assert._Break();
					return;
				}
			}
			else if (!expected.Equals(actual))
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002190 File Offset: 0x00000390
		[Conditional("DEBUG")]
		public static void AreNotEqual<T>(T notExpected, T actual)
		{
			if (notExpected == null)
			{
				if (actual == null || actual.Equals(notExpected))
				{
					Assert._Break();
					return;
				}
			}
			else if (notExpected.Equals(actual))
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021E2 File Offset: 0x000003E2
		[Conditional("DEBUG")]
		public static void Implies(bool condition, bool result)
		{
			if (condition && !result)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021EF File Offset: 0x000003EF
		[Conditional("DEBUG")]
		public static void Implies(bool condition, Assert.ImplicationFunction result)
		{
			if (condition && !result())
			{
				Assert._Break();
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		public static void IsNeitherNullNorEmpty(string value)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002201 File Offset: 0x00000401
		[Conditional("DEBUG")]
		public static void IsNeitherNullNorWhitespace(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Assert._Break();
			}
			if (value.Trim().Length == 0)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002222 File Offset: 0x00000422
		[Conditional("DEBUG")]
		public static void IsNotNull<T>(T value) where T : class
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002234 File Offset: 0x00000434
		[Conditional("DEBUG")]
		public static void IsDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002260 File Offset: 0x00000460
		[Conditional("DEBUG")]
		public static void IsNotDefault<T>(T value) where T : struct
		{
			value.Equals(default(T));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002289 File Offset: 0x00000489
		[Conditional("DEBUG")]
		public static void IsFalse(bool condition, string message)
		{
			if (condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002293 File Offset: 0x00000493
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002293 File Offset: 0x00000493
		[Conditional("DEBUG")]
		public static void IsTrue(bool condition, string message)
		{
			if (!condition)
			{
				Assert._Break();
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000229D File Offset: 0x0000049D
		[Conditional("DEBUG")]
		public static void Fail()
		{
			Assert._Break();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000229D File Offset: 0x0000049D
		[Conditional("DEBUG")]
		public static void Fail(string message)
		{
			Assert._Break();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022A4 File Offset: 0x000004A4
		[Conditional("DEBUG")]
		public static void IsNull<T>(T item) where T : class
		{
			if (item != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022B3 File Offset: 0x000004B3
		[Conditional("DEBUG")]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000022C2 File Offset: 0x000004C2
		[Conditional("DEBUG")]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000022D1 File Offset: 0x000004D1
		[Conditional("DEBUG")]
		public static void IsApartmentState(ApartmentState expectedState)
		{
			if (Thread.CurrentThread.GetApartmentState() != expectedState)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000022E5 File Offset: 0x000004E5
		[Conditional("DEBUG")]
		public static void NullableIsNotNull<T>(T? value) where T : struct
		{
			if (value == null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000022F5 File Offset: 0x000004F5
		[Conditional("DEBUG")]
		public static void NullableIsNull<T>(T? value) where T : struct
		{
			if (value != null)
			{
				Assert._Break();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002305 File Offset: 0x00000505
		[Conditional("DEBUG")]
		public static void IsNotOnMainThread()
		{
			if (Application.Current.Dispatcher.CheckAccess())
			{
				Assert._Break();
			}
		}

		// Token: 0x02000808 RID: 2056
		// (Invoke) Token: 0x06007E16 RID: 32278
		public delegate void EvaluateFunction();

		// Token: 0x02000809 RID: 2057
		// (Invoke) Token: 0x06007E1A RID: 32282
		public delegate bool ImplicationFunction();
	}
}

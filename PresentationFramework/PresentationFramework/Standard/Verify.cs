using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Standard
{
	// Token: 0x0200008D RID: 141
	internal static class Verify
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x00004236 File Offset: 0x00002436
		[DebuggerStepThrough]
		public static void IsApartmentState(ApartmentState requiredState, string message)
		{
			if (Thread.CurrentThread.GetApartmentState() != requiredState)
			{
				throw new InvalidOperationException(message);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000424C File Offset: 0x0000244C
		[DebuggerStepThrough]
		public static void IsNeitherNullNorEmpty(string value, string name)
		{
			if (value == null)
			{
				throw new ArgumentNullException(name, "The parameter can not be either null or empty.");
			}
			if ("" == value)
			{
				throw new ArgumentException("The parameter can not be either null or empty.", name);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00004276 File Offset: 0x00002476
		[DebuggerStepThrough]
		public static void IsNeitherNullNorWhitespace(string value, string name)
		{
			if (value == null)
			{
				throw new ArgumentNullException(name, "The parameter can not be either null or empty or consist only of white space characters.");
			}
			if ("" == value.Trim())
			{
				throw new ArgumentException("The parameter can not be either null or empty or consist only of white space characters.", name);
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000042A8 File Offset: 0x000024A8
		[DebuggerStepThrough]
		public static void IsNotDefault<T>(T obj, string name) where T : struct
		{
			T t = default(T);
			if (t.Equals(obj))
			{
				throw new ArgumentException("The parameter must not be the default value.", name);
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000042E0 File Offset: 0x000024E0
		[DebuggerStepThrough]
		public static void IsNotNull<T>(T obj, string name) where T : class
		{
			if (obj == null)
			{
				throw new ArgumentNullException(name);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000042F1 File Offset: 0x000024F1
		[DebuggerStepThrough]
		public static void IsNull<T>(T obj, string name) where T : class
		{
			if (obj != null)
			{
				throw new ArgumentException("The parameter must be null.", name);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00004307 File Offset: 0x00002507
		[DebuggerStepThrough]
		public static void PropertyIsNotNull<T>(T obj, string name) where T : class
		{
			if (obj == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The property {0} cannot be null at this time.", new object[]
				{
					name
				}));
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00004330 File Offset: 0x00002530
		[DebuggerStepThrough]
		public static void PropertyIsNull<T>(T obj, string name) where T : class
		{
			if (obj != null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The property {0} must be null at this time.", new object[]
				{
					name
				}));
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00004359 File Offset: 0x00002559
		[DebuggerStepThrough]
		public static void IsTrue(bool statement, string name)
		{
			if (!statement)
			{
				throw new ArgumentException("", name);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000436A File Offset: 0x0000256A
		[DebuggerStepThrough]
		public static void IsTrue(bool statement, string name, string message)
		{
			if (!statement)
			{
				throw new ArgumentException(message, name);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00004378 File Offset: 0x00002578
		[DebuggerStepThrough]
		public static void AreEqual<T>(T expected, T actual, string parameterName, string message)
		{
			if (expected == null)
			{
				if (actual != null && !actual.Equals(expected))
				{
					throw new ArgumentException(message, parameterName);
				}
			}
			else if (!expected.Equals(actual))
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x000043D0 File Offset: 0x000025D0
		[DebuggerStepThrough]
		public static void AreNotEqual<T>(T notExpected, T actual, string parameterName, string message)
		{
			if (notExpected == null)
			{
				if (actual == null || actual.Equals(notExpected))
				{
					throw new ArgumentException(message, parameterName);
				}
			}
			else if (notExpected.Equals(actual))
			{
				throw new ArgumentException(message, parameterName);
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00004427 File Offset: 0x00002627
		[DebuggerStepThrough]
		public static void UriIsAbsolute(Uri uri, string parameterName)
		{
			Verify.IsNotNull<Uri>(uri, parameterName);
			if (!uri.IsAbsoluteUri)
			{
				throw new ArgumentException("The URI must be absolute.", parameterName);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00004444 File Offset: 0x00002644
		[DebuggerStepThrough]
		public static void BoundedInteger(int lowerBoundInclusive, int value, int upperBoundExclusive, string parameterName)
		{
			if (value < lowerBoundInclusive || value >= upperBoundExclusive)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The integer value must be bounded with [{0}, {1})", new object[]
				{
					lowerBoundInclusive,
					upperBoundExclusive
				}), parameterName);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000447C File Offset: 0x0000267C
		[DebuggerStepThrough]
		public static void BoundedDoubleInc(double lowerBoundInclusive, double value, double upperBoundInclusive, string message, string parameter)
		{
			if (value < lowerBoundInclusive || value > upperBoundInclusive)
			{
				throw new ArgumentException(message, parameter);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000448F File Offset: 0x0000268F
		[DebuggerStepThrough]
		public static void TypeSupportsInterface(Type type, Type interfaceType, string parameterName)
		{
			Verify.IsNotNull<Type>(type, "type");
			Verify.IsNotNull<Type>(interfaceType, "interfaceType");
			if (type.GetInterface(interfaceType.Name) == null)
			{
				throw new ArgumentException("The type of this parameter does not support a required interface", parameterName);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000044C7 File Offset: 0x000026C7
		[DebuggerStepThrough]
		public static void FileExists(string filePath, string parameterName)
		{
			Verify.IsNeitherNullNorEmpty(filePath, parameterName);
			if (!File.Exists(filePath))
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No file exists at \"{0}\"", new object[]
				{
					filePath
				}), parameterName);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000044F8 File Offset: 0x000026F8
		[DebuggerStepThrough]
		internal static void ImplementsInterface(object parameter, Type interfaceType, string parameterName)
		{
			bool flag = false;
			foreach (Type left in parameter.GetType().GetInterfaces())
			{
				if (left == interfaceType)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The parameter must implement interface {0}.", new object[]
				{
					interfaceType.ToString()
				}), parameterName);
			}
		}
	}
}

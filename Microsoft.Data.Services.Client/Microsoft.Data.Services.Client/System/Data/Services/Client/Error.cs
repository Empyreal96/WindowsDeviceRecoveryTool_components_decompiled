using System;
using System.Linq.Expressions;

namespace System.Data.Services.Client
{
	// Token: 0x02000110 RID: 272
	internal static class Error
	{
		// Token: 0x060008D2 RID: 2258 RVA: 0x00024780 File Offset: 0x00022980
		internal static ArgumentException Argument(string message, string parameterName)
		{
			return Error.Trace<ArgumentException>(new ArgumentException(message, parameterName));
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002478E File Offset: 0x0002298E
		internal static InvalidOperationException InvalidOperation(string message)
		{
			return Error.Trace<InvalidOperationException>(new InvalidOperationException(message));
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002479B File Offset: 0x0002299B
		internal static InvalidOperationException InvalidOperation(string message, Exception innerException)
		{
			return Error.Trace<InvalidOperationException>(new InvalidOperationException(message, innerException));
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x000247A9 File Offset: 0x000229A9
		internal static NotSupportedException NotSupported(string message)
		{
			return Error.Trace<NotSupportedException>(new NotSupportedException(message));
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000247B6 File Offset: 0x000229B6
		internal static void ThrowObjectDisposed(Type type)
		{
			throw Error.Trace<ObjectDisposedException>(new ObjectDisposedException(type.ToString()));
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000247C8 File Offset: 0x000229C8
		internal static InvalidOperationException HttpHeaderFailure(int errorCode, string message)
		{
			return Error.Trace<InvalidOperationException>(new InvalidOperationException(message));
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000247D5 File Offset: 0x000229D5
		internal static NotSupportedException MethodNotSupported(MethodCallExpression m)
		{
			return Error.NotSupported(Strings.ALinq_MethodNotSupported(m.Method.Name));
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000247EC File Offset: 0x000229EC
		internal static void ThrowBatchUnexpectedContent(InternalError value)
		{
			throw Error.InvalidOperation(Strings.Batch_UnexpectedContent((int)value));
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000247FE File Offset: 0x000229FE
		internal static void ThrowBatchExpectedResponse(InternalError value)
		{
			throw Error.InvalidOperation(Strings.Batch_ExpectedResponse((int)value));
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00024810 File Offset: 0x00022A10
		internal static InvalidOperationException InternalError(InternalError value)
		{
			return Error.InvalidOperation(Strings.Context_InternalError((int)value));
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00024822 File Offset: 0x00022A22
		internal static void ThrowInternalError(InternalError value)
		{
			throw Error.InternalError(value);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002482A File Offset: 0x00022A2A
		private static T Trace<T>(T exception) where T : Exception
		{
			return exception;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002482D File Offset: 0x00022A2D
		internal static Exception ArgumentNull(string paramName)
		{
			return new ArgumentNullException(paramName);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00024835 File Offset: 0x00022A35
		internal static Exception ArgumentOutOfRange(string paramName)
		{
			return new ArgumentOutOfRangeException(paramName);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002483D File Offset: 0x00022A3D
		internal static Exception NotImplemented()
		{
			return new NotImplementedException();
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00024844 File Offset: 0x00022A44
		internal static Exception NotSupported()
		{
			return new NotSupportedException();
		}
	}
}

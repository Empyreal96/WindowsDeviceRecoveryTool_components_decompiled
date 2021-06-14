using System;
using Nokia.Lucid.Primitives;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x02000025 RID: 37
	internal static class RobustTrace
	{
		// Token: 0x06000128 RID: 296 RVA: 0x0000B00C File Offset: 0x0000920C
		public static void Trace(Action trace)
		{
			try
			{
				trace();
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000B040 File Offset: 0x00009240
		public static void Trace<TArg>(Action<TArg> trace, TArg arg)
		{
			try
			{
				trace(arg);
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000B074 File Offset: 0x00009274
		public static void Trace<TArg0, TArg1>(Action<TArg0, TArg1> trace, TArg0 arg0, TArg1 arg1)
		{
			try
			{
				trace(arg0, arg1);
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000B0A8 File Offset: 0x000092A8
		public static void Trace<TArg0, TArg1, TArg2>(Action<TArg0, TArg1, TArg2> trace, TArg0 arg0, TArg1 arg1, TArg2 arg2)
		{
			try
			{
				trace(arg0, arg1, arg2);
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000B0E0 File Offset: 0x000092E0
		public static void Trace<TArg0, TArg1, TArg2, TArg3>(Action<TArg0, TArg1, TArg2, TArg3> trace, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			try
			{
				trace(arg0, arg1, arg2, arg3);
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000B118 File Offset: 0x00009318
		public static void Trace<TArg0, TArg1, TArg2, TArg3, TArg4>(Action<TArg0, TArg1, TArg2, TArg3, TArg4> trace, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4)
		{
			try
			{
				trace(arg0, arg1, arg2, arg3, arg4);
			}
			catch (Exception exception)
			{
				if (ExceptionServices.IsCriticalException(exception))
				{
					throw;
				}
			}
		}
	}
}

using System;
using System.Globalization;
using System.IO;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200007C RID: 124
	internal static class ExtensionMethods
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00008665 File Offset: 0x00006865
		public static void WriteRoundtrippable(this TextWriter writer, double d)
		{
			writer.Write(d.ToString("R", CultureInfo.InvariantCulture));
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00008680 File Offset: 0x00006880
		internal static TResult? IfValidReturningNullable<TArg, TResult>(this TArg arg, Func<TArg, TResult> op) where TArg : class where TResult : struct
		{
			if (arg != null)
			{
				return new TResult?(op(arg));
			}
			return null;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000086AC File Offset: 0x000068AC
		internal static TResult IfValid<TArg, TResult>(this TArg arg, Func<TArg, TResult> op) where TArg : class where TResult : class
		{
			if (arg != null)
			{
				return op(arg);
			}
			return default(TResult);
		}
	}
}

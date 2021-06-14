using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Data.Edm.Internal
{
	// Token: 0x020000D3 RID: 211
	internal sealed class Memoizer<TArg, TResult>
	{
		// Token: 0x0600043E RID: 1086 RVA: 0x0000BB95 File Offset: 0x00009D95
		internal Memoizer(Func<TArg, TResult> function, IEqualityComparer<TArg> argComparer)
		{
			this.function = function;
			this.resultCache = new Dictionary<TArg, Memoizer<TArg, TResult>.Result>(argComparer);
			this.slimLock = new ReaderWriterLockSlim();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000BBDC File Offset: 0x00009DDC
		internal TResult Evaluate(TArg arg)
		{
			this.slimLock.EnterReadLock();
			Memoizer<TArg, TResult>.Result result;
			bool flag;
			try
			{
				flag = this.resultCache.TryGetValue(arg, out result);
			}
			finally
			{
				this.slimLock.ExitReadLock();
			}
			if (!flag)
			{
				this.slimLock.EnterWriteLock();
				try
				{
					if (!this.resultCache.TryGetValue(arg, out result))
					{
						result = new Memoizer<TArg, TResult>.Result(() => this.function(arg));
						this.resultCache.Add(arg, result);
					}
				}
				finally
				{
					this.slimLock.ExitWriteLock();
				}
			}
			return result.GetValue();
		}

		// Token: 0x0400019C RID: 412
		private readonly Func<TArg, TResult> function;

		// Token: 0x0400019D RID: 413
		private readonly Dictionary<TArg, Memoizer<TArg, TResult>.Result> resultCache;

		// Token: 0x0400019E RID: 414
		private readonly ReaderWriterLockSlim slimLock;

		// Token: 0x020000D4 RID: 212
		private class Result
		{
			// Token: 0x06000440 RID: 1088 RVA: 0x0000BCA8 File Offset: 0x00009EA8
			internal Result(Func<TResult> createValueDelegate)
			{
				this.createValueDelegate = createValueDelegate;
			}

			// Token: 0x06000441 RID: 1089 RVA: 0x0000BCB8 File Offset: 0x00009EB8
			internal TResult GetValue()
			{
				if (this.createValueDelegate == null)
				{
					return this.value;
				}
				TResult result;
				lock (this)
				{
					if (this.createValueDelegate == null)
					{
						result = this.value;
					}
					else
					{
						this.value = this.createValueDelegate();
						this.createValueDelegate = null;
						result = this.value;
					}
				}
				return result;
			}

			// Token: 0x0400019F RID: 415
			private TResult value;

			// Token: 0x040001A0 RID: 416
			private Func<TResult> createValueDelegate;
		}
	}
}

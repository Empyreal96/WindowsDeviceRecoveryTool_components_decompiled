using System;

namespace System.Data.Services.Client
{
	// Token: 0x02000007 RID: 7
	internal sealed class SimpleLazy<T>
	{
		// Token: 0x06000021 RID: 33 RVA: 0x000026DD File Offset: 0x000008DD
		internal SimpleLazy(Func<T> factory) : this(factory, false)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000026E7 File Offset: 0x000008E7
		internal SimpleLazy(Func<T> factory, bool isThreadSafe)
		{
			this.factory = factory;
			this.valueCreated = false;
			if (isThreadSafe)
			{
				this.mutex = new object();
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000270C File Offset: 0x0000090C
		internal T Value
		{
			get
			{
				if (!this.valueCreated)
				{
					if (this.mutex != null)
					{
						lock (this.mutex)
						{
							if (!this.valueCreated)
							{
								this.CreateValue();
							}
							goto IL_41;
						}
					}
					this.CreateValue();
				}
				IL_41:
				return this.value;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002770 File Offset: 0x00000970
		private void CreateValue()
		{
			this.value = this.factory();
			this.valueCreated = true;
		}

		// Token: 0x04000001 RID: 1
		private readonly object mutex;

		// Token: 0x04000002 RID: 2
		private readonly Func<T> factory;

		// Token: 0x04000003 RID: 3
		private T value;

		// Token: 0x04000004 RID: 4
		private bool valueCreated;
	}
}

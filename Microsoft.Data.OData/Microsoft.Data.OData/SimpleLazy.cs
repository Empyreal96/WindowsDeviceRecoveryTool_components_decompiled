using System;

namespace Microsoft.Data.OData
{
	// Token: 0x0200014D RID: 333
	internal sealed class SimpleLazy<T>
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x0001CBED File Offset: 0x0001ADED
		internal SimpleLazy(Func<T> factory) : this(factory, false)
		{
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0001CBF7 File Offset: 0x0001ADF7
		internal SimpleLazy(Func<T> factory, bool isThreadSafe)
		{
			this.factory = factory;
			this.valueCreated = false;
			if (isThreadSafe)
			{
				this.mutex = new object();
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0001CC1C File Offset: 0x0001AE1C
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

		// Token: 0x0600090D RID: 2317 RVA: 0x0001CC80 File Offset: 0x0001AE80
		private void CreateValue()
		{
			this.value = this.factory();
			this.valueCreated = true;
		}

		// Token: 0x0400035E RID: 862
		private readonly object mutex;

		// Token: 0x0400035F RID: 863
		private readonly Func<T> factory;

		// Token: 0x04000360 RID: 864
		private T value;

		// Token: 0x04000361 RID: 865
		private bool valueCreated;
	}
}

using System;

namespace System.Management
{
	// Token: 0x02000027 RID: 39
	internal class WmiDelegateInvoker
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00007EDD File Offset: 0x000060DD
		internal WmiDelegateInvoker(object sender)
		{
			this.sender = sender;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007EEC File Offset: 0x000060EC
		internal void FireEventToDelegates(MulticastDelegate md, ManagementEventArgs args)
		{
			try
			{
				if (md != null)
				{
					foreach (Delegate @delegate in md.GetInvocationList())
					{
						try
						{
							@delegate.DynamicInvoke(new object[]
							{
								this.sender,
								args
							});
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000120 RID: 288
		internal object sender;
	}
}

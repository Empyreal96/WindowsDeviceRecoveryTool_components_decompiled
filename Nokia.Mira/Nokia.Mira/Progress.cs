using System;

namespace Nokia.Mira
{
	// Token: 0x02000020 RID: 32
	public sealed class Progress<T> : IProgress<T>
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00002E8E File Offset: 0x0000108E
		public Progress(Action<T> action)
		{
			this.action = action;
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002EAB File Offset: 0x000010AB
		public void Report(T value)
		{
			this.action(value);
		}

		// Token: 0x0400003C RID: 60
		private readonly Action<T> action;
	}
}

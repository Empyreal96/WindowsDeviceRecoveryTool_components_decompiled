using System;

namespace SoftwareRepository.Streaming
{
	// Token: 0x0200000F RID: 15
	public class DownloadProgress<T> : IProgress<T>
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000030DE File Offset: 0x000012DE
		public DownloadProgress(Action<T> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			this.action = action;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000030FB File Offset: 0x000012FB
		public void Report(T value)
		{
			this.action(value);
		}

		// Token: 0x0400003D RID: 61
		private readonly Action<T> action;
	}
}

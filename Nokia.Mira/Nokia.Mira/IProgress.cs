using System;

namespace Nokia.Mira
{
	// Token: 0x0200001F RID: 31
	public interface IProgress<in T>
	{
		// Token: 0x06000082 RID: 130
		void Report(T value);
	}
}

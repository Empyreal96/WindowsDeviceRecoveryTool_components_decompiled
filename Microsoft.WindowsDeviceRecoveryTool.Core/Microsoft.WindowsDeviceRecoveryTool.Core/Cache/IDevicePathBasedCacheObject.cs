using System;
using System.Threading.Tasks;

namespace Microsoft.WindowsDeviceRecoveryTool.Core.Cache
{
	// Token: 0x02000005 RID: 5
	public interface IDevicePathBasedCacheObject
	{
		// Token: 0x06000009 RID: 9
		bool TryGetReadInformationTaskForReader<T>(Type readerType, out Task<T> readInformationTask);

		// Token: 0x0600000A RID: 10
		void AddReadInformationTaskForReader<T>(Type readerType, Task<T> readInformationTask);
	}
}

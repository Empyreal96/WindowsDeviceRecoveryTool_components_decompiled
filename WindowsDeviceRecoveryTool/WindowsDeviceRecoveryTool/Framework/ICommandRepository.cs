using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x020000E5 RID: 229
	public interface ICommandRepository : IDictionary<string, IDelegateCommand>, ICollection<KeyValuePair<string, IDelegateCommand>>, IEnumerable<KeyValuePair<string, IDelegateCommand>>, IEnumerable
	{
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;

namespace Microsoft.WindowsDeviceRecoveryTool.Framework
{
	// Token: 0x020000E6 RID: 230
	[Export(typeof(ICommandRepository))]
	[Serializable]
	public class CommandRepository : Dictionary<string, IDelegateCommand>, ICommandRepository, IDictionary<string, IDelegateCommand>, ICollection<KeyValuePair<string, IDelegateCommand>>, IEnumerable<KeyValuePair<string, IDelegateCommand>>, IEnumerable
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x00027643 File Offset: 0x00025843
		public CommandRepository()
		{
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002764E File Offset: 0x0002584E
		protected CommandRepository(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}

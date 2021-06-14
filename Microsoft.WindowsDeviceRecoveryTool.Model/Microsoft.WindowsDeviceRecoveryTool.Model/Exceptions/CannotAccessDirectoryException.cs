using System;

namespace Microsoft.WindowsDeviceRecoveryTool.Model.Exceptions
{
	// Token: 0x02000034 RID: 52
	[Serializable]
	public class CannotAccessDirectoryException : Exception
	{
		// Token: 0x0600016D RID: 365 RVA: 0x00004B39 File Offset: 0x00002D39
		public CannotAccessDirectoryException(string path) : base(path)
		{
		}
	}
}

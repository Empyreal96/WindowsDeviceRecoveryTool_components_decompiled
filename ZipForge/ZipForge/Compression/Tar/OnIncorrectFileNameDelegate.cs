using System;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x02000059 RID: 89
	// (Invoke) Token: 0x060003BE RID: 958
	public delegate void OnIncorrectFileNameDelegate(object sender, string sourceFileName, ref string destFileName, ref bool cancel);
}

using System;

namespace ComponentAce.Compression.Tar
{
	// Token: 0x0200005A RID: 90
	// (Invoke) Token: 0x060003C2 RID: 962
	public delegate void OnIncorrectStorePathDelegate(object sender, string sourceFileName, ref string destFileName, ref bool cancel);
}

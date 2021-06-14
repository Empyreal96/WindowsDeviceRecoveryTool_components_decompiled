using System;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000788 RID: 1928
	[Serializable]
	internal class JournalEntryUri : JournalEntry, ISerializable
	{
		// Token: 0x06007954 RID: 31060 RVA: 0x0022718B File Offset: 0x0022538B
		internal JournalEntryUri(JournalEntryGroupState jeGroupState, Uri uri) : base(jeGroupState, uri)
		{
		}

		// Token: 0x06007955 RID: 31061 RVA: 0x00227195 File Offset: 0x00225395
		protected JournalEntryUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06007956 RID: 31062 RVA: 0x0022719F File Offset: 0x0022539F
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(base.Source != null, "Can't journal by Uri without a Uri.");
			base.SaveState(contentObject);
		}
	}
}

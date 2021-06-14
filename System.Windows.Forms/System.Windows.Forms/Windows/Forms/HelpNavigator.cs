using System;

namespace System.Windows.Forms
{
	/// <summary>Specifies constants indicating which elements of the Help file to display.</summary>
	// Token: 0x02000264 RID: 612
	public enum HelpNavigator
	{
		/// <summary>The Help file opens to a specified topic, if the topic exists.</summary>
		// Token: 0x04000FD9 RID: 4057
		Topic = -2147483647,
		/// <summary>The Help file opens to the table of contents.</summary>
		// Token: 0x04000FDA RID: 4058
		TableOfContents,
		/// <summary>The Help file opens to the index.</summary>
		// Token: 0x04000FDB RID: 4059
		Index,
		/// <summary>The Help file opens to the search page.</summary>
		// Token: 0x04000FDC RID: 4060
		Find,
		/// <summary>The Help file opens to the index entry for the first letter of a specified topic.</summary>
		// Token: 0x04000FDD RID: 4061
		AssociateIndex,
		/// <summary>The Help file opens to the topic with the specified index entry, if one exists; otherwise, the index entry closest to the specified keyword is displayed.</summary>
		// Token: 0x04000FDE RID: 4062
		KeywordIndex,
		/// <summary>The Help file opens to a topic indicated by a numeric topic identifier.</summary>
		// Token: 0x04000FDF RID: 4063
		TopicId
	}
}

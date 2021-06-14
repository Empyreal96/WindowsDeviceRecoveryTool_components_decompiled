using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000760 RID: 1888
	internal class ContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x0600783C RID: 30780 RVA: 0x002241F1 File Offset: 0x002223F1
		internal ContentModelTreeEnumerator(ContentControl contentControl, object content) : base(content)
		{
			this._owner = contentControl;
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x0600783D RID: 30781 RVA: 0x00224201 File Offset: 0x00222401
		protected override bool IsUnchanged
		{
			get
			{
				return base.Content == this._owner.Content;
			}
		}

		// Token: 0x040038EB RID: 14571
		private ContentControl _owner;
	}
}

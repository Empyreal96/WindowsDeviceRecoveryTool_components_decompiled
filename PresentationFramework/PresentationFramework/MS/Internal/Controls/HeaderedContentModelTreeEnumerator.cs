using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000761 RID: 1889
	internal class HeaderedContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x0600783E RID: 30782 RVA: 0x00224216 File Offset: 0x00222416
		internal HeaderedContentModelTreeEnumerator(HeaderedContentControl headeredContentControl, object content, object header) : base(header)
		{
			this._owner = headeredContentControl;
			this._content = content;
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x0600783F RID: 30783 RVA: 0x0022422D File Offset: 0x0022242D
		protected override object Current
		{
			get
			{
				if (base.Index == 1 && this._content != null)
				{
					return this._content;
				}
				return base.Current;
			}
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x00224250 File Offset: 0x00222450
		protected override bool MoveNext()
		{
			if (this._content != null)
			{
				if (base.Index == 0)
				{
					int index = base.Index;
					base.Index = index + 1;
					base.VerifyUnchanged();
					return true;
				}
				if (base.Index == 1)
				{
					int index = base.Index;
					base.Index = index + 1;
					return false;
				}
			}
			return base.MoveNext();
		}

		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x06007841 RID: 30785 RVA: 0x002242A8 File Offset: 0x002224A8
		protected override bool IsUnchanged
		{
			get
			{
				object content = base.Content;
				return content == this._owner.Header && this._content == this._owner.Content;
			}
		}

		// Token: 0x040038EC RID: 14572
		private HeaderedContentControl _owner;

		// Token: 0x040038ED RID: 14573
		private object _content;
	}
}

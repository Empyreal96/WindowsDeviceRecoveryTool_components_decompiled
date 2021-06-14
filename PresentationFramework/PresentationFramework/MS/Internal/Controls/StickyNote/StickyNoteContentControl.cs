using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x0200076A RID: 1898
	internal abstract class StickyNoteContentControl
	{
		// Token: 0x0600786D RID: 30829 RVA: 0x002251A7 File Offset: 0x002233A7
		protected StickyNoteContentControl(FrameworkElement innerControl)
		{
			this.SetInnerControl(innerControl);
		}

		// Token: 0x0600786E RID: 30830 RVA: 0x0000326D File Offset: 0x0000146D
		private StickyNoteContentControl()
		{
		}

		// Token: 0x0600786F RID: 30831
		public abstract void Save(XmlNode node);

		// Token: 0x06007870 RID: 30832
		public abstract void Load(XmlNode node);

		// Token: 0x06007871 RID: 30833
		public abstract void Clear();

		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x06007872 RID: 30834
		public abstract bool IsEmpty { get; }

		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x06007873 RID: 30835
		public abstract StickyNoteType Type { get; }

		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x06007874 RID: 30836 RVA: 0x002251B6 File Offset: 0x002233B6
		public FrameworkElement InnerControl
		{
			get
			{
				return this._innerControl;
			}
		}

		// Token: 0x06007875 RID: 30837 RVA: 0x002251BE File Offset: 0x002233BE
		protected void SetInnerControl(FrameworkElement innerControl)
		{
			this._innerControl = innerControl;
		}

		// Token: 0x04003919 RID: 14617
		protected FrameworkElement _innerControl;

		// Token: 0x0400391A RID: 14618
		protected const long MaxBufferSize = 1610612733L;
	}
}

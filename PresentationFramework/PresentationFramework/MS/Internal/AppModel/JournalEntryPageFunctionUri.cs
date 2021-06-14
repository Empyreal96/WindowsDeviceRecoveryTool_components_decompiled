using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078E RID: 1934
	[Serializable]
	internal class JournalEntryPageFunctionUri : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x0600797A RID: 31098 RVA: 0x00227671 File Offset: 0x00225871
		internal JournalEntryPageFunctionUri(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction, Uri markupUri) : base(jeGroupState, pageFunction)
		{
			this._markupUri = markupUri;
		}

		// Token: 0x0600797B RID: 31099 RVA: 0x00227682 File Offset: 0x00225882
		protected JournalEntryPageFunctionUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._markupUri = (Uri)info.GetValue("_markupUri", typeof(Uri));
		}

		// Token: 0x0600797C RID: 31100 RVA: 0x002276AC File Offset: 0x002258AC
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_markupUri", this._markupUri);
		}

		// Token: 0x0600797D RID: 31101 RVA: 0x002276C8 File Offset: 0x002258C8
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase pageFunctionBase = Application.LoadComponent(this._markupUri, true) as PageFunctionBase;
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x04003986 RID: 14726
		private Uri _markupUri;
	}
}

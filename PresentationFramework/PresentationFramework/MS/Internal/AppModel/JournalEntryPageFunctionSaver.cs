using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078C RID: 1932
	[Serializable]
	internal abstract class JournalEntryPageFunctionSaver : JournalEntryPageFunction, ISerializable
	{
		// Token: 0x0600796E RID: 31086 RVA: 0x002273E6 File Offset: 0x002255E6
		internal JournalEntryPageFunctionSaver(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
		}

		// Token: 0x0600796F RID: 31087 RVA: 0x002273F0 File Offset: 0x002255F0
		protected JournalEntryPageFunctionSaver(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._returnEventSaver = (ReturnEventSaver)info.GetValue("_returnEventSaver", typeof(ReturnEventSaver));
		}

		// Token: 0x06007970 RID: 31088 RVA: 0x0022741A File Offset: 0x0022561A
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_returnEventSaver", this._returnEventSaver);
		}

		// Token: 0x06007971 RID: 31089 RVA: 0x00227438 File Offset: 0x00225638
		internal override void SaveState(object contentObject)
		{
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			this._returnEventSaver = pageFunctionBase._Saver;
			base.SaveState(contentObject);
		}

		// Token: 0x06007972 RID: 31090 RVA: 0x00227460 File Offset: 0x00225660
		internal override void RestoreState(object contentObject)
		{
			if (contentObject == null)
			{
				throw new ArgumentNullException("contentObject");
			}
			PageFunctionBase pageFunctionBase = (PageFunctionBase)contentObject;
			if (pageFunctionBase == null)
			{
				throw new Exception(SR.Get("InvalidPageFunctionType", new object[]
				{
					contentObject.GetType()
				}));
			}
			pageFunctionBase.ParentPageFunctionId = base.ParentPageFunctionId;
			pageFunctionBase.PageFunctionId = base.PageFunctionId;
			pageFunctionBase._Saver = this._returnEventSaver;
			pageFunctionBase._Resume = true;
			base.RestoreState(pageFunctionBase);
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x002274D8 File Offset: 0x002256D8
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			IDownloader downloader = navigator as IDownloader;
			NavigationService navigationService = (downloader != null) ? downloader.Downloader : null;
			PageFunctionBase content = (navigationService != null && navigationService.ContentId == base.ContentId) ? ((PageFunctionBase)navigationService.Content) : this.ResumePageFunction();
			return navigator.Navigate(content, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x04003984 RID: 14724
		private ReturnEventSaver _returnEventSaver;
	}
}

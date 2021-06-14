using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Utility;

namespace MS.Internal.AppModel
{
	// Token: 0x02000797 RID: 1943
	internal sealed class RequestSetStatusBarEventArgs : RoutedEventArgs
	{
		// Token: 0x060079D9 RID: 31193 RVA: 0x00228867 File Offset: 0x00226A67
		[SecurityCritical]
		internal RequestSetStatusBarEventArgs(string text)
		{
			this._text.Value = text;
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x060079DA RID: 31194 RVA: 0x00228886 File Offset: 0x00226A86
		[SecurityCritical]
		internal RequestSetStatusBarEventArgs(Uri targetUri)
		{
			if (targetUri == null)
			{
				this._text.Value = string.Empty;
			}
			else
			{
				this._text.Value = BindUriHelper.UriToString(targetUri);
			}
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x060079DB RID: 31195 RVA: 0x002288C5 File Offset: 0x00226AC5
		internal string Text
		{
			get
			{
				return this._text.Value;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x060079DC RID: 31196 RVA: 0x002288D2 File Offset: 0x00226AD2
		internal static RequestSetStatusBarEventArgs Clear
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return new RequestSetStatusBarEventArgs(string.Empty);
			}
		}

		// Token: 0x040039A0 RID: 14752
		private SecurityCriticalDataForSet<string> _text;
	}
}

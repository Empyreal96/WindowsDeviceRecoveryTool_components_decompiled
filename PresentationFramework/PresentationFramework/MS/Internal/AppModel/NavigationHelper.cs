using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace MS.Internal.AppModel
{
	// Token: 0x02000791 RID: 1937
	internal static class NavigationHelper
	{
		// Token: 0x060079B5 RID: 31157 RVA: 0x00227DA0 File Offset: 0x00225FA0
		internal static Visual FindRootViewer(ContentControl navigator, string contentPresenterName)
		{
			object content = navigator.Content;
			if (content == null || content is Visual)
			{
				return content as Visual;
			}
			ContentPresenter contentPresenter = null;
			if (navigator.Template != null)
			{
				contentPresenter = (ContentPresenter)navigator.Template.FindName(contentPresenterName, navigator);
			}
			if (contentPresenter == null || contentPresenter.InternalVisualChildrenCount == 0)
			{
				return null;
			}
			return contentPresenter.InternalGetVisualChild(0);
		}
	}
}

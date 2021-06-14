using System;
using System.Windows.Navigation;

namespace System.Windows.Documents
{
	// Token: 0x02000352 RID: 850
	internal static class FixedHyperLink
	{
		// Token: 0x06002D44 RID: 11588 RVA: 0x000CC618 File Offset: 0x000CA818
		public static void OnNavigationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FixedDocument fixedDocument = d as FixedDocument;
			if (fixedDocument != null)
			{
				NavigationService navigationService = (NavigationService)e.OldValue;
				NavigationService navigationService2 = (NavigationService)e.NewValue;
				if (navigationService != null)
				{
					navigationService.FragmentNavigation -= FixedHyperLink.FragmentHandler;
				}
				if (navigationService2 != null)
				{
					navigationService2.FragmentNavigation += FixedHyperLink.FragmentHandler;
				}
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x000CC674 File Offset: 0x000CA874
		internal static void FragmentHandler(object sender, FragmentNavigationEventArgs e)
		{
			NavigationService navigationService = sender as NavigationService;
			if (navigationService != null)
			{
				string fragment = e.Fragment;
				IFixedNavigate fixedNavigate = navigationService.Content as IFixedNavigate;
				if (fixedNavigate != null)
				{
					fixedNavigate.NavigateAsync(e.Fragment);
					e.Handled = true;
				}
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x000CC6B4 File Offset: 0x000CA8B4
		internal static void NavigateToElement(object ElementHost, string elementID)
		{
			FixedPage fixedPage = null;
			FrameworkElement frameworkElement = ((IFixedNavigate)ElementHost).FindElementByID(elementID, out fixedPage) as FrameworkElement;
			if (frameworkElement != null)
			{
				if (frameworkElement is FixedPage)
				{
					frameworkElement.BringIntoView();
					return;
				}
				frameworkElement.BringIntoView(frameworkElement.VisualContentBounds);
			}
		}
	}
}

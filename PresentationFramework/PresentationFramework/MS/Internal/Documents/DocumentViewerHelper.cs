using System;
using System.Globalization;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.PresentationFramework;

namespace MS.Internal.Documents
{
	// Token: 0x020006C3 RID: 1731
	internal static class DocumentViewerHelper
	{
		// Token: 0x06006FD5 RID: 28629 RVA: 0x00202218 File Offset: 0x00200418
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ToggleFindToolBar(Decorator findToolBarHost, EventHandler handlerFindClicked, bool enable)
		{
			if (enable)
			{
				FindToolBar findToolBar = new FindToolBar();
				findToolBarHost.Child = findToolBar;
				findToolBarHost.Visibility = Visibility.Visible;
				KeyboardNavigation.SetTabNavigation(findToolBarHost, KeyboardNavigationMode.Continue);
				FocusManager.SetIsFocusScope(findToolBarHost, true);
				findToolBar.SetResourceReference(FrameworkElement.StyleProperty, DocumentViewerHelper.FindToolBarStyleKey);
				findToolBar.FindClicked += handlerFindClicked;
				findToolBar.DocumentLoaded = true;
				findToolBar.GoToTextBox();
				return;
			}
			FindToolBar findToolBar2 = findToolBarHost.Child as FindToolBar;
			findToolBar2.FindClicked -= handlerFindClicked;
			findToolBar2.DocumentLoaded = false;
			findToolBarHost.Child = null;
			findToolBarHost.Visibility = Visibility.Collapsed;
			KeyboardNavigation.SetTabNavigation(findToolBarHost, KeyboardNavigationMode.None);
			findToolBarHost.ClearValue(FocusManager.IsFocusScopeProperty);
		}

		// Token: 0x06006FD6 RID: 28630 RVA: 0x002022AC File Offset: 0x002004AC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static ITextRange Find(FindToolBar findToolBar, TextEditor textEditor, ITextView textView, ITextView masterPageTextView)
		{
			ITextPointer textPointer = null;
			Invariant.Assert(findToolBar != null);
			Invariant.Assert(textEditor != null);
			FindFlags findFlags = FindFlags.None;
			findFlags |= (findToolBar.SearchUp ? FindFlags.FindInReverse : FindFlags.None);
			findFlags |= (findToolBar.MatchCase ? FindFlags.MatchCase : FindFlags.None);
			findFlags |= (findToolBar.MatchWholeWord ? FindFlags.FindWholeWordsOnly : FindFlags.None);
			findFlags |= (findToolBar.MatchDiacritic ? FindFlags.MatchDiacritics : FindFlags.None);
			findFlags |= (findToolBar.MatchKashida ? FindFlags.MatchKashida : FindFlags.None);
			findFlags |= (findToolBar.MatchAlefHamza ? FindFlags.MatchAlefHamza : FindFlags.None);
			ITextContainer textContainer = textEditor.TextContainer;
			ITextRange selection = textEditor.Selection;
			string searchText = findToolBar.SearchText;
			CultureInfo documentCultureInfo = DocumentViewerHelper.GetDocumentCultureInfo(textContainer);
			ITextPointer textPointer2;
			ITextPointer textPointer3;
			ITextRange textRange;
			if (selection.IsEmpty)
			{
				if (textView != null && !textView.IsValid)
				{
					textView = null;
				}
				if (textView != null && textView.Contains(selection.Start))
				{
					textPointer2 = (findToolBar.SearchUp ? textContainer.Start : selection.Start);
					textPointer3 = (findToolBar.SearchUp ? selection.Start : textContainer.End);
				}
				else
				{
					if (masterPageTextView != null && masterPageTextView.IsValid)
					{
						foreach (TextSegment textSegment in masterPageTextView.TextSegments)
						{
							if (!textSegment.IsNull)
							{
								if (textPointer == null)
								{
									textPointer = ((!findToolBar.SearchUp) ? textSegment.Start : textSegment.End);
								}
								else if (!findToolBar.SearchUp)
								{
									if (textSegment.Start.CompareTo(textPointer) < 0)
									{
										textPointer = textSegment.Start;
									}
								}
								else if (textSegment.End.CompareTo(textPointer) > 0)
								{
									textPointer = textSegment.End;
								}
							}
						}
					}
					if (textPointer != null)
					{
						textPointer2 = (findToolBar.SearchUp ? textContainer.Start : textPointer);
						textPointer3 = (findToolBar.SearchUp ? textPointer : textContainer.End);
					}
					else
					{
						textPointer2 = textContainer.Start;
						textPointer3 = textContainer.End;
					}
				}
			}
			else
			{
				textRange = TextFindEngine.Find(selection.Start, selection.End, searchText, findFlags, documentCultureInfo);
				if (textRange != null && textRange.Start != null && textRange.Start.CompareTo(selection.Start) == 0 && textRange.End.CompareTo(selection.End) == 0)
				{
					textPointer2 = (findToolBar.SearchUp ? selection.Start : selection.End);
					textPointer3 = (findToolBar.SearchUp ? textContainer.Start : textContainer.End);
				}
				else
				{
					textPointer2 = (findToolBar.SearchUp ? selection.End : selection.Start);
					textPointer3 = (findToolBar.SearchUp ? textContainer.Start : textContainer.End);
				}
			}
			textRange = null;
			if (textPointer2 != null && textPointer3 != null && textPointer2.CompareTo(textPointer3) != 0)
			{
				if (textPointer2.CompareTo(textPointer3) > 0)
				{
					ITextPointer textPointer4 = textPointer2;
					textPointer2 = textPointer3;
					textPointer3 = textPointer4;
				}
				textRange = TextFindEngine.Find(textPointer2, textPointer3, searchText, findFlags, documentCultureInfo);
				if (textRange != null && !textRange.IsEmpty)
				{
					selection.Select(textRange.Start, textRange.End);
				}
			}
			return textRange;
		}

		// Token: 0x06006FD7 RID: 28631 RVA: 0x002025B8 File Offset: 0x002007B8
		private static CultureInfo GetDocumentCultureInfo(ITextContainer textContainer)
		{
			CultureInfo cultureInfo = null;
			if (textContainer.Parent != null)
			{
				XmlLanguage xmlLanguage = (XmlLanguage)textContainer.Parent.GetValue(FrameworkElement.LanguageProperty);
				if (xmlLanguage != null)
				{
					try
					{
						cultureInfo = xmlLanguage.GetSpecificCulture();
					}
					catch (InvalidOperationException)
					{
						cultureInfo = null;
					}
				}
			}
			if (cultureInfo == null)
			{
				cultureInfo = CultureInfo.CurrentCulture;
			}
			return cultureInfo;
		}

		// Token: 0x06006FD8 RID: 28632 RVA: 0x00202610 File Offset: 0x00200810
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ShowFindUnsuccessfulMessage(FindToolBar findToolBar)
		{
			string text = findToolBar.SearchUp ? SR.Get("DocumentViewerSearchUpCompleteLabel") : SR.Get("DocumentViewerSearchDownCompleteLabel");
			text = string.Format(CultureInfo.CurrentCulture, text, new object[]
			{
				findToolBar.SearchText
			});
			HwndSource hwndSource = PresentationSource.CriticalFromVisual(findToolBar) as HwndSource;
			IntPtr parentHwnd = (hwndSource != null) ? hwndSource.CriticalHandle : IntPtr.Zero;
			SecurityHelper.ShowMessageBoxHelper(parentHwnd, text, SR.Get("DocumentViewerSearchCompleteTitle"), MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}

		// Token: 0x17001A92 RID: 6802
		// (get) Token: 0x06006FD9 RID: 28633 RVA: 0x00202688 File Offset: 0x00200888
		private static ResourceKey FindToolBarStyleKey
		{
			get
			{
				if (DocumentViewerHelper._findToolBarStyleKey == null)
				{
					DocumentViewerHelper._findToolBarStyleKey = new ComponentResourceKey(typeof(PresentationUIStyleResources), "PUIFlowViewers_FindToolBar");
				}
				return DocumentViewerHelper._findToolBarStyleKey;
			}
		}

		// Token: 0x06006FDA RID: 28634 RVA: 0x002026AF File Offset: 0x002008AF
		internal static bool IsLogicalDescendent(DependencyObject child, DependencyObject parent)
		{
			while (child != null)
			{
				if (child == parent)
				{
					return true;
				}
				child = LogicalTreeHelper.GetParent(child);
			}
			return false;
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x002026C8 File Offset: 0x002008C8
		internal static void KeyDownHelper(KeyEventArgs e, DependencyObject findToolBarHost)
		{
			if (!e.Handled && findToolBarHost != null && (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down))
			{
				DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
				if (dependencyObject != null && dependencyObject is Visual && VisualTreeHelper.IsAncestorOf(findToolBarHost, dependencyObject))
				{
					FocusNavigationDirection direction = KeyboardNavigation.KeyToTraversalDirection(e.Key);
					DependencyObject dependencyObject2 = KeyboardNavigation.Current.PredictFocusedElement(dependencyObject, direction);
					if (dependencyObject2 != null && dependencyObject2 is IInputElement && VisualTreeHelper.IsAncestorOf(findToolBarHost, dependencyObject))
					{
						((IInputElement)dependencyObject2).Focus();
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x00202770 File Offset: 0x00200970
		internal static void OnContextMenuOpening(FlowDocument document, Control viewer, ContextMenuEventArgs e)
		{
			ContextMenu contextMenu = null;
			if (e.TargetElement != null)
			{
				contextMenu = (e.TargetElement.GetValue(FrameworkElement.ContextMenuProperty) as ContextMenu);
			}
			if (contextMenu == null)
			{
				contextMenu = viewer.ContextMenu;
			}
			if (contextMenu != null)
			{
				if (document != null && DoubleUtil.LessThan(e.CursorLeft, 0.0))
				{
					ITextContainer textContainer = (ITextContainer)((IServiceProvider)document).GetService(typeof(ITextContainer));
					ITextPointer textPointer = null;
					if (textContainer.TextSelection != null)
					{
						if ((textContainer.TextSelection.IsEmpty || !textContainer.TextSelection.TextEditor.UiScope.IsFocused) && e.TargetElement is TextElement)
						{
							textPointer = ((TextElement)e.TargetElement).ContentStart;
						}
						else
						{
							textPointer = textContainer.TextSelection.Start.CreatePointer(LogicalDirection.Forward);
						}
					}
					else if (e.TargetElement is TextElement)
					{
						textPointer = ((TextElement)e.TargetElement).ContentStart;
					}
					ITextView textView = textContainer.TextView;
					if (textPointer != null && textView != null && textView.IsValid && textView.Contains(textPointer))
					{
						Rect rect = textView.GetRectangleFromTextPosition(textPointer);
						if (rect != Rect.Empty)
						{
							rect = DocumentViewerHelper.CalculateVisibleRect(rect, textView.RenderScope);
							if (rect != Rect.Empty)
							{
								GeneralTransform generalTransform = textView.RenderScope.TransformToAncestor(viewer);
								Point point = generalTransform.Transform(rect.BottomLeft);
								contextMenu.Placement = PlacementMode.Relative;
								contextMenu.PlacementTarget = viewer;
								contextMenu.HorizontalOffset = point.X;
								contextMenu.VerticalOffset = point.Y;
								contextMenu.IsOpen = true;
								e.Handled = true;
							}
						}
					}
				}
				if (!e.Handled)
				{
					contextMenu.ClearValue(ContextMenu.PlacementProperty);
					contextMenu.ClearValue(ContextMenu.PlacementTargetProperty);
					contextMenu.ClearValue(ContextMenu.HorizontalOffsetProperty);
					contextMenu.ClearValue(ContextMenu.VerticalOffsetProperty);
				}
			}
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x00202948 File Offset: 0x00200B48
		internal static Rect CalculateVisibleRect(Rect visibleRect, Visual originalVisual)
		{
			Visual visual = VisualTreeHelper.GetParent(originalVisual) as Visual;
			while (visual != null && visibleRect != Rect.Empty)
			{
				if (VisualTreeHelper.GetClip(visual) != null)
				{
					GeneralTransform inverse = originalVisual.TransformToAncestor(visual).Inverse;
					if (inverse != null)
					{
						Rect rect = VisualTreeHelper.GetClip(visual).Bounds;
						rect = inverse.TransformBounds(rect);
						visibleRect.Intersect(rect);
					}
					else
					{
						visibleRect = Rect.Empty;
					}
				}
				visual = (VisualTreeHelper.GetParent(visual) as Visual);
			}
			return visibleRect;
		}

		// Token: 0x040036D6 RID: 14038
		private static ResourceKey _findToolBarStyleKey;
	}
}

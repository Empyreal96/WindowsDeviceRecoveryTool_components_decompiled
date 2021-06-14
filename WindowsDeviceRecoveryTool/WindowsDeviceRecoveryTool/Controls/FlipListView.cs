using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.WindowsDeviceRecoveryTool.Controls
{
	// Token: 0x02000014 RID: 20
	[TemplatePart(Name = "PART_SwitchNextButton", Type = typeof(Button))]
	[TemplatePart(Name = "PART_ItemsPresenter", Type = typeof(ItemsPresenter))]
	[TemplatePart(Name = "PART_ItemsScrollViewer", Type = typeof(ScrollViewer))]
	[TemplateVisualState(Name = "ShowingPage", GroupName = "FlipStates")]
	[TemplatePart(Name = "PART_SwitchNextButton", Type = typeof(Button))]
	[TemplateVisualState(Name = "FlipNext", GroupName = "FlipStates")]
	[TemplateVisualState(Name = "FlipPrevious", GroupName = "FlipStates")]
	public class FlipListView : ListView
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000039EC File Offset: 0x00001BEC
		static FlipListView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipListView), new FrameworkPropertyMetadata(typeof(FlipListView)));
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003AFB File Offset: 0x00001CFB
		public FlipListView()
		{
			base.Loaded += this.OnLoaded;
			base.LayoutUpdated += this.OnLayoutUpdated;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003B2C File Offset: 0x00001D2C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FlipListView.FlipListViewAutomationPeer(this);
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003B44 File Offset: 0x00001D44
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003B66 File Offset: 0x00001D66
		public Orientation ItemsOrientantion
		{
			get
			{
				return (Orientation)base.GetValue(FlipListView.ItemsOrientantionProperty);
			}
			set
			{
				base.SetValue(FlipListView.ItemsOrientantionProperty, value);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00003B7C File Offset: 0x00001D7C
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003B9E File Offset: 0x00001D9E
		public Style SwitchNextButtonStyle
		{
			get
			{
				return (Style)base.GetValue(FlipListView.SwitchNextButtonStyleProperty);
			}
			set
			{
				base.SetValue(FlipListView.SwitchNextButtonStyleProperty, value);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00003BB0 File Offset: 0x00001DB0
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003BD2 File Offset: 0x00001DD2
		public Style SwitchPreviousButtonStyle
		{
			get
			{
				return (Style)base.GetValue(FlipListView.SwitchPreviousButtonStyleProperty);
			}
			set
			{
				base.SetValue(FlipListView.SwitchPreviousButtonStyleProperty, value);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003BE4 File Offset: 0x00001DE4
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00003C06 File Offset: 0x00001E06
		public bool CanFlipNext
		{
			get
			{
				return (bool)base.GetValue(FlipListView.CanFlipNextProperty);
			}
			private set
			{
				base.SetValue(FlipListView.CanFlipNextProperty, value);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003C1C File Offset: 0x00001E1C
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00003C3E File Offset: 0x00001E3E
		public bool CanFlipPrevious
		{
			get
			{
				return (bool)base.GetValue(FlipListView.CanFlipPreviousProperty);
			}
			private set
			{
				base.SetValue(FlipListView.CanFlipPreviousProperty, value);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003C53 File Offset: 0x00001E53
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.OnApplySwitchNextButtonTemplate();
			this.OnApplySwitchPrevButtonTemplate();
			this.OnApplyItemsPresenterTemplate();
			this.OnApplyItemsScrollViewerTemplate();
			this.border = (base.GetTemplateChild("Border") as Border);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003C90 File Offset: 0x00001E90
		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);
			if (e.Key == Key.Next)
			{
				this.SwitchPrevButtonPartOnClick(this, e);
			}
			if (e.Key == Key.Prior)
			{
				this.SwitchNextButtonPartOnClick(this, e);
			}
			if (e.Key == Key.End)
			{
				if (!this.ScrollToLastPage())
				{
					this.itemsScrollViewer.ScrollToRightEnd();
				}
				this.GoToState("FlipNext", true);
			}
			if (e.Key == Key.Home)
			{
				if (!this.ScrollToFirstPage())
				{
					this.itemsScrollViewer.ScrollToLeftEnd();
				}
				this.GoToState("FlipNext", true);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003D50 File Offset: 0x00001F50
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (e.Key == Key.Right && this.buttonFocused == this.switchNextButtonPart)
			{
				this.SwitchNextButtonPartOnClick(this, e);
			}
			if (e.Key == Key.Left && this.buttonFocused == this.switchPrevButtonPart)
			{
				this.SwitchPrevButtonPartOnClick(this, e);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003DC2 File Offset: 0x00001FC2
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			this.GoToState("FlipNext", true);
			this.SetItemsPresenterPagedSize();
			this.ComputePagesData();
			this.ScrollToFirstPage();
			this.ValidateCanFlipProperties();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003DF8 File Offset: 0x00001FF8
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action != NotifyCollectionChangedAction.Reset)
			{
				this.ComputePagesData();
				this.ScrollToFirstPage();
			}
			else if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this.GoToState("FlipNext", true);
			}
			this.ValidateCanFlipProperties();
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003E54 File Offset: 0x00002054
		private static bool IsFullyVisible(FrameworkElement element, FrameworkElement container)
		{
			Rect rect = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
			Rect rect2 = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
			return rect2.Contains(rect.TopLeft) && rect2.Contains(rect.BottomRight);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003EE0 File Offset: 0x000020E0
		private void OnApplyItemsScrollViewerTemplate()
		{
			if (this.itemsScrollViewer != null)
			{
				this.itemsScrollViewer.ScrollChanged -= this.ItemsScrollViewerOnScrollChanged;
				this.itemsScrollViewer.Loaded -= this.ItemsScrollViewerOnLoaded;
			}
			this.itemsScrollViewer = (base.GetTemplateChild("PART_ItemsScrollViewer") as ScrollViewer);
			if (this.itemsScrollViewer != null)
			{
				this.itemsScrollViewer.ScrollChanged += this.ItemsScrollViewerOnScrollChanged;
				this.itemsScrollViewer.Loaded += this.ItemsScrollViewerOnLoaded;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003F82 File Offset: 0x00002182
		private void ItemsScrollViewerOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			this.SetItemsPresenterPagedSize();
			this.ComputePagesData();
			this.ScrollToFirstPage();
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003F9C File Offset: 0x0000219C
		private void ItemsScrollViewerOnScrollChanged(object sender, ScrollChangedEventArgs routedEventArgs)
		{
			this.SetItemsPresenterPagedSize();
			this.ComputePagesData();
			int itemIndex = (this.currentPage != null) ? this.currentPage.FirstIndexInPage : 0;
			this.ScrollToPage(this.FindPageForitem(itemIndex));
			this.SetItemsVisibility(base.Items);
			this.ValidateCanFlipProperties();
			if (this.IsInVisualState("FlipNext") || this.IsInVisualState("FlipPrevious"))
			{
				this.GoToState("ShowingPage", true);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004048 File Offset: 0x00002248
		private void ValidateCanFlipProperties()
		{
			if (this.pages == null || this.pages.Count == 0 || this.currentPage == null)
			{
				this.CanFlipPrevious = (this.CanFlipNext = false);
			}
			else
			{
				this.CanFlipPrevious = (this.currentPage.PageIndex > 0);
				this.CanFlipNext = this.pages.Any((FlipListView.FlipPage p) => p.PageIndex > this.currentPage.PageIndex);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000040C7 File Offset: 0x000022C7
		private void OnApplyItemsPresenterTemplate()
		{
			this.itemsPresenterPart = (base.GetTemplateChild("PART_ItemsPresenter") as ItemsPresenter);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000040E0 File Offset: 0x000022E0
		private void SetItemsPresenterPagedSize()
		{
			if (this.itemsScrollViewer != null && this.itemsPresenterPart != null)
			{
				Size size = new Size(this.itemsScrollViewer.ViewportWidth, this.itemsScrollViewer.ViewportHeight);
				if (this.ItemsOrientantion == Orientation.Vertical)
				{
					double num = this.itemsPresenterPart.ActualWidth % size.Width;
					if (num > 0.0)
					{
						this.itemsPresenterPart.Width = this.itemsPresenterPart.ActualWidth + (size.Width - num);
					}
				}
				else
				{
					double num = this.itemsPresenterPart.ActualHeight % size.Height;
					if (num > 0.0)
					{
						this.itemsPresenterPart.Height = this.itemsPresenterPart.ActualHeight + (size.Height - num);
					}
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000041D8 File Offset: 0x000023D8
		private void ComputePagesData()
		{
			if (base.Items == null || base.Items.IsEmpty)
			{
				this.pages = new List<FlipListView.FlipPage>();
				this.currentPage = null;
			}
			else
			{
				List<FlipListView.FlipPage> list = new List<FlipListView.FlipPage>();
				ListViewItem listViewItem = base.ItemContainerGenerator.ContainerFromIndex(0) as ListViewItem;
				if (listViewItem != null && this.itemsScrollViewer != null && this.itemsPresenterPart != null)
				{
					if (this.itemsScrollViewer.ViewportHeight > 0.0 && this.itemsScrollViewer.ViewportWidth > 0.0)
					{
						Size renderSize = listViewItem.RenderSize;
						if (renderSize.Width <= 0.0 || renderSize.Height <= 0.0)
						{
							renderSize = this.lastKnownItemSize;
						}
						else
						{
							this.lastKnownItemSize = renderSize;
						}
						int num = (int)(this.itemsScrollViewer.ViewportHeight / renderSize.Height);
						int num2 = (int)(this.itemsScrollViewer.ViewportWidth / renderSize.Width);
						int num3 = num2 * num;
						if (num3 > 0)
						{
							int count = base.Items.Count;
							int num4 = 0;
							for (int i = 0; i < count; i += num3)
							{
								list.Add(new FlipListView.FlipPage
								{
									PageIndex = num4,
									FirstIndexInPage = i,
									LastIndexInPage = i + num3 - 1,
									PageSize = new Size(this.itemsScrollViewer.ViewportWidth, this.itemsScrollViewer.ViewportHeight)
								});
								num4++;
							}
						}
					}
				}
				this.pages = list;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000043F0 File Offset: 0x000025F0
		private FlipListView.FlipPage FindPageForitem(int itemIndex)
		{
			if (this.pages == null || this.pages.Count == 0)
			{
				this.ComputePagesData();
			}
			return this.pages.FirstOrDefault((FlipListView.FlipPage p) => p.FirstIndexInPage <= itemIndex && p.LastIndexInPage >= itemIndex);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004454 File Offset: 0x00002654
		private bool ScrollToPage(FlipListView.FlipPage page)
		{
			bool result;
			if (page == null)
			{
				result = false;
			}
			else
			{
				if (this.currentPage != null)
				{
					this.currentPage.IsVisible = false;
				}
				page.IsVisible = true;
				this.ScrollToItem(page.FirstIndexInPage);
				this.currentPage = page;
				result = true;
			}
			return result;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000044B0 File Offset: 0x000026B0
		private void SetItemsVisibility(ItemCollection items)
		{
			int count = items.Count;
			for (int i = 0; i < count; i++)
			{
				object item = items[i];
				DependencyObject dependencyObject = base.ItemContainerGenerator.ContainerFromItem(item);
				ListViewItem listViewItem = dependencyObject as ListViewItem;
				if (listViewItem != null)
				{
					if (!FlipListView.IsFullyVisible(listViewItem, this.itemsScrollViewer))
					{
						listViewItem.Visibility = Visibility.Hidden;
					}
					else
					{
						listViewItem.Visibility = Visibility.Visible;
					}
					TileAutomationPeer tileAutomationPeer = new TileAutomationPeer(listViewItem);
					object obj = typeof(UIElement).InvokeMember("AutomationPeerField", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField, null, null, null);
					obj.GetType().InvokeMember("SetValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, obj, new object[]
					{
						listViewItem,
						tileAutomationPeer
					});
				}
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004590 File Offset: 0x00002790
		private void OnApplySwitchPrevButtonTemplate()
		{
			if (this.switchPrevButtonPart != null)
			{
				this.switchPrevButtonPart.Click -= this.SwitchPrevButtonPartOnClick;
				this.switchPrevButtonPart.GotFocus -= this.SwitchButtonPartOnGotFocus;
				this.switchPrevButtonPart.LostFocus -= this.SwitchButtonPartOnLostFocus;
			}
			this.switchPrevButtonPart = (base.GetTemplateChild("PART_SwitchPrevButton") as Button);
			if (this.switchPrevButtonPart != null)
			{
				this.switchPrevButtonPart.Click += this.SwitchPrevButtonPartOnClick;
				this.switchPrevButtonPart.GotFocus += this.SwitchButtonPartOnGotFocus;
				this.switchPrevButtonPart.LostFocus += this.SwitchButtonPartOnLostFocus;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004664 File Offset: 0x00002864
		private void OnApplySwitchNextButtonTemplate()
		{
			if (this.switchNextButtonPart != null)
			{
				this.switchNextButtonPart.Click -= this.SwitchNextButtonPartOnClick;
				this.switchNextButtonPart.GotFocus -= this.SwitchButtonPartOnGotFocus;
				this.switchNextButtonPart.LostFocus -= this.SwitchButtonPartOnLostFocus;
			}
			this.switchNextButtonPart = (base.GetTemplateChild("PART_SwitchNextButton") as Button);
			if (this.switchNextButtonPart != null)
			{
				this.switchNextButtonPart.Click += this.SwitchNextButtonPartOnClick;
				this.switchNextButtonPart.GotFocus += this.SwitchButtonPartOnGotFocus;
				this.switchNextButtonPart.LostFocus += this.SwitchButtonPartOnLostFocus;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004736 File Offset: 0x00002936
		private void SwitchButtonPartOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			this.buttonFocused = null;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004740 File Offset: 0x00002940
		private void SwitchButtonPartOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			this.buttonFocused = (sender as Button);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004750 File Offset: 0x00002950
		private void SwitchNextButtonPartOnClick(object sender, RoutedEventArgs e)
		{
			if (this.itemsScrollViewer != null && this.CanFlipNext)
			{
				if (!this.ScrollToNextPage())
				{
					if (this.ItemsOrientantion == Orientation.Vertical)
					{
						this.itemsScrollViewer.PageRight();
					}
					else
					{
						this.itemsScrollViewer.PageDown();
					}
				}
				this.GoToState("FlipNext", true);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000047C0 File Offset: 0x000029C0
		private void SwitchPrevButtonPartOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			if (this.itemsScrollViewer != null && this.CanFlipPrevious)
			{
				if (!this.ScrollToPreviousPage())
				{
					if (this.ItemsOrientantion == Orientation.Vertical)
					{
						this.itemsScrollViewer.PageLeft();
					}
					else
					{
						this.itemsScrollViewer.PageUp();
					}
				}
				this.GoToState("FlipPrevious", true);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004858 File Offset: 0x00002A58
		private bool ScrollToPreviousPage()
		{
			bool result;
			if (this.currentPage != null && this.currentPage.PageIndex > 0)
			{
				bool flag = this.ScrollToPage(this.pages.FirstOrDefault((FlipListView.FlipPage p) => p.PageIndex == this.currentPage.PageIndex - 1));
				if (this.switchNextButtonPart != null)
				{
					this.switchNextButtonPart.Focus();
					Keyboard.Focus(this.switchNextButtonPart);
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004908 File Offset: 0x00002B08
		private bool ScrollToNextPage()
		{
			bool result;
			if (this.currentPage != null)
			{
				bool flag = this.ScrollToPage(this.pages.FirstOrDefault((FlipListView.FlipPage p) => p.PageIndex == this.currentPage.PageIndex + 1));
				if (this.switchPrevButtonPart != null)
				{
					this.switchPrevButtonPart.Focus();
					Keyboard.Focus(this.switchPrevButtonPart);
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000497C File Offset: 0x00002B7C
		private bool ScrollToFirstPage()
		{
			bool result;
			if (this.pages == null || this.pages.Count == 0)
			{
				result = false;
			}
			else
			{
				bool flag = this.ScrollToPage(this.pages.FirstOrDefault<FlipListView.FlipPage>());
				if (this.switchNextButtonPart != null)
				{
					this.switchNextButtonPart.Focus();
					Keyboard.Focus(this.switchNextButtonPart);
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000049F0 File Offset: 0x00002BF0
		private bool ScrollToLastPage()
		{
			bool result;
			if (this.pages == null || this.pages.Count == 0)
			{
				result = false;
			}
			else
			{
				bool flag = this.ScrollToPage(this.pages.LastOrDefault<FlipListView.FlipPage>());
				if (this.switchNextButtonPart != null)
				{
					this.switchNextButtonPart.Focus();
					Keyboard.Focus(this.switchNextButtonPart);
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00004A64 File Offset: 0x00002C64
		private bool ScrollToItem(int index)
		{
			ListViewItem listViewItem = base.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
			bool result;
			if (this.itemsScrollViewer == null || listViewItem == null || this.itemsPresenterPart == null)
			{
				result = false;
			}
			else
			{
				Point point = listViewItem.TranslatePoint(default(Point), this.itemsPresenterPart);
				if (this.ItemsOrientantion == Orientation.Vertical)
				{
					this.itemsScrollViewer.ScrollToHorizontalOffset(point.X);
				}
				else
				{
					this.itemsScrollViewer.ScrollToVerticalOffset(point.Y);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004B00 File Offset: 0x00002D00
		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			this.GoToState("ShowingPage", true);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004B10 File Offset: 0x00002D10
		private void OnLayoutUpdated(object sender, EventArgs eventArgs)
		{
			this.SetItemsVisibility(base.Items);
			this.GoToState("ShowingPage", true);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004B30 File Offset: 0x00002D30
		private bool GoToState(string stateName, bool useTransitions)
		{
			bool result;
			if (this.border != null && VisualStateManager.GoToElementState(this.border, stateName, useTransitions))
			{
				this.currentFlipState = stateName;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004B70 File Offset: 0x00002D70
		private bool IsInVisualState(string stateName)
		{
			return string.Equals(stateName, this.currentFlipState, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x04000022 RID: 34
		private const string FlipVisualStateGroupName = "FlipStates";

		// Token: 0x04000023 RID: 35
		private const string FlipNextVisualStateName = "FlipNext";

		// Token: 0x04000024 RID: 36
		private const string FlipPreviousVisualStateName = "FlipPrevious";

		// Token: 0x04000025 RID: 37
		private const string ShowingPageVisualStateName = "ShowingPage";

		// Token: 0x04000026 RID: 38
		private const string SwitchNextButtonPartName = "PART_SwitchNextButton";

		// Token: 0x04000027 RID: 39
		private const string SwitchPrevButtonPartName = "PART_SwitchPrevButton";

		// Token: 0x04000028 RID: 40
		private const string ItemsPresenterPartName = "PART_ItemsPresenter";

		// Token: 0x04000029 RID: 41
		private const string ItemsScrollViewerPartName = "PART_ItemsScrollViewer";

		// Token: 0x0400002A RID: 42
		public static readonly DependencyProperty SwitchNextButtonStyleProperty = DependencyProperty.Register("SwitchNextButtonStyle", typeof(Style), typeof(FlipListView), new PropertyMetadata(null));

		// Token: 0x0400002B RID: 43
		public static readonly DependencyProperty SwitchPreviousButtonStyleProperty = DependencyProperty.Register("SwitchPreviousButtonStyle", typeof(Style), typeof(FlipListView), new PropertyMetadata(null));

		// Token: 0x0400002C RID: 44
		public static readonly DependencyProperty CanFlipNextProperty = DependencyProperty.Register("CanFlipNext", typeof(bool), typeof(FlipListView), new PropertyMetadata(false));

		// Token: 0x0400002D RID: 45
		public static readonly DependencyProperty CanFlipPreviousProperty = DependencyProperty.Register("CanFlipPrevious", typeof(bool), typeof(FlipListView), new PropertyMetadata(false));

		// Token: 0x0400002E RID: 46
		public static readonly DependencyProperty ItemsOrientantionProperty = DependencyProperty.Register("ItemsOrientantion", typeof(Orientation), typeof(FlipListView), new PropertyMetadata(Orientation.Horizontal));

		// Token: 0x0400002F RID: 47
		private Button switchPrevButtonPart;

		// Token: 0x04000030 RID: 48
		private Button switchNextButtonPart;

		// Token: 0x04000031 RID: 49
		private ItemsPresenter itemsPresenterPart;

		// Token: 0x04000032 RID: 50
		private ScrollViewer itemsScrollViewer;

		// Token: 0x04000033 RID: 51
		private List<FlipListView.FlipPage> pages;

		// Token: 0x04000034 RID: 52
		private FlipListView.FlipPage currentPage;

		// Token: 0x04000035 RID: 53
		private string currentFlipState;

		// Token: 0x04000036 RID: 54
		private Border border;

		// Token: 0x04000037 RID: 55
		private Size lastKnownItemSize;

		// Token: 0x04000038 RID: 56
		private Button buttonFocused;

		// Token: 0x02000015 RID: 21
		private class FlipPage
		{
			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060000AD RID: 173 RVA: 0x00004B90 File Offset: 0x00002D90
			// (set) Token: 0x060000AE RID: 174 RVA: 0x00004BA7 File Offset: 0x00002DA7
			public int PageIndex { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x060000AF RID: 175 RVA: 0x00004BB0 File Offset: 0x00002DB0
			// (set) Token: 0x060000B0 RID: 176 RVA: 0x00004BC7 File Offset: 0x00002DC7
			public int FirstIndexInPage { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004BD0 File Offset: 0x00002DD0
			// (set) Token: 0x060000B2 RID: 178 RVA: 0x00004BE7 File Offset: 0x00002DE7
			public int LastIndexInPage { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004BF0 File Offset: 0x00002DF0
			// (set) Token: 0x060000B4 RID: 180 RVA: 0x00004C07 File Offset: 0x00002E07
			public Size PageSize { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004C10 File Offset: 0x00002E10
			// (set) Token: 0x060000B6 RID: 182 RVA: 0x00004C27 File Offset: 0x00002E27
			public bool IsVisible { get; set; }
		}

		// Token: 0x02000016 RID: 22
		public class FlipListViewAutomationPeer : FrameworkElementAutomationPeer
		{
			// Token: 0x060000B8 RID: 184 RVA: 0x00004C38 File Offset: 0x00002E38
			public FlipListViewAutomationPeer(FlipListView owner) : base(owner)
			{
			}

			// Token: 0x060000B9 RID: 185 RVA: 0x00004C44 File Offset: 0x00002E44
			protected override string GetAutomationIdCore()
			{
				return string.Empty;
			}

			// Token: 0x060000BA RID: 186 RVA: 0x00004C5C File Offset: 0x00002E5C
			protected override string GetNameCore()
			{
				return string.Empty;
			}
		}
	}
}

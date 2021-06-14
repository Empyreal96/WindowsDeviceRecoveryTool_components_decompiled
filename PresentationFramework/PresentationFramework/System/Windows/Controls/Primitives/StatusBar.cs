using System;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that displays items and information in a horizontal bar in an application window.</summary>
	// Token: 0x020005A9 RID: 1449
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(StatusBarItem))]
	public class StatusBar : ItemsControl
	{
		// Token: 0x06006018 RID: 24600 RVA: 0x001AF4B4 File Offset: 0x001AD6B4
		static StatusBar()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(typeof(StatusBar)));
			StatusBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(StatusBar));
			Control.IsTabStopProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DockPanel)));
			itemsPanelTemplate.Seal();
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(StatusBar), new FrameworkPropertyMetadata(itemsPanelTemplate));
			ControlsTraceLogger.AddControl((TelemetryControls)((ulong)int.MinValue));
		}

		/// <summary>Gets or sets the custom logic for choosing a template used to display each item. </summary>
		/// <returns>A custom object that provides logic and returns an item container. </returns>
		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06006019 RID: 24601 RVA: 0x001AF592 File Offset: 0x001AD792
		// (set) Token: 0x0600601A RID: 24602 RVA: 0x001AF5A4 File Offset: 0x001AD7A4
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(StatusBar.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(StatusBar.ItemContainerTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the menu selects different item containers, depending on the type of the item in the underlying collection or some other heuristic.</summary>
		/// <returns>
		///     <see langword="true" /> the menu selects different item containers; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x0600601B RID: 24603 RVA: 0x001AF5B2 File Offset: 0x001AD7B2
		// (set) Token: 0x0600601C RID: 24604 RVA: 0x001AF5C4 File Offset: 0x001AD7C4
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(StatusBar.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(StatusBar.UsesItemContainerTemplateProperty, value);
			}
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <param name="item">The specified object to evaluate.</param>
		/// <returns>Returns <see langword="true" /> if the item is (or is eligible to be) its own container; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600601D RID: 24605 RVA: 0x001AF5D4 File Offset: 0x001AD7D4
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is StatusBarItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" />.</summary>
		/// <returns>The element used to display the specified item.</returns>
		// Token: 0x0600601E RID: 24606 RVA: 0x001AF604 File Offset: 0x001AD804
		protected override DependencyObject GetContainerForItemOverride()
		{
			object currentItem = this._currentItem;
			this._currentItem = null;
			if (this.UsesItemContainerTemplate)
			{
				DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
				if (dataTemplate != null)
				{
					object obj = dataTemplate.LoadContent();
					if (obj is StatusBarItem || obj is Separator)
					{
						return obj as DependencyObject;
					}
					throw new InvalidOperationException(SR.Get("InvalidItemContainer", new object[]
					{
						base.GetType().Name,
						typeof(StatusBarItem).Name,
						typeof(Separator).Name,
						obj
					}));
				}
			}
			return new StatusBarItem();
		}

		/// <summary>Prepares an item for display in the <see cref="T:System.Windows.Controls.Primitives.StatusBar" />.</summary>
		/// <param name="element">The item to display in the <see cref="T:System.Windows.Controls.Primitives.StatusBar" />.</param>
		/// <param name="item">The content of the item to display.</param>
		// Token: 0x0600601F RID: 24607 RVA: 0x001AF6A8 File Offset: 0x001AD8A8
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			Separator separator = element as Separator;
			if (separator != null)
			{
				bool flag;
				BaseValueSourceInternal valueSource = separator.GetValueSource(FrameworkElement.StyleProperty, null, out flag);
				if (valueSource <= BaseValueSourceInternal.ImplicitReference)
				{
					separator.SetResourceReference(FrameworkElement.StyleProperty, StatusBar.SeparatorStyleKey);
				}
				separator.DefaultStyleKey = StatusBar.SeparatorStyleKey;
			}
		}

		/// <summary>Determines whether to apply the <see cref="T:System.Windows.Style" /> for an item in the <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> to an object.</summary>
		/// <param name="container">The container for the item.</param>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="item" /> is not a <see cref="T:System.Windows.Controls.Separator" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006020 RID: 24608 RVA: 0x0016FF49 File Offset: 0x0016E149
		protected override bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return !(item is Separator) && base.ShouldApplyItemContainerStyle(container, item);
		}

		/// <summary>Specifies an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.StatusBar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.StatusBarAutomationPeer" /> for this <see cref="T:System.Windows.Controls.Primitives.StatusBar" />.</returns>
		// Token: 0x06006021 RID: 24609 RVA: 0x001AF6F5 File Offset: 0x001AD8F5
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new StatusBarAutomationPeer(this);
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x001AF6FD File Offset: 0x001AD8FD
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return StatusBar._dType;
			}
		}

		/// <summary>The key that represents the style to use for <see cref="T:System.Windows.Controls.Separator" /> objects in the <see cref="T:System.Windows.Controls.Primitives.StatusBar" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the style to use for <see cref="T:System.Windows.Controls.Separator" /> objects.</returns>
		// Token: 0x17001720 RID: 5920
		// (get) Token: 0x06006023 RID: 24611 RVA: 0x001AF704 File Offset: 0x001AD904
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.StatusBarSeparatorStyleKey;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.StatusBar.ItemContainerTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.StatusBar.ItemContainerTemplateSelector" /> dependency property.</returns>
		// Token: 0x040030EC RID: 12524
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty = MenuBase.ItemContainerTemplateSelectorProperty.AddOwner(typeof(StatusBar), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.StatusBar.UsesItemContainerTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.StatusBar.UsesItemContainerTemplate" /> dependency property.</returns>
		// Token: 0x040030ED RID: 12525
		public static readonly DependencyProperty UsesItemContainerTemplateProperty = MenuBase.UsesItemContainerTemplateProperty.AddOwner(typeof(StatusBar));

		// Token: 0x040030EE RID: 12526
		private object _currentItem;

		// Token: 0x040030EF RID: 12527
		private static DependencyObjectType _dType;
	}
}

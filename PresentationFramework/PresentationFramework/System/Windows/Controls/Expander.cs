using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents the control that displays a header that has a collapsible window that displays content.</summary>
	// Token: 0x020004CF RID: 1231
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Expander : HeaderedContentControl
	{
		// Token: 0x06004B07 RID: 19207 RVA: 0x0015259C File Offset: 0x0015079C
		static Expander()
		{
			Expander.ExpandedEvent = EventManager.RegisterRoutedEvent("Expanded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Expander));
			Expander.CollapsedEvent = EventManager.RegisterRoutedEvent("Collapsed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Expander));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Expander), new FrameworkPropertyMetadata(typeof(Expander)));
			Expander._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Expander));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Expander), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(Expander), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Expander), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.Expander);
		}

		/// <summary>Gets or sets the direction in which the <see cref="T:System.Windows.Controls.Expander" /> content window opens.  </summary>
		/// <returns>One of the <see cref="T:System.Windows.Controls.ExpandDirection" /> values that defines which direction the content window opens. The default is <see cref="F:System.Windows.Controls.ExpandDirection.Down" />. </returns>
		// Token: 0x1700124C RID: 4684
		// (get) Token: 0x06004B08 RID: 19208 RVA: 0x00152724 File Offset: 0x00150924
		// (set) Token: 0x06004B09 RID: 19209 RVA: 0x00152736 File Offset: 0x00150936
		[Bindable(true)]
		[Category("Behavior")]
		public ExpandDirection ExpandDirection
		{
			get
			{
				return (ExpandDirection)base.GetValue(Expander.ExpandDirectionProperty);
			}
			set
			{
				base.SetValue(Expander.ExpandDirectionProperty, value);
			}
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x0015274C File Offset: 0x0015094C
		private static bool IsValidExpandDirection(object o)
		{
			ExpandDirection expandDirection = (ExpandDirection)o;
			return expandDirection == ExpandDirection.Down || expandDirection == ExpandDirection.Left || expandDirection == ExpandDirection.Right || expandDirection == ExpandDirection.Up;
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x00152774 File Offset: 0x00150974
		private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Expander expander = (Expander)d;
			bool flag = (bool)e.NewValue;
			ExpanderAutomationPeer expanderAutomationPeer = UIElementAutomationPeer.FromElement(expander) as ExpanderAutomationPeer;
			if (expanderAutomationPeer != null)
			{
				expanderAutomationPeer.RaiseExpandCollapseAutomationEvent(!flag, flag);
			}
			if (flag)
			{
				expander.OnExpanded();
			}
			else
			{
				expander.OnCollapsed();
			}
			expander.UpdateVisualState();
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Controls.Expander" /> content window is visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the content window is expanded; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06004B0C RID: 19212 RVA: 0x001527C6 File Offset: 0x001509C6
		// (set) Token: 0x06004B0D RID: 19213 RVA: 0x001527D8 File Offset: 0x001509D8
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsExpanded
		{
			get
			{
				return (bool)base.GetValue(Expander.IsExpandedProperty);
			}
			set
			{
				base.SetValue(Expander.IsExpandedProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Occurs when the content window of an <see cref="T:System.Windows.Controls.Expander" /> control opens to display both its header and content. </summary>
		// Token: 0x140000CF RID: 207
		// (add) Token: 0x06004B0E RID: 19214 RVA: 0x001527EB File Offset: 0x001509EB
		// (remove) Token: 0x06004B0F RID: 19215 RVA: 0x001527F9 File Offset: 0x001509F9
		public event RoutedEventHandler Expanded
		{
			add
			{
				base.AddHandler(Expander.ExpandedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Expander.ExpandedEvent, value);
			}
		}

		/// <summary>Occurs when the content window of an <see cref="T:System.Windows.Controls.Expander" /> control closes and only the <see cref="P:System.Windows.Controls.HeaderedContentControl.Header" /> is visible.</summary>
		// Token: 0x140000D0 RID: 208
		// (add) Token: 0x06004B10 RID: 19216 RVA: 0x00152807 File Offset: 0x00150A07
		// (remove) Token: 0x06004B11 RID: 19217 RVA: 0x00152815 File Offset: 0x00150A15
		public event RoutedEventHandler Collapsed
		{
			add
			{
				base.AddHandler(Expander.CollapsedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Expander.CollapsedEvent, value);
			}
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x00152824 File Offset: 0x00150A24
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Unfocused"
				});
			}
			if (this.IsExpanded)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Expanded"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Collapsed"
				});
			}
			switch (this.ExpandDirection)
			{
			case ExpandDirection.Down:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandDown"
				});
				break;
			case ExpandDirection.Up:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandUp"
				});
				break;
			case ExpandDirection.Left:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandLeft"
				});
				break;
			default:
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"ExpandRight"
				});
				break;
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Expander.Expanded" /> event when the <see cref="P:System.Windows.Controls.Expander.IsExpanded" /> property changes from <see langword="false" /> to <see langword="true" />.</summary>
		// Token: 0x06004B13 RID: 19219 RVA: 0x00152980 File Offset: 0x00150B80
		protected virtual void OnExpanded()
		{
			base.RaiseEvent(new RoutedEventArgs
			{
				RoutedEvent = Expander.ExpandedEvent,
				Source = this
			});
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Expander.Collapsed" /> event when the <see cref="P:System.Windows.Controls.Expander.IsExpanded" /> property changes from <see langword="true" /> to <see langword="false" />.</summary>
		// Token: 0x06004B14 RID: 19220 RVA: 0x001529AC File Offset: 0x00150BAC
		protected virtual void OnCollapsed()
		{
			base.RaiseEvent(new RoutedEventArgs(Expander.CollapsedEvent, this));
		}

		/// <summary>Creates the implementation of <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Expander" /> control.</summary>
		/// <returns>A new <see cref="T:System.Windows.Automation.Peers.ExpanderAutomationPeer" /> for this <see cref="T:System.Windows.Controls.Expander" /> control.</returns>
		// Token: 0x06004B15 RID: 19221 RVA: 0x001529BF File Offset: 0x00150BBF
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ExpanderAutomationPeer(this);
		}

		/// <summary>Invoked whenever application code or internal processes call the <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> method. </summary>
		// Token: 0x06004B16 RID: 19222 RVA: 0x001529C7 File Offset: 0x00150BC7
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._expanderToggleButton = (base.GetTemplateChild("HeaderSite") as ToggleButton);
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x001529E5 File Offset: 0x00150BE5
		internal bool IsExpanderToggleButtonFocused
		{
			get
			{
				ToggleButton expanderToggleButton = this._expanderToggleButton;
				return expanderToggleButton != null && expanderToggleButton.IsKeyboardFocused;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06004B18 RID: 19224 RVA: 0x001529F8 File Offset: 0x00150BF8
		internal ToggleButton ExpanderToggleButton
		{
			get
			{
				return this._expanderToggleButton;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00152A00 File Offset: 0x00150C00
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Expander._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Expander.ExpandDirection" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Expander.ExpandDirection" /> dependency property.</returns>
		// Token: 0x04002AAD RID: 10925
		public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register("ExpandDirection", typeof(ExpandDirection), typeof(Expander), new FrameworkPropertyMetadata(ExpandDirection.Down, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)), new ValidateValueCallback(Expander.IsValidExpandDirection));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Expander.IsExpanded" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Expander.IsExpanded" /> dependency property.</returns>
		// Token: 0x04002AAE RID: 10926
		public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(Expander), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Expander.OnIsExpandedChanged)));

		// Token: 0x04002AB1 RID: 10929
		private const string ExpanderToggleButtonTemplateName = "HeaderSite";

		// Token: 0x04002AB2 RID: 10930
		private ToggleButton _expanderToggleButton;

		// Token: 0x04002AB3 RID: 10931
		private static DependencyObjectType _dType;
	}
}

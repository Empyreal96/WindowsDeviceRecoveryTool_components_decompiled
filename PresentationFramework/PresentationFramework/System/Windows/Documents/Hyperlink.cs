using System;
using System.ComponentModel;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.PresentationFramework;

namespace System.Windows.Documents
{
	/// <summary>An inline-level flow content element that provides facilities for hosting hyperlinks within flow content.</summary>
	// Token: 0x0200037A RID: 890
	[TextElementEditingBehavior(IsMergeable = false, IsTypographicOnly = false)]
	[UIPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class Hyperlink : Span, ICommandSource, IUriContext
	{
		// Token: 0x0600301C RID: 12316 RVA: 0x000D8900 File Offset: 0x000D6B00
		static Hyperlink()
		{
			Hyperlink.RequestNavigateEvent = EventManager.RegisterRoutedEvent("RequestNavigate", RoutingStrategy.Bubble, typeof(RequestNavigateEventHandler), typeof(Hyperlink));
			Hyperlink.ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(Hyperlink));
			Hyperlink.RequestSetStatusBarEvent = EventManager.RegisterRoutedEvent("RequestSetStatusBar", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Hyperlink));
			Hyperlink.IsHyperlinkPressedProperty = DependencyProperty.Register("IsHyperlinkPressed", typeof(bool), typeof(Hyperlink), new FrameworkPropertyMetadata(false));
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Hyperlink), new FrameworkPropertyMetadata(typeof(Hyperlink)));
			Hyperlink._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Hyperlink));
			ContentElement.FocusableProperty.OverrideMetadata(typeof(Hyperlink), new FrameworkPropertyMetadata(true));
			EventManager.RegisterClassHandler(typeof(Hyperlink), Mouse.QueryCursorEvent, new QueryCursorEventHandler(Hyperlink.OnQueryCursor));
		}

		/// <summary>Initializes a new, default instance of the <see cref="T:System.Windows.Documents.Hyperlink" /> class.</summary>
		// Token: 0x0600301D RID: 12317 RVA: 0x000D8B07 File Offset: 0x000D6D07
		public Hyperlink()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Hyperlink" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <param name="childInline">An <see cref="T:System.Windows.Documents.Inline" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Hyperlink" />.</param>
		// Token: 0x0600301E RID: 12318 RVA: 0x000D8B16 File Offset: 0x000D6D16
		public Hyperlink(Inline childInline) : base(childInline)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Hyperlink" /> class, taking a specified <see cref="T:System.Windows.Documents.Inline" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Hyperlink" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the new <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <param name="childInline">An <see cref="T:System.Windows.Documents.Inline" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Hyperlink" />.  This parameter may be <see langword="null" />, in which case no <see cref="T:System.Windows.Documents.Inline" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the <see cref="T:System.Windows.Documents.Hyperlink" /> element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x0600301F RID: 12319 RVA: 0x000D8B26 File Offset: 0x000D6D26
		public Hyperlink(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Hyperlink" /> class, taking two <see cref="T:System.Windows.Documents.TextPointer" /> objects that indicate the beginning and end of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <param name="start">A <see cref="T:System.Windows.Documents.TextPointer" /> indicating the beginning of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Hyperlink" />.</param>
		/// <param name="end">A <see cref="T:System.Windows.Documents.TextPointer" /> indicating the end of a selection of content to be contained by the new <see cref="T:System.Windows.Documents.Hyperlink" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="start" /> or <paramref name="end" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="start" /> and <paramref name="end" /> do not resolve to a range of content suitable for enclosure by a <see cref="T:System.Windows.Documents.Span" /> element; for example, if <paramref name="start" /> and <paramref name="end" /> indicate positions in different paragraphs.</exception>
		// Token: 0x06003020 RID: 12320 RVA: 0x000D8B38 File Offset: 0x000D6D38
		public Hyperlink(TextPointer start, TextPointer end) : base(start, end)
		{
			TextPointer textPointer = base.ContentStart.CreatePointer();
			TextPointer contentEnd = base.ContentEnd;
			while (textPointer.CompareTo(contentEnd) < 0)
			{
				Hyperlink hyperlink = textPointer.GetAdjacentElement(LogicalDirection.Forward) as Hyperlink;
				if (hyperlink != null)
				{
					hyperlink.Reposition(null, null);
				}
				else
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
		}

		/// <summary>Simulates the act of a user clicking the <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		// Token: 0x06003021 RID: 12321 RVA: 0x000D8B95 File Offset: 0x000D6D95
		public void DoClick()
		{
			Hyperlink.DoNonUserInitiatedNavigation(this);
		}

		/// <summary>Gets or sets a command to associate with the <see cref="T:System.Windows.Documents.Hyperlink" />.  </summary>
		/// <returns>A command to associate with the <see cref="T:System.Windows.Documents.Hyperlink" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000D8B9D File Offset: 0x000D6D9D
		// (set) Token: 0x06003023 RID: 12323 RVA: 0x000D8BAF File Offset: 0x000D6DAF
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(Hyperlink.CommandProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandProperty, value);
			}
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000D8BC0 File Offset: 0x000D6DC0
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)d;
			hyperlink.OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000D8BF2 File Offset: 0x000D6DF2
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
		}

		// Token: 0x06003026 RID: 12326 RVA: 0x000D8C08 File Offset: 0x000D6E08
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000D8C22 File Offset: 0x000D6E22
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000D8C3C File Offset: 0x000D6E3C
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000D8C44 File Offset: 0x000D6E44
		private void UpdateCanExecute()
		{
			if (this.Command != null)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
		}

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000D8C62 File Offset: 0x000D6E62
		// (set) Token: 0x0600302B RID: 12331 RVA: 0x000D8C6A File Offset: 0x000D6E6A
		private bool CanExecute
		{
			get
			{
				return this._canExecute;
			}
			set
			{
				if (this._canExecute != value)
				{
					this._canExecute = value;
					base.CoerceValue(ContentElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x000D8C87 File Offset: 0x000D6E87
		private bool IsEditable
		{
			get
			{
				return base.TextContainer.TextSelection != null && !base.TextContainer.TextSelection.TextEditor.IsReadOnly;
			}
		}

		/// <summary>Gets a value that indicates whether or not the <see cref="T:System.Windows.Documents.Hyperlink" /> is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Documents.Hyperlink" /> is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x000D8CB0 File Offset: 0x000D6EB0
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		/// <summary>Gets or sets command parameters associated with the command specified by the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> property.  </summary>
		/// <returns>An object specifying parameters for the command specified by the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> property. The default is <see langword="null" />.</returns>
		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x000D8CC2 File Offset: 0x000D6EC2
		// (set) Token: 0x0600302F RID: 12335 RVA: 0x000D8CCF File Offset: 0x000D6ECF
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(Hyperlink.CommandParameterProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandParameterProperty, value);
			}
		}

		/// <summary>Gets or sets a target element on which to execute the command specified by the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> property.  </summary>
		/// <returns>A target element on which to execute the command specified by the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> property. The default is <see langword="null" />.</returns>
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000D8CDD File Offset: 0x000D6EDD
		// (set) Token: 0x06003031 RID: 12337 RVA: 0x000D8CEF File Offset: 0x000D6EEF
		[Bindable(true)]
		[Category("Action")]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(Hyperlink.CommandTargetProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandTargetProperty, value);
			}
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000D8D00 File Offset: 0x000D6F00
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static object CoerceNavigateUri(DependencyObject d, object value)
		{
			if (Hyperlink.s_criticalNavigateUriProtectee.Value == d.GetHashCode() && Hyperlink.ShouldPreventUriSpoofing)
			{
				value = DependencyProperty.UnsetValue;
			}
			return value;
		}

		/// <summary>Gets or sets a URI to navigate to when the <see cref="T:System.Windows.Documents.Hyperlink" /> is activated. </summary>
		/// <returns>The <see cref="T:System.Uri" /> to navigate to when the <see cref="T:System.Windows.Documents.Hyperlink" /> is activated. The default is <see langword="null" />.
		///       <see cref="T:System.Windows.Documents.Hyperlink" /> can navigate to the value of the <see cref="P:System.Windows.Documents.Hyperlink.NavigateUri" /> property only if either the direct or indirect parent of a <see cref="T:System.Windows.Documents.Hyperlink" /> is a navigation host, including <see cref="T:System.Windows.Navigation.NavigationWindow" />, <see cref="T:System.Windows.Controls.Frame" />, or any browser that can host XBAPs (which includes Internet Explorer 7, Microsoft Internet Explorer 6, and Firefox 2.0+). For more information, see the Navigation Hosts section in Navigation Overview.</returns>
		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x000D8D44 File Offset: 0x000D6F44
		// (set) Token: 0x06003034 RID: 12340 RVA: 0x000D8D56 File Offset: 0x000D6F56
		[Bindable(true)]
		[CustomCategory("Navigation")]
		[Localizability(LocalizationCategory.Hyperlink)]
		public Uri NavigateUri
		{
			get
			{
				return (Uri)base.GetValue(Hyperlink.NavigateUriProperty);
			}
			set
			{
				base.SetValue(Hyperlink.NavigateUriProperty, value);
			}
		}

		/// <summary>Gets or sets the name of a target window or frame for the <see cref="T:System.Windows.Documents.Hyperlink" />.  </summary>
		/// <returns>A string specifying the name of a target window or frame for the <see cref="T:System.Windows.Documents.Hyperlink" />.</returns>
		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x000D8D64 File Offset: 0x000D6F64
		// (set) Token: 0x06003036 RID: 12342 RVA: 0x000D8D76 File Offset: 0x000D6F76
		[Bindable(true)]
		[CustomCategory("Navigation")]
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable)]
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(Hyperlink.TargetNameProperty);
			}
			set
			{
				base.SetValue(Hyperlink.TargetNameProperty, value);
			}
		}

		/// <summary>Occurs when navigation events are requested.</summary>
		// Token: 0x14000079 RID: 121
		// (add) Token: 0x06003037 RID: 12343 RVA: 0x000D8D84 File Offset: 0x000D6F84
		// (remove) Token: 0x06003038 RID: 12344 RVA: 0x000D8D92 File Offset: 0x000D6F92
		public event RequestNavigateEventHandler RequestNavigate
		{
			add
			{
				base.AddHandler(Hyperlink.RequestNavigateEvent, value);
			}
			remove
			{
				base.RemoveHandler(Hyperlink.RequestNavigateEvent, value);
			}
		}

		/// <summary>Occurs when the left mouse button is clicked on a <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		// Token: 0x1400007A RID: 122
		// (add) Token: 0x06003039 RID: 12345 RVA: 0x000D8DA0 File Offset: 0x000D6FA0
		// (remove) Token: 0x0600303A RID: 12346 RVA: 0x000D8DAE File Offset: 0x000D6FAE
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(Hyperlink.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Hyperlink.ClickEvent, value);
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.ContentElement.MouseLeftButtonDown" /> routed event.</summary>
		/// <param name="e">Arguments associated with the <see cref="E:System.Windows.ContentElement.MouseLeftButtonDown" /> event.</param>
		// Token: 0x0600303B RID: 12347 RVA: 0x000D8DBC File Offset: 0x000D6FBC
		protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (base.IsEnabled && (!this.IsEditable || (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None))
			{
				Hyperlink.OnMouseLeftButtonDown(this, e);
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.ContentElement.MouseLeftButtonUp" /> routed event.</summary>
		/// <param name="e">Arguments associated with the <see cref="E:System.Windows.ContentElement.MouseLeftButtonUp" /> event.</param>
		// Token: 0x0600303C RID: 12348 RVA: 0x000D8DE5 File Offset: 0x000D6FE5
		[SecurityCritical]
		protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			Hyperlink.OnMouseLeftButtonUp(this, e);
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000D8DF5 File Offset: 0x000D6FF5
		[SecurityCritical]
		private static void CacheNavigateUri(DependencyObject d, Uri targetUri)
		{
			d.VerifyAccess();
			Hyperlink.s_cachedNavigateUri.Value = targetUri;
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000D8E08 File Offset: 0x000D7008
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void NavigateToUri(IInputElement sourceElement, Uri targetUri, string targetWindow)
		{
			DependencyObject dependencyObject = (DependencyObject)sourceElement;
			dependencyObject.VerifyAccess();
			Uri value = Hyperlink.s_cachedNavigateUri.Value;
			if (value == null || value.Equals(targetUri) || !Hyperlink.ShouldPreventUriSpoofing)
			{
				if (!(sourceElement is Hyperlink))
				{
					targetUri = FixedPage.GetLinkUri(sourceElement, targetUri);
				}
				RequestNavigateEventArgs requestNavigateEventArgs = new RequestNavigateEventArgs(targetUri, targetWindow);
				requestNavigateEventArgs.Source = sourceElement;
				sourceElement.RaiseEvent(requestNavigateEventArgs);
				if (requestNavigateEventArgs.Handled)
				{
					dependencyObject.Dispatcher.BeginInvoke(DispatcherPriority.Send, new SendOrPostCallback(Hyperlink.ClearStatusBarAndCachedUri), sourceElement);
				}
			}
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x000D8E90 File Offset: 0x000D7090
		[SecurityCritical]
		private static void UpdateStatusBar(object sender)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			Uri targetUri = (Uri)dependencyObject.GetValue(Hyperlink.GetNavigateUriProperty(inputElement));
			Hyperlink.s_criticalNavigateUriProtectee.Value = new int?(dependencyObject.GetHashCode());
			Hyperlink.CacheNavigateUri(dependencyObject, targetUri);
			RequestSetStatusBarEventArgs e = new RequestSetStatusBarEventArgs(targetUri);
			inputElement.RaiseEvent(e);
		}

		// Token: 0x06003040 RID: 12352 RVA: 0x000D8EE8 File Offset: 0x000D70E8
		private static DependencyProperty GetNavigateUriProperty(object element)
		{
			Hyperlink hyperlink = element as Hyperlink;
			if (hyperlink != null)
			{
				return Hyperlink.NavigateUriProperty;
			}
			return FixedPage.NavigateUriProperty;
		}

		// Token: 0x06003041 RID: 12353 RVA: 0x000D8F0C File Offset: 0x000D710C
		[SecurityCritical]
		private static void ClearStatusBarAndCachedUri(object sender)
		{
			IInputElement inputElement = (IInputElement)sender;
			inputElement.RaiseEvent(RequestSetStatusBarEventArgs.Clear);
			Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
			Hyperlink.s_criticalNavigateUriProtectee.Value = null;
		}

		/// <summary>Handles the <see cref="E:System.Windows.Documents.Hyperlink.Click" /> routed event.</summary>
		// Token: 0x06003042 RID: 12354 RVA: 0x000D8F4C File Offset: 0x000D714C
		protected virtual void OnClick()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			Hyperlink.DoNavigation(this);
			base.RaiseEvent(new RoutedEventArgs(Hyperlink.ClickEvent, this));
			CommandHelpers.ExecuteCommandSource(this);
		}

		/// <summary>Handles the <see cref="E:System.Windows.ContentElement.KeyDown" /> routed event.</summary>
		/// <param name="e">Arguments associated with the <see cref="E:System.Windows.ContentElement.KeyDown" /> event.</param>
		// Token: 0x06003043 RID: 12355 RVA: 0x000D8F8F File Offset: 0x000D718F
		[SecurityCritical]
		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Return)
			{
				Hyperlink.OnKeyDown(this, e);
				return;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x0003BCFF File Offset: 0x00039EFF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.Hyperlink" />.</returns>
		// Token: 0x06003045 RID: 12357 RVA: 0x000D8FB1 File Offset: 0x000D71B1
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HyperlinkAutomationPeer(this);
		}

		/// <summary>Gets or sets a base URI for the <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <returns>A base URI for the <see cref="T:System.Windows.Documents.Hyperlink" />.</returns>
		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000D8FB9 File Offset: 0x000D71B9
		// (set) Token: 0x06003047 RID: 12359 RVA: 0x000D8FC1 File Offset: 0x000D71C1
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		/// <summary>Gets or sets a base URI for the <see cref="T:System.Windows.Documents.Hyperlink" />.</summary>
		/// <returns>A base URI for the <see cref="T:System.Windows.Documents.Hyperlink" />.</returns>
		// Token: 0x17000C2F RID: 3119
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06003049 RID: 12361 RVA: 0x000C2181 File Offset: 0x000C0381
		protected virtual Uri BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x17000C30 RID: 3120
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x000D8FCA File Offset: 0x000D71CA
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal string Text
		{
			get
			{
				return TextRangeBase.GetTextInternal(base.ContentStart, base.ContentEnd);
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000D8FE0 File Offset: 0x000D71E0
		private static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)sender;
			if (hyperlink.IsEnabled && hyperlink.IsEditable && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
			{
				e.Cursor = hyperlink.TextContainer.TextSelection.TextEditor._cursor;
				e.Handled = true;
			}
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x000D9030 File Offset: 0x000D7230
		private static bool ShouldPreventUriSpoofing
		{
			[SecurityCritical]
			get
			{
				if (Hyperlink.s_shouldPreventUriSpoofing.Value == null)
				{
					try
					{
						new WebPermission(PermissionState.Unrestricted).Demand();
						Hyperlink.s_shouldPreventUriSpoofing.Value = new bool?(false);
					}
					catch (SecurityException)
					{
						Hyperlink.s_shouldPreventUriSpoofing.Value = new bool?(true);
					}
				}
				return Hyperlink.s_shouldPreventUriSpoofing.Value.Value;
			}
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000D90A4 File Offset: 0x000D72A4
		internal static void OnNavigateUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IInputElement inputElement = d as IInputElement;
			if (inputElement != null)
			{
				Uri uri = (Uri)e.NewValue;
				if (uri != null)
				{
					FrameworkElement frameworkElement = d as FrameworkElement;
					if (frameworkElement != null && (frameworkElement is Path || frameworkElement is Canvas || frameworkElement is Glyphs || frameworkElement is FixedPage))
					{
						if (FixedPage.CanNavigateToUri(uri))
						{
							Hyperlink.SetUpNavigationEventHandlers(inputElement);
							frameworkElement.Cursor = Cursors.Hand;
							return;
						}
						frameworkElement.Cursor = Cursors.No;
						return;
					}
					else
					{
						FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
						if (frameworkContentElement != null && frameworkContentElement is Hyperlink)
						{
							Hyperlink.SetUpNavigationEventHandlers(inputElement);
						}
					}
				}
			}
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000D913C File Offset: 0x000D733C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void SetUpNavigationEventHandlers(IInputElement element)
		{
			if (!(element is Hyperlink))
			{
				Hyperlink.SetUpEventHandler(element, UIElement.KeyDownEvent, new KeyEventHandler(Hyperlink.OnKeyDown));
				Hyperlink.SetUpEventHandler(element, UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Hyperlink.OnMouseLeftButtonDown));
				Hyperlink.SetUpEventHandler(element, UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Hyperlink.OnMouseLeftButtonUp));
			}
			Hyperlink.SetUpEventHandler(element, UIElement.MouseEnterEvent, new MouseEventHandler(Hyperlink.OnMouseEnter));
			Hyperlink.SetUpEventHandler(element, UIElement.MouseLeaveEvent, new MouseEventHandler(Hyperlink.OnMouseLeave));
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x000D91C4 File Offset: 0x000D73C4
		private static void SetUpEventHandler(IInputElement element, RoutedEvent routedEvent, Delegate handler)
		{
			element.RemoveHandler(routedEvent, handler);
			element.AddHandler(routedEvent, handler);
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x000D91D6 File Offset: 0x000D73D6
		[SecurityCritical]
		private static void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Return)
			{
				Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
				if (e.UserInitiated)
				{
					Hyperlink.DoUserInitiatedNavigation(sender);
				}
				else
				{
					Hyperlink.DoNonUserInitiatedNavigation(sender);
				}
				e.Handled = true;
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x000D9214 File Offset: 0x000D7414
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			inputElement.Focus();
			if (e.ButtonState == MouseButtonState.Pressed)
			{
				Mouse.Capture(inputElement);
				if (inputElement.IsMouseCaptured)
				{
					if (e.ButtonState == MouseButtonState.Pressed)
					{
						dependencyObject.SetValue(Hyperlink.IsHyperlinkPressedProperty, true);
					}
					else
					{
						inputElement.ReleaseMouseCapture();
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000D9274 File Offset: 0x000D7474
		[SecurityCritical]
		private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			if (inputElement.IsMouseCaptured)
			{
				inputElement.ReleaseMouseCapture();
			}
			if ((bool)dependencyObject.GetValue(Hyperlink.IsHyperlinkPressedProperty))
			{
				dependencyObject.SetValue(Hyperlink.IsHyperlinkPressedProperty, false);
				if (inputElement.IsMouseOver)
				{
					if (e.UserInitiated)
					{
						Hyperlink.DoUserInitiatedNavigation(sender);
					}
					else
					{
						Hyperlink.DoNonUserInitiatedNavigation(sender);
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000D92E0 File Offset: 0x000D74E0
		[SecurityCritical]
		private static void OnMouseEnter(object sender, MouseEventArgs e)
		{
			Hyperlink.UpdateStatusBar(sender);
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000D92E8 File Offset: 0x000D74E8
		[SecurityCritical]
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			IInputElement inputElement = (IInputElement)sender;
			if (!inputElement.IsMouseOver)
			{
				Hyperlink.ClearStatusBarAndCachedUri(sender);
			}
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000D930C File Offset: 0x000D750C
		[SecurityCritical]
		private static void DoUserInitiatedNavigation(object sender)
		{
			CodeAccessPermission codeAccessPermission = SecurityHelper.CreateUserInitiatedNavigationPermission();
			codeAccessPermission.Assert();
			try
			{
				Hyperlink.DispatchNavigation(sender);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000D9344 File Offset: 0x000D7544
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void DoNonUserInitiatedNavigation(object sender)
		{
			Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
			Hyperlink.DispatchNavigation(sender);
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000D9358 File Offset: 0x000D7558
		private static void DispatchNavigation(object sender)
		{
			Hyperlink hyperlink = sender as Hyperlink;
			if (hyperlink != null)
			{
				hyperlink.OnClick();
				return;
			}
			Hyperlink.DoNavigation(sender);
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000D937C File Offset: 0x000D757C
		private static void DoNavigation(object sender)
		{
			IInputElement element = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			Uri targetUri = (Uri)dependencyObject.GetValue(Hyperlink.GetNavigateUriProperty(element));
			string targetWindow = (string)dependencyObject.GetValue(Hyperlink.TargetNameProperty);
			Hyperlink.RaiseNavigate(element, targetUri, targetWindow);
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000D93C2 File Offset: 0x000D75C2
		internal static void RaiseNavigate(IInputElement element, Uri targetUri, string targetWindow)
		{
			if (targetUri != null)
			{
				Hyperlink.NavigateToUri(element, targetUri, targetWindow);
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x0600305A RID: 12378 RVA: 0x000D93D5 File Offset: 0x000D75D5
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Hyperlink._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Hyperlink.Command" /> dependency property.</returns>
		// Token: 0x04001E68 RID: 7784
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(Hyperlink), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnCommandChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Hyperlink.CommandParameter" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Hyperlink.CommandParameter" /> dependency property.</returns>
		// Token: 0x04001E69 RID: 7785
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(Hyperlink), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Hyperlink.CommandTarget" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Hyperlink.CommandTarget" /> dependency property.</returns>
		// Token: 0x04001E6A RID: 7786
		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(Hyperlink), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Hyperlink.NavigateUri" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Hyperlink.NavigateUri" /> dependency property.</returns>
		// Token: 0x04001E6B RID: 7787
		[CommonDependencyProperty]
		public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register("NavigateUri", typeof(Uri), typeof(Hyperlink), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnNavigateUriChanged), new CoerceValueCallback(Hyperlink.CoerceNavigateUri)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Hyperlink.TargetName" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Hyperlink.TargetName" /> dependency property.</returns>
		// Token: 0x04001E6C RID: 7788
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(Hyperlink), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04001E6F RID: 7791
		internal static readonly RoutedEvent RequestSetStatusBarEvent;

		// Token: 0x04001E70 RID: 7792
		[ThreadStatic]
		private static SecurityCriticalDataForSet<Uri> s_cachedNavigateUri;

		// Token: 0x04001E71 RID: 7793
		[ThreadStatic]
		private static SecurityCriticalDataForSet<int?> s_criticalNavigateUriProtectee;

		// Token: 0x04001E72 RID: 7794
		private static SecurityCriticalDataForSet<bool?> s_shouldPreventUriSpoofing;

		// Token: 0x04001E73 RID: 7795
		private bool _canExecute = true;

		// Token: 0x04001E74 RID: 7796
		private static readonly DependencyProperty IsHyperlinkPressedProperty;

		// Token: 0x04001E75 RID: 7797
		private static DependencyObjectType _dType;
	}
}

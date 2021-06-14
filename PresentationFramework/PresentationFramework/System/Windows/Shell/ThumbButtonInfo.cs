using System;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Shell
{
	/// <summary>Represents information about how to display a button in the Windows 7 taskbar thumbnail.</summary>
	// Token: 0x0200014D RID: 333
	public sealed class ThumbButtonInfo : Freezable, ICommandSource
	{
		// Token: 0x06000EA5 RID: 3749 RVA: 0x0003848F File Offset: 0x0003668F
		protected override Freezable CreateInstanceCore()
		{
			return new ThumbButtonInfo();
		}

		/// <summary>Gets or sets a value that specifies the display state of the thumbnail button.</summary>
		/// <returns>An enumeration value that specifies the display state of the thumbnail button. The default is <see cref="F:System.Windows.Visibility.Visible" />.</returns>
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x00038496 File Offset: 0x00036696
		// (set) Token: 0x06000EA7 RID: 3751 RVA: 0x000384A8 File Offset: 0x000366A8
		public Visibility Visibility
		{
			get
			{
				return (Visibility)base.GetValue(ThumbButtonInfo.VisibilityProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.VisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the taskbar thumbnail closes when the thumbnail button is clicked.</summary>
		/// <returns>
		///     <see langword="true" /> if the thumbnail closes; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x000384BB File Offset: 0x000366BB
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x000384CD File Offset: 0x000366CD
		public bool DismissWhenClicked
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.DismissWhenClickedProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DismissWhenClickedProperty, value);
			}
		}

		/// <summary>Gets or sets the image that is displayed on the thumbnail button.</summary>
		/// <returns>The image that is displayed on the thumbnail button. The default is <see langword="null" />.</returns>
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x000384DB File Offset: 0x000366DB
		// (set) Token: 0x06000EAB RID: 3755 RVA: 0x000384ED File Offset: 0x000366ED
		public ImageSource ImageSource
		{
			get
			{
				return (ImageSource)base.GetValue(ThumbButtonInfo.ImageSourceProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.ImageSourceProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a border and highlight is displayed around the thumbnail button.</summary>
		/// <returns>
		///     <see langword="true" /> if a border and highlight is displayed around the thumbnail button; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x000384FB File Offset: 0x000366FB
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x0003850D File Offset: 0x0003670D
		public bool IsBackgroundVisible
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsBackgroundVisibleProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsBackgroundVisibleProperty, value);
			}
		}

		/// <summary>Gets or sets the text to display for the thumbnail button tooltip.</summary>
		/// <returns>The text to display for the thumbnail button tooltip. The default is an empty string.</returns>
		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0003851B File Offset: 0x0003671B
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x0003852D File Offset: 0x0003672D
		public string Description
		{
			get
			{
				return (string)base.GetValue(ThumbButtonInfo.DescriptionProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.DescriptionProperty, value);
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0003853C File Offset: 0x0003673C
		private static object CoerceDescription(DependencyObject d, object value)
		{
			string text = (string)value;
			if (text != null && text.Length >= 260)
			{
				text = text.Substring(0, 259);
			}
			return text;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00038570 File Offset: 0x00036770
		private object CoerceIsEnabledValue(object value)
		{
			bool flag = (bool)value;
			return flag && this.CanExecute;
		}

		/// <summary>Gets or sets a value that indicates whether the thumbnail button is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if the thumbnail button is enabled; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x00038595 File Offset: 0x00036795
		// (set) Token: 0x06000EB3 RID: 3763 RVA: 0x000385A7 File Offset: 0x000367A7
		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsEnabledProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsEnabledProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can interact with the thumbnail button.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can interact with the thumbnail button; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x000385B5 File Offset: 0x000367B5
		// (set) Token: 0x06000EB5 RID: 3765 RVA: 0x000385C7 File Offset: 0x000367C7
		public bool IsInteractive
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo.IsInteractiveProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.IsInteractiveProperty, value);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x000385D8 File Offset: 0x000367D8
		private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
		{
			ICommand command = (ICommand)e.OldValue;
			ICommand command2 = (ICommand)e.NewValue;
			if (command == command2)
			{
				return;
			}
			if (command != null)
			{
				this.UnhookCommand(command);
			}
			if (command2 != null)
			{
				this.HookCommand(command2);
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00038618 File Offset: 0x00036818
		// (set) Token: 0x06000EB8 RID: 3768 RVA: 0x0003862A File Offset: 0x0003682A
		private bool CanExecute
		{
			get
			{
				return (bool)base.GetValue(ThumbButtonInfo._CanExecuteProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo._CanExecuteProperty, value);
			}
		}

		/// <summary>Occurs when the thumbnail button is clicked.</summary>
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000EB9 RID: 3769 RVA: 0x00038638 File Offset: 0x00036838
		// (remove) Token: 0x06000EBA RID: 3770 RVA: 0x00038670 File Offset: 0x00036870
		public event EventHandler Click;

		// Token: 0x06000EBB RID: 3771 RVA: 0x000386A8 File Offset: 0x000368A8
		internal void InvokeClick()
		{
			EventHandler click = this.Click;
			if (click != null)
			{
				click(this, EventArgs.Empty);
			}
			this._InvokeCommand();
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x000386D4 File Offset: 0x000368D4
		private void _InvokeCommand()
		{
			ICommand command = this.Command;
			if (command != null)
			{
				object commandParameter = this.CommandParameter;
				IInputElement commandTarget = this.CommandTarget;
				RoutedCommand routedCommand = command as RoutedCommand;
				if (routedCommand != null)
				{
					if (routedCommand.CanExecute(commandParameter, commandTarget))
					{
						routedCommand.Execute(commandParameter, commandTarget);
						return;
					}
				}
				else if (command.CanExecute(commandParameter))
				{
					command.Execute(commandParameter);
				}
			}
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00038726 File Offset: 0x00036926
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00038740 File Offset: 0x00036940
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0003875A File Offset: 0x0003695A
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00038764 File Offset: 0x00036964
		private void UpdateCanExecute()
		{
			if (this.Command == null)
			{
				this.CanExecute = true;
				return;
			}
			object commandParameter = this.CommandParameter;
			IInputElement commandTarget = this.CommandTarget;
			RoutedCommand routedCommand = this.Command as RoutedCommand;
			if (routedCommand != null)
			{
				this.CanExecute = routedCommand.CanExecute(commandParameter, commandTarget);
				return;
			}
			this.CanExecute = this.Command.CanExecute(commandParameter);
		}

		/// <summary>Gets or sets the command to invoke when this thumbnail button is clicked.</summary>
		/// <returns>The command to invoke when this thumbnail button is clicked. The default is <see langword="null" />.</returns>
		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x000387BF File Offset: 0x000369BF
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x000387D1 File Offset: 0x000369D1
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(ThumbButtonInfo.CommandProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandProperty, value);
			}
		}

		/// <summary>Gets or sets the parameter to pass to the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Command" /> property.</summary>
		/// <returns>The parameter to pass to the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Command" /> property. The default is <see langword="null" />.</returns>
		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x000387DF File Offset: 0x000369DF
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x000387EC File Offset: 0x000369EC
		public object CommandParameter
		{
			get
			{
				return base.GetValue(ThumbButtonInfo.CommandParameterProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandParameterProperty, value);
			}
		}

		/// <summary>Gets or sets the element on which to raise the specified command.</summary>
		/// <returns>The element on which to raise the specified command. The default is <see langword="null" />.</returns>
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x000387FA File Offset: 0x000369FA
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0003880C File Offset: 0x00036A0C
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(ThumbButtonInfo.CommandTargetProperty);
			}
			set
			{
				base.SetValue(ThumbButtonInfo.CommandTargetProperty, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Visibility" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Visibility" /> dependency property.</returns>
		// Token: 0x04001144 RID: 4420
		public static readonly DependencyProperty VisibilityProperty = DependencyProperty.Register("Visibility", typeof(Visibility), typeof(ThumbButtonInfo), new PropertyMetadata(Visibility.Visible));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.DismissWhenClicked" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.DismissWhenClicked" /> dependency property.</returns>
		// Token: 0x04001145 RID: 4421
		public static readonly DependencyProperty DismissWhenClickedProperty = DependencyProperty.Register("DismissWhenClicked", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.ImageSource" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.ImageSource" /> dependency property.</returns>
		// Token: 0x04001146 RID: 4422
		public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ThumbButtonInfo), new PropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsBackgroundVisible" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsBackgroundVisible" /> dependency property.</returns>
		// Token: 0x04001147 RID: 4423
		public static readonly DependencyProperty IsBackgroundVisibleProperty = DependencyProperty.Register("IsBackgroundVisible", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Description" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Description" /> dependency property.</returns>
		// Token: 0x04001148 RID: 4424
		public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ThumbButtonInfo), new PropertyMetadata(string.Empty, null, new CoerceValueCallback(ThumbButtonInfo.CoerceDescription)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsEnabled" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsEnabled" /> dependency property.</returns>
		// Token: 0x04001149 RID: 4425
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, null, (DependencyObject d, object e) => ((ThumbButtonInfo)d).CoerceIsEnabledValue(e)));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsInteractive" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.IsInteractive" /> dependency property.</returns>
		// Token: 0x0400114A RID: 4426
		public static readonly DependencyProperty IsInteractiveProperty = DependencyProperty.Register("IsInteractive", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Command" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.Command" /> dependency property.</returns>
		// Token: 0x0400114B RID: 4427
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).OnCommandChanged(e);
		}));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.CommandParameter" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.CommandParameter" /> dependency property.</returns>
		// Token: 0x0400114C RID: 4428
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).UpdateCanExecute();
		}));

		/// <summary>Identifies the <see cref="P:System.Windows.Shell.ThumbButtonInfo.CommandTarget" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shell.ThumbButtonInfo.CommandTarget" /> dependency property.</returns>
		// Token: 0x0400114D RID: 4429
		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ThumbButtonInfo), new PropertyMetadata(null, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThumbButtonInfo)d).UpdateCanExecute();
		}));

		// Token: 0x0400114E RID: 4430
		private static readonly DependencyProperty _CanExecuteProperty = DependencyProperty.Register("_CanExecute", typeof(bool), typeof(ThumbButtonInfo), new PropertyMetadata(true, delegate(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ThumbButtonInfo.IsEnabledProperty);
		}));
	}
}

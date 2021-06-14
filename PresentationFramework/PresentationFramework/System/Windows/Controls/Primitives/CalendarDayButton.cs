using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a day on a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x02000578 RID: 1400
	public sealed class CalendarDayButton : Button
	{
		// Token: 0x06005C3E RID: 23614 RVA: 0x0019EB24 File Offset: 0x0019CD24
		static CalendarDayButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarDayButton), new FrameworkPropertyMetadata(typeof(CalendarDayButton)));
		}

		/// <summary>Gets a value that indicates whether the date represented by this button is the current date.</summary>
		/// <returns>
		///     <see langword="true" /> if the date is the current date; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x0019ECC1 File Offset: 0x0019CEC1
		public bool IsToday
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsTodayProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the date represented by this button is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the date is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x0019ECD3 File Offset: 0x0019CED3
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsSelectedProperty);
			}
		}

		/// <summary>Gets a value that indicates whether this button represents a day that is not in the currently displayed month.</summary>
		/// <returns>
		///     <see langword="true" /> if the button represents a day that is not in the currently displayed month; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06005C42 RID: 23618 RVA: 0x0019ECE5 File Offset: 0x0019CEE5
		public bool IsInactive
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsInactiveProperty);
			}
		}

		/// <summary>Gets a value that indicates whether the date is unavailable.</summary>
		/// <returns>
		///     <see langword="true" /> if the date unavailable; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x06005C43 RID: 23619 RVA: 0x0019ECF7 File Offset: 0x0019CEF7
		public bool IsBlackedOut
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsBlackedOutProperty);
			}
		}

		/// <summary>Gets a value that indicates whether this button is highlighted.</summary>
		/// <returns>
		///     <see langword="true" /> if the button is highlighted; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x06005C44 RID: 23620 RVA: 0x0019ED09 File Offset: 0x0019CF09
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(CalendarDayButton.IsHighlightedProperty);
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x06005C45 RID: 23621 RVA: 0x0019ED1B File Offset: 0x0019CF1B
		// (set) Token: 0x06005C46 RID: 23622 RVA: 0x0019ED23 File Offset: 0x0019CF23
		internal Calendar Owner { get; set; }

		// Token: 0x06005C47 RID: 23623 RVA: 0x0019EB0C File Offset: 0x0019CD0C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarButtonAutomationPeer(this);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x0019ED2C File Offset: 0x0019CF2C
		internal override void ChangeVisualState(bool useTransitions)
		{
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Active",
				"Inactive"
			});
			if (this.IsInactive)
			{
				VisualStateManager.GoToState(this, "Inactive", useTransitions);
			}
			VisualStateManager.GoToState(this, "RegularDay", useTransitions);
			if (this.IsToday && this.Owner != null && this.Owner.IsTodayHighlighted)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Today",
					"RegularDay"
				});
			}
			VisualStateManager.GoToState(this, "Unselected", useTransitions);
			if (this.IsSelected || this.IsHighlighted)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			if (this.IsBlackedOut)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"BlackoutDay",
					"NormalDay"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "NormalDay", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"CalendarButtonFocused",
					"CalendarButtonUnfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "CalendarButtonUnfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x001420D5 File Offset: 0x001402D5
		internal void NotifyNeedsVisualStateUpdate()
		{
			base.UpdateVisualState();
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x0019EB14 File Offset: 0x0019CD14
		internal void SetContentInternal(string value)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, value);
		}

		// Token: 0x04002FAF RID: 12207
		private const int DEFAULTCONTENT = 1;

		// Token: 0x04002FB0 RID: 12208
		internal static readonly DependencyPropertyKey IsTodayPropertyKey = DependencyProperty.RegisterReadOnly("IsToday", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsToday" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsToday" /> dependency property.</returns>
		// Token: 0x04002FB1 RID: 12209
		public static readonly DependencyProperty IsTodayProperty = CalendarDayButton.IsTodayPropertyKey.DependencyProperty;

		// Token: 0x04002FB2 RID: 12210
		internal static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsSelected" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsSelected" /> dependency property.</returns>
		// Token: 0x04002FB3 RID: 12211
		public static readonly DependencyProperty IsSelectedProperty = CalendarDayButton.IsSelectedPropertyKey.DependencyProperty;

		// Token: 0x04002FB4 RID: 12212
		internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly("IsInactive", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsInactive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsInactive" /> dependency property.</returns>
		// Token: 0x04002FB5 RID: 12213
		public static readonly DependencyProperty IsInactiveProperty = CalendarDayButton.IsInactivePropertyKey.DependencyProperty;

		// Token: 0x04002FB6 RID: 12214
		internal static readonly DependencyPropertyKey IsBlackedOutPropertyKey = DependencyProperty.RegisterReadOnly("IsBlackedOut", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsBlackedOut" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsBlackedOut" /> dependency property.</returns>
		// Token: 0x04002FB7 RID: 12215
		public static readonly DependencyProperty IsBlackedOutProperty = CalendarDayButton.IsBlackedOutPropertyKey.DependencyProperty;

		// Token: 0x04002FB8 RID: 12216
		internal static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(CalendarDayButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsHighlighted" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarDayButton.IsHighlighted" /> dependency property.</returns>
		// Token: 0x04002FB9 RID: 12217
		public static readonly DependencyProperty IsHighlightedProperty = CalendarDayButton.IsHighlightedPropertyKey.DependencyProperty;
	}
}

using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a month or year on a <see cref="T:System.Windows.Controls.Calendar" /> object.</summary>
	// Token: 0x02000577 RID: 1399
	public sealed class CalendarButton : Button
	{
		// Token: 0x06005C33 RID: 23603 RVA: 0x0019E940 File Offset: 0x0019CB40
		static CalendarButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(CalendarButton), new FrameworkPropertyMetadata(typeof(CalendarButton)));
		}

		/// <summary>Gets a value that indicates whether this button represents a year or month that contains selected dates.</summary>
		/// <returns>
		///     <see langword="true" /> if this button represents a year or month that contains selected dates; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x06005C35 RID: 23605 RVA: 0x0019EA0A File Offset: 0x0019CC0A
		// (set) Token: 0x06005C36 RID: 23606 RVA: 0x0019EA1C File Offset: 0x0019CC1C
		public bool HasSelectedDays
		{
			get
			{
				return (bool)base.GetValue(CalendarButton.HasSelectedDaysProperty);
			}
			internal set
			{
				base.SetValue(CalendarButton.HasSelectedDaysPropertyKey, value);
			}
		}

		/// <summary>Gets a value that indicates whether this button represents a year that is not in the currently displayed decade.</summary>
		/// <returns>
		///     <see langword="true" /> if this button represents a day that is not in the currently displayed month, or a year that is not in the currently displayed decade; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x06005C37 RID: 23607 RVA: 0x0019EA2A File Offset: 0x0019CC2A
		// (set) Token: 0x06005C38 RID: 23608 RVA: 0x0019EA3C File Offset: 0x0019CC3C
		public bool IsInactive
		{
			get
			{
				return (bool)base.GetValue(CalendarButton.IsInactiveProperty);
			}
			internal set
			{
				base.SetValue(CalendarButton.IsInactivePropertyKey, value);
			}
		}

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x06005C39 RID: 23609 RVA: 0x0019EA4A File Offset: 0x0019CC4A
		// (set) Token: 0x06005C3A RID: 23610 RVA: 0x0019EA52 File Offset: 0x0019CC52
		internal Calendar Owner { get; set; }

		// Token: 0x06005C3B RID: 23611 RVA: 0x0019EA5C File Offset: 0x0019CC5C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (this.HasSelectedDays)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
			}
			if (!this.IsInactive)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Active",
					"Inactive"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Inactive", useTransitions);
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

		// Token: 0x06005C3C RID: 23612 RVA: 0x0019EB0C File Offset: 0x0019CD0C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new CalendarButtonAutomationPeer(this);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x0019EB14 File Offset: 0x0019CD14
		internal void SetContentInternal(string value)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, value);
		}

		// Token: 0x04002FAA RID: 12202
		internal static readonly DependencyPropertyKey HasSelectedDaysPropertyKey = DependencyProperty.RegisterReadOnly("HasSelectedDays", typeof(bool), typeof(CalendarButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarButton.HasSelectedDays" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarButton.HasSelectedDays" /> dependency property.</returns>
		// Token: 0x04002FAB RID: 12203
		public static readonly DependencyProperty HasSelectedDaysProperty = CalendarButton.HasSelectedDaysPropertyKey.DependencyProperty;

		// Token: 0x04002FAC RID: 12204
		internal static readonly DependencyPropertyKey IsInactivePropertyKey = DependencyProperty.RegisterReadOnly("IsInactive", typeof(bool), typeof(CalendarButton), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.CalendarButton.IsInactive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.CalendarButton.IsInactive" /> dependency property.</returns>
		// Token: 0x04002FAD RID: 12205
		public static readonly DependencyProperty IsInactiveProperty = CalendarButton.IsInactivePropertyKey.DependencyProperty;
	}
}

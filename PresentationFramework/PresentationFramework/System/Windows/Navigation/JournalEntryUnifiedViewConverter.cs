using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Navigation
{
	/// <summary>
	///     <see cref="T:System.Windows.Navigation.JournalEntryUnifiedViewConverter" /> merges navigation back history and navigation forward history (as exposed by <see cref="T:System.Windows.Controls.Frame" /> or <see cref="T:System.Windows.Navigation.NavigationWindow" />) into a single, Windows Internet Explorer 7-style navigation menu.</summary>
	// Token: 0x02000307 RID: 775
	public sealed class JournalEntryUnifiedViewConverter : IMultiValueConverter
	{
		/// <summary>Gets the <see cref="P:System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPosition" /> attached property for the specified element.</summary>
		/// <param name="element">The element from which to get the attached property value.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPosition" /> attached property of the journal entry for the specified element. </returns>
		// Token: 0x06002916 RID: 10518 RVA: 0x000BDFE9 File Offset: 0x000BC1E9
		public static JournalEntryPosition GetJournalEntryPosition(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (JournalEntryPosition)element.GetValue(JournalEntryUnifiedViewConverter.JournalEntryPositionProperty);
		}

		/// <summary>Sets the <see cref="F:System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPositionProperty" /> attached property of the specified element.</summary>
		/// <param name="element">The element on which to set the attached property value.</param>
		/// <param name="position">Position of the <see cref="T:System.Windows.Navigation.JournalEntryPosition" /> object.</param>
		// Token: 0x06002917 RID: 10519 RVA: 0x000BE009 File Offset: 0x000BC209
		public static void SetJournalEntryPosition(DependencyObject element, JournalEntryPosition position)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(JournalEntryUnifiedViewConverter.JournalEntryPositionProperty, position);
		}

		/// <summary>Merges two navigation history stacks.</summary>
		/// <param name="values">An array of two navigation stacks. </param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>An <see cref="T:System.Collections.IEnumerable" /> that can be used to enumerate the merged list of navigation history stacks if neither passed navigation stack is <see langword="null" />. <see langword="null" />, otherwise.</returns>
		// Token: 0x06002918 RID: 10520 RVA: 0x000BE02C File Offset: 0x000BC22C
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values != null && values.Length == 2)
			{
				JournalEntryStack journalEntryStack = values[0] as JournalEntryStack;
				JournalEntryStack journalEntryStack2 = values[1] as JournalEntryStack;
				if (journalEntryStack != null && journalEntryStack2 != null)
				{
					LimitedJournalEntryStackEnumerable backStack = (LimitedJournalEntryStackEnumerable)journalEntryStack.GetLimitedJournalEntryStackEnumerable();
					LimitedJournalEntryStackEnumerable forwardStack = (LimitedJournalEntryStackEnumerable)journalEntryStack2.GetLimitedJournalEntryStackEnumerable();
					return new UnifiedJournalEntryStackEnumerable(backStack, forwardStack);
				}
			}
			return null;
		}

		/// <summary>Not implemented.</summary>
		/// <param name="value">This parameter is not used.</param>
		/// <param name="targetTypes">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>Always returns <see cref="F:System.Windows.Data.Binding.DoNothing" />.</returns>
		// Token: 0x06002919 RID: 10521 RVA: 0x000BE07B File Offset: 0x000BC27B
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new object[]
			{
				Binding.DoNothing
			};
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPosition" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Navigation.JournalEntryUnifiedViewConverter.JournalEntryPosition" /> attached property.</returns>
		// Token: 0x04001BCB RID: 7115
		public static readonly DependencyProperty JournalEntryPositionProperty = DependencyProperty.RegisterAttached("JournalEntryPosition", typeof(JournalEntryPosition), typeof(JournalEntryUnifiedViewConverter), new PropertyMetadata(JournalEntryPosition.Current));
	}
}

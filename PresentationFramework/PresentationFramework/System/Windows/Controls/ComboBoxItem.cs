using System;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Implements a selectable item inside a <see cref="T:System.Windows.Controls.ComboBox" />.  </summary>
	// Token: 0x02000486 RID: 1158
	[Localizability(LocalizationCategory.ComboBox)]
	public class ComboBoxItem : ListBoxItem
	{
		// Token: 0x060043BE RID: 17342 RVA: 0x00135AE0 File Offset: 0x00133CE0
		static ComboBoxItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxItem), new FrameworkPropertyMetadata(typeof(ComboBoxItem)));
			ComboBoxItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ComboBoxItem));
		}

		/// <summary>Gets a value that indicates whether the item is highlighted.   </summary>
		/// <returns>
		///     <see langword="true" /> if a <see cref="T:System.Windows.Controls.ComboBoxItem" /> is highlighted; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x060043BF RID: 17343 RVA: 0x00135B60 File Offset: 0x00133D60
		// (set) Token: 0x060043C0 RID: 17344 RVA: 0x00135B72 File Offset: 0x00133D72
		public bool IsHighlighted
		{
			get
			{
				return (bool)base.GetValue(ComboBoxItem.IsHighlightedProperty);
			}
			protected set
			{
				base.SetValue(ComboBoxItem.IsHighlightedPropertyKey, BooleanBoxes.Box(value));
			}
		}

		/// <summary> Responds to the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.MouseButtonEventArgs" />.</param>
		// Token: 0x060043C1 RID: 17345 RVA: 0x00135B88 File Offset: 0x00133D88
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemMouseDown(this);
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary> Responds to the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.MouseButtonEventArgs" />.</param>
		// Token: 0x060043C2 RID: 17346 RVA: 0x00135BB4 File Offset: 0x00133DB4
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemMouseUp(this);
			}
			base.OnMouseLeftButtonUp(e);
		}

		/// <summary> Responds to a <see cref="E:System.Windows.UIElement.MouseEnter" /> event. </summary>
		/// <param name="e">Provides data for <see cref="T:System.Windows.Input.MouseEventArgs" />.</param>
		// Token: 0x060043C3 RID: 17347 RVA: 0x00135BE0 File Offset: 0x00133DE0
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemEnter(this);
			}
			base.OnMouseEnter(e);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ContentControl.Content" /> property changes. </summary>
		/// <param name="oldContent">Old content.</param>
		/// <param name="newContent">New content.</param>
		// Token: 0x060043C4 RID: 17348 RVA: 0x00135C0C File Offset: 0x00133E0C
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			ComboBox parentComboBox;
			if (base.IsSelected && (parentComboBox = this.ParentComboBox) != null)
			{
				parentComboBox.SelectedItemUpdated();
			}
			base.SetFlags(newContent is UIElement, VisualFlags.IsLayoutIslandRoot);
		}

		/// <summary>Announces that the keyboard is focused on this element. </summary>
		/// <param name="e">Keyboard input event arguments.</param>
		// Token: 0x060043C5 RID: 17349 RVA: 0x00135C50 File Offset: 0x00133E50
		protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			e.Handled = true;
			ComboBox parentComboBox = this.ParentComboBox;
			if (parentComboBox != null)
			{
				parentComboBox.NotifyComboBoxItemEnter(this);
			}
			base.OnGotKeyboardFocus(e);
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x00135C7C File Offset: 0x00133E7C
		private ComboBox ParentComboBox
		{
			get
			{
				return base.ParentSelector as ComboBox;
			}
		}

		// Token: 0x060043C7 RID: 17351 RVA: 0x00135C89 File Offset: 0x00133E89
		internal void SetIsHighlighted(bool isHighlighted)
		{
			this.IsHighlighted = isHighlighted;
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x00135C92 File Offset: 0x00133E92
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ComboBoxItem._dType;
			}
		}

		// Token: 0x04002860 RID: 10336
		private static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted", typeof(bool), typeof(ComboBoxItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ComboBoxItem.IsHighlighted" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ComboBoxItem.IsHighlighted" /> dependency property.</returns>
		// Token: 0x04002861 RID: 10337
		public static readonly DependencyProperty IsHighlightedProperty = ComboBoxItem.IsHighlightedPropertyKey.DependencyProperty;

		// Token: 0x04002862 RID: 10338
		private static DependencyObjectType _dType;
	}
}

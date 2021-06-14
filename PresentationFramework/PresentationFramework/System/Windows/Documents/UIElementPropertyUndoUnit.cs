using System;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x0200036E RID: 878
	internal class UIElementPropertyUndoUnit : IUndoUnit
	{
		// Token: 0x06002F0C RID: 12044 RVA: 0x000D4AEE File Offset: 0x000D2CEE
		private UIElementPropertyUndoUnit(UIElement uiElement, DependencyProperty property, object oldValue)
		{
			this._uiElement = uiElement;
			this._property = property;
			this._oldValue = oldValue;
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000D4B0B File Offset: 0x000D2D0B
		public void Do()
		{
			if (this._oldValue != DependencyProperty.UnsetValue)
			{
				this._uiElement.SetValue(this._property, this._oldValue);
				return;
			}
			this._uiElement.ClearValue(this._property);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000D4B43 File Offset: 0x000D2D43
		public bool Merge(IUndoUnit unit)
		{
			Invariant.Assert(unit != null);
			return false;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000D4B4F File Offset: 0x000D2D4F
		internal static void Add(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, HorizontalAlignment newValue)
		{
			UIElementPropertyUndoUnit.AddPrivate(textContainer, uiElement, property, newValue);
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000D4B5F File Offset: 0x000D2D5F
		internal static void Add(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, FlowDirection newValue)
		{
			UIElementPropertyUndoUnit.AddPrivate(textContainer, uiElement, property, newValue);
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000D4B70 File Offset: 0x000D2D70
		private static void AddPrivate(ITextContainer textContainer, UIElement uiElement, DependencyProperty property, object newValue)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(textContainer);
			if (orClearUndoManager == null)
			{
				return;
			}
			object obj = uiElement.ReadLocalValue(property);
			if (obj is Expression)
			{
				if (orClearUndoManager.IsEnabled)
				{
					orClearUndoManager.Clear();
				}
				return;
			}
			if (obj.Equals(newValue))
			{
				return;
			}
			orClearUndoManager.Add(new UIElementPropertyUndoUnit(uiElement, property, obj));
		}

		// Token: 0x04001E1E RID: 7710
		private readonly UIElement _uiElement;

		// Token: 0x04001E1F RID: 7711
		private readonly DependencyProperty _property;

		// Token: 0x04001E20 RID: 7712
		private readonly object _oldValue;
	}
}

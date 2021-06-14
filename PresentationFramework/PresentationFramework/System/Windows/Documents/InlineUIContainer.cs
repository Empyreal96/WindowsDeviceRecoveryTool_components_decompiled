using System;
using System.Windows.Markup;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>An inline-level flow content element which enables <see cref="T:System.Windows.UIElement" /> elements (i.e. a <see cref="T:System.Windows.Controls.Button" />) to be embedded (hosted) in flow content.</summary>
	// Token: 0x0200037E RID: 894
	[ContentProperty("Child")]
	[TextElementEditingBehavior(IsMergeable = false)]
	public class InlineUIContainer : Inline
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.InlineUIContainer" /> class.</summary>
		// Token: 0x060030A2 RID: 12450 RVA: 0x000DB589 File Offset: 0x000D9789
		public InlineUIContainer()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.InlineUIContainer" /> class, taking a specified <see cref="T:System.Windows.UIElement" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.InlineUIContainer" />.</summary>
		/// <param name="childUIElement">An <see cref="T:System.Windows.UIElement" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.InlineUIContainer" />.</param>
		// Token: 0x060030A3 RID: 12451 RVA: 0x000DB591 File Offset: 0x000D9791
		public InlineUIContainer(UIElement childUIElement) : this(childUIElement, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.InlineUIContainer" /> class, taking a specified <see cref="T:System.Windows.UIElement" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.InlineUIContainer" />, and a <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position for the new <see cref="T:System.Windows.Documents.InlineUIContainer" /> element.</summary>
		/// <param name="childUIElement">A <see cref="T:System.Windows.UIElement" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.InlineUIContainer" />.  This parameter may be <see langword="null" />, in which case no <see cref="T:System.Windows.UIElement" /> is inserted.</param>
		/// <param name="insertionPosition">A <see cref="T:System.Windows.Documents.TextPointer" /> specifying an insertion position at which to insert the <see cref="T:System.Windows.Documents.InlineUIContainer" /> element after it is created, or <see langword="null" /> for no automatic insertion.</param>
		// Token: 0x060030A4 RID: 12452 RVA: 0x000DB59C File Offset: 0x000D979C
		public InlineUIContainer(UIElement childUIElement, TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
				this.Child = childUIElement;
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.UIElement" /> hosted by the <see cref="T:System.Windows.Documents.InlineUIContainer" />.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> hosted by the <see cref="T:System.Windows.Documents.InlineUIContainer" />.</returns>
		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x060030A5 RID: 12453 RVA: 0x000C4572 File Offset: 0x000C2772
		// (set) Token: 0x060030A6 RID: 12454 RVA: 0x000DB5F0 File Offset: 0x000D97F0
		public UIElement Child
		{
			get
			{
				return base.ContentStart.GetAdjacentElement(LogicalDirection.Forward) as UIElement;
			}
			set
			{
				TextContainer textContainer = base.TextContainer;
				textContainer.BeginChange();
				try
				{
					TextPointer contentStart = base.ContentStart;
					UIElement child = this.Child;
					if (child != null)
					{
						textContainer.DeleteContentInternal(contentStart, base.ContentEnd);
						TextElement.ContainerTextElementField.ClearValue(child);
					}
					if (value != null)
					{
						TextElement.ContainerTextElementField.SetValue(value, this);
						contentStart.InsertUIElement(value);
					}
				}
				finally
				{
					textContainer.EndChange();
				}
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x000DB664 File Offset: 0x000D9864
		internal UIElementIsland UIElementIsland
		{
			get
			{
				this.UpdateUIElementIsland();
				return this._uiElementIsland;
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000DB674 File Offset: 0x000D9874
		private void UpdateUIElementIsland()
		{
			UIElement child = this.Child;
			if (this._uiElementIsland == null || this._uiElementIsland.Root != child)
			{
				if (this._uiElementIsland != null)
				{
					this._uiElementIsland.Dispose();
					this._uiElementIsland = null;
				}
				if (child != null)
				{
					this._uiElementIsland = new UIElementIsland(child);
				}
			}
		}

		// Token: 0x04001E8B RID: 7819
		private UIElementIsland _uiElementIsland;
	}
}

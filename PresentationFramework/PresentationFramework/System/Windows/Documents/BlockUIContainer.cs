using System;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	/// <summary>A block-level flow content element which enables <see cref="T:System.Windows.UIElement" /> elements (i.e. a <see cref="T:System.Windows.Controls.Button" />) to be embedded (hosted) in flow content.</summary>
	// Token: 0x0200032F RID: 815
	[ContentProperty("Child")]
	public class BlockUIContainer : Block
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.BlockUIContainer" /> class.</summary>
		// Token: 0x06002B0C RID: 11020 RVA: 0x000C454D File Offset: 0x000C274D
		public BlockUIContainer()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.BlockUIContainer" /> class, taking a specified <see cref="T:System.Windows.UIElement" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.BlockUIContainer" />.</summary>
		/// <param name="uiElement">An <see cref="T:System.Windows.UIElement" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.BlockUIContainer" />.</param>
		// Token: 0x06002B0D RID: 11021 RVA: 0x000C4555 File Offset: 0x000C2755
		public BlockUIContainer(UIElement uiElement)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			this.Child = uiElement;
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.UIElement" /> hosted by the <see cref="T:System.Windows.Documents.BlockUIContainer" />.</summary>
		/// <returns>The <see cref="T:System.Windows.UIElement" /> hosted by the <see cref="T:System.Windows.Documents.BlockUIContainer" />.</returns>
		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000C4572 File Offset: 0x000C2772
		// (set) Token: 0x06002B0F RID: 11023 RVA: 0x000C4588 File Offset: 0x000C2788
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
	}
}

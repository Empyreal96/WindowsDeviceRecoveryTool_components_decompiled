using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Documents.Inline" /> elements. <see cref="T:System.Windows.Documents.InlineCollection" /> defines the allowable child content of the <see cref="T:System.Windows.Documents.Paragraph" />, <see cref="T:System.Windows.Documents.Span" />, and <see cref="T:System.Windows.Controls.TextBlock" /> elements. </summary>
	// Token: 0x0200037D RID: 893
	[ContentWrapper(typeof(Run))]
	[ContentWrapper(typeof(InlineUIContainer))]
	[WhitespaceSignificantCollection]
	public class InlineCollection : TextElementCollection<Inline>, IList, ICollection, IEnumerable
	{
		// Token: 0x06003099 RID: 12441 RVA: 0x000DB375 File Offset: 0x000D9575
		internal InlineCollection(DependencyObject owner, bool isOwnerParent) : base(owner, isOwnerParent)
		{
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000DB380 File Offset: 0x000D9580
		internal override int OnAdd(object value)
		{
			string text = value as string;
			int result;
			if (text != null)
			{
				result = this.AddText(text, true);
			}
			else
			{
				base.TextContainer.BeginChange();
				try
				{
					UIElement uielement = value as UIElement;
					if (uielement != null)
					{
						result = this.AddUIElement(uielement, true);
					}
					else
					{
						result = base.OnAdd(value);
					}
				}
				finally
				{
					base.TextContainer.EndChange();
				}
			}
			return result;
		}

		/// <summary>Adds an implicit <see cref="T:System.Windows.Documents.Run" /> element with the given text, supplied as a <see cref="T:System.String" />.</summary>
		/// <param name="text">Text set as the <see cref="P:System.Windows.Documents.Run.Text" /> property for the implicit <see cref="T:System.Windows.Documents.Run" />.</param>
		// Token: 0x0600309B RID: 12443 RVA: 0x000DB3EC File Offset: 0x000D95EC
		public void Add(string text)
		{
			this.AddText(text, false);
		}

		/// <summary>Adds an implicit <see cref="T:System.Windows.Documents.InlineUIContainer" /> with the supplied <see cref="T:System.Windows.UIElement" /> already in it. </summary>
		/// <param name="uiElement">
		///       <see cref="T:System.Windows.UIElement" /> set as the <see cref="P:System.Windows.Documents.InlineUIContainer.Child" /> property for the implicit <see cref="T:System.Windows.Documents.InlineUIContainer" />.</param>
		// Token: 0x0600309C RID: 12444 RVA: 0x000DB3F7 File Offset: 0x000D95F7
		public void Add(UIElement uiElement)
		{
			this.AddUIElement(uiElement, false);
		}

		/// <summary>Gets the first <see cref="T:System.Windows.Documents.Inline" /> element within this instance of <see cref="T:System.Windows.Documents.InlineCollection" />.</summary>
		/// <returns>The first <see cref="T:System.Windows.Documents.Inline" /> element within this instance of <see cref="T:System.Windows.Documents.InlineCollection" />.</returns>
		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x000DB402 File Offset: 0x000D9602
		public Inline FirstInline
		{
			get
			{
				return base.FirstChild;
			}
		}

		/// <summary>Gets the last <see cref="T:System.Windows.Documents.Inline" /> element within this instance of <see cref="T:System.Windows.Documents.InlineCollection" />.</summary>
		/// <returns>The last <see cref="T:System.Windows.Documents.Inline" /> element within this instance of <see cref="T:System.Windows.Documents.InlineCollection" />.</returns>
		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x000DB40A File Offset: 0x000D960A
		public Inline LastInline
		{
			get
			{
				return base.LastChild;
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000DB414 File Offset: 0x000D9614
		internal override void ValidateChild(Inline child)
		{
			base.ValidateChild(child);
			if (base.Parent is TextElement)
			{
				TextSchema.ValidateChild((TextElement)base.Parent, child, true, true);
				return;
			}
			if (!TextSchema.IsValidChildOfContainer(base.Parent.GetType(), child.GetType()))
			{
				throw new InvalidOperationException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					base.Parent.GetType().Name,
					child.GetType().Name
				}));
			}
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000DB49C File Offset: 0x000D969C
		private int AddText(string text, bool returnIndex)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (base.Parent is TextBlock)
			{
				TextBlock textBlock = (TextBlock)base.Parent;
				if (!textBlock.HasComplexContent)
				{
					textBlock.Text += text;
					return 0;
				}
			}
			base.TextContainer.BeginChange();
			int result;
			try
			{
				Run run = Inline.CreateImplicitRun(base.Parent);
				int num;
				if (returnIndex)
				{
					num = base.OnAdd(run);
				}
				else
				{
					base.Add(run);
					num = -1;
				}
				run.Text = text;
				result = num;
			}
			finally
			{
				base.TextContainer.EndChange();
			}
			return result;
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000DB544 File Offset: 0x000D9744
		private int AddUIElement(UIElement uiElement, bool returnIndex)
		{
			if (uiElement == null)
			{
				throw new ArgumentNullException("uiElement");
			}
			InlineUIContainer inlineUIContainer = Inline.CreateImplicitInlineUIContainer(base.Parent);
			int result;
			if (returnIndex)
			{
				result = base.OnAdd(inlineUIContainer);
			}
			else
			{
				base.Add(inlineUIContainer);
				result = -1;
			}
			inlineUIContainer.Child = uiElement;
			return result;
		}
	}
}

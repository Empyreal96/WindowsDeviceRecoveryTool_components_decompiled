using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents an item in a list or outline. </summary>
	// Token: 0x02000452 RID: 1106
	public class ListItemStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.ListItemStructure" /> class. </summary>
		// Token: 0x06004013 RID: 16403 RVA: 0x00125D47 File Offset: 0x00123F47
		public ListItemStructure()
		{
			this._elementType = FixedElement.ElementType.ListItem;
		}

		/// <summary>Adds a block to a list item.</summary>
		/// <param name="element">The block to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06004014 RID: 16404 RVA: 0x00125B2E File Offset: 0x00123D2E
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x06004015 RID: 16405 RVA: 0x00125D58 File Offset: 0x00123F58
		void IAddChild.AddChild(object value)
		{
			if (value is ParagraphStructure || value is TableStructure || value is ListStructure || value is FigureStructure)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType4", new object[]
			{
				value.GetType(),
				typeof(ParagraphStructure),
				typeof(TableStructure),
				typeof(ListStructure),
				typeof(FigureStructure)
			}), "value");
		}

		/// <summary>Not implemented.</summary>
		/// <param name="text">Not used.</param>
		// Token: 0x06004016 RID: 16406 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004018 RID: 16408 RVA: 0x00125BDE File Offset: 0x00123DDE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		/// <summary>Gets or sets the name of the numeral, character, or bullet symbol for the list item as it appears in the formatting markup of the document.</summary>
		/// <returns>A <see cref="T:System.String" /> marking list item.</returns>
		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06004019 RID: 16409 RVA: 0x00125DEE File Offset: 0x00123FEE
		// (set) Token: 0x0600401A RID: 16410 RVA: 0x00125DF6 File Offset: 0x00123FF6
		public string Marker
		{
			get
			{
				return this._markerName;
			}
			set
			{
				this._markerName = value;
			}
		}

		// Token: 0x0400275B RID: 10075
		private string _markerName;
	}
}

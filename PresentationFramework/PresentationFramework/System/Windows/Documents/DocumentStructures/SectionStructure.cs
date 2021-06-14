using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a section of content in a document.</summary>
	// Token: 0x0200044E RID: 1102
	public class SectionStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.SectionStructure" /> class. </summary>
		// Token: 0x06003FFB RID: 16379 RVA: 0x00125B1E File Offset: 0x00123D1E
		public SectionStructure()
		{
			this._elementType = FixedElement.ElementType.Section;
		}

		/// <summary>Adds a block to the section.</summary>
		/// <param name="element">The block element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The element is <see langword="null" />.</exception>
		// Token: 0x06003FFC RID: 16380 RVA: 0x00125B2E File Offset: 0x00123D2E
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x06003FFD RID: 16381 RVA: 0x00125B48 File Offset: 0x00123D48
		void IAddChild.AddChild(object value)
		{
			if (value is ParagraphStructure || value is FigureStructure || value is ListStructure || value is TableStructure)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType4", new object[]
			{
				value.GetType(),
				typeof(ParagraphStructure),
				typeof(FigureStructure),
				typeof(ListStructure),
				typeof(TableStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06003FFE RID: 16382 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>System.Collections.IEnumerator</returns>
		// Token: 0x06004000 RID: 16384 RVA: 0x00125BDE File Offset: 0x00123DDE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a drawing, chart, or diagram in a document. </summary>
	// Token: 0x02000450 RID: 1104
	public class FigureStructure : SemanticBasicElement, IAddChild, IEnumerable<NamedElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.FigureStructure" /> class. </summary>
		// Token: 0x06004007 RID: 16391 RVA: 0x00125C57 File Offset: 0x00123E57
		public FigureStructure()
		{
			this._elementType = FixedElement.ElementType.Figure;
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> to add. </param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks. </exception>
		// Token: 0x06004008 RID: 16392 RVA: 0x00125C68 File Offset: 0x00123E68
		void IAddChild.AddChild(object value)
		{
			if (value is NamedElement)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(NamedElement)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.  </param>
		// Token: 0x06004009 RID: 16393 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		/// <summary>Add a named element to the figure.</summary>
		/// <param name="element">The element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The element is <see langword="null" />.</exception>
		// Token: 0x0600400A RID: 16394 RVA: 0x00125B2E File Offset: 0x00123D2E
		public void Add(NamedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		// Token: 0x0600400B RID: 16395 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<NamedElement> IEnumerable<NamedElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x0600400C RID: 16396 RVA: 0x00125C4F File Offset: 0x00123E4F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedElement>)this).GetEnumerator();
		}
	}
}

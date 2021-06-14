using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a set of one or more rows in a table.</summary>
	// Token: 0x02000454 RID: 1108
	public class TableRowGroupStructure : SemanticBasicElement, IAddChild, IEnumerable<TableRowStructure>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.TableRowGroupStructure" /> class. </summary>
		// Token: 0x06004021 RID: 16417 RVA: 0x00125E87 File Offset: 0x00124087
		public TableRowGroupStructure()
		{
			this._elementType = FixedElement.ElementType.TableRowGroup;
		}

		/// <summary>Adds a row to the table row group.</summary>
		/// <param name="tableRow">The row to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The row is null.</exception>
		// Token: 0x06004022 RID: 16418 RVA: 0x00125E97 File Offset: 0x00124097
		public void Add(TableRowStructure tableRow)
		{
			if (tableRow == null)
			{
				throw new ArgumentNullException("tableRow");
			}
			((IAddChild)this).AddChild(tableRow);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x06004023 RID: 16419 RVA: 0x00125EB0 File Offset: 0x001240B0
		void IAddChild.AddChild(object value)
		{
			if (value is TableRowStructure)
			{
				this._elementList.Add((TableRowStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004024 RID: 16420 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004025 RID: 16421 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<TableRowStructure> IEnumerable<TableRowStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004026 RID: 16422 RVA: 0x00125F07 File Offset: 0x00124107
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableRowStructure>)this).GetEnumerator();
		}
	}
}

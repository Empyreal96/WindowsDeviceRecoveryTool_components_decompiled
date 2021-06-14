using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a table in a document.</summary>
	// Token: 0x02000453 RID: 1107
	public class TableStructure : SemanticBasicElement, IAddChild, IEnumerable<TableRowGroupStructure>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.TableStructure" /> class. </summary>
		// Token: 0x0600401B RID: 16411 RVA: 0x00125DFF File Offset: 0x00123FFF
		public TableStructure()
		{
			this._elementType = FixedElement.ElementType.Table;
		}

		/// <summary>Adds a group of rows to a table.</summary>
		/// <param name="tableRowGroup">The group of rows to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The group of rows is <see langword="null" />.</exception>
		// Token: 0x0600401C RID: 16412 RVA: 0x00125E0F File Offset: 0x0012400F
		public void Add(TableRowGroupStructure tableRowGroup)
		{
			if (tableRowGroup == null)
			{
				throw new ArgumentNullException("tableRowGroup");
			}
			((IAddChild)this).AddChild(tableRowGroup);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x0600401D RID: 16413 RVA: 0x00125E28 File Offset: 0x00124028
		void IAddChild.AddChild(object value)
		{
			if (value is TableRowGroupStructure)
			{
				this._elementList.Add((TableRowGroupStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowGroupStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x0600401E RID: 16414 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<TableRowGroupStructure> IEnumerable<TableRowGroupStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004020 RID: 16416 RVA: 0x00125E7F File Offset: 0x0012407F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableRowGroupStructure>)this).GetEnumerator();
		}
	}
}

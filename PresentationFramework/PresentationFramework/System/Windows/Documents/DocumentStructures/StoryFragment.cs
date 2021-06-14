using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents all or part of a story within an XPS document.</summary>
	// Token: 0x02000458 RID: 1112
	[ContentProperty("BlockElementList")]
	public class StoryFragment : IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> class. </summary>
		// Token: 0x0600403E RID: 16446 RVA: 0x00126103 File Offset: 0x00124303
		public StoryFragment()
		{
			this._elementList = new List<BlockElement>();
		}

		/// <summary>Add a block to the story fragment.</summary>
		/// <param name="element">The block to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The block passed is <see langword="null" />.</exception>
		// Token: 0x0600403F RID: 16447 RVA: 0x00125B2E File Offset: 0x00123D2E
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x06004040 RID: 16448 RVA: 0x00126118 File Offset: 0x00124318
		void IAddChild.AddChild(object value)
		{
			if (value is SectionStructure || value is ParagraphStructure || value is FigureStructure || value is ListStructure || value is TableStructure || value is StoryBreak)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType6", new object[]
			{
				value.GetType(),
				typeof(SectionStructure),
				typeof(ParagraphStructure),
				typeof(FigureStructure),
				typeof(ListStructure),
				typeof(TableStructure),
				typeof(StoryBreak)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004041 RID: 16449 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x00041D30 File Offset: 0x0003FF30
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004043 RID: 16451 RVA: 0x00125BDE File Offset: 0x00123DDE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		/// <summary>Gets or sets the name of the story. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the name of the story.</returns>
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06004044 RID: 16452 RVA: 0x001261D8 File Offset: 0x001243D8
		// (set) Token: 0x06004045 RID: 16453 RVA: 0x001261E0 File Offset: 0x001243E0
		public string StoryName
		{
			get
			{
				return this._storyName;
			}
			set
			{
				this._storyName = value;
			}
		}

		/// <summary>Gets or sets the name of the story fragment. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the name of this fragment. </returns>
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06004046 RID: 16454 RVA: 0x001261E9 File Offset: 0x001243E9
		// (set) Token: 0x06004047 RID: 16455 RVA: 0x001261F1 File Offset: 0x001243F1
		public string FragmentName
		{
			get
			{
				return this._fragmentName;
			}
			set
			{
				this._fragmentName = value;
			}
		}

		/// <summary>Gets or sets the type of fragment. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the type of fragment.</returns>
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x001261FA File Offset: 0x001243FA
		// (set) Token: 0x06004049 RID: 16457 RVA: 0x00126202 File Offset: 0x00124402
		public string FragmentType
		{
			get
			{
				return this._fragmentType;
			}
			set
			{
				this._fragmentType = value;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x0012620B File Offset: 0x0012440B
		internal List<BlockElement> BlockElementList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400275F RID: 10079
		private List<BlockElement> _elementList;

		// Token: 0x04002760 RID: 10080
		private string _storyName;

		// Token: 0x04002761 RID: 10081
		private string _fragmentName;

		// Token: 0x04002762 RID: 10082
		private string _fragmentType;
	}
}

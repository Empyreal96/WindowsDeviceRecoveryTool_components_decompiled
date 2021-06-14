using System;

namespace System.Windows.Documents
{
	// Token: 0x0200041C RID: 1052
	internal class TextTreeExtractElementUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06003D17 RID: 15639 RVA: 0x0011BD38 File Offset: 0x00119F38
		internal TextTreeExtractElementUndoUnit(TextContainer tree, TextTreeTextElementNode elementNode) : base(tree, elementNode.GetSymbolOffset(tree.Generation))
		{
			this._symbolCount = elementNode.SymbolCount;
			this._type = elementNode.TextElement.GetType();
			this._localValues = TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement);
			this._resources = elementNode.TextElement.Resources;
			if (elementNode.TextElement is Table)
			{
				this._columns = TextTreeDeleteContentUndoUnit.SaveColumns((Table)elementNode.TextElement);
			}
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0011BDBC File Offset: 0x00119FBC
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer start = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset + this._symbolCount - 2, LogicalDirection.Forward);
			TextElement textElement = (TextElement)Activator.CreateInstance(this._type);
			textElement.Reposition(start, textPointer);
			textElement.Resources = this._resources;
			textPointer.MoveToNextContextPosition(LogicalDirection.Backward);
			base.TextContainer.SetValues(textPointer, TextTreeUndoUnit.ArrayToLocalValueEnumerator(this._localValues));
			if (textElement is Table)
			{
				TextTreeDeleteContentUndoUnit.RestoreColumns((Table)textElement, this._columns);
			}
		}

		// Token: 0x04002653 RID: 9811
		private readonly int _symbolCount;

		// Token: 0x04002654 RID: 9812
		private readonly Type _type;

		// Token: 0x04002655 RID: 9813
		private readonly PropertyRecord[] _localValues;

		// Token: 0x04002656 RID: 9814
		private readonly ResourceDictionary _resources;

		// Token: 0x04002657 RID: 9815
		private readonly TableColumn[] _columns;
	}
}

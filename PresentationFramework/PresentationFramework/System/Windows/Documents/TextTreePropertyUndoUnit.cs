using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000422 RID: 1058
	internal class TextTreePropertyUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06003D79 RID: 15737 RVA: 0x0011C37B File Offset: 0x0011A57B
		internal TextTreePropertyUndoUnit(TextContainer tree, int symbolOffset, PropertyRecord propertyRecord) : base(tree, symbolOffset)
		{
			this._propertyRecord = propertyRecord;
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0011C38C File Offset: 0x0011A58C
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer textPointer = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			Invariant.Assert(textPointer.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart, "TextTree undo unit out of sync with TextTree.");
			if (this._propertyRecord.Value != DependencyProperty.UnsetValue)
			{
				base.TextContainer.SetValue(textPointer, this._propertyRecord.Property, this._propertyRecord.Value);
				return;
			}
			textPointer.Parent.ClearValue(this._propertyRecord.Property);
		}

		// Token: 0x04002669 RID: 9833
		private readonly PropertyRecord _propertyRecord;
	}
}

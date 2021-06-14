using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Represents a misspelled word in an editing control (i.e. <see cref="T:System.Windows.Controls.TextBox" /> or <see cref="T:System.Windows.Controls.RichTextBox" />).</summary>
	// Token: 0x02000561 RID: 1377
	public class SpellingError
	{
		// Token: 0x06005B5A RID: 23386 RVA: 0x0019C376 File Offset: 0x0019A576
		internal SpellingError(Speller speller, ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start.CompareTo(end) < 0);
			this._speller = speller;
			this._start = start.GetFrozenPointer(LogicalDirection.Forward);
			this._end = end.GetFrozenPointer(LogicalDirection.Backward);
		}

		/// <summary>Replaces the spelling error text with the specified correction.</summary>
		/// <param name="correctedText">The text used to replace the misspelled text.</param>
		// Token: 0x06005B5B RID: 23387 RVA: 0x0019C3B0 File Offset: 0x0019A5B0
		public void Correct(string correctedText)
		{
			if (correctedText == null)
			{
				correctedText = string.Empty;
			}
			ITextRange textRange = new TextRange(this._start, this._end);
			textRange.Text = correctedText;
		}

		/// <summary>Instructs the control to ignore this error and any duplicates for the remainder of the lifetime of the control.</summary>
		// Token: 0x06005B5C RID: 23388 RVA: 0x0019C3E0 File Offset: 0x0019A5E0
		public void IgnoreAll()
		{
			this._speller.IgnoreAll(TextRangeBase.GetTextInternal(this._start, this._end));
		}

		/// <summary>Gets a list of suggested spelling replacements for the misspelled word.</summary>
		/// <returns>The collection of spelling suggestions for the misspelled word.</returns>
		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x0019C400 File Offset: 0x0019A600
		public IEnumerable<string> Suggestions
		{
			get
			{
				IList suggestions = this._speller.GetSuggestionsForError(this);
				int num;
				for (int i = 0; i < suggestions.Count; i = num + 1)
				{
					yield return (string)suggestions[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x0019C41D File Offset: 0x0019A61D
		internal ITextPointer Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x0019C425 File Offset: 0x0019A625
		internal ITextPointer End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x04002F75 RID: 12149
		private readonly Speller _speller;

		// Token: 0x04002F76 RID: 12150
		private readonly ITextPointer _start;

		// Token: 0x04002F77 RID: 12151
		private readonly ITextPointer _end;
	}
}

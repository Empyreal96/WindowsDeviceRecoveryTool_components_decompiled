using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace System.Windows.Documents
{
	/// <summary> A helper class that sets text effects in a text container </summary>
	// Token: 0x020003FE RID: 1022
	public static class TextEffectResolver
	{
		/// <summary> Resolves text effect on a text range to a list of text effect targets. </summary>
		/// <param name="startPosition">The starting text pointer</param>
		/// <param name="endPosition">The end text pointer</param>
		/// <param name="effect">The effect to apply on the text</param>
		/// <returns>Collection of <see cref="T:System.Windows.Documents.TextEffectTarget" /> objects corresponding to the text range.</returns>
		// Token: 0x06003921 RID: 14625 RVA: 0x00103210 File Offset: 0x00101410
		public static TextEffectTarget[] Resolve(TextPointer startPosition, TextPointer endPosition, TextEffect effect)
		{
			if (effect == null)
			{
				throw new ArgumentNullException("effect");
			}
			ValidationHelper.VerifyPositionPair(startPosition, endPosition);
			TextPointer textPointer = new TextPointer(startPosition);
			TextEffectResolver.MoveToFirstCharacterSymbol(textPointer);
			List<TextEffectTarget> list = new List<TextEffectTarget>();
			while (textPointer.CompareTo(endPosition) < 0)
			{
				TextEffect textEffect = effect.Clone();
				TextPointer textPointer2 = new TextPointer(textPointer);
				TextEffectResolver.MoveToFirstNonCharacterSymbol(textPointer2, endPosition);
				textPointer2 = (TextPointer)TextPointerBase.Min(textPointer2, endPosition);
				textEffect.PositionStart = textPointer.TextContainer.Start.GetOffsetToPosition(textPointer);
				textEffect.PositionCount = textPointer.GetOffsetToPosition(textPointer2);
				list.Add(new TextEffectTarget(textPointer.Parent, textEffect));
				textPointer = textPointer2;
				TextEffectResolver.MoveToFirstCharacterSymbol(textPointer);
			}
			return list.ToArray();
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x001032B7 File Offset: 0x001014B7
		private static void MoveToFirstCharacterSymbol(TextPointer navigator)
		{
			while (navigator.GetPointerContext(LogicalDirection.Forward) != TextPointerContext.Text && navigator.MoveToNextContextPosition(LogicalDirection.Forward))
			{
			}
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x001032CC File Offset: 0x001014CC
		private static void MoveToFirstNonCharacterSymbol(TextPointer navigator, TextPointer stopHint)
		{
			while (navigator.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text && navigator.CompareTo(stopHint) < 0 && navigator.MoveToNextContextPosition(LogicalDirection.Forward))
			{
			}
		}
	}
}

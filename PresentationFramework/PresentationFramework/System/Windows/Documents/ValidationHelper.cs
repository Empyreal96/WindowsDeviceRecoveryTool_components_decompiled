using System;
using System.ComponentModel;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200042D RID: 1069
	internal static class ValidationHelper
	{
		// Token: 0x06003EED RID: 16109 RVA: 0x0011F3DD File Offset: 0x0011D5DD
		internal static void VerifyPosition(ITextContainer tree, ITextPointer position)
		{
			ValidationHelper.VerifyPosition(tree, position, "position");
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x0011F3EB File Offset: 0x0011D5EB
		internal static void VerifyPosition(ITextContainer container, ITextPointer position, string paramName)
		{
			if (position == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (position.TextContainer != container)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedTree", new object[]
				{
					paramName
				}));
			}
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x0011F41C File Offset: 0x0011D61C
		internal static void VerifyPositionPair(ITextPointer startPosition, ITextPointer endPosition)
		{
			if (startPosition == null)
			{
				throw new ArgumentNullException("startPosition");
			}
			if (endPosition == null)
			{
				throw new ArgumentNullException("endPosition");
			}
			if (startPosition.TextContainer != endPosition.TextContainer)
			{
				throw new ArgumentException(SR.Get("InDifferentTextContainers", new object[]
				{
					"startPosition",
					"endPosition"
				}));
			}
			if (startPosition.CompareTo(endPosition) > 0)
			{
				throw new ArgumentException(SR.Get("BadTextPositionOrder", new object[]
				{
					"startPosition",
					"endPosition"
				}));
			}
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0011F4A9 File Offset: 0x0011D6A9
		internal static void VerifyDirection(LogicalDirection direction, string argumentName)
		{
			if (direction != LogicalDirection.Forward && direction != LogicalDirection.Backward)
			{
				throw new InvalidEnumArgumentException(argumentName, (int)direction, typeof(LogicalDirection));
			}
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0011F4C4 File Offset: 0x0011D6C4
		internal static void VerifyElementEdge(ElementEdge edge, string param)
		{
			if (edge != ElementEdge.BeforeStart && edge != ElementEdge.AfterStart && edge != ElementEdge.BeforeEnd && edge != ElementEdge.AfterEnd)
			{
				throw new InvalidEnumArgumentException(param, (int)edge, typeof(ElementEdge));
			}
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x0011F4E8 File Offset: 0x0011D6E8
		internal static void ValidateChild(TextPointer position, object child, string paramName)
		{
			Invariant.Assert(position != null);
			if (child == null)
			{
				throw new ArgumentNullException(paramName);
			}
			if (!TextSchema.IsValidChild(position, child.GetType()))
			{
				throw new ArgumentException(SR.Get("TextSchema_ChildTypeIsInvalid", new object[]
				{
					position.Parent.GetType().Name,
					child.GetType().Name
				}));
			}
			if (child is TextElement)
			{
				if (((TextElement)child).Parent != null)
				{
					throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
					{
						child.GetType().Name
					}));
				}
			}
			else
			{
				Invariant.Assert(child is UIElement);
			}
		}
	}
}

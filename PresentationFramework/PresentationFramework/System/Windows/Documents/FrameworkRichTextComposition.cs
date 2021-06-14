using System;
using System.Windows.Input;

namespace System.Windows.Documents
{
	/// <summary>  Represents a composition related to text input. You can use this class to find the text position of the composition or the result string.</summary>
	// Token: 0x02000372 RID: 882
	public sealed class FrameworkRichTextComposition : FrameworkTextComposition
	{
		// Token: 0x06002FB3 RID: 12211 RVA: 0x000D6D69 File Offset: 0x000D4F69
		internal FrameworkRichTextComposition(InputManager inputManager, IInputElement source, object owner) : base(inputManager, source, owner)
		{
		}

		/// <summary> Gets the start position of the result text of the text input. </summary>
		/// <returns>The start position of the result text of the text input.</returns>
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x000D6D74 File Offset: 0x000D4F74
		public TextPointer ResultStart
		{
			get
			{
				if (base._ResultStart != null)
				{
					return (TextPointer)base._ResultStart.GetFrozenPointer(LogicalDirection.Backward);
				}
				return null;
			}
		}

		/// <summary> Gets the end position of the result text of the text input. </summary>
		/// <returns>The end position of the result text of the text input.</returns>
		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06002FB5 RID: 12213 RVA: 0x000D6D91 File Offset: 0x000D4F91
		public TextPointer ResultEnd
		{
			get
			{
				if (base._ResultEnd != null)
				{
					return (TextPointer)base._ResultEnd.GetFrozenPointer(LogicalDirection.Forward);
				}
				return null;
			}
		}

		/// <summary> Gets the start position of the current composition text. </summary>
		/// <returns>The start position of the current composition text.</returns>
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06002FB6 RID: 12214 RVA: 0x000D6DAE File Offset: 0x000D4FAE
		public TextPointer CompositionStart
		{
			get
			{
				if (base._CompositionStart != null)
				{
					return (TextPointer)base._CompositionStart.GetFrozenPointer(LogicalDirection.Backward);
				}
				return null;
			}
		}

		/// <summary> Gets the end position of the current composition text. </summary>
		/// <returns>The end position of the current composition text.</returns>
		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000D6DCB File Offset: 0x000D4FCB
		public TextPointer CompositionEnd
		{
			get
			{
				if (base._CompositionEnd != null)
				{
					return (TextPointer)base._CompositionEnd.GetFrozenPointer(LogicalDirection.Forward);
				}
				return null;
			}
		}
	}
}

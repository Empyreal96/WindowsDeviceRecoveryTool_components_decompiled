using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Input;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	/// <summary>Represents a composition during the text input events of a <see cref="T:System.Windows.Controls.TextBox" />. </summary>
	// Token: 0x02000373 RID: 883
	public class FrameworkTextComposition : TextComposition
	{
		// Token: 0x06002FB8 RID: 12216 RVA: 0x000D6DE8 File Offset: 0x000D4FE8
		internal FrameworkTextComposition(InputManager inputManager, IInputElement source, object owner) : base(inputManager, source, string.Empty, TextCompositionAutoComplete.Off)
		{
			this._owner = owner;
		}

		/// <summary>Finalizes the composition. </summary>
		// Token: 0x06002FB9 RID: 12217 RVA: 0x000D6DFF File Offset: 0x000D4FFF
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void Complete()
		{
			this._pendingComplete = true;
		}

		/// <summary>Gets the offset of the finalized text when the <see cref="E:System.Windows.Input.TextCompositionManager.TextInput" /> event occurs.</summary>
		/// <returns>The offset of the finalized text when the <see cref="E:System.Windows.Input.TextCompositionManager.TextInput" /> event occurs.</returns>
		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06002FBA RID: 12218 RVA: 0x000D6E08 File Offset: 0x000D5008
		public int ResultOffset
		{
			get
			{
				if (this._ResultStart != null)
				{
					return this._offset;
				}
				return -1;
			}
		}

		/// <summary>Gets the length of the finalized text in Unicode symbols when the <see cref="E:System.Windows.Input.TextCompositionManager.TextInput" /> event occurs.</summary>
		/// <returns>The length of the finalized text in Unicode symbols when the <see cref="E:System.Windows.Input.TextCompositionManager.TextInput" /> event occurs.</returns>
		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06002FBB RID: 12219 RVA: 0x000D6E1A File Offset: 0x000D501A
		public int ResultLength
		{
			get
			{
				if (this._ResultStart != null)
				{
					return this._length;
				}
				return -1;
			}
		}

		/// <summary>Gets the position at which the composition text occurs in the <see cref="T:System.Windows.Controls.TextBox" />.</summary>
		/// <returns>The position at which the composition text occurs in the <see cref="T:System.Windows.Controls.TextBox" />.</returns>
		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x000D6E2C File Offset: 0x000D502C
		public int CompositionOffset
		{
			get
			{
				if (this._CompositionStart != null)
				{
					return this._offset;
				}
				return -1;
			}
		}

		/// <summary>Gets the length of the current composition in Unicode symbols.</summary>
		/// <returns>The length of the current composition in Unicode symbols.</returns>
		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06002FBD RID: 12221 RVA: 0x000D6E3E File Offset: 0x000D503E
		public int CompositionLength
		{
			get
			{
				if (this._CompositionStart != null)
				{
					return this._length;
				}
				return -1;
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000D6E50 File Offset: 0x000D5050
		[SecurityCritical]
		internal static void CompleteCurrentComposition(UnsafeNativeMethods.ITfDocumentMgr documentMgr)
		{
			UnsafeNativeMethods.ITfContext tfContext;
			documentMgr.GetBase(out tfContext);
			UnsafeNativeMethods.ITfCompositionView composition = FrameworkTextComposition.GetComposition(tfContext);
			if (composition != null)
			{
				UnsafeNativeMethods.ITfContextOwnerCompositionServices tfContextOwnerCompositionServices = tfContext as UnsafeNativeMethods.ITfContextOwnerCompositionServices;
				tfContextOwnerCompositionServices.TerminateComposition(composition);
				Marshal.ReleaseComObject(composition);
			}
			Marshal.ReleaseComObject(tfContext);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000D6E8C File Offset: 0x000D508C
		[SecurityCritical]
		internal static UnsafeNativeMethods.ITfCompositionView GetCurrentCompositionView(UnsafeNativeMethods.ITfDocumentMgr documentMgr)
		{
			UnsafeNativeMethods.ITfContext tfContext;
			documentMgr.GetBase(out tfContext);
			UnsafeNativeMethods.ITfCompositionView composition = FrameworkTextComposition.GetComposition(tfContext);
			Marshal.ReleaseComObject(tfContext);
			return composition;
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000D6EB0 File Offset: 0x000D50B0
		internal void SetResultPositions(ITextPointer start, ITextPointer end, string text)
		{
			Invariant.Assert(start != null);
			Invariant.Assert(end != null);
			Invariant.Assert(text != null);
			this._compositionStart = null;
			this._compositionEnd = null;
			this._resultStart = start.GetFrozenPointer(LogicalDirection.Backward);
			this._resultEnd = end.GetFrozenPointer(LogicalDirection.Forward);
			base.Text = text;
			base.CompositionText = string.Empty;
			this._offset = ((this._resultStart == null) ? -1 : this._resultStart.Offset);
			this._length = ((this._resultStart == null) ? -1 : this._resultStart.GetOffsetToPosition(this._resultEnd));
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000D6F50 File Offset: 0x000D5150
		internal void SetCompositionPositions(ITextPointer start, ITextPointer end, string text)
		{
			Invariant.Assert(start != null);
			Invariant.Assert(end != null);
			Invariant.Assert(text != null);
			this._compositionStart = start.GetFrozenPointer(LogicalDirection.Backward);
			this._compositionEnd = end.GetFrozenPointer(LogicalDirection.Forward);
			this._resultStart = null;
			this._resultEnd = null;
			base.Text = string.Empty;
			base.CompositionText = text;
			this._offset = ((this._compositionStart == null) ? -1 : this._compositionStart.Offset);
			this._length = ((this._compositionStart == null) ? -1 : this._compositionStart.GetOffsetToPosition(this._compositionEnd));
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06002FC2 RID: 12226 RVA: 0x000D6FF0 File Offset: 0x000D51F0
		internal ITextPointer _ResultStart
		{
			get
			{
				return this._resultStart;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06002FC3 RID: 12227 RVA: 0x000D6FF8 File Offset: 0x000D51F8
		internal ITextPointer _ResultEnd
		{
			get
			{
				return this._resultEnd;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06002FC4 RID: 12228 RVA: 0x000D7000 File Offset: 0x000D5200
		internal ITextPointer _CompositionStart
		{
			get
			{
				return this._compositionStart;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06002FC5 RID: 12229 RVA: 0x000D7008 File Offset: 0x000D5208
		internal ITextPointer _CompositionEnd
		{
			get
			{
				return this._compositionEnd;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06002FC6 RID: 12230 RVA: 0x000D7010 File Offset: 0x000D5210
		internal bool PendingComplete
		{
			get
			{
				return this._pendingComplete;
			}
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06002FC7 RID: 12231 RVA: 0x000D7018 File Offset: 0x000D5218
		internal object Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000D7020 File Offset: 0x000D5220
		[SecurityCritical]
		private static UnsafeNativeMethods.ITfCompositionView GetComposition(UnsafeNativeMethods.ITfContext context)
		{
			UnsafeNativeMethods.ITfCompositionView[] array = new UnsafeNativeMethods.ITfCompositionView[1];
			UnsafeNativeMethods.ITfContextComposition tfContextComposition = (UnsafeNativeMethods.ITfContextComposition)context;
			UnsafeNativeMethods.IEnumITfCompositionView enumITfCompositionView;
			tfContextComposition.EnumCompositions(out enumITfCompositionView);
			int num;
			enumITfCompositionView.Next(1, array, out num);
			Marshal.ReleaseComObject(enumITfCompositionView);
			return array[0];
		}

		// Token: 0x04001E4A RID: 7754
		private ITextPointer _resultStart;

		// Token: 0x04001E4B RID: 7755
		private ITextPointer _resultEnd;

		// Token: 0x04001E4C RID: 7756
		private ITextPointer _compositionStart;

		// Token: 0x04001E4D RID: 7757
		private ITextPointer _compositionEnd;

		// Token: 0x04001E4E RID: 7758
		private int _offset;

		// Token: 0x04001E4F RID: 7759
		private int _length;

		// Token: 0x04001E50 RID: 7760
		private readonly object _owner;

		// Token: 0x04001E51 RID: 7761
		private bool _pendingComplete;
	}
}

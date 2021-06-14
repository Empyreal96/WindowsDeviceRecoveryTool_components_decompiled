using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	/// <summary>A block-level flow content element used for grouping other <see cref="T:System.Windows.Documents.Block" /> elements.</summary>
	// Token: 0x020003D6 RID: 982
	[ContentProperty("Blocks")]
	public class Section : Block
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.Section" /> class. </summary>
		// Token: 0x06003529 RID: 13609 RVA: 0x000C454D File Offset: 0x000C274D
		public Section()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Section" /> class, taking a specified <see cref="T:System.Windows.Documents.Block" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.Section" />.</summary>
		/// <param name="block">A <see cref="T:System.Windows.Documents.Block" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.Section" />.</param>
		// Token: 0x0600352A RID: 13610 RVA: 0x000F0F2B File Offset: 0x000EF12B
		public Section(Block block)
		{
			if (block == null)
			{
				throw new ArgumentNullException("block");
			}
			this.Blocks.Add(block);
		}

		/// <summary>Gets or sets a value that indicates whether or not a trailing paragraph break should be inserted after the last paragraph when placing the contents of a root <see cref="T:System.Windows.Documents.Section" /> element on the clipboard.</summary>
		/// <returns>
		///     true to indicate that a trailing paragraph break should be included; otherwise, false.</returns>
		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x0600352B RID: 13611 RVA: 0x000F0F4D File Offset: 0x000EF14D
		// (set) Token: 0x0600352C RID: 13612 RVA: 0x000F0F58 File Offset: 0x000EF158
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(true)]
		public bool HasTrailingParagraphBreakOnPaste
		{
			get
			{
				return !this._ignoreTrailingParagraphBreakOnPaste;
			}
			set
			{
				this._ignoreTrailingParagraphBreakOnPaste = !value;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.BlockCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Section" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.BlockCollection" /> containing the <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.Section" />This property has no default value.</returns>
		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x0600352D RID: 13613 RVA: 0x000C3B80 File Offset: 0x000C1D80
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="manager">A serialization service manager object for this object.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Documents.Section.Blocks" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.NullReferenceException">Raised when <paramref name="manager" /> is <see langword="null" />.</exception>
		// Token: 0x0600352E RID: 13614 RVA: 0x000C3C87 File Offset: 0x000C1E87
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBlocks(XamlDesignerSerializationManager manager)
		{
			return manager != null && manager.XmlWriter == null;
		}

		// Token: 0x040024FF RID: 9471
		internal const string HasTrailingParagraphBreakOnPastePropertyName = "HasTrailingParagraphBreakOnPaste";

		// Token: 0x04002500 RID: 9472
		private bool _ignoreTrailingParagraphBreakOnPaste;
	}
}

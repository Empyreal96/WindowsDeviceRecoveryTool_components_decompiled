using System;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>An abstract class that provides a base for all inline flow content elements.</summary>
	// Token: 0x0200037C RID: 892
	[TextElementEditingBehavior(IsMergeable = true, IsTypographicOnly = true)]
	public abstract class Inline : TextElement
	{
		/// <summary>Gets an <see cref="T:System.Windows.Documents.InlineCollection" /> that contains the <see cref="T:System.Windows.Documents.Inline" /> elements that are siblings (peers) to this element.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.InlineCollection" /> object that contains the <see cref="T:System.Windows.Documents.Inline" /> elements that are siblings to this element.This property has no default value.</returns>
		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600308C RID: 12428 RVA: 0x000DB200 File Offset: 0x000D9400
		public InlineCollection SiblingInlines
		{
			get
			{
				if (base.Parent == null)
				{
					return null;
				}
				return new InlineCollection(this, false);
			}
		}

		/// <summary>Gets the next <see cref="T:System.Windows.Documents.Inline" /> element that is a peer to this element.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.Inline" /> object representing the next <see cref="T:System.Windows.Documents.Inline" /> element that is a peer to this element, or null if there is no next <see cref="T:System.Windows.Documents.Inline" /> element.This property has no default value.</returns>
		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000DB213 File Offset: 0x000D9413
		public Inline NextInline
		{
			get
			{
				return base.NextElement as Inline;
			}
		}

		/// <summary>Gets the previous <see cref="T:System.Windows.Documents.Inline" /> element that is a peer to this element.</summary>
		/// <returns>An <see cref="T:System.Windows.Documents.Inline" /> object representing the previous <see cref="T:System.Windows.Documents.Inline" /> element that is a peer to this element, or null if there is no previous <see cref="T:System.Windows.Documents.Inline" /> element.This property has no default value.</returns>
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000DB220 File Offset: 0x000D9420
		public Inline PreviousInline
		{
			get
			{
				return base.PreviousElement as Inline;
			}
		}

		/// <summary>Gets or sets the baseline alignment for the <see cref="T:System.Windows.Documents.Inline" /> element.   </summary>
		/// <returns>A member or the <see cref="T:System.Windows.BaselineAlignment" /> enumeration specifying the baseline alignment for the <see cref="T:System.Windows.Documents.Inline" /> element.The default value is <see cref="T:System.Windows.BaselineAlignment" />.Baseline.</returns>
		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000DB22D File Offset: 0x000D942D
		// (set) Token: 0x06003090 RID: 12432 RVA: 0x000DB23F File Offset: 0x000D943F
		public BaselineAlignment BaselineAlignment
		{
			get
			{
				return (BaselineAlignment)base.GetValue(Inline.BaselineAlignmentProperty);
			}
			set
			{
				base.SetValue(Inline.BaselineAlignmentProperty, value);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.TextDecorationCollection" /> that contains text decorations to apply to this element.  </summary>
		/// <returns>A <see cref="T:System.Windows.TextDecorationCollection" /> collection that contains text decorations to apply to this element.The default value is null (no text decorations applied).</returns>
		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000DB252 File Offset: 0x000D9452
		// (set) Token: 0x06003092 RID: 12434 RVA: 0x000DB264 File Offset: 0x000D9464
		public TextDecorationCollection TextDecorations
		{
			get
			{
				return (TextDecorationCollection)base.GetValue(Inline.TextDecorationsProperty);
			}
			set
			{
				base.SetValue(Inline.TextDecorationsProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the relative direction for flow of content within a <see cref="T:System.Windows.Documents.Inline" /> element.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FlowDirection" /> enumeration specifying the relative flow direction.  Getting this property returns the currently effective flow direction.  Setting this property causes the contents of the <see cref="T:System.Windows.Documents.Inline" /> element to re-flow in the indicated direction.The default value is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003093 RID: 12435 RVA: 0x000DB272 File Offset: 0x000D9472
		// (set) Token: 0x06003094 RID: 12436 RVA: 0x000DB284 File Offset: 0x000D9484
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(Inline.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(Inline.FlowDirectionProperty, value);
			}
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000DB297 File Offset: 0x000D9497
		internal static Run CreateImplicitRun(DependencyObject parent)
		{
			return new Run();
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000DB29E File Offset: 0x000D949E
		internal static InlineUIContainer CreateImplicitInlineUIContainer(DependencyObject parent)
		{
			return new InlineUIContainer();
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000DB2A8 File Offset: 0x000D94A8
		private static bool IsValidBaselineAlignment(object o)
		{
			BaselineAlignment baselineAlignment = (BaselineAlignment)o;
			return baselineAlignment == BaselineAlignment.Baseline || baselineAlignment == BaselineAlignment.Bottom || baselineAlignment == BaselineAlignment.Center || baselineAlignment == BaselineAlignment.Subscript || baselineAlignment == BaselineAlignment.Superscript || baselineAlignment == BaselineAlignment.TextBottom || baselineAlignment == BaselineAlignment.TextTop || baselineAlignment == BaselineAlignment.Top;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Inline.BaselineAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Inline.BaselineAlignment" /> dependency property.</returns>
		// Token: 0x04001E88 RID: 7816
		public static readonly DependencyProperty BaselineAlignmentProperty = DependencyProperty.Register("BaselineAlignment", typeof(BaselineAlignment), typeof(Inline), new FrameworkPropertyMetadata(BaselineAlignment.Baseline, FrameworkPropertyMetadataOptions.AffectsParentMeasure), new ValidateValueCallback(Inline.IsValidBaselineAlignment));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Inline.TextDecorations" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Inline.TextDecorations" /> dependency property.</returns>
		// Token: 0x04001E89 RID: 7817
		public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(Inline), new FrameworkPropertyMetadata(new FreezableDefaultValueFactory(TextDecorationCollection.Empty), FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Inline.FlowDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Inline.FlowDirection" /> dependency property.</returns>
		// Token: 0x04001E8A RID: 7818
		public static readonly DependencyProperty FlowDirectionProperty = FrameworkElement.FlowDirectionProperty.AddOwner(typeof(Inline));
	}
}

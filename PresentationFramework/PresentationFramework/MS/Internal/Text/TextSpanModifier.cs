using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000608 RID: 1544
	internal class TextSpanModifier : TextModifier
	{
		// Token: 0x060066AA RID: 26282 RVA: 0x001CD0B4 File Offset: 0x001CB2B4
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush)
		{
			this._length = length;
			this._modifierDecorations = textDecorations;
			this._modifierBrush = foregroundBrush;
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x001CD0D1 File Offset: 0x001CB2D1
		public TextSpanModifier(int length, TextDecorationCollection textDecorations, Brush foregroundBrush, FlowDirection flowDirection) : this(length, textDecorations, foregroundBrush)
		{
			this._hasDirectionalEmbedding = true;
			this._flowDirection = flowDirection;
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x060066AC RID: 26284 RVA: 0x001CD0EB File Offset: 0x001CB2EB
		public sealed override int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x060066AD RID: 26285 RVA: 0x0000C238 File Offset: 0x0000A438
		public sealed override TextRunProperties Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x001CD0F4 File Offset: 0x001CB2F4
		public sealed override TextRunProperties ModifyProperties(TextRunProperties properties)
		{
			if (properties == null || this._modifierDecorations == null || this._modifierDecorations.Count == 0)
			{
				return properties;
			}
			Brush brush = this._modifierBrush;
			if (brush == properties.ForegroundBrush)
			{
				brush = null;
			}
			TextDecorationCollection textDecorations = properties.TextDecorations;
			TextDecorationCollection textDecorationCollection;
			if (textDecorations == null || textDecorations.Count == 0)
			{
				if (brush == null)
				{
					textDecorationCollection = this._modifierDecorations;
				}
				else
				{
					textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				}
			}
			else
			{
				textDecorationCollection = this.CopyTextDecorations(this._modifierDecorations, brush);
				foreach (TextDecoration value in textDecorations)
				{
					textDecorationCollection.Add(value);
				}
			}
			return new TextSpanModifier.MergedTextRunProperties(properties, textDecorationCollection);
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x060066AF RID: 26287 RVA: 0x001CD1B4 File Offset: 0x001CB3B4
		public override bool HasDirectionalEmbedding
		{
			get
			{
				return this._hasDirectionalEmbedding;
			}
		}

		// Token: 0x170018B3 RID: 6323
		// (get) Token: 0x060066B0 RID: 26288 RVA: 0x001CD1BC File Offset: 0x001CB3BC
		public override FlowDirection FlowDirection
		{
			get
			{
				return this._flowDirection;
			}
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x001CD1C4 File Offset: 0x001CB3C4
		private TextDecorationCollection CopyTextDecorations(TextDecorationCollection textDecorations, Brush brush)
		{
			TextDecorationCollection textDecorationCollection = new TextDecorationCollection();
			Pen pen = null;
			foreach (TextDecoration textDecoration in textDecorations)
			{
				if (textDecoration.Pen == null && brush != null)
				{
					if (pen == null)
					{
						pen = new Pen(brush, 1.0);
					}
					TextDecoration textDecoration2 = textDecoration.Clone();
					textDecoration2.Pen = pen;
					textDecorationCollection.Add(textDecoration2);
				}
				else
				{
					textDecorationCollection.Add(textDecoration);
				}
			}
			return textDecorationCollection;
		}

		// Token: 0x04003334 RID: 13108
		private int _length;

		// Token: 0x04003335 RID: 13109
		private TextDecorationCollection _modifierDecorations;

		// Token: 0x04003336 RID: 13110
		private Brush _modifierBrush;

		// Token: 0x04003337 RID: 13111
		private FlowDirection _flowDirection;

		// Token: 0x04003338 RID: 13112
		private bool _hasDirectionalEmbedding;

		// Token: 0x02000A1A RID: 2586
		private class MergedTextRunProperties : TextRunProperties
		{
			// Token: 0x06008AA9 RID: 35497 RVA: 0x002578E5 File Offset: 0x00255AE5
			internal MergedTextRunProperties(TextRunProperties runProperties, TextDecorationCollection textDecorations)
			{
				this._runProperties = runProperties;
				this._textDecorations = textDecorations;
				base.PixelsPerDip = this._runProperties.PixelsPerDip;
			}

			// Token: 0x17001F4E RID: 8014
			// (get) Token: 0x06008AAA RID: 35498 RVA: 0x0025790C File Offset: 0x00255B0C
			public override Typeface Typeface
			{
				get
				{
					return this._runProperties.Typeface;
				}
			}

			// Token: 0x17001F4F RID: 8015
			// (get) Token: 0x06008AAB RID: 35499 RVA: 0x00257919 File Offset: 0x00255B19
			public override double FontRenderingEmSize
			{
				get
				{
					return this._runProperties.FontRenderingEmSize;
				}
			}

			// Token: 0x17001F50 RID: 8016
			// (get) Token: 0x06008AAC RID: 35500 RVA: 0x00257926 File Offset: 0x00255B26
			public override double FontHintingEmSize
			{
				get
				{
					return this._runProperties.FontHintingEmSize;
				}
			}

			// Token: 0x17001F51 RID: 8017
			// (get) Token: 0x06008AAD RID: 35501 RVA: 0x00257933 File Offset: 0x00255B33
			public override TextDecorationCollection TextDecorations
			{
				get
				{
					return this._textDecorations;
				}
			}

			// Token: 0x17001F52 RID: 8018
			// (get) Token: 0x06008AAE RID: 35502 RVA: 0x0025793B File Offset: 0x00255B3B
			public override Brush ForegroundBrush
			{
				get
				{
					return this._runProperties.ForegroundBrush;
				}
			}

			// Token: 0x17001F53 RID: 8019
			// (get) Token: 0x06008AAF RID: 35503 RVA: 0x00257948 File Offset: 0x00255B48
			public override Brush BackgroundBrush
			{
				get
				{
					return this._runProperties.BackgroundBrush;
				}
			}

			// Token: 0x17001F54 RID: 8020
			// (get) Token: 0x06008AB0 RID: 35504 RVA: 0x00257955 File Offset: 0x00255B55
			public override CultureInfo CultureInfo
			{
				get
				{
					return this._runProperties.CultureInfo;
				}
			}

			// Token: 0x17001F55 RID: 8021
			// (get) Token: 0x06008AB1 RID: 35505 RVA: 0x00257962 File Offset: 0x00255B62
			public override TextEffectCollection TextEffects
			{
				get
				{
					return this._runProperties.TextEffects;
				}
			}

			// Token: 0x17001F56 RID: 8022
			// (get) Token: 0x06008AB2 RID: 35506 RVA: 0x0025796F File Offset: 0x00255B6F
			public override BaselineAlignment BaselineAlignment
			{
				get
				{
					return this._runProperties.BaselineAlignment;
				}
			}

			// Token: 0x17001F57 RID: 8023
			// (get) Token: 0x06008AB3 RID: 35507 RVA: 0x0025797C File Offset: 0x00255B7C
			public override TextRunTypographyProperties TypographyProperties
			{
				get
				{
					return this._runProperties.TypographyProperties;
				}
			}

			// Token: 0x17001F58 RID: 8024
			// (get) Token: 0x06008AB4 RID: 35508 RVA: 0x00257989 File Offset: 0x00255B89
			public override NumberSubstitution NumberSubstitution
			{
				get
				{
					return this._runProperties.NumberSubstitution;
				}
			}

			// Token: 0x040046D0 RID: 18128
			private TextRunProperties _runProperties;

			// Token: 0x040046D1 RID: 18129
			private TextDecorationCollection _textDecorations;
		}
	}
}

using System;

namespace System.Windows.Forms.VisualStyles
{
	/// <summary>Provides basic information about the font specified by a visual style for a particular element.</summary>
	// Token: 0x02000472 RID: 1138
	public struct TextMetrics
	{
		/// <summary>Gets or sets the height of characters in the font.</summary>
		/// <returns>The height of characters in the font.</returns>
		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06004CFB RID: 19707 RVA: 0x0013C5E3 File Offset: 0x0013A7E3
		// (set) Token: 0x06004CFC RID: 19708 RVA: 0x0013C5EB File Offset: 0x0013A7EB
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>Gets or sets the ascent of characters in the font.</summary>
		/// <returns>The ascent of characters in the font.</returns>
		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06004CFD RID: 19709 RVA: 0x0013C5F4 File Offset: 0x0013A7F4
		// (set) Token: 0x06004CFE RID: 19710 RVA: 0x0013C5FC File Offset: 0x0013A7FC
		public int Ascent
		{
			get
			{
				return this.ascent;
			}
			set
			{
				this.ascent = value;
			}
		}

		/// <summary>Gets or sets the descent of characters in the font.</summary>
		/// <returns>The descent of characters in the font.</returns>
		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06004CFF RID: 19711 RVA: 0x0013C605 File Offset: 0x0013A805
		// (set) Token: 0x06004D00 RID: 19712 RVA: 0x0013C60D File Offset: 0x0013A80D
		public int Descent
		{
			get
			{
				return this.descent;
			}
			set
			{
				this.descent = value;
			}
		}

		/// <summary>Gets or sets the amount of leading inside the bounds set by the <see cref="P:System.Windows.Forms.VisualStyles.TextMetrics.Height" /> property. </summary>
		/// <returns>The amount of leading inside the bounds set by the <see cref="P:System.Windows.Forms.VisualStyles.TextMetrics.Height" /> property.</returns>
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0013C616 File Offset: 0x0013A816
		// (set) Token: 0x06004D02 RID: 19714 RVA: 0x0013C61E File Offset: 0x0013A81E
		public int InternalLeading
		{
			get
			{
				return this.internalLeading;
			}
			set
			{
				this.internalLeading = value;
			}
		}

		/// <summary>Gets or sets the amount of extra leading that the application adds between rows.</summary>
		/// <returns>The amount of extra leading (space) required between rows. </returns>
		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06004D03 RID: 19715 RVA: 0x0013C627 File Offset: 0x0013A827
		// (set) Token: 0x06004D04 RID: 19716 RVA: 0x0013C62F File Offset: 0x0013A82F
		public int ExternalLeading
		{
			get
			{
				return this.externalLeading;
			}
			set
			{
				this.externalLeading = value;
			}
		}

		/// <summary>Gets or sets the average width of characters in the font.</summary>
		/// <returns>The average width of characters in the font.</returns>
		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06004D05 RID: 19717 RVA: 0x0013C638 File Offset: 0x0013A838
		// (set) Token: 0x06004D06 RID: 19718 RVA: 0x0013C640 File Offset: 0x0013A840
		public int AverageCharWidth
		{
			get
			{
				return this.aveCharWidth;
			}
			set
			{
				this.aveCharWidth = value;
			}
		}

		/// <summary>Gets or sets the width of the widest character in the font.</summary>
		/// <returns>The width of the widest character in the font.</returns>
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06004D07 RID: 19719 RVA: 0x0013C649 File Offset: 0x0013A849
		// (set) Token: 0x06004D08 RID: 19720 RVA: 0x0013C651 File Offset: 0x0013A851
		public int MaxCharWidth
		{
			get
			{
				return this.maxCharWidth;
			}
			set
			{
				this.maxCharWidth = value;
			}
		}

		/// <summary>Gets or sets the weight of the font.</summary>
		/// <returns>The weight of the font.</returns>
		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06004D09 RID: 19721 RVA: 0x0013C65A File Offset: 0x0013A85A
		// (set) Token: 0x06004D0A RID: 19722 RVA: 0x0013C662 File Offset: 0x0013A862
		public int Weight
		{
			get
			{
				return this.weight;
			}
			set
			{
				this.weight = value;
			}
		}

		/// <summary>Gets or sets the extra width per string that may be added to some synthesized fonts.</summary>
		/// <returns>The extra width per string that may be added to some synthesized fonts.</returns>
		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06004D0B RID: 19723 RVA: 0x0013C66B File Offset: 0x0013A86B
		// (set) Token: 0x06004D0C RID: 19724 RVA: 0x0013C673 File Offset: 0x0013A873
		public int Overhang
		{
			get
			{
				return this.overhang;
			}
			set
			{
				this.overhang = value;
			}
		}

		/// <summary>Gets or sets the horizontal aspect of the device for which the font was designed.</summary>
		/// <returns>The horizontal aspect of the device for which the font was designed.</returns>
		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06004D0D RID: 19725 RVA: 0x0013C67C File Offset: 0x0013A87C
		// (set) Token: 0x06004D0E RID: 19726 RVA: 0x0013C684 File Offset: 0x0013A884
		public int DigitizedAspectX
		{
			get
			{
				return this.digitizedAspectX;
			}
			set
			{
				this.digitizedAspectX = value;
			}
		}

		/// <summary>Gets or sets the vertical aspect of the device for which the font was designed.</summary>
		/// <returns>The vertical aspect of the device for which the font was designed.</returns>
		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06004D0F RID: 19727 RVA: 0x0013C68D File Offset: 0x0013A88D
		// (set) Token: 0x06004D10 RID: 19728 RVA: 0x0013C695 File Offset: 0x0013A895
		public int DigitizedAspectY
		{
			get
			{
				return this.digitizedAspectY;
			}
			set
			{
				this.digitizedAspectY = value;
			}
		}

		/// <summary>Gets or sets the first character defined in the font.</summary>
		/// <returns>The first character defined in the font.</returns>
		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06004D11 RID: 19729 RVA: 0x0013C69E File Offset: 0x0013A89E
		// (set) Token: 0x06004D12 RID: 19730 RVA: 0x0013C6A6 File Offset: 0x0013A8A6
		public char FirstChar
		{
			get
			{
				return this.firstChar;
			}
			set
			{
				this.firstChar = value;
			}
		}

		/// <summary>Gets or sets the last character defined in the font.</summary>
		/// <returns>The last character defined in the font.</returns>
		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06004D13 RID: 19731 RVA: 0x0013C6AF File Offset: 0x0013A8AF
		// (set) Token: 0x06004D14 RID: 19732 RVA: 0x0013C6B7 File Offset: 0x0013A8B7
		public char LastChar
		{
			get
			{
				return this.lastChar;
			}
			set
			{
				this.lastChar = value;
			}
		}

		/// <summary>Gets or sets the character to be substituted for characters not in the font.</summary>
		/// <returns>The character to be substituted for characters not in the font.</returns>
		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06004D15 RID: 19733 RVA: 0x0013C6C0 File Offset: 0x0013A8C0
		// (set) Token: 0x06004D16 RID: 19734 RVA: 0x0013C6C8 File Offset: 0x0013A8C8
		public char DefaultChar
		{
			get
			{
				return this.defaultChar;
			}
			set
			{
				this.defaultChar = value;
			}
		}

		/// <summary>Gets or sets the character used to define word breaks for text justification.</summary>
		/// <returns>The character used to define word breaks for text justification.</returns>
		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06004D17 RID: 19735 RVA: 0x0013C6D1 File Offset: 0x0013A8D1
		// (set) Token: 0x06004D18 RID: 19736 RVA: 0x0013C6D9 File Offset: 0x0013A8D9
		public char BreakChar
		{
			get
			{
				return this.breakChar;
			}
			set
			{
				this.breakChar = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the font is italic.</summary>
		/// <returns>
		///     <see langword="true" /> if the font is italic; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06004D19 RID: 19737 RVA: 0x0013C6E2 File Offset: 0x0013A8E2
		// (set) Token: 0x06004D1A RID: 19738 RVA: 0x0013C6EA File Offset: 0x0013A8EA
		public bool Italic
		{
			get
			{
				return this.italic;
			}
			set
			{
				this.italic = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the font is underlined.</summary>
		/// <returns>
		///     <see langword="true" /> if the font is underlined; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06004D1B RID: 19739 RVA: 0x0013C6F3 File Offset: 0x0013A8F3
		// (set) Token: 0x06004D1C RID: 19740 RVA: 0x0013C6FB File Offset: 0x0013A8FB
		public bool Underlined
		{
			get
			{
				return this.underlined;
			}
			set
			{
				this.underlined = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the font specifies a horizontal line through the characters.</summary>
		/// <returns>
		///     <see langword="true" /> if the font has a horizontal line through the characters; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06004D1D RID: 19741 RVA: 0x0013C704 File Offset: 0x0013A904
		// (set) Token: 0x06004D1E RID: 19742 RVA: 0x0013C70C File Offset: 0x0013A90C
		public bool StruckOut
		{
			get
			{
				return this.struckOut;
			}
			set
			{
				this.struckOut = value;
			}
		}

		/// <summary>Gets or sets information about the pitch, technology, and family of a physical font.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.VisualStyles.TextMetricsPitchAndFamilyValues" /> values that specifies the pitch, technology, and family of a physical font.</returns>
		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06004D1F RID: 19743 RVA: 0x0013C715 File Offset: 0x0013A915
		// (set) Token: 0x06004D20 RID: 19744 RVA: 0x0013C71D File Offset: 0x0013A91D
		public TextMetricsPitchAndFamilyValues PitchAndFamily
		{
			get
			{
				return this.pitchAndFamily;
			}
			set
			{
				this.pitchAndFamily = value;
			}
		}

		/// <summary>Gets or sets the character set of the font.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.VisualStyles.TextMetricsCharacterSet" /> values that specifies the character set of the font.</returns>
		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06004D21 RID: 19745 RVA: 0x0013C726 File Offset: 0x0013A926
		// (set) Token: 0x06004D22 RID: 19746 RVA: 0x0013C72E File Offset: 0x0013A92E
		public TextMetricsCharacterSet CharSet
		{
			get
			{
				return this.charSet;
			}
			set
			{
				this.charSet = value;
			}
		}

		// Token: 0x04003293 RID: 12947
		private int height;

		// Token: 0x04003294 RID: 12948
		private int ascent;

		// Token: 0x04003295 RID: 12949
		private int descent;

		// Token: 0x04003296 RID: 12950
		private int internalLeading;

		// Token: 0x04003297 RID: 12951
		private int externalLeading;

		// Token: 0x04003298 RID: 12952
		private int aveCharWidth;

		// Token: 0x04003299 RID: 12953
		private int maxCharWidth;

		// Token: 0x0400329A RID: 12954
		private int weight;

		// Token: 0x0400329B RID: 12955
		private int overhang;

		// Token: 0x0400329C RID: 12956
		private int digitizedAspectX;

		// Token: 0x0400329D RID: 12957
		private int digitizedAspectY;

		// Token: 0x0400329E RID: 12958
		private char firstChar;

		// Token: 0x0400329F RID: 12959
		private char lastChar;

		// Token: 0x040032A0 RID: 12960
		private char defaultChar;

		// Token: 0x040032A1 RID: 12961
		private char breakChar;

		// Token: 0x040032A2 RID: 12962
		private bool italic;

		// Token: 0x040032A3 RID: 12963
		private bool underlined;

		// Token: 0x040032A4 RID: 12964
		private bool struckOut;

		// Token: 0x040032A5 RID: 12965
		private TextMetricsPitchAndFamilyValues pitchAndFamily;

		// Token: 0x040032A6 RID: 12966
		private TextMetricsCharacterSet charSet;
	}
}

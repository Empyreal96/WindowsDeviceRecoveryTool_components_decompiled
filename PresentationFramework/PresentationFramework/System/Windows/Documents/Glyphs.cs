using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.Utility;

namespace System.Windows.Documents
{
	/// <summary>Represents the set of glyphs that are used for rendering fixed text.</summary>
	// Token: 0x02000374 RID: 884
	public sealed class Glyphs : FrameworkElement, IUriContext
	{
		/// <summary>Creates a <see cref="T:System.Windows.Media.GlyphRun" /> from the properties of the <see cref="T:System.Windows.Documents.Glyphs" /> object.</summary>
		/// <returns>A <see cref="T:System.Windows.Media.GlyphRun" /> that was created using the properties of the <see cref="T:System.Windows.Documents.Glyphs" /> object.</returns>
		// Token: 0x06002FCA RID: 12234 RVA: 0x000D7060 File Offset: 0x000D5260
		public GlyphRun ToGlyphRun()
		{
			this.ComputeMeasurementGlyphRunAndOrigin();
			if (this._measurementGlyphRun == null)
			{
				return null;
			}
			return this._measurementGlyphRun;
		}

		/// <summary>For a description of this member, see <see cref="P:System.Windows.Markup.IUriContext.BaseUri" />.</summary>
		/// <returns>The base URI of the current context.</returns>
		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x000C216F File Offset: 0x000C036F
		// (set) Token: 0x06002FCC RID: 12236 RVA: 0x000C2181 File Offset: 0x000C0381
		Uri IUriContext.BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000D7078 File Offset: 0x000D5278
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			Rect rect;
			if (this._measurementGlyphRun != null)
			{
				rect = this._measurementGlyphRun.ComputeInkBoundingBox();
			}
			else
			{
				rect = Rect.Empty;
			}
			if (!rect.IsEmpty)
			{
				rect.X += this._glyphRunOrigin.X;
				rect.Y += this._glyphRunOrigin.Y;
			}
			return finalSize;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000D70E8 File Offset: 0x000D52E8
		protected override void OnRender(DrawingContext context)
		{
			if (this._glyphRunProperties == null || this._measurementGlyphRun == null)
			{
				return;
			}
			context.PushGuidelineY1(this._glyphRunOrigin.Y);
			try
			{
				context.DrawGlyphRun(this.Fill, this._measurementGlyphRun);
			}
			finally
			{
				context.Pop();
			}
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000D7144 File Offset: 0x000D5344
		protected override Size MeasureOverride(Size constraint)
		{
			this.ComputeMeasurementGlyphRunAndOrigin();
			if (this._measurementGlyphRun == null)
			{
				return default(Size);
			}
			Rect rect = this._measurementGlyphRun.ComputeAlignmentBox();
			rect.Offset(this._glyphRunOrigin.X, this._glyphRunOrigin.Y);
			return new Size(Math.Max(0.0, rect.Right), Math.Max(0.0, rect.Bottom));
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000D71C4 File Offset: 0x000D53C4
		private void ComputeMeasurementGlyphRunAndOrigin()
		{
			if (this._glyphRunProperties == null)
			{
				this._measurementGlyphRun = null;
				this.ParseGlyphRunProperties();
				if (this._glyphRunProperties == null)
				{
					return;
				}
			}
			else if (this._measurementGlyphRun != null)
			{
				return;
			}
			bool flag = (this.BidiLevel & 1) == 0;
			bool flag2 = !DoubleUtil.IsNaN(this.OriginX);
			bool flag3 = !DoubleUtil.IsNaN(this.OriginY);
			bool flag4 = false;
			Rect rect = default(Rect);
			if (flag2 && flag3 && flag)
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(new Point(this.OriginX, this.OriginY), base.Language);
				flag4 = true;
			}
			else
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(default(Point), base.Language);
				rect = this._measurementGlyphRun.ComputeAlignmentBox();
			}
			if (flag2)
			{
				this._glyphRunOrigin.X = this.OriginX;
			}
			else
			{
				this._glyphRunOrigin.X = (flag ? 0.0 : rect.Width);
			}
			if (flag3)
			{
				this._glyphRunOrigin.Y = this.OriginY;
			}
			else
			{
				this._glyphRunOrigin.Y = -rect.Y;
			}
			if (!flag4)
			{
				this._measurementGlyphRun = this._glyphRunProperties.CreateGlyphRun(this._glyphRunOrigin, base.Language);
			}
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000D730C File Offset: 0x000D550C
		private void ParseCaretStops(Glyphs.LayoutDependentGlyphRunProperties glyphRunProperties)
		{
			string caretStops = this.CaretStops;
			if (string.IsNullOrEmpty(caretStops))
			{
				glyphRunProperties.caretStops = null;
				return;
			}
			int num;
			if (!string.IsNullOrEmpty(glyphRunProperties.unicodeString))
			{
				num = glyphRunProperties.unicodeString.Length + 1;
			}
			else if (glyphRunProperties.clusterMap != null && glyphRunProperties.clusterMap.Length != 0)
			{
				num = glyphRunProperties.clusterMap.Length + 1;
			}
			else
			{
				num = glyphRunProperties.glyphIndices.Length + 1;
			}
			bool[] array = new bool[num];
			int i = 0;
			foreach (char c in caretStops)
			{
				if (!char.IsWhiteSpace(c))
				{
					int num2;
					if ('0' <= c && c <= '9')
					{
						num2 = (int)(c - '0');
					}
					else if ('a' <= c && c <= 'f')
					{
						num2 = (int)(c - 'a' + '\n');
					}
					else
					{
						if ('A' > c || c > 'F')
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsContainsHexDigits"), "CaretStops");
						}
						num2 = (int)(c - 'A' + '\n');
					}
					if ((num2 & 8) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 4) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 2) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
					if ((num2 & 1) != 0)
					{
						if (i >= array.Length)
						{
							throw new ArgumentException(SR.Get("GlyphsCaretStopsLengthCorrespondsToUnicodeString"), "CaretStops");
						}
						array[i] = true;
					}
					i++;
				}
			}
			while (i < array.Length)
			{
				array[i++] = true;
			}
			glyphRunProperties.caretStops = array;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000D74CC File Offset: 0x000D56CC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void ParseGlyphRunProperties()
		{
			Glyphs.LayoutDependentGlyphRunProperties layoutDependentGlyphRunProperties = null;
			Uri uri = this.FontUri;
			if (uri != null)
			{
				if (string.IsNullOrEmpty(this.UnicodeString) && string.IsNullOrEmpty(this.Indices))
				{
					throw new ArgumentException(SR.Get("GlyphsUnicodeStringAndIndicesCannotBothBeEmpty"));
				}
				layoutDependentGlyphRunProperties = new Glyphs.LayoutDependentGlyphRunProperties(base.GetDpi().PixelsPerDip);
				if (!uri.IsAbsoluteUri)
				{
					uri = BindUriHelper.GetResolvedUri(BaseUriHelper.GetBaseUri(this), uri);
				}
				layoutDependentGlyphRunProperties.glyphTypeface = new GlyphTypeface(uri, this.StyleSimulations);
				layoutDependentGlyphRunProperties.unicodeString = this.UnicodeString;
				layoutDependentGlyphRunProperties.sideways = this.IsSideways;
				layoutDependentGlyphRunProperties.deviceFontName = this.DeviceFontName;
				List<Glyphs.ParsedGlyphData> list;
				int num = this.ParseGlyphsProperty(layoutDependentGlyphRunProperties.glyphTypeface, layoutDependentGlyphRunProperties.unicodeString, layoutDependentGlyphRunProperties.sideways, out list, out layoutDependentGlyphRunProperties.clusterMap);
				layoutDependentGlyphRunProperties.glyphIndices = new ushort[num];
				layoutDependentGlyphRunProperties.advanceWidths = new double[num];
				this.ParseCaretStops(layoutDependentGlyphRunProperties);
				layoutDependentGlyphRunProperties.glyphOffsets = null;
				int num2 = 0;
				layoutDependentGlyphRunProperties.fontRenderingSize = this.FontRenderingEmSize;
				layoutDependentGlyphRunProperties.bidiLevel = this.BidiLevel;
				double num3 = layoutDependentGlyphRunProperties.fontRenderingSize / 100.0;
				foreach (Glyphs.ParsedGlyphData parsedGlyphData in list)
				{
					layoutDependentGlyphRunProperties.glyphIndices[num2] = parsedGlyphData.glyphIndex;
					layoutDependentGlyphRunProperties.advanceWidths[num2] = parsedGlyphData.advanceWidth * num3;
					if (parsedGlyphData.offsetX != 0.0 || parsedGlyphData.offsetY != 0.0)
					{
						if (layoutDependentGlyphRunProperties.glyphOffsets == null)
						{
							layoutDependentGlyphRunProperties.glyphOffsets = new Point[num];
						}
						layoutDependentGlyphRunProperties.glyphOffsets[num2].X = parsedGlyphData.offsetX * num3;
						layoutDependentGlyphRunProperties.glyphOffsets[num2].Y = parsedGlyphData.offsetY * num3;
					}
					num2++;
				}
			}
			this._glyphRunProperties = layoutDependentGlyphRunProperties;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000D76D0 File Offset: 0x000D58D0
		private static bool IsEmpty(string s)
		{
			foreach (char c in s)
			{
				if (!char.IsWhiteSpace(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000D7704 File Offset: 0x000D5904
		private bool ReadGlyphIndex(string valueSpec, ref bool inCluster, ref int glyphClusterSize, ref int characterClusterSize, ref ushort glyphIndex)
		{
			string s = valueSpec;
			int num = valueSpec.IndexOf('(');
			if (num != -1)
			{
				for (int i = 0; i < num; i++)
				{
					if (!char.IsWhiteSpace(valueSpec[i]))
					{
						throw new ArgumentException(SR.Get("GlyphsClusterBadCharactersBeforeBracket"));
					}
				}
				if (inCluster)
				{
					throw new ArgumentException(SR.Get("GlyphsClusterNoNestedClusters"));
				}
				int num2 = valueSpec.IndexOf(')');
				if (num2 == -1 || num2 <= num + 1)
				{
					throw new ArgumentException(SR.Get("GlyphsClusterNoMatchingBracket"));
				}
				int num3 = valueSpec.IndexOf(':');
				if (num3 == -1)
				{
					string s2 = valueSpec.Substring(num + 1, num2 - (num + 1));
					characterClusterSize = int.Parse(s2, CultureInfo.InvariantCulture);
					glyphClusterSize = 1;
				}
				else
				{
					if (num3 <= num + 1 || num3 >= num2 - 1)
					{
						throw new ArgumentException(SR.Get("GlyphsClusterMisplacedSeparator"));
					}
					string s3 = valueSpec.Substring(num + 1, num3 - (num + 1));
					characterClusterSize = int.Parse(s3, CultureInfo.InvariantCulture);
					string s4 = valueSpec.Substring(num3 + 1, num2 - (num3 + 1));
					glyphClusterSize = int.Parse(s4, CultureInfo.InvariantCulture);
				}
				inCluster = true;
				s = valueSpec.Substring(num2 + 1);
			}
			if (Glyphs.IsEmpty(s))
			{
				return false;
			}
			glyphIndex = ushort.Parse(s, CultureInfo.InvariantCulture);
			return true;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000D783C File Offset: 0x000D5A3C
		private static double GetAdvanceWidth(GlyphTypeface glyphTypeface, ushort glyphIndex, bool sideways)
		{
			double num = sideways ? glyphTypeface.AdvanceHeights[glyphIndex] : glyphTypeface.AdvanceWidths[glyphIndex];
			return num * 100.0;
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x000D7874 File Offset: 0x000D5A74
		private ushort GetGlyphFromCharacter(GlyphTypeface glyphTypeface, char character)
		{
			ushort result;
			glyphTypeface.CharacterToGlyphMap.TryGetValue((int)character, out result);
			return result;
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x000D7891 File Offset: 0x000D5A91
		private static void SetClusterMapEntry(ushort[] clusterMap, int index, ushort value)
		{
			if (index < 0 || index >= clusterMap.Length)
			{
				throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
			}
			clusterMap[index] = value;
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x000D78B4 File Offset: 0x000D5AB4
		private int ParseGlyphsProperty(GlyphTypeface fontFace, string unicodeString, bool sideways, out List<Glyphs.ParsedGlyphData> parsedGlyphs, out ushort[] clusterMap)
		{
			string indices = this.Indices;
			int num = 0;
			int i = 0;
			int num2 = 1;
			int num3 = 1;
			bool flag = false;
			int num4;
			if (!string.IsNullOrEmpty(unicodeString))
			{
				clusterMap = new ushort[unicodeString.Length];
				num4 = unicodeString.Length;
			}
			else
			{
				clusterMap = null;
				num4 = 8;
			}
			if (!string.IsNullOrEmpty(indices))
			{
				num4 = Math.Max(num4, indices.Length / 5);
			}
			parsedGlyphs = new List<Glyphs.ParsedGlyphData>(num4);
			Glyphs.ParsedGlyphData parsedGlyphData = new Glyphs.ParsedGlyphData();
			if (!string.IsNullOrEmpty(indices))
			{
				int num5 = 0;
				int num6 = 0;
				for (int j = 0; j <= indices.Length; j++)
				{
					char c = (j < indices.Length) ? indices[j] : '\0';
					if (c == ',' || c == ';' || j == indices.Length)
					{
						int length = j - num6;
						string text = indices.Substring(num6, length);
						switch (num5)
						{
						case 0:
						{
							bool flag2 = flag;
							if (!this.ReadGlyphIndex(text, ref flag, ref num3, ref num2, ref parsedGlyphData.glyphIndex))
							{
								if (string.IsNullOrEmpty(unicodeString))
								{
									throw new ArgumentException(SR.Get("GlyphsIndexRequiredIfNoUnicode"));
								}
								if (unicodeString.Length <= i)
								{
									throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
								}
								parsedGlyphData.glyphIndex = this.GetGlyphFromCharacter(fontFace, unicodeString[i]);
							}
							if (!flag2 && clusterMap != null)
							{
								if (flag)
								{
									for (int k = i; k < i + num2; k++)
									{
										Glyphs.SetClusterMapEntry(clusterMap, k, (ushort)num);
									}
								}
								else
								{
									Glyphs.SetClusterMapEntry(clusterMap, i, (ushort)num);
								}
							}
							parsedGlyphData.advanceWidth = Glyphs.GetAdvanceWidth(fontFace, parsedGlyphData.glyphIndex, sideways);
							break;
						}
						case 1:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.advanceWidth = double.Parse(text, CultureInfo.InvariantCulture);
								if (parsedGlyphData.advanceWidth < 0.0)
								{
									throw new ArgumentException(SR.Get("GlyphsAdvanceWidthCannotBeNegative"));
								}
							}
							break;
						case 2:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.offsetX = double.Parse(text, CultureInfo.InvariantCulture);
							}
							break;
						case 3:
							if (!Glyphs.IsEmpty(text))
							{
								parsedGlyphData.offsetY = double.Parse(text, CultureInfo.InvariantCulture);
							}
							break;
						default:
							throw new ArgumentException(SR.Get("GlyphsTooManyCommas"));
						}
						num5++;
						num6 = j + 1;
					}
					if (c == ';' || j == indices.Length)
					{
						parsedGlyphs.Add(parsedGlyphData);
						parsedGlyphData = new Glyphs.ParsedGlyphData();
						if (flag)
						{
							num3--;
							if (num3 == 0)
							{
								i += num2;
								flag = false;
							}
						}
						else
						{
							i++;
						}
						num++;
						num5 = 0;
						num6 = j + 1;
					}
				}
			}
			if (unicodeString != null)
			{
				while (i < unicodeString.Length)
				{
					if (flag)
					{
						throw new ArgumentException(SR.Get("GlyphsIndexRequiredWithinCluster"));
					}
					if (unicodeString.Length <= i)
					{
						throw new ArgumentException(SR.Get("GlyphsUnicodeStringIsTooShort"));
					}
					parsedGlyphData.glyphIndex = this.GetGlyphFromCharacter(fontFace, unicodeString[i]);
					parsedGlyphData.advanceWidth = Glyphs.GetAdvanceWidth(fontFace, parsedGlyphData.glyphIndex, sideways);
					parsedGlyphs.Add(parsedGlyphData);
					parsedGlyphData = new Glyphs.ParsedGlyphData();
					Glyphs.SetClusterMapEntry(clusterMap, i, (ushort)num);
					i++;
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x000D7BD5 File Offset: 0x000D5DD5
		private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((UIElement)d).InvalidateVisual();
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x000D7BE2 File Offset: 0x000D5DE2
		private static void GlyphRunPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Glyphs)d)._glyphRunProperties = null;
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x000D7BF0 File Offset: 0x000D5DF0
		private static void OriginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Glyphs)d)._measurementGlyphRun = null;
		}

		/// <summary>Gets the sets the <see cref="T:System.Windows.Media.Brush" /> that is used for the fill of the <see cref="T:System.Windows.Documents.Glyphs" /> class.  </summary>
		/// <returns>A <see cref="T:System.Windows.Media.Brush" /> that is used for the fill of the <see cref="T:System.Windows.Documents.Glyphs" /> class.</returns>
		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06002FDC RID: 12252 RVA: 0x000D7BFE File Offset: 0x000D5DFE
		// (set) Token: 0x06002FDD RID: 12253 RVA: 0x000D7C10 File Offset: 0x000D5E10
		public Brush Fill
		{
			get
			{
				return (Brush)base.GetValue(Glyphs.FillProperty);
			}
			set
			{
				base.SetValue(Glyphs.FillProperty, value);
			}
		}

		/// <summary>Gets or sets a collection of glyph specifications that represents the <see cref="T:System.Windows.Documents.Glyphs" /> object.</summary>
		/// <returns>A collection of glyph specifications that represents the <see cref="T:System.Windows.Documents.Glyphs" /> object.</returns>
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x000D7C1E File Offset: 0x000D5E1E
		// (set) Token: 0x06002FDF RID: 12255 RVA: 0x000D7C30 File Offset: 0x000D5E30
		public string Indices
		{
			get
			{
				return (string)base.GetValue(Glyphs.IndicesProperty);
			}
			set
			{
				base.SetValue(Glyphs.IndicesProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.String" /> that represents the Unicode string for the <see cref="T:System.Windows.Documents.Glyphs" /> object.  </summary>
		/// <returns>A Unicode string for the <see cref="T:System.Windows.Documents.Glyphs" /> object.</returns>
		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06002FE0 RID: 12256 RVA: 0x000D7C3E File Offset: 0x000D5E3E
		// (set) Token: 0x06002FE1 RID: 12257 RVA: 0x000D7C50 File Offset: 0x000D5E50
		public string UnicodeString
		{
			get
			{
				return (string)base.GetValue(Glyphs.UnicodeStringProperty);
			}
			set
			{
				base.SetValue(Glyphs.UnicodeStringProperty, value);
			}
		}

		/// <summary>Gets or sets the caret stops that correspond to the code points in the Unicode string representing the <see cref="T:System.Windows.Documents.Glyphs" />.  </summary>
		/// <returns>A value of type <see cref="T:System.String" /> that represents whether the code points have caret stops.</returns>
		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x000D7C5E File Offset: 0x000D5E5E
		// (set) Token: 0x06002FE3 RID: 12259 RVA: 0x000D7C70 File Offset: 0x000D5E70
		public string CaretStops
		{
			get
			{
				return (string)base.GetValue(Glyphs.CaretStopsProperty);
			}
			set
			{
				base.SetValue(Glyphs.CaretStopsProperty, value);
			}
		}

		/// <summary>Gets or sets the em size used for rendering the <see cref="T:System.Windows.Documents.Glyphs" /> class.  </summary>
		/// <returns>A <see cref="T:System.Double" /> value that represents the em size used for rendering.</returns>
		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06002FE4 RID: 12260 RVA: 0x000D7C7E File Offset: 0x000D5E7E
		// (set) Token: 0x06002FE5 RID: 12261 RVA: 0x000D7C90 File Offset: 0x000D5E90
		[TypeConverter("System.Windows.FontSizeConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double FontRenderingEmSize
		{
			get
			{
				return (double)base.GetValue(Glyphs.FontRenderingEmSizeProperty);
			}
			set
			{
				base.SetValue(Glyphs.FontRenderingEmSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the value of the x origin for the <see cref="T:System.Windows.Documents.Glyphs" /> object.  </summary>
		/// <returns>The x origin for the <see cref="T:System.Windows.Documents.Glyphs" /> object.</returns>
		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06002FE6 RID: 12262 RVA: 0x000D7CA3 File Offset: 0x000D5EA3
		// (set) Token: 0x06002FE7 RID: 12263 RVA: 0x000D7CB5 File Offset: 0x000D5EB5
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double OriginX
		{
			get
			{
				return (double)base.GetValue(Glyphs.OriginXProperty);
			}
			set
			{
				base.SetValue(Glyphs.OriginXProperty, value);
			}
		}

		/// <summary>Gets or sets the value of the y origin for the <see cref="T:System.Windows.Documents.Glyphs" /> object.  </summary>
		/// <returns>The y origin for the <see cref="T:System.Windows.Documents.Glyphs" /> object.</returns>
		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000D7CC8 File Offset: 0x000D5EC8
		// (set) Token: 0x06002FE9 RID: 12265 RVA: 0x000D7CDA File Offset: 0x000D5EDA
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public double OriginY
		{
			get
			{
				return (double)base.GetValue(Glyphs.OriginYProperty);
			}
			set
			{
				base.SetValue(Glyphs.OriginYProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Uri" /> that represents the location of the font used for rendering the <see cref="T:System.Windows.Documents.Glyphs" /> class.  </summary>
		/// <returns>A <see cref="T:System.Uri" /> that represents the location of the font used for rendering the <see cref="T:System.Windows.Documents.Glyphs" /> class.</returns>
		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x000D7CED File Offset: 0x000D5EED
		// (set) Token: 0x06002FEB RID: 12267 RVA: 0x000D7CFF File Offset: 0x000D5EFF
		public Uri FontUri
		{
			get
			{
				return (Uri)base.GetValue(Glyphs.FontUriProperty);
			}
			set
			{
				base.SetValue(Glyphs.FontUriProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.StyleSimulations" /> for the <see cref="T:System.Windows.Documents.Glyphs" /> class.  </summary>
		/// <returns>The <see cref="T:System.Windows.Media.StyleSimulations" /> for the <see cref="T:System.Windows.Documents.Glyphs" /> class.</returns>
		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x000D7D0D File Offset: 0x000D5F0D
		// (set) Token: 0x06002FED RID: 12269 RVA: 0x000D7D1F File Offset: 0x000D5F1F
		public StyleSimulations StyleSimulations
		{
			get
			{
				return (StyleSimulations)base.GetValue(Glyphs.StyleSimulationsProperty);
			}
			set
			{
				base.SetValue(Glyphs.StyleSimulationsProperty, value);
			}
		}

		/// <summary>Determines whether to rotate the <see cref="T:System.Windows.Documents.Glyphs" /> object.  </summary>
		/// <returns>
		///     <see langword="true" /> if the glyphs that make up the <see cref="T:System.Windows.Documents.Glyphs" /> object are rotated 90° counter-clockwise; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06002FEE RID: 12270 RVA: 0x000D7D32 File Offset: 0x000D5F32
		// (set) Token: 0x06002FEF RID: 12271 RVA: 0x000D7D44 File Offset: 0x000D5F44
		public bool IsSideways
		{
			get
			{
				return (bool)base.GetValue(Glyphs.IsSidewaysProperty);
			}
			set
			{
				base.SetValue(Glyphs.IsSidewaysProperty, value);
			}
		}

		/// <summary>Gets or sets the bidirectional nesting level of <see cref="T:System.Windows.Documents.Glyphs" />.  </summary>
		/// <returns>An <see cref="T:System.Int32" /> value that represents the bidirectional nesting level.</returns>
		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06002FF0 RID: 12272 RVA: 0x000D7D52 File Offset: 0x000D5F52
		// (set) Token: 0x06002FF1 RID: 12273 RVA: 0x000D7D64 File Offset: 0x000D5F64
		public int BidiLevel
		{
			get
			{
				return (int)base.GetValue(Glyphs.BidiLevelProperty);
			}
			set
			{
				base.SetValue(Glyphs.BidiLevelProperty, value);
			}
		}

		/// <summary>Gets or sets the specific device font for which the <see cref="T:System.Windows.Documents.Glyphs" /> object has been optimized.  </summary>
		/// <returns>A <see cref="T:System.String" /> value that represents the name of the device font.</returns>
		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06002FF2 RID: 12274 RVA: 0x000D7D77 File Offset: 0x000D5F77
		// (set) Token: 0x06002FF3 RID: 12275 RVA: 0x000D7D89 File Offset: 0x000D5F89
		public string DeviceFontName
		{
			get
			{
				return (string)base.GetValue(Glyphs.DeviceFontNameProperty);
			}
			set
			{
				base.SetValue(Glyphs.DeviceFontNameProperty, value);
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06002FF4 RID: 12276 RVA: 0x000D7D97 File Offset: 0x000D5F97
		internal GlyphRun MeasurementGlyphRun
		{
			get
			{
				if (this._glyphRunProperties == null || this._measurementGlyphRun == null)
				{
					this.ComputeMeasurementGlyphRunAndOrigin();
				}
				return this._measurementGlyphRun;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.Fill" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.Fill" /> dependency property. </returns>
		// Token: 0x04001E52 RID: 7762
		public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(Glyphs.FillChanged), null));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.Indices" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.Indices" /> dependency property. </returns>
		// Token: 0x04001E53 RID: 7763
		public static readonly DependencyProperty IndicesProperty = DependencyProperty.Register("Indices", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.UnicodeString" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.UnicodeString" /> dependency property. </returns>
		// Token: 0x04001E54 RID: 7764
		public static readonly DependencyProperty UnicodeStringProperty = DependencyProperty.Register("UnicodeString", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.CaretStops" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.CaretStops" /> dependency property. </returns>
		// Token: 0x04001E55 RID: 7765
		public static readonly DependencyProperty CaretStopsProperty = DependencyProperty.Register("CaretStops", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.FontRenderingEmSize" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.FontRenderingEmSize" /> dependency property. </returns>
		// Token: 0x04001E56 RID: 7766
		public static readonly DependencyProperty FontRenderingEmSizeProperty = DependencyProperty.Register("FontRenderingEmSize", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.OriginX" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.OriginX" /> dependency property. </returns>
		// Token: 0x04001E57 RID: 7767
		public static readonly DependencyProperty OriginXProperty = DependencyProperty.Register("OriginX", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.OriginPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.OriginY" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.OriginY" /> dependency property. </returns>
		// Token: 0x04001E58 RID: 7768
		public static readonly DependencyProperty OriginYProperty = DependencyProperty.Register("OriginY", typeof(double), typeof(Glyphs), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.OriginPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.FontUri" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.FontUri" /> dependency property. </returns>
		// Token: 0x04001E59 RID: 7769
		public static readonly DependencyProperty FontUriProperty = DependencyProperty.Register("FontUri", typeof(Uri), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.StyleSimulations" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.StyleSimulations" /> dependency property. </returns>
		// Token: 0x04001E5A RID: 7770
		public static readonly DependencyProperty StyleSimulationsProperty = DependencyProperty.Register("StyleSimulations", typeof(StyleSimulations), typeof(Glyphs), new FrameworkPropertyMetadata(StyleSimulations.None, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.IsSideways" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.IsSideways" /> dependency property. </returns>
		// Token: 0x04001E5B RID: 7771
		public static readonly DependencyProperty IsSidewaysProperty = DependencyProperty.Register("IsSideways", typeof(bool), typeof(Glyphs), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.BidiLevel" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.BidiLevel" /> dependency property. </returns>
		// Token: 0x04001E5C RID: 7772
		public static readonly DependencyProperty BidiLevelProperty = DependencyProperty.Register("BidiLevel", typeof(int), typeof(Glyphs), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Glyphs.DeviceFontName" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Glyphs.DeviceFontName" /> dependency property. </returns>
		// Token: 0x04001E5D RID: 7773
		public static readonly DependencyProperty DeviceFontNameProperty = DependencyProperty.Register("DeviceFontName", typeof(string), typeof(Glyphs), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Glyphs.GlyphRunPropertyChanged)));

		// Token: 0x04001E5E RID: 7774
		private Glyphs.LayoutDependentGlyphRunProperties _glyphRunProperties;

		// Token: 0x04001E5F RID: 7775
		private GlyphRun _measurementGlyphRun;

		// Token: 0x04001E60 RID: 7776
		private Point _glyphRunOrigin;

		// Token: 0x04001E61 RID: 7777
		private const double EmMultiplier = 100.0;

		// Token: 0x020008D5 RID: 2261
		private class ParsedGlyphData
		{
			// Token: 0x04004277 RID: 17015
			public ushort glyphIndex;

			// Token: 0x04004278 RID: 17016
			public double advanceWidth;

			// Token: 0x04004279 RID: 17017
			public double offsetX;

			// Token: 0x0400427A RID: 17018
			public double offsetY;
		}

		// Token: 0x020008D6 RID: 2262
		private class LayoutDependentGlyphRunProperties
		{
			// Token: 0x0600849F RID: 33951 RVA: 0x002493E3 File Offset: 0x002475E3
			public LayoutDependentGlyphRunProperties(double pixelsPerDip)
			{
				this._pixelsPerDip = (float)pixelsPerDip;
			}

			// Token: 0x060084A0 RID: 33952 RVA: 0x002493F4 File Offset: 0x002475F4
			public GlyphRun CreateGlyphRun(Point origin, XmlLanguage language)
			{
				return new GlyphRun(this.glyphTypeface, this.bidiLevel, this.sideways, this.fontRenderingSize, this._pixelsPerDip, this.glyphIndices, origin, this.advanceWidths, this.glyphOffsets, this.unicodeString.ToCharArray(), this.deviceFontName, this.clusterMap, this.caretStops, language);
			}

			// Token: 0x0400427B RID: 17019
			public double fontRenderingSize;

			// Token: 0x0400427C RID: 17020
			public ushort[] glyphIndices;

			// Token: 0x0400427D RID: 17021
			public double[] advanceWidths;

			// Token: 0x0400427E RID: 17022
			public Point[] glyphOffsets;

			// Token: 0x0400427F RID: 17023
			public ushort[] clusterMap;

			// Token: 0x04004280 RID: 17024
			public bool sideways;

			// Token: 0x04004281 RID: 17025
			public int bidiLevel;

			// Token: 0x04004282 RID: 17026
			public GlyphTypeface glyphTypeface;

			// Token: 0x04004283 RID: 17027
			public string unicodeString;

			// Token: 0x04004284 RID: 17028
			public IList<bool> caretStops;

			// Token: 0x04004285 RID: 17029
			public string deviceFontName;

			// Token: 0x04004286 RID: 17030
			private float _pixelsPerDip;
		}
	}
}

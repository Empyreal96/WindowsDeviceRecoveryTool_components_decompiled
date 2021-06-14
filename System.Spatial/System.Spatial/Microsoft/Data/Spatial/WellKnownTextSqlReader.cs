using System;
using System.Globalization;
using System.IO;
using System.Spatial;
using System.Text;
using System.Xml;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200007E RID: 126
	internal class WellKnownTextSqlReader : SpatialReader<TextReader>
	{
		// Token: 0x060002FD RID: 765 RVA: 0x00008769 File Offset: 0x00006969
		public WellKnownTextSqlReader(SpatialPipeline destination) : this(destination, false)
		{
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00008773 File Offset: 0x00006973
		public WellKnownTextSqlReader(SpatialPipeline destination, bool allowOnlyTwoDimensions) : base(destination)
		{
			this.allowOnlyTwoDimensions = allowOnlyTwoDimensions;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00008783 File Offset: 0x00006983
		protected override void ReadGeographyImplementation(TextReader input)
		{
			new WellKnownTextSqlReader.Parser(input, new TypeWashedToGeographyLongLatPipeline(base.Destination), this.allowOnlyTwoDimensions).Read();
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000087A1 File Offset: 0x000069A1
		protected override void ReadGeometryImplementation(TextReader input)
		{
			new WellKnownTextSqlReader.Parser(input, new TypeWashedToGeometryPipeline(base.Destination), this.allowOnlyTwoDimensions).Read();
		}

		// Token: 0x040000E6 RID: 230
		private bool allowOnlyTwoDimensions;

		// Token: 0x0200007F RID: 127
		private class Parser
		{
			// Token: 0x06000301 RID: 769 RVA: 0x000087BF File Offset: 0x000069BF
			public Parser(TextReader reader, TypeWashedPipeline pipeline, bool allowOnlyTwoDimensions)
			{
				this.lexer = new WellKnownTextLexer(reader);
				this.pipeline = pipeline;
				this.allowOnlyTwoDimensions = allowOnlyTwoDimensions;
			}

			// Token: 0x06000302 RID: 770 RVA: 0x000087E1 File Offset: 0x000069E1
			public void Read()
			{
				this.ParseSRID();
				this.ParseTaggedText();
			}

			// Token: 0x06000303 RID: 771 RVA: 0x000087EF File Offset: 0x000069EF
			private bool IsTokenMatch(WellKnownTextTokenType type, string text)
			{
				return this.lexer.CurrentToken.MatchToken((int)type, text, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06000304 RID: 772 RVA: 0x00008804 File Offset: 0x00006A04
			private bool NextToken()
			{
				while (this.lexer.Next())
				{
					if (!this.lexer.CurrentToken.MatchToken(8, string.Empty, StringComparison.Ordinal))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06000305 RID: 773 RVA: 0x00008831 File Offset: 0x00006A31
			private void ParseCollectionText()
			{
				if (!this.ReadEmptySet())
				{
					this.ReadToken(WellKnownTextTokenType.LeftParen, null);
					this.ParseTaggedText();
					while (this.ReadOptionalToken(WellKnownTextTokenType.Comma, null))
					{
						this.ParseTaggedText();
					}
					this.ReadToken(WellKnownTextTokenType.RightParen, null);
				}
			}

			// Token: 0x06000306 RID: 774 RVA: 0x00008863 File Offset: 0x00006A63
			private void ParseLineStringText()
			{
				if (!this.ReadEmptySet())
				{
					this.ReadToken(WellKnownTextTokenType.LeftParen, null);
					this.ParsePoint(true);
					while (this.ReadOptionalToken(WellKnownTextTokenType.Comma, null))
					{
						this.ParsePoint(false);
					}
					this.ReadToken(WellKnownTextTokenType.RightParen, null);
					this.pipeline.EndFigure();
				}
			}

			// Token: 0x06000307 RID: 775 RVA: 0x000088A4 File Offset: 0x00006AA4
			private void ParseMultiGeoText(SpatialType innerType, Action innerReader)
			{
				if (!this.ReadEmptySet())
				{
					this.ReadToken(WellKnownTextTokenType.LeftParen, null);
					this.pipeline.BeginGeo(innerType);
					innerReader();
					this.pipeline.EndGeo();
					while (this.ReadOptionalToken(WellKnownTextTokenType.Comma, null))
					{
						this.pipeline.BeginGeo(innerType);
						innerReader();
						this.pipeline.EndGeo();
					}
					this.ReadToken(WellKnownTextTokenType.RightParen, null);
				}
			}

			// Token: 0x06000308 RID: 776 RVA: 0x00008910 File Offset: 0x00006B10
			private void ParsePoint(bool firstFigure)
			{
				double coordinate = this.ReadDouble();
				double coordinate2 = this.ReadDouble();
				double? coordinate3;
				if (this.TryReadOptionalNullableDouble(out coordinate3) && this.allowOnlyTwoDimensions)
				{
					throw new FormatException(Strings.WellKnownText_TooManyDimensions);
				}
				double? coordinate4;
				if (this.TryReadOptionalNullableDouble(out coordinate4) && this.allowOnlyTwoDimensions)
				{
					throw new FormatException(Strings.WellKnownText_TooManyDimensions);
				}
				if (firstFigure)
				{
					this.pipeline.BeginFigure(coordinate, coordinate2, coordinate3, coordinate4);
					return;
				}
				this.pipeline.LineTo(coordinate, coordinate2, coordinate3, coordinate4);
			}

			// Token: 0x06000309 RID: 777 RVA: 0x00008987 File Offset: 0x00006B87
			private void ParsePointText()
			{
				if (!this.ReadEmptySet())
				{
					this.ReadToken(WellKnownTextTokenType.LeftParen, null);
					this.ParsePoint(true);
					this.ReadToken(WellKnownTextTokenType.RightParen, null);
					this.pipeline.EndFigure();
				}
			}

			// Token: 0x0600030A RID: 778 RVA: 0x000089B3 File Offset: 0x00006BB3
			private void ParsePolygonText()
			{
				if (!this.ReadEmptySet())
				{
					this.ReadToken(WellKnownTextTokenType.LeftParen, null);
					this.ParseLineStringText();
					while (this.ReadOptionalToken(WellKnownTextTokenType.Comma, null))
					{
						this.ParseLineStringText();
					}
					this.ReadToken(WellKnownTextTokenType.RightParen, null);
				}
			}

			// Token: 0x0600030B RID: 779 RVA: 0x000089E8 File Offset: 0x00006BE8
			private void ParseSRID()
			{
				if (this.ReadOptionalToken(WellKnownTextTokenType.Text, "SRID"))
				{
					this.ReadToken(WellKnownTextTokenType.Equals, null);
					this.pipeline.SetCoordinateSystem(new int?(this.ReadInteger()));
					this.ReadToken(WellKnownTextTokenType.Semicolon, null);
					return;
				}
				this.pipeline.SetCoordinateSystem(null);
			}

			// Token: 0x0600030C RID: 780 RVA: 0x00008A40 File Offset: 0x00006C40
			private void ParseTaggedText()
			{
				if (!this.NextToken())
				{
					throw new FormatException(Strings.WellKnownText_UnknownTaggedText(string.Empty));
				}
				string key;
				switch (key = this.lexer.CurrentToken.Text.ToUpperInvariant())
				{
				case "POINT":
					this.pipeline.BeginGeo(SpatialType.Point);
					this.ParsePointText();
					this.pipeline.EndGeo();
					return;
				case "LINESTRING":
					this.pipeline.BeginGeo(SpatialType.LineString);
					this.ParseLineStringText();
					this.pipeline.EndGeo();
					return;
				case "POLYGON":
					this.pipeline.BeginGeo(SpatialType.Polygon);
					this.ParsePolygonText();
					this.pipeline.EndGeo();
					return;
				case "MULTIPOINT":
					this.pipeline.BeginGeo(SpatialType.MultiPoint);
					this.ParseMultiGeoText(SpatialType.Point, new Action(this.ParsePointText));
					this.pipeline.EndGeo();
					return;
				case "MULTILINESTRING":
					this.pipeline.BeginGeo(SpatialType.MultiLineString);
					this.ParseMultiGeoText(SpatialType.LineString, new Action(this.ParseLineStringText));
					this.pipeline.EndGeo();
					return;
				case "MULTIPOLYGON":
					this.pipeline.BeginGeo(SpatialType.MultiPolygon);
					this.ParseMultiGeoText(SpatialType.Polygon, new Action(this.ParsePolygonText));
					this.pipeline.EndGeo();
					return;
				case "GEOMETRYCOLLECTION":
					this.pipeline.BeginGeo(SpatialType.Collection);
					this.ParseCollectionText();
					this.pipeline.EndGeo();
					return;
				case "FULLGLOBE":
					this.pipeline.BeginGeo(SpatialType.FullGlobe);
					this.pipeline.EndGeo();
					return;
				}
				throw new FormatException(Strings.WellKnownText_UnknownTaggedText(this.lexer.CurrentToken.Text));
			}

			// Token: 0x0600030D RID: 781 RVA: 0x00008C64 File Offset: 0x00006E64
			private double ReadDouble()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.ReadToken(WellKnownTextTokenType.Number, null);
				stringBuilder.Append(this.lexer.CurrentToken.Text);
				if (this.ReadOptionalToken(WellKnownTextTokenType.Period, null))
				{
					stringBuilder.Append(".");
					this.ReadToken(WellKnownTextTokenType.Number, null);
					stringBuilder.Append(this.lexer.CurrentToken.Text);
				}
				return double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
			}

			// Token: 0x0600030E RID: 782 RVA: 0x00008CDB File Offset: 0x00006EDB
			private bool ReadEmptySet()
			{
				return this.ReadOptionalToken(WellKnownTextTokenType.Text, "EMPTY");
			}

			// Token: 0x0600030F RID: 783 RVA: 0x00008CE9 File Offset: 0x00006EE9
			private int ReadInteger()
			{
				this.ReadToken(WellKnownTextTokenType.Number, null);
				return XmlConvert.ToInt32(this.lexer.CurrentToken.Text);
			}

			// Token: 0x06000310 RID: 784 RVA: 0x00008D08 File Offset: 0x00006F08
			private bool TryReadOptionalNullableDouble(out double? value)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.ReadOptionalToken(WellKnownTextTokenType.Number, null))
				{
					stringBuilder.Append(this.lexer.CurrentToken.Text);
					if (this.ReadOptionalToken(WellKnownTextTokenType.Period, null))
					{
						stringBuilder.Append(".");
						this.ReadToken(WellKnownTextTokenType.Number, null);
						stringBuilder.Append(this.lexer.CurrentToken.Text);
					}
					value = new double?(double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture));
					return true;
				}
				value = null;
				return this.ReadOptionalToken(WellKnownTextTokenType.Text, "NULL");
			}

			// Token: 0x06000311 RID: 785 RVA: 0x00008DA4 File Offset: 0x00006FA4
			private bool ReadOptionalToken(WellKnownTextTokenType expectedTokenType, string expectedTokenText)
			{
				LexerToken lexerToken;
				while (this.lexer.Peek(out lexerToken))
				{
					if (lexerToken.MatchToken(8, null, StringComparison.OrdinalIgnoreCase))
					{
						this.lexer.Next();
					}
					else
					{
						if (lexerToken.MatchToken((int)expectedTokenType, expectedTokenText, StringComparison.OrdinalIgnoreCase))
						{
							this.lexer.Next();
							return true;
						}
						return false;
					}
				}
				return false;
			}

			// Token: 0x06000312 RID: 786 RVA: 0x00008DF7 File Offset: 0x00006FF7
			private void ReadToken(WellKnownTextTokenType type, string text)
			{
				if (!this.NextToken() || !this.IsTokenMatch(type, text))
				{
					throw new FormatException(Strings.WellKnownText_UnexpectedToken(type, text, this.lexer.CurrentToken));
				}
			}

			// Token: 0x040000E7 RID: 231
			private readonly bool allowOnlyTwoDimensions;

			// Token: 0x040000E8 RID: 232
			private readonly TextLexerBase lexer;

			// Token: 0x040000E9 RID: 233
			private readonly TypeWashedPipeline pipeline;
		}
	}
}

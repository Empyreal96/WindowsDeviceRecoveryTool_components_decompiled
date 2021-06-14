using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.OData.Metadata;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000033 RID: 51
	internal sealed class SegmentArgumentParser
	{
		// Token: 0x06000150 RID: 336 RVA: 0x000061A0 File Offset: 0x000043A0
		private SegmentArgumentParser()
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000061A8 File Offset: 0x000043A8
		private SegmentArgumentParser(Dictionary<string, string> namedValues, List<string> positionalValues, bool keysAsSegments)
		{
			this.namedValues = namedValues;
			this.positionalValues = positionalValues;
			this.keysAsSegments = keysAsSegments;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000061C5 File Offset: 0x000043C5
		internal bool AreValuesNamed
		{
			get
			{
				return this.namedValues != null;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000061D3 File Offset: 0x000043D3
		internal bool IsEmpty
		{
			get
			{
				return this == SegmentArgumentParser.Empty;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000061DD File Offset: 0x000043DD
		internal IDictionary<string, string> NamedValues
		{
			get
			{
				return this.namedValues;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000061E5 File Offset: 0x000043E5
		internal IList<string> PositionalValues
		{
			get
			{
				return this.positionalValues;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000061ED File Offset: 0x000043ED
		internal int ValueCount
		{
			get
			{
				if (this == SegmentArgumentParser.Empty)
				{
					return 0;
				}
				if (this.namedValues != null)
				{
					return this.namedValues.Count;
				}
				return this.positionalValues.Count;
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006218 File Offset: 0x00004418
		internal static bool TryParseKeysFromUri(string text, out SegmentArgumentParser instance)
		{
			return SegmentArgumentParser.TryParseFromUri(text, true, false, out instance);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006224 File Offset: 0x00004424
		internal static SegmentArgumentParser FromSegment(string segmentText)
		{
			return new SegmentArgumentParser(null, new List<string>
			{
				segmentText
			}, true);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006246 File Offset: 0x00004446
		internal static bool TryParseNullableTokens(string text, out SegmentArgumentParser instance)
		{
			return SegmentArgumentParser.TryParseFromUri(text, false, true, out instance);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006254 File Offset: 0x00004454
		internal bool TryConvertValues(IList<IEdmStructuralProperty> keyProperties, out IEnumerable<KeyValuePair<string, object>> keyPairs)
		{
			if (this.NamedValues != null)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
				keyPairs = dictionary;
				using (IEnumerator<IEdmStructuralProperty> enumerator = keyProperties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IEdmStructuralProperty edmStructuralProperty = enumerator.Current;
						string valueText;
						if (!this.NamedValues.TryGetValue(edmStructuralProperty.Name, out valueText))
						{
							return false;
						}
						object value;
						if (!this.TryConvertValue(edmStructuralProperty.Type.AsPrimitive(), valueText, out value))
						{
							return false;
						}
						dictionary[edmStructuralProperty.Name] = value;
					}
					return true;
				}
			}
			List<KeyValuePair<string, object>> list = new List<KeyValuePair<string, object>>(this.positionalValues.Count);
			keyPairs = list;
			for (int i = 0; i < keyProperties.Count; i++)
			{
				string valueText2 = this.positionalValues[i];
				IEdmProperty edmProperty = keyProperties[i];
				object value2;
				if (!this.TryConvertValue(edmProperty.Type.AsPrimitive(), valueText2, out value2))
				{
					return false;
				}
				list.Add(new KeyValuePair<string, object>(edmProperty.Name, value2));
			}
			return true;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006380 File Offset: 0x00004580
		private bool TryConvertValue(IEdmPrimitiveTypeReference primitiveType, string valueText, out object convertedValue)
		{
			Type primitiveClrType = EdmLibraryExtensions.GetPrimitiveClrType((IEdmPrimitiveType)primitiveType.Definition, primitiveType.IsNullable);
			LiteralParser literalParser = LiteralParser.ForKeys(this.keysAsSegments);
			return literalParser.TryParseLiteral(primitiveClrType, valueText, out convertedValue);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000063BC File Offset: 0x000045BC
		private static bool TryParseFromUri(string text, bool allowNamedValues, bool allowNull, out SegmentArgumentParser instance)
		{
			Dictionary<string, string> dictionary = null;
			List<string> list = null;
			ExpressionLexer expressionLexer = new ExpressionLexer(text, true, false);
			ExpressionToken currentToken = expressionLexer.CurrentToken;
			if (currentToken.Kind == ExpressionTokenKind.End)
			{
				instance = SegmentArgumentParser.Empty;
				return true;
			}
			instance = null;
			for (;;)
			{
				if (currentToken.Kind == ExpressionTokenKind.Identifier && allowNamedValues)
				{
					if (list != null)
					{
						break;
					}
					string identifier = expressionLexer.CurrentToken.GetIdentifier();
					expressionLexer.NextToken();
					if (expressionLexer.CurrentToken.Kind != ExpressionTokenKind.Equal)
					{
						return false;
					}
					expressionLexer.NextToken();
					if (!expressionLexer.CurrentToken.IsKeyValueToken)
					{
						return false;
					}
					string text2 = expressionLexer.CurrentToken.Text;
					SegmentArgumentParser.CreateIfNull<Dictionary<string, string>>(ref dictionary);
					if (dictionary.ContainsKey(identifier))
					{
						return false;
					}
					dictionary.Add(identifier, text2);
				}
				else
				{
					if (!currentToken.IsKeyValueToken && (!allowNull || currentToken.Kind != ExpressionTokenKind.NullLiteral))
					{
						return false;
					}
					if (dictionary != null)
					{
						return false;
					}
					SegmentArgumentParser.CreateIfNull<List<string>>(ref list);
					list.Add(expressionLexer.CurrentToken.Text);
				}
				expressionLexer.NextToken();
				currentToken = expressionLexer.CurrentToken;
				if (currentToken.Kind == ExpressionTokenKind.Comma)
				{
					expressionLexer.NextToken();
					currentToken = expressionLexer.CurrentToken;
					if (currentToken.Kind == ExpressionTokenKind.End)
					{
						return false;
					}
				}
				if (currentToken.Kind == ExpressionTokenKind.End)
				{
					goto Block_13;
				}
			}
			return false;
			Block_13:
			instance = new SegmentArgumentParser(dictionary, list, false);
			return true;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000064F8 File Offset: 0x000046F8
		private static void CreateIfNull<T>(ref T value) where T : new()
		{
			if (value == null)
			{
				value = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
			}
		}

		// Token: 0x04000064 RID: 100
		private static readonly SegmentArgumentParser Empty = new SegmentArgumentParser();

		// Token: 0x04000065 RID: 101
		private readonly Dictionary<string, string> namedValues;

		// Token: 0x04000066 RID: 102
		private readonly List<string> positionalValues;

		// Token: 0x04000067 RID: 103
		private readonly bool keysAsSegments;
	}
}

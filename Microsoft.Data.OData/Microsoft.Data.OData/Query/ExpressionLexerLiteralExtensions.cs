using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x02000028 RID: 40
	internal static class ExpressionLexerLiteralExtensions
	{
		// Token: 0x06000105 RID: 261 RVA: 0x00004FD0 File Offset: 0x000031D0
		internal static bool IsLiteralType(this ExpressionTokenKind tokenKind)
		{
			switch (tokenKind)
			{
			case ExpressionTokenKind.NullLiteral:
			case ExpressionTokenKind.BooleanLiteral:
			case ExpressionTokenKind.StringLiteral:
			case ExpressionTokenKind.IntegerLiteral:
			case ExpressionTokenKind.Int64Literal:
			case ExpressionTokenKind.SingleLiteral:
			case ExpressionTokenKind.DateTimeLiteral:
			case ExpressionTokenKind.DateTimeOffsetLiteral:
			case ExpressionTokenKind.TimeLiteral:
			case ExpressionTokenKind.DecimalLiteral:
			case ExpressionTokenKind.DoubleLiteral:
			case ExpressionTokenKind.GuidLiteral:
			case ExpressionTokenKind.BinaryLiteral:
			case ExpressionTokenKind.GeographyLiteral:
			case ExpressionTokenKind.GeometryLiteral:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005028 File Offset: 0x00003228
		internal static object ReadLiteralToken(this ExpressionLexer expressionLexer)
		{
			expressionLexer.NextToken();
			if (expressionLexer.CurrentToken.Kind.IsLiteralType())
			{
				return expressionLexer.TryParseLiteral();
			}
			throw new ODataException(Strings.ExpressionLexer_ExpectedLiteralToken(expressionLexer.CurrentToken.Text));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005060 File Offset: 0x00003260
		private static object ParseNullLiteral(this ExpressionLexer expressionLexer)
		{
			expressionLexer.NextToken();
			ODataUriNullValue odataUriNullValue = new ODataUriNullValue();
			if (expressionLexer.ExpressionText == "null")
			{
				return odataUriNullValue;
			}
			int num = "'".Length * 2 + "null".Length;
			int startIndex = "'".Length + "null".Length;
			odataUriNullValue.TypeName = expressionLexer.ExpressionText.Substring(startIndex, expressionLexer.ExpressionText.Length - num);
			return odataUriNullValue;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000050DC File Offset: 0x000032DC
		private static object ParseTypedLiteral(this ExpressionLexer expressionLexer, IEdmPrimitiveTypeReference targetTypeReference)
		{
			object result;
			if (!UriPrimitiveTypeParser.TryUriStringToPrimitive(expressionLexer.CurrentToken.Text, targetTypeReference, out result))
			{
				string message = Strings.UriQueryExpressionParser_UnrecognizedLiteral(targetTypeReference.FullName(), expressionLexer.CurrentToken.Text, expressionLexer.CurrentToken.Position, expressionLexer.ExpressionText);
				throw new ODataException(message);
			}
			expressionLexer.NextToken();
			return result;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000513C File Offset: 0x0000333C
		private static object TryParseLiteral(this ExpressionLexer expressionLexer)
		{
			switch (expressionLexer.CurrentToken.Kind)
			{
			case ExpressionTokenKind.NullLiteral:
				return expressionLexer.ParseNullLiteral();
			case ExpressionTokenKind.BooleanLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetBoolean(false));
			case ExpressionTokenKind.StringLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetString(true));
			case ExpressionTokenKind.IntegerLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetInt32(false));
			case ExpressionTokenKind.Int64Literal:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetInt64(false));
			case ExpressionTokenKind.SingleLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetSingle(false));
			case ExpressionTokenKind.DateTimeLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.DateTime, false));
			case ExpressionTokenKind.DateTimeOffsetLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetDateTimeOffset(false));
			case ExpressionTokenKind.TimeLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetTemporal(EdmPrimitiveTypeKind.Time, false));
			case ExpressionTokenKind.DecimalLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetDecimal(false));
			case ExpressionTokenKind.DoubleLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetDouble(false));
			case ExpressionTokenKind.GuidLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetGuid(false));
			case ExpressionTokenKind.BinaryLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetBinary(true));
			case ExpressionTokenKind.GeographyLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.Geography, false));
			case ExpressionTokenKind.GeometryLiteral:
				return expressionLexer.ParseTypedLiteral(EdmCoreModel.Instance.GetSpatial(EdmPrimitiveTypeKind.Geometry, false));
			default:
				return null;
			}
		}
	}
}

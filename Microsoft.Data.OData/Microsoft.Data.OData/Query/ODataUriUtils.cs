using System;
using System.Spatial;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020001E8 RID: 488
	public static class ODataUriUtils
	{
		// Token: 0x06000F08 RID: 3848 RVA: 0x00035C07 File Offset: 0x00033E07
		public static object ConvertFromUriLiteral(string value, ODataVersion version)
		{
			return ODataUriUtils.ConvertFromUriLiteral(value, version, null, null);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00035C14 File Offset: 0x00033E14
		public static object ConvertFromUriLiteral(string value, ODataVersion version, IEdmModel model, IEdmTypeReference typeReference)
		{
			ExceptionUtils.CheckArgumentNotNull<string>(value, "value");
			if (typeReference != null && model == null)
			{
				throw new ODataException(Strings.ODataUriUtils_ConvertFromUriLiteralTypeRefWithoutModel);
			}
			if (model == null)
			{
				model = EdmCoreModel.Instance;
			}
			ExpressionLexer expressionLexer = new ExpressionLexer(value, false, false);
			ExpressionToken expressionToken;
			Exception ex;
			expressionLexer.TryPeekNextToken(out expressionToken, out ex);
			if (expressionToken.Kind == ExpressionTokenKind.BracketedExpression)
			{
				return ODataUriConversionUtils.ConvertFromComplexOrCollectionValue(value, version, model, typeReference);
			}
			object obj = expressionLexer.ReadLiteralToken();
			if (typeReference != null)
			{
				obj = ODataUriConversionUtils.VerifyAndCoerceUriPrimitiveLiteral(obj, model, typeReference, version);
			}
			if (obj is ISpatial)
			{
				ODataVersionChecker.CheckSpatialValue(version);
			}
			return obj;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00035C93 File Offset: 0x00033E93
		public static string ConvertToUriLiteral(object value, ODataVersion version)
		{
			return ODataUriUtils.ConvertToUriLiteral(value, version, null);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00035C9D File Offset: 0x00033E9D
		public static string ConvertToUriLiteral(object value, ODataVersion version, IEdmModel model)
		{
			return ODataUriUtils.ConvertToUriLiteral(value, version, model, ODataFormat.VerboseJson);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x00035CAC File Offset: 0x00033EAC
		public static string ConvertToUriLiteral(object value, ODataVersion version, IEdmModel model, ODataFormat format)
		{
			if (value == null)
			{
				value = new ODataUriNullValue();
			}
			if (model == null)
			{
				model = EdmCoreModel.Instance;
			}
			ODataUriNullValue odataUriNullValue = value as ODataUriNullValue;
			if (odataUriNullValue != null)
			{
				return ODataUriConversionUtils.ConvertToUriNullValue(odataUriNullValue);
			}
			ODataCollectionValue odataCollectionValue = value as ODataCollectionValue;
			if (odataCollectionValue != null)
			{
				return ODataUriConversionUtils.ConvertToUriCollectionLiteral(odataCollectionValue, model, version, format);
			}
			ODataComplexValue odataComplexValue = value as ODataComplexValue;
			if (odataComplexValue != null)
			{
				return ODataUriConversionUtils.ConvertToUriComplexLiteral(odataComplexValue, model, version, format);
			}
			return ODataUriConversionUtils.ConvertToUriPrimitiveLiteral(value, version);
		}
	}
}

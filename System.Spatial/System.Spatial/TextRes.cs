using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Spatial
{
	// Token: 0x0200008C RID: 140
	internal sealed class TextRes
	{
		// Token: 0x06000371 RID: 881 RVA: 0x00009A6B File Offset: 0x00007C6B
		internal TextRes()
		{
			this.resources = new ResourceManager("System.Spatial", base.GetType().Assembly);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00009A90 File Offset: 0x00007C90
		private static TextRes GetLoader()
		{
			if (TextRes.loader == null)
			{
				TextRes value = new TextRes();
				Interlocked.CompareExchange<TextRes>(ref TextRes.loader, value, null);
			}
			return TextRes.loader;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00009ABC File Offset: 0x00007CBC
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00009ABF File Offset: 0x00007CBF
		public static ResourceManager Resources
		{
			get
			{
				return TextRes.GetLoader().resources;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00009ACC File Offset: 0x00007CCC
		public static string GetString(string name, params object[] args)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			string @string = textRes.resources.GetString(name, TextRes.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00009B50 File Offset: 0x00007D50
		public static string GetString(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetString(name, TextRes.Culture);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00009B79 File Offset: 0x00007D79
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return TextRes.GetString(name);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00009B84 File Offset: 0x00007D84
		public static object GetObject(string name)
		{
			TextRes textRes = TextRes.GetLoader();
			if (textRes == null)
			{
				return null;
			}
			return textRes.resources.GetObject(name, TextRes.Culture);
		}

		// Token: 0x04000119 RID: 281
		internal const string PriorityQueueDoesNotContainItem = "PriorityQueueDoesNotContainItem";

		// Token: 0x0400011A RID: 282
		internal const string PriorityQueueOperationNotValidOnEmptyQueue = "PriorityQueueOperationNotValidOnEmptyQueue";

		// Token: 0x0400011B RID: 283
		internal const string PriorityQueueEnqueueExistingPriority = "PriorityQueueEnqueueExistingPriority";

		// Token: 0x0400011C RID: 284
		internal const string SpatialImplementation_NoRegisteredOperations = "SpatialImplementation_NoRegisteredOperations";

		// Token: 0x0400011D RID: 285
		internal const string InvalidPointCoordinate = "InvalidPointCoordinate";

		// Token: 0x0400011E RID: 286
		internal const string Point_AccessCoordinateWhenEmpty = "Point_AccessCoordinateWhenEmpty";

		// Token: 0x0400011F RID: 287
		internal const string SpatialBuilder_CannotCreateBeforeDrawn = "SpatialBuilder_CannotCreateBeforeDrawn";

		// Token: 0x04000120 RID: 288
		internal const string GmlReader_UnexpectedElement = "GmlReader_UnexpectedElement";

		// Token: 0x04000121 RID: 289
		internal const string GmlReader_ExpectReaderAtElement = "GmlReader_ExpectReaderAtElement";

		// Token: 0x04000122 RID: 290
		internal const string GmlReader_InvalidSpatialType = "GmlReader_InvalidSpatialType";

		// Token: 0x04000123 RID: 291
		internal const string GmlReader_EmptyRingsNotAllowed = "GmlReader_EmptyRingsNotAllowed";

		// Token: 0x04000124 RID: 292
		internal const string GmlReader_PosNeedTwoNumbers = "GmlReader_PosNeedTwoNumbers";

		// Token: 0x04000125 RID: 293
		internal const string GmlReader_PosListNeedsEvenCount = "GmlReader_PosListNeedsEvenCount";

		// Token: 0x04000126 RID: 294
		internal const string GmlReader_InvalidSrsName = "GmlReader_InvalidSrsName";

		// Token: 0x04000127 RID: 295
		internal const string GmlReader_InvalidAttribute = "GmlReader_InvalidAttribute";

		// Token: 0x04000128 RID: 296
		internal const string WellKnownText_UnexpectedToken = "WellKnownText_UnexpectedToken";

		// Token: 0x04000129 RID: 297
		internal const string WellKnownText_UnexpectedCharacter = "WellKnownText_UnexpectedCharacter";

		// Token: 0x0400012A RID: 298
		internal const string WellKnownText_UnknownTaggedText = "WellKnownText_UnknownTaggedText";

		// Token: 0x0400012B RID: 299
		internal const string WellKnownText_TooManyDimensions = "WellKnownText_TooManyDimensions";

		// Token: 0x0400012C RID: 300
		internal const string Validator_SridMismatch = "Validator_SridMismatch";

		// Token: 0x0400012D RID: 301
		internal const string Validator_InvalidType = "Validator_InvalidType";

		// Token: 0x0400012E RID: 302
		internal const string Validator_FullGlobeInCollection = "Validator_FullGlobeInCollection";

		// Token: 0x0400012F RID: 303
		internal const string Validator_LineStringNeedsTwoPoints = "Validator_LineStringNeedsTwoPoints";

		// Token: 0x04000130 RID: 304
		internal const string Validator_FullGlobeCannotHaveElements = "Validator_FullGlobeCannotHaveElements";

		// Token: 0x04000131 RID: 305
		internal const string Validator_NestingOverflow = "Validator_NestingOverflow";

		// Token: 0x04000132 RID: 306
		internal const string Validator_InvalidPointCoordinate = "Validator_InvalidPointCoordinate";

		// Token: 0x04000133 RID: 307
		internal const string Validator_UnexpectedCall = "Validator_UnexpectedCall";

		// Token: 0x04000134 RID: 308
		internal const string Validator_UnexpectedCall2 = "Validator_UnexpectedCall2";

		// Token: 0x04000135 RID: 309
		internal const string Validator_InvalidPolygonPoints = "Validator_InvalidPolygonPoints";

		// Token: 0x04000136 RID: 310
		internal const string Validator_InvalidLatitudeCoordinate = "Validator_InvalidLatitudeCoordinate";

		// Token: 0x04000137 RID: 311
		internal const string Validator_InvalidLongitudeCoordinate = "Validator_InvalidLongitudeCoordinate";

		// Token: 0x04000138 RID: 312
		internal const string Validator_UnexpectedGeography = "Validator_UnexpectedGeography";

		// Token: 0x04000139 RID: 313
		internal const string Validator_UnexpectedGeometry = "Validator_UnexpectedGeometry";

		// Token: 0x0400013A RID: 314
		internal const string GeoJsonReader_MissingRequiredMember = "GeoJsonReader_MissingRequiredMember";

		// Token: 0x0400013B RID: 315
		internal const string GeoJsonReader_InvalidPosition = "GeoJsonReader_InvalidPosition";

		// Token: 0x0400013C RID: 316
		internal const string GeoJsonReader_InvalidTypeName = "GeoJsonReader_InvalidTypeName";

		// Token: 0x0400013D RID: 317
		internal const string GeoJsonReader_InvalidNullElement = "GeoJsonReader_InvalidNullElement";

		// Token: 0x0400013E RID: 318
		internal const string GeoJsonReader_ExpectedNumeric = "GeoJsonReader_ExpectedNumeric";

		// Token: 0x0400013F RID: 319
		internal const string GeoJsonReader_ExpectedArray = "GeoJsonReader_ExpectedArray";

		// Token: 0x04000140 RID: 320
		internal const string GeoJsonReader_InvalidCrsType = "GeoJsonReader_InvalidCrsType";

		// Token: 0x04000141 RID: 321
		internal const string GeoJsonReader_InvalidCrsName = "GeoJsonReader_InvalidCrsName";

		// Token: 0x04000142 RID: 322
		internal const string JsonReaderExtensions_CannotReadPropertyValueAsString = "JsonReaderExtensions_CannotReadPropertyValueAsString";

		// Token: 0x04000143 RID: 323
		internal const string JsonReaderExtensions_CannotReadValueAsJsonObject = "JsonReaderExtensions_CannotReadValueAsJsonObject";

		// Token: 0x04000144 RID: 324
		private static TextRes loader;

		// Token: 0x04000145 RID: 325
		private ResourceManager resources;
	}
}

using System;

namespace System.Spatial
{
	// Token: 0x0200008D RID: 141
	internal static class Strings
	{
		// Token: 0x0600037A RID: 890 RVA: 0x00009BB0 File Offset: 0x00007DB0
		internal static string PriorityQueueDoesNotContainItem(object p0)
		{
			return TextRes.GetString("PriorityQueueDoesNotContainItem", new object[]
			{
				p0
			});
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00009BD3 File Offset: 0x00007DD3
		internal static string PriorityQueueOperationNotValidOnEmptyQueue
		{
			get
			{
				return TextRes.GetString("PriorityQueueOperationNotValidOnEmptyQueue");
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00009BDF File Offset: 0x00007DDF
		internal static string PriorityQueueEnqueueExistingPriority
		{
			get
			{
				return TextRes.GetString("PriorityQueueEnqueueExistingPriority");
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00009BEB File Offset: 0x00007DEB
		internal static string SpatialImplementation_NoRegisteredOperations
		{
			get
			{
				return TextRes.GetString("SpatialImplementation_NoRegisteredOperations");
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00009BF8 File Offset: 0x00007DF8
		internal static string InvalidPointCoordinate(object p0, object p1)
		{
			return TextRes.GetString("InvalidPointCoordinate", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00009C1F File Offset: 0x00007E1F
		internal static string Point_AccessCoordinateWhenEmpty
		{
			get
			{
				return TextRes.GetString("Point_AccessCoordinateWhenEmpty");
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00009C2B File Offset: 0x00007E2B
		internal static string SpatialBuilder_CannotCreateBeforeDrawn
		{
			get
			{
				return TextRes.GetString("SpatialBuilder_CannotCreateBeforeDrawn");
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00009C38 File Offset: 0x00007E38
		internal static string GmlReader_UnexpectedElement(object p0)
		{
			return TextRes.GetString("GmlReader_UnexpectedElement", new object[]
			{
				p0
			});
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00009C5B File Offset: 0x00007E5B
		internal static string GmlReader_ExpectReaderAtElement
		{
			get
			{
				return TextRes.GetString("GmlReader_ExpectReaderAtElement");
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00009C68 File Offset: 0x00007E68
		internal static string GmlReader_InvalidSpatialType(object p0)
		{
			return TextRes.GetString("GmlReader_InvalidSpatialType", new object[]
			{
				p0
			});
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00009C8B File Offset: 0x00007E8B
		internal static string GmlReader_EmptyRingsNotAllowed
		{
			get
			{
				return TextRes.GetString("GmlReader_EmptyRingsNotAllowed");
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00009C97 File Offset: 0x00007E97
		internal static string GmlReader_PosNeedTwoNumbers
		{
			get
			{
				return TextRes.GetString("GmlReader_PosNeedTwoNumbers");
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00009CA3 File Offset: 0x00007EA3
		internal static string GmlReader_PosListNeedsEvenCount
		{
			get
			{
				return TextRes.GetString("GmlReader_PosListNeedsEvenCount");
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00009CB0 File Offset: 0x00007EB0
		internal static string GmlReader_InvalidSrsName(object p0)
		{
			return TextRes.GetString("GmlReader_InvalidSrsName", new object[]
			{
				p0
			});
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00009CD4 File Offset: 0x00007ED4
		internal static string GmlReader_InvalidAttribute(object p0, object p1)
		{
			return TextRes.GetString("GmlReader_InvalidAttribute", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00009CFC File Offset: 0x00007EFC
		internal static string WellKnownText_UnexpectedToken(object p0, object p1, object p2)
		{
			return TextRes.GetString("WellKnownText_UnexpectedToken", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00009D28 File Offset: 0x00007F28
		internal static string WellKnownText_UnexpectedCharacter(object p0)
		{
			return TextRes.GetString("WellKnownText_UnexpectedCharacter", new object[]
			{
				p0
			});
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00009D4C File Offset: 0x00007F4C
		internal static string WellKnownText_UnknownTaggedText(object p0)
		{
			return TextRes.GetString("WellKnownText_UnknownTaggedText", new object[]
			{
				p0
			});
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00009D6F File Offset: 0x00007F6F
		internal static string WellKnownText_TooManyDimensions
		{
			get
			{
				return TextRes.GetString("WellKnownText_TooManyDimensions");
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00009D7B File Offset: 0x00007F7B
		internal static string Validator_SridMismatch
		{
			get
			{
				return TextRes.GetString("Validator_SridMismatch");
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00009D88 File Offset: 0x00007F88
		internal static string Validator_InvalidType(object p0)
		{
			return TextRes.GetString("Validator_InvalidType", new object[]
			{
				p0
			});
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00009DAB File Offset: 0x00007FAB
		internal static string Validator_FullGlobeInCollection
		{
			get
			{
				return TextRes.GetString("Validator_FullGlobeInCollection");
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00009DB7 File Offset: 0x00007FB7
		internal static string Validator_LineStringNeedsTwoPoints
		{
			get
			{
				return TextRes.GetString("Validator_LineStringNeedsTwoPoints");
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00009DC3 File Offset: 0x00007FC3
		internal static string Validator_FullGlobeCannotHaveElements
		{
			get
			{
				return TextRes.GetString("Validator_FullGlobeCannotHaveElements");
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00009DD0 File Offset: 0x00007FD0
		internal static string Validator_NestingOverflow(object p0)
		{
			return TextRes.GetString("Validator_NestingOverflow", new object[]
			{
				p0
			});
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00009DF4 File Offset: 0x00007FF4
		internal static string Validator_InvalidPointCoordinate(object p0, object p1, object p2, object p3)
		{
			return TextRes.GetString("Validator_InvalidPointCoordinate", new object[]
			{
				p0,
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00009E24 File Offset: 0x00008024
		internal static string Validator_UnexpectedCall(object p0, object p1)
		{
			return TextRes.GetString("Validator_UnexpectedCall", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00009E4C File Offset: 0x0000804C
		internal static string Validator_UnexpectedCall2(object p0, object p1, object p2)
		{
			return TextRes.GetString("Validator_UnexpectedCall2", new object[]
			{
				p0,
				p1,
				p2
			});
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00009E77 File Offset: 0x00008077
		internal static string Validator_InvalidPolygonPoints
		{
			get
			{
				return TextRes.GetString("Validator_InvalidPolygonPoints");
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00009E84 File Offset: 0x00008084
		internal static string Validator_InvalidLatitudeCoordinate(object p0)
		{
			return TextRes.GetString("Validator_InvalidLatitudeCoordinate", new object[]
			{
				p0
			});
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00009EA8 File Offset: 0x000080A8
		internal static string Validator_InvalidLongitudeCoordinate(object p0)
		{
			return TextRes.GetString("Validator_InvalidLongitudeCoordinate", new object[]
			{
				p0
			});
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00009ECB File Offset: 0x000080CB
		internal static string Validator_UnexpectedGeography
		{
			get
			{
				return TextRes.GetString("Validator_UnexpectedGeography");
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00009ED7 File Offset: 0x000080D7
		internal static string Validator_UnexpectedGeometry
		{
			get
			{
				return TextRes.GetString("Validator_UnexpectedGeometry");
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00009EE4 File Offset: 0x000080E4
		internal static string GeoJsonReader_MissingRequiredMember(object p0)
		{
			return TextRes.GetString("GeoJsonReader_MissingRequiredMember", new object[]
			{
				p0
			});
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00009F07 File Offset: 0x00008107
		internal static string GeoJsonReader_InvalidPosition
		{
			get
			{
				return TextRes.GetString("GeoJsonReader_InvalidPosition");
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00009F14 File Offset: 0x00008114
		internal static string GeoJsonReader_InvalidTypeName(object p0)
		{
			return TextRes.GetString("GeoJsonReader_InvalidTypeName", new object[]
			{
				p0
			});
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600039E RID: 926 RVA: 0x00009F37 File Offset: 0x00008137
		internal static string GeoJsonReader_InvalidNullElement
		{
			get
			{
				return TextRes.GetString("GeoJsonReader_InvalidNullElement");
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00009F43 File Offset: 0x00008143
		internal static string GeoJsonReader_ExpectedNumeric
		{
			get
			{
				return TextRes.GetString("GeoJsonReader_ExpectedNumeric");
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x00009F4F File Offset: 0x0000814F
		internal static string GeoJsonReader_ExpectedArray
		{
			get
			{
				return TextRes.GetString("GeoJsonReader_ExpectedArray");
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00009F5C File Offset: 0x0000815C
		internal static string GeoJsonReader_InvalidCrsType(object p0)
		{
			return TextRes.GetString("GeoJsonReader_InvalidCrsType", new object[]
			{
				p0
			});
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00009F80 File Offset: 0x00008180
		internal static string GeoJsonReader_InvalidCrsName(object p0)
		{
			return TextRes.GetString("GeoJsonReader_InvalidCrsName", new object[]
			{
				p0
			});
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00009FA4 File Offset: 0x000081A4
		internal static string JsonReaderExtensions_CannotReadPropertyValueAsString(object p0, object p1)
		{
			return TextRes.GetString("JsonReaderExtensions_CannotReadPropertyValueAsString", new object[]
			{
				p0,
				p1
			});
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00009FCC File Offset: 0x000081CC
		internal static string JsonReaderExtensions_CannotReadValueAsJsonObject(object p0)
		{
			return TextRes.GetString("JsonReaderExtensions_CannotReadValueAsJsonObject", new object[]
			{
				p0
			});
		}
	}
}

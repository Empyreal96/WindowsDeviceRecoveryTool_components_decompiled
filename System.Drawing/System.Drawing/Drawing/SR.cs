using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000051 RID: 81
	internal sealed class SR
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0001C43A File Offset: 0x0001A63A
		internal SR()
		{
			this.resources = new ResourceManager("System.Drawing.Res", base.GetType().Assembly);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001C460 File Offset: 0x0001A660
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				SR value = new SR();
				Interlocked.CompareExchange<SR>(ref SR.loader, value, null);
			}
			return SR.loader;
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0001C48C File Offset: 0x0001A68C
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0001C48F File Offset: 0x0001A68F
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001C49C File Offset: 0x0001A69C
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
			if (args != null && args.Length != 0)
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

		// Token: 0x060006FD RID: 1789 RVA: 0x0001C51C File Offset: 0x0001A71C
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001C545 File Offset: 0x0001A745
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return SR.GetString(name);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001C550 File Offset: 0x0001A750
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x040005A8 RID: 1448
		internal const string CantTellPrinterName = "CantTellPrinterName";

		// Token: 0x040005A9 RID: 1449
		internal const string CantChangeImmutableObjects = "CantChangeImmutableObjects";

		// Token: 0x040005AA RID: 1450
		internal const string CantMakeIconTransparent = "CantMakeIconTransparent";

		// Token: 0x040005AB RID: 1451
		internal const string ColorNotSystemColor = "ColorNotSystemColor";

		// Token: 0x040005AC RID: 1452
		internal const string DotNET_ComponentType = "DotNET_ComponentType";

		// Token: 0x040005AD RID: 1453
		internal const string GdiplusAborted = "GdiplusAborted";

		// Token: 0x040005AE RID: 1454
		internal const string GdiplusAccessDenied = "GdiplusAccessDenied";

		// Token: 0x040005AF RID: 1455
		internal const string GdiplusCannotCreateGraphicsFromIndexedPixelFormat = "GdiplusCannotCreateGraphicsFromIndexedPixelFormat";

		// Token: 0x040005B0 RID: 1456
		internal const string GdiplusCannotSetPixelFromIndexedPixelFormat = "GdiplusCannotSetPixelFromIndexedPixelFormat";

		// Token: 0x040005B1 RID: 1457
		internal const string GdiplusDestPointsInvalidParallelogram = "GdiplusDestPointsInvalidParallelogram";

		// Token: 0x040005B2 RID: 1458
		internal const string GdiplusDestPointsInvalidLength = "GdiplusDestPointsInvalidLength";

		// Token: 0x040005B3 RID: 1459
		internal const string GdiplusFileNotFound = "GdiplusFileNotFound";

		// Token: 0x040005B4 RID: 1460
		internal const string GdiplusFontFamilyNotFound = "GdiplusFontFamilyNotFound";

		// Token: 0x040005B5 RID: 1461
		internal const string GdiplusFontStyleNotFound = "GdiplusFontStyleNotFound";

		// Token: 0x040005B6 RID: 1462
		internal const string GdiplusGenericError = "GdiplusGenericError";

		// Token: 0x040005B7 RID: 1463
		internal const string GdiplusInsufficientBuffer = "GdiplusInsufficientBuffer";

		// Token: 0x040005B8 RID: 1464
		internal const string GdiplusInvalidParameter = "GdiplusInvalidParameter";

		// Token: 0x040005B9 RID: 1465
		internal const string GdiplusInvalidRectangle = "GdiplusInvalidRectangle";

		// Token: 0x040005BA RID: 1466
		internal const string GdiplusInvalidSize = "GdiplusInvalidSize";

		// Token: 0x040005BB RID: 1467
		internal const string GdiplusOutOfMemory = "GdiplusOutOfMemory";

		// Token: 0x040005BC RID: 1468
		internal const string GdiplusNotImplemented = "GdiplusNotImplemented";

		// Token: 0x040005BD RID: 1469
		internal const string GdiplusNotInitialized = "GdiplusNotInitialized";

		// Token: 0x040005BE RID: 1470
		internal const string GdiplusNotTrueTypeFont = "GdiplusNotTrueTypeFont";

		// Token: 0x040005BF RID: 1471
		internal const string GdiplusNotTrueTypeFont_NoName = "GdiplusNotTrueTypeFont_NoName";

		// Token: 0x040005C0 RID: 1472
		internal const string GdiplusObjectBusy = "GdiplusObjectBusy";

		// Token: 0x040005C1 RID: 1473
		internal const string GdiplusOverflow = "GdiplusOverflow";

		// Token: 0x040005C2 RID: 1474
		internal const string GdiplusPropertyNotFoundError = "GdiplusPropertyNotFoundError";

		// Token: 0x040005C3 RID: 1475
		internal const string GdiplusPropertyNotSupportedError = "GdiplusPropertyNotSupportedError";

		// Token: 0x040005C4 RID: 1476
		internal const string GdiplusUnknown = "GdiplusUnknown";

		// Token: 0x040005C5 RID: 1477
		internal const string GdiplusUnknownImageFormat = "GdiplusUnknownImageFormat";

		// Token: 0x040005C6 RID: 1478
		internal const string GdiplusUnsupportedGdiplusVersion = "GdiplusUnsupportedGdiplusVersion";

		// Token: 0x040005C7 RID: 1479
		internal const string GdiplusWrongState = "GdiplusWrongState";

		// Token: 0x040005C8 RID: 1480
		internal const string GlobalAssemblyCache = "GlobalAssemblyCache";

		// Token: 0x040005C9 RID: 1481
		internal const string GraphicsBufferCurrentlyBusy = "GraphicsBufferCurrentlyBusy";

		// Token: 0x040005CA RID: 1482
		internal const string GraphicsBufferQueryFail = "GraphicsBufferQueryFail";

		// Token: 0x040005CB RID: 1483
		internal const string ToolboxItemLocked = "ToolboxItemLocked";

		// Token: 0x040005CC RID: 1484
		internal const string ToolboxItemInvalidPropertyType = "ToolboxItemInvalidPropertyType";

		// Token: 0x040005CD RID: 1485
		internal const string ToolboxItemValueNotSerializable = "ToolboxItemValueNotSerializable";

		// Token: 0x040005CE RID: 1486
		internal const string ToolboxItemInvalidKey = "ToolboxItemInvalidKey";

		// Token: 0x040005CF RID: 1487
		internal const string IllegalState = "IllegalState";

		// Token: 0x040005D0 RID: 1488
		internal const string InterpolationColorsColorBlendNotSet = "InterpolationColorsColorBlendNotSet";

		// Token: 0x040005D1 RID: 1489
		internal const string InterpolationColorsCommon = "InterpolationColorsCommon";

		// Token: 0x040005D2 RID: 1490
		internal const string InterpolationColorsInvalidColorBlendObject = "InterpolationColorsInvalidColorBlendObject";

		// Token: 0x040005D3 RID: 1491
		internal const string InterpolationColorsInvalidStartPosition = "InterpolationColorsInvalidStartPosition";

		// Token: 0x040005D4 RID: 1492
		internal const string InterpolationColorsInvalidEndPosition = "InterpolationColorsInvalidEndPosition";

		// Token: 0x040005D5 RID: 1493
		internal const string InterpolationColorsLength = "InterpolationColorsLength";

		// Token: 0x040005D6 RID: 1494
		internal const string InterpolationColorsLengthsDiffer = "InterpolationColorsLengthsDiffer";

		// Token: 0x040005D7 RID: 1495
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x040005D8 RID: 1496
		internal const string InvalidBoundArgument = "InvalidBoundArgument";

		// Token: 0x040005D9 RID: 1497
		internal const string InvalidClassName = "InvalidClassName";

		// Token: 0x040005DA RID: 1498
		internal const string InvalidColor = "InvalidColor";

		// Token: 0x040005DB RID: 1499
		internal const string InvalidDashPattern = "InvalidDashPattern";

		// Token: 0x040005DC RID: 1500
		internal const string InvalidEx2BoundArgument = "InvalidEx2BoundArgument";

		// Token: 0x040005DD RID: 1501
		internal const string InvalidFrame = "InvalidFrame";

		// Token: 0x040005DE RID: 1502
		internal const string InvalidGDIHandle = "InvalidGDIHandle";

		// Token: 0x040005DF RID: 1503
		internal const string InvalidImage = "InvalidImage";

		// Token: 0x040005E0 RID: 1504
		internal const string InvalidLowBoundArgumentEx = "InvalidLowBoundArgumentEx";

		// Token: 0x040005E1 RID: 1505
		internal const string InvalidPermissionLevel = "InvalidPermissionLevel";

		// Token: 0x040005E2 RID: 1506
		internal const string InvalidPermissionState = "InvalidPermissionState";

		// Token: 0x040005E3 RID: 1507
		internal const string InvalidPictureType = "InvalidPictureType";

		// Token: 0x040005E4 RID: 1508
		internal const string InvalidPrinterException_InvalidPrinter = "InvalidPrinterException_InvalidPrinter";

		// Token: 0x040005E5 RID: 1509
		internal const string InvalidPrinterException_NoDefaultPrinter = "InvalidPrinterException_NoDefaultPrinter";

		// Token: 0x040005E6 RID: 1510
		internal const string InvalidPrinterHandle = "InvalidPrinterHandle";

		// Token: 0x040005E7 RID: 1511
		internal const string ValidRangeX = "ValidRangeX";

		// Token: 0x040005E8 RID: 1512
		internal const string ValidRangeY = "ValidRangeY";

		// Token: 0x040005E9 RID: 1513
		internal const string NativeHandle0 = "NativeHandle0";

		// Token: 0x040005EA RID: 1514
		internal const string NoDefaultPrinter = "NoDefaultPrinter";

		// Token: 0x040005EB RID: 1515
		internal const string NotImplemented = "NotImplemented";

		// Token: 0x040005EC RID: 1516
		internal const string PDOCbeginPrintDescr = "PDOCbeginPrintDescr";

		// Token: 0x040005ED RID: 1517
		internal const string PDOCdocumentNameDescr = "PDOCdocumentNameDescr";

		// Token: 0x040005EE RID: 1518
		internal const string PDOCdocumentPageSettingsDescr = "PDOCdocumentPageSettingsDescr";

		// Token: 0x040005EF RID: 1519
		internal const string PDOCendPrintDescr = "PDOCendPrintDescr";

		// Token: 0x040005F0 RID: 1520
		internal const string PDOCoriginAtMarginsDescr = "PDOCoriginAtMarginsDescr";

		// Token: 0x040005F1 RID: 1521
		internal const string PDOCprintControllerDescr = "PDOCprintControllerDescr";

		// Token: 0x040005F2 RID: 1522
		internal const string PDOCprintPageDescr = "PDOCprintPageDescr";

		// Token: 0x040005F3 RID: 1523
		internal const string PDOCprinterSettingsDescr = "PDOCprinterSettingsDescr";

		// Token: 0x040005F4 RID: 1524
		internal const string PDOCqueryPageSettingsDescr = "PDOCqueryPageSettingsDescr";

		// Token: 0x040005F5 RID: 1525
		internal const string PrintDocumentDesc = "PrintDocumentDesc";

		// Token: 0x040005F6 RID: 1526
		internal const string PrintingPermissionBadXml = "PrintingPermissionBadXml";

		// Token: 0x040005F7 RID: 1527
		internal const string PrintingPermissionAttributeInvalidPermissionLevel = "PrintingPermissionAttributeInvalidPermissionLevel";

		// Token: 0x040005F8 RID: 1528
		internal const string PropertyValueInvalidEntry = "PropertyValueInvalidEntry";

		// Token: 0x040005F9 RID: 1529
		internal const string PSizeNotCustom = "PSizeNotCustom";

		// Token: 0x040005FA RID: 1530
		internal const string ResourceNotFound = "ResourceNotFound";

		// Token: 0x040005FB RID: 1531
		internal const string TargetNotPrintingPermission = "TargetNotPrintingPermission";

		// Token: 0x040005FC RID: 1532
		internal const string TextParseFailedFormat = "TextParseFailedFormat";

		// Token: 0x040005FD RID: 1533
		internal const string TriStateCompareError = "TriStateCompareError";

		// Token: 0x040005FE RID: 1534
		internal const string toStringIcon = "toStringIcon";

		// Token: 0x040005FF RID: 1535
		internal const string toStringNone = "toStringNone";

		// Token: 0x04000600 RID: 1536
		internal const string DCTypeInvalid = "DCTypeInvalid";

		// Token: 0x04000601 RID: 1537
		private static SR loader;

		// Token: 0x04000602 RID: 1538
		private ResourceManager resources;
	}
}

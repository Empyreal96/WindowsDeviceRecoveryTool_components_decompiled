using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x02000445 RID: 1093
	internal sealed class SR
	{
		// Token: 0x06004CA7 RID: 19623 RVA: 0x0013AA10 File Offset: 0x00138C10
		internal SR()
		{
			this.resources = new ResourceManager("System.Windows.Forms", base.GetType().Assembly);
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x0013AA34 File Offset: 0x00138C34
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				SR value = new SR();
				Interlocked.CompareExchange<SR>(ref SR.loader, value, null);
			}
			return SR.loader;
		}

		// Token: 0x170012D5 RID: 4821
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x0000DE5C File Offset: 0x0000C05C
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x0013AA60 File Offset: 0x00138C60
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x0013AA6C File Offset: 0x00138C6C
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

		// Token: 0x06004CAC RID: 19628 RVA: 0x0013AAEC File Offset: 0x00138CEC
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x0013AB15 File Offset: 0x00138D15
		public static string GetString(string name, out bool usedFallback)
		{
			usedFallback = false;
			return SR.GetString(name);
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x0013AB20 File Offset: 0x00138D20
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x040027EE RID: 10222
		internal const string AboutBoxDesc = "AboutBoxDesc";

		// Token: 0x040027EF RID: 10223
		internal const string AccDGCollapse = "AccDGCollapse";

		// Token: 0x040027F0 RID: 10224
		internal const string AccDGEdit = "AccDGEdit";

		// Token: 0x040027F1 RID: 10225
		internal const string AccDGExpand = "AccDGExpand";

		// Token: 0x040027F2 RID: 10226
		internal const string AccDGNavigate = "AccDGNavigate";

		// Token: 0x040027F3 RID: 10227
		internal const string AccDGNavigateBack = "AccDGNavigateBack";

		// Token: 0x040027F4 RID: 10228
		internal const string AccDGNewRow = "AccDGNewRow";

		// Token: 0x040027F5 RID: 10229
		internal const string AccDGParentRow = "AccDGParentRow";

		// Token: 0x040027F6 RID: 10230
		internal const string AccDGParentRows = "AccDGParentRows";

		// Token: 0x040027F7 RID: 10231
		internal const string AccessibleActionCheck = "AccessibleActionCheck";

		// Token: 0x040027F8 RID: 10232
		internal const string AccessibleActionClick = "AccessibleActionClick";

		// Token: 0x040027F9 RID: 10233
		internal const string AccessibleActionCollapse = "AccessibleActionCollapse";

		// Token: 0x040027FA RID: 10234
		internal const string AccessibleActionExpand = "AccessibleActionExpand";

		// Token: 0x040027FB RID: 10235
		internal const string AccessibleActionPress = "AccessibleActionPress";

		// Token: 0x040027FC RID: 10236
		internal const string AccessibleActionUncheck = "AccessibleActionUncheck";

		// Token: 0x040027FD RID: 10237
		internal const string AddDifferentThreads = "AddDifferentThreads";

		// Token: 0x040027FE RID: 10238
		internal const string ApplicationCannotChangeThreadExceptionMode = "ApplicationCannotChangeThreadExceptionMode";

		// Token: 0x040027FF RID: 10239
		internal const string ApplicationCannotChangeApplicationExceptionMode = "ApplicationCannotChangeApplicationExceptionMode";

		// Token: 0x04002800 RID: 10240
		internal const string ApplyCaption = "ApplyCaption";

		// Token: 0x04002801 RID: 10241
		internal const string ArraysNotSameSize = "ArraysNotSameSize";

		// Token: 0x04002802 RID: 10242
		internal const string AutoCompleteFailure = "AutoCompleteFailure";

		// Token: 0x04002803 RID: 10243
		internal const string AutoCompleteFailureListItems = "AutoCompleteFailureListItems";

		// Token: 0x04002804 RID: 10244
		internal const string AXAddInvalidEvent = "AXAddInvalidEvent";

		// Token: 0x04002805 RID: 10245
		internal const string AXDuplicateControl = "AXDuplicateControl";

		// Token: 0x04002806 RID: 10246
		internal const string AXEditProperties = "AXEditProperties";

		// Token: 0x04002807 RID: 10247
		internal const string AXFontUnitNotPoint = "AXFontUnitNotPoint";

		// Token: 0x04002808 RID: 10248
		internal const string AxInterfaceNotSupported = "AxInterfaceNotSupported";

		// Token: 0x04002809 RID: 10249
		internal const string AXInvalidArgument = "AXInvalidArgument";

		// Token: 0x0400280A RID: 10250
		internal const string AXInvalidMethodInvoke = "AXInvalidMethodInvoke";

		// Token: 0x0400280B RID: 10251
		internal const string AXInvalidPropertyGet = "AXInvalidPropertyGet";

		// Token: 0x0400280C RID: 10252
		internal const string AXInvalidPropertySet = "AXInvalidPropertySet";

		// Token: 0x0400280D RID: 10253
		internal const string AXMTAThread = "AXMTAThread";

		// Token: 0x0400280E RID: 10254
		internal const string AXNoConnectionPoint = "AXNoConnectionPoint";

		// Token: 0x0400280F RID: 10255
		internal const string AXNoConnectionPointContainer = "AXNoConnectionPointContainer";

		// Token: 0x04002810 RID: 10256
		internal const string AXNoEventInterface = "AXNoEventInterface";

		// Token: 0x04002811 RID: 10257
		internal const string AXNohWnd = "AXNohWnd";

		// Token: 0x04002812 RID: 10258
		internal const string AXNoLicenseToUse = "AXNoLicenseToUse";

		// Token: 0x04002813 RID: 10259
		internal const string AXNoSinkAdvise = "AXNoSinkAdvise";

		// Token: 0x04002814 RID: 10260
		internal const string AXNoSinkImplementation = "AXNoSinkImplementation";

		// Token: 0x04002815 RID: 10261
		internal const string AXNoThreadInfo = "AXNoThreadInfo";

		// Token: 0x04002816 RID: 10262
		internal const string AXNotImplemented = "AXNotImplemented";

		// Token: 0x04002817 RID: 10263
		internal const string AXNoTopLevelContainerControl = "AXNoTopLevelContainerControl";

		// Token: 0x04002818 RID: 10264
		internal const string AXOcxStateLoaded = "AXOcxStateLoaded";

		// Token: 0x04002819 RID: 10265
		internal const string AXProperties = "AXProperties";

		// Token: 0x0400281A RID: 10266
		internal const string AXSingleThreaded = "AXSingleThreaded";

		// Token: 0x0400281B RID: 10267
		internal const string AXTopLevelSource = "AXTopLevelSource";

		// Token: 0x0400281C RID: 10268
		internal const string AXUnknownError = "AXUnknownError";

		// Token: 0x0400281D RID: 10269
		internal const string AXUnknownImage = "AXUnknownImage";

		// Token: 0x0400281E RID: 10270
		internal const string AXWindowlessControl = "AXWindowlessControl";

		// Token: 0x0400281F RID: 10271
		internal const string BadDataSourceForComplexBinding = "BadDataSourceForComplexBinding";

		// Token: 0x04002820 RID: 10272
		internal const string BindingManagerBadIndex = "BindingManagerBadIndex";

		// Token: 0x04002821 RID: 10273
		internal const string BindingNavigatorAddNewItemPropDescr = "BindingNavigatorAddNewItemPropDescr";

		// Token: 0x04002822 RID: 10274
		internal const string BindingNavigatorAddNewItemText = "BindingNavigatorAddNewItemText";

		// Token: 0x04002823 RID: 10275
		internal const string BindingNavigatorBindingSourcePropDescr = "BindingNavigatorBindingSourcePropDescr";

		// Token: 0x04002824 RID: 10276
		internal const string BindingNavigatorCountItemFormat = "BindingNavigatorCountItemFormat";

		// Token: 0x04002825 RID: 10277
		internal const string BindingNavigatorCountItemFormatPropDescr = "BindingNavigatorCountItemFormatPropDescr";

		// Token: 0x04002826 RID: 10278
		internal const string BindingNavigatorCountItemPropDescr = "BindingNavigatorCountItemPropDescr";

		// Token: 0x04002827 RID: 10279
		internal const string BindingNavigatorCountItemTip = "BindingNavigatorCountItemTip";

		// Token: 0x04002828 RID: 10280
		internal const string BindingNavigatorDeleteItemPropDescr = "BindingNavigatorDeleteItemPropDescr";

		// Token: 0x04002829 RID: 10281
		internal const string BindingNavigatorDeleteItemText = "BindingNavigatorDeleteItemText";

		// Token: 0x0400282A RID: 10282
		internal const string BindingNavigatorMoveFirstItemPropDescr = "BindingNavigatorMoveFirstItemPropDescr";

		// Token: 0x0400282B RID: 10283
		internal const string BindingNavigatorMoveFirstItemText = "BindingNavigatorMoveFirstItemText";

		// Token: 0x0400282C RID: 10284
		internal const string BindingNavigatorMoveLastItemPropDescr = "BindingNavigatorMoveLastItemPropDescr";

		// Token: 0x0400282D RID: 10285
		internal const string BindingNavigatorMoveLastItemText = "BindingNavigatorMoveLastItemText";

		// Token: 0x0400282E RID: 10286
		internal const string BindingNavigatorMoveNextItemPropDescr = "BindingNavigatorMoveNextItemPropDescr";

		// Token: 0x0400282F RID: 10287
		internal const string BindingNavigatorMoveNextItemText = "BindingNavigatorMoveNextItemText";

		// Token: 0x04002830 RID: 10288
		internal const string BindingNavigatorMovePreviousItemPropDescr = "BindingNavigatorMovePreviousItemPropDescr";

		// Token: 0x04002831 RID: 10289
		internal const string BindingNavigatorMovePreviousItemText = "BindingNavigatorMovePreviousItemText";

		// Token: 0x04002832 RID: 10290
		internal const string BindingNavigatorPositionAccessibleName = "BindingNavigatorPositionAccessibleName";

		// Token: 0x04002833 RID: 10291
		internal const string BindingNavigatorPositionItemPropDescr = "BindingNavigatorPositionItemPropDescr";

		// Token: 0x04002834 RID: 10292
		internal const string BindingNavigatorPositionItemTip = "BindingNavigatorPositionItemTip";

		// Token: 0x04002835 RID: 10293
		internal const string BindingNavigatorRefreshItemsEventDescr = "BindingNavigatorRefreshItemsEventDescr";

		// Token: 0x04002836 RID: 10294
		internal const string BindingNavigatorToolStripName = "BindingNavigatorToolStripName";

		// Token: 0x04002837 RID: 10295
		internal const string BindingsCollectionAdd1 = "BindingsCollectionAdd1";

		// Token: 0x04002838 RID: 10296
		internal const string BindingsCollectionAdd2 = "BindingsCollectionAdd2";

		// Token: 0x04002839 RID: 10297
		internal const string BindingsCollectionBadIndex = "BindingsCollectionBadIndex";

		// Token: 0x0400283A RID: 10298
		internal const string BindingsCollectionDup = "BindingsCollectionDup";

		// Token: 0x0400283B RID: 10299
		internal const string BindingsCollectionForeign = "BindingsCollectionForeign";

		// Token: 0x0400283C RID: 10300
		internal const string BindingSourceAddingNewEventHandlerDescr = "BindingSourceAddingNewEventHandlerDescr";

		// Token: 0x0400283D RID: 10301
		internal const string BindingSourceAllowNewDescr = "BindingSourceAllowNewDescr";

		// Token: 0x0400283E RID: 10302
		internal const string BindingSourceBadSortString = "BindingSourceBadSortString";

		// Token: 0x0400283F RID: 10303
		internal const string BindingSourceBindingCompleteEventHandlerDescr = "BindingSourceBindingCompleteEventHandlerDescr";

		// Token: 0x04002840 RID: 10304
		internal const string BindingSourceBindingListWrapperAddToReadOnlyList = "BindingSourceBindingListWrapperAddToReadOnlyList";

		// Token: 0x04002841 RID: 10305
		internal const string BindingSourceBindingListWrapperNeedAParameterlessConstructor = "BindingSourceBindingListWrapperNeedAParameterlessConstructor";

		// Token: 0x04002842 RID: 10306
		internal const string BindingSourceBindingListWrapperNeedToSetAllowNew = "BindingSourceBindingListWrapperNeedToSetAllowNew";

		// Token: 0x04002843 RID: 10307
		internal const string BindingSourceCurrentChangedEventHandlerDescr = "BindingSourceCurrentChangedEventHandlerDescr";

		// Token: 0x04002844 RID: 10308
		internal const string BindingSourceCurrentItemChangedEventHandlerDescr = "BindingSourceCurrentItemChangedEventHandlerDescr";

		// Token: 0x04002845 RID: 10309
		internal const string BindingSourceDataErrorEventHandlerDescr = "BindingSourceDataErrorEventHandlerDescr";

		// Token: 0x04002846 RID: 10310
		internal const string BindingSourceDataMemberChangedEventHandlerDescr = "BindingSourceDataMemberChangedEventHandlerDescr";

		// Token: 0x04002847 RID: 10311
		internal const string BindingSourceDataMemberDescr = "BindingSourceDataMemberDescr";

		// Token: 0x04002848 RID: 10312
		internal const string BindingSourceDataSourceChangedEventHandlerDescr = "BindingSourceDataSourceChangedEventHandlerDescr";

		// Token: 0x04002849 RID: 10313
		internal const string BindingSourceDataSourceDescr = "BindingSourceDataSourceDescr";

		// Token: 0x0400284A RID: 10314
		internal const string BindingSourceFilterDescr = "BindingSourceFilterDescr";

		// Token: 0x0400284B RID: 10315
		internal const string BindingSourceInstanceError = "BindingSourceInstanceError";

		// Token: 0x0400284C RID: 10316
		internal const string BindingSourceItemChangedEventModeDescr = "BindingSourceItemChangedEventModeDescr";

		// Token: 0x0400284D RID: 10317
		internal const string BindingSourceItemTypeIsValueType = "BindingSourceItemTypeIsValueType";

		// Token: 0x0400284E RID: 10318
		internal const string BindingSourceItemTypeMismatchOnAdd = "BindingSourceItemTypeMismatchOnAdd";

		// Token: 0x0400284F RID: 10319
		internal const string BindingSourceListChangedEventHandlerDescr = "BindingSourceListChangedEventHandlerDescr";

		// Token: 0x04002850 RID: 10320
		internal const string BindingSourcePositionChangedEventHandlerDescr = "BindingSourcePositionChangedEventHandlerDescr";

		// Token: 0x04002851 RID: 10321
		internal const string BindingSourceRecursionDetected = "BindingSourceRecursionDetected";

		// Token: 0x04002852 RID: 10322
		internal const string BindingSourceRemoveCurrentNoCurrentItem = "BindingSourceRemoveCurrentNoCurrentItem";

		// Token: 0x04002853 RID: 10323
		internal const string BindingSourceRemoveCurrentNotAllowed = "BindingSourceRemoveCurrentNotAllowed";

		// Token: 0x04002854 RID: 10324
		internal const string BindingSourceSortDescr = "BindingSourceSortDescr";

		// Token: 0x04002855 RID: 10325
		internal const string BindingSourceSortStringPropertyNotInIBindingList = "BindingSourceSortStringPropertyNotInIBindingList";

		// Token: 0x04002856 RID: 10326
		internal const string BlinkRateMustBeZeroOrMore = "BlinkRateMustBeZeroOrMore";

		// Token: 0x04002857 RID: 10327
		internal const string borderStyleDescr = "borderStyleDescr";

		// Token: 0x04002858 RID: 10328
		internal const string ButtonAutoEllipsisDescr = "ButtonAutoEllipsisDescr";

		// Token: 0x04002859 RID: 10329
		internal const string ButtonBorderColorDescr = "ButtonBorderColorDescr";

		// Token: 0x0400285A RID: 10330
		internal const string ButtonBorderSizeDescr = "ButtonBorderSizeDescr";

		// Token: 0x0400285B RID: 10331
		internal const string ButtonCheckedBackColorDescr = "ButtonCheckedBackColorDescr";

		// Token: 0x0400285C RID: 10332
		internal const string ButtonDialogResultDescr = "ButtonDialogResultDescr";

		// Token: 0x0400285D RID: 10333
		internal const string ButtonFlatAppearance = "ButtonFlatAppearance";

		// Token: 0x0400285E RID: 10334
		internal const string ButtonFlatAppearanceInvalidBorderColor = "ButtonFlatAppearanceInvalidBorderColor";

		// Token: 0x0400285F RID: 10335
		internal const string ButtonFlatStyleDescr = "ButtonFlatStyleDescr";

		// Token: 0x04002860 RID: 10336
		internal const string ButtonImageAlignDescr = "ButtonImageAlignDescr";

		// Token: 0x04002861 RID: 10337
		internal const string ButtonImageDescr = "ButtonImageDescr";

		// Token: 0x04002862 RID: 10338
		internal const string ButtonImageIndexDescr = "ButtonImageIndexDescr";

		// Token: 0x04002863 RID: 10339
		internal const string ButtonImageListDescr = "ButtonImageListDescr";

		// Token: 0x04002864 RID: 10340
		internal const string ButtonMouseDownBackColorDescr = "ButtonMouseDownBackColorDescr";

		// Token: 0x04002865 RID: 10341
		internal const string ButtonMouseOverBackColorDescr = "ButtonMouseOverBackColorDescr";

		// Token: 0x04002866 RID: 10342
		internal const string ButtonTextAlignDescr = "ButtonTextAlignDescr";

		// Token: 0x04002867 RID: 10343
		internal const string ButtonTextImageRelationDescr = "ButtonTextImageRelationDescr";

		// Token: 0x04002868 RID: 10344
		internal const string ButtonUseMnemonicDescr = "ButtonUseMnemonicDescr";

		// Token: 0x04002869 RID: 10345
		internal const string ButtonUseVisualStyleBackColorDescr = "ButtonUseVisualStyleBackColorDescr";

		// Token: 0x0400286A RID: 10346
		internal const string CancelCaption = "CancelCaption";

		// Token: 0x0400286B RID: 10347
		internal const string CannotActivateControl = "CannotActivateControl";

		// Token: 0x0400286C RID: 10348
		internal const string CannotChangePrintedDocument = "CannotChangePrintedDocument";

		// Token: 0x0400286D RID: 10349
		internal const string CannotConvertDoubleToDate = "CannotConvertDoubleToDate";

		// Token: 0x0400286E RID: 10350
		internal const string CannotConvertIntToFloat = "CannotConvertIntToFloat";

		// Token: 0x0400286F RID: 10351
		internal const string CantNestMessageLoops = "CantNestMessageLoops";

		// Token: 0x04002870 RID: 10352
		internal const string CantShowMBServiceWithHelp = "CantShowMBServiceWithHelp";

		// Token: 0x04002871 RID: 10353
		internal const string CantShowMBServiceWithOwner = "CantShowMBServiceWithOwner";

		// Token: 0x04002872 RID: 10354
		internal const string CantShowModalOnNonInteractive = "CantShowModalOnNonInteractive";

		// Token: 0x04002873 RID: 10355
		internal const string CatAccessibility = "CatAccessibility";

		// Token: 0x04002874 RID: 10356
		internal const string CatAction = "CatAction";

		// Token: 0x04002875 RID: 10357
		internal const string CatAppearance = "CatAppearance";

		// Token: 0x04002876 RID: 10358
		internal const string CatAsynchronous = "CatAsynchronous";

		// Token: 0x04002877 RID: 10359
		internal const string CatBehavior = "CatBehavior";

		// Token: 0x04002878 RID: 10360
		internal const string CatColors = "CatColors";

		// Token: 0x04002879 RID: 10361
		internal const string CatData = "CatData";

		// Token: 0x0400287A RID: 10362
		internal const string CatDisplay = "CatDisplay";

		// Token: 0x0400287B RID: 10363
		internal const string CatDragDrop = "CatDragDrop";

		// Token: 0x0400287C RID: 10364
		internal const string CatFocus = "CatFocus";

		// Token: 0x0400287D RID: 10365
		internal const string CatFolderBrowsing = "CatFolderBrowsing";

		// Token: 0x0400287E RID: 10366
		internal const string CatItems = "CatItems";

		// Token: 0x0400287F RID: 10367
		internal const string CatKey = "CatKey";

		// Token: 0x04002880 RID: 10368
		internal const string CatLayout = "CatLayout";

		// Token: 0x04002881 RID: 10369
		internal const string CatMouse = "CatMouse";

		// Token: 0x04002882 RID: 10370
		internal const string CatPrivate = "CatPrivate";

		// Token: 0x04002883 RID: 10371
		internal const string CatPropertyChanged = "CatPropertyChanged";

		// Token: 0x04002884 RID: 10372
		internal const string CatWindowStyle = "CatWindowStyle";

		// Token: 0x04002885 RID: 10373
		internal const string CDallowFullOpenDescr = "CDallowFullOpenDescr";

		// Token: 0x04002886 RID: 10374
		internal const string CDanyColorDescr = "CDanyColorDescr";

		// Token: 0x04002887 RID: 10375
		internal const string CDcolorDescr = "CDcolorDescr";

		// Token: 0x04002888 RID: 10376
		internal const string CDcustomColorsDescr = "CDcustomColorsDescr";

		// Token: 0x04002889 RID: 10377
		internal const string CDfullOpenDescr = "CDfullOpenDescr";

		// Token: 0x0400288A RID: 10378
		internal const string CDshowHelpDescr = "CDshowHelpDescr";

		// Token: 0x0400288B RID: 10379
		internal const string CDsolidColorOnlyDescr = "CDsolidColorOnlyDescr";

		// Token: 0x0400288C RID: 10380
		internal const string CheckBoxAppearanceDescr = "CheckBoxAppearanceDescr";

		// Token: 0x0400288D RID: 10381
		internal const string CheckBoxAutoCheckDescr = "CheckBoxAutoCheckDescr";

		// Token: 0x0400288E RID: 10382
		internal const string CheckBoxCheckAlignDescr = "CheckBoxCheckAlignDescr";

		// Token: 0x0400288F RID: 10383
		internal const string CheckBoxCheckedDescr = "CheckBoxCheckedDescr";

		// Token: 0x04002890 RID: 10384
		internal const string CheckBoxCheckStateDescr = "CheckBoxCheckStateDescr";

		// Token: 0x04002891 RID: 10385
		internal const string CheckBoxOnAppearanceChangedDescr = "CheckBoxOnAppearanceChangedDescr";

		// Token: 0x04002892 RID: 10386
		internal const string CheckBoxOnCheckedChangedDescr = "CheckBoxOnCheckedChangedDescr";

		// Token: 0x04002893 RID: 10387
		internal const string CheckBoxOnCheckStateChangedDescr = "CheckBoxOnCheckStateChangedDescr";

		// Token: 0x04002894 RID: 10388
		internal const string CheckBoxThreeStateDescr = "CheckBoxThreeStateDescr";

		// Token: 0x04002895 RID: 10389
		internal const string CheckedListBoxCheckedIndexCollectionIsReadOnly = "CheckedListBoxCheckedIndexCollectionIsReadOnly";

		// Token: 0x04002896 RID: 10390
		internal const string CheckedListBoxCheckedItemCollectionIsReadOnly = "CheckedListBoxCheckedItemCollectionIsReadOnly";

		// Token: 0x04002897 RID: 10391
		internal const string CheckedListBoxCheckOnClickDescr = "CheckedListBoxCheckOnClickDescr";

		// Token: 0x04002898 RID: 10392
		internal const string CheckedListBoxInvalidSelectionMode = "CheckedListBoxInvalidSelectionMode";

		// Token: 0x04002899 RID: 10393
		internal const string CheckedListBoxItemCheckDescr = "CheckedListBoxItemCheckDescr";

		// Token: 0x0400289A RID: 10394
		internal const string CheckedListBoxThreeDCheckBoxesDescr = "CheckedListBoxThreeDCheckBoxesDescr";

		// Token: 0x0400289B RID: 10395
		internal const string CircularOwner = "CircularOwner";

		// Token: 0x0400289C RID: 10396
		internal const string Clipboard_InvalidPath = "Clipboard_InvalidPath";

		// Token: 0x0400289D RID: 10397
		internal const string ClipboardOperationFailed = "ClipboardOperationFailed";

		// Token: 0x0400289E RID: 10398
		internal const string ClipboardSecurityException = "ClipboardSecurityException";

		// Token: 0x0400289F RID: 10399
		internal const string CloseCaption = "CloseCaption";

		// Token: 0x040028A0 RID: 10400
		internal const string ClosingWhileCreatingHandle = "ClosingWhileCreatingHandle";

		// Token: 0x040028A1 RID: 10401
		internal const string collectionChangedEventDescr = "collectionChangedEventDescr";

		// Token: 0x040028A2 RID: 10402
		internal const string collectionChangingEventDescr = "collectionChangingEventDescr";

		// Token: 0x040028A3 RID: 10403
		internal const string CollectionEmptyException = "CollectionEmptyException";

		// Token: 0x040028A4 RID: 10404
		internal const string ColumnAlignment = "ColumnAlignment";

		// Token: 0x040028A5 RID: 10405
		internal const string ColumnCaption = "ColumnCaption";

		// Token: 0x040028A6 RID: 10406
		internal const string ColumnHeaderBadDisplayIndex = "ColumnHeaderBadDisplayIndex";

		// Token: 0x040028A7 RID: 10407
		internal const string ColumnHeaderCollectionInvalidArgument = "ColumnHeaderCollectionInvalidArgument";

		// Token: 0x040028A8 RID: 10408
		internal const string ColumnHeaderDisplayIndexDescr = "ColumnHeaderDisplayIndexDescr";

		// Token: 0x040028A9 RID: 10409
		internal const string ColumnHeaderNameDescr = "ColumnHeaderNameDescr";

		// Token: 0x040028AA RID: 10410
		internal const string ColumnWidth = "ColumnWidth";

		// Token: 0x040028AB RID: 10411
		internal const string COM2BadHandlerType = "COM2BadHandlerType";

		// Token: 0x040028AC RID: 10412
		internal const string COM2NamesAndValuesNotEqual = "COM2NamesAndValuesNotEqual";

		// Token: 0x040028AD RID: 10413
		internal const string COM2ReadonlyProperty = "COM2ReadonlyProperty";

		// Token: 0x040028AE RID: 10414
		internal const string COM2UnhandledVT = "COM2UnhandledVT";

		// Token: 0x040028AF RID: 10415
		internal const string ComboBoxAutoCompleteCustomSourceDescr = "ComboBoxAutoCompleteCustomSourceDescr";

		// Token: 0x040028B0 RID: 10416
		internal const string ComboBoxAutoCompleteModeDescr = "ComboBoxAutoCompleteModeDescr";

		// Token: 0x040028B1 RID: 10417
		internal const string ComboBoxAutoCompleteModeOnlyNoneAllowed = "ComboBoxAutoCompleteModeOnlyNoneAllowed";

		// Token: 0x040028B2 RID: 10418
		internal const string ComboBoxAutoCompleteSourceDescr = "ComboBoxAutoCompleteSourceDescr";

		// Token: 0x040028B3 RID: 10419
		internal const string ComboBoxAutoCompleteSourceOnlyListItemsAllowed = "ComboBoxAutoCompleteSourceOnlyListItemsAllowed";

		// Token: 0x040028B4 RID: 10420
		internal const string ComboBoxDataSourceWithSort = "ComboBoxDataSourceWithSort";

		// Token: 0x040028B5 RID: 10421
		internal const string ComboBoxDrawModeDescr = "ComboBoxDrawModeDescr";

		// Token: 0x040028B6 RID: 10422
		internal const string ComboBoxDropDownHeightDescr = "ComboBoxDropDownHeightDescr";

		// Token: 0x040028B7 RID: 10423
		internal const string ComboBoxDropDownStyleChangedDescr = "ComboBoxDropDownStyleChangedDescr";

		// Token: 0x040028B8 RID: 10424
		internal const string ComboBoxDropDownWidthDescr = "ComboBoxDropDownWidthDescr";

		// Token: 0x040028B9 RID: 10425
		internal const string ComboBoxDroppedDownDescr = "ComboBoxDroppedDownDescr";

		// Token: 0x040028BA RID: 10426
		internal const string ComboBoxFlatStyleDescr = "ComboBoxFlatStyleDescr";

		// Token: 0x040028BB RID: 10427
		internal const string ComboBoxIntegralHeightDescr = "ComboBoxIntegralHeightDescr";

		// Token: 0x040028BC RID: 10428
		internal const string ComboBoxItemHeightDescr = "ComboBoxItemHeightDescr";

		// Token: 0x040028BD RID: 10429
		internal const string ComboBoxItemOverflow = "ComboBoxItemOverflow";

		// Token: 0x040028BE RID: 10430
		internal const string ComboBoxItemsDescr = "ComboBoxItemsDescr";

		// Token: 0x040028BF RID: 10431
		internal const string ComboBoxMaxDropDownItemsDescr = "ComboBoxMaxDropDownItemsDescr";

		// Token: 0x040028C0 RID: 10432
		internal const string ComboBoxMaxLengthDescr = "ComboBoxMaxLengthDescr";

		// Token: 0x040028C1 RID: 10433
		internal const string ComboBoxOnDropDownClosedDescr = "ComboBoxOnDropDownClosedDescr";

		// Token: 0x040028C2 RID: 10434
		internal const string ComboBoxOnDropDownDescr = "ComboBoxOnDropDownDescr";

		// Token: 0x040028C3 RID: 10435
		internal const string ComboBoxOnTextUpdateDescr = "ComboBoxOnTextUpdateDescr";

		// Token: 0x040028C4 RID: 10436
		internal const string ComboBoxPreferredHeightDescr = "ComboBoxPreferredHeightDescr";

		// Token: 0x040028C5 RID: 10437
		internal const string ComboBoxSelectedIndexDescr = "ComboBoxSelectedIndexDescr";

		// Token: 0x040028C6 RID: 10438
		internal const string ComboBoxSelectedItemDescr = "ComboBoxSelectedItemDescr";

		// Token: 0x040028C7 RID: 10439
		internal const string ComboBoxSelectedTextDescr = "ComboBoxSelectedTextDescr";

		// Token: 0x040028C8 RID: 10440
		internal const string ComboBoxSelectionLengthDescr = "ComboBoxSelectionLengthDescr";

		// Token: 0x040028C9 RID: 10441
		internal const string ComboBoxSelectionStartDescr = "ComboBoxSelectionStartDescr";

		// Token: 0x040028CA RID: 10442
		internal const string ComboBoxSortedDescr = "ComboBoxSortedDescr";

		// Token: 0x040028CB RID: 10443
		internal const string ComboBoxSortWithDataSource = "ComboBoxSortWithDataSource";

		// Token: 0x040028CC RID: 10444
		internal const string ComboBoxStyleDescr = "ComboBoxStyleDescr";

		// Token: 0x040028CD RID: 10445
		internal const string CommandIdNotAllocated = "CommandIdNotAllocated";

		// Token: 0x040028CE RID: 10446
		internal const string CommonDialogHelpRequested = "CommonDialogHelpRequested";

		// Token: 0x040028CF RID: 10447
		internal const string ComponentEditorFormBadComponent = "ComponentEditorFormBadComponent";

		// Token: 0x040028D0 RID: 10448
		internal const string ComponentEditorFormProperties = "ComponentEditorFormProperties";

		// Token: 0x040028D1 RID: 10449
		internal const string ComponentEditorFormPropertiesNoName = "ComponentEditorFormPropertiesNoName";

		// Token: 0x040028D2 RID: 10450
		internal const string ComponentManagerProxyOutOfMemory = "ComponentManagerProxyOutOfMemory";

		// Token: 0x040028D3 RID: 10451
		internal const string Config_base_unrecognized_attribute = "Config_base_unrecognized_attribute";

		// Token: 0x040028D4 RID: 10452
		internal const string ConnPointAdviseFailed = "ConnPointAdviseFailed";

		// Token: 0x040028D5 RID: 10453
		internal const string ConnPointCouldNotCreate = "ConnPointCouldNotCreate";

		// Token: 0x040028D6 RID: 10454
		internal const string ConnPointSinkIF = "ConnPointSinkIF";

		// Token: 0x040028D7 RID: 10455
		internal const string ConnPointSourceIF = "ConnPointSourceIF";

		// Token: 0x040028D8 RID: 10456
		internal const string ConnPointUnhandledType = "ConnPointUnhandledType";

		// Token: 0x040028D9 RID: 10457
		internal const string ContainerControlActiveControlDescr = "ContainerControlActiveControlDescr";

		// Token: 0x040028DA RID: 10458
		internal const string ContainerControlAutoScaleModeDescr = "ContainerControlAutoScaleModeDescr";

		// Token: 0x040028DB RID: 10459
		internal const string ContainerControlAutoValidate = "ContainerControlAutoValidate";

		// Token: 0x040028DC RID: 10460
		internal const string ContainerControlBindingContextDescr = "ContainerControlBindingContextDescr";

		// Token: 0x040028DD RID: 10461
		internal const string ContainerControlInvalidAutoScaleDimensions = "ContainerControlInvalidAutoScaleDimensions";

		// Token: 0x040028DE RID: 10462
		internal const string ContainerControlOnAutoValidateChangedDescr = "ContainerControlOnAutoValidateChangedDescr";

		// Token: 0x040028DF RID: 10463
		internal const string ContainerControlParentFormDescr = "ContainerControlParentFormDescr";

		// Token: 0x040028E0 RID: 10464
		internal const string ContextMenuCollapseDescr = "ContextMenuCollapseDescr";

		// Token: 0x040028E1 RID: 10465
		internal const string ContextMenuImageListDescr = "ContextMenuImageListDescr";

		// Token: 0x040028E2 RID: 10466
		internal const string ContextMenuInvalidParent = "ContextMenuInvalidParent";

		// Token: 0x040028E3 RID: 10467
		internal const string ContextMenuIsImageMarginPresentDescr = "ContextMenuIsImageMarginPresentDescr";

		// Token: 0x040028E4 RID: 10468
		internal const string ContextMenuSourceControlDescr = "ContextMenuSourceControlDescr";

		// Token: 0x040028E5 RID: 10469
		internal const string ContextMenuStripSourceControlDescr = "ContextMenuStripSourceControlDescr";

		// Token: 0x040028E6 RID: 10470
		internal const string ControlAccessibileObjectInvalid = "ControlAccessibileObjectInvalid";

		// Token: 0x040028E7 RID: 10471
		internal const string ControlAccessibilityObjectDescr = "ControlAccessibilityObjectDescr";

		// Token: 0x040028E8 RID: 10472
		internal const string ControlAccessibleDefaultActionDescr = "ControlAccessibleDefaultActionDescr";

		// Token: 0x040028E9 RID: 10473
		internal const string ControlAccessibleDescriptionDescr = "ControlAccessibleDescriptionDescr";

		// Token: 0x040028EA RID: 10474
		internal const string ControlAccessibleNameDescr = "ControlAccessibleNameDescr";

		// Token: 0x040028EB RID: 10475
		internal const string ControlAccessibleRoleDescr = "ControlAccessibleRoleDescr";

		// Token: 0x040028EC RID: 10476
		internal const string ControlAllowDropDescr = "ControlAllowDropDescr";

		// Token: 0x040028ED RID: 10477
		internal const string ControlAllowTransparencyDescr = "ControlAllowTransparencyDescr";

		// Token: 0x040028EE RID: 10478
		internal const string ControlAnchorDescr = "ControlAnchorDescr";

		// Token: 0x040028EF RID: 10479
		internal const string ControlArrayCannotAddComponentArray = "ControlArrayCannotAddComponentArray";

		// Token: 0x040028F0 RID: 10480
		internal const string ControlArrayCannotPerformAddCopy = "ControlArrayCannotPerformAddCopy";

		// Token: 0x040028F1 RID: 10481
		internal const string ControlArrayCloningException = "ControlArrayCloningException";

		// Token: 0x040028F2 RID: 10482
		internal const string ControlArrayDuplicateException = "ControlArrayDuplicateException";

		// Token: 0x040028F3 RID: 10483
		internal const string ControlArrayValidationException = "ControlArrayValidationException";

		// Token: 0x040028F4 RID: 10484
		internal const string ControlAutoRelocateDescr = "ControlAutoRelocateDescr";

		// Token: 0x040028F5 RID: 10485
		internal const string ControlAutoSizeDescr = "ControlAutoSizeDescr";

		// Token: 0x040028F6 RID: 10486
		internal const string ControlAutoSizeModeDescr = "ControlAutoSizeModeDescr";

		// Token: 0x040028F7 RID: 10487
		internal const string ControlBackColorDescr = "ControlBackColorDescr";

		// Token: 0x040028F8 RID: 10488
		internal const string ControlBackgroundImageDescr = "ControlBackgroundImageDescr";

		// Token: 0x040028F9 RID: 10489
		internal const string ControlBackgroundImageLayoutDescr = "ControlBackgroundImageLayoutDescr";

		// Token: 0x040028FA RID: 10490
		internal const string ControlBadAsyncResult = "ControlBadAsyncResult";

		// Token: 0x040028FB RID: 10491
		internal const string ControlBadControl = "ControlBadControl";

		// Token: 0x040028FC RID: 10492
		internal const string ControlBindingContextDescr = "ControlBindingContextDescr";

		// Token: 0x040028FD RID: 10493
		internal const string ControlBindingsDescr = "ControlBindingsDescr";

		// Token: 0x040028FE RID: 10494
		internal const string ControlBottomDescr = "ControlBottomDescr";

		// Token: 0x040028FF RID: 10495
		internal const string ControlBoundsDescr = "ControlBoundsDescr";

		// Token: 0x04002900 RID: 10496
		internal const string ControlCanFocusDescr = "ControlCanFocusDescr";

		// Token: 0x04002901 RID: 10497
		internal const string ControlCannotBeNull = "ControlCannotBeNull";

		// Token: 0x04002902 RID: 10498
		internal const string ControlCanSelectDescr = "ControlCanSelectDescr";

		// Token: 0x04002903 RID: 10499
		internal const string ControlCaptureDescr = "ControlCaptureDescr";

		// Token: 0x04002904 RID: 10500
		internal const string ControlCausesValidationDescr = "ControlCausesValidationDescr";

		// Token: 0x04002905 RID: 10501
		internal const string ControlCheckForIllegalCrossThreadCalls = "ControlCheckForIllegalCrossThreadCalls";

		// Token: 0x04002906 RID: 10502
		internal const string ControlClientRectangleDescr = "ControlClientRectangleDescr";

		// Token: 0x04002907 RID: 10503
		internal const string ControlClientSizeDescr = "ControlClientSizeDescr";

		// Token: 0x04002908 RID: 10504
		internal const string ControlCompanyNameDescr = "ControlCompanyNameDescr";

		// Token: 0x04002909 RID: 10505
		internal const string ControlContainsFocusDescr = "ControlContainsFocusDescr";

		// Token: 0x0400290A RID: 10506
		internal const string ControlContextMenuDescr = "ControlContextMenuDescr";

		// Token: 0x0400290B RID: 10507
		internal const string ControlContextMenuStripChangedDescr = "ControlContextMenuStripChangedDescr";

		// Token: 0x0400290C RID: 10508
		internal const string ControlControlsDescr = "ControlControlsDescr";

		// Token: 0x0400290D RID: 10509
		internal const string ControlCreatedDescr = "ControlCreatedDescr";

		// Token: 0x0400290E RID: 10510
		internal const string ControlCursorDescr = "ControlCursorDescr";

		// Token: 0x0400290F RID: 10511
		internal const string ControlDisplayRectangleDescr = "ControlDisplayRectangleDescr";

		// Token: 0x04002910 RID: 10512
		internal const string ControlDisposedDescr = "ControlDisposedDescr";

		// Token: 0x04002911 RID: 10513
		internal const string ControlDisposingDescr = "ControlDisposingDescr";

		// Token: 0x04002912 RID: 10514
		internal const string ControlDockDescr = "ControlDockDescr";

		// Token: 0x04002913 RID: 10515
		internal const string ControlDoubleBufferedDescr = "ControlDoubleBufferedDescr";

		// Token: 0x04002914 RID: 10516
		internal const string ControlEnabledDescr = "ControlEnabledDescr";

		// Token: 0x04002915 RID: 10517
		internal const string ControlFocusedDescr = "ControlFocusedDescr";

		// Token: 0x04002916 RID: 10518
		internal const string ControlFontDescr = "ControlFontDescr";

		// Token: 0x04002917 RID: 10519
		internal const string ControlForeColorDescr = "ControlForeColorDescr";

		// Token: 0x04002918 RID: 10520
		internal const string ControlHandleCreatedDescr = "ControlHandleCreatedDescr";

		// Token: 0x04002919 RID: 10521
		internal const string ControlHandleDescr = "ControlHandleDescr";

		// Token: 0x0400291A RID: 10522
		internal const string ControlHasChildrenDescr = "ControlHasChildrenDescr";

		// Token: 0x0400291B RID: 10523
		internal const string ControlHeightDescr = "ControlHeightDescr";

		// Token: 0x0400291C RID: 10524
		internal const string ControlIMEModeDescr = "ControlIMEModeDescr";

		// Token: 0x0400291D RID: 10525
		internal const string ControlInvalidLastScalingFactor = "ControlInvalidLastScalingFactor";

		// Token: 0x0400291E RID: 10526
		internal const string ControlInvokeRequiredDescr = "ControlInvokeRequiredDescr";

		// Token: 0x0400291F RID: 10527
		internal const string ControlIsAccessibleDescr = "ControlIsAccessibleDescr";

		// Token: 0x04002920 RID: 10528
		internal const string ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly = "ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly";

		// Token: 0x04002921 RID: 10529
		internal const string ControlLeftDescr = "ControlLeftDescr";

		// Token: 0x04002922 RID: 10530
		internal const string ControlLocationDescr = "ControlLocationDescr";

		// Token: 0x04002923 RID: 10531
		internal const string ControlMarginDescr = "ControlMarginDescr";

		// Token: 0x04002924 RID: 10532
		internal const string ControlMaximumSizeDescr = "ControlMaximumSizeDescr";

		// Token: 0x04002925 RID: 10533
		internal const string ControlMetaFileDCWrapperSizeInvalid = "ControlMetaFileDCWrapperSizeInvalid";

		// Token: 0x04002926 RID: 10534
		internal const string ControlMinimumSizeDescr = "ControlMinimumSizeDescr";

		// Token: 0x04002927 RID: 10535
		internal const string ControlNotChild = "ControlNotChild";

		// Token: 0x04002928 RID: 10536
		internal const string ControlOnAutoSizeChangedDescr = "ControlOnAutoSizeChangedDescr";

		// Token: 0x04002929 RID: 10537
		internal const string ControlOnBackColorChangedDescr = "ControlOnBackColorChangedDescr";

		// Token: 0x0400292A RID: 10538
		internal const string ControlOnBackgroundImageChangedDescr = "ControlOnBackgroundImageChangedDescr";

		// Token: 0x0400292B RID: 10539
		internal const string ControlOnBackgroundImageLayoutChangedDescr = "ControlOnBackgroundImageLayoutChangedDescr";

		// Token: 0x0400292C RID: 10540
		internal const string ControlOnBindingContextChangedDescr = "ControlOnBindingContextChangedDescr";

		// Token: 0x0400292D RID: 10541
		internal const string ControlOnCausesValidationChangedDescr = "ControlOnCausesValidationChangedDescr";

		// Token: 0x0400292E RID: 10542
		internal const string ControlOnChangeUICuesDescr = "ControlOnChangeUICuesDescr";

		// Token: 0x0400292F RID: 10543
		internal const string ControlOnClickDescr = "ControlOnClickDescr";

		// Token: 0x04002930 RID: 10544
		internal const string ControlOnClientSizeChangedDescr = "ControlOnClientSizeChangedDescr";

		// Token: 0x04002931 RID: 10545
		internal const string ControlOnContextMenuChangedDescr = "ControlOnContextMenuChangedDescr";

		// Token: 0x04002932 RID: 10546
		internal const string ControlOnControlAddedDescr = "ControlOnControlAddedDescr";

		// Token: 0x04002933 RID: 10547
		internal const string ControlOnControlRemovedDescr = "ControlOnControlRemovedDescr";

		// Token: 0x04002934 RID: 10548
		internal const string ControlOnCreateHandleDescr = "ControlOnCreateHandleDescr";

		// Token: 0x04002935 RID: 10549
		internal const string ControlOnCursorChangedDescr = "ControlOnCursorChangedDescr";

		// Token: 0x04002936 RID: 10550
		internal const string ControlOnDestroyHandleDescr = "ControlOnDestroyHandleDescr";

		// Token: 0x04002937 RID: 10551
		internal const string ControlOnDockChangedDescr = "ControlOnDockChangedDescr";

		// Token: 0x04002938 RID: 10552
		internal const string ControlOnDoubleClickDescr = "ControlOnDoubleClickDescr";

		// Token: 0x04002939 RID: 10553
		internal const string ControlOnDragDropDescr = "ControlOnDragDropDescr";

		// Token: 0x0400293A RID: 10554
		internal const string ControlOnDragEnterDescr = "ControlOnDragEnterDescr";

		// Token: 0x0400293B RID: 10555
		internal const string ControlOnDragLeaveDescr = "ControlOnDragLeaveDescr";

		// Token: 0x0400293C RID: 10556
		internal const string ControlOnDragOverDescr = "ControlOnDragOverDescr";

		// Token: 0x0400293D RID: 10557
		internal const string ControlOnEnabledChangedDescr = "ControlOnEnabledChangedDescr";

		// Token: 0x0400293E RID: 10558
		internal const string ControlOnEnterDescr = "ControlOnEnterDescr";

		// Token: 0x0400293F RID: 10559
		internal const string ControlOnFontChangedDescr = "ControlOnFontChangedDescr";

		// Token: 0x04002940 RID: 10560
		internal const string ControlOnForeColorChangedDescr = "ControlOnForeColorChangedDescr";

		// Token: 0x04002941 RID: 10561
		internal const string ControlOnGiveFeedbackDescr = "ControlOnGiveFeedbackDescr";

		// Token: 0x04002942 RID: 10562
		internal const string ControlOnGotFocusDescr = "ControlOnGotFocusDescr";

		// Token: 0x04002943 RID: 10563
		internal const string ControlOnHelpDescr = "ControlOnHelpDescr";

		// Token: 0x04002944 RID: 10564
		internal const string ControlOnImeModeChangedDescr = "ControlOnImeModeChangedDescr";

		// Token: 0x04002945 RID: 10565
		internal const string ControlOnInvalidateDescr = "ControlOnInvalidateDescr";

		// Token: 0x04002946 RID: 10566
		internal const string ControlOnKeyDownDescr = "ControlOnKeyDownDescr";

		// Token: 0x04002947 RID: 10567
		internal const string ControlOnKeyPressDescr = "ControlOnKeyPressDescr";

		// Token: 0x04002948 RID: 10568
		internal const string ControlOnKeyUpDescr = "ControlOnKeyUpDescr";

		// Token: 0x04002949 RID: 10569
		internal const string ControlOnLayoutDescr = "ControlOnLayoutDescr";

		// Token: 0x0400294A RID: 10570
		internal const string ControlOnLeaveDescr = "ControlOnLeaveDescr";

		// Token: 0x0400294B RID: 10571
		internal const string ControlOnLocationChangedDescr = "ControlOnLocationChangedDescr";

		// Token: 0x0400294C RID: 10572
		internal const string ControlOnLostFocusDescr = "ControlOnLostFocusDescr";

		// Token: 0x0400294D RID: 10573
		internal const string ControlOnMarginChangedDescr = "ControlOnMarginChangedDescr";

		// Token: 0x0400294E RID: 10574
		internal const string ControlOnMouseCaptureChangedDescr = "ControlOnMouseCaptureChangedDescr";

		// Token: 0x0400294F RID: 10575
		internal const string ControlOnMouseClickDescr = "ControlOnMouseClickDescr";

		// Token: 0x04002950 RID: 10576
		internal const string ControlOnMouseDoubleClickDescr = "ControlOnMouseDoubleClickDescr";

		// Token: 0x04002951 RID: 10577
		internal const string ControlOnMouseDownDescr = "ControlOnMouseDownDescr";

		// Token: 0x04002952 RID: 10578
		internal const string ControlOnMouseEnterDescr = "ControlOnMouseEnterDescr";

		// Token: 0x04002953 RID: 10579
		internal const string ControlOnMouseHoverDescr = "ControlOnMouseHoverDescr";

		// Token: 0x04002954 RID: 10580
		internal const string ControlOnMouseLeaveDescr = "ControlOnMouseLeaveDescr";

		// Token: 0x04002955 RID: 10581
		internal const string ControlOnMouseMoveDescr = "ControlOnMouseMoveDescr";

		// Token: 0x04002956 RID: 10582
		internal const string ControlOnMouseUpDescr = "ControlOnMouseUpDescr";

		// Token: 0x04002957 RID: 10583
		internal const string ControlOnMouseWheelDescr = "ControlOnMouseWheelDescr";

		// Token: 0x04002958 RID: 10584
		internal const string ControlOnMoveDescr = "ControlOnMoveDescr";

		// Token: 0x04002959 RID: 10585
		internal const string ControlOnPaddingChangedDescr = "ControlOnPaddingChangedDescr";

		// Token: 0x0400295A RID: 10586
		internal const string ControlOnPaintDescr = "ControlOnPaintDescr";

		// Token: 0x0400295B RID: 10587
		internal const string ControlOnParentChangedDescr = "ControlOnParentChangedDescr";

		// Token: 0x0400295C RID: 10588
		internal const string ControlOnQueryAccessibilityHelpDescr = "ControlOnQueryAccessibilityHelpDescr";

		// Token: 0x0400295D RID: 10589
		internal const string ControlOnQueryContinueDragDescr = "ControlOnQueryContinueDragDescr";

		// Token: 0x0400295E RID: 10590
		internal const string ControlOnResizeBeginDescr = "ControlOnResizeBeginDescr";

		// Token: 0x0400295F RID: 10591
		internal const string ControlOnResizeDescr = "ControlOnResizeDescr";

		// Token: 0x04002960 RID: 10592
		internal const string ControlOnResizeEndDescr = "ControlOnResizeEndDescr";

		// Token: 0x04002961 RID: 10593
		internal const string ControlOnRightToLeftChangedDescr = "ControlOnRightToLeftChangedDescr";

		// Token: 0x04002962 RID: 10594
		internal const string ControlOnRightToLeftLayoutChangedDescr = "ControlOnRightToLeftLayoutChangedDescr";

		// Token: 0x04002963 RID: 10595
		internal const string ControlOnSizeChangedDescr = "ControlOnSizeChangedDescr";

		// Token: 0x04002964 RID: 10596
		internal const string ControlOnStyleChangedDescr = "ControlOnStyleChangedDescr";

		// Token: 0x04002965 RID: 10597
		internal const string ControlOnSystemColorsChangedDescr = "ControlOnSystemColorsChangedDescr";

		// Token: 0x04002966 RID: 10598
		internal const string ControlOnTabIndexChangedDescr = "ControlOnTabIndexChangedDescr";

		// Token: 0x04002967 RID: 10599
		internal const string ControlOnTabStopChangedDescr = "ControlOnTabStopChangedDescr";

		// Token: 0x04002968 RID: 10600
		internal const string ControlOnTextChangedDescr = "ControlOnTextChangedDescr";

		// Token: 0x04002969 RID: 10601
		internal const string ControlOnValidatedDescr = "ControlOnValidatedDescr";

		// Token: 0x0400296A RID: 10602
		internal const string ControlOnValidatingDescr = "ControlOnValidatingDescr";

		// Token: 0x0400296B RID: 10603
		internal const string ControlOnVisibleChangedDescr = "ControlOnVisibleChangedDescr";

		// Token: 0x0400296C RID: 10604
		internal const string ControlPaddingDescr = "ControlPaddingDescr";

		// Token: 0x0400296D RID: 10605
		internal const string ControlParentDescr = "ControlParentDescr";

		// Token: 0x0400296E RID: 10606
		internal const string ControlProductNameDescr = "ControlProductNameDescr";

		// Token: 0x0400296F RID: 10607
		internal const string ControlProductVersionDescr = "ControlProductVersionDescr";

		// Token: 0x04002970 RID: 10608
		internal const string ControlRecreatingHandleDescr = "ControlRecreatingHandleDescr";

		// Token: 0x04002971 RID: 10609
		internal const string ControlRegionChangedDescr = "ControlRegionChangedDescr";

		// Token: 0x04002972 RID: 10610
		internal const string ControlRegionDescr = "ControlRegionDescr";

		// Token: 0x04002973 RID: 10611
		internal const string ControlResizeRedrawDescr = "ControlResizeRedrawDescr";

		// Token: 0x04002974 RID: 10612
		internal const string ControlRightDescr = "ControlRightDescr";

		// Token: 0x04002975 RID: 10613
		internal const string ControlRightToLeftDescr = "ControlRightToLeftDescr";

		// Token: 0x04002976 RID: 10614
		internal const string ControlRightToLeftLayoutDescr = "ControlRightToLeftLayoutDescr";

		// Token: 0x04002977 RID: 10615
		internal const string ControlSizeDescr = "ControlSizeDescr";

		// Token: 0x04002978 RID: 10616
		internal const string ControlTabIndexDescr = "ControlTabIndexDescr";

		// Token: 0x04002979 RID: 10617
		internal const string ControlTabStopDescr = "ControlTabStopDescr";

		// Token: 0x0400297A RID: 10618
		internal const string ControlTagDescr = "ControlTagDescr";

		// Token: 0x0400297B RID: 10619
		internal const string ControlTextDescr = "ControlTextDescr";

		// Token: 0x0400297C RID: 10620
		internal const string ControlTopDescr = "ControlTopDescr";

		// Token: 0x0400297D RID: 10621
		internal const string ControlTopLevelControlDescr = "ControlTopLevelControlDescr";

		// Token: 0x0400297E RID: 10622
		internal const string ControlUnsupportedProperty = "ControlUnsupportedProperty";

		// Token: 0x0400297F RID: 10623
		internal const string ControlUserPreferenceChangedDescr = "ControlUserPreferenceChangedDescr";

		// Token: 0x04002980 RID: 10624
		internal const string ControlUserPreferenceChangingDescr = "ControlUserPreferenceChangingDescr";

		// Token: 0x04002981 RID: 10625
		internal const string ControlUseWaitCursorDescr = "ControlUseWaitCursorDescr";

		// Token: 0x04002982 RID: 10626
		internal const string ControlVisibleDescr = "ControlVisibleDescr";

		// Token: 0x04002983 RID: 10627
		internal const string ControlWidthDescr = "ControlWidthDescr";

		// Token: 0x04002984 RID: 10628
		internal const string ControlWindowTargetDescr = "ControlWindowTargetDescr";

		// Token: 0x04002985 RID: 10629
		internal const string ControlWithScrollbarsPositionDescr = "ControlWithScrollbarsPositionDescr";

		// Token: 0x04002986 RID: 10630
		internal const string ControlWithScrollbarsVirtualSizeDescr = "ControlWithScrollbarsVirtualSizeDescr";

		// Token: 0x04002987 RID: 10631
		internal const string CurrencyManagerCantAddNew = "CurrencyManagerCantAddNew";

		// Token: 0x04002988 RID: 10632
		internal const string CursorCannotCovertToBytes = "CursorCannotCovertToBytes";

		// Token: 0x04002989 RID: 10633
		internal const string CursorCannotCovertToString = "CursorCannotCovertToString";

		// Token: 0x0400298A RID: 10634
		internal const string CursorNonSerializableHandle = "CursorNonSerializableHandle";

		// Token: 0x0400298B RID: 10635
		internal const string DataBindingAddNewNotSupportedOnPropertyManager = "DataBindingAddNewNotSupportedOnPropertyManager";

		// Token: 0x0400298C RID: 10636
		internal const string DataBindingCycle = "DataBindingCycle";

		// Token: 0x0400298D RID: 10637
		internal const string DataBindingPushDataException = "DataBindingPushDataException";

		// Token: 0x0400298E RID: 10638
		internal const string DataBindingRemoveAtNotSupportedOnPropertyManager = "DataBindingRemoveAtNotSupportedOnPropertyManager";

		// Token: 0x0400298F RID: 10639
		internal const string DataGridAllowSortingDescr = "DataGridAllowSortingDescr";

		// Token: 0x04002990 RID: 10640
		internal const string DataGridAlternatingBackColorDescr = "DataGridAlternatingBackColorDescr";

		// Token: 0x04002991 RID: 10641
		internal const string DataGridBackButtonClickDescr = "DataGridBackButtonClickDescr";

		// Token: 0x04002992 RID: 10642
		internal const string DataGridBackgroundColorDescr = "DataGridBackgroundColorDescr";

		// Token: 0x04002993 RID: 10643
		internal const string DataGridBeginInit = "DataGridBeginInit";

		// Token: 0x04002994 RID: 10644
		internal const string DataGridBoolColumnAllowNullValue = "DataGridBoolColumnAllowNullValue";

		// Token: 0x04002995 RID: 10645
		internal const string DataGridBorderStyleDescr = "DataGridBorderStyleDescr";

		// Token: 0x04002996 RID: 10646
		internal const string DataGridCaptionBackButtonToolTip = "DataGridCaptionBackButtonToolTip";

		// Token: 0x04002997 RID: 10647
		internal const string DataGridCaptionBackColorDescr = "DataGridCaptionBackColorDescr";

		// Token: 0x04002998 RID: 10648
		internal const string DataGridCaptionDetailsButtonToolTip = "DataGridCaptionDetailsButtonToolTip";

		// Token: 0x04002999 RID: 10649
		internal const string DataGridCaptionFontDescr = "DataGridCaptionFontDescr";

		// Token: 0x0400299A RID: 10650
		internal const string DataGridCaptionForeColorDescr = "DataGridCaptionForeColorDescr";

		// Token: 0x0400299B RID: 10651
		internal const string DataGridCaptionTextDescr = "DataGridCaptionTextDescr";

		// Token: 0x0400299C RID: 10652
		internal const string DataGridCaptionVisibleDescr = "DataGridCaptionVisibleDescr";

		// Token: 0x0400299D RID: 10653
		internal const string DataGridColumnCollectionMissing = "DataGridColumnCollectionMissing";

		// Token: 0x0400299E RID: 10654
		internal const string DataGridColumnHeadersVisibleDescr = "DataGridColumnHeadersVisibleDescr";

		// Token: 0x0400299F RID: 10655
		internal const string DataGridColumnListManagerPosition = "DataGridColumnListManagerPosition";

		// Token: 0x040029A0 RID: 10656
		internal const string DataGridColumnNoPropertyDescriptor = "DataGridColumnNoPropertyDescriptor";

		// Token: 0x040029A1 RID: 10657
		internal const string DataGridColumnStyleDuplicateMappingName = "DataGridColumnStyleDuplicateMappingName";

		// Token: 0x040029A2 RID: 10658
		internal const string DataGridColumnUnbound = "DataGridColumnUnbound";

		// Token: 0x040029A3 RID: 10659
		internal const string DataGridColumnWidth = "DataGridColumnWidth";

		// Token: 0x040029A4 RID: 10660
		internal const string DataGridCurrentCellDescr = "DataGridCurrentCellDescr";

		// Token: 0x040029A5 RID: 10661
		internal const string DataGridDataMemberDescr = "DataGridDataMemberDescr";

		// Token: 0x040029A6 RID: 10662
		internal const string DataGridDataSourceDescr = "DataGridDataSourceDescr";

		// Token: 0x040029A7 RID: 10663
		internal const string DataGridDefaultColumnCollectionChanged = "DataGridDefaultColumnCollectionChanged";

		// Token: 0x040029A8 RID: 10664
		internal const string DataGridDefaultTableSet = "DataGridDefaultTableSet";

		// Token: 0x040029A9 RID: 10665
		internal const string DataGridDownButtonClickDescr = "DataGridDownButtonClickDescr";

		// Token: 0x040029AA RID: 10666
		internal const string DataGridEmptyColor = "DataGridEmptyColor";

		// Token: 0x040029AB RID: 10667
		internal const string DataGridErrorMessageBoxCaption = "DataGridErrorMessageBoxCaption";

		// Token: 0x040029AC RID: 10668
		internal const string DataGridExceptionInPaint = "DataGridExceptionInPaint";

		// Token: 0x040029AD RID: 10669
		internal const string DataGridFailedToGetRegionInfo = "DataGridFailedToGetRegionInfo";

		// Token: 0x040029AE RID: 10670
		internal const string DataGridFirstVisibleColumnDescr = "DataGridFirstVisibleColumnDescr";

		// Token: 0x040029AF RID: 10671
		internal const string DataGridFlatModeDescr = "DataGridFlatModeDescr";

		// Token: 0x040029B0 RID: 10672
		internal const string DataGridGridLineColorDescr = "DataGridGridLineColorDescr";

		// Token: 0x040029B1 RID: 10673
		internal const string DataGridGridLineStyleDescr = "DataGridGridLineStyleDescr";

		// Token: 0x040029B2 RID: 10674
		internal const string DataGridGridTablesDescr = "DataGridGridTablesDescr";

		// Token: 0x040029B3 RID: 10675
		internal const string DataGridHeaderBackColorDescr = "DataGridHeaderBackColorDescr";

		// Token: 0x040029B4 RID: 10676
		internal const string DataGridHeaderFontDescr = "DataGridHeaderFontDescr";

		// Token: 0x040029B5 RID: 10677
		internal const string DataGridHeaderForeColorDescr = "DataGridHeaderForeColorDescr";

		// Token: 0x040029B6 RID: 10678
		internal const string DataGridHorizScrollBarDescr = "DataGridHorizScrollBarDescr";

		// Token: 0x040029B7 RID: 10679
		internal const string DataGridLinkColorDescr = "DataGridLinkColorDescr";

		// Token: 0x040029B8 RID: 10680
		internal const string DataGridLinkHoverColorDescr = "DataGridLinkHoverColorDescr";

		// Token: 0x040029B9 RID: 10681
		internal const string DataGridListManagerDescr = "DataGridListManagerDescr";

		// Token: 0x040029BA RID: 10682
		internal const string DataGridNavigateEventDescr = "DataGridNavigateEventDescr";

		// Token: 0x040029BB RID: 10683
		internal const string DataGridNavigationModeDescr = "DataGridNavigationModeDescr";

		// Token: 0x040029BC RID: 10684
		internal const string DataGridNodeClickEventDescr = "DataGridNodeClickEventDescr";

		// Token: 0x040029BD RID: 10685
		internal const string DataGridNullText = "DataGridNullText";

		// Token: 0x040029BE RID: 10686
		internal const string DataGridOnBackgroundColorChangedDescr = "DataGridOnBackgroundColorChangedDescr";

		// Token: 0x040029BF RID: 10687
		internal const string DataGridOnBorderStyleChangedDescr = "DataGridOnBorderStyleChangedDescr";

		// Token: 0x040029C0 RID: 10688
		internal const string DataGridOnCaptionVisibleChangedDescr = "DataGridOnCaptionVisibleChangedDescr";

		// Token: 0x040029C1 RID: 10689
		internal const string DataGridOnCurrentCellChangedDescr = "DataGridOnCurrentCellChangedDescr";

		// Token: 0x040029C2 RID: 10690
		internal const string DataGridOnDataSourceChangedDescr = "DataGridOnDataSourceChangedDescr";

		// Token: 0x040029C3 RID: 10691
		internal const string DataGridOnFlatModeChangedDescr = "DataGridOnFlatModeChangedDescr";

		// Token: 0x040029C4 RID: 10692
		internal const string DataGridOnNavigationModeChangedDescr = "DataGridOnNavigationModeChangedDescr";

		// Token: 0x040029C5 RID: 10693
		internal const string DataGridOnParentRowsLabelStyleChangedDescr = "DataGridOnParentRowsLabelStyleChangedDescr";

		// Token: 0x040029C6 RID: 10694
		internal const string DataGridOnParentRowsVisibleChangedDescr = "DataGridOnParentRowsVisibleChangedDescr";

		// Token: 0x040029C7 RID: 10695
		internal const string DataGridOnReadOnlyChangedDescr = "DataGridOnReadOnlyChangedDescr";

		// Token: 0x040029C8 RID: 10696
		internal const string DataGridParentRowsBackColorDescr = "DataGridParentRowsBackColorDescr";

		// Token: 0x040029C9 RID: 10697
		internal const string DataGridParentRowsForeColorDescr = "DataGridParentRowsForeColorDescr";

		// Token: 0x040029CA RID: 10698
		internal const string DataGridParentRowsLabelStyleDescr = "DataGridParentRowsLabelStyleDescr";

		// Token: 0x040029CB RID: 10699
		internal const string DataGridParentRowsVisibleDescr = "DataGridParentRowsVisibleDescr";

		// Token: 0x040029CC RID: 10700
		internal const string DataGridPreferredColumnWidthDescr = "DataGridPreferredColumnWidthDescr";

		// Token: 0x040029CD RID: 10701
		internal const string DataGridPreferredRowHeightDescr = "DataGridPreferredRowHeightDescr";

		// Token: 0x040029CE RID: 10702
		internal const string DataGridPushedIncorrectValueIntoColumn = "DataGridPushedIncorrectValueIntoColumn";

		// Token: 0x040029CF RID: 10703
		internal const string DataGridReadOnlyDescr = "DataGridReadOnlyDescr";

		// Token: 0x040029D0 RID: 10704
		internal const string DataGridRowHeadersVisibleDescr = "DataGridRowHeadersVisibleDescr";

		// Token: 0x040029D1 RID: 10705
		internal const string DataGridRowHeaderWidthDescr = "DataGridRowHeaderWidthDescr";

		// Token: 0x040029D2 RID: 10706
		internal const string DataGridRowRowHeight = "DataGridRowRowHeight";

		// Token: 0x040029D3 RID: 10707
		internal const string DataGridRowRowNumber = "DataGridRowRowNumber";

		// Token: 0x040029D4 RID: 10708
		internal const string DataGridScrollEventDescr = "DataGridScrollEventDescr";

		// Token: 0x040029D5 RID: 10709
		internal const string DataGridSelectedIndexDescr = "DataGridSelectedIndexDescr";

		// Token: 0x040029D6 RID: 10710
		internal const string DataGridSelectionBackColorDescr = "DataGridSelectionBackColorDescr";

		// Token: 0x040029D7 RID: 10711
		internal const string DataGridSelectionForeColorDescr = "DataGridSelectionForeColorDescr";

		// Token: 0x040029D8 RID: 10712
		internal const string DataGridSetListManager = "DataGridSetListManager";

		// Token: 0x040029D9 RID: 10713
		internal const string DataGridSetSelectIndex = "DataGridSetSelectIndex";

		// Token: 0x040029DA RID: 10714
		internal const string DataGridSettingCurrentCellNotGood = "DataGridSettingCurrentCellNotGood";

		// Token: 0x040029DB RID: 10715
		internal const string DataGridTableCollectionMissingTable = "DataGridTableCollectionMissingTable";

		// Token: 0x040029DC RID: 10716
		internal const string DataGridTableStyleCollectionAddedParentedTableStyle = "DataGridTableStyleCollectionAddedParentedTableStyle";

		// Token: 0x040029DD RID: 10717
		internal const string DataGridTableStyleDuplicateMappingName = "DataGridTableStyleDuplicateMappingName";

		// Token: 0x040029DE RID: 10718
		internal const string DataGridTableStyleTransparentAlternatingBackColorNotAllowed = "DataGridTableStyleTransparentAlternatingBackColorNotAllowed";

		// Token: 0x040029DF RID: 10719
		internal const string DataGridTableStyleTransparentBackColorNotAllowed = "DataGridTableStyleTransparentBackColorNotAllowed";

		// Token: 0x040029E0 RID: 10720
		internal const string DataGridTableStyleTransparentHeaderBackColorNotAllowed = "DataGridTableStyleTransparentHeaderBackColorNotAllowed";

		// Token: 0x040029E1 RID: 10721
		internal const string DataGridTableStyleTransparentSelectionBackColorNotAllowed = "DataGridTableStyleTransparentSelectionBackColorNotAllowed";

		// Token: 0x040029E2 RID: 10722
		internal const string DataGridToolTipEmptyIcon = "DataGridToolTipEmptyIcon";

		// Token: 0x040029E3 RID: 10723
		internal const string DataGridTransparentAlternatingBackColorNotAllowed = "DataGridTransparentAlternatingBackColorNotAllowed";

		// Token: 0x040029E4 RID: 10724
		internal const string DataGridTransparentBackColorNotAllowed = "DataGridTransparentBackColorNotAllowed";

		// Token: 0x040029E5 RID: 10725
		internal const string DataGridTransparentCaptionBackColorNotAllowed = "DataGridTransparentCaptionBackColorNotAllowed";

		// Token: 0x040029E6 RID: 10726
		internal const string DataGridTransparentHeaderBackColorNotAllowed = "DataGridTransparentHeaderBackColorNotAllowed";

		// Token: 0x040029E7 RID: 10727
		internal const string DataGridTransparentParentRowsBackColorNotAllowed = "DataGridTransparentParentRowsBackColorNotAllowed";

		// Token: 0x040029E8 RID: 10728
		internal const string DataGridTransparentSelectionBackColorNotAllowed = "DataGridTransparentSelectionBackColorNotAllowed";

		// Token: 0x040029E9 RID: 10729
		internal const string DataGridUnbound = "DataGridUnbound";

		// Token: 0x040029EA RID: 10730
		internal const string DataGridVertScrollBarDescr = "DataGridVertScrollBarDescr";

		// Token: 0x040029EB RID: 10731
		internal const string DataGridView_AccButtonCellDefaultAction = "DataGridView_AccButtonCellDefaultAction";

		// Token: 0x040029EC RID: 10732
		internal const string DataGridView_AccCellDefaultAction = "DataGridView_AccCellDefaultAction";

		// Token: 0x040029ED RID: 10733
		internal const string DataGridView_AccCheckBoxCellDefaultActionCheck = "DataGridView_AccCheckBoxCellDefaultActionCheck";

		// Token: 0x040029EE RID: 10734
		internal const string DataGridView_AccCheckBoxCellDefaultActionUncheck = "DataGridView_AccCheckBoxCellDefaultActionUncheck";

		// Token: 0x040029EF RID: 10735
		internal const string DataGridView_AccColumnHeaderCellDefaultAction = "DataGridView_AccColumnHeaderCellDefaultAction";

		// Token: 0x040029F0 RID: 10736
		internal const string DataGridView_AccColumnHeaderCellSelectDefaultAction = "DataGridView_AccColumnHeaderCellSelectDefaultAction";

		// Token: 0x040029F1 RID: 10737
		internal const string DataGridView_AccDataGridViewCellName = "DataGridView_AccDataGridViewCellName";

		// Token: 0x040029F2 RID: 10738
		internal const string DataGridView_AccEditingControlAccName = "DataGridView_AccEditingControlAccName";

		// Token: 0x040029F3 RID: 10739
		internal const string DataGridView_AccEditingPanelAccName = "DataGridView_AccEditingPanelAccName";

		// Token: 0x040029F4 RID: 10740
		internal const string DataGridView_AccHorizontalScrollBarAccName = "DataGridView_AccHorizontalScrollBarAccName";

		// Token: 0x040029F5 RID: 10741
		internal const string DataGridView_AccLinkCellDefaultAction = "DataGridView_AccLinkCellDefaultAction";

		// Token: 0x040029F6 RID: 10742
		internal const string DataGridView_AccNullValue = "DataGridView_AccNullValue";

		// Token: 0x040029F7 RID: 10743
		internal const string DataGridView_AccRowCreateNew = "DataGridView_AccRowCreateNew";

		// Token: 0x040029F8 RID: 10744
		internal const string DataGridView_AccRowName = "DataGridView_AccRowName";

		// Token: 0x040029F9 RID: 10745
		internal const string DataGridView_AccSelectedCellsName = "DataGridView_AccSelectedCellsName";

		// Token: 0x040029FA RID: 10746
		internal const string DataGridView_AccSelectedRowCellsName = "DataGridView_AccSelectedRowCellsName";

		// Token: 0x040029FB RID: 10747
		internal const string DataGridView_AccTopLeftColumnHeaderCellDefaultAction = "DataGridView_AccTopLeftColumnHeaderCellDefaultAction";

		// Token: 0x040029FC RID: 10748
		internal const string DataGridView_AccTopLeftColumnHeaderCellName = "DataGridView_AccTopLeftColumnHeaderCellName";

		// Token: 0x040029FD RID: 10749
		internal const string DataGridView_AccTopLeftColumnHeaderCellNameRTL = "DataGridView_AccTopLeftColumnHeaderCellNameRTL";

		// Token: 0x040029FE RID: 10750
		internal const string DataGridView_AccTopRow = "DataGridView_AccTopRow";

		// Token: 0x040029FF RID: 10751
		internal const string DataGridView_AccVerticalScrollBarAccName = "DataGridView_AccVerticalScrollBarAccName";

		// Token: 0x04002A00 RID: 10752
		internal const string DataGridView_AColumnHasNoCellTemplate = "DataGridView_AColumnHasNoCellTemplate";

		// Token: 0x04002A01 RID: 10753
		internal const string DataGridView_AdvancedCellBorderStyleInvalid = "DataGridView_AdvancedCellBorderStyleInvalid";

		// Token: 0x04002A02 RID: 10754
		internal const string DataGridView_AllowUserToAddRowsDescr = "DataGridView_AllowUserToAddRowsDescr";

		// Token: 0x04002A03 RID: 10755
		internal const string DataGridView_AllowUserToDeleteRowsDescr = "DataGridView_AllowUserToDeleteRowsDescr";

		// Token: 0x04002A04 RID: 10756
		internal const string DataGridView_AllowUserToOrderColumnsDescr = "DataGridView_AllowUserToOrderColumnsDescr";

		// Token: 0x04002A05 RID: 10757
		internal const string DataGridView_AllowUserToResizeColumnsDescr = "DataGridView_AllowUserToResizeColumnsDescr";

		// Token: 0x04002A06 RID: 10758
		internal const string DataGridView_AllowUserToResizeRowsDescr = "DataGridView_AllowUserToResizeRowsDescr";

		// Token: 0x04002A07 RID: 10759
		internal const string DataGridView_AlternatingRowsDefaultCellStyleDescr = "DataGridView_AlternatingRowsDefaultCellStyleDescr";

		// Token: 0x04002A08 RID: 10760
		internal const string DataGridView_AtLeastOneColumnIsNull = "DataGridView_AtLeastOneColumnIsNull";

		// Token: 0x04002A09 RID: 10761
		internal const string DataGridView_AtLeastOneRowIsNull = "DataGridView_AtLeastOneRowIsNull";

		// Token: 0x04002A0A RID: 10762
		internal const string DataGridView_AutoSizeColumnsModeDescr = "DataGridView_AutoSizeColumnsModeDescr";

		// Token: 0x04002A0B RID: 10763
		internal const string DataGridView_AutoSizeRowsModeDescr = "DataGridView_AutoSizeRowsModeDescr";

		// Token: 0x04002A0C RID: 10764
		internal const string DataGridView_BeginEditNotReentrant = "DataGridView_BeginEditNotReentrant";

		// Token: 0x04002A0D RID: 10765
		internal const string DataGridView_BorderStyleDescr = "DataGridView_BorderStyleDescr";

		// Token: 0x04002A0E RID: 10766
		internal const string DataGridView_ButtonColumnFlatStyleDescr = "DataGridView_ButtonColumnFlatStyleDescr";

		// Token: 0x04002A0F RID: 10767
		internal const string DataGridView_ButtonColumnTextDescr = "DataGridView_ButtonColumnTextDescr";

		// Token: 0x04002A10 RID: 10768
		internal const string DataGridView_ButtonColumnUseColumnTextForButtonValueDescr = "DataGridView_ButtonColumnUseColumnTextForButtonValueDescr";

		// Token: 0x04002A11 RID: 10769
		internal const string DataGridView_CancelRowEditDescr = "DataGridView_CancelRowEditDescr";

		// Token: 0x04002A12 RID: 10770
		internal const string DataGridView_CannotAddAutoFillColumn = "DataGridView_CannotAddAutoFillColumn";

		// Token: 0x04002A13 RID: 10771
		internal const string DataGridView_CannotAddAutoSizedColumn = "DataGridView_CannotAddAutoSizedColumn";

		// Token: 0x04002A14 RID: 10772
		internal const string DataGridView_CannotAddFrozenColumn = "DataGridView_CannotAddFrozenColumn";

		// Token: 0x04002A15 RID: 10773
		internal const string DataGridView_CannotAddFrozenRow = "DataGridView_CannotAddFrozenRow";

		// Token: 0x04002A16 RID: 10774
		internal const string DataGridView_CannotAddIdenticalColumns = "DataGridView_CannotAddIdenticalColumns";

		// Token: 0x04002A17 RID: 10775
		internal const string DataGridView_CannotAddIdenticalRows = "DataGridView_CannotAddIdenticalRows";

		// Token: 0x04002A18 RID: 10776
		internal const string DataGridView_CannotAddNonFrozenColumn = "DataGridView_CannotAddNonFrozenColumn";

		// Token: 0x04002A19 RID: 10777
		internal const string DataGridView_CannotAddNonFrozenRow = "DataGridView_CannotAddNonFrozenRow";

		// Token: 0x04002A1A RID: 10778
		internal const string DataGridView_CannotAddUntypedColumn = "DataGridView_CannotAddUntypedColumn";

		// Token: 0x04002A1B RID: 10779
		internal const string DataGridView_CannotAlterAutoFillColumnParameter = "DataGridView_CannotAlterAutoFillColumnParameter";

		// Token: 0x04002A1C RID: 10780
		internal const string DataGridView_CannotAlterDisplayIndexWithinAdjustments = "DataGridView_CannotAlterDisplayIndexWithinAdjustments";

		// Token: 0x04002A1D RID: 10781
		internal const string DataGridView_CannotAutoFillFrozenColumns = "DataGridView_CannotAutoFillFrozenColumns";

		// Token: 0x04002A1E RID: 10782
		internal const string DataGridView_CannotAutoSizeColumnsInvisibleColumnHeaders = "DataGridView_CannotAutoSizeColumnsInvisibleColumnHeaders";

		// Token: 0x04002A1F RID: 10783
		internal const string DataGridView_CannotAutoSizeInvisibleColumnHeader = "DataGridView_CannotAutoSizeInvisibleColumnHeader";

		// Token: 0x04002A20 RID: 10784
		internal const string DataGridView_CannotAutoSizeRowInvisibleRowHeader = "DataGridView_CannotAutoSizeRowInvisibleRowHeader";

		// Token: 0x04002A21 RID: 10785
		internal const string DataGridView_CannotAutoSizeRowsInvisibleRowHeader = "DataGridView_CannotAutoSizeRowsInvisibleRowHeader";

		// Token: 0x04002A22 RID: 10786
		internal const string DataGridView_CannotMakeAutoSizedColumnVisible = "DataGridView_CannotMakeAutoSizedColumnVisible";

		// Token: 0x04002A23 RID: 10787
		internal const string DataGridView_CannotMoveFrozenColumn = "DataGridView_CannotMoveFrozenColumn";

		// Token: 0x04002A24 RID: 10788
		internal const string DataGridView_CannotMoveNonFrozenColumn = "DataGridView_CannotMoveNonFrozenColumn";

		// Token: 0x04002A25 RID: 10789
		internal const string DataGridView_CannotSetColumnCountOnDataBoundDataGridView = "DataGridView_CannotSetColumnCountOnDataBoundDataGridView";

		// Token: 0x04002A26 RID: 10790
		internal const string DataGridView_CannotSetRowCountOnDataBoundDataGridView = "DataGridView_CannotSetRowCountOnDataBoundDataGridView";

		// Token: 0x04002A27 RID: 10791
		internal const string DataGridView_CannotSortDataBoundDataGridViewBoundToNonIBindingList = "DataGridView_CannotSortDataBoundDataGridViewBoundToNonIBindingList";

		// Token: 0x04002A28 RID: 10792
		internal const string DataGridView_CannotThrowNullException = "DataGridView_CannotThrowNullException";

		// Token: 0x04002A29 RID: 10793
		internal const string DataGridView_CannotUseAComparerToSortDataGridViewWhenDataBound = "DataGridView_CannotUseAComparerToSortDataGridViewWhenDataBound";

		// Token: 0x04002A2A RID: 10794
		internal const string DataGridView_CellBeginEditDescr = "DataGridView_CellBeginEditDescr";

		// Token: 0x04002A2B RID: 10795
		internal const string DataGridView_CellBorderStyleChangedDescr = "DataGridView_CellBorderStyleChangedDescr";

		// Token: 0x04002A2C RID: 10796
		internal const string DataGridView_CellBorderStyleDescr = "DataGridView_CellBorderStyleDescr";

		// Token: 0x04002A2D RID: 10797
		internal const string DataGridView_CellChangeCannotBeCommittedOrAborted = "DataGridView_CellChangeCannotBeCommittedOrAborted";

		// Token: 0x04002A2E RID: 10798
		internal const string DataGridView_CellClickDescr = "DataGridView_CellClickDescr";

		// Token: 0x04002A2F RID: 10799
		internal const string DataGridView_CellContentClick = "DataGridView_CellContentClick";

		// Token: 0x04002A30 RID: 10800
		internal const string DataGridView_CellContentDoubleClick = "DataGridView_CellContentDoubleClick";

		// Token: 0x04002A31 RID: 10801
		internal const string DataGridView_CellContextMenuStripChanged = "DataGridView_CellContextMenuStripChanged";

		// Token: 0x04002A32 RID: 10802
		internal const string DataGridView_CellContextMenuStripNeeded = "DataGridView_CellContextMenuStripNeeded";

		// Token: 0x04002A33 RID: 10803
		internal const string DataGridView_CellDoesNotBelongToDataGridView = "DataGridView_CellDoesNotBelongToDataGridView";

		// Token: 0x04002A34 RID: 10804
		internal const string DataGridView_CellDoesNotYetBelongToDataGridView = "DataGridView_CellDoesNotYetBelongToDataGridView";

		// Token: 0x04002A35 RID: 10805
		internal const string DataGridView_CellDoubleClickDescr = "DataGridView_CellDoubleClickDescr";

		// Token: 0x04002A36 RID: 10806
		internal const string DataGridView_CellEndEditDescr = "DataGridView_CellEndEditDescr";

		// Token: 0x04002A37 RID: 10807
		internal const string DataGridView_CellEnterDescr = "DataGridView_CellEnterDescr";

		// Token: 0x04002A38 RID: 10808
		internal const string DataGridView_CellErrorTextChangedDescr = "DataGridView_CellErrorTextChangedDescr";

		// Token: 0x04002A39 RID: 10809
		internal const string DataGridView_CellErrorTextNeededDescr = "DataGridView_CellErrorTextNeededDescr";

		// Token: 0x04002A3A RID: 10810
		internal const string DataGridView_CellFormattingDescr = "DataGridView_CellFormattingDescr";

		// Token: 0x04002A3B RID: 10811
		internal const string DataGridView_CellLeaveDescr = "DataGridView_CellLeaveDescr";

		// Token: 0x04002A3C RID: 10812
		internal const string DataGridView_CellMouseClickDescr = "DataGridView_CellMouseClickDescr";

		// Token: 0x04002A3D RID: 10813
		internal const string DataGridView_CellMouseDoubleClickDescr = "DataGridView_CellMouseDoubleClickDescr";

		// Token: 0x04002A3E RID: 10814
		internal const string DataGridView_CellMouseDownDescr = "DataGridView_CellMouseDownDescr";

		// Token: 0x04002A3F RID: 10815
		internal const string DataGridView_CellMouseEnterDescr = "DataGridView_CellMouseEnterDescr";

		// Token: 0x04002A40 RID: 10816
		internal const string DataGridView_CellMouseLeaveDescr = "DataGridView_CellMouseLeaveDescr";

		// Token: 0x04002A41 RID: 10817
		internal const string DataGridView_CellMouseMoveDescr = "DataGridView_CellMouseMoveDescr";

		// Token: 0x04002A42 RID: 10818
		internal const string DataGridView_CellMouseUpDescr = "DataGridView_CellMouseUpDescr";

		// Token: 0x04002A43 RID: 10819
		internal const string DataGridView_CellNeedsDataGridViewForInheritedStyle = "DataGridView_CellNeedsDataGridViewForInheritedStyle";

		// Token: 0x04002A44 RID: 10820
		internal const string DataGridView_CellPaintingDescr = "DataGridView_CellPaintingDescr";

		// Token: 0x04002A45 RID: 10821
		internal const string DataGridView_CellParsingDescr = "DataGridView_CellParsingDescr";

		// Token: 0x04002A46 RID: 10822
		internal const string DataGridView_CellStateChangedDescr = "DataGridView_CellStateChangedDescr";

		// Token: 0x04002A47 RID: 10823
		internal const string DataGridView_CellStyleChangedDescr = "DataGridView_CellStyleChangedDescr";

		// Token: 0x04002A48 RID: 10824
		internal const string DataGridView_CellStyleContentChangedDescr = "DataGridView_CellStyleContentChangedDescr";

		// Token: 0x04002A49 RID: 10825
		internal const string DataGridView_CellToolTipTextChangedDescr = "DataGridView_CellToolTipTextChangedDescr";

		// Token: 0x04002A4A RID: 10826
		internal const string DataGridView_CellToolTipTextDescr = "DataGridView_CellToolTipTextDescr";

		// Token: 0x04002A4B RID: 10827
		internal const string DataGridView_CellToolTipTextNeededDescr = "DataGridView_CellToolTipTextNeededDescr";

		// Token: 0x04002A4C RID: 10828
		internal const string DataGridView_CellValidatedDescr = "DataGridView_CellValidatedDescr";

		// Token: 0x04002A4D RID: 10829
		internal const string DataGridView_CellValidatingDescr = "DataGridView_CellValidatingDescr";

		// Token: 0x04002A4E RID: 10830
		internal const string DataGridView_CellValueChangedDescr = "DataGridView_CellValueChangedDescr";

		// Token: 0x04002A4F RID: 10831
		internal const string DataGridView_CellValueNeededDescr = "DataGridView_CellValueNeededDescr";

		// Token: 0x04002A50 RID: 10832
		internal const string DataGridView_CellValuePushedDescr = "DataGridView_CellValuePushedDescr";

		// Token: 0x04002A51 RID: 10833
		internal const string DataGridView_CheckBoxColumnFalseValueDescr = "DataGridView_CheckBoxColumnFalseValueDescr";

		// Token: 0x04002A52 RID: 10834
		internal const string DataGridView_CheckBoxColumnFlatStyleDescr = "DataGridView_CheckBoxColumnFlatStyleDescr";

		// Token: 0x04002A53 RID: 10835
		internal const string DataGridView_CheckBoxColumnIndeterminateValueDescr = "DataGridView_CheckBoxColumnIndeterminateValueDescr";

		// Token: 0x04002A54 RID: 10836
		internal const string DataGridView_CheckBoxColumnThreeStateDescr = "DataGridView_CheckBoxColumnThreeStateDescr";

		// Token: 0x04002A55 RID: 10837
		internal const string DataGridView_CheckBoxColumnTrueValueDescr = "DataGridView_CheckBoxColumnTrueValueDescr";

		// Token: 0x04002A56 RID: 10838
		internal const string DataGridView_ClipboardCopyModeDescr = "DataGridView_ClipboardCopyModeDescr";

		// Token: 0x04002A57 RID: 10839
		internal const string DataGridView_ColumnAddedDescr = "DataGridView_ColumnAddedDescr";

		// Token: 0x04002A58 RID: 10840
		internal const string DataGridView_ColumnAlreadyBelongsToDataGridView = "DataGridView_ColumnAlreadyBelongsToDataGridView";

		// Token: 0x04002A59 RID: 10841
		internal const string DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly = "DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly";

		// Token: 0x04002A5A RID: 10842
		internal const string DataGridView_ColumnContextMenuStripChangedDescr = "DataGridView_ColumnContextMenuStripChangedDescr";

		// Token: 0x04002A5B RID: 10843
		internal const string DataGridView_ColumnContextMenuStripDescr = "DataGridView_ColumnContextMenuStripDescr";

		// Token: 0x04002A5C RID: 10844
		internal const string DataGridView_ColumnDataPropertyNameChangedDescr = "DataGridView_ColumnDataPropertyNameChangedDescr";

		// Token: 0x04002A5D RID: 10845
		internal const string DataGridView_ColumnDataPropertyNameDescr = "DataGridView_ColumnDataPropertyNameDescr";

		// Token: 0x04002A5E RID: 10846
		internal const string DataGridView_ColumnDefaultCellStyleChangedDescr = "DataGridView_ColumnDefaultCellStyleChangedDescr";

		// Token: 0x04002A5F RID: 10847
		internal const string DataGridView_ColumnDefaultCellStyleDescr = "DataGridView_ColumnDefaultCellStyleDescr";

		// Token: 0x04002A60 RID: 10848
		internal const string DataGridView_ColumnDisplayIndexChangedDescr = "DataGridView_ColumnDisplayIndexChangedDescr";

		// Token: 0x04002A61 RID: 10849
		internal const string DataGridView_ColumnDividerDoubleClickDescr = "DataGridView_ColumnDividerDoubleClickDescr";

		// Token: 0x04002A62 RID: 10850
		internal const string DataGridView_ColumnDividerWidthChangedDescr = "DataGridView_ColumnDividerWidthChangedDescr";

		// Token: 0x04002A63 RID: 10851
		internal const string DataGridView_ColumnDividerWidthDescr = "DataGridView_ColumnDividerWidthDescr";

		// Token: 0x04002A64 RID: 10852
		internal const string DataGridView_ColumnDoesNotBelongToDataGridView = "DataGridView_ColumnDoesNotBelongToDataGridView";

		// Token: 0x04002A65 RID: 10853
		internal const string DataGridView_ColumnFrozenDescr = "DataGridView_ColumnFrozenDescr";

		// Token: 0x04002A66 RID: 10854
		internal const string DataGridView_ColumnHeaderCellChangedDescr = "DataGridView_ColumnHeaderCellChangedDescr";

		// Token: 0x04002A67 RID: 10855
		internal const string DataGridView_ColumnHeaderMouseClickDescr = "DataGridView_ColumnHeaderMouseClickDescr";

		// Token: 0x04002A68 RID: 10856
		internal const string DataGridView_ColumnHeaderMouseDoubleClickDescr = "DataGridView_ColumnHeaderMouseDoubleClickDescr";

		// Token: 0x04002A69 RID: 10857
		internal const string DataGridView_ColumnHeadersBorderStyleChangedDescr = "DataGridView_ColumnHeadersBorderStyleChangedDescr";

		// Token: 0x04002A6A RID: 10858
		internal const string DataGridView_ColumnHeadersBorderStyleDescr = "DataGridView_ColumnHeadersBorderStyleDescr";

		// Token: 0x04002A6B RID: 10859
		internal const string DataGridView_ColumnHeadersCannotBeInvisible = "DataGridView_ColumnHeadersCannotBeInvisible";

		// Token: 0x04002A6C RID: 10860
		internal const string DataGridView_ColumnHeadersDefaultCellStyleDescr = "DataGridView_ColumnHeadersDefaultCellStyleDescr";

		// Token: 0x04002A6D RID: 10861
		internal const string DataGridView_ColumnHeadersHeightDescr = "DataGridView_ColumnHeadersHeightDescr";

		// Token: 0x04002A6E RID: 10862
		internal const string DataGridView_ColumnHeadersHeightSizeModeChangedDescr = "DataGridView_ColumnHeadersHeightSizeModeChangedDescr";

		// Token: 0x04002A6F RID: 10863
		internal const string DataGridView_ColumnHeadersHeightSizeModeDescr = "DataGridView_ColumnHeadersHeightSizeModeDescr";

		// Token: 0x04002A70 RID: 10864
		internal const string DataGridView_ColumnHeaderTextDescr = "DataGridView_ColumnHeaderTextDescr";

		// Token: 0x04002A71 RID: 10865
		internal const string DataGridView_ColumnMinimumWidthChangedDescr = "DataGridView_ColumnMinimumWidthChangedDescr";

		// Token: 0x04002A72 RID: 10866
		internal const string DataGridView_ColumnMinimumWidthDescr = "DataGridView_ColumnMinimumWidthDescr";

		// Token: 0x04002A73 RID: 10867
		internal const string DataGridView_ColumnNameChangedDescr = "DataGridView_ColumnNameChangedDescr";

		// Token: 0x04002A74 RID: 10868
		internal const string DataGridView_ColumnNameDescr = "DataGridView_ColumnNameDescr";

		// Token: 0x04002A75 RID: 10869
		internal const string DataGridView_ColumnNeedsToBeDataBoundWhenSortingDataBoundDataGridView = "DataGridView_ColumnNeedsToBeDataBoundWhenSortingDataBoundDataGridView";

		// Token: 0x04002A76 RID: 10870
		internal const string DataGridView_ColumnReadOnlyDescr = "DataGridView_ColumnReadOnlyDescr";

		// Token: 0x04002A77 RID: 10871
		internal const string DataGridView_ColumnRemovedDescr = "DataGridView_ColumnRemovedDescr";

		// Token: 0x04002A78 RID: 10872
		internal const string DataGridView_ColumnResizableDescr = "DataGridView_ColumnResizableDescr";

		// Token: 0x04002A79 RID: 10873
		internal const string DataGridView_ColumnSortModeDescr = "DataGridView_ColumnSortModeDescr";

		// Token: 0x04002A7A RID: 10874
		internal const string DataGridView_ColumnStateChangedDescr = "DataGridView_ColumnStateChangedDescr";

		// Token: 0x04002A7B RID: 10875
		internal const string DataGridView_ColumnToolTipTextChangedDescr = "DataGridView_ColumnToolTipTextChangedDescr";

		// Token: 0x04002A7C RID: 10876
		internal const string DataGridView_ColumnToolTipTextDescr = "DataGridView_ColumnToolTipTextDescr";

		// Token: 0x04002A7D RID: 10877
		internal const string DataGridView_ColumnVisibleDescr = "DataGridView_ColumnVisibleDescr";

		// Token: 0x04002A7E RID: 10878
		internal const string DataGridView_ColumnWidthChangedDescr = "DataGridView_ColumnWidthChangedDescr";

		// Token: 0x04002A7F RID: 10879
		internal const string DataGridView_ColumnWidthDescr = "DataGridView_ColumnWidthDescr";

		// Token: 0x04002A80 RID: 10880
		internal const string DataGridView_ComboBoxColumnAutoCompleteDescr = "DataGridView_ComboBoxColumnAutoCompleteDescr";

		// Token: 0x04002A81 RID: 10881
		internal const string DataGridView_ComboBoxColumnDataSourceDescr = "DataGridView_ComboBoxColumnDataSourceDescr";

		// Token: 0x04002A82 RID: 10882
		internal const string DataGridView_ComboBoxColumnDisplayMemberDescr = "DataGridView_ComboBoxColumnDisplayMemberDescr";

		// Token: 0x04002A83 RID: 10883
		internal const string DataGridView_ComboBoxColumnDisplayStyleDescr = "DataGridView_ComboBoxColumnDisplayStyleDescr";

		// Token: 0x04002A84 RID: 10884
		internal const string DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr = "DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr";

		// Token: 0x04002A85 RID: 10885
		internal const string DataGridView_ComboBoxColumnDropDownWidthDescr = "DataGridView_ComboBoxColumnDropDownWidthDescr";

		// Token: 0x04002A86 RID: 10886
		internal const string DataGridView_ComboBoxColumnFlatStyleDescr = "DataGridView_ComboBoxColumnFlatStyleDescr";

		// Token: 0x04002A87 RID: 10887
		internal const string DataGridView_ComboBoxColumnItemsDescr = "DataGridView_ComboBoxColumnItemsDescr";

		// Token: 0x04002A88 RID: 10888
		internal const string DataGridView_ComboBoxColumnMaxDropDownItemsDescr = "DataGridView_ComboBoxColumnMaxDropDownItemsDescr";

		// Token: 0x04002A89 RID: 10889
		internal const string DataGridView_ComboBoxColumnSortedDescr = "DataGridView_ComboBoxColumnSortedDescr";

		// Token: 0x04002A8A RID: 10890
		internal const string DataGridView_ComboBoxColumnValueMemberDescr = "DataGridView_ComboBoxColumnValueMemberDescr";

		// Token: 0x04002A8B RID: 10891
		internal const string DataGridView_CommitFailedCannotCompleteOperation = "DataGridView_CommitFailedCannotCompleteOperation";

		// Token: 0x04002A8C RID: 10892
		internal const string DataGridView_CurrencyManagerRowCannotBeInvisible = "DataGridView_CurrencyManagerRowCannotBeInvisible";

		// Token: 0x04002A8D RID: 10893
		internal const string DataGridView_CurrentCellCannotBeInvisible = "DataGridView_CurrentCellCannotBeInvisible";

		// Token: 0x04002A8E RID: 10894
		internal const string DataGridView_CurrentCellChangedDescr = "DataGridView_CurrentCellChangedDescr";

		// Token: 0x04002A8F RID: 10895
		internal const string DataGridView_CurrentCellDirtyStateChangedDescr = "DataGridView_CurrentCellDirtyStateChangedDescr";

		// Token: 0x04002A90 RID: 10896
		internal const string DataGridView_CustomCellBorderStyleInvalid = "DataGridView_CustomCellBorderStyleInvalid";

		// Token: 0x04002A91 RID: 10897
		internal const string DataGridView_DataBindingCompleteDescr = "DataGridView_DataBindingCompleteDescr";

		// Token: 0x04002A92 RID: 10898
		internal const string DataGridView_DataErrorDescr = "DataGridView_DataErrorDescr";

		// Token: 0x04002A93 RID: 10899
		internal const string DataGridView_DefaultCellStyleDescr = "DataGridView_DefaultCellStyleDescr";

		// Token: 0x04002A94 RID: 10900
		internal const string DataGridView_DefaultValuesNeededDescr = "DataGridView_DefaultValuesNeededDescr";

		// Token: 0x04002A95 RID: 10901
		internal const string DataGridView_DisabledClipboardCopy = "DataGridView_DisabledClipboardCopy";

		// Token: 0x04002A96 RID: 10902
		internal const string DataGridView_EditingControlShowingDescr = "DataGridView_EditingControlShowingDescr";

		// Token: 0x04002A97 RID: 10903
		internal const string DataGridView_EditModeChangedDescr = "DataGridView_EditModeChangedDescr";

		// Token: 0x04002A98 RID: 10904
		internal const string DataGridView_EditModeDescr = "DataGridView_EditModeDescr";

		// Token: 0x04002A99 RID: 10905
		internal const string DataGridView_EmptyColor = "DataGridView_EmptyColor";

		// Token: 0x04002A9A RID: 10906
		internal const string DataGridView_EnableHeadersVisualStylesDescr = "DataGridView_EnableHeadersVisualStylesDescr";

		// Token: 0x04002A9B RID: 10907
		internal const string DataGridView_ErrorMessageCaption = "DataGridView_ErrorMessageCaption";

		// Token: 0x04002A9C RID: 10908
		internal const string DataGridView_ErrorMessageText_NoException = "DataGridView_ErrorMessageText_NoException";

		// Token: 0x04002A9D RID: 10909
		internal const string DataGridView_ErrorMessageText_WithException = "DataGridView_ErrorMessageText_WithException";

		// Token: 0x04002A9E RID: 10910
		internal const string DataGridView_FirstDisplayedCellCannotBeAHeaderOrSharedCell = "DataGridView_FirstDisplayedCellCannotBeAHeaderOrSharedCell";

		// Token: 0x04002A9F RID: 10911
		internal const string DataGridView_FirstDisplayedCellCannotBeInvisible = "DataGridView_FirstDisplayedCellCannotBeInvisible";

		// Token: 0x04002AA0 RID: 10912
		internal const string DataGridView_FirstDisplayedScrollingColumnCannotBeFrozen = "DataGridView_FirstDisplayedScrollingColumnCannotBeFrozen";

		// Token: 0x04002AA1 RID: 10913
		internal const string DataGridView_FirstDisplayedScrollingColumnCannotBeInvisible = "DataGridView_FirstDisplayedScrollingColumnCannotBeInvisible";

		// Token: 0x04002AA2 RID: 10914
		internal const string DataGridView_FirstDisplayedScrollingRowCannotBeFrozen = "DataGridView_FirstDisplayedScrollingRowCannotBeFrozen";

		// Token: 0x04002AA3 RID: 10915
		internal const string DataGridView_FirstDisplayedScrollingRowCannotBeInvisible = "DataGridView_FirstDisplayedScrollingRowCannotBeInvisible";

		// Token: 0x04002AA4 RID: 10916
		internal const string DataGridView_ForbiddenOperationInEventHandler = "DataGridView_ForbiddenOperationInEventHandler";

		// Token: 0x04002AA5 RID: 10917
		internal const string DataGridView_FrozenColumnsPreventFirstDisplayedScrollingColumn = "DataGridView_FrozenColumnsPreventFirstDisplayedScrollingColumn";

		// Token: 0x04002AA6 RID: 10918
		internal const string DataGridView_FrozenRowsPreventFirstDisplayedScrollingRow = "DataGridView_FrozenRowsPreventFirstDisplayedScrollingRow";

		// Token: 0x04002AA7 RID: 10919
		internal const string DataGridView_HeaderCellReadOnlyProperty = "DataGridView_HeaderCellReadOnlyProperty";

		// Token: 0x04002AA8 RID: 10920
		internal const string DataGridView_IBindingListNeedsToSupportSorting = "DataGridView_IBindingListNeedsToSupportSorting";

		// Token: 0x04002AA9 RID: 10921
		internal const string DataGridView_InvalidDataGridViewElementStateCombination = "DataGridView_InvalidDataGridViewElementStateCombination";

		// Token: 0x04002AAA RID: 10922
		internal const string DataGridView_InvalidDataGridViewPaintPartsCombination = "DataGridView_InvalidDataGridViewPaintPartsCombination";

		// Token: 0x04002AAB RID: 10923
		internal const string DataGridView_InvalidEditingControl = "DataGridView_InvalidEditingControl";

		// Token: 0x04002AAC RID: 10924
		internal const string DataGridView_InvalidOperationInVirtualMode = "DataGridView_InvalidOperationInVirtualMode";

		// Token: 0x04002AAD RID: 10925
		internal const string DataGridView_InvalidOperationOnSharedCell = "DataGridView_InvalidOperationOnSharedCell";

		// Token: 0x04002AAE RID: 10926
		internal const string DataGridView_InvalidOperationOnSharedRow = "DataGridView_InvalidOperationOnSharedRow";

		// Token: 0x04002AAF RID: 10927
		internal const string DataGridView_InvalidPropertyGetOnSharedCell = "DataGridView_InvalidPropertyGetOnSharedCell";

		// Token: 0x04002AB0 RID: 10928
		internal const string DataGridView_InvalidPropertyGetOnSharedRow = "DataGridView_InvalidPropertyGetOnSharedRow";

		// Token: 0x04002AB1 RID: 10929
		internal const string DataGridView_InvalidPropertySetOnSharedRow = "DataGridView_InvalidPropertySetOnSharedRow";

		// Token: 0x04002AB2 RID: 10930
		internal const string DataGridView_LinkColumnActiveLinkColorDescr = "DataGridView_LinkColumnActiveLinkColorDescr";

		// Token: 0x04002AB3 RID: 10931
		internal const string DataGridView_LinkColumnLinkBehaviorDescr = "DataGridView_LinkColumnLinkBehaviorDescr";

		// Token: 0x04002AB4 RID: 10932
		internal const string DataGridView_LinkColumnLinkColorDescr = "DataGridView_LinkColumnLinkColorDescr";

		// Token: 0x04002AB5 RID: 10933
		internal const string DataGridView_LinkColumnTextDescr = "DataGridView_LinkColumnTextDescr";

		// Token: 0x04002AB6 RID: 10934
		internal const string DataGridView_LinkColumnTrackVisitedStateDescr = "DataGridView_LinkColumnTrackVisitedStateDescr";

		// Token: 0x04002AB7 RID: 10935
		internal const string DataGridView_LinkColumnUseColumnTextForLinkValueDescr = "DataGridView_LinkColumnUseColumnTextForLinkValueDescr";

		// Token: 0x04002AB8 RID: 10936
		internal const string DataGridView_LinkColumnVisitedLinkColorDescr = "DataGridView_LinkColumnVisitedLinkColorDescr";

		// Token: 0x04002AB9 RID: 10937
		internal const string DataGridView_MultiSelectDescr = "DataGridView_MultiSelectDescr";

		// Token: 0x04002ABA RID: 10938
		internal const string DataGridView_NeedAutoSizingCriteria = "DataGridView_NeedAutoSizingCriteria";

		// Token: 0x04002ABB RID: 10939
		internal const string DataGridView_NeedColumnAutoSizingCriteria = "DataGridView_NeedColumnAutoSizingCriteria";

		// Token: 0x04002ABC RID: 10940
		internal const string DataGridView_NewRowNeededDescr = "DataGridView_NewRowNeededDescr";

		// Token: 0x04002ABD RID: 10941
		internal const string DataGridView_NoCurrentCell = "DataGridView_NoCurrentCell";

		// Token: 0x04002ABE RID: 10942
		internal const string DataGridView_NoRoomForDisplayedColumns = "DataGridView_NoRoomForDisplayedColumns";

		// Token: 0x04002ABF RID: 10943
		internal const string DataGridView_NoRoomForDisplayedRows = "DataGridView_NoRoomForDisplayedRows";

		// Token: 0x04002AC0 RID: 10944
		internal const string DataGridView_OperationDisabledInVirtualMode = "DataGridView_OperationDisabledInVirtualMode";

		// Token: 0x04002AC1 RID: 10945
		internal const string DataGridView_PreviousModesHasWrongLength = "DataGridView_PreviousModesHasWrongLength";

		// Token: 0x04002AC2 RID: 10946
		internal const string DataGridView_PropertyMustBeZero = "DataGridView_PropertyMustBeZero";

		// Token: 0x04002AC3 RID: 10947
		internal const string DataGridView_ReadOnlyCollection = "DataGridView_ReadOnlyCollection";

		// Token: 0x04002AC4 RID: 10948
		internal const string DataGridView_ReadOnlyDescr = "DataGridView_ReadOnlyDescr";

		// Token: 0x04002AC5 RID: 10949
		internal const string DataGridView_RowAlreadyBelongsToDataGridView = "DataGridView_RowAlreadyBelongsToDataGridView";

		// Token: 0x04002AC6 RID: 10950
		internal const string DataGridView_RowContextMenuStripChangedDescr = "DataGridView_RowContextMenuStripChangedDescr";

		// Token: 0x04002AC7 RID: 10951
		internal const string DataGridView_RowContextMenuStripDescr = "DataGridView_RowContextMenuStripDescr";

		// Token: 0x04002AC8 RID: 10952
		internal const string DataGridView_RowContextMenuStripNeededDescr = "DataGridView_RowContextMenuStripNeededDescr";

		// Token: 0x04002AC9 RID: 10953
		internal const string DataGridView_RowDefaultCellStyleChangedDescr = "DataGridView_RowDefaultCellStyleChangedDescr";

		// Token: 0x04002ACA RID: 10954
		internal const string DataGridView_RowDefaultCellStyleDescr = "DataGridView_RowDefaultCellStyleDescr";

		// Token: 0x04002ACB RID: 10955
		internal const string DataGridView_RowDirtyStateNeededDescr = "DataGridView_RowDirtyStateNeededDescr";

		// Token: 0x04002ACC RID: 10956
		internal const string DataGridView_RowDividerDoubleClickDescr = "DataGridView_RowDividerDoubleClickDescr";

		// Token: 0x04002ACD RID: 10957
		internal const string DataGridView_RowDividerHeightChangedDescr = "DataGridView_RowDividerHeightChangedDescr";

		// Token: 0x04002ACE RID: 10958
		internal const string DataGridView_RowDividerHeightDescr = "DataGridView_RowDividerHeightDescr";

		// Token: 0x04002ACF RID: 10959
		internal const string DataGridView_RowDoesNotBelongToDataGridView = "DataGridView_RowDoesNotBelongToDataGridView";

		// Token: 0x04002AD0 RID: 10960
		internal const string DataGridView_RowDoesNotYetBelongToDataGridView = "DataGridView_RowDoesNotYetBelongToDataGridView";

		// Token: 0x04002AD1 RID: 10961
		internal const string DataGridView_RowEnterDescr = "DataGridView_RowEnterDescr";

		// Token: 0x04002AD2 RID: 10962
		internal const string DataGridView_RowErrorTextChangedDescr = "DataGridView_RowErrorTextChangedDescr";

		// Token: 0x04002AD3 RID: 10963
		internal const string DataGridView_RowErrorTextDescr = "DataGridView_RowErrorTextDescr";

		// Token: 0x04002AD4 RID: 10964
		internal const string DataGridView_RowErrorTextNeededDescr = "DataGridView_RowErrorTextNeededDescr";

		// Token: 0x04002AD5 RID: 10965
		internal const string DataGridView_RowHeaderCellAccDefaultAction = "DataGridView_RowHeaderCellAccDefaultAction";

		// Token: 0x04002AD6 RID: 10966
		internal const string DataGridView_RowHeaderCellChangedDescr = "DataGridView_RowHeaderCellChangedDescr";

		// Token: 0x04002AD7 RID: 10967
		internal const string DataGridView_RowHeaderMouseClickDescr = "DataGridView_RowHeaderMouseClickDescr";

		// Token: 0x04002AD8 RID: 10968
		internal const string DataGridView_RowHeaderMouseDoubleClickDescr = "DataGridView_RowHeaderMouseDoubleClickDescr";

		// Token: 0x04002AD9 RID: 10969
		internal const string DataGridView_RowHeadersBorderStyleChangedDescr = "DataGridView_RowHeadersBorderStyleChangedDescr";

		// Token: 0x04002ADA RID: 10970
		internal const string DataGridView_RowHeadersBorderStyleDescr = "DataGridView_RowHeadersBorderStyleDescr";

		// Token: 0x04002ADB RID: 10971
		internal const string DataGridView_RowHeadersCannotBeInvisible = "DataGridView_RowHeadersCannotBeInvisible";

		// Token: 0x04002ADC RID: 10972
		internal const string DataGridView_RowHeadersDefaultCellStyleDescr = "DataGridView_RowHeadersDefaultCellStyleDescr";

		// Token: 0x04002ADD RID: 10973
		internal const string DataGridView_RowHeadersWidthDescr = "DataGridView_RowHeadersWidthDescr";

		// Token: 0x04002ADE RID: 10974
		internal const string DataGridView_RowHeadersWidthSizeModeChangedDescr = "DataGridView_RowHeadersWidthSizeModeChangedDescr";

		// Token: 0x04002ADF RID: 10975
		internal const string DataGridView_RowHeadersWidthSizeModeDescr = "DataGridView_RowHeadersWidthSizeModeDescr";

		// Token: 0x04002AE0 RID: 10976
		internal const string DataGridView_RowHeightChangedDescr = "DataGridView_RowHeightChangedDescr";

		// Token: 0x04002AE1 RID: 10977
		internal const string DataGridView_RowHeightDescr = "DataGridView_RowHeightDescr";

		// Token: 0x04002AE2 RID: 10978
		internal const string DataGridView_RowHeightInfoNeededDescr = "DataGridView_RowHeightInfoNeededDescr";

		// Token: 0x04002AE3 RID: 10979
		internal const string DataGridView_RowHeightInfoPushedDescr = "DataGridView_RowHeightInfoPushedDescr";

		// Token: 0x04002AE4 RID: 10980
		internal const string DataGridView_RowLeaveDescr = "DataGridView_RowLeaveDescr";

		// Token: 0x04002AE5 RID: 10981
		internal const string DataGridView_RowMinimumHeightChangedDescr = "DataGridView_RowMinimumHeightChangedDescr";

		// Token: 0x04002AE6 RID: 10982
		internal const string DataGridView_RowMinimumHeightDescr = "DataGridView_RowMinimumHeightDescr";

		// Token: 0x04002AE7 RID: 10983
		internal const string DataGridView_RowMustBeUnshared = "DataGridView_RowMustBeUnshared";

		// Token: 0x04002AE8 RID: 10984
		internal const string DataGridView_RowPostPaintDescr = "DataGridView_RowPostPaintDescr";

		// Token: 0x04002AE9 RID: 10985
		internal const string DataGridView_RowPrePaintDescr = "DataGridView_RowPrePaintDescr";

		// Token: 0x04002AEA RID: 10986
		internal const string DataGridView_RowReadOnlyDescr = "DataGridView_RowReadOnlyDescr";

		// Token: 0x04002AEB RID: 10987
		internal const string DataGridView_RowResizableDescr = "DataGridView_RowResizableDescr";

		// Token: 0x04002AEC RID: 10988
		internal const string DataGridView_RowsAddedDescr = "DataGridView_RowsAddedDescr";

		// Token: 0x04002AED RID: 10989
		internal const string DataGridView_RowsDefaultCellStyleDescr = "DataGridView_RowsDefaultCellStyleDescr";

		// Token: 0x04002AEE RID: 10990
		internal const string DataGridView_RowsRemovedDescr = "DataGridView_RowsRemovedDescr";

		// Token: 0x04002AEF RID: 10991
		internal const string DataGridView_RowStateChangedDescr = "DataGridView_RowStateChangedDescr";

		// Token: 0x04002AF0 RID: 10992
		internal const string DataGridView_RowTemplateDescr = "DataGridView_RowTemplateDescr";

		// Token: 0x04002AF1 RID: 10993
		internal const string DataGridView_RowUnsharedDescr = "DataGridView_RowUnsharedDescr";

		// Token: 0x04002AF2 RID: 10994
		internal const string DataGridView_RowValidatedDescr = "DataGridView_RowValidatedDescr";

		// Token: 0x04002AF3 RID: 10995
		internal const string DataGridView_RowValidatingDescr = "DataGridView_RowValidatingDescr";

		// Token: 0x04002AF4 RID: 10996
		internal const string DataGridView_ScrollBarsDescr = "DataGridView_ScrollBarsDescr";

		// Token: 0x04002AF5 RID: 10997
		internal const string DataGridView_ScrollDescr = "DataGridView_ScrollDescr";

		// Token: 0x04002AF6 RID: 10998
		internal const string DataGridView_SelectionChangedDescr = "DataGridView_SelectionChangedDescr";

		// Token: 0x04002AF7 RID: 10999
		internal const string DataGridView_SelectionModeAndSortModeClash = "DataGridView_SelectionModeAndSortModeClash";

		// Token: 0x04002AF8 RID: 11000
		internal const string DataGridView_SelectionModeDescr = "DataGridView_SelectionModeDescr";

		// Token: 0x04002AF9 RID: 11001
		internal const string DataGridView_SelectionModeReset = "DataGridView_SelectionModeReset";

		// Token: 0x04002AFA RID: 11002
		internal const string DataGridView_SetCurrentCellAddressCoreNotReentrant = "DataGridView_SetCurrentCellAddressCoreNotReentrant";

		// Token: 0x04002AFB RID: 11003
		internal const string DataGridView_ShowCellErrorsDescr = "DataGridView_ShowCellErrorsDescr";

		// Token: 0x04002AFC RID: 11004
		internal const string DataGridView_ShowCellToolTipsDescr = "DataGridView_ShowCellToolTipsDescr";

		// Token: 0x04002AFD RID: 11005
		internal const string DataGridView_ShowEditingIconDescr = "DataGridView_ShowEditingIconDescr";

		// Token: 0x04002AFE RID: 11006
		internal const string DataGridView_ShowRowErrorsDescr = "DataGridView_ShowRowErrorsDescr";

		// Token: 0x04002AFF RID: 11007
		internal const string DataGridView_SizeTooLarge = "DataGridView_SizeTooLarge";

		// Token: 0x04002B00 RID: 11008
		internal const string DataGridView_SortCompareDescr = "DataGridView_SortCompareDescr";

		// Token: 0x04002B01 RID: 11009
		internal const string DataGridView_SortedDescr = "DataGridView_SortedDescr";

		// Token: 0x04002B02 RID: 11010
		internal const string DataGridView_StandardTabDescr = "DataGridView_StandardTabDescr";

		// Token: 0x04002B03 RID: 11011
		internal const string DataGridView_TextBoxColumnMaxInputLengthDescr = "DataGridView_TextBoxColumnMaxInputLengthDescr";

		// Token: 0x04002B04 RID: 11012
		internal const string DataGridView_TransparentColor = "DataGridView_TransparentColor";

		// Token: 0x04002B05 RID: 11013
		internal const string DataGridView_UserAddedRowDescr = "DataGridView_UserAddedRowDescr";

		// Token: 0x04002B06 RID: 11014
		internal const string DataGridView_UserDeletedRowDescr = "DataGridView_UserDeletedRowDescr";

		// Token: 0x04002B07 RID: 11015
		internal const string DataGridView_UserDeletingRowDescr = "DataGridView_UserDeletingRowDescr";

		// Token: 0x04002B08 RID: 11016
		internal const string DataGridView_WeightSumCannotExceedLongMaxValue = "DataGridView_WeightSumCannotExceedLongMaxValue";

		// Token: 0x04002B09 RID: 11017
		internal const string DataGridView_WrongType = "DataGridView_WrongType";

		// Token: 0x04002B0A RID: 11018
		internal const string DataGridViewAlternatingRowsDefaultCellStyleChangedDescr = "DataGridViewAlternatingRowsDefaultCellStyleChangedDescr";

		// Token: 0x04002B0B RID: 11019
		internal const string DataGridViewAutoSizeColumnModeChangedDescr = "DataGridViewAutoSizeColumnModeChangedDescr";

		// Token: 0x04002B0C RID: 11020
		internal const string DataGridViewAutoSizeColumnsModeChangedDescr = "DataGridViewAutoSizeColumnsModeChangedDescr";

		// Token: 0x04002B0D RID: 11021
		internal const string DataGridViewAutoSizeRowsModeChangedDescr = "DataGridViewAutoSizeRowsModeChangedDescr";

		// Token: 0x04002B0E RID: 11022
		internal const string DataGridViewBackgroundColorChangedDescr = "DataGridViewBackgroundColorChangedDescr";

		// Token: 0x04002B0F RID: 11023
		internal const string DataGridViewBackgroundColorDescr = "DataGridViewBackgroundColorDescr";

		// Token: 0x04002B10 RID: 11024
		internal const string DataGridViewBand_CannotSelect = "DataGridViewBand_CannotSelect";

		// Token: 0x04002B11 RID: 11025
		internal const string DataGridViewBand_MinimumHeightSmallerThanOne = "DataGridViewBand_MinimumHeightSmallerThanOne";

		// Token: 0x04002B12 RID: 11026
		internal const string DataGridViewBand_MinimumWidthSmallerThanOne = "DataGridViewBand_MinimumWidthSmallerThanOne";

		// Token: 0x04002B13 RID: 11027
		internal const string DataGridViewBand_NewRowCannotBeInvisible = "DataGridViewBand_NewRowCannotBeInvisible";

		// Token: 0x04002B14 RID: 11028
		internal const string DataGridViewBeginInit = "DataGridViewBeginInit";

		// Token: 0x04002B15 RID: 11029
		internal const string DataGridViewBorderStyleChangedDescr = "DataGridViewBorderStyleChangedDescr";

		// Token: 0x04002B16 RID: 11030
		internal const string DataGridViewCell_CannotSetReadOnlyState = "DataGridViewCell_CannotSetReadOnlyState";

		// Token: 0x04002B17 RID: 11031
		internal const string DataGridViewCell_CannotSetSelectedState = "DataGridViewCell_CannotSetSelectedState";

		// Token: 0x04002B18 RID: 11032
		internal const string DataGridViewCell_FormattedValueHasWrongType = "DataGridViewCell_FormattedValueHasWrongType";

		// Token: 0x04002B19 RID: 11033
		internal const string DataGridViewCell_FormattedValueTypeNull = "DataGridViewCell_FormattedValueTypeNull";

		// Token: 0x04002B1A RID: 11034
		internal const string DataGridViewCell_ValueTypeNull = "DataGridViewCell_ValueTypeNull";

		// Token: 0x04002B1B RID: 11035
		internal const string DataGridViewCellAccessibleObject_OwnerAlreadySet = "DataGridViewCellAccessibleObject_OwnerAlreadySet";

		// Token: 0x04002B1C RID: 11036
		internal const string DataGridViewCellAccessibleObject_OwnerNotSet = "DataGridViewCellAccessibleObject_OwnerNotSet";

		// Token: 0x04002B1D RID: 11037
		internal const string DataGridViewCellCollection_AtLeastOneCellIsNull = "DataGridViewCellCollection_AtLeastOneCellIsNull";

		// Token: 0x04002B1E RID: 11038
		internal const string DataGridViewCellCollection_CannotAddIdenticalCells = "DataGridViewCellCollection_CannotAddIdenticalCells";

		// Token: 0x04002B1F RID: 11039
		internal const string DataGridViewCellCollection_CellAlreadyBelongsToDataGridView = "DataGridViewCellCollection_CellAlreadyBelongsToDataGridView";

		// Token: 0x04002B20 RID: 11040
		internal const string DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow = "DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow";

		// Token: 0x04002B21 RID: 11041
		internal const string DataGridViewCellCollection_CellNotFound = "DataGridViewCellCollection_CellNotFound";

		// Token: 0x04002B22 RID: 11042
		internal const string DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView = "DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView";

		// Token: 0x04002B23 RID: 11043
		internal const string DataGridViewCellStyle_TransparentColorNotAllowed = "DataGridViewCellStyle_TransparentColorNotAllowed";

		// Token: 0x04002B24 RID: 11044
		internal const string DataGridViewCellStyleAlignmentDescr = "DataGridViewCellStyleAlignmentDescr";

		// Token: 0x04002B25 RID: 11045
		internal const string DataGridViewCheckBoxCell_ClipboardChecked = "DataGridViewCheckBoxCell_ClipboardChecked";

		// Token: 0x04002B26 RID: 11046
		internal const string DataGridViewCheckBoxCell_ClipboardFalse = "DataGridViewCheckBoxCell_ClipboardFalse";

		// Token: 0x04002B27 RID: 11047
		internal const string DataGridViewCheckBoxCell_ClipboardIndeterminate = "DataGridViewCheckBoxCell_ClipboardIndeterminate";

		// Token: 0x04002B28 RID: 11048
		internal const string DataGridViewCheckBoxCell_ClipboardTrue = "DataGridViewCheckBoxCell_ClipboardTrue";

		// Token: 0x04002B29 RID: 11049
		internal const string DataGridViewCheckBoxCell_ClipboardUnchecked = "DataGridViewCheckBoxCell_ClipboardUnchecked";

		// Token: 0x04002B2A RID: 11050
		internal const string DataGridViewCheckBoxCell_InvalidValueType = "DataGridViewCheckBoxCell_InvalidValueType";

		// Token: 0x04002B2B RID: 11051
		internal const string DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders = "DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders";

		// Token: 0x04002B2C RID: 11052
		internal const string DataGridViewColumn_AutoSizeModeDescr = "DataGridViewColumn_AutoSizeModeDescr";

		// Token: 0x04002B2D RID: 11053
		internal const string DataGridViewColumn_CellTemplateRequired = "DataGridViewColumn_CellTemplateRequired";

		// Token: 0x04002B2E RID: 11054
		internal const string DataGridViewColumn_DisplayIndexExceedsColumnCount = "DataGridViewColumn_DisplayIndexExceedsColumnCount";

		// Token: 0x04002B2F RID: 11055
		internal const string DataGridViewColumn_DisplayIndexNegative = "DataGridViewColumn_DisplayIndexNegative";

		// Token: 0x04002B30 RID: 11056
		internal const string DataGridViewColumn_DisplayIndexTooLarge = "DataGridViewColumn_DisplayIndexTooLarge";

		// Token: 0x04002B31 RID: 11057
		internal const string DataGridViewColumn_DisplayIndexTooNegative = "DataGridViewColumn_DisplayIndexTooNegative";

		// Token: 0x04002B32 RID: 11058
		internal const string DataGridViewColumn_FillWeightDescr = "DataGridViewColumn_FillWeightDescr";

		// Token: 0x04002B33 RID: 11059
		internal const string DataGridViewColumn_FrozenColumnCannotAutoFill = "DataGridViewColumn_FrozenColumnCannotAutoFill";

		// Token: 0x04002B34 RID: 11060
		internal const string DataGridViewColumn_SortModeAndSelectionModeClash = "DataGridViewColumn_SortModeAndSelectionModeClash";

		// Token: 0x04002B35 RID: 11061
		internal const string DataGridViewColumnCollection_ColumnNotFound = "DataGridViewColumnCollection_ColumnNotFound";

		// Token: 0x04002B36 RID: 11062
		internal const string DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash = "DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash";

		// Token: 0x04002B37 RID: 11063
		internal const string DataGridViewColumnHeadersDefaultCellStyleChangedDescr = "DataGridViewColumnHeadersDefaultCellStyleChangedDescr";

		// Token: 0x04002B38 RID: 11064
		internal const string DataGridViewColumnHeadersHeightChangedDescr = "DataGridViewColumnHeadersHeightChangedDescr";

		// Token: 0x04002B39 RID: 11065
		internal const string DataGridViewColumnHeadersVisibleDescr = "DataGridViewColumnHeadersVisibleDescr";

		// Token: 0x04002B3A RID: 11066
		internal const string DataGridViewColumnSortModeChangedDescr = "DataGridViewColumnSortModeChangedDescr";

		// Token: 0x04002B3B RID: 11067
		internal const string DataGridViewComboBoxCell_DropDownWidthOutOfRange = "DataGridViewComboBoxCell_DropDownWidthOutOfRange";

		// Token: 0x04002B3C RID: 11068
		internal const string DataGridViewComboBoxCell_FieldNotFound = "DataGridViewComboBoxCell_FieldNotFound";

		// Token: 0x04002B3D RID: 11069
		internal const string DataGridViewComboBoxCell_InvalidValue = "DataGridViewComboBoxCell_InvalidValue";

		// Token: 0x04002B3E RID: 11070
		internal const string DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange = "DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange";

		// Token: 0x04002B3F RID: 11071
		internal const string DataGridViewDataMemberChangedDescr = "DataGridViewDataMemberChangedDescr";

		// Token: 0x04002B40 RID: 11072
		internal const string DataGridViewDataMemberDescr = "DataGridViewDataMemberDescr";

		// Token: 0x04002B41 RID: 11073
		internal const string DataGridViewDataSourceChangedDescr = "DataGridViewDataSourceChangedDescr";

		// Token: 0x04002B42 RID: 11074
		internal const string DataGridViewDataSourceDescr = "DataGridViewDataSourceDescr";

		// Token: 0x04002B43 RID: 11075
		internal const string DataGridViewDefaultCellStyleChangedDescr = "DataGridViewDefaultCellStyleChangedDescr";

		// Token: 0x04002B44 RID: 11076
		internal const string DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange = "DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange";

		// Token: 0x04002B45 RID: 11077
		internal const string DataGridViewElementPaintingEventArgs_RowIndexOutOfRange = "DataGridViewElementPaintingEventArgs_RowIndexOutOfRange";

		// Token: 0x04002B46 RID: 11078
		internal const string DataGridViewGridColorDescr = "DataGridViewGridColorDescr";

		// Token: 0x04002B47 RID: 11079
		internal const string DataGridViewImageColumn_DescriptionDescr = "DataGridViewImageColumn_DescriptionDescr";

		// Token: 0x04002B48 RID: 11080
		internal const string DataGridViewImageColumn_IconDescr = "DataGridViewImageColumn_IconDescr";

		// Token: 0x04002B49 RID: 11081
		internal const string DataGridViewImageColumn_ImageDescr = "DataGridViewImageColumn_ImageDescr";

		// Token: 0x04002B4A RID: 11082
		internal const string DataGridViewImageColumn_ImageLayoutDescr = "DataGridViewImageColumn_ImageLayoutDescr";

		// Token: 0x04002B4B RID: 11083
		internal const string DataGridViewImageColumn_PaddingDescr = "DataGridViewImageColumn_PaddingDescr";

		// Token: 0x04002B4C RID: 11084
		internal const string DataGridViewImageColumn_ValuesAreIconsDescr = "DataGridViewImageColumn_ValuesAreIconsDescr";

		// Token: 0x04002B4D RID: 11085
		internal const string DataGridViewOnAllowUserToAddRowsChangedDescr = "DataGridViewOnAllowUserToAddRowsChangedDescr";

		// Token: 0x04002B4E RID: 11086
		internal const string DataGridViewOnAllowUserToDeleteRowsChangedDescr = "DataGridViewOnAllowUserToDeleteRowsChangedDescr";

		// Token: 0x04002B4F RID: 11087
		internal const string DataGridViewOnAllowUserToOrderColumnsChangedDescr = "DataGridViewOnAllowUserToOrderColumnsChangedDescr";

		// Token: 0x04002B50 RID: 11088
		internal const string DataGridViewOnAllowUserToResizeColumnsChangedDescr = "DataGridViewOnAllowUserToResizeColumnsChangedDescr";

		// Token: 0x04002B51 RID: 11089
		internal const string DataGridViewOnAllowUserToResizeRowsChangedDescr = "DataGridViewOnAllowUserToResizeRowsChangedDescr";

		// Token: 0x04002B52 RID: 11090
		internal const string DataGridViewOnGridColorChangedDescr = "DataGridViewOnGridColorChangedDescr";

		// Token: 0x04002B53 RID: 11091
		internal const string DataGridViewOnMultiSelectChangedDescr = "DataGridViewOnMultiSelectChangedDescr";

		// Token: 0x04002B54 RID: 11092
		internal const string DataGridViewOnReadOnlyChangedDescr = "DataGridViewOnReadOnlyChangedDescr";

		// Token: 0x04002B55 RID: 11093
		internal const string DataGridViewRowAccessibleObject_OwnerAlreadySet = "DataGridViewRowAccessibleObject_OwnerAlreadySet";

		// Token: 0x04002B56 RID: 11094
		internal const string DataGridViewRowAccessibleObject_OwnerNotSet = "DataGridViewRowAccessibleObject_OwnerNotSet";

		// Token: 0x04002B57 RID: 11095
		internal const string DataGridViewRowCollection_AddUnboundRow = "DataGridViewRowCollection_AddUnboundRow";

		// Token: 0x04002B58 RID: 11096
		internal const string DataGridViewRowCollection_CannotAddOrInsertSelectedRow = "DataGridViewRowCollection_CannotAddOrInsertSelectedRow";

		// Token: 0x04002B59 RID: 11097
		internal const string DataGridViewRowCollection_CannotDeleteNewRow = "DataGridViewRowCollection_CannotDeleteNewRow";

		// Token: 0x04002B5A RID: 11098
		internal const string DataGridViewRowCollection_CantClearRowCollectionWithWrongSource = "DataGridViewRowCollection_CantClearRowCollectionWithWrongSource";

		// Token: 0x04002B5B RID: 11099
		internal const string DataGridViewRowCollection_CantRemoveRowsWithWrongSource = "DataGridViewRowCollection_CantRemoveRowsWithWrongSource";

		// Token: 0x04002B5C RID: 11100
		internal const string DataGridViewRowCollection_CountOutOfRange = "DataGridViewRowCollection_CountOutOfRange";

		// Token: 0x04002B5D RID: 11101
		internal const string DataGridViewRowCollection_EnumFinished = "DataGridViewRowCollection_EnumFinished";

		// Token: 0x04002B5E RID: 11102
		internal const string DataGridViewRowCollection_EnumNotStarted = "DataGridViewRowCollection_EnumNotStarted";

		// Token: 0x04002B5F RID: 11103
		internal const string DataGridViewRowCollection_IndexDestinationOutOfRange = "DataGridViewRowCollection_IndexDestinationOutOfRange";

		// Token: 0x04002B60 RID: 11104
		internal const string DataGridViewRowCollection_IndexSourceOutOfRange = "DataGridViewRowCollection_IndexSourceOutOfRange";

		// Token: 0x04002B61 RID: 11105
		internal const string DataGridViewRowCollection_NoColumns = "DataGridViewRowCollection_NoColumns";

		// Token: 0x04002B62 RID: 11106
		internal const string DataGridViewRowCollection_NoInsertionAfterNewRow = "DataGridViewRowCollection_NoInsertionAfterNewRow";

		// Token: 0x04002B63 RID: 11107
		internal const string DataGridViewRowCollection_NoRowToDuplicate = "DataGridViewRowCollection_NoRowToDuplicate";

		// Token: 0x04002B64 RID: 11108
		internal const string DataGridViewRowCollection_RowIndexOutOfRange = "DataGridViewRowCollection_RowIndexOutOfRange";

		// Token: 0x04002B65 RID: 11109
		internal const string DataGridViewRowCollection_RowTemplateTooManyCells = "DataGridViewRowCollection_RowTemplateTooManyCells";

		// Token: 0x04002B66 RID: 11110
		internal const string DataGridViewRowCollection_TooManyCells = "DataGridViewRowCollection_TooManyCells";

		// Token: 0x04002B67 RID: 11111
		internal const string DataGridViewRowHeadersDefaultCellStyleChangedDescr = "DataGridViewRowHeadersDefaultCellStyleChangedDescr";

		// Token: 0x04002B68 RID: 11112
		internal const string DataGridViewRowHeadersVisibleDescr = "DataGridViewRowHeadersVisibleDescr";

		// Token: 0x04002B69 RID: 11113
		internal const string DataGridViewRowHeadersWidthChangedDescr = "DataGridViewRowHeadersWidthChangedDescr";

		// Token: 0x04002B6A RID: 11114
		internal const string DataGridViewRowsDefaultCellStyleChangedDescr = "DataGridViewRowsDefaultCellStyleChangedDescr";

		// Token: 0x04002B6B RID: 11115
		internal const string DataGridViewTopRowAccessibleObject_OwnerAlreadySet = "DataGridViewTopRowAccessibleObject_OwnerAlreadySet";

		// Token: 0x04002B6C RID: 11116
		internal const string DataGridViewTopRowAccessibleObject_OwnerNotSet = "DataGridViewTopRowAccessibleObject_OwnerNotSet";

		// Token: 0x04002B6D RID: 11117
		internal const string DataGridViewTypeColumn_WrongCellTemplateType = "DataGridViewTypeColumn_WrongCellTemplateType";

		// Token: 0x04002B6E RID: 11118
		internal const string DataGridViewVirtualModeDescr = "DataGridViewVirtualModeDescr";

		// Token: 0x04002B6F RID: 11119
		internal const string DataGridVisibleColumnCountDescr = "DataGridVisibleColumnCountDescr";

		// Token: 0x04002B70 RID: 11120
		internal const string DataGridVisibleRowCountDescr = "DataGridVisibleRowCountDescr";

		// Token: 0x04002B71 RID: 11121
		internal const string DataObjectDibNotSupported = "DataObjectDibNotSupported";

		// Token: 0x04002B72 RID: 11122
		internal const string DataSourceDataMemberPropNotFound = "DataSourceDataMemberPropNotFound";

		// Token: 0x04002B73 RID: 11123
		internal const string DataSourceLocksItems = "DataSourceLocksItems";

		// Token: 0x04002B74 RID: 11124
		internal const string DataStreamWrite = "DataStreamWrite";

		// Token: 0x04002B75 RID: 11125
		internal const string DateTimePickerCalendarFontDescr = "DateTimePickerCalendarFontDescr";

		// Token: 0x04002B76 RID: 11126
		internal const string DateTimePickerCalendarForeColorDescr = "DateTimePickerCalendarForeColorDescr";

		// Token: 0x04002B77 RID: 11127
		internal const string DateTimePickerCalendarMonthBackgroundDescr = "DateTimePickerCalendarMonthBackgroundDescr";

		// Token: 0x04002B78 RID: 11128
		internal const string DateTimePickerCalendarTitleBackColorDescr = "DateTimePickerCalendarTitleBackColorDescr";

		// Token: 0x04002B79 RID: 11129
		internal const string DateTimePickerCalendarTitleForeColorDescr = "DateTimePickerCalendarTitleForeColorDescr";

		// Token: 0x04002B7A RID: 11130
		internal const string DateTimePickerCalendarTrailingForeColorDescr = "DateTimePickerCalendarTrailingForeColorDescr";

		// Token: 0x04002B7B RID: 11131
		internal const string DateTimePickerCheckedDescr = "DateTimePickerCheckedDescr";

		// Token: 0x04002B7C RID: 11132
		internal const string DateTimePickerCustomFormatDescr = "DateTimePickerCustomFormatDescr";

		// Token: 0x04002B7D RID: 11133
		internal const string DateTimePickerDropDownAlignDescr = "DateTimePickerDropDownAlignDescr";

		// Token: 0x04002B7E RID: 11134
		internal const string DateTimePickerFormatDescr = "DateTimePickerFormatDescr";

		// Token: 0x04002B7F RID: 11135
		internal const string DateTimePickerMaxDate = "DateTimePickerMaxDate";

		// Token: 0x04002B80 RID: 11136
		internal const string DateTimePickerMaxDateDescr = "DateTimePickerMaxDateDescr";

		// Token: 0x04002B81 RID: 11137
		internal const string DateTimePickerMinDate = "DateTimePickerMinDate";

		// Token: 0x04002B82 RID: 11138
		internal const string DateTimePickerMinDateDescr = "DateTimePickerMinDateDescr";

		// Token: 0x04002B83 RID: 11139
		internal const string DateTimePickerOnCloseUpDescr = "DateTimePickerOnCloseUpDescr";

		// Token: 0x04002B84 RID: 11140
		internal const string DateTimePickerOnDropDownDescr = "DateTimePickerOnDropDownDescr";

		// Token: 0x04002B85 RID: 11141
		internal const string DateTimePickerOnFormatChangedDescr = "DateTimePickerOnFormatChangedDescr";

		// Token: 0x04002B86 RID: 11142
		internal const string DateTimePickerShowNoneDescr = "DateTimePickerShowNoneDescr";

		// Token: 0x04002B87 RID: 11143
		internal const string DateTimePickerShowUpDownDescr = "DateTimePickerShowUpDownDescr";

		// Token: 0x04002B88 RID: 11144
		internal const string DateTimePickerValueDescr = "DateTimePickerValueDescr";

		// Token: 0x04002B89 RID: 11145
		internal const string DebuggingExceptionOnly = "DebuggingExceptionOnly";

		// Token: 0x04002B8A RID: 11146
		internal const string DefManifestNotFound = "DefManifestNotFound";

		// Token: 0x04002B8B RID: 11147
		internal const string DescriptionBindingNavigator = "DescriptionBindingNavigator";

		// Token: 0x04002B8C RID: 11148
		internal const string DescriptionBindingSource = "DescriptionBindingSource";

		// Token: 0x04002B8D RID: 11149
		internal const string DescriptionButton = "DescriptionButton";

		// Token: 0x04002B8E RID: 11150
		internal const string DescriptionCheckBox = "DescriptionCheckBox";

		// Token: 0x04002B8F RID: 11151
		internal const string DescriptionCheckedListBox = "DescriptionCheckedListBox";

		// Token: 0x04002B90 RID: 11152
		internal const string DescriptionColorDialog = "DescriptionColorDialog";

		// Token: 0x04002B91 RID: 11153
		internal const string DescriptionComboBox = "DescriptionComboBox";

		// Token: 0x04002B92 RID: 11154
		internal const string DescriptionContextMenuStrip = "DescriptionContextMenuStrip";

		// Token: 0x04002B93 RID: 11155
		internal const string DescriptionDataGridView = "DescriptionDataGridView";

		// Token: 0x04002B94 RID: 11156
		internal const string DescriptionDateTimePicker = "DescriptionDateTimePicker";

		// Token: 0x04002B95 RID: 11157
		internal const string DescriptionDomainUpDown = "DescriptionDomainUpDown";

		// Token: 0x04002B96 RID: 11158
		internal const string DescriptionErrorProvider = "DescriptionErrorProvider";

		// Token: 0x04002B97 RID: 11159
		internal const string DescriptionFlowLayoutPanel = "DescriptionFlowLayoutPanel";

		// Token: 0x04002B98 RID: 11160
		internal const string DescriptionFolderBrowserDialog = "DescriptionFolderBrowserDialog";

		// Token: 0x04002B99 RID: 11161
		internal const string DescriptionFontDialog = "DescriptionFontDialog";

		// Token: 0x04002B9A RID: 11162
		internal const string DescriptionGroupBox = "DescriptionGroupBox";

		// Token: 0x04002B9B RID: 11163
		internal const string DescriptionHelpProvider = "DescriptionHelpProvider";

		// Token: 0x04002B9C RID: 11164
		internal const string DescriptionHScrollBar = "DescriptionHScrollBar";

		// Token: 0x04002B9D RID: 11165
		internal const string DescriptionImageList = "DescriptionImageList";

		// Token: 0x04002B9E RID: 11166
		internal const string DescriptionLabel = "DescriptionLabel";

		// Token: 0x04002B9F RID: 11167
		internal const string DescriptionLinkLabel = "DescriptionLinkLabel";

		// Token: 0x04002BA0 RID: 11168
		internal const string DescriptionListBox = "DescriptionListBox";

		// Token: 0x04002BA1 RID: 11169
		internal const string DescriptionListView = "DescriptionListView";

		// Token: 0x04002BA2 RID: 11170
		internal const string DescriptionMaskedTextBox = "DescriptionMaskedTextBox";

		// Token: 0x04002BA3 RID: 11171
		internal const string DescriptionMenuStrip = "DescriptionMenuStrip";

		// Token: 0x04002BA4 RID: 11172
		internal const string DescriptionMonthCalendar = "DescriptionMonthCalendar";

		// Token: 0x04002BA5 RID: 11173
		internal const string DescriptionNotifyIcon = "DescriptionNotifyIcon";

		// Token: 0x04002BA6 RID: 11174
		internal const string DescriptionNumericUpDown = "DescriptionNumericUpDown";

		// Token: 0x04002BA7 RID: 11175
		internal const string DescriptionOpenFileDialog = "DescriptionOpenFileDialog";

		// Token: 0x04002BA8 RID: 11176
		internal const string DescriptionPageSetupDialog = "DescriptionPageSetupDialog";

		// Token: 0x04002BA9 RID: 11177
		internal const string DescriptionPanel = "DescriptionPanel";

		// Token: 0x04002BAA RID: 11178
		internal const string DescriptionPictureBox = "DescriptionPictureBox";

		// Token: 0x04002BAB RID: 11179
		internal const string DescriptionPrintDialog = "DescriptionPrintDialog";

		// Token: 0x04002BAC RID: 11180
		internal const string DescriptionPrintPreviewControl = "DescriptionPrintPreviewControl";

		// Token: 0x04002BAD RID: 11181
		internal const string DescriptionPrintPreviewDialog = "DescriptionPrintPreviewDialog";

		// Token: 0x04002BAE RID: 11182
		internal const string DescriptionProgressBar = "DescriptionProgressBar";

		// Token: 0x04002BAF RID: 11183
		internal const string DescriptionPropertyGrid = "DescriptionPropertyGrid";

		// Token: 0x04002BB0 RID: 11184
		internal const string DescriptionRadioButton = "DescriptionRadioButton";

		// Token: 0x04002BB1 RID: 11185
		internal const string DescriptionRichTextBox = "DescriptionRichTextBox";

		// Token: 0x04002BB2 RID: 11186
		internal const string DescriptionSaveFileDialog = "DescriptionSaveFileDialog";

		// Token: 0x04002BB3 RID: 11187
		internal const string DescriptionSplitContainer = "DescriptionSplitContainer";

		// Token: 0x04002BB4 RID: 11188
		internal const string DescriptionSplitter = "DescriptionSplitter";

		// Token: 0x04002BB5 RID: 11189
		internal const string DescriptionStatusStrip = "DescriptionStatusStrip";

		// Token: 0x04002BB6 RID: 11190
		internal const string DescriptionTabControl = "DescriptionTabControl";

		// Token: 0x04002BB7 RID: 11191
		internal const string DescriptionTableLayoutPanel = "DescriptionTableLayoutPanel";

		// Token: 0x04002BB8 RID: 11192
		internal const string DescriptionTextBox = "DescriptionTextBox";

		// Token: 0x04002BB9 RID: 11193
		internal const string DescriptionTimer = "DescriptionTimer";

		// Token: 0x04002BBA RID: 11194
		internal const string DescriptionToolStrip = "DescriptionToolStrip";

		// Token: 0x04002BBB RID: 11195
		internal const string DescriptionToolTip = "DescriptionToolTip";

		// Token: 0x04002BBC RID: 11196
		internal const string DescriptionTrackBar = "DescriptionTrackBar";

		// Token: 0x04002BBD RID: 11197
		internal const string DescriptionTreeView = "DescriptionTreeView";

		// Token: 0x04002BBE RID: 11198
		internal const string DescriptionVScrollBar = "DescriptionVScrollBar";

		// Token: 0x04002BBF RID: 11199
		internal const string DescriptionWebBrowser = "DescriptionWebBrowser";

		// Token: 0x04002BC0 RID: 11200
		internal const string DispInvokeFailed = "DispInvokeFailed";

		// Token: 0x04002BC1 RID: 11201
		internal const string DomainUpDownItemsDescr = "DomainUpDownItemsDescr";

		// Token: 0x04002BC2 RID: 11202
		internal const string DomainUpDownOnSelectedItemChangedDescr = "DomainUpDownOnSelectedItemChangedDescr";

		// Token: 0x04002BC3 RID: 11203
		internal const string DomainUpDownSelectedIndexDescr = "DomainUpDownSelectedIndexDescr";

		// Token: 0x04002BC4 RID: 11204
		internal const string DomainUpDownSelectedItemDescr = "DomainUpDownSelectedItemDescr";

		// Token: 0x04002BC5 RID: 11205
		internal const string DomainUpDownSortedDescr = "DomainUpDownSortedDescr";

		// Token: 0x04002BC6 RID: 11206
		internal const string DomainUpDownWrapDescr = "DomainUpDownWrapDescr";

		// Token: 0x04002BC7 RID: 11207
		internal const string DragDropRegFailed = "DragDropRegFailed";

		// Token: 0x04002BC8 RID: 11208
		internal const string drawItemEventDescr = "drawItemEventDescr";

		// Token: 0x04002BC9 RID: 11209
		internal const string ErrorBadInputLanguage = "ErrorBadInputLanguage";

		// Token: 0x04002BCA RID: 11210
		internal const string ErrorCollectionFull = "ErrorCollectionFull";

		// Token: 0x04002BCB RID: 11211
		internal const string ErrorCreatingHandle = "ErrorCreatingHandle";

		// Token: 0x04002BCC RID: 11212
		internal const string ErrorNoMarshalingThread = "ErrorNoMarshalingThread";

		// Token: 0x04002BCD RID: 11213
		internal const string ErrorPropertyPageFailed = "ErrorPropertyPageFailed";

		// Token: 0x04002BCE RID: 11214
		internal const string ErrorProviderBlinkRateDescr = "ErrorProviderBlinkRateDescr";

		// Token: 0x04002BCF RID: 11215
		internal const string ErrorProviderBlinkStyleDescr = "ErrorProviderBlinkStyleDescr";

		// Token: 0x04002BD0 RID: 11216
		internal const string ErrorProviderContainerControlDescr = "ErrorProviderContainerControlDescr";

		// Token: 0x04002BD1 RID: 11217
		internal const string ErrorProviderDataMemberDescr = "ErrorProviderDataMemberDescr";

		// Token: 0x04002BD2 RID: 11218
		internal const string ErrorProviderDataSourceDescr = "ErrorProviderDataSourceDescr";

		// Token: 0x04002BD3 RID: 11219
		internal const string ErrorProviderErrorDescr = "ErrorProviderErrorDescr";

		// Token: 0x04002BD4 RID: 11220
		internal const string ErrorProviderIconAlignmentDescr = "ErrorProviderIconAlignmentDescr";

		// Token: 0x04002BD5 RID: 11221
		internal const string ErrorProviderIconDescr = "ErrorProviderIconDescr";

		// Token: 0x04002BD6 RID: 11222
		internal const string ErrorProviderIconPaddingDescr = "ErrorProviderIconPaddingDescr";

		// Token: 0x04002BD7 RID: 11223
		internal const string ErrorSettingWindowRegion = "ErrorSettingWindowRegion";

		// Token: 0x04002BD8 RID: 11224
		internal const string ErrorTypeConverterFailed = "ErrorTypeConverterFailed";

		// Token: 0x04002BD9 RID: 11225
		internal const string ExceptionCreatingCompEditorControl = "ExceptionCreatingCompEditorControl";

		// Token: 0x04002BDA RID: 11226
		internal const string ExceptionCreatingObject = "ExceptionCreatingObject";

		// Token: 0x04002BDB RID: 11227
		internal const string ExDlgCancel = "ExDlgCancel";

		// Token: 0x04002BDC RID: 11228
		internal const string ExDlgCaption = "ExDlgCaption";

		// Token: 0x04002BDD RID: 11229
		internal const string ExDlgCaption2 = "ExDlgCaption2";

		// Token: 0x04002BDE RID: 11230
		internal const string ExDlgContinue = "ExDlgContinue";

		// Token: 0x04002BDF RID: 11231
		internal const string ExDlgContinueErrorText = "ExDlgContinueErrorText";

		// Token: 0x04002BE0 RID: 11232
		internal const string ExDlgDetailsText = "ExDlgDetailsText";

		// Token: 0x04002BE1 RID: 11233
		internal const string ExDlgErrorText = "ExDlgErrorText";

		// Token: 0x04002BE2 RID: 11234
		internal const string ExDlgHelp = "ExDlgHelp";

		// Token: 0x04002BE3 RID: 11235
		internal const string ExDlgMsgExceptionSection = "ExDlgMsgExceptionSection";

		// Token: 0x04002BE4 RID: 11236
		internal const string ExDlgMsgFooterNonSwitchable = "ExDlgMsgFooterNonSwitchable";

		// Token: 0x04002BE5 RID: 11237
		internal const string ExDlgMsgFooterSwitchable = "ExDlgMsgFooterSwitchable";

		// Token: 0x04002BE6 RID: 11238
		internal const string ExDlgMsgHeaderNonSwitchable = "ExDlgMsgHeaderNonSwitchable";

		// Token: 0x04002BE7 RID: 11239
		internal const string ExDlgMsgHeaderSwitchable = "ExDlgMsgHeaderSwitchable";

		// Token: 0x04002BE8 RID: 11240
		internal const string ExDlgMsgJITDebuggingSection = "ExDlgMsgJITDebuggingSection";

		// Token: 0x04002BE9 RID: 11241
		internal const string ExDlgMsgLoadedAssembliesEntry = "ExDlgMsgLoadedAssembliesEntry";

		// Token: 0x04002BEA RID: 11242
		internal const string ExDlgMsgLoadedAssembliesSection = "ExDlgMsgLoadedAssembliesSection";

		// Token: 0x04002BEB RID: 11243
		internal const string ExDlgMsgSectionSeperator = "ExDlgMsgSectionSeperator";

		// Token: 0x04002BEC RID: 11244
		internal const string ExDlgMsgSeperator = "ExDlgMsgSeperator";

		// Token: 0x04002BED RID: 11245
		internal const string ExDlgOk = "ExDlgOk";

		// Token: 0x04002BEE RID: 11246
		internal const string ExDlgQuit = "ExDlgQuit";

		// Token: 0x04002BEF RID: 11247
		internal const string ExDlgSecurityContinueErrorText = "ExDlgSecurityContinueErrorText";

		// Token: 0x04002BF0 RID: 11248
		internal const string ExDlgSecurityErrorText = "ExDlgSecurityErrorText";

		// Token: 0x04002BF1 RID: 11249
		internal const string ExDlgShowDetails = "ExDlgShowDetails";

		// Token: 0x04002BF2 RID: 11250
		internal const string ExDlgWarningText = "ExDlgWarningText";

		// Token: 0x04002BF3 RID: 11251
		internal const string ExternalException = "ExternalException";

		// Token: 0x04002BF4 RID: 11252
		internal const string FDaddExtensionDescr = "FDaddExtensionDescr";

		// Token: 0x04002BF5 RID: 11253
		internal const string FDcheckFileExistsDescr = "FDcheckFileExistsDescr";

		// Token: 0x04002BF6 RID: 11254
		internal const string FDcheckPathExistsDescr = "FDcheckPathExistsDescr";

		// Token: 0x04002BF7 RID: 11255
		internal const string FDdefaultExtDescr = "FDdefaultExtDescr";

		// Token: 0x04002BF8 RID: 11256
		internal const string FDdereferenceLinksDescr = "FDdereferenceLinksDescr";

		// Token: 0x04002BF9 RID: 11257
		internal const string FDfileNameDescr = "FDfileNameDescr";

		// Token: 0x04002BFA RID: 11258
		internal const string FDFileNamesDescr = "FDFileNamesDescr";

		// Token: 0x04002BFB RID: 11259
		internal const string FDfileOkDescr = "FDfileOkDescr";

		// Token: 0x04002BFC RID: 11260
		internal const string FDfilterDescr = "FDfilterDescr";

		// Token: 0x04002BFD RID: 11261
		internal const string FDfilterIndexDescr = "FDfilterIndexDescr";

		// Token: 0x04002BFE RID: 11262
		internal const string FDinitialDirDescr = "FDinitialDirDescr";

		// Token: 0x04002BFF RID: 11263
		internal const string FDrestoreDirectoryDescr = "FDrestoreDirectoryDescr";

		// Token: 0x04002C00 RID: 11264
		internal const string FDshowHelpDescr = "FDshowHelpDescr";

		// Token: 0x04002C01 RID: 11265
		internal const string FDsupportMultiDottedExtensionsDescr = "FDsupportMultiDottedExtensionsDescr";

		// Token: 0x04002C02 RID: 11266
		internal const string FDtitleDescr = "FDtitleDescr";

		// Token: 0x04002C03 RID: 11267
		internal const string FDvalidateNamesDescr = "FDvalidateNamesDescr";

		// Token: 0x04002C04 RID: 11268
		internal const string FileDialogBufferTooSmall = "FileDialogBufferTooSmall";

		// Token: 0x04002C05 RID: 11269
		internal const string FileDialogCreatePrompt = "FileDialogCreatePrompt";

		// Token: 0x04002C06 RID: 11270
		internal const string FileDialogFileNotFound = "FileDialogFileNotFound";

		// Token: 0x04002C07 RID: 11271
		internal const string FileDialogInvalidFileName = "FileDialogInvalidFileName";

		// Token: 0x04002C08 RID: 11272
		internal const string FileDialogInvalidFilter = "FileDialogInvalidFilter";

		// Token: 0x04002C09 RID: 11273
		internal const string FileDialogInvalidFilterIndex = "FileDialogInvalidFilterIndex";

		// Token: 0x04002C0A RID: 11274
		internal const string FileDialogOverwritePrompt = "FileDialogOverwritePrompt";

		// Token: 0x04002C0B RID: 11275
		internal const string FileDialogSubLassFailure = "FileDialogSubLassFailure";

		// Token: 0x04002C0C RID: 11276
		internal const string FilterRequiresIBindingListView = "FilterRequiresIBindingListView";

		// Token: 0x04002C0D RID: 11277
		internal const string FindKeyMayNotBeEmptyOrNull = "FindKeyMayNotBeEmptyOrNull";

		// Token: 0x04002C0E RID: 11278
		internal const string FlowPanelFlowDirectionDescr = "FlowPanelFlowDirectionDescr";

		// Token: 0x04002C0F RID: 11279
		internal const string FlowPanelWrapContentsDescr = "FlowPanelWrapContentsDescr";

		// Token: 0x04002C10 RID: 11280
		internal const string FnDallowScriptChangeDescr = "FnDallowScriptChangeDescr";

		// Token: 0x04002C11 RID: 11281
		internal const string FnDallowSimulationsDescr = "FnDallowSimulationsDescr";

		// Token: 0x04002C12 RID: 11282
		internal const string FnDallowVectorFontsDescr = "FnDallowVectorFontsDescr";

		// Token: 0x04002C13 RID: 11283
		internal const string FnDallowVerticalFontsDescr = "FnDallowVerticalFontsDescr";

		// Token: 0x04002C14 RID: 11284
		internal const string FnDapplyDescr = "FnDapplyDescr";

		// Token: 0x04002C15 RID: 11285
		internal const string FnDcolorDescr = "FnDcolorDescr";

		// Token: 0x04002C16 RID: 11286
		internal const string FnDfixedPitchOnlyDescr = "FnDfixedPitchOnlyDescr";

		// Token: 0x04002C17 RID: 11287
		internal const string FnDfontDescr = "FnDfontDescr";

		// Token: 0x04002C18 RID: 11288
		internal const string FnDfontMustExistDescr = "FnDfontMustExistDescr";

		// Token: 0x04002C19 RID: 11289
		internal const string FnDmaxSizeDescr = "FnDmaxSizeDescr";

		// Token: 0x04002C1A RID: 11290
		internal const string FnDminSizeDescr = "FnDminSizeDescr";

		// Token: 0x04002C1B RID: 11291
		internal const string FnDscriptsOnlyDescr = "FnDscriptsOnlyDescr";

		// Token: 0x04002C1C RID: 11292
		internal const string FnDshowApplyDescr = "FnDshowApplyDescr";

		// Token: 0x04002C1D RID: 11293
		internal const string FnDshowColorDescr = "FnDshowColorDescr";

		// Token: 0x04002C1E RID: 11294
		internal const string FnDshowEffectsDescr = "FnDshowEffectsDescr";

		// Token: 0x04002C1F RID: 11295
		internal const string FnDshowHelpDescr = "FnDshowHelpDescr";

		// Token: 0x04002C20 RID: 11296
		internal const string FolderBrowserDialogDescription = "FolderBrowserDialogDescription";

		// Token: 0x04002C21 RID: 11297
		internal const string FolderBrowserDialogNoRootFolder = "FolderBrowserDialogNoRootFolder";

		// Token: 0x04002C22 RID: 11298
		internal const string FolderBrowserDialogRootFolder = "FolderBrowserDialogRootFolder";

		// Token: 0x04002C23 RID: 11299
		internal const string FolderBrowserDialogSelectedPath = "FolderBrowserDialogSelectedPath";

		// Token: 0x04002C24 RID: 11300
		internal const string FolderBrowserDialogShowNewFolderButton = "FolderBrowserDialogShowNewFolderButton";

		// Token: 0x04002C25 RID: 11301
		internal const string FormAcceptButtonDescr = "FormAcceptButtonDescr";

		// Token: 0x04002C26 RID: 11302
		internal const string FormActiveMDIChildDescr = "FormActiveMDIChildDescr";

		// Token: 0x04002C27 RID: 11303
		internal const string FormatControlFormatDescr = "FormatControlFormatDescr";

		// Token: 0x04002C28 RID: 11304
		internal const string Formatter_CantConvert = "Formatter_CantConvert";

		// Token: 0x04002C29 RID: 11305
		internal const string Formatter_CantConvertNull = "Formatter_CantConvertNull";

		// Token: 0x04002C2A RID: 11306
		internal const string FormAutoScaleDescr = "FormAutoScaleDescr";

		// Token: 0x04002C2B RID: 11307
		internal const string FormAutoScrollDescr = "FormAutoScrollDescr";

		// Token: 0x04002C2C RID: 11308
		internal const string FormAutoScrollMarginDescr = "FormAutoScrollMarginDescr";

		// Token: 0x04002C2D RID: 11309
		internal const string FormAutoScrollMinSizeDescr = "FormAutoScrollMinSizeDescr";

		// Token: 0x04002C2E RID: 11310
		internal const string FormAutoScrollPositionDescr = "FormAutoScrollPositionDescr";

		// Token: 0x04002C2F RID: 11311
		internal const string FormBorderStyleDescr = "FormBorderStyleDescr";

		// Token: 0x04002C30 RID: 11312
		internal const string FormCancelButtonDescr = "FormCancelButtonDescr";

		// Token: 0x04002C31 RID: 11313
		internal const string FormControlBoxDescr = "FormControlBoxDescr";

		// Token: 0x04002C32 RID: 11314
		internal const string FormDesktopBoundsDescr = "FormDesktopBoundsDescr";

		// Token: 0x04002C33 RID: 11315
		internal const string FormDesktopLocationDescr = "FormDesktopLocationDescr";

		// Token: 0x04002C34 RID: 11316
		internal const string FormDialogResultDescr = "FormDialogResultDescr";

		// Token: 0x04002C35 RID: 11317
		internal const string FormHelpButtonClickedDescr = "FormHelpButtonClickedDescr";

		// Token: 0x04002C36 RID: 11318
		internal const string FormHelpButtonDescr = "FormHelpButtonDescr";

		// Token: 0x04002C37 RID: 11319
		internal const string FormIconDescr = "FormIconDescr";

		// Token: 0x04002C38 RID: 11320
		internal const string FormIsMDIChildDescr = "FormIsMDIChildDescr";

		// Token: 0x04002C39 RID: 11321
		internal const string FormIsMDIContainerDescr = "FormIsMDIContainerDescr";

		// Token: 0x04002C3A RID: 11322
		internal const string FormKeyPreviewDescr = "FormKeyPreviewDescr";

		// Token: 0x04002C3B RID: 11323
		internal const string FormMaximizeBoxDescr = "FormMaximizeBoxDescr";

		// Token: 0x04002C3C RID: 11324
		internal const string FormMaximumSizeDescr = "FormMaximumSizeDescr";

		// Token: 0x04002C3D RID: 11325
		internal const string FormMDIChildrenDescr = "FormMDIChildrenDescr";

		// Token: 0x04002C3E RID: 11326
		internal const string FormMDIParentAndChild = "FormMDIParentAndChild";

		// Token: 0x04002C3F RID: 11327
		internal const string FormMDIParentCannotAdd = "FormMDIParentCannotAdd";

		// Token: 0x04002C40 RID: 11328
		internal const string FormMDIParentDescr = "FormMDIParentDescr";

		// Token: 0x04002C41 RID: 11329
		internal const string FormMenuDescr = "FormMenuDescr";

		// Token: 0x04002C42 RID: 11330
		internal const string FormMenuStripDescr = "FormMenuStripDescr";

		// Token: 0x04002C43 RID: 11331
		internal const string FormMergedMenuDescr = "FormMergedMenuDescr";

		// Token: 0x04002C44 RID: 11332
		internal const string FormMinimizeBoxDescr = "FormMinimizeBoxDescr";

		// Token: 0x04002C45 RID: 11333
		internal const string FormMinimumSizeDescr = "FormMinimumSizeDescr";

		// Token: 0x04002C46 RID: 11334
		internal const string FormModalDescr = "FormModalDescr";

		// Token: 0x04002C47 RID: 11335
		internal const string FormOnActivateDescr = "FormOnActivateDescr";

		// Token: 0x04002C48 RID: 11336
		internal const string FormOnClosedDescr = "FormOnClosedDescr";

		// Token: 0x04002C49 RID: 11337
		internal const string FormOnClosingDescr = "FormOnClosingDescr";

		// Token: 0x04002C4A RID: 11338
		internal const string FormOnDeactivateDescr = "FormOnDeactivateDescr";

		// Token: 0x04002C4B RID: 11339
		internal const string FormOnFormClosedDescr = "FormOnFormClosedDescr";

		// Token: 0x04002C4C RID: 11340
		internal const string FormOnFormClosingDescr = "FormOnFormClosingDescr";

		// Token: 0x04002C4D RID: 11341
		internal const string FormOnInputLangChangeDescr = "FormOnInputLangChangeDescr";

		// Token: 0x04002C4E RID: 11342
		internal const string FormOnInputLangChangeRequestDescr = "FormOnInputLangChangeRequestDescr";

		// Token: 0x04002C4F RID: 11343
		internal const string FormOnLoadDescr = "FormOnLoadDescr";

		// Token: 0x04002C50 RID: 11344
		internal const string FormOnMaximizedBoundsChangedDescr = "FormOnMaximizedBoundsChangedDescr";

		// Token: 0x04002C51 RID: 11345
		internal const string FormOnMaximumSizeChangedDescr = "FormOnMaximumSizeChangedDescr";

		// Token: 0x04002C52 RID: 11346
		internal const string FormOnMDIChildActivateDescr = "FormOnMDIChildActivateDescr";

		// Token: 0x04002C53 RID: 11347
		internal const string FormOnMenuCompleteDescr = "FormOnMenuCompleteDescr";

		// Token: 0x04002C54 RID: 11348
		internal const string FormOnMenuStartDescr = "FormOnMenuStartDescr";

		// Token: 0x04002C55 RID: 11349
		internal const string FormOnMinimumSizeChangedDescr = "FormOnMinimumSizeChangedDescr";

		// Token: 0x04002C56 RID: 11350
		internal const string FormOnResizeBeginDescr = "FormOnResizeBeginDescr";

		// Token: 0x04002C57 RID: 11351
		internal const string FormOnResizeEndDescr = "FormOnResizeEndDescr";

		// Token: 0x04002C58 RID: 11352
		internal const string FormOnShownDescr = "FormOnShownDescr";

		// Token: 0x04002C59 RID: 11353
		internal const string FormOpacityDescr = "FormOpacityDescr";

		// Token: 0x04002C5A RID: 11354
		internal const string FormOwnedFormsDescr = "FormOwnedFormsDescr";

		// Token: 0x04002C5B RID: 11355
		internal const string FormOwnerDescr = "FormOwnerDescr";

		// Token: 0x04002C5C RID: 11356
		internal const string FormShowIconDescr = "FormShowIconDescr";

		// Token: 0x04002C5D RID: 11357
		internal const string FormShowInTaskbarDescr = "FormShowInTaskbarDescr";

		// Token: 0x04002C5E RID: 11358
		internal const string FormSizeGripStyleDescr = "FormSizeGripStyleDescr";

		// Token: 0x04002C5F RID: 11359
		internal const string FormStartPositionDescr = "FormStartPositionDescr";

		// Token: 0x04002C60 RID: 11360
		internal const string FormTopMostDescr = "FormTopMostDescr";

		// Token: 0x04002C61 RID: 11361
		internal const string FormTransparencyKeyDescr = "FormTransparencyKeyDescr";

		// Token: 0x04002C62 RID: 11362
		internal const string FormWindowStateDescr = "FormWindowStateDescr";

		// Token: 0x04002C63 RID: 11363
		internal const string GridItemDisposed = "GridItemDisposed";

		// Token: 0x04002C64 RID: 11364
		internal const string GridItemNotExpandable = "GridItemNotExpandable";

		// Token: 0x04002C65 RID: 11365
		internal const string GridPanelCellPositionDescr = "GridPanelCellPositionDescr";

		// Token: 0x04002C66 RID: 11366
		internal const string GridPanelColumnDescr = "GridPanelColumnDescr";

		// Token: 0x04002C67 RID: 11367
		internal const string GridPanelColumnsDescr = "GridPanelColumnsDescr";

		// Token: 0x04002C68 RID: 11368
		internal const string GridPanelColumnStylesDescr = "GridPanelColumnStylesDescr";

		// Token: 0x04002C69 RID: 11369
		internal const string GridPanelGetAlignmentDescr = "GridPanelGetAlignmentDescr";

		// Token: 0x04002C6A RID: 11370
		internal const string GridPanelGetBoxStretchDescr = "GridPanelGetBoxStretchDescr";

		// Token: 0x04002C6B RID: 11371
		internal const string GridPanelGetColumnSpanDescr = "GridPanelGetColumnSpanDescr";

		// Token: 0x04002C6C RID: 11372
		internal const string GridPanelGetRowSpanDescr = "GridPanelGetRowSpanDescr";

		// Token: 0x04002C6D RID: 11373
		internal const string GridPanelRowDescr = "GridPanelRowDescr";

		// Token: 0x04002C6E RID: 11374
		internal const string GridPanelRowsDescr = "GridPanelRowsDescr";

		// Token: 0x04002C6F RID: 11375
		internal const string GridPanelRowStylesDescr = "GridPanelRowStylesDescr";

		// Token: 0x04002C70 RID: 11376
		internal const string HandleAlreadyExists = "HandleAlreadyExists";

		// Token: 0x04002C71 RID: 11377
		internal const string HelpCaption = "HelpCaption";

		// Token: 0x04002C72 RID: 11378
		internal const string HelpInvalidURL = "HelpInvalidURL";

		// Token: 0x04002C73 RID: 11379
		internal const string HelpProviderHelpKeywordDescr = "HelpProviderHelpKeywordDescr";

		// Token: 0x04002C74 RID: 11380
		internal const string HelpProviderHelpNamespaceDescr = "HelpProviderHelpNamespaceDescr";

		// Token: 0x04002C75 RID: 11381
		internal const string HelpProviderHelpStringDescr = "HelpProviderHelpStringDescr";

		// Token: 0x04002C76 RID: 11382
		internal const string HelpProviderNavigatorDescr = "HelpProviderNavigatorDescr";

		// Token: 0x04002C77 RID: 11383
		internal const string HelpProviderShowHelpDescr = "HelpProviderShowHelpDescr";

		// Token: 0x04002C78 RID: 11384
		internal const string HtmlDocumentInvalidDomain = "HtmlDocumentInvalidDomain";

		// Token: 0x04002C79 RID: 11385
		internal const string HtmlElementAttributeNotSupported = "HtmlElementAttributeNotSupported";

		// Token: 0x04002C7A RID: 11386
		internal const string HtmlElementMethodNotSupported = "HtmlElementMethodNotSupported";

		// Token: 0x04002C7B RID: 11387
		internal const string HtmlElementPropertyNotSupported = "HtmlElementPropertyNotSupported";

		// Token: 0x04002C7C RID: 11388
		internal const string ICurrencyManagerProviderDescr = "ICurrencyManagerProviderDescr";

		// Token: 0x04002C7D RID: 11389
		internal const string IllegalCrossThreadCall = "IllegalCrossThreadCall";

		// Token: 0x04002C7E RID: 11390
		internal const string IllegalCrossThreadCallWithStack = "IllegalCrossThreadCallWithStack";

		// Token: 0x04002C7F RID: 11391
		internal const string IllegalState = "IllegalState";

		// Token: 0x04002C80 RID: 11392
		internal const string ImageListAddFailed = "ImageListAddFailed";

		// Token: 0x04002C81 RID: 11393
		internal const string ImageListAllowMirroringDescr = "ImageListAllowMirroringDescr";

		// Token: 0x04002C82 RID: 11394
		internal const string ImageListBadImage = "ImageListBadImage";

		// Token: 0x04002C83 RID: 11395
		internal const string ImageListBitmap = "ImageListBitmap";

		// Token: 0x04002C84 RID: 11396
		internal const string ImageListCantRecreate = "ImageListCantRecreate";

		// Token: 0x04002C85 RID: 11397
		internal const string ImageListColorDepthDescr = "ImageListColorDepthDescr";

		// Token: 0x04002C86 RID: 11398
		internal const string ImageListCreateFailed = "ImageListCreateFailed";

		// Token: 0x04002C87 RID: 11399
		internal const string ImageListEntryType = "ImageListEntryType";

		// Token: 0x04002C88 RID: 11400
		internal const string ImageListGetFailed = "ImageListGetFailed";

		// Token: 0x04002C89 RID: 11401
		internal const string ImageListHandleCreatedDescr = "ImageListHandleCreatedDescr";

		// Token: 0x04002C8A RID: 11402
		internal const string ImageListHandleDescr = "ImageListHandleDescr";

		// Token: 0x04002C8B RID: 11403
		internal const string ImageListImagesDescr = "ImageListImagesDescr";

		// Token: 0x04002C8C RID: 11404
		internal const string ImageListImageStreamDescr = "ImageListImageStreamDescr";

		// Token: 0x04002C8D RID: 11405
		internal const string ImageListImageTooShort = "ImageListImageTooShort";

		// Token: 0x04002C8E RID: 11406
		internal const string ImageListOnRecreateHandleDescr = "ImageListOnRecreateHandleDescr";

		// Token: 0x04002C8F RID: 11407
		internal const string ImageListRemoveFailed = "ImageListRemoveFailed";

		// Token: 0x04002C90 RID: 11408
		internal const string ImageListReplaceFailed = "ImageListReplaceFailed";

		// Token: 0x04002C91 RID: 11409
		internal const string ImageListRightToLeftInvalidArgument = "ImageListRightToLeftInvalidArgument";

		// Token: 0x04002C92 RID: 11410
		internal const string ImageListSizeDescr = "ImageListSizeDescr";

		// Token: 0x04002C93 RID: 11411
		internal const string ImageListStreamerLoadFailed = "ImageListStreamerLoadFailed";

		// Token: 0x04002C94 RID: 11412
		internal const string ImageListStreamerSaveFailed = "ImageListStreamerSaveFailed";

		// Token: 0x04002C95 RID: 11413
		internal const string ImageListStripBadWidth = "ImageListStripBadWidth";

		// Token: 0x04002C96 RID: 11414
		internal const string ImageListTransparentColorDescr = "ImageListTransparentColorDescr";

		// Token: 0x04002C97 RID: 11415
		internal const string IndexOutOfRange = "IndexOutOfRange";

		// Token: 0x04002C98 RID: 11416
		internal const string Invalid_boolean_attribute = "Invalid_boolean_attribute";

		// Token: 0x04002C99 RID: 11417
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x04002C9A RID: 11418
		internal const string InvalidBoundArgument = "InvalidBoundArgument";

		// Token: 0x04002C9B RID: 11419
		internal const string InvalidCrossThreadControlCall = "InvalidCrossThreadControlCall";

		// Token: 0x04002C9C RID: 11420
		internal const string InvalidExBoundArgument = "InvalidExBoundArgument";

		// Token: 0x04002C9D RID: 11421
		internal const string InvalidFileFormat = "InvalidFileFormat";

		// Token: 0x04002C9E RID: 11422
		internal const string InvalidFileType = "InvalidFileType";

		// Token: 0x04002C9F RID: 11423
		internal const string InvalidGDIHandle = "InvalidGDIHandle";

		// Token: 0x04002CA0 RID: 11424
		internal const string InvalidHighBoundArgument = "InvalidHighBoundArgument";

		// Token: 0x04002CA1 RID: 11425
		internal const string InvalidHighBoundArgumentEx = "InvalidHighBoundArgumentEx";

		// Token: 0x04002CA2 RID: 11426
		internal const string InvalidLowBoundArgument = "InvalidLowBoundArgument";

		// Token: 0x04002CA3 RID: 11427
		internal const string InvalidLowBoundArgumentEx = "InvalidLowBoundArgumentEx";

		// Token: 0x04002CA4 RID: 11428
		internal const string InvalidNullArgument = "InvalidNullArgument";

		// Token: 0x04002CA5 RID: 11429
		internal const string InvalidNullItemInCollection = "InvalidNullItemInCollection";

		// Token: 0x04002CA6 RID: 11430
		internal const string InvalidPictureFormat = "InvalidPictureFormat";

		// Token: 0x04002CA7 RID: 11431
		internal const string InvalidPictureType = "InvalidPictureType";

		// Token: 0x04002CA8 RID: 11432
		internal const string InvalidResXBasePathOperation = "InvalidResXBasePathOperation";

		// Token: 0x04002CA9 RID: 11433
		internal const string InvalidResXFile = "InvalidResXFile";

		// Token: 0x04002CAA RID: 11434
		internal const string InvalidResXFileReaderWriterTypes = "InvalidResXFileReaderWriterTypes";

		// Token: 0x04002CAB RID: 11435
		internal const string InvalidResXNoType = "InvalidResXNoType";

		// Token: 0x04002CAC RID: 11436
		internal const string InvalidResXResourceNoName = "InvalidResXResourceNoName";

		// Token: 0x04002CAD RID: 11437
		internal const string InvalidSendKeysKeyword = "InvalidSendKeysKeyword";

		// Token: 0x04002CAE RID: 11438
		internal const string InvalidSendKeysRepeat = "InvalidSendKeysRepeat";

		// Token: 0x04002CAF RID: 11439
		internal const string InvalidSendKeysString = "InvalidSendKeysString";

		// Token: 0x04002CB0 RID: 11440
		internal const string InvalidSingleMonthSize = "InvalidSingleMonthSize";

		// Token: 0x04002CB1 RID: 11441
		internal const string InvalidWndClsName = "InvalidWndClsName";

		// Token: 0x04002CB2 RID: 11442
		internal const string InvocationException = "InvocationException";

		// Token: 0x04002CB3 RID: 11443
		internal const string IsMirroredDescr = "IsMirroredDescr";

		// Token: 0x04002CB4 RID: 11444
		internal const string ISupportInitializeDescr = "ISupportInitializeDescr";

		// Token: 0x04002CB5 RID: 11445
		internal const string KeysConverterInvalidKeyCombination = "KeysConverterInvalidKeyCombination";

		// Token: 0x04002CB6 RID: 11446
		internal const string KeysConverterInvalidKeyName = "KeysConverterInvalidKeyName";

		// Token: 0x04002CB7 RID: 11447
		internal const string LabelAutoEllipsisDescr = "LabelAutoEllipsisDescr";

		// Token: 0x04002CB8 RID: 11448
		internal const string LabelAutoSizeDescr = "LabelAutoSizeDescr";

		// Token: 0x04002CB9 RID: 11449
		internal const string LabelBackgroundImageDescr = "LabelBackgroundImageDescr";

		// Token: 0x04002CBA RID: 11450
		internal const string LabelBorderDescr = "LabelBorderDescr";

		// Token: 0x04002CBB RID: 11451
		internal const string LabelOnTextAlignChangedDescr = "LabelOnTextAlignChangedDescr";

		// Token: 0x04002CBC RID: 11452
		internal const string LabelPreferredHeightDescr = "LabelPreferredHeightDescr";

		// Token: 0x04002CBD RID: 11453
		internal const string LabelPreferredWidthDescr = "LabelPreferredWidthDescr";

		// Token: 0x04002CBE RID: 11454
		internal const string LabelTextAlignDescr = "LabelTextAlignDescr";

		// Token: 0x04002CBF RID: 11455
		internal const string LabelUseMnemonicDescr = "LabelUseMnemonicDescr";

		// Token: 0x04002CC0 RID: 11456
		internal const string LayoutEngineUnsupportedType = "LayoutEngineUnsupportedType";

		// Token: 0x04002CC1 RID: 11457
		internal const string LinkLabelActiveLinkColorDescr = "LinkLabelActiveLinkColorDescr";

		// Token: 0x04002CC2 RID: 11458
		internal const string LinkLabelAreaLength = "LinkLabelAreaLength";

		// Token: 0x04002CC3 RID: 11459
		internal const string LinkLabelAreaStart = "LinkLabelAreaStart";

		// Token: 0x04002CC4 RID: 11460
		internal const string LinkLabelBadLink = "LinkLabelBadLink";

		// Token: 0x04002CC5 RID: 11461
		internal const string LinkLabelDisabledLinkColorDescr = "LinkLabelDisabledLinkColorDescr";

		// Token: 0x04002CC6 RID: 11462
		internal const string LinkLabelLinkAreaDescr = "LinkLabelLinkAreaDescr";

		// Token: 0x04002CC7 RID: 11463
		internal const string LinkLabelLinkBehaviorDescr = "LinkLabelLinkBehaviorDescr";

		// Token: 0x04002CC8 RID: 11464
		internal const string LinkLabelLinkClickedDescr = "LinkLabelLinkClickedDescr";

		// Token: 0x04002CC9 RID: 11465
		internal const string LinkLabelLinkColorDescr = "LinkLabelLinkColorDescr";

		// Token: 0x04002CCA RID: 11466
		internal const string LinkLabelLinkVisitedDescr = "LinkLabelLinkVisitedDescr";

		// Token: 0x04002CCB RID: 11467
		internal const string LinkLabelOverlap = "LinkLabelOverlap";

		// Token: 0x04002CCC RID: 11468
		internal const string LinkLabelVisitedLinkColorDescr = "LinkLabelVisitedLinkColorDescr";

		// Token: 0x04002CCD RID: 11469
		internal const string ListBindingBindField = "ListBindingBindField";

		// Token: 0x04002CCE RID: 11470
		internal const string ListBindingBindProperty = "ListBindingBindProperty";

		// Token: 0x04002CCF RID: 11471
		internal const string ListBindingBindPropertyReadOnly = "ListBindingBindPropertyReadOnly";

		// Token: 0x04002CD0 RID: 11472
		internal const string ListBindingFormatFailed = "ListBindingFormatFailed";

		// Token: 0x04002CD1 RID: 11473
		internal const string ListBoxBorderDescr = "ListBoxBorderDescr";

		// Token: 0x04002CD2 RID: 11474
		internal const string ListBoxCantInsertIntoIntegerCollection = "ListBoxCantInsertIntoIntegerCollection";

		// Token: 0x04002CD3 RID: 11475
		internal const string ListBoxColumnWidthDescr = "ListBoxColumnWidthDescr";

		// Token: 0x04002CD4 RID: 11476
		internal const string ListBoxCustomTabOffsetsDescr = "ListBoxCustomTabOffsetsDescr";

		// Token: 0x04002CD5 RID: 11477
		internal const string ListBoxDrawModeDescr = "ListBoxDrawModeDescr";

		// Token: 0x04002CD6 RID: 11478
		internal const string ListBoxHorizontalExtentDescr = "ListBoxHorizontalExtentDescr";

		// Token: 0x04002CD7 RID: 11479
		internal const string ListBoxHorizontalScrollbarDescr = "ListBoxHorizontalScrollbarDescr";

		// Token: 0x04002CD8 RID: 11480
		internal const string ListBoxIntegralHeightDescr = "ListBoxIntegralHeightDescr";

		// Token: 0x04002CD9 RID: 11481
		internal const string ListBoxInvalidSelectionMode = "ListBoxInvalidSelectionMode";

		// Token: 0x04002CDA RID: 11482
		internal const string ListBoxItemHeightDescr = "ListBoxItemHeightDescr";

		// Token: 0x04002CDB RID: 11483
		internal const string ListBoxItemOverflow = "ListBoxItemOverflow";

		// Token: 0x04002CDC RID: 11484
		internal const string ListBoxItemsDescr = "ListBoxItemsDescr";

		// Token: 0x04002CDD RID: 11485
		internal const string ListBoxMultiColumnDescr = "ListBoxMultiColumnDescr";

		// Token: 0x04002CDE RID: 11486
		internal const string ListBoxPreferredHeightDescr = "ListBoxPreferredHeightDescr";

		// Token: 0x04002CDF RID: 11487
		internal const string ListBoxScrollIsVisibleDescr = "ListBoxScrollIsVisibleDescr";

		// Token: 0x04002CE0 RID: 11488
		internal const string ListBoxSelectedIndexCollectionIsReadOnly = "ListBoxSelectedIndexCollectionIsReadOnly";

		// Token: 0x04002CE1 RID: 11489
		internal const string ListBoxSelectedIndexDescr = "ListBoxSelectedIndexDescr";

		// Token: 0x04002CE2 RID: 11490
		internal const string ListBoxSelectedIndicesDescr = "ListBoxSelectedIndicesDescr";

		// Token: 0x04002CE3 RID: 11491
		internal const string ListBoxSelectedItemDescr = "ListBoxSelectedItemDescr";

		// Token: 0x04002CE4 RID: 11492
		internal const string ListBoxSelectedItemsDescr = "ListBoxSelectedItemsDescr";

		// Token: 0x04002CE5 RID: 11493
		internal const string ListBoxSelectedObjectCollectionIsReadOnly = "ListBoxSelectedObjectCollectionIsReadOnly";

		// Token: 0x04002CE6 RID: 11494
		internal const string ListBoxSelectionModeDescr = "ListBoxSelectionModeDescr";

		// Token: 0x04002CE7 RID: 11495
		internal const string ListBoxSortedDescr = "ListBoxSortedDescr";

		// Token: 0x04002CE8 RID: 11496
		internal const string ListBoxTopIndexDescr = "ListBoxTopIndexDescr";

		// Token: 0x04002CE9 RID: 11497
		internal const string ListBoxUseTabStopsDescr = "ListBoxUseTabStopsDescr";

		// Token: 0x04002CEA RID: 11498
		internal const string ListBoxVarHeightMultiCol = "ListBoxVarHeightMultiCol";

		// Token: 0x04002CEB RID: 11499
		internal const string ListControlDataSourceDescr = "ListControlDataSourceDescr";

		// Token: 0x04002CEC RID: 11500
		internal const string ListControlDisplayMemberDescr = "ListControlDisplayMemberDescr";

		// Token: 0x04002CED RID: 11501
		internal const string ListControlEmptyValueMemberInSettingSelectedValue = "ListControlEmptyValueMemberInSettingSelectedValue";

		// Token: 0x04002CEE RID: 11502
		internal const string ListControlFormatDescr = "ListControlFormatDescr";

		// Token: 0x04002CEF RID: 11503
		internal const string ListControlFormatInfoChangedDescr = "ListControlFormatInfoChangedDescr";

		// Token: 0x04002CF0 RID: 11504
		internal const string ListControlFormatStringChangedDescr = "ListControlFormatStringChangedDescr";

		// Token: 0x04002CF1 RID: 11505
		internal const string ListControlFormatStringDescr = "ListControlFormatStringDescr";

		// Token: 0x04002CF2 RID: 11506
		internal const string ListControlFormattingEnabledChangedDescr = "ListControlFormattingEnabledChangedDescr";

		// Token: 0x04002CF3 RID: 11507
		internal const string ListControlFormattingEnabledDescr = "ListControlFormattingEnabledDescr";

		// Token: 0x04002CF4 RID: 11508
		internal const string ListControlOnDataSourceChangedDescr = "ListControlOnDataSourceChangedDescr";

		// Token: 0x04002CF5 RID: 11509
		internal const string ListControlOnDisplayMemberChangedDescr = "ListControlOnDisplayMemberChangedDescr";

		// Token: 0x04002CF6 RID: 11510
		internal const string ListControlOnSelectedValueChangedDescr = "ListControlOnSelectedValueChangedDescr";

		// Token: 0x04002CF7 RID: 11511
		internal const string ListControlOnValueMemberChangedDescr = "ListControlOnValueMemberChangedDescr";

		// Token: 0x04002CF8 RID: 11512
		internal const string ListControlSelectedValueDescr = "ListControlSelectedValueDescr";

		// Token: 0x04002CF9 RID: 11513
		internal const string ListControlValueMemberDescr = "ListControlValueMemberDescr";

		// Token: 0x04002CFA RID: 11514
		internal const string ListControlWrongDisplayMember = "ListControlWrongDisplayMember";

		// Token: 0x04002CFB RID: 11515
		internal const string ListControlWrongValueMember = "ListControlWrongValueMember";

		// Token: 0x04002CFC RID: 11516
		internal const string ListEnumCurrentOutOfRange = "ListEnumCurrentOutOfRange";

		// Token: 0x04002CFD RID: 11517
		internal const string ListEnumVersionMismatch = "ListEnumVersionMismatch";

		// Token: 0x04002CFE RID: 11518
		internal const string ListManagerBadPosition = "ListManagerBadPosition";

		// Token: 0x04002CFF RID: 11519
		internal const string ListManagerEmptyList = "ListManagerEmptyList";

		// Token: 0x04002D00 RID: 11520
		internal const string ListManagerNoValue = "ListManagerNoValue";

		// Token: 0x04002D01 RID: 11521
		internal const string ListManagerSetDataSource = "ListManagerSetDataSource";

		// Token: 0x04002D02 RID: 11522
		internal const string ListViewActivationDescr = "ListViewActivationDescr";

		// Token: 0x04002D03 RID: 11523
		internal const string ListViewActivationMustBeOnWhenHotTrackingIsOn = "ListViewActivationMustBeOnWhenHotTrackingIsOn";

		// Token: 0x04002D04 RID: 11524
		internal const string ListViewAddColumnFailed = "ListViewAddColumnFailed";

		// Token: 0x04002D05 RID: 11525
		internal const string ListViewAddItemFailed = "ListViewAddItemFailed";

		// Token: 0x04002D06 RID: 11526
		internal const string ListViewAfterLabelEditDescr = "ListViewAfterLabelEditDescr";

		// Token: 0x04002D07 RID: 11527
		internal const string ListViewAlignmentDescr = "ListViewAlignmentDescr";

		// Token: 0x04002D08 RID: 11528
		internal const string ListViewAllowColumnReorderDescr = "ListViewAllowColumnReorderDescr";

		// Token: 0x04002D09 RID: 11529
		internal const string ListViewAutoArrangeDescr = "ListViewAutoArrangeDescr";

		// Token: 0x04002D0A RID: 11530
		internal const string ListViewBackgroundImageTiledDescr = "ListViewBackgroundImageTiledDescr";

		// Token: 0x04002D0B RID: 11531
		internal const string ListViewBadListViewSubItem = "ListViewBadListViewSubItem";

		// Token: 0x04002D0C RID: 11532
		internal const string ListViewBeforeLabelEditDescr = "ListViewBeforeLabelEditDescr";

		// Token: 0x04002D0D RID: 11533
		internal const string ListViewBeginEditFailed = "ListViewBeginEditFailed";

		// Token: 0x04002D0E RID: 11534
		internal const string ListViewCacheVirtualItemsEventDescr = "ListViewCacheVirtualItemsEventDescr";

		// Token: 0x04002D0F RID: 11535
		internal const string ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode = "ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode";

		// Token: 0x04002D10 RID: 11536
		internal const string ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode = "ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode";

		// Token: 0x04002D11 RID: 11537
		internal const string ListViewCantAddItemsToAVirtualListView = "ListViewCantAddItemsToAVirtualListView";

		// Token: 0x04002D12 RID: 11538
		internal const string ListViewCantGetEnumeratorInVirtualMode = "ListViewCantGetEnumeratorInVirtualMode";

		// Token: 0x04002D13 RID: 11539
		internal const string ListViewCantModifyTheItemCollInAVirtualListView = "ListViewCantModifyTheItemCollInAVirtualListView";

		// Token: 0x04002D14 RID: 11540
		internal const string ListViewCantRemoveItemsFromAVirtualListView = "ListViewCantRemoveItemsFromAVirtualListView";

		// Token: 0x04002D15 RID: 11541
		internal const string ListViewCantSetViewToTileViewInVirtualMode = "ListViewCantSetViewToTileViewInVirtualMode";

		// Token: 0x04002D16 RID: 11542
		internal const string ListViewCantSetVirtualModeWhenInTileView = "ListViewCantSetVirtualModeWhenInTileView";

		// Token: 0x04002D17 RID: 11543
		internal const string ListViewCheckBoxesDescr = "ListViewCheckBoxesDescr";

		// Token: 0x04002D18 RID: 11544
		internal const string ListViewCheckBoxesNotSupportedInTileView = "ListViewCheckBoxesNotSupportedInTileView";

		// Token: 0x04002D19 RID: 11545
		internal const string ListViewColumnClickDescr = "ListViewColumnClickDescr";

		// Token: 0x04002D1A RID: 11546
		internal const string ListViewColumnInfoSet = "ListViewColumnInfoSet";

		// Token: 0x04002D1B RID: 11547
		internal const string ListViewColumnReorderedDscr = "ListViewColumnReorderedDscr";

		// Token: 0x04002D1C RID: 11548
		internal const string ListViewColumnsDescr = "ListViewColumnsDescr";

		// Token: 0x04002D1D RID: 11549
		internal const string ListViewColumnWidthChangedDscr = "ListViewColumnWidthChangedDscr";

		// Token: 0x04002D1E RID: 11550
		internal const string ListViewColumnWidthChangingDscr = "ListViewColumnWidthChangingDscr";

		// Token: 0x04002D1F RID: 11551
		internal const string ListViewDrawColumnHeaderEventDescr = "ListViewDrawColumnHeaderEventDescr";

		// Token: 0x04002D20 RID: 11552
		internal const string ListViewDrawItemEventDescr = "ListViewDrawItemEventDescr";

		// Token: 0x04002D21 RID: 11553
		internal const string ListViewDrawSubItemEventDescr = "ListViewDrawSubItemEventDescr";

		// Token: 0x04002D22 RID: 11554
		internal const string ListViewFindNearestItemWorksOnlyInIconView = "ListViewFindNearestItemWorksOnlyInIconView";

		// Token: 0x04002D23 RID: 11555
		internal const string ListViewFocusedItemDescr = "ListViewFocusedItemDescr";

		// Token: 0x04002D24 RID: 11556
		internal const string ListViewFullRowSelectDescr = "ListViewFullRowSelectDescr";

		// Token: 0x04002D25 RID: 11557
		internal const string ListViewGetTopItem = "ListViewGetTopItem";

		// Token: 0x04002D26 RID: 11558
		internal const string ListViewGridLinesDescr = "ListViewGridLinesDescr";

		// Token: 0x04002D27 RID: 11559
		internal const string ListViewGroupDefaultGroup = "ListViewGroupDefaultGroup";

		// Token: 0x04002D28 RID: 11560
		internal const string ListViewGroupDefaultHeader = "ListViewGroupDefaultHeader";

		// Token: 0x04002D29 RID: 11561
		internal const string ListViewGroupNameDescr = "ListViewGroupNameDescr";

		// Token: 0x04002D2A RID: 11562
		internal const string ListViewGroupsDescr = "ListViewGroupsDescr";

		// Token: 0x04002D2B RID: 11563
		internal const string ListViewHeaderStyleDescr = "ListViewHeaderStyleDescr";

		// Token: 0x04002D2C RID: 11564
		internal const string ListViewHideSelectionDescr = "ListViewHideSelectionDescr";

		// Token: 0x04002D2D RID: 11565
		internal const string ListViewHotTrackingDescr = "ListViewHotTrackingDescr";

		// Token: 0x04002D2E RID: 11566
		internal const string ListViewHoverMustBeOnWhenHotTrackingIsOn = "ListViewHoverMustBeOnWhenHotTrackingIsOn";

		// Token: 0x04002D2F RID: 11567
		internal const string ListViewHoverSelectDescr = "ListViewHoverSelectDescr";

		// Token: 0x04002D30 RID: 11568
		internal const string ListViewIndentCountCantBeNegative = "ListViewIndentCountCantBeNegative";

		// Token: 0x04002D31 RID: 11569
		internal const string ListViewInsertionMarkDescr = "ListViewInsertionMarkDescr";

		// Token: 0x04002D32 RID: 11570
		internal const string ListViewItemCheckedDescr = "ListViewItemCheckedDescr";

		// Token: 0x04002D33 RID: 11571
		internal const string ListViewItemClickDescr = "ListViewItemClickDescr";

		// Token: 0x04002D34 RID: 11572
		internal const string ListViewItemDragDescr = "ListViewItemDragDescr";

		// Token: 0x04002D35 RID: 11573
		internal const string ListViewItemImageIndexDescr = "ListViewItemImageIndexDescr";

		// Token: 0x04002D36 RID: 11574
		internal const string ListViewItemImageKeyDescr = "ListViewItemImageKeyDescr";

		// Token: 0x04002D37 RID: 11575
		internal const string ListViewItemIndentCountDescr = "ListViewItemIndentCountDescr";

		// Token: 0x04002D38 RID: 11576
		internal const string ListViewItemMouseHoverDescr = "ListViewItemMouseHoverDescr";

		// Token: 0x04002D39 RID: 11577
		internal const string ListViewItemsDescr = "ListViewItemsDescr";

		// Token: 0x04002D3A RID: 11578
		internal const string ListViewItemSelectionChangedDescr = "ListViewItemSelectionChangedDescr";

		// Token: 0x04002D3B RID: 11579
		internal const string ListViewItemSorterDescr = "ListViewItemSorterDescr";

		// Token: 0x04002D3C RID: 11580
		internal const string ListViewItemStateImageIndexDescr = "ListViewItemStateImageIndexDescr";

		// Token: 0x04002D3D RID: 11581
		internal const string ListViewItemStateImageKeyDescr = "ListViewItemStateImageKeyDescr";

		// Token: 0x04002D3E RID: 11582
		internal const string ListViewItemSubItemsDescr = "ListViewItemSubItemsDescr";

		// Token: 0x04002D3F RID: 11583
		internal const string ListViewLabelEditDescr = "ListViewLabelEditDescr";

		// Token: 0x04002D40 RID: 11584
		internal const string ListViewLabelWrapDescr = "ListViewLabelWrapDescr";

		// Token: 0x04002D41 RID: 11585
		internal const string ListViewLargeImageListDescr = "ListViewLargeImageListDescr";

		// Token: 0x04002D42 RID: 11586
		internal const string ListViewMultiSelectDescr = "ListViewMultiSelectDescr";

		// Token: 0x04002D43 RID: 11587
		internal const string ListViewOwnerDrawDescr = "ListViewOwnerDrawDescr";

		// Token: 0x04002D44 RID: 11588
		internal const string ListViewRetrieveVirtualItemEventDescr = "ListViewRetrieveVirtualItemEventDescr";

		// Token: 0x04002D45 RID: 11589
		internal const string ListViewScrollableDescr = "ListViewScrollableDescr";

		// Token: 0x04002D46 RID: 11590
		internal const string ListViewSearchForVirtualItemDescr = "ListViewSearchForVirtualItemDescr";

		// Token: 0x04002D47 RID: 11591
		internal const string ListViewSelectedIndexChangedDescr = "ListViewSelectedIndexChangedDescr";

		// Token: 0x04002D48 RID: 11592
		internal const string ListViewSelectedItemsDescr = "ListViewSelectedItemsDescr";

		// Token: 0x04002D49 RID: 11593
		internal const string ListViewSetTopItem = "ListViewSetTopItem";

		// Token: 0x04002D4A RID: 11594
		internal const string ListViewShowGroupsDescr = "ListViewShowGroupsDescr";

		// Token: 0x04002D4B RID: 11595
		internal const string ListViewShowItemToolTipsDescr = "ListViewShowItemToolTipsDescr";

		// Token: 0x04002D4C RID: 11596
		internal const string ListViewSmallImageListDescr = "ListViewSmallImageListDescr";

		// Token: 0x04002D4D RID: 11597
		internal const string ListViewSortingDescr = "ListViewSortingDescr";

		// Token: 0x04002D4E RID: 11598
		internal const string ListViewSortNotAllowedInVirtualListView = "ListViewSortNotAllowedInVirtualListView";

		// Token: 0x04002D4F RID: 11599
		internal const string ListViewStartIndexCannotBeLargerThanEndIndex = "ListViewStartIndexCannotBeLargerThanEndIndex";

		// Token: 0x04002D50 RID: 11600
		internal const string ListViewStateImageListDescr = "ListViewStateImageListDescr";

		// Token: 0x04002D51 RID: 11601
		internal const string ListViewSubItemCollectionInvalidArgument = "ListViewSubItemCollectionInvalidArgument";

		// Token: 0x04002D52 RID: 11602
		internal const string ListViewTileSizeDescr = "ListViewTileSizeDescr";

		// Token: 0x04002D53 RID: 11603
		internal const string ListViewTileSizeMustBePositive = "ListViewTileSizeMustBePositive";

		// Token: 0x04002D54 RID: 11604
		internal const string ListViewTileViewDoesNotSupportCheckBoxes = "ListViewTileViewDoesNotSupportCheckBoxes";

		// Token: 0x04002D55 RID: 11605
		internal const string ListViewTopItemDescr = "ListViewTopItemDescr";

		// Token: 0x04002D56 RID: 11606
		internal const string ListViewViewDescr = "ListViewViewDescr";

		// Token: 0x04002D57 RID: 11607
		internal const string ListViewVirtualItemRequired = "ListViewVirtualItemRequired";

		// Token: 0x04002D58 RID: 11608
		internal const string ListViewVirtualItemsSelectionRangeChangedDescr = "ListViewVirtualItemsSelectionRangeChangedDescr";

		// Token: 0x04002D59 RID: 11609
		internal const string ListViewVirtualItemStateChangedDescr = "ListViewVirtualItemStateChangedDescr";

		// Token: 0x04002D5A RID: 11610
		internal const string ListViewVirtualListSizeDescr = "ListViewVirtualListSizeDescr";

		// Token: 0x04002D5B RID: 11611
		internal const string ListViewVirtualListSizeInvalidArgument = "ListViewVirtualListSizeInvalidArgument";

		// Token: 0x04002D5C RID: 11612
		internal const string ListViewVirtualListViewRequiresNoCheckedItems = "ListViewVirtualListViewRequiresNoCheckedItems";

		// Token: 0x04002D5D RID: 11613
		internal const string ListViewVirtualListViewRequiresNoItems = "ListViewVirtualListViewRequiresNoItems";

		// Token: 0x04002D5E RID: 11614
		internal const string ListViewVirtualListViewRequiresNoSelectedItems = "ListViewVirtualListViewRequiresNoSelectedItems";

		// Token: 0x04002D5F RID: 11615
		internal const string ListViewVirtualModeCantAccessSubItem = "ListViewVirtualModeCantAccessSubItem";

		// Token: 0x04002D60 RID: 11616
		internal const string ListViewVirtualModeDescr = "ListViewVirtualModeDescr";

		// Token: 0x04002D61 RID: 11617
		internal const string LoadDLLError = "LoadDLLError";

		// Token: 0x04002D62 RID: 11618
		internal const string LoadTextError = "LoadTextError";

		// Token: 0x04002D63 RID: 11619
		internal const string MainMenuCollapseDescr = "MainMenuCollapseDescr";

		// Token: 0x04002D64 RID: 11620
		internal const string MainMenuImageListDescr = "MainMenuImageListDescr";

		// Token: 0x04002D65 RID: 11621
		internal const string MainMenuIsImageMarginPresentDescr = "MainMenuIsImageMarginPresentDescr";

		// Token: 0x04002D66 RID: 11622
		internal const string MainMenuToStringNoForm = "MainMenuToStringNoForm";

		// Token: 0x04002D67 RID: 11623
		internal const string MaskedTextBoxAllowPromptAsInputDescr = "MaskedTextBoxAllowPromptAsInputDescr";

		// Token: 0x04002D68 RID: 11624
		internal const string MaskedTextBoxAsciiOnlyDescr = "MaskedTextBoxAsciiOnlyDescr";

		// Token: 0x04002D69 RID: 11625
		internal const string MaskedTextBoxBeepOnErrorDescr = "MaskedTextBoxBeepOnErrorDescr";

		// Token: 0x04002D6A RID: 11626
		internal const string MaskedTextBoxCultureDescr = "MaskedTextBoxCultureDescr";

		// Token: 0x04002D6B RID: 11627
		internal const string MaskedTextBoxCutCopyMaskFormat = "MaskedTextBoxCutCopyMaskFormat";

		// Token: 0x04002D6C RID: 11628
		internal const string MaskedTextBoxHidePromptOnLeaveDescr = "MaskedTextBoxHidePromptOnLeaveDescr";

		// Token: 0x04002D6D RID: 11629
		internal const string MaskedTextBoxIncompleteMsg = "MaskedTextBoxIncompleteMsg";

		// Token: 0x04002D6E RID: 11630
		internal const string MaskedTextBoxInsertKeyModeDescr = "MaskedTextBoxInsertKeyModeDescr";

		// Token: 0x04002D6F RID: 11631
		internal const string MaskedTextBoxInvalidCharError = "MaskedTextBoxInvalidCharError";

		// Token: 0x04002D70 RID: 11632
		internal const string MaskedTextBoxIsOverwriteModeChangedDescr = "MaskedTextBoxIsOverwriteModeChangedDescr";

		// Token: 0x04002D71 RID: 11633
		internal const string MaskedTextBoxMaskChangedDescr = "MaskedTextBoxMaskChangedDescr";

		// Token: 0x04002D72 RID: 11634
		internal const string MaskedTextBoxMaskDescr = "MaskedTextBoxMaskDescr";

		// Token: 0x04002D73 RID: 11635
		internal const string MaskedTextBoxMaskInputRejectedDescr = "MaskedTextBoxMaskInputRejectedDescr";

		// Token: 0x04002D74 RID: 11636
		internal const string MaskedTextBoxMaskInvalidChar = "MaskedTextBoxMaskInvalidChar";

		// Token: 0x04002D75 RID: 11637
		internal const string MaskedTextBoxPasswordAndPromptCharError = "MaskedTextBoxPasswordAndPromptCharError";

		// Token: 0x04002D76 RID: 11638
		internal const string MaskedTextBoxPasswordCharDescr = "MaskedTextBoxPasswordCharDescr";

		// Token: 0x04002D77 RID: 11639
		internal const string MaskedTextBoxPromptCharDescr = "MaskedTextBoxPromptCharDescr";

		// Token: 0x04002D78 RID: 11640
		internal const string MaskedTextBoxRejectInputOnFirstFailureDescr = "MaskedTextBoxRejectInputOnFirstFailureDescr";

		// Token: 0x04002D79 RID: 11641
		internal const string MaskedTextBoxResetOnPrompt = "MaskedTextBoxResetOnPrompt";

		// Token: 0x04002D7A RID: 11642
		internal const string MaskedTextBoxResetOnSpace = "MaskedTextBoxResetOnSpace";

		// Token: 0x04002D7B RID: 11643
		internal const string MaskedTextBoxSkipLiterals = "MaskedTextBoxSkipLiterals";

		// Token: 0x04002D7C RID: 11644
		internal const string MaskedTextBoxTextMaskFormat = "MaskedTextBoxTextMaskFormat";

		// Token: 0x04002D7D RID: 11645
		internal const string MaskedTextBoxTypeValidationCompletedDescr = "MaskedTextBoxTypeValidationCompletedDescr";

		// Token: 0x04002D7E RID: 11646
		internal const string MaskedTextBoxTypeValidationSucceeded = "MaskedTextBoxTypeValidationSucceeded";

		// Token: 0x04002D7F RID: 11647
		internal const string MaskedTextBoxUseSystemPasswordCharDescr = "MaskedTextBoxUseSystemPasswordCharDescr";

		// Token: 0x04002D80 RID: 11648
		internal const string MaskedTextBoxValidatedValueChangedDescr = "MaskedTextBoxValidatedValueChangedDescr";

		// Token: 0x04002D81 RID: 11649
		internal const string MaskedTextBoxValidatingTypeDescr = "MaskedTextBoxValidatingTypeDescr";

		// Token: 0x04002D82 RID: 11650
		internal const string MDIChildAddToNonMDIParent = "MDIChildAddToNonMDIParent";

		// Token: 0x04002D83 RID: 11651
		internal const string MDIContainerMustBeTopLevel = "MDIContainerMustBeTopLevel";

		// Token: 0x04002D84 RID: 11652
		internal const string MDIMenuMoreWindows = "MDIMenuMoreWindows";

		// Token: 0x04002D85 RID: 11653
		internal const string MDIParentNotContainer = "MDIParentNotContainer";

		// Token: 0x04002D86 RID: 11654
		internal const string measureItemEventDescr = "measureItemEventDescr";

		// Token: 0x04002D87 RID: 11655
		internal const string MenuBadMenuItem = "MenuBadMenuItem";

		// Token: 0x04002D88 RID: 11656
		internal const string MenuImageMarginColorDescr = "MenuImageMarginColorDescr";

		// Token: 0x04002D89 RID: 11657
		internal const string MenuIsParentDescr = "MenuIsParentDescr";

		// Token: 0x04002D8A RID: 11658
		internal const string MenuItemAlreadyExists = "MenuItemAlreadyExists";

		// Token: 0x04002D8B RID: 11659
		internal const string MenuItemCheckedDescr = "MenuItemCheckedDescr";

		// Token: 0x04002D8C RID: 11660
		internal const string MenuItemDefaultDescr = "MenuItemDefaultDescr";

		// Token: 0x04002D8D RID: 11661
		internal const string MenuItemEnabledDescr = "MenuItemEnabledDescr";

		// Token: 0x04002D8E RID: 11662
		internal const string MenuItemImageDescr = "MenuItemImageDescr";

		// Token: 0x04002D8F RID: 11663
		internal const string MenuItemImageListDescr = "MenuItemImageListDescr";

		// Token: 0x04002D90 RID: 11664
		internal const string MenuItemImageMarginColorDescr = "MenuItemImageMarginColorDescr";

		// Token: 0x04002D91 RID: 11665
		internal const string MenuItemInvalidCheckProperty = "MenuItemInvalidCheckProperty";

		// Token: 0x04002D92 RID: 11666
		internal const string MenuItemMDIListDescr = "MenuItemMDIListDescr";

		// Token: 0x04002D93 RID: 11667
		internal const string MenuItemMergeOrderDescr = "MenuItemMergeOrderDescr";

		// Token: 0x04002D94 RID: 11668
		internal const string MenuItemMergeTypeDescr = "MenuItemMergeTypeDescr";

		// Token: 0x04002D95 RID: 11669
		internal const string MenuItemOnClickDescr = "MenuItemOnClickDescr";

		// Token: 0x04002D96 RID: 11670
		internal const string MenuItemOnInitDescr = "MenuItemOnInitDescr";

		// Token: 0x04002D97 RID: 11671
		internal const string MenuItemOnSelectDescr = "MenuItemOnSelectDescr";

		// Token: 0x04002D98 RID: 11672
		internal const string MenuItemOwnerDrawDescr = "MenuItemOwnerDrawDescr";

		// Token: 0x04002D99 RID: 11673
		internal const string MenuItemRadioCheckDescr = "MenuItemRadioCheckDescr";

		// Token: 0x04002D9A RID: 11674
		internal const string MenuItemShortCutDescr = "MenuItemShortCutDescr";

		// Token: 0x04002D9B RID: 11675
		internal const string MenuItemShowShortCutDescr = "MenuItemShowShortCutDescr";

		// Token: 0x04002D9C RID: 11676
		internal const string MenuItemTextDescr = "MenuItemTextDescr";

		// Token: 0x04002D9D RID: 11677
		internal const string MenuItemVisibleDescr = "MenuItemVisibleDescr";

		// Token: 0x04002D9E RID: 11678
		internal const string MenuMDIListItemDescr = "MenuMDIListItemDescr";

		// Token: 0x04002D9F RID: 11679
		internal const string MenuMenuItemsDescr = "MenuMenuItemsDescr";

		// Token: 0x04002DA0 RID: 11680
		internal const string MenuMergeWithSelf = "MenuMergeWithSelf";

		// Token: 0x04002DA1 RID: 11681
		internal const string MenuRightToLeftDescr = "MenuRightToLeftDescr";

		// Token: 0x04002DA2 RID: 11682
		internal const string MenuStripMdiWindowListItem = "MenuStripMdiWindowListItem";

		// Token: 0x04002DA3 RID: 11683
		internal const string MenuStripMenuActivateDescr = "MenuStripMenuActivateDescr";

		// Token: 0x04002DA4 RID: 11684
		internal const string MenuStripMenuDeactivateDescr = "MenuStripMenuDeactivateDescr";

		// Token: 0x04002DA5 RID: 11685
		internal const string MonthCalendarAnnuallyBoldedDatesDescr = "MonthCalendarAnnuallyBoldedDatesDescr";

		// Token: 0x04002DA6 RID: 11686
		internal const string MonthCalendarDimensionsDescr = "MonthCalendarDimensionsDescr";

		// Token: 0x04002DA7 RID: 11687
		internal const string MonthCalendarFirstDayOfWeekDescr = "MonthCalendarFirstDayOfWeekDescr";

		// Token: 0x04002DA8 RID: 11688
		internal const string MonthCalendarForeColorDescr = "MonthCalendarForeColorDescr";

		// Token: 0x04002DA9 RID: 11689
		internal const string MonthCalendarInvalidDimensions = "MonthCalendarInvalidDimensions";

		// Token: 0x04002DAA RID: 11690
		internal const string MonthCalendarMaxDateDescr = "MonthCalendarMaxDateDescr";

		// Token: 0x04002DAB RID: 11691
		internal const string MonthCalendarMaxSelCount = "MonthCalendarMaxSelCount";

		// Token: 0x04002DAC RID: 11692
		internal const string MonthCalendarMaxSelectionCountDescr = "MonthCalendarMaxSelectionCountDescr";

		// Token: 0x04002DAD RID: 11693
		internal const string MonthCalendarMinDateDescr = "MonthCalendarMinDateDescr";

		// Token: 0x04002DAE RID: 11694
		internal const string MonthCalendarMonthBackColorDescr = "MonthCalendarMonthBackColorDescr";

		// Token: 0x04002DAF RID: 11695
		internal const string MonthCalendarMonthlyBoldedDatesDescr = "MonthCalendarMonthlyBoldedDatesDescr";

		// Token: 0x04002DB0 RID: 11696
		internal const string MonthCalendarOnDateChangedDescr = "MonthCalendarOnDateChangedDescr";

		// Token: 0x04002DB1 RID: 11697
		internal const string MonthCalendarOnDateSelectedDescr = "MonthCalendarOnDateSelectedDescr";

		// Token: 0x04002DB2 RID: 11698
		internal const string MonthCalendarRange = "MonthCalendarRange";

		// Token: 0x04002DB3 RID: 11699
		internal const string MonthCalendarScrollChangeDescr = "MonthCalendarScrollChangeDescr";

		// Token: 0x04002DB4 RID: 11700
		internal const string MonthCalendarSelectionEndDescr = "MonthCalendarSelectionEndDescr";

		// Token: 0x04002DB5 RID: 11701
		internal const string MonthCalendarSelectionRangeDescr = "MonthCalendarSelectionRangeDescr";

		// Token: 0x04002DB6 RID: 11702
		internal const string MonthCalendarSelectionStartDescr = "MonthCalendarSelectionStartDescr";

		// Token: 0x04002DB7 RID: 11703
		internal const string MonthCalendarShowTodayCircleDescr = "MonthCalendarShowTodayCircleDescr";

		// Token: 0x04002DB8 RID: 11704
		internal const string MonthCalendarShowTodayDescr = "MonthCalendarShowTodayDescr";

		// Token: 0x04002DB9 RID: 11705
		internal const string MonthCalendarShowWeekNumbersDescr = "MonthCalendarShowWeekNumbersDescr";

		// Token: 0x04002DBA RID: 11706
		internal const string MonthCalendarSingleMonthSizeDescr = "MonthCalendarSingleMonthSizeDescr";

		// Token: 0x04002DBB RID: 11707
		internal const string MonthCalendarTitleBackColorDescr = "MonthCalendarTitleBackColorDescr";

		// Token: 0x04002DBC RID: 11708
		internal const string MonthCalendarTitleForeColorDescr = "MonthCalendarTitleForeColorDescr";

		// Token: 0x04002DBD RID: 11709
		internal const string MonthCalendarTodayDateDescr = "MonthCalendarTodayDateDescr";

		// Token: 0x04002DBE RID: 11710
		internal const string MonthCalendarTodayDateSetDescr = "MonthCalendarTodayDateSetDescr";

		// Token: 0x04002DBF RID: 11711
		internal const string MonthCalendarTrailingForeColorDescr = "MonthCalendarTrailingForeColorDescr";

		// Token: 0x04002DC0 RID: 11712
		internal const string NoAllowNewOnReadOnlyList = "NoAllowNewOnReadOnlyList";

		// Token: 0x04002DC1 RID: 11713
		internal const string NoAllowRemoveOnReadOnlyList = "NoAllowRemoveOnReadOnlyList";

		// Token: 0x04002DC2 RID: 11714
		internal const string NoDefaultConstructor = "NoDefaultConstructor";

		// Token: 0x04002DC3 RID: 11715
		internal const string NoMoreColumns = "NoMoreColumns";

		// Token: 0x04002DC4 RID: 11716
		internal const string NonTopLevelCantHaveOwner = "NonTopLevelCantHaveOwner";

		// Token: 0x04002DC5 RID: 11717
		internal const string NotAvailable = "NotAvailable";

		// Token: 0x04002DC6 RID: 11718
		internal const string NotifyIconBalloonTipIconDescr = "NotifyIconBalloonTipIconDescr";

		// Token: 0x04002DC7 RID: 11719
		internal const string NotifyIconBalloonTipTextDescr = "NotifyIconBalloonTipTextDescr";

		// Token: 0x04002DC8 RID: 11720
		internal const string NotifyIconBalloonTipTitleDescr = "NotifyIconBalloonTipTitleDescr";

		// Token: 0x04002DC9 RID: 11721
		internal const string NotifyIconEmptyOrNullTipText = "NotifyIconEmptyOrNullTipText";

		// Token: 0x04002DCA RID: 11722
		internal const string NotifyIconIconDescr = "NotifyIconIconDescr";

		// Token: 0x04002DCB RID: 11723
		internal const string NotifyIconMenuDescr = "NotifyIconMenuDescr";

		// Token: 0x04002DCC RID: 11724
		internal const string NotifyIconMouseClickDescr = "NotifyIconMouseClickDescr";

		// Token: 0x04002DCD RID: 11725
		internal const string NotifyIconMouseDoubleClickDescr = "NotifyIconMouseDoubleClickDescr";

		// Token: 0x04002DCE RID: 11726
		internal const string NotifyIconOnBalloonTipClickedDescr = "NotifyIconOnBalloonTipClickedDescr";

		// Token: 0x04002DCF RID: 11727
		internal const string NotifyIconOnBalloonTipClosedDescr = "NotifyIconOnBalloonTipClosedDescr";

		// Token: 0x04002DD0 RID: 11728
		internal const string NotifyIconOnBalloonTipShownDescr = "NotifyIconOnBalloonTipShownDescr";

		// Token: 0x04002DD1 RID: 11729
		internal const string NotifyIconTextDescr = "NotifyIconTextDescr";

		// Token: 0x04002DD2 RID: 11730
		internal const string NotifyIconVisDescr = "NotifyIconVisDescr";

		// Token: 0x04002DD3 RID: 11731
		internal const string NotSerializableType = "NotSerializableType";

		// Token: 0x04002DD4 RID: 11732
		internal const string NotSupported = "NotSupported";

		// Token: 0x04002DD5 RID: 11733
		internal const string NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull = "NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull";

		// Token: 0x04002DD6 RID: 11734
		internal const string NumericUpDownAccelerationCompareException = "NumericUpDownAccelerationCompareException";

		// Token: 0x04002DD7 RID: 11735
		internal const string NumericUpDownDecimalPlacesDescr = "NumericUpDownDecimalPlacesDescr";

		// Token: 0x04002DD8 RID: 11736
		internal const string NumericUpDownHexadecimalDescr = "NumericUpDownHexadecimalDescr";

		// Token: 0x04002DD9 RID: 11737
		internal const string NumericUpDownIncrementDescr = "NumericUpDownIncrementDescr";

		// Token: 0x04002DDA RID: 11738
		internal const string NumericUpDownLessThanZeroError = "NumericUpDownLessThanZeroError";

		// Token: 0x04002DDB RID: 11739
		internal const string NumericUpDownMaximumDescr = "NumericUpDownMaximumDescr";

		// Token: 0x04002DDC RID: 11740
		internal const string NumericUpDownMinimumDescr = "NumericUpDownMinimumDescr";

		// Token: 0x04002DDD RID: 11741
		internal const string NumericUpDownOnValueChangedDescr = "NumericUpDownOnValueChangedDescr";

		// Token: 0x04002DDE RID: 11742
		internal const string NumericUpDownThousandsSeparatorDescr = "NumericUpDownThousandsSeparatorDescr";

		// Token: 0x04002DDF RID: 11743
		internal const string NumericUpDownValueDescr = "NumericUpDownValueDescr";

		// Token: 0x04002DE0 RID: 11744
		internal const string ObjectDisposed = "ObjectDisposed";

		// Token: 0x04002DE1 RID: 11745
		internal const string ObjectHasParent = "ObjectHasParent";

		// Token: 0x04002DE2 RID: 11746
		internal const string OFDcheckFileExistsDescr = "OFDcheckFileExistsDescr";

		// Token: 0x04002DE3 RID: 11747
		internal const string OFDmultiSelectDescr = "OFDmultiSelectDescr";

		// Token: 0x04002DE4 RID: 11748
		internal const string OFDreadOnlyCheckedDescr = "OFDreadOnlyCheckedDescr";

		// Token: 0x04002DE5 RID: 11749
		internal const string OFDshowReadOnlyDescr = "OFDshowReadOnlyDescr";

		// Token: 0x04002DE6 RID: 11750
		internal const string OKCaption = "OKCaption";

		// Token: 0x04002DE7 RID: 11751
		internal const string OnlyOneControl = "OnlyOneControl";

		// Token: 0x04002DE8 RID: 11752
		internal const string OperationRequiresIBindingList = "OperationRequiresIBindingList";

		// Token: 0x04002DE9 RID: 11753
		internal const string OperationRequiresIBindingListView = "OperationRequiresIBindingListView";

		// Token: 0x04002DEA RID: 11754
		internal const string OutOfMemory = "OutOfMemory";

		// Token: 0x04002DEB RID: 11755
		internal const string OwnsSelfOrOwner = "OwnsSelfOrOwner";

		// Token: 0x04002DEC RID: 11756
		internal const string PaddingAllDescr = "PaddingAllDescr";

		// Token: 0x04002DED RID: 11757
		internal const string PaddingBottomDescr = "PaddingBottomDescr";

		// Token: 0x04002DEE RID: 11758
		internal const string PaddingLeftDescr = "PaddingLeftDescr";

		// Token: 0x04002DEF RID: 11759
		internal const string PaddingRightDescr = "PaddingRightDescr";

		// Token: 0x04002DF0 RID: 11760
		internal const string PaddingTopDescr = "PaddingTopDescr";

		// Token: 0x04002DF1 RID: 11761
		internal const string PanelBorderStyleDescr = "PanelBorderStyleDescr";

		// Token: 0x04002DF2 RID: 11762
		internal const string PBRSDocCommentPaneTitle = "PBRSDocCommentPaneTitle";

		// Token: 0x04002DF3 RID: 11763
		internal const string PBRSErrorInvalidPropertyValue = "PBRSErrorInvalidPropertyValue";

		// Token: 0x04002DF4 RID: 11764
		internal const string PBRSErrorTitle = "PBRSErrorTitle";

		// Token: 0x04002DF5 RID: 11765
		internal const string PBRSFormatExceptionMessage = "PBRSFormatExceptionMessage";

		// Token: 0x04002DF6 RID: 11766
		internal const string PBRSToolTipAlphabetic = "PBRSToolTipAlphabetic";

		// Token: 0x04002DF7 RID: 11767
		internal const string PBRSToolTipCategorized = "PBRSToolTipCategorized";

		// Token: 0x04002DF8 RID: 11768
		internal const string PBRSToolTipEvents = "PBRSToolTipEvents";

		// Token: 0x04002DF9 RID: 11769
		internal const string PBRSToolTipProperties = "PBRSToolTipProperties";

		// Token: 0x04002DFA RID: 11770
		internal const string PBRSToolTipPropertyPages = "PBRSToolTipPropertyPages";

		// Token: 0x04002DFB RID: 11771
		internal const string PDallowCurrentPageDescr = "PDallowCurrentPageDescr";

		// Token: 0x04002DFC RID: 11772
		internal const string PDallowPagesDescr = "PDallowPagesDescr";

		// Token: 0x04002DFD RID: 11773
		internal const string PDallowPrintToFileDescr = "PDallowPrintToFileDescr";

		// Token: 0x04002DFE RID: 11774
		internal const string PDallowSelectionDescr = "PDallowSelectionDescr";

		// Token: 0x04002DFF RID: 11775
		internal const string PDcantShowWithoutPrinter = "PDcantShowWithoutPrinter";

		// Token: 0x04002E00 RID: 11776
		internal const string PDdocumentDescr = "PDdocumentDescr";

		// Token: 0x04002E01 RID: 11777
		internal const string PDpageOutOfRange = "PDpageOutOfRange";

		// Token: 0x04002E02 RID: 11778
		internal const string PDprinterSettingsDescr = "PDprinterSettingsDescr";

		// Token: 0x04002E03 RID: 11779
		internal const string PDprintToFileDescr = "PDprintToFileDescr";

		// Token: 0x04002E04 RID: 11780
		internal const string PDshowHelpDescr = "PDshowHelpDescr";

		// Token: 0x04002E05 RID: 11781
		internal const string PDshowNetworkDescr = "PDshowNetworkDescr";

		// Token: 0x04002E06 RID: 11782
		internal const string PDuseEXDialog = "PDuseEXDialog";

		// Token: 0x04002E07 RID: 11783
		internal const string PictureBoxBorderStyleDescr = "PictureBoxBorderStyleDescr";

		// Token: 0x04002E08 RID: 11784
		internal const string PictureBoxCancelAsyncDescr = "PictureBoxCancelAsyncDescr";

		// Token: 0x04002E09 RID: 11785
		internal const string PictureBoxErrorImageDescr = "PictureBoxErrorImageDescr";

		// Token: 0x04002E0A RID: 11786
		internal const string PictureBoxImageDescr = "PictureBoxImageDescr";

		// Token: 0x04002E0B RID: 11787
		internal const string PictureBoxImageLocationDescr = "PictureBoxImageLocationDescr";

		// Token: 0x04002E0C RID: 11788
		internal const string PictureBoxInitialImageDescr = "PictureBoxInitialImageDescr";

		// Token: 0x04002E0D RID: 11789
		internal const string PictureBoxLoad0Descr = "PictureBoxLoad0Descr";

		// Token: 0x04002E0E RID: 11790
		internal const string PictureBoxLoad1Descr = "PictureBoxLoad1Descr";

		// Token: 0x04002E0F RID: 11791
		internal const string PictureBoxLoadAsync0Descr = "PictureBoxLoadAsync0Descr";

		// Token: 0x04002E10 RID: 11792
		internal const string PictureBoxLoadAsync1Descr = "PictureBoxLoadAsync1Descr";

		// Token: 0x04002E11 RID: 11793
		internal const string PictureBoxLoadCompletedDescr = "PictureBoxLoadCompletedDescr";

		// Token: 0x04002E12 RID: 11794
		internal const string PictureBoxLoadProgressChangedDescr = "PictureBoxLoadProgressChangedDescr";

		// Token: 0x04002E13 RID: 11795
		internal const string PictureBoxLoadProgressDescr = "PictureBoxLoadProgressDescr";

		// Token: 0x04002E14 RID: 11796
		internal const string PictureBoxNoImageLocation = "PictureBoxNoImageLocation";

		// Token: 0x04002E15 RID: 11797
		internal const string PictureBoxOnSizeModeChangedDescr = "PictureBoxOnSizeModeChangedDescr";

		// Token: 0x04002E16 RID: 11798
		internal const string PictureBoxSizeModeDescr = "PictureBoxSizeModeDescr";

		// Token: 0x04002E17 RID: 11799
		internal const string PictureBoxWaitOnLoadDescr = "PictureBoxWaitOnLoadDescr";

		// Token: 0x04002E18 RID: 11800
		internal const string PopupControlBadParentArgument = "PopupControlBadParentArgument";

		// Token: 0x04002E19 RID: 11801
		internal const string PreviewKeyDownDescr = "PreviewKeyDownDescr";

		// Token: 0x04002E1A RID: 11802
		internal const string PrintControllerWithStatusDialog_Cancel = "PrintControllerWithStatusDialog_Cancel";

		// Token: 0x04002E1B RID: 11803
		internal const string PrintControllerWithStatusDialog_Canceling = "PrintControllerWithStatusDialog_Canceling";

		// Token: 0x04002E1C RID: 11804
		internal const string PrintControllerWithStatusDialog_DialogTitlePreview = "PrintControllerWithStatusDialog_DialogTitlePreview";

		// Token: 0x04002E1D RID: 11805
		internal const string PrintControllerWithStatusDialog_DialogTitlePrint = "PrintControllerWithStatusDialog_DialogTitlePrint";

		// Token: 0x04002E1E RID: 11806
		internal const string PrintControllerWithStatusDialog_NowPrinting = "PrintControllerWithStatusDialog_NowPrinting";

		// Token: 0x04002E1F RID: 11807
		internal const string PrintPreviewAntiAliasDescr = "PrintPreviewAntiAliasDescr";

		// Token: 0x04002E20 RID: 11808
		internal const string PrintPreviewAutoZoomDescr = "PrintPreviewAutoZoomDescr";

		// Token: 0x04002E21 RID: 11809
		internal const string PrintPreviewColumnsDescr = "PrintPreviewColumnsDescr";

		// Token: 0x04002E22 RID: 11810
		internal const string PrintPreviewControlZoomNegative = "PrintPreviewControlZoomNegative";

		// Token: 0x04002E23 RID: 11811
		internal const string PrintPreviewDialog_Close = "PrintPreviewDialog_Close";

		// Token: 0x04002E24 RID: 11812
		internal const string PrintPreviewDialog_FourPages = "PrintPreviewDialog_FourPages";

		// Token: 0x04002E25 RID: 11813
		internal const string PrintPreviewDialog_OnePage = "PrintPreviewDialog_OnePage";

		// Token: 0x04002E26 RID: 11814
		internal const string PrintPreviewDialog_Page = "PrintPreviewDialog_Page";

		// Token: 0x04002E27 RID: 11815
		internal const string PrintPreviewDialog_Print = "PrintPreviewDialog_Print";

		// Token: 0x04002E28 RID: 11816
		internal const string PrintPreviewDialog_PrintPreview = "PrintPreviewDialog_PrintPreview";

		// Token: 0x04002E29 RID: 11817
		internal const string PrintPreviewDialog_SixPages = "PrintPreviewDialog_SixPages";

		// Token: 0x04002E2A RID: 11818
		internal const string PrintPreviewDialog_ThreePages = "PrintPreviewDialog_ThreePages";

		// Token: 0x04002E2B RID: 11819
		internal const string PrintPreviewDialog_TwoPages = "PrintPreviewDialog_TwoPages";

		// Token: 0x04002E2C RID: 11820
		internal const string PrintPreviewDialog_Zoom = "PrintPreviewDialog_Zoom";

		// Token: 0x04002E2D RID: 11821
		internal const string PrintPreviewDialog_Zoom10 = "PrintPreviewDialog_Zoom10";

		// Token: 0x04002E2E RID: 11822
		internal const string PrintPreviewDialog_Zoom100 = "PrintPreviewDialog_Zoom100";

		// Token: 0x04002E2F RID: 11823
		internal const string PrintPreviewDialog_Zoom150 = "PrintPreviewDialog_Zoom150";

		// Token: 0x04002E30 RID: 11824
		internal const string PrintPreviewDialog_Zoom200 = "PrintPreviewDialog_Zoom200";

		// Token: 0x04002E31 RID: 11825
		internal const string PrintPreviewDialog_Zoom25 = "PrintPreviewDialog_Zoom25";

		// Token: 0x04002E32 RID: 11826
		internal const string PrintPreviewDialog_Zoom50 = "PrintPreviewDialog_Zoom50";

		// Token: 0x04002E33 RID: 11827
		internal const string PrintPreviewDialog_Zoom500 = "PrintPreviewDialog_Zoom500";

		// Token: 0x04002E34 RID: 11828
		internal const string PrintPreviewDialog_Zoom75 = "PrintPreviewDialog_Zoom75";

		// Token: 0x04002E35 RID: 11829
		internal const string PrintPreviewDialog_ZoomAuto = "PrintPreviewDialog_ZoomAuto";

		// Token: 0x04002E36 RID: 11830
		internal const string PrintPreviewDocumentDescr = "PrintPreviewDocumentDescr";

		// Token: 0x04002E37 RID: 11831
		internal const string PrintPreviewExceptionPrinting = "PrintPreviewExceptionPrinting";

		// Token: 0x04002E38 RID: 11832
		internal const string PrintPreviewNoPages = "PrintPreviewNoPages";

		// Token: 0x04002E39 RID: 11833
		internal const string PrintPreviewPrintPreviewControlDescr = "PrintPreviewPrintPreviewControlDescr";

		// Token: 0x04002E3A RID: 11834
		internal const string PrintPreviewRowsDescr = "PrintPreviewRowsDescr";

		// Token: 0x04002E3B RID: 11835
		internal const string PrintPreviewStartPageDescr = "PrintPreviewStartPageDescr";

		// Token: 0x04002E3C RID: 11836
		internal const string PrintPreviewZoomDescr = "PrintPreviewZoomDescr";

		// Token: 0x04002E3D RID: 11837
		internal const string ProfessionalColorsButtonCheckedGradientBeginDescr = "ProfessionalColorsButtonCheckedGradientBeginDescr";

		// Token: 0x04002E3E RID: 11838
		internal const string ProfessionalColorsButtonCheckedGradientEndDescr = "ProfessionalColorsButtonCheckedGradientEndDescr";

		// Token: 0x04002E3F RID: 11839
		internal const string ProfessionalColorsButtonCheckedGradientMiddleDescr = "ProfessionalColorsButtonCheckedGradientMiddleDescr";

		// Token: 0x04002E40 RID: 11840
		internal const string ProfessionalColorsButtonCheckedHighlightBorderDescr = "ProfessionalColorsButtonCheckedHighlightBorderDescr";

		// Token: 0x04002E41 RID: 11841
		internal const string ProfessionalColorsButtonCheckedHighlightDescr = "ProfessionalColorsButtonCheckedHighlightDescr";

		// Token: 0x04002E42 RID: 11842
		internal const string ProfessionalColorsButtonPressedBorderDescr = "ProfessionalColorsButtonPressedBorderDescr";

		// Token: 0x04002E43 RID: 11843
		internal const string ProfessionalColorsButtonPressedGradientBeginDescr = "ProfessionalColorsButtonPressedGradientBeginDescr";

		// Token: 0x04002E44 RID: 11844
		internal const string ProfessionalColorsButtonPressedGradientEndDescr = "ProfessionalColorsButtonPressedGradientEndDescr";

		// Token: 0x04002E45 RID: 11845
		internal const string ProfessionalColorsButtonPressedGradientMiddleDescr = "ProfessionalColorsButtonPressedGradientMiddleDescr";

		// Token: 0x04002E46 RID: 11846
		internal const string ProfessionalColorsButtonPressedHighlightBorderDescr = "ProfessionalColorsButtonPressedHighlightBorderDescr";

		// Token: 0x04002E47 RID: 11847
		internal const string ProfessionalColorsButtonPressedHighlightDescr = "ProfessionalColorsButtonPressedHighlightDescr";

		// Token: 0x04002E48 RID: 11848
		internal const string ProfessionalColorsButtonSelectedBorderDescr = "ProfessionalColorsButtonSelectedBorderDescr";

		// Token: 0x04002E49 RID: 11849
		internal const string ProfessionalColorsButtonSelectedGradientBeginDescr = "ProfessionalColorsButtonSelectedGradientBeginDescr";

		// Token: 0x04002E4A RID: 11850
		internal const string ProfessionalColorsButtonSelectedGradientEndDescr = "ProfessionalColorsButtonSelectedGradientEndDescr";

		// Token: 0x04002E4B RID: 11851
		internal const string ProfessionalColorsButtonSelectedGradientMiddleDescr = "ProfessionalColorsButtonSelectedGradientMiddleDescr";

		// Token: 0x04002E4C RID: 11852
		internal const string ProfessionalColorsButtonSelectedHighlightBorderDescr = "ProfessionalColorsButtonSelectedHighlightBorderDescr";

		// Token: 0x04002E4D RID: 11853
		internal const string ProfessionalColorsButtonSelectedHighlightDescr = "ProfessionalColorsButtonSelectedHighlightDescr";

		// Token: 0x04002E4E RID: 11854
		internal const string ProfessionalColorsCheckBackgroundDescr = "ProfessionalColorsCheckBackgroundDescr";

		// Token: 0x04002E4F RID: 11855
		internal const string ProfessionalColorsCheckPressedBackgroundDescr = "ProfessionalColorsCheckPressedBackgroundDescr";

		// Token: 0x04002E50 RID: 11856
		internal const string ProfessionalColorsCheckSelectedBackgroundDescr = "ProfessionalColorsCheckSelectedBackgroundDescr";

		// Token: 0x04002E51 RID: 11857
		internal const string ProfessionalColorsGripDarkDescr = "ProfessionalColorsGripDarkDescr";

		// Token: 0x04002E52 RID: 11858
		internal const string ProfessionalColorsGripLightDescr = "ProfessionalColorsGripLightDescr";

		// Token: 0x04002E53 RID: 11859
		internal const string ProfessionalColorsImageMarginGradientBeginDescr = "ProfessionalColorsImageMarginGradientBeginDescr";

		// Token: 0x04002E54 RID: 11860
		internal const string ProfessionalColorsImageMarginGradientEndDescr = "ProfessionalColorsImageMarginGradientEndDescr";

		// Token: 0x04002E55 RID: 11861
		internal const string ProfessionalColorsImageMarginGradientMiddleDescr = "ProfessionalColorsImageMarginGradientMiddleDescr";

		// Token: 0x04002E56 RID: 11862
		internal const string ProfessionalColorsImageMarginRevealedGradientBeginDescr = "ProfessionalColorsImageMarginRevealedGradientBeginDescr";

		// Token: 0x04002E57 RID: 11863
		internal const string ProfessionalColorsImageMarginRevealedGradientEndDescr = "ProfessionalColorsImageMarginRevealedGradientEndDescr";

		// Token: 0x04002E58 RID: 11864
		internal const string ProfessionalColorsImageMarginRevealedGradientMiddleDescr = "ProfessionalColorsImageMarginRevealedGradientMiddleDescr";

		// Token: 0x04002E59 RID: 11865
		internal const string ProfessionalColorsMenuBorderDescr = "ProfessionalColorsMenuBorderDescr";

		// Token: 0x04002E5A RID: 11866
		internal const string ProfessionalColorsMenuItemBorderDescr = "ProfessionalColorsMenuItemBorderDescr";

		// Token: 0x04002E5B RID: 11867
		internal const string ProfessionalColorsMenuItemPressedGradientBeginDescr = "ProfessionalColorsMenuItemPressedGradientBeginDescr";

		// Token: 0x04002E5C RID: 11868
		internal const string ProfessionalColorsMenuItemPressedGradientEndDescr = "ProfessionalColorsMenuItemPressedGradientEndDescr";

		// Token: 0x04002E5D RID: 11869
		internal const string ProfessionalColorsMenuItemPressedGradientMiddleDescr = "ProfessionalColorsMenuItemPressedGradientMiddleDescr";

		// Token: 0x04002E5E RID: 11870
		internal const string ProfessionalColorsMenuItemSelectedDescr = "ProfessionalColorsMenuItemSelectedDescr";

		// Token: 0x04002E5F RID: 11871
		internal const string ProfessionalColorsMenuItemSelectedGradientBeginDescr = "ProfessionalColorsMenuItemSelectedGradientBeginDescr";

		// Token: 0x04002E60 RID: 11872
		internal const string ProfessionalColorsMenuItemSelectedGradientEndDescr = "ProfessionalColorsMenuItemSelectedGradientEndDescr";

		// Token: 0x04002E61 RID: 11873
		internal const string ProfessionalColorsMenuStripGradientBeginDescr = "ProfessionalColorsMenuStripGradientBeginDescr";

		// Token: 0x04002E62 RID: 11874
		internal const string ProfessionalColorsMenuStripGradientEndDescr = "ProfessionalColorsMenuStripGradientEndDescr";

		// Token: 0x04002E63 RID: 11875
		internal const string ProfessionalColorsOverflowButtonGradientBeginDescr = "ProfessionalColorsOverflowButtonGradientBeginDescr";

		// Token: 0x04002E64 RID: 11876
		internal const string ProfessionalColorsOverflowButtonGradientEndDescr = "ProfessionalColorsOverflowButtonGradientEndDescr";

		// Token: 0x04002E65 RID: 11877
		internal const string ProfessionalColorsOverflowButtonGradientMiddleDescr = "ProfessionalColorsOverflowButtonGradientMiddleDescr";

		// Token: 0x04002E66 RID: 11878
		internal const string ProfessionalColorsRaftingContainerGradientBeginDescr = "ProfessionalColorsRaftingContainerGradientBeginDescr";

		// Token: 0x04002E67 RID: 11879
		internal const string ProfessionalColorsRaftingContainerGradientEndDescr = "ProfessionalColorsRaftingContainerGradientEndDescr";

		// Token: 0x04002E68 RID: 11880
		internal const string ProfessionalColorsSeparatorDarkDescr = "ProfessionalColorsSeparatorDarkDescr";

		// Token: 0x04002E69 RID: 11881
		internal const string ProfessionalColorsSeparatorLightDescr = "ProfessionalColorsSeparatorLightDescr";

		// Token: 0x04002E6A RID: 11882
		internal const string ProfessionalColorsStatusStripGradientBeginDescr = "ProfessionalColorsStatusStripGradientBeginDescr";

		// Token: 0x04002E6B RID: 11883
		internal const string ProfessionalColorsStatusStripGradientEndDescr = "ProfessionalColorsStatusStripGradientEndDescr";

		// Token: 0x04002E6C RID: 11884
		internal const string ProfessionalColorsToolStripBorderDescr = "ProfessionalColorsToolStripBorderDescr";

		// Token: 0x04002E6D RID: 11885
		internal const string ProfessionalColorsToolStripContentPanelGradientBeginDescr = "ProfessionalColorsToolStripContentPanelGradientBeginDescr";

		// Token: 0x04002E6E RID: 11886
		internal const string ProfessionalColorsToolStripContentPanelGradientEndDescr = "ProfessionalColorsToolStripContentPanelGradientEndDescr";

		// Token: 0x04002E6F RID: 11887
		internal const string ProfessionalColorsToolStripDropDownBackgroundDescr = "ProfessionalColorsToolStripDropDownBackgroundDescr";

		// Token: 0x04002E70 RID: 11888
		internal const string ProfessionalColorsToolStripGradientBeginDescr = "ProfessionalColorsToolStripGradientBeginDescr";

		// Token: 0x04002E71 RID: 11889
		internal const string ProfessionalColorsToolStripGradientEndDescr = "ProfessionalColorsToolStripGradientEndDescr";

		// Token: 0x04002E72 RID: 11890
		internal const string ProfessionalColorsToolStripGradientMiddleDescr = "ProfessionalColorsToolStripGradientMiddleDescr";

		// Token: 0x04002E73 RID: 11891
		internal const string ProfessionalColorsToolStripPanelGradientBeginDescr = "ProfessionalColorsToolStripPanelGradientBeginDescr";

		// Token: 0x04002E74 RID: 11892
		internal const string ProfessionalColorsToolStripPanelGradientEndDescr = "ProfessionalColorsToolStripPanelGradientEndDescr";

		// Token: 0x04002E75 RID: 11893
		internal const string ProgressBarIncrementMarqueeException = "ProgressBarIncrementMarqueeException";

		// Token: 0x04002E76 RID: 11894
		internal const string ProgressBarMarqueeAnimationSpeed = "ProgressBarMarqueeAnimationSpeed";

		// Token: 0x04002E77 RID: 11895
		internal const string ProgressBarMaximumDescr = "ProgressBarMaximumDescr";

		// Token: 0x04002E78 RID: 11896
		internal const string ProgressBarMinimumDescr = "ProgressBarMinimumDescr";

		// Token: 0x04002E79 RID: 11897
		internal const string ProgressBarPerformStepMarqueeException = "ProgressBarPerformStepMarqueeException";

		// Token: 0x04002E7A RID: 11898
		internal const string ProgressBarStepDescr = "ProgressBarStepDescr";

		// Token: 0x04002E7B RID: 11899
		internal const string ProgressBarStyleDescr = "ProgressBarStyleDescr";

		// Token: 0x04002E7C RID: 11900
		internal const string ProgressBarValueDescr = "ProgressBarValueDescr";

		// Token: 0x04002E7D RID: 11901
		internal const string ProgressBarValueMarqueeException = "ProgressBarValueMarqueeException";

		// Token: 0x04002E7E RID: 11902
		internal const string PropertyCategoryAppearance = "PropertyCategoryAppearance";

		// Token: 0x04002E7F RID: 11903
		internal const string PropertyCategoryBehavior = "PropertyCategoryBehavior";

		// Token: 0x04002E80 RID: 11904
		internal const string PropertyCategoryData = "PropertyCategoryData";

		// Token: 0x04002E81 RID: 11905
		internal const string PropertyCategoryDDE = "PropertyCategoryDDE";

		// Token: 0x04002E82 RID: 11906
		internal const string PropertyCategoryFont = "PropertyCategoryFont";

		// Token: 0x04002E83 RID: 11907
		internal const string PropertyCategoryList = "PropertyCategoryList";

		// Token: 0x04002E84 RID: 11908
		internal const string PropertyCategoryMisc = "PropertyCategoryMisc";

		// Token: 0x04002E85 RID: 11909
		internal const string PropertyCategoryPosition = "PropertyCategoryPosition";

		// Token: 0x04002E86 RID: 11910
		internal const string PropertyCategoryScale = "PropertyCategoryScale";

		// Token: 0x04002E87 RID: 11911
		internal const string PropertyCategoryText = "PropertyCategoryText";

		// Token: 0x04002E88 RID: 11912
		internal const string PropertyGridBadTabIndex = "PropertyGridBadTabIndex";

		// Token: 0x04002E89 RID: 11913
		internal const string PropertyGridCanShowCommandsDesc = "PropertyGridCanShowCommandsDesc";

		// Token: 0x04002E8A RID: 11914
		internal const string PropertyGridCanShowVisualStyleGlyphsDesc = "PropertyGridCanShowVisualStyleGlyphsDesc";

		// Token: 0x04002E8B RID: 11915
		internal const string PropertyGridCategoryForeColorDesc = "PropertyGridCategoryForeColorDesc";

		// Token: 0x04002E8C RID: 11916
		internal const string PropertyGridCategorySplitterColorDesc = "PropertyGridCategorySplitterColorDesc";

		// Token: 0x04002E8D RID: 11917
		internal const string PropertyGridCommandsActiveLinkColorDesc = "PropertyGridCommandsActiveLinkColorDesc";

		// Token: 0x04002E8E RID: 11918
		internal const string PropertyGridCommandsBackColorDesc = "PropertyGridCommandsBackColorDesc";

		// Token: 0x04002E8F RID: 11919
		internal const string PropertyGridCommandsBorderColorDesc = "PropertyGridCommandsBorderColorDesc";

		// Token: 0x04002E90 RID: 11920
		internal const string PropertyGridCommandsDisabledLinkColorDesc = "PropertyGridCommandsDisabledLinkColorDesc";

		// Token: 0x04002E91 RID: 11921
		internal const string PropertyGridCommandsForeColorDesc = "PropertyGridCommandsForeColorDesc";

		// Token: 0x04002E92 RID: 11922
		internal const string PropertyGridCommandsLinkColorDesc = "PropertyGridCommandsLinkColorDesc";

		// Token: 0x04002E93 RID: 11923
		internal const string PropertyGridCommandsVisibleIfAvailable = "PropertyGridCommandsVisibleIfAvailable";

		// Token: 0x04002E94 RID: 11924
		internal const string PropertyGridDefaultAccessibleName = "PropertyGridDefaultAccessibleName";

		// Token: 0x04002E95 RID: 11925
		internal const string PropertyGridDisabledItemForeColorDesc = "PropertyGridDisabledItemForeColorDesc";

		// Token: 0x04002E96 RID: 11926
		internal const string PropertyGridDropDownButtonAccessibleName = "PropertyGridDropDownButtonAccessibleName";

		// Token: 0x04002E97 RID: 11927
		internal const string PropertyGridExceptionInfo = "PropertyGridExceptionInfo";

		// Token: 0x04002E98 RID: 11928
		internal const string PropertyGridExceptionWhilePaintingLabel = "PropertyGridExceptionWhilePaintingLabel";

		// Token: 0x04002E99 RID: 11929
		internal const string PropertyGridHelpBackColorDesc = "PropertyGridHelpBackColorDesc";

		// Token: 0x04002E9A RID: 11930
		internal const string PropertyGridHelpBorderColorDesc = "PropertyGridHelpBorderColorDesc";

		// Token: 0x04002E9B RID: 11931
		internal const string PropertyGridHelpForeColorDesc = "PropertyGridHelpForeColorDesc";

		// Token: 0x04002E9C RID: 11932
		internal const string PropertyGridHelpVisibleDesc = "PropertyGridHelpVisibleDesc";

		// Token: 0x04002E9D RID: 11933
		internal const string PropertyGridSelectedItemWithFocusBackColorDesc = "PropertyGridSelectedItemWithFocusBackColorDesc";

		// Token: 0x04002E9E RID: 11934
		internal const string PropertyGridSelectedItemWithFocusForeColorDesc = "PropertyGridSelectedItemWithFocusForeColorDesc";

		// Token: 0x04002E9F RID: 11935
		internal const string PropertyGridInternalNoProp = "PropertyGridInternalNoProp";

		// Token: 0x04002EA0 RID: 11936
		internal const string PropertyGridInvalidGridEntry = "PropertyGridInvalidGridEntry";

		// Token: 0x04002EA1 RID: 11937
		internal const string PropertyGridLargeButtonsDesc = "PropertyGridLargeButtonsDesc";

		// Token: 0x04002EA2 RID: 11938
		internal const string PropertyGridLineColorDesc = "PropertyGridLineColorDesc";

		// Token: 0x04002EA3 RID: 11939
		internal const string PropertyGridNoBitmap = "PropertyGridNoBitmap";

		// Token: 0x04002EA4 RID: 11940
		internal const string PropertyGridPropertySortChangedDescr = "PropertyGridPropertySortChangedDescr";

		// Token: 0x04002EA5 RID: 11941
		internal const string PropertyGridPropertySortDesc = "PropertyGridPropertySortDesc";

		// Token: 0x04002EA6 RID: 11942
		internal const string PropertyGridPropertyTabchangedDescr = "PropertyGridPropertyTabchangedDescr";

		// Token: 0x04002EA7 RID: 11943
		internal const string PropertyGridPropertyTabCollectionReadOnly = "PropertyGridPropertyTabCollectionReadOnly";

		// Token: 0x04002EA8 RID: 11944
		internal const string PropertyGridPropertyValueChangedDescr = "PropertyGridPropertyValueChangedDescr";

		// Token: 0x04002EA9 RID: 11945
		internal const string PropertyGridRemotedObject = "PropertyGridRemotedObject";

		// Token: 0x04002EAA RID: 11946
		internal const string PropertyGridRemoveStaticTabs = "PropertyGridRemoveStaticTabs";

		// Token: 0x04002EAB RID: 11947
		internal const string PropertyGridResetValue = "PropertyGridResetValue";

		// Token: 0x04002EAC RID: 11948
		internal const string PropertyGridSelectedGridItemChangedDescr = "PropertyGridSelectedGridItemChangedDescr";

		// Token: 0x04002EAD RID: 11949
		internal const string PropertyGridSelectedObjectDesc = "PropertyGridSelectedObjectDesc";

		// Token: 0x04002EAE RID: 11950
		internal const string PropertyGridSelectedObjectsChangedDescr = "PropertyGridSelectedObjectsChangedDescr";

		// Token: 0x04002EAF RID: 11951
		internal const string PropertyGridSet = "PropertyGridSet";

		// Token: 0x04002EB0 RID: 11952
		internal const string PropertyGridSetNull = "PropertyGridSetNull";

		// Token: 0x04002EB1 RID: 11953
		internal const string PropertyGridSetValue = "PropertyGridSetValue";

		// Token: 0x04002EB2 RID: 11954
		internal const string PropertyGridTabName = "PropertyGridTabName";

		// Token: 0x04002EB3 RID: 11955
		internal const string PropertyGridTabScope = "PropertyGridTabScope";

		// Token: 0x04002EB4 RID: 11956
		internal const string PropertyGridTitle = "PropertyGridTitle";

		// Token: 0x04002EB5 RID: 11957
		internal const string PropertyGridToolbarAccessibleName = "PropertyGridToolbarAccessibleName";

		// Token: 0x04002EB6 RID: 11958
		internal const string PropertyGridToolbarVisibleDesc = "PropertyGridToolbarVisibleDesc";

		// Token: 0x04002EB7 RID: 11959
		internal const string PropertyGridViewBackColorDesc = "PropertyGridViewBackColorDesc";

		// Token: 0x04002EB8 RID: 11960
		internal const string PropertyGridViewBorderColorDesc = "PropertyGridViewBorderColorDesc";

		// Token: 0x04002EB9 RID: 11961
		internal const string PropertyGridViewEditorCreatedInvalidObject = "PropertyGridViewEditorCreatedInvalidObject";

		// Token: 0x04002EBA RID: 11962
		internal const string PropertyGridViewForeColorDesc = "PropertyGridViewForeColorDesc";

		// Token: 0x04002EBB RID: 11963
		internal const string PropertyManagerPropDoesNotExist = "PropertyManagerPropDoesNotExist";

		// Token: 0x04002EBC RID: 11964
		internal const string PropertyValueInvalidEntry = "PropertyValueInvalidEntry";

		// Token: 0x04002EBD RID: 11965
		internal const string PSDallowMarginsDescr = "PSDallowMarginsDescr";

		// Token: 0x04002EBE RID: 11966
		internal const string PSDallowOrientationDescr = "PSDallowOrientationDescr";

		// Token: 0x04002EBF RID: 11967
		internal const string PSDallowPaperDescr = "PSDallowPaperDescr";

		// Token: 0x04002EC0 RID: 11968
		internal const string PSDallowPrinterDescr = "PSDallowPrinterDescr";

		// Token: 0x04002EC1 RID: 11969
		internal const string PSDcantShowWithoutPage = "PSDcantShowWithoutPage";

		// Token: 0x04002EC2 RID: 11970
		internal const string PSDenableMetricDescr = "PSDenableMetricDescr";

		// Token: 0x04002EC3 RID: 11971
		internal const string PSDminMarginsDescr = "PSDminMarginsDescr";

		// Token: 0x04002EC4 RID: 11972
		internal const string PSDpageSettingsDescr = "PSDpageSettingsDescr";

		// Token: 0x04002EC5 RID: 11973
		internal const string PSDprinterSettingsDescr = "PSDprinterSettingsDescr";

		// Token: 0x04002EC6 RID: 11974
		internal const string PSDshowHelpDescr = "PSDshowHelpDescr";

		// Token: 0x04002EC7 RID: 11975
		internal const string PSDshowNetworkDescr = "PSDshowNetworkDescr";

		// Token: 0x04002EC8 RID: 11976
		internal const string RadioButtonAppearanceDescr = "RadioButtonAppearanceDescr";

		// Token: 0x04002EC9 RID: 11977
		internal const string RadioButtonAutoCheckDescr = "RadioButtonAutoCheckDescr";

		// Token: 0x04002ECA RID: 11978
		internal const string RadioButtonCheckAlignDescr = "RadioButtonCheckAlignDescr";

		// Token: 0x04002ECB RID: 11979
		internal const string RadioButtonCheckedDescr = "RadioButtonCheckedDescr";

		// Token: 0x04002ECC RID: 11980
		internal const string RadioButtonOnAppearanceChangedDescr = "RadioButtonOnAppearanceChangedDescr";

		// Token: 0x04002ECD RID: 11981
		internal const string RadioButtonOnCheckedChangedDescr = "RadioButtonOnCheckedChangedDescr";

		// Token: 0x04002ECE RID: 11982
		internal const string RadioButtonOnStartPageChangedDescr = "RadioButtonOnStartPageChangedDescr";

		// Token: 0x04002ECF RID: 11983
		internal const string RadioButtonOnTextAlignChangedDescr = "RadioButtonOnTextAlignChangedDescr";

		// Token: 0x04002ED0 RID: 11984
		internal const string ReadonlyControlsCollection = "ReadonlyControlsCollection";

		// Token: 0x04002ED1 RID: 11985
		internal const string RegisterCFFailed = "RegisterCFFailed";

		// Token: 0x04002ED2 RID: 11986
		internal const string RelatedListManagerChild = "RelatedListManagerChild";

		// Token: 0x04002ED3 RID: 11987
		internal const string RestartNotSupported = "RestartNotSupported";

		// Token: 0x04002ED4 RID: 11988
		internal const string ResXResourceWriterSaved = "ResXResourceWriterSaved";

		// Token: 0x04002ED5 RID: 11989
		internal const string RichControlLresult = "RichControlLresult";

		// Token: 0x04002ED6 RID: 11990
		internal const string RichTextBox_IDCut = "RichTextBox_IDCut";

		// Token: 0x04002ED7 RID: 11991
		internal const string RichTextBox_IDDelete = "RichTextBox_IDDelete";

		// Token: 0x04002ED8 RID: 11992
		internal const string RichTextBox_IDDragDrop = "RichTextBox_IDDragDrop";

		// Token: 0x04002ED9 RID: 11993
		internal const string RichTextBox_IDPaste = "RichTextBox_IDPaste";

		// Token: 0x04002EDA RID: 11994
		internal const string RichTextBox_IDTyping = "RichTextBox_IDTyping";

		// Token: 0x04002EDB RID: 11995
		internal const string RichTextBox_IDUnknown = "RichTextBox_IDUnknown";

		// Token: 0x04002EDC RID: 11996
		internal const string RichTextBoxAutoWordSelection = "RichTextBoxAutoWordSelection";

		// Token: 0x04002EDD RID: 11997
		internal const string RichTextBoxBulletIndent = "RichTextBoxBulletIndent";

		// Token: 0x04002EDE RID: 11998
		internal const string RichTextBoxCanRedoDescr = "RichTextBoxCanRedoDescr";

		// Token: 0x04002EDF RID: 11999
		internal const string RichTextBoxContentsResized = "RichTextBoxContentsResized";

		// Token: 0x04002EE0 RID: 12000
		internal const string RichTextBoxDetectURLs = "RichTextBoxDetectURLs";

		// Token: 0x04002EE1 RID: 12001
		internal const string RichTextBoxEnableAutoDragDrop = "RichTextBoxEnableAutoDragDrop";

		// Token: 0x04002EE2 RID: 12002
		internal const string RichTextBoxHScroll = "RichTextBoxHScroll";

		// Token: 0x04002EE3 RID: 12003
		internal const string RichTextBoxIMEChange = "RichTextBoxIMEChange";

		// Token: 0x04002EE4 RID: 12004
		internal const string RichTextBoxLinkClick = "RichTextBoxLinkClick";

		// Token: 0x04002EE5 RID: 12005
		internal const string RichTextBoxProtected = "RichTextBoxProtected";

		// Token: 0x04002EE6 RID: 12006
		internal const string RichTextBoxRedoActionNameDescr = "RichTextBoxRedoActionNameDescr";

		// Token: 0x04002EE7 RID: 12007
		internal const string RichTextBoxRightMargin = "RichTextBoxRightMargin";

		// Token: 0x04002EE8 RID: 12008
		internal const string RichTextBoxRTF = "RichTextBoxRTF";

		// Token: 0x04002EE9 RID: 12009
		internal const string RichTextBoxScrollBars = "RichTextBoxScrollBars";

		// Token: 0x04002EEA RID: 12010
		internal const string RichTextBoxSelAlignment = "RichTextBoxSelAlignment";

		// Token: 0x04002EEB RID: 12011
		internal const string RichTextBoxSelBackColor = "RichTextBoxSelBackColor";

		// Token: 0x04002EEC RID: 12012
		internal const string RichTextBoxSelBullet = "RichTextBoxSelBullet";

		// Token: 0x04002EED RID: 12013
		internal const string RichTextBoxSelChange = "RichTextBoxSelChange";

		// Token: 0x04002EEE RID: 12014
		internal const string RichTextBoxSelCharOffset = "RichTextBoxSelCharOffset";

		// Token: 0x04002EEF RID: 12015
		internal const string RichTextBoxSelColor = "RichTextBoxSelColor";

		// Token: 0x04002EF0 RID: 12016
		internal const string RichTextBoxSelFont = "RichTextBoxSelFont";

		// Token: 0x04002EF1 RID: 12017
		internal const string RichTextBoxSelHangingIndent = "RichTextBoxSelHangingIndent";

		// Token: 0x04002EF2 RID: 12018
		internal const string RichTextBoxSelIndent = "RichTextBoxSelIndent";

		// Token: 0x04002EF3 RID: 12019
		internal const string RichTextBoxSelMargin = "RichTextBoxSelMargin";

		// Token: 0x04002EF4 RID: 12020
		internal const string RichTextBoxSelProtected = "RichTextBoxSelProtected";

		// Token: 0x04002EF5 RID: 12021
		internal const string RichTextBoxSelRightIndent = "RichTextBoxSelRightIndent";

		// Token: 0x04002EF6 RID: 12022
		internal const string RichTextBoxSelRTF = "RichTextBoxSelRTF";

		// Token: 0x04002EF7 RID: 12023
		internal const string RichTextBoxSelTabs = "RichTextBoxSelTabs";

		// Token: 0x04002EF8 RID: 12024
		internal const string RichTextBoxSelText = "RichTextBoxSelText";

		// Token: 0x04002EF9 RID: 12025
		internal const string RichTextBoxSelTypeDescr = "RichTextBoxSelTypeDescr";

		// Token: 0x04002EFA RID: 12026
		internal const string RichTextBoxUndoActionNameDescr = "RichTextBoxUndoActionNameDescr";

		// Token: 0x04002EFB RID: 12027
		internal const string RichTextBoxVScroll = "RichTextBoxVScroll";

		// Token: 0x04002EFC RID: 12028
		internal const string RichTextBoxZoomFactor = "RichTextBoxZoomFactor";

		// Token: 0x04002EFD RID: 12029
		internal const string RichTextFindEndInvalid = "RichTextFindEndInvalid";

		// Token: 0x04002EFE RID: 12030
		internal const string RTL = "RTL";

		// Token: 0x04002EFF RID: 12031
		internal const string SafeTopLevelCaptionFormat = "SafeTopLevelCaptionFormat";

		// Token: 0x04002F00 RID: 12032
		internal const string SaveFileDialogCreatePrompt = "SaveFileDialogCreatePrompt";

		// Token: 0x04002F01 RID: 12033
		internal const string SaveFileDialogOverWritePrompt = "SaveFileDialogOverWritePrompt";

		// Token: 0x04002F02 RID: 12034
		internal const string SaveTextError = "SaveTextError";

		// Token: 0x04002F03 RID: 12035
		internal const string ScrollableControlHorizontalScrollDescr = "ScrollableControlHorizontalScrollDescr";

		// Token: 0x04002F04 RID: 12036
		internal const string ScrollableControlRaiseMouseEnterLeaveEventsForScrollBarsDescr = "ScrollableControlRaiseMouseEnterLeaveEventsForScrollBarsDescr";

		// Token: 0x04002F05 RID: 12037
		internal const string ScrollableControlVerticalScrollDescr = "ScrollableControlVerticalScrollDescr";

		// Token: 0x04002F06 RID: 12038
		internal const string ScrollBarEnableDescr = "ScrollBarEnableDescr";

		// Token: 0x04002F07 RID: 12039
		internal const string ScrollBarLargeChangeDescr = "ScrollBarLargeChangeDescr";

		// Token: 0x04002F08 RID: 12040
		internal const string ScrollBarMaximumDescr = "ScrollBarMaximumDescr";

		// Token: 0x04002F09 RID: 12041
		internal const string ScrollBarMinimumDescr = "ScrollBarMinimumDescr";

		// Token: 0x04002F0A RID: 12042
		internal const string ScrollBarOnScrollDescr = "ScrollBarOnScrollDescr";

		// Token: 0x04002F0B RID: 12043
		internal const string ScrollBarSmallChangeDescr = "ScrollBarSmallChangeDescr";

		// Token: 0x04002F0C RID: 12044
		internal const string ScrollBarValueDescr = "ScrollBarValueDescr";

		// Token: 0x04002F0D RID: 12045
		internal const string ScrollBarVisibleDescr = "ScrollBarVisibleDescr";

		// Token: 0x04002F0E RID: 12046
		internal const string SecurityAboutDialog = "SecurityAboutDialog";

		// Token: 0x04002F0F RID: 12047
		internal const string SecurityApplication = "SecurityApplication";

		// Token: 0x04002F10 RID: 12048
		internal const string SecurityAsmNameColumn = "SecurityAsmNameColumn";

		// Token: 0x04002F11 RID: 12049
		internal const string SecurityAssembliesTab = "SecurityAssembliesTab";

		// Token: 0x04002F12 RID: 12050
		internal const string SecurityBugReportLabel = "SecurityBugReportLabel";

		// Token: 0x04002F13 RID: 12051
		internal const string SecurityBugReportTab = "SecurityBugReportTab";

		// Token: 0x04002F14 RID: 12052
		internal const string SecurityClose = "SecurityClose";

		// Token: 0x04002F15 RID: 12053
		internal const string SecurityCodeBaseColumn = "SecurityCodeBaseColumn";

		// Token: 0x04002F16 RID: 12054
		internal const string SecurityFileVersionColumn = "SecurityFileVersionColumn";

		// Token: 0x04002F17 RID: 12055
		internal const string SecurityIncludeAppInfo = "SecurityIncludeAppInfo";

		// Token: 0x04002F18 RID: 12056
		internal const string SecurityIncludeSysInfo = "SecurityIncludeSysInfo";

		// Token: 0x04002F19 RID: 12057
		internal const string SecurityInfoTab = "SecurityInfoTab";

		// Token: 0x04002F1A RID: 12058
		internal const string SecurityRestrictedText = "SecurityRestrictedText";

		// Token: 0x04002F1B RID: 12059
		internal const string SecurityRestrictedWindowTextMixedZone = "SecurityRestrictedWindowTextMixedZone";

		// Token: 0x04002F1C RID: 12060
		internal const string SecurityRestrictedWindowTextMultipleSites = "SecurityRestrictedWindowTextMultipleSites";

		// Token: 0x04002F1D RID: 12061
		internal const string SecurityRestrictedWindowTextUnknownSite = "SecurityRestrictedWindowTextUnknownSite";

		// Token: 0x04002F1E RID: 12062
		internal const string SecurityRestrictedWindowTextUnknownZone = "SecurityRestrictedWindowTextUnknownZone";

		// Token: 0x04002F1F RID: 12063
		internal const string SecuritySaveBug = "SecuritySaveBug";

		// Token: 0x04002F20 RID: 12064
		internal const string SecuritySaveFilter = "SecuritySaveFilter";

		// Token: 0x04002F21 RID: 12065
		internal const string SecuritySubmitBug = "SecuritySubmitBug";

		// Token: 0x04002F22 RID: 12066
		internal const string SecuritySubmitBugUrl = "SecuritySubmitBugUrl";

		// Token: 0x04002F23 RID: 12067
		internal const string SecuritySwitchDescrColumn = "SecuritySwitchDescrColumn";

		// Token: 0x04002F24 RID: 12068
		internal const string SecuritySwitchesTab = "SecuritySwitchesTab";

		// Token: 0x04002F25 RID: 12069
		internal const string SecuritySwitchLabel = "SecuritySwitchLabel";

		// Token: 0x04002F26 RID: 12070
		internal const string SecuritySwitchNameColumn = "SecuritySwitchNameColumn";

		// Token: 0x04002F27 RID: 12071
		internal const string SecurityToolTipCaption = "SecurityToolTipCaption";

		// Token: 0x04002F28 RID: 12072
		internal const string SecurityToolTipMainText = "SecurityToolTipMainText";

		// Token: 0x04002F29 RID: 12073
		internal const string SecurityToolTipSourceInformation = "SecurityToolTipSourceInformation";

		// Token: 0x04002F2A RID: 12074
		internal const string SecurityToolTipTextFormat = "SecurityToolTipTextFormat";

		// Token: 0x04002F2B RID: 12075
		internal const string SecurityUnrestrictedText = "SecurityUnrestrictedText";

		// Token: 0x04002F2C RID: 12076
		internal const string SecurityVersionColumn = "SecurityVersionColumn";

		// Token: 0x04002F2D RID: 12077
		internal const string selectedIndexChangedEventDescr = "selectedIndexChangedEventDescr";

		// Token: 0x04002F2E RID: 12078
		internal const string selectedIndexDescr = "selectedIndexDescr";

		// Token: 0x04002F2F RID: 12079
		internal const string SelectedNotEqualActual = "SelectedNotEqualActual";

		// Token: 0x04002F30 RID: 12080
		internal const string selectionChangeCommittedEventDescr = "selectionChangeCommittedEventDescr";

		// Token: 0x04002F31 RID: 12081
		internal const string SelTabCountRange = "SelTabCountRange";

		// Token: 0x04002F32 RID: 12082
		internal const string SendKeysGroupDelimError = "SendKeysGroupDelimError";

		// Token: 0x04002F33 RID: 12083
		internal const string SendKeysHookFailed = "SendKeysHookFailed";

		// Token: 0x04002F34 RID: 12084
		internal const string SendKeysKeywordDelimError = "SendKeysKeywordDelimError";

		// Token: 0x04002F35 RID: 12085
		internal const string SendKeysNestingError = "SendKeysNestingError";

		// Token: 0x04002F36 RID: 12086
		internal const string SendKeysNoMessageLoop = "SendKeysNoMessageLoop";

		// Token: 0x04002F37 RID: 12087
		internal const string SerializationException = "SerializationException";

		// Token: 0x04002F38 RID: 12088
		internal const string ShowDialogOnDisabled = "ShowDialogOnDisabled";

		// Token: 0x04002F39 RID: 12089
		internal const string ShowDialogOnModal = "ShowDialogOnModal";

		// Token: 0x04002F3A RID: 12090
		internal const string ShowDialogOnNonTopLevel = "ShowDialogOnNonTopLevel";

		// Token: 0x04002F3B RID: 12091
		internal const string ShowDialogOnVisible = "ShowDialogOnVisible";

		// Token: 0x04002F3C RID: 12092
		internal const string SortRequiresIBindingList = "SortRequiresIBindingList";

		// Token: 0x04002F3D RID: 12093
		internal const string SplitContainerFixedPanelDescr = "SplitContainerFixedPanelDescr";

		// Token: 0x04002F3E RID: 12094
		internal const string SplitContainerIsSplitterFixedDescr = "SplitContainerIsSplitterFixedDescr";

		// Token: 0x04002F3F RID: 12095
		internal const string SplitContainerOrientationDescr = "SplitContainerOrientationDescr";

		// Token: 0x04002F40 RID: 12096
		internal const string SplitContainerPanel1CollapsedDescr = "SplitContainerPanel1CollapsedDescr";

		// Token: 0x04002F41 RID: 12097
		internal const string SplitContainerPanel1Descr = "SplitContainerPanel1Descr";

		// Token: 0x04002F42 RID: 12098
		internal const string SplitContainerPanel1MinSizeDescr = "SplitContainerPanel1MinSizeDescr";

		// Token: 0x04002F43 RID: 12099
		internal const string SplitContainerPanel2CollapsedDescr = "SplitContainerPanel2CollapsedDescr";

		// Token: 0x04002F44 RID: 12100
		internal const string SplitContainerPanel2Descr = "SplitContainerPanel2Descr";

		// Token: 0x04002F45 RID: 12101
		internal const string SplitContainerPanel2MinSizeDescr = "SplitContainerPanel2MinSizeDescr";

		// Token: 0x04002F46 RID: 12102
		internal const string SplitContainerPanelHeight = "SplitContainerPanelHeight";

		// Token: 0x04002F47 RID: 12103
		internal const string SplitContainerPanelWidth = "SplitContainerPanelWidth";

		// Token: 0x04002F48 RID: 12104
		internal const string SplitContainerSplitterDistanceDescr = "SplitContainerSplitterDistanceDescr";

		// Token: 0x04002F49 RID: 12105
		internal const string SplitContainerSplitterIncrementDescr = "SplitContainerSplitterIncrementDescr";

		// Token: 0x04002F4A RID: 12106
		internal const string SplitContainerSplitterRectangleDescr = "SplitContainerSplitterRectangleDescr";

		// Token: 0x04002F4B RID: 12107
		internal const string SplitContainerSplitterWidthDescr = "SplitContainerSplitterWidthDescr";

		// Token: 0x04002F4C RID: 12108
		internal const string SplitterBorderStyleDescr = "SplitterBorderStyleDescr";

		// Token: 0x04002F4D RID: 12109
		internal const string SplitterDistanceNotAllowed = "SplitterDistanceNotAllowed";

		// Token: 0x04002F4E RID: 12110
		internal const string SplitterInvalidDockEnum = "SplitterInvalidDockEnum";

		// Token: 0x04002F4F RID: 12111
		internal const string SplitterMinExtraDescr = "SplitterMinExtraDescr";

		// Token: 0x04002F50 RID: 12112
		internal const string SplitterMinSizeDescr = "SplitterMinSizeDescr";

		// Token: 0x04002F51 RID: 12113
		internal const string SplitterSplitPositionDescr = "SplitterSplitPositionDescr";

		// Token: 0x04002F52 RID: 12114
		internal const string SplitterSplitterMovedDescr = "SplitterSplitterMovedDescr";

		// Token: 0x04002F53 RID: 12115
		internal const string SplitterSplitterMovingDescr = "SplitterSplitterMovingDescr";

		// Token: 0x04002F54 RID: 12116
		internal const string StatusBarAddFailed = "StatusBarAddFailed";

		// Token: 0x04002F55 RID: 12117
		internal const string StatusBarBadStatusBarPanel = "StatusBarBadStatusBarPanel";

		// Token: 0x04002F56 RID: 12118
		internal const string StatusBarDrawItem = "StatusBarDrawItem";

		// Token: 0x04002F57 RID: 12119
		internal const string StatusBarOnPanelClickDescr = "StatusBarOnPanelClickDescr";

		// Token: 0x04002F58 RID: 12120
		internal const string StatusBarPanelAlignmentDescr = "StatusBarPanelAlignmentDescr";

		// Token: 0x04002F59 RID: 12121
		internal const string StatusBarPanelAutoSizeDescr = "StatusBarPanelAutoSizeDescr";

		// Token: 0x04002F5A RID: 12122
		internal const string StatusBarPanelBorderStyleDescr = "StatusBarPanelBorderStyleDescr";

		// Token: 0x04002F5B RID: 12123
		internal const string StatusBarPanelIconDescr = "StatusBarPanelIconDescr";

		// Token: 0x04002F5C RID: 12124
		internal const string StatusBarPanelMinWidthDescr = "StatusBarPanelMinWidthDescr";

		// Token: 0x04002F5D RID: 12125
		internal const string StatusBarPanelNameDescr = "StatusBarPanelNameDescr";

		// Token: 0x04002F5E RID: 12126
		internal const string StatusBarPanelsDescr = "StatusBarPanelsDescr";

		// Token: 0x04002F5F RID: 12127
		internal const string StatusBarPanelStyleDescr = "StatusBarPanelStyleDescr";

		// Token: 0x04002F60 RID: 12128
		internal const string StatusBarPanelTextDescr = "StatusBarPanelTextDescr";

		// Token: 0x04002F61 RID: 12129
		internal const string StatusBarPanelToolTipTextDescr = "StatusBarPanelToolTipTextDescr";

		// Token: 0x04002F62 RID: 12130
		internal const string StatusBarPanelWidthDescr = "StatusBarPanelWidthDescr";

		// Token: 0x04002F63 RID: 12131
		internal const string StatusBarShowPanelsDescr = "StatusBarShowPanelsDescr";

		// Token: 0x04002F64 RID: 12132
		internal const string StatusBarSizingGripDescr = "StatusBarSizingGripDescr";

		// Token: 0x04002F65 RID: 12133
		internal const string StatusStripPanelBorderSidesDescr = "StatusStripPanelBorderSidesDescr";

		// Token: 0x04002F66 RID: 12134
		internal const string StatusStripPanelBorderStyleDescr = "StatusStripPanelBorderStyleDescr";

		// Token: 0x04002F67 RID: 12135
		internal const string StatusStripSizingGripDescr = "StatusStripSizingGripDescr";

		// Token: 0x04002F68 RID: 12136
		internal const string SystemInformationFeatureNotSupported = "SystemInformationFeatureNotSupported";

		// Token: 0x04002F69 RID: 12137
		internal const string TabBaseAlignmentDescr = "TabBaseAlignmentDescr";

		// Token: 0x04002F6A RID: 12138
		internal const string TabBaseAppearanceDescr = "TabBaseAppearanceDescr";

		// Token: 0x04002F6B RID: 12139
		internal const string TabBaseDrawModeDescr = "TabBaseDrawModeDescr";

		// Token: 0x04002F6C RID: 12140
		internal const string TabBaseHotTrackDescr = "TabBaseHotTrackDescr";

		// Token: 0x04002F6D RID: 12141
		internal const string TabBaseImageListDescr = "TabBaseImageListDescr";

		// Token: 0x04002F6E RID: 12142
		internal const string TabBaseItemSizeDescr = "TabBaseItemSizeDescr";

		// Token: 0x04002F6F RID: 12143
		internal const string TabBaseMultilineDescr = "TabBaseMultilineDescr";

		// Token: 0x04002F70 RID: 12144
		internal const string TabBasePaddingDescr = "TabBasePaddingDescr";

		// Token: 0x04002F71 RID: 12145
		internal const string TabBaseRowCountDescr = "TabBaseRowCountDescr";

		// Token: 0x04002F72 RID: 12146
		internal const string TabBaseShowToolTipsDescr = "TabBaseShowToolTipsDescr";

		// Token: 0x04002F73 RID: 12147
		internal const string TabBaseSizeModeDescr = "TabBaseSizeModeDescr";

		// Token: 0x04002F74 RID: 12148
		internal const string TabBaseTabCountDescr = "TabBaseTabCountDescr";

		// Token: 0x04002F75 RID: 12149
		internal const string TabControlDeselectedEventDescr = "TabControlDeselectedEventDescr";

		// Token: 0x04002F76 RID: 12150
		internal const string TabControlDeselectingEventDescr = "TabControlDeselectingEventDescr";

		// Token: 0x04002F77 RID: 12151
		internal const string TabControlInvalidTabPageType = "TabControlInvalidTabPageType";

		// Token: 0x04002F78 RID: 12152
		internal const string TabControlSelectedEventDescr = "TabControlSelectedEventDescr";

		// Token: 0x04002F79 RID: 12153
		internal const string TabControlSelectedTabDescr = "TabControlSelectedTabDescr";

		// Token: 0x04002F7A RID: 12154
		internal const string TabControlSelectingEventDescr = "TabControlSelectingEventDescr";

		// Token: 0x04002F7B RID: 12155
		internal const string TABCONTROLTabPageNotOnTabControl = "TABCONTROLTabPageNotOnTabControl";

		// Token: 0x04002F7C RID: 12156
		internal const string TABCONTROLTabPageOnTabPage = "TABCONTROLTabPageOnTabPage";

		// Token: 0x04002F7D RID: 12157
		internal const string TabControlTabsDescr = "TabControlTabsDescr";

		// Token: 0x04002F7E RID: 12158
		internal const string TabItemImageIndexDescr = "TabItemImageIndexDescr";

		// Token: 0x04002F7F RID: 12159
		internal const string TabItemToolTipTextDescr = "TabItemToolTipTextDescr";

		// Token: 0x04002F80 RID: 12160
		internal const string TabItemUseVisualStyleBackColorDescr = "TabItemUseVisualStyleBackColorDescr";

		// Token: 0x04002F81 RID: 12161
		internal const string TableBeginMustBeCalledPrior = "TableBeginMustBeCalledPrior";

		// Token: 0x04002F82 RID: 12162
		internal const string TableBeginNotCalled = "TableBeginNotCalled";

		// Token: 0x04002F83 RID: 12163
		internal const string TableLayoutPanelCellBorderStyleDescr = "TableLayoutPanelCellBorderStyleDescr";

		// Token: 0x04002F84 RID: 12164
		internal const string TableLayoutPanelFullDesc = "TableLayoutPanelFullDesc";

		// Token: 0x04002F85 RID: 12165
		internal const string TableLayoutPanelGrowStyleDescr = "TableLayoutPanelGrowStyleDescr";

		// Token: 0x04002F86 RID: 12166
		internal const string TableLayoutPanelOnPaintCellDescr = "TableLayoutPanelOnPaintCellDescr";

		// Token: 0x04002F87 RID: 12167
		internal const string TableLayoutPanelSpanDesc = "TableLayoutPanelSpanDesc";

		// Token: 0x04002F88 RID: 12168
		internal const string TableLayoutSettingSettingsIsNotSupported = "TableLayoutSettingSettingsIsNotSupported";

		// Token: 0x04002F89 RID: 12169
		internal const string TableLayoutSettingsGetCellPositionDescr = "TableLayoutSettingsGetCellPositionDescr";

		// Token: 0x04002F8A RID: 12170
		internal const string TableLayoutSettingsSetCellPositionDescr = "TableLayoutSettingsSetCellPositionDescr";

		// Token: 0x04002F8B RID: 12171
		internal const string TablePrintLayoutFromDifferentDocument = "TablePrintLayoutFromDifferentDocument";

		// Token: 0x04002F8C RID: 12172
		internal const string TextBoxAcceptsReturnDescr = "TextBoxAcceptsReturnDescr";

		// Token: 0x04002F8D RID: 12173
		internal const string TextBoxAcceptsTabDescr = "TextBoxAcceptsTabDescr";

		// Token: 0x04002F8E RID: 12174
		internal const string TextBoxAutoCompleteCustomSourceDescr = "TextBoxAutoCompleteCustomSourceDescr";

		// Token: 0x04002F8F RID: 12175
		internal const string TextBoxAutoCompleteModeDescr = "TextBoxAutoCompleteModeDescr";

		// Token: 0x04002F90 RID: 12176
		internal const string TextBoxAutoCompleteSourceDescr = "TextBoxAutoCompleteSourceDescr";

		// Token: 0x04002F91 RID: 12177
		internal const string TextBoxAutoCompleteSourceNoItems = "TextBoxAutoCompleteSourceNoItems";

		// Token: 0x04002F92 RID: 12178
		internal const string TextBoxAutoSizeDescr = "TextBoxAutoSizeDescr";

		// Token: 0x04002F93 RID: 12179
		internal const string TextBoxBaseOnAcceptsTabChangedDescr = "TextBoxBaseOnAcceptsTabChangedDescr";

		// Token: 0x04002F94 RID: 12180
		internal const string TextBoxBaseOnAutoSizeChangedDescr = "TextBoxBaseOnAutoSizeChangedDescr";

		// Token: 0x04002F95 RID: 12181
		internal const string TextBoxBaseOnBorderStyleChangedDescr = "TextBoxBaseOnBorderStyleChangedDescr";

		// Token: 0x04002F96 RID: 12182
		internal const string TextBoxBaseOnHideSelectionChangedDescr = "TextBoxBaseOnHideSelectionChangedDescr";

		// Token: 0x04002F97 RID: 12183
		internal const string TextBoxBaseOnModifiedChangedDescr = "TextBoxBaseOnModifiedChangedDescr";

		// Token: 0x04002F98 RID: 12184
		internal const string TextBoxBaseOnMultilineChangedDescr = "TextBoxBaseOnMultilineChangedDescr";

		// Token: 0x04002F99 RID: 12185
		internal const string TextBoxBaseOnReadOnlyChangedDescr = "TextBoxBaseOnReadOnlyChangedDescr";

		// Token: 0x04002F9A RID: 12186
		internal const string TextBoxBorderDescr = "TextBoxBorderDescr";

		// Token: 0x04002F9B RID: 12187
		internal const string TextBoxCanUndoDescr = "TextBoxCanUndoDescr";

		// Token: 0x04002F9C RID: 12188
		internal const string TextBoxCharacterCasingDescr = "TextBoxCharacterCasingDescr";

		// Token: 0x04002F9D RID: 12189
		internal const string TextBoxHideSelectionDescr = "TextBoxHideSelectionDescr";

		// Token: 0x04002F9E RID: 12190
		internal const string TextBoxLinesDescr = "TextBoxLinesDescr";

		// Token: 0x04002F9F RID: 12191
		internal const string TextBoxMaxLengthDescr = "TextBoxMaxLengthDescr";

		// Token: 0x04002FA0 RID: 12192
		internal const string TextBoxModifiedDescr = "TextBoxModifiedDescr";

		// Token: 0x04002FA1 RID: 12193
		internal const string TextBoxMultilineDescr = "TextBoxMultilineDescr";

		// Token: 0x04002FA2 RID: 12194
		internal const string TextBoxPasswordCharDescr = "TextBoxPasswordCharDescr";

		// Token: 0x04002FA3 RID: 12195
		internal const string TextBoxPreferredHeightDescr = "TextBoxPreferredHeightDescr";

		// Token: 0x04002FA4 RID: 12196
		internal const string TextBoxReadOnlyDescr = "TextBoxReadOnlyDescr";

		// Token: 0x04002FA5 RID: 12197
		internal const string TextBoxScrollBarsDescr = "TextBoxScrollBarsDescr";

		// Token: 0x04002FA6 RID: 12198
		internal const string TextBoxSelectedTextDescr = "TextBoxSelectedTextDescr";

		// Token: 0x04002FA7 RID: 12199
		internal const string TextBoxSelectionLengthDescr = "TextBoxSelectionLengthDescr";

		// Token: 0x04002FA8 RID: 12200
		internal const string TextBoxSelectionStartDescr = "TextBoxSelectionStartDescr";

		// Token: 0x04002FA9 RID: 12201
		internal const string TextBoxShortcutsEnabledDescr = "TextBoxShortcutsEnabledDescr";

		// Token: 0x04002FAA RID: 12202
		internal const string TextBoxTextAlignDescr = "TextBoxTextAlignDescr";

		// Token: 0x04002FAB RID: 12203
		internal const string TextBoxUseSystemPasswordCharDescr = "TextBoxUseSystemPasswordCharDescr";

		// Token: 0x04002FAC RID: 12204
		internal const string TextBoxWordWrapDescr = "TextBoxWordWrapDescr";

		// Token: 0x04002FAD RID: 12205
		internal const string TextParseFailedFormat = "TextParseFailedFormat";

		// Token: 0x04002FAE RID: 12206
		internal const string ThreadMustBeSTA = "ThreadMustBeSTA";

		// Token: 0x04002FAF RID: 12207
		internal const string ThreadNoLongerValid = "ThreadNoLongerValid";

		// Token: 0x04002FB0 RID: 12208
		internal const string ThreadNotPumpingMessages = "ThreadNotPumpingMessages";

		// Token: 0x04002FB1 RID: 12209
		internal const string TimerEnabledDescr = "TimerEnabledDescr";

		// Token: 0x04002FB2 RID: 12210
		internal const string TimerIntervalDescr = "TimerIntervalDescr";

		// Token: 0x04002FB3 RID: 12211
		internal const string TimerInvalidInterval = "TimerInvalidInterval";

		// Token: 0x04002FB4 RID: 12212
		internal const string TimerTimerDescr = "TimerTimerDescr";

		// Token: 0x04002FB5 RID: 12213
		internal const string ToolBarAppearanceDescr = "ToolBarAppearanceDescr";

		// Token: 0x04002FB6 RID: 12214
		internal const string ToolBarAutoSizeDescr = "ToolBarAutoSizeDescr";

		// Token: 0x04002FB7 RID: 12215
		internal const string ToolBarBadToolBarButton = "ToolBarBadToolBarButton";

		// Token: 0x04002FB8 RID: 12216
		internal const string ToolBarBorderStyleDescr = "ToolBarBorderStyleDescr";

		// Token: 0x04002FB9 RID: 12217
		internal const string ToolBarButtonClickDescr = "ToolBarButtonClickDescr";

		// Token: 0x04002FBA RID: 12218
		internal const string ToolBarButtonDropDownDescr = "ToolBarButtonDropDownDescr";

		// Token: 0x04002FBB RID: 12219
		internal const string ToolBarButtonEnabledDescr = "ToolBarButtonEnabledDescr";

		// Token: 0x04002FBC RID: 12220
		internal const string ToolBarButtonImageIndexDescr = "ToolBarButtonImageIndexDescr";

		// Token: 0x04002FBD RID: 12221
		internal const string ToolBarButtonInvalidDropDownMenuType = "ToolBarButtonInvalidDropDownMenuType";

		// Token: 0x04002FBE RID: 12222
		internal const string ToolBarButtonMenuDescr = "ToolBarButtonMenuDescr";

		// Token: 0x04002FBF RID: 12223
		internal const string ToolBarButtonNotFound = "ToolBarButtonNotFound";

		// Token: 0x04002FC0 RID: 12224
		internal const string ToolBarButtonPartialPushDescr = "ToolBarButtonPartialPushDescr";

		// Token: 0x04002FC1 RID: 12225
		internal const string ToolBarButtonPushedDescr = "ToolBarButtonPushedDescr";

		// Token: 0x04002FC2 RID: 12226
		internal const string ToolBarButtonsDescr = "ToolBarButtonsDescr";

		// Token: 0x04002FC3 RID: 12227
		internal const string ToolBarButtonSizeDescr = "ToolBarButtonSizeDescr";

		// Token: 0x04002FC4 RID: 12228
		internal const string ToolBarButtonStyleDescr = "ToolBarButtonStyleDescr";

		// Token: 0x04002FC5 RID: 12229
		internal const string ToolBarButtonTextDescr = "ToolBarButtonTextDescr";

		// Token: 0x04002FC6 RID: 12230
		internal const string ToolBarButtonToolTipTextDescr = "ToolBarButtonToolTipTextDescr";

		// Token: 0x04002FC7 RID: 12231
		internal const string ToolBarButtonVisibleDescr = "ToolBarButtonVisibleDescr";

		// Token: 0x04002FC8 RID: 12232
		internal const string ToolBarDividerDescr = "ToolBarDividerDescr";

		// Token: 0x04002FC9 RID: 12233
		internal const string ToolBarDropDownArrowsDescr = "ToolBarDropDownArrowsDescr";

		// Token: 0x04002FCA RID: 12234
		internal const string ToolBarImageListDescr = "ToolBarImageListDescr";

		// Token: 0x04002FCB RID: 12235
		internal const string ToolBarImageSizeDescr = "ToolBarImageSizeDescr";

		// Token: 0x04002FCC RID: 12236
		internal const string ToolBarShowToolTipsDescr = "ToolBarShowToolTipsDescr";

		// Token: 0x04002FCD RID: 12237
		internal const string ToolBarTextAlignDescr = "ToolBarTextAlignDescr";

		// Token: 0x04002FCE RID: 12238
		internal const string ToolBarWrappableDescr = "ToolBarWrappableDescr";

		// Token: 0x04002FCF RID: 12239
		internal const string ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue = "ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue";

		// Token: 0x04002FD0 RID: 12240
		internal const string ToolStripAllowItemReorderDescr = "ToolStripAllowItemReorderDescr";

		// Token: 0x04002FD1 RID: 12241
		internal const string ToolStripAllowMergeDescr = "ToolStripAllowMergeDescr";

		// Token: 0x04002FD2 RID: 12242
		internal const string ToolStripBackColorDescr = "ToolStripBackColorDescr";

		// Token: 0x04002FD3 RID: 12243
		internal const string ToolStripButtonCheckedDescr = "ToolStripButtonCheckedDescr";

		// Token: 0x04002FD4 RID: 12244
		internal const string ToolStripButtonCheckOnClickDescr = "ToolStripButtonCheckOnClickDescr";

		// Token: 0x04002FD5 RID: 12245
		internal const string ToolStripCanOnlyPositionItsOwnItems = "ToolStripCanOnlyPositionItsOwnItems";

		// Token: 0x04002FD6 RID: 12246
		internal const string ToolStripCanOverflowDescr = "ToolStripCanOverflowDescr";

		// Token: 0x04002FD7 RID: 12247
		internal const string ToolStripCollectionMustInsertAndRemove = "ToolStripCollectionMustInsertAndRemove";

		// Token: 0x04002FD8 RID: 12248
		internal const string ToolStripContainerBottomToolStripPanelDescr = "ToolStripContainerBottomToolStripPanelDescr";

		// Token: 0x04002FD9 RID: 12249
		internal const string ToolStripContainerBottomToolStripPanelVisibleDescr = "ToolStripContainerBottomToolStripPanelVisibleDescr";

		// Token: 0x04002FDA RID: 12250
		internal const string ToolStripContainerContentPanelDescr = "ToolStripContainerContentPanelDescr";

		// Token: 0x04002FDB RID: 12251
		internal const string ToolStripContainerDesc = "ToolStripContainerDesc";

		// Token: 0x04002FDC RID: 12252
		internal const string ToolStripContainerLeftToolStripPanelDescr = "ToolStripContainerLeftToolStripPanelDescr";

		// Token: 0x04002FDD RID: 12253
		internal const string ToolStripContainerLeftToolStripPanelVisibleDescr = "ToolStripContainerLeftToolStripPanelVisibleDescr";

		// Token: 0x04002FDE RID: 12254
		internal const string ToolStripContainerRightToolStripPanelDescr = "ToolStripContainerRightToolStripPanelDescr";

		// Token: 0x04002FDF RID: 12255
		internal const string ToolStripContainerRightToolStripPanelVisibleDescr = "ToolStripContainerRightToolStripPanelVisibleDescr";

		// Token: 0x04002FE0 RID: 12256
		internal const string ToolStripContainerTopToolStripPanelDescr = "ToolStripContainerTopToolStripPanelDescr";

		// Token: 0x04002FE1 RID: 12257
		internal const string ToolStripContainerTopToolStripPanelVisibleDescr = "ToolStripContainerTopToolStripPanelVisibleDescr";

		// Token: 0x04002FE2 RID: 12258
		internal const string ToolStripContainerUseContentPanel = "ToolStripContainerUseContentPanel";

		// Token: 0x04002FE3 RID: 12259
		internal const string ToolStripContentPanelOnLoadDescr = "ToolStripContentPanelOnLoadDescr";

		// Token: 0x04002FE4 RID: 12260
		internal const string ToolStripDefaultDropDownDirectionDescr = "ToolStripDefaultDropDownDirectionDescr";

		// Token: 0x04002FE5 RID: 12261
		internal const string ToolStripDoesntSupportAutoScroll = "ToolStripDoesntSupportAutoScroll";

		// Token: 0x04002FE6 RID: 12262
		internal const string ToolStripDropDownAutoCloseDescr = "ToolStripDropDownAutoCloseDescr";

		// Token: 0x04002FE7 RID: 12263
		internal const string ToolStripDropDownButtonShowDropDownArrowDescr = "ToolStripDropDownButtonShowDropDownArrowDescr";

		// Token: 0x04002FE8 RID: 12264
		internal const string ToolStripDropDownClosedDecr = "ToolStripDropDownClosedDecr";

		// Token: 0x04002FE9 RID: 12265
		internal const string ToolStripDropDownClosingDecr = "ToolStripDropDownClosingDecr";

		// Token: 0x04002FEA RID: 12266
		internal const string ToolStripDropDownDescr = "ToolStripDropDownDescr";

		// Token: 0x04002FEB RID: 12267
		internal const string ToolStripDropDownItemDropDownDirectionDescr = "ToolStripDropDownItemDropDownDirectionDescr";

		// Token: 0x04002FEC RID: 12268
		internal const string ToolStripDropDownItemsDescr = "ToolStripDropDownItemsDescr";

		// Token: 0x04002FED RID: 12269
		internal const string ToolStripDropDownMenuShowCheckMarginDescr = "ToolStripDropDownMenuShowCheckMarginDescr";

		// Token: 0x04002FEE RID: 12270
		internal const string ToolStripDropDownMenuShowImageMarginDescr = "ToolStripDropDownMenuShowImageMarginDescr";

		// Token: 0x04002FEF RID: 12271
		internal const string ToolStripDropDownOpenedDescr = "ToolStripDropDownOpenedDescr";

		// Token: 0x04002FF0 RID: 12272
		internal const string ToolStripDropDownOpeningDescr = "ToolStripDropDownOpeningDescr";

		// Token: 0x04002FF1 RID: 12273
		internal const string ToolStripDropDownPreferredWidthDescr = "ToolStripDropDownPreferredWidthDescr";

		// Token: 0x04002FF2 RID: 12274
		internal const string ToolStripGripAccessibleName = "ToolStripGripAccessibleName";

		// Token: 0x04002FF3 RID: 12275
		internal const string ToolStripDropDownsCantBeRafted = "ToolStripDropDownsCantBeRafted";

		// Token: 0x04002FF4 RID: 12276
		internal const string ToolStripGripDisplayStyleDescr = "ToolStripGripDisplayStyleDescr";

		// Token: 0x04002FF5 RID: 12277
		internal const string ToolStripGripMargin = "ToolStripGripMargin";

		// Token: 0x04002FF6 RID: 12278
		internal const string ToolStripGripStyleDescr = "ToolStripGripStyleDescr";

		// Token: 0x04002FF7 RID: 12279
		internal const string ToolStripImageListDescr = "ToolStripImageListDescr";

		// Token: 0x04002FF8 RID: 12280
		internal const string ToolStripImageScalingSizeDescr = "ToolStripImageScalingSizeDescr";

		// Token: 0x04002FF9 RID: 12281
		internal const string ToolStripItemAccessibilityObjectDescr = "ToolStripItemAccessibilityObjectDescr";

		// Token: 0x04002FFA RID: 12282
		internal const string ToolStripItemAccessibleDefaultActionDescr = "ToolStripItemAccessibleDefaultActionDescr";

		// Token: 0x04002FFB RID: 12283
		internal const string ToolStripItemAccessibleDescriptionDescr = "ToolStripItemAccessibleDescriptionDescr";

		// Token: 0x04002FFC RID: 12284
		internal const string ToolStripItemAccessibleNameDescr = "ToolStripItemAccessibleNameDescr";

		// Token: 0x04002FFD RID: 12285
		internal const string ToolStripItemAccessibleRoleDescr = "ToolStripItemAccessibleRoleDescr";

		// Token: 0x04002FFE RID: 12286
		internal const string ToolStripItemAddedDescr = "ToolStripItemAddedDescr";

		// Token: 0x04002FFF RID: 12287
		internal const string ToolStripItemAlignment = "ToolStripItemAlignment";

		// Token: 0x04003000 RID: 12288
		internal const string ToolStripItemAlignmentDescr = "ToolStripItemAlignmentDescr";

		// Token: 0x04003001 RID: 12289
		internal const string ToolStripItemAllowDropDescr = "ToolStripItemAllowDropDescr";

		// Token: 0x04003002 RID: 12290
		internal const string ToolStripItemAutoSizeDescr = "ToolStripItemAutoSizeDescr";

		// Token: 0x04003003 RID: 12291
		internal const string ToolStripItemAutoToolTipDescr = "ToolStripItemAutoToolTipDescr";

		// Token: 0x04003004 RID: 12292
		internal const string ToolStripItemAvailableDescr = "ToolStripItemAvailableDescr";

		// Token: 0x04003005 RID: 12293
		internal const string ToolStripItemBackColorDescr = "ToolStripItemBackColorDescr";

		// Token: 0x04003006 RID: 12294
		internal const string ToolStripItemCircularReference = "ToolStripItemCircularReference";

		// Token: 0x04003007 RID: 12295
		internal const string ToolStripItemCollectionIsReadOnly = "ToolStripItemCollectionIsReadOnly";

		// Token: 0x04003008 RID: 12296
		internal const string ToolStripItemDisplayStyleDescr = "ToolStripItemDisplayStyleDescr";

		// Token: 0x04003009 RID: 12297
		internal const string ToolStripItemDoubleClickedEnabledDescr = "ToolStripItemDoubleClickedEnabledDescr";

		// Token: 0x0400300A RID: 12298
		internal const string ToolStripItemDrawModeDescr = "ToolStripItemDrawModeDescr";

		// Token: 0x0400300B RID: 12299
		internal const string ToolStripItemEnabledChangedDescr = "ToolStripItemEnabledChangedDescr";

		// Token: 0x0400300C RID: 12300
		internal const string ToolStripItemEnabledDescr = "ToolStripItemEnabledDescr";

		// Token: 0x0400300D RID: 12301
		internal const string ToolStripItemFontDescr = "ToolStripItemFontDescr";

		// Token: 0x0400300E RID: 12302
		internal const string ToolStripItemForeColorDescr = "ToolStripItemForeColorDescr";

		// Token: 0x0400300F RID: 12303
		internal const string ToolStripItemImageAlignDescr = "ToolStripItemImageAlignDescr";

		// Token: 0x04003010 RID: 12304
		internal const string ToolStripItemImageDescr = "ToolStripItemImageDescr";

		// Token: 0x04003011 RID: 12305
		internal const string ToolStripItemImageIndexDescr = "ToolStripItemImageIndexDescr";

		// Token: 0x04003012 RID: 12306
		internal const string ToolStripItemImageKeyDescr = "ToolStripItemImageKeyDescr";

		// Token: 0x04003013 RID: 12307
		internal const string ToolStripItemImageList = "ToolStripItemImageList";

		// Token: 0x04003014 RID: 12308
		internal const string ToolStripItemImageScalingDescr = "ToolStripItemImageScalingDescr";

		// Token: 0x04003015 RID: 12309
		internal const string ToolStripItemImageTransparentColorDescr = "ToolStripItemImageTransparentColorDescr";

		// Token: 0x04003016 RID: 12310
		internal const string ToolStripItemMarginDescr = "ToolStripItemMarginDescr";

		// Token: 0x04003017 RID: 12311
		internal const string ToolStripItemOnAvailableChangedDescr = "ToolStripItemOnAvailableChangedDescr";

		// Token: 0x04003018 RID: 12312
		internal const string ToolStripItemOnBackColorChangedDescr = "ToolStripItemOnBackColorChangedDescr";

		// Token: 0x04003019 RID: 12313
		internal const string ToolStripItemOnClickDescr = "ToolStripItemOnClickDescr";

		// Token: 0x0400301A RID: 12314
		internal const string ToolStripItemOnDragDropDescr = "ToolStripItemOnDragDropDescr";

		// Token: 0x0400301B RID: 12315
		internal const string ToolStripItemOnDragEnterDescr = "ToolStripItemOnDragEnterDescr";

		// Token: 0x0400301C RID: 12316
		internal const string ToolStripItemOnDragLeaveDescr = "ToolStripItemOnDragLeaveDescr";

		// Token: 0x0400301D RID: 12317
		internal const string ToolStripItemOnDragOverDescr = "ToolStripItemOnDragOverDescr";

		// Token: 0x0400301E RID: 12318
		internal const string ToolStripItemOnForeColorChangedDescr = "ToolStripItemOnForeColorChangedDescr";

		// Token: 0x0400301F RID: 12319
		internal const string ToolStripItemOnGiveFeedbackDescr = "ToolStripItemOnGiveFeedbackDescr";

		// Token: 0x04003020 RID: 12320
		internal const string ToolStripItemOnGotFocusDescr = "ToolStripItemOnGotFocusDescr";

		// Token: 0x04003021 RID: 12321
		internal const string ToolStripItemOnLocationChangedDescr = "ToolStripItemOnLocationChangedDescr";

		// Token: 0x04003022 RID: 12322
		internal const string ToolStripItemOnLostFocusDescr = "ToolStripItemOnLostFocusDescr";

		// Token: 0x04003023 RID: 12323
		internal const string ToolStripItemOnMouseDownDescr = "ToolStripItemOnMouseDownDescr";

		// Token: 0x04003024 RID: 12324
		internal const string ToolStripItemOnMouseEnterDescr = "ToolStripItemOnMouseEnterDescr";

		// Token: 0x04003025 RID: 12325
		internal const string ToolStripItemOnMouseHoverDescr = "ToolStripItemOnMouseHoverDescr";

		// Token: 0x04003026 RID: 12326
		internal const string ToolStripItemOnMouseLeaveDescr = "ToolStripItemOnMouseLeaveDescr";

		// Token: 0x04003027 RID: 12327
		internal const string ToolStripItemOnMouseMoveDescr = "ToolStripItemOnMouseMoveDescr";

		// Token: 0x04003028 RID: 12328
		internal const string ToolStripItemOnMouseUpDescr = "ToolStripItemOnMouseUpDescr";

		// Token: 0x04003029 RID: 12329
		internal const string ToolStripItemOnPaintDescr = "ToolStripItemOnPaintDescr";

		// Token: 0x0400302A RID: 12330
		internal const string ToolStripItemOnQueryAccessibilityHelpDescr = "ToolStripItemOnQueryAccessibilityHelpDescr";

		// Token: 0x0400302B RID: 12331
		internal const string ToolStripItemOnQueryContinueDragDescr = "ToolStripItemOnQueryContinueDragDescr";

		// Token: 0x0400302C RID: 12332
		internal const string ToolStripItemOnRightToLeftChangedDescr = "ToolStripItemOnRightToLeftChangedDescr";

		// Token: 0x0400302D RID: 12333
		internal const string ToolStripItemOnTextChangedDescr = "ToolStripItemOnTextChangedDescr";

		// Token: 0x0400302E RID: 12334
		internal const string ToolStripItemOnVisibleChangedDescr = "ToolStripItemOnVisibleChangedDescr";

		// Token: 0x0400302F RID: 12335
		internal const string ToolStripItemOverflow = "ToolStripItemOverflow";

		// Token: 0x04003030 RID: 12336
		internal const string ToolStripItemOverflowDescr = "ToolStripItemOverflowDescr";

		// Token: 0x04003031 RID: 12337
		internal const string ToolStripItemOwnerChangedDescr = "ToolStripItemOwnerChangedDescr";

		// Token: 0x04003032 RID: 12338
		internal const string ToolStripItemPaddingDescr = "ToolStripItemPaddingDescr";

		// Token: 0x04003033 RID: 12339
		internal const string ToolStripItemRemovedDescr = "ToolStripItemRemovedDescr";

		// Token: 0x04003034 RID: 12340
		internal const string ToolStripItemRightToLeftAutoMirrorImageDescr = "ToolStripItemRightToLeftAutoMirrorImageDescr";

		// Token: 0x04003035 RID: 12341
		internal const string ToolStripItemRightToLeftDescr = "ToolStripItemRightToLeftDescr";

		// Token: 0x04003036 RID: 12342
		internal const string ToolStripItemsDescr = "ToolStripItemsDescr";

		// Token: 0x04003037 RID: 12343
		internal const string ToolStripItemSize = "ToolStripItemSize";

		// Token: 0x04003038 RID: 12344
		internal const string ToolStripItemSizeDescr = "ToolStripItemSizeDescr";

		// Token: 0x04003039 RID: 12345
		internal const string ToolStripItemTagDescr = "ToolStripItemTagDescr";

		// Token: 0x0400303A RID: 12346
		internal const string ToolStripItemTextAlignDescr = "ToolStripItemTextAlignDescr";

		// Token: 0x0400303B RID: 12347
		internal const string ToolStripItemTextDescr = "ToolStripItemTextDescr";

		// Token: 0x0400303C RID: 12348
		internal const string ToolStripItemTextImageRelationDescr = "ToolStripItemTextImageRelationDescr";

		// Token: 0x0400303D RID: 12349
		internal const string ToolStripItemToolTipTextDescr = "ToolStripItemToolTipTextDescr";

		// Token: 0x0400303E RID: 12350
		internal const string ToolStripItemVisibleDescr = "ToolStripItemVisibleDescr";

		// Token: 0x0400303F RID: 12351
		internal const string ToolStripLabelActiveLinkColorDescr = "ToolStripLabelActiveLinkColorDescr";

		// Token: 0x04003040 RID: 12352
		internal const string ToolStripLabelIsLinkDescr = "ToolStripLabelIsLinkDescr";

		// Token: 0x04003041 RID: 12353
		internal const string ToolStripLabelLinkBehaviorDescr = "ToolStripLabelLinkBehaviorDescr";

		// Token: 0x04003042 RID: 12354
		internal const string ToolStripLabelLinkColorDescr = "ToolStripLabelLinkColorDescr";

		// Token: 0x04003043 RID: 12355
		internal const string ToolStripLabelLinkVisitedDescr = "ToolStripLabelLinkVisitedDescr";

		// Token: 0x04003044 RID: 12356
		internal const string ToolStripLabelVisitedLinkColorDescr = "ToolStripLabelVisitedLinkColorDescr";

		// Token: 0x04003045 RID: 12357
		internal const string ToolStripLayoutCompleteDescr = "ToolStripLayoutCompleteDescr";

		// Token: 0x04003046 RID: 12358
		internal const string ToolStripLayoutStyle = "ToolStripLayoutStyle";

		// Token: 0x04003047 RID: 12359
		internal const string ToolStripLayoutStyleChangedDescr = "ToolStripLayoutStyleChangedDescr";

		// Token: 0x04003048 RID: 12360
		internal const string ToolStripMenuItemShortcutKeyDisplayStringDescr = "ToolStripMenuItemShortcutKeyDisplayStringDescr";

		// Token: 0x04003049 RID: 12361
		internal const string ToolStripMergeActionDescr = "ToolStripMergeActionDescr";

		// Token: 0x0400304A RID: 12362
		internal const string ToolStripMergeImpossibleIdentical = "ToolStripMergeImpossibleIdentical";

		// Token: 0x0400304B RID: 12363
		internal const string ToolStripMergeIndexDescr = "ToolStripMergeIndexDescr";

		// Token: 0x0400304C RID: 12364
		internal const string ToolStripMustSupplyItsOwnComboBox = "ToolStripMustSupplyItsOwnComboBox";

		// Token: 0x0400304D RID: 12365
		internal const string ToolStripMustSupplyItsOwnTextBox = "ToolStripMustSupplyItsOwnTextBox";

		// Token: 0x0400304E RID: 12366
		internal const string ToolStripOnBeginDrag = "ToolStripOnBeginDrag";

		// Token: 0x0400304F RID: 12367
		internal const string ToolStripOnEndDrag = "ToolStripOnEndDrag";

		// Token: 0x04003050 RID: 12368
		internal const string ToolStripOptions = "ToolStripOptions";

		// Token: 0x04003051 RID: 12369
		internal const string ToolStripPaintGripDescr = "ToolStripPaintGripDescr";

		// Token: 0x04003052 RID: 12370
		internal const string ToolStripPanelRowsDescr = "ToolStripPanelRowsDescr";

		// Token: 0x04003053 RID: 12371
		internal const string ToolStripPanelRowControlCollectionIncorrectIndexLength = "ToolStripPanelRowControlCollectionIncorrectIndexLength";

		// Token: 0x04003054 RID: 12372
		internal const string ToolStripRendererChanged = "ToolStripRendererChanged";

		// Token: 0x04003055 RID: 12373
		internal const string ToolStripRenderModeDescr = "ToolStripRenderModeDescr";

		// Token: 0x04003056 RID: 12374
		internal const string ToolStripRenderModeUseRendererPropertyInstead = "ToolStripRenderModeUseRendererPropertyInstead";

		// Token: 0x04003057 RID: 12375
		internal const string ToolStripSaveSettingsDescr = "ToolStripSaveSettingsDescr";

		// Token: 0x04003058 RID: 12376
		internal const string ToolStripSettingsKeyDescr = "ToolStripSettingsKeyDescr";

		// Token: 0x04003059 RID: 12377
		internal const string ToolStripShowDropDownInvalidOperation = "ToolStripShowDropDownInvalidOperation";

		// Token: 0x0400305A RID: 12378
		internal const string ToolStripShowItemToolTipsDescr = "ToolStripShowItemToolTipsDescr";

		// Token: 0x0400305B RID: 12379
		internal const string ToolStripSplitButtonDropDownButtonWidthDescr = "ToolStripSplitButtonDropDownButtonWidthDescr";

		// Token: 0x0400305C RID: 12380
		internal const string ToolStripSplitButtonOnButtonClickDescr = "ToolStripSplitButtonOnButtonClickDescr";

		// Token: 0x0400305D RID: 12381
		internal const string ToolStripSplitButtonOnButtonDoubleClickDescr = "ToolStripSplitButtonOnButtonDoubleClickDescr";

		// Token: 0x0400305E RID: 12382
		internal const string ToolStripSplitButtonOnDefaultItemChangedDescr = "ToolStripSplitButtonOnDefaultItemChangedDescr";

		// Token: 0x0400305F RID: 12383
		internal const string ToolStripSplitButtonSplitterWidthDescr = "ToolStripSplitButtonSplitterWidthDescr";

		// Token: 0x04003060 RID: 12384
		internal const string ToolStripSplitStackLayoutContainerMustBeAToolStrip = "ToolStripSplitStackLayoutContainerMustBeAToolStrip";

		// Token: 0x04003061 RID: 12385
		internal const string ToolStripStatusLabelBorderSidesDescr = "ToolStripStatusLabelBorderSidesDescr";

		// Token: 0x04003062 RID: 12386
		internal const string ToolStripStatusLabelBorderStyleDescr = "ToolStripStatusLabelBorderStyleDescr";

		// Token: 0x04003063 RID: 12387
		internal const string ToolStripStatusLabelSpringDescr = "ToolStripStatusLabelSpringDescr";

		// Token: 0x04003064 RID: 12388
		internal const string ToolStripStretchDescr = "ToolStripStretchDescr";

		// Token: 0x04003065 RID: 12389
		internal const string ToolStripTextBoxTextBoxTextAlignChangedDescr = "ToolStripTextBoxTextBoxTextAlignChangedDescr";

		// Token: 0x04003066 RID: 12390
		internal const string ToolStripTextDirectionDescr = "ToolStripTextDirectionDescr";

		// Token: 0x04003067 RID: 12391
		internal const string ToolTipActiveDescr = "ToolTipActiveDescr";

		// Token: 0x04003068 RID: 12392
		internal const string ToolTipAddFailed = "ToolTipAddFailed";

		// Token: 0x04003069 RID: 12393
		internal const string ToolTipAutomaticDelayDescr = "ToolTipAutomaticDelayDescr";

		// Token: 0x0400306A RID: 12394
		internal const string ToolTipAutoPopDelayDescr = "ToolTipAutoPopDelayDescr";

		// Token: 0x0400306B RID: 12395
		internal const string ToolTipBackColorDescr = "ToolTipBackColorDescr";

		// Token: 0x0400306C RID: 12396
		internal const string ToolTipDrawEventDescr = "ToolTipDrawEventDescr";

		// Token: 0x0400306D RID: 12397
		internal const string ToolTipEmptyColor = "ToolTipEmptyColor";

		// Token: 0x0400306E RID: 12398
		internal const string ToolTipForeColorDescr = "ToolTipForeColorDescr";

		// Token: 0x0400306F RID: 12399
		internal const string ToolTipInitialDelayDescr = "ToolTipInitialDelayDescr";

		// Token: 0x04003070 RID: 12400
		internal const string ToolTipIsBalloonDescr = "ToolTipIsBalloonDescr";

		// Token: 0x04003071 RID: 12401
		internal const string ToolTipOwnerDrawDescr = "ToolTipOwnerDrawDescr";

		// Token: 0x04003072 RID: 12402
		internal const string ToolTipPopupEventDescr = "ToolTipPopupEventDescr";

		// Token: 0x04003073 RID: 12403
		internal const string ToolTipReshowDelayDescr = "ToolTipReshowDelayDescr";

		// Token: 0x04003074 RID: 12404
		internal const string ToolTipShowAlwaysDescr = "ToolTipShowAlwaysDescr";

		// Token: 0x04003075 RID: 12405
		internal const string ToolTipStripAmpersandsDescr = "ToolTipStripAmpersandsDescr";

		// Token: 0x04003076 RID: 12406
		internal const string ToolTipTitleDescr = "ToolTipTitleDescr";

		// Token: 0x04003077 RID: 12407
		internal const string ToolTipToolTipDescr = "ToolTipToolTipDescr";

		// Token: 0x04003078 RID: 12408
		internal const string ToolTipToolTipIconDescr = "ToolTipToolTipIconDescr";

		// Token: 0x04003079 RID: 12409
		internal const string ToolTipUseAnimationDescr = "ToolTipUseAnimationDescr";

		// Token: 0x0400307A RID: 12410
		internal const string ToolTipUseFadingDescr = "ToolTipUseFadingDescr";

		// Token: 0x0400307B RID: 12411
		internal const string TooManyResumeUpdateMenuHandles = "TooManyResumeUpdateMenuHandles";

		// Token: 0x0400307C RID: 12412
		internal const string TopLevelControlAdd = "TopLevelControlAdd";

		// Token: 0x0400307D RID: 12413
		internal const string TopLevelNotAllowedIfActiveX = "TopLevelNotAllowedIfActiveX";

		// Token: 0x0400307E RID: 12414
		internal const string TopLevelParentedControl = "TopLevelParentedControl";

		// Token: 0x0400307F RID: 12415
		internal const string toStringAlt = "toStringAlt";

		// Token: 0x04003080 RID: 12416
		internal const string toStringBack = "toStringBack";

		// Token: 0x04003081 RID: 12417
		internal const string toStringControl = "toStringControl";

		// Token: 0x04003082 RID: 12418
		internal const string toStringDefault = "toStringDefault";

		// Token: 0x04003083 RID: 12419
		internal const string toStringDelete = "toStringDelete";

		// Token: 0x04003084 RID: 12420
		internal const string toStringEnd = "toStringEnd";

		// Token: 0x04003085 RID: 12421
		internal const string toStringEnter = "toStringEnter";

		// Token: 0x04003086 RID: 12422
		internal const string toStringHome = "toStringHome";

		// Token: 0x04003087 RID: 12423
		internal const string toStringInsert = "toStringInsert";

		// Token: 0x04003088 RID: 12424
		internal const string toStringNone = "toStringNone";

		// Token: 0x04003089 RID: 12425
		internal const string toStringPageDown = "toStringPageDown";

		// Token: 0x0400308A RID: 12426
		internal const string toStringPageUp = "toStringPageUp";

		// Token: 0x0400308B RID: 12427
		internal const string toStringShift = "toStringShift";

		// Token: 0x0400308C RID: 12428
		internal const string TrackBarAutoSizeDescr = "TrackBarAutoSizeDescr";

		// Token: 0x0400308D RID: 12429
		internal const string TrackBarLargeChangeDescr = "TrackBarLargeChangeDescr";

		// Token: 0x0400308E RID: 12430
		internal const string TrackBarLargeChangeError = "TrackBarLargeChangeError";

		// Token: 0x0400308F RID: 12431
		internal const string TrackBarMaximumDescr = "TrackBarMaximumDescr";

		// Token: 0x04003090 RID: 12432
		internal const string TrackBarMinimumDescr = "TrackBarMinimumDescr";

		// Token: 0x04003091 RID: 12433
		internal const string TrackBarOnScrollDescr = "TrackBarOnScrollDescr";

		// Token: 0x04003092 RID: 12434
		internal const string TrackBarOrientationDescr = "TrackBarOrientationDescr";

		// Token: 0x04003093 RID: 12435
		internal const string TrackBarSmallChangeDescr = "TrackBarSmallChangeDescr";

		// Token: 0x04003094 RID: 12436
		internal const string TrackBarSmallChangeError = "TrackBarSmallChangeError";

		// Token: 0x04003095 RID: 12437
		internal const string TrackBarTickFrequencyDescr = "TrackBarTickFrequencyDescr";

		// Token: 0x04003096 RID: 12438
		internal const string TrackBarTickStyleDescr = "TrackBarTickStyleDescr";

		// Token: 0x04003097 RID: 12439
		internal const string TrackBarValueDescr = "TrackBarValueDescr";

		// Token: 0x04003098 RID: 12440
		internal const string TransparentBackColorNotAllowed = "TransparentBackColorNotAllowed";

		// Token: 0x04003099 RID: 12441
		internal const string TrayIcon_TextTooLong = "TrayIcon_TextTooLong";

		// Token: 0x0400309A RID: 12442
		internal const string TreeNodeBackColorDescr = "TreeNodeBackColorDescr";

		// Token: 0x0400309B RID: 12443
		internal const string TreeNodeBeginEditFailed = "TreeNodeBeginEditFailed";

		// Token: 0x0400309C RID: 12444
		internal const string TreeNodeCheckedDescr = "TreeNodeCheckedDescr";

		// Token: 0x0400309D RID: 12445
		internal const string TreeNodeCollectionBadTreeNode = "TreeNodeCollectionBadTreeNode";

		// Token: 0x0400309E RID: 12446
		internal const string TreeNodeForeColorDescr = "TreeNodeForeColorDescr";

		// Token: 0x0400309F RID: 12447
		internal const string TreeNodeImageIndexDescr = "TreeNodeImageIndexDescr";

		// Token: 0x040030A0 RID: 12448
		internal const string TreeNodeImageKeyDescr = "TreeNodeImageKeyDescr";

		// Token: 0x040030A1 RID: 12449
		internal const string TreeNodeIndexDescr = "TreeNodeIndexDescr";

		// Token: 0x040030A2 RID: 12450
		internal const string TreeNodeNodeFontDescr = "TreeNodeNodeFontDescr";

		// Token: 0x040030A3 RID: 12451
		internal const string TreeNodeNodeNameDescr = "TreeNodeNodeNameDescr";

		// Token: 0x040030A4 RID: 12452
		internal const string TreeNodeNoParent = "TreeNodeNoParent";

		// Token: 0x040030A5 RID: 12453
		internal const string TreeNodeSelectedImageIndexDescr = "TreeNodeSelectedImageIndexDescr";

		// Token: 0x040030A6 RID: 12454
		internal const string TreeNodeSelectedImageKeyDescr = "TreeNodeSelectedImageKeyDescr";

		// Token: 0x040030A7 RID: 12455
		internal const string TreeNodeStateImageIndexDescr = "TreeNodeStateImageIndexDescr";

		// Token: 0x040030A8 RID: 12456
		internal const string TreeNodeStateImageKeyDescr = "TreeNodeStateImageKeyDescr";

		// Token: 0x040030A9 RID: 12457
		internal const string TreeNodeTextDescr = "TreeNodeTextDescr";

		// Token: 0x040030AA RID: 12458
		internal const string TreeNodeToolTipTextDescr = "TreeNodeToolTipTextDescr";

		// Token: 0x040030AB RID: 12459
		internal const string TreeViewAfterCheckDescr = "TreeViewAfterCheckDescr";

		// Token: 0x040030AC RID: 12460
		internal const string TreeViewAfterCollapseDescr = "TreeViewAfterCollapseDescr";

		// Token: 0x040030AD RID: 12461
		internal const string TreeViewAfterEditDescr = "TreeViewAfterEditDescr";

		// Token: 0x040030AE RID: 12462
		internal const string TreeViewAfterExpandDescr = "TreeViewAfterExpandDescr";

		// Token: 0x040030AF RID: 12463
		internal const string TreeViewAfterSelectDescr = "TreeViewAfterSelectDescr";

		// Token: 0x040030B0 RID: 12464
		internal const string TreeViewBeforeCheckDescr = "TreeViewBeforeCheckDescr";

		// Token: 0x040030B1 RID: 12465
		internal const string TreeViewBeforeCollapseDescr = "TreeViewBeforeCollapseDescr";

		// Token: 0x040030B2 RID: 12466
		internal const string TreeViewBeforeEditDescr = "TreeViewBeforeEditDescr";

		// Token: 0x040030B3 RID: 12467
		internal const string TreeViewBeforeExpandDescr = "TreeViewBeforeExpandDescr";

		// Token: 0x040030B4 RID: 12468
		internal const string TreeViewBeforeSelectDescr = "TreeViewBeforeSelectDescr";

		// Token: 0x040030B5 RID: 12469
		internal const string TreeViewCheckBoxesDescr = "TreeViewCheckBoxesDescr";

		// Token: 0x040030B6 RID: 12470
		internal const string TreeViewDrawModeDescr = "TreeViewDrawModeDescr";

		// Token: 0x040030B7 RID: 12471
		internal const string TreeViewDrawNodeEventDescr = "TreeViewDrawNodeEventDescr";

		// Token: 0x040030B8 RID: 12472
		internal const string TreeViewFullRowSelectDescr = "TreeViewFullRowSelectDescr";

		// Token: 0x040030B9 RID: 12473
		internal const string TreeViewHideSelectionDescr = "TreeViewHideSelectionDescr";

		// Token: 0x040030BA RID: 12474
		internal const string TreeViewHotTrackingDescr = "TreeViewHotTrackingDescr";

		// Token: 0x040030BB RID: 12475
		internal const string TreeViewImageIndexDescr = "TreeViewImageIndexDescr";

		// Token: 0x040030BC RID: 12476
		internal const string TreeViewImageKeyDescr = "TreeViewImageKeyDescr";

		// Token: 0x040030BD RID: 12477
		internal const string TreeViewImageListDescr = "TreeViewImageListDescr";

		// Token: 0x040030BE RID: 12478
		internal const string TreeViewIndentDescr = "TreeViewIndentDescr";

		// Token: 0x040030BF RID: 12479
		internal const string TreeViewItemHeightDescr = "TreeViewItemHeightDescr";

		// Token: 0x040030C0 RID: 12480
		internal const string TreeViewLabelEditDescr = "TreeViewLabelEditDescr";

		// Token: 0x040030C1 RID: 12481
		internal const string TreeViewLineColorDescr = "TreeViewLineColorDescr";

		// Token: 0x040030C2 RID: 12482
		internal const string TreeViewNodeMouseClickDescr = "TreeViewNodeMouseClickDescr";

		// Token: 0x040030C3 RID: 12483
		internal const string TreeViewNodeMouseDoubleClickDescr = "TreeViewNodeMouseDoubleClickDescr";

		// Token: 0x040030C4 RID: 12484
		internal const string TreeViewNodeMouseHoverDescr = "TreeViewNodeMouseHoverDescr";

		// Token: 0x040030C5 RID: 12485
		internal const string TreeViewNodesDescr = "TreeViewNodesDescr";

		// Token: 0x040030C6 RID: 12486
		internal const string TreeViewNodeSorterDescr = "TreeViewNodeSorterDescr";

		// Token: 0x040030C7 RID: 12487
		internal const string TreeViewPathSeparatorDescr = "TreeViewPathSeparatorDescr";

		// Token: 0x040030C8 RID: 12488
		internal const string TreeViewScrollableDescr = "TreeViewScrollableDescr";

		// Token: 0x040030C9 RID: 12489
		internal const string TreeViewSelectedImageIndexDescr = "TreeViewSelectedImageIndexDescr";

		// Token: 0x040030CA RID: 12490
		internal const string TreeViewSelectedImageKeyDescr = "TreeViewSelectedImageKeyDescr";

		// Token: 0x040030CB RID: 12491
		internal const string TreeViewSelectedNodeDescr = "TreeViewSelectedNodeDescr";

		// Token: 0x040030CC RID: 12492
		internal const string TreeViewShowLinesDescr = "TreeViewShowLinesDescr";

		// Token: 0x040030CD RID: 12493
		internal const string TreeViewShowPlusMinusDescr = "TreeViewShowPlusMinusDescr";

		// Token: 0x040030CE RID: 12494
		internal const string TreeViewShowRootLinesDescr = "TreeViewShowRootLinesDescr";

		// Token: 0x040030CF RID: 12495
		internal const string TreeViewShowShowNodeToolTipsDescr = "TreeViewShowShowNodeToolTipsDescr";

		// Token: 0x040030D0 RID: 12496
		internal const string TreeViewSortedDescr = "TreeViewSortedDescr";

		// Token: 0x040030D1 RID: 12497
		internal const string TreeViewStateImageListDescr = "TreeViewStateImageListDescr";

		// Token: 0x040030D2 RID: 12498
		internal const string TreeViewTopNodeDescr = "TreeViewTopNodeDescr";

		// Token: 0x040030D3 RID: 12499
		internal const string TreeViewVisibleCountDescr = "TreeViewVisibleCountDescr";

		// Token: 0x040030D4 RID: 12500
		internal const string TrustManager_WarningIconAccessibleDescription_HighRisk = "TrustManager_WarningIconAccessibleDescription_HighRisk";

		// Token: 0x040030D5 RID: 12501
		internal const string TrustManager_WarningIconAccessibleDescription_LowRisk = "TrustManager_WarningIconAccessibleDescription_LowRisk";

		// Token: 0x040030D6 RID: 12502
		internal const string TrustManager_WarningIconAccessibleDescription_MediumRisk = "TrustManager_WarningIconAccessibleDescription_MediumRisk";

		// Token: 0x040030D7 RID: 12503
		internal const string TrustManagerBadXml = "TrustManagerBadXml";

		// Token: 0x040030D8 RID: 12504
		internal const string TrustManagerMoreInfo_InstallTitle = "TrustManagerMoreInfo_InstallTitle";

		// Token: 0x040030D9 RID: 12505
		internal const string TrustManagerMoreInfo_InternetSource = "TrustManagerMoreInfo_InternetSource";

		// Token: 0x040030DA RID: 12506
		internal const string TrustManagerMoreInfo_KnownPublisher = "TrustManagerMoreInfo_KnownPublisher";

		// Token: 0x040030DB RID: 12507
		internal const string TrustManagerMoreInfo_LocalComputerSource = "TrustManagerMoreInfo_LocalComputerSource";

		// Token: 0x040030DC RID: 12508
		internal const string TrustManagerMoreInfo_LocalNetworkSource = "TrustManagerMoreInfo_LocalNetworkSource";

		// Token: 0x040030DD RID: 12509
		internal const string TrustManagerMoreInfo_Location = "TrustManagerMoreInfo_Location";

		// Token: 0x040030DE RID: 12510
		internal const string TrustManagerMoreInfo_RunTitle = "TrustManagerMoreInfo_RunTitle";

		// Token: 0x040030DF RID: 12511
		internal const string TrustManagerMoreInfo_SafeAccess = "TrustManagerMoreInfo_SafeAccess";

		// Token: 0x040030E0 RID: 12512
		internal const string TrustManagerMoreInfo_UnknownPublisher = "TrustManagerMoreInfo_UnknownPublisher";

		// Token: 0x040030E1 RID: 12513
		internal const string TrustManagerMoreInfo_UnsafeAccess = "TrustManagerMoreInfo_UnsafeAccess";

		// Token: 0x040030E2 RID: 12514
		internal const string TrustManagerMoreInfo_UntrustedSitesSource = "TrustManagerMoreInfo_UntrustedSitesSource";

		// Token: 0x040030E3 RID: 12515
		internal const string TrustManagerMoreInfo_TrustedSitesSource = "TrustManagerMoreInfo_TrustedSitesSource";

		// Token: 0x040030E4 RID: 12516
		internal const string TrustManagerMoreInfo_WithoutShortcut = "TrustManagerMoreInfo_WithoutShortcut";

		// Token: 0x040030E5 RID: 12517
		internal const string TrustManagerMoreInfo_WithShortcut = "TrustManagerMoreInfo_WithShortcut";

		// Token: 0x040030E6 RID: 12518
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallBlocked = "TrustManagerPromptUI_AccessibleDescription_InstallBlocked";

		// Token: 0x040030E7 RID: 12519
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallConfirmation = "TrustManagerPromptUI_AccessibleDescription_InstallConfirmation";

		// Token: 0x040030E8 RID: 12520
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallWithElevatedPermissions = "TrustManagerPromptUI_AccessibleDescription_InstallWithElevatedPermissions";

		// Token: 0x040030E9 RID: 12521
		internal const string TrustManagerPromptUI_AccessibleDescription_RunBlocked = "TrustManagerPromptUI_AccessibleDescription_RunBlocked";

		// Token: 0x040030EA RID: 12522
		internal const string TrustManagerPromptUI_AccessibleDescription_RunConfirmation = "TrustManagerPromptUI_AccessibleDescription_RunConfirmation";

		// Token: 0x040030EB RID: 12523
		internal const string TrustManagerPromptUI_AccessibleDescription_RunWithElevatedPermissions = "TrustManagerPromptUI_AccessibleDescription_RunWithElevatedPermissions";

		// Token: 0x040030EC RID: 12524
		internal const string TrustManagerPromptUI_BlockedApp = "TrustManagerPromptUI_BlockedApp";

		// Token: 0x040030ED RID: 12525
		internal const string TrustManagerPromptUI_InstalledAppBlockedWarning = "TrustManagerPromptUI_InstalledAppBlockedWarning";

		// Token: 0x040030EE RID: 12526
		internal const string TrustManagerPromptUI_InstallFromLocalMachineWarning = "TrustManagerPromptUI_InstallFromLocalMachineWarning";

		// Token: 0x040030EF RID: 12527
		internal const string TrustManagerPromptUI_InstallQuestion = "TrustManagerPromptUI_InstallQuestion";

		// Token: 0x040030F0 RID: 12528
		internal const string TrustManagerPromptUI_InstallTitle = "TrustManagerPromptUI_InstallTitle";

		// Token: 0x040030F1 RID: 12529
		internal const string TrustManagerPromptUI_InstallWarning = "TrustManagerPromptUI_InstallWarning";

		// Token: 0x040030F2 RID: 12530
		internal const string TrustManagerPromptUI_MoreInformation = "TrustManagerPromptUI_MoreInformation";

		// Token: 0x040030F3 RID: 12531
		internal const string TrustManagerPromptUI_MoreInformationAccessibleDescription = "TrustManagerPromptUI_MoreInformationAccessibleDescription";

		// Token: 0x040030F4 RID: 12532
		internal const string TrustManagerPromptUI_MoreInformationAccessibleName = "TrustManagerPromptUI_MoreInformationAccessibleName";

		// Token: 0x040030F5 RID: 12533
		internal const string TrustManagerPromptUI_NoPublisherInstallQuestion = "TrustManagerPromptUI_NoPublisherInstallQuestion";

		// Token: 0x040030F6 RID: 12534
		internal const string TrustManagerPromptUI_NoPublisherRunQuestion = "TrustManagerPromptUI_NoPublisherRunQuestion";

		// Token: 0x040030F7 RID: 12535
		internal const string TrustManagerPromptUI_Close = "TrustManagerPromptUI_Close";

		// Token: 0x040030F8 RID: 12536
		internal const string TrustManagerPromptUI_DoNotInstall = "TrustManagerPromptUI_DoNotInstall";

		// Token: 0x040030F9 RID: 12537
		internal const string TrustManagerPromptUI_DoNotRun = "TrustManagerPromptUI_DoNotRun";

		// Token: 0x040030FA RID: 12538
		internal const string TrustManagerPromptUI_Run = "TrustManagerPromptUI_Run";

		// Token: 0x040030FB RID: 12539
		internal const string TrustManagerPromptUI_RunAppBlockedWarning = "TrustManagerPromptUI_RunAppBlockedWarning";

		// Token: 0x040030FC RID: 12540
		internal const string TrustManagerPromptUI_RunFromLocalMachineWarning = "TrustManagerPromptUI_RunFromLocalMachineWarning";

		// Token: 0x040030FD RID: 12541
		internal const string TrustManagerPromptUI_RunQuestion = "TrustManagerPromptUI_RunQuestion";

		// Token: 0x040030FE RID: 12542
		internal const string TrustManagerPromptUI_RunTitle = "TrustManagerPromptUI_RunTitle";

		// Token: 0x040030FF RID: 12543
		internal const string TrustManagerPromptUI_RunWarning = "TrustManagerPromptUI_RunWarning";

		// Token: 0x04003100 RID: 12544
		internal const string TrustManagerPromptUI_UnknownPublisher = "TrustManagerPromptUI_UnknownPublisher";

		// Token: 0x04003101 RID: 12545
		internal const string TrustManagerPromptUI_WarningAccessibleDescription = "TrustManagerPromptUI_WarningAccessibleDescription";

		// Token: 0x04003102 RID: 12546
		internal const string TrustManagerPromptUI_WarningAccessibleName = "TrustManagerPromptUI_WarningAccessibleName";

		// Token: 0x04003103 RID: 12547
		internal const string TypedControlCollectionShouldBeOfType = "TypedControlCollectionShouldBeOfType";

		// Token: 0x04003104 RID: 12548
		internal const string TypedControlCollectionShouldBeOfTypes = "TypedControlCollectionShouldBeOfTypes";

		// Token: 0x04003105 RID: 12549
		internal const string TYPEINFOPROCESSORGetDocumentationFailed = "TYPEINFOPROCESSORGetDocumentationFailed";

		// Token: 0x04003106 RID: 12550
		internal const string TYPEINFOPROCESSORGetRefTypeInfoFailed = "TYPEINFOPROCESSORGetRefTypeInfoFailed";

		// Token: 0x04003107 RID: 12551
		internal const string TYPEINFOPROCESSORGetTypeAttrFailed = "TYPEINFOPROCESSORGetTypeAttrFailed";

		// Token: 0x04003108 RID: 12552
		internal const string TypeLoadException = "TypeLoadException";

		// Token: 0x04003109 RID: 12553
		internal const string TypeLoadExceptionShort = "TypeLoadExceptionShort";

		// Token: 0x0400310A RID: 12554
		internal const string UnableToInitComponent = "UnableToInitComponent";

		// Token: 0x0400310B RID: 12555
		internal const string UnableToSetPanelText = "UnableToSetPanelText";

		// Token: 0x0400310C RID: 12556
		internal const string UnknownAttr = "UnknownAttr";

		// Token: 0x0400310D RID: 12557
		internal const string UnknownInputLanguageLayout = "UnknownInputLanguageLayout";

		// Token: 0x0400310E RID: 12558
		internal const string UnsafeNativeMethodsNotImplemented = "UnsafeNativeMethodsNotImplemented";

		// Token: 0x0400310F RID: 12559
		internal const string UpDownBaseAlignmentDescr = "UpDownBaseAlignmentDescr";

		// Token: 0x04003110 RID: 12560
		internal const string UpDownBaseBorderStyleDescr = "UpDownBaseBorderStyleDescr";

		// Token: 0x04003111 RID: 12561
		internal const string UpDownBaseDownButtonAccName = "UpDownBaseDownButtonAccName";

		// Token: 0x04003112 RID: 12562
		internal const string UpDownBaseInterceptArrowKeysDescr = "UpDownBaseInterceptArrowKeysDescr";

		// Token: 0x04003113 RID: 12563
		internal const string UpDownBasePreferredHeightDescr = "UpDownBasePreferredHeightDescr";

		// Token: 0x04003114 RID: 12564
		internal const string UpDownBaseReadOnlyDescr = "UpDownBaseReadOnlyDescr";

		// Token: 0x04003115 RID: 12565
		internal const string UpDownBaseTextAlignDescr = "UpDownBaseTextAlignDescr";

		// Token: 0x04003116 RID: 12566
		internal const string UpDownBaseUpButtonAccName = "UpDownBaseUpButtonAccName";

		// Token: 0x04003117 RID: 12567
		internal const string UseCompatibleTextRenderingDescr = "UseCompatibleTextRenderingDescr";

		// Token: 0x04003118 RID: 12568
		internal const string UserControlBorderStyleDescr = "UserControlBorderStyleDescr";

		// Token: 0x04003119 RID: 12569
		internal const string UserControlOnLoadDescr = "UserControlOnLoadDescr";

		// Token: 0x0400311A RID: 12570
		internal const string valueChangedEventDescr = "valueChangedEventDescr";

		// Token: 0x0400311B RID: 12571
		internal const string VisualStyleHandleCreationFailed = "VisualStyleHandleCreationFailed";

		// Token: 0x0400311C RID: 12572
		internal const string VisualStyleNotActive = "VisualStyleNotActive";

		// Token: 0x0400311D RID: 12573
		internal const string VisualStylesDisabledInClientArea = "VisualStylesDisabledInClientArea";

		// Token: 0x0400311E RID: 12574
		internal const string VisualStylesInvalidCombination = "VisualStylesInvalidCombination";

		// Token: 0x0400311F RID: 12575
		internal const string WebBrowserAllowDropNotSupported = "WebBrowserAllowDropNotSupported";

		// Token: 0x04003120 RID: 12576
		internal const string WebBrowserAllowNavigationDescr = "WebBrowserAllowNavigationDescr";

		// Token: 0x04003121 RID: 12577
		internal const string WebBrowserAllowWebBrowserDropDescr = "WebBrowserAllowWebBrowserDropDescr";

		// Token: 0x04003122 RID: 12578
		internal const string WebBrowserBackgroundImageLayoutNotSupported = "WebBrowserBackgroundImageLayoutNotSupported";

		// Token: 0x04003123 RID: 12579
		internal const string WebBrowserBackgroundImageNotSupported = "WebBrowserBackgroundImageNotSupported";

		// Token: 0x04003124 RID: 12580
		internal const string WebBrowserCanGoBackChangedDescr = "WebBrowserCanGoBackChangedDescr";

		// Token: 0x04003125 RID: 12581
		internal const string WebBrowserCanGoForwardChangedDescr = "WebBrowserCanGoForwardChangedDescr";

		// Token: 0x04003126 RID: 12582
		internal const string WebBrowserCursorNotSupported = "WebBrowserCursorNotSupported";

		// Token: 0x04003127 RID: 12583
		internal const string WebBrowserDocumentCompletedDescr = "WebBrowserDocumentCompletedDescr";

		// Token: 0x04003128 RID: 12584
		internal const string WebBrowserDocumentTitleChangedDescr = "WebBrowserDocumentTitleChangedDescr";

		// Token: 0x04003129 RID: 12585
		internal const string WebBrowserEnabledNotSupported = "WebBrowserEnabledNotSupported";

		// Token: 0x0400312A RID: 12586
		internal const string WebBrowserEncryptionLevelChangedDescr = "WebBrowserEncryptionLevelChangedDescr";

		// Token: 0x0400312B RID: 12587
		internal const string WebBrowserFileDownloadDescr = "WebBrowserFileDownloadDescr";

		// Token: 0x0400312C RID: 12588
		internal const string WebBrowserInIENotSupported = "WebBrowserInIENotSupported";

		// Token: 0x0400312D RID: 12589
		internal const string WebBrowserIsOfflineDescr = "WebBrowserIsOfflineDescr";

		// Token: 0x0400312E RID: 12590
		internal const string WebBrowserIsWebBrowserContextMenuEnabledDescr = "WebBrowserIsWebBrowserContextMenuEnabledDescr";

		// Token: 0x0400312F RID: 12591
		internal const string WebBrowserNavigateAbsoluteUri = "WebBrowserNavigateAbsoluteUri";

		// Token: 0x04003130 RID: 12592
		internal const string WebBrowserNavigatedDescr = "WebBrowserNavigatedDescr";

		// Token: 0x04003131 RID: 12593
		internal const string WebBrowserNavigatingDescr = "WebBrowserNavigatingDescr";

		// Token: 0x04003132 RID: 12594
		internal const string WebBrowserNewWindowDescr = "WebBrowserNewWindowDescr";

		// Token: 0x04003133 RID: 12595
		internal const string WebBrowserNoCastToIWebBrowser2 = "WebBrowserNoCastToIWebBrowser2";

		// Token: 0x04003134 RID: 12596
		internal const string WebBrowserObjectForScriptingComVisibleOnly = "WebBrowserObjectForScriptingComVisibleOnly";

		// Token: 0x04003135 RID: 12597
		internal const string WebBrowserProgressChangedDescr = "WebBrowserProgressChangedDescr";

		// Token: 0x04003136 RID: 12598
		internal const string WebBrowserRightToLeftNotSupported = "WebBrowserRightToLeftNotSupported";

		// Token: 0x04003137 RID: 12599
		internal const string WebBrowserScriptErrorsSuppressedDescr = "WebBrowserScriptErrorsSuppressedDescr";

		// Token: 0x04003138 RID: 12600
		internal const string WebBrowserScrollBarsEnabledDescr = "WebBrowserScrollBarsEnabledDescr";

		// Token: 0x04003139 RID: 12601
		internal const string WebBrowserSecurityLevelDescr = "WebBrowserSecurityLevelDescr";

		// Token: 0x0400313A RID: 12602
		internal const string WebBrowserStatusTextChangedDescr = "WebBrowserStatusTextChangedDescr";

		// Token: 0x0400313B RID: 12603
		internal const string WebBrowserTextNotSupported = "WebBrowserTextNotSupported";

		// Token: 0x0400313C RID: 12604
		internal const string WebBrowserUrlDescr = "WebBrowserUrlDescr";

		// Token: 0x0400313D RID: 12605
		internal const string WebBrowserUseWaitCursorNotSupported = "WebBrowserUseWaitCursorNotSupported";

		// Token: 0x0400313E RID: 12606
		internal const string WebBrowserWebBrowserShortcutsEnabledDescr = "WebBrowserWebBrowserShortcutsEnabledDescr";

		// Token: 0x0400313F RID: 12607
		internal const string WidthGreaterThanMinWidth = "WidthGreaterThanMinWidth";

		// Token: 0x04003140 RID: 12608
		internal const string Win32WindowAlreadyCreated = "Win32WindowAlreadyCreated";

		// Token: 0x04003141 RID: 12609
		internal const string WindowsFormsSetEvent = "WindowsFormsSetEvent";

		// Token: 0x04003142 RID: 12610
		internal const string ControlOnDpiChangedBeforeParentDescr = "ControlOnDpiChangedBeforeParentDescr";

		// Token: 0x04003143 RID: 12611
		internal const string ControlOnDpiChangedAfterParentDescr = "ControlOnDpiChangedAfterParentDescr";

		// Token: 0x04003144 RID: 12612
		internal const string FormOnDpiChangedDescr = "FormOnDpiChangedDescr";

		// Token: 0x04003145 RID: 12613
		internal const string MDIChildSystemMenuItemAccessibleName = "MDIChildSystemMenuItemAccessibleName";

		// Token: 0x04003146 RID: 12614
		internal const string MonthCalendarSingleDateSelected = "MonthCalendarSingleDateSelected";

		// Token: 0x04003147 RID: 12615
		internal const string MonthCalendarSingleYearSelected = "MonthCalendarSingleYearSelected";

		// Token: 0x04003148 RID: 12616
		internal const string MonthCalendarYearRangeSelected = "MonthCalendarYearRangeSelected";

		// Token: 0x04003149 RID: 12617
		internal const string MonthCalendarSingleDecadeSelected = "MonthCalendarSingleDecadeSelected";

		// Token: 0x0400314A RID: 12618
		internal const string MonthCalendarRangeSelected = "MonthCalendarRangeSelected";

		// Token: 0x0400314B RID: 12619
		internal const string PropertyGridDropDownButtonComboBoxAccessibleName = "PropertyGridDropDownButtonComboBoxAccessibleName";

		// Token: 0x0400314C RID: 12620
		internal const string CombinationOfAccessibilitySwitchesNotSupported = "CombinationOfAccessibilitySwitchesNotSupported";

		// Token: 0x0400314D RID: 12621
		internal const string TrustManagerPromptUI_From = "TrustManagerPromptUI_From";

		// Token: 0x0400314E RID: 12622
		internal const string TrustManagerPromptUI_GlobeIcon = "TrustManagerPromptUI_GlobeIcon";

		// Token: 0x0400314F RID: 12623
		internal const string TrustManagerPromptUI_Name = "TrustManagerPromptUI_Name";

		// Token: 0x04003150 RID: 12624
		internal const string TrustManagerPromptUI_Publisher = "TrustManagerPromptUI_Publisher";

		// Token: 0x04003151 RID: 12625
		internal const string DateTimePickerLocalizedControlType = "DateTimePickerLocalizedControlType";

		// Token: 0x04003152 RID: 12626
		internal const string SpinnerAccessibleName = "SpinnerAccessibleName";

		// Token: 0x04003153 RID: 12627
		internal const string LiveRegionAutomationLiveSettingDescr = "LiveRegionAutomationLiveSettingDescr";

		// Token: 0x04003154 RID: 12628
		internal const string AccessibleObjectLiveRegionNotSupported = "AccessibleObjectLiveRegionNotSupported";

		// Token: 0x04003155 RID: 12629
		internal const string OwnerControlIsNotALiveRegion = "OwnerControlIsNotALiveRegion";

		// Token: 0x04003156 RID: 12630
		internal const string NotSortedAccessibleStatus = "NotSortedAccessibleStatus";

		// Token: 0x04003157 RID: 12631
		internal const string SortedAscendingAccessibleStatus = "SortedAscendingAccessibleStatus";

		// Token: 0x04003158 RID: 12632
		internal const string SortedDescendingAccessibleStatus = "SortedDescendingAccessibleStatus";

		// Token: 0x04003159 RID: 12633
		internal const string DataGridViewSortedAscendingAccessibleStatusFormat = "DataGridViewSortedAscendingAccessibleStatusFormat";

		// Token: 0x0400315A RID: 12634
		internal const string DataGridViewSortedDescendingAccessibleStatusFormat = "DataGridViewSortedDescendingAccessibleStatusFormat";

		// Token: 0x0400315B RID: 12635
		internal const string PropertyGridPropertyValueSelectedFormat = "PropertyGridPropertyValueSelectedFormat";

		// Token: 0x0400315C RID: 12636
		internal const string KeyboardToolTipDisplayBehaviorRequiresAccessibilityImprovementsLevel3 = "KeyboardToolTipDisplayBehaviorRequiresAccessibilityImprovementsLevel3";

		// Token: 0x0400315D RID: 12637
		internal const string ControlDpiChangeScale = "ControlDpiChangeScale";

		// Token: 0x0400315E RID: 12638
		internal const string DataGridViewEditingPanelUiaProviderDescription = "DataGridViewEditingPanelUiaProviderDescription";

		// Token: 0x0400315F RID: 12639
		internal const string ComboBoxValueSelectedFormat = "ComboBoxValueSelectedFormat";

		// Token: 0x04003160 RID: 12640
		private static SR loader;

		// Token: 0x04003161 RID: 12641
		private ResourceManager resources;
	}
}

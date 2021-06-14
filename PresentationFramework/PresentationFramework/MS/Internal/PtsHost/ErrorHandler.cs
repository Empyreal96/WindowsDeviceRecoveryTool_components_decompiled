using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200061D RID: 1565
	internal static class ErrorHandler
	{
		// Token: 0x060067CD RID: 26573 RVA: 0x00002137 File Offset: 0x00000337
		internal static void Assert(bool condition, string message)
		{
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x00002137 File Offset: 0x00000337
		internal static void Assert(bool condition, string format, params object[] args)
		{
		}

		// Token: 0x0400338B RID: 13195
		internal static string PtsCacheAlreadyCreated = "PTS cache is already created.";

		// Token: 0x0400338C RID: 13196
		internal static string PtsCacheAlreadyDestroyed = "PTS cache is already destroyed.";

		// Token: 0x0400338D RID: 13197
		internal static string NullPtsHost = "Valid PtsHost object is required.";

		// Token: 0x0400338E RID: 13198
		internal static string CreateContextFailed = "Failed to create PTS Context.";

		// Token: 0x0400338F RID: 13199
		internal static string EnumIntegrationError = "Some enum values has been changed. Need to update dependent code.";

		// Token: 0x04003390 RID: 13200
		internal static string NoNeedToDestroyPtsPage = "PTS page is not created, there is no need to destroy it.";

		// Token: 0x04003391 RID: 13201
		internal static string NotSupportedFiniteIncremental = "Incremental update is not supported yet.";

		// Token: 0x04003392 RID: 13202
		internal static string NotSupportedFootnotes = "Footnotes are not supported yet.";

		// Token: 0x04003393 RID: 13203
		internal static string NotSupportedCompositeColumns = "Composite columns are not supported yet.";

		// Token: 0x04003394 RID: 13204
		internal static string NotSupportedDropCap = "DropCap is not supported yet.";

		// Token: 0x04003395 RID: 13205
		internal static string NotSupportedForcedLineBreaks = "Forced vertical line break is not supported yet.";

		// Token: 0x04003396 RID: 13206
		internal static string NotSupportedMultiSection = "Multiply sections are not supported yet.";

		// Token: 0x04003397 RID: 13207
		internal static string NotSupportedSubtrackShift = "Column shifting is not supported yet.";

		// Token: 0x04003398 RID: 13208
		internal static string NullObjectInCreateHandle = "Valid object is required to create handle.";

		// Token: 0x04003399 RID: 13209
		internal static string InvalidHandle = "No object associated with the handle or type mismatch.";

		// Token: 0x0400339A RID: 13210
		internal static string HandleOutOfRange = "Object handle has to be within handle array's range.";

		// Token: 0x0400339B RID: 13211
		internal static string BreakRecordDisposed = "Break record already disposed.";

		// Token: 0x0400339C RID: 13212
		internal static string BreakRecordNotNeeded = "There is no need to create break record.";

		// Token: 0x0400339D RID: 13213
		internal static string BrokenParaHasMcs = "Broken paragraph cannot have margin collapsing state.";

		// Token: 0x0400339E RID: 13214
		internal static string BrokenParaHasTopSpace = "Top space should be always suppressed at the top of broken paragraph.";

		// Token: 0x0400339F RID: 13215
		internal static string GoalReachedHasBreakRecord = "Goal is reached, so there should be no break record.";

		// Token: 0x040033A0 RID: 13216
		internal static string BrokenContentRequiresBreakRecord = "Goal is not reached, break record is required to continue.";

		// Token: 0x040033A1 RID: 13217
		internal static string PTSAssert = "PTS Assert:\n\t{0}\n\t{1}\n\t{2}\n\t{3}";

		// Token: 0x040033A2 RID: 13218
		internal static string ParaHandleMismatch = "Paragraph handle mismatch.";

		// Token: 0x040033A3 RID: 13219
		internal static string PTSObjectsCountMismatch = "Actual number of PTS objects does not match number of requested PTS objects.";

		// Token: 0x040033A4 RID: 13220
		internal static string SubmitForEmptyRange = "Submitting embedded objects for empty range.";

		// Token: 0x040033A5 RID: 13221
		internal static string SubmitInvalidList = "Submitting invalid list of embedded objects.";

		// Token: 0x040033A6 RID: 13222
		internal static string HandledInsideSegmentPara = "Paragraph structure invalidation should be handled by Segments.";

		// Token: 0x040033A7 RID: 13223
		internal static string EmptyParagraph = "There are no lines in the paragraph.";

		// Token: 0x040033A8 RID: 13224
		internal static string ParaStartsWithEOP = "NameTable is out of sync with TextContainer. The next paragraph begins with EOP.";

		// Token: 0x040033A9 RID: 13225
		internal static string FetchParaAtTextRangePosition = "Trying to fetch paragraph at not supported TextPointer - TextRange.";

		// Token: 0x040033AA RID: 13226
		internal static string ParagraphCharacterCountMismatch = "Paragraph's character count is out of sync.";

		// Token: 0x040033AB RID: 13227
		internal static string ContainerNeedsTextElement = "Container paragraph can be only created for TextElement.";

		// Token: 0x040033AC RID: 13228
		internal static string CannotPositionInsideUIElement = "Cannot position TextPointer inside a UIElement.";

		// Token: 0x040033AD RID: 13229
		internal static string CannotFindUIElement = "Cannot find specified UIElement in the TextContainer.";

		// Token: 0x040033AE RID: 13230
		internal static string InvalidDocumentPage = "DocumentPage is not created for IDocumentPaginatorSource object.";

		// Token: 0x040033AF RID: 13231
		internal static string NoVisualToTransfer = "Old paragraph does not have a visual node. Cannot transfer data.";

		// Token: 0x040033B0 RID: 13232
		internal static string UpdateShiftedNotValid = "Update shifted is not a valid update type for top level PTS objects.";

		// Token: 0x040033B1 RID: 13233
		internal static string ColumnVisualCountMismatch = "Number of column visuals does not match number of columns.";

		// Token: 0x040033B2 RID: 13234
		internal static string VisualTypeMismatch = "Visual does not match expected type.";

		// Token: 0x040033B3 RID: 13235
		internal static string EmbeddedObjectTypeMismatch = "EmbeddedObject type missmatch.";

		// Token: 0x040033B4 RID: 13236
		internal static string EmbeddedObjectOwnerMismatch = "Cannot transfer data from an embedded object representing another element.";

		// Token: 0x040033B5 RID: 13237
		internal static string LineAlreadyDestroyed = "Line has been already disposed.";

		// Token: 0x040033B6 RID: 13238
		internal static string OnlyOneRectIsExpected = "Expecting only one rect for text object.";

		// Token: 0x040033B7 RID: 13239
		internal static string NotInLineBoundary = "Requesting data outside of line's range.";

		// Token: 0x040033B8 RID: 13240
		internal static string FetchRunAtTextArrayStart = "Trying to fetch run at the beginning of TextContainer.";

		// Token: 0x040033B9 RID: 13241
		internal static string TextFormatterHostNotInitialized = "TextFormatter host is not initialized.";

		// Token: 0x040033BA RID: 13242
		internal static string NegativeCharacterIndex = "Character index must be non-negative.";

		// Token: 0x040033BB RID: 13243
		internal static string NoClientDataForObjectRun = "ClientData should be always provided for object runs.";

		// Token: 0x040033BC RID: 13244
		internal static string UnknownDOTypeInTextArray = "Unknown DependencyObject type stored in TextContainer.";

		// Token: 0x040033BD RID: 13245
		internal static string NegativeObjectWidth = "Negative object's width within a text line.";

		// Token: 0x040033BE RID: 13246
		internal static string NoUIElementForObjectPosition = "TextContainer does not have a UIElement for position of Object type.";

		// Token: 0x040033BF RID: 13247
		internal static string InlineObjectCacheCorrupted = "Paragraph's inline object cache is corrupted.";
	}
}

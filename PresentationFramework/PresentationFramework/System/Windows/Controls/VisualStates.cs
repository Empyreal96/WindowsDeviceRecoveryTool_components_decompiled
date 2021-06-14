using System;

namespace System.Windows.Controls
{
	// Token: 0x02000516 RID: 1302
	internal static class VisualStates
	{
		// Token: 0x06005416 RID: 21526 RVA: 0x00174F94 File Offset: 0x00173194
		public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
		{
			if (stateNames == null)
			{
				return;
			}
			foreach (string stateName in stateNames)
			{
				if (VisualStateManager.GoToState(control, stateName, useTransitions))
				{
					break;
				}
			}
		}

		// Token: 0x04002D23 RID: 11555
		internal const string StateToday = "Today";

		// Token: 0x04002D24 RID: 11556
		internal const string StateRegularDay = "RegularDay";

		// Token: 0x04002D25 RID: 11557
		internal const string GroupDay = "DayStates";

		// Token: 0x04002D26 RID: 11558
		internal const string StateBlackoutDay = "BlackoutDay";

		// Token: 0x04002D27 RID: 11559
		internal const string StateNormalDay = "NormalDay";

		// Token: 0x04002D28 RID: 11560
		internal const string GroupBlackout = "BlackoutDayStates";

		// Token: 0x04002D29 RID: 11561
		public const string StateCalendarButtonUnfocused = "CalendarButtonUnfocused";

		// Token: 0x04002D2A RID: 11562
		public const string StateCalendarButtonFocused = "CalendarButtonFocused";

		// Token: 0x04002D2B RID: 11563
		public const string GroupCalendarButtonFocus = "CalendarButtonFocusStates";

		// Token: 0x04002D2C RID: 11564
		public const string StateNormal = "Normal";

		// Token: 0x04002D2D RID: 11565
		public const string StateMouseOver = "MouseOver";

		// Token: 0x04002D2E RID: 11566
		public const string StatePressed = "Pressed";

		// Token: 0x04002D2F RID: 11567
		public const string StateDisabled = "Disabled";

		// Token: 0x04002D30 RID: 11568
		public const string StateReadOnly = "ReadOnly";

		// Token: 0x04002D31 RID: 11569
		internal const string StateDeterminate = "Determinate";

		// Token: 0x04002D32 RID: 11570
		public const string GroupCommon = "CommonStates";

		// Token: 0x04002D33 RID: 11571
		public const string StateUnfocused = "Unfocused";

		// Token: 0x04002D34 RID: 11572
		public const string StateFocused = "Focused";

		// Token: 0x04002D35 RID: 11573
		public const string StateFocusedDropDown = "FocusedDropDown";

		// Token: 0x04002D36 RID: 11574
		public const string GroupFocus = "FocusStates";

		// Token: 0x04002D37 RID: 11575
		public const string StateExpanded = "Expanded";

		// Token: 0x04002D38 RID: 11576
		public const string StateCollapsed = "Collapsed";

		// Token: 0x04002D39 RID: 11577
		public const string GroupExpansion = "ExpansionStates";

		// Token: 0x04002D3A RID: 11578
		public const string StateOpen = "Open";

		// Token: 0x04002D3B RID: 11579
		public const string StateClosed = "Closed";

		// Token: 0x04002D3C RID: 11580
		public const string GroupOpen = "OpenStates";

		// Token: 0x04002D3D RID: 11581
		public const string StateHasItems = "HasItems";

		// Token: 0x04002D3E RID: 11582
		public const string StateNoItems = "NoItems";

		// Token: 0x04002D3F RID: 11583
		public const string GroupHasItems = "HasItemsStates";

		// Token: 0x04002D40 RID: 11584
		public const string StateExpandDown = "ExpandDown";

		// Token: 0x04002D41 RID: 11585
		public const string StateExpandUp = "ExpandUp";

		// Token: 0x04002D42 RID: 11586
		public const string StateExpandLeft = "ExpandLeft";

		// Token: 0x04002D43 RID: 11587
		public const string StateExpandRight = "ExpandRight";

		// Token: 0x04002D44 RID: 11588
		public const string GroupExpandDirection = "ExpandDirectionStates";

		// Token: 0x04002D45 RID: 11589
		public const string StateSelected = "Selected";

		// Token: 0x04002D46 RID: 11590
		public const string StateSelectedUnfocused = "SelectedUnfocused";

		// Token: 0x04002D47 RID: 11591
		public const string StateSelectedInactive = "SelectedInactive";

		// Token: 0x04002D48 RID: 11592
		public const string StateUnselected = "Unselected";

		// Token: 0x04002D49 RID: 11593
		public const string GroupSelection = "SelectionStates";

		// Token: 0x04002D4A RID: 11594
		public const string StateEditable = "Editable";

		// Token: 0x04002D4B RID: 11595
		public const string StateUneditable = "Uneditable";

		// Token: 0x04002D4C RID: 11596
		public const string GroupEdit = "EditStates";

		// Token: 0x04002D4D RID: 11597
		public const string StateActive = "Active";

		// Token: 0x04002D4E RID: 11598
		public const string StateInactive = "Inactive";

		// Token: 0x04002D4F RID: 11599
		public const string GroupActive = "ActiveStates";

		// Token: 0x04002D50 RID: 11600
		public const string StateValid = "Valid";

		// Token: 0x04002D51 RID: 11601
		public const string StateInvalidFocused = "InvalidFocused";

		// Token: 0x04002D52 RID: 11602
		public const string StateInvalidUnfocused = "InvalidUnfocused";

		// Token: 0x04002D53 RID: 11603
		public const string GroupValidation = "ValidationStates";

		// Token: 0x04002D54 RID: 11604
		public const string StateUnwatermarked = "Unwatermarked";

		// Token: 0x04002D55 RID: 11605
		public const string StateWatermarked = "Watermarked";

		// Token: 0x04002D56 RID: 11606
		public const string GroupWatermark = "WatermarkStates";

		// Token: 0x04002D57 RID: 11607
		public const string StateChecked = "Checked";

		// Token: 0x04002D58 RID: 11608
		public const string StateUnchecked = "Unchecked";

		// Token: 0x04002D59 RID: 11609
		public const string StateIndeterminate = "Indeterminate";

		// Token: 0x04002D5A RID: 11610
		public const string GroupCheck = "CheckStates";

		// Token: 0x04002D5B RID: 11611
		public const string StateRegular = "Regular";

		// Token: 0x04002D5C RID: 11612
		public const string StateCurrent = "Current";

		// Token: 0x04002D5D RID: 11613
		public const string GroupCurrent = "CurrentStates";

		// Token: 0x04002D5E RID: 11614
		public const string StateDisplay = "Display";

		// Token: 0x04002D5F RID: 11615
		public const string StateEditing = "Editing";

		// Token: 0x04002D60 RID: 11616
		public const string GroupInteraction = "InteractionStates";

		// Token: 0x04002D61 RID: 11617
		public const string StateUnsorted = "Unsorted";

		// Token: 0x04002D62 RID: 11618
		public const string StateSortAscending = "SortAscending";

		// Token: 0x04002D63 RID: 11619
		public const string StateSortDescending = "SortDescending";

		// Token: 0x04002D64 RID: 11620
		public const string GroupSort = "SortStates";

		// Token: 0x04002D65 RID: 11621
		public const string DATAGRIDROW_stateAlternate = "Normal_AlternatingRow";

		// Token: 0x04002D66 RID: 11622
		public const string DATAGRIDROW_stateMouseOver = "MouseOver";

		// Token: 0x04002D67 RID: 11623
		public const string DATAGRIDROW_stateMouseOverEditing = "MouseOver_Unfocused_Editing";

		// Token: 0x04002D68 RID: 11624
		public const string DATAGRIDROW_stateMouseOverEditingFocused = "MouseOver_Editing";

		// Token: 0x04002D69 RID: 11625
		public const string DATAGRIDROW_stateMouseOverSelected = "MouseOver_Unfocused_Selected";

		// Token: 0x04002D6A RID: 11626
		public const string DATAGRIDROW_stateMouseOverSelectedFocused = "MouseOver_Selected";

		// Token: 0x04002D6B RID: 11627
		public const string DATAGRIDROW_stateNormal = "Normal";

		// Token: 0x04002D6C RID: 11628
		public const string DATAGRIDROW_stateNormalEditing = "Unfocused_Editing";

		// Token: 0x04002D6D RID: 11629
		public const string DATAGRIDROW_stateNormalEditingFocused = "Normal_Editing";

		// Token: 0x04002D6E RID: 11630
		public const string DATAGRIDROW_stateSelected = "Unfocused_Selected";

		// Token: 0x04002D6F RID: 11631
		public const string DATAGRIDROW_stateSelectedFocused = "Normal_Selected";

		// Token: 0x04002D70 RID: 11632
		public const string DATAGRIDROWHEADER_stateMouseOver = "MouseOver";

		// Token: 0x04002D71 RID: 11633
		public const string DATAGRIDROWHEADER_stateMouseOverCurrentRow = "MouseOver_CurrentRow";

		// Token: 0x04002D72 RID: 11634
		public const string DATAGRIDROWHEADER_stateMouseOverEditingRow = "MouseOver_Unfocused_EditingRow";

		// Token: 0x04002D73 RID: 11635
		public const string DATAGRIDROWHEADER_stateMouseOverEditingRowFocused = "MouseOver_EditingRow";

		// Token: 0x04002D74 RID: 11636
		public const string DATAGRIDROWHEADER_stateMouseOverSelected = "MouseOver_Unfocused_Selected";

		// Token: 0x04002D75 RID: 11637
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRow = "MouseOver_Unfocused_CurrentRow_Selected";

		// Token: 0x04002D76 RID: 11638
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedCurrentRowFocused = "MouseOver_CurrentRow_Selected";

		// Token: 0x04002D77 RID: 11639
		public const string DATAGRIDROWHEADER_stateMouseOverSelectedFocused = "MouseOver_Selected";

		// Token: 0x04002D78 RID: 11640
		public const string DATAGRIDROWHEADER_stateNormal = "Normal";

		// Token: 0x04002D79 RID: 11641
		public const string DATAGRIDROWHEADER_stateNormalCurrentRow = "Normal_CurrentRow";

		// Token: 0x04002D7A RID: 11642
		public const string DATAGRIDROWHEADER_stateNormalEditingRow = "Unfocused_EditingRow";

		// Token: 0x04002D7B RID: 11643
		public const string DATAGRIDROWHEADER_stateNormalEditingRowFocused = "Normal_EditingRow";

		// Token: 0x04002D7C RID: 11644
		public const string DATAGRIDROWHEADER_stateSelected = "Unfocused_Selected";

		// Token: 0x04002D7D RID: 11645
		public const string DATAGRIDROWHEADER_stateSelectedCurrentRow = "Unfocused_CurrentRow_Selected";

		// Token: 0x04002D7E RID: 11646
		public const string DATAGRIDROWHEADER_stateSelectedCurrentRowFocused = "Normal_CurrentRow_Selected";

		// Token: 0x04002D7F RID: 11647
		public const string DATAGRIDROWHEADER_stateSelectedFocused = "Normal_Selected";
	}
}

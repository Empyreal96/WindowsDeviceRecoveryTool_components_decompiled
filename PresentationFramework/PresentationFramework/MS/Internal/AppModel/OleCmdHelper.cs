using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace MS.Internal.AppModel
{
	// Token: 0x02000792 RID: 1938
	internal sealed class OleCmdHelper : MarshalByRefObject
	{
		// Token: 0x060079B6 RID: 31158 RVA: 0x00227DF9 File Offset: 0x00225FF9
		internal OleCmdHelper()
		{
		}

		// Token: 0x060079B7 RID: 31159 RVA: 0x00227E04 File Offset: 0x00226004
		[SecurityCritical]
		internal void QueryStatus(Guid guidCmdGroup, uint cmdId, ref uint flags)
		{
			if (Application.Current == null || Application.IsShuttingDown)
			{
				Marshal.ThrowExceptionForHR(-2147467259);
			}
			IDictionary oleCmdMappingTable = this.GetOleCmdMappingTable(guidCmdGroup);
			if (oleCmdMappingTable == null)
			{
				Marshal.ThrowExceptionForHR(-2147221244);
			}
			CommandWithArgument commandWithArgument = oleCmdMappingTable[cmdId] as CommandWithArgument;
			if (commandWithArgument == null)
			{
				flags = 0U;
				return;
			}
			flags = (((bool)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.QueryEnabled), commandWithArgument)) ? 3U : 1U);
		}

		// Token: 0x060079B8 RID: 31160 RVA: 0x00227E84 File Offset: 0x00226084
		[SecurityCritical]
		private object QueryEnabled(object command)
		{
			if (Application.Current.MainWindow == null)
			{
				return false;
			}
			IInputElement inputElement = FocusManager.GetFocusedElement(Application.Current.MainWindow);
			if (inputElement == null)
			{
				inputElement = Application.Current.MainWindow;
			}
			return BooleanBoxes.Box(((CommandWithArgument)command).QueryEnabled(inputElement, null));
		}

		// Token: 0x060079B9 RID: 31161 RVA: 0x00227ED4 File Offset: 0x002260D4
		[SecurityCritical]
		internal void ExecCommand(Guid guidCmdGroup, uint commandId, object arg)
		{
			if (Application.Current == null || Application.IsShuttingDown)
			{
				Marshal.ThrowExceptionForHR(-2147467259);
			}
			int num = (int)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.ExecCommandCallback), new object[]
			{
				guidCmdGroup,
				commandId,
				arg
			});
			if (num < 0)
			{
				Marshal.ThrowExceptionForHR(num);
			}
		}

		// Token: 0x060079BA RID: 31162 RVA: 0x00227F44 File Offset: 0x00226144
		[SecurityCritical]
		private object ExecCommandCallback(object arguments)
		{
			object[] array = (object[])arguments;
			Invariant.Assert(array.Length == 3);
			Guid guidCmdGroup = (Guid)array[0];
			uint num = (uint)array[1];
			object argument = array[2];
			IDictionary oleCmdMappingTable = this.GetOleCmdMappingTable(guidCmdGroup);
			if (oleCmdMappingTable == null)
			{
				return -2147221244;
			}
			CommandWithArgument commandWithArgument = oleCmdMappingTable[num] as CommandWithArgument;
			if (commandWithArgument == null)
			{
				return -2147221248;
			}
			if (Application.Current.MainWindow == null)
			{
				return -2147221247;
			}
			IInputElement inputElement = FocusManager.GetFocusedElement(Application.Current.MainWindow);
			if (inputElement == null)
			{
				inputElement = Application.Current.MainWindow;
			}
			return commandWithArgument.Execute(inputElement, argument) ? 0 : -2147221247;
		}

		// Token: 0x060079BB RID: 31163 RVA: 0x00228008 File Offset: 0x00226208
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private IDictionary GetOleCmdMappingTable(Guid guidCmdGroup)
		{
			IDictionary result = null;
			if (guidCmdGroup.Equals(OleCmdHelper.CGID_ApplicationCommands))
			{
				this.EnsureApplicationCommandsTable();
				result = this._applicationCommandsMappingTable.Value;
			}
			else if (guidCmdGroup.Equals(Guid.Empty))
			{
				this.EnsureOleCmdMappingTable();
				result = this._oleCmdMappingTable.Value;
			}
			else if (guidCmdGroup.Equals(OleCmdHelper.CGID_EditingCommands))
			{
				this.EnsureEditingCommandsTable();
				result = this._editingCommandsMappingTable.Value;
			}
			return result;
		}

		// Token: 0x060079BC RID: 31164 RVA: 0x0022807C File Offset: 0x0022627C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EnsureOleCmdMappingTable()
		{
			if (this._oleCmdMappingTable.Value == null)
			{
				this._oleCmdMappingTable.Value = new SortedList(10);
				this._oleCmdMappingTable.Value.Add(3U, new CommandWithArgument(ApplicationCommands.Save));
				this._oleCmdMappingTable.Value.Add(4U, new CommandWithArgument(ApplicationCommands.SaveAs));
				this._oleCmdMappingTable.Value.Add(6U, new CommandWithArgument(ApplicationCommands.Print));
				this._oleCmdMappingTable.Value.Add(11U, new CommandWithArgument(ApplicationCommands.Cut));
				this._oleCmdMappingTable.Value.Add(12U, new CommandWithArgument(ApplicationCommands.Copy));
				this._oleCmdMappingTable.Value.Add(13U, new CommandWithArgument(ApplicationCommands.Paste));
				this._oleCmdMappingTable.Value.Add(10U, new CommandWithArgument(ApplicationCommands.Properties));
				this._oleCmdMappingTable.Value.Add(22U, new CommandWithArgument(NavigationCommands.Refresh));
				this._oleCmdMappingTable.Value.Add(23U, new CommandWithArgument(NavigationCommands.BrowseStop));
			}
		}

		// Token: 0x060079BD RID: 31165 RVA: 0x002281D4 File Offset: 0x002263D4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EnsureApplicationCommandsTable()
		{
			if (this._applicationCommandsMappingTable.Value == null)
			{
				this._applicationCommandsMappingTable.Value = new Hashtable(19);
				this._applicationCommandsMappingTable.Value.Add(8001U, new CommandWithArgument(ApplicationCommands.Cut));
				this._applicationCommandsMappingTable.Value.Add(8002U, new CommandWithArgument(ApplicationCommands.Copy));
				this._applicationCommandsMappingTable.Value.Add(8003U, new CommandWithArgument(ApplicationCommands.Paste));
				this._applicationCommandsMappingTable.Value.Add(8004U, new CommandWithArgument(ApplicationCommands.SelectAll));
				this._applicationCommandsMappingTable.Value.Add(8005U, new CommandWithArgument(ApplicationCommands.Find));
				this._applicationCommandsMappingTable.Value.Add(8016U, new CommandWithArgument(NavigationCommands.Refresh));
				this._applicationCommandsMappingTable.Value.Add(8015U, new CommandWithArgument(NavigationCommands.BrowseStop));
				this._applicationCommandsMappingTable.Value.Add(8007U, new CommandWithArgument(DocumentApplicationDocumentViewer.Sign));
				this._applicationCommandsMappingTable.Value.Add(8008U, new CommandWithArgument(DocumentApplicationDocumentViewer.RequestSigners));
				this._applicationCommandsMappingTable.Value.Add(8009U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowSignatureSummary));
				this._applicationCommandsMappingTable.Value.Add(8011U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMPublishingUI));
				this._applicationCommandsMappingTable.Value.Add(8012U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMPermissions));
				this._applicationCommandsMappingTable.Value.Add(8013U, new CommandWithArgument(DocumentApplicationDocumentViewer.ShowRMCredentialManager));
				this._applicationCommandsMappingTable.Value.Add(8019U, new CommandWithArgument(NavigationCommands.IncreaseZoom));
				this._applicationCommandsMappingTable.Value.Add(8020U, new CommandWithArgument(NavigationCommands.DecreaseZoom));
				this._applicationCommandsMappingTable.Value.Add(8021U, new CommandWithArgument(NavigationCommands.Zoom, 400));
				this._applicationCommandsMappingTable.Value.Add(8022U, new CommandWithArgument(NavigationCommands.Zoom, 250));
				this._applicationCommandsMappingTable.Value.Add(8023U, new CommandWithArgument(NavigationCommands.Zoom, 150));
				this._applicationCommandsMappingTable.Value.Add(8024U, new CommandWithArgument(NavigationCommands.Zoom, 100));
				this._applicationCommandsMappingTable.Value.Add(8025U, new CommandWithArgument(NavigationCommands.Zoom, 75));
				this._applicationCommandsMappingTable.Value.Add(8026U, new CommandWithArgument(NavigationCommands.Zoom, 50));
				this._applicationCommandsMappingTable.Value.Add(8027U, new CommandWithArgument(NavigationCommands.Zoom, 25));
				this._applicationCommandsMappingTable.Value.Add(8028U, new CommandWithArgument(DocumentViewer.FitToWidthCommand));
				this._applicationCommandsMappingTable.Value.Add(8029U, new CommandWithArgument(DocumentViewer.FitToHeightCommand));
				this._applicationCommandsMappingTable.Value.Add(8030U, new CommandWithArgument(DocumentViewer.FitToMaxPagesAcrossCommand, 2));
				this._applicationCommandsMappingTable.Value.Add(8031U, new CommandWithArgument(DocumentViewer.ViewThumbnailsCommand));
			}
		}

		// Token: 0x060079BE RID: 31166 RVA: 0x002285EC File Offset: 0x002267EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void EnsureEditingCommandsTable()
		{
			if (this._editingCommandsMappingTable.Value == null)
			{
				this._editingCommandsMappingTable.Value = new SortedList(2);
				this._editingCommandsMappingTable.Value.Add(1U, new CommandWithArgument(EditingCommands.Backspace));
				this._editingCommandsMappingTable.Value.Add(2U, new CommandWithArgument(EditingCommands.Delete));
			}
		}

		// Token: 0x04003992 RID: 14738
		internal const int OLECMDERR_E_NOTSUPPORTED = -2147221248;

		// Token: 0x04003993 RID: 14739
		internal const int OLECMDERR_E_DISABLED = -2147221247;

		// Token: 0x04003994 RID: 14740
		internal const int OLECMDERR_E_UNKNOWNGROUP = -2147221244;

		// Token: 0x04003995 RID: 14741
		internal const uint CommandUnsupported = 0U;

		// Token: 0x04003996 RID: 14742
		internal const uint CommandEnabled = 3U;

		// Token: 0x04003997 RID: 14743
		internal const uint CommandDisabled = 1U;

		// Token: 0x04003998 RID: 14744
		internal static readonly Guid CGID_ApplicationCommands = new Guid(3955001955U, 34137, 18578, 151, 168, 49, 233, 176, 233, 133, 145);

		// Token: 0x04003999 RID: 14745
		internal static readonly Guid CGID_EditingCommands = new Guid(209178181, 3356, 20266, 178, 147, 237, 213, 226, 126, 186, 71);

		// Token: 0x0400399A RID: 14746
		private SecurityCriticalDataForSet<SortedList> _oleCmdMappingTable;

		// Token: 0x0400399B RID: 14747
		private SecurityCriticalDataForSet<Hashtable> _applicationCommandsMappingTable;

		// Token: 0x0400399C RID: 14748
		private SecurityCriticalDataForSet<SortedList> _editingCommandsMappingTable;
	}
}

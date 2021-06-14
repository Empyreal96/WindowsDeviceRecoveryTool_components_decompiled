using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Displays a message window, also known as a dialog box, which presents a message to the user. It is a modal window, blocking other actions in the application until the user closes it. A <see cref="T:System.Windows.Forms.MessageBox" /> can contain text, buttons, and symbols that inform and instruct the user.</summary>
	// Token: 0x020002E9 RID: 745
	public class MessageBox
	{
		// Token: 0x06002CF5 RID: 11509 RVA: 0x000027DB File Offset: 0x000009DB
		private MessageBox()
		{
		}

		// Token: 0x06002CF6 RID: 11510 RVA: 0x000D0EE4 File Offset: 0x000CF0E4
		private static DialogResult Win32ToDialogResult(int value)
		{
			switch (value)
			{
			case 1:
				return DialogResult.OK;
			case 2:
				return DialogResult.Cancel;
			case 3:
				return DialogResult.Abort;
			case 4:
				return DialogResult.Retry;
			case 5:
				return DialogResult.Ignore;
			case 6:
				return DialogResult.Yes;
			case 7:
				return DialogResult.No;
			default:
				return DialogResult.No;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x000D0F1B File Offset: 0x000CF11B
		internal static HelpInfo HelpInfo
		{
			get
			{
				if (MessageBox.helpInfoTable != null && MessageBox.helpInfoTable.Length != 0)
				{
					return MessageBox.helpInfoTable[MessageBox.helpInfoTable.Length - 1];
				}
				return null;
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x000D0F40 File Offset: 0x000CF140
		private static void PopHelpInfo()
		{
			if (MessageBox.helpInfoTable != null)
			{
				if (MessageBox.helpInfoTable.Length == 1)
				{
					MessageBox.helpInfoTable = null;
					return;
				}
				int num = MessageBox.helpInfoTable.Length - 1;
				HelpInfo[] destinationArray = new HelpInfo[num];
				Array.Copy(MessageBox.helpInfoTable, destinationArray, num);
				MessageBox.helpInfoTable = destinationArray;
			}
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x000D0F88 File Offset: 0x000CF188
		private static void PushHelpInfo(HelpInfo hpi)
		{
			int num = 0;
			HelpInfo[] array;
			if (MessageBox.helpInfoTable == null)
			{
				array = new HelpInfo[num + 1];
			}
			else
			{
				num = MessageBox.helpInfoTable.Length;
				array = new HelpInfo[num + 1];
				Array.Copy(MessageBox.helpInfoTable, array, num);
			}
			array[num] = hpi;
			MessageBox.helpInfoTable = array;
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="displayHelpButton">
		///       <see langword="true" /> to show the Help button; otherwise, <see langword="false" />. The default is <see langword="false" />. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFA RID: 11514 RVA: 0x000D0FD0 File Offset: 0x000CF1D0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool displayHelpButton)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, displayHelpButton);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFB RID: 11515 RVA: 0x000D0FE4 File Offset: 0x000CF1E4
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFC RID: 11516 RVA: 0x000D1008 File Offset: 0x000CF208
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file and Help keyword.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="keyword">The Help keyword to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFD RID: 11517 RVA: 0x000D1030 File Offset: 0x000CF230
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, keyword);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file and Help keyword.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="keyword">The Help keyword to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFE RID: 11518 RVA: 0x000D1058 File Offset: 0x000CF258
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, string keyword)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, keyword);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file and <see langword="HelpNavigator" />.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002CFF RID: 11519 RVA: 0x000D1080 File Offset: 0x000CF280
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, navigator);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file and <see langword="HelpNavigator" />.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002D00 RID: 11520 RVA: 0x000D10A8 File Offset: 0x000CF2A8
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, navigator);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file, <see langword="HelpNavigator" />, and Help topic.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <param name="param">The numeric ID of the Help topic to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002D01 RID: 11521 RVA: 0x000D10D0 File Offset: 0x000CF2D0
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, navigator, param);
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, options, and Help button, using the specified Help file, <see langword="HelpNavigator" />, and Help topic.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <param name="helpFilePath">The path and name of the Help file to display when the user clicks the Help button. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <param name="param">The numeric ID of the Help topic to display when the user clicks the Help button. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002D02 RID: 11522 RVA: 0x000D10F8 File Offset: 0x000CF2F8
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, object param)
		{
			HelpInfo hpi = new HelpInfo(helpFilePath, navigator, param);
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, hpi);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, default button, and options.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- The <paramref name="defaultButton" /> specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002D03 RID: 11523 RVA: 0x000D1121 File Offset: 0x000CF321
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, options, false);
		}

		/// <summary>Displays a message box with the specified text, caption, buttons, icon, and default button.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- 
		///         <paramref name="defaultButton" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D04 RID: 11524 RVA: 0x000D1132 File Offset: 0x000CF332
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box with specified text, caption, buttons, and icon.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <paramref name="buttons" /> parameter specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- The <paramref name="icon" /> parameter specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D05 RID: 11525 RVA: 0x000D1142 File Offset: 0x000CF342
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box with specified text, caption, and buttons.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The <paramref name="buttons" /> parameter specified is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D06 RID: 11526 RVA: 0x000D1151 File Offset: 0x000CF351
		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
		{
			return MessageBox.ShowCore(null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box with specified text and caption.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x06002D07 RID: 11527 RVA: 0x000D1160 File Offset: 0x000CF360
		public static DialogResult Show(string text, string caption)
		{
			return MessageBox.ShowCore(null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box with specified text.</summary>
		/// <param name="text">The text to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x06002D08 RID: 11528 RVA: 0x000D116F File Offset: 0x000CF36F
		public static DialogResult Show(string text)
		{
			return MessageBox.ShowCore(null, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text, caption, buttons, icon, default button, and options.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values the specifies the default button for the message box. </param>
		/// <param name="options">One of the <see cref="T:System.Windows.Forms.MessageBoxOptions" /> values that specifies which display and association options will be used for the message box. You may pass in 0 if you wish to use the defaults.</param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- 
		///         <paramref name="defaultButton" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="options" /> specified both <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> and <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" />.-or- 
		///         <paramref name="options" /> specified <see cref="F:System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly" /> or <see cref="F:System.Windows.Forms.MessageBoxOptions.ServiceNotification" /> and specified a value in the <paramref name="owner" /> parameter. These two options should be used only if you invoke the version of this method that does not take an <paramref name="owner" /> parameter.-or- 
		///         <paramref name="buttons" /> specified an invalid combination of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		// Token: 0x06002D09 RID: 11529 RVA: 0x000D1182 File Offset: 0x000CF382
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text, caption, buttons, icon, and default button.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <param name="defaultButton">One of the <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" /> values that specifies the default button for the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />.-or- 
		///         <paramref name="defaultButton" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxDefaultButton" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D0A RID: 11530 RVA: 0x000D1194 File Offset: 0x000CF394
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text, caption, buttons, and icon.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <param name="icon">One of the <see cref="T:System.Windows.Forms.MessageBoxIcon" /> values that specifies which icon to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />.-or- 
		///         <paramref name="icon" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxIcon" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D0B RID: 11531 RVA: 0x000D11A5 File Offset: 0x000CF3A5
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text, caption, and buttons.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <param name="buttons">One of the <see cref="T:System.Windows.Forms.MessageBoxButtons" /> values that specifies which buttons to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="buttons" /> is not a member of <see cref="T:System.Windows.Forms.MessageBoxButtons" />. </exception>
		/// <exception cref="T:System.InvalidOperationException">An attempt was made to display the <see cref="T:System.Windows.Forms.MessageBox" /> in a process that is not running in User Interactive mode. This is specified by the <see cref="P:System.Windows.Forms.SystemInformation.UserInteractive" /> property. </exception>
		// Token: 0x06002D0C RID: 11532 RVA: 0x000D11B5 File Offset: 0x000CF3B5
		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
		{
			return MessageBox.ShowCore(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text and caption.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box.</param>
		/// <param name="text">The text to display in the message box. </param>
		/// <param name="caption">The text to display in the title bar of the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x06002D0D RID: 11533 RVA: 0x000D11C4 File Offset: 0x000CF3C4
		public static DialogResult Show(IWin32Window owner, string text, string caption)
		{
			return MessageBox.ShowCore(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		/// <summary>Displays a message box in front of the specified object and with the specified text.</summary>
		/// <param name="owner">An implementation of <see cref="T:System.Windows.Forms.IWin32Window" /> that will own the modal dialog box. </param>
		/// <param name="text">The text to display in the message box. </param>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DialogResult" /> values.</returns>
		// Token: 0x06002D0E RID: 11534 RVA: 0x000D11D3 File Offset: 0x000CF3D3
		public static DialogResult Show(IWin32Window owner, string text)
		{
			return MessageBox.ShowCore(owner, text, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0, false);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000D11E8 File Offset: 0x000CF3E8
		private static DialogResult ShowCore(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, HelpInfo hpi)
		{
			DialogResult result = DialogResult.None;
			try
			{
				MessageBox.PushHelpInfo(hpi);
				result = MessageBox.ShowCore(owner, text, caption, buttons, icon, defaultButton, options, true);
			}
			finally
			{
				MessageBox.PopHelpInfo();
			}
			return result;
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000D1228 File Offset: 0x000CF428
		private static DialogResult ShowCore(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, bool showHelp)
		{
			if (!ClientUtils.IsEnumValid(buttons, (int)buttons, 0, 5))
			{
				throw new InvalidEnumArgumentException("buttons", (int)buttons, typeof(MessageBoxButtons));
			}
			if (!WindowsFormsUtils.EnumValidator.IsEnumWithinShiftedRange(icon, 4, 0, 4))
			{
				throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxIcon));
			}
			if (!WindowsFormsUtils.EnumValidator.IsEnumWithinShiftedRange(defaultButton, 8, 0, 2))
			{
				throw new InvalidEnumArgumentException("defaultButton", (int)defaultButton, typeof(DialogResult));
			}
			if (!SystemInformation.UserInteractive && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == (MessageBoxOptions)0)
			{
				throw new InvalidOperationException(SR.GetString("CantShowModalOnNonInteractive"));
			}
			if (owner != null && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != (MessageBoxOptions)0)
			{
				throw new ArgumentException(SR.GetString("CantShowMBServiceWithOwner"), "options");
			}
			if (showHelp && (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != (MessageBoxOptions)0)
			{
				throw new ArgumentException(SR.GetString("CantShowMBServiceWithHelp"), "options");
			}
			if ((options & ~(MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading)) != (MessageBoxOptions)0)
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			IntSecurity.SafeSubWindows.Demand();
			int num = showHelp ? 16384 : 0;
			num |= (int)(buttons | (MessageBoxButtons)icon | (MessageBoxButtons)defaultButton | (MessageBoxButtons)options);
			IntPtr handle = IntPtr.Zero;
			if (showHelp || (options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) == (MessageBoxOptions)0)
			{
				if (owner == null)
				{
					handle = UnsafeNativeMethods.GetActiveWindow();
				}
				else
				{
					handle = Control.GetSafeHandle(owner);
				}
			}
			IntPtr userCookie = IntPtr.Zero;
			if (Application.UseVisualStyles)
			{
				if (UnsafeNativeMethods.GetModuleHandle("shell32.dll") == IntPtr.Zero && UnsafeNativeMethods.LoadLibraryFromSystemPathIfAvailable("shell32.dll") == IntPtr.Zero)
				{
					int lastWin32Error = Marshal.GetLastWin32Error();
					throw new Win32Exception(lastWin32Error, SR.GetString("LoadDLLError", new object[]
					{
						"shell32.dll"
					}));
				}
				userCookie = UnsafeNativeMethods.ThemingScope.Activate();
			}
			Application.BeginModalMessageLoop();
			DialogResult result;
			try
			{
				result = MessageBox.Win32ToDialogResult(SafeNativeMethods.MessageBox(new HandleRef(owner, handle), text, caption, num));
			}
			finally
			{
				Application.EndModalMessageLoop();
				UnsafeNativeMethods.ThemingScope.Deactivate(userCookie);
			}
			UnsafeNativeMethods.SendMessage(new HandleRef(owner, handle), 7, 0, 0);
			return result;
		}

		// Token: 0x0400132C RID: 4908
		private const int IDOK = 1;

		// Token: 0x0400132D RID: 4909
		private const int IDCANCEL = 2;

		// Token: 0x0400132E RID: 4910
		private const int IDABORT = 3;

		// Token: 0x0400132F RID: 4911
		private const int IDRETRY = 4;

		// Token: 0x04001330 RID: 4912
		private const int IDIGNORE = 5;

		// Token: 0x04001331 RID: 4913
		private const int IDYES = 6;

		// Token: 0x04001332 RID: 4914
		private const int IDNO = 7;

		// Token: 0x04001333 RID: 4915
		private const int HELP_BUTTON = 16384;

		// Token: 0x04001334 RID: 4916
		[ThreadStatic]
		private static HelpInfo[] helpInfoTable;
	}
}

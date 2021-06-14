using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows
{
	/// <summary>Displays a message box. </summary>
	// Token: 0x020000D8 RID: 216
	public sealed class MessageBox
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x0000326D File Offset: 0x0000146D
		private MessageBox()
		{
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x000174EF File Offset: 0x000156EF
		private static MessageBoxResult Win32ToMessageBoxResult(int value)
		{
			switch (value)
			{
			case 1:
				return MessageBoxResult.OK;
			case 2:
				return MessageBoxResult.Cancel;
			case 6:
				return MessageBoxResult.Yes;
			case 7:
				return MessageBoxResult.No;
			}
			return MessageBoxResult.No;
		}

		/// <summary>Displays a message box that has a message, title bar caption, button, and icon; and that accepts a default message box result, complies with the specified options, and returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <param name="defaultResult">A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies the default result of the message box.</param>
		/// <param name="options">A <see cref="T:System.Windows.MessageBoxOptions" /> value object that specifies the options.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077A RID: 1914 RVA: 0x00017520 File Offset: 0x00015720
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, options);
		}

		/// <summary>Displays a message box that has a message, title bar caption, button, and icon; and that accepts a default message box result and returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <param name="defaultResult">A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies the default result of the message box.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077B RID: 1915 RVA: 0x00017534 File Offset: 0x00015734
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077C RID: 1916 RVA: 0x00017547 File Offset: 0x00015747
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box that has a message, title bar caption, and button; and that returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077D RID: 1917 RVA: 0x00017559 File Offset: 0x00015759
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box that has a message and title bar caption; and that returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077E RID: 1918 RVA: 0x0001756B File Offset: 0x0001576B
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText, string caption)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box that has a message and that returns a result.</summary>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x0600077F RID: 1919 RVA: 0x0001757D File Offset: 0x0001577D
		[SecurityCritical]
		public static MessageBoxResult Show(string messageBoxText)
		{
			return MessageBox.ShowCore(IntPtr.Zero, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message, title bar caption, button, and icon; and accepts a default message box result, complies with the specified options, and returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <param name="defaultResult">A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies the default result of the message box.</param>
		/// <param name="options">A <see cref="T:System.Windows.MessageBoxOptions" /> value object that specifies the options.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000780 RID: 1920 RVA: 0x00017593 File Offset: 0x00015793
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, defaultResult, options);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message, title bar caption, button, and icon; and accepts a default message box result and returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <param name="defaultResult">A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies the default result of the message box.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000781 RID: 1921 RVA: 0x000175AE File Offset: 0x000157AE
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, defaultResult, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message, title bar caption, button, and icon; and it also returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <param name="icon">A <see cref="T:System.Windows.MessageBoxImage" /> value that specifies the icon to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000782 RID: 1922 RVA: 0x000175C8 File Offset: 0x000157C8
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, icon, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message, title bar caption, and button; and it also returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <param name="button">A <see cref="T:System.Windows.MessageBoxButton" /> value that specifies which button or buttons to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000783 RID: 1923 RVA: 0x000175E1 File Offset: 0x000157E1
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption, MessageBoxButton button)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, button, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message and title bar caption; and it returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <param name="caption">A <see cref="T:System.String" /> that specifies the title bar caption to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000784 RID: 1924 RVA: 0x000175F9 File Offset: 0x000157F9
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		/// <summary>Displays a message box in front of the specified window. The message box displays a message and returns a result.</summary>
		/// <param name="owner">A <see cref="T:System.Windows.Window" /> that represents the owner window of the message box.</param>
		/// <param name="messageBoxText">A <see cref="T:System.String" /> that specifies the text to display.</param>
		/// <returns>A <see cref="T:System.Windows.MessageBoxResult" /> value that specifies which message box button is clicked by the user.</returns>
		// Token: 0x06000785 RID: 1925 RVA: 0x00017611 File Offset: 0x00015811
		[SecurityCritical]
		public static MessageBoxResult Show(Window owner, string messageBoxText)
		{
			return MessageBox.ShowCore(new WindowInteropHelper(owner).CriticalHandle, messageBoxText, string.Empty, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, MessageBoxOptions.None);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00017630 File Offset: 0x00015830
		private static int DefaultResultToButtonNumber(MessageBoxResult result, MessageBoxButton button)
		{
			if (result == MessageBoxResult.None)
			{
				return 0;
			}
			switch (button)
			{
			case MessageBoxButton.OK:
				return 0;
			case MessageBoxButton.OKCancel:
				if (result == MessageBoxResult.Cancel)
				{
					return 256;
				}
				return 0;
			case MessageBoxButton.YesNoCancel:
				if (result == MessageBoxResult.No)
				{
					return 256;
				}
				if (result == MessageBoxResult.Cancel)
				{
					return 512;
				}
				return 0;
			case MessageBoxButton.YesNo:
				if (result == MessageBoxResult.No)
				{
					return 256;
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00017690 File Offset: 0x00015890
		[SecurityCritical]
		internal static MessageBoxResult ShowCore(IntPtr owner, string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon, MessageBoxResult defaultResult, MessageBoxOptions options)
		{
			if (!MessageBox.IsValidMessageBoxButton(button))
			{
				throw new InvalidEnumArgumentException("button", (int)button, typeof(MessageBoxButton));
			}
			if (!MessageBox.IsValidMessageBoxImage(icon))
			{
				throw new InvalidEnumArgumentException("icon", (int)icon, typeof(MessageBoxImage));
			}
			if (!MessageBox.IsValidMessageBoxResult(defaultResult))
			{
				throw new InvalidEnumArgumentException("defaultResult", (int)defaultResult, typeof(MessageBoxResult));
			}
			if (!MessageBox.IsValidMessageBoxOptions(options))
			{
				throw new InvalidEnumArgumentException("options", (int)options, typeof(MessageBoxOptions));
			}
			if ((options & (MessageBoxOptions.ServiceNotification | MessageBoxOptions.DefaultDesktopOnly)) != MessageBoxOptions.None)
			{
				SecurityHelper.DemandUnmanagedCode();
				if (owner != IntPtr.Zero)
				{
					throw new ArgumentException(SR.Get("CantShowMBServiceWithOwner"));
				}
			}
			else if (owner == IntPtr.Zero)
			{
				owner = UnsafeNativeMethods.GetActiveWindow();
			}
			int type = (int)(button | (MessageBoxButton)icon | (MessageBoxButton)MessageBox.DefaultResultToButtonNumber(defaultResult, button) | (MessageBoxButton)options);
			return MessageBox.Win32ToMessageBoxResult(UnsafeNativeMethods.MessageBox(new HandleRef(null, owner), messageBoxText, caption, type));
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00017782 File Offset: 0x00015982
		private static bool IsValidMessageBoxButton(MessageBoxButton value)
		{
			return value == MessageBoxButton.OK || value == MessageBoxButton.OKCancel || value == MessageBoxButton.YesNo || value == MessageBoxButton.YesNoCancel;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00017795 File Offset: 0x00015995
		private static bool IsValidMessageBoxImage(MessageBoxImage value)
		{
			return value == MessageBoxImage.Asterisk || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation || value == MessageBoxImage.Hand || value == MessageBoxImage.Asterisk || value == MessageBoxImage.None || value == MessageBoxImage.Question || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000177C4 File Offset: 0x000159C4
		private static bool IsValidMessageBoxResult(MessageBoxResult value)
		{
			return value == MessageBoxResult.Cancel || value == MessageBoxResult.No || value == MessageBoxResult.None || value == MessageBoxResult.OK || value == MessageBoxResult.Yes;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000177DC File Offset: 0x000159DC
		private static bool IsValidMessageBoxOptions(MessageBoxOptions value)
		{
			int num = -3801089;
			return (value & (MessageBoxOptions)num) == MessageBoxOptions.None;
		}

		// Token: 0x0400073B RID: 1851
		private const int IDOK = 1;

		// Token: 0x0400073C RID: 1852
		private const int IDCANCEL = 2;

		// Token: 0x0400073D RID: 1853
		private const int IDABORT = 3;

		// Token: 0x0400073E RID: 1854
		private const int IDRETRY = 4;

		// Token: 0x0400073F RID: 1855
		private const int IDIGNORE = 5;

		// Token: 0x04000740 RID: 1856
		private const int IDYES = 6;

		// Token: 0x04000741 RID: 1857
		private const int IDNO = 7;

		// Token: 0x04000742 RID: 1858
		private const int DEFAULT_BUTTON1 = 0;

		// Token: 0x04000743 RID: 1859
		private const int DEFAULT_BUTTON2 = 256;

		// Token: 0x04000744 RID: 1860
		private const int DEFAULT_BUTTON3 = 512;
	}
}

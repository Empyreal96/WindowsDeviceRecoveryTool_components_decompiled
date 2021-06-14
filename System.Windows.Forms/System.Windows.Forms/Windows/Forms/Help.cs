using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Encapsulates the HTML Help 1.0 engine.</summary>
	// Token: 0x02000260 RID: 608
	public class Help
	{
		// Token: 0x060024AC RID: 9388 RVA: 0x000027DB File Offset: 0x000009DB
		private Help()
		{
		}

		/// <summary>Displays the contents of the Help file at the specified URL.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="url">The path and name of the Help file. </param>
		// Token: 0x060024AD RID: 9389 RVA: 0x000B1EB6 File Offset: 0x000B00B6
		public static void ShowHelp(Control parent, string url)
		{
			Help.ShowHelp(parent, url, HelpNavigator.TableOfContents, null);
		}

		/// <summary>Displays the contents of the Help file found at the specified URL for a specific topic.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="url">The path and name of the Help file. </param>
		/// <param name="navigator">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		// Token: 0x060024AE RID: 9390 RVA: 0x000B1EC5 File Offset: 0x000B00C5
		public static void ShowHelp(Control parent, string url, HelpNavigator navigator)
		{
			Help.ShowHelp(parent, url, navigator, null);
		}

		/// <summary>Displays the contents of the Help file found at the specified URL for a specific keyword.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="url">The path and name of the Help file. </param>
		/// <param name="keyword">The keyword to display Help for. </param>
		// Token: 0x060024AF RID: 9391 RVA: 0x000B1ED0 File Offset: 0x000B00D0
		public static void ShowHelp(Control parent, string url, string keyword)
		{
			if (keyword != null && keyword.Length != 0)
			{
				Help.ShowHelp(parent, url, HelpNavigator.Topic, keyword);
				return;
			}
			Help.ShowHelp(parent, url, HelpNavigator.TableOfContents, null);
		}

		/// <summary>Displays the contents of the Help file located at the URL supplied by the user.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="url">The path and name of the Help file. </param>
		/// <param name="command">One of the <see cref="T:System.Windows.Forms.HelpNavigator" /> values. </param>
		/// <param name="parameter">A string that contains the topic identifier.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="parameter" /> is an integer.</exception>
		// Token: 0x060024B0 RID: 9392 RVA: 0x000B1EF8 File Offset: 0x000B00F8
		public static void ShowHelp(Control parent, string url, HelpNavigator command, object parameter)
		{
			int helpFileType = Help.GetHelpFileType(url);
			if (helpFileType == 2)
			{
				Help.ShowHTML10Help(parent, url, command, parameter);
				return;
			}
			if (helpFileType != 3)
			{
				return;
			}
			Help.ShowHTMLFile(parent, url, command, parameter);
		}

		/// <summary>Displays the index of the specified Help file.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="url">The path and name of the Help file. </param>
		// Token: 0x060024B1 RID: 9393 RVA: 0x000B1F28 File Offset: 0x000B0128
		public static void ShowHelpIndex(Control parent, string url)
		{
			Help.ShowHelp(parent, url, HelpNavigator.Index, null);
		}

		/// <summary>Displays a Help pop-up window.</summary>
		/// <param name="parent">A <see cref="T:System.Windows.Forms.Control" /> that identifies the parent of the Help dialog box. </param>
		/// <param name="caption">The message to display in the pop-up window. </param>
		/// <param name="location">A value that specifies the horizontal and vertical coordinates at which to display the pop-up window, relative to the upper-left corner of the screen. </param>
		// Token: 0x060024B2 RID: 9394 RVA: 0x000B1F38 File Offset: 0x000B0138
		public static void ShowPopup(Control parent, string caption, Point location)
		{
			NativeMethods.HH_POPUP hh_POPUP = new NativeMethods.HH_POPUP();
			IntPtr intPtr = Marshal.StringToCoTaskMemAuto(caption);
			try
			{
				hh_POPUP.pszText = intPtr;
				hh_POPUP.idString = 0;
				hh_POPUP.pt = new NativeMethods.POINT(location.X, location.Y);
				hh_POPUP.clrBackground = (Color.FromKnownColor(KnownColor.Window).ToArgb() & 16777215);
				Help.ShowHTML10Help(parent, null, HelpNavigator.Topic, hh_POPUP);
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000B1FBC File Offset: 0x000B01BC
		private static void ShowHTML10Help(Control parent, string url, HelpNavigator command, object param)
		{
			IntSecurity.UnmanagedCode.Demand();
			string pszFile = url;
			Uri uri = Help.Resolve(url);
			if (uri != null)
			{
				pszFile = uri.AbsoluteUri;
			}
			if (uri == null || uri.IsFile)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string lpszLongPath = (uri != null && uri.IsFile) ? uri.LocalPath : url;
				uint shortPathName = UnsafeNativeMethods.GetShortPathName(lpszLongPath, stringBuilder, 0U);
				if (shortPathName > 0U)
				{
					stringBuilder.Capacity = (int)shortPathName;
					shortPathName = UnsafeNativeMethods.GetShortPathName(lpszLongPath, stringBuilder, shortPathName);
					pszFile = stringBuilder.ToString();
				}
			}
			HandleRef hwndCaller;
			if (parent != null)
			{
				hwndCaller = new HandleRef(parent, parent.Handle);
			}
			else
			{
				hwndCaller = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
			}
			string text = param as string;
			if (text != null)
			{
				object obj;
				int uCommand = Help.MapCommandToHTMLCommand(command, text, out obj);
				string text2 = obj as string;
				if (text2 != null)
				{
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, uCommand, text2);
					return;
				}
				if (obj is int)
				{
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, uCommand, (int)obj);
					return;
				}
				if (obj is NativeMethods.HH_FTS_QUERY)
				{
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, uCommand, (NativeMethods.HH_FTS_QUERY)obj);
					return;
				}
				if (obj is NativeMethods.HH_AKLINK)
				{
					SafeNativeMethods.HtmlHelp(NativeMethods.NullHandleRef, pszFile, 0, null);
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, uCommand, (NativeMethods.HH_AKLINK)obj);
					return;
				}
				SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, uCommand, (string)param);
				return;
			}
			else
			{
				if (param == null)
				{
					object obj;
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, Help.MapCommandToHTMLCommand(command, null, out obj), 0);
					return;
				}
				if (param is NativeMethods.HH_POPUP)
				{
					SafeNativeMethods.HtmlHelp(hwndCaller, pszFile, 14, (NativeMethods.HH_POPUP)param);
					return;
				}
				if (param.GetType() == typeof(int))
				{
					throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
					{
						"param",
						"Integer"
					}));
				}
				return;
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000B217C File Offset: 0x000B037C
		private static void ShowHTMLFile(Control parent, string url, HelpNavigator command, object param)
		{
			Uri uri = Help.Resolve(url);
			if (uri == null)
			{
				throw new ArgumentException(SR.GetString("HelpInvalidURL", new object[]
				{
					url
				}), "url");
			}
			string scheme = uri.Scheme;
			if (scheme == "http" || scheme == "https")
			{
				new WebPermission(NetworkAccess.Connect, url).Demand();
			}
			else
			{
				IntSecurity.UnmanagedCode.Demand();
			}
			if (command != HelpNavigator.Topic)
			{
				if (command - HelpNavigator.TableOfContents > 2)
				{
				}
			}
			else if (param != null && param is string)
			{
				uri = new Uri(uri.ToString() + "#" + (string)param);
			}
			HandleRef hwnd;
			if (parent != null)
			{
				hwnd = new HandleRef(parent, parent.Handle);
			}
			else
			{
				hwnd = new HandleRef(null, UnsafeNativeMethods.GetActiveWindow());
			}
			UnsafeNativeMethods.ShellExecute_NoBFM(hwnd, null, uri.ToString(), null, null, 1);
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000B2260 File Offset: 0x000B0460
		private static Uri Resolve(string partialUri)
		{
			Uri uri = null;
			if (!string.IsNullOrEmpty(partialUri))
			{
				try
				{
					uri = new Uri(partialUri);
				}
				catch (UriFormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
			}
			if (uri != null && uri.Scheme == "file")
			{
				string localPath = NativeMethods.GetLocalPath(partialUri);
				new FileIOPermission(FileIOPermissionAccess.Read, localPath).Assert();
				try
				{
					if (!File.Exists(localPath))
					{
						uri = null;
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			if (uri == null)
			{
				try
				{
					uri = new Uri(new Uri(AppDomain.CurrentDomain.SetupInformation.ApplicationBase), partialUri);
				}
				catch (UriFormatException)
				{
				}
				catch (ArgumentNullException)
				{
				}
				if (uri != null && uri.Scheme == "file")
				{
					string path = uri.LocalPath + uri.Fragment;
					new FileIOPermission(FileIOPermissionAccess.Read, path).Assert();
					try
					{
						if (!File.Exists(path))
						{
							uri = null;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			return uri;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000B238C File Offset: 0x000B058C
		private static int GetHelpFileType(string url)
		{
			if (url == null)
			{
				return 3;
			}
			Uri uri = Help.Resolve(url);
			if (uri == null || uri.Scheme == "file")
			{
				string a = Path.GetExtension((uri == null) ? url : (uri.LocalPath + uri.Fragment)).ToLower(CultureInfo.InvariantCulture);
				if (a == ".chm" || a == ".col")
				{
					return 2;
				}
			}
			return 3;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000B240C File Offset: 0x000B060C
		private static int MapCommandToHTMLCommand(HelpNavigator command, string param, out object htmlParam)
		{
			htmlParam = param;
			if (string.IsNullOrEmpty(param) && (command == HelpNavigator.AssociateIndex || command == HelpNavigator.KeywordIndex))
			{
				return 2;
			}
			switch (command)
			{
			case HelpNavigator.Topic:
				return 0;
			case HelpNavigator.TableOfContents:
				return 1;
			case HelpNavigator.Index:
				return 2;
			case HelpNavigator.Find:
				htmlParam = new NativeMethods.HH_FTS_QUERY
				{
					pszSearchQuery = param
				};
				return 3;
			case HelpNavigator.AssociateIndex:
			case HelpNavigator.KeywordIndex:
				break;
			case HelpNavigator.TopicId:
				try
				{
					htmlParam = int.Parse(param, CultureInfo.InvariantCulture);
					return 15;
				}
				catch
				{
					return 2;
				}
				break;
			default:
				return (int)command;
			}
			htmlParam = new NativeMethods.HH_AKLINK
			{
				pszKeywords = param,
				fIndexOnFail = true,
				fReserved = false
			};
			if (command != HelpNavigator.KeywordIndex)
			{
				return 19;
			}
			return 13;
		}

		// Token: 0x04000FB2 RID: 4018
		internal static readonly TraceSwitch WindowsFormsHelpTrace;

		// Token: 0x04000FB3 RID: 4019
		private const int HH_DISPLAY_TOPIC = 0;

		// Token: 0x04000FB4 RID: 4020
		private const int HH_HELP_FINDER = 0;

		// Token: 0x04000FB5 RID: 4021
		private const int HH_DISPLAY_TOC = 1;

		// Token: 0x04000FB6 RID: 4022
		private const int HH_DISPLAY_INDEX = 2;

		// Token: 0x04000FB7 RID: 4023
		private const int HH_DISPLAY_SEARCH = 3;

		// Token: 0x04000FB8 RID: 4024
		private const int HH_SET_WIN_TYPE = 4;

		// Token: 0x04000FB9 RID: 4025
		private const int HH_GET_WIN_TYPE = 5;

		// Token: 0x04000FBA RID: 4026
		private const int HH_GET_WIN_HANDLE = 6;

		// Token: 0x04000FBB RID: 4027
		private const int HH_ENUM_INFO_TYPE = 7;

		// Token: 0x04000FBC RID: 4028
		private const int HH_SET_INFO_TYPE = 8;

		// Token: 0x04000FBD RID: 4029
		private const int HH_SYNC = 9;

		// Token: 0x04000FBE RID: 4030
		private const int HH_ADD_NAV_UI = 10;

		// Token: 0x04000FBF RID: 4031
		private const int HH_ADD_BUTTON = 11;

		// Token: 0x04000FC0 RID: 4032
		private const int HH_GETBROWSER_APP = 12;

		// Token: 0x04000FC1 RID: 4033
		private const int HH_KEYWORD_LOOKUP = 13;

		// Token: 0x04000FC2 RID: 4034
		private const int HH_DISPLAY_TEXT_POPUP = 14;

		// Token: 0x04000FC3 RID: 4035
		private const int HH_HELP_CONTEXT = 15;

		// Token: 0x04000FC4 RID: 4036
		private const int HH_TP_HELP_CONTEXTMENU = 16;

		// Token: 0x04000FC5 RID: 4037
		private const int HH_TP_HELP_WM_HELP = 17;

		// Token: 0x04000FC6 RID: 4038
		private const int HH_CLOSE_ALL = 18;

		// Token: 0x04000FC7 RID: 4039
		private const int HH_ALINK_LOOKUP = 19;

		// Token: 0x04000FC8 RID: 4040
		private const int HH_GET_LAST_ERROR = 20;

		// Token: 0x04000FC9 RID: 4041
		private const int HH_ENUM_CATEGORY = 21;

		// Token: 0x04000FCA RID: 4042
		private const int HH_ENUM_CATEGORY_IT = 22;

		// Token: 0x04000FCB RID: 4043
		private const int HH_RESET_IT_FILTER = 23;

		// Token: 0x04000FCC RID: 4044
		private const int HH_SET_INCLUSIVE_FILTER = 24;

		// Token: 0x04000FCD RID: 4045
		private const int HH_SET_EXCLUSIVE_FILTER = 25;

		// Token: 0x04000FCE RID: 4046
		private const int HH_SET_GUID = 26;

		// Token: 0x04000FCF RID: 4047
		private const int HTML10HELP = 2;

		// Token: 0x04000FD0 RID: 4048
		private const int HTMLFILE = 3;
	}
}

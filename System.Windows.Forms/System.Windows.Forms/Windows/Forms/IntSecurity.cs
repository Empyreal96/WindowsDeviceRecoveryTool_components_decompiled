using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000437 RID: 1079
	internal static class IntSecurity
	{
		// Token: 0x1700124D RID: 4685
		// (get) Token: 0x06004B0F RID: 19215 RVA: 0x00136028 File Offset: 0x00134228
		public static CodeAccessPermission AdjustCursorClip
		{
			get
			{
				if (IntSecurity.adjustCursorClip == null)
				{
					IntSecurity.adjustCursorClip = IntSecurity.AllWindows;
				}
				return IntSecurity.adjustCursorClip;
			}
		}

		// Token: 0x1700124E RID: 4686
		// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00136040 File Offset: 0x00134240
		public static CodeAccessPermission AdjustCursorPosition
		{
			get
			{
				return IntSecurity.AllWindows;
			}
		}

		// Token: 0x1700124F RID: 4687
		// (get) Token: 0x06004B11 RID: 19217 RVA: 0x00136047 File Offset: 0x00134247
		public static CodeAccessPermission AffectMachineState
		{
			get
			{
				if (IntSecurity.affectMachineState == null)
				{
					IntSecurity.affectMachineState = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.affectMachineState;
			}
		}

		// Token: 0x17001250 RID: 4688
		// (get) Token: 0x06004B12 RID: 19218 RVA: 0x0013605F File Offset: 0x0013425F
		public static CodeAccessPermission AffectThreadBehavior
		{
			get
			{
				if (IntSecurity.affectThreadBehavior == null)
				{
					IntSecurity.affectThreadBehavior = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.affectThreadBehavior;
			}
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06004B13 RID: 19219 RVA: 0x00136077 File Offset: 0x00134277
		public static CodeAccessPermission AllPrinting
		{
			get
			{
				if (IntSecurity.allPrinting == null)
				{
					IntSecurity.allPrinting = new PrintingPermission(PrintingPermissionLevel.AllPrinting);
				}
				return IntSecurity.allPrinting;
			}
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06004B14 RID: 19220 RVA: 0x00136090 File Offset: 0x00134290
		public static PermissionSet AllPrintingAndUnmanagedCode
		{
			get
			{
				if (IntSecurity.allPrintingAndUnmanagedCode == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.SetPermission(IntSecurity.UnmanagedCode);
					permissionSet.SetPermission(IntSecurity.AllPrinting);
					IntSecurity.allPrintingAndUnmanagedCode = permissionSet;
				}
				return IntSecurity.allPrintingAndUnmanagedCode;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x001360CE File Offset: 0x001342CE
		public static CodeAccessPermission AllWindows
		{
			get
			{
				if (IntSecurity.allWindows == null)
				{
					IntSecurity.allWindows = new UIPermission(UIPermissionWindow.AllWindows);
				}
				return IntSecurity.allWindows;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06004B16 RID: 19222 RVA: 0x001360E7 File Offset: 0x001342E7
		public static CodeAccessPermission ClipboardRead
		{
			get
			{
				if (IntSecurity.clipboardRead == null)
				{
					IntSecurity.clipboardRead = new UIPermission(UIPermissionClipboard.AllClipboard);
				}
				return IntSecurity.clipboardRead;
			}
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06004B17 RID: 19223 RVA: 0x00136100 File Offset: 0x00134300
		public static CodeAccessPermission ClipboardOwn
		{
			get
			{
				if (IntSecurity.clipboardOwn == null)
				{
					IntSecurity.clipboardOwn = new UIPermission(UIPermissionClipboard.OwnClipboard);
				}
				return IntSecurity.clipboardOwn;
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06004B18 RID: 19224 RVA: 0x00136119 File Offset: 0x00134319
		public static PermissionSet ClipboardWrite
		{
			get
			{
				if (IntSecurity.clipboardWrite == null)
				{
					IntSecurity.clipboardWrite = new PermissionSet(PermissionState.None);
					IntSecurity.clipboardWrite.SetPermission(IntSecurity.UnmanagedCode);
					IntSecurity.clipboardWrite.SetPermission(IntSecurity.ClipboardOwn);
				}
				return IntSecurity.clipboardWrite;
			}
		}

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00136152 File Offset: 0x00134352
		public static CodeAccessPermission ChangeWindowRegionForTopLevel
		{
			get
			{
				if (IntSecurity.changeWindowRegionForTopLevel == null)
				{
					IntSecurity.changeWindowRegionForTopLevel = IntSecurity.AllWindows;
				}
				return IntSecurity.changeWindowRegionForTopLevel;
			}
		}

		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x0013616A File Offset: 0x0013436A
		public static CodeAccessPermission ControlFromHandleOrLocation
		{
			get
			{
				if (IntSecurity.controlFromHandleOrLocation == null)
				{
					IntSecurity.controlFromHandleOrLocation = IntSecurity.AllWindows;
				}
				return IntSecurity.controlFromHandleOrLocation;
			}
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x00136182 File Offset: 0x00134382
		public static CodeAccessPermission CreateAnyWindow
		{
			get
			{
				if (IntSecurity.createAnyWindow == null)
				{
					IntSecurity.createAnyWindow = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.createAnyWindow;
			}
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x0013619A File Offset: 0x0013439A
		public static CodeAccessPermission CreateGraphicsForControl
		{
			get
			{
				if (IntSecurity.createGraphicsForControl == null)
				{
					IntSecurity.createGraphicsForControl = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.createGraphicsForControl;
			}
		}

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06004B1D RID: 19229 RVA: 0x001361B2 File Offset: 0x001343B2
		public static CodeAccessPermission DefaultPrinting
		{
			get
			{
				if (IntSecurity.defaultPrinting == null)
				{
					IntSecurity.defaultPrinting = new PrintingPermission(PrintingPermissionLevel.DefaultPrinting);
				}
				return IntSecurity.defaultPrinting;
			}
		}

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x001361CB File Offset: 0x001343CB
		public static CodeAccessPermission FileDialogCustomization
		{
			get
			{
				if (IntSecurity.fileDialogCustomization == null)
				{
					IntSecurity.fileDialogCustomization = new FileIOPermission(PermissionState.Unrestricted);
				}
				return IntSecurity.fileDialogCustomization;
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x001361E4 File Offset: 0x001343E4
		public static CodeAccessPermission FileDialogOpenFile
		{
			get
			{
				if (IntSecurity.fileDialogOpenFile == null)
				{
					IntSecurity.fileDialogOpenFile = new FileDialogPermission(FileDialogPermissionAccess.Open);
				}
				return IntSecurity.fileDialogOpenFile;
			}
		}

		// Token: 0x1700125E RID: 4702
		// (get) Token: 0x06004B20 RID: 19232 RVA: 0x001361FD File Offset: 0x001343FD
		public static CodeAccessPermission FileDialogSaveFile
		{
			get
			{
				if (IntSecurity.fileDialogSaveFile == null)
				{
					IntSecurity.fileDialogSaveFile = new FileDialogPermission(FileDialogPermissionAccess.Save);
				}
				return IntSecurity.fileDialogSaveFile;
			}
		}

		// Token: 0x1700125F RID: 4703
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x00136216 File Offset: 0x00134416
		public static CodeAccessPermission GetCapture
		{
			get
			{
				if (IntSecurity.getCapture == null)
				{
					IntSecurity.getCapture = IntSecurity.AllWindows;
				}
				return IntSecurity.getCapture;
			}
		}

		// Token: 0x17001260 RID: 4704
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x0013622E File Offset: 0x0013442E
		public static CodeAccessPermission GetParent
		{
			get
			{
				if (IntSecurity.getParent == null)
				{
					IntSecurity.getParent = IntSecurity.AllWindows;
				}
				return IntSecurity.getParent;
			}
		}

		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06004B23 RID: 19235 RVA: 0x00136246 File Offset: 0x00134446
		public static CodeAccessPermission ManipulateWndProcAndHandles
		{
			get
			{
				if (IntSecurity.manipulateWndProcAndHandles == null)
				{
					IntSecurity.manipulateWndProcAndHandles = IntSecurity.AllWindows;
				}
				return IntSecurity.manipulateWndProcAndHandles;
			}
		}

		// Token: 0x17001262 RID: 4706
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x0013625E File Offset: 0x0013445E
		public static CodeAccessPermission ModifyCursor
		{
			get
			{
				if (IntSecurity.modifyCursor == null)
				{
					IntSecurity.modifyCursor = IntSecurity.SafeSubWindows;
				}
				return IntSecurity.modifyCursor;
			}
		}

		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06004B25 RID: 19237 RVA: 0x00136276 File Offset: 0x00134476
		public static CodeAccessPermission ModifyFocus
		{
			get
			{
				if (IntSecurity.modifyFocus == null)
				{
					IntSecurity.modifyFocus = IntSecurity.AllWindows;
				}
				return IntSecurity.modifyFocus;
			}
		}

		// Token: 0x17001264 RID: 4708
		// (get) Token: 0x06004B26 RID: 19238 RVA: 0x0013628E File Offset: 0x0013448E
		public static CodeAccessPermission ObjectFromWin32Handle
		{
			get
			{
				if (IntSecurity.objectFromWin32Handle == null)
				{
					IntSecurity.objectFromWin32Handle = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.objectFromWin32Handle;
			}
		}

		// Token: 0x17001265 RID: 4709
		// (get) Token: 0x06004B27 RID: 19239 RVA: 0x001362A6 File Offset: 0x001344A6
		public static CodeAccessPermission SafePrinting
		{
			get
			{
				if (IntSecurity.safePrinting == null)
				{
					IntSecurity.safePrinting = new PrintingPermission(PrintingPermissionLevel.SafePrinting);
				}
				return IntSecurity.safePrinting;
			}
		}

		// Token: 0x17001266 RID: 4710
		// (get) Token: 0x06004B28 RID: 19240 RVA: 0x001362BF File Offset: 0x001344BF
		public static CodeAccessPermission SafeSubWindows
		{
			get
			{
				if (IntSecurity.safeSubWindows == null)
				{
					IntSecurity.safeSubWindows = new UIPermission(UIPermissionWindow.SafeSubWindows);
				}
				return IntSecurity.safeSubWindows;
			}
		}

		// Token: 0x17001267 RID: 4711
		// (get) Token: 0x06004B29 RID: 19241 RVA: 0x001362D8 File Offset: 0x001344D8
		public static CodeAccessPermission SafeTopLevelWindows
		{
			get
			{
				if (IntSecurity.safeTopLevelWindows == null)
				{
					IntSecurity.safeTopLevelWindows = new UIPermission(UIPermissionWindow.SafeTopLevelWindows);
				}
				return IntSecurity.safeTopLevelWindows;
			}
		}

		// Token: 0x17001268 RID: 4712
		// (get) Token: 0x06004B2A RID: 19242 RVA: 0x001362F1 File Offset: 0x001344F1
		public static CodeAccessPermission SendMessages
		{
			get
			{
				if (IntSecurity.sendMessages == null)
				{
					IntSecurity.sendMessages = IntSecurity.UnmanagedCode;
				}
				return IntSecurity.sendMessages;
			}
		}

		// Token: 0x17001269 RID: 4713
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x00136309 File Offset: 0x00134509
		public static CodeAccessPermission SensitiveSystemInformation
		{
			get
			{
				if (IntSecurity.sensitiveSystemInformation == null)
				{
					IntSecurity.sensitiveSystemInformation = new EnvironmentPermission(PermissionState.Unrestricted);
				}
				return IntSecurity.sensitiveSystemInformation;
			}
		}

		// Token: 0x1700126A RID: 4714
		// (get) Token: 0x06004B2C RID: 19244 RVA: 0x00136322 File Offset: 0x00134522
		public static CodeAccessPermission TransparentWindows
		{
			get
			{
				if (IntSecurity.transparentWindows == null)
				{
					IntSecurity.transparentWindows = IntSecurity.AllWindows;
				}
				return IntSecurity.transparentWindows;
			}
		}

		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0013633A File Offset: 0x0013453A
		public static CodeAccessPermission TopLevelWindow
		{
			get
			{
				if (IntSecurity.topLevelWindow == null)
				{
					IntSecurity.topLevelWindow = IntSecurity.SafeTopLevelWindows;
				}
				return IntSecurity.topLevelWindow;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06004B2E RID: 19246 RVA: 0x00136352 File Offset: 0x00134552
		public static CodeAccessPermission UnmanagedCode
		{
			get
			{
				if (IntSecurity.unmanagedCode == null)
				{
					IntSecurity.unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				}
				return IntSecurity.unmanagedCode;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06004B2F RID: 19247 RVA: 0x0013636B File Offset: 0x0013456B
		public static CodeAccessPermission UnrestrictedWindows
		{
			get
			{
				if (IntSecurity.unrestrictedWindows == null)
				{
					IntSecurity.unrestrictedWindows = IntSecurity.AllWindows;
				}
				return IntSecurity.unrestrictedWindows;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06004B30 RID: 19248 RVA: 0x00136383 File Offset: 0x00134583
		public static CodeAccessPermission WindowAdornmentModification
		{
			get
			{
				if (IntSecurity.windowAdornmentModification == null)
				{
					IntSecurity.windowAdornmentModification = IntSecurity.AllWindows;
				}
				return IntSecurity.windowAdornmentModification;
			}
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0013639C File Offset: 0x0013459C
		internal static string UnsafeGetFullPath(string fileName)
		{
			string result = fileName;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				result = Path.GetFullPath(fileName);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001363E0 File Offset: 0x001345E0
		internal static void DemandFileIO(FileIOPermissionAccess access, string fileName)
		{
			new FileIOPermission(access, IntSecurity.UnsafeGetFullPath(fileName)).Demand();
		}

		// Token: 0x0400275B RID: 10075
		public static readonly TraceSwitch SecurityDemand = new TraceSwitch("SecurityDemand", "Trace when security demands occur.");

		// Token: 0x0400275C RID: 10076
		private static CodeAccessPermission adjustCursorClip;

		// Token: 0x0400275D RID: 10077
		private static CodeAccessPermission affectMachineState;

		// Token: 0x0400275E RID: 10078
		private static CodeAccessPermission affectThreadBehavior;

		// Token: 0x0400275F RID: 10079
		private static CodeAccessPermission allPrinting;

		// Token: 0x04002760 RID: 10080
		private static PermissionSet allPrintingAndUnmanagedCode;

		// Token: 0x04002761 RID: 10081
		private static CodeAccessPermission allWindows;

		// Token: 0x04002762 RID: 10082
		private static CodeAccessPermission clipboardRead;

		// Token: 0x04002763 RID: 10083
		private static CodeAccessPermission clipboardOwn;

		// Token: 0x04002764 RID: 10084
		private static PermissionSet clipboardWrite;

		// Token: 0x04002765 RID: 10085
		private static CodeAccessPermission changeWindowRegionForTopLevel;

		// Token: 0x04002766 RID: 10086
		private static CodeAccessPermission controlFromHandleOrLocation;

		// Token: 0x04002767 RID: 10087
		private static CodeAccessPermission createAnyWindow;

		// Token: 0x04002768 RID: 10088
		private static CodeAccessPermission createGraphicsForControl;

		// Token: 0x04002769 RID: 10089
		private static CodeAccessPermission defaultPrinting;

		// Token: 0x0400276A RID: 10090
		private static CodeAccessPermission fileDialogCustomization;

		// Token: 0x0400276B RID: 10091
		private static CodeAccessPermission fileDialogOpenFile;

		// Token: 0x0400276C RID: 10092
		private static CodeAccessPermission fileDialogSaveFile;

		// Token: 0x0400276D RID: 10093
		private static CodeAccessPermission getCapture;

		// Token: 0x0400276E RID: 10094
		private static CodeAccessPermission getParent;

		// Token: 0x0400276F RID: 10095
		private static CodeAccessPermission manipulateWndProcAndHandles;

		// Token: 0x04002770 RID: 10096
		private static CodeAccessPermission modifyCursor;

		// Token: 0x04002771 RID: 10097
		private static CodeAccessPermission modifyFocus;

		// Token: 0x04002772 RID: 10098
		private static CodeAccessPermission objectFromWin32Handle;

		// Token: 0x04002773 RID: 10099
		private static CodeAccessPermission safePrinting;

		// Token: 0x04002774 RID: 10100
		private static CodeAccessPermission safeSubWindows;

		// Token: 0x04002775 RID: 10101
		private static CodeAccessPermission safeTopLevelWindows;

		// Token: 0x04002776 RID: 10102
		private static CodeAccessPermission sendMessages;

		// Token: 0x04002777 RID: 10103
		private static CodeAccessPermission sensitiveSystemInformation;

		// Token: 0x04002778 RID: 10104
		private static CodeAccessPermission transparentWindows;

		// Token: 0x04002779 RID: 10105
		private static CodeAccessPermission topLevelWindow;

		// Token: 0x0400277A RID: 10106
		private static CodeAccessPermission unmanagedCode;

		// Token: 0x0400277B RID: 10107
		private static CodeAccessPermission unrestrictedWindows;

		// Token: 0x0400277C RID: 10108
		private static CodeAccessPermission windowAdornmentModification;
	}
}

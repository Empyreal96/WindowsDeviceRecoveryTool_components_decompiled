using System;
using System.Runtime.InteropServices;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x02000014 RID: 20
	internal class NativeMethods
	{
		// Token: 0x060000CE RID: 206
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, ref NativeMethods.DeviceInformationData DeviceInfoData);

		// Token: 0x060000CF RID: 207
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr SetupDiGetClassDevs(ref Guid classGuid, string enumerator, IntPtr parent, int flags);

		// Token: 0x060000D0 RID: 208
		[DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x060000D1 RID: 209
		[DllImport("Cfgmgr32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint CM_Get_DevNode_Status(ref uint status, ref uint problemNumber, uint devInst, uint flags);

		// Token: 0x060000D2 RID: 210
		[DllImport("ntdll.dll")]
		public static extern int RtlComputeCrc32(int PartialCrc, IntPtr Buffer, int Length);

		// Token: 0x02000015 RID: 21
		public enum WinError : uint
		{
			// Token: 0x0400004D RID: 77
			Success,
			// Token: 0x0400004E RID: 78
			FileNotFound = 2U,
			// Token: 0x0400004F RID: 79
			NoMoreFiles = 18U,
			// Token: 0x04000050 RID: 80
			NotReady = 21U,
			// Token: 0x04000051 RID: 81
			GeneralFailure = 31U,
			// Token: 0x04000052 RID: 82
			InvalidParameter = 87U,
			// Token: 0x04000053 RID: 83
			InsufficientBuffer = 122U,
			// Token: 0x04000054 RID: 84
			IoPending = 997U,
			// Token: 0x04000055 RID: 85
			DeviceNotConnected = 1167U,
			// Token: 0x04000056 RID: 86
			TimeZoneIdInvalid = 4294967295U,
			// Token: 0x04000057 RID: 87
			InvalidHandleValue = 4294967295U,
			// Token: 0x04000058 RID: 88
			PathNotFound = 3U,
			// Token: 0x04000059 RID: 89
			AlreadyExists = 183U,
			// Token: 0x0400005A RID: 90
			NoMoreItems = 259U
		}

		// Token: 0x02000016 RID: 22
		public enum SetupApiErr : uint
		{
			// Token: 0x0400005C RID: 92
			InWow64 = 3758096949U
		}

		// Token: 0x02000017 RID: 23
		public enum DiFuction : uint
		{
			// Token: 0x0400005E RID: 94
			PropertyChange = 18U
		}

		// Token: 0x02000018 RID: 24
		public enum DICS : uint
		{
			// Token: 0x04000060 RID: 96
			Enable = 1U,
			// Token: 0x04000061 RID: 97
			Disable,
			// Token: 0x04000062 RID: 98
			PropertyChange,
			// Token: 0x04000063 RID: 99
			Start,
			// Token: 0x04000064 RID: 100
			Stop
		}

		// Token: 0x02000019 RID: 25
		[Flags]
		public enum DICSFlags : uint
		{
			// Token: 0x04000066 RID: 102
			Global = 1U,
			// Token: 0x04000067 RID: 103
			ConfigSpecific = 2U,
			// Token: 0x04000068 RID: 104
			ConfigGeneral = 4U
		}

		// Token: 0x0200001A RID: 26
		public enum DIGCF
		{
			// Token: 0x0400006A RID: 106
			Default = 1,
			// Token: 0x0400006B RID: 107
			Present,
			// Token: 0x0400006C RID: 108
			AllClasses = 4,
			// Token: 0x0400006D RID: 109
			Profile = 8,
			// Token: 0x0400006E RID: 110
			DeviceInterface = 16
		}

		// Token: 0x0200001B RID: 27
		[Flags]
		public enum DIDMFlags : uint
		{
			// Token: 0x04000070 RID: 112
			HasProblem = 1024U
		}

		// Token: 0x0200001C RID: 28
		public enum CMPROB : uint
		{
			// Token: 0x04000072 RID: 114
			EntryIsWrongType = 4U
		}

		// Token: 0x0200001D RID: 29
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ClassInstallHeader
		{
			// Token: 0x04000073 RID: 115
			public int Size;

			// Token: 0x04000074 RID: 116
			public NativeMethods.DiFuction InstallFunction;
		}

		// Token: 0x0200001E RID: 30
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PropertyChangeParams
		{
			// Token: 0x04000075 RID: 117
			public NativeMethods.ClassInstallHeader Header;

			// Token: 0x04000076 RID: 118
			public uint StateChange;

			// Token: 0x04000077 RID: 119
			public uint Scope;

			// Token: 0x04000078 RID: 120
			public uint HwProfile;
		}

		// Token: 0x0200001F RID: 31
		public struct DeviceInformationData
		{
			// Token: 0x04000079 RID: 121
			public int Size;

			// Token: 0x0400007A RID: 122
			public Guid ClassGuid;

			// Token: 0x0400007B RID: 123
			public uint DevInst;

			// Token: 0x0400007C RID: 124
			public IntPtr Reserved;
		}
	}
}

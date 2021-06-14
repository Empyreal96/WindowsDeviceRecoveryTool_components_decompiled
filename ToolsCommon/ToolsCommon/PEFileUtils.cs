using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x0200002A RID: 42
	public static class PEFileUtils
	{
		// Token: 0x06000153 RID: 339 RVA: 0x000083D4 File Offset: 0x000065D4
		private static T ReadStruct<T>(BinaryReader br)
		{
			byte[] value = br.ReadBytes(Marshal.SizeOf(typeof(T)));
			GCHandle gchandle = GCHandle.Alloc(value, GCHandleType.Pinned);
			T result = (T)((object)Marshal.PtrToStructure(gchandle.AddrOfPinnedObject(), typeof(T)));
			gchandle.Free();
			return result;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00008424 File Offset: 0x00006624
		public static bool IsPE(string path)
		{
			bool result;
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(path)))
			{
				if (binaryReader.BaseStream.Length < (long)Marshal.SizeOf(typeof(PEFileUtils.IMAGE_DOS_HEADER)))
				{
					result = false;
				}
				else
				{
					PEFileUtils.IMAGE_DOS_HEADER image_DOS_HEADER = PEFileUtils.ReadStruct<PEFileUtils.IMAGE_DOS_HEADER>(binaryReader);
					if (image_DOS_HEADER.e_lfanew < Marshal.SizeOf(typeof(PEFileUtils.IMAGE_DOS_HEADER)))
					{
						result = false;
					}
					else if (image_DOS_HEADER.e_lfanew > 2147483647 - Marshal.SizeOf(typeof(PEFileUtils.IMAGE_NT_HEADERS32)))
					{
						result = false;
					}
					else if (binaryReader.BaseStream.Length < (long)(image_DOS_HEADER.e_lfanew + Marshal.SizeOf(typeof(PEFileUtils.IMAGE_NT_HEADERS32))))
					{
						result = false;
					}
					else
					{
						binaryReader.BaseStream.Seek((long)image_DOS_HEADER.e_lfanew, SeekOrigin.Begin);
						uint num = binaryReader.ReadUInt32();
						if (num != PEFileUtils.c_iPESignature)
						{
							result = false;
						}
						else
						{
							PEFileUtils.ReadStruct<PEFileUtils.IMAGE_FILE_HEADER>(binaryReader);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400007A RID: 122
		private static uint c_iPESignature = 4660U;

		// Token: 0x0200002B RID: 43
		[StructLayout(LayoutKind.Explicit)]
		public struct IMAGE_DOS_HEADER
		{
			// Token: 0x0400007B RID: 123
			[FieldOffset(60)]
			public int e_lfanew;
		}

		// Token: 0x0200002C RID: 44
		public struct IMAGE_FILE_HEADER
		{
			// Token: 0x0400007C RID: 124
			public ushort Machine;

			// Token: 0x0400007D RID: 125
			public ushort NumberOfSections;

			// Token: 0x0400007E RID: 126
			public ulong TimeDateStamp;

			// Token: 0x0400007F RID: 127
			public ulong PointerToSymbolTable;

			// Token: 0x04000080 RID: 128
			public ulong NumberOfSymbols;

			// Token: 0x04000081 RID: 129
			public ushort SizeOfOptionalHeader;

			// Token: 0x04000082 RID: 130
			public ushort Characteristics;
		}

		// Token: 0x0200002D RID: 45
		public struct IMAGE_DATA_DIRECTORY
		{
			// Token: 0x04000083 RID: 131
			public uint VirtualAddress;

			// Token: 0x04000084 RID: 132
			public uint Size;
		}

		// Token: 0x0200002E RID: 46
		public struct IMAGE_OPTIONAL_HEADER32
		{
			// Token: 0x04000085 RID: 133
			public ushort Magic;

			// Token: 0x04000086 RID: 134
			public byte MajorLinkerVersion;

			// Token: 0x04000087 RID: 135
			public byte MinorLinkerVersion;

			// Token: 0x04000088 RID: 136
			public uint SizeOfCode;

			// Token: 0x04000089 RID: 137
			public uint SizeOfInitializedData;

			// Token: 0x0400008A RID: 138
			public uint SizeOfUninitializedData;

			// Token: 0x0400008B RID: 139
			public uint AddressOfEntryPoint;

			// Token: 0x0400008C RID: 140
			public uint BaseOfCode;

			// Token: 0x0400008D RID: 141
			public uint BaseOfData;

			// Token: 0x0400008E RID: 142
			public uint ImageBase;

			// Token: 0x0400008F RID: 143
			public uint SectionAlignment;

			// Token: 0x04000090 RID: 144
			public uint FileAlignment;

			// Token: 0x04000091 RID: 145
			public ushort MajorOperatingSystemVersion;

			// Token: 0x04000092 RID: 146
			public ushort MinorOperatingSystemVersion;

			// Token: 0x04000093 RID: 147
			public ushort MajorImageVersion;

			// Token: 0x04000094 RID: 148
			public ushort MinorImageVersion;

			// Token: 0x04000095 RID: 149
			public ushort MajorSubsystemVersion;

			// Token: 0x04000096 RID: 150
			public ushort MinorSubsystemVersion;

			// Token: 0x04000097 RID: 151
			public uint Win32VersionValue;

			// Token: 0x04000098 RID: 152
			public uint SizeOfImage;

			// Token: 0x04000099 RID: 153
			public uint SizeOfHeaders;

			// Token: 0x0400009A RID: 154
			public uint CheckSum;

			// Token: 0x0400009B RID: 155
			public ushort Subsystem;

			// Token: 0x0400009C RID: 156
			public ushort DllCharacteristics;

			// Token: 0x0400009D RID: 157
			public uint SizeOfStackReserve;

			// Token: 0x0400009E RID: 158
			public uint SizeOfStackCommit;

			// Token: 0x0400009F RID: 159
			public uint SizeOfHeapReserve;

			// Token: 0x040000A0 RID: 160
			public uint SizeOfHeapCommit;

			// Token: 0x040000A1 RID: 161
			public uint LoaderFlags;

			// Token: 0x040000A2 RID: 162
			public uint NumberOfRvaAndSizes;
		}

		// Token: 0x0200002F RID: 47
		public struct IMAGE_OPTIONAL_HEADER64
		{
			// Token: 0x040000A3 RID: 163
			public ushort Magic;

			// Token: 0x040000A4 RID: 164
			public byte MajorLinkerVersion;

			// Token: 0x040000A5 RID: 165
			public byte MinorLinkerVersion;

			// Token: 0x040000A6 RID: 166
			public uint SizeOfCode;

			// Token: 0x040000A7 RID: 167
			public uint SizeOfInitializedData;

			// Token: 0x040000A8 RID: 168
			public uint SizeOfUninitializedData;

			// Token: 0x040000A9 RID: 169
			public uint AddressOfEntryPoint;

			// Token: 0x040000AA RID: 170
			public uint BaseOfCode;

			// Token: 0x040000AB RID: 171
			public ulong ImageBase;

			// Token: 0x040000AC RID: 172
			public uint SectionAlignment;

			// Token: 0x040000AD RID: 173
			public uint FileAlignment;

			// Token: 0x040000AE RID: 174
			public ushort MajorOperatingSystemVersion;

			// Token: 0x040000AF RID: 175
			public ushort MinorOperatingSystemVersion;

			// Token: 0x040000B0 RID: 176
			public ushort MajorImageVersion;

			// Token: 0x040000B1 RID: 177
			public ushort MinorImageVersion;

			// Token: 0x040000B2 RID: 178
			public ushort MajorSubsystemVersion;

			// Token: 0x040000B3 RID: 179
			public ushort MinorSubsystemVersion;

			// Token: 0x040000B4 RID: 180
			public uint Win32VersionValue;

			// Token: 0x040000B5 RID: 181
			public uint SizeOfImage;

			// Token: 0x040000B6 RID: 182
			public uint SizeOfHeaders;

			// Token: 0x040000B7 RID: 183
			public uint CheckSum;

			// Token: 0x040000B8 RID: 184
			public ushort Subsystem;

			// Token: 0x040000B9 RID: 185
			public ushort DllCharacteristics;

			// Token: 0x040000BA RID: 186
			public ulong SizeOfStackReserve;

			// Token: 0x040000BB RID: 187
			public ulong SizeOfStackCommit;

			// Token: 0x040000BC RID: 188
			public ulong SizeOfHeapReserve;

			// Token: 0x040000BD RID: 189
			public ulong SizeOfHeapCommit;

			// Token: 0x040000BE RID: 190
			public uint LoaderFlags;

			// Token: 0x040000BF RID: 191
			public uint NumberOfRvaAndSizes;
		}

		// Token: 0x02000030 RID: 48
		public struct IMAGE_NT_HEADERS32
		{
			// Token: 0x040000C0 RID: 192
			public uint Signature;

			// Token: 0x040000C1 RID: 193
			public PEFileUtils.IMAGE_FILE_HEADER FileHeader;

			// Token: 0x040000C2 RID: 194
			public PEFileUtils.IMAGE_OPTIONAL_HEADER32 OptionalHeader;
		}

		// Token: 0x02000031 RID: 49
		public struct IMAGE_NT_HEADERS64
		{
			// Token: 0x040000C3 RID: 195
			public uint Signature;

			// Token: 0x040000C4 RID: 196
			public PEFileUtils.IMAGE_FILE_HEADER FileHeader;

			// Token: 0x040000C5 RID: 197
			public PEFileUtils.IMAGE_OPTIONAL_HEADER64 OptionalHeader;
		}
	}
}

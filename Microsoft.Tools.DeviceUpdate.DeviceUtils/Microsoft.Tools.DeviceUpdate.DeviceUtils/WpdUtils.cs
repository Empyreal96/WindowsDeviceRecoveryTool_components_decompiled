using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using PortableDeviceApiLib;
using PortableDeviceConstants;
using PortableDeviceTypesLib;

namespace Microsoft.Tools.DeviceUpdate.DeviceUtils
{
	// Token: 0x0200002A RID: 42
	public class WpdUtils
	{
		// Token: 0x06000142 RID: 322 RVA: 0x000116D4 File Offset: 0x0000F8D4
		public static void SetCommand(PortableDeviceApiLib.IPortableDeviceValues wpdValues, _tagpropertykey command)
		{
			try
			{
				Guid fmtid = command.fmtid;
				wpdValues.SetGuidValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_COMMAND_CATEGORY, ref fmtid);
				wpdValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_COMMAND_ID, command.pid);
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00011724 File Offset: 0x0000F924
		public static void SetOperation(PortableDeviceApiLib.IPortableDeviceValues wpdValues, uint mtpOpcode, PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters)
		{
			try
			{
				wpdValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_OPERATION_CODE, mtpOpcode);
				wpdValues.SetIPortableDevicePropVariantCollectionValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS, mtpParameters);
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00011764 File Offset: 0x0000F964
		public static void AddUnsignedIntegerValue(PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters, uint value)
		{
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_OBJECT_ID, value);
				tag_inner_PROPVARIANT tag_inner_PROPVARIANT;
				portableDeviceValues.GetValue(ref PortableDevicePKeys.WPD_OBJECT_ID, out tag_inner_PROPVARIANT);
				tag_inner_PROPVARIANT tag_inner_PROPVARIANT2 = tag_inner_PROPVARIANT;
				mtpParameters.Add(ref tag_inner_PROPVARIANT2);
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000117CC File Offset: 0x0000F9CC
		public static void ExecuteMtpOpcode(IPortableDevice device, uint opcode, out uint hresult, out uint responseCode)
		{
			PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters = (PortableDeviceApiLib.IPortableDevicePropVariantCollection)((PortableDevicePropVariantCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("08A99E2F-6D6D-4B80-AF5A-BAF2BCBE4CB9"))));
			WpdUtils.ExecuteMtpOpcode(device, opcode, mtpParameters, out hresult, out responseCode);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00011804 File Offset: 0x0000FA04
		public static void ExecuteMtpOpcode(IPortableDevice device, uint opcode, PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters, out uint hresult, out uint responseCode)
		{
			try
			{
				hresult = 0U;
				responseCode = 0U;
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues2 = null;
				WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITHOUT_DATA_PHASE);
				WpdUtils.SetOperation(portableDeviceValues, opcode, mtpParameters);
				device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
				int num = 0;
				portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
				hresult = (uint)num;
				if (hresult == 0U)
				{
					portableDeviceValues2.GetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_RESPONSE_CODE, out responseCode);
				}
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00011894 File Offset: 0x0000FA94
		public static void ExecuteMtpOpcodeAndReadData(IPortableDevice device, PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters, uint opCode, Stream stream, out uint hresult, out uint responseCode)
		{
			try
			{
				byte[] array = new byte[1048576];
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				byte[] array2 = new byte[1048576];
				int num = 0;
				hresult = 0U;
				responseCode = 0U;
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues2 = null;
				WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_READ);
				WpdUtils.SetOperation(portableDeviceValues, opCode, mtpParameters);
				device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
				portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
				hresult = (uint)num;
				if (hresult == 0U)
				{
					string value = null;
					portableDeviceValues2.GetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, out value);
					uint num2 = 0U;
					portableDeviceValues2.GetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE, out num2);
					while (num2 > 0U)
					{
						IntPtr source = 0;
						uint num3 = Math.Min(num2, (uint)array.Length);
						uint num4 = 0U;
						portableDeviceValues.Clear();
						portableDeviceValues2 = null;
						WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_READ_DATA);
						portableDeviceValues.SetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, value);
						portableDeviceValues.SetBufferValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, gchandle.AddrOfPinnedObject(), num3);
						portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_READ, num3);
						device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
						portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
						hresult = (uint)num;
						if (hresult != 0U)
						{
							return;
						}
						portableDeviceValues2.GetBufferValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, out source, out num4);
						Marshal.Copy(source, array2, 0, (int)num4);
						stream.Write(array2, 0, (int)num4);
						num2 -= num4;
					}
					portableDeviceValues.Clear();
					portableDeviceValues2 = null;
					WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER);
					portableDeviceValues.SetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, value);
					device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
					portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
					hresult = (uint)num;
					if (hresult == 0U)
					{
						portableDeviceValues2.GetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_RESPONSE_CODE, out responseCode);
					}
				}
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00011A84 File Offset: 0x0000FC84
		public static void ExecuteMtpOpcodeAndWriteData(IPortableDevice device, PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpParameters, uint opCode, Stream stream, uint streamSize, out uint hresult, out uint responseCode, out PortableDeviceApiLib.IPortableDevicePropVariantCollection mtpResponseParameters)
		{
			try
			{
				byte[] array = new byte[1048576];
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				int num = 0;
				hresult = 0U;
				responseCode = 0U;
				mtpResponseParameters = null;
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues2 = null;
				WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_WRITE);
				WpdUtils.SetOperation(portableDeviceValues, opCode, mtpParameters);
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE, streamSize);
				device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
				portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
				hresult = (uint)num;
				if (hresult == 0U)
				{
					string value = null;
					portableDeviceValues2.GetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, out value);
					while (streamSize > 0U)
					{
						uint num2 = Math.Min(streamSize, (uint)array.Length);
						uint num3 = 0U;
						portableDeviceValues.Clear();
						portableDeviceValues2 = null;
						stream.Read(array, 0, (int)num2);
						WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_WRITE_DATA);
						portableDeviceValues.SetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, value);
						portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_WRITE, num2);
						portableDeviceValues.SetBufferValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, gchandle.AddrOfPinnedObject(), num2);
						device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
						portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
						hresult = (uint)num;
						if (hresult != 0U)
						{
							return;
						}
						portableDeviceValues2.GetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_WRITTEN, out num3);
						if (num2 != num3)
						{
							hresult = 2147500037U;
							return;
						}
						streamSize -= num3;
					}
					portableDeviceValues.Clear();
					portableDeviceValues2 = null;
					WpdUtils.SetCommand(portableDeviceValues, PortableDevicePKeys.WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER);
					portableDeviceValues.SetStringValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, value);
					device.SendCommand(0U, portableDeviceValues, out portableDeviceValues2);
					portableDeviceValues2.GetErrorValue(ref PortableDevicePKeys.WPD_PROPERTY_COMMON_HRESULT, out num);
					hresult = (uint)num;
					if (hresult == 0U)
					{
						portableDeviceValues2.GetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_RESPONSE_CODE, out responseCode);
						portableDeviceValues2.GetIPortableDevicePropVariantCollectionValue(ref PortableDevicePKeys.WPD_PROPERTY_MTP_EXT_RESPONSE_PARAMS, out mtpResponseParameters);
					}
				}
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00011C64 File Offset: 0x0000FE64
		public static void GetDeviceService(IPortableDeviceManager wpdManager, string deviceId, string serviceName, out IPortableDeviceService wpdService)
		{
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = (PortableDeviceApiLib.IPortableDeviceValues)((PortableDeviceValues)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("0C15D503-D017-47CE-9016-7B3F978721CC"))));
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				Version version = executingAssembly.GetName().Version;
				wpdService = null;
				portableDeviceValues.SetStringValue(ref PortableDevicePKeys.WPD_CLIENT_NAME, executingAssembly.FullName);
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_CLIENT_MAJOR_VERSION, (uint)version.Major);
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_CLIENT_MINOR_VERSION, (uint)version.Minor);
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_CLIENT_REVISION, (uint)version.Revision);
				portableDeviceValues.SetUnsignedIntegerValue(ref PortableDevicePKeys.WPD_CLIENT_SECURITY_QUALITY_OF_SERVICE, 131072U);
				IPortableDeviceServiceManager portableDeviceServiceManager = (IPortableDeviceServiceManager)wpdManager;
				uint num = 0U;
				portableDeviceServiceManager.GetDeviceServices(deviceId, ref PortableDeviceGuids.GUID_DEVINTERFACE_WPD_SERVICE, null, ref num);
				string[] array = new string[num];
				portableDeviceServiceManager.GetDeviceServices(deviceId, ref PortableDeviceGuids.GUID_DEVINTERFACE_WPD_SERVICE, array, ref num);
				foreach (string pszPnPServiceID in array)
				{
					IPortableDeviceService portableDeviceService = (PortableDeviceService)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("EF5DB4C2-9312-422C-9152-411CD9C4DD84")));
					portableDeviceService.Open(pszPnPServiceID, portableDeviceValues);
					string pszObjectID = null;
					portableDeviceService.GetServiceObjectID(out pszObjectID);
					IPortableDeviceContent2 portableDeviceContent = null;
					portableDeviceService.Content(out portableDeviceContent);
					IPortableDeviceProperties portableDeviceProperties = null;
					portableDeviceContent.Properties(out portableDeviceProperties);
					PortableDeviceApiLib.IPortableDeviceKeyCollection portableDeviceKeyCollection = (PortableDeviceApiLib.IPortableDeviceKeyCollection)((PortableDeviceKeyCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DE2D022D-2480-43BE-97F0-D1FA2CF98F4F"))));
					portableDeviceKeyCollection.Add(ref PortableDevicePKeys.WPD_OBJECT_NAME);
					PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues2 = null;
					portableDeviceProperties.GetValues(pszObjectID, portableDeviceKeyCollection, out portableDeviceValues2);
					string a = null;
					portableDeviceValues2.GetStringValue(ref PortableDevicePKeys.WPD_OBJECT_NAME, out a);
					if (a == serviceName)
					{
						wpdService = portableDeviceService;
						break;
					}
					portableDeviceService.Close();
				}
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00011E28 File Offset: 0x00010028
		public static void GetServiceProperty(IPortableDeviceService service, _tagpropertykey key, out PortableDeviceApiLib.IPortableDeviceValues values)
		{
			try
			{
				values = null;
				string pszObjectID = null;
				service.GetServiceObjectID(out pszObjectID);
				IPortableDeviceContent2 portableDeviceContent = null;
				service.Content(out portableDeviceContent);
				IPortableDeviceProperties portableDeviceProperties = null;
				portableDeviceContent.Properties(out portableDeviceProperties);
				PortableDeviceApiLib.IPortableDeviceKeyCollection portableDeviceKeyCollection = (PortableDeviceApiLib.IPortableDeviceKeyCollection)((PortableDeviceKeyCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("DE2D022D-2480-43BE-97F0-D1FA2CF98F4F"))));
				portableDeviceKeyCollection.Add(ref key);
				portableDeviceProperties.GetValues(pszObjectID, portableDeviceKeyCollection, out values);
			}
			catch (Exception)
			{
				throw new MtpException();
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00011EA0 File Offset: 0x000100A0
		public static string GetServicePropertyStringValue(IPortableDeviceService service, _tagpropertykey key)
		{
			string result;
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
				string text = null;
				WpdUtils.GetServiceProperty(service, key, out portableDeviceValues);
				portableDeviceValues.GetStringValue(ref key, out text);
				result = text;
			}
			catch (Exception)
			{
				throw new MtpException();
			}
			return result;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00011EE0 File Offset: 0x000100E0
		public static ushort[] GetServicePropertyUint16Array(IPortableDeviceService service, _tagpropertykey key)
		{
			ushort[] result;
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
				uint num = 0U;
				WpdUtils.GetServiceProperty(service, key, out portableDeviceValues);
				IntPtr source;
				portableDeviceValues.GetBufferValue(ref key, out source, out num);
				short[] array = new short[num / 2U];
				ushort[] array2 = new ushort[num / 2U];
				Marshal.Copy(source, array, 0, array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = (ushort)array[i];
				}
				result = array2;
			}
			catch (Exception)
			{
				throw new MtpException();
			}
			return result;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00011F64 File Offset: 0x00010164
		public static uint GetServicePropertyUnsignedIntegerValue(IPortableDeviceService service, _tagpropertykey key)
		{
			uint result;
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
				uint num = 0U;
				WpdUtils.GetServiceProperty(service, key, out portableDeviceValues);
				portableDeviceValues.GetUnsignedIntegerValue(ref key, out num);
				result = num;
			}
			catch (Exception)
			{
				throw new MtpException();
			}
			return result;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00011FA4 File Offset: 0x000101A4
		public static byte GetServicePropertyByteValue(IPortableDeviceService service, _tagpropertykey key)
		{
			byte byteValue;
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
				WpdUtils.GetServiceProperty(service, key, out portableDeviceValues);
				tag_inner_PROPVARIANT tag_inner_PROPVARIANT;
				portableDeviceValues.GetValue(ref key, out tag_inner_PROPVARIANT);
				IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(tag_inner_PROPVARIANT));
				Marshal.StructureToPtr(tag_inner_PROPVARIANT, ptr, false);
				byteValue = ((WpdUtils.WpdVariant)Marshal.PtrToStructure(ptr, typeof(WpdUtils.WpdVariant))).byteValue;
			}
			catch (Exception)
			{
				throw new MtpException();
			}
			return byteValue;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00012020 File Offset: 0x00010220
		public static Guid GetServicePropertyGuid(IPortableDeviceService service, _tagpropertykey key)
		{
			Guid result;
			try
			{
				PortableDeviceApiLib.IPortableDeviceValues portableDeviceValues = null;
				Guid empty = Guid.Empty;
				WpdUtils.GetServiceProperty(service, key, out portableDeviceValues);
				portableDeviceValues.GetGuidValue(ref key, out empty);
				result = empty;
			}
			catch (Exception)
			{
				throw new MtpException();
			}
			return result;
		}

		// Token: 0x0400037C RID: 892
		public const uint PtpResponseCodeOK = 8193U;

		// Token: 0x0400037D RID: 893
		public const uint E_FAIL = 2147500037U;

		// Token: 0x0400037E RID: 894
		private const uint SECURITY_IMPERSONATION = 131072U;

		// Token: 0x0400037F RID: 895
		private const int ChunkSize = 1048576;

		// Token: 0x0200002B RID: 43
		[StructLayout(LayoutKind.Explicit, Size = 16)]
		private struct WpdVariant
		{
			// Token: 0x04000380 RID: 896
			[FieldOffset(0)]
			public short vt;

			// Token: 0x04000381 RID: 897
			[FieldOffset(8)]
			public byte byteValue;
		}
	}
}

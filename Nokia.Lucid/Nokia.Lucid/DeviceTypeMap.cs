using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nokia.Lucid.Properties;

namespace Nokia.Lucid
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public struct DeviceTypeMap : IEquatable<DeviceTypeMap>
	{
		// Token: 0x060001CE RID: 462 RVA: 0x0000CE5C File Offset: 0x0000B05C
		public DeviceTypeMap(Guid interfaceClass, DeviceType deviceType)
		{
			this.mappings = new Dictionary<Guid, DeviceType>
			{
				{
					interfaceClass,
					deviceType
				}
			};
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000CE80 File Offset: 0x0000B080
		public DeviceTypeMap(IEnumerable<KeyValuePair<Guid, DeviceType>> mappings)
		{
			if (mappings == null)
			{
				this.mappings = null;
				return;
			}
			this.mappings = new Dictionary<Guid, DeviceType>();
			foreach (KeyValuePair<Guid, DeviceType> keyValuePair in mappings)
			{
				this.mappings.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
		private DeviceTypeMap(IEnumerable<KeyValuePair<Guid, DeviceType>> mappings, Guid interfaceClass, DeviceType deviceType)
		{
			this = new DeviceTypeMap(mappings);
			if (this.mappings == null)
			{
				this.mappings = new Dictionary<Guid, DeviceType>(1);
			}
			this.mappings[interfaceClass] = deviceType;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000CF1A File Offset: 0x0000B11A
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000CF21 File Offset: 0x0000B121
		public static DeviceTypeMap DefaultMap
		{
			get
			{
				return DeviceTypeMap.defaultMap;
			}
			set
			{
				DeviceTypeMap.defaultMap = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public IEnumerable<Guid> InterfaceClasses
		{
			get
			{
				if (this.mappings != null)
				{
					foreach (KeyValuePair<Guid, DeviceType> i in this.mappings)
					{
						KeyValuePair<Guid, DeviceType> keyValuePair = i;
						yield return keyValuePair.Key;
					}
				}
				yield break;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000D106 File Offset: 0x0000B306
		public bool IsEmpty
		{
			get
			{
				return this.mappings == null || this.mappings.Count == 0;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D120 File Offset: 0x0000B320
		public static DeviceTypeMap CreateDefaultMap()
		{
			return new DeviceTypeMap(new Dictionary<Guid, DeviceType>
			{
				{
					WindowsPhoneIdentifiers.CareConnectivityDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.LumiaConnectivityDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.ApolloDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.TestServerDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.LabelAppDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.NcsdDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.UefiDeviceInterfaceGuid,
					DeviceType.Interface
				},
				{
					WindowsPhoneIdentifiers.EdDeviceInterfaceGuid,
					DeviceType.Interface
				}
			});
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D199 File Offset: 0x0000B399
		public static bool operator !=(DeviceTypeMap left, DeviceTypeMap right)
		{
			return !object.Equals(left, right);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D1AF File Offset: 0x0000B3AF
		public static bool operator ==(DeviceTypeMap left, DeviceTypeMap right)
		{
			return object.Equals(left, right);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D1C2 File Offset: 0x0000B3C2
		public DeviceTypeMap SetMapping(Guid interfaceClass, DeviceType deviceType)
		{
			return new DeviceTypeMap(this.mappings, interfaceClass, deviceType);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000D1D1 File Offset: 0x0000B3D1
		public DeviceTypeMap SetMappings(IEnumerable<KeyValuePair<Guid, DeviceType>> range)
		{
			if (range == null)
			{
				return this;
			}
			if (this.mappings == null)
			{
				return new DeviceTypeMap(range);
			}
			return new DeviceTypeMap(this.mappings.Union(range));
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000D21C File Offset: 0x0000B41C
		public DeviceTypeMap ClearMapping(Guid interfaceClass)
		{
			return new DeviceTypeMap(from m in this.mappings
			where m.Key != interfaceClass
			select m);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000D252 File Offset: 0x0000B452
		public DeviceTypeMap ClearMappings(params Guid[] range)
		{
			return this.ClearMappings((IEnumerable<Guid>)range);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D270 File Offset: 0x0000B470
		public DeviceTypeMap ClearMappings(IEnumerable<Guid> range)
		{
			if (range == null || this.mappings == null)
			{
				return this;
			}
			return new DeviceTypeMap(this.mappings.Except(this.mappings.Join(range, (KeyValuePair<Guid, DeviceType> m) => m.Key, (Guid m) => m, (KeyValuePair<Guid, DeviceType> m, Guid g) => m)));
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000D302 File Offset: 0x0000B502
		public bool TryGetMapping(Guid interfaceClass, out DeviceType deviceType)
		{
			return this.mappings.TryGetValue(interfaceClass, out deviceType);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000D314 File Offset: 0x0000B514
		public DeviceType GetMapping(Guid interfaceClass)
		{
			DeviceType result;
			if (!this.mappings.TryGetValue(interfaceClass, out result))
			{
				string message = string.Format(CultureInfo.CurrentCulture, Resources.KeyNotFoundException_MessageFormat_DeviceTypeMappingNotFound, new object[]
				{
					interfaceClass
				});
				throw new KeyNotFoundException(message);
			}
			return result;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000D35A File Offset: 0x0000B55A
		public override bool Equals(object obj)
		{
			return obj is DeviceTypeMap && this.Equals((DeviceTypeMap)obj);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D384 File Offset: 0x0000B584
		public bool Equals(DeviceTypeMap other)
		{
			if (this.IsEmpty)
			{
				return other.IsEmpty;
			}
			if (other.IsEmpty)
			{
				return false;
			}
			if (this.mappings.Count != other.mappings.Count)
			{
				return false;
			}
			return (from m in this.mappings
			orderby m.Key
			select m).SequenceEqual(from m in other.mappings
			orderby m.Key
			select m);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D426 File Offset: 0x0000B626
		public override int GetHashCode()
		{
			if (this.mappings == null)
			{
				return 0;
			}
			return (from m in this.mappings
			orderby m.Key
			select m).GetHashCode();
		}

		// Token: 0x04000133 RID: 307
		private static DeviceTypeMap defaultMap = DeviceTypeMap.CreateDefaultMap();

		// Token: 0x04000134 RID: 308
		private readonly Dictionary<Guid, DeviceType> mappings;
	}
}

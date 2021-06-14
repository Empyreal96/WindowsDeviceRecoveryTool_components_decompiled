using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004B0 RID: 1200
	internal class Com2Properties
	{
		// Token: 0x14000409 RID: 1033
		// (add) Token: 0x060050AF RID: 20655 RVA: 0x0014E4A0 File Offset: 0x0014C6A0
		// (remove) Token: 0x060050B0 RID: 20656 RVA: 0x0014E4D8 File Offset: 0x0014C6D8
		public event EventHandler Disposed;

		// Token: 0x060050B1 RID: 20657 RVA: 0x0014E510 File Offset: 0x0014C710
		public Com2Properties(object obj, Com2PropertyDescriptor[] props, int defaultIndex)
		{
			this.SetProps(props);
			this.weakObjRef = new WeakReference(obj);
			this.defaultIndex = defaultIndex;
			this.typeInfoVersions = this.GetTypeInfoVersions(obj);
			this.touchedTime = DateTime.Now.Ticks;
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x060050B2 RID: 20658 RVA: 0x0014E564 File Offset: 0x0014C764
		// (set) Token: 0x060050B3 RID: 20659 RVA: 0x0014E56F File Offset: 0x0014C76F
		internal bool AlwaysValid
		{
			get
			{
				return this.alwaysValid > 0;
			}
			set
			{
				if (!value)
				{
					if (this.alwaysValid > 0)
					{
						this.alwaysValid--;
					}
					return;
				}
				if (this.alwaysValid == 0 && !this.CheckValid())
				{
					return;
				}
				this.alwaysValid++;
			}
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x060050B4 RID: 20660 RVA: 0x0014E5AB File Offset: 0x0014C7AB
		public Com2PropertyDescriptor DefaultProperty
		{
			get
			{
				if (!this.CheckValid(true))
				{
					return null;
				}
				if (this.defaultIndex != -1)
				{
					return this.props[this.defaultIndex];
				}
				if (this.props.Length != 0)
				{
					return this.props[0];
				}
				return null;
			}
		}

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x0014E5E2 File Offset: 0x0014C7E2
		public object TargetObject
		{
			get
			{
				if (!this.CheckValid(false) || this.touchedTime == 0L)
				{
					return null;
				}
				return this.weakObjRef.Target;
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x060050B6 RID: 20662 RVA: 0x0014E604 File Offset: 0x0014C804
		public long TicksSinceTouched
		{
			get
			{
				if (this.touchedTime == 0L)
				{
					return 0L;
				}
				return DateTime.Now.Ticks - this.touchedTime;
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x0014E630 File Offset: 0x0014C830
		public Com2PropertyDescriptor[] Properties
		{
			get
			{
				this.CheckValid(true);
				if (this.touchedTime == 0L || this.props == null)
				{
					return null;
				}
				this.touchedTime = DateTime.Now.Ticks;
				for (int i = 0; i < this.props.Length; i++)
				{
					this.props[i].SetNeedsRefresh(255, true);
				}
				return this.props;
			}
		}

		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x060050B8 RID: 20664 RVA: 0x0014E696 File Offset: 0x0014C896
		public bool TooOld
		{
			get
			{
				this.CheckValid(false, false);
				return this.touchedTime != 0L && this.TicksSinceTouched > Com2Properties.AGE_THRESHHOLD;
			}
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0014E6B8 File Offset: 0x0014C8B8
		public void AddExtendedBrowsingHandlers(Hashtable handlers)
		{
			object targetObject = this.TargetObject;
			if (targetObject == null)
			{
				return;
			}
			for (int i = 0; i < Com2Properties.extendedInterfaces.Length; i++)
			{
				Type type = Com2Properties.extendedInterfaces[i];
				if (type.IsInstanceOfType(targetObject))
				{
					Com2ExtendedBrowsingHandler com2ExtendedBrowsingHandler = (Com2ExtendedBrowsingHandler)handlers[type];
					if (com2ExtendedBrowsingHandler == null)
					{
						com2ExtendedBrowsingHandler = (Com2ExtendedBrowsingHandler)Activator.CreateInstance(Com2Properties.extendedInterfaceHandlerTypes[i]);
						handlers[type] = com2ExtendedBrowsingHandler;
					}
					if (!type.IsAssignableFrom(com2ExtendedBrowsingHandler.Interface))
					{
						throw new ArgumentException(SR.GetString("COM2BadHandlerType", new object[]
						{
							type.Name,
							com2ExtendedBrowsingHandler.Interface.Name
						}));
					}
					com2ExtendedBrowsingHandler.SetupPropertyHandlers(this.props);
				}
			}
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x0014E76C File Offset: 0x0014C96C
		public void Dispose()
		{
			if (this.props != null)
			{
				if (this.Disposed != null)
				{
					this.Disposed(this, EventArgs.Empty);
				}
				this.weakObjRef = null;
				this.props = null;
				this.touchedTime = 0L;
			}
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x0014E7A5 File Offset: 0x0014C9A5
		public bool CheckValid()
		{
			return this.CheckValid(false);
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0014E7AE File Offset: 0x0014C9AE
		public bool CheckValid(bool checkVersions)
		{
			return this.CheckValid(checkVersions, true);
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x0014E7B8 File Offset: 0x0014C9B8
		private long[] GetTypeInfoVersions(object comObject)
		{
			UnsafeNativeMethods.ITypeInfo[] array = Com2TypeInfoProcessor.FindTypeInfos(comObject, false);
			long[] array2 = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = this.GetTypeInfoVersion(array[i]);
			}
			return array2;
		}

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x060050BE RID: 20670 RVA: 0x0014E7F1 File Offset: 0x0014C9F1
		private static int CountMemberOffset
		{
			get
			{
				if (Com2Properties.countOffset == -1)
				{
					Com2Properties.countOffset = Marshal.SizeOf(typeof(Guid)) + IntPtr.Size + 24;
				}
				return Com2Properties.countOffset;
			}
		}

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x060050BF RID: 20671 RVA: 0x0014E81D File Offset: 0x0014CA1D
		private static int VersionOffset
		{
			get
			{
				if (Com2Properties.versionOffset == -1)
				{
					Com2Properties.versionOffset = Com2Properties.CountMemberOffset + 12;
				}
				return Com2Properties.versionOffset;
			}
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x0014E83C File Offset: 0x0014CA3C
		private unsafe long GetTypeInfoVersion(UnsafeNativeMethods.ITypeInfo pTypeInfo)
		{
			IntPtr zero = IntPtr.Zero;
			int typeAttr = pTypeInfo.GetTypeAttr(ref zero);
			if (!NativeMethods.Succeeded(typeAttr))
			{
				return 0L;
			}
			long result;
			try
			{
				System.Runtime.InteropServices.ComTypes.TYPEATTR typeattr;
				try
				{
					typeattr = *(System.Runtime.InteropServices.ComTypes.TYPEATTR*)((void*)zero);
				}
				catch
				{
					return 0L;
				}
				long num = 0L;
				int* ptr = (int*)(&num);
				byte* ptr2 = (byte*)(&typeattr);
				*ptr = *(int*)(ptr2 + Com2Properties.CountMemberOffset);
				ptr++;
				*ptr = *(int*)(ptr2 + Com2Properties.VersionOffset);
				result = num;
			}
			finally
			{
				pTypeInfo.ReleaseTypeAttr(zero);
			}
			return result;
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x0014E8D0 File Offset: 0x0014CAD0
		internal bool CheckValid(bool checkVersions, bool callDispose)
		{
			if (this.AlwaysValid)
			{
				return true;
			}
			bool flag = this.weakObjRef != null && this.weakObjRef.IsAlive;
			if (flag && checkVersions)
			{
				long[] array = this.GetTypeInfoVersions(this.weakObjRef.Target);
				if (array.Length != this.typeInfoVersions.Length)
				{
					flag = false;
				}
				else
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != this.typeInfoVersions[i])
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					this.typeInfoVersions = array;
				}
			}
			if (!flag && callDispose)
			{
				this.Dispose();
			}
			return flag;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x0014E960 File Offset: 0x0014CB60
		internal void SetProps(Com2PropertyDescriptor[] props)
		{
			this.props = props;
			if (props != null)
			{
				for (int i = 0; i < props.Length; i++)
				{
					props[i].PropertyManager = this;
				}
			}
		}

		// Token: 0x0400341D RID: 13341
		private static TraceSwitch DbgCom2PropertiesSwitch = new TraceSwitch("DbgCom2Properties", "Com2Properties: debug Com2 properties manager");

		// Token: 0x0400341E RID: 13342
		private static long AGE_THRESHHOLD = (long)((ulong)-1294967296);

		// Token: 0x0400341F RID: 13343
		internal WeakReference weakObjRef;

		// Token: 0x04003420 RID: 13344
		private Com2PropertyDescriptor[] props;

		// Token: 0x04003421 RID: 13345
		private int defaultIndex = -1;

		// Token: 0x04003422 RID: 13346
		private long touchedTime;

		// Token: 0x04003423 RID: 13347
		private long[] typeInfoVersions;

		// Token: 0x04003424 RID: 13348
		private int alwaysValid;

		// Token: 0x04003425 RID: 13349
		private static Type[] extendedInterfaces = new Type[]
		{
			typeof(NativeMethods.ICategorizeProperties),
			typeof(NativeMethods.IProvidePropertyBuilder),
			typeof(NativeMethods.IPerPropertyBrowsing),
			typeof(NativeMethods.IVsPerPropertyBrowsing),
			typeof(NativeMethods.IManagedPerPropertyBrowsing)
		};

		// Token: 0x04003426 RID: 13350
		private static Type[] extendedInterfaceHandlerTypes = new Type[]
		{
			typeof(Com2ICategorizePropertiesHandler),
			typeof(Com2IProvidePropertyBuilderHandler),
			typeof(Com2IPerPropertyBrowsingHandler),
			typeof(Com2IVsPerPropertyBrowsingHandler),
			typeof(Com2IManagedPerPropertyBrowsingHandler)
		};

		// Token: 0x04003428 RID: 13352
		private static int countOffset = -1;

		// Token: 0x04003429 RID: 13353
		private static int versionOffset = -1;
	}
}

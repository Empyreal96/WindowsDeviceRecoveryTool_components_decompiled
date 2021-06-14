using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004C2 RID: 1218
	internal class ComNativeDescriptor : TypeDescriptionProvider
	{
		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x00151738 File Offset: 0x0014F938
		internal static ComNativeDescriptor Instance
		{
			get
			{
				if (ComNativeDescriptor.handler == null)
				{
					ComNativeDescriptor.handler = new ComNativeDescriptor();
				}
				return ComNativeDescriptor.handler;
			}
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x00151750 File Offset: 0x0014F950
		public static object GetNativePropertyValue(object component, string propertyName, ref bool succeeded)
		{
			return ComNativeDescriptor.Instance.GetPropertyValue(component, propertyName, ref succeeded);
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x0015175F File Offset: 0x0014F95F
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			return new ComNativeDescriptor.ComTypeDescriptor(this, instance);
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x00151768 File Offset: 0x0014F968
		internal string GetClassName(object component)
		{
			string text = null;
			if (component is NativeMethods.IVsPerPropertyBrowsing)
			{
				int className = ((NativeMethods.IVsPerPropertyBrowsing)component).GetClassName(ref text);
				if (NativeMethods.Succeeded(className) && text != null)
				{
					return text;
				}
			}
			UnsafeNativeMethods.ITypeInfo typeInfo = Com2TypeInfoProcessor.FindTypeInfo(component, true);
			if (typeInfo == null)
			{
				return "";
			}
			if (typeInfo != null)
			{
				string text2 = null;
				try
				{
					typeInfo.GetDocumentation(-1, ref text, ref text2, null, null);
					while (text != null && text.Length > 0 && text[0] == '_')
					{
						text = text.Substring(1);
					}
					return text;
				}
				catch
				{
				}
			}
			return "";
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x00151800 File Offset: 0x0014FA00
		internal TypeConverter GetConverter(object component)
		{
			return TypeDescriptor.GetConverter(typeof(IComponent));
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x00151811 File Offset: 0x0014FA11
		internal object GetEditor(object component, Type baseEditorType)
		{
			return TypeDescriptor.GetEditor(component.GetType(), baseEditorType);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00151820 File Offset: 0x0014FA20
		internal string GetName(object component)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return "";
			}
			int nameDispId = Com2TypeInfoProcessor.GetNameDispId((UnsafeNativeMethods.IDispatch)component);
			if (nameDispId != -1)
			{
				bool flag = false;
				object propertyValue = this.GetPropertyValue(component, nameDispId, ref flag);
				if (flag && propertyValue != null)
				{
					return propertyValue.ToString();
				}
			}
			return "";
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x0015186C File Offset: 0x0014FA6C
		internal object GetPropertyValue(object component, string propertyName, ref bool succeeded)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			string[] rgszNames = new string[]
			{
				propertyName
			};
			int[] array = new int[]
			{
				-1
			};
			Guid empty = Guid.Empty;
			try
			{
				int idsOfNames = dispatch.GetIDsOfNames(ref empty, rgszNames, 1, SafeNativeMethods.GetThreadLCID(), array);
				if (array[0] == -1 || NativeMethods.Failed(idsOfNames))
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
			return this.GetPropertyValue(component, array[0], ref succeeded);
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001518F4 File Offset: 0x0014FAF4
		internal object GetPropertyValue(object component, int dispid, ref bool succeeded)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return null;
			}
			object[] array = new object[1];
			if (this.GetPropertyValue(component, dispid, array) == 0)
			{
				succeeded = true;
				return array[0];
			}
			succeeded = false;
			return null;
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00151928 File Offset: 0x0014FB28
		internal int GetPropertyValue(object component, int dispid, object[] retval)
		{
			if (!(component is UnsafeNativeMethods.IDispatch))
			{
				return -2147467262;
			}
			UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)component;
			try
			{
				Guid empty = Guid.Empty;
				NativeMethods.tagEXCEPINFO tagEXCEPINFO = new NativeMethods.tagEXCEPINFO();
				int num;
				try
				{
					num = dispatch.Invoke(dispid, ref empty, SafeNativeMethods.GetThreadLCID(), 2, new NativeMethods.tagDISPPARAMS(), retval, tagEXCEPINFO, null);
					if (num == -2147352567)
					{
						num = tagEXCEPINFO.scode;
					}
				}
				catch (ExternalException ex)
				{
					num = ex.ErrorCode;
				}
				return num;
			}
			catch
			{
			}
			return -2147467259;
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001519B8 File Offset: 0x0014FBB8
		internal bool IsNameDispId(object obj, int dispid)
		{
			return obj != null && obj.GetType().IsCOMObject && dispid == Com2TypeInfoProcessor.GetNameDispId((UnsafeNativeMethods.IDispatch)obj);
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x001519DC File Offset: 0x0014FBDC
		private void CheckClear(object component)
		{
			int num = this.clearCount + 1;
			this.clearCount = num;
			if (num % 25 == 0)
			{
				WeakHashtable obj = this.nativeProps;
				lock (obj)
				{
					this.clearCount = 0;
					List<object> list = null;
					foreach (object obj2 in this.nativeProps)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						Com2Properties com2Properties = dictionaryEntry.Value as Com2Properties;
						if (com2Properties != null && com2Properties.TooOld)
						{
							if (list == null)
							{
								list = new List<object>(3);
							}
							list.Add(dictionaryEntry.Key);
						}
					}
					if (list != null)
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							object key = list[i];
							Com2Properties com2Properties = this.nativeProps[key] as Com2Properties;
							if (com2Properties != null)
							{
								com2Properties.Disposed -= this.OnPropsInfoDisposed;
								com2Properties.Dispose();
								this.nativeProps.Remove(key);
							}
						}
					}
				}
			}
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x00151B18 File Offset: 0x0014FD18
		private Com2Properties GetPropsInfo(object component)
		{
			this.CheckClear(component);
			Com2Properties com2Properties = (Com2Properties)this.nativeProps[component];
			if (com2Properties == null || !com2Properties.CheckValid())
			{
				com2Properties = Com2TypeInfoProcessor.GetProperties(component);
				if (com2Properties != null)
				{
					com2Properties.Disposed += this.OnPropsInfoDisposed;
					this.nativeProps.SetWeak(component, com2Properties);
					com2Properties.AddExtendedBrowsingHandlers(this.extendedBrowsingHandlers);
				}
			}
			return com2Properties;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00151B80 File Offset: 0x0014FD80
		internal AttributeCollection GetAttributes(object component)
		{
			ArrayList arrayList = new ArrayList();
			if (component is NativeMethods.IManagedPerPropertyBrowsing)
			{
				object[] componentAttributes = Com2IManagedPerPropertyBrowsingHandler.GetComponentAttributes((NativeMethods.IManagedPerPropertyBrowsing)component, -1);
				for (int i = 0; i < componentAttributes.Length; i++)
				{
					arrayList.Add(componentAttributes[i]);
				}
			}
			if (Com2ComponentEditor.NeedsComponentEditor(component))
			{
				EditorAttribute value = new EditorAttribute(typeof(Com2ComponentEditor), typeof(ComponentEditor));
				arrayList.Add(value);
			}
			if (arrayList == null || arrayList.Count == 0)
			{
				return this.staticAttrs;
			}
			Attribute[] array = new Attribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return new AttributeCollection(array);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00151C1C File Offset: 0x0014FE1C
		internal PropertyDescriptor GetDefaultProperty(object component)
		{
			this.CheckClear(component);
			Com2Properties propsInfo = this.GetPropsInfo(component);
			if (propsInfo != null)
			{
				return propsInfo.DefaultProperty;
			}
			return null;
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00151C43 File Offset: 0x0014FE43
		internal EventDescriptorCollection GetEvents(object component)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00151C43 File Offset: 0x0014FE43
		internal EventDescriptorCollection GetEvents(object component, Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x0000DE5C File Offset: 0x0000C05C
		internal EventDescriptor GetDefaultEvent(object component)
		{
			return null;
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x00151C4C File Offset: 0x0014FE4C
		internal PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			Com2Properties propsInfo = this.GetPropsInfo(component);
			if (propsInfo == null)
			{
				return PropertyDescriptorCollection.Empty;
			}
			PropertyDescriptorCollection result;
			try
			{
				propsInfo.AlwaysValid = true;
				PropertyDescriptor[] properties = propsInfo.Properties;
				result = new PropertyDescriptorCollection(properties);
			}
			finally
			{
				propsInfo.AlwaysValid = false;
			}
			return result;
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x00151C9C File Offset: 0x0014FE9C
		private void OnPropsInfoDisposed(object sender, EventArgs e)
		{
			Com2Properties com2Properties = sender as Com2Properties;
			if (com2Properties != null)
			{
				com2Properties.Disposed -= this.OnPropsInfoDisposed;
				WeakHashtable obj = this.nativeProps;
				lock (obj)
				{
					object obj2 = com2Properties.TargetObject;
					if (obj2 == null && this.nativeProps.ContainsValue(com2Properties))
					{
						foreach (object obj3 in this.nativeProps)
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
							if (dictionaryEntry.Value == com2Properties)
							{
								obj2 = dictionaryEntry.Key;
								break;
							}
						}
						if (obj2 == null)
						{
							return;
						}
					}
					this.nativeProps.Remove(obj2);
				}
			}
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x00151D7C File Offset: 0x0014FF7C
		internal static void ResolveVariantTypeConverterAndTypeEditor(object propertyValue, ref TypeConverter currentConverter, Type editorType, ref object currentEditor)
		{
			if (propertyValue != null && propertyValue != null && !Convert.IsDBNull(propertyValue))
			{
				Type type = propertyValue.GetType();
				TypeConverter converter = TypeDescriptor.GetConverter(type);
				if (converter != null && converter.GetType() != typeof(TypeConverter))
				{
					currentConverter = converter;
				}
				object editor = TypeDescriptor.GetEditor(type, editorType);
				if (editor != null)
				{
					currentEditor = editor;
				}
			}
		}

		// Token: 0x04003467 RID: 13415
		private static ComNativeDescriptor handler;

		// Token: 0x04003468 RID: 13416
		private AttributeCollection staticAttrs = new AttributeCollection(new Attribute[]
		{
			BrowsableAttribute.Yes,
			DesignTimeVisibleAttribute.No
		});

		// Token: 0x04003469 RID: 13417
		private WeakHashtable nativeProps = new WeakHashtable();

		// Token: 0x0400346A RID: 13418
		private Hashtable extendedBrowsingHandlers = new Hashtable();

		// Token: 0x0400346B RID: 13419
		private int clearCount;

		// Token: 0x0400346C RID: 13420
		private const int CLEAR_INTERVAL = 25;

		// Token: 0x02000846 RID: 2118
		private sealed class ComTypeDescriptor : ICustomTypeDescriptor
		{
			// Token: 0x06006F7E RID: 28542 RVA: 0x00198B64 File Offset: 0x00196D64
			internal ComTypeDescriptor(ComNativeDescriptor handler, object instance)
			{
				this._handler = handler;
				this._instance = instance;
			}

			// Token: 0x06006F7F RID: 28543 RVA: 0x00198B7A File Offset: 0x00196D7A
			AttributeCollection ICustomTypeDescriptor.GetAttributes()
			{
				return this._handler.GetAttributes(this._instance);
			}

			// Token: 0x06006F80 RID: 28544 RVA: 0x00198B8D File Offset: 0x00196D8D
			string ICustomTypeDescriptor.GetClassName()
			{
				return this._handler.GetClassName(this._instance);
			}

			// Token: 0x06006F81 RID: 28545 RVA: 0x00198BA0 File Offset: 0x00196DA0
			string ICustomTypeDescriptor.GetComponentName()
			{
				return this._handler.GetName(this._instance);
			}

			// Token: 0x06006F82 RID: 28546 RVA: 0x00198BB3 File Offset: 0x00196DB3
			TypeConverter ICustomTypeDescriptor.GetConverter()
			{
				return this._handler.GetConverter(this._instance);
			}

			// Token: 0x06006F83 RID: 28547 RVA: 0x00198BC6 File Offset: 0x00196DC6
			EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
			{
				return this._handler.GetDefaultEvent(this._instance);
			}

			// Token: 0x06006F84 RID: 28548 RVA: 0x00198BD9 File Offset: 0x00196DD9
			PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
			{
				return this._handler.GetDefaultProperty(this._instance);
			}

			// Token: 0x06006F85 RID: 28549 RVA: 0x00198BEC File Offset: 0x00196DEC
			object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
			{
				return this._handler.GetEditor(this._instance, editorBaseType);
			}

			// Token: 0x06006F86 RID: 28550 RVA: 0x00198C00 File Offset: 0x00196E00
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
			{
				return this._handler.GetEvents(this._instance);
			}

			// Token: 0x06006F87 RID: 28551 RVA: 0x00198C13 File Offset: 0x00196E13
			EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
			{
				return this._handler.GetEvents(this._instance, attributes);
			}

			// Token: 0x06006F88 RID: 28552 RVA: 0x00198C27 File Offset: 0x00196E27
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
			{
				return this._handler.GetProperties(this._instance, null);
			}

			// Token: 0x06006F89 RID: 28553 RVA: 0x00198C3B File Offset: 0x00196E3B
			PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
			{
				return this._handler.GetProperties(this._instance, attributes);
			}

			// Token: 0x06006F8A RID: 28554 RVA: 0x00198C4F File Offset: 0x00196E4F
			object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
			{
				return this._instance;
			}

			// Token: 0x040042E2 RID: 17122
			private ComNativeDescriptor _handler;

			// Token: 0x040042E3 RID: 17123
			private object _instance;
		}
	}
}

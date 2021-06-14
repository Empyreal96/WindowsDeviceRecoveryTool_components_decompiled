using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027B RID: 635
	internal class ElementProperty : ElementObjectPropertyBase
	{
		// Token: 0x0600242B RID: 9259 RVA: 0x000B05B8 File Offset: 0x000AE7B8
		internal ElementProperty(ElementMarkupObject obj, PropertyDescriptor descriptor) : base(obj)
		{
			this._descriptor = descriptor;
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x000B05C8 File Offset: 0x000AE7C8
		public override string Name
		{
			get
			{
				return this._descriptor.Name;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x000B05D5 File Offset: 0x000AE7D5
		public override Type PropertyType
		{
			get
			{
				return this._descriptor.PropertyType;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x000B05E2 File Offset: 0x000AE7E2
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._descriptor;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000B05EA File Offset: 0x000AE7EA
		public override bool IsAttached
		{
			get
			{
				this.UpdateDependencyProperty();
				return this._isAttached;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000B05F8 File Offset: 0x000AE7F8
		public override DependencyProperty DependencyProperty
		{
			get
			{
				this.UpdateDependencyProperty();
				return this._dependencyProperty;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000B0608 File Offset: 0x000AE808
		public override object Value
		{
			get
			{
				DependencyProperty dependencyProperty = this.DependencyProperty;
				object obj;
				if (dependencyProperty != null)
				{
					DependencyObject dependencyObject = this._object.Instance as DependencyObject;
					obj = dependencyObject.ReadLocalValue(dependencyProperty);
					Expression expression = obj as Expression;
					if (expression != null)
					{
						TypeConverter converter = TypeDescriptor.GetConverter(obj);
						if (base.Manager.XamlWriterMode == XamlWriterMode.Expression && converter.CanConvertTo(typeof(MarkupExtension)))
						{
							obj = converter.ConvertTo(expression, typeof(MarkupExtension));
						}
						else
						{
							obj = expression.GetValue(dependencyObject, dependencyProperty);
						}
					}
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = dependencyProperty.GetDefaultValue(dependencyObject.DependencyObjectType);
					}
				}
				else if ((this.Name == "Template" || this.Name == "VisualTree") && base.Context.Instance is FrameworkTemplate && (base.Context.Instance as FrameworkTemplate).HasContent)
				{
					obj = (base.Context.Instance as FrameworkTemplate).LoadContent();
				}
				else
				{
					obj = this._descriptor.GetValue(this._object.Instance);
				}
				if (!(obj is MarkupExtension) && !base.CanConvertToString(obj))
				{
					obj = ElementProperty.CheckForMarkupExtension(this.PropertyType, obj, base.Context, true);
				}
				return obj;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000B0747 File Offset: 0x000AE947
		public override AttributeCollection Attributes
		{
			get
			{
				return this._descriptor.Attributes;
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000B0754 File Offset: 0x000AE954
		private void UpdateDependencyProperty()
		{
			if (!this._isDependencyPropertyCached)
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(this._descriptor);
				if (dependencyPropertyDescriptor != null)
				{
					this._dependencyProperty = dependencyPropertyDescriptor.DependencyProperty;
					this._isAttached = dependencyPropertyDescriptor.IsAttached;
				}
				this._isDependencyPropertyCached = true;
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000B0798 File Offset: 0x000AE998
		internal static object CheckForMarkupExtension(Type propertyType, object value, IValueSerializerContext context, bool convertEnums)
		{
			if (value == null)
			{
				return new NullExtension();
			}
			TypeConverter converter = TypeDescriptor.GetConverter(value);
			if (converter.CanConvertTo(context, typeof(MarkupExtension)))
			{
				return converter.ConvertTo(context, TypeConverterHelper.InvariantEnglishUS, value, typeof(MarkupExtension));
			}
			Type type = value as Type;
			if (type != null)
			{
				if (propertyType == typeof(Type))
				{
					return value;
				}
				return new TypeExtension(type);
			}
			else
			{
				if (convertEnums)
				{
					Enum @enum = value as Enum;
					if (@enum != null)
					{
						ValueSerializer valueSerializerFor = context.GetValueSerializerFor(typeof(Type));
						string str = valueSerializerFor.ConvertToString(@enum.GetType(), context);
						return new StaticExtension(str + "." + @enum.ToString());
					}
				}
				Array array = value as Array;
				if (array != null)
				{
					return new ArrayExtension(array);
				}
				return value;
			}
		}

		// Token: 0x04001B27 RID: 6951
		private PropertyDescriptor _descriptor;

		// Token: 0x04001B28 RID: 6952
		private bool _isDependencyPropertyCached;

		// Token: 0x04001B29 RID: 6953
		private DependencyProperty _dependencyProperty;

		// Token: 0x04001B2A RID: 6954
		private bool _isAttached;
	}
}

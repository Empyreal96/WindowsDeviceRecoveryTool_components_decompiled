using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000288 RID: 648
	internal class FrameworkElementFactoryProperty : ElementPropertyBase
	{
		// Token: 0x06002486 RID: 9350 RVA: 0x000B1082 File Offset: 0x000AF282
		public FrameworkElementFactoryProperty(PropertyValue propertyValue, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._propertyValue = propertyValue;
			this._item = item;
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x000B10A0 File Offset: 0x000AF2A0
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				if (!this._descriptorCalculated)
				{
					this._descriptorCalculated = true;
					if (DependencyProperty.FromName(this._propertyValue.Property.Name, this._item.ObjectType) == this._propertyValue.Property)
					{
						this._descriptor = DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType);
					}
				}
				return this._descriptor;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x000B1110 File Offset: 0x000AF310
		public override bool IsAttached
		{
			get
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = this.PropertyDescriptor as DependencyPropertyDescriptor;
				return dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x000B1134 File Offset: 0x000AF334
		public override AttributeCollection Attributes
		{
			get
			{
				if (this._descriptor != null)
				{
					return this._descriptor.Attributes;
				}
				PropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType);
				return propertyDescriptor.Attributes;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x000B1177 File Offset: 0x000AF377
		public override string Name
		{
			get
			{
				return this._propertyValue.Property.Name;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000B1189 File Offset: 0x000AF389
		public override Type PropertyType
		{
			get
			{
				return this._propertyValue.Property.PropertyType;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x000B119B File Offset: 0x000AF39B
		public override DependencyProperty DependencyProperty
		{
			get
			{
				return this._propertyValue.Property;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000B11A8 File Offset: 0x000AF3A8
		public override object Value
		{
			get
			{
				PropertyValueType valueType = this._propertyValue.ValueType;
				if (valueType == PropertyValueType.Set || valueType == PropertyValueType.TemplateBinding)
				{
					return this._propertyValue.Value;
				}
				if (valueType != PropertyValueType.Resource)
				{
					return null;
				}
				return new DynamicResourceExtension(this._propertyValue.Value);
			}
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000B11EC File Offset: 0x000AF3EC
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000B11F9 File Offset: 0x000AF3F9
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x04001B39 RID: 6969
		private PropertyValue _propertyValue;

		// Token: 0x04001B3A RID: 6970
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x04001B3B RID: 6971
		private bool _descriptorCalculated;

		// Token: 0x04001B3C RID: 6972
		private PropertyDescriptor _descriptor;
	}
}

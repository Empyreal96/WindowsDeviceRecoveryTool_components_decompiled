using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Management
{
	// Token: 0x02000045 RID: 69
	internal class ManagementScopeConverter : ExpandableObjectConverter
	{
		// Token: 0x06000285 RID: 645 RVA: 0x0000DAF1 File Offset: 0x0000BCF1
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(ManagementScope) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00009871 File Offset: 0x00007A71
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000DB10 File Offset: 0x0000BD10
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ManagementScope && destinationType == typeof(InstanceDescriptor))
			{
				ManagementScope managementScope = (ManagementScope)value;
				ConstructorInfo constructor = typeof(ManagementScope).GetConstructor(new Type[]
				{
					typeof(string)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						managementScope.Path.Path
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

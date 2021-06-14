using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace System.Management
{
	// Token: 0x02000034 RID: 52
	internal class ManagementPathConverter : ExpandableObjectConverter
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00009853 File Offset: 0x00007A53
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(ManagementPath) || base.CanConvertFrom(context, sourceType);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009871 File Offset: 0x00007A71
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009890 File Offset: 0x00007A90
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (value is ManagementPath && destinationType == typeof(InstanceDescriptor))
			{
				ManagementPath managementPath = (ManagementPath)value;
				ConstructorInfo constructor = typeof(ManagementPath).GetConstructor(new Type[]
				{
					typeof(string)
				});
				if (constructor != null)
				{
					return new InstanceDescriptor(constructor, new object[]
					{
						managementPath.Path
					});
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}

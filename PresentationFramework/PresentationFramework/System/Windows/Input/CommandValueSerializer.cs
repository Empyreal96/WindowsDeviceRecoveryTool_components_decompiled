using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows.Input
{
	// Token: 0x0200017B RID: 379
	internal class CommandValueSerializer : ValueSerializer
	{
		// Token: 0x060015FD RID: 5629 RVA: 0x0006D1D4 File Offset: 0x0006B3D4
		public override bool CanConvertToString(object value, IValueSerializerContext context)
		{
			if (context == null || context.GetValueSerializerFor(typeof(Type)) == null)
			{
				return false;
			}
			RoutedCommand routedCommand = value as RoutedCommand;
			if (routedCommand == null || routedCommand.OwnerType == null)
			{
				return false;
			}
			if (CommandConverter.IsKnownType(routedCommand.OwnerType))
			{
				return true;
			}
			string name = routedCommand.Name + "Command";
			Type ownerType = routedCommand.OwnerType;
			string name2 = ownerType.Name;
			PropertyInfo property = ownerType.GetProperty(name, BindingFlags.Static | BindingFlags.Public);
			if (property != null)
			{
				return true;
			}
			FieldInfo field = ownerType.GetField(name, BindingFlags.Static | BindingFlags.Public);
			return field != null;
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanConvertFromString(string value, IValueSerializerContext context)
		{
			return true;
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0006D270 File Offset: 0x0006B470
		public override string ConvertToString(object value, IValueSerializerContext context)
		{
			if (value == null)
			{
				return string.Empty;
			}
			RoutedCommand routedCommand = value as RoutedCommand;
			if (routedCommand == null || !(null != routedCommand.OwnerType))
			{
				throw base.GetConvertToException(value, typeof(string));
			}
			if (CommandConverter.IsKnownType(routedCommand.OwnerType))
			{
				return routedCommand.Name;
			}
			if (context == null)
			{
				throw new InvalidOperationException(SR.Get("ValueSerializerContextUnavailable", new object[]
				{
					base.GetType().Name
				}));
			}
			ValueSerializer valueSerializerFor = context.GetValueSerializerFor(typeof(Type));
			if (valueSerializerFor == null)
			{
				throw new InvalidOperationException(SR.Get("TypeValueSerializerUnavailable", new object[]
				{
					base.GetType().Name
				}));
			}
			return valueSerializerFor.ConvertToString(routedCommand.OwnerType, context) + "." + routedCommand.Name + "Command";
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0006D350 File Offset: 0x0006B550
		public override IEnumerable<Type> TypeReferences(object value, IValueSerializerContext context)
		{
			if (value != null)
			{
				RoutedCommand routedCommand = value as RoutedCommand;
				if (routedCommand != null && routedCommand.OwnerType != null && !CommandConverter.IsKnownType(routedCommand.OwnerType))
				{
					return new Type[]
					{
						routedCommand.OwnerType
					};
				}
			}
			return base.TypeReferences(value, context);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0006D3A0 File Offset: 0x0006B5A0
		public override object ConvertFromString(string value, IValueSerializerContext context)
		{
			if (value != null)
			{
				if (!(value != string.Empty))
				{
					return null;
				}
				Type ownerType = null;
				int num = value.IndexOf('.');
				string localName;
				if (num >= 0)
				{
					string value2 = value.Substring(0, num);
					if (context == null)
					{
						throw new InvalidOperationException(SR.Get("ValueSerializerContextUnavailable", new object[]
						{
							base.GetType().Name
						}));
					}
					ValueSerializer valueSerializerFor = context.GetValueSerializerFor(typeof(Type));
					if (valueSerializerFor == null)
					{
						throw new InvalidOperationException(SR.Get("TypeValueSerializerUnavailable", new object[]
						{
							base.GetType().Name
						}));
					}
					ownerType = (valueSerializerFor.ConvertFromString(value2, context) as Type);
					localName = value.Substring(num + 1).Trim();
				}
				else
				{
					localName = value.Trim();
				}
				ICommand command = CommandConverter.ConvertFromHelper(ownerType, localName);
				if (command != null)
				{
					return command;
				}
			}
			return base.ConvertFromString(value, context);
		}
	}
}

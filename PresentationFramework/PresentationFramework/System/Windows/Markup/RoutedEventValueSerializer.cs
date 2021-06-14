using System;
using System.Collections.Generic;
using MS.Internal.WindowsBase;

namespace System.Windows.Markup
{
	// Token: 0x0200022B RID: 555
	internal class RoutedEventValueSerializer : ValueSerializer
	{
		// Token: 0x06002244 RID: 8772 RVA: 0x000AA9FB File Offset: 0x000A8BFB
		public override bool CanConvertToString(object value, IValueSerializerContext context)
		{
			return ValueSerializer.GetSerializerFor(typeof(Type), context) != null;
		}

		// Token: 0x06002245 RID: 8773 RVA: 0x000AA9FB File Offset: 0x000A8BFB
		public override bool CanConvertFromString(string value, IValueSerializerContext context)
		{
			return ValueSerializer.GetSerializerFor(typeof(Type), context) != null;
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000AAA10 File Offset: 0x000A8C10
		public override string ConvertToString(object value, IValueSerializerContext context)
		{
			RoutedEvent routedEvent = value as RoutedEvent;
			if (routedEvent != null)
			{
				ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), context);
				if (serializerFor != null)
				{
					return serializerFor.ConvertToString(routedEvent.OwnerType, context) + "." + routedEvent.Name;
				}
			}
			return base.ConvertToString(value, context);
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x000AAA61 File Offset: 0x000A8C61
		private static void ForceTypeConstructors(Type currentType)
		{
			while (currentType != null && !RoutedEventValueSerializer.initializedTypes.ContainsKey(currentType))
			{
				SecurityHelper.RunClassConstructor(currentType);
				RoutedEventValueSerializer.initializedTypes[currentType] = currentType;
				currentType = currentType.BaseType;
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000AAA98 File Offset: 0x000A8C98
		public override object ConvertFromString(string value, IValueSerializerContext context)
		{
			ValueSerializer serializerFor = ValueSerializer.GetSerializerFor(typeof(Type), context);
			if (serializerFor != null)
			{
				int num = value.IndexOf('.');
				if (num > 0)
				{
					Type type = serializerFor.ConvertFromString(value.Substring(0, num), context) as Type;
					string name = value.Substring(num + 1).Trim();
					RoutedEventValueSerializer.ForceTypeConstructors(type);
					return EventManager.GetRoutedEventFromName(name, type);
				}
			}
			return base.ConvertFromString(value, context);
		}

		// Token: 0x040019DC RID: 6620
		private static Dictionary<Type, Type> initializedTypes = new Dictionary<Type, Type>();
	}
}

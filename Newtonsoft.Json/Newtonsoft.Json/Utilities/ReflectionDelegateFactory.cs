using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000E5 RID: 229
	internal abstract class ReflectionDelegateFactory
	{
		// Token: 0x06000B11 RID: 2833 RVA: 0x0002CA44 File Offset: 0x0002AC44
		public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateGet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return this.CreateGet<T>(fieldInfo);
			}
			throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002CA98 File Offset: 0x0002AC98
		public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return this.CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return this.CreateSet<T>(fieldInfo);
			}
			throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		// Token: 0x06000B13 RID: 2835
		public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

		// Token: 0x06000B14 RID: 2836
		public abstract ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method);

		// Token: 0x06000B15 RID: 2837
		public abstract Func<T> CreateDefaultConstructor<T>(Type type);

		// Token: 0x06000B16 RID: 2838
		public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

		// Token: 0x06000B17 RID: 2839
		public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

		// Token: 0x06000B18 RID: 2840
		public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

		// Token: 0x06000B19 RID: 2841
		public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
	}
}

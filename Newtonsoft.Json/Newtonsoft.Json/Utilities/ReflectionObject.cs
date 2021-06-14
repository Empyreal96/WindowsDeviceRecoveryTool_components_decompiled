using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000F0 RID: 240
	internal class ReflectionObject
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x0002DA06 File Offset: 0x0002BC06
		// (set) Token: 0x06000B4B RID: 2891 RVA: 0x0002DA0E File Offset: 0x0002BC0E
		public ObjectConstructor<object> Creator { get; private set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x0002DA17 File Offset: 0x0002BC17
		// (set) Token: 0x06000B4D RID: 2893 RVA: 0x0002DA1F File Offset: 0x0002BC1F
		public IDictionary<string, ReflectionMember> Members { get; private set; }

		// Token: 0x06000B4E RID: 2894 RVA: 0x0002DA28 File Offset: 0x0002BC28
		public ReflectionObject()
		{
			this.Members = new Dictionary<string, ReflectionMember>();
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0002DA3C File Offset: 0x0002BC3C
		public object GetValue(object target, string member)
		{
			Func<object, object> getter = this.Members[member].Getter;
			return getter(target);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0002DA64 File Offset: 0x0002BC64
		public void SetValue(object target, string member, object value)
		{
			Action<object, object> setter = this.Members[member].Setter;
			setter(target, value);
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002DA8B File Offset: 0x0002BC8B
		public Type GetType(string member)
		{
			return this.Members[member].MemberType;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0002DA9E File Offset: 0x0002BC9E
		public static ReflectionObject Create(Type t, params string[] memberNames)
		{
			return ReflectionObject.Create(t, null, memberNames);
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0002DB0C File Offset: 0x0002BD0C
		public static ReflectionObject Create(Type t, MethodBase creator, params string[] memberNames)
		{
			ReflectionObject reflectionObject = new ReflectionObject();
			ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
			if (creator != null)
			{
				reflectionObject.Creator = reflectionDelegateFactory.CreateParametrizedConstructor(creator);
			}
			else if (ReflectionUtils.HasDefaultConstructor(t, false))
			{
				Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
				reflectionObject.Creator = ((object[] args) => ctor());
			}
			int i = 0;
			while (i < memberNames.Length)
			{
				string text = memberNames[i];
				MemberInfo[] member = t.GetMember(text, BindingFlags.Instance | BindingFlags.Public);
				if (member.Length != 1)
				{
					throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				MemberInfo memberInfo = member.Single<MemberInfo>();
				ReflectionMember reflectionMember = new ReflectionMember();
				MemberTypes memberTypes = memberInfo.MemberType();
				if (memberTypes == MemberTypes.Field)
				{
					goto IL_B7;
				}
				if (memberTypes != MemberTypes.Method)
				{
					if (memberTypes == MemberTypes.Property)
					{
						goto IL_B7;
					}
					throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith(CultureInfo.InvariantCulture, memberInfo.MemberType(), memberInfo.Name));
				}
				else
				{
					MethodInfo methodInfo = (MethodInfo)memberInfo;
					if (methodInfo.IsPublic)
					{
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (parameters.Length == 0 && methodInfo.ReturnType != typeof(void))
						{
							MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Getter = ((object target) => call(target, new object[0]));
						}
						else if (parameters.Length == 1 && methodInfo.ReturnType == typeof(void))
						{
							MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
							reflectionMember.Setter = delegate(object target, object arg)
							{
								call(target, new object[]
								{
									arg
								});
							};
						}
					}
				}
				IL_1CD:
				if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
				{
					reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
				}
				if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
				{
					reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
				}
				reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				reflectionObject.Members[text] = reflectionMember;
				i++;
				continue;
				IL_B7:
				if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
				{
					reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
				}
				if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
				{
					reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
					goto IL_1CD;
				}
				goto IL_1CD;
			}
			return reflectionObject;
		}
	}
}

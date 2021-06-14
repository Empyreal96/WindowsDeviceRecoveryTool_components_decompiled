using System;
using System.Reflection;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016F RID: 367
	internal class WpfKnownMemberInvoker : XamlMemberInvoker
	{
		// Token: 0x06001570 RID: 5488 RVA: 0x00069A1D File Offset: 0x00067C1D
		public WpfKnownMemberInvoker(WpfKnownMember member) : base(member)
		{
			this._member = member;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00069A2D File Offset: 0x00067C2D
		public override object GetValue(object instance)
		{
			if (this._member.DependencyProperty != null)
			{
				return ((DependencyObject)instance).GetValue(this._member.DependencyProperty);
			}
			return this._member.GetDelegate(instance);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x00069A64 File Offset: 0x00067C64
		public override void SetValue(object instance, object value)
		{
			if (this._member.DependencyProperty != null)
			{
				((DependencyObject)instance).SetValue(this._member.DependencyProperty, value);
				return;
			}
			this._member.SetDelegate(instance, value);
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00069AA0 File Offset: 0x00067CA0
		public override ShouldSerializeResult ShouldSerializeValue(object instance)
		{
			if (!this._hasShouldSerializeMethodBeenLookedup)
			{
				Type declaringType = this._member.UnderlyingMember.DeclaringType;
				string name = "ShouldSerialize" + this._member.Name;
				BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				Type[] types = new Type[]
				{
					typeof(DependencyObject)
				};
				if (this._member.IsAttachable)
				{
					this._shouldSerializeMethod = declaringType.GetMethod(name, bindingFlags, null, types, null);
				}
				else
				{
					bindingFlags |= BindingFlags.Instance;
					this._shouldSerializeMethod = declaringType.GetMethod(name, bindingFlags, null, types, null);
				}
				this._hasShouldSerializeMethodBeenLookedup = true;
			}
			if (this._shouldSerializeMethod != null)
			{
				object[] parameters = new object[]
				{
					instance as DependencyObject
				};
				bool flag;
				if (this._member.IsAttachable)
				{
					flag = (bool)this._shouldSerializeMethod.Invoke(null, parameters);
				}
				else
				{
					flag = (bool)this._shouldSerializeMethod.Invoke(instance, parameters);
				}
				if (!flag)
				{
					return ShouldSerializeResult.False;
				}
				return ShouldSerializeResult.True;
			}
			else
			{
				DependencyObject dependencyObject = instance as DependencyObject;
				if (dependencyObject != null && this._member.DependencyProperty != null && !dependencyObject.ShouldSerializeProperty(this._member.DependencyProperty))
				{
					return ShouldSerializeResult.False;
				}
				return base.ShouldSerializeValue(instance);
			}
		}

		// Token: 0x04001252 RID: 4690
		private WpfKnownMember _member;

		// Token: 0x04001253 RID: 4691
		private bool _hasShouldSerializeMethodBeenLookedup;

		// Token: 0x04001254 RID: 4692
		private MethodInfo _shouldSerializeMethod;
	}
}

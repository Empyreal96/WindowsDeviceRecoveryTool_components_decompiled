using System;
using System.Reflection;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000172 RID: 370
	internal class WpfMemberInvoker : XamlMemberInvoker
	{
		// Token: 0x060015A7 RID: 5543 RVA: 0x0006A23B File Offset: 0x0006843B
		public WpfMemberInvoker(WpfXamlMember member) : base(member)
		{
			this._member = member;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0006A24C File Offset: 0x0006844C
		public override void SetValue(object instance, object value)
		{
			DependencyObject dependencyObject = instance as DependencyObject;
			if (dependencyObject != null)
			{
				if (this._member.DependencyProperty != null)
				{
					dependencyObject.SetValue(this._member.DependencyProperty, value);
					return;
				}
				if (this._member.RoutedEvent != null)
				{
					Delegate @delegate = value as Delegate;
					if (@delegate != null)
					{
						UIElement.AddHandler(dependencyObject, this._member.RoutedEvent, @delegate);
						return;
					}
				}
			}
			base.SetValue(instance, value);
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x0006A2B8 File Offset: 0x000684B8
		public override object GetValue(object instance)
		{
			DependencyObject dependencyObject = instance as DependencyObject;
			if (dependencyObject != null && this._member.DependencyProperty != null)
			{
				object value = dependencyObject.GetValue(this._member.DependencyProperty);
				if (value != null)
				{
					return value;
				}
				if (!this._member.ApplyGetterFallback || this._member.UnderlyingMember == null)
				{
					return value;
				}
			}
			return base.GetValue(instance);
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x0006A31C File Offset: 0x0006851C
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

		// Token: 0x04001264 RID: 4708
		private WpfXamlMember _member;

		// Token: 0x04001265 RID: 4709
		private bool _hasShouldSerializeMethodBeenLookedup;

		// Token: 0x04001266 RID: 4710
		private MethodInfo _shouldSerializeMethod;
	}
}

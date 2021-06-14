using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000272 RID: 626
	internal class HtmlToClrEventProxy : IReflect
	{
		// Token: 0x060025C5 RID: 9669 RVA: 0x000B49B8 File Offset: 0x000B2BB8
		public HtmlToClrEventProxy(object sender, string eventName, EventHandler eventHandler)
		{
			this.eventHandler = eventHandler;
			this.eventName = eventName;
			Type typeFromHandle = typeof(HtmlToClrEventProxy);
			this.typeIReflectImplementation = typeFromHandle;
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x000B49EB File Offset: 0x000B2BEB
		public string EventName
		{
			get
			{
				return this.eventName;
			}
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x000B49F3 File Offset: 0x000B2BF3
		[DispId(0)]
		public void OnHtmlEvent()
		{
			this.InvokeClrEvent();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x000B49FB File Offset: 0x000B2BFB
		private void InvokeClrEvent()
		{
			if (this.eventHandler != null)
			{
				this.eventHandler(this.sender, EventArgs.Empty);
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x000B4A1B File Offset: 0x000B2C1B
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				return this.typeIReflectImplementation.UnderlyingSystemType;
			}
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000B4A28 File Offset: 0x000B2C28
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetField(name, bindingAttr);
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000B4A37 File Offset: 0x000B2C37
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetFields(bindingAttr);
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000B4A45 File Offset: 0x000B2C45
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMember(name, bindingAttr);
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000B4A54 File Offset: 0x000B2C54
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMembers(bindingAttr);
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x000B4A62 File Offset: 0x000B2C62
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMethod(name, bindingAttr);
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x000B4A71 File Offset: 0x000B2C71
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeIReflectImplementation.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000B4A85 File Offset: 0x000B2C85
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetMethods(bindingAttr);
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000B4A93 File Offset: 0x000B2C93
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetProperties(bindingAttr);
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x000B4AA1 File Offset: 0x000B2CA1
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return this.typeIReflectImplementation.GetProperty(name, bindingAttr);
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x000B4AB0 File Offset: 0x000B2CB0
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeIReflectImplementation.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x000B4AC8 File Offset: 0x000B2CC8
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (name == "[DISPID=0]")
			{
				this.OnHtmlEvent();
				return null;
			}
			return this.typeIReflectImplementation.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x0400101E RID: 4126
		private EventHandler eventHandler;

		// Token: 0x0400101F RID: 4127
		private IReflect typeIReflectImplementation;

		// Token: 0x04001020 RID: 4128
		private object sender;

		// Token: 0x04001021 RID: 4129
		private string eventName;
	}
}

using System;
using System.Reflection;
using System.Windows.Input;

namespace System.Windows.Interactivity
{
	// Token: 0x02000013 RID: 19
	public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003858 File Offset: 0x00001A58
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00003866 File Offset: 0x00001A66
		public string CommandName
		{
			get
			{
				base.ReadPreamble();
				return this.commandName;
			}
			set
			{
				if (this.CommandName != value)
				{
					base.WritePreamble();
					this.commandName = value;
					base.WritePostscript();
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000089 RID: 137 RVA: 0x00003889 File Offset: 0x00001A89
		// (set) Token: 0x0600008A RID: 138 RVA: 0x0000389B File Offset: 0x00001A9B
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(InvokeCommandAction.CommandProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandProperty, value);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000038A9 File Offset: 0x00001AA9
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000038B6 File Offset: 0x00001AB6
		public object CommandParameter
		{
			get
			{
				return base.GetValue(InvokeCommandAction.CommandParameterProperty);
			}
			set
			{
				base.SetValue(InvokeCommandAction.CommandParameterProperty, value);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000038C4 File Offset: 0x00001AC4
		protected override void Invoke(object parameter)
		{
			if (base.AssociatedObject != null)
			{
				ICommand command = this.ResolveCommand();
				if (command != null && command.CanExecute(this.CommandParameter))
				{
					command.Execute(this.CommandParameter);
				}
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003900 File Offset: 0x00001B00
		private ICommand ResolveCommand()
		{
			ICommand result = null;
			if (this.Command != null)
			{
				result = this.Command;
			}
			else if (base.AssociatedObject != null)
			{
				Type type = base.AssociatedObject.GetType();
				PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				foreach (PropertyInfo propertyInfo in properties)
				{
					if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && string.Equals(propertyInfo.Name, this.CommandName, StringComparison.Ordinal))
					{
						result = (ICommand)propertyInfo.GetValue(base.AssociatedObject, null);
					}
				}
			}
			return result;
		}

		// Token: 0x04000027 RID: 39
		private string commandName;

		// Token: 0x04000028 RID: 40
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandAction), null);

		// Token: 0x04000029 RID: 41
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(InvokeCommandAction), null);
	}
}

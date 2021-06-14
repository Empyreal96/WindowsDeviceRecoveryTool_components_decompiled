using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Microsoft.WindowsDeviceRecoveryTool.Localization
{
	// Token: 0x02000002 RID: 2
	public class LocalizationExtension : MarkupExtension, INotifyPropertyChanged
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public LocalizationExtension(string key, string arg1, string arg2)
		{
			this.key = key;
			this.parameterNames = new string[2];
			this.parameterNames[0] = arg1;
			this.parameterNames[1] = arg2;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002080 File Offset: 0x00000280
		public LocalizationExtension(string key, string arg1)
		{
			this.key = key;
			this.parameterNames = new string[1];
			this.parameterNames[0] = arg1;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020A7 File Offset: 0x000002A7
		public LocalizationExtension(string key)
		{
			this.key = key;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000004 RID: 4 RVA: 0x000020BC File Offset: 0x000002BC
		// (remove) Token: 0x06000005 RID: 5 RVA: 0x000020F8 File Offset: 0x000002F8
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002134 File Offset: 0x00000334
		public object Value
		{
			get
			{
				object result;
				if (this.parameterValues == null)
				{
					result = this.translatedValue;
				}
				else
				{
					result = string.Format(this.translatedValue, this.parameterValues);
				}
				return result;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002174 File Offset: 0x00000374
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			this.frameworkElement = null;
			this.parameters = null;
			IProvideValueTarget provideValueTarget = serviceProvider as IProvideValueTarget;
			if (provideValueTarget != null)
			{
				this.frameworkElement = (provideValueTarget.TargetObject as FrameworkElement);
				if (this.frameworkElement != null)
				{
					if (this.parameterNames != null)
					{
						if (this.frameworkElement.DataContext == null)
						{
							this.frameworkElement.DataContextChanged += this.FrameworkElementOnDataContextChanged;
						}
						else
						{
							this.AssignParameters();
						}
					}
				}
			}
			Binding binding = new Binding("Value")
			{
				Source = this,
				Mode = BindingMode.OneWay
			};
			this.ConnectEvents();
			this.UpdateParameters();
			this.UpdateTranslation();
			return binding.ProvideValue(serviceProvider);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002250 File Offset: 0x00000450
		private void AssignParameters()
		{
			this.parameters = new PropertyInfo[this.parameterNames.Length];
			for (int i = 0; i < this.parameters.Length; i++)
			{
				PropertyInfo property = this.frameworkElement.DataContext.GetType().GetProperty(this.parameterNames[i]);
				if (property != null)
				{
					this.parameters[i] = property;
				}
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022C1 File Offset: 0x000004C1
		private void FrameworkElementOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			this.frameworkElement.DataContextChanged -= this.FrameworkElementOnDataContextChanged;
			this.AssignParameters();
			this.ConnectEvents();
			this.UpdateParameters();
			this.UpdateTranslation();
			this.OnValueChanged();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002318 File Offset: 0x00000518
		private void ConnectEvents()
		{
			if (this.parameters != null)
			{
				bool flag = this.parameters.Any((PropertyInfo t) => t != null);
				if (flag)
				{
					INotifyPropertyChanged notifyPropertyChanged = this.frameworkElement.DataContext as INotifyPropertyChanged;
					if (notifyPropertyChanged != null)
					{
						notifyPropertyChanged.PropertyChanged += this.OnPropertyChanged;
					}
				}
			}
			LocalizationManager.Instance().LanguageChanged += this.OnLanguageChanged;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023B0 File Offset: 0x000005B0
		private void UpdateParameters()
		{
			if (this.parameters != null)
			{
				this.parameterValues = new object[this.parameters.Length];
				for (int i = 0; i < this.parameters.Length; i++)
				{
					if (this.parameters[i] != null)
					{
						this.parameterValues[i] = this.parameters[i].GetValue(this.frameworkElement.DataContext, null);
					}
					else
					{
						this.parameterValues[i] = "NOT FOUND";
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002442 File Offset: 0x00000642
		private void UpdateTranslation()
		{
			this.translatedValue = LocalizationManager.GetTranslation(this.key);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002456 File Offset: 0x00000656
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.UpdateParameters();
			this.OnValueChanged();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002467 File Offset: 0x00000667
		private void OnLanguageChanged(object sender, EventArgs e)
		{
			this.UpdateTranslation();
			this.OnValueChanged();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002478 File Offset: 0x00000678
		private void OnValueChanged()
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly string key;

		// Token: 0x04000002 RID: 2
		private readonly string[] parameterNames;

		// Token: 0x04000003 RID: 3
		private PropertyInfo[] parameters;

		// Token: 0x04000004 RID: 4
		private object[] parameterValues;

		// Token: 0x04000005 RID: 5
		private string translatedValue;

		// Token: 0x04000006 RID: 6
		private FrameworkElement frameworkElement;
	}
}

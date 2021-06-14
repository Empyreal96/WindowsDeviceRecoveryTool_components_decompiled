using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Provides high-level access to the definition of a binding, which connects the properties of binding target objects (typically, WPF elements), and any data source (for example, a database, an XML file, or any object that contains data).</summary>
	// Token: 0x0200019A RID: 410
	public class Binding : BindingBase
	{
		/// <summary>Adds a handler for the <see cref="E:System.Windows.Data.Binding.SourceUpdated" /> attached event. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to the event.</param>
		/// <param name="handler">The handler to add.</param>
		// Token: 0x060017D1 RID: 6097 RVA: 0x00073EC9 File Offset: 0x000720C9
		public static void AddSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.AddHandler(element, Binding.SourceUpdatedEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Data.Binding.SourceUpdated" /> attached event. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to the event.</param>
		/// <param name="handler">The handler to remove.</param>
		// Token: 0x060017D2 RID: 6098 RVA: 0x00073ED7 File Offset: 0x000720D7
		public static void RemoveSourceUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Binding.SourceUpdatedEvent, handler);
		}

		/// <summary>Adds a handler for the <see cref="E:System.Windows.Data.Binding.TargetUpdated" /> attached event. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to the event.</param>
		/// <param name="handler">The handler to add.</param>
		// Token: 0x060017D3 RID: 6099 RVA: 0x00073EE5 File Offset: 0x000720E5
		public static void AddTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.AddHandler(element, Binding.TargetUpdatedEvent, handler);
		}

		/// <summary>Removes a handler for the <see cref="E:System.Windows.Data.Binding.TargetUpdated" /> attached event. </summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> that listens to the event.</param>
		/// <param name="handler">The handler to remove.</param>
		// Token: 0x060017D4 RID: 6100 RVA: 0x00073EF3 File Offset: 0x000720F3
		public static void RemoveTargetUpdatedHandler(DependencyObject element, EventHandler<DataTransferEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Binding.TargetUpdatedEvent, handler);
		}

		/// <summary>Returns an XML namespace manager object used by the binding attached to the specified object. </summary>
		/// <param name="target">The object from which to get namespace information.</param>
		/// <returns>A returned object used for viewing XML namespaces that relate to the binding on the passed object element. This object should be cast as <see cref="T:System.Xml.XmlNamespaceManager" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="target" /> parameter cannot be <see langword="null" />.</exception>
		// Token: 0x060017D5 RID: 6101 RVA: 0x00073F01 File Offset: 0x00072101
		public static XmlNamespaceManager GetXmlNamespaceManager(DependencyObject target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			return (XmlNamespaceManager)target.GetValue(Binding.XmlNamespaceManagerProperty);
		}

		/// <summary>Sets a namespace manager object used by the binding attached to the provided element. </summary>
		/// <param name="target">The object from which to get namespace information.</param>
		/// <param name="value">The <see cref="T:System.Xml.XmlNamespaceManager" /> to use for namespace evaluation in the passed element.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="target" /> is <see langword="null" />. </exception>
		// Token: 0x060017D6 RID: 6102 RVA: 0x00073F21 File Offset: 0x00072121
		public static void SetXmlNamespaceManager(DependencyObject target, XmlNamespaceManager value)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			target.SetValue(Binding.XmlNamespaceManagerProperty, value);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00073F3D File Offset: 0x0007213D
		private static bool IsValidXmlNamespaceManager(object value)
		{
			return value == null || SystemXmlHelper.IsXmlNamespaceManager(value);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.Binding" /> class.</summary>
		// Token: 0x060017D8 RID: 6104 RVA: 0x00073F4A File Offset: 0x0007214A
		public Binding()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.Binding" /> class with an initial path.</summary>
		/// <param name="path">The initial <see cref="P:System.Windows.Data.Binding.Path" /> for the binding.</param>
		// Token: 0x060017D9 RID: 6105 RVA: 0x00073F5D File Offset: 0x0007215D
		public Binding(string path)
		{
			if (path != null)
			{
				if (Dispatcher.CurrentDispatcher == null)
				{
					throw new InvalidOperationException();
				}
				this.Path = new PropertyPath(path, null);
			}
		}

		/// <summary>Gets a collection of rules that check the validity of the user input.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.ValidationRule" /> objects.</returns>
		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060017DA RID: 6106 RVA: 0x00073F8D File Offset: 0x0007218D
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				if (!base.HasValue(BindingBase.Feature.ValidationRules))
				{
					base.SetValue(BindingBase.Feature.ValidationRules, new ValidationRuleCollection());
				}
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.Binding.ValidationRules" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x060017DB RID: 6107 RVA: 0x00073FB4 File Offset: 0x000721B4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeValidationRules()
		{
			return base.HasValue(BindingBase.Feature.ValidationRules) && this.ValidationRules.Count > 0;
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.ExceptionValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.ExceptionValidationRule" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x00073FD0 File Offset: 0x000721D0
		// (set) Token: 0x060017DD RID: 6109 RVA: 0x00073FE0 File Offset: 0x000721E0
		[DefaultValue(false)]
		public bool ValidatesOnExceptions
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnExceptions, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.DataErrorValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.DataErrorValidationRule" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x0007400F File Offset: 0x0007220F
		// (set) Token: 0x060017DF RID: 6111 RVA: 0x0007401C File Offset: 0x0007221C
		[DefaultValue(false)]
		public bool ValidatesOnDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnDataErrors, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />.</summary>
		/// <returns>
		///     <see langword="true" /> to include the <see cref="T:System.Windows.Controls.NotifyDataErrorValidationRule" />; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x060017E0 RID: 6112 RVA: 0x0007404B File Offset: 0x0007224B
		// (set) Token: 0x060017E1 RID: 6113 RVA: 0x00074058 File Offset: 0x00072258
		[DefaultValue(true)]
		public bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors, value);
				}
			}
		}

		/// <summary>Gets or sets the path to the binding source property.</summary>
		/// <returns>The path to the binding source. The default is <see langword="null" />.</returns>
		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060017E2 RID: 6114 RVA: 0x00074087 File Offset: 0x00072287
		// (set) Token: 0x060017E3 RID: 6115 RVA: 0x00074090 File Offset: 0x00072290
		public PropertyPath Path
		{
			get
			{
				return this._ppath;
			}
			set
			{
				base.CheckSealed();
				this._ppath = value;
				this._attachedPropertiesInPath = -1;
				base.ClearFlag(BindingBase.BindingFlags.PathGeneratedInternally);
				if (this._ppath == null || !this._ppath.StartsWithStaticProperty)
				{
					return;
				}
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.StaticSource || FrameworkCompatibilityPreferences.TargetsDesktop_V4_0)
				{
					this.SourceReference = Binding.StaticSourceRef;
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.StaticSource,
					this._sourceInUse
				}));
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.Binding.Path" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x060017E4 RID: 6116 RVA: 0x00074122 File Offset: 0x00072322
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializePath()
		{
			return this._ppath != null && !base.TestFlag(BindingBase.BindingFlags.PathGeneratedInternally);
		}

		/// <summary>Gets or sets an <see langword="XPath" /> query that returns the value on the XML binding source to use.</summary>
		/// <returns>The <see langword="XPath" /> query. The default is <see langword="null" />.</returns>
		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x060017E5 RID: 6117 RVA: 0x0007413C File Offset: 0x0007233C
		// (set) Token: 0x060017E6 RID: 6118 RVA: 0x0007414B File Offset: 0x0007234B
		[DefaultValue(null)]
		public string XPath
		{
			get
			{
				return (string)base.GetValue(BindingBase.Feature.XPath, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.XPath, value, null);
			}
		}

		/// <summary>Gets or sets a value that indicates the direction of the data flow in the binding.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Data.BindingMode" /> values. The default is <see cref="F:System.Windows.Data.BindingMode.Default" />, which returns the default binding mode value of the target dependency property. However, the default value varies for each dependency property. In general, user-editable control properties, such as those of text boxes and check boxes, default to two-way bindings, whereas most other properties default to one-way bindings.A programmatic way to determine whether a dependency property binds one-way or two-way by default is to get the property metadata of the property using <see cref="M:System.Windows.DependencyProperty.GetMetadata(System.Type)" /> and then check the Boolean value of the <see cref="P:System.Windows.FrameworkPropertyMetadata.BindsTwoWayByDefault" /> property.</returns>
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060017E7 RID: 6119 RVA: 0x0007415C File Offset: 0x0007235C
		// (set) Token: 0x060017E8 RID: 6120 RVA: 0x000741A4 File Offset: 0x000723A4
		[DefaultValue(BindingMode.Default)]
		public BindingMode Mode
		{
			get
			{
				switch (base.GetFlagsWithinMask(BindingBase.BindingFlags.PropagationMask))
				{
				case BindingBase.BindingFlags.OneTime:
					return BindingMode.OneTime;
				case BindingBase.BindingFlags.OneWay:
					return BindingMode.OneWay;
				case BindingBase.BindingFlags.OneWayToSource:
					return BindingMode.OneWayToSource;
				case BindingBase.BindingFlags.TwoWay:
					return BindingMode.TwoWay;
				case BindingBase.BindingFlags.PropDefault:
					return BindingMode.Default;
				default:
					Invariant.Assert(false, "Unexpected BindingMode value");
					return BindingMode.TwoWay;
				}
			}
			set
			{
				base.CheckSealed();
				BindingBase.BindingFlags bindingFlags = BindingBase.FlagsFrom(value);
				if (bindingFlags == BindingBase.BindingFlags.IllegalInput)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BindingMode));
				}
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, bindingFlags);
			}
		}

		/// <summary>Gets or sets a value that determines the timing of binding source updates.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> values. The default is <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />, which returns the default <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> value of the target dependency property. However, the default value for most dependency properties is <see cref="F:System.Windows.Data.UpdateSourceTrigger.PropertyChanged" />, while the <see cref="P:System.Windows.Controls.TextBox.Text" /> property has a default value of <see cref="F:System.Windows.Data.UpdateSourceTrigger.LostFocus" />.A programmatic way to determine the default <see cref="P:System.Windows.Data.Binding.UpdateSourceTrigger" /> value of a dependency property is to get the property metadata of the property using <see cref="M:System.Windows.DependencyProperty.GetMetadata(System.Type)" /> and then check the value of the <see cref="P:System.Windows.FrameworkPropertyMetadata.DefaultUpdateSourceTrigger" /> property.</returns>
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060017E9 RID: 6121 RVA: 0x000741E4 File Offset: 0x000723E4
		// (set) Token: 0x060017EA RID: 6122 RVA: 0x00074238 File Offset: 0x00072438
		[DefaultValue(UpdateSourceTrigger.Default)]
		public UpdateSourceTrigger UpdateSourceTrigger
		{
			get
			{
				BindingBase.BindingFlags flagsWithinMask = base.GetFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault);
				if (flagsWithinMask <= BindingBase.BindingFlags.UpdateOnLostFocus)
				{
					if (flagsWithinMask == BindingBase.BindingFlags.OneTime)
					{
						return UpdateSourceTrigger.PropertyChanged;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateOnLostFocus)
					{
						return UpdateSourceTrigger.LostFocus;
					}
				}
				else
				{
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateExplicitly)
					{
						return UpdateSourceTrigger.Explicit;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateDefault)
					{
						return UpdateSourceTrigger.Default;
					}
				}
				Invariant.Assert(false, "Unexpected UpdateSourceTrigger value");
				return UpdateSourceTrigger.Default;
			}
			set
			{
				base.CheckSealed();
				BindingBase.BindingFlags bindingFlags = BindingBase.FlagsFrom(value);
				if (bindingFlags == BindingBase.BindingFlags.IllegalInput)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(UpdateSourceTrigger));
				}
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault, bindingFlags);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.Data.Binding.SourceUpdated" /> event when a value is transferred from the binding target to the binding source.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.Data.Binding.SourceUpdated" /> event should be raised when the binding source value is updated; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060017EB RID: 6123 RVA: 0x0007427C File Offset: 0x0007247C
		// (set) Token: 0x060017EC RID: 6124 RVA: 0x0007428C File Offset: 0x0007248C
		[DefaultValue(false)]
		public bool NotifyOnSourceUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.Data.Binding.TargetUpdated" /> event when a value is transferred from the binding source to the binding target.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.Data.Binding.TargetUpdated" /> event should be raised when the binding target value is updated; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x000742BB File Offset: 0x000724BB
		// (set) Token: 0x060017EE RID: 6126 RVA: 0x000742C4 File Offset: 0x000724C4
		[DefaultValue(false)]
		public bool NotifyOnTargetUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated, value);
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to raise the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event on the bound object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event should be raised on the bound object when there is a validation error during source updates; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x000742EB File Offset: 0x000724EB
		// (set) Token: 0x060017F0 RID: 6128 RVA: 0x000742F8 File Offset: 0x000724F8
		[DefaultValue(false)]
		public bool NotifyOnValidationError
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
			}
			set
			{
				bool flag = base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
				if (flag != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnValidationError, value);
				}
			}
		}

		/// <summary>Gets or sets the converter to use.</summary>
		/// <returns>A value of type <see cref="T:System.Windows.Data.IValueConverter" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x060017F1 RID: 6129 RVA: 0x00074327 File Offset: 0x00072527
		// (set) Token: 0x060017F2 RID: 6130 RVA: 0x00074337 File Offset: 0x00072537
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return (IValueConverter)base.GetValue(BindingBase.Feature.Converter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Converter, value, null);
			}
		}

		/// <summary>Gets or sets the parameter to pass to the <see cref="P:System.Windows.Data.Binding.Converter" />.</summary>
		/// <returns>The parameter to pass to the <see cref="P:System.Windows.Data.Binding.Converter" />. The default is <see langword="null" />.</returns>
		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00074349 File Offset: 0x00072549
		// (set) Token: 0x060017F4 RID: 6132 RVA: 0x00074354 File Offset: 0x00072554
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return base.GetValue(BindingBase.Feature.ConverterParameter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.ConverterParameter, value, null);
			}
		}

		/// <summary>Gets or sets the culture in which to evaluate the converter.</summary>
		/// <returns>The default is <see langword="null" />.</returns>
		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00074366 File Offset: 0x00072566
		// (set) Token: 0x060017F6 RID: 6134 RVA: 0x00074375 File Offset: 0x00072575
		[DefaultValue(null)]
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture
		{
			get
			{
				return (CultureInfo)base.GetValue(BindingBase.Feature.Culture, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Culture, value, null);
			}
		}

		/// <summary>Gets or sets the object to use as the binding source.</summary>
		/// <returns>The object to use as the binding source.</returns>
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00074388 File Offset: 0x00072588
		// (set) Token: 0x060017F8 RID: 6136 RVA: 0x000743B8 File Offset: 0x000725B8
		public object Source
		{
			get
			{
				WeakReference<object> weakReference = (WeakReference<object>)base.GetValue(BindingBase.Feature.ObjectSource, null);
				if (weakReference == null)
				{
					return null;
				}
				object result;
				if (!weakReference.TryGetTarget(out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse != Binding.SourceProperties.None && this._sourceInUse != Binding.SourceProperties.Source)
				{
					throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
					{
						Binding.SourceProperties.Source,
						this._sourceInUse
					}));
				}
				if (value != DependencyProperty.UnsetValue)
				{
					base.SetValue(BindingBase.Feature.ObjectSource, new WeakReference<object>(value));
					this.SourceReference = new ExplicitObjectRef(value);
					return;
				}
				base.ClearValue(BindingBase.Feature.ObjectSource);
				this.SourceReference = null;
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.Binding.Source" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x060017F9 RID: 6137 RVA: 0x0000B02A File Offset: 0x0000922A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeSource()
		{
			return false;
		}

		/// <summary>Gets or sets the binding source by specifying its location relative to the position of the binding target.</summary>
		/// <returns>A <see cref="T:System.Windows.Data.RelativeSource" /> object specifying the relative location of the binding source to use. The default is <see langword="null" />.</returns>
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00074439 File Offset: 0x00072639
		// (set) Token: 0x060017FB RID: 6139 RVA: 0x0007444C File Offset: 0x0007264C
		[DefaultValue(null)]
		public RelativeSource RelativeSource
		{
			get
			{
				return (RelativeSource)base.GetValue(BindingBase.Feature.RelativeSource, null);
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.RelativeSource)
				{
					base.SetValue(BindingBase.Feature.RelativeSource, value, null);
					this.SourceReference = ((value != null) ? new RelativeObjectRef(value) : null);
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.RelativeSource,
					this._sourceInUse
				}));
			}
		}

		/// <summary>Gets or sets the name of the element to use as the binding source object.</summary>
		/// <returns>The value of the <see langword="Name" /> property or x:Name Directive of the element of interest. You can refer to elements in code only if they are registered to the appropriate <see cref="T:System.Windows.NameScope" /> through <see langword="RegisterName" />. For more information, see WPF XAML Namescopes.The default is <see langword="null" />.</returns>
		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x000744B9 File Offset: 0x000726B9
		// (set) Token: 0x060017FD RID: 6141 RVA: 0x000744CC File Offset: 0x000726CC
		[DefaultValue(null)]
		public string ElementName
		{
			get
			{
				return (string)base.GetValue(BindingBase.Feature.ElementSource, null);
			}
			set
			{
				base.CheckSealed();
				if (this._sourceInUse == Binding.SourceProperties.None || this._sourceInUse == Binding.SourceProperties.ElementName)
				{
					base.SetValue(BindingBase.Feature.ElementSource, value, null);
					this.SourceReference = ((value != null) ? new ElementObjectRef(value) : null);
					return;
				}
				throw new InvalidOperationException(SR.Get("BindingConflict", new object[]
				{
					Binding.SourceProperties.ElementName,
					this._sourceInUse
				}));
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Data.Binding" /> should get and set values asynchronously.</summary>
		/// <returns>The default is <see langword="null" />.</returns>
		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x00074539 File Offset: 0x00072739
		// (set) Token: 0x060017FF RID: 6143 RVA: 0x00074541 File Offset: 0x00072741
		[DefaultValue(false)]
		public bool IsAsync
		{
			get
			{
				return this._isAsync;
			}
			set
			{
				base.CheckSealed();
				this._isAsync = value;
			}
		}

		/// <summary>Gets or sets opaque data passed to the asynchronous data dispatcher.</summary>
		/// <returns>Data passed to the asynchronous data dispatcher.</returns>
		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x00074550 File Offset: 0x00072750
		// (set) Token: 0x06001801 RID: 6145 RVA: 0x0007455A File Offset: 0x0007275A
		[DefaultValue(null)]
		public object AsyncState
		{
			get
			{
				return base.GetValue(BindingBase.Feature.AsyncState, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.AsyncState, value, null);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to evaluate the <see cref="P:System.Windows.Data.Binding.Path" /> relative to the data item or the <see cref="T:System.Windows.Data.DataSourceProvider" /> object.</summary>
		/// <returns>
		///     <see langword="false" /> to evaluate the path relative to the data item itself; otherwise, <see langword="true" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x0007456B File Offset: 0x0007276B
		// (set) Token: 0x06001803 RID: 6147 RVA: 0x00074573 File Offset: 0x00072773
		[DefaultValue(false)]
		public bool BindsDirectlyToSource
		{
			get
			{
				return this._bindsDirectlyToSource;
			}
			set
			{
				base.CheckSealed();
				this._bindsDirectlyToSource = value;
			}
		}

		/// <summary>Gets or sets a handler you can use to provide custom logic for handling exceptions that the binding engine encounters during the update of the binding source value. This is only applicable if you have associated an <see cref="T:System.Windows.Controls.ExceptionValidationRule" /> with your binding.</summary>
		/// <returns>A method that provides custom logic for handling exceptions that the binding engine encounters during the update of the binding source value.</returns>
		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x00074582 File Offset: 0x00072782
		// (set) Token: 0x06001805 RID: 6149 RVA: 0x00074592 File Offset: 0x00072792
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
		{
			get
			{
				return (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			}
			set
			{
				base.SetValue(BindingBase.Feature.ExceptionFilterCallback, value, null);
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0007459E File Offset: 0x0007279E
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			return BindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000745A9 File Offset: 0x000727A9
		internal override ValidationRule LookupValidationRule(Type type)
		{
			return BindingBase.LookupValidationRule(type, this.ValidationRulesInternal);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000745B8 File Offset: 0x000727B8
		internal object DoFilterException(object bindExpr, Exception exception)
		{
			UpdateSourceExceptionFilterCallback updateSourceExceptionFilterCallback = (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			if (updateSourceExceptionFilterCallback != null)
			{
				return updateSourceExceptionFilterCallback(bindExpr, exception);
			}
			return exception;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000745E1 File Offset: 0x000727E1
		internal void UsePath(PropertyPath path)
		{
			this._ppath = path;
			base.SetFlag(BindingBase.BindingFlags.PathGeneratedInternally);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000745F5 File Offset: 0x000727F5
		internal override BindingBase CreateClone()
		{
			return new Binding();
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000745FC File Offset: 0x000727FC
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			Binding binding = (Binding)baseClone;
			binding._ppath = this._ppath;
			base.CopyValue(BindingBase.Feature.XPath, binding);
			binding._source = this._source;
			base.CopyValue(BindingBase.Feature.Culture, binding);
			binding._isAsync = this._isAsync;
			base.CopyValue(BindingBase.Feature.AsyncState, binding);
			binding._bindsDirectlyToSource = this._bindsDirectlyToSource;
			binding._doesNotTransferDefaultValue = this._doesNotTransferDefaultValue;
			base.CopyValue(BindingBase.Feature.ObjectSource, binding);
			base.CopyValue(BindingBase.Feature.RelativeSource, binding);
			base.CopyValue(BindingBase.Feature.Converter, binding);
			base.CopyValue(BindingBase.Feature.ConverterParameter, binding);
			binding._attachedPropertiesInPath = this._attachedPropertiesInPath;
			base.CopyValue(BindingBase.Feature.ValidationRules, binding);
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x000746A4 File Offset: 0x000728A4
		internal override CultureInfo ConverterCultureInternal
		{
			get
			{
				return this.ConverterCulture;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x000746AC File Offset: 0x000728AC
		// (set) Token: 0x0600180E RID: 6158 RVA: 0x000746C3 File Offset: 0x000728C3
		internal ObjectRef SourceReference
		{
			get
			{
				if (this._source != Binding.UnsetSource)
				{
					return this._source;
				}
				return null;
			}
			set
			{
				base.CheckSealed();
				this._source = value;
				this.DetermineSource();
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x000746D8 File Offset: 0x000728D8
		internal bool TreeContextIsRequired
		{
			get
			{
				if (this._attachedPropertiesInPath < 0)
				{
					if (this._ppath != null)
					{
						this._attachedPropertiesInPath = this._ppath.ComputeUnresolvedAttachedPropertiesInPath();
					}
					else
					{
						this._attachedPropertiesInPath = 0;
					}
				}
				bool flag = this._attachedPropertiesInPath > 0;
				if (!flag && base.HasValue(BindingBase.Feature.XPath) && this.XPath.IndexOf(':') >= 0)
				{
					flag = true;
				}
				return flag;
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x00074739 File Offset: 0x00072939
		internal override Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001811 RID: 6161 RVA: 0x00074749 File Offset: 0x00072949
		// (set) Token: 0x06001812 RID: 6162 RVA: 0x00074754 File Offset: 0x00072954
		internal bool TransfersDefaultValue
		{
			get
			{
				return !this._doesNotTransferDefaultValue;
			}
			set
			{
				base.CheckSealed();
				this._doesNotTransferDefaultValue = !value;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x00074766 File Offset: 0x00072966
		internal override bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return this.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00074770 File Offset: 0x00072970
		private void DetermineSource()
		{
			this._sourceInUse = ((this._source == Binding.UnsetSource) ? Binding.SourceProperties.None : (base.HasValue(BindingBase.Feature.RelativeSource) ? Binding.SourceProperties.RelativeSource : (base.HasValue(BindingBase.Feature.ElementSource) ? Binding.SourceProperties.ElementName : (base.HasValue(BindingBase.Feature.ObjectSource) ? Binding.SourceProperties.Source : ((this._source == Binding.StaticSourceRef) ? Binding.SourceProperties.StaticSource : Binding.SourceProperties.InternalSource)))));
		}

		/// <summary>Identifies the <see cref="E:System.Windows.Data.Binding.SourceUpdated" /> attached event.</summary>
		// Token: 0x040012E8 RID: 4840
		public static readonly RoutedEvent SourceUpdatedEvent = EventManager.RegisterRoutedEvent("SourceUpdated", RoutingStrategy.Bubble, typeof(EventHandler<DataTransferEventArgs>), typeof(Binding));

		/// <summary>Identifies the <see cref="E:System.Windows.Data.Binding.TargetUpdated" /> attached event.</summary>
		// Token: 0x040012E9 RID: 4841
		public static readonly RoutedEvent TargetUpdatedEvent = EventManager.RegisterRoutedEvent("TargetUpdated", RoutingStrategy.Bubble, typeof(EventHandler<DataTransferEventArgs>), typeof(Binding));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.Binding.XmlNamespaceManager" /> attached property.</summary>
		// Token: 0x040012EA RID: 4842
		public static readonly DependencyProperty XmlNamespaceManagerProperty = DependencyProperty.RegisterAttached("XmlNamespaceManager", typeof(object), typeof(Binding), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits), new ValidateValueCallback(Binding.IsValidXmlNamespaceManager));

		/// <summary>Used as a returned value to instruct the binding engine not to perform any action.</summary>
		// Token: 0x040012EB RID: 4843
		public static readonly object DoNothing = new NamedObject("Binding.DoNothing");

		/// <summary>Used as the <see cref="P:System.ComponentModel.PropertyChangedEventArgs.PropertyName" /> of <see cref="T:System.ComponentModel.PropertyChangedEventArgs" /> to indicate that an indexer property has changed. </summary>
		// Token: 0x040012EC RID: 4844
		public const string IndexerName = "Item[]";

		// Token: 0x040012ED RID: 4845
		private Binding.SourceProperties _sourceInUse;

		// Token: 0x040012EE RID: 4846
		private PropertyPath _ppath;

		// Token: 0x040012EF RID: 4847
		private ObjectRef _source = Binding.UnsetSource;

		// Token: 0x040012F0 RID: 4848
		private bool _isAsync;

		// Token: 0x040012F1 RID: 4849
		private bool _bindsDirectlyToSource;

		// Token: 0x040012F2 RID: 4850
		private bool _doesNotTransferDefaultValue;

		// Token: 0x040012F3 RID: 4851
		private int _attachedPropertiesInPath;

		// Token: 0x040012F4 RID: 4852
		private static readonly ObjectRef UnsetSource = new ExplicitObjectRef(null);

		// Token: 0x040012F5 RID: 4853
		private static readonly ObjectRef StaticSourceRef = new ExplicitObjectRef(BindingExpression.StaticSource);

		// Token: 0x02000860 RID: 2144
		private enum SourceProperties : byte
		{
			// Token: 0x04004095 RID: 16533
			None,
			// Token: 0x04004096 RID: 16534
			RelativeSource,
			// Token: 0x04004097 RID: 16535
			ElementName,
			// Token: 0x04004098 RID: 16536
			Source,
			// Token: 0x04004099 RID: 16537
			StaticSource,
			// Token: 0x0400409A RID: 16538
			InternalSource
		}
	}
}

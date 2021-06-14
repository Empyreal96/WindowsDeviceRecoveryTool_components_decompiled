using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	/// <summary>Represents the base class for specifying a conditional value within a <see cref="T:System.Windows.Style" /> object. </summary>
	// Token: 0x02000134 RID: 308
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public abstract class TriggerBase : DependencyObject
	{
		// Token: 0x06000CB2 RID: 3250 RVA: 0x0002F2D4 File Offset: 0x0002D4D4
		internal TriggerBase()
		{
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.TriggerAction" /> objects to apply when the trigger object becomes active. This property does not apply to the <see cref="T:System.Windows.EventTrigger" /> class.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x0002F6A6 File Offset: 0x0002D8A6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection EnterActions
		{
			get
			{
				base.VerifyAccess();
				if (this._enterActions == null)
				{
					this._enterActions = new TriggerActionCollection();
					if (base.IsSealed)
					{
						this._enterActions.Seal(this);
					}
				}
				return this._enterActions;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0002F6DB File Offset: 0x0002D8DB
		internal bool HasEnterActions
		{
			get
			{
				return this._enterActions != null && this._enterActions.Count > 0;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.TriggerAction" /> objects to apply when the trigger object becomes inactive. This property does not apply to the <see cref="T:System.Windows.EventTrigger" /> class.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0002F6F5 File Offset: 0x0002D8F5
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection ExitActions
		{
			get
			{
				base.VerifyAccess();
				if (this._exitActions == null)
				{
					this._exitActions = new TriggerActionCollection();
					if (base.IsSealed)
					{
						this._exitActions.Seal(this);
					}
				}
				return this._exitActions;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0002F72A File Offset: 0x0002D92A
		internal bool HasExitActions
		{
			get
			{
				return this._exitActions != null && this._exitActions.Count > 0;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00016748 File Offset: 0x00014948
		internal bool ExecuteEnterActionsOnApply
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0000B02A File Offset: 0x0000922A
		internal bool ExecuteExitActionsOnApply
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002F744 File Offset: 0x0002D944
		internal void ProcessParametersContainer(DependencyProperty dp)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				throw new ArgumentException(SR.Get("StylePropertyInStyleNotAllowed"));
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002F75E File Offset: 0x0002D95E
		internal string ProcessParametersVisualTreeChild(DependencyProperty dp, string target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (target.Length == 0)
			{
				throw new ArgumentException(SR.Get("ChildNameMustBeNonEmpty"));
			}
			return string.Intern(target);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0002F78C File Offset: 0x0002D98C
		internal void AddToPropertyValues(string childName, DependencyProperty dp, object value, PropertyValueType valueType)
		{
			this.PropertyValues.Add(new PropertyValue
			{
				ValueType = valueType,
				Conditions = null,
				ChildName = childName,
				Property = dp,
				ValueInternal = value
			});
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002F7D8 File Offset: 0x0002D9D8
		internal override void Seal()
		{
			base.VerifyAccess();
			base.Seal();
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = this.PropertyValues[i];
				DependencyProperty property = propertyValue.Property;
				for (int j = 0; j < propertyValue.Conditions.Length; j++)
				{
					DependencyProperty property2 = propertyValue.Conditions[j].Property;
					if (property2 == property && propertyValue.ChildName == "~Self")
					{
						throw new InvalidOperationException(SR.Get("PropertyTriggerCycleDetected", new object[]
						{
							property2.Name
						}));
					}
				}
			}
			if (this._enterActions != null)
			{
				this._enterActions.Seal(this);
			}
			if (this._exitActions != null)
			{
				this._exitActions.Seal(this);
			}
			base.DetachFromDispatcher();
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0002F8AC File Offset: 0x0002DAAC
		internal void ProcessSettersCollection(SetterBaseCollection setters)
		{
			if (setters != null)
			{
				setters.Seal();
				for (int i = 0; i < setters.Count; i++)
				{
					Setter setter = setters[i] as Setter;
					if (setter == null)
					{
						throw new InvalidOperationException(SR.Get("VisualTriggerSettersIncludeUnsupportedSetterType", new object[]
						{
							setters[i].GetType().Name
						}));
					}
					DependencyProperty property = setter.Property;
					object valueInternal = setter.ValueInternal;
					string text = setter.TargetName;
					if (text == null)
					{
						this.ProcessParametersContainer(property);
						text = "~Self";
					}
					else
					{
						text = this.ProcessParametersVisualTreeChild(property, text);
					}
					DynamicResourceExtension dynamicResourceExtension = valueInternal as DynamicResourceExtension;
					if (dynamicResourceExtension == null)
					{
						this.AddToPropertyValues(text, property, valueInternal, PropertyValueType.Trigger);
					}
					else
					{
						this.AddToPropertyValues(text, property, dynamicResourceExtension.ResourceKey, PropertyValueType.PropertyTriggerResource);
					}
				}
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0002F977 File Offset: 0x0002DB77
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0002F97F File Offset: 0x0002DB7F
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0002F994 File Offset: 0x0002DB94
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002F9A9 File Offset: 0x0002DBA9
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0002F9B1 File Offset: 0x0002DBB1
		internal long Layer
		{
			get
			{
				return this._globalLayerRank;
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		internal void EstablishLayer()
		{
			if (this._globalLayerRank == 0L)
			{
				object synchronized = TriggerBase.Synchronized;
				lock (synchronized)
				{
					long nextGlobalLayerRank = TriggerBase._nextGlobalLayerRank;
					TriggerBase._nextGlobalLayerRank = nextGlobalLayerRank + 1L;
					this._globalLayerRank = nextGlobalLayerRank;
				}
				if (TriggerBase._nextGlobalLayerRank == 9223372036854775807L)
				{
					throw new InvalidOperationException(SR.Get("PropertyTriggerLayerLimitExceeded"));
				}
			}
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return false;
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002FA34 File Offset: 0x0002DC34
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0002FA3C File Offset: 0x0002DC3C
		internal TriggerCondition[] TriggerConditions
		{
			get
			{
				return this._triggerConditions;
			}
			set
			{
				this._triggerConditions = value;
			}
		}

		// Token: 0x04000B19 RID: 2841
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x04000B1A RID: 2842
		private static object Synchronized = new object();

		// Token: 0x04000B1B RID: 2843
		private TriggerCondition[] _triggerConditions;

		// Token: 0x04000B1C RID: 2844
		private DependencyObject _inheritanceContext;

		// Token: 0x04000B1D RID: 2845
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x04000B1E RID: 2846
		private TriggerActionCollection _enterActions;

		// Token: 0x04000B1F RID: 2847
		private TriggerActionCollection _exitActions;

		// Token: 0x04000B20 RID: 2848
		private long _globalLayerRank;

		// Token: 0x04000B21 RID: 2849
		private static long _nextGlobalLayerRank = Storyboard.Layers.PropertyTriggerStartLayer;
	}
}

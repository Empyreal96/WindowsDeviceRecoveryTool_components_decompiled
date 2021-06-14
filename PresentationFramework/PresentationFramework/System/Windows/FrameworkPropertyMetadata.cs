using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace System.Windows
{
	/// <summary>Reports or applies metadata for a dependency property, specifically adding framework-specific property system characteristics.</summary>
	// Token: 0x020000C9 RID: 201
	public class FrameworkPropertyMetadata : UIPropertyMetadata
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class. </summary>
		// Token: 0x0600069E RID: 1694 RVA: 0x0001517F File Offset: 0x0001337F
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata()
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the specified default value. </summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of a specific type.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x0600069F RID: 1695 RVA: 0x0001518D File Offset: 0x0001338D
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue) : base(defaultValue)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the specified <see cref="T:System.Windows.PropertyChangedCallback" /> callback.</summary>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		// Token: 0x060006A0 RID: 1696 RVA: 0x0001519C File Offset: 0x0001339C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback) : base(propertyChangedCallback)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the specified callbacks. </summary>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <param name="coerceValueCallback">A reference to a handler implementation will be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> for this dependency property.</param>
		// Token: 0x060006A1 RID: 1697 RVA: 0x000151AB File Offset: 0x000133AB
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(propertyChangedCallback)
		{
			this.Initialize();
			base.CoerceValueCallback = coerceValueCallback;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and specified <see cref="T:System.Windows.PropertyChangedCallback" /> callback. </summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of a specific type.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A2 RID: 1698 RVA: 0x000151C1 File Offset: 0x000133C1
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, propertyChangedCallback)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and specified callbacks.</summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a specific type.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <param name="coerceValueCallback">A reference to a handler implementation that will be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> for this dependency property.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A3 RID: 1699 RVA: 0x000151D1 File Offset: 0x000133D1
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(defaultValue, propertyChangedCallback, coerceValueCallback)
		{
			this.Initialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and framework-level metadata options. </summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of a specific type.</param>
		/// <param name="flags">The metadata option flags (a combination of <see cref="T:System.Windows.FrameworkPropertyMetadataOptions" /> values). These options specify characteristics of the dependency property that interact with systems such as layout or data binding.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A4 RID: 1700 RVA: 0x000151E2 File Offset: 0x000133E2
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags) : base(defaultValue)
		{
			this.TranslateFlags(flags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and framework metadata options, and specified <see cref="T:System.Windows.PropertyChangedCallback" /> callback. </summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a value of a specific type.</param>
		/// <param name="flags">The metadata option flags (a combination of <see cref="T:System.Windows.FrameworkPropertyMetadataOptions" /> values). These options specify characteristics of the dependency property that interact with systems such as layout or data binding.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A5 RID: 1701 RVA: 0x000151F2 File Offset: 0x000133F2
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback) : base(defaultValue, propertyChangedCallback)
		{
			this.TranslateFlags(flags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and framework metadata options, and specified callbacks. </summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a specific type.</param>
		/// <param name="flags">The metadata option flags (a combination of <see cref="T:System.Windows.FrameworkPropertyMetadataOptions" /> values). These options specify characteristics of the dependency property that interact with systems such as layout or data binding.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <param name="coerceValueCallback">A reference to a handler implementation that will be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> against this property.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A6 RID: 1702 RVA: 0x00015203 File Offset: 0x00013403
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback) : base(defaultValue, propertyChangedCallback, coerceValueCallback)
		{
			this.TranslateFlags(flags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and framework metadata options, specified callbacks, and a Boolean that can be used to prevent animation of the property.</summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a specific type.</param>
		/// <param name="flags">The metadata option flags (a combination of <see cref="T:System.Windows.FrameworkPropertyMetadataOptions" /> values). These options specify characteristics of the dependency property that interact with systems such as layout or data binding.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <param name="coerceValueCallback">A reference to a handler implementation that will be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> on this dependency property.</param>
		/// <param name="isAnimationProhibited">
		///       <see langword="true" /> to prevent the property system from animating the property that this metadata is applied to. Such properties will raise a run-time exception originating from the property system if animations of them are attempted. <see langword="false" /> to permit animating the property. The default is <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A7 RID: 1703 RVA: 0x00015216 File Offset: 0x00013416
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback, bool isAnimationProhibited) : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
		{
			this.TranslateFlags(flags);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.FrameworkPropertyMetadata" /> class with the provided default value and framework metadata options, specified callbacks, a Boolean that can be used to prevent animation of the property, and a data-binding update trigger default.</summary>
		/// <param name="defaultValue">The default value of the dependency property, usually provided as a specific type.</param>
		/// <param name="flags">The metadata option flags (a combination of <see cref="T:System.Windows.FrameworkPropertyMetadataOptions" /> values). These options specify characteristics of the dependency property that interact with systems such as layout or data binding.</param>
		/// <param name="propertyChangedCallback">A reference to a handler implementation that the property system will call whenever the effective value of the property changes.</param>
		/// <param name="coerceValueCallback">A reference to a handler implementation that will be called whenever the property system calls <see cref="M:System.Windows.DependencyObject.CoerceValue(System.Windows.DependencyProperty)" /> against this property.</param>
		/// <param name="isAnimationProhibited">
		///       <see langword="true" /> to prevent the property system from animating the property that this metadata is applied to. Such properties will raise a run-time exception originating from the property system if animations of them are attempted. The default is <see langword="false" />.</param>
		/// <param name="defaultUpdateSourceTrigger">The <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> to use when bindings for this property are applied that have their <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> set to <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="defaultValue" /> is set to <see cref="F:System.Windows.DependencyProperty.UnsetValue" />; see Remarks.</exception>
		// Token: 0x060006A8 RID: 1704 RVA: 0x0001522C File Offset: 0x0001342C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FrameworkPropertyMetadata(object defaultValue, FrameworkPropertyMetadataOptions flags, PropertyChangedCallback propertyChangedCallback, CoerceValueCallback coerceValueCallback, bool isAnimationProhibited, UpdateSourceTrigger defaultUpdateSourceTrigger) : base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
		{
			if (!BindingOperations.IsValidUpdateSourceTrigger(defaultUpdateSourceTrigger))
			{
				throw new InvalidEnumArgumentException("defaultUpdateSourceTrigger", (int)defaultUpdateSourceTrigger, typeof(UpdateSourceTrigger));
			}
			if (defaultUpdateSourceTrigger == UpdateSourceTrigger.Default)
			{
				throw new ArgumentException(SR.Get("NoDefaultUpdateSourceTrigger"), "defaultUpdateSourceTrigger");
			}
			this.TranslateFlags(flags);
			this.DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001528D File Offset: 0x0001348D
		private void Initialize()
		{
			this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerEnumBit1);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000152A7 File Offset: 0x000134A7
		private static bool IsFlagSet(FrameworkPropertyMetadataOptions flag, FrameworkPropertyMetadataOptions flags)
		{
			return (flags & flag) > FrameworkPropertyMetadataOptions.None;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000152B0 File Offset: 0x000134B0
		private void TranslateFlags(FrameworkPropertyMetadataOptions flags)
		{
			this.Initialize();
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsMeasure, flags))
			{
				this.AffectsMeasure = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsArrange, flags))
			{
				this.AffectsArrange = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentMeasure, flags))
			{
				this.AffectsParentMeasure = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsParentArrange, flags))
			{
				this.AffectsParentArrange = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.AffectsRender, flags))
			{
				this.AffectsRender = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.Inherits, flags))
			{
				base.IsInherited = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior, flags))
			{
				this.OverridesInheritanceBehavior = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.NotDataBindable, flags))
			{
				this.IsNotDataBindable = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, flags))
			{
				this.BindsTwoWayByDefault = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.Journal, flags))
			{
				this.Journal = true;
			}
			if (FrameworkPropertyMetadata.IsFlagSet(FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, flags))
			{
				this.SubPropertiesDoNotAffectRender = true;
			}
		}

		/// <summary> Gets or sets a value that indicates whether a dependency property potentially affects the measure pass during layout engine operations. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists potentially affects the measure pass; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x00015386 File Offset: 0x00013586
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x00015390 File Offset: 0x00013590
		public bool AffectsMeasure
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether a dependency property potentially affects the arrange pass during layout engine operations. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists potentially affects the arrange pass; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x000153B3 File Offset: 0x000135B3
		// (set) Token: 0x060006AF RID: 1711 RVA: 0x000153C0 File Offset: 0x000135C0
		public bool AffectsArrange
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether a dependency property potentially affects the measure pass of its parent element's layout during layout engine operations. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists potentially affects the measure pass specifically on its parent element; otherwise, <see langword="false" />.The default is <see langword="false" />. </returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x000153E6 File Offset: 0x000135E6
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x000153F3 File Offset: 0x000135F3
		public bool AffectsParentMeasure
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether a dependency property potentially affects the arrange pass of its parent element's layout during layout engine operations. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists potentially affects the arrange pass specifically on its parent element; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00015419 File Offset: 0x00013619
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00015426 File Offset: 0x00013626
		public bool AffectsParentArrange
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a dependency property potentially affects the general layout in some way that does not specifically influence arrangement or measurement, but would require a redraw. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists affects rendering; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0001544C File Offset: 0x0001364C
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00015459 File Offset: 0x00013659
		public bool AffectsRender
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether the value of the dependency property is inheritable. </summary>
		/// <returns>
		///     <see langword="true" /> if the property value is inheritable; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001547F File Offset: 0x0001367F
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00015487 File Offset: 0x00013687
		public bool Inherits
		{
			get
			{
				return base.IsInherited;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.IsInherited = value;
				this.SetModified(PropertyMetadata.MetadataFlags.FW_InheritsModifiedID);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the property value inheritance evaluation should span across certain content boundaries in the logical tree of elements. </summary>
		/// <returns>
		///     <see langword="true" /> if the property value inheritance should span across certain content boundaries; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x000154B3 File Offset: 0x000136B3
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x000154C0 File Offset: 0x000136C0
		public bool OverridesInheritanceBehavior
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorModifiedID);
			}
		}

		/// <summary> Gets or sets a value that indicates whether the dependency property supports data binding. </summary>
		/// <returns>
		///     <see langword="true" /> if the property does not support data binding; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x000154F1 File Offset: 0x000136F1
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x000154FE File Offset: 0x000136FE
		public bool IsNotDataBindable
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID, value);
			}
		}

		/// <summary> Gets or sets a value that indicates whether the property binds two-way by default. </summary>
		/// <returns>
		///     <see langword="true" /> if the dependency property on which this metadata exists binds two-way by default; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00015524 File Offset: 0x00013724
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00015531 File Offset: 0x00013731
		public bool BindsTwoWayByDefault
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID, value);
			}
		}

		/// <summary>Gets or sets the default for <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> to use when bindings for the property with this metadata are applied, which have their <see cref="T:System.Windows.Data.UpdateSourceTrigger" /> set to <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />.</summary>
		/// <returns>A value of the enumeration, other than <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />.</returns>
		/// <exception cref="T:System.ArgumentException">This property is set to <see cref="F:System.Windows.Data.UpdateSourceTrigger.Default" />; the value you set is supposed to become the default when requested by bindings.</exception>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00015557 File Offset: 0x00013757
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x00015564 File Offset: 0x00013764
		public UpdateSourceTrigger DefaultUpdateSourceTrigger
		{
			get
			{
				return (UpdateSourceTrigger)(this._flags >> 30 & (PropertyMetadata.MetadataFlags)3U);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				if (!BindingOperations.IsValidUpdateSourceTrigger(value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(UpdateSourceTrigger));
				}
				if (value == UpdateSourceTrigger.Default)
				{
					throw new ArgumentException(SR.Get("NoDefaultUpdateSourceTrigger"), "value");
				}
				this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | (PropertyMetadata.MetadataFlags)(value << 30));
				this.SetModified(PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerModifiedID);
			}
		}

		/// <summary> Gets or sets a value that indicates whether this property contains journaling information that applications can or should store as part of a journaling implementation. </summary>
		/// <returns>
		///     <see langword="true" /> if journaling should be performed on the dependency property that this metadata is applied to; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x000155E1 File Offset: 0x000137E1
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x000155EE File Offset: 0x000137EE
		public bool Journal
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledModifiedID);
			}
		}

		/// <summary>Gets or sets a value that indicates whether sub-properties of the dependency property do not affect the rendering of the containing object. </summary>
		/// <returns>
		///     <see langword="true" /> if changes to sub-property values do not affect rendering if changed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The metadata has already been applied to a dependency property operation, so that metadata is sealed and properties of the metadata cannot be set.</exception>
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0001561F File Offset: 0x0001381F
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x0001562C File Offset: 0x0001382C
		public bool SubPropertiesDoNotAffectRender
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID, value);
				this.SetModified(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderModifiedID);
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0001565D File Offset: 0x0001385D
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0001566A File Offset: 0x0001386A
		private bool ReadOnly
		{
			get
			{
				return base.ReadFlag(PropertyMetadata.MetadataFlags.FW_ReadOnlyID);
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("TypeMetadataCannotChangeAfterUse"));
				}
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ReadOnlyID, value);
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00015690 File Offset: 0x00013890
		internal override PropertyMetadata CreateInstance()
		{
			return new FrameworkPropertyMetadata();
		}

		/// <summary>Enables a merge of the source metadata with base metadata. </summary>
		/// <param name="baseMetadata">The base metadata to merge.</param>
		/// <param name="dp">The dependency property this metadata is being applied to.</param>
		// Token: 0x060006C7 RID: 1735 RVA: 0x00015698 File Offset: 0x00013898
		protected override void Merge(PropertyMetadata baseMetadata, DependencyProperty dp)
		{
			base.Merge(baseMetadata, dp);
			FrameworkPropertyMetadata frameworkPropertyMetadata = baseMetadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsMeasureID) | frameworkPropertyMetadata.AffectsMeasure);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsArrangeID) | frameworkPropertyMetadata.AffectsArrange);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentMeasureID) | frameworkPropertyMetadata.AffectsParentMeasure);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsParentArrangeID) | frameworkPropertyMetadata.AffectsParentArrange);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_AffectsRenderID) | frameworkPropertyMetadata.AffectsRender);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_BindsTwoWayByDefaultID) | frameworkPropertyMetadata.BindsTwoWayByDefault);
				base.WriteFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID, base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID) | frameworkPropertyMetadata.IsNotDataBindable);
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_SubPropertiesDoNotAffectRenderID, frameworkPropertyMetadata.SubPropertiesDoNotAffectRender);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_InheritsModifiedID))
				{
					base.IsInherited = frameworkPropertyMetadata.Inherits;
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_OverridesInheritanceBehaviorID, frameworkPropertyMetadata.OverridesInheritanceBehavior);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledModifiedID))
				{
					base.WriteFlag(PropertyMetadata.MetadataFlags.FW_ShouldBeJournaledID, frameworkPropertyMetadata.Journal);
				}
				if (!this.IsModified(PropertyMetadata.MetadataFlags.FW_DefaultUpdateSourceTriggerModifiedID))
				{
					this._flags = ((this._flags & (PropertyMetadata.MetadataFlags)1073741823U) | (PropertyMetadata.MetadataFlags)(frameworkPropertyMetadata.DefaultUpdateSourceTrigger << 30));
				}
			}
		}

		/// <summary>Called when this metadata has been applied to a property, which indicates that the metadata is being sealed. </summary>
		/// <param name="dp">The dependency property to which the metadata has been applied.</param>
		/// <param name="targetType">The type associated with this metadata if this is type-specific metadata. If this is default metadata, this value can be <see langword="null" />.</param>
		// Token: 0x060006C8 RID: 1736 RVA: 0x0001581B File Offset: 0x00013A1B
		protected override void OnApply(DependencyProperty dp, Type targetType)
		{
			this.ReadOnly = dp.ReadOnly;
			base.OnApply(dp, targetType);
		}

		/// <summary> Gets a value that indicates whether data binding is supported for the dependency property. </summary>
		/// <returns>
		///     <see langword="true" /> if data binding is supported on the dependency property to which this metadata applies; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006C9 RID: 1737 RVA: 0x00015831 File Offset: 0x00013A31
		public bool IsDataBindingAllowed
		{
			get
			{
				return !base.ReadFlag(PropertyMetadata.MetadataFlags.FW_IsNotDataBindableID) && !this.ReadOnly;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001584B File Offset: 0x00013A4B
		internal void SetModified(PropertyMetadata.MetadataFlags id)
		{
			base.WriteFlag(id, true);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00015855 File Offset: 0x00013A55
		internal bool IsModified(PropertyMetadata.MetadataFlags id)
		{
			return base.ReadFlag(id);
		}
	}
}

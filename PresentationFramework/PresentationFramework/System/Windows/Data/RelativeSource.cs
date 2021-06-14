using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Data
{
	/// <summary>Implements a markup extension that describes the location of the binding source relative to the position of the binding target.</summary>
	// Token: 0x020001BB RID: 443
	[MarkupExtensionReturnType(typeof(RelativeSource))]
	public class RelativeSource : MarkupExtension, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.RelativeSource" /> class.</summary>
		// Token: 0x06001C9B RID: 7323 RVA: 0x00086562 File Offset: 0x00084762
		public RelativeSource()
		{
			this._mode = RelativeSourceMode.FindAncestor;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.RelativeSource" /> class with an initial mode.</summary>
		/// <param name="mode">One of the <see cref="T:System.Windows.Data.RelativeSourceMode" /> values.</param>
		// Token: 0x06001C9C RID: 7324 RVA: 0x00086578 File Offset: 0x00084778
		public RelativeSource(RelativeSourceMode mode)
		{
			this.InitializeMode(mode);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.RelativeSource" /> class with an initial mode and additional tree-walking qualifiers for finding the desired relative source.</summary>
		/// <param name="mode">One of the <see cref="T:System.Windows.Data.RelativeSourceMode" /> values. For this signature to be relevant, this should be <see cref="F:System.Windows.Data.RelativeSourceMode.FindAncestor" />.</param>
		/// <param name="ancestorType">The <see cref="T:System.Type" /> of ancestor to look for.</param>
		/// <param name="ancestorLevel">The ordinal position of the desired ancestor among all ancestors of the given type. </param>
		// Token: 0x06001C9D RID: 7325 RVA: 0x0008658E File Offset: 0x0008478E
		public RelativeSource(RelativeSourceMode mode, Type ancestorType, int ancestorLevel)
		{
			this.InitializeMode(mode);
			this.AncestorType = ancestorType;
			this.AncestorLevel = ancestorLevel;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06001C9E RID: 7326 RVA: 0x00002137 File Offset: 0x00000337
		void ISupportInitialize.BeginInit()
		{
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06001C9F RID: 7327 RVA: 0x000865B4 File Offset: 0x000847B4
		void ISupportInitialize.EndInit()
		{
			if (this.IsUninitialized)
			{
				throw new InvalidOperationException(SR.Get("RelativeSourceNeedsMode"));
			}
			if (this._mode == RelativeSourceMode.FindAncestor && this.AncestorType == null)
			{
				throw new InvalidOperationException(SR.Get("RelativeSourceNeedsAncestorType"));
			}
		}

		/// <summary>Gets a static value that is used to return a <see cref="T:System.Windows.Data.RelativeSource" /> constructed for the <see cref="F:System.Windows.Data.RelativeSourceMode.PreviousData" /> mode.</summary>
		/// <returns>A static <see cref="T:System.Windows.Data.RelativeSource" />.</returns>
		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x00086600 File Offset: 0x00084800
		public static RelativeSource PreviousData
		{
			get
			{
				if (RelativeSource.s_previousData == null)
				{
					RelativeSource.s_previousData = new RelativeSource(RelativeSourceMode.PreviousData);
				}
				return RelativeSource.s_previousData;
			}
		}

		/// <summary>Gets a static value that is used to return a <see cref="T:System.Windows.Data.RelativeSource" /> constructed for the <see cref="F:System.Windows.Data.RelativeSourceMode.TemplatedParent" /> mode.</summary>
		/// <returns>A static <see cref="T:System.Windows.Data.RelativeSource" />.</returns>
		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x00086619 File Offset: 0x00084819
		public static RelativeSource TemplatedParent
		{
			get
			{
				if (RelativeSource.s_templatedParent == null)
				{
					RelativeSource.s_templatedParent = new RelativeSource(RelativeSourceMode.TemplatedParent);
				}
				return RelativeSource.s_templatedParent;
			}
		}

		/// <summary>Gets a static value that is used to return a <see cref="T:System.Windows.Data.RelativeSource" /> constructed for the <see cref="F:System.Windows.Data.RelativeSourceMode.Self" /> mode.</summary>
		/// <returns>A static <see cref="T:System.Windows.Data.RelativeSource" />.</returns>
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00086632 File Offset: 0x00084832
		public static RelativeSource Self
		{
			get
			{
				if (RelativeSource.s_self == null)
				{
					RelativeSource.s_self = new RelativeSource(RelativeSourceMode.Self);
				}
				return RelativeSource.s_self;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Data.RelativeSourceMode" /> value that describes the location of the binding source relative to the position of the binding target.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Data.RelativeSourceMode" /> values. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">This property is immutable after initialization. Instead of changing the <see cref="P:System.Windows.Data.RelativeSource.Mode" /> on this instance, create a new <see cref="T:System.Windows.Data.RelativeSource" /> or use a different static instance.</exception>
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x0008664B File Offset: 0x0008484B
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x00086653 File Offset: 0x00084853
		[ConstructorArgument("mode")]
		public RelativeSourceMode Mode
		{
			get
			{
				return this._mode;
			}
			set
			{
				if (this.IsUninitialized)
				{
					this.InitializeMode(value);
					return;
				}
				if (value != this._mode)
				{
					throw new InvalidOperationException(SR.Get("RelativeSourceModeIsImmutable"));
				}
			}
		}

		/// <summary>Gets or sets the type of ancestor to look for.</summary>
		/// <returns>The type of ancestor. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Data.RelativeSource" /> is not in the <see cref="F:System.Windows.Data.RelativeSourceMode.FindAncestor" /> mode.</exception>
		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x0008667E File Offset: 0x0008487E
		// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x00086686 File Offset: 0x00084886
		public Type AncestorType
		{
			get
			{
				return this._ancestorType;
			}
			set
			{
				if (this.IsUninitialized)
				{
					this.AncestorLevel = 1;
				}
				if (this._mode != RelativeSourceMode.FindAncestor)
				{
					if (value != null)
					{
						throw new InvalidOperationException(SR.Get("RelativeSourceNotInFindAncestorMode"));
					}
				}
				else
				{
					this._ancestorType = value;
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.RelativeSource.AncestorType" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CA7 RID: 7335 RVA: 0x000866C0 File Offset: 0x000848C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeAncestorType()
		{
			return this._mode == RelativeSourceMode.FindAncestor;
		}

		/// <summary>Gets or sets the level of ancestor to look for, in <see cref="F:System.Windows.Data.RelativeSourceMode.FindAncestor" /> mode. Use 1 to indicate the one nearest to the binding target element.</summary>
		/// <returns>The ancestor level. Use 1 to indicate the one nearest to the binding target element.</returns>
		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x000866CB File Offset: 0x000848CB
		// (set) Token: 0x06001CA9 RID: 7337 RVA: 0x000866D3 File Offset: 0x000848D3
		public int AncestorLevel
		{
			get
			{
				return this._ancestorLevel;
			}
			set
			{
				if (this._mode != RelativeSourceMode.FindAncestor)
				{
					if (value != 0)
					{
						throw new InvalidOperationException(SR.Get("RelativeSourceNotInFindAncestorMode"));
					}
				}
				else
				{
					if (value < 1)
					{
						throw new ArgumentOutOfRangeException(SR.Get("RelativeSourceInvalidAncestorLevel"));
					}
					this._ancestorLevel = value;
				}
			}
		}

		/// <summary>Indicates whether the <see cref="P:System.Windows.Data.RelativeSource.AncestorLevel" /> property should be persisted.</summary>
		/// <returns>
		///     <see langword="true" /> if the property value has changed from its default; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001CAA RID: 7338 RVA: 0x000866C0 File Offset: 0x000848C0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeAncestorLevel()
		{
			return this._mode == RelativeSourceMode.FindAncestor;
		}

		/// <summary>Returns an object that should be set as the value on the target object's property for this markup extension. For <see cref="T:System.Windows.Data.RelativeSource" />, this is another <see cref="T:System.Windows.Data.RelativeSource" />, using the appropriate source for the specified mode. </summary>
		/// <param name="serviceProvider">An object that can provide services for the markup extension. In this implementation, this parameter can be <see langword="null" />.</param>
		/// <returns>Another <see cref="T:System.Windows.Data.RelativeSource" />.</returns>
		// Token: 0x06001CAB RID: 7339 RVA: 0x0008670C File Offset: 0x0008490C
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this._mode == RelativeSourceMode.PreviousData)
			{
				return RelativeSource.PreviousData;
			}
			if (this._mode == RelativeSourceMode.Self)
			{
				return RelativeSource.Self;
			}
			if (this._mode == RelativeSourceMode.TemplatedParent)
			{
				return RelativeSource.TemplatedParent;
			}
			return this;
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x0008673B File Offset: 0x0008493B
		private bool IsUninitialized
		{
			get
			{
				return this._ancestorLevel == -1;
			}
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00086748 File Offset: 0x00084948
		private void InitializeMode(RelativeSourceMode mode)
		{
			if (mode == RelativeSourceMode.FindAncestor)
			{
				this._ancestorLevel = 1;
				this._mode = mode;
				return;
			}
			if (mode == RelativeSourceMode.PreviousData || mode == RelativeSourceMode.Self || mode == RelativeSourceMode.TemplatedParent)
			{
				this._ancestorLevel = 0;
				this._mode = mode;
				return;
			}
			throw new ArgumentException(SR.Get("RelativeSourceModeInvalid"), "mode");
		}

		// Token: 0x040013E4 RID: 5092
		private RelativeSourceMode _mode;

		// Token: 0x040013E5 RID: 5093
		private Type _ancestorType;

		// Token: 0x040013E6 RID: 5094
		private int _ancestorLevel = -1;

		// Token: 0x040013E7 RID: 5095
		private static RelativeSource s_previousData;

		// Token: 0x040013E8 RID: 5096
		private static RelativeSource s_templatedParent;

		// Token: 0x040013E9 RID: 5097
		private static RelativeSource s_self;
	}
}

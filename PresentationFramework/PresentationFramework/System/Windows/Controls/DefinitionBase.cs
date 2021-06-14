using System;
using System.Collections;
using System.Collections.Generic;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Defines the functionality required to support a shared-size group that is used by the <see cref="T:System.Windows.Controls.ColumnDefinitionCollection" /> and <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> classes. This is an abstract class. </summary>
	// Token: 0x020004C9 RID: 1225
	[Localizability(LocalizationCategory.Ignore)]
	public abstract class DefinitionBase : FrameworkContentElement
	{
		// Token: 0x06004A56 RID: 19030 RVA: 0x0014FBEA File Offset: 0x0014DDEA
		internal DefinitionBase(bool isColumnDefinition)
		{
			this._isColumnDefinition = isColumnDefinition;
			this._parentIndex = -1;
		}

		/// <summary>Gets or sets a value that identifies a <see cref="T:System.Windows.Controls.ColumnDefinition" /> or <see cref="T:System.Windows.Controls.RowDefinition" /> as a member of a defined group that shares sizing properties.   </summary>
		/// <returns>A <see cref="T:System.String" /> that identifies a shared-size group.</returns>
		// Token: 0x1700121C RID: 4636
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x0014FC00 File Offset: 0x0014DE00
		// (set) Token: 0x06004A58 RID: 19032 RVA: 0x0014FC12 File Offset: 0x0014DE12
		public string SharedSizeGroup
		{
			get
			{
				return (string)base.GetValue(DefinitionBase.SharedSizeGroupProperty);
			}
			set
			{
				base.SetValue(DefinitionBase.SharedSizeGroupProperty, value);
			}
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0014FC20 File Offset: 0x0014DE20
		internal void OnEnterParentTree()
		{
			if (this._sharedState == null)
			{
				string sharedSizeGroup = this.SharedSizeGroup;
				if (sharedSizeGroup != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = this.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						this._sharedState = privateSharedSizeScope.EnsureSharedState(sharedSizeGroup);
						this._sharedState.AddMember(this);
					}
				}
			}
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0014FC62 File Offset: 0x0014DE62
		internal void OnExitParentTree()
		{
			this._offset = 0.0;
			if (this._sharedState != null)
			{
				this._sharedState.RemoveMember(this);
				this._sharedState = null;
			}
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0014FC8E File Offset: 0x0014DE8E
		internal void OnBeforeLayout(Grid grid)
		{
			this._minSize = 0.0;
			this.LayoutWasUpdated = true;
			if (this._sharedState != null)
			{
				this._sharedState.EnsureDeferredValidation(grid);
			}
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0014FCBA File Offset: 0x0014DEBA
		internal void UpdateMinSize(double minSize)
		{
			this._minSize = Math.Max(this._minSize, minSize);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0014FCCE File Offset: 0x0014DECE
		internal void SetMinSize(double minSize)
		{
			this._minSize = minSize;
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0014FCD8 File Offset: 0x0014DED8
		internal static void OnUserSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.Invalidate();
					return;
				}
				Grid grid = (Grid)definitionBase.Parent;
				if (((GridLength)e.OldValue).GridUnitType != ((GridLength)e.NewValue).GridUnitType)
				{
					grid.Invalidate();
					return;
				}
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0014FD4C File Offset: 0x0014DF4C
		internal static bool IsUserSizePropertyValueValid(object value)
		{
			return ((GridLength)value).Value >= 0.0;
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x0014FD78 File Offset: 0x0014DF78
		internal static void OnUserMinSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				Grid grid = (Grid)definitionBase.Parent;
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0014FDA8 File Offset: 0x0014DFA8
		internal static bool IsUserMinSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x0014FDDC File Offset: 0x0014DFDC
		internal static void OnUserMaxSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				Grid grid = (Grid)definitionBase.Parent;
				grid.InvalidateMeasure();
			}
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x0014FE0C File Offset: 0x0014E00C
		internal static bool IsUserMaxSizePropertyValueValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x0014FE3C File Offset: 0x0014E03C
		internal static void OnIsSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				DefinitionBase.SharedSizeScope value = new DefinitionBase.SharedSizeScope();
				d.SetValue(DefinitionBase.PrivateSharedSizeScopeProperty, value);
				return;
			}
			d.ClearValue(DefinitionBase.PrivateSharedSizeScopeProperty);
		}

		// Token: 0x1700121D RID: 4637
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x0014FE75 File Offset: 0x0014E075
		internal bool IsShared
		{
			get
			{
				return this._sharedState != null;
			}
		}

		// Token: 0x1700121E RID: 4638
		// (get) Token: 0x06004A66 RID: 19046 RVA: 0x0014FE80 File Offset: 0x0014E080
		internal GridLength UserSize
		{
			get
			{
				if (this._sharedState == null)
				{
					return this.UserSizeValueCache;
				}
				return this._sharedState.UserSize;
			}
		}

		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x06004A67 RID: 19047 RVA: 0x0014FE9C File Offset: 0x0014E09C
		internal double UserMinSize
		{
			get
			{
				return this.UserMinSizeValueCache;
			}
		}

		// Token: 0x17001220 RID: 4640
		// (get) Token: 0x06004A68 RID: 19048 RVA: 0x0014FEA4 File Offset: 0x0014E0A4
		internal double UserMaxSize
		{
			get
			{
				return this.UserMaxSizeValueCache;
			}
		}

		// Token: 0x17001221 RID: 4641
		// (get) Token: 0x06004A69 RID: 19049 RVA: 0x0014FEAC File Offset: 0x0014E0AC
		// (set) Token: 0x06004A6A RID: 19050 RVA: 0x0014FEB4 File Offset: 0x0014E0B4
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x17001222 RID: 4642
		// (get) Token: 0x06004A6B RID: 19051 RVA: 0x0014FEBD File Offset: 0x0014E0BD
		// (set) Token: 0x06004A6C RID: 19052 RVA: 0x0014FEC5 File Offset: 0x0014E0C5
		internal Grid.LayoutTimeSizeType SizeType
		{
			get
			{
				return this._sizeType;
			}
			set
			{
				this._sizeType = value;
			}
		}

		// Token: 0x17001223 RID: 4643
		// (get) Token: 0x06004A6D RID: 19053 RVA: 0x0014FECE File Offset: 0x0014E0CE
		// (set) Token: 0x06004A6E RID: 19054 RVA: 0x0014FED6 File Offset: 0x0014E0D6
		internal double MeasureSize
		{
			get
			{
				return this._measureSize;
			}
			set
			{
				this._measureSize = value;
			}
		}

		// Token: 0x17001224 RID: 4644
		// (get) Token: 0x06004A6F RID: 19055 RVA: 0x0014FEE0 File Offset: 0x0014E0E0
		internal double PreferredSize
		{
			get
			{
				double num = this.MinSize;
				if (this._sizeType != Grid.LayoutTimeSizeType.Auto && num < this._measureSize)
				{
					num = this._measureSize;
				}
				return num;
			}
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06004A70 RID: 19056 RVA: 0x0014FF0E File Offset: 0x0014E10E
		// (set) Token: 0x06004A71 RID: 19057 RVA: 0x0014FF16 File Offset: 0x0014E116
		internal double SizeCache
		{
			get
			{
				return this._sizeCache;
			}
			set
			{
				this._sizeCache = value;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x0014FF20 File Offset: 0x0014E120
		internal double MinSize
		{
			get
			{
				double minSize = this._minSize;
				if (this.UseSharedMinimum && this._sharedState != null && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06004A73 RID: 19059 RVA: 0x0014FF60 File Offset: 0x0014E160
		internal double MinSizeForArrange
		{
			get
			{
				double minSize = this._minSize;
				if (this._sharedState != null && (this.UseSharedMinimum || !this.LayoutWasUpdated) && minSize < this._sharedState.MinSize)
				{
					minSize = this._sharedState.MinSize;
				}
				return minSize;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x0014FFA7 File Offset: 0x0014E1A7
		internal double RawMinSize
		{
			get
			{
				return this._minSize;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06004A75 RID: 19061 RVA: 0x0014FFAF File Offset: 0x0014E1AF
		// (set) Token: 0x06004A76 RID: 19062 RVA: 0x0014FFB7 File Offset: 0x0014E1B7
		internal double FinalOffset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06004A77 RID: 19063 RVA: 0x0014FFC0 File Offset: 0x0014E1C0
		internal GridLength UserSizeValueCache
		{
			get
			{
				return (GridLength)base.GetValue(this._isColumnDefinition ? ColumnDefinition.WidthProperty : RowDefinition.HeightProperty);
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06004A78 RID: 19064 RVA: 0x0014FFE1 File Offset: 0x0014E1E1
		internal double UserMinSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MinWidthProperty : RowDefinition.MinHeightProperty);
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06004A79 RID: 19065 RVA: 0x00150002 File Offset: 0x0014E202
		internal double UserMaxSizeValueCache
		{
			get
			{
				return (double)base.GetValue(this._isColumnDefinition ? ColumnDefinition.MaxWidthProperty : RowDefinition.MaxHeightProperty);
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06004A7A RID: 19066 RVA: 0x00150023 File Offset: 0x0014E223
		internal bool InParentLogicalTree
		{
			get
			{
				return this._parentIndex != -1;
			}
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x00150031 File Offset: 0x0014E231
		private void SetFlags(bool value, DefinitionBase.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x00150050 File Offset: 0x0014E250
		private bool CheckFlagsAnd(DefinitionBase.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x00150060 File Offset: 0x0014E260
		private static void OnSharedSizeGroupPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				string text = (string)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && text != null)
				{
					DefinitionBase.SharedSizeScope privateSharedSizeScope = definitionBase.PrivateSharedSizeScope;
					if (privateSharedSizeScope != null)
					{
						definitionBase._sharedState = privateSharedSizeScope.EnsureSharedState(text);
						definitionBase._sharedState.AddMember(definitionBase);
					}
				}
			}
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x001500D4 File Offset: 0x0014E2D4
		private static bool SharedSizeGroupPropertyValueValid(object value)
		{
			if (value == null)
			{
				return true;
			}
			string text = (string)value;
			if (text != string.Empty)
			{
				int num = -1;
				while (++num < text.Length)
				{
					bool flag = char.IsDigit(text[num]);
					if ((num == 0 && flag) || (!flag && !char.IsLetter(text[num]) && '_' != text[num]))
					{
						break;
					}
				}
				if (num == text.Length)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x00150148 File Offset: 0x0014E348
		private static void OnPrivateSharedSizeScopePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DefinitionBase definitionBase = (DefinitionBase)d;
			if (definitionBase.InParentLogicalTree)
			{
				DefinitionBase.SharedSizeScope sharedSizeScope = (DefinitionBase.SharedSizeScope)e.NewValue;
				if (definitionBase._sharedState != null)
				{
					definitionBase._sharedState.RemoveMember(definitionBase);
					definitionBase._sharedState = null;
				}
				if (definitionBase._sharedState == null && sharedSizeScope != null)
				{
					string sharedSizeGroup = definitionBase.SharedSizeGroup;
					if (sharedSizeGroup != null)
					{
						definitionBase._sharedState = sharedSizeScope.EnsureSharedState(definitionBase.SharedSizeGroup);
						definitionBase._sharedState.AddMember(definitionBase);
					}
				}
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x001501BF File Offset: 0x0014E3BF
		private DefinitionBase.SharedSizeScope PrivateSharedSizeScope
		{
			get
			{
				return (DefinitionBase.SharedSizeScope)base.GetValue(DefinitionBase.PrivateSharedSizeScopeProperty);
			}
		}

		// Token: 0x1700122F RID: 4655
		// (get) Token: 0x06004A81 RID: 19073 RVA: 0x001501D1 File Offset: 0x0014E3D1
		// (set) Token: 0x06004A82 RID: 19074 RVA: 0x001501DB File Offset: 0x0014E3DB
		private bool UseSharedMinimum
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.UseSharedMinimum);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.UseSharedMinimum);
			}
		}

		// Token: 0x17001230 RID: 4656
		// (get) Token: 0x06004A83 RID: 19075 RVA: 0x001501E6 File Offset: 0x0014E3E6
		// (set) Token: 0x06004A84 RID: 19076 RVA: 0x001501F0 File Offset: 0x0014E3F0
		private bool LayoutWasUpdated
		{
			get
			{
				return this.CheckFlagsAnd(DefinitionBase.Flags.LayoutWasUpdated);
			}
			set
			{
				this.SetFlags(value, DefinitionBase.Flags.LayoutWasUpdated);
			}
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x001501FC File Offset: 0x0014E3FC
		static DefinitionBase()
		{
			DefinitionBase.PrivateSharedSizeScopeProperty.OverrideMetadata(typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnPrivateSharedSizeScopePropertyChanged)));
		}

		// Token: 0x04002A55 RID: 10837
		private readonly bool _isColumnDefinition;

		// Token: 0x04002A56 RID: 10838
		private DefinitionBase.Flags _flags;

		// Token: 0x04002A57 RID: 10839
		private int _parentIndex;

		// Token: 0x04002A58 RID: 10840
		private Grid.LayoutTimeSizeType _sizeType;

		// Token: 0x04002A59 RID: 10841
		private double _minSize;

		// Token: 0x04002A5A RID: 10842
		private double _measureSize;

		// Token: 0x04002A5B RID: 10843
		private double _sizeCache;

		// Token: 0x04002A5C RID: 10844
		private double _offset;

		// Token: 0x04002A5D RID: 10845
		private DefinitionBase.SharedSizeState _sharedState;

		// Token: 0x04002A5E RID: 10846
		internal const bool ThisIsColumnDefinition = true;

		// Token: 0x04002A5F RID: 10847
		internal const bool ThisIsRowDefinition = false;

		// Token: 0x04002A60 RID: 10848
		internal static readonly DependencyProperty PrivateSharedSizeScopeProperty = DependencyProperty.RegisterAttached("PrivateSharedSizeScope", typeof(DefinitionBase.SharedSizeScope), typeof(DefinitionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DefinitionBase.SharedSizeGroup" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DefinitionBase.SharedSizeGroup" /> dependency property.</returns>
		// Token: 0x04002A61 RID: 10849
		public static readonly DependencyProperty SharedSizeGroupProperty = DependencyProperty.Register("SharedSizeGroup", typeof(string), typeof(DefinitionBase), new FrameworkPropertyMetadata(new PropertyChangedCallback(DefinitionBase.OnSharedSizeGroupPropertyChanged)), new ValidateValueCallback(DefinitionBase.SharedSizeGroupPropertyValueValid));

		// Token: 0x0200096E RID: 2414
		[Flags]
		private enum Flags : byte
		{
			// Token: 0x0400443D RID: 17469
			UseSharedMinimum = 32,
			// Token: 0x0400443E RID: 17470
			LayoutWasUpdated = 64
		}

		// Token: 0x0200096F RID: 2415
		private class SharedSizeScope
		{
			// Token: 0x0600876F RID: 34671 RVA: 0x0024FC24 File Offset: 0x0024DE24
			internal DefinitionBase.SharedSizeState EnsureSharedState(string sharedSizeGroup)
			{
				DefinitionBase.SharedSizeState sharedSizeState = this._registry[sharedSizeGroup] as DefinitionBase.SharedSizeState;
				if (sharedSizeState == null)
				{
					sharedSizeState = new DefinitionBase.SharedSizeState(this, sharedSizeGroup);
					this._registry[sharedSizeGroup] = sharedSizeState;
				}
				return sharedSizeState;
			}

			// Token: 0x06008770 RID: 34672 RVA: 0x0024FC5C File Offset: 0x0024DE5C
			internal void Remove(object key)
			{
				this._registry.Remove(key);
			}

			// Token: 0x0400443F RID: 17471
			private Hashtable _registry = new Hashtable();
		}

		// Token: 0x02000970 RID: 2416
		private class SharedSizeState
		{
			// Token: 0x06008772 RID: 34674 RVA: 0x0024FC7D File Offset: 0x0024DE7D
			internal SharedSizeState(DefinitionBase.SharedSizeScope sharedSizeScope, string sharedSizeGroupId)
			{
				this._sharedSizeScope = sharedSizeScope;
				this._sharedSizeGroupId = sharedSizeGroupId;
				this._registry = new List<DefinitionBase>();
				this._layoutUpdated = new EventHandler(this.OnLayoutUpdated);
				this._broadcastInvalidation = true;
			}

			// Token: 0x06008773 RID: 34675 RVA: 0x0024FCB7 File Offset: 0x0024DEB7
			internal void AddMember(DefinitionBase member)
			{
				this._registry.Add(member);
				this.Invalidate();
			}

			// Token: 0x06008774 RID: 34676 RVA: 0x0024FCCB File Offset: 0x0024DECB
			internal void RemoveMember(DefinitionBase member)
			{
				this.Invalidate();
				this._registry.Remove(member);
				if (this._registry.Count == 0)
				{
					this._sharedSizeScope.Remove(this._sharedSizeGroupId);
				}
			}

			// Token: 0x06008775 RID: 34677 RVA: 0x0024FD00 File Offset: 0x0024DF00
			internal void Invalidate()
			{
				this._userSizeValid = false;
				if (this._broadcastInvalidation)
				{
					int i = 0;
					int count = this._registry.Count;
					while (i < count)
					{
						Grid grid = (Grid)this._registry[i].Parent;
						grid.Invalidate();
						i++;
					}
					this._broadcastInvalidation = false;
				}
			}

			// Token: 0x06008776 RID: 34678 RVA: 0x0024FD58 File Offset: 0x0024DF58
			internal void EnsureDeferredValidation(UIElement layoutUpdatedHost)
			{
				if (this._layoutUpdatedHost == null)
				{
					this._layoutUpdatedHost = layoutUpdatedHost;
					this._layoutUpdatedHost.LayoutUpdated += this._layoutUpdated;
				}
			}

			// Token: 0x17001E9A RID: 7834
			// (get) Token: 0x06008777 RID: 34679 RVA: 0x0024FD7A File Offset: 0x0024DF7A
			internal double MinSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._minSize;
				}
			}

			// Token: 0x17001E9B RID: 7835
			// (get) Token: 0x06008778 RID: 34680 RVA: 0x0024FD90 File Offset: 0x0024DF90
			internal GridLength UserSize
			{
				get
				{
					if (!this._userSizeValid)
					{
						this.EnsureUserSizeValid();
					}
					return this._userSize;
				}
			}

			// Token: 0x06008779 RID: 34681 RVA: 0x0024FDA8 File Offset: 0x0024DFA8
			private void EnsureUserSizeValid()
			{
				this._userSize = new GridLength(1.0, GridUnitType.Auto);
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					GridLength userSizeValueCache = this._registry[i].UserSizeValueCache;
					if (userSizeValueCache.GridUnitType == GridUnitType.Pixel)
					{
						if (this._userSize.GridUnitType == GridUnitType.Auto)
						{
							this._userSize = userSizeValueCache;
						}
						else if (this._userSize.Value < userSizeValueCache.Value)
						{
							this._userSize = userSizeValueCache;
						}
					}
					i++;
				}
				this._minSize = (this._userSize.IsAbsolute ? this._userSize.Value : 0.0);
				this._userSizeValid = true;
			}

			// Token: 0x0600877A RID: 34682 RVA: 0x0024FE5F File Offset: 0x0024E05F
			private void OnLayoutUpdated(object sender, EventArgs e)
			{
				if (!FrameworkAppContextSwitches.SharedSizeGroupDoesRedundantLayout)
				{
					this.ValidateSharedSizeGroup();
					return;
				}
				this.ValidateSharedSizeGroupLegacy();
			}

			// Token: 0x0600877B RID: 34683 RVA: 0x0024FE78 File Offset: 0x0024E078
			private void ValidateSharedSizeGroup()
			{
				double num = 0.0;
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					num = Math.Max(num, this._registry[i]._minSize);
					i++;
				}
				bool flag = !DoubleUtil.AreClose(this._minSize, num);
				int j = 0;
				int count2 = this._registry.Count;
				while (j < count2)
				{
					DefinitionBase definitionBase = this._registry[j];
					bool flag2 = !DoubleUtil.AreClose(definitionBase._minSize, num);
					bool flag3;
					if (!definitionBase.UseSharedMinimum)
					{
						flag3 = !flag2;
					}
					else if (flag2)
					{
						flag3 = !flag;
					}
					else
					{
						flag3 = (definitionBase.LayoutWasUpdated && DoubleUtil.GreaterThanOrClose(definitionBase._minSize, this.MinSize));
					}
					if (!flag3)
					{
						Grid grid = (Grid)definitionBase.Parent;
						grid.InvalidateMeasure();
					}
					else if (!DoubleUtil.AreClose(num, definitionBase.SizeCache))
					{
						Grid grid2 = (Grid)definitionBase.Parent;
						grid2.InvalidateArrange();
					}
					definitionBase.UseSharedMinimum = flag2;
					definitionBase.LayoutWasUpdated = false;
					j++;
				}
				this._minSize = num;
				this._layoutUpdatedHost.LayoutUpdated -= this._layoutUpdated;
				this._layoutUpdatedHost = null;
				this._broadcastInvalidation = true;
			}

			// Token: 0x0600877C RID: 34684 RVA: 0x0024FFC8 File Offset: 0x0024E1C8
			private void ValidateSharedSizeGroupLegacy()
			{
				double num = 0.0;
				int i = 0;
				int count = this._registry.Count;
				while (i < count)
				{
					num = Math.Max(num, this._registry[i].MinSize);
					i++;
				}
				bool flag = !DoubleUtil.AreClose(this._minSize, num);
				int j = 0;
				int count2 = this._registry.Count;
				while (j < count2)
				{
					DefinitionBase definitionBase = this._registry[j];
					if (flag || definitionBase.LayoutWasUpdated)
					{
						if (!DoubleUtil.AreClose(num, definitionBase.MinSize))
						{
							Grid grid = (Grid)definitionBase.Parent;
							grid.InvalidateMeasure();
							definitionBase.UseSharedMinimum = true;
						}
						else
						{
							definitionBase.UseSharedMinimum = false;
							if (!DoubleUtil.AreClose(num, definitionBase.SizeCache))
							{
								Grid grid2 = (Grid)definitionBase.Parent;
								grid2.InvalidateArrange();
							}
						}
						definitionBase.LayoutWasUpdated = false;
					}
					j++;
				}
				this._minSize = num;
				this._layoutUpdatedHost.LayoutUpdated -= this._layoutUpdated;
				this._layoutUpdatedHost = null;
				this._broadcastInvalidation = true;
			}

			// Token: 0x04004440 RID: 17472
			private readonly DefinitionBase.SharedSizeScope _sharedSizeScope;

			// Token: 0x04004441 RID: 17473
			private readonly string _sharedSizeGroupId;

			// Token: 0x04004442 RID: 17474
			private readonly List<DefinitionBase> _registry;

			// Token: 0x04004443 RID: 17475
			private readonly EventHandler _layoutUpdated;

			// Token: 0x04004444 RID: 17476
			private UIElement _layoutUpdatedHost;

			// Token: 0x04004445 RID: 17477
			private bool _broadcastInvalidation;

			// Token: 0x04004446 RID: 17478
			private bool _userSizeValid;

			// Token: 0x04004447 RID: 17479
			private GridLength _userSize;

			// Token: 0x04004448 RID: 17480
			private double _minSize;
		}
	}
}

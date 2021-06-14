using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Arranges <see cref="T:System.Windows.Controls.ToolBar" /> items inside a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
	// Token: 0x020005B3 RID: 1459
	public class ToolBarPanel : StackPanel
	{
		// Token: 0x06006108 RID: 24840 RVA: 0x001B3974 File Offset: 0x001B1B74
		static ToolBarPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBarPanel);
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x0600610A RID: 24842 RVA: 0x001B398C File Offset: 0x001B1B8C
		// (set) Token: 0x0600610B RID: 24843 RVA: 0x001B3994 File Offset: 0x001B1B94
		internal double MinLength { get; private set; }

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x0600610C RID: 24844 RVA: 0x001B399D File Offset: 0x001B1B9D
		// (set) Token: 0x0600610D RID: 24845 RVA: 0x001B39A5 File Offset: 0x001B1BA5
		internal double MaxLength { get; private set; }

		// Token: 0x0600610E RID: 24846 RVA: 0x001B39B0 File Offset: 0x001B1BB0
		private bool MeasureGeneratedItems(bool asNeededPass, Size constraint, bool horizontal, double maxExtent, ref Size panelDesiredSize, out double overflowExtent)
		{
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			bool flag = false;
			bool result = false;
			bool flag2 = false;
			overflowExtent = 0.0;
			UIElementCollection internalChildren = base.InternalChildren;
			int num = internalChildren.Count;
			int num2 = 0;
			for (int i = 0; i < this._generatedItemsCollection.Count; i++)
			{
				UIElement uielement = this._generatedItemsCollection[i];
				OverflowMode overflowMode = ToolBar.GetOverflowMode(uielement);
				bool flag3 = overflowMode == OverflowMode.AsNeeded;
				if (flag3 == asNeededPass)
				{
					DependencyObject parent = VisualTreeHelper.GetParent(uielement);
					if (overflowMode != OverflowMode.Always && !flag)
					{
						ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
						uielement.Measure(constraint);
						Size desiredSize = uielement.DesiredSize;
						if (flag3)
						{
							double value;
							if (horizontal)
							{
								value = desiredSize.Width + panelDesiredSize.Width;
							}
							else
							{
								value = desiredSize.Height + panelDesiredSize.Height;
							}
							if (DoubleUtil.GreaterThan(value, maxExtent))
							{
								flag = true;
							}
						}
						if (!flag)
						{
							if (horizontal)
							{
								panelDesiredSize.Width += desiredSize.Width;
								panelDesiredSize.Height = Math.Max(panelDesiredSize.Height, desiredSize.Height);
							}
							else
							{
								panelDesiredSize.Width = Math.Max(panelDesiredSize.Width, desiredSize.Width);
								panelDesiredSize.Height += desiredSize.Height;
							}
							if (parent != this)
							{
								if (parent == toolBarOverflowPanel && toolBarOverflowPanel != null)
								{
									toolBarOverflowPanel.Children.Remove(uielement);
								}
								if (num2 < num)
								{
									internalChildren.InsertInternal(num2, uielement);
								}
								else
								{
									internalChildren.AddInternal(uielement);
								}
								num++;
							}
							num2++;
						}
					}
					if (overflowMode == OverflowMode.Always || flag)
					{
						result = true;
						if (uielement.MeasureDirty)
						{
							ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
							uielement.Measure(constraint);
						}
						Size desiredSize2 = uielement.DesiredSize;
						if (horizontal)
						{
							overflowExtent += desiredSize2.Width;
							panelDesiredSize.Height = Math.Max(panelDesiredSize.Height, desiredSize2.Height);
						}
						else
						{
							overflowExtent += desiredSize2.Height;
							panelDesiredSize.Width = Math.Max(panelDesiredSize.Width, desiredSize2.Width);
						}
						ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.TrueBox);
						if (parent == this)
						{
							internalChildren.RemoveNoVerify(uielement);
							num--;
							flag2 = true;
						}
						else if (parent == null)
						{
							flag2 = true;
						}
					}
				}
				else if (num2 < num && internalChildren[num2] == uielement)
				{
					num2++;
				}
			}
			if (flag2 && toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.InvalidateMeasure();
			}
			return result;
		}

		/// <summary>Remeasures a toolbar panel.</summary>
		/// <param name="constraint">The measurement constraints; a <see cref="T:System.Windows.Controls.Primitives.ToolBarPanel" /> cannot return a size larger than the constraint.</param>
		/// <returns>The desired size of the panel.</returns>
		// Token: 0x0600610F RID: 24847 RVA: 0x001B3C24 File Offset: 0x001B1E24
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			if (base.IsItemsHost)
			{
				Size constraint2 = constraint;
				bool flag = base.Orientation == Orientation.Horizontal;
				double maxExtent;
				if (flag)
				{
					constraint2.Width = double.PositiveInfinity;
					maxExtent = constraint.Width;
				}
				else
				{
					constraint2.Height = double.PositiveInfinity;
					maxExtent = constraint.Height;
				}
				double num;
				bool flag2 = this.MeasureGeneratedItems(false, constraint2, flag, maxExtent, ref result, out num);
				this.MinLength = (flag ? result.Width : result.Height);
				bool flag3 = this.MeasureGeneratedItems(true, constraint2, flag, maxExtent, ref result, out num);
				this.MaxLength = (flag ? result.Width : result.Height) + num;
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					toolBar.SetValue(ToolBar.HasOverflowItemsPropertyKey, flag2 || flag3);
				}
			}
			else
			{
				result = base.MeasureOverride(constraint);
			}
			return result;
		}

		/// <summary>Arranges and sizes <see cref="T:System.Windows.Controls.ToolBar" /> items inside a <see cref="T:System.Windows.Controls.Primitives.ToolBarPanel" />.</summary>
		/// <param name="arrangeSize">The size that the <see cref="T:System.Windows.Controls.Primitives.ToolBarPanel" /> assumes to position its children.</param>
		/// <returns>The size of the panel.</returns>
		// Token: 0x06006110 RID: 24848 RVA: 0x001B3D08 File Offset: 0x001B1F08
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			UIElementCollection internalChildren = base.InternalChildren;
			bool flag = base.Orientation == Orientation.Horizontal;
			Rect finalRect = new Rect(arrangeSize);
			double num = 0.0;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (flag)
				{
					finalRect.X += num;
					num = uielement.DesiredSize.Width;
					finalRect.Width = num;
					finalRect.Height = Math.Max(arrangeSize.Height, uielement.DesiredSize.Height);
				}
				else
				{
					finalRect.Y += num;
					num = uielement.DesiredSize.Height;
					finalRect.Height = num;
					finalRect.Width = Math.Max(arrangeSize.Width, uielement.DesiredSize.Width);
				}
				uielement.Arrange(finalRect);
				i++;
			}
			return arrangeSize;
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x001B3E08 File Offset: 0x001B2008
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (base.TemplatedParent is ToolBar && !base.HasNonDefaultValue(StackPanel.OrientationProperty))
			{
				Binding binding = new Binding();
				binding.RelativeSource = RelativeSource.TemplatedParent;
				binding.Path = new PropertyPath(ToolBar.OrientationProperty);
				base.SetBinding(StackPanel.OrientationProperty, binding);
			}
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x001B3E64 File Offset: 0x001B2064
		internal override void GenerateChildren()
		{
			base.GenerateChildren();
			UIElementCollection internalChildren = base.InternalChildren;
			if (this._generatedItemsCollection == null)
			{
				this._generatedItemsCollection = new List<UIElement>(internalChildren.Count);
			}
			else
			{
				this._generatedItemsCollection.Clear();
			}
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			if (toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.Children.Clear();
				toolBarOverflowPanel.InvalidateMeasure();
			}
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = internalChildren[i];
				ToolBar.SetIsOverflowItem(uielement, BooleanBoxes.FalseBox);
				this._generatedItemsCollection.Add(uielement);
			}
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x001B3EF4 File Offset: 0x001B20F4
		internal override bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
		{
			switch (args.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.AddChildren(args.Position, args.ItemCount);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveChildren(args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.ReplaceChildren(args.Position, args.ItemCount, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Move:
				this.MoveChildren(args.OldPosition, args.Position, args.ItemUICount);
				break;
			case NotifyCollectionChangedAction.Reset:
				base.OnItemsChangedInternal(sender, args);
				break;
			}
			return true;
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x001B3F8C File Offset: 0x001B218C
		private void AddChildren(GeneratorPosition pos, int itemCount)
		{
			IItemContainerGenerator generator = base.Generator;
			using (generator.StartAt(pos, GeneratorDirection.Forward))
			{
				for (int i = 0; i < itemCount; i++)
				{
					UIElement uielement = generator.GenerateNext() as UIElement;
					if (uielement != null)
					{
						this._generatedItemsCollection.Insert(pos.Index + 1 + i, uielement);
						generator.PrepareItemContainer(uielement);
					}
					else
					{
						ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
						if (itemContainerGenerator != null)
						{
							itemContainerGenerator.Verify();
						}
					}
				}
			}
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x001B4018 File Offset: 0x001B2218
		private void RemoveChild(UIElement child)
		{
			DependencyObject parent = VisualTreeHelper.GetParent(child);
			if (parent == this)
			{
				base.InternalChildren.RemoveInternal(child);
				return;
			}
			ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
			if (parent == toolBarOverflowPanel && toolBarOverflowPanel != null)
			{
				toolBarOverflowPanel.Children.Remove(child);
			}
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x001B4058 File Offset: 0x001B2258
		private void RemoveChildren(GeneratorPosition pos, int containerCount)
		{
			for (int i = 0; i < containerCount; i++)
			{
				this.RemoveChild(this._generatedItemsCollection[pos.Index + i]);
			}
			this._generatedItemsCollection.RemoveRange(pos.Index, containerCount);
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x001B40A0 File Offset: 0x001B22A0
		private void ReplaceChildren(GeneratorPosition pos, int itemCount, int containerCount)
		{
			IItemContainerGenerator generator = base.Generator;
			using (generator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int i = 0; i < itemCount; i++)
				{
					bool flag;
					UIElement uielement = generator.GenerateNext(out flag) as UIElement;
					if (uielement != null && !flag)
					{
						this.RemoveChild(this._generatedItemsCollection[pos.Index + i]);
						this._generatedItemsCollection[pos.Index + i] = uielement;
						generator.PrepareItemContainer(uielement);
					}
					else
					{
						ItemContainerGenerator itemContainerGenerator = base.Generator as ItemContainerGenerator;
						if (itemContainerGenerator != null)
						{
							itemContainerGenerator.Verify();
						}
					}
				}
			}
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x001B4150 File Offset: 0x001B2350
		private void MoveChildren(GeneratorPosition fromPos, GeneratorPosition toPos, int containerCount)
		{
			if (fromPos == toPos)
			{
				return;
			}
			IItemContainerGenerator generator = base.Generator;
			int num = generator.IndexFromGeneratorPosition(toPos);
			UIElement[] array = new UIElement[containerCount];
			for (int i = 0; i < containerCount; i++)
			{
				UIElement uielement = this._generatedItemsCollection[fromPos.Index + i];
				this.RemoveChild(uielement);
				array[i] = uielement;
			}
			this._generatedItemsCollection.RemoveRange(fromPos.Index, containerCount);
			for (int j = 0; j < containerCount; j++)
			{
				this._generatedItemsCollection.Insert(num + j, array[j]);
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06006119 RID: 24857 RVA: 0x001B38F6 File Offset: 0x001B1AF6
		private ToolBar ToolBar
		{
			get
			{
				return base.TemplatedParent as ToolBar;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x0600611A RID: 24858 RVA: 0x001B41E4 File Offset: 0x001B23E4
		private ToolBarOverflowPanel ToolBarOverflowPanel
		{
			get
			{
				ToolBar toolBar = this.ToolBar;
				if (toolBar != null)
				{
					return toolBar.ToolBarOverflowPanel;
				}
				return null;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600611B RID: 24859 RVA: 0x001B4203 File Offset: 0x001B2403
		internal List<UIElement> GeneratedItemsCollection
		{
			get
			{
				return this._generatedItemsCollection;
			}
		}

		// Token: 0x0400313A RID: 12602
		private List<UIElement> _generatedItemsCollection;
	}
}

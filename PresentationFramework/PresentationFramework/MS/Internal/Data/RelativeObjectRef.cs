using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace MS.Internal.Data
{
	// Token: 0x02000735 RID: 1845
	internal sealed class RelativeObjectRef : ObjectRef
	{
		// Token: 0x06007603 RID: 30211 RVA: 0x0021A89B File Offset: 0x00218A9B
		internal RelativeObjectRef(RelativeSource relativeSource)
		{
			if (relativeSource == null)
			{
				throw new ArgumentNullException("relativeSource");
			}
			this._relativeSource = relativeSource;
		}

		// Token: 0x06007604 RID: 30212 RVA: 0x0021A8B8 File Offset: 0x00218AB8
		public override string ToString()
		{
			RelativeSourceMode mode = this._relativeSource.Mode;
			string result;
			if (mode == RelativeSourceMode.FindAncestor)
			{
				result = string.Format(CultureInfo.InvariantCulture, "RelativeSource {0}, AncestorType='{1}', AncestorLevel='{2}'", new object[]
				{
					this._relativeSource.Mode,
					this._relativeSource.AncestorType,
					this._relativeSource.AncestorLevel
				});
			}
			else
			{
				result = string.Format(CultureInfo.InvariantCulture, "RelativeSource {0}", new object[]
				{
					this._relativeSource.Mode
				});
			}
			return result;
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x0021A94B File Offset: 0x00218B4B
		internal override object GetObject(DependencyObject d, ObjectRefArgs args)
		{
			return this.GetDataObjectImpl(d, args);
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x0021A958 File Offset: 0x00218B58
		internal override object GetDataObject(DependencyObject d, ObjectRefArgs args)
		{
			object obj = this.GetDataObjectImpl(d, args);
			DependencyObject dependencyObject = obj as DependencyObject;
			if (dependencyObject != null && this.ReturnsDataContext)
			{
				obj = dependencyObject.GetValue(ItemContainerGenerator.ItemForItemContainerProperty);
				if (obj == null)
				{
					obj = dependencyObject.GetValue(FrameworkElement.DataContextProperty);
				}
			}
			return obj;
		}

		// Token: 0x06007607 RID: 30215 RVA: 0x0021A99C File Offset: 0x00218B9C
		private object GetDataObjectImpl(DependencyObject d, ObjectRefArgs args)
		{
			if (d == null)
			{
				return null;
			}
			switch (this._relativeSource.Mode)
			{
			case RelativeSourceMode.PreviousData:
				return this.GetPreviousData(d);
			case RelativeSourceMode.TemplatedParent:
				d = Helper.GetTemplatedParent(d);
				break;
			case RelativeSourceMode.Self:
				break;
			case RelativeSourceMode.FindAncestor:
				d = this.FindAncestorOfType(this._relativeSource.AncestorType, this._relativeSource.AncestorLevel, d, args.IsTracing);
				if (d == null)
				{
					return DependencyProperty.UnsetValue;
				}
				break;
			default:
				return null;
			}
			if (args.IsTracing)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.RelativeSource(new object[]
				{
					this._relativeSource.Mode,
					TraceData.Identify(d)
				}));
			}
			return d;
		}

		// Token: 0x17001C1B RID: 7195
		// (get) Token: 0x06007608 RID: 30216 RVA: 0x0021AA4A File Offset: 0x00218C4A
		internal bool ReturnsDataContext
		{
			get
			{
				return this._relativeSource.Mode == RelativeSourceMode.PreviousData;
			}
		}

		// Token: 0x06007609 RID: 30217 RVA: 0x0021AA5A File Offset: 0x00218C5A
		protected override bool ProtectedTreeContextIsRequired(DependencyObject target)
		{
			return this._relativeSource.Mode == RelativeSourceMode.FindAncestor || this._relativeSource.Mode == RelativeSourceMode.PreviousData;
		}

		// Token: 0x17001C1C RID: 7196
		// (get) Token: 0x0600760A RID: 30218 RVA: 0x0021AA7C File Offset: 0x00218C7C
		protected override bool ProtectedUsesMentor
		{
			get
			{
				RelativeSourceMode mode = this._relativeSource.Mode;
				return mode <= RelativeSourceMode.TemplatedParent;
			}
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x0021AA9C File Offset: 0x00218C9C
		internal override string Identify()
		{
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "RelativeSource ({0})", new object[]
			{
				this._relativeSource.Mode
			});
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x0021AAC8 File Offset: 0x00218CC8
		private object GetPreviousData(DependencyObject d)
		{
			while (d != null)
			{
				if (BindingExpression.HasLocalDataContext(d))
				{
					ContentPresenter contentPresenter;
					FrameworkElement frameworkElement;
					FrameworkElement frameworkElement2;
					if ((contentPresenter = (d as ContentPresenter)) != null)
					{
						frameworkElement = contentPresenter;
						frameworkElement2 = (contentPresenter.TemplatedParent as FrameworkElement);
						if (!(frameworkElement2 is ContentControl) && !(frameworkElement2 is HeaderedItemsControl))
						{
							frameworkElement2 = (contentPresenter.Parent as GridViewRowPresenterBase);
						}
					}
					else
					{
						frameworkElement = (d as FrameworkElement);
						frameworkElement2 = (((frameworkElement != null) ? frameworkElement.Parent : null) as GridViewRowPresenterBase);
					}
					if (frameworkElement == null || frameworkElement2 == null || !ItemsControl.EqualsEx(frameworkElement.DataContext, frameworkElement2.DataContext))
					{
						break;
					}
					d = frameworkElement2;
					if (BindingExpression.HasLocalDataContext(frameworkElement2))
					{
						break;
					}
				}
				d = FrameworkElement.GetFrameworkParent(d);
			}
			if (d == null)
			{
				return DependencyProperty.UnsetValue;
			}
			Visual visual = d as Visual;
			DependencyObject dependencyObject = (visual != null) ? VisualTreeHelper.GetParent(visual) : null;
			if (ItemsControl.GetItemsOwner(dependencyObject) == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.RefPreviousNotInContext);
				}
				return null;
			}
			Visual visual2 = dependencyObject as Visual;
			int num = (visual2 != null) ? visual2.InternalVisualChildrenCount : 0;
			int num2 = -1;
			Visual visual3 = null;
			if (num != 0)
			{
				num2 = this.IndexOf(visual2, visual, out visual3);
			}
			if (num2 > 0)
			{
				d = visual3;
			}
			else
			{
				d = null;
				if (num2 < 0 && TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.RefNoWrapperInChildren);
				}
			}
			return d;
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x0021AC08 File Offset: 0x00218E08
		private DependencyObject FindAncestorOfType(Type type, int level, DependencyObject d, bool isTracing)
		{
			if (type == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.RefAncestorTypeNotSpecified);
				}
				return null;
			}
			if (level < 1)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.RefAncestorLevelInvalid);
				}
				return null;
			}
			FrameworkObject frameworkObject = new FrameworkObject(d);
			frameworkObject.Reset(frameworkObject.GetPreferVisualParent(true).DO);
			while (frameworkObject.DO != null)
			{
				if (isTracing)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.AncestorLookup(new object[]
					{
						type.Name,
						TraceData.Identify(frameworkObject.DO)
					}));
				}
				if (type.IsInstanceOfType(frameworkObject.DO) && --level <= 0)
				{
					break;
				}
				frameworkObject.Reset(frameworkObject.PreferVisualParent.DO);
			}
			return frameworkObject.DO;
		}

		// Token: 0x0600760E RID: 30222 RVA: 0x0021ACD8 File Offset: 0x00218ED8
		private int IndexOf(Visual parent, Visual child, out Visual prevChild)
		{
			bool flag = false;
			prevChild = null;
			int internalVisualChildrenCount = parent.InternalVisualChildrenCount;
			int i;
			for (i = 0; i < internalVisualChildrenCount; i++)
			{
				Visual visual = parent.InternalGetVisualChild(i);
				if (child == visual)
				{
					flag = true;
					break;
				}
				prevChild = visual;
			}
			if (flag)
			{
				return i;
			}
			return -1;
		}

		// Token: 0x04003854 RID: 14420
		private RelativeSource _relativeSource;
	}
}

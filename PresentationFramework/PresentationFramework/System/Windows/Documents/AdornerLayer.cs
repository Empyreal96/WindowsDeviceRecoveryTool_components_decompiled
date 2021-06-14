using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Media;

namespace System.Windows.Documents
{
	/// <summary>Represents a surface for rendering adorners.</summary>
	// Token: 0x0200032B RID: 811
	public class AdornerLayer : FrameworkElement
	{
		// Token: 0x06002AAA RID: 10922 RVA: 0x000C2E51 File Offset: 0x000C1051
		internal AdornerLayer() : this(Dispatcher.CurrentDispatcher)
		{
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000C2E60 File Offset: 0x000C1060
		internal AdornerLayer(Dispatcher context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			base.LayoutUpdated += this.OnLayoutUpdated;
			this._children = new VisualCollection(this);
		}

		/// <summary>Adds an adorner to the adorner layer.</summary>
		/// <param name="adorner">The adorner to add.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when adorner is null.</exception>
		// Token: 0x06002AAC RID: 10924 RVA: 0x000C2EB9 File Offset: 0x000C10B9
		public void Add(Adorner adorner)
		{
			this.Add(adorner, int.MaxValue);
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Documents.Adorner" /> from the adorner layer.</summary>
		/// <param name="adorner">The <see cref="T:System.Windows.Documents.Adorner" /> to remove.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when adorner is null.</exception>
		// Token: 0x06002AAD RID: 10925 RVA: 0x000C2EC8 File Offset: 0x000C10C8
		public void Remove(Adorner adorner)
		{
			if (adorner == null)
			{
				throw new ArgumentNullException("adorner");
			}
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				return;
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				return;
			}
			this.RemoveAdornerInfo(this.ElementMap, adorner, adorner.AdornedElement);
			this.RemoveAdornerInfo(this._zOrderMap, adorner, adornerInfo.ZOrder);
			this._children.Remove(adorner);
			base.RemoveLogicalChild(adorner);
		}

		/// <summary>Updates the layout and redraws all of the adorners in the adorner layer.</summary>
		// Token: 0x06002AAE RID: 10926 RVA: 0x000C2F4C File Offset: 0x000C114C
		public void Update()
		{
			foreach (object obj in this.ElementMap.Keys)
			{
				UIElement key = (UIElement)obj;
				ArrayList arrayList = (ArrayList)this.ElementMap[key];
				int i = 0;
				if (arrayList != null)
				{
					while (i < arrayList.Count)
					{
						this.InvalidateAdorner((AdornerLayer.AdornerInfo)arrayList[i++]);
					}
				}
			}
			this.UpdateAdorner(null);
		}

		/// <summary>Updates the layout and redraws all of the adorners in the adorner layer that are bound to the specified <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> associated with the adorners to update.</param>
		/// <exception cref="T:System.ArgumentNullException">Raised when element is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">Raised when the specified element cannot be found.</exception>
		// Token: 0x06002AAF RID: 10927 RVA: 0x000C2FE8 File Offset: 0x000C11E8
		public void Update(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			int i = 0;
			while (i < arrayList.Count)
			{
				this.InvalidateAdorner((AdornerLayer.AdornerInfo)arrayList[i++]);
			}
			this.UpdateAdorner(element);
		}

		/// <summary>Returns an array of adorners that are bound to the specified <see cref="T:System.Windows.UIElement" />.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> to retrieve an array of adorners for.</param>
		/// <returns>An array of adorners that decorate the specified <see cref="T:System.Windows.UIElement" />, or null if there are no adorners bound to the specified element.</returns>
		// Token: 0x06002AB0 RID: 10928 RVA: 0x000C3054 File Offset: 0x000C1254
		public Adorner[] GetAdorners(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null || arrayList.Count == 0)
			{
				return null;
			}
			Adorner[] array = new Adorner[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = ((AdornerLayer.AdornerInfo)arrayList[i]).Adorner;
			}
			return array;
		}

		/// <summary>Gets an <see cref="T:System.Windows.Media.AdornerHitTestResult" /> for a specified point.</summary>
		/// <param name="point">The point to hit test.</param>
		/// <returns>An <see cref="T:System.Windows.Media.AdornerHitTestResult" /> for the specified point.</returns>
		// Token: 0x06002AB1 RID: 10929 RVA: 0x000C30C0 File Offset: 0x000C12C0
		public AdornerHitTestResult AdornerHitTest(Point point)
		{
			PointHitTestResult pointHitTestResult = VisualTreeUtils.AsNearestPointHitTestResult(VisualTreeHelper.HitTest(this, point, false));
			if (pointHitTestResult != null && pointHitTestResult.VisualHit != null)
			{
				for (Visual visual = pointHitTestResult.VisualHit; visual != this; visual = (Visual)VisualTreeHelper.GetParent(visual))
				{
					if (visual is Adorner)
					{
						return new AdornerHitTestResult(pointHitTestResult.VisualHit, pointHitTestResult.PointHit, visual as Adorner);
					}
				}
				return null;
			}
			return null;
		}

		/// <summary>Returns the first adorner layer in the visual tree above a specified <see cref="T:System.Windows.Media.Visual" />.</summary>
		/// <param name="visual">The visual element for which to find an adorner layer.</param>
		/// <returns>An adorner layer for the specified visual, or null if no adorner layer can be found.</returns>
		/// <exception cref="T:System.ArgumentNullException">Raised when visual is null.</exception>
		// Token: 0x06002AB2 RID: 10930 RVA: 0x000C3124 File Offset: 0x000C1324
		public static AdornerLayer GetAdornerLayer(Visual visual)
		{
			if (visual == null)
			{
				throw new ArgumentNullException("visual");
			}
			for (Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual; visual2 != null; visual2 = (VisualTreeHelper.GetParent(visual2) as Visual))
			{
				if (visual2 is AdornerDecorator)
				{
					return ((AdornerDecorator)visual2).AdornerLayer;
				}
				if (visual2 is ScrollContentPresenter)
				{
					return ((ScrollContentPresenter)visual2).AdornerLayer;
				}
			}
			return null;
		}

		/// <summary>Gets the number of child <see cref="T:System.Windows.Media.Visual" /> objects in this instance of <see cref="T:System.Windows.Documents.AdornerLayer" />.</summary>
		/// <returns>The number of child <see cref="T:System.Windows.Media.Visual" /> objects in this instance of <see cref="T:System.Windows.Documents.AdornerLayer" />.</returns>
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x000C3185 File Offset: 0x000C1385
		protected override int VisualChildrenCount
		{
			get
			{
				return this._children.Count;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Visual" /> child at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">The index position of the wanted <see cref="T:System.Windows.Media.Visual" /> child.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Visual" /> child of the parent <see cref="T:System.Windows.Documents.AdornerLayer" /> element.</returns>
		// Token: 0x06002AB4 RID: 10932 RVA: 0x000C3192 File Offset: 0x000C1392
		protected override Visual GetVisualChild(int index)
		{
			return this._children[index];
		}

		/// <summary>Gets an enumerator that can iterate the logical child elements of this <see cref="T:System.Windows.Documents.AdornerLayer" /> element. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" />. This property has no default value.</returns>
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000C31A0 File Offset: 0x000C13A0
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.VisualChildrenCount == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this._children.GetEnumerator();
			}
		}

		/// <summary>Measures the size required for child elements and determines a size for the <see cref="T:System.Windows.Documents.AdornerLayer" />.</summary>
		/// <param name="constraint">Unused.</param>
		/// <returns>This method always returns a <see cref="T:System.Windows.Size" /> of (0,0).</returns>
		// Token: 0x06002AB6 RID: 10934 RVA: 0x000C31C0 File Offset: 0x000C13C0
		protected override Size MeasureOverride(Size constraint)
		{
			DictionaryEntry[] array = new DictionaryEntry[this._zOrderMap.Count];
			this._zOrderMap.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList = (ArrayList)array[i].Value;
				int j = 0;
				while (j < arrayList.Count)
				{
					AdornerLayer.AdornerInfo adornerInfo = (AdornerLayer.AdornerInfo)arrayList[j++];
					adornerInfo.Adorner.Measure(constraint);
				}
			}
			return default(Size);
		}

		/// <summary>Positions child elements and determines a size for the <see cref="T:System.Windows.Documents.AdornerLayer" />.</summary>
		/// <param name="finalSize">The size reserved for this element by its parent.</param>
		/// <returns>The actual size needed by the element.  This return value is typically the same as the value passed to finalSize.</returns>
		// Token: 0x06002AB7 RID: 10935 RVA: 0x000C3244 File Offset: 0x000C1444
		protected override Size ArrangeOverride(Size finalSize)
		{
			DictionaryEntry[] array = new DictionaryEntry[this._zOrderMap.Count];
			this._zOrderMap.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				ArrayList arrayList = (ArrayList)array[i].Value;
				int j = 0;
				while (j < arrayList.Count)
				{
					AdornerLayer.AdornerInfo adornerInfo = (AdornerLayer.AdornerInfo)arrayList[j++];
					if (!adornerInfo.Adorner.IsArrangeValid)
					{
						adornerInfo.Adorner.Arrange(new Rect(default(Point), adornerInfo.Adorner.DesiredSize));
						GeneralTransform desiredTransform = adornerInfo.Adorner.GetDesiredTransform(adornerInfo.Transform);
						GeneralTransform proposedTransform = this.GetProposedTransform(adornerInfo.Adorner, desiredTransform);
						int num = this._children.IndexOf(adornerInfo.Adorner);
						if (num >= 0)
						{
							Transform adornerTransform = (proposedTransform != null) ? proposedTransform.AffineTransform : null;
							((Adorner)this._children[num]).AdornerTransform = adornerTransform;
						}
					}
					if (adornerInfo.Adorner.IsClipEnabled)
					{
						adornerInfo.Adorner.AdornerClip = adornerInfo.Clip;
					}
					else if (adornerInfo.Adorner.AdornerClip != null)
					{
						adornerInfo.Adorner.AdornerClip = null;
					}
				}
			}
			return finalSize;
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x000C339C File Offset: 0x000C159C
		internal void Add(Adorner adorner, int zOrder)
		{
			if (adorner == null)
			{
				throw new ArgumentNullException("adorner");
			}
			AdornerLayer.AdornerInfo adornerInfo = new AdornerLayer.AdornerInfo(adorner);
			adornerInfo.ZOrder = zOrder;
			this.AddAdornerInfo(this.ElementMap, adornerInfo, adorner.AdornedElement);
			this.AddAdornerToVisualTree(adornerInfo, zOrder);
			base.AddLogicalChild(adorner);
			this.UpdateAdorner(adorner.AdornedElement);
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x000C33F3 File Offset: 0x000C15F3
		internal void InvalidateAdorner(AdornerLayer.AdornerInfo adornerInfo)
		{
			adornerInfo.Adorner.InvalidateMeasure();
			adornerInfo.Adorner.InvalidateVisual();
			adornerInfo.RenderSize = new Size(double.NaN, double.NaN);
			adornerInfo.Transform = null;
		}

		// Token: 0x06002ABA RID: 10938 RVA: 0x000C342F File Offset: 0x000C162F
		internal void OnLayoutUpdated(object sender, EventArgs args)
		{
			if (this.ElementMap.Count == 0)
			{
				return;
			}
			this.UpdateAdorner(null);
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000C3448 File Offset: 0x000C1648
		internal void SetAdornerZOrder(Adorner adorner, int zOrder)
		{
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				throw new InvalidOperationException(SR.Get("AdornerNotFound"));
			}
			this.RemoveAdornerInfo(this._zOrderMap, adorner, adornerInfo.ZOrder);
			this._children.Remove(adorner);
			adornerInfo.ZOrder = zOrder;
			this.AddAdornerToVisualTree(adornerInfo, zOrder);
			this.InvalidateAdorner(adornerInfo);
			this.UpdateAdorner(adorner.AdornedElement);
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000C34E4 File Offset: 0x000C16E4
		internal int GetAdornerZOrder(Adorner adorner)
		{
			ArrayList arrayList = this.ElementMap[adorner.AdornedElement] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
			if (adornerInfo == null)
			{
				throw new InvalidOperationException(SR.Get("AdornerNotFound"));
			}
			return adornerInfo.ZOrder;
		}

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06002ABD RID: 10941 RVA: 0x000C353D File Offset: 0x000C173D
		internal HybridDictionary ElementMap
		{
			get
			{
				return this._elementMap;
			}
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000C3548 File Offset: 0x000C1748
		private void AddAdornerToVisualTree(AdornerLayer.AdornerInfo adornerInfo, int zOrder)
		{
			Adorner adorner = adornerInfo.Adorner;
			this.AddAdornerInfo(this._zOrderMap, adornerInfo, zOrder);
			ArrayList arrayList = (ArrayList)this._zOrderMap[zOrder];
			if (arrayList.Count > 1)
			{
				int num = arrayList.IndexOf(adornerInfo);
				int index = this._children.IndexOf(((AdornerLayer.AdornerInfo)arrayList[num - 1]).Adorner) + 1;
				this._children.Insert(index, adorner);
				return;
			}
			IList keyList = this._zOrderMap.GetKeyList();
			int num2 = keyList.IndexOf(zOrder) - 1;
			if (num2 < 0)
			{
				this._children.Insert(0, adorner);
				return;
			}
			arrayList = (ArrayList)this._zOrderMap[keyList[num2]];
			int index2 = this._children.IndexOf(((AdornerLayer.AdornerInfo)arrayList[arrayList.Count - 1]).Adorner) + 1;
			this._children.Insert(index2, adorner);
		}

		// Token: 0x06002ABF RID: 10943 RVA: 0x000C3648 File Offset: 0x000C1848
		private void Clear(UIElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				throw new InvalidOperationException(SR.Get("AdornedElementNotFound"));
			}
			while (arrayList.Count > 0)
			{
				AdornerLayer.AdornerInfo adornerInfo = arrayList[0] as AdornerLayer.AdornerInfo;
				this.Remove(adornerInfo.Adorner);
			}
			this.ElementMap.Remove(element);
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000C36B8 File Offset: 0x000C18B8
		private void UpdateElementAdorners(UIElement element)
		{
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null)
			{
				return;
			}
			ArrayList arrayList = this.ElementMap[element] as ArrayList;
			if (arrayList == null)
			{
				return;
			}
			bool flag = false;
			GeneralTransform generalTransform = element.TransformToAncestor(visual);
			for (int i = 0; i < arrayList.Count; i++)
			{
				AdornerLayer.AdornerInfo adornerInfo = (AdornerLayer.AdornerInfo)arrayList[i];
				Size renderSize = element.RenderSize;
				Geometry geometry = null;
				bool flag2 = false;
				if (adornerInfo.Adorner.IsClipEnabled)
				{
					geometry = this.GetClipGeometry(adornerInfo.Adorner.AdornedElement, adornerInfo.Adorner);
					if ((adornerInfo.Clip == null && geometry != null) || (adornerInfo.Clip != null && geometry == null) || (adornerInfo.Clip != null && geometry != null && adornerInfo.Clip.Bounds != geometry.Bounds))
					{
						flag2 = true;
					}
				}
				if (adornerInfo.Adorner.NeedsUpdate(adornerInfo.RenderSize) || adornerInfo.Transform == null || generalTransform.AffineTransform == null || adornerInfo.Transform.AffineTransform == null || generalTransform.AffineTransform.Value != adornerInfo.Transform.AffineTransform.Value || flag2)
				{
					this.InvalidateAdorner(adornerInfo);
					adornerInfo.RenderSize = renderSize;
					adornerInfo.Transform = generalTransform;
					if (adornerInfo.Adorner.IsClipEnabled)
					{
						adornerInfo.Clip = geometry;
					}
					flag = true;
				}
			}
			if (flag)
			{
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000C3834 File Offset: 0x000C1A34
		private void UpdateAdorner(UIElement element)
		{
			Visual visual = VisualTreeHelper.GetParent(this) as Visual;
			if (visual == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList(1);
			if (element != null)
			{
				if (!element.IsDescendantOf(visual))
				{
					arrayList.Add(element);
				}
				else
				{
					this.UpdateElementAdorners(element);
				}
			}
			else
			{
				ICollection keys = this.ElementMap.Keys;
				UIElement[] array = new UIElement[keys.Count];
				keys.CopyTo(array, 0);
				foreach (UIElement uielement in array)
				{
					if (!uielement.IsDescendantOf(visual))
					{
						arrayList.Add(uielement);
					}
					else
					{
						this.UpdateElementAdorners(uielement);
					}
				}
			}
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.Clear((UIElement)arrayList[j]);
			}
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000C38F4 File Offset: 0x000C1AF4
		private CombinedGeometry GetClipGeometry(Visual element, Adorner adorner)
		{
			Visual visual = null;
			Visual visual2 = VisualTreeHelper.GetParent(this) as Visual;
			if (visual2 == null)
			{
				return null;
			}
			CombinedGeometry combinedGeometry = null;
			if (!visual2.IsAncestorOf(element))
			{
				return null;
			}
			while (element != visual2 && element != null)
			{
				Geometry clip = VisualTreeHelper.GetClip(element);
				if (clip != null)
				{
					if (combinedGeometry == null)
					{
						combinedGeometry = new CombinedGeometry(clip, null);
					}
					else
					{
						GeneralTransform generalTransform = visual.TransformToAncestor(element);
						combinedGeometry.Transform = generalTransform.AffineTransform;
						combinedGeometry = new CombinedGeometry(combinedGeometry, clip);
						combinedGeometry.GeometryCombineMode = GeometryCombineMode.Intersect;
					}
					visual = element;
				}
				element = (Visual)VisualTreeHelper.GetParent(element);
			}
			if (combinedGeometry != null)
			{
				GeneralTransform generalTransform2 = visual.TransformToAncestor(visual2);
				if (generalTransform2 == null)
				{
					combinedGeometry = null;
				}
				else
				{
					TransformGroup transformGroup = new TransformGroup();
					transformGroup.Children.Add(generalTransform2.AffineTransform);
					generalTransform2 = visual2.TransformToDescendant(adorner);
					if (generalTransform2 == null)
					{
						combinedGeometry = null;
					}
					else
					{
						transformGroup.Children.Add(generalTransform2.AffineTransform);
						combinedGeometry.Transform = transformGroup;
					}
				}
			}
			return combinedGeometry;
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x000C39D0 File Offset: 0x000C1BD0
		private bool RemoveAdornerInfo(IDictionary infoMap, Adorner adorner, object key)
		{
			ArrayList arrayList = infoMap[key] as ArrayList;
			if (arrayList != null)
			{
				AdornerLayer.AdornerInfo adornerInfo = this.GetAdornerInfo(arrayList, adorner);
				if (adornerInfo != null)
				{
					arrayList.Remove(adornerInfo);
					if (arrayList.Count == 0)
					{
						infoMap.Remove(key);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000C3A14 File Offset: 0x000C1C14
		private AdornerLayer.AdornerInfo GetAdornerInfo(ArrayList adornerInfos, Adorner adorner)
		{
			if (adornerInfos != null)
			{
				for (int i = 0; i < adornerInfos.Count; i++)
				{
					if (((AdornerLayer.AdornerInfo)adornerInfos[i]).Adorner == adorner)
					{
						return (AdornerLayer.AdornerInfo)adornerInfos[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000C3A58 File Offset: 0x000C1C58
		private void AddAdornerInfo(IDictionary infoMap, AdornerLayer.AdornerInfo adornerInfo, object key)
		{
			ArrayList arrayList;
			if (infoMap[key] == null)
			{
				arrayList = new ArrayList(1);
				infoMap[key] = arrayList;
			}
			else
			{
				arrayList = (ArrayList)infoMap[key];
			}
			arrayList.Add(adornerInfo);
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06002AC6 RID: 10950 RVA: 0x00094CFC File Offset: 0x00092EFC
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x000C3A94 File Offset: 0x000C1C94
		private GeneralTransform GetProposedTransform(Adorner adorner, GeneralTransform sourceTransform)
		{
			if (adorner.FlowDirection != base.FlowDirection)
			{
				GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
				Matrix matrix = new Matrix(-1.0, 0.0, 0.0, 1.0, adorner.RenderSize.Width, 0.0);
				MatrixTransform value = new MatrixTransform(matrix);
				generalTransformGroup.Children.Add(value);
				if (sourceTransform != null && sourceTransform != Transform.Identity)
				{
					generalTransformGroup.Children.Add(sourceTransform);
				}
				return generalTransformGroup;
			}
			return sourceTransform;
		}

		// Token: 0x04001C4B RID: 7243
		private HybridDictionary _elementMap = new HybridDictionary(10);

		// Token: 0x04001C4C RID: 7244
		private SortedList _zOrderMap = new SortedList(10);

		// Token: 0x04001C4D RID: 7245
		private const int DefaultZOrder = 2147483647;

		// Token: 0x04001C4E RID: 7246
		private VisualCollection _children;

		// Token: 0x020008C4 RID: 2244
		internal class AdornerInfo
		{
			// Token: 0x06008458 RID: 33880 RVA: 0x0024811F File Offset: 0x0024631F
			internal AdornerInfo(Adorner adorner)
			{
				Invariant.Assert(adorner != null);
				this._adorner = adorner;
			}

			// Token: 0x17001DF4 RID: 7668
			// (get) Token: 0x06008459 RID: 33881 RVA: 0x00248137 File Offset: 0x00246337
			internal Adorner Adorner
			{
				get
				{
					return this._adorner;
				}
			}

			// Token: 0x17001DF5 RID: 7669
			// (get) Token: 0x0600845A RID: 33882 RVA: 0x0024813F File Offset: 0x0024633F
			// (set) Token: 0x0600845B RID: 33883 RVA: 0x00248147 File Offset: 0x00246347
			internal Size RenderSize
			{
				get
				{
					return this._computedSize;
				}
				set
				{
					this._computedSize = value;
				}
			}

			// Token: 0x17001DF6 RID: 7670
			// (get) Token: 0x0600845C RID: 33884 RVA: 0x00248150 File Offset: 0x00246350
			// (set) Token: 0x0600845D RID: 33885 RVA: 0x00248158 File Offset: 0x00246358
			internal GeneralTransform Transform
			{
				get
				{
					return this._transform;
				}
				set
				{
					this._transform = value;
				}
			}

			// Token: 0x17001DF7 RID: 7671
			// (get) Token: 0x0600845E RID: 33886 RVA: 0x00248161 File Offset: 0x00246361
			// (set) Token: 0x0600845F RID: 33887 RVA: 0x00248169 File Offset: 0x00246369
			internal int ZOrder
			{
				get
				{
					return this._zOrder;
				}
				set
				{
					this._zOrder = value;
				}
			}

			// Token: 0x17001DF8 RID: 7672
			// (get) Token: 0x06008460 RID: 33888 RVA: 0x00248172 File Offset: 0x00246372
			// (set) Token: 0x06008461 RID: 33889 RVA: 0x0024817A File Offset: 0x0024637A
			internal Geometry Clip
			{
				get
				{
					return this._clip;
				}
				set
				{
					this._clip = value;
				}
			}

			// Token: 0x0400421B RID: 16923
			private Adorner _adorner;

			// Token: 0x0400421C RID: 16924
			private Size _computedSize;

			// Token: 0x0400421D RID: 16925
			private GeneralTransform _transform;

			// Token: 0x0400421E RID: 16926
			private int _zOrder;

			// Token: 0x0400421F RID: 16927
			private Geometry _clip;
		}
	}
}

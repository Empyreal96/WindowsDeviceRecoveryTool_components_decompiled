using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MS.Internal.Data
{
	// Token: 0x0200070F RID: 1807
	internal class CommitManager
	{
		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x06007435 RID: 29749 RVA: 0x002137F5 File Offset: 0x002119F5
		internal bool IsEmpty
		{
			get
			{
				return this._bindings.Count == 0 && this._bindingGroups.Count == 0;
			}
		}

		// Token: 0x06007436 RID: 29750 RVA: 0x00213814 File Offset: 0x00211A14
		internal void AddBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Add(bindingGroup);
		}

		// Token: 0x06007437 RID: 29751 RVA: 0x00213822 File Offset: 0x00211A22
		internal void RemoveBindingGroup(BindingGroup bindingGroup)
		{
			this._bindingGroups.Remove(bindingGroup);
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x00213831 File Offset: 0x00211A31
		internal void AddBinding(BindingExpressionBase binding)
		{
			this._bindings.Add(binding);
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x0021383F File Offset: 0x00211A3F
		internal void RemoveBinding(BindingExpressionBase binding)
		{
			this._bindings.Remove(binding);
		}

		// Token: 0x0600743A RID: 29754 RVA: 0x00213850 File Offset: 0x00211A50
		internal List<BindingGroup> GetBindingGroupsInScope(DependencyObject element)
		{
			List<BindingGroup> list = this._bindingGroups.ToList();
			List<BindingGroup> list2 = CommitManager.EmptyBindingGroupList;
			foreach (BindingGroup bindingGroup in list)
			{
				DependencyObject owner = bindingGroup.Owner;
				if (owner != null && this.IsInScope(element, owner))
				{
					if (list2 == CommitManager.EmptyBindingGroupList)
					{
						list2 = new List<BindingGroup>();
					}
					list2.Add(bindingGroup);
				}
			}
			return list2;
		}

		// Token: 0x0600743B RID: 29755 RVA: 0x002138D8 File Offset: 0x00211AD8
		internal List<BindingExpressionBase> GetBindingsInScope(DependencyObject element)
		{
			List<BindingExpressionBase> list = this._bindings.ToList();
			List<BindingExpressionBase> list2 = CommitManager.EmptyBindingList;
			foreach (BindingExpressionBase bindingExpressionBase in list)
			{
				DependencyObject targetElement = bindingExpressionBase.TargetElement;
				if (targetElement != null && bindingExpressionBase.IsEligibleForCommit && this.IsInScope(element, targetElement))
				{
					if (list2 == CommitManager.EmptyBindingList)
					{
						list2 = new List<BindingExpressionBase>();
					}
					list2.Add(bindingExpressionBase);
				}
			}
			return list2;
		}

		// Token: 0x0600743C RID: 29756 RVA: 0x00213968 File Offset: 0x00211B68
		internal bool Purge()
		{
			bool flag = false;
			int count = this._bindings.Count;
			if (count > 0)
			{
				List<BindingExpressionBase> list = this._bindings.ToList();
				foreach (BindingExpressionBase bindingExpressionBase in list)
				{
					DependencyObject targetElement = bindingExpressionBase.TargetElement;
				}
			}
			flag = (flag || this._bindings.Count < count);
			count = this._bindingGroups.Count;
			if (count > 0)
			{
				List<BindingGroup> list2 = this._bindingGroups.ToList();
				foreach (BindingGroup bindingGroup in list2)
				{
					DependencyObject owner = bindingGroup.Owner;
				}
			}
			flag = (flag || this._bindingGroups.Count < count);
			return flag;
		}

		// Token: 0x0600743D RID: 29757 RVA: 0x00213A64 File Offset: 0x00211C64
		private bool IsInScope(DependencyObject ancestor, DependencyObject element)
		{
			return ancestor == null || VisualTreeHelper.IsAncestorOf(ancestor, element);
		}

		// Token: 0x040037CA RID: 14282
		private CommitManager.Set<BindingGroup> _bindingGroups = new CommitManager.Set<BindingGroup>();

		// Token: 0x040037CB RID: 14283
		private CommitManager.Set<BindingExpressionBase> _bindings = new CommitManager.Set<BindingExpressionBase>();

		// Token: 0x040037CC RID: 14284
		private static readonly List<BindingGroup> EmptyBindingGroupList = new List<BindingGroup>();

		// Token: 0x040037CD RID: 14285
		private static readonly List<BindingExpressionBase> EmptyBindingList = new List<BindingExpressionBase>();

		// Token: 0x02000B49 RID: 2889
		private class Set<T> : Dictionary<T, object>, IEnumerable<T>, IEnumerable
		{
			// Token: 0x06008DA3 RID: 36259 RVA: 0x00259DB5 File Offset: 0x00257FB5
			public Set()
			{
			}

			// Token: 0x06008DA4 RID: 36260 RVA: 0x00259DBD File Offset: 0x00257FBD
			public Set(IDictionary<T, object> other) : base(other)
			{
			}

			// Token: 0x06008DA5 RID: 36261 RVA: 0x00259DC6 File Offset: 0x00257FC6
			public Set(IEqualityComparer<T> comparer) : base(comparer)
			{
			}

			// Token: 0x06008DA6 RID: 36262 RVA: 0x00259DCF File Offset: 0x00257FCF
			public void Add(T item)
			{
				if (!base.ContainsKey(item))
				{
					base.Add(item, null);
				}
			}

			// Token: 0x06008DA7 RID: 36263 RVA: 0x00259DE2 File Offset: 0x00257FE2
			IEnumerator<T> IEnumerable<!0>.GetEnumerator()
			{
				return base.Keys.GetEnumerator();
			}

			// Token: 0x06008DA8 RID: 36264 RVA: 0x00259DF4 File Offset: 0x00257FF4
			public List<T> ToList()
			{
				return new List<T>(base.Keys);
			}
		}
	}
}

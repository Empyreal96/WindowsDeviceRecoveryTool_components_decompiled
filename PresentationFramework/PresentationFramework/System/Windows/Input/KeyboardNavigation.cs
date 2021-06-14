using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Input
{
	/// <summary>Provides logical and directional navigation between focusable objects. </summary>
	// Token: 0x0200017D RID: 381
	public sealed class KeyboardNavigation
	{
		// Token: 0x06001603 RID: 5635 RVA: 0x0006D488 File Offset: 0x0006B688
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal KeyboardNavigation()
		{
			InputManager inputManager = InputManager.Current;
			inputManager.PostProcessInput += this.PostProcessInput;
			inputManager.TranslateAccelerator += this.TranslateAccelerator;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0006D508 File Offset: 0x0006B708
		internal static DependencyObject GetTabOnceActiveElement(DependencyObject d)
		{
			WeakReference weakReference = (WeakReference)d.GetValue(KeyboardNavigation.TabOnceActiveElementProperty);
			if (weakReference != null && weakReference.IsAlive)
			{
				DependencyObject dependencyObject = weakReference.Target as DependencyObject;
				if (KeyboardNavigation.GetVisualRoot(dependencyObject) == KeyboardNavigation.GetVisualRoot(d))
				{
					return dependencyObject;
				}
				d.SetValue(KeyboardNavigation.TabOnceActiveElementProperty, null);
			}
			return null;
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0006D55A File Offset: 0x0006B75A
		internal static void SetTabOnceActiveElement(DependencyObject d, DependencyObject value)
		{
			d.SetValue(KeyboardNavigation.TabOnceActiveElementProperty, new WeakReference(value));
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0006D570 File Offset: 0x0006B770
		private static DependencyObject GetControlTabOnceActiveElement(DependencyObject d)
		{
			WeakReference weakReference = (WeakReference)d.GetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty);
			if (weakReference != null && weakReference.IsAlive)
			{
				DependencyObject dependencyObject = weakReference.Target as DependencyObject;
				if (KeyboardNavigation.GetVisualRoot(dependencyObject) == KeyboardNavigation.GetVisualRoot(d))
				{
					return dependencyObject;
				}
				d.SetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty, null);
			}
			return null;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0006D5C2 File Offset: 0x0006B7C2
		private static void SetControlTabOnceActiveElement(DependencyObject d, DependencyObject value)
		{
			d.SetValue(KeyboardNavigation.ControlTabOnceActiveElementProperty, new WeakReference(value));
		}

		// Token: 0x06001608 RID: 5640 RVA: 0x0006D5D5 File Offset: 0x0006B7D5
		private DependencyObject GetActiveElement(DependencyObject d)
		{
			if (this._navigationProperty != KeyboardNavigation.ControlTabNavigationProperty)
			{
				return KeyboardNavigation.GetTabOnceActiveElement(d);
			}
			return KeyboardNavigation.GetControlTabOnceActiveElement(d);
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x0006D5F1 File Offset: 0x0006B7F1
		private void SetActiveElement(DependencyObject d, DependencyObject value)
		{
			if (this._navigationProperty == KeyboardNavigation.TabNavigationProperty)
			{
				KeyboardNavigation.SetTabOnceActiveElement(d, value);
				return;
			}
			KeyboardNavigation.SetControlTabOnceActiveElement(d, value);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x0006D610 File Offset: 0x0006B810
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static Visual GetVisualRoot(DependencyObject d)
		{
			if (d is Visual || d is Visual3D)
			{
				PresentationSource presentationSource = PresentationSource.CriticalFromVisual(d);
				if (presentationSource != null)
				{
					return presentationSource.RootVisual;
				}
			}
			else
			{
				FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					return KeyboardNavigation.GetVisualRoot(frameworkContentElement.Parent);
				}
			}
			return null;
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x0006D655 File Offset: 0x0006B855
		private static object CoerceShowKeyboardCues(DependencyObject d, object value)
		{
			if (!SystemParameters.KeyboardCues)
			{
				return value;
			}
			return BooleanBoxes.TrueBox;
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x0600160C RID: 5644 RVA: 0x0006D668 File Offset: 0x0006B868
		// (remove) Token: 0x0600160D RID: 5645 RVA: 0x0006D6B0 File Offset: 0x0006B8B0
		internal event KeyboardFocusChangedEventHandler FocusChanged
		{
			add
			{
				KeyboardNavigation.WeakReferenceList weakFocusChangedHandlers = this._weakFocusChangedHandlers;
				lock (weakFocusChangedHandlers)
				{
					this._weakFocusChangedHandlers.Add(value);
				}
			}
			remove
			{
				KeyboardNavigation.WeakReferenceList weakFocusChangedHandlers = this._weakFocusChangedHandlers;
				lock (weakFocusChangedHandlers)
				{
					this._weakFocusChangedHandlers.Remove(value);
				}
			}
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x0006D6F8 File Offset: 0x0006B8F8
		internal void NotifyFocusChanged(object sender, KeyboardFocusChangedEventArgs e)
		{
			this._weakFocusChangedHandlers.Process(delegate(object item)
			{
				KeyboardFocusChangedEventHandler keyboardFocusChangedEventHandler = item as KeyboardFocusChangedEventHandler;
				if (keyboardFocusChangedEventHandler != null)
				{
					keyboardFocusChangedEventHandler(sender, e);
				}
				return false;
			});
		}

		/// <summary>Set the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabIndex" /> attached property for the specified element. </summary>
		/// <param name="element">The element on which to set the attached property to.</param>
		/// <param name="index">The property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600160F RID: 5647 RVA: 0x0006D730 File Offset: 0x0006B930
		public static void SetTabIndex(DependencyObject element, int index)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.TabIndexProperty, index);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabIndex" />  attached property for the specified element. </summary>
		/// <param name="element">The element from which to read the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabIndex" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001610 RID: 5648 RVA: 0x0006D751 File Offset: 0x0006B951
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static int GetTabIndex(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return KeyboardNavigation.GetTabIndexHelper(element);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.IsTabStop" /> attached property for the specified element. </summary>
		/// <param name="element">The element to which to write the attached property.</param>
		/// <param name="isTabStop">The property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001611 RID: 5649 RVA: 0x0006D767 File Offset: 0x0006B967
		public static void SetIsTabStop(DependencyObject element, bool isTabStop)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.IsTabStopProperty, BooleanBoxes.Box(isTabStop));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.IsTabStop" /> attached property for the specified element. </summary>
		/// <param name="element">The element from which to read the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.IsTabStop" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001612 RID: 5650 RVA: 0x0006D788 File Offset: 0x0006B988
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetIsTabStop(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(KeyboardNavigation.IsTabStopProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element on which to set the attached property.</param>
		/// <param name="mode">Property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001613 RID: 5651 RVA: 0x0006D7A8 File Offset: 0x0006B9A8
		public static void SetTabNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.TabNavigationProperty, mode);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element from which to get the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.TabNavigation" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001614 RID: 5652 RVA: 0x0006D7C9 File Offset: 0x0006B9C9
		[CustomCategory("Accessibility")]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static KeyboardNavigationMode GetTabNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.TabNavigationProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.ControlTabNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element on which to set the attached property.</param>
		/// <param name="mode">The property value to set</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001615 RID: 5653 RVA: 0x0006D7E9 File Offset: 0x0006B9E9
		public static void SetControlTabNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.ControlTabNavigationProperty, mode);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.ControlTabNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element from which to get the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.ControlTabNavigation" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001616 RID: 5654 RVA: 0x0006D80A File Offset: 0x0006BA0A
		[CustomCategory("Accessibility")]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static KeyboardNavigationMode GetControlTabNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.ControlTabNavigationProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.DirectionalNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element on which to set the attached property.</param>
		/// <param name="mode">Property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001617 RID: 5655 RVA: 0x0006D82A File Offset: 0x0006BA2A
		public static void SetDirectionalNavigation(DependencyObject element, KeyboardNavigationMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.DirectionalNavigationProperty, mode);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.DirectionalNavigation" /> attached property for the specified element. </summary>
		/// <param name="element">Element from which to get the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.DirectionalNavigation" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001618 RID: 5656 RVA: 0x0006D84B File Offset: 0x0006BA4B
		[CustomCategory("Accessibility")]
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static KeyboardNavigationMode GetDirectionalNavigation(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (KeyboardNavigationMode)element.GetValue(KeyboardNavigation.DirectionalNavigationProperty);
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.AcceptsReturn" />  attached property for the specified element. </summary>
		/// <param name="element">The element to write the attached property to.</param>
		/// <param name="enabled">The property value to set.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06001619 RID: 5657 RVA: 0x0006D86B File Offset: 0x0006BA6B
		public static void SetAcceptsReturn(DependencyObject element, bool enabled)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(KeyboardNavigation.AcceptsReturnProperty, BooleanBoxes.Box(enabled));
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Input.KeyboardNavigation.AcceptsReturn" /> attached property for the specified element. </summary>
		/// <param name="element">The element from which to read the attached property.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Input.KeyboardNavigation.AcceptsReturn" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x0600161A RID: 5658 RVA: 0x0006D88C File Offset: 0x0006BA8C
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static bool GetAcceptsReturn(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(KeyboardNavigation.AcceptsReturnProperty);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0006D8AC File Offset: 0x0006BAAC
		private static bool IsValidKeyNavigationMode(object o)
		{
			KeyboardNavigationMode keyboardNavigationMode = (KeyboardNavigationMode)o;
			return keyboardNavigationMode == KeyboardNavigationMode.Contained || keyboardNavigationMode == KeyboardNavigationMode.Continue || keyboardNavigationMode == KeyboardNavigationMode.Cycle || keyboardNavigationMode == KeyboardNavigationMode.None || keyboardNavigationMode == KeyboardNavigationMode.Once || keyboardNavigationMode == KeyboardNavigationMode.Local;
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0006D8DC File Offset: 0x0006BADC
		internal static UIElement GetParentUIElementFromContentElement(ContentElement ce)
		{
			IContentHost contentHost = null;
			return KeyboardNavigation.GetParentUIElementFromContentElement(ce, ref contentHost);
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0006D8F4 File Offset: 0x0006BAF4
		private static UIElement GetParentUIElementFromContentElement(ContentElement ce, ref IContentHost ichParent)
		{
			if (ce == null)
			{
				return null;
			}
			IContentHost contentHost = ContentHostHelper.FindContentHost(ce);
			if (ichParent == null)
			{
				ichParent = contentHost;
			}
			DependencyObject dependencyObject = contentHost as DependencyObject;
			if (dependencyObject != null)
			{
				UIElement uielement = dependencyObject as UIElement;
				if (uielement != null)
				{
					return uielement;
				}
				Visual visual = dependencyObject as Visual;
				while (visual != null)
				{
					visual = (VisualTreeHelper.GetParent(visual) as Visual);
					UIElement uielement2 = visual as UIElement;
					if (uielement2 != null)
					{
						return uielement2;
					}
				}
				ContentElement contentElement = dependencyObject as ContentElement;
				if (contentElement != null)
				{
					return KeyboardNavigation.GetParentUIElementFromContentElement(contentElement, ref ichParent);
				}
			}
			return null;
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0006D968 File Offset: 0x0006BB68
		internal void HideFocusVisual()
		{
			if (this._focusVisualAdornerCache != null)
			{
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this._focusVisualAdornerCache) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Remove(this._focusVisualAdornerCache);
				}
				this._focusVisualAdornerCache = null;
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0006D9A4 File Offset: 0x0006BBA4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static bool IsKeyboardMostRecentInputDevice()
		{
			return InputManager.Current.MostRecentInputDevice is KeyboardDevice;
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x0006D9B8 File Offset: 0x0006BBB8
		// (set) Token: 0x06001621 RID: 5665 RVA: 0x0006D9BF File Offset: 0x0006BBBF
		internal static bool AlwaysShowFocusVisual
		{
			get
			{
				return KeyboardNavigation._alwaysShowFocusVisual;
			}
			set
			{
				KeyboardNavigation._alwaysShowFocusVisual = value;
			}
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0006D9C7 File Offset: 0x0006BBC7
		internal static void ShowFocusVisual()
		{
			KeyboardNavigation.Current.ShowFocusVisual(Keyboard.FocusedElement as DependencyObject);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0006D9E0 File Offset: 0x0006BBE0
		private void ShowFocusVisual(DependencyObject element)
		{
			this.HideFocusVisual();
			if (!KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				KeyboardNavigation.EnableKeyboardCues(element, false);
			}
			if (KeyboardNavigation.AlwaysShowFocusVisual || KeyboardNavigation.IsKeyboardMostRecentInputDevice())
			{
				FrameworkElement frameworkElement = element as FrameworkElement;
				if (frameworkElement != null)
				{
					AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(frameworkElement);
					if (adornerLayer == null)
					{
						return;
					}
					Style style = frameworkElement.FocusVisualStyle;
					if (style == FrameworkElement.DefaultFocusVisualStyle)
					{
						style = (SystemResources.FindResourceInternal(SystemParameters.FocusVisualStyleKey) as Style);
					}
					if (style != null)
					{
						this._focusVisualAdornerCache = new KeyboardNavigation.FocusVisualAdorner(frameworkElement, style);
						adornerLayer.Add(this._focusVisualAdornerCache);
						return;
					}
				}
				else
				{
					FrameworkContentElement frameworkContentElement = element as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						IContentHost contentHost = null;
						UIElement parentUIElementFromContentElement = KeyboardNavigation.GetParentUIElementFromContentElement(frameworkContentElement, ref contentHost);
						if (contentHost != null && parentUIElementFromContentElement != null)
						{
							AdornerLayer adornerLayer2 = AdornerLayer.GetAdornerLayer(parentUIElementFromContentElement);
							if (adornerLayer2 != null)
							{
								Style style2 = frameworkContentElement.FocusVisualStyle;
								if (style2 == FrameworkElement.DefaultFocusVisualStyle)
								{
									style2 = (SystemResources.FindResourceInternal(SystemParameters.FocusVisualStyleKey) as Style);
								}
								if (style2 != null)
								{
									this._focusVisualAdornerCache = new KeyboardNavigation.FocusVisualAdorner(frameworkContentElement, parentUIElementFromContentElement, contentHost, style2);
									adornerLayer2.Add(this._focusVisualAdornerCache);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0006DAD8 File Offset: 0x0006BCD8
		internal static void UpdateFocusedElement(DependencyObject focusTarget)
		{
			DependencyObject focusScope = FocusManager.GetFocusScope(focusTarget);
			if (focusScope != null && focusScope != focusTarget)
			{
				FocusManager.SetFocusedElement(focusScope, focusTarget as IInputElement);
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(focusTarget);
				if (visualRoot != null && focusScope == visualRoot)
				{
					KeyboardNavigation.Current.NotifyFocusEnterMainFocusScope(visualRoot, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0006DB1D File Offset: 0x0006BD1D
		internal void UpdateActiveElement(DependencyObject activeElement)
		{
			this.UpdateActiveElement(activeElement, KeyboardNavigation.TabNavigationProperty);
			this.UpdateActiveElement(activeElement, KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x0006DB38 File Offset: 0x0006BD38
		private void UpdateActiveElement(DependencyObject activeElement, DependencyProperty dp)
		{
			this._navigationProperty = dp;
			DependencyObject groupParent = this.GetGroupParent(activeElement);
			this.UpdateActiveElement(groupParent, activeElement, dp);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0006DB5D File Offset: 0x0006BD5D
		internal void UpdateActiveElement(DependencyObject container, DependencyObject activeElement)
		{
			this.UpdateActiveElement(container, activeElement, KeyboardNavigation.TabNavigationProperty);
			this.UpdateActiveElement(container, activeElement, KeyboardNavigation.ControlTabNavigationProperty);
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x0006DB79 File Offset: 0x0006BD79
		private void UpdateActiveElement(DependencyObject container, DependencyObject activeElement, DependencyProperty dp)
		{
			this._navigationProperty = dp;
			if (activeElement == container)
			{
				return;
			}
			if (this.GetKeyNavigationMode(container) == KeyboardNavigationMode.Once)
			{
				this.SetActiveElement(container, activeElement);
			}
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x0006DB99 File Offset: 0x0006BD99
		internal bool Navigate(DependencyObject currentElement, TraversalRequest request)
		{
			return this.Navigate(currentElement, request, Keyboard.Modifiers, false);
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x0006DBA9 File Offset: 0x0006BDA9
		private bool Navigate(DependencyObject currentElement, TraversalRequest request, ModifierKeys modifierKeys, bool fromProcessInputTabKey = false)
		{
			return this.Navigate(currentElement, request, modifierKeys, null, fromProcessInputTabKey);
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x0006DBB8 File Offset: 0x0006BDB8
		private bool Navigate(DependencyObject currentElement, TraversalRequest request, ModifierKeys modifierKeys, DependencyObject firstElement, bool fromProcessInputTabKey = false)
		{
			DependencyObject dependencyObject = null;
			switch (request.FocusNavigationDirection)
			{
			case FocusNavigationDirection.Next:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetNextTab(currentElement, this.GetGroupParent(currentElement, true), false);
				break;
			case FocusNavigationDirection.Previous:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetPrevTab(currentElement, null, false);
				break;
			case FocusNavigationDirection.First:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetNextTab(null, currentElement, true);
				break;
			case FocusNavigationDirection.Last:
				this._navigationProperty = (((modifierKeys & ModifierKeys.Control) == ModifierKeys.Control) ? KeyboardNavigation.ControlTabNavigationProperty : KeyboardNavigation.TabNavigationProperty);
				dependencyObject = this.GetPrevTab(null, currentElement, true);
				break;
			case FocusNavigationDirection.Left:
			case FocusNavigationDirection.Right:
			case FocusNavigationDirection.Up:
			case FocusNavigationDirection.Down:
				this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
				dependencyObject = this.GetNextInDirection(currentElement, request.FocusNavigationDirection);
				break;
			}
			if (dependencyObject == null)
			{
				if (request.Wrapped || request.FocusNavigationDirection == FocusNavigationDirection.First || request.FocusNavigationDirection == FocusNavigationDirection.Last)
				{
					return false;
				}
				bool flag = true;
				bool flag2 = this.NavigateOutsidePresentationSource(currentElement, request, fromProcessInputTabKey, ref flag);
				if (flag2)
				{
					return true;
				}
				if (flag && (request.FocusNavigationDirection == FocusNavigationDirection.Next || request.FocusNavigationDirection == FocusNavigationDirection.Previous))
				{
					Visual visualRoot = KeyboardNavigation.GetVisualRoot(currentElement);
					if (visualRoot != null)
					{
						return this.Navigate(visualRoot, new TraversalRequest((request.FocusNavigationDirection == FocusNavigationDirection.Next) ? FocusNavigationDirection.First : FocusNavigationDirection.Last));
					}
				}
				return false;
			}
			else
			{
				IKeyboardInputSink keyboardInputSink = dependencyObject as IKeyboardInputSink;
				if (keyboardInputSink == null)
				{
					IInputElement inputElement = dependencyObject as IInputElement;
					inputElement.Focus();
					return inputElement.IsKeyboardFocusWithin;
				}
				bool flag3;
				if (request.FocusNavigationDirection == FocusNavigationDirection.First || request.FocusNavigationDirection == FocusNavigationDirection.Next)
				{
					flag3 = keyboardInputSink.TabInto(new TraversalRequest(FocusNavigationDirection.First));
				}
				else if (request.FocusNavigationDirection == FocusNavigationDirection.Last || request.FocusNavigationDirection == FocusNavigationDirection.Previous)
				{
					flag3 = keyboardInputSink.TabInto(new TraversalRequest(FocusNavigationDirection.Last));
				}
				else
				{
					flag3 = keyboardInputSink.TabInto(new TraversalRequest(request.FocusNavigationDirection)
					{
						Wrapped = true
					});
				}
				if (!flag3 && firstElement != dependencyObject)
				{
					flag3 = this.Navigate(dependencyObject, request, modifierKeys, (firstElement == null) ? dependencyObject : firstElement, false);
				}
				return flag3;
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0006DDC8 File Offset: 0x0006BFC8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool NavigateOutsidePresentationSource(DependencyObject currentElement, TraversalRequest request, bool fromProcessInput, ref bool shouldCycle)
		{
			Visual visual = currentElement as Visual;
			if (visual == null)
			{
				visual = KeyboardNavigation.GetParentUIElementFromContentElement(currentElement as ContentElement);
				if (visual == null)
				{
					return false;
				}
			}
			IKeyboardInputSink keyboardInputSink = PresentationSource.CriticalFromVisual(visual) as IKeyboardInputSink;
			if (keyboardInputSink != null)
			{
				IKeyboardInputSite keyboardInputSite = null;
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					keyboardInputSite = keyboardInputSink.KeyboardInputSite;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (keyboardInputSite != null && this.ShouldNavigateOutsidePresentationSource(currentElement, request))
				{
					if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
					{
						IAvalonAdapter avalonAdapter = keyboardInputSite as IAvalonAdapter;
						if (avalonAdapter != null && fromProcessInput)
						{
							return avalonAdapter.OnNoMoreTabStops(request, ref shouldCycle);
						}
					}
					return keyboardInputSite.OnNoMoreTabStops(request);
				}
			}
			return false;
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0006DE64 File Offset: 0x0006C064
		private bool ShouldNavigateOutsidePresentationSource(DependencyObject currentElement, TraversalRequest request)
		{
			if (request.FocusNavigationDirection == FocusNavigationDirection.Left || request.FocusNavigationDirection == FocusNavigationDirection.Right || request.FocusNavigationDirection == FocusNavigationDirection.Up || request.FocusNavigationDirection == FocusNavigationDirection.Down)
			{
				DependencyObject groupParent;
				while ((groupParent = this.GetGroupParent(currentElement)) != null && groupParent != currentElement)
				{
					KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(groupParent);
					if (keyNavigationMode == KeyboardNavigationMode.Contained || keyNavigationMode == KeyboardNavigationMode.Cycle)
					{
						return false;
					}
					currentElement = groupParent;
				}
			}
			return true;
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x0006DEBE File Offset: 0x0006C0BE
		internal static KeyboardNavigation Current
		{
			get
			{
				return FrameworkElement.KeyboardNavigation;
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0006DEC5 File Offset: 0x0006C0C5
		[SecurityCritical]
		private void PostProcessInput(object sender, ProcessInputEventArgs e)
		{
			this.ProcessInput(e.StagingItem.Input);
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0006DED8 File Offset: 0x0006C0D8
		[SecurityCritical]
		private void TranslateAccelerator(object sender, KeyEventArgs e)
		{
			this.ProcessInput(e);
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0006DEE4 File Offset: 0x0006C0E4
		[SecurityCritical]
		private void ProcessInput(InputEventArgs inputEventArgs)
		{
			this.ProcessForMenuMode(inputEventArgs);
			this.ProcessForUIState(inputEventArgs);
			if (inputEventArgs.RoutedEvent != Keyboard.KeyDownEvent)
			{
				return;
			}
			KeyEventArgs keyEventArgs = (KeyEventArgs)inputEventArgs;
			if (keyEventArgs.Handled)
			{
				return;
			}
			DependencyObject dependencyObject = keyEventArgs.OriginalSource as DependencyObject;
			DependencyObject dependencyObject2 = keyEventArgs.KeyboardDevice.Target as DependencyObject;
			if (dependencyObject2 != null && dependencyObject != dependencyObject2 && dependencyObject is HwndHost)
			{
				dependencyObject = dependencyObject2;
			}
			if (dependencyObject == null)
			{
				HwndSource hwndSource = keyEventArgs.UnsafeInputSource as HwndSource;
				if (hwndSource == null)
				{
					return;
				}
				dependencyObject = hwndSource.RootVisual;
				if (dependencyObject == null)
				{
					return;
				}
			}
			Key realKey = this.GetRealKey(keyEventArgs);
			if (realKey != Key.Tab && realKey - Key.Left > 3)
			{
				if (realKey - Key.LeftAlt <= 1)
				{
					KeyboardNavigation.ShowFocusVisual();
					KeyboardNavigation.EnableKeyboardCues(dependencyObject, true);
				}
			}
			else
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			keyEventArgs.Handled = this.Navigate(dependencyObject, keyEventArgs.Key, keyEventArgs.KeyboardDevice.Modifiers, true);
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0006DFBC File Offset: 0x0006C1BC
		internal static void EnableKeyboardCues(DependencyObject element, bool enable)
		{
			Visual visual = element as Visual;
			if (visual == null)
			{
				visual = KeyboardNavigation.GetParentUIElementFromContentElement(element as ContentElement);
				if (visual == null)
				{
					return;
				}
			}
			Visual visualRoot = KeyboardNavigation.GetVisualRoot(visual);
			if (visualRoot != null)
			{
				visualRoot.SetValue(KeyboardNavigation.ShowKeyboardCuesProperty, enable ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0006E007 File Offset: 0x0006C207
		internal static FocusNavigationDirection KeyToTraversalDirection(Key key)
		{
			switch (key)
			{
			case Key.Left:
				return FocusNavigationDirection.Left;
			case Key.Up:
				return FocusNavigationDirection.Up;
			case Key.Right:
				return FocusNavigationDirection.Right;
			case Key.Down:
				return FocusNavigationDirection.Down;
			default:
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0006E031 File Offset: 0x0006C231
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction)
		{
			return this.PredictFocusedElement(sourceElement, direction, false);
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0006E03C File Offset: 0x0006C23C
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			return this.PredictFocusedElement(sourceElement, direction, treeViewNavigation, true);
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0006E048 File Offset: 0x0006C248
		internal DependencyObject PredictFocusedElement(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, bool considerDescendants)
		{
			if (sourceElement == null)
			{
				return null;
			}
			this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			return this.GetNextInDirection(sourceElement, direction, treeViewNavigation, considerDescendants);
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0006E084 File Offset: 0x0006C284
		internal DependencyObject PredictFocusedElementAtViewportEdge(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, FrameworkElement viewportBoundsElement, DependencyObject container)
		{
			DependencyObject result;
			try
			{
				this._containerHashtable.Clear();
				result = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement, direction, treeViewNavigation, viewportBoundsElement, container);
			}
			finally
			{
				this._containerHashtable.Clear();
			}
			return result;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0006E0CC File Offset: 0x0006C2CC
		private DependencyObject PredictFocusedElementAtViewportEdgeRecursive(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, FrameworkElement viewportBoundsElement, DependencyObject container)
		{
			this._navigationProperty = KeyboardNavigation.DirectionalNavigationProperty;
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			if (container == null)
			{
				container = this.GetGroupParent(sourceElement);
			}
			if (container == sourceElement)
			{
				return null;
			}
			if (this.IsEndlessLoop(sourceElement, container))
			{
				return null;
			}
			DependencyObject dependencyObject = this.FindElementAtViewportEdge(sourceElement, viewportBoundsElement, container, direction, treeViewNavigation);
			if (dependencyObject != null)
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					return dependencyObject;
				}
				DependencyObject sourceElement2 = dependencyObject;
				dependencyObject = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement, direction, treeViewNavigation, viewportBoundsElement, dependencyObject);
				if (dependencyObject != null)
				{
					return dependencyObject;
				}
				dependencyObject = this.PredictFocusedElementAtViewportEdgeRecursive(sourceElement2, direction, treeViewNavigation, viewportBoundsElement, null);
			}
			return dependencyObject;
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0006E164 File Offset: 0x0006C364
		internal bool Navigate(DependencyObject sourceElement, Key key, ModifierKeys modifiers, bool fromProcessInput = false)
		{
			bool result = false;
			if (key != Key.Tab)
			{
				switch (key)
				{
				case Key.Left:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Left), modifiers, false);
					break;
				case Key.Up:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Up), modifiers, false);
					break;
				case Key.Right:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Right), modifiers, false);
					break;
				case Key.Down:
					result = this.Navigate(sourceElement, new TraversalRequest(FocusNavigationDirection.Down), modifiers, false);
					break;
				}
			}
			else
			{
				result = this.Navigate(sourceElement, new TraversalRequest(((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next), modifiers, fromProcessInput);
			}
			return result;
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0006E1F8 File Offset: 0x0006C3F8
		private static bool IsInNavigationTree(DependencyObject visual)
		{
			UIElement uielement = visual as UIElement;
			if (uielement != null && uielement.IsVisible)
			{
				return true;
			}
			if (visual is IContentHost && !(visual is UIElementIsland))
			{
				return true;
			}
			UIElement3D uielement3D = visual as UIElement3D;
			return uielement3D != null && uielement3D.IsVisible;
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0006E240 File Offset: 0x0006C440
		private DependencyObject GetPreviousSibling(DependencyObject e)
		{
			DependencyObject parent = KeyboardNavigation.GetParent(e);
			IContentHost contentHost = parent as IContentHost;
			if (contentHost != null)
			{
				IInputElement inputElement = null;
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement2 = hostedElements.Current;
					if (inputElement2 == e)
					{
						return inputElement as DependencyObject;
					}
					if (inputElement2 is UIElement || inputElement2 is UIElement3D)
					{
						inputElement = inputElement2;
					}
					else
					{
						ContentElement contentElement = inputElement2 as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							inputElement = inputElement2;
						}
					}
				}
				return null;
			}
			DependencyObject dependencyObject = parent as UIElement;
			if (dependencyObject == null)
			{
				dependencyObject = (parent as UIElement3D);
			}
			DependencyObject dependencyObject2 = e as Visual;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as Visual3D);
			}
			if (dependencyObject != null && dependencyObject2 != null)
			{
				int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
				DependencyObject result = null;
				for (int i = 0; i < childrenCount; i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
					if (child == dependencyObject2)
					{
						break;
					}
					if (KeyboardNavigation.IsInNavigationTree(child))
					{
						result = child;
					}
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0006E328 File Offset: 0x0006C528
		private DependencyObject GetNextSibling(DependencyObject e)
		{
			DependencyObject parent = KeyboardNavigation.GetParent(e);
			IContentHost contentHost = parent as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				bool flag = false;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement = hostedElements.Current;
					if (flag)
					{
						if (inputElement is UIElement || inputElement is UIElement3D)
						{
							return inputElement as DependencyObject;
						}
						ContentElement contentElement = inputElement as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							return contentElement;
						}
					}
					else if (inputElement == e)
					{
						flag = true;
					}
				}
			}
			else
			{
				DependencyObject dependencyObject = parent as UIElement;
				if (dependencyObject == null)
				{
					dependencyObject = (parent as UIElement3D);
				}
				DependencyObject dependencyObject2 = e as Visual;
				if (dependencyObject2 == null)
				{
					dependencyObject2 = (e as Visual3D);
				}
				if (dependencyObject != null && dependencyObject2 != null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);
					int i;
					for (i = 0; i < childrenCount; i++)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
						if (child == dependencyObject2)
						{
							break;
						}
					}
					for (i++; i < childrenCount; i++)
					{
						DependencyObject child2 = VisualTreeHelper.GetChild(dependencyObject, i);
						if (KeyboardNavigation.IsInNavigationTree(child2))
						{
							return child2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0006E42C File Offset: 0x0006C62C
		private DependencyObject FocusedElement(DependencyObject e)
		{
			IInputElement inputElement = e as IInputElement;
			if (inputElement != null && !inputElement.IsKeyboardFocusWithin)
			{
				DependencyObject dependencyObject = FocusManager.GetFocusedElement(e) as DependencyObject;
				if (dependencyObject != null && (this._navigationProperty == KeyboardNavigation.ControlTabNavigationProperty || !this.IsFocusScope(e)))
				{
					Visual visual = dependencyObject as Visual;
					if (visual == null)
					{
						Visual3D visual3D = dependencyObject as Visual3D;
						if (visual3D == null)
						{
							visual = KeyboardNavigation.GetParentUIElementFromContentElement(dependencyObject as ContentElement);
						}
						else if (visual3D != e && visual3D.IsDescendantOf(e))
						{
							return dependencyObject;
						}
					}
					if (visual != null && visual != e && visual.IsDescendantOf(e))
					{
						return dependencyObject;
					}
				}
			}
			return null;
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0006E4B4 File Offset: 0x0006C6B4
		private DependencyObject GetFirstChild(DependencyObject e)
		{
			DependencyObject dependencyObject = this.FocusedElement(e);
			if (dependencyObject != null)
			{
				return dependencyObject;
			}
			IContentHost contentHost = e as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement = hostedElements.Current;
					if (inputElement is UIElement || inputElement is UIElement3D)
					{
						return inputElement as DependencyObject;
					}
					ContentElement contentElement = inputElement as ContentElement;
					if (contentElement != null && this.IsTabStop(contentElement))
					{
						return contentElement;
					}
				}
				return null;
			}
			DependencyObject dependencyObject2 = e as UIElement;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as UIElement3D);
			}
			if (dependencyObject2 == null || UIElementHelper.IsVisible(dependencyObject2))
			{
				DependencyObject dependencyObject3 = e as Visual;
				if (dependencyObject3 == null)
				{
					dependencyObject3 = (e as Visual3D);
				}
				if (dependencyObject3 != null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject3);
					for (int i = 0; i < childrenCount; i++)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject3, i);
						if (KeyboardNavigation.IsInNavigationTree(child))
						{
							return child;
						}
						DependencyObject firstChild = this.GetFirstChild(child);
						if (firstChild != null)
						{
							return firstChild;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0006E5A0 File Offset: 0x0006C7A0
		private DependencyObject GetLastChild(DependencyObject e)
		{
			DependencyObject dependencyObject = this.FocusedElement(e);
			if (dependencyObject != null)
			{
				return dependencyObject;
			}
			IContentHost contentHost = e as IContentHost;
			if (contentHost != null)
			{
				IEnumerator<IInputElement> hostedElements = contentHost.HostedElements;
				IInputElement inputElement = null;
				while (hostedElements.MoveNext())
				{
					IInputElement inputElement2 = hostedElements.Current;
					if (inputElement2 is UIElement || inputElement2 is UIElement3D)
					{
						inputElement = inputElement2;
					}
					else
					{
						ContentElement contentElement = inputElement2 as ContentElement;
						if (contentElement != null && this.IsTabStop(contentElement))
						{
							inputElement = inputElement2;
						}
					}
				}
				return inputElement as DependencyObject;
			}
			DependencyObject dependencyObject2 = e as UIElement;
			if (dependencyObject2 == null)
			{
				dependencyObject2 = (e as UIElement3D);
			}
			if (dependencyObject2 == null || UIElementHelper.IsVisible(dependencyObject2))
			{
				DependencyObject dependencyObject3 = e as Visual;
				if (dependencyObject3 == null)
				{
					dependencyObject3 = (e as Visual3D);
				}
				if (dependencyObject3 != null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject3);
					for (int i = childrenCount - 1; i >= 0; i--)
					{
						DependencyObject child = VisualTreeHelper.GetChild(dependencyObject3, i);
						if (KeyboardNavigation.IsInNavigationTree(child))
						{
							return child;
						}
						DependencyObject lastChild = this.GetLastChild(child);
						if (lastChild != null)
						{
							return lastChild;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x0006E694 File Offset: 0x0006C894
		internal static DependencyObject GetParent(DependencyObject e)
		{
			if (e is Visual || e is Visual3D)
			{
				DependencyObject dependencyObject = e;
				while ((dependencyObject = VisualTreeHelper.GetParent(dependencyObject)) != null)
				{
					if (KeyboardNavigation.IsInNavigationTree(dependencyObject))
					{
						return dependencyObject;
					}
				}
			}
			else
			{
				ContentElement contentElement = e as ContentElement;
				if (contentElement != null)
				{
					return ContentHostHelper.FindContentHost(contentElement) as DependencyObject;
				}
			}
			return null;
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x0006E6E4 File Offset: 0x0006C8E4
		private DependencyObject GetNextInTree(DependencyObject e, DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			if (e == container || !this.IsGroup(e))
			{
				dependencyObject = this.GetFirstChild(e);
			}
			if (dependencyObject != null || e == container)
			{
				return dependencyObject;
			}
			DependencyObject dependencyObject2 = e;
			DependencyObject nextSibling;
			for (;;)
			{
				nextSibling = this.GetNextSibling(dependencyObject2);
				if (nextSibling != null)
				{
					break;
				}
				dependencyObject2 = KeyboardNavigation.GetParent(dependencyObject2);
				if (dependencyObject2 == null || dependencyObject2 == container)
				{
					goto IL_3D;
				}
			}
			return nextSibling;
			IL_3D:
			return null;
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x0006E730 File Offset: 0x0006C930
		private DependencyObject GetPreviousInTree(DependencyObject e, DependencyObject container)
		{
			if (e == container)
			{
				return null;
			}
			DependencyObject previousSibling = this.GetPreviousSibling(e);
			if (previousSibling == null)
			{
				return KeyboardNavigation.GetParent(e);
			}
			if (this.IsGroup(previousSibling))
			{
				return previousSibling;
			}
			return this.GetLastInTree(previousSibling);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x0006E768 File Offset: 0x0006C968
		private DependencyObject GetLastInTree(DependencyObject container)
		{
			DependencyObject result;
			do
			{
				result = container;
				container = this.GetLastChild(container);
			}
			while (container != null && !this.IsGroup(container));
			if (container != null)
			{
				return container;
			}
			return result;
		}

		// Token: 0x06001644 RID: 5700 RVA: 0x0006E792 File Offset: 0x0006C992
		private DependencyObject GetGroupParent(DependencyObject e)
		{
			return this.GetGroupParent(e, false);
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0006E79C File Offset: 0x0006C99C
		private DependencyObject GetGroupParent(DependencyObject e, bool includeCurrent)
		{
			DependencyObject result = e;
			if (!includeCurrent)
			{
				result = e;
				e = KeyboardNavigation.GetParent(e);
				if (e == null)
				{
					return result;
				}
			}
			while (e != null)
			{
				if (this.IsGroup(e))
				{
					return e;
				}
				result = e;
				e = KeyboardNavigation.GetParent(e);
			}
			return result;
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x0006E7D8 File Offset: 0x0006C9D8
		private bool IsTabStop(DependencyObject e)
		{
			FrameworkElement frameworkElement = e as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.Focusable && (bool)frameworkElement.GetValue(KeyboardNavigation.IsTabStopProperty) && frameworkElement.IsEnabled && frameworkElement.IsVisible;
			}
			FrameworkContentElement frameworkContentElement = e as FrameworkContentElement;
			return frameworkContentElement != null && frameworkContentElement.Focusable && (bool)frameworkContentElement.GetValue(KeyboardNavigation.IsTabStopProperty) && frameworkContentElement.IsEnabled;
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0006E846 File Offset: 0x0006CA46
		private bool IsGroup(DependencyObject e)
		{
			return this.GetKeyNavigationMode(e) > KeyboardNavigationMode.Continue;
		}

		// Token: 0x06001648 RID: 5704 RVA: 0x0006E854 File Offset: 0x0006CA54
		internal bool IsFocusableInternal(DependencyObject element)
		{
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				return uielement.Focusable && uielement.IsEnabled && uielement.IsVisible;
			}
			ContentElement contentElement = element as ContentElement;
			return contentElement != null && (contentElement != null && contentElement.Focusable) && contentElement.IsEnabled;
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0006E8A3 File Offset: 0x0006CAA3
		private bool IsElementEligible(DependencyObject element, bool treeViewNavigation)
		{
			if (treeViewNavigation)
			{
				return element is TreeViewItem && this.IsFocusableInternal(element);
			}
			return this.IsTabStop(element);
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x0006E8C1 File Offset: 0x0006CAC1
		private bool IsGroupElementEligible(DependencyObject element, bool treeViewNavigation)
		{
			if (treeViewNavigation)
			{
				return element is TreeViewItem && this.IsFocusableInternal(element);
			}
			return this.IsTabStopOrGroup(element);
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x0006E8DF File Offset: 0x0006CADF
		private KeyboardNavigationMode GetKeyNavigationMode(DependencyObject e)
		{
			return (KeyboardNavigationMode)e.GetValue(this._navigationProperty);
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x0006E8F2 File Offset: 0x0006CAF2
		private bool IsTabStopOrGroup(DependencyObject e)
		{
			return this.IsTabStop(e) || this.IsGroup(e);
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0006E906 File Offset: 0x0006CB06
		private static int GetTabIndexHelper(DependencyObject d)
		{
			return (int)d.GetValue(KeyboardNavigation.TabIndexProperty);
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x0006E918 File Offset: 0x0006CB18
		internal DependencyObject GetFirstTabInGroup(DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			int num = int.MinValue;
			DependencyObject dependencyObject2 = container;
			while ((dependencyObject2 = this.GetNextInTree(dependencyObject2, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject2))
				{
					int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(dependencyObject2);
					if (tabIndexHelper < num || dependencyObject == null)
					{
						num = tabIndexHelper;
						dependencyObject = dependencyObject2;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0006E95C File Offset: 0x0006CB5C
		private DependencyObject GetNextTabWithSameIndex(DependencyObject e, DependencyObject container)
		{
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			DependencyObject dependencyObject = e;
			while ((dependencyObject = this.GetNextInTree(dependencyObject, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject) && KeyboardNavigation.GetTabIndexHelper(dependencyObject) == tabIndexHelper)
				{
					return dependencyObject;
				}
			}
			return null;
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0006E998 File Offset: 0x0006CB98
		private DependencyObject GetNextTabWithNextIndex(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			int num = int.MinValue;
			int num2 = int.MinValue;
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			DependencyObject dependencyObject3 = container;
			while ((dependencyObject3 = this.GetNextInTree(dependencyObject3, container)) != null)
			{
				if (this.IsTabStopOrGroup(dependencyObject3))
				{
					int tabIndexHelper2 = KeyboardNavigation.GetTabIndexHelper(dependencyObject3);
					if (tabIndexHelper2 > tabIndexHelper && (tabIndexHelper2 < num2 || dependencyObject == null))
					{
						num2 = tabIndexHelper2;
						dependencyObject = dependencyObject3;
					}
					if (tabIndexHelper2 < num || dependencyObject2 == null)
					{
						num = tabIndexHelper2;
						dependencyObject2 = dependencyObject3;
					}
				}
			}
			if (tabbingType == KeyboardNavigationMode.Cycle && dependencyObject == null)
			{
				dependencyObject = dependencyObject2;
			}
			return dependencyObject;
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0006EA10 File Offset: 0x0006CC10
		private DependencyObject GetNextTabInGroup(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			if (tabbingType == KeyboardNavigationMode.None)
			{
				return null;
			}
			if (e == null || e == container)
			{
				return this.GetFirstTabInGroup(container);
			}
			if (tabbingType == KeyboardNavigationMode.Once)
			{
				return null;
			}
			DependencyObject nextTabWithSameIndex = this.GetNextTabWithSameIndex(e, container);
			if (nextTabWithSameIndex != null)
			{
				return nextTabWithSameIndex;
			}
			return this.GetNextTabWithNextIndex(e, container, tabbingType);
		}

		// Token: 0x06001652 RID: 5714 RVA: 0x0006EA50 File Offset: 0x0006CC50
		private DependencyObject GetNextTab(DependencyObject e, DependencyObject container, bool goDownOnly)
		{
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			if (e == null)
			{
				if (this.IsTabStop(container))
				{
					return container;
				}
				DependencyObject activeElement = this.GetActiveElement(container);
				if (activeElement != null)
				{
					return this.GetNextTab(null, activeElement, true);
				}
			}
			else if ((keyNavigationMode == KeyboardNavigationMode.Once || keyNavigationMode == KeyboardNavigationMode.None) && container != e)
			{
				if (goDownOnly)
				{
					return null;
				}
				DependencyObject groupParent = this.GetGroupParent(container);
				return this.GetNextTab(container, groupParent, goDownOnly);
			}
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = e;
			KeyboardNavigationMode keyboardNavigationMode = keyNavigationMode;
			while ((dependencyObject2 = this.GetNextTabInGroup(dependencyObject2, container, keyboardNavigationMode)) != null && dependencyObject != dependencyObject2)
			{
				if (dependencyObject == null)
				{
					dependencyObject = dependencyObject2;
				}
				DependencyObject nextTab = this.GetNextTab(null, dependencyObject2, true);
				if (nextTab != null)
				{
					return nextTab;
				}
				if (keyboardNavigationMode == KeyboardNavigationMode.Once)
				{
					keyboardNavigationMode = KeyboardNavigationMode.Contained;
				}
			}
			if (!goDownOnly && keyboardNavigationMode != KeyboardNavigationMode.Contained && KeyboardNavigation.GetParent(container) != null)
			{
				return this.GetNextTab(container, this.GetGroupParent(container), false);
			}
			return null;
		}

		// Token: 0x06001653 RID: 5715 RVA: 0x0006EB08 File Offset: 0x0006CD08
		internal DependencyObject GetLastTabInGroup(DependencyObject container)
		{
			DependencyObject dependencyObject = null;
			int num = int.MaxValue;
			DependencyObject dependencyObject2 = this.GetLastInTree(container);
			while (dependencyObject2 != null && dependencyObject2 != container)
			{
				if (this.IsTabStopOrGroup(dependencyObject2))
				{
					int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(dependencyObject2);
					if (tabIndexHelper > num || dependencyObject == null)
					{
						num = tabIndexHelper;
						dependencyObject = dependencyObject2;
					}
				}
				dependencyObject2 = this.GetPreviousInTree(dependencyObject2, container);
			}
			return dependencyObject;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0006EB54 File Offset: 0x0006CD54
		private DependencyObject GetPrevTabWithSameIndex(DependencyObject e, DependencyObject container)
		{
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			for (DependencyObject previousInTree = this.GetPreviousInTree(e, container); previousInTree != null; previousInTree = this.GetPreviousInTree(previousInTree, container))
			{
				if (this.IsTabStopOrGroup(previousInTree) && KeyboardNavigation.GetTabIndexHelper(previousInTree) == tabIndexHelper && previousInTree != container)
				{
					return previousInTree;
				}
			}
			return null;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0006EB98 File Offset: 0x0006CD98
		private DependencyObject GetPrevTabWithPrevIndex(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			int tabIndexHelper = KeyboardNavigation.GetTabIndexHelper(e);
			int num = int.MaxValue;
			int num2 = int.MaxValue;
			for (DependencyObject dependencyObject3 = this.GetLastInTree(container); dependencyObject3 != null; dependencyObject3 = this.GetPreviousInTree(dependencyObject3, container))
			{
				if (this.IsTabStopOrGroup(dependencyObject3) && dependencyObject3 != container)
				{
					int tabIndexHelper2 = KeyboardNavigation.GetTabIndexHelper(dependencyObject3);
					if (tabIndexHelper2 < tabIndexHelper && (tabIndexHelper2 > num2 || dependencyObject2 == null))
					{
						num2 = tabIndexHelper2;
						dependencyObject2 = dependencyObject3;
					}
					if (tabIndexHelper2 > num || dependencyObject == null)
					{
						num = tabIndexHelper2;
						dependencyObject = dependencyObject3;
					}
				}
			}
			if (tabbingType == KeyboardNavigationMode.Cycle && dependencyObject2 == null)
			{
				dependencyObject2 = dependencyObject;
			}
			return dependencyObject2;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0006EC1C File Offset: 0x0006CE1C
		private DependencyObject GetPrevTabInGroup(DependencyObject e, DependencyObject container, KeyboardNavigationMode tabbingType)
		{
			if (tabbingType == KeyboardNavigationMode.None)
			{
				return null;
			}
			if (e == null)
			{
				return this.GetLastTabInGroup(container);
			}
			if (tabbingType == KeyboardNavigationMode.Once)
			{
				return null;
			}
			if (e == container)
			{
				return null;
			}
			DependencyObject prevTabWithSameIndex = this.GetPrevTabWithSameIndex(e, container);
			if (prevTabWithSameIndex != null)
			{
				return prevTabWithSameIndex;
			}
			return this.GetPrevTabWithPrevIndex(e, container, tabbingType);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0006EC60 File Offset: 0x0006CE60
		private DependencyObject GetPrevTab(DependencyObject e, DependencyObject container, bool goDownOnly)
		{
			if (container == null)
			{
				container = this.GetGroupParent(e);
			}
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			if (e == null)
			{
				DependencyObject activeElement = this.GetActiveElement(container);
				if (activeElement != null)
				{
					return this.GetPrevTab(null, activeElement, true);
				}
				if (keyNavigationMode == KeyboardNavigationMode.Once)
				{
					DependencyObject nextTabInGroup = this.GetNextTabInGroup(null, container, keyNavigationMode);
					if (nextTabInGroup != null)
					{
						return this.GetPrevTab(null, nextTabInGroup, true);
					}
					if (this.IsTabStop(container))
					{
						return container;
					}
					if (goDownOnly)
					{
						return null;
					}
					return this.GetPrevTab(container, null, false);
				}
			}
			else if (keyNavigationMode == KeyboardNavigationMode.Once || keyNavigationMode == KeyboardNavigationMode.None)
			{
				if (goDownOnly || container == e)
				{
					return null;
				}
				if (this.IsTabStop(container))
				{
					return container;
				}
				return this.GetPrevTab(container, null, false);
			}
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = e;
			while ((dependencyObject2 = this.GetPrevTabInGroup(dependencyObject2, container, keyNavigationMode)) != null && (dependencyObject2 != container || keyNavigationMode != KeyboardNavigationMode.Local))
			{
				if (this.IsTabStop(dependencyObject2) && !this.IsGroup(dependencyObject2))
				{
					return dependencyObject2;
				}
				if (dependencyObject == dependencyObject2)
				{
					break;
				}
				if (dependencyObject == null)
				{
					dependencyObject = dependencyObject2;
				}
				DependencyObject prevTab = this.GetPrevTab(null, dependencyObject2, true);
				if (prevTab != null)
				{
					return prevTab;
				}
			}
			if (keyNavigationMode == KeyboardNavigationMode.Contained)
			{
				return null;
			}
			if (e != container && this.IsTabStop(container))
			{
				return container;
			}
			if (!goDownOnly && KeyboardNavigation.GetParent(container) != null)
			{
				return this.GetPrevTab(container, null, false);
			}
			return null;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0006ED6C File Offset: 0x0006CF6C
		internal static Rect GetRectangle(DependencyObject element)
		{
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				if (!uielement.IsArrangeValid)
				{
					uielement.UpdateLayout();
				}
				Visual visualRoot = KeyboardNavigation.GetVisualRoot(uielement);
				if (visualRoot != null)
				{
					GeneralTransform generalTransform = uielement.TransformToAncestor(visualRoot);
					Thickness thickness = (Thickness)uielement.GetValue(KeyboardNavigation.DirectionalNavigationMarginProperty);
					double x = -thickness.Left;
					double y = -thickness.Top;
					double num = uielement.RenderSize.Width + thickness.Left + thickness.Right;
					double num2 = uielement.RenderSize.Height + thickness.Top + thickness.Bottom;
					if (num < 0.0)
					{
						x = uielement.RenderSize.Width * 0.5;
						num = 0.0;
					}
					if (num2 < 0.0)
					{
						y = uielement.RenderSize.Height * 0.5;
						num2 = 0.0;
					}
					return generalTransform.TransformBounds(new Rect(x, y, num, num2));
				}
			}
			else
			{
				ContentElement contentElement = element as ContentElement;
				if (contentElement != null)
				{
					IContentHost contentHost = null;
					UIElement parentUIElementFromContentElement = KeyboardNavigation.GetParentUIElementFromContentElement(contentElement, ref contentHost);
					Visual visual = contentHost as Visual;
					if (contentHost != null && visual != null && parentUIElementFromContentElement != null)
					{
						Visual visualRoot2 = KeyboardNavigation.GetVisualRoot(visual);
						if (visualRoot2 != null)
						{
							if (!parentUIElementFromContentElement.IsMeasureValid)
							{
								parentUIElementFromContentElement.UpdateLayout();
							}
							ReadOnlyCollection<Rect> rectangles = contentHost.GetRectangles(contentElement);
							IEnumerator<Rect> enumerator = rectangles.GetEnumerator();
							if (enumerator.MoveNext())
							{
								GeneralTransform generalTransform2 = visual.TransformToAncestor(visualRoot2);
								Rect rect = enumerator.Current;
								return generalTransform2.TransformBounds(rect);
							}
						}
					}
				}
				else
				{
					UIElement3D uielement3D = element as UIElement3D;
					if (uielement3D != null)
					{
						Visual visualRoot3 = KeyboardNavigation.GetVisualRoot(uielement3D);
						Visual containingVisual2D = VisualTreeHelper.GetContainingVisual2D(uielement3D);
						if (visualRoot3 != null && containingVisual2D != null)
						{
							Rect visual2DContentBounds = uielement3D.Visual2DContentBounds;
							GeneralTransform generalTransform3 = containingVisual2D.TransformToAncestor(visualRoot3);
							return generalTransform3.TransformBounds(visual2DContentBounds);
						}
					}
				}
			}
			return Rect.Empty;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0006EF6C File Offset: 0x0006D16C
		private Rect GetRepresentativeRectangle(DependencyObject element)
		{
			Rect rectangle = KeyboardNavigation.GetRectangle(element);
			TreeViewItem treeViewItem = element as TreeViewItem;
			if (treeViewItem != null)
			{
				Panel itemsHost = treeViewItem.ItemsHost;
				if (itemsHost != null && itemsHost.IsVisible)
				{
					Rect rectangle2 = KeyboardNavigation.GetRectangle(itemsHost);
					if (rectangle2 != Rect.Empty)
					{
						bool? flag = null;
						FrameworkElement frameworkElement = treeViewItem.TryGetHeaderElement();
						if (frameworkElement != null && frameworkElement != treeViewItem && frameworkElement.IsVisible)
						{
							Rect rectangle3 = KeyboardNavigation.GetRectangle(frameworkElement);
							if (!rectangle3.IsEmpty)
							{
								if (DoubleUtil.LessThan(rectangle3.Top, rectangle2.Top))
								{
									flag = new bool?(true);
								}
								else if (DoubleUtil.GreaterThan(rectangle3.Bottom, rectangle2.Bottom))
								{
									flag = new bool?(false);
								}
							}
						}
						double num = rectangle2.Top - rectangle.Top;
						double num2 = rectangle.Bottom - rectangle2.Bottom;
						if (flag == null)
						{
							flag = new bool?(DoubleUtil.GreaterThanOrClose(num, num2));
						}
						if (flag == true)
						{
							rectangle.Height = Math.Min(Math.Max(num, 0.0), rectangle.Height);
						}
						else
						{
							double num3 = Math.Min(Math.Max(num2, 0.0), rectangle.Height);
							rectangle.Y = rectangle.Bottom - num3;
							rectangle.Height = num3;
						}
					}
				}
			}
			return rectangle;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0006F0E4 File Offset: 0x0006D2E4
		private double GetDistance(Point p1, Point p2)
		{
			double num = p1.X - p2.X;
			double num2 = p1.Y - p2.Y;
			return Math.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0006F120 File Offset: 0x0006D320
		private double GetPerpDistance(Rect sourceRect, Rect targetRect, FocusNavigationDirection direction)
		{
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				return sourceRect.Right - targetRect.Right;
			case FocusNavigationDirection.Right:
				return targetRect.Left - sourceRect.Left;
			case FocusNavigationDirection.Up:
				return sourceRect.Bottom - targetRect.Bottom;
			case FocusNavigationDirection.Down:
				return targetRect.Top - sourceRect.Top;
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0006F19C File Offset: 0x0006D39C
		private double GetDistance(Rect sourceRect, Rect targetRect, FocusNavigationDirection direction)
		{
			Point p;
			Point p2;
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				p = sourceRect.TopRight;
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					p.Y = this._horizontalBaseline;
				}
				p2 = targetRect.TopRight;
				break;
			case FocusNavigationDirection.Right:
				p = sourceRect.TopLeft;
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					p.Y = this._horizontalBaseline;
				}
				p2 = targetRect.TopLeft;
				break;
			case FocusNavigationDirection.Up:
				p = sourceRect.BottomLeft;
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					p.X = this._verticalBaseline;
				}
				p2 = targetRect.BottomLeft;
				break;
			case FocusNavigationDirection.Down:
				p = sourceRect.TopLeft;
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					p.X = this._verticalBaseline;
				}
				p2 = targetRect.TopLeft;
				break;
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
			return this.GetDistance(p, p2);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0006F2A8 File Offset: 0x0006D4A8
		private bool IsInDirection(Rect fromRect, Rect toRect, FocusNavigationDirection direction)
		{
			switch (direction)
			{
			case FocusNavigationDirection.Left:
				return DoubleUtil.GreaterThanOrClose(fromRect.Left, toRect.Right);
			case FocusNavigationDirection.Right:
				return DoubleUtil.LessThanOrClose(fromRect.Right, toRect.Left);
			case FocusNavigationDirection.Up:
				return DoubleUtil.GreaterThanOrClose(fromRect.Top, toRect.Bottom);
			case FocusNavigationDirection.Down:
				return DoubleUtil.LessThanOrClose(fromRect.Bottom, toRect.Top);
			default:
				throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
			}
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0006F334 File Offset: 0x0006D534
		private bool IsFocusScope(DependencyObject e)
		{
			return FocusManager.GetIsFocusScope(e) || KeyboardNavigation.GetParent(e) == null;
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0006F34C File Offset: 0x0006D54C
		private bool IsAncestorOf(DependencyObject sourceElement, DependencyObject targetElement)
		{
			Visual visual = sourceElement as Visual;
			Visual visual2 = targetElement as Visual;
			return visual != null && visual2 != null && visual.IsAncestorOf(visual2);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0006F376 File Offset: 0x0006D576
		internal bool IsAncestorOfEx(DependencyObject sourceElement, DependencyObject targetElement)
		{
			while (targetElement != null && targetElement != sourceElement)
			{
				targetElement = KeyboardNavigation.GetParent(targetElement);
			}
			return targetElement == sourceElement;
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0006F390 File Offset: 0x0006D590
		private bool IsInRange(DependencyObject sourceElement, DependencyObject targetElement, Rect sourceRect, Rect targetRect, FocusNavigationDirection direction, double startRange, double endRange)
		{
			if (direction - FocusNavigationDirection.Left > 1)
			{
				if (direction - FocusNavigationDirection.Up > 1)
				{
					throw new InvalidEnumArgumentException("direction", (int)direction, typeof(FocusNavigationDirection));
				}
				if (this._verticalBaseline != -1.7976931348623157E+308)
				{
					startRange = Math.Min(startRange, this._verticalBaseline);
					endRange = Math.Max(endRange, this._verticalBaseline);
				}
				if (DoubleUtil.GreaterThan(targetRect.Right, startRange) && DoubleUtil.LessThan(targetRect.Left, endRange))
				{
					if (sourceElement == null)
					{
						return true;
					}
					if (direction == FocusNavigationDirection.Down)
					{
						return DoubleUtil.GreaterThan(targetRect.Top, sourceRect.Top) || (DoubleUtil.AreClose(targetRect.Top, sourceRect.Top) && this.IsAncestorOfEx(sourceElement, targetElement));
					}
					return DoubleUtil.LessThan(targetRect.Bottom, sourceRect.Bottom) || (DoubleUtil.AreClose(targetRect.Bottom, sourceRect.Bottom) && this.IsAncestorOfEx(sourceElement, targetElement));
				}
			}
			else
			{
				if (this._horizontalBaseline != -1.7976931348623157E+308)
				{
					startRange = Math.Min(startRange, this._horizontalBaseline);
					endRange = Math.Max(endRange, this._horizontalBaseline);
				}
				if (DoubleUtil.GreaterThan(targetRect.Bottom, startRange) && DoubleUtil.LessThan(targetRect.Top, endRange))
				{
					if (sourceElement == null)
					{
						return true;
					}
					if (direction == FocusNavigationDirection.Right)
					{
						return DoubleUtil.GreaterThan(targetRect.Left, sourceRect.Left) || (DoubleUtil.AreClose(targetRect.Left, sourceRect.Left) && this.IsAncestorOfEx(sourceElement, targetElement));
					}
					return DoubleUtil.LessThan(targetRect.Right, sourceRect.Right) || (DoubleUtil.AreClose(targetRect.Right, sourceRect.Right) && this.IsAncestorOfEx(sourceElement, targetElement));
				}
			}
			return false;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0006F565 File Offset: 0x0006D765
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction)
		{
			return this.GetNextInDirection(sourceElement, direction, false);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0006F570 File Offset: 0x0006D770
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			return this.GetNextInDirection(sourceElement, direction, treeViewNavigation, true);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0006F57C File Offset: 0x0006D77C
		private DependencyObject GetNextInDirection(DependencyObject sourceElement, FocusNavigationDirection direction, bool treeViewNavigation, bool considerDescendants)
		{
			this._containerHashtable.Clear();
			DependencyObject dependencyObject = this.MoveNext(sourceElement, null, direction, double.MinValue, double.MinValue, treeViewNavigation, considerDescendants);
			if (dependencyObject != null)
			{
				UIElement uielement = sourceElement as UIElement;
				if (uielement != null)
				{
					uielement.RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
				}
				else
				{
					ContentElement contentElement = sourceElement as ContentElement;
					if (contentElement != null)
					{
						contentElement.RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
					}
				}
				UIElement uielement2 = dependencyObject as UIElement;
				if (uielement2 == null)
				{
					uielement2 = KeyboardNavigation.GetParentUIElementFromContentElement(dependencyObject as ContentElement);
				}
				else
				{
					ContentElement contentElement2 = dependencyObject as ContentElement;
					if (contentElement2 != null)
					{
						contentElement2.AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus), true);
					}
				}
				if (uielement2 != null)
				{
					uielement2.LayoutUpdated += this.OnLayoutUpdated;
					if (dependencyObject == uielement2)
					{
						uielement2.AddHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus), true);
					}
				}
			}
			this._containerHashtable.Clear();
			return dependencyObject;
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0006F678 File Offset: 0x0006D878
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			UIElement uielement = sender as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0006F6C0 File Offset: 0x0006D8C0
		private void _LostFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			this._verticalBaseline = double.MinValue;
			this._horizontalBaseline = double.MinValue;
			if (sender is UIElement)
			{
				((UIElement)sender).RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
				return;
			}
			if (sender is ContentElement)
			{
				((ContentElement)sender).RemoveHandler(Keyboard.PreviewLostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(this._LostFocus));
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0006F734 File Offset: 0x0006D934
		private bool IsEndlessLoop(DependencyObject element, DependencyObject container)
		{
			object key = (element != null) ? element : KeyboardNavigation._fakeNull;
			Hashtable hashtable = this._containerHashtable[container] as Hashtable;
			if (hashtable != null)
			{
				if (hashtable[key] != null)
				{
					return true;
				}
			}
			else
			{
				hashtable = new Hashtable(10);
				this._containerHashtable[container] = hashtable;
			}
			hashtable[key] = BooleanBoxes.TrueBox;
			return false;
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0006F790 File Offset: 0x0006D990
		private void ResetBaseLines(double value, bool horizontalDirection)
		{
			if (horizontalDirection)
			{
				this._verticalBaseline = double.MinValue;
				if (this._horizontalBaseline == -1.7976931348623157E+308)
				{
					this._horizontalBaseline = value;
					return;
				}
			}
			else
			{
				this._horizontalBaseline = double.MinValue;
				if (this._verticalBaseline == -1.7976931348623157E+308)
				{
					this._verticalBaseline = value;
				}
			}
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0006F7F0 File Offset: 0x0006D9F0
		private DependencyObject FindNextInDirection(DependencyObject sourceElement, Rect sourceRect, DependencyObject container, FocusNavigationDirection direction, double startRange, double endRange, bool treeViewNavigation, bool considerDescendants)
		{
			DependencyObject dependencyObject = null;
			Rect targetRect = Rect.Empty;
			double value = 0.0;
			bool flag = sourceElement == null;
			DependencyObject dependencyObject2 = container;
			while ((dependencyObject2 = this.GetNextInTree(dependencyObject2, container)) != null)
			{
				if (dependencyObject2 != sourceElement && this.IsGroupElementEligible(dependencyObject2, treeViewNavigation))
				{
					Rect representativeRectangle = this.GetRepresentativeRectangle(dependencyObject2);
					if (representativeRectangle != Rect.Empty)
					{
						bool flag2 = this.IsInDirection(sourceRect, representativeRectangle, direction);
						bool flag3 = this.IsInRange(sourceElement, dependencyObject2, sourceRect, representativeRectangle, direction, startRange, endRange);
						if (flag || flag2 || flag3)
						{
							double num = flag3 ? this.GetPerpDistance(sourceRect, representativeRectangle, direction) : this.GetDistance(sourceRect, representativeRectangle, direction);
							if (!double.IsNaN(num))
							{
								if (dependencyObject == null && (considerDescendants || !this.IsAncestorOfEx(sourceElement, dependencyObject2)))
								{
									dependencyObject = dependencyObject2;
									targetRect = representativeRectangle;
									value = num;
								}
								else if ((DoubleUtil.LessThan(num, value) || (DoubleUtil.AreClose(num, value) && this.GetDistance(sourceRect, targetRect, direction) > this.GetDistance(sourceRect, representativeRectangle, direction))) && (considerDescendants || !this.IsAncestorOfEx(sourceElement, dependencyObject2)))
								{
									dependencyObject = dependencyObject2;
									targetRect = representativeRectangle;
									value = num;
								}
							}
						}
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0006F914 File Offset: 0x0006DB14
		private DependencyObject MoveNext(DependencyObject sourceElement, DependencyObject container, FocusNavigationDirection direction, double startRange, double endRange, bool treeViewNavigation, bool considerDescendants)
		{
			if (container == null)
			{
				container = this.GetGroupParent(sourceElement);
			}
			if (container == sourceElement)
			{
				return null;
			}
			if (this.IsEndlessLoop(sourceElement, container))
			{
				return null;
			}
			KeyboardNavigationMode keyNavigationMode = this.GetKeyNavigationMode(container);
			bool flag = sourceElement == null;
			if (keyNavigationMode == KeyboardNavigationMode.None && flag)
			{
				return null;
			}
			Rect sourceRect = flag ? KeyboardNavigation.GetRectangle(container) : this.GetRepresentativeRectangle(sourceElement);
			bool flag2 = direction == FocusNavigationDirection.Right || direction == FocusNavigationDirection.Left;
			this.ResetBaseLines(flag2 ? sourceRect.Top : sourceRect.Left, flag2);
			if (startRange == -1.7976931348623157E+308 || endRange == -1.7976931348623157E+308)
			{
				startRange = (flag2 ? sourceRect.Top : sourceRect.Left);
				endRange = (flag2 ? sourceRect.Bottom : sourceRect.Right);
			}
			if (keyNavigationMode == KeyboardNavigationMode.Once && !flag)
			{
				return this.MoveNext(container, null, direction, startRange, endRange, treeViewNavigation, true);
			}
			DependencyObject dependencyObject = this.FindNextInDirection(sourceElement, sourceRect, container, direction, startRange, endRange, treeViewNavigation, considerDescendants);
			if (dependencyObject == null)
			{
				if (keyNavigationMode == KeyboardNavigationMode.Cycle)
				{
					return this.MoveNext(null, container, direction, startRange, endRange, treeViewNavigation, true);
				}
				if (keyNavigationMode != KeyboardNavigationMode.Contained)
				{
					return this.MoveNext(container, null, direction, startRange, endRange, treeViewNavigation, true);
				}
				return null;
			}
			else
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					return dependencyObject;
				}
				DependencyObject activeElementChain = this.GetActiveElementChain(dependencyObject, treeViewNavigation);
				if (activeElementChain != null)
				{
					return activeElementChain;
				}
				DependencyObject dependencyObject2 = this.MoveNext(null, dependencyObject, direction, startRange, endRange, treeViewNavigation, true);
				if (dependencyObject2 != null)
				{
					return dependencyObject2;
				}
				return this.MoveNext(dependencyObject, null, direction, startRange, endRange, treeViewNavigation, true);
			}
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0006FA80 File Offset: 0x0006DC80
		private DependencyObject GetActiveElementChain(DependencyObject element, bool treeViewNavigation)
		{
			DependencyObject result = null;
			DependencyObject dependencyObject = element;
			while ((dependencyObject = this.GetActiveElement(dependencyObject)) != null)
			{
				if (this.IsElementEligible(dependencyObject, treeViewNavigation))
				{
					result = dependencyObject;
				}
			}
			return result;
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0006FAAC File Offset: 0x0006DCAC
		private DependencyObject FindElementAtViewportEdge(DependencyObject sourceElement, FrameworkElement viewportBoundsElement, DependencyObject container, FocusNavigationDirection direction, bool treeViewNavigation)
		{
			Rect rect = new Rect(0.0, 0.0, 0.0, 0.0);
			if (sourceElement != null && ItemsControl.GetElementViewportPosition(viewportBoundsElement, ItemsControl.TryGetTreeViewItemHeader(sourceElement) as UIElement, direction, false, out rect) == ElementViewportPosition.None)
			{
				rect = new Rect(0.0, 0.0, 0.0, 0.0);
			}
			DependencyObject dependencyObject = null;
			double value = double.NegativeInfinity;
			double value2 = double.NegativeInfinity;
			DependencyObject dependencyObject2 = null;
			double value3 = double.NegativeInfinity;
			double value4 = double.NegativeInfinity;
			DependencyObject dependencyObject3 = container;
			while ((dependencyObject3 = this.GetNextInTree(dependencyObject3, container)) != null)
			{
				if (this.IsGroupElementEligible(dependencyObject3, treeViewNavigation))
				{
					DependencyObject dependencyObject4 = dependencyObject3;
					if (treeViewNavigation)
					{
						dependencyObject4 = ItemsControl.TryGetTreeViewItemHeader(dependencyObject3);
					}
					Rect rect2;
					ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportBoundsElement, dependencyObject4 as UIElement, direction, false, out rect2);
					if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
					{
						double num = double.NegativeInfinity;
						switch (direction)
						{
						case FocusNavigationDirection.Left:
							num = -rect2.Left;
							break;
						case FocusNavigationDirection.Right:
							num = rect2.Right;
							break;
						case FocusNavigationDirection.Up:
							num = -rect2.Top;
							break;
						case FocusNavigationDirection.Down:
							num = rect2.Bottom;
							break;
						}
						double num2 = double.NegativeInfinity;
						if (direction - FocusNavigationDirection.Left > 1)
						{
							if (direction - FocusNavigationDirection.Up <= 1)
							{
								num2 = this.ComputeRangeScore(rect.Left, rect.Right, rect2.Left, rect2.Right);
							}
						}
						else
						{
							num2 = this.ComputeRangeScore(rect.Top, rect.Bottom, rect2.Top, rect2.Bottom);
						}
						if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport)
						{
							if (dependencyObject == null || DoubleUtil.GreaterThan(num, value) || (DoubleUtil.AreClose(num, value) && DoubleUtil.GreaterThan(num2, value2)))
							{
								dependencyObject = dependencyObject3;
								value = num;
								value2 = num2;
							}
						}
						else if (dependencyObject2 == null || DoubleUtil.GreaterThan(num, value3) || (DoubleUtil.AreClose(num, value3) && DoubleUtil.GreaterThan(num2, value4)))
						{
							dependencyObject2 = dependencyObject3;
							value3 = num;
							value4 = num2;
						}
					}
				}
			}
			if (dependencyObject == null)
			{
				return dependencyObject2;
			}
			return dependencyObject;
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x0006FCD8 File Offset: 0x0006DED8
		private double ComputeRangeScore(double rangeStart1, double rangeEnd1, double rangeStart2, double rangeEnd2)
		{
			if (DoubleUtil.GreaterThan(rangeStart1, rangeStart2))
			{
				double num = rangeStart1;
				rangeStart1 = rangeStart2;
				rangeStart2 = num;
				num = rangeEnd1;
				rangeEnd1 = rangeEnd2;
				rangeEnd2 = num;
			}
			if (DoubleUtil.LessThan(rangeEnd1, rangeEnd2))
			{
				return rangeEnd1 - rangeStart2;
			}
			return rangeEnd2 - rangeStart2;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x0006FD14 File Offset: 0x0006DF14
		[SecurityCritical]
		private void ProcessForMenuMode(InputEventArgs inputEventArgs)
		{
			if (inputEventArgs.RoutedEvent == Keyboard.LostKeyboardFocusEvent)
			{
				KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs = inputEventArgs as KeyboardFocusChangedEventArgs;
				if ((keyboardFocusChangedEventArgs != null && keyboardFocusChangedEventArgs.NewFocus == null) || inputEventArgs.Handled)
				{
					this._lastKeyPressed = Key.None;
					return;
				}
			}
			else if (inputEventArgs.RoutedEvent == Keyboard.KeyDownEvent)
			{
				if (inputEventArgs.Handled)
				{
					this._lastKeyPressed = Key.None;
					return;
				}
				KeyEventArgs keyEventArgs = inputEventArgs as KeyEventArgs;
				if (!keyEventArgs.IsRepeat)
				{
					if (this._lastKeyPressed == Key.None)
					{
						if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Windows)) == ModifierKeys.None)
						{
							this._lastKeyPressed = this.GetRealKey(keyEventArgs);
						}
					}
					else
					{
						this._lastKeyPressed = Key.None;
					}
					this._win32MenuModeWorkAround = false;
					return;
				}
			}
			else
			{
				if (inputEventArgs.RoutedEvent == Keyboard.KeyUpEvent)
				{
					if (!inputEventArgs.Handled)
					{
						KeyEventArgs keyEventArgs2 = inputEventArgs as KeyEventArgs;
						Key realKey = this.GetRealKey(keyEventArgs2);
						if (realKey == this._lastKeyPressed && this.IsMenuKey(realKey))
						{
							KeyboardNavigation.EnableKeyboardCues(keyEventArgs2.Source as DependencyObject, true);
							keyEventArgs2.Handled = this.OnEnterMenuMode(keyEventArgs2.Source);
						}
						if (this._win32MenuModeWorkAround)
						{
							if (this.IsMenuKey(realKey))
							{
								this._win32MenuModeWorkAround = false;
								keyEventArgs2.Handled = true;
							}
						}
						else if (keyEventArgs2.Handled)
						{
							this._win32MenuModeWorkAround = true;
						}
					}
					this._lastKeyPressed = Key.None;
					return;
				}
				if (inputEventArgs.RoutedEvent == Mouse.MouseDownEvent || inputEventArgs.RoutedEvent == Mouse.MouseUpEvent)
				{
					this._lastKeyPressed = Key.None;
					this._win32MenuModeWorkAround = false;
				}
			}
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0006FE6E File Offset: 0x0006E06E
		private bool IsMenuKey(Key key)
		{
			return key == Key.LeftAlt || key == Key.RightAlt || key == Key.F10;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0006FE81 File Offset: 0x0006E081
		private Key GetRealKey(KeyEventArgs e)
		{
			if (e.Key != Key.System)
			{
				return e.Key;
			}
			return e.SystemKey;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x0006FEA0 File Offset: 0x0006E0A0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool OnEnterMenuMode(object eventSource)
		{
			if (this._weakEnterMenuModeHandlers == null)
			{
				return false;
			}
			KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
			bool flag = false;
			bool result;
			try
			{
				Monitor.Enter(weakEnterMenuModeHandlers, ref flag);
				if (this._weakEnterMenuModeHandlers.Count == 0)
				{
					result = false;
				}
				else
				{
					PresentationSource source = null;
					if (eventSource != null)
					{
						Visual visual = eventSource as Visual;
						source = ((visual != null) ? PresentationSource.CriticalFromVisual(visual) : null);
					}
					else
					{
						IntPtr activeWindow = UnsafeNativeMethods.GetActiveWindow();
						if (activeWindow != IntPtr.Zero)
						{
							source = HwndSource.CriticalFromHwnd(activeWindow);
						}
					}
					if (source == null)
					{
						result = false;
					}
					else
					{
						EventArgs e = EventArgs.Empty;
						bool handled = false;
						this._weakEnterMenuModeHandlers.Process(delegate(object obj)
						{
							KeyboardNavigation.EnterMenuModeEventHandler enterMenuModeEventHandler = obj as KeyboardNavigation.EnterMenuModeEventHandler;
							if (enterMenuModeEventHandler != null && enterMenuModeEventHandler(source, e))
							{
								handled = true;
							}
							return handled;
						});
						result = handled;
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(weakEnterMenuModeHandlers);
				}
			}
			return result;
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001672 RID: 5746 RVA: 0x0006FF88 File Offset: 0x0006E188
		// (remove) Token: 0x06001673 RID: 5747 RVA: 0x0006FFE8 File Offset: 0x0006E1E8
		internal event KeyboardNavigation.EnterMenuModeEventHandler EnterMenuMode
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			add
			{
				SecurityHelper.DemandUIWindowPermission();
				if (this._weakEnterMenuModeHandlers == null)
				{
					this._weakEnterMenuModeHandlers = new KeyboardNavigation.WeakReferenceList();
				}
				KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
				lock (weakEnterMenuModeHandlers)
				{
					this._weakEnterMenuModeHandlers.Add(value);
				}
			}
			remove
			{
				if (this._weakEnterMenuModeHandlers != null)
				{
					KeyboardNavigation.WeakReferenceList weakEnterMenuModeHandlers = this._weakEnterMenuModeHandlers;
					lock (weakEnterMenuModeHandlers)
					{
						this._weakEnterMenuModeHandlers.Remove(value);
					}
				}
			}
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x00070038 File Offset: 0x0006E238
		[SecurityCritical]
		private void ProcessForUIState(InputEventArgs inputEventArgs)
		{
			RawUIStateInputReport rawUIStateInputReport = this.ExtractRawUIStateInputReport(inputEventArgs, InputManager.InputReportEvent);
			PresentationSource inputSource;
			if (rawUIStateInputReport != null && (inputSource = rawUIStateInputReport.InputSource) != null && (rawUIStateInputReport.Targets & RawUIStateTargets.HideAccelerators) != RawUIStateTargets.None)
			{
				Visual rootVisual = inputSource.RootVisual;
				bool enable = rawUIStateInputReport.Action == RawUIStateActions.Clear;
				KeyboardNavigation.EnableKeyboardCues(rootVisual, enable);
			}
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x00070084 File Offset: 0x0006E284
		[SecurityCritical]
		private RawUIStateInputReport ExtractRawUIStateInputReport(InputEventArgs e, RoutedEvent Event)
		{
			RawUIStateInputReport result = null;
			InputReportEventArgs inputReportEventArgs = e as InputReportEventArgs;
			if (inputReportEventArgs != null && inputReportEventArgs.Report.Type == InputType.Keyboard && inputReportEventArgs.RoutedEvent == Event)
			{
				result = (inputReportEventArgs.Report as RawUIStateInputReport);
			}
			return result;
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06001676 RID: 5750 RVA: 0x000700C0 File Offset: 0x0006E2C0
		// (remove) Token: 0x06001677 RID: 5751 RVA: 0x00070108 File Offset: 0x0006E308
		internal event EventHandler FocusEnterMainFocusScope
		{
			add
			{
				KeyboardNavigation.WeakReferenceList weakFocusEnterMainFocusScopeHandlers = this._weakFocusEnterMainFocusScopeHandlers;
				lock (weakFocusEnterMainFocusScopeHandlers)
				{
					this._weakFocusEnterMainFocusScopeHandlers.Add(value);
				}
			}
			remove
			{
				KeyboardNavigation.WeakReferenceList weakFocusEnterMainFocusScopeHandlers = this._weakFocusEnterMainFocusScopeHandlers;
				lock (weakFocusEnterMainFocusScopeHandlers)
				{
					this._weakFocusEnterMainFocusScopeHandlers.Remove(value);
				}
			}
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x00070150 File Offset: 0x0006E350
		private void NotifyFocusEnterMainFocusScope(object sender, EventArgs e)
		{
			this._weakFocusEnterMainFocusScopeHandlers.Process(delegate(object item)
			{
				EventHandler eventHandler = item as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(sender, e);
				}
				return false;
			});
		}

		// Token: 0x04001288 RID: 4744
		private static readonly DependencyProperty TabOnceActiveElementProperty = DependencyProperty.RegisterAttached("TabOnceActiveElement", typeof(WeakReference), typeof(KeyboardNavigation));

		// Token: 0x04001289 RID: 4745
		internal static readonly DependencyProperty ControlTabOnceActiveElementProperty = DependencyProperty.RegisterAttached("ControlTabOnceActiveElement", typeof(WeakReference), typeof(KeyboardNavigation));

		// Token: 0x0400128A RID: 4746
		internal static readonly DependencyProperty DirectionalNavigationMarginProperty = DependencyProperty.RegisterAttached("DirectionalNavigationMargin", typeof(Thickness), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(default(Thickness)));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.TabIndex" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Input.KeyboardNavigation.TabIndex" /> attached property.</returns>
		// Token: 0x0400128B RID: 4747
		public static readonly DependencyProperty TabIndexProperty = DependencyProperty.RegisterAttached("TabIndex", typeof(int), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(int.MaxValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.IsTabStop" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Input.KeyboardNavigation.IsTabStop" /> attached property.</returns>
		// Token: 0x0400128C RID: 4748
		public static readonly DependencyProperty IsTabStopProperty = DependencyProperty.RegisterAttached("IsTabStop", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.TabNavigation" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Input.KeyboardNavigation.TabNavigation" /> attached property.</returns>
		// Token: 0x0400128D RID: 4749
		[CustomCategory("Accessibility")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CommonDependencyProperty]
		public static readonly DependencyProperty TabNavigationProperty = DependencyProperty.RegisterAttached("TabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.ControlTabNavigation" /> attached property. </summary>
		/// <returns>The identifier for the  attached property.</returns>
		// Token: 0x0400128E RID: 4750
		[CustomCategory("Accessibility")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CommonDependencyProperty]
		public static readonly DependencyProperty ControlTabNavigationProperty = DependencyProperty.RegisterAttached("ControlTabNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.DirectionalNavigation" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Input.KeyboardNavigation.DirectionalNavigation" /> attached property.</returns>
		// Token: 0x0400128F RID: 4751
		[CustomCategory("Accessibility")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		[CommonDependencyProperty]
		public static readonly DependencyProperty DirectionalNavigationProperty = DependencyProperty.RegisterAttached("DirectionalNavigation", typeof(KeyboardNavigationMode), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(KeyboardNavigationMode.Continue), new ValidateValueCallback(KeyboardNavigation.IsValidKeyNavigationMode));

		// Token: 0x04001290 RID: 4752
		internal static readonly DependencyProperty ShowKeyboardCuesProperty = DependencyProperty.RegisterAttached("ShowKeyboardCues", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior, null, new CoerceValueCallback(KeyboardNavigation.CoerceShowKeyboardCues)));

		/// <summary>Identifies the <see cref="P:System.Windows.Input.KeyboardNavigation.AcceptsReturn" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Input.KeyboardNavigation.AcceptsReturn" /> attached property.</returns>
		// Token: 0x04001291 RID: 4753
		public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.RegisterAttached("AcceptsReturn", typeof(bool), typeof(KeyboardNavigation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04001292 RID: 4754
		private KeyboardNavigation.WeakReferenceList _weakFocusChangedHandlers = new KeyboardNavigation.WeakReferenceList();

		// Token: 0x04001293 RID: 4755
		private static bool _alwaysShowFocusVisual = SystemParameters.KeyboardCues;

		// Token: 0x04001294 RID: 4756
		private KeyboardNavigation.FocusVisualAdorner _focusVisualAdornerCache;

		// Token: 0x04001295 RID: 4757
		private Key _lastKeyPressed;

		// Token: 0x04001296 RID: 4758
		private KeyboardNavigation.WeakReferenceList _weakEnterMenuModeHandlers;

		// Token: 0x04001297 RID: 4759
		private bool _win32MenuModeWorkAround;

		// Token: 0x04001298 RID: 4760
		private KeyboardNavigation.WeakReferenceList _weakFocusEnterMainFocusScopeHandlers = new KeyboardNavigation.WeakReferenceList();

		// Token: 0x04001299 RID: 4761
		private const double BASELINE_DEFAULT = -1.7976931348623157E+308;

		// Token: 0x0400129A RID: 4762
		private double _verticalBaseline = double.MinValue;

		// Token: 0x0400129B RID: 4763
		private double _horizontalBaseline = double.MinValue;

		// Token: 0x0400129C RID: 4764
		private DependencyProperty _navigationProperty;

		// Token: 0x0400129D RID: 4765
		private Hashtable _containerHashtable = new Hashtable(10);

		// Token: 0x0400129E RID: 4766
		private static object _fakeNull = new object();

		// Token: 0x02000853 RID: 2131
		private sealed class FocusVisualAdorner : Adorner
		{
			// Token: 0x060082A4 RID: 33444 RVA: 0x0024356C File Offset: 0x0024176C
			public FocusVisualAdorner(UIElement adornedElement, Style focusVisualStyle) : base(adornedElement)
			{
				this._adorderChild = new Control
				{
					Style = focusVisualStyle
				};
				base.IsClipEnabled = true;
				base.IsHitTestVisible = false;
				base.IsEnabled = false;
				base.AddVisualChild(this._adorderChild);
			}

			// Token: 0x060082A5 RID: 33445 RVA: 0x002435C0 File Offset: 0x002417C0
			public FocusVisualAdorner(ContentElement adornedElement, UIElement adornedElementParent, IContentHost contentHostParent, Style focusVisualStyle) : base(adornedElementParent)
			{
				this._contentHostParent = contentHostParent;
				this._adornedContentElement = adornedElement;
				this._focusVisualStyle = focusVisualStyle;
				Canvas canvas = new Canvas();
				this._canvasChildren = canvas.Children;
				this._adorderChild = canvas;
				base.AddVisualChild(this._adorderChild);
				base.IsClipEnabled = true;
				base.IsHitTestVisible = false;
				base.IsEnabled = false;
			}

			// Token: 0x060082A6 RID: 33446 RVA: 0x00243630 File Offset: 0x00241830
			protected override Size MeasureOverride(Size constraint)
			{
				Size size = default(Size);
				if (this._adornedContentElement == null)
				{
					size = base.AdornedElement.RenderSize;
					constraint = size;
				}
				((UIElement)this.GetVisualChild(0)).Measure(constraint);
				return size;
			}

			// Token: 0x060082A7 RID: 33447 RVA: 0x00243670 File Offset: 0x00241870
			protected override Size ArrangeOverride(Size size)
			{
				Size size2 = base.ArrangeOverride(size);
				if (this._adornedContentElement != null)
				{
					if (this._contentRects == null)
					{
						this._canvasChildren.Clear();
					}
					else
					{
						IContentHost contentHost = this.ContentHost;
						if (!(contentHost is Visual) || !base.AdornedElement.IsAncestorOf((Visual)contentHost))
						{
							this._canvasChildren.Clear();
							return default(Size);
						}
						Rect empty = Rect.Empty;
						IEnumerator<Rect> enumerator = this._contentRects.GetEnumerator();
						if (this._canvasChildren.Count == this._contentRects.Count)
						{
							for (int i = 0; i < this._canvasChildren.Count; i++)
							{
								enumerator.MoveNext();
								Rect rect = enumerator.Current;
								rect = this._hostToAdornedElement.TransformBounds(rect);
								Control control = (Control)this._canvasChildren[i];
								control.Width = rect.Width;
								control.Height = rect.Height;
								Canvas.SetLeft(control, rect.X);
								Canvas.SetTop(control, rect.Y);
							}
							this._adorderChild.InvalidateArrange();
						}
						else
						{
							this._canvasChildren.Clear();
							while (enumerator.MoveNext())
							{
								Rect rect2 = enumerator.Current;
								rect2 = this._hostToAdornedElement.TransformBounds(rect2);
								Control control2 = new Control();
								control2.Style = this._focusVisualStyle;
								control2.Width = rect2.Width;
								control2.Height = rect2.Height;
								Canvas.SetLeft(control2, rect2.X);
								Canvas.SetTop(control2, rect2.Y);
								this._canvasChildren.Add(control2);
							}
						}
					}
				}
				((UIElement)this.GetVisualChild(0)).Arrange(new Rect(default(Point), size2));
				return size2;
			}

			// Token: 0x17001D94 RID: 7572
			// (get) Token: 0x060082A8 RID: 33448 RVA: 0x00016748 File Offset: 0x00014948
			protected override int VisualChildrenCount
			{
				get
				{
					return 1;
				}
			}

			// Token: 0x060082A9 RID: 33449 RVA: 0x00243849 File Offset: 0x00241A49
			protected override Visual GetVisualChild(int index)
			{
				if (index == 0)
				{
					return this._adorderChild;
				}
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}

			// Token: 0x17001D95 RID: 7573
			// (get) Token: 0x060082AA RID: 33450 RVA: 0x0024386F File Offset: 0x00241A6F
			private IContentHost ContentHost
			{
				get
				{
					if (this._adornedContentElement != null && (this._contentHostParent == null || VisualTreeHelper.GetParent(this._contentHostParent as Visual) == null))
					{
						this._contentHostParent = ContentHostHelper.FindContentHost(this._adornedContentElement);
					}
					return this._contentHostParent;
				}
			}

			// Token: 0x060082AB RID: 33451 RVA: 0x002438AC File Offset: 0x00241AAC
			internal override bool NeedsUpdate(Size oldSize)
			{
				if (this._adornedContentElement == null)
				{
					return !DoubleUtil.AreClose(base.AdornedElement.RenderSize, oldSize);
				}
				ReadOnlyCollection<Rect> contentRects = this._contentRects;
				this._contentRects = null;
				IContentHost contentHost = this.ContentHost;
				if (contentHost != null)
				{
					this._contentRects = contentHost.GetRectangles(this._adornedContentElement);
				}
				GeneralTransform hostToAdornedElement = this._hostToAdornedElement;
				if (contentHost is Visual && base.AdornedElement.IsAncestorOf((Visual)contentHost))
				{
					this._hostToAdornedElement = ((Visual)contentHost).TransformToAncestor(base.AdornedElement);
				}
				else
				{
					this._hostToAdornedElement = Transform.Identity;
				}
				if (hostToAdornedElement != this._hostToAdornedElement && (!(hostToAdornedElement is MatrixTransform) || !(this._hostToAdornedElement is MatrixTransform) || !Matrix.Equals(((MatrixTransform)hostToAdornedElement).Matrix, ((MatrixTransform)this._hostToAdornedElement).Matrix)))
				{
					return true;
				}
				if (this._contentRects != null && contentRects != null && this._contentRects.Count == contentRects.Count)
				{
					for (int i = 0; i < contentRects.Count; i++)
					{
						if (!DoubleUtil.AreClose(contentRects[i].Size, this._contentRects[i].Size))
						{
							return true;
						}
					}
					return false;
				}
				return this._contentRects != contentRects;
			}

			// Token: 0x04004069 RID: 16489
			private GeneralTransform _hostToAdornedElement = Transform.Identity;

			// Token: 0x0400406A RID: 16490
			private IContentHost _contentHostParent;

			// Token: 0x0400406B RID: 16491
			private ContentElement _adornedContentElement;

			// Token: 0x0400406C RID: 16492
			private Style _focusVisualStyle;

			// Token: 0x0400406D RID: 16493
			private UIElement _adorderChild;

			// Token: 0x0400406E RID: 16494
			private UIElementCollection _canvasChildren;

			// Token: 0x0400406F RID: 16495
			private ReadOnlyCollection<Rect> _contentRects;
		}

		// Token: 0x02000854 RID: 2132
		// (Invoke) Token: 0x060082AD RID: 33453
		internal delegate bool EnterMenuModeEventHandler(object sender, EventArgs e);

		// Token: 0x02000855 RID: 2133
		private class WeakReferenceList : DispatcherObject
		{
			// Token: 0x17001D96 RID: 7574
			// (get) Token: 0x060082B0 RID: 33456 RVA: 0x002439F6 File Offset: 0x00241BF6
			public int Count
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x060082B1 RID: 33457 RVA: 0x00243A03 File Offset: 0x00241C03
			public void Add(object item)
			{
				if (this._list.Count == this._list.Capacity)
				{
					this.Purge();
				}
				this._list.Add(new WeakReference(item));
			}

			// Token: 0x060082B2 RID: 33458 RVA: 0x00243A34 File Offset: 0x00241C34
			public void Remove(object target)
			{
				bool flag = false;
				for (int i = 0; i < this._list.Count; i++)
				{
					object target2 = this._list[i].Target;
					if (target2 != null)
					{
						if (target2 == target)
						{
							this._list.RemoveAt(i);
							i--;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.Purge();
				}
			}

			// Token: 0x060082B3 RID: 33459 RVA: 0x00243A90 File Offset: 0x00241C90
			public void Process(Func<object, bool> action)
			{
				bool flag = false;
				for (int i = 0; i < this._list.Count; i++)
				{
					object target = this._list[i].Target;
					if (target != null)
					{
						if (action(target))
						{
							break;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (flag)
				{
					this.ScheduleCleanup();
				}
			}

			// Token: 0x060082B4 RID: 33460 RVA: 0x00243AE0 File Offset: 0x00241CE0
			private void Purge()
			{
				int num = 0;
				int count = this._list.Count;
				for (int i = 0; i < count; i++)
				{
					if (this._list[i].IsAlive)
					{
						this._list[num++] = this._list[i];
					}
				}
				if (num < count)
				{
					this._list.RemoveRange(num, count - num);
					int num2 = num << 1;
					if (num2 < this._list.Capacity)
					{
						this._list.Capacity = num2;
					}
				}
			}

			// Token: 0x060082B5 RID: 33461 RVA: 0x00243B67 File Offset: 0x00241D67
			private void ScheduleCleanup()
			{
				if (!this._isCleanupRequested)
				{
					this._isCleanupRequested = true;
					base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(delegate(object unused)
					{
						lock (this)
						{
							this.Purge();
							this._isCleanupRequested = false;
						}
						return null;
					}), null);
				}
			}

			// Token: 0x04004070 RID: 16496
			private List<WeakReference> _list = new List<WeakReference>(1);

			// Token: 0x04004071 RID: 16497
			private bool _isCleanupRequested;
		}
	}
}

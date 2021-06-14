using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Provides methods and attached properties that support data validation.</summary>
	// Token: 0x02000550 RID: 1360
	public static class Validation
	{
		/// <summary>Adds an event handler for the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event to the specified object.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to add <paramref name="handler" /> to.</param>
		/// <param name="handler">The handler to add.</param>
		// Token: 0x0600596E RID: 22894 RVA: 0x0018B106 File Offset: 0x00189306
		public static void AddErrorHandler(DependencyObject element, EventHandler<ValidationErrorEventArgs> handler)
		{
			UIElement.AddHandler(element, Validation.ErrorEvent, handler);
		}

		/// <summary>Adds an event handler for the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event from the specified object.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to remove <paramref name="handler" /> from.</param>
		/// <param name="handler">The handler to remove.</param>
		// Token: 0x0600596F RID: 22895 RVA: 0x0018B114 File Offset: 0x00189314
		public static void RemoveErrorHandler(DependencyObject element, EventHandler<ValidationErrorEventArgs> handler)
		{
			UIElement.RemoveHandler(element, Validation.ErrorEvent, handler);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Validation.Errors" /> attached property of the specified element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to read the value from.</param>
		/// <returns>A <see cref="T:System.Collections.ObjectModel.ReadOnlyObservableCollection`1" /> of <see cref="T:System.Windows.Controls.ValidationError" /> objects.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005970 RID: 22896 RVA: 0x0018B122 File Offset: 0x00189322
		public static ReadOnlyObservableCollection<ValidationError> GetErrors(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ReadOnlyObservableCollection<ValidationError>)element.GetValue(Validation.ErrorsProperty);
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x0018B144 File Offset: 0x00189344
		private static void OnErrorsInternalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ValidationErrorCollection validationErrorCollection = e.NewValue as ValidationErrorCollection;
			if (validationErrorCollection != null)
			{
				d.SetValue(Validation.ErrorsPropertyKey, new ReadOnlyObservableCollection<ValidationError>(validationErrorCollection));
				return;
			}
			d.ClearValue(Validation.ErrorsPropertyKey);
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x0018B17E File Offset: 0x0018937E
		internal static ValidationErrorCollection GetErrorsInternal(DependencyObject target)
		{
			return (ValidationErrorCollection)target.GetValue(Validation.ValidationErrorsInternalProperty);
		}

		// Token: 0x06005973 RID: 22899 RVA: 0x0018B190 File Offset: 0x00189390
		private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = d as Control;
			if (control != null)
			{
				Control.OnVisualStatePropertyChanged(control, e);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Validation.HasError" /> attached property of the specified element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to read the value from.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Validation.HasError" /> attached property of the specified element.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005974 RID: 22900 RVA: 0x0018B1AE File Offset: 0x001893AE
		public static bool GetHasError(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(Validation.HasErrorProperty);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Validation.ErrorTemplate" /> attached property of the specified element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to read the value from.</param>
		/// <returns>The <see cref="T:System.Windows.Controls.ControlTemplate" /> used to generate validation error feedback on the adorner layer.</returns>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005975 RID: 22901 RVA: 0x0018B1CE File Offset: 0x001893CE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static ControlTemplate GetErrorTemplate(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ErrorTemplateProperty) as ControlTemplate;
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.Validation.ErrorTemplate" /> attached property to the specified element.</summary>
		/// <param name="element">The <see cref="T:System.Windows.UIElement" /> or <see cref="T:System.Windows.ContentElement" /> object to set <paramref name="value" /> on.</param>
		/// <param name="value">The <see cref="T:System.Windows.Controls.ControlTemplate" /> to use to generate validation error feedback on the adorner layer.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06005976 RID: 22902 RVA: 0x0018B1F0 File Offset: 0x001893F0
		public static void SetErrorTemplate(DependencyObject element, ControlTemplate value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			object objA = element.ReadLocalValue(Validation.ErrorTemplateProperty);
			if (!object.Equals(objA, value))
			{
				element.SetValue(Validation.ErrorTemplateProperty, value);
			}
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x0018B22C File Offset: 0x0018942C
		private static void OnErrorTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (Validation.GetHasError(d))
			{
				Validation.ShowValidationAdorner(d, false);
				Validation.ShowValidationAdorner(d, true);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" /> attached property for the specified element.</summary>
		/// <param name="element">The element from which to get the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" />.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" />.</returns>
		// Token: 0x06005978 RID: 22904 RVA: 0x0018B244 File Offset: 0x00189444
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static DependencyObject GetValidationAdornerSite(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ValidationAdornerSiteProperty) as DependencyObject;
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" /> attached property to the specified value on the specified element.</summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" />.</param>
		/// <param name="value">The <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" /> of the specified element.</param>
		// Token: 0x06005979 RID: 22905 RVA: 0x0018B264 File Offset: 0x00189464
		public static void SetValidationAdornerSite(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Validation.ValidationAdornerSiteProperty, value);
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x0018B280 File Offset: 0x00189480
		private static void OnValidationAdornerSiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			DependencyObject dependencyObject = (DependencyObject)e.OldValue;
			DependencyObject dependencyObject2 = (DependencyObject)e.NewValue;
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(Validation.ValidationAdornerSiteForProperty);
			}
			if (dependencyObject2 != null && d != Validation.GetValidationAdornerSiteFor(dependencyObject2))
			{
				Validation.SetValidationAdornerSiteFor(dependencyObject2, d);
			}
			if (Validation.GetHasError(d))
			{
				if (dependencyObject == null)
				{
					dependencyObject = d;
				}
				Validation.ShowValidationAdornerHelper(d, dependencyObject, false);
				Validation.ShowValidationAdorner(d, true);
			}
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" /> attached property for the specified element.</summary>
		/// <param name="element">The element from which to get the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" />.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" />.</returns>
		// Token: 0x0600597B RID: 22907 RVA: 0x0018B2EE File Offset: 0x001894EE
		[AttachedPropertyBrowsableForType(typeof(DependencyObject))]
		public static DependencyObject GetValidationAdornerSiteFor(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return element.GetValue(Validation.ValidationAdornerSiteForProperty) as DependencyObject;
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" /> attached property to the specified value on the specified element.</summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" />.</param>
		/// <param name="value">The <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" /> of the specified element.</param>
		// Token: 0x0600597C RID: 22908 RVA: 0x0018B30E File Offset: 0x0018950E
		public static void SetValidationAdornerSiteFor(DependencyObject element, DependencyObject value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(Validation.ValidationAdornerSiteForProperty, value);
		}

		// Token: 0x0600597D RID: 22909 RVA: 0x0018B32C File Offset: 0x0018952C
		private static void OnValidationAdornerSiteForChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			DependencyObject dependencyObject = (DependencyObject)e.OldValue;
			DependencyObject dependencyObject2 = (DependencyObject)e.NewValue;
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(Validation.ValidationAdornerSiteProperty);
			}
			if (dependencyObject2 != null && d != Validation.GetValidationAdornerSite(dependencyObject2))
			{
				Validation.SetValidationAdornerSite(dependencyObject2, d);
			}
		}

		// Token: 0x0600597E RID: 22910 RVA: 0x0018B380 File Offset: 0x00189580
		internal static void ShowValidationAdorner(DependencyObject targetElement, bool show)
		{
			if (!Validation.HasValidationGroup(targetElement as FrameworkElement))
			{
				DependencyObject dependencyObject = Validation.GetValidationAdornerSite(targetElement);
				if (dependencyObject == null)
				{
					dependencyObject = targetElement;
				}
				Validation.ShowValidationAdornerHelper(targetElement, dependencyObject, show);
			}
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x0018B3B0 File Offset: 0x001895B0
		private static bool HasValidationGroup(FrameworkElement fe)
		{
			if (fe != null)
			{
				IList<VisualStateGroup> visualStateGroupsInternal = VisualStateManager.GetVisualStateGroupsInternal(fe);
				if (Validation.HasValidationGroup(visualStateGroupsInternal))
				{
					return true;
				}
				if (fe.StateGroupsRoot != null)
				{
					visualStateGroupsInternal = VisualStateManager.GetVisualStateGroupsInternal(fe.StateGroupsRoot);
					return Validation.HasValidationGroup(visualStateGroupsInternal);
				}
			}
			return false;
		}

		// Token: 0x06005980 RID: 22912 RVA: 0x0018B3F0 File Offset: 0x001895F0
		private static bool HasValidationGroup(IList<VisualStateGroup> groups)
		{
			if (groups != null)
			{
				for (int i = 0; i < groups.Count; i++)
				{
					VisualStateGroup visualStateGroup = groups[i];
					if (visualStateGroup.Name == "ValidationStates")
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x0018B42E File Offset: 0x0018962E
		private static void ShowValidationAdornerHelper(DependencyObject targetElement, DependencyObject adornerSite, bool show)
		{
			Validation.ShowValidationAdornerHelper(targetElement, adornerSite, show, true);
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x0018B43C File Offset: 0x0018963C
		private static object ShowValidationAdornerOperation(object arg)
		{
			object[] array = (object[])arg;
			DependencyObject targetElement = (DependencyObject)array[0];
			DependencyObject adornerSite = (DependencyObject)array[1];
			bool show = (bool)array[2];
			Validation.ShowValidationAdornerHelper(targetElement, adornerSite, show, false);
			return null;
		}

		// Token: 0x06005983 RID: 22915 RVA: 0x0018B478 File Offset: 0x00189678
		private static void ShowValidationAdornerHelper(DependencyObject targetElement, DependencyObject adornerSite, bool show, bool tryAgain)
		{
			UIElement uielement = adornerSite as UIElement;
			if (uielement != null)
			{
				AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(uielement);
				if (adornerLayer == null)
				{
					if (tryAgain)
					{
						adornerSite.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(Validation.ShowValidationAdornerOperation), new object[]
						{
							targetElement,
							adornerSite,
							show
						});
					}
					return;
				}
				TemplatedAdorner templatedAdorner = uielement.ReadLocalValue(Validation.ValidationAdornerProperty) as TemplatedAdorner;
				if (show && templatedAdorner == null)
				{
					ControlTemplate errorTemplate = Validation.GetErrorTemplate(uielement);
					if (errorTemplate == null)
					{
						errorTemplate = Validation.GetErrorTemplate(targetElement);
					}
					if (errorTemplate != null)
					{
						templatedAdorner = new TemplatedAdorner(uielement, errorTemplate);
						adornerLayer.Add(templatedAdorner);
						uielement.SetValue(Validation.ValidationAdornerProperty, templatedAdorner);
						return;
					}
				}
				else if (!show && templatedAdorner != null)
				{
					templatedAdorner.ClearChild();
					adornerLayer.Remove(templatedAdorner);
					uielement.ClearValue(Validation.ValidationAdornerProperty);
				}
			}
		}

		/// <summary>Marks the specified <see cref="T:System.Windows.Data.BindingExpression" /> object as invalid with the specified <see cref="T:System.Windows.Controls.ValidationError" /> object.</summary>
		/// <param name="bindingExpression">The <see cref="T:System.Windows.Data.BindingExpression" /> object to mark as invalid.</param>
		/// <param name="validationError">The <see cref="T:System.Windows.Controls.ValidationError" /> object to use.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="bindingExpression" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="validationError" /> is <see langword="null" />.</exception>
		// Token: 0x06005984 RID: 22916 RVA: 0x0018B535 File Offset: 0x00189735
		public static void MarkInvalid(BindingExpressionBase bindingExpression, ValidationError validationError)
		{
			if (bindingExpression == null)
			{
				throw new ArgumentNullException("bindingExpression");
			}
			if (validationError == null)
			{
				throw new ArgumentNullException("validationError");
			}
			bindingExpression.UpdateValidationError(validationError, false);
		}

		/// <summary>Removes all <see cref="T:System.Windows.Controls.ValidationError" /> objects from the specified <see cref="T:System.Windows.Data.BindingExpressionBase" /> object.</summary>
		/// <param name="bindingExpression">The object to turn valid.</param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="bindingExpression" /> is <see langword="null" />.</exception>
		// Token: 0x06005985 RID: 22917 RVA: 0x0018B55B File Offset: 0x0018975B
		public static void ClearInvalid(BindingExpressionBase bindingExpression)
		{
			if (bindingExpression == null)
			{
				throw new ArgumentNullException("bindingExpression");
			}
			bindingExpression.UpdateValidationError(null, false);
		}

		// Token: 0x06005986 RID: 22918 RVA: 0x0018B574 File Offset: 0x00189774
		internal static void AddValidationError(ValidationError validationError, DependencyObject targetElement, bool shouldRaiseEvent)
		{
			if (targetElement == null)
			{
				return;
			}
			ValidationErrorCollection validationErrorCollection = Validation.GetErrorsInternal(targetElement);
			bool flag;
			if (validationErrorCollection == null)
			{
				flag = true;
				validationErrorCollection = new ValidationErrorCollection();
				validationErrorCollection.Add(validationError);
				targetElement.SetValue(Validation.ValidationErrorsInternalProperty, validationErrorCollection);
			}
			else
			{
				flag = (validationErrorCollection.Count == 0);
				validationErrorCollection.Add(validationError);
			}
			if (flag)
			{
				targetElement.SetValue(Validation.HasErrorPropertyKey, BooleanBoxes.TrueBox);
			}
			if (shouldRaiseEvent)
			{
				Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Added);
			}
			if (flag)
			{
				Validation.ShowValidationAdorner(targetElement, true);
			}
		}

		// Token: 0x06005987 RID: 22919 RVA: 0x0018B5E8 File Offset: 0x001897E8
		internal static void RemoveValidationError(ValidationError validationError, DependencyObject targetElement, bool shouldRaiseEvent)
		{
			if (targetElement == null)
			{
				return;
			}
			ValidationErrorCollection errorsInternal = Validation.GetErrorsInternal(targetElement);
			if (errorsInternal == null || errorsInternal.Count == 0 || !errorsInternal.Contains(validationError))
			{
				return;
			}
			bool flag = errorsInternal.Count == 1;
			if (flag)
			{
				targetElement.ClearValue(Validation.HasErrorPropertyKey);
				targetElement.ClearValue(Validation.ValidationErrorsInternalProperty);
				if (shouldRaiseEvent)
				{
					Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Removed);
				}
				Validation.ShowValidationAdorner(targetElement, false);
				return;
			}
			errorsInternal.Remove(validationError);
			if (shouldRaiseEvent)
			{
				Validation.OnValidationError(targetElement, validationError, ValidationErrorEventAction.Removed);
			}
		}

		// Token: 0x06005988 RID: 22920 RVA: 0x0018B660 File Offset: 0x00189860
		private static void OnValidationError(DependencyObject source, ValidationError validationError, ValidationErrorEventAction action)
		{
			ValidationErrorEventArgs e = new ValidationErrorEventArgs(validationError, action);
			if (source is ContentElement)
			{
				((ContentElement)source).RaiseEvent(e);
				return;
			}
			if (source is UIElement)
			{
				((UIElement)source).RaiseEvent(e);
				return;
			}
			if (source is UIElement3D)
			{
				((UIElement3D)source).RaiseEvent(e);
			}
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x0018B6B4 File Offset: 0x001898B4
		private static ControlTemplate CreateDefaultErrorTemplate()
		{
			ControlTemplate controlTemplate = new ControlTemplate(typeof(Control));
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Border), "Border");
			frameworkElementFactory.SetValue(Border.BorderBrushProperty, Brushes.Red);
			frameworkElementFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1.0));
			FrameworkElementFactory child = new FrameworkElementFactory(typeof(AdornedElementPlaceholder), "Placeholder");
			frameworkElementFactory.AppendChild(child);
			controlTemplate.VisualTree = frameworkElementFactory;
			controlTemplate.Seal();
			return controlTemplate;
		}

		/// <summary>Identifies the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event.</summary>
		/// <returns>The identifier for the <see cref="E:System.Windows.Controls.Validation.Error" /> attached event.</returns>
		// Token: 0x04002F00 RID: 12032
		public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent("ValidationError", RoutingStrategy.Bubble, typeof(EventHandler<ValidationErrorEventArgs>), typeof(Validation));

		// Token: 0x04002F01 RID: 12033
		internal static readonly DependencyPropertyKey ErrorsPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Errors", typeof(ReadOnlyObservableCollection<ValidationError>), typeof(Validation), new FrameworkPropertyMetadata(ValidationErrorCollection.Empty, FrameworkPropertyMetadataOptions.NotDataBindable));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Validation.Errors" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Validation.Errors" /> attached property.</returns>
		// Token: 0x04002F02 RID: 12034
		public static readonly DependencyProperty ErrorsProperty = Validation.ErrorsPropertyKey.DependencyProperty;

		// Token: 0x04002F03 RID: 12035
		internal static readonly DependencyProperty ValidationErrorsInternalProperty = DependencyProperty.RegisterAttached("ErrorsInternal", typeof(ValidationErrorCollection), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnErrorsInternalChanged)));

		// Token: 0x04002F04 RID: 12036
		internal static readonly DependencyPropertyKey HasErrorPropertyKey = DependencyProperty.RegisterAttachedReadOnly("HasError", typeof(bool), typeof(Validation), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Validation.OnHasErrorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Validation.HasError" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Validation.HasError" /> attached property.</returns>
		// Token: 0x04002F05 RID: 12037
		public static readonly DependencyProperty HasErrorProperty = Validation.HasErrorPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Validation.ErrorTemplate" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Validation.ErrorTemplate" /> attached property.</returns>
		// Token: 0x04002F06 RID: 12038
		public static readonly DependencyProperty ErrorTemplateProperty = DependencyProperty.RegisterAttached("ErrorTemplate", typeof(ControlTemplate), typeof(Validation), new FrameworkPropertyMetadata(Validation.CreateDefaultErrorTemplate(), FrameworkPropertyMetadataOptions.NotDataBindable, new PropertyChangedCallback(Validation.OnErrorTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSite" /> attached property.</returns>
		// Token: 0x04002F07 RID: 12039
		public static readonly DependencyProperty ValidationAdornerSiteProperty = DependencyProperty.RegisterAttached("ValidationAdornerSite", typeof(DependencyObject), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnValidationAdornerSiteChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Validation.ValidationAdornerSiteFor" /> attached property.</returns>
		// Token: 0x04002F08 RID: 12040
		public static readonly DependencyProperty ValidationAdornerSiteForProperty = DependencyProperty.RegisterAttached("ValidationAdornerSiteFor", typeof(DependencyObject), typeof(Validation), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Validation.OnValidationAdornerSiteForChanged)));

		// Token: 0x04002F09 RID: 12041
		private static readonly DependencyProperty ValidationAdornerProperty = DependencyProperty.RegisterAttached("ValidationAdorner", typeof(TemplatedAdorner), typeof(Validation), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.NotDataBindable));
	}
}

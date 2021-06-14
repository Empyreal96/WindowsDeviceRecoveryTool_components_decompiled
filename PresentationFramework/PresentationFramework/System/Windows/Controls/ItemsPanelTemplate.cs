using System;
using System.Xaml;

namespace System.Windows.Controls
{
	/// <summary>Specifies the panel that the <see cref="T:System.Windows.Controls.ItemsPresenter" /> creates for the layout of the items of an <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
	// Token: 0x020004F7 RID: 1271
	public class ItemsPanelTemplate : FrameworkTemplate
	{
		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> class.</summary>
		// Token: 0x060050FC RID: 20732 RVA: 0x0000A1F1 File Offset: 0x000083F1
		public ItemsPanelTemplate()
		{
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> class with the specified template.</summary>
		/// <param name="root">The <see cref="T:System.Windows.FrameworkElementFactory" /> object that represents the template.</param>
		// Token: 0x060050FD RID: 20733 RVA: 0x0016BB05 File Offset: 0x00169D05
		public ItemsPanelTemplate(FrameworkElementFactory root)
		{
			base.VisualTree = root;
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x060050FE RID: 20734 RVA: 0x0016BB14 File Offset: 0x00169D14
		internal override Type TargetTypeInternal
		{
			get
			{
				return ItemsPanelTemplate.DefaultTargetType;
			}
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x0000A2A7 File Offset: 0x000084A7
		internal override void SetTargetTypeInternal(Type targetType)
		{
			throw new InvalidOperationException(SR.Get("TemplateNotTargetType"));
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06005100 RID: 20736 RVA: 0x0016BB1B File Offset: 0x00169D1B
		internal static Type DefaultTargetType
		{
			get
			{
				return typeof(ItemsPresenter);
			}
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x0016BB28 File Offset: 0x00169D28
		internal override void ProcessTemplateBeforeSeal()
		{
			FrameworkElementFactory visualTree;
			if (base.HasContent)
			{
				TemplateContent template = base.Template;
				XamlType xamlType = template.SchemaContext.GetXamlType(typeof(Panel));
				if (template.RootType == null || !template.RootType.CanAssignTo(xamlType))
				{
					throw new InvalidOperationException(SR.Get("ItemsPanelNotAPanel", new object[]
					{
						template.RootType
					}));
				}
			}
			else if ((visualTree = base.VisualTree) != null)
			{
				if (!typeof(Panel).IsAssignableFrom(visualTree.Type))
				{
					throw new InvalidOperationException(SR.Get("ItemsPanelNotAPanel", new object[]
					{
						visualTree.Type
					}));
				}
				visualTree.SetValue(Panel.IsItemsHostProperty, true);
			}
		}

		/// <summary>Checks that the templated parent is a non-null <see cref="T:System.Windows.Controls.ItemsPresenter" /> object.</summary>
		/// <param name="templatedParent">The element this template is applied to. This must be an <see cref="T:System.Windows.Controls.ItemsPresenter" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="templatedParent" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="templatedParent" /> is not an <see cref="T:System.Windows.Controls.ItemsPresenter" />.</exception>
		// Token: 0x06005102 RID: 20738 RVA: 0x0016BBE8 File Offset: 0x00169DE8
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (!(templatedParent is ItemsPresenter))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					"ItemsPresenter",
					templatedParent.GetType().Name
				}));
			}
		}
	}
}

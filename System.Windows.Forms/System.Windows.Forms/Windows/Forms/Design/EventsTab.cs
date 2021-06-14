using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms.Design
{
	/// <summary>Provides a <see cref="T:System.Windows.Forms.Design.PropertyTab" /> that can display events for selection and linking.</summary>
	// Token: 0x02000498 RID: 1176
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class EventsTab : PropertyTab
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.Design.EventsTab" /> class.</summary>
		/// <param name="sp">An <see cref="T:System.IServiceProvider" /> to use. </param>
		// Token: 0x06004FFE RID: 20478 RVA: 0x0014C348 File Offset: 0x0014A548
		public EventsTab(IServiceProvider sp)
		{
			this.sp = sp;
		}

		/// <summary>Gets the name of the tab.</summary>
		/// <returns>The name of the tab.</returns>
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06004FFF RID: 20479 RVA: 0x0014C357 File Offset: 0x0014A557
		public override string TabName
		{
			get
			{
				return SR.GetString("PBRSToolTipEvents");
			}
		}

		/// <summary>Gets the Help keyword for the tab.</summary>
		/// <returns>The Help keyword for the tab.</returns>
		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06005000 RID: 20480 RVA: 0x0014C363 File Offset: 0x0014A563
		public override string HelpKeyword
		{
			get
			{
				return "Events";
			}
		}

		/// <summary>Gets a value indicating whether the specified object can be extended.</summary>
		/// <param name="extendee">The object to test for extensibility. </param>
		/// <returns>
		///     <see langword="true" /> if the specified object can be extended; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005001 RID: 20481 RVA: 0x0014C36A File Offset: 0x0014A56A
		public override bool CanExtend(object extendee)
		{
			return !Marshal.IsComObject(extendee);
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x0014C375 File Offset: 0x0014A575
		private void OnActiveDesignerChanged(object sender, ActiveDesignerEventArgs adevent)
		{
			this.currentHost = adevent.NewDesigner;
		}

		/// <summary>Gets the default property from the specified object.</summary>
		/// <param name="obj">The object to retrieve the default property of. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptor" /> indicating the default property.</returns>
		// Token: 0x06005003 RID: 20483 RVA: 0x0014C384 File Offset: 0x0014A584
		public override PropertyDescriptor GetDefaultProperty(object obj)
		{
			IEventBindingService eventPropertyService = this.GetEventPropertyService(obj, null);
			if (eventPropertyService == null)
			{
				return null;
			}
			EventDescriptor defaultEvent = TypeDescriptor.GetDefaultEvent(obj);
			if (defaultEvent != null)
			{
				return eventPropertyService.GetEventProperty(defaultEvent);
			}
			return null;
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x0014C3B4 File Offset: 0x0014A5B4
		private IEventBindingService GetEventPropertyService(object obj, ITypeDescriptorContext context)
		{
			IEventBindingService eventBindingService = null;
			if (!this.sunkEvent)
			{
				IDesignerEventService designerEventService = (IDesignerEventService)this.sp.GetService(typeof(IDesignerEventService));
				if (designerEventService != null)
				{
					designerEventService.ActiveDesignerChanged += this.OnActiveDesignerChanged;
				}
				this.sunkEvent = true;
			}
			if (eventBindingService == null && this.currentHost != null)
			{
				eventBindingService = (IEventBindingService)this.currentHost.GetService(typeof(IEventBindingService));
			}
			if (eventBindingService == null && obj is IComponent)
			{
				ISite site = ((IComponent)obj).Site;
				if (site != null)
				{
					eventBindingService = (IEventBindingService)site.GetService(typeof(IEventBindingService));
				}
			}
			if (eventBindingService == null && context != null)
			{
				eventBindingService = (IEventBindingService)context.GetService(typeof(IEventBindingService));
			}
			return eventBindingService;
		}

		/// <summary>Gets all the properties of the event tab that match the specified attributes.</summary>
		/// <param name="component">The component to retrieve the properties of. </param>
		/// <param name="attributes">An array of <see cref="T:System.Attribute" /> that indicates the attributes of the event properties to retrieve. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties. This will be an empty <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> if the component does not implement an event service.</returns>
		// Token: 0x06005005 RID: 20485 RVA: 0x00142987 File Offset: 0x00140B87
		public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
		{
			return this.GetProperties(null, component, attributes);
		}

		/// <summary>Gets all the properties of the event tab that match the specified attributes and context.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that can be used to gain context information. </param>
		/// <param name="component">The component to retrieve the properties of. </param>
		/// <param name="attributes">An array of type <see cref="T:System.Attribute" /> that indicates the attributes of the event properties to retrieve. </param>
		/// <returns>A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that contains the properties. This will be an empty <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> if the component does not implement an event service.</returns>
		// Token: 0x06005006 RID: 20486 RVA: 0x0014C478 File Offset: 0x0014A678
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
		{
			IEventBindingService eventPropertyService = this.GetEventPropertyService(component, context);
			if (eventPropertyService == null)
			{
				return new PropertyDescriptorCollection(null);
			}
			EventDescriptorCollection events = TypeDescriptor.GetEvents(component, attributes);
			PropertyDescriptorCollection propertyDescriptorCollection = eventPropertyService.GetEventProperties(events);
			Attribute[] array = new Attribute[attributes.Length + 1];
			Array.Copy(attributes, 0, array, 0, attributes.Length);
			array[attributes.Length] = DesignerSerializationVisibilityAttribute.Content;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component, array);
			if (properties.Count > 0)
			{
				ArrayList arrayList = null;
				for (int i = 0; i < properties.Count; i++)
				{
					PropertyDescriptor propertyDescriptor = properties[i];
					TypeConverter converter = propertyDescriptor.Converter;
					if (converter.GetPropertiesSupported())
					{
						object value = propertyDescriptor.GetValue(component);
						EventDescriptorCollection events2 = TypeDescriptor.GetEvents(value, attributes);
						if (events2.Count > 0)
						{
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							propertyDescriptor = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[]
							{
								MergablePropertyAttribute.No
							});
							arrayList.Add(propertyDescriptor);
						}
					}
				}
				if (arrayList != null)
				{
					PropertyDescriptor[] array2 = new PropertyDescriptor[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					PropertyDescriptor[] array3 = new PropertyDescriptor[propertyDescriptorCollection.Count + array2.Length];
					propertyDescriptorCollection.CopyTo(array3, 0);
					Array.Copy(array2, 0, array3, propertyDescriptorCollection.Count, array2.Length);
					propertyDescriptorCollection = new PropertyDescriptorCollection(array3);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x040033F7 RID: 13303
		private IServiceProvider sp;

		// Token: 0x040033F8 RID: 13304
		private IDesignerHost currentHost;

		// Token: 0x040033F9 RID: 13305
		private bool sunkEvent;
	}
}

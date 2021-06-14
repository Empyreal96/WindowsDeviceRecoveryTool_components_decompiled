using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Xaml;

namespace System.Windows.Markup
{
	/// <summary>Converts the string name of an event setter handler to a delegate representation.</summary>
	// Token: 0x020001C2 RID: 450
	public sealed class EventSetterHandlerConverter : TypeConverter
	{
		/// <summary>Returns whether this converter can convert an object of one type to a <see cref="T:System.Delegate" />.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />. </returns>
		// Token: 0x06001D02 RID: 7426 RVA: 0x00018B21 File Offset: 0x00016D21
		public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>Returns whether this converter can convert the object to the specified type.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to. </param>
		/// <returns>Always returns <see langword="false" />.</returns>
		// Token: 0x06001D03 RID: 7427 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
		{
			return false;
		}

		/// <summary>Converts the specified string to a new <see cref="T:System.Delegate" /> for the event handler.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="source">The source string to convert.</param>
		/// <returns>A new <see cref="T:System.Delegate" /> that represents the referenced event handler.</returns>
		/// <exception cref="T:System.NotSupportedException">The necessary services are not available.-or-Could not perform the specific conversion.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="typeDescriptorContext" /> or <paramref name="source" /> are <see langword="null" />.</exception>
		// Token: 0x06001D04 RID: 7428 RVA: 0x00087860 File Offset: 0x00085A60
		public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object source)
		{
			if (typeDescriptorContext == null)
			{
				throw new ArgumentNullException("typeDescriptorContext");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (EventSetterHandlerConverter.s_ServiceProviderContextType == null)
			{
				Assembly assembly = typeof(IRootObjectProvider).Assembly;
				EventSetterHandlerConverter.s_ServiceProviderContextType = assembly.GetType("MS.Internal.Xaml.ServiceProviderContext");
			}
			if (typeDescriptorContext.GetType() != EventSetterHandlerConverter.s_ServiceProviderContextType)
			{
				throw new ArgumentException(SR.Get("TextRange_InvalidParameterValue"), "typeDescriptorContext");
			}
			IRootObjectProvider rootObjectProvider = typeDescriptorContext.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
			if (rootObjectProvider != null && source is string)
			{
				IProvideValueTarget provideValueTarget = typeDescriptorContext.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
				if (provideValueTarget != null)
				{
					EventSetter eventSetter = provideValueTarget.TargetObject as EventSetter;
					string text;
					if (eventSetter != null && (text = (source as string)) != null)
					{
						text = text.Trim();
						return Delegate.CreateDelegate(eventSetter.Event.HandlerType, rootObjectProvider.RootObject, text);
					}
				}
			}
			throw base.GetConvertFromException(source);
		}

		/// <summary>Converts the specified value object to the specified type. Always throws an exception.</summary>
		/// <param name="typeDescriptorContext">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="cultureInfo">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the <paramref name="value" /> parameter to. </param>
		/// <returns>Always throws an exception.</returns>
		/// <exception cref="T:System.NotSupportedException">Thrown in all cases.</exception>
		// Token: 0x06001D05 RID: 7429 RVA: 0x0008795A File Offset: 0x00085B5A
		public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, CultureInfo cultureInfo, object value, Type destinationType)
		{
			throw base.GetConvertToException(value, destinationType);
		}

		// Token: 0x040013FF RID: 5119
		private static Type s_ServiceProviderContextType;
	}
}

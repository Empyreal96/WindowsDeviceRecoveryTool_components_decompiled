using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Baml2006;
using System.Windows.Markup;
using System.Xaml;

namespace System.Windows
{
	/// <summary>Converts a stream to a <see cref="T:System.Windows.DeferrableContent" /> instance.</summary>
	// Token: 0x020000AD RID: 173
	public class DeferrableContentConverter : TypeConverter
	{
		/// <summary>Returns whether this converter can convert the specified object to a <see cref="T:System.Windows.DeferrableContent" /> object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from. </param>
		/// <returns>
		///     <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />. </returns>
		// Token: 0x060003AB RID: 939 RVA: 0x0000A709 File Offset: 0x00008909
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return typeof(Stream).IsAssignableFrom(sourceType) || sourceType == typeof(byte[]) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>Converts the specified stream to a new <see cref="T:System.Windows.DeferrableContent" /> object.</summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value">The source stream to convert.</param>
		/// <returns>A new <see cref="T:System.Windows.DeferrableContent" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="context" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="context" /> is not able to provide the necessary XAML schema context for BAML.-or-
		///         <see cref="T:System.Windows.Markup.IProvideValueTarget" /> service interpretation of <paramref name="context" /> determines that the target object is not a <see cref="T:System.Windows.ResourceDictionary" />.-or-
		///         <paramref name="value" /> is not a valid byte stream.</exception>
		// Token: 0x060003AC RID: 940 RVA: 0x0000A73C File Offset: 0x0000893C
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return base.ConvertFrom(context, culture, value);
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			XamlSchemaContext schemaContext = DeferrableContentConverter.RequireService<IXamlSchemaContextProvider>(context).SchemaContext;
			Baml2006SchemaContext baml2006SchemaContext = schemaContext as Baml2006SchemaContext;
			if (baml2006SchemaContext == null)
			{
				throw new InvalidOperationException(SR.Get("ExpectedBamlSchemaContext"));
			}
			IXamlObjectWriterFactory objectWriterFactory = DeferrableContentConverter.RequireService<IXamlObjectWriterFactory>(context);
			IProvideValueTarget provideValueTarget = DeferrableContentConverter.RequireService<IProvideValueTarget>(context);
			IRootObjectProvider rootObjectProvider = DeferrableContentConverter.RequireService<IRootObjectProvider>(context);
			if (!(provideValueTarget.TargetObject is ResourceDictionary))
			{
				throw new InvalidOperationException(SR.Get("ExpectedResourceDictionaryTarget"));
			}
			Stream stream = value as Stream;
			if (stream == null)
			{
				byte[] array = value as byte[];
				if (array != null)
				{
					stream = new MemoryStream(array);
				}
			}
			if (stream == null)
			{
				throw new InvalidOperationException(SR.Get("ExpectedBinaryContent"));
			}
			return new DeferrableContent(stream, baml2006SchemaContext, objectWriterFactory, context, rootObjectProvider.RootObject);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000A810 File Offset: 0x00008A10
		private static T RequireService<T>(IServiceProvider provider) where T : class
		{
			T t = provider.GetService(typeof(T)) as T;
			if (t == null)
			{
				throw new InvalidOperationException(SR.Get("DeferringLoaderNoContext", new object[]
				{
					typeof(DeferrableContentConverter).Name,
					typeof(T).Name
				}));
			}
			return t;
		}
	}
}

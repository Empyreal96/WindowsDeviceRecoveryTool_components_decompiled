using System;
using System.Xaml;

namespace System.Windows
{
	/// <summary>Implements <see cref="T:System.Xaml.XamlDeferringLoader" /> in order to defer loading of the XAML content that is defined for a template in WPF XAML.</summary>
	// Token: 0x02000121 RID: 289
	public class TemplateContentLoader : XamlDeferringLoader
	{
		/// <summary>Loads XAML content in a deferred mode, based on a <see cref="T:System.Xaml.XamlReader" /> and certain required services from a service provider.</summary>
		/// <param name="xamlReader">The initiating reader that is then returned on calls to <see cref="M:System.Windows.TemplateContentLoader.Save(System.Object,System.IServiceProvider)" />.</param>
		/// <param name="serviceProvider">Service provider for required services.</param>
		/// <returns>The root object for the node stream of the input <see cref="T:System.Xaml.XamlReader" />. Specifically, this is a <see cref="T:System.Windows.TemplateContent" /> instance.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="xamlReader" /> or <paramref name="serviceProvider" /> are <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="serviceProvider" /> does not provide a required service.</exception>
		// Token: 0x06000C0B RID: 3083 RVA: 0x0002CEF8 File Offset: 0x0002B0F8
		public override object Load(XamlReader xamlReader, IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}
			if (xamlReader == null)
			{
				throw new ArgumentNullException("xamlReader");
			}
			IXamlObjectWriterFactory factory = TemplateContentLoader.RequireService<IXamlObjectWriterFactory>(serviceProvider);
			return new TemplateContent(xamlReader, factory, serviceProvider);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0002CF30 File Offset: 0x0002B130
		private static T RequireService<T>(IServiceProvider provider) where T : class
		{
			T t = provider.GetService(typeof(T)) as T;
			if (t == null)
			{
				throw new InvalidOperationException(SR.Get("DeferringLoaderNoContext", new object[]
				{
					typeof(TemplateContentLoader).Name,
					typeof(T).Name
				}));
			}
			return t;
		}

		/// <summary>Do not use; always throws an exception. See Remarks.</summary>
		/// <param name="value">The input value to commit for deferred loading.</param>
		/// <param name="serviceProvider">Service provider for required services.</param>
		/// <returns>Always throws an exception. See Remarks.</returns>
		/// <exception cref="T:System.NotSupportedException">Thrown in all cases.</exception>
		// Token: 0x06000C0D RID: 3085 RVA: 0x0002CF9B File Offset: 0x0002B19B
		public override XamlReader Save(object value, IServiceProvider serviceProvider)
		{
			throw new NotSupportedException(SR.Get("DeferringLoaderNoSave", new object[]
			{
				typeof(TemplateContentLoader).Name
			}));
		}
	}
}

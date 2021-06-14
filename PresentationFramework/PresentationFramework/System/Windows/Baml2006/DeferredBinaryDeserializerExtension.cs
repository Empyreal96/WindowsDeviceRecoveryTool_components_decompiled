using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Baml2006
{
	// Token: 0x02000169 RID: 361
	internal class DeferredBinaryDeserializerExtension : MarkupExtension
	{
		// Token: 0x0600107A RID: 4218 RVA: 0x000419C0 File Offset: 0x0003FBC0
		public DeferredBinaryDeserializerExtension(IFreezeFreezables freezer, BinaryReader reader, int converterId, int dataByteSize)
		{
			this._freezer = freezer;
			this._canFreeze = freezer.FreezeFreezables;
			byte[] buffer = reader.ReadBytes(dataByteSize);
			this._stream = new MemoryStream(buffer);
			this._reader = new BinaryReader(this._stream);
			this._converterId = converterId;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00041A14 File Offset: 0x0003FC14
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			this._stream.Position = 0L;
			switch (this._converterId)
			{
			case 744:
				return SolidColorBrush.DeserializeFrom(this._reader, new DeferredBinaryDeserializerExtension.DeferredBinaryDeserializerExtensionContext(serviceProvider, this._freezer, this._canFreeze));
			case 746:
				return Parsers.DeserializeStreamGeometry(this._reader);
			case 747:
				return Point3DCollection.DeserializeFrom(this._reader);
			case 748:
				return PointCollection.DeserializeFrom(this._reader);
			case 752:
				return Vector3DCollection.DeserializeFrom(this._reader);
			}
			throw new NotImplementedException();
		}

		// Token: 0x0400122D RID: 4653
		private IFreezeFreezables _freezer;

		// Token: 0x0400122E RID: 4654
		private bool _canFreeze;

		// Token: 0x0400122F RID: 4655
		private readonly BinaryReader _reader;

		// Token: 0x04001230 RID: 4656
		private readonly Stream _stream;

		// Token: 0x04001231 RID: 4657
		private readonly int _converterId;

		// Token: 0x0200084B RID: 2123
		private class DeferredBinaryDeserializerExtensionContext : ITypeDescriptorContext, IServiceProvider, IFreezeFreezables
		{
			// Token: 0x06007F34 RID: 32564 RVA: 0x00241323 File Offset: 0x0023F523
			public DeferredBinaryDeserializerExtensionContext(IServiceProvider serviceProvider, IFreezeFreezables freezer, bool canFreeze)
			{
				this._freezer = freezer;
				this._canFreeze = canFreeze;
				this._serviceProvider = serviceProvider;
			}

			// Token: 0x06007F35 RID: 32565 RVA: 0x00241340 File Offset: 0x0023F540
			object IServiceProvider.GetService(Type serviceType)
			{
				if (serviceType == typeof(IFreezeFreezables))
				{
					return this;
				}
				return this._serviceProvider.GetService(serviceType);
			}

			// Token: 0x06007F36 RID: 32566 RVA: 0x00002137 File Offset: 0x00000337
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x06007F37 RID: 32567 RVA: 0x0000B02A File Offset: 0x0000922A
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001D8D RID: 7565
			// (get) Token: 0x06007F38 RID: 32568 RVA: 0x0000C238 File Offset: 0x0000A438
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D8E RID: 7566
			// (get) Token: 0x06007F39 RID: 32569 RVA: 0x0000C238 File Offset: 0x0000A438
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D8F RID: 7567
			// (get) Token: 0x06007F3A RID: 32570 RVA: 0x0000C238 File Offset: 0x0000A438
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D90 RID: 7568
			// (get) Token: 0x06007F3B RID: 32571 RVA: 0x00241362 File Offset: 0x0023F562
			bool IFreezeFreezables.FreezeFreezables
			{
				get
				{
					return this._canFreeze;
				}
			}

			// Token: 0x06007F3C RID: 32572 RVA: 0x0024136A File Offset: 0x0023F56A
			bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
			{
				return this._freezer.TryFreeze(value, freezable);
			}

			// Token: 0x06007F3D RID: 32573 RVA: 0x00241379 File Offset: 0x0023F579
			Freezable IFreezeFreezables.TryGetFreezable(string value)
			{
				return this._freezer.TryGetFreezable(value);
			}

			// Token: 0x04003CFC RID: 15612
			private IServiceProvider _serviceProvider;

			// Token: 0x04003CFD RID: 15613
			private IFreezeFreezables _freezer;

			// Token: 0x04003CFE RID: 15614
			private bool _canFreeze;
		}
	}
}

using System;
using System.Diagnostics;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x02000321 RID: 801
	internal class PropertyStore
	{
		// Token: 0x060031DA RID: 12762 RVA: 0x000E9224 File Offset: 0x000E7424
		public bool ContainsInteger(int key)
		{
			bool result;
			this.GetInteger(key, out result);
			return result;
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x000E923C File Offset: 0x000E743C
		public bool ContainsObject(int key)
		{
			bool result;
			this.GetObject(key, out result);
			return result;
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x000E9254 File Offset: 0x000E7454
		public static int CreateKey()
		{
			return PropertyStore.currentKey++;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x000E9264 File Offset: 0x000E7464
		public Color GetColor(int key)
		{
			bool flag;
			return this.GetColor(key, out flag);
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x000E927C File Offset: 0x000E747C
		public Color GetColor(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.ColorWrapper colorWrapper = @object as PropertyStore.ColorWrapper;
				if (colorWrapper != null)
				{
					return colorWrapper.Color;
				}
			}
			found = false;
			return Color.Empty;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x000E92B0 File Offset: 0x000E74B0
		public Padding GetPadding(int key)
		{
			bool flag;
			return this.GetPadding(key, out flag);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x000E92C8 File Offset: 0x000E74C8
		public Padding GetPadding(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.PaddingWrapper paddingWrapper = @object as PropertyStore.PaddingWrapper;
				if (paddingWrapper != null)
				{
					return paddingWrapper.Padding;
				}
			}
			found = false;
			return Padding.Empty;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x000E92FC File Offset: 0x000E74FC
		public Size GetSize(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.SizeWrapper sizeWrapper = @object as PropertyStore.SizeWrapper;
				if (sizeWrapper != null)
				{
					return sizeWrapper.Size;
				}
			}
			found = false;
			return Size.Empty;
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x000E9330 File Offset: 0x000E7530
		public Rectangle GetRectangle(int key)
		{
			bool flag;
			return this.GetRectangle(key, out flag);
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x000E9348 File Offset: 0x000E7548
		public Rectangle GetRectangle(int key, out bool found)
		{
			object @object = this.GetObject(key, out found);
			if (found)
			{
				PropertyStore.RectangleWrapper rectangleWrapper = @object as PropertyStore.RectangleWrapper;
				if (rectangleWrapper != null)
				{
					return rectangleWrapper.Rectangle;
				}
			}
			found = false;
			return Rectangle.Empty;
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x000E937C File Offset: 0x000E757C
		public int GetInteger(int key)
		{
			bool flag;
			return this.GetInteger(key, out flag);
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x000E9394 File Offset: 0x000E7594
		public int GetInteger(int key, out bool found)
		{
			int result = 0;
			short num;
			short entryKey = this.SplitKey(key, out num);
			found = false;
			int num2;
			if (this.LocateIntegerEntry(entryKey, out num2) && (1 << (int)num & (int)this.intEntries[num2].Mask) != 0)
			{
				found = true;
				switch (num)
				{
				case 0:
					result = this.intEntries[num2].Value1;
					break;
				case 1:
					result = this.intEntries[num2].Value2;
					break;
				case 2:
					result = this.intEntries[num2].Value3;
					break;
				case 3:
					result = this.intEntries[num2].Value4;
					break;
				}
			}
			return result;
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x000E9444 File Offset: 0x000E7644
		public object GetObject(int key)
		{
			bool flag;
			return this.GetObject(key, out flag);
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x000E945C File Offset: 0x000E765C
		public object GetObject(int key, out bool found)
		{
			object result = null;
			short num;
			short entryKey = this.SplitKey(key, out num);
			found = false;
			int num2;
			if (this.LocateObjectEntry(entryKey, out num2) && (1 << (int)num & (int)this.objEntries[num2].Mask) != 0)
			{
				found = true;
				switch (num)
				{
				case 0:
					result = this.objEntries[num2].Value1;
					break;
				case 1:
					result = this.objEntries[num2].Value2;
					break;
				case 2:
					result = this.objEntries[num2].Value3;
					break;
				case 3:
					result = this.objEntries[num2].Value4;
					break;
				}
			}
			return result;
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x000E950C File Offset: 0x000E770C
		private bool LocateIntegerEntry(short entryKey, out int index)
		{
			if (this.intEntries == null)
			{
				index = 0;
				return false;
			}
			int num = this.intEntries.Length;
			if (num > 16)
			{
				int num2 = num - 1;
				int num3 = 0;
				int num4;
				for (;;)
				{
					num4 = (num2 + num3) / 2;
					short key = this.intEntries[num4].Key;
					if (key == entryKey)
					{
						break;
					}
					if (entryKey < key)
					{
						num2 = num4 - 1;
					}
					else
					{
						num3 = num4 + 1;
					}
					if (num2 < num3)
					{
						goto Block_14;
					}
				}
				index = num4;
				return true;
				Block_14:
				index = num4;
				if (entryKey > this.intEntries[num4].Key)
				{
					index++;
				}
				return false;
			}
			index = 0;
			int num5 = num / 2;
			if (this.intEntries[num5].Key <= entryKey)
			{
				index = num5;
			}
			if (this.intEntries[index].Key == entryKey)
			{
				return true;
			}
			num5 = (num + 1) / 4;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 3) / 8;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 7) / 16;
			if (this.intEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.intEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			if (entryKey > this.intEntries[index].Key)
			{
				index++;
			}
			return false;
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x000E96A0 File Offset: 0x000E78A0
		private bool LocateObjectEntry(short entryKey, out int index)
		{
			if (this.objEntries == null)
			{
				index = 0;
				return false;
			}
			int num = this.objEntries.Length;
			if (num > 16)
			{
				int num2 = num - 1;
				int num3 = 0;
				int num4;
				for (;;)
				{
					num4 = (num2 + num3) / 2;
					short key = this.objEntries[num4].Key;
					if (key == entryKey)
					{
						break;
					}
					if (entryKey < key)
					{
						num2 = num4 - 1;
					}
					else
					{
						num3 = num4 + 1;
					}
					if (num2 < num3)
					{
						goto Block_14;
					}
				}
				index = num4;
				return true;
				Block_14:
				index = num4;
				if (entryKey > this.objEntries[num4].Key)
				{
					index++;
				}
				return false;
			}
			index = 0;
			int num5 = num / 2;
			if (this.objEntries[num5].Key <= entryKey)
			{
				index = num5;
			}
			if (this.objEntries[index].Key == entryKey)
			{
				return true;
			}
			num5 = (num + 1) / 4;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 3) / 8;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			num5 = (num + 7) / 16;
			if (this.objEntries[index + num5].Key <= entryKey)
			{
				index += num5;
				if (this.objEntries[index].Key == entryKey)
				{
					return true;
				}
			}
			if (entryKey > this.objEntries[index].Key)
			{
				index++;
			}
			return false;
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x000E9834 File Offset: 0x000E7A34
		public void RemoveInteger(int key)
		{
			short num;
			short entryKey = this.SplitKey(key, out num);
			int num2;
			if (this.LocateIntegerEntry(entryKey, out num2))
			{
				if ((1 << (int)num & (int)this.intEntries[num2].Mask) == 0)
				{
					return;
				}
				PropertyStore.IntegerEntry[] array = this.intEntries;
				int num3 = num2;
				array[num3].Mask = (array[num3].Mask & ~(short)(1 << (int)num));
				if (this.intEntries[num2].Mask == 0)
				{
					PropertyStore.IntegerEntry[] array2 = new PropertyStore.IntegerEntry[this.intEntries.Length - 1];
					if (num2 > 0)
					{
						Array.Copy(this.intEntries, 0, array2, 0, num2);
					}
					if (num2 < array2.Length)
					{
						Array.Copy(this.intEntries, num2 + 1, array2, num2, this.intEntries.Length - num2 - 1);
					}
					this.intEntries = array2;
					return;
				}
				switch (num)
				{
				case 0:
					this.intEntries[num2].Value1 = 0;
					return;
				case 1:
					this.intEntries[num2].Value2 = 0;
					return;
				case 2:
					this.intEntries[num2].Value3 = 0;
					return;
				case 3:
					this.intEntries[num2].Value4 = 0;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x000E9958 File Offset: 0x000E7B58
		public void RemoveObject(int key)
		{
			short num;
			short entryKey = this.SplitKey(key, out num);
			int num2;
			if (this.LocateObjectEntry(entryKey, out num2))
			{
				if ((1 << (int)num & (int)this.objEntries[num2].Mask) == 0)
				{
					return;
				}
				PropertyStore.ObjectEntry[] array = this.objEntries;
				int num3 = num2;
				array[num3].Mask = (array[num3].Mask & ~(short)(1 << (int)num));
				if (this.objEntries[num2].Mask == 0)
				{
					if (this.objEntries.Length == 1)
					{
						this.objEntries = null;
						return;
					}
					PropertyStore.ObjectEntry[] array2 = new PropertyStore.ObjectEntry[this.objEntries.Length - 1];
					if (num2 > 0)
					{
						Array.Copy(this.objEntries, 0, array2, 0, num2);
					}
					if (num2 < array2.Length)
					{
						Array.Copy(this.objEntries, num2 + 1, array2, num2, this.objEntries.Length - num2 - 1);
					}
					this.objEntries = array2;
					return;
				}
				else
				{
					switch (num)
					{
					case 0:
						this.objEntries[num2].Value1 = null;
						return;
					case 1:
						this.objEntries[num2].Value2 = null;
						return;
					case 2:
						this.objEntries[num2].Value3 = null;
						return;
					case 3:
						this.objEntries[num2].Value4 = null;
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x000E9A8C File Offset: 0x000E7C8C
		public void SetColor(int key, Color value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.ColorWrapper(value));
				return;
			}
			PropertyStore.ColorWrapper colorWrapper = @object as PropertyStore.ColorWrapper;
			if (colorWrapper != null)
			{
				colorWrapper.Color = value;
				return;
			}
			this.SetObject(key, new PropertyStore.ColorWrapper(value));
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x000E9AD4 File Offset: 0x000E7CD4
		public void SetPadding(int key, Padding value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.PaddingWrapper(value));
				return;
			}
			PropertyStore.PaddingWrapper paddingWrapper = @object as PropertyStore.PaddingWrapper;
			if (paddingWrapper != null)
			{
				paddingWrapper.Padding = value;
				return;
			}
			this.SetObject(key, new PropertyStore.PaddingWrapper(value));
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x000E9B1C File Offset: 0x000E7D1C
		public void SetRectangle(int key, Rectangle value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.RectangleWrapper(value));
				return;
			}
			PropertyStore.RectangleWrapper rectangleWrapper = @object as PropertyStore.RectangleWrapper;
			if (rectangleWrapper != null)
			{
				rectangleWrapper.Rectangle = value;
				return;
			}
			this.SetObject(key, new PropertyStore.RectangleWrapper(value));
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x000E9B64 File Offset: 0x000E7D64
		public void SetSize(int key, Size value)
		{
			bool flag;
			object @object = this.GetObject(key, out flag);
			if (!flag)
			{
				this.SetObject(key, new PropertyStore.SizeWrapper(value));
				return;
			}
			PropertyStore.SizeWrapper sizeWrapper = @object as PropertyStore.SizeWrapper;
			if (sizeWrapper != null)
			{
				sizeWrapper.Size = value;
				return;
			}
			this.SetObject(key, new PropertyStore.SizeWrapper(value));
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x000E9BAC File Offset: 0x000E7DAC
		public void SetInteger(int key, int value)
		{
			short num2;
			short num = this.SplitKey(key, out num2);
			int num3;
			if (!this.LocateIntegerEntry(num, out num3))
			{
				if (this.intEntries != null)
				{
					PropertyStore.IntegerEntry[] destinationArray = new PropertyStore.IntegerEntry[this.intEntries.Length + 1];
					if (num3 > 0)
					{
						Array.Copy(this.intEntries, 0, destinationArray, 0, num3);
					}
					if (this.intEntries.Length - num3 > 0)
					{
						Array.Copy(this.intEntries, num3, destinationArray, num3 + 1, this.intEntries.Length - num3);
					}
					this.intEntries = destinationArray;
				}
				else
				{
					this.intEntries = new PropertyStore.IntegerEntry[1];
				}
				this.intEntries[num3].Key = num;
			}
			switch (num2)
			{
			case 0:
				this.intEntries[num3].Value1 = value;
				break;
			case 1:
				this.intEntries[num3].Value2 = value;
				break;
			case 2:
				this.intEntries[num3].Value3 = value;
				break;
			case 3:
				this.intEntries[num3].Value4 = value;
				break;
			}
			this.intEntries[num3].Mask = (short)(1 << (int)num2 | (int)((ushort)this.intEntries[num3].Mask));
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x000E9CD8 File Offset: 0x000E7ED8
		public void SetObject(int key, object value)
		{
			short num2;
			short num = this.SplitKey(key, out num2);
			int num3;
			if (!this.LocateObjectEntry(num, out num3))
			{
				if (this.objEntries != null)
				{
					PropertyStore.ObjectEntry[] destinationArray = new PropertyStore.ObjectEntry[this.objEntries.Length + 1];
					if (num3 > 0)
					{
						Array.Copy(this.objEntries, 0, destinationArray, 0, num3);
					}
					if (this.objEntries.Length - num3 > 0)
					{
						Array.Copy(this.objEntries, num3, destinationArray, num3 + 1, this.objEntries.Length - num3);
					}
					this.objEntries = destinationArray;
				}
				else
				{
					this.objEntries = new PropertyStore.ObjectEntry[1];
				}
				this.objEntries[num3].Key = num;
			}
			switch (num2)
			{
			case 0:
				this.objEntries[num3].Value1 = value;
				break;
			case 1:
				this.objEntries[num3].Value2 = value;
				break;
			case 2:
				this.objEntries[num3].Value3 = value;
				break;
			case 3:
				this.objEntries[num3].Value4 = value;
				break;
			}
			this.objEntries[num3].Mask = (short)((int)((ushort)this.objEntries[num3].Mask) | 1 << (int)num2);
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x000E9E04 File Offset: 0x000E8004
		private short SplitKey(int key, out short element)
		{
			element = (short)(key & 3);
			return (short)((long)key & (long)((ulong)-4));
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x000E9E14 File Offset: 0x000E8014
		[Conditional("DEBUG_PROPERTYSTORE")]
		private void Debug_VerifyLocateIntegerEntry(int index, short entryKey, int length)
		{
			int num = length - 1;
			int num2 = 0;
			int num3;
			do
			{
				num3 = (num + num2) / 2;
				short key = this.intEntries[num3].Key;
				if (key != entryKey)
				{
					if (entryKey < key)
					{
						num = num3 - 1;
					}
					else
					{
						num2 = num3 + 1;
					}
				}
			}
			while (num >= num2);
			if (entryKey > this.intEntries[num3].Key)
			{
				num3++;
			}
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x000E9E70 File Offset: 0x000E8070
		[Conditional("DEBUG_PROPERTYSTORE")]
		private void Debug_VerifyLocateObjectEntry(int index, short entryKey, int length)
		{
			int num = length - 1;
			int num2 = 0;
			int num3;
			do
			{
				num3 = (num + num2) / 2;
				short key = this.objEntries[num3].Key;
				if (key != entryKey)
				{
					if (entryKey < key)
					{
						num = num3 - 1;
					}
					else
					{
						num2 = num3 + 1;
					}
				}
			}
			while (num >= num2);
			if (entryKey > this.objEntries[num3].Key)
			{
				num3++;
			}
		}

		// Token: 0x04001E28 RID: 7720
		private static int currentKey;

		// Token: 0x04001E29 RID: 7721
		private PropertyStore.IntegerEntry[] intEntries;

		// Token: 0x04001E2A RID: 7722
		private PropertyStore.ObjectEntry[] objEntries;

		// Token: 0x02000708 RID: 1800
		private struct IntegerEntry
		{
			// Token: 0x04004121 RID: 16673
			public short Key;

			// Token: 0x04004122 RID: 16674
			public short Mask;

			// Token: 0x04004123 RID: 16675
			public int Value1;

			// Token: 0x04004124 RID: 16676
			public int Value2;

			// Token: 0x04004125 RID: 16677
			public int Value3;

			// Token: 0x04004126 RID: 16678
			public int Value4;
		}

		// Token: 0x02000709 RID: 1801
		private struct ObjectEntry
		{
			// Token: 0x04004127 RID: 16679
			public short Key;

			// Token: 0x04004128 RID: 16680
			public short Mask;

			// Token: 0x04004129 RID: 16681
			public object Value1;

			// Token: 0x0400412A RID: 16682
			public object Value2;

			// Token: 0x0400412B RID: 16683
			public object Value3;

			// Token: 0x0400412C RID: 16684
			public object Value4;
		}

		// Token: 0x0200070A RID: 1802
		private sealed class ColorWrapper
		{
			// Token: 0x06005FD4 RID: 24532 RVA: 0x00189779 File Offset: 0x00187979
			public ColorWrapper(Color color)
			{
				this.Color = color;
			}

			// Token: 0x0400412D RID: 16685
			public Color Color;
		}

		// Token: 0x0200070B RID: 1803
		private sealed class PaddingWrapper
		{
			// Token: 0x06005FD5 RID: 24533 RVA: 0x00189788 File Offset: 0x00187988
			public PaddingWrapper(Padding padding)
			{
				this.Padding = padding;
			}

			// Token: 0x0400412E RID: 16686
			public Padding Padding;
		}

		// Token: 0x0200070C RID: 1804
		private sealed class RectangleWrapper
		{
			// Token: 0x06005FD6 RID: 24534 RVA: 0x00189797 File Offset: 0x00187997
			public RectangleWrapper(Rectangle rectangle)
			{
				this.Rectangle = rectangle;
			}

			// Token: 0x0400412F RID: 16687
			public Rectangle Rectangle;
		}

		// Token: 0x0200070D RID: 1805
		private sealed class SizeWrapper
		{
			// Token: 0x06005FD7 RID: 24535 RVA: 0x001897A6 File Offset: 0x001879A6
			public SizeWrapper(Size size)
			{
				this.Size = size;
			}

			// Token: 0x04004130 RID: 16688
			public Size Size;
		}
	}
}

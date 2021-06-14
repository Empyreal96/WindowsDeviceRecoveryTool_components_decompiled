using System;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x0200004D RID: 77
	internal class Allocator
	{
		// Token: 0x06000307 RID: 775 RVA: 0x000194FC File Offset: 0x000184FC
		static Allocator()
		{
			Allocator.indexToUnits = new byte[38];
			uint num = 0U;
			uint num2 = 1U;
			while (num < 4U)
			{
				Allocator.indexToUnits[(int)((UIntPtr)num)] = (byte)num2;
				num += 1U;
				num2 += 1U;
			}
			num2 += 1U;
			while (num < 8U)
			{
				Allocator.indexToUnits[(int)((UIntPtr)num)] = (byte)num2;
				num += 1U;
				num2 += 2U;
			}
			num2 += 1U;
			while (num < 12U)
			{
				Allocator.indexToUnits[(int)((UIntPtr)num)] = (byte)num2;
				num += 1U;
				num2 += 3U;
			}
			num2 += 1U;
			while (num < 38U)
			{
				Allocator.indexToUnits[(int)((UIntPtr)num)] = (byte)num2;
				num += 1U;
				num2 += 4U;
			}
			Allocator.unitsToIndex = new byte[128];
			num = (num2 = 0U);
			while (num2 < 128U)
			{
				num += (((uint)Allocator.indexToUnits[(int)((UIntPtr)num)] < num2 + 1U) ? 1U : 0U);
				Allocator.unitsToIndex[(int)((UIntPtr)num2)] = (byte)num;
				num2 += 1U;
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x000195D4 File Offset: 0x000185D4
		public static void Initialize()
		{
			int num = 0;
			while ((long)num < 38L)
			{
				Allocator.MemoryNodes[num] = new MemoryNode((uint)(16L + (long)(num * 12)));
				Allocator.MemoryNodes[num].Stamp = 0U;
				Allocator.MemoryNodes[num].Next = MemoryNode.Zero;
				Allocator.MemoryNodes[num].UnitCount = 0U;
				num++;
			}
			Allocator.Text = Allocator.Heap;
			uint offset = 12U * (Allocator.AllocatorSize / 8U / 12U * 7U);
			Allocator.HighUnit = Allocator.Heap + Allocator.AllocatorSize;
			Allocator.LowUnit = Allocator.HighUnit - offset;
			Allocator.BaseUnit = Allocator.HighUnit - offset;
			Allocator.GlueCount = 0U;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0001969C File Offset: 0x0001869C
		public static void Start(int allocatorSize)
		{
			uint num = (uint)((uint)allocatorSize << 20);
			if (Allocator.AllocatorSize != num)
			{
				Allocator.Stop();
				byte[] memory = new byte[472U + num];
				Pointer.Memory = memory;
				MemoryNode.Memory = memory;
				Model.PpmContext.Memory = memory;
				PpmState.Memory = memory;
				Allocator.Heap = new Pointer(472U);
				Allocator.AllocatorSize = num;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000196F6 File Offset: 0x000186F6
		public static void Stop()
		{
			if (Allocator.AllocatorSize != 0U)
			{
				Allocator.AllocatorSize = 0U;
				Pointer.Memory = null;
				MemoryNode.Memory = null;
				Model.PpmContext.Memory = null;
				PpmState.Memory = null;
				Allocator.Heap = Pointer.Zero;
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00019728 File Offset: 0x00018728
		public static uint GetMemoryUsed()
		{
			uint num = Allocator.AllocatorSize - (Allocator.HighUnit - Allocator.LowUnit) - (Allocator.BaseUnit - Allocator.Text);
			for (uint num2 = 0U; num2 < 38U; num2 += 1U)
			{
				num -= (uint)(12 * Allocator.indexToUnits[(int)((UIntPtr)num2)]) * Allocator.MemoryNodes[(int)((UIntPtr)num2)].Stamp;
			}
			return num;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0001978C File Offset: 0x0001878C
		public static Pointer AllocateUnits(uint unitCount)
		{
			uint num = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(unitCount - 1U))];
			if (Allocator.MemoryNodes[(int)((UIntPtr)num)].Available)
			{
				return Allocator.MemoryNodes[(int)((UIntPtr)num)].Remove();
			}
			Pointer lowUnit = Allocator.LowUnit;
			Allocator.LowUnit += (uint)(Allocator.indexToUnits[(int)((UIntPtr)num)] * 12);
			if (Allocator.LowUnit <= Allocator.HighUnit)
			{
				return lowUnit;
			}
			Allocator.LowUnit -= (uint)(Allocator.indexToUnits[(int)((UIntPtr)num)] * 12);
			return Allocator.AllocateUnitsRare(num);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00019824 File Offset: 0x00018824
		public static Pointer AllocateContext()
		{
			if (Allocator.HighUnit != Allocator.LowUnit)
			{
				return Allocator.HighUnit -= 12U;
			}
			if (Allocator.MemoryNodes[0].Available)
			{
				return Allocator.MemoryNodes[0].Remove();
			}
			return Allocator.AllocateUnitsRare(0U);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00019884 File Offset: 0x00018884
		public static Pointer ExpandUnits(Pointer oldPointer, uint oldUnitCount)
		{
			uint num = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(oldUnitCount - 1U))];
			uint num2 = (uint)Allocator.unitsToIndex[(int)((UIntPtr)oldUnitCount)];
			if (num == num2)
			{
				return oldPointer;
			}
			Pointer pointer = Allocator.AllocateUnits(oldUnitCount + 1U);
			if (pointer != Pointer.Zero)
			{
				Allocator.CopyUnits(pointer, oldPointer, oldUnitCount);
				Allocator.MemoryNodes[(int)((UIntPtr)num)].Insert(oldPointer, oldUnitCount);
			}
			return pointer;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000198E4 File Offset: 0x000188E4
		public static Pointer ShrinkUnits(Pointer oldPointer, uint oldUnitCount, uint newUnitCount)
		{
			uint num = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(oldUnitCount - 1U))];
			uint num2 = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(newUnitCount - 1U))];
			if (num == num2)
			{
				return oldPointer;
			}
			if (Allocator.MemoryNodes[(int)((UIntPtr)num2)].Available)
			{
				Pointer pointer = Allocator.MemoryNodes[(int)((UIntPtr)num2)].Remove();
				Allocator.CopyUnits(pointer, oldPointer, newUnitCount);
				Allocator.MemoryNodes[(int)((UIntPtr)num)].Insert(oldPointer, (uint)Allocator.indexToUnits[(int)((UIntPtr)num)]);
				return pointer;
			}
			Allocator.SplitBlock(oldPointer, num, num2);
			return oldPointer;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0001996C File Offset: 0x0001896C
		public static void FreeUnits(Pointer pointer, uint unitCount)
		{
			uint num = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(unitCount - 1U))];
			Allocator.MemoryNodes[(int)((UIntPtr)num)].Insert(pointer, (uint)Allocator.indexToUnits[(int)((UIntPtr)num)]);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000199A4 File Offset: 0x000189A4
		public static void SpecialFreeUnits(Pointer pointer)
		{
			if (pointer != Allocator.BaseUnit)
			{
				Allocator.MemoryNodes[0].Insert(pointer, 1U);
				return;
			}
			pointer.Stamp = uint.MaxValue;
			Allocator.BaseUnit += 12U;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000199F8 File Offset: 0x000189F8
		public static Pointer MoveUnitsUp(Pointer oldPointer, uint unitCount)
		{
			uint num = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(unitCount - 1U))];
			if (oldPointer > Allocator.BaseUnit + 16384 || oldPointer > Allocator.MemoryNodes[(int)((UIntPtr)num)].Next)
			{
				return oldPointer;
			}
			Pointer pointer = Allocator.MemoryNodes[(int)((UIntPtr)num)].Remove();
			Allocator.CopyUnits(pointer, oldPointer, unitCount);
			unitCount = (uint)Allocator.indexToUnits[(int)((UIntPtr)num)];
			if (oldPointer != Allocator.BaseUnit)
			{
				Allocator.MemoryNodes[(int)((UIntPtr)num)].Insert(oldPointer, unitCount);
			}
			else
			{
				Allocator.BaseUnit += unitCount * 12U;
			}
			return pointer;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00019AAC File Offset: 0x00018AAC
		public static void ExpandText()
		{
			uint[] array = new uint[38];
			for (;;)
			{
				MemoryNode memoryNode2;
				MemoryNode memoryNode = memoryNode2 = Allocator.BaseUnit;
				if (memoryNode2.Stamp != 4294967295U)
				{
					break;
				}
				Allocator.BaseUnit = memoryNode + memoryNode.UnitCount;
				array[(int)Allocator.unitsToIndex[(int)((UIntPtr)(memoryNode.UnitCount - 1U))]] += 1U;
				memoryNode.Stamp = 0U;
			}
			for (uint num = 0U; num < 38U; num += 1U)
			{
				MemoryNode memoryNode = Allocator.MemoryNodes[(int)((UIntPtr)num)];
				while (array[(int)((UIntPtr)num)] != 0U)
				{
					while (memoryNode.Next.Stamp == 0U)
					{
						memoryNode.Unlink();
						MemoryNode[] memoryNodes = Allocator.MemoryNodes;
						UIntPtr uintPtr = (UIntPtr)num;
						memoryNodes[(int)uintPtr].Stamp = memoryNodes[(int)uintPtr].Stamp - 1U;
						if ((array[(int)((UIntPtr)num)] -= 1U) == 0U)
						{
							break;
						}
					}
					memoryNode = memoryNode.Next;
				}
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00019B9C File Offset: 0x00018B9C
		private static Pointer AllocateUnitsRare(uint index)
		{
			if (Allocator.GlueCount == 0U)
			{
				Allocator.GlueFreeBlocks();
				if (Allocator.MemoryNodes[(int)((UIntPtr)index)].Available)
				{
					return Allocator.MemoryNodes[(int)((UIntPtr)index)].Remove();
				}
			}
			uint num = index;
			while ((num += 1U) != 38U)
			{
				if (Allocator.MemoryNodes[(int)((UIntPtr)num)].Available)
				{
					Pointer pointer = Allocator.MemoryNodes[(int)((UIntPtr)num)].Remove();
					Allocator.SplitBlock(pointer, num, index);
					return pointer;
				}
			}
			Allocator.GlueCount -= 1U;
			num = (uint)(Allocator.indexToUnits[(int)((UIntPtr)index)] * 12);
			if (Allocator.BaseUnit - Allocator.Text <= num)
			{
				return Pointer.Zero;
			}
			return Allocator.BaseUnit -= num;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00019C60 File Offset: 0x00018C60
		private static void SplitBlock(Pointer pointer, uint oldIndex, uint newIndex)
		{
			uint num = (uint)(Allocator.indexToUnits[(int)((UIntPtr)oldIndex)] - Allocator.indexToUnits[(int)((UIntPtr)newIndex)]);
			Pointer pointer2 = pointer + (uint)(Allocator.indexToUnits[(int)((UIntPtr)newIndex)] * 12);
			uint num2 = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(num - 1U))];
			if ((uint)Allocator.indexToUnits[(int)((UIntPtr)num2)] != num)
			{
				uint num3 = (uint)Allocator.indexToUnits[(int)((UIntPtr)(num2 -= 1U))];
				Allocator.MemoryNodes[(int)((UIntPtr)num2)].Insert(pointer2, num3);
				pointer2 += num3 * 12U;
				num -= num3;
			}
			Allocator.MemoryNodes[(int)Allocator.unitsToIndex[(int)((UIntPtr)(num - 1U))]].Insert(pointer2, num);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00019CFC File Offset: 0x00018CFC
		private static void GlueFreeBlocks()
		{
			MemoryNode memoryNode = new MemoryNode(4U);
			memoryNode.Stamp = 0U;
			memoryNode.Next = MemoryNode.Zero;
			memoryNode.UnitCount = 0U;
			if (Allocator.LowUnit != Allocator.HighUnit)
			{
				Allocator.LowUnit[0] = 0;
			}
			MemoryNode memoryNode2 = memoryNode;
			for (uint num = 0U; num < 38U; num += 1U)
			{
				while (Allocator.MemoryNodes[(int)((UIntPtr)num)].Available)
				{
					MemoryNode memoryNode3 = Allocator.MemoryNodes[(int)((UIntPtr)num)].Remove();
					if (memoryNode3.UnitCount != 0U)
					{
						for (;;)
						{
							MemoryNode memoryNode5;
							MemoryNode memoryNode4 = memoryNode5 = memoryNode3 + memoryNode3.UnitCount;
							if (memoryNode5.Stamp != 4294967295U)
							{
								break;
							}
							memoryNode3.UnitCount += memoryNode4.UnitCount;
							memoryNode4.UnitCount = 0U;
						}
						memoryNode2.Link(memoryNode3);
						memoryNode2 = memoryNode3;
					}
				}
			}
			while (memoryNode.Available)
			{
				MemoryNode memoryNode3 = memoryNode.Remove();
				uint num2 = memoryNode3.UnitCount;
				if (num2 != 0U)
				{
					while (num2 > 128U)
					{
						Allocator.MemoryNodes[(int)((UIntPtr)37)].Insert(memoryNode3, 128U);
						num2 -= 128U;
						memoryNode3 += 128;
					}
					uint num3 = (uint)Allocator.unitsToIndex[(int)((UIntPtr)(num2 - 1U))];
					if ((uint)Allocator.indexToUnits[(int)((UIntPtr)num3)] != num2)
					{
						uint num4 = num2 - (uint)Allocator.indexToUnits[(int)((UIntPtr)(num3 -= 1U))];
						Allocator.MemoryNodes[(int)((UIntPtr)(num4 - 1U))].Insert(memoryNode3 + (num2 - num4), num4);
					}
					Allocator.MemoryNodes[(int)((UIntPtr)num3)].Insert(memoryNode3, (uint)Allocator.indexToUnits[(int)((UIntPtr)num3)]);
				}
			}
			Allocator.GlueCount = 8192U;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00019EA8 File Offset: 0x00018EA8
		private static void CopyUnits(Pointer target, Pointer source, uint unitCount)
		{
			do
			{
				target[0] = source[0];
				target[1] = source[1];
				target[2] = source[2];
				target[3] = source[3];
				target[4] = source[4];
				target[5] = source[5];
				target[6] = source[6];
				target[7] = source[7];
				target[8] = source[8];
				target[9] = source[9];
				target[10] = source[10];
				target[11] = source[11];
				target += 12U;
				source += 12U;
			}
			while ((unitCount -= 1U) != 0U);
		}

		// Token: 0x04000214 RID: 532
		private const uint UnitSize = 12U;

		// Token: 0x04000215 RID: 533
		private const uint LocalOffset = 4U;

		// Token: 0x04000216 RID: 534
		private const uint NodeOffset = 16U;

		// Token: 0x04000217 RID: 535
		private const uint HeapOffset = 472U;

		// Token: 0x04000218 RID: 536
		private const uint N1 = 4U;

		// Token: 0x04000219 RID: 537
		private const uint N2 = 4U;

		// Token: 0x0400021A RID: 538
		private const uint N3 = 4U;

		// Token: 0x0400021B RID: 539
		private const uint N4 = 26U;

		// Token: 0x0400021C RID: 540
		private const uint IndexCount = 38U;

		// Token: 0x0400021D RID: 541
		private static readonly byte[] indexToUnits;

		// Token: 0x0400021E RID: 542
		private static readonly byte[] unitsToIndex;

		// Token: 0x0400021F RID: 543
		public static uint AllocatorSize;

		// Token: 0x04000220 RID: 544
		public static uint GlueCount;

		// Token: 0x04000221 RID: 545
		public static Pointer BaseUnit;

		// Token: 0x04000222 RID: 546
		public static Pointer LowUnit;

		// Token: 0x04000223 RID: 547
		public static Pointer HighUnit;

		// Token: 0x04000224 RID: 548
		public static Pointer Text;

		// Token: 0x04000225 RID: 549
		public static Pointer Heap;

		// Token: 0x04000226 RID: 550
		public static MemoryNode[] MemoryNodes = new MemoryNode[38];
	}
}

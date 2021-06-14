using System;
using System.IO;

namespace ComponentAce.Compression.Libs.PPMd
{
	// Token: 0x02000050 RID: 80
	internal class Model
	{
		// Token: 0x0600033B RID: 827 RVA: 0x0001AA60 File Offset: 0x00019A60
		static Model()
		{
			Model.numberStatisticsToBinarySummaryIndex[0] = 0;
			Model.numberStatisticsToBinarySummaryIndex[1] = 2;
			for (int i = 2; i < 11; i++)
			{
				Model.numberStatisticsToBinarySummaryIndex[i] = 4;
			}
			for (int j = 11; j < 256; j++)
			{
				Model.numberStatisticsToBinarySummaryIndex[j] = 6;
			}
			uint num = 1U;
			uint num2 = 1U;
			uint num3 = 5U;
			for (int k = 0; k < 5; k++)
			{
				Model.probabilities[k] = (byte)k;
			}
			for (int l = 5; l < 260; l++)
			{
				Model.probabilities[l] = (byte)num3;
				num -= 1U;
				if (num == 0U)
				{
					num2 += 1U;
					num = num2;
					num3 += 1U;
				}
			}
			Model.see2Contexts = new See2Context[24, 32];
			for (int m = 0; m < 24; m++)
			{
				for (int n = 0; n < 32; n++)
				{
					Model.see2Contexts[m, n] = new See2Context();
				}
			}
			Model.emptySee2Context = new See2Context();
			Model.emptySee2Context.Summary = 44943;
			Model.emptySee2Context.Shift = 172;
			Model.emptySee2Context.Count = 132;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001AC04 File Offset: 0x00019C04
		public static bool Encode(Stream target, Stream source, int modelOrder, ModelRestorationMethod modelRestorationMethod)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			Coder.RangeEncoderInitialize();
			Model.StartModel(modelOrder, modelRestorationMethod);
			for (;;)
			{
				Model.PpmContext suffix = Model.maximumContext;
				byte b = suffix.NumberStatistics;
				int symbol = source.ReadByte();
				if (b != 0)
				{
					suffix.EncodeSymbol1(symbol);
					Coder.RangeEncodeSymbol();
				}
				else
				{
					suffix.EncodeBinarySymbol(symbol);
					Coder.RangeShiftEncodeSymbol(14);
				}
				while (Model.foundState == PpmState.Zero)
				{
					Coder.RangeEncoderNormalize(target);
					do
					{
						Model.orderFall++;
						suffix = suffix.Suffix;
						if (suffix == Model.PpmContext.Zero)
						{
							goto IL_103;
						}
					}
					while (suffix.NumberStatistics == Model.numberMasked);
					suffix.EncodeSymbol2(symbol);
					Coder.RangeEncodeSymbol();
				}
				if (Model.orderFall == 0 && Model.foundState.Successor >= Allocator.BaseUnit)
				{
					Model.maximumContext = Model.foundState.Successor;
				}
				else
				{
					Model.UpdateModel(suffix);
					if (Model.escapeCount == 0)
					{
						Model.ClearMask();
					}
				}
				Coder.RangeEncoderNormalize(target);
			}
			IL_103:
			Coder.RangeEncoderFlush(target);
			return true;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0001AD1B File Offset: 0x00019D1B
		public static void StartEncoding(ModelRestorationMethod method, int modelOrder)
		{
			Model.modelOrder = modelOrder;
			Model.modelRestorationMethod = method;
			Coder.RangeEncoderInitialize();
			Model.StartModel(modelOrder, Model.modelRestorationMethod);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001AD3C File Offset: 0x00019D3C
		public static bool EncodeBuffer(Stream target, Stream source, bool isLastBuffer)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			for (;;)
			{
				Model.PpmContext suffix = Model.maximumContext;
				byte b = suffix.NumberStatistics;
				int num = source.ReadByte();
				if (num == -1 && !isLastBuffer)
				{
					break;
				}
				if (b != 0)
				{
					suffix.EncodeSymbol1(num);
					Coder.RangeEncodeSymbol();
				}
				else
				{
					suffix.EncodeBinarySymbol(num);
					Coder.RangeShiftEncodeSymbol(14);
				}
				while (Model.foundState == PpmState.Zero)
				{
					Coder.RangeEncoderNormalize(target);
					do
					{
						Model.orderFall++;
						suffix = suffix.Suffix;
						if (suffix == Model.PpmContext.Zero)
						{
							return true;
						}
					}
					while (suffix.NumberStatistics == Model.numberMasked);
					suffix.EncodeSymbol2(num);
					Coder.RangeEncodeSymbol();
				}
				if (Model.orderFall == 0 && Model.foundState.Successor >= Allocator.BaseUnit)
				{
					Model.maximumContext = Model.foundState.Successor;
				}
				else
				{
					Model.UpdateModel(suffix);
					if (Model.escapeCount == 0)
					{
						Model.ClearMask();
					}
				}
				Coder.RangeEncoderNormalize(target);
			}
			return true;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001AE4A File Offset: 0x00019E4A
		public static void StopEncoding(Stream target)
		{
			Coder.RangeEncoderFlush(target);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0001AE52 File Offset: 0x00019E52
		public static void StartDecoding(int modelOrder, ModelRestorationMethod modelRestorationMethod)
		{
			Model.modelOrder = modelOrder;
			Model.modelRestorationMethod = modelRestorationMethod;
			Model.firstRun = true;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0001AE66 File Offset: 0x00019E66
		public static byte EntryPoint
		{
			get
			{
				return Model.entryPoint;
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0001AE70 File Offset: 0x00019E70
		public static bool DecodeBuffer(Stream target, Stream source, bool isLastBuffer)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (Model.firstRun && Model.entryPoint == 0)
			{
				Coder.RangeDecoderInitialize(source);
				Model.StartModel(Model.modelOrder, Model.modelRestorationMethod);
				Model.minimumContext = Model.maximumContext;
				Model.numberStatistics = Model.minimumContext.NumberStatistics;
				Model.firstRun = false;
			}
			for (;;)
			{
				if (Model.entryPoint == 0)
				{
					if (Model.numberStatistics != 0)
					{
						Model.minimumContext.DecodeSymbol1();
					}
					else
					{
						Model.minimumContext.DecodeBinarySymbol();
					}
					Coder.RangeRemoveSubrange();
				}
				while ((Model.foundState == PpmState.Zero && Model.entryPoint != 2) || Model.entryPoint == 1)
				{
					Model.entryPoint = 0;
					if (isLastBuffer)
					{
						Coder.RangeDecoderNormalize(source);
					}
					else if (!Coder.SafeRangeDecoderNormalize(source))
					{
						goto Block_8;
					}
					do
					{
						Model.orderFall++;
						Model.minimumContext = Model.minimumContext.Suffix;
						if (Model.minimumContext == Model.PpmContext.Zero)
						{
							goto Block_9;
						}
					}
					while (Model.minimumContext.NumberStatistics == Model.numberMasked);
					Model.minimumContext.DecodeSymbol2();
					Coder.RangeRemoveSubrange();
				}
				if (Model.entryPoint != 2)
				{
					target.WriteByte(Model.foundState.Symbol);
					if (Model.orderFall == 0 && Model.foundState.Successor >= Allocator.BaseUnit)
					{
						Model.maximumContext = Model.foundState.Successor;
					}
					else
					{
						Model.UpdateModel(Model.minimumContext);
						if (Model.escapeCount == 0)
						{
							Model.ClearMask();
						}
					}
					Model.minimumContext = Model.maximumContext;
					Model.numberStatistics = Model.minimumContext.NumberStatistics;
				}
				Model.entryPoint = 0;
				if (isLastBuffer)
				{
					Coder.RangeDecoderNormalize(source);
				}
				else if (!Coder.SafeRangeDecoderNormalize(source))
				{
					goto Block_18;
				}
			}
			Block_8:
			Model.entryPoint = 1;
			return true;
			Block_9:
			Model.entryPoint = 0;
			return true;
			Block_18:
			Model.entryPoint = 2;
			return true;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001B048 File Offset: 0x0001A048
		public static bool Decode(Stream target, Stream source, int modelOrder, ModelRestorationMethod modelRestorationMethod)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			Coder.RangeDecoderInitialize(source);
			Model.StartModel(modelOrder, modelRestorationMethod);
			Model.PpmContext suffix = Model.maximumContext;
			byte b = suffix.NumberStatistics;
			for (;;)
			{
				if (b != 0)
				{
					suffix.DecodeSymbol1();
				}
				else
				{
					suffix.DecodeBinarySymbol();
				}
				Coder.RangeRemoveSubrange();
				while (Model.foundState == PpmState.Zero)
				{
					Coder.RangeDecoderNormalize(source);
					do
					{
						Model.orderFall++;
						suffix = suffix.Suffix;
						if (suffix == Model.PpmContext.Zero)
						{
							return true;
						}
					}
					while (suffix.NumberStatistics == Model.numberMasked);
					suffix.DecodeSymbol2();
					Coder.RangeRemoveSubrange();
				}
				target.WriteByte(Model.foundState.Symbol);
				if (Model.orderFall == 0 && Model.foundState.Successor >= Allocator.BaseUnit)
				{
					Model.maximumContext = Model.foundState.Successor;
				}
				else
				{
					Model.UpdateModel(suffix);
					if (Model.escapeCount == 0)
					{
						Model.ClearMask();
					}
				}
				suffix = Model.maximumContext;
				b = suffix.NumberStatistics;
				Coder.RangeDecoderNormalize(source);
			}
			return true;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0001B16C File Offset: 0x0001A16C
		private static void StartModel(int modelOrder, ModelRestorationMethod modelRestorationMethod)
		{
			Array.Clear(Model.characterMask, 0, Model.characterMask.Length);
			Model.escapeCount = 1;
			if (modelOrder < 2)
			{
				Model.orderFall = Model.modelOrder;
				Model.PpmContext suffix = Model.maximumContext;
				while (suffix.Suffix != Model.PpmContext.Zero)
				{
					Model.orderFall--;
					suffix = suffix.Suffix;
				}
				return;
			}
			Model.modelOrder = modelOrder;
			Model.orderFall = modelOrder;
			Model.method = modelRestorationMethod;
			Allocator.Initialize();
			Model.initialRunLength = -((modelOrder < 12) ? modelOrder : 12) - 1;
			Model.runLength = Model.initialRunLength;
			Model.maximumContext = Allocator.AllocateContext();
			Model.maximumContext.Suffix = Model.PpmContext.Zero;
			Model.maximumContext.NumberStatistics = byte.MaxValue;
			Model.maximumContext.SummaryFrequency = (ushort)(Model.maximumContext.NumberStatistics + 2);
			Model.maximumContext.Statistics = Allocator.AllocateUnits(128U);
			Model.previousSuccess = 0;
			for (int i = 0; i < 256; i++)
			{
				PpmState ppmState = Model.maximumContext.Statistics[i];
				ppmState.Symbol = (byte)i;
				ppmState.Frequency = 1;
				ppmState.Successor = Model.PpmContext.Zero;
			}
			uint num = 0U;
			int num2 = 0;
			while (num < 25U)
			{
				while ((uint)Model.probabilities[num2] == num)
				{
					num2++;
				}
				for (int j = 0; j < 8; j++)
				{
					checked(Model.binarySummary[(int)((IntPtr)(unchecked((ulong)num))), (int)((IntPtr)(unchecked((long)j)))]) = (ushort)(16384L - (long)((int)Model.InitialBinaryEscapes[j] / (num2 + 1)));
				}
				for (int k = 8; k < 64; k += 8)
				{
					for (int l = 0; l < 8; l++)
					{
						checked
						{
							Model.binarySummary[(int)((IntPtr)(unchecked((ulong)num))), (int)((IntPtr)(unchecked((long)(k + l))))] = Model.binarySummary[(int)((IntPtr)(unchecked((ulong)num))), (int)((IntPtr)(unchecked((long)l)))];
						}
					}
				}
				num += 1U;
			}
			num = 0U;
			uint num3 = 0U;
			while (num < 24U)
			{
				while ((uint)Model.probabilities[(int)((UIntPtr)(num3 + 3U))] == num + 3U)
				{
					num3 += 1U;
				}
				for (int m = 0; m < 32; m++)
				{
					checked(Model.see2Contexts[(int)((IntPtr)(unchecked((ulong)num))), (int)((IntPtr)(unchecked((long)m)))]).Initialize(2U * num3 + 5U);
				}
				num += 1U;
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0001B3A0 File Offset: 0x0001A3A0
		private static void UpdateModel(Model.PpmContext minimumContext)
		{
			PpmState ppmState = PpmState.Zero;
			Model.PpmContext suffix = Model.maximumContext;
			uint frequency = (uint)Model.foundState.Frequency;
			byte symbol = Model.foundState.Symbol;
			Model.PpmContext ppmContext = Model.foundState.Successor;
			Model.PpmContext suffix2 = minimumContext.Suffix;
			if (frequency < 31U && suffix2 != Model.PpmContext.Zero)
			{
				if (suffix2.NumberStatistics != 0)
				{
					ppmState = suffix2.Statistics;
					if (ppmState.Symbol != symbol)
					{
						byte symbol2;
						do
						{
							symbol2 = ppmState[1].Symbol;
							ppmState = ++ppmState;
						}
						while (symbol2 != symbol);
						if (ppmState[0].Frequency >= ppmState[-1].Frequency)
						{
							Model.Swap(ppmState[0], ppmState[-1]);
							ppmState = --ppmState;
						}
					}
					uint num = (ppmState.Frequency < 115) ? 2U : 0U;
					ppmState.Frequency += (byte)num;
					suffix2.SummaryFrequency += (ushort)((byte)num);
				}
				else
				{
					ppmState = suffix2.FirstState;
					ppmState.Frequency += ((ppmState.Frequency < 32) ? 1 : 0);
				}
			}
			if (Model.orderFall == 0 && ppmContext != Model.PpmContext.Zero)
			{
				Model.foundState.Successor = Model.CreateSuccessors(true, ppmState, minimumContext);
				if (!(Model.foundState.Successor == Model.PpmContext.Zero))
				{
					Model.maximumContext = Model.foundState.Successor;
					return;
				}
			}
			else
			{
				Allocator.Text[0] = symbol;
				Allocator.Text = ++Allocator.Text;
				Model.PpmContext successor = Allocator.Text;
				if (!(Allocator.Text >= Allocator.BaseUnit))
				{
					if (ppmContext != Model.PpmContext.Zero)
					{
						if (ppmContext < Allocator.BaseUnit)
						{
							ppmContext = Model.CreateSuccessors(false, ppmState, minimumContext);
						}
					}
					else
					{
						ppmContext = Model.ReduceOrder(ppmState, minimumContext);
					}
					if (!(ppmContext == Model.PpmContext.Zero))
					{
						if (--Model.orderFall == 0)
						{
							successor = ppmContext;
							Allocator.Text -= ((Model.maximumContext != minimumContext) ? 1 : 0);
						}
						else if (Model.method > ModelRestorationMethod.Freeze)
						{
							successor = ppmContext;
							Allocator.Text = Allocator.Heap;
							Model.orderFall = 0;
						}
						uint num2 = (uint)minimumContext.NumberStatistics;
						uint num3 = (uint)minimumContext.SummaryFrequency - num2 - frequency;
						byte b = (symbol >= 64) ? 8 : 0;
						while (suffix != minimumContext)
						{
							uint num4 = (uint)suffix.NumberStatistics;
							if (num4 != 0U)
							{
								if ((num4 & 1U) != 0U)
								{
									ppmState = Allocator.ExpandUnits(suffix.Statistics, num4 + 1U >> 1);
									if (ppmState == PpmState.Zero)
									{
										goto IL_458;
									}
									suffix.Statistics = ppmState;
								}
								suffix.SummaryFrequency += ((3U * num4 + 1U < num2) ? 1 : 0);
							}
							else
							{
								ppmState = Allocator.AllocateUnits(1U);
								if (ppmState == PpmState.Zero)
								{
									goto IL_458;
								}
								Model.Copy(ppmState, suffix.FirstState);
								suffix.Statistics = ppmState;
								if (ppmState.Frequency < 30)
								{
									ppmState.Frequency += ppmState.Frequency;
								}
								else
								{
									ppmState.Frequency = 120;
								}
								suffix.SummaryFrequency = (ushort)((int)ppmState.Frequency + Model.initialEscape + ((num2 > 2U) ? 1 : 0));
							}
							uint num = (uint)((ulong)(2U * frequency) * (ulong)((long)(suffix.SummaryFrequency + 6)));
							uint num5 = num3 + (uint)suffix.SummaryFrequency;
							if (num < 6U * num5)
							{
								num = 1U + ((num > num5) ? 1U : 0U) + ((num >= 4U * num5) ? 1U : 0U);
								suffix.SummaryFrequency += 4;
							}
							else
							{
								num = 4U + ((num > 9U * num5) ? 1U : 0U) + ((num > 12U * num5) ? 1U : 0U) + ((num > 15U * num5) ? 1U : 0U);
								suffix.SummaryFrequency += (ushort)num;
							}
							ppmState = suffix.Statistics + (int)(suffix.NumberStatistics += 1);
							ppmState.Successor = successor;
							ppmState.Symbol = symbol;
							ppmState.Frequency = (byte)num;
							suffix.Flags |= b;
							suffix = suffix.Suffix;
						}
						Model.maximumContext = ppmContext;
						return;
					}
				}
			}
			IL_458:
			Model.RestoreModel(suffix, minimumContext, ppmContext);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001B810 File Offset: 0x0001A810
		private static Model.PpmContext CreateSuccessors(bool skip, PpmState state, Model.PpmContext context)
		{
			Model.PpmContext successor = Model.foundState.Successor;
			PpmState[] array = new PpmState[16];
			uint num = 0U;
			byte symbol = Model.foundState.Symbol;
			if (!skip)
			{
				array[(int)((UIntPtr)(num++))] = Model.foundState;
				if (context.Suffix == Model.PpmContext.Zero)
				{
					goto IL_173;
				}
			}
			bool flag = false;
			if (state != PpmState.Zero)
			{
				context = context.Suffix;
				flag = true;
			}
			for (;;)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					context = context.Suffix;
					if (context.NumberStatistics != 0)
					{
						state = context.Statistics;
						byte b;
						if (state.Symbol != symbol)
						{
							do
							{
								b = state[1].Symbol;
								state = ++state;
							}
							while (b != symbol);
						}
						b = ((state.Frequency < 115) ? 1 : 0);
						state.Frequency += b;
						context.SummaryFrequency += (ushort)b;
					}
					else
					{
						state = context.FirstState;
						state.Frequency += (byte)(((context.Suffix.NumberStatistics == 0) ? 1 : 0) & ((state.Frequency < 24) ? 1 : 0));
					}
				}
				if (state.Successor != successor)
				{
					break;
				}
				array[(int)((UIntPtr)(num++))] = state;
				if (!(context.Suffix != Model.PpmContext.Zero))
				{
					goto IL_173;
				}
			}
			context = state.Successor;
			IL_173:
			if (num == 0U)
			{
				return context;
			}
			byte b2 = 0;
			byte b3 = (symbol >= 64) ? 16 : 0;
			symbol = successor.NumberStatistics;
			byte firstStateSymbol = symbol;
			Model.PpmContext firstStateSuccessor = successor + 1;
			b3 |= ((symbol >= 64) ? 8 : 0);
			byte firstStateFrequency;
			if (context.NumberStatistics != 0)
			{
				state = context.Statistics;
				if (state.Symbol != symbol)
				{
					byte symbol2;
					do
					{
						symbol2 = state[1].Symbol;
						state = ++state;
					}
					while (symbol2 != symbol);
				}
				uint num2 = (uint)(state.Frequency - 1);
				uint num3 = (uint)((long)(context.SummaryFrequency - (ushort)context.NumberStatistics) - (long)((ulong)num2));
				firstStateFrequency = (byte)(1U + ((2U * num2 <= num3) ? ((5U * num2 > num3) ? 1U : 0U) : ((num2 + 2U * num3 - 3U) / num3)));
			}
			else
			{
				firstStateFrequency = context.FirstStateFrequency;
			}
			for (;;)
			{
				Model.PpmContext ppmContext = Allocator.AllocateContext();
				if (ppmContext == Model.PpmContext.Zero)
				{
					break;
				}
				ppmContext.NumberStatistics = b2;
				ppmContext.Flags = b3;
				ppmContext.FirstStateSymbol = firstStateSymbol;
				ppmContext.FirstStateFrequency = firstStateFrequency;
				ppmContext.FirstStateSuccessor = firstStateSuccessor;
				ppmContext.Suffix = context;
				context = ppmContext;
				array[(int)((UIntPtr)(num -= 1U))].Successor = context;
				if (num == 0U)
				{
					return context;
				}
			}
			return Model.PpmContext.Zero;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001BAD4 File Offset: 0x0001AAD4
		private static Model.PpmContext ReduceOrder(PpmState state, Model.PpmContext context)
		{
			PpmState[] array = new PpmState[16];
			uint num = 0U;
			Model.PpmContext context2 = context;
			Model.PpmContext ppmContext = Allocator.Text;
			byte symbol = Model.foundState.Symbol;
			array[(int)((UIntPtr)(num++))] = Model.foundState;
			Model.foundState.Successor = ppmContext;
			Model.orderFall++;
			bool flag = false;
			if (state != PpmState.Zero)
			{
				context = context.Suffix;
				flag = true;
			}
			for (;;)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					if (context.Suffix == Model.PpmContext.Zero)
					{
						break;
					}
					context = context.Suffix;
					if (context.NumberStatistics != 0)
					{
						state = context.Statistics;
						byte b;
						if (state.Symbol != symbol)
						{
							do
							{
								b = state[1].Symbol;
								state = ++state;
							}
							while (b != symbol);
						}
						b = ((state.Frequency < 115) ? 2 : 0);
						state.Frequency += b;
						context.SummaryFrequency += (ushort)b;
					}
					else
					{
						state = context.FirstState;
						state.Frequency += ((state.Frequency < 32) ? 1 : 0);
					}
				}
				if (state.Successor != Model.PpmContext.Zero)
				{
					goto IL_1A6;
				}
				array[(int)((UIntPtr)(num++))] = state;
				state.Successor = ppmContext;
				Model.orderFall++;
			}
			if (Model.method > ModelRestorationMethod.Freeze)
			{
				do
				{
					array[(int)((UIntPtr)(num -= 1U))].Successor = context;
				}
				while (num != 0U);
				Allocator.Text = Allocator.Heap + 1;
				Model.orderFall = 1;
			}
			return context;
			IL_1A6:
			if (Model.method > ModelRestorationMethod.Freeze)
			{
				context = state.Successor;
				do
				{
					array[(int)((UIntPtr)(num -= 1U))].Successor = context;
				}
				while (num != 0U);
				Allocator.Text = Allocator.Heap + 1;
				Model.orderFall = 1;
				return context;
			}
			if (state.Successor <= ppmContext)
			{
				PpmState ppmState = Model.foundState;
				Model.foundState = state;
				state.Successor = Model.CreateSuccessors(false, PpmState.Zero, context);
				Model.foundState = ppmState;
			}
			if (Model.orderFall == 1 && context2 == Model.maximumContext)
			{
				Model.foundState.Successor = state.Successor;
				Allocator.Text = --Allocator.Text;
			}
			return state.Successor;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0001BD38 File Offset: 0x0001AD38
		private static void RestoreModel(Model.PpmContext context, Model.PpmContext minimumContext, Model.PpmContext foundStateSuccessor)
		{
			Allocator.Text = Allocator.Heap;
			Model.PpmContext suffix = Model.maximumContext;
			while (suffix != context)
			{
				if ((suffix.NumberStatistics -= 1) == 0)
				{
					suffix.Flags = (suffix.Flags & 16) + ((suffix.Statistics.Symbol >= 64) ? 8 : 0);
					PpmState statistics = suffix.Statistics;
					Model.Copy(suffix.FirstState, statistics);
					Allocator.SpecialFreeUnits(statistics);
					suffix.FirstStateFrequency = (byte)(suffix.FirstStateFrequency + 11 >> 3);
				}
				else
				{
					suffix.Refresh((uint)(suffix.NumberStatistics + 3 >> 1), false);
				}
				suffix = suffix.Suffix;
			}
			while (suffix != minimumContext)
			{
				if (suffix.NumberStatistics == 0)
				{
					suffix.FirstStateFrequency -= (byte)(suffix.FirstStateFrequency >> 1);
				}
				else if ((suffix.SummaryFrequency += 4) > (ushort)(128 + 4 * suffix.NumberStatistics))
				{
					suffix.Refresh((uint)(suffix.NumberStatistics + 2 >> 1), true);
				}
				suffix = suffix.Suffix;
			}
			if (Model.method > ModelRestorationMethod.Freeze)
			{
				Model.maximumContext = foundStateSuccessor;
				Allocator.GlueCount += (((Allocator.MemoryNodes[1].Stamp & 1U) == 0U) ? 1U : 0U);
				return;
			}
			if (Model.method == ModelRestorationMethod.Freeze)
			{
				while (Model.maximumContext.Suffix != Model.PpmContext.Zero)
				{
					Model.maximumContext = Model.maximumContext.Suffix;
				}
				Model.maximumContext.RemoveBinaryContexts(0);
				Model.method += 1;
				Allocator.GlueCount = 0U;
				Model.orderFall = Model.modelOrder;
				return;
			}
			if (Model.method == ModelRestorationMethod.Restart || Allocator.GetMemoryUsed() < Allocator.AllocatorSize >> 1)
			{
				Model.StartModel(Model.modelOrder, Model.method);
				Model.escapeCount = 0;
				return;
			}
			while (Model.maximumContext.Suffix != Model.PpmContext.Zero)
			{
				Model.maximumContext = Model.maximumContext.Suffix;
			}
			do
			{
				Model.maximumContext.CutOff(0);
				Allocator.ExpandText();
			}
			while (Allocator.GetMemoryUsed() > 3U * (Allocator.AllocatorSize >> 2));
			Allocator.GlueCount = 0U;
			Model.orderFall = Model.modelOrder;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001BF74 File Offset: 0x0001AF74
		private static void Swap(PpmState state1, PpmState state2)
		{
			byte symbol = state1.Symbol;
			byte frequency = state1.Frequency;
			Model.PpmContext successor = state1.Successor;
			state1.Symbol = state2.Symbol;
			state1.Frequency = state2.Frequency;
			state1.Successor = state2.Successor;
			state2.Symbol = symbol;
			state2.Frequency = frequency;
			state2.Successor = successor;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0001BFDB File Offset: 0x0001AFDB
		private static void Copy(PpmState state1, PpmState state2)
		{
			state1.Symbol = state2.Symbol;
			state1.Frequency = state2.Frequency;
			state1.Successor = state2.Successor;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001C007 File Offset: 0x0001B007
		private static int Mean(int sum, int shift, int round)
		{
			return sum + (1 << shift - round) >> shift;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001C018 File Offset: 0x0001B018
		private static void ClearMask()
		{
			Model.escapeCount = 1;
			Array.Clear(Model.characterMask, 0, Model.characterMask.Length);
		}

		// Token: 0x04000233 RID: 563
		public const uint Signature = 2225909647U;

		// Token: 0x04000234 RID: 564
		public const char Variant = 'I';

		// Token: 0x04000235 RID: 565
		public const int MaximumOrder = 16;

		// Token: 0x04000236 RID: 566
		private const byte UpperFrequency = 5;

		// Token: 0x04000237 RID: 567
		private const byte IntervalBitCount = 7;

		// Token: 0x04000238 RID: 568
		private const byte PeriodBitCount = 7;

		// Token: 0x04000239 RID: 569
		private const byte TotalBitCount = 14;

		// Token: 0x0400023A RID: 570
		private const uint Interval = 128U;

		// Token: 0x0400023B RID: 571
		private const uint BinaryScale = 16384U;

		// Token: 0x0400023C RID: 572
		private const uint MaximumFrequency = 124U;

		// Token: 0x0400023D RID: 573
		private const uint OrderBound = 9U;

		// Token: 0x0400023E RID: 574
		private static See2Context[,] see2Contexts;

		// Token: 0x0400023F RID: 575
		private static See2Context emptySee2Context;

		// Token: 0x04000240 RID: 576
		private static Model.PpmContext maximumContext;

		// Token: 0x04000241 RID: 577
		private static ushort[,] binarySummary = new ushort[25, 64];

		// Token: 0x04000242 RID: 578
		private static byte[] numberStatisticsToBinarySummaryIndex = new byte[256];

		// Token: 0x04000243 RID: 579
		private static byte[] probabilities = new byte[260];

		// Token: 0x04000244 RID: 580
		private static byte[] characterMask = new byte[256];

		// Token: 0x04000245 RID: 581
		private static byte escapeCount;

		// Token: 0x04000246 RID: 582
		private static int modelOrder;

		// Token: 0x04000247 RID: 583
		private static int orderFall;

		// Token: 0x04000248 RID: 584
		private static int initialEscape;

		// Token: 0x04000249 RID: 585
		private static int initialRunLength;

		// Token: 0x0400024A RID: 586
		private static int runLength;

		// Token: 0x0400024B RID: 587
		private static byte previousSuccess;

		// Token: 0x0400024C RID: 588
		private static byte numberMasked;

		// Token: 0x0400024D RID: 589
		private static ModelRestorationMethod method;

		// Token: 0x0400024E RID: 590
		private static PpmState foundState;

		// Token: 0x0400024F RID: 591
		private static readonly ushort[] InitialBinaryEscapes = new ushort[]
		{
			15581,
			7999,
			22975,
			18675,
			25761,
			23228,
			26162,
			24657
		};

		// Token: 0x04000250 RID: 592
		private static readonly byte[] ExponentialEscapes = new byte[]
		{
			25,
			14,
			9,
			7,
			5,
			5,
			4,
			4,
			4,
			3,
			3,
			3,
			2,
			2,
			2,
			2
		};

		// Token: 0x04000251 RID: 593
		private static readonly uint[] crcTable = new uint[]
		{
			0U,
			1996959894U,
			3993919788U,
			2567524794U,
			124634137U,
			1886057615U,
			3915621685U,
			2657392035U,
			249268274U,
			2044508324U,
			3772115230U,
			2547177864U,
			162941995U,
			2125561021U,
			3887607047U,
			2428444049U,
			498536548U,
			1789927666U,
			4089016648U,
			2227061214U,
			450548861U,
			1843258603U,
			4107580753U,
			2211677639U,
			325883990U,
			1684777152U,
			4251122042U,
			2321926636U,
			335633487U,
			1661365465U,
			4195302755U,
			2366115317U,
			997073096U,
			1281953886U,
			3579855332U,
			2724688242U,
			1006888145U,
			1258607687U,
			3524101629U,
			2768942443U,
			901097722U,
			1119000684U,
			3686517206U,
			2898065728U,
			853044451U,
			1172266101U,
			3705015759U,
			2882616665U,
			651767980U,
			1373503546U,
			3369554304U,
			3218104598U,
			565507253U,
			1454621731U,
			3485111705U,
			3099436303U,
			671266974U,
			1594198024U,
			3322730930U,
			2970347812U,
			795835527U,
			1483230225U,
			3244367275U,
			3060149565U,
			1994146192U,
			31158534U,
			2563907772U,
			4023717930U,
			1907459465U,
			112637215U,
			2680153253U,
			3904427059U,
			2013776290U,
			251722036U,
			2517215374U,
			3775830040U,
			2137656763U,
			141376813U,
			2439277719U,
			3865271297U,
			1802195444U,
			476864866U,
			2238001368U,
			4066508878U,
			1812370925U,
			453092731U,
			2181625025U,
			4111451223U,
			1706088902U,
			314042704U,
			2344532202U,
			4240017532U,
			1658658271U,
			366619977U,
			2362670323U,
			4224994405U,
			1303535960U,
			984961486U,
			2747007092U,
			3569037538U,
			1256170817U,
			1037604311U,
			2765210733U,
			3554079995U,
			1131014506U,
			879679996U,
			2909243462U,
			3663771856U,
			1141124467U,
			855842277U,
			2852801631U,
			3708648649U,
			1342533948U,
			654459306U,
			3188396048U,
			3373015174U,
			1466479909U,
			544179635U,
			3110523913U,
			3462522015U,
			1591671054U,
			702138776U,
			2966460450U,
			3352799412U,
			1504918807U,
			783551873U,
			3082640443U,
			3233442989U,
			3988292384U,
			2596254646U,
			62317068U,
			1957810842U,
			3939845945U,
			2647816111U,
			81470997U,
			1943803523U,
			3814918930U,
			2489596804U,
			225274430U,
			2053790376U,
			3826175755U,
			2466906013U,
			167816743U,
			2097651377U,
			4027552580U,
			2265490386U,
			503444072U,
			1762050814U,
			4150417245U,
			2154129355U,
			426522225U,
			1852507879U,
			4275313526U,
			2312317920U,
			282753626U,
			1742555852U,
			4189708143U,
			2394877945U,
			397917763U,
			1622183637U,
			3604390888U,
			2714866558U,
			953729732U,
			1340076626U,
			3518719985U,
			2797360999U,
			1068828381U,
			1219638859U,
			3624741850U,
			2936675148U,
			906185462U,
			1090812512U,
			3747672003U,
			2825379669U,
			829329135U,
			1181335161U,
			3412177804U,
			3160834842U,
			628085408U,
			1382605366U,
			3423369109U,
			3138078467U,
			570562233U,
			1426400815U,
			3317316542U,
			2998733608U,
			733239954U,
			1555261956U,
			3268935591U,
			3050360625U,
			752459403U,
			1541320221U,
			2607071920U,
			3965973030U,
			1969922972U,
			40735498U,
			2617837225U,
			3943577151U,
			1913087877U,
			83908371U,
			2512341634U,
			3803740692U,
			2075208622U,
			213261112U,
			2463272603U,
			3855990285U,
			2094854071U,
			198958881U,
			2262029012U,
			4057260610U,
			1759359992U,
			534414190U,
			2176718541U,
			4139329115U,
			1873836001U,
			414664567U,
			2282248934U,
			4279200368U,
			1711684554U,
			285281116U,
			2405801727U,
			4167216745U,
			1634467795U,
			376229701U,
			2685067896U,
			3608007406U,
			1308918612U,
			956543938U,
			2808555105U,
			3495958263U,
			1231636301U,
			1047427035U,
			2932959818U,
			3654703836U,
			1088359270U,
			936918000U,
			2847714899U,
			3736837829U,
			1202900863U,
			817233897U,
			3183342108U,
			3401237130U,
			1404277552U,
			615818150U,
			3134207493U,
			3453421203U,
			1423857449U,
			601450431U,
			3009837614U,
			3294710456U,
			1567103746U,
			711928724U,
			3020668471U,
			3272380065U,
			1510334235U,
			755167117U
		};

		// Token: 0x04000252 RID: 594
		private static ModelRestorationMethod modelRestorationMethod;

		// Token: 0x04000253 RID: 595
		private static bool firstRun;

		// Token: 0x04000254 RID: 596
		private static Model.PpmContext minimumContext;

		// Token: 0x04000255 RID: 597
		private static byte numberStatistics;

		// Token: 0x04000256 RID: 598
		private static byte entryPoint = 0;

		// Token: 0x02000051 RID: 81
		internal struct PpmContext
		{
			// Token: 0x0600034E RID: 846 RVA: 0x0001C03A File Offset: 0x0001B03A
			public PpmContext(uint address)
			{
				this.Address = address;
			}

			// Token: 0x17000077 RID: 119
			// (get) Token: 0x0600034F RID: 847 RVA: 0x0001C043 File Offset: 0x0001B043
			// (set) Token: 0x06000350 RID: 848 RVA: 0x0001C052 File Offset: 0x0001B052
			public byte NumberStatistics
			{
				get
				{
					return Model.PpmContext.Memory[(int)((UIntPtr)this.Address)];
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)this.Address)] = value;
				}
			}

			// Token: 0x17000078 RID: 120
			// (get) Token: 0x06000351 RID: 849 RVA: 0x0001C062 File Offset: 0x0001B062
			// (set) Token: 0x06000352 RID: 850 RVA: 0x0001C073 File Offset: 0x0001B073
			public byte Flags
			{
				get
				{
					return Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 1U))];
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 1U))] = value;
				}
			}

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x06000353 RID: 851 RVA: 0x0001C085 File Offset: 0x0001B085
			// (set) Token: 0x06000354 RID: 852 RVA: 0x0001C0A9 File Offset: 0x0001B0A9
			public ushort SummaryFrequency
			{
				get
				{
					return (ushort)((int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 2U))] | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 3U))] << 8);
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 2U))] = (byte)value;
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 3U))] = (byte)(value >> 8);
				}
			}

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x06000355 RID: 853 RVA: 0x0001C0D0 File Offset: 0x0001B0D0
			// (set) Token: 0x06000356 RID: 854 RVA: 0x0001C12C File Offset: 0x0001B12C
			public PpmState Statistics
			{
				get
				{
					return new PpmState((uint)((int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 4U))] | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 5U))] << 8 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 6U))] << 16 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 7U))] << 24));
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 4U))] = (byte)value.Address;
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 5U))] = (byte)(value.Address >> 8);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 6U))] = (byte)(value.Address >> 16);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 7U))] = (byte)(value.Address >> 24);
				}
			}

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000357 RID: 855 RVA: 0x0001C1A0 File Offset: 0x0001B1A0
			// (set) Token: 0x06000358 RID: 856 RVA: 0x0001C1FC File Offset: 0x0001B1FC
			public Model.PpmContext Suffix
			{
				get
				{
					return new Model.PpmContext((uint)((int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 8U))] | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 9U))] << 8 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 10U))] << 16 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 11U))] << 24));
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 8U))] = (byte)value.Address;
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 9U))] = (byte)(value.Address >> 8);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 10U))] = (byte)(value.Address >> 16);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 11U))] = (byte)(value.Address >> 24);
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x06000359 RID: 857 RVA: 0x0001C270 File Offset: 0x0001B270
			public PpmState FirstState
			{
				get
				{
					return new PpmState(this.Address + 2U);
				}
			}

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x0600035A RID: 858 RVA: 0x0001C27F File Offset: 0x0001B27F
			// (set) Token: 0x0600035B RID: 859 RVA: 0x0001C290 File Offset: 0x0001B290
			public byte FirstStateSymbol
			{
				get
				{
					return Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 2U))];
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 2U))] = value;
				}
			}

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x0600035C RID: 860 RVA: 0x0001C2A2 File Offset: 0x0001B2A2
			// (set) Token: 0x0600035D RID: 861 RVA: 0x0001C2B3 File Offset: 0x0001B2B3
			public byte FirstStateFrequency
			{
				get
				{
					return Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 3U))];
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 3U))] = value;
				}
			}

			// Token: 0x1700007F RID: 127
			// (get) Token: 0x0600035E RID: 862 RVA: 0x0001C2C8 File Offset: 0x0001B2C8
			// (set) Token: 0x0600035F RID: 863 RVA: 0x0001C324 File Offset: 0x0001B324
			public Model.PpmContext FirstStateSuccessor
			{
				get
				{
					return new Model.PpmContext((uint)((int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 4U))] | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 5U))] << 8 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 6U))] << 16 | (int)Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 7U))] << 24));
				}
				set
				{
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 4U))] = (byte)value.Address;
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 5U))] = (byte)(value.Address >> 8);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 6U))] = (byte)(value.Address >> 16);
					Model.PpmContext.Memory[(int)((UIntPtr)(this.Address + 7U))] = (byte)(value.Address >> 24);
				}
			}

			// Token: 0x06000360 RID: 864 RVA: 0x0001C395 File Offset: 0x0001B395
			public static implicit operator Model.PpmContext(Pointer pointer)
			{
				return new Model.PpmContext(pointer.Address);
			}

			// Token: 0x06000361 RID: 865 RVA: 0x0001C3A3 File Offset: 0x0001B3A3
			public static Model.PpmContext operator +(Model.PpmContext context, int offset)
			{
				context.Address = (uint)((ulong)context.Address + (ulong)((long)(offset * 12)));
				return context;
			}

			// Token: 0x06000362 RID: 866 RVA: 0x0001C3BC File Offset: 0x0001B3BC
			public static Model.PpmContext operator -(Model.PpmContext context, int offset)
			{
				context.Address = (uint)((ulong)context.Address - (ulong)((long)(offset * 12)));
				return context;
			}

			// Token: 0x06000363 RID: 867 RVA: 0x0001C3D5 File Offset: 0x0001B3D5
			public static bool operator <=(Model.PpmContext context1, Model.PpmContext context2)
			{
				return context1.Address <= context2.Address;
			}

			// Token: 0x06000364 RID: 868 RVA: 0x0001C3EA File Offset: 0x0001B3EA
			public static bool operator >=(Model.PpmContext context1, Model.PpmContext context2)
			{
				return context1.Address >= context2.Address;
			}

			// Token: 0x06000365 RID: 869 RVA: 0x0001C3FF File Offset: 0x0001B3FF
			public static bool operator ==(Model.PpmContext context1, Model.PpmContext context2)
			{
				return context1.Address == context2.Address;
			}

			// Token: 0x06000366 RID: 870 RVA: 0x0001C411 File Offset: 0x0001B411
			public static bool operator !=(Model.PpmContext context1, Model.PpmContext context2)
			{
				return context1.Address != context2.Address;
			}

			// Token: 0x06000367 RID: 871 RVA: 0x0001C428 File Offset: 0x0001B428
			public override bool Equals(object obj)
			{
				if (obj is Model.PpmContext)
				{
					return ((Model.PpmContext)obj).Address == this.Address;
				}
				return base.Equals(obj);
			}

			// Token: 0x06000368 RID: 872 RVA: 0x0001C465 File Offset: 0x0001B465
			public override int GetHashCode()
			{
				return this.Address.GetHashCode();
			}

			// Token: 0x06000369 RID: 873 RVA: 0x0001C474 File Offset: 0x0001B474
			public void EncodeBinarySymbol(int symbol)
			{
				PpmState firstState = this.FirstState;
				int num = (int)Model.probabilities[(int)(firstState.Frequency - 1)];
				int num2 = (int)(Model.numberStatisticsToBinarySummaryIndex[(int)this.Suffix.NumberStatistics] + Model.previousSuccess + this.Flags) + (Model.runLength >> 26 & 32);
				if ((int)firstState.Symbol == symbol)
				{
					Model.foundState = firstState;
					firstState.Frequency += ((firstState.Frequency < 196) ? 1 : 0);
					Coder.LowCount = 0U;
					Coder.HighCount = (uint)Model.binarySummary[num, num2];
					ref ushort ptr = ref Model.binarySummary[num, num2];
					ptr += (ushort)(128L - (long)Model.Mean((int)Model.binarySummary[num, num2], 7, 2));
					Model.previousSuccess = 1;
					Model.runLength++;
					return;
				}
				Coder.LowCount = (uint)Model.binarySummary[num, num2];
				ref ushort ptr2 = ref Model.binarySummary[num, num2];
				ptr2 -= (ushort)Model.Mean((int)Model.binarySummary[num, num2], 7, 2);
				Coder.HighCount = 16384U;
				Model.initialEscape = (int)Model.ExponentialEscapes[Model.binarySummary[num, num2] >> 10];
				Model.characterMask[(int)firstState.Symbol] = Model.escapeCount;
				Model.previousSuccess = 0;
				Model.numberMasked = 0;
				Model.foundState = PpmState.Zero;
			}

			// Token: 0x0600036A RID: 874 RVA: 0x0001C5E4 File Offset: 0x0001B5E4
			public void EncodeSymbol1(int symbol)
			{
				uint num = (uint)this.Statistics.Symbol;
				PpmState ppmState = this.Statistics;
				Coder.Scale = (uint)this.SummaryFrequency;
				if ((ulong)num == (ulong)((long)symbol))
				{
					Coder.HighCount = (uint)ppmState.Frequency;
					Model.previousSuccess = ((2U * Coder.HighCount >= Coder.Scale) ? 1 : 0);
					Model.foundState = ppmState;
					Model.foundState.Frequency = Model.foundState.Frequency + 4;
					this.SummaryFrequency += 4;
					Model.runLength += (int)Model.previousSuccess;
					if (ppmState.Frequency > 124)
					{
						this.Rescale();
					}
					Coder.LowCount = 0U;
					return;
				}
				uint num2 = (uint)ppmState.Frequency;
				num = (uint)this.NumberStatistics;
				Model.previousSuccess = 0;
				do
				{
					PpmState ppmState2;
					ppmState = (ppmState2 = ++ppmState);
					if ((int)ppmState2.Symbol == symbol)
					{
						goto Block_6;
					}
					num2 += (uint)ppmState.Frequency;
				}
				while ((num -= 1U) != 0U);
				Coder.LowCount = num2;
				Model.characterMask[(int)ppmState.Symbol] = Model.escapeCount;
				Model.numberMasked = this.NumberStatistics;
				num = (uint)this.NumberStatistics;
				Model.foundState = PpmState.Zero;
				do
				{
					byte[] characterMask = Model.characterMask;
					PpmState ppmState3;
					ppmState = (ppmState3 = --ppmState);
					characterMask[(int)ppmState3.Symbol] = Model.escapeCount;
				}
				while ((num -= 1U) != 0U);
				Coder.HighCount = Coder.Scale;
				return;
				Block_6:
				Coder.HighCount = (Coder.LowCount = num2) + (uint)ppmState.Frequency;
				this.Update1(ppmState);
			}

			// Token: 0x0600036B RID: 875 RVA: 0x0001C748 File Offset: 0x0001B748
			public void EncodeSymbol2(int symbol)
			{
				See2Context see2Context = this.MakeEscapeFrequency();
				uint num = 0U;
				uint num2 = (uint)(this.NumberStatistics - Model.numberMasked);
				PpmState ppmState = this.Statistics - 1;
				for (;;)
				{
					uint symbol2 = (uint)ppmState[1].Symbol;
					ppmState = ++ppmState;
					if (Model.characterMask[(int)((UIntPtr)symbol2)] != Model.escapeCount)
					{
						Model.characterMask[(int)((UIntPtr)symbol2)] = Model.escapeCount;
						if ((ulong)symbol2 == (ulong)((long)symbol))
						{
							goto IL_B2;
						}
						num += (uint)ppmState.Frequency;
						if ((num2 -= 1U) == 0U)
						{
							break;
						}
					}
				}
				Coder.LowCount = num;
				Coder.Scale += Coder.LowCount;
				Coder.HighCount = Coder.Scale;
				See2Context see2Context2 = see2Context;
				see2Context2.Summary += (ushort)Coder.Scale;
				Model.numberMasked = this.NumberStatistics;
				return;
				IL_B2:
				Coder.LowCount = num;
				num += (uint)ppmState.Frequency;
				Coder.HighCount = num;
				PpmState state = ppmState;
				while ((num2 -= 1U) != 0U)
				{
					uint symbol2;
					do
					{
						symbol2 = (uint)state[1].Symbol;
						state = ++state;
					}
					while (Model.characterMask[(int)((UIntPtr)symbol2)] == Model.escapeCount);
					num += (uint)state.Frequency;
				}
				Coder.Scale += num;
				see2Context.Update();
				this.Update2(ppmState);
			}

			// Token: 0x0600036C RID: 876 RVA: 0x0001C878 File Offset: 0x0001B878
			public void DecodeBinarySymbol()
			{
				PpmState firstState = this.FirstState;
				int num = (int)Model.probabilities[(int)(firstState.Frequency - 1)];
				int num2 = (int)(Model.numberStatisticsToBinarySummaryIndex[(int)this.Suffix.NumberStatistics] + Model.previousSuccess + this.Flags) + (Model.runLength >> 26 & 32);
				if (Coder.RangeGetCurrentShiftCount(14) < (uint)Model.binarySummary[num, num2])
				{
					Model.foundState = firstState;
					firstState.Frequency += ((firstState.Frequency < 196) ? 1 : 0);
					Coder.LowCount = 0U;
					Coder.HighCount = (uint)Model.binarySummary[num, num2];
					ref ushort ptr = ref Model.binarySummary[num, num2];
					ptr += (ushort)(128L - (long)Model.Mean((int)Model.binarySummary[num, num2], 7, 2));
					Model.previousSuccess = 1;
					Model.runLength++;
					return;
				}
				Coder.LowCount = (uint)Model.binarySummary[num, num2];
				ref ushort ptr2 = ref Model.binarySummary[num, num2];
				ptr2 -= (ushort)Model.Mean((int)Model.binarySummary[num, num2], 7, 2);
				Coder.HighCount = 16384U;
				Model.initialEscape = (int)Model.ExponentialEscapes[Model.binarySummary[num, num2] >> 10];
				Model.characterMask[(int)firstState.Symbol] = Model.escapeCount;
				Model.previousSuccess = 0;
				Model.numberMasked = 0;
				Model.foundState = PpmState.Zero;
			}

			// Token: 0x0600036D RID: 877 RVA: 0x0001C9F4 File Offset: 0x0001B9F4
			public void DecodeSymbol1()
			{
				uint num = (uint)this.Statistics.Frequency;
				PpmState ppmState = this.Statistics;
				Coder.Scale = (uint)this.SummaryFrequency;
				uint num2 = Coder.RangeGetCurrentCount();
				if (num2 < num)
				{
					Coder.HighCount = num;
					Model.previousSuccess = ((2U * Coder.HighCount >= Coder.Scale) ? 1 : 0);
					Model.foundState = ppmState;
					num += 4U;
					Model.foundState.Frequency = (byte)num;
					this.SummaryFrequency += 4;
					Model.runLength += (int)Model.previousSuccess;
					if (num > 124U)
					{
						this.Rescale();
					}
					Coder.LowCount = 0U;
					return;
				}
				uint num3 = (uint)this.NumberStatistics;
				Model.previousSuccess = 0;
				do
				{
					uint num4 = num;
					PpmState ppmState2;
					ppmState = (ppmState2 = ++ppmState);
					if ((num = num4 + (uint)ppmState2.Frequency) > num2)
					{
						goto Block_6;
					}
				}
				while ((num3 -= 1U) != 0U);
				Coder.LowCount = num;
				Model.characterMask[(int)ppmState.Symbol] = Model.escapeCount;
				Model.numberMasked = this.NumberStatistics;
				num3 = (uint)this.NumberStatistics;
				Model.foundState = PpmState.Zero;
				do
				{
					byte[] characterMask = Model.characterMask;
					PpmState ppmState3;
					ppmState = (ppmState3 = --ppmState);
					characterMask[(int)ppmState3.Symbol] = Model.escapeCount;
				}
				while ((num3 -= 1U) != 0U);
				Coder.HighCount = Coder.Scale;
				return;
				Block_6:
				Coder.HighCount = num;
				Coder.LowCount = Coder.HighCount - (uint)ppmState.Frequency;
				this.Update1(ppmState);
			}

			// Token: 0x0600036E RID: 878 RVA: 0x0001CB44 File Offset: 0x0001BB44
			public void DecodeSymbol2()
			{
				See2Context see2Context = this.MakeEscapeFrequency();
				uint num = 0U;
				uint num2 = (uint)(this.NumberStatistics - Model.numberMasked);
				uint num3 = 0U;
				PpmState ppmState = this.Statistics - 1;
				for (;;)
				{
					uint symbol = (uint)ppmState[1].Symbol;
					ppmState = ++ppmState;
					if (Model.characterMask[(int)((UIntPtr)symbol)] != Model.escapeCount)
					{
						num += (uint)ppmState.Frequency;
						Model.PpmContext.decodeStates[(int)((UIntPtr)(num3++))] = ppmState;
						if ((num2 -= 1U) == 0U)
						{
							break;
						}
					}
				}
				Coder.Scale += num;
				uint num4 = Coder.RangeGetCurrentCount();
				num3 = 0U;
				ppmState = Model.PpmContext.decodeStates[(int)((UIntPtr)num3)];
				if (num4 < num)
				{
					num = 0U;
					while ((num += (uint)ppmState.Frequency) <= num4)
					{
						ppmState = Model.PpmContext.decodeStates[(int)((UIntPtr)(num3 += 1U))];
					}
					Coder.HighCount = num;
					Coder.LowCount = Coder.HighCount - (uint)ppmState.Frequency;
					see2Context.Update();
					this.Update2(ppmState);
					return;
				}
				Coder.LowCount = num;
				Coder.HighCount = Coder.Scale;
				num2 = (uint)(this.NumberStatistics - Model.numberMasked);
				Model.numberMasked = this.NumberStatistics;
				do
				{
					Model.characterMask[(int)Model.PpmContext.decodeStates[(int)((UIntPtr)num3)].Symbol] = Model.escapeCount;
					num3 += 1U;
				}
				while ((num2 -= 1U) != 0U);
				See2Context see2Context2 = see2Context;
				see2Context2.Summary += (ushort)Coder.Scale;
			}

			// Token: 0x0600036F RID: 879 RVA: 0x0001CCB8 File Offset: 0x0001BCB8
			public void Update1(PpmState state)
			{
				Model.foundState = state;
				Model.foundState.Frequency = Model.foundState.Frequency + 4;
				this.SummaryFrequency += 4;
				if (state[0].Frequency > state[-1].Frequency)
				{
					Model.Swap(state[0], state[-1]);
					state = (Model.foundState = --state);
					if (state.Frequency > 124)
					{
						this.Rescale();
					}
				}
			}

			// Token: 0x06000370 RID: 880 RVA: 0x0001CD44 File Offset: 0x0001BD44
			public void Update2(PpmState state)
			{
				Model.foundState = state;
				Model.foundState.Frequency = Model.foundState.Frequency + 4;
				this.SummaryFrequency += 4;
				if (state.Frequency > 124)
				{
					this.Rescale();
				}
				Model.escapeCount += 1;
				Model.runLength = Model.initialRunLength;
			}

			// Token: 0x06000371 RID: 881 RVA: 0x0001CDA4 File Offset: 0x0001BDA4
			public See2Context MakeEscapeFrequency()
			{
				uint num = (uint)(2 * this.NumberStatistics);
				See2Context see2Context;
				if (this.NumberStatistics != 255)
				{
					num = (uint)this.Suffix.NumberStatistics;
					int num2 = (int)(Model.probabilities[(int)(this.NumberStatistics + 2)] - 3);
					int num3 = (int)(((this.SummaryFrequency > (ushort)(11 * (this.NumberStatistics + 1))) ? 1 : 0) + (((long)(2 * this.NumberStatistics) < (long)((ulong)(num + (uint)Model.numberMasked))) ? 2 : 0) + this.Flags);
					see2Context = Model.see2Contexts[num2, num3];
					Coder.Scale = see2Context.Mean();
				}
				else
				{
					see2Context = Model.emptySee2Context;
					Coder.Scale = 1U;
				}
				return see2Context;
			}

			// Token: 0x06000372 RID: 882 RVA: 0x0001CE48 File Offset: 0x0001BE48
			public void Rescale()
			{
				uint num = (uint)this.NumberStatistics;
				PpmState ppmState = Model.foundState;
				while (ppmState != this.Statistics)
				{
					Model.Swap(ppmState[0], ppmState[-1]);
					ppmState = --ppmState;
				}
				ppmState.Frequency += 4;
				this.SummaryFrequency += 4;
				uint num2 = (uint)(this.SummaryFrequency - (ushort)ppmState.Frequency);
				int num3 = (Model.orderFall != 0 || Model.method > ModelRestorationMethod.Freeze) ? 1 : 0;
				ppmState.Frequency = (byte)((int)ppmState.Frequency + num3 >> 1);
				this.SummaryFrequency = (ushort)ppmState.Frequency;
				do
				{
					uint num4 = num2;
					PpmState ppmState2;
					ppmState = (ppmState2 = ++ppmState);
					num2 = num4 - (uint)ppmState2.Frequency;
					ppmState.Frequency = (byte)((int)ppmState.Frequency + num3 >> 1);
					this.SummaryFrequency += (ushort)ppmState.Frequency;
					if (ppmState[0].Frequency > ppmState[-1].Frequency)
					{
						PpmState state = ppmState;
						byte symbol = state.Symbol;
						byte b = state.Frequency;
						Model.PpmContext successor = state.Successor;
						byte b2;
						PpmState ppmState3;
						do
						{
							Model.Copy(state[0], state[-1]);
							b2 = b;
							state = (ppmState3 = --state);
						}
						while (b2 > ppmState3[-1].Frequency);
						state.Symbol = symbol;
						state.Frequency = b;
						state.Successor = successor;
					}
				}
				while ((num -= 1U) != 0U);
				if (ppmState.Frequency == 0)
				{
					PpmState ppmState4;
					do
					{
						num += 1U;
						ppmState = (ppmState4 = --ppmState);
					}
					while (ppmState4.Frequency == 0);
					num2 += num;
					uint num5 = (uint)(this.NumberStatistics + 2 >> 1);
					this.NumberStatistics -= (byte)num;
					if (this.NumberStatistics == 0)
					{
						byte symbol = this.Statistics.Symbol;
						byte b = this.Statistics.Frequency;
						Model.PpmContext successor = this.Statistics.Successor;
						b = (byte)(((long)(2 * b) + (long)((ulong)num2) - 1L) / (long)((ulong)num2));
						if (b > 41)
						{
							b = 41;
						}
						Allocator.FreeUnits(this.Statistics, num5);
						this.FirstStateSymbol = symbol;
						this.FirstStateFrequency = b;
						this.FirstStateSuccessor = successor;
						this.Flags = (this.Flags & 16) + ((symbol >= 64) ? 8 : 0);
						Model.foundState = this.FirstState;
						return;
					}
					this.Statistics = Allocator.ShrinkUnits(this.Statistics, num5, (uint)(this.NumberStatistics + 2 >> 1));
					this.Flags &= 247;
					num = (uint)this.NumberStatistics;
					ppmState = this.Statistics;
					this.Flags |= ((ppmState.Symbol >= 64) ? 8 : 0);
					do
					{
						byte flags = this.Flags;
						PpmState ppmState5;
						ppmState = (ppmState5 = ++ppmState);
						this.Flags = (flags | ((ppmState5.Symbol >= 64) ? 8 : 0));
					}
					while ((num -= 1U) != 0U);
				}
				num2 -= num2 >> 1;
				this.SummaryFrequency += (ushort)num2;
				this.Flags |= 4;
				Model.foundState = this.Statistics;
			}

			// Token: 0x06000373 RID: 883 RVA: 0x0001D184 File Offset: 0x0001C184
			public void Refresh(uint oldUnitCount, bool scale)
			{
				int num = (int)this.NumberStatistics;
				int num2 = scale ? 1 : 0;
				this.Statistics = Allocator.ShrinkUnits(this.Statistics, oldUnitCount, (uint)(num + 2 >> 1));
				PpmState state = this.Statistics;
				this.Flags = (this.Flags & 16 + (scale ? 4 : 0)) + ((state.Symbol >= 64) ? 8 : 0);
				int num3 = (int)(this.SummaryFrequency - (ushort)state.Frequency);
				state.Frequency = (byte)((int)state.Frequency + num2 >> num2);
				this.SummaryFrequency = (ushort)state.Frequency;
				do
				{
					int num4 = num3;
					PpmState ppmState;
					state = (ppmState = ++state);
					num3 = num4 - (int)ppmState.Frequency;
					state.Frequency = (byte)((int)state.Frequency + num2 >> num2);
					this.SummaryFrequency += (ushort)state.Frequency;
					this.Flags |= ((state.Symbol >= 64) ? 8 : 0);
				}
				while (--num != 0);
				num3 = num3 + num2 >> num2;
				this.SummaryFrequency += (ushort)num3;
			}

			// Token: 0x06000374 RID: 884 RVA: 0x0001D2A0 File Offset: 0x0001C2A0
			public Model.PpmContext CutOff(int order)
			{
				PpmState ppmState;
				if (this.NumberStatistics != 0)
				{
					uint num = (uint)(this.NumberStatistics + 2 >> 1);
					this.Statistics = Allocator.MoveUnitsUp(this.Statistics, num);
					int numberStatistics = (int)this.NumberStatistics;
					ppmState = this.Statistics + numberStatistics;
					while (ppmState >= this.Statistics)
					{
						if (ppmState.Successor < Allocator.BaseUnit)
						{
							ppmState.Successor = Model.PpmContext.Zero;
							Model.Swap(ppmState, this.Statistics[numberStatistics--]);
						}
						else if (order < Model.modelOrder)
						{
							ppmState.Successor = ppmState.Successor.CutOff(order + 1);
						}
						else
						{
							ppmState.Successor = Model.PpmContext.Zero;
						}
						ppmState = --ppmState;
					}
					if (numberStatistics != (int)this.NumberStatistics && order != 0)
					{
						this.NumberStatistics = (byte)numberStatistics;
						ppmState = this.Statistics;
						if (numberStatistics < 0)
						{
							Allocator.FreeUnits(ppmState, num);
							Allocator.SpecialFreeUnits(this);
							return Model.PpmContext.Zero;
						}
						if (numberStatistics == 0)
						{
							this.Flags = (this.Flags & 16) + ((ppmState.Symbol >= 64) ? 8 : 0);
							Model.Copy(this.FirstState, ppmState);
							Allocator.FreeUnits(ppmState, num);
							this.FirstStateFrequency = (byte)(this.FirstStateFrequency + 11 >> 3);
						}
						else
						{
							this.Refresh(num, (int)this.SummaryFrequency > 16 * numberStatistics);
						}
					}
					return this;
				}
				ppmState = this.FirstState;
				if (!(ppmState.Successor >= Allocator.BaseUnit))
				{
					Allocator.SpecialFreeUnits(this);
					return Model.PpmContext.Zero;
				}
				if (order < Model.modelOrder)
				{
					ppmState.Successor = ppmState.Successor.CutOff(order + 1);
				}
				else
				{
					ppmState.Successor = Model.PpmContext.Zero;
				}
				if (ppmState.Successor == Model.PpmContext.Zero && (long)order > 9L)
				{
					Allocator.SpecialFreeUnits(this);
					return Model.PpmContext.Zero;
				}
				return this;
			}

			// Token: 0x06000375 RID: 885 RVA: 0x0001D4C8 File Offset: 0x0001C4C8
			public Model.PpmContext RemoveBinaryContexts(int order)
			{
				if (this.NumberStatistics != 0)
				{
					PpmState ppmState = this.Statistics + (int)this.NumberStatistics;
					while (ppmState >= this.Statistics)
					{
						if (ppmState.Successor >= Allocator.BaseUnit && order < Model.modelOrder)
						{
							ppmState.Successor = ppmState.Successor.RemoveBinaryContexts(order + 1);
						}
						else
						{
							ppmState.Successor = Model.PpmContext.Zero;
						}
						ppmState = --ppmState;
					}
					return this;
				}
				PpmState firstState = this.FirstState;
				if (firstState.Successor >= Allocator.BaseUnit && order < Model.modelOrder)
				{
					firstState.Successor = firstState.Successor.RemoveBinaryContexts(order + 1);
				}
				else
				{
					firstState.Successor = Model.PpmContext.Zero;
				}
				if (firstState.Successor == Model.PpmContext.Zero && (this.Suffix.NumberStatistics == 0 || this.Suffix.Flags == 255))
				{
					Allocator.FreeUnits(this, 1U);
					return Model.PpmContext.Zero;
				}
				return this;
			}

			// Token: 0x04000257 RID: 599
			public const int Size = 12;

			// Token: 0x04000258 RID: 600
			public uint Address;

			// Token: 0x04000259 RID: 601
			public static byte[] Memory;

			// Token: 0x0400025A RID: 602
			public static readonly Model.PpmContext Zero = new Model.PpmContext(0U);

			// Token: 0x0400025B RID: 603
			private static PpmState[] decodeStates = new PpmState[256];
		}
	}
}

using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Content.Items.Accessories.Runes;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Ores;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalItems
{
	internal class ModifiedCrates : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			int itemType = item.type;

			LeadingConditionRule hardmode = new LeadingConditionRule(Condition.Hardmode.ToDropCondition(ShowItemDropInUI.Always));

			switch (itemType)
			{
				case ItemID.WoodenCrate:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MinersRing>(), 20));
				break;

				case ItemID.IronCrate:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlankRune>(), 15));
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StoneWand>(), 15));
				break;

				case ItemID.GoldenCrate:
				case ItemID.GoldenCrateHard:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneOfSpelunking>(), 20));
				break;

				case ItemID.FloatingIslandFishingCrate:
				case ItemID.FloatingIslandFishingCrateHard:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HangGlider>(), 8));
				break;

				case ItemID.DungeonFishingCrate:
				case ItemID.DungeonFishingCrateHard:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneOfMovement>(), 8));
				break;

				case ItemID.OasisCrate:
				case ItemID.OasisCrateHard:
					int[] desertMimicItems = { ModContent.ItemType<DesertsFury>(), ModContent.ItemType<DustbringerStaff>(), ModContent.ItemType<DesertBlaster>(), ModContent.ItemType<SunScepter>() };
					hardmode.OnSuccess(ItemDropRule.OneFromOptions(2, desertMimicItems));
					itemLoot.Add(hardmode);
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Dunerang>(), 5));
				break;

				case ItemID.JungleFishingCrate:
				case ItemID.JungleFishingCrateHard:
					int[] jungleMimicItems = { ModContent.ItemType<PetalBlade>(), ModContent.ItemType<SporeRod>(), ModContent.ItemType<JungleChakram>(), ModContent.ItemType<BlossomLash>() };
					hardmode.OnSuccess(ItemDropRule.OneFromOptions(2, jungleMimicItems));
					itemLoot.Add(hardmode);
				break;

				case ItemID.OceanCrate:
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShellHorn>(), 5));
				break;
			}

			// *** ULTIMATE SKYBLOCK COMPATIBILITY STUFF ***
			if (ModLoader.TryGetMod("UltimateSkyblock", out Mod UltimateSkyblock))
			{
				switch (itemType)
				{
					case ItemID.FrozenCrate:
					case ItemID.FrozenCrateHard:
						itemLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAny.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<Permafrost>(), 5, 20, 35));
						itemLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAny.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<FrostBar>(), 12, 6, 16));
						itemLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAny.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<IceEssence>(), 8, 3, 4));
					break;

					case ItemID.OasisCrate:
					case ItemID.OasisCrateHard:
						itemLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<DuneOre>(), 5, 20, 35));
						itemLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<DuneBar>(), 12, 6, 16));
					break;
				}
			}
		}
	}
}

using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Summon;
using AssortedAdditions.Content.Items.Placeables.Ores;
using AssortedAdditions.Content.Items.Accessories.Runes;
using AssortedAdditions.Content.Items.Consumables;

namespace AssortedAdditions.Common.GlobalNPCs
{
    // This class is for adding loot drops to existing NPCs
    internal class ModifiedLootDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {	
			// This used to be a bunch of if statements before changing to switch case which seems to be a bit clearer
			// If a better alternative is found, change to that
			switch (npc.type)
			{
				#region BOSSES
				case NPCID.CultistBoss:
					npcLoot.Add(ItemDropRule.BossBag(ItemID.CultistBossBag)); // The cultist boss now drops its treasure bag since this mod adds some items to it

					// These would drop from the treasure bag on expert mode
					LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert()); // New condition for when its not expert mode
					notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<DeathRay>(), // Drops one of these
						ModContent.ItemType<ScifiBlaster>(),
						ModContent.ItemType<CultClassic>(),
						ModContent.ItemType<Motivator>()));
					npcLoot.Add(notExpert);

					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EchoChamber>(), 2, 1, 1));
					break;

				#endregion

				#region EVENTS
				// Pirate invasion
				case NPCID.Parrot:
				case NPCID.PirateCorsair:
				case NPCID.PirateCrossbower:
				case NPCID.PirateDeadeye:
				case NPCID.PirateDeckhand:
				case NPCID.PirateGhost:
				case NPCID.PirateCaptain:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneOfProsperity>(), 100));
					break;

				case NPCID.PirateShip:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RuneOfProsperity>(), 15));
					break;

				#endregion

				#region NORMAL ENEMIES

				// Demon eyes (only positive IDs since the negatives are variants of the positives)
				case NPCID.DemonEye:
				case NPCID.CataractEye:
				case NPCID.SleepyEye:
				case NPCID.DialatedEye:
				case NPCID.GreenEye:
				case NPCID.PurpleEye:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeBow>(), 40, 1, 1)); // 2.5% chance to drop
					break;

				// Some desert enemies
				case NPCID.TombCrawlerHead:
					// 33% chance to drop 4-7 dune ores after crimson/corruption boss has been defeated
					npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always),
						ModContent.ItemType<DuneOre>(), 3, 4, 7));
					break;

				case NPCID.Antlion:
					// ~15% chance to drop 2-4 dune ores after crimson/corruption boss has been defeated
					npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always),
						ModContent.ItemType<DuneOre>(), 7, 2, 4));
					break;

				case NPCID.Vulture:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VultureWings>(), 20)); // 5% chance
					break;

				// Ice cavern enemies
				case NPCID.IceBat:
				case NPCID.SnowFlinx:
				case NPCID.UndeadViking:
				case NPCID.CyanBeetle:
				case NPCID.SpikedIceSlime:
				case NPCID.ArmoredViking:
				case NPCID.IceTortoise:
				case NPCID.IceElemental:
				case NPCID.IcyMerman:
				case NPCID.IceMimic:

					// Ice biome enemies have a 10% chance to drop Ice Essence after one of the mech bosses have been defeated
					// Alternatively could have checked if the player is in the ice biome zone and make all enemies drop the essence
					npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAny.ToDropCondition(ShowItemDropInUI.Always),
						ModContent.ItemType<IceEssence>(), 10, 1, 1));

					if(npc.type == NPCID.ArmoredViking)
					{
						npcLoot.Add(ItemDropRule.Common(ItemID.ArmorPolish, 40, 1, 1));
					}
					break;

				// Add armor polish to other armored enemies as well ^^
				// (not to rusty armored bones since its armor is rusty and having it drop the item makes less sense)
				case NPCID.HellArmoredBones:
					npcLoot.Add(ItemDropRule.Common(ItemID.ArmorPolish, 40, 1, 1));
					break;

				case NPCID.Shark:
					// 25% chance to drop 1-3 Shark Tooth
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SharkTooth>(), 4, 1, 3));
					break;

				case NPCID.ThePossessed:
				case NPCID.Fritz:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Adrenaline>(), 40, 1, 1)); // 2.5% chance to drop
					break;

				// Add vitamins to more enemy loot pools so its less rare
				case NPCID.Herpling:
				case NPCID.IchorSticker:
				case NPCID.Clinger:
				case NPCID.SeekerHead:
					npcLoot.Add(ItemDropRule.Common(ItemID.Vitamins, 40, 1, 1)); // 2.5% chance to drop
					break;

				case NPCID.CaveBat:
				case NPCID.JungleBat:
				case NPCID.GiantBat:
				case NPCID.SporeBat:
				case NPCID.GiantFlyingFox:
					// Bats drop antidote during hardmode
					npcLoot.Add(ItemDropRule.ByCondition(Condition.Hardmode.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<Antidote>(), 40, 1)); // 2.5 % chance
					break;

				case NPCID.Dandelion:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DandelionFlower>(), 3, 1, 1)); // 33% chance
					break;

				case NPCID.BigMimicCorruption:
				case NPCID.BigMimicCrimson:
				case NPCID.BigMimicHallow:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MimicsTongue>(), 3)); // 33% chance
					break;

				case NPCID.Ghost:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GhostlyBlade>(), 12)); // 8% chance																		   
					npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAll.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<GraveFlowers>(), 20, 1)); // 5% chance to drop in hardmode
					break;

				case NPCID.RuneWizard:
					// 50% chance to drop one of these runes
					npcLoot.Add(ItemDropRule.OneFromOptionsWithNumerator(2, 1,
						ModContent.ItemType<BlankRune>(),
						ModContent.ItemType<RuneOfHealth>(),
						ModContent.ItemType<ManaRune>()));
					break;

				case NPCID.Tim:
					npcLoot.Add(ItemDropRule.ByCondition(Condition.Hardmode.ToDropCondition(ShowItemDropInUI.Always), 
						ModContent.ItemType<BlankRune>()));
					break;

				case NPCID.DarkCaster:
				case NPCID.GoblinSorcerer:
				case NPCID.FireImp:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BlankRune>(), 25, 1));
					break;

				case NPCID.GraniteGolem:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteArmorShard>(), 10, 1));
					break;

				case NPCID.GraniteFlyer:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteArmorShard>(), 2, 1, 3));
					break;

				#endregion
			}
		}
    }
}

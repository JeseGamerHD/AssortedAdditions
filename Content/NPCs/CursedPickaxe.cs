using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Banners;
using AssortedAdditions.Common.Configs;

namespace AssortedAdditions.Content.NPCs
{
    internal class CursedPickaxe : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;

            // Immune to debuffs:
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 66;
            NPC.height = 66;
            NPC.damage = 80;
            NPC.defense = 18;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0.4f; // 60% resist
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 3000; // 3 silver

            AnimationType = NPCID.EnchantedSword;
            NPC.aiStyle = 23; // Copy behaviour and animation from flying weapons
            AIType = NPCID.EnchantedSword;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<CursedPickaxeBanner>();
        }

        // Should only spawn after one of the mech bosses have been defeated
        // and only in the underground ice biome
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMechBossAny && spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneRockLayerHeight)
            {
                float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier == 1f
                    ? ServerSidedToggles.Instance.CursedPickaxeSpawnMultiplier : ServerSidedToggles.Instance.NPCSpawnMultiplier;

				return 0.004f * multiplier;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Nazar, 100, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceEssence>(), 10, 1, 1));
        }

        // Spawns cloud gores upon death
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.position, 11, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.position, 12, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.position, 13, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Cursed, 360);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White; // the npc is visible even without light
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Pickaxe once lost to the ice caverns, now possessed by evil spirits seeking revenge.")
            });
        }
    }
}

using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs
{
	internal class GraniteGrunt : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 15;

			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Velocity = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 56;
			NPC.damage = 24;
			NPC.defense = 16;
			NPC.lifeMax = 85;
			NPC.knockBackResist = 0.4f; // 60% resist
			NPC.HitSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/GraniteGruntHit");
			NPC.DeathSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/GraniteGruntDeath");
			NPC.value = 300; // 3 silver

			AnimationType = NPCID.PossessedArmor;
			NPC.aiStyle = NPCAIStyleID.Fighter;
			AIType = NPCID.PossessedArmor;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<GraniteGruntBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneGranite || spawnInfo.Granite)
			{
				return 0.05f;
			}

			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteArmorShard>(), 4, 1, 2));
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GraniteGruntGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GraniteGruntGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GraniteGruntGore3").Type, 1f);
			}
		}

		public override void OnKill()
		{
			for(int i = 0; i < 20; i++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Granite, NPC.velocity.X, NPC.velocity.Y, 0, default, Main.rand.NextFloat(1, 2));
				dust.velocity *= 4f;
				dust.noGravity = true;
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Strong are the magicked spirits which inhabit the granite masses deep below, this type in particular taking on a humanoid shape.")
			});
		}
	}
}

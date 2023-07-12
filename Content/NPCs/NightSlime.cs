using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Bestiary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Placeables.Banners;

namespace ModdingTutorial.Content.NPCs
{
    internal class NightSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 32;
            NPC.damage = 10;
            NPC.defense = 2;
            NPC.lifeMax = 30;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 70;

            AnimationType = NPCID.BlueSlime;
            NPC.aiStyle = 1; // Copy behaviour and animation from basic slimes
            AIType = NPCID.BlueSlime;

            Banner = NPC.type; // Each enemy has their own banner
            BannerItem = ModContent.ItemType<NightSlimeBanner>();
        }

        // Spawns at night with slightly lessened chance
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance * 0.9f; // 10% less likely to spawn than zombies, for example
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 5));
            // add some custom loot item later TODO
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                // Also sets the background image ^^ (forest at night)
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Unlike most of its relatives, this slime only comes out during the night time.")
            });
        }

        // Parts of the slime glow
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glowMask = ModContent.Request<Texture2D>("ModdingTutorial/Content/NPCs/NightSlime_Glow").Value; // The glowmask sprite
            Vector2 drawPosition = NPC.position + new Vector2(0, 4) - Main.screenPosition; // Position the sprite gets drawn at
            Rectangle frame = new Rectangle(0, NPC.frame.Y, glowMask.Width, glowMask.Height / 2); // Frame(s) of the sprite, divided by 2 here since there are two frames

            // Actual drawing happens here:
            spriteBatch.Draw(glowMask,
                                drawPosition,
                                frame,
                                Color.White * 0.75f, // How bright the glow is
                                NPC.rotation,
                                Vector2.Zero,
                                1f,
                                NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                                0f);
        }
    }
}

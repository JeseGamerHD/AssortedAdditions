using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Armor;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using AssortedAdditions.Common.Configs;

namespace AssortedAdditions.Content.NPCs
{
    internal class LivingCactus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 40;
            NPC.damage = 15;
            NPC.defense = 3;
            NPC.lifeMax = 35;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 75;

            NPC.noGravity = false;
            NPC.noTileCollide = false;

            NPC.aiStyle = 0;

            Banner = NPC.type; // Each enemy has their own banner
            BannerItem = ModContent.ItemType<LivingCactusBanner>();
        }


        // 0 = idle
        // 1 = attack
        float State
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public float Rotation
        {
            get => NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        public float Timer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);
            Player target = Main.player[NPC.target];
            float speed = 15f;
            float inertia = 40f;

            // Check if player has died
            // and reset rotation and state if needed
            if (target.dead)
            {
                State = 0;
                NPC.rotation = default;
                return;
            }

            // The NPC will be in idle position until approached
            if (target.Distance(NPC.Center) < 150f)
            {
                State = 1;
            }

            // If player is not close keep idling
            if (State != 1)
            {
                return;
            }

            // State 1 reached
            // NPC will now attack

            // Basic movement:
            // NPC chases after the player
            Vector2 direction = target.Center - NPC.Center;
            direction.Normalize();
            direction *= speed;
            // Only set the X velocity so the npc won't fly after the player
            NPC.velocity.X = (NPC.velocity.X * (inertia - 1f) + direction.X) / inertia;

            // If the NPC manages to get higher than the player, then add y velocity to bring it down
            // E.g it has jumped over the player or it is jumping against a wall
            if (NPC.position.Y < target.position.Y)
            {
                NPC.velocity.Y = (NPC.velocity.Y * (inertia - 1) + direction.Y) / inertia;
            }

            // Jumping over obstacles:
            if (NPC.collideX)
            {
                Timer++;
                if (Timer < 20 && NPC.velocity.Y >= 0) // NPC goes up
                {
                    NPC.velocity.Y -= 10f;
                }

                if (Timer > 40)
                {
                    NPC.velocity.Y += 1f; // NPC goes down
                    Timer = 0;
                }
            }
            else
            {
                Timer = 0;
            }

            // NPC rotates when moving (rolls on the ground)
            // won't rotate if the distance is small since rolling without moving looks weird
            Rotation++;
            if (target.position.X > NPC.position.X)
            {
                if (target.Center.X - NPC.Center.X > 10)
                    NPC.rotation = MathHelper.ToRadians(Rotation * 8f);
            }
            else
            {
                if (target.Center.X - NPC.Center.X < 10)
                    NPC.rotation = -1 * MathHelper.ToRadians(Rotation * 8f);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            State = 1; // If shot from further away, set state to attack
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("LivingCactusGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("LivingCactusGore2").Type, 1f);
            }
        }

        private int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            // When state changes briefly display 2nd frame
            if (State == 1 && NPC.frameCounter < 4)
            {
                frame = 1;
                NPC.frameCounter++;
            }

            // Then keep using either frame 3 or 4 depending on which side the player is on
            if (NPC.frameCounter >= 4 && State != 0)
            {
                if (NPC.direction == 1)
                {
                    frame = 3;
                }
                else
                {
                    frame = 2;

                }
            }

            // State gets set to 0 when player dies
            // so set the frame to 0 as well
            if (State == 0)
            {
                frame = 0;
            }

            // Set the frame:
            NPC.frame.Y = frameHeight * frame;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Only spawns in the forest biome during the day
            // less likely to spawn during hardmode
            if (spawnInfo.Player.ZoneDesert)
            {
                float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier == 1f
                    ? ServerSidedToggles.Instance.LivingCactusSpawnMultiplier : ServerSidedToggles.Instance.NPCSpawnMultiplier;

				if (!Main.hardMode)
                {
                    return SpawnCondition.OverworldDayDesert.Chance * 0.5f * multiplier;
                }

                return SpawnCondition.OverworldDayDesert.Chance * 0.15f * multiplier;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Cactus, 1, 1, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CactusFlower>(), 10, 1, 1));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                // Also sets the background image ^^
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These ball shaped creatures inhabit the desert and blend into the environment with their cacti-like look")
            });
        }
    }
}

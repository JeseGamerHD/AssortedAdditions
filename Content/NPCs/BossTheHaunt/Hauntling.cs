using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using AssortedAdditions.Helpers;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using AssortedAdditions.Common.Configs;
using Terraria.GameContent.ItemDropRules;
using AssortedAdditions.Content.Items.Consumables;

namespace AssortedAdditions.Content.NPCs.BossTheHaunt
{
	internal class Hauntling : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 2;

			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 3;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.value = 200;
			NPC.damage = 45;
			NPC.defense = 12;
			NPC.lifeMax = 175;
			NPC.knockBackResist = 0.5f;

			NPC.HitSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/PhantomMageHit");
			NPC.DeathSound = SoundID.NPCDeath6;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			NPC.aiStyle = 0;
		}

		private ref float Dash => ref NPC.ai[0];
		private ref float Timer => ref NPC.ai[1];

		private float speed = 10f;
		private float inertia = 25f;
		public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player target = Main.player[NPC.target];
			float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);

			if(target.dead || !target.active)
			{
				DespawnBehaviour();
				return;
			}

			if (Timer > 0)
			{
				Timer--;
			}

			if (Timer == 0)
			{
				inertia = 25f;
			}

			// Fly towards the player
			if (distanceFromTarget > 200f && Timer == 0)
			{
				// The immediate range around the target (so it doesn't latch onto it when close)
				Vector2 direction = target.Center - NPC.Center;
				direction.Normalize();
				direction *= speed;

				NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
			}
			// When close enough do a dash
			else if (distanceFromTarget <= 200f && Timer == 0)
			{
				Dash = 1;
				Timer = 45;
				NPC.netUpdate = true;
			}

			if (Dash == 1)
			{
				NPC.velocity = Vector2.Zero;
				NPC.velocity.X = NPC.DirectionTo(target.Center).X * 16f;
				NPC.velocity.Y = NPC.DirectionTo(target.Center).Y * 6f;
				Dash = 0;
				NPC.netUpdate = true;
			}

			NPC.rotation = NPC.velocity.X * 0.01f;

			if (Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustDirect(NPC.position - NPC.velocity, NPC.width, NPC.height, DustID.Smoke, 0, 0, 100, default, 1.25f);
				dust.noGravity = true;
			}
		}

		int timer = 0;
		private void DespawnBehaviour()
		{
			timer++;

            if (timer >= 300)
            {
				NPC.active = false;
            }

            Vector2 direction = new Vector2(NPC.Center.X - 700, NPC.Center.Y + 300) - NPC.Center;
			direction.Normalize();
			direction *= speed;

			NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
		}

		public override void PostAI()
		{
			HelperMethods.StopNPCOverlap(NPC, 0.1f);
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 11, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 12, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 13, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAll.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<GraveFlowers>(), 30));
			npcLoot.Add(ItemDropRule.Common(ItemID.Heart, 12));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier;

			if (Main.hardMode && spawnInfo.Player.ZoneGraveyard)
			{
				return 0.09f * multiplier;
			}

			return 0f;
		}

		public override Color? GetAlpha(Color drawColor)
		{
			return Color.White;
		}

		private int currentFrame = 0;
		public override void FindFrame(int frameHeight)
		{
			int frameSpeed = 20;
			NPC.frameCounter++;

			if (NPC.frameCounter % frameSpeed == 0)
			{
				currentFrame++;
			}

			if (currentFrame > 1)
			{
				currentFrame = 0;
			}

			NPC.frame.Y = frameHeight * currentFrame;
			NPC.spriteDirection = NPC.direction;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Asset<Texture2D> texture = TextureAssets.Npc[NPC.type];

			int frameHeight = texture.Value.Height / Main.npcFrameCount[NPC.type];
			Rectangle frame = new Rectangle(0, currentFrame * frameHeight, texture.Value.Width, frameHeight);

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = NPC.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);

				SpriteEffects spriteDirection = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, NPC.rotation, drawOrigin, NPC.scale, spriteDirection, 0);
			}

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.aiStyle != NPCAIStyleID.CursedSkull)
			{
				Asset<Texture2D> glowMask = ModContent.Request<Texture2D>("AssortedAdditions/Content/NPCs/BossTheHaunt/Hauntling_Glow");
				Vector2 drawPosition = NPC.position + new Vector2(0, 4) - Main.screenPosition;
				Rectangle frame = new Rectangle(0, NPC.frame.Y, glowMask.Value.Width, glowMask.Value.Height / Main.npcFrameCount[NPC.type]);

				SpriteEffects spriteDirection = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				spriteBatch.Draw(glowMask.Value, drawPosition, frame, Color.White * 0.75f, NPC.rotation, Vector2.Zero, 1f, spriteDirection, 0f);
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				new FlavorTextBestiaryInfoElement("Don't let this one get too close as it will attempt to dash at you.")
			});
		}
	}
}

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs
{
	internal class TrainingDummyNPC : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// Hides this NPC from the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(){ Hide = true };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			NPCID.Sets.CantTakeLunchMoney[Type] = true;

			Main.npcFrameCount[Type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 30;
			NPC.scale = 2f;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.value = 0;
			NPC.lifeMax = int.MaxValue;
			NPC.knockBackResist = 0f;
			NPC.HitSound = SoundID.NPCHit15;
		}

		public override void UpdateLifeRegen(ref int damage)
		{
			NPC.lifeRegen += 99999;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}

		// States for animating the dummy
		private readonly float Idle = 0;
		private readonly float JustHit = 1;
		private readonly float DoingAnimation = 2;

		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			// Start the animation if one is not already in progress
			if (NPC.ai[2] != DoingAnimation)
			{
				NPC.ai[2] = JustHit;
			}
		}

		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			if (NPC.ai[2] != DoingAnimation)
			{
				NPC.ai[2] = JustHit;
			}
		}

		private int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			// Idle (not being hit)
			if (NPC.ai[2] == Idle)
			{
				frame = 3;
				NPC.frameCounter = 0;
			}
			else
			{
				// Was Just hit, start the animation
				if (NPC.ai[2] != DoingAnimation)
				{
					frame = 0;
					NPC.ai[2] = DoingAnimation;
				}

				if (NPC.frameCounter % 4 == 0)
				{
					frame++;
				}
				NPC.frameCounter++;

				// Animation finished, reset to idle
				if (frame > 4)
				{
					NPC.ai[2] = Idle;
					frame = 0;
				}
			}

			NPC.frame.Y = frameHeight * frame;
		}
	}
}

using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.Audio;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	#region Minion
	internal class GraniteStaffMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.minionSlots = 1f;

			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;

			Projectile.DamageType = DamageClass.Summon;
			Projectile.aiStyle = 0;
		}

		private bool canSpawnOrbiters = true;
		private (Projectile first, Projectile second) childProjectile;

		// Despawn minion if buff is removed
		// Otherwise keep minion alive
		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner];

			if (CheckActive(owner))
			{
				if (canSpawnOrbiters && Main.myPlayer == Projectile.owner)
				{
					SpawnOrbiters();
				}

				return true;
			}

			return false;
		}

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<GraniteStaffBuff>()); // If not, minion despawns
				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<GraniteStaffBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		private void SpawnOrbiters()
		{
			// Spawn the orbiting projectiles and store them so they can be killed when parent dies ("respawns" when minion slots are full)
			childProjectile.first = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
				ModContent.ProjectileType<GraniteStaffMinionOrbit>(), Projectile.damage, 2, Projectile.owner, Projectile.identity, 25);

			childProjectile.second = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
				ModContent.ProjectileType<GraniteStaffMinionOrbit>(), Projectile.damage, 2, Projectile.owner, Projectile.identity, -25);

			Projectile.netUpdate = true;

			canSpawnOrbiters = false;
		}

		// Copied (mostly) from vanilla AI_062() in Projectile.cs (Only parts related to the Hornet Minion)
		// I tried renaming the numX variables to something more descriptive, but there might've been some misunderstandings
		public override void AI()
		{
			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Purple, 0.25f);

			if (Main.rand.NextBool(6))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height, DustID.Granite);
				dust.noGravity = true;
			}

			PreventMinionOverlap();

			Vector2 targetPos = Projectile.position;
			float closestTargetDistance = 2000f;
			bool canAttack = false;

			NPC ownerMinionAttackTargetNPC2 = Projectile.OwnerMinionAttackTargetNPC;
			if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(Projectile))
			{
				float distanceFromTarget = Vector2.Distance(ownerMinionAttackTargetNPC2.Center, Projectile.Center);
				float maxSearchRange = closestTargetDistance * 3f;
				if (distanceFromTarget < maxSearchRange && !canAttack && Collision.CanHit(Projectile.Center, 1, 1, ownerMinionAttackTargetNPC2.Center, 1, 1))
				{
					closestTargetDistance = distanceFromTarget;
					targetPos = ownerMinionAttackTargetNPC2.Center;
					canAttack = true;
				}
			}
			if (!canAttack)
			{
				for (int i = 0; i < 200; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(Projectile))
					{
						float potentialTargetDistance = Vector2.Distance(npc.Center, Projectile.Center);
						if (!(potentialTargetDistance >= closestTargetDistance) && Collision.CanHit(Projectile.Center, 1, 1, npc.Center, 1, 1))
						{
							closestTargetDistance = potentialTargetDistance;
							targetPos = npc.Center;
							canAttack = true;
						}
					}
				}
			}

			int ownerDistanceCheck = 500;
			if (canAttack)
			{
				ownerDistanceCheck = 1000;
			}

			Player player = Main.player[Projectile.owner];
			if (Vector2.Distance(player.Center, Projectile.Center) > ownerDistanceCheck)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] >= 2f)
			{
				Projectile.ai[0] += 1f;
				if (!canAttack)
				{
					Projectile.ai[0] += 1f;
				}

				float num34 = 40f;
				float slowDownMultiplier = 0.69f;
				if (Projectile.ai[0] > num34)
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				Projectile.velocity *= slowDownMultiplier;
			}
			else
			{
				if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
				{
					Projectile.ai[0] = 1f;
				}

				float num25 = 6f;
				if (Projectile.ai[0] == 1f)
				{
					num25 = 15f;
				}

				Vector2 idlePos = player.Center - Projectile.Center + new Vector2(0, -60f);
				float distanceToIdlePos = idlePos.Length();

				if (distanceToIdlePos > 200f && num25 < 9f)
				{
					num25 = 9f;
				}

				if (distanceToIdlePos < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				if (distanceToIdlePos > 1300f) // Teleport to the player if minion falls behind
				{
					Projectile.position.X = Main.player[Projectile.owner].Center.X - (Projectile.width / 2);
					Projectile.position.Y = Main.player[Projectile.owner].Center.Y - (Projectile.width / 2);
				}
				else if (distanceToIdlePos > 70f)
				{
					idlePos = idlePos.SafeNormalize(Vector2.Zero);
					idlePos *= num25;
					Projectile.velocity = (Projectile.velocity * 19f + idlePos) / 22f;
				}
				else
				{ // If the minion stops, nudge it back to speed
					if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
					{
						Projectile.velocity.X = -0.15f;
						Projectile.velocity.Y = -0.05f;
					}
					Projectile.velocity *= 1.01f;
				}
			}

			Projectile.rotation = Projectile.velocity.X * 0.05f;

			if (Projectile.ai[1] > 0f)
			{
				Projectile.ai[1] += Main.rand.Next(1, 4);
			}

			int num39 = 90;
			if (Projectile.ai[1] > num39)
			{
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] != 0f)
			{
				return;
			}

			// Shoot a projectile at a target
			float shootVelocity = 10f;
			int ProjectileType = ModContent.ProjectileType<GraniteMinionShoot>();

			if (Projectile.ai[1] == 0f && canAttack)
			{
				Vector2 v5 = targetPos - Projectile.Center;
				Projectile.ai[1] += 1f;
				if (Main.myPlayer == Projectile.owner)
				{
					v5 = v5.SafeNormalize(Vector2.Zero);
					v5 *= shootVelocity;
					int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position.X, Projectile.position.Y, v5.X, v5.Y, ProjectileType, Projectile.damage + 4, Projectile.knockBack, Projectile.owner);
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].netUpdate = true;
					Projectile.netUpdate = true;
				}
			}
		}

		private void PreventMinionOverlap()
		{
			float nudge = 0.05f;
			for (int m = 0; m < 1000; m++)
			{
				if (m != Projectile.whoAmI && Main.projectile[m].active && Main.projectile[m].owner == Projectile.owner && Main.projectile[m].type == Projectile.type
					&& Math.Abs(Projectile.position.X - Main.projectile[m].position.X) + Math.Abs(Projectile.position.Y - Main.projectile[m].position.Y) < Projectile.width)
				{
					if (Projectile.position.X < Main.projectile[m].position.X)
					{
						Projectile.velocity.X -= nudge;
					}
					else
					{
						Projectile.velocity.X += nudge;
					}
					if (Projectile.position.Y < Main.projectile[m].position.Y)
					{
						Projectile.velocity.Y -= nudge;
					}
					else
					{
						Projectile.velocity.Y += nudge;
					}
				}
			}
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			// When minion slots are full and the player "respawns" the same minion,
			// the orbiters would not die. Killing them manually seems to work though.
			childProjectile.first.Kill();
			childProjectile.second.Kill();
		}
	}
	#endregion

	#region The projectile that the minion shoots
	public class GraniteMinionShoot : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/MagicProj/SpiritVaseProj";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 12;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 0;
			Projectile.light = 0.25f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;

			Projectile.DamageType = DamageClass.Summon;

			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Purple, 0.25f);

			if (Projectile.ai[0] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item75 with { Pitch = -0.4f });
				Projectile.ai[0]++;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width, Projectile.height, DustID.Clentaminator_Blue, 0, 0, 175, default, 0.75f);
				dust.velocity *= 1.4f;
				dust.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			VertexStrip vertexStrip = new();
			MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(4f);
			miscShaderData.Apply();
			vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
			vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			return true;
		}

		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.MidnightBlue, Color.MidnightBlue, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: false)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 0.1f;
			float lerpValue = Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(0f, 32f, num);
		}
	}
	#endregion
}

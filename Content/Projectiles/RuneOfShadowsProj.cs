using System;
using System.Collections.Generic;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Items.Accessories.Runes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedAdditions.Content.Buffs;
using Terraria.Audio;
using AssortedAdditions.Helpers;

namespace AssortedAdditions.Content.Projectiles
{
	internal class RuneOfShadowsProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 32;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = -1;
			Projectile.light = 0.15f;
			Projectile.damage = 65;
			Projectile.knockBack = 4f;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}

		public float ActiveAbility
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public bool visualToggle = true; // For changing the drawlayer to create a sort of 3D orbit around the player
		public int riseAmount = 0;
		
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			// Activates an ability when a specific key is pressed (only works for the owner)
			// Timer != 0 is to give time for all projectiles to spawn before the ability can be used
			// Won't work during cooldown or if the ability is active
			if (CustomKeyBinds.RuneAbility.JustPressed && Main.myPlayer == Projectile.owner && Timer != 0 
				&& !player.HasBuff(ModContent.BuffType<RuneOfShadowsCooldown>()) && ActiveAbility == 0)
			{
				ActiveAbility = 1;
				Projectile.netUpdate = true;

				// Actual cooldown given when projectile dies
				// ActiveAbility == 0 prevents from spamming already
				// If the cooldown was given here, only one projectile would do the ability
			}

			float speed = 0.8f; // How fast the projectile orbits the player
			float xOffSet = 35; // Offset from the player's center
			float yOffSet = 2; // The orbit is formed with these

			// Check that player is alive and has the buff, ability not active
			if (!player.dead && player.GetModPlayer<RuneOfShadowsPlayer>().isWearingRuneOfShadows)
			{
				// Keep projectile alive
				if(ActiveAbility == 0)
				{
					Projectile.timeLeft = 2;
				}
				
				Timer += 0.05f; // Projectile orbits the player in an ellipse pattern
				Vector2 ellipse = new(xOffSet * (float)Math.Cos(speed * Timer), yOffSet * (float)Math.Sin(speed * Timer));
				Vector2 orbit = player.Center + ellipse;
				
				// If ability is active
				if (ActiveAbility != 0)
				{
					if (ActiveAbility == 1) // Sets these only once
					{
						Projectile.timeLeft = 180;
						Projectile.ai[1] = player.Center.X;
						Projectile.ai[2] = player.Center.Y; // Store the player's position
						Projectile.netUpdate = true;

						SoundStyle abilitySound = new SoundStyle("AssortedAdditions/Assets/Sounds/ProjectileSound/RuneOfShadowsAbility");
						SoundEngine.PlaySound(abilitySound with { MaxInstances = 1}, player.position);
					}

					riseAmount += 7; // Projectile rises up while doing the orbit movement
					orbit = new Vector2(Projectile.ai[1], Projectile.ai[2] - riseAmount) + ellipse;
					// The projectiles will orbit a rising position starting from the position that the ability was activated at
				}
				
				// Finally set the projectile center to the orbit
				Projectile.Center = orbit;
				Projectile.netUpdate = true;
			}
			else
			{ // Rune is no longer equipped + ability is not active = kill the projectile
				if(Main.myPlayer == Projectile.owner && ActiveAbility == 0)
				{
					Projectile.Kill();
				}
			}

			// Safe guard for when projectile goes too high up (out of world)
			// For some reason it stays alive which causes issues so kill it
			if(Projectile.position.Y < 600)
			{
				Projectile.Kill();
			}

			// The projectile orbits the player in a 3D sort of way
			// It goes over the player's body and then goes behind it
			if (player.Center.X - Projectile.Center.X >= xOffSet - 1) // - 1 because sometimes won't reach the assigned value
			{
				visualToggle = false;
			}

			if (player.Center.X - Projectile.Center.X <= -1 * (xOffSet - 1)) // same here, but also -1 * since the distance is negative
			{
				visualToggle = true;
			}

			// Add some lighting
			Lighting.AddLight(Projectile.Center, TorchID.Purple);

			// Loop through the sprite frames
			int frameSpeed = 5;
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= frameSpeed)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			// Dust effect for ability
			if(ActiveAbility != 0)
			{
				Dust dust = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y + 15) - Projectile.velocity, 
					DustID.Shadowflame, null, 75, default, 1.5f); // +15 to Y to start the dust trail from below the projectile instead of its center
				dust.noGravity = true;
				dust.velocity *= 0;
			}
		}

		public override void OnKill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<RuneOfShadowsPlayer>().canSpawnProjectile = true; // Allow the projectiles to respawn if the player respawns after dying or re-equips the rune
			player.AddBuff(ModContent.BuffType<RuneOfShadowsCooldown>(), 180); // TODO increase to 3 minutes once done

			// Only one can be spawned
			if (HelperMethods.CountProjectiles(ModContent.ProjectileType<RuneOfShadowsAbilityControl>(), player.whoAmI, true) == 0) 
			{
				float x = Projectile.ai[1];
				float y = Projectile.ai[2] - 1800;

				if(y < 660) // Don't spawn this outside of the world border
				{
					y = 660;
				}

				// Use stored position of where ability was activated for the spawn position. Should be high up from that spot.
				if(Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(x, y), Vector2.Zero,
						ModContent.ProjectileType<RuneOfShadowsAbilityControl>(), 0, 0, player.whoAmI);
				}
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (visualToggle)
			{
				overPlayers.Add(index); // Going over the player
			}
			else
			{
				overPlayers.Remove(index); // Going behind the player
			}
		}

		private static readonly VertexStrip _vertexStrip = new();

		public override bool PreDraw(ref Color lightColor)
		{
            if (ActiveAbility != 0)
            {
				MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
				miscShaderData.UseSaturation(-2.8f);
				miscShaderData.UseOpacity(4f);
				miscShaderData.Apply();
				_vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
				_vertexStrip.DrawTrail();
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}

			return true;
		}

		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.DarkOrchid, Color.Magenta, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 3.5f;
			float lerpValue = Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(16f, 32f, num);
		}
	}

	/// <summary>
	/// Helper projectile, handles the ability projectile spawns
	/// </summary>
	public class RuneOfShadowsAbilityControl : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Projectiles/SkeletonPotionProj"; // Doesn't matter will be invisible

		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;

			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.friendly = true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public float XPos
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}

		public int timer = 0;
		public const int MAX_TIME = 540;

		public override void AI()
		{
			if(timer <= MAX_TIME && timer % 4 == 0)
			{
				// Pick a random X position from this projectile's position to spawn ability projectile at
				// Y will be this projectile's Y position
				XPos = Main.rand.NextFloat(Projectile.position.X - 200, Projectile.position.X + 200);
				Projectile.netUpdate = true;

				if(Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(XPos, Projectile.position.Y), Vector2.Zero, 
						ModContent.ProjectileType<RuneOfShadowsAbility>(), 80, 0f, Projectile.owner);
				}
			}

			if(timer > MAX_TIME)
			{
				Projectile.Kill();
			}

			timer++;
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AssortedAdditions.Helpers;

namespace AssortedAdditions.Content.Projectiles.RangedProj;

internal class GuidedMissileProj : ModProjectile
{
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 14;
        Projectile.aiStyle = 0; // Custom AI style
        Projectile.timeLeft = 300; // 5 second life
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.friendly = true;
        Projectile.hostile = false;
    }

	public override void AI()
    {
        float maxDetectRadius = 400f; // The maximum radius at which a projectile can detect a target

        if (Main.rand.NextBool(1))
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.WhiteTorch, 0, 0, 100, Color.Gray, 2f);
            dust.noGravity = true;

            if (Main.rand.NextBool(3))
            {
                Dust dustfire = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.Flare, 0, 0, 100, Color.Orange, 2f);
                dustfire.noGravity = true;
            }
        }

		Player player = Main.player[Projectile.owner];
        if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
        {
            // If the player channels the weapon, follow the cursor
            // Projectile check only works if item.channel is true for the weapon.
            if (player.channel)
            {
                Projectile.ai[2] = (Main.MouseWorld - Projectile.Center).ToRotation();
                Projectile.netUpdate = true;

                // With these the projectile will move towards the target (the cursor) in a smooth way
                float curve = Projectile.velocity.ToRotation();
                float maxTurn = MathHelper.ToRadians(8f); // Adjusting Projectile affects the speed at which the projectile curves at the target
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(Projectile.ai[2], maxTurn)) - curve);
                // TODO fix movement being choppy in multiplayer
			}
            else
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;
            }
        }
		// If the player stops channeling, the projectile tries to home in on a target (if there are none it continues in the direction it was heading)
		else if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 1f)
		{
			NPC closestNPC = HelperMethods.FindClosestNPC(Projectile.Center, maxDetectRadius);

			if (closestNPC != null) // If no target is found, missile flies off
			{
				Projectile.ai[2] = (closestNPC.Center - Projectile.Center).ToRotation();
                Projectile.netUpdate = true;
				float curve = Projectile.velocity.ToRotation();
				float maxTurn = MathHelper.ToRadians(4f);
                Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(Projectile.ai[2], maxTurn)) - curve);
            }
        }

        // Set the rotation so the projectile points towards where it's going.
        Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.netUpdate = true;
	}

    public override void OnKill(int timeLeft)
    {
        // If the projectile dies without hitting an enemy, crate a small explosion that hits all enemies in the area.
        if (Projectile.penetrate == 1)
        {
            // Makes the projectile hit all enemies as it circunvents the penetrate limit.
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;

            int explosionArea = 60;
            // Resize the projectile hitbox to be bigger.
            Projectile.position = Projectile.Center;
            Projectile.Size += new Vector2(explosionArea);
            Projectile.Center = Projectile.position;

            Projectile.tileCollide = false;
            Projectile.velocity *= 0.01f;
            // Damage enemies inside the hitbox area
            Projectile.Damage();
            Projectile.scale = 0.01f;

            //Resize the hitbox to its original size
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(10);
            Projectile.Center = Projectile.position;
        }

        SoundEngine.PlaySound(SoundID.Item62, Projectile.position);

        // Smoke Dust
        for (int i = 0; i < 30; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
            dust.velocity *= 1.4f;
        }

        // Fire Dust
        for (int i = 0; i < 40; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
            dust.noGravity = true;
            dust.velocity *= 5f;
            dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
            dust.velocity *= 3f;
            dust.noGravity = true;
        }
    }

}

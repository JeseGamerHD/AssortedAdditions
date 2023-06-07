using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ModdingTutorial.Content.Projectiles.RangedProj;

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
        DrawOriginOffsetY = 0;  
        DrawOriginOffsetX = 0;
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

        if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
        {
            Player player = Main.player[Projectile.owner];
            // If the player channels the weapon, follow the cursor
            // This check only works if item.channel is true for the weapon.
            if (player.channel)
            {
                float maxDistance = 10f; // This also sets the maximun speed the projectile can reach while following the cursor.
                Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
                float distanceToCursor = vectorToCursor.Length();

                // Here we can see that the speed of the projectile depends on the distance to the cursor.
                if (distanceToCursor > maxDistance)
                {
                    distanceToCursor = maxDistance / distanceToCursor;
                    vectorToCursor *= distanceToCursor;
                }

                int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
                int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
                int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
                int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

                // This code checks if the precious velocity of the projectile is different enough from its new velocity,
                // and if it is, syncs it with the server and the other clients in MP.
                // We previously multiplied the speed by 1000, then casted it to int,
                // this is to reduce its precision and prevent the speed from being synced too much.
                if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000)
                {
                    Projectile.netUpdate = true;
                }
                Projectile.velocity = vectorToCursor;
            }
            // If the player stops channeling, the projectile tries to home in on a target
            else if (Projectile.ai[0] == 0f)
            {
                Projectile.netUpdate = true;

                NPC closestNPC = FindClosestNPC(maxDetectRadius);
                
                if (closestNPC == null) // If no target is found, missile flies off
                {
                    if(Projectile.velocity == Vector2.Zero)
                    {
                        Vector2 vectorToCursor = Projectile.Center - player.Center;
                        float distanceToCursor = vectorToCursor.Length();
                        distanceToCursor = 18f / distanceToCursor;
                        vectorToCursor *= distanceToCursor;
                        Projectile.velocity = vectorToCursor;
                    }

                    return;
                }

                // If found, change the velocity of the projectile and turn it in the direction of the target
                Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f;
                Projectile.rotation = Projectile.velocity.ToRotation();

                Projectile.ai[0] = 1f;
            }
        }

        // Set the rotation so the projectile points towards where it's going.
        if (Projectile.velocity != Vector2.Zero)
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }

    public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
        Projectile.Kill();
    }

    public NPC FindClosestNPC(float maxDetectDistance)
    {
        NPC closestNPC = null;

        // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
        float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

        // Loop through all NPCs(max always 200)
        for (int k = 0; k < Main.maxNPCs; k++)
        {
            NPC target = Main.npc[k];

            if (target.CanBeChasedBy())
            {
                // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                // Check if it is within the radius
                if (sqrDistanceToTarget < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToTarget;
                    closestNPC = target;
                }
            }
        }

        return closestNPC;
    }

    public override void Kill(int timeLeft)
    {
        // If the projectile dies without hitting an enemy, crate a small explosion that hits all enemies in the area.
        if (Projectile.penetrate == 1)
        {
            // Makes the projectile hit all enemies as it circunvents the penetrate limit.
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;

            int explosionArea = 60;
            Vector2 oldSize = Projectile.Size;
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

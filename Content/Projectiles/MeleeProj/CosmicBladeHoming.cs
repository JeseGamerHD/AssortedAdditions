using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.ID;

namespace AssortedAdditions.Content.Projectiles.MeleeProj
{
    internal class CosmicBladeHoming : ModProjectile
    {
        public override void SetStaticDefaults() // These are needed for shader trail
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Melee;
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            float range = 600f; // Max radius that the projectile can detect a target
            Timer++; // Timer to determine when to start homing

            // Homing doesn't work instantly
            // Otherwise the projectiles would just stack on top of each other
            if (Timer < 30)
            {
                // Face towards where its going
                Projectile.rotation = Projectile.velocity.ToRotation();
                return; // Stop here if timer is not over 45
            }

            NPC closestNPC = FindClosestNPC(range);
            if (closestNPC == null)
            {
                // Face towards where its going
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }

            // With these the projectile will move towards the target in a smooth way
            // instead of snapping it moves in a curved way
            float target = (closestNPC.Center - Projectile.Center).ToRotation();
            float curve = Projectile.velocity.ToRotation();
            float maxTurn = MathHelper.ToRadians(3f);

            // Set the velocity and rotation:
            Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(target, maxTurn)) - curve);
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        // Finding the closest NPC to attack within range
        private NPC FindClosestNPC(float range)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = range * range;

            // Loop through all NPCs(max always 200)
            for (int k = 0; k < Main.maxNPCs; k++)
            {
                NPC target = Main.npc[k];
                // Check if NPC able to be targeted. It means that NPC is
                // 1. active (alive)
                // 2. chaseable (e.g. not a cultist archer)
                // 3. max life bigger than 5 (e.g. not a critter)
                // 4. can take damage (e.g. moonlord core after all it's parts are downed)
                // 5. hostile (!friendly)
                // 6. not immortal (e.g. not a target dummy)
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

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
        }

        // The cool trail effect happens under here:

        private static readonly VertexStrip _vertexStrip = new();

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"]; // Basically a copy of rainbow rod with its colors changed
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        private Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Indigo, Color.Indigo, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }
    }
}

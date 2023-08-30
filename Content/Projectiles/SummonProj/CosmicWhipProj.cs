using ModdingTutorial.Content.Buffs;
using Terraria.GameContent.Drawing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace ModdingTutorial.Content.Projectiles.SummonProj
{
    internal class CosmicWhipProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // This makes the projectile use whip collision detection and allows flasks to be applied to it.
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            // This method quickly sets the whip's properties.
            Projectile.DefaultToWhip();

            // These values needed to be adjusted a bit
            Projectile.WhipSettings.Segments = 30;
            Projectile.WhipSettings.RangeMultiplier = 1f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MotivatorDebuff>(), 240);
            target.AddBuff(BuffID.ShadowFlame, 120);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.5f); // Multihit penalty. Decrease the damage the more enemies the whip hits.

            // Creates cool sparkly effect when enemy is hit, vanilla has many effects -> ParticleOrchestraType.Effect
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.NightsEdge,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);
        }

        public override void PostAI()
        {
            Player owner = Main.player[Projectile.owner];
            float swingTime = owner.itemAnimationMax * Projectile.MaxUpdates;

            // Spawn Dust along the whip path
            // This is the dust code used by Durendal. Consult the Terraria source code for even more examples, found in Projectile.AI_165_Whip.
            float swingProgress = Timer / swingTime;
            // This code limits dust to only spawn during the the actual swing.
            if (Utils.GetLerpValue(0.1f, 0.7f, swingProgress, clamped: true) * Utils.GetLerpValue(0.9f, 0.7f, swingProgress, clamped: true) > 0.5f && Main.rand.NextBool(1))
            {
                List<Vector2> points = Projectile.WhipPointsForCollision;
                points.Clear();
                Projectile.FillWhipControlPoints(Projectile, points);
                int pointIndex = Main.rand.Next(points.Count - 10, points.Count);
                Rectangle spawnArea = Utils.CenteredRectangle(points[pointIndex], new Vector2(30f, 30f));
                int dustType = DustID.PurpleTorch;

                // After choosing a randomized dust and a whip segment to spawn from, dust is spawned.
                Dust dust = Dust.NewDustDirect(spawnArea.TopLeft(), spawnArea.Width, spawnArea.Height, dustType, 0f, 0f, 100, default, 3f);
                dust.position = points[pointIndex];
                dust.fadeIn = 0.3f;
                dust.noGravity = true;

                // Also spawn cool sparkles
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.SilverBulletSparkle,
                    new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(spawnArea) },
                    Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            //Main.DrawWhip_WhipBland(Projectile, list);
            // The code below is for custom drawing.
            // If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
            // However, you must adhere to how they draw if you do.

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Main.instance.LoadProjectile(Type);
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                // These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
                // You can change them if they don't!
                Rectangle frame = new Rectangle(0, 0, 18, 24); // The size of the Handle (measured in pixels)
                Vector2 origin = new Vector2(5, 8); // Offset for where the player's hand will start measured from the top left of the image.
                float scale = 1;

                // These statements determine what part of the spritesheet to draw for the current segment.
                // They can also be changed to suit your sprite.
                if (i == list.Count - 2)
                {
                    // This is the head of the whip. You need to measure the sprite to figure out these values.
                    frame.Y = 70; // Distance from the top of the sprite to the start of the frame.
                    frame.Height = 22; // Height of the frame.

                    // For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    // Third segment
                    frame.Y = 56;
                    frame.Height = 14;
                }
                else if (i > 5)
                {
                    // Second Segment
                    frame.Y = 40;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    // First Segment
                    frame.Y = 24;
                    frame.Height = 16;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}

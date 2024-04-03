using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class JungleChakramProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3; // The recording mode
            Main.projFrames[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.penetrate = -1;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            AIType = ProjectileID.LightDisc;
        }

        private bool setOnce;
        public override void AI()
        {
            if (!setOnce)
            {
                if (Main.rand.NextBool())
                {
                    Projectile.frame = 0;
                    setOnce = true;
                }
                else
                {
                    Projectile.frame = 1;
                    setOnce = true;
                }
            }

            if (Main.rand.NextBool(5))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleGrass, 0, 0, 150, default, 1.5f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.JungleSpore, 0, 0, 150, default, 1f);
                dust2.noGravity = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
            SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

			// Use these to limit the trail to one frame, without this the whole spritesheet would draw
			int frameHeight = texture.Value.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Value.Width, frameHeight);

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}

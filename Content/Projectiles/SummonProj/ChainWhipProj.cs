﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Buffs;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.GameContent;
using AssortedAdditions.Content.Buffs.Whips;
using ReLogic.Content;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
    internal class ChainWhipProj : ModProjectile
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
            Projectile.WhipSettings.RangeMultiplier = 0.75f;
        }

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<ChainWhipDebuff>(), 240);
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
            Projectile.damage = (int)(Projectile.damage * 0.5f); // Multihit penalty. Decrease the damage the more enemies the whip hits.

            target.AddBuff(BuffID.Bleeding, 240); // Doesn't do anything

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height,
                    DustID.Blood, 0, 0, 100, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            List<Vector2> list = new List<Vector2>();
            Projectile.FillWhipControlPoints(Projectile, list);

            SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

			Vector2 pos = list[0];

            for (int i = 0; i < list.Count - 1; i++)
            {
                // These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
                // You can change them if they don't!
                Rectangle frame = new Rectangle(0, 0, 10, 26); // The size of the Handle (measured in pixels)
                Vector2 origin = new Vector2(5, 8); // Offset for where the player's hand will start measured from the top left of the image.
                float scale = 1;

                // These statements determine what part of the spritesheet to draw for the current segment.
                // They can also be changed to suit your sprite.
                if (i == list.Count - 2)
                {
                    // This is the head of the whip. You need to measure the sprite to figure out these values.
                    frame.Y = 72; // Distance from the top of the sprite to the start of the frame.
                    frame.Height = 16; // Height of the frame.

                    // For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
                    Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                    float t = Timer / timeToFlyOut;
                    scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
                }
                else if (i > 10)
                {
                    // Third segment
                    frame.Y = 58;
                    frame.Height = 16;
                }
                else if (i > 5)
                {
                    // Second Segment
                    frame.Y = 42;
                    frame.Height = 16;
                }
                else if (i > 0)
                {
                    // First Segment
                    frame.Y = 26;
                    frame.Height = 16;
                }

                Vector2 element = list[i];
                Vector2 diff = list[i + 1] - element;

                float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
                Color color = Lighting.GetColor(element.ToTileCoordinates());

                Main.EntitySpriteDraw(texture.Value, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

                pos += diff;
            }
            return false;
        }
    }
}

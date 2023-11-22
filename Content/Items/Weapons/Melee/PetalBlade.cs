using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using AssortedAdditions.Content.Projectiles.MeleeProj;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
    internal class PetalBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 70;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.damage = 60;
            Item.knockBack = 6f;
            Item.shootSpeed = 7f;

            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 8);
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<PetalBladeProj>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            int amountToShoot = Main.rand.Next(1, 4);

            for (int i = 0; i < amountToShoot; i++)
            {
                // Rotate the velocity randomly.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(30));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, type, 20, 4, player.whoAmI);
            }

            SoundEngine.PlaySound(SoundID.Item17, player.position);

            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            int dustType;
            if (Main.rand.NextBool(3))
            {
                if (Main.rand.NextBool())
                {
                    dustType = DustID.JungleGrass;
                }
                else
                {
                    dustType = DustID.JungleSpore;
                }

                Dust dust = Dust.NewDustDirect(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height,
                    dustType, 0, 0, 0, default, 1f);
                dust.noGravity = true;
            }
        }
    }
}

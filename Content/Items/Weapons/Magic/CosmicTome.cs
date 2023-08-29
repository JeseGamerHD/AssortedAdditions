using ModdingTutorial.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class CosmicTome : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.damage = 85;
            Item.knockBack = 4f;
            Item.shootSpeed = 13;
            Item.mana = 8;

            Item.autoReuse = true;

            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item9;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 10);
            Item.shoot = ModContent.ProjectileType<CosmicTomeProj>();
        }

        // The projectile spawns in the sky and falls towards the cursor position when it was fired
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }

            position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
            position.Y -= 100;
            Vector2 heading = target - position;

            if (heading.Y < 0f)
            {
                heading.Y *= -1f;
            }

            if (heading.Y < 20f)
            {
                heading.Y = 20f;
            }

            heading.Normalize();
            heading *= velocity.Length();
            heading.Y += Main.rand.Next(-40, 41) * 0.02f;
            Projectile.NewProjectile(source, position, heading, type, damage, knockback, player.whoAmI, 0f, ceilingLimit);

            return false;
        }
    }
}

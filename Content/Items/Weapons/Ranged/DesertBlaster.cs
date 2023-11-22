using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class DesertBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 57;
            Item.height = 37;
            Item.damage = 20;
            Item.crit = 4;
            Item.knockBack = 2;
            Item.useAnimation = 7;
            Item.useTime = 7;
            Item.shootSpeed = 18;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Pink;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.value = Item.sellPrice(gold: 5);
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.PurificationPowder; // Why this? No clue... 
        }

        public override Vector2? HoldoutOffset() => new(5, 8); // Used for alligning the sprite

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Makes the bullets slightly inaccurate (bullet spread)
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(3));
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        // Can alternatively use rockets
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.damage = 40;
                Item.autoReuse = true;
                Item.useAnimation = 24;
                Item.useTime = 24;
                Item.useAmmo = AmmoID.Rocket;
                Item.shoot = ProjectileID.RocketI;
            }
            else
            {
                Item.damage = 20;
                Item.autoReuse = true;
                Item.useAnimation = 7;
                Item.useTime = 7;
                Item.useAmmo = AmmoID.Bullet;
                Item.shoot = ProjectileID.PurificationPowder;
            }

            return base.CanUseItem(player);
        }
    }
}

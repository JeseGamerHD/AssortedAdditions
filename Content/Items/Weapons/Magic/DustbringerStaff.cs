using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.MagicProj;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class DustbringerStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.damage = 66;
            Item.crit = 4;
            Item.knockBack = 7;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 7;
            Item.mana = 10;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
            Item.UseSound = SoundID.DD2_BetsyWindAttack;
            Item.shoot = ModContent.ProjectileType<DustbringerStaffProj>();

            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new(-15, 0); // Alligns the sprite properly
    }
}

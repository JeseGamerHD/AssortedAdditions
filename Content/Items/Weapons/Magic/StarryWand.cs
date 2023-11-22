using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Projectiles.MagicProj;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class StarryWand : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.damage = 18;
            Item.crit = 6;
            Item.knockBack = 3;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.shootSpeed = 6;
            Item.mana = 4;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 20);
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<StarryWandProj>();

            Item.autoReuse = true;
            Item.noMelee = true;
        }
    }
}

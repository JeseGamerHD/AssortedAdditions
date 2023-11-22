using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Projectiles.MagicProj;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
    internal class StoneWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 25;
            Item.knockBack = 8f;
            Item.useAnimation = 35;
            Item.useTime = 35;
            Item.shootSpeed = 18;
            Item.mana = 8;

            Item.autoReuse = true;
            Item.noMelee = true;

            Item.value = Item.sellPrice(silver: 15);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item88;
            Item.shoot = ModContent.ProjectileType<StoneWandProj>();
        }
    }
}

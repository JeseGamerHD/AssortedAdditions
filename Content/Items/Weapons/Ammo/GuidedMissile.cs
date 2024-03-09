using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ammo
{
    internal class GuidedMissile : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 20;
            Item.maxStack = 9999;
            Item.knockBack = 4f;

            Item.consumable = true;

            Item.value = Item.sellPrice(0, 0, 2, 0);
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<GuidedMissileProj>();
            Item.ammo = Item.type;
        }
    }
}

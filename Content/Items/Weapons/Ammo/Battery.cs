﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ammo
{
    internal class Battery : ModItem // Ammo for different energy weapons
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 28;
            Item.damage = 0; // Won't add any damage
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(copper: 20);
            Item.DamageType = DamageClass.Ranged;
            Item.consumable = true;
            Item.rare = ItemRarityID.Blue;
            Item.knockBack = 0;
            Item.shoot = ModContent.ProjectileType<PlasmaCarbineProj>();

            Item.ammo = Item.type;
        }
    }
}

﻿using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    internal class DraconicChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2, silver: 65);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.07f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DragonScale>(), 12);
            recipe.AddIngredient(ItemID.PalladiumBar, 24);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DragonScale>(), 12);
            recipe2.AddIngredient(ItemID.CobaltBar, 24);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}

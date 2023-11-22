using Terraria.GameContent.Creative;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    internal class DraconicGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2, silver: 50);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 0.08f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DragonScale>(), 9);
            recipe.AddIngredient(ItemID.PalladiumBar, 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DragonScale>(), 9);
            recipe2.AddIngredient(ItemID.CobaltBar, 18);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.Register();
        }
    }
}

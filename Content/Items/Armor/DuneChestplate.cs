using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    internal class DuneChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("7% increased ranged critical strike chance");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetCritChance(DamageClass.Ranged) += 0.07f; // 7% increased ranged crit chance
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

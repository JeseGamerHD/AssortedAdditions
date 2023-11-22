using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class SteelHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {

            // Tooltip.SetDefault("A helmet made out of steel");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 70);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<SteelBreastplate>();
            bool legsMatch = legs.type == ModContent.ItemType<SteelGreaves>();

            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Set bonus: 6 defense";
            player.statDefense += 6;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<SteelBar>(), 20);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

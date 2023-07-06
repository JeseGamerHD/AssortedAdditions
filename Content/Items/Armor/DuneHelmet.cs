using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using ModdingTutorial.Content.Items.Placeables;

namespace ModdingTutorial.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class DuneHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("7% increased ranged damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 1, silver: 40);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }

        // Add armor piece specific buffs inside this method
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.07f; // 7% increase in ranged damage
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<DuneChestplate>();
            bool legsMatch = legs.type == ModContent.ItemType<DuneGreaves>();

            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Set bonus: 10% Ranged damage";
            player.statDefense += 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

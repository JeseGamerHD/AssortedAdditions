using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Common.Players;

namespace ModdingTutorial.Content.Items.Accessories
{
    internal class Medkit : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Set a flag to true, ModPlayer will keep player from dying (only once) and activate a buff which also acts as a cooldown
            player.GetModPlayer<AccessoryFlags>().isWearingMedkit = true; 
            player.buffImmune[BuffID.Rabies] = true;
            player.buffImmune[BuffID.Bleeding] = true;

            // Increase max run speed manually
            if (player.statLife <= player.statLifeMax2 * 0.7f && player.statLife > player.statLifeMax2 * 0.5f)
            {
                player.maxRunSpeed += 1;
            }
            else if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.maxRunSpeed += 2;
            }

            // The amount of stat increase is tied to health so create a formula:
            // (max health - current health) / max health ===> (missing health / max health)
            // Important to have (float) since statsLifes are int, only one cast required since it forces the rest
            float healthRatio = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2;

            // Now multiply the formula by the max increase that the stat can have
            // E.g. max 25% increase in attack speed (0.25)
            player.GetAttackSpeed(DamageClass.Generic) += healthRatio * 0.25f;
            player.moveSpeed += healthRatio * 0.25f;
        }

        // Can't be equipped with some accessories to prevent insane speeds
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ItemID.PanicNecklace || incomingItem.type == ItemID.SweetheartNecklace)
            {
                return false;
            }

            return incomingItem.type != ModContent.ItemType<Adrenaline>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MedicatedDressing>());
            recipe.AddIngredient(ModContent.ItemType<Adrenaline>());
            recipe.AddIngredient(ModContent.ItemType<Antidote>());
            recipe.AddIngredient(ItemID.GreaterHealingPotion);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ItemID.MedicatedBandage);
            recipe2.AddIngredient(ModContent.ItemType<Adrenaline>());
            recipe2.AddIngredient(ModContent.ItemType<Antidote>());
            recipe2.AddIngredient(ItemID.GreaterHealingPotion);
            recipe2.AddTile(TileID.TinkerersWorkbench);
            recipe2.Register();
        }
    }
}

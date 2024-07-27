using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Projectiles.PetProj;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Buffs.Pets;

namespace AssortedAdditions.Content.Items.Pets
{
    internal class IlluminatedCrystal : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.DefaultToVanitypet(ModContent.ProjectileType<IlluminatedCrystalPet>(), ModContent.BuffType<IlluminatedCrystalBuff>());
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 1);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Buff applied here, the buff then spawns the pet projectile
            player.AddBuff(Item.buffType, 2);

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrystalShard, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ModContent.ItemType<MagicEssence>());
            recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
            recipe.Register();
        }
    }
}

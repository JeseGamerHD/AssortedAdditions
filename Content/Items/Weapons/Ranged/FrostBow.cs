using ModdingTutorial.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class FrostBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 70;
            Item.damage = 75;
            Item.knockBack = 1;
            Item.shootSpeed = 14;
            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item5;
            Item.value = Item.sellPrice(gold: 5);
            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<FrostBowProj>();
        }

        public override Vector2? HoldoutOffset() => new(-8, 0); // Alligns the sprite properly

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Always shoots custom projectile no matter which arrow was used
            type = ModContent.ProjectileType<FrostBowProj>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 12);
            recipe.AddIngredient(ModContent.ItemType<IceEssence>(), 8);
            recipe.AddTile(TileID.IceMachine);
            recipe.Register();
        }
    }
}

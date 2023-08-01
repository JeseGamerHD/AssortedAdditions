using ModdingTutorial.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class IcyStaff : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 69;
            Item.height = 68;
            Item.damage = 48;
            Item.knockBack = 0;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.shootSpeed = 14;
            Item.mana = 5;
            Item.scale = 0.9f;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 4);
            Item.UseSound = SoundID.Item9;
            Item.shoot = ModContent.ProjectileType<IcyWandProj>();

            Item.autoReuse = true;
            Item.noMelee = true;
        }

        public override Vector2? HoldoutOffset() => new(-15, 0); // Used for alligning the sprite

        // Makes the projectiles have a spread
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
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

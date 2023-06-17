using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.RangedProj;
using ModdingTutorial.Content.Items.Weapons.Ammo;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class PlasmaCarbine : ModItem
    {
        public override void SetStaticDefaults() 
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Tooltip.SetDefault("Battery powered");
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.height = 50;
            Item.autoReuse = true;

            Item.damage = 65;
            Item.crit = 6;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4;
            Item.noMelee = true;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);

            Item.shootSpeed = 10;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.scale = 0.75f; // Sprite needs to be scaled down a little

            Item.UseSound = SoundID.Item33; // Needs custom sound
            Item.shoot = ModContent.ProjectileType<PlasmaCarbineProj>();
            Item.useAmmo = ModContent.ItemType<Battery>();
        }

        public override Vector2? HoldoutOffset() => new(-23, 5); // Used for alligning the sprite

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset; // Projectiles come out of the muzzle properly using this
            }
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.98f; // Small chance to consume ammo
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Musket, 1);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.SoulofLight, 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

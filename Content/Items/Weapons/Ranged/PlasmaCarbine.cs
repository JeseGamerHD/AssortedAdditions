using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.RangedProj;
using AssortedAdditions.Content.Items.Weapons.Ammo;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class PlasmaCarbine : ModItem
    {

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

            Item.rare = ItemRarityID.Yellow;
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
            return Main.rand.NextBool(2, 10);
        }

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Asset<Texture2D> texture = TextureAssets.Item[Item.type];

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture.Value);
			}

			// Draw item with 0.75 scale (when dropped)
			Main.spriteBatch.Draw(texture.Value, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.75f, SpriteEffects.None, 0f);

			return false;
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

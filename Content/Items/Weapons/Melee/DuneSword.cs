using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Items.Placeables.Ores;
using Terraria.DataStructures;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
    internal class DuneSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
			Item.useTime = 20;
            Item.useAnimation = 20;
			Item.damage = 40;
			Item.knockBack = 2;
            Item.crit = 6;

			Item.autoReuse = true;
			Item.useTurn = true;
            //Item.ChangePlayerDirectionOnShoot = true;
            //Item.shoot = ProjectileID.PurificationPowder;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.value = Item.sellPrice(silver: 54);
        }

/*		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            return false;
		}*/

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            //Emit dusts when the sword is swung
            int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.YellowTorch, 0, 0, 100, Color.Yellow, 1f);
            Main.dust[dust].noGravity = true;
        }
	}
}

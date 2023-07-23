using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Weapons.Melee
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

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 40;
            Item.knockBack = 2;
            Item.crit = 6;

            Item.value = Item.sellPrice(silver: 54);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
        }

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

using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Projectiles.MeleeProj;
using Terraria.Audio;
using ModdingTutorial.Content.Items.Placeables.Ores;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class DuneSpear : ModItem
    {
        public override void SetStaticDefaults()
        {

            ItemID.Sets.SkipsInitialUseSound[Item.type] = true; // Sound is played in UseItem() instead
            ItemID.Sets.Spears[Item.type] = true; // Item is a spear
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;
            Item.useTime = 28;
            Item.useAnimation = 22;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.autoReuse = true;

            Item.value = Item.sellPrice(silver: 58);
            Item.rare = ItemRarityID.Orange;

            Item.damage = 30;
            Item.knockBack = 7f;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;

            Item.shootSpeed = 5f;
            Item.shoot = ModContent.ProjectileType<DuneSpearProj>();
        }

        public override bool CanUseItem(Player player)
        { // Only one spear can exists at a time
            return player.ownedProjectileCounts[Item.shoot] < 1;

        }

        public override bool? UseItem(Player player)
        {
            // Because we're skipping sound playback on use animation start, we have to play it ourselves whenever the item is actually used.
            if (!Main.dedServ && Item.UseSound.HasValue)
            {
                SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
            }

            return null;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DuneBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

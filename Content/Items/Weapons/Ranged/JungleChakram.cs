using ModdingTutorial.Content.Projectiles.RangedProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Weapons.Ranged
{
    internal class JungleChakram : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 44;
            Item.damage = 75;
            Item.knockBack = 8f;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.shootSpeed = 16f;
            Item.crit = 6;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.noUseGraphic = true;

            Item.value = Item.sellPrice(gold: 8);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<JungleChakramProj>();
        }
    }
}

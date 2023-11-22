using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedAdditions.Content.Projectiles.RangedProj;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class Dunerang : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.autoReuse = true;
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7;
            Item.crit = 6;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.shootSpeed = 12f;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item7;
            Item.value = Item.sellPrice(gold: 1);
            Item.noUseGraphic = true;

            Item.shoot = ModContent.ProjectileType<DunerangProj>();
        }

        // Only one can be shot at a time
        // New one can be thrown once the first one has returned
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1;
        }
    }
}

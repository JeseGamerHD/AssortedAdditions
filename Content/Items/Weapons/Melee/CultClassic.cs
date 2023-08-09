using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.Projectiles.MeleeProj;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class CultClassic : ModItem
    {
        public override void SetStaticDefaults()
        { 
            // These are all related to gamepad controls and don't seem to affect anything else
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 20;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true; // Required, but unused
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 26;
            Item.useTime = 25; // Vanilla yoyo's have this at 25
            Item.useAnimation = 25; // ^^
            Item.damage = 140;
            Item.knockBack = 4.5f;
            Item.shootSpeed = 16f;

            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.MeleeNoSpeed; // MeleeNoSpeed means the item will not scale with attack speed.
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.sellPrice(gold: 8);
            Item.shoot = ModContent.ProjectileType<CultClassicProj>();
        }
    }
}

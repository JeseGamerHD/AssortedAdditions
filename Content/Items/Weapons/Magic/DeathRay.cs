using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Projectiles.MagicProj;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class DeathRay : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.channel = true; //Channel so that you can held the weapon [Important]
            Item.mana = 7;
            Item.rare = ItemRarityID.Cyan;
            Item.width = 50;
            Item.height = 40;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item13;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shootSpeed = 14f;
            Item.useAnimation = 20;
            Item.shoot = ModContent.ProjectileType<DeathRayProj>();
            Item.value = Item.sellPrice(gold: 10);
        }

        public override Vector2? HoldoutOffset() => new(-1, 1); // Alligns the sprite properly
    }
}

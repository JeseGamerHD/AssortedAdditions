using ModdingTutorial.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace ModdingTutorial.Content.Items.Weapons.Magic
{
    internal class SporeRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 45;
            Item.knockBack = 0;
            Item.useAnimation = 28;
            Item.useTime = 28;
            Item.shootSpeed = 8;
            Item.mana = 9;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Magic;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 8);
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<SporeOrbBig>();

            Item.autoReuse = true;
            Item.noMelee = true;
        }
    }
}

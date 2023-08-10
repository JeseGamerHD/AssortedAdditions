using Terraria.ID;
using Terraria.ModLoader;
using ModdingTutorial.Content.Projectiles.SummonProj;

namespace ModdingTutorial.Content.Items.Weapons.Summon
{
    internal class Motivator : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<MotivatorProj>(), 190, 3, 10);
            Item.rare = ItemRarityID.Yellow;
        }

        // Makes the whip receive melee prefixes
        public override bool MeleePrefix()
        {
            return true;
        }
    }
}

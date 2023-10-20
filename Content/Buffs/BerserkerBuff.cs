using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs
{
    internal class BerserkerBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
        }
    }
}

using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs
{
    internal class WardingPotionBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
        }
    }
}

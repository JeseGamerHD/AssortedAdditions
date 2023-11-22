using AssortedAdditions.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Buffs
{
    internal class EchoChamberBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (!player.GetModPlayer<AccessoryFlags>().isWearingEchoChamber)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 18000;
            }
        }
    }
}

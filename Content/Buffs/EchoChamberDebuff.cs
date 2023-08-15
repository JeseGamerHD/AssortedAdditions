using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs
{
    internal class EchoChamberDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; // Important to remember for debuffs
        }
    }
}

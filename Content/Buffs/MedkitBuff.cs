using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Buffs
{
    internal class MedkitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 3;
        }

    }
}

using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.Players
{
    internal class ArmorSetBuffs : ModPlayer
    {
        public int DraconicArmorBuff; // Used as a timer for spawning projectiles

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            DraconicArmorBuff = 0;
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }
}

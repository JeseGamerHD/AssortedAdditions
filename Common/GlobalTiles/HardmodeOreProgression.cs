using AssortedAdditions.Common.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalTiles
{
    // The hardmode ore progression is different. Only palladium/cobalt can be mined after Wall of Flesh
    // Orichalcum/Mythril can be mined after defeating one mech boss
    // Adamantite/Titanium can be mined after defeating The Destroyer and The Twins
    internal class HardmodeOreProgression : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            // The player can toggle this on/off if they want to use vanilla progression
            if (ModContent.GetInstance<VanillaChangeToggle>().HardmodeOreProgressionToggle)
            {
                switch (type)
                {
                    case TileID.Mythril:
                        if (Condition.DownedMechBossAny.IsMet())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case TileID.Orichalcum:
                        if (Condition.DownedMechBossAny.IsMet())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case TileID.Adamantite:
                        if (NPC.downedMechBoss1 && NPC.downedMechBoss2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case TileID.Titanium:
                        if (NPC.downedMechBoss1 && NPC.downedMechBoss2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    default:
                        break;
                }
            }

            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}

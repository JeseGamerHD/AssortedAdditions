using AssortedAdditions.Common.Configs;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Helpers;
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
            if (ModContent.GetInstance<ServerSidedToggles>().HardmodeOreProgressionToggle)
            {
                switch (type)
                {
                    case TileID.Mythril:
                        return NPC.downedMechBossAny || DownedBossSystem.downedFireDragon;

                    case TileID.Orichalcum:
                        return NPC.downedMechBossAny || DownedBossSystem.downedFireDragon;

                    case TileID.Adamantite:
						return HelperMethods.AtLeastTrue(2, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3);


					case TileID.Titanium:
						return HelperMethods.AtLeastTrue(2, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3);

					default:
                        break;
                }
            }

            return base.CanKillTile(i, j, type, ref blockDamaged);
        }
    }
}

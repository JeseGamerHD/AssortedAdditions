using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace AssortedAdditions.Common.ItemDropRules
{
    // Rule that checks if the 2nd boss has been defeated

    // No longer used since 1.4.4 added an easier way to add conditions to npcLoot
    // Left as a example incase needed later
    /*    public class WormOrBrainDefeated : IItemDropRuleCondition
        {
            public bool CanDrop(DropAttemptInfo info)
            {
                return NPC.downedBoss2;
            }

            public bool CanShowItemDropInUI()
            {
                return true;
            }

            public string GetConditionDescription()
            {
                return "Drops after Eater of Worlds or Brain of Cthulhu has been defeated";
            }
        }*/
}

using Terraria;
using Terraria.GameContent.ItemDropRules;

namespace ModdingTutorial.Common.ItemDropRules
{
    // Rule that checks if the 2nd boss has been defeated
    public class WormOrBrainDefeated : IItemDropRuleCondition
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
    }
}

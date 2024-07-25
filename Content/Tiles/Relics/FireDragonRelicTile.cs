using Terraria.ModLoader;
using AssortedAdditions.Content.Items.Placeables.Relics;

namespace AssortedAdditions.Content.Tiles.Relics
{
    public class FireDragonRelicTile : BossRelicTile
    {
        public override string RelicTextureName => "AssortedAdditions/Content/Tiles/Relics/FireDragonRelicTile";

        public override int RelicItemType => ModContent.ItemType<FireDragonRelic>();

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
    }
}
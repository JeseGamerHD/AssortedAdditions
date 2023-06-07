using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria.Localization;

namespace ModdingTutorial.Content.Tiles
{
    internal class DuneBar : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileShine[Type] = 1100;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(200, 200, 200), Language.GetText("MapObject.DuneBar"));
        }

        public override bool Drop(int x, int y)
        {
            Tile t = Main.tile[x, y];
            int style = t.TileFrameX / 18;

            switch (style)
            {
                case 0:
                    Item.NewItem(new EntitySource_TileBreak(x, y),
                    x * 16,
                    y * 16, 16, 16,
                    ModContent.ItemType<Items.Placeables.DuneBar>());
                    break;

                    //case x: if want to create multiple in same file
            }

            return base.Drop(x, y);
        }
    }
}

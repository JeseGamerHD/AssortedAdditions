using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ModdingTutorial.Content.Tiles.Banners
{
    internal class GrabberPlantBannerTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            //TileID.Sets.SwaysInWindBasic[Type] = true; // TODO add better effect once TML supports banner movement, now it uses foliage sway...
            TileObjectData.newTile.StyleHorizontal = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleWrapLimit = 111;
            TileObjectData.addTile(Type);
            DustType = -1;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(16, 110, 41), name); // Name will be just Banner like the vanilla ones (set in localisation)
        }

        // The banner buff:
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                int type = ModContent.NPCType<NPCs.GrabberPlant>();
                Main.SceneMetrics.hasBanner = true;
                Main.SceneMetrics.NPCBannerBuff[type] = true;
            }
        }
    }
}

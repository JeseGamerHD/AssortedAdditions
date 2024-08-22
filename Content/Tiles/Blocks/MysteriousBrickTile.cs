using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Blocks
{
    internal class MysteriousBrickTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(0, 0, 0));
            DustType = DustID.Asphalt;
            HitSound = SoundID.Tink;

            MineResist = 1.3f;
            MinPick = 200;
        }

		public override bool CanExplode(int i, int j)
		{
            return false;
		}
	}
}

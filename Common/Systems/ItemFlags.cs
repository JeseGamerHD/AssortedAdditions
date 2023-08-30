using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModdingTutorial.Common.Systems
{
    internal class ItemFlags : ModSystem
    {
        // When the player uses a mysterious key it sets this to true
        // After that the wizard will sell the mysterious items so there is an alternative way to obtain them
        // after the chest has been looted
        public bool mysteriousKeyWasUsed = false;

        public override void ClearWorld()
        {
            mysteriousKeyWasUsed = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            if(mysteriousKeyWasUsed)
            {
                tag["mysteriousKeyWasUsed"] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            mysteriousKeyWasUsed = tag.ContainsKey("mysteriousKeyWasUsed");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = mysteriousKeyWasUsed;

            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            mysteriousKeyWasUsed = flags[0];
        }

    }
}

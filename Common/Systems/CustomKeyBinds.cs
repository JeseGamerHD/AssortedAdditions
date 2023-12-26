using Terraria.ModLoader;

namespace AssortedAdditions.Common.Systems
{
	internal class CustomKeyBinds : ModSystem
	{
		public static ModKeybind RuneAbility {  get; private set; }

		public override void Load()
		{
			RuneAbility = KeybindLoader.RegisterKeybind(Mod, "Runes", "Q");
		}

		public override void Unload()
		{
			RuneAbility = null;
		}
	}
}

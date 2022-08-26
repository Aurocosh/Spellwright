using Terraria.ModLoader;

namespace Spellwright.Common.System
{
    internal class KeybindSystem : ModSystem
    {
        internal static ModKeybind OpenIncantationUIHotKey { get; private set; }
        internal static ModKeybind CastCantripHotKey { get; private set; }

        public override void Load()
        {
            OpenIncantationUIHotKey = KeybindLoader.RegisterKeybind(Mod, "Start incantation", "X");
            CastCantripHotKey = KeybindLoader.RegisterKeybind(Mod, "Cast cantrip", "Mouse4");
        }

        // Please see ExampleMod.cs' Unload() method for a detailed explanation of the unloading process.
        public override void Unload()
        {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            OpenIncantationUIHotKey = null;
            CastCantripHotKey = null;
        }
    }
}

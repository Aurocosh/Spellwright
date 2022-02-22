using Microsoft.Xna.Framework;
using Spellwright.Core.Spells;
using Spellwright.Network;
using Spellwright.UI.Components;
using Spellwright.UI.States;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Utilities;

namespace Spellwright
{
    public class Spellwright : Mod
    {
        internal static ModKeybind OpenIncantationUIHotKey;
        internal static ModKeybind CastCantripHotKey;

        internal static Spellwright instance;
        internal static Dictionary<string, ModTranslation> translations;

        internal SpellInputUiState spellInputState;

        internal UserInterface userInterface;

        public Spellwright()
        {
        }

        public override void Load()
        {
            // Since we are using hooks not in older versions, and since ItemID.Count changed, we need to do this.
            if (BuildInfo.tMLVersion < new Version(0, 11, 5))
                throw new Exception("\nThis mod uses functionality only present in the latest tModLoader. Please update tModLoader to use this mod\n\n");
            instance = this;

            OpenIncantationUIHotKey = KeybindLoader.RegisterKeybind(this, "Start incantation", "K");
            CastCantripHotKey = KeybindLoader.RegisterKeybind(this, "Cast cantrip", "K");

            if (Main.rand == null)
                Main.rand = new UnifiedRandom();

            FieldInfo translationsField = typeof(LocalizationLoader).GetField("translations", BindingFlags.Static | BindingFlags.NonPublic);
            translations = (Dictionary<string, ModTranslation>)translationsField.GetValue(this);

            spellInputState = new SpellInputUiState();

            userInterface = new UserInterface();
            userInterface.IsVisible = true;
            userInterface.SetState(spellInputState);

            spellInputState.Deactivate();
        }

        public override void Unload()
        {
            instance = null;

            UITextBox.textboxBackground = null;
            spellInputState = null;
            userInterface = null;
            OpenIncantationUIHotKey = null;
            CastCantripHotKey = null;

            SpellLoader.Unload();
        }

        public void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
        }
        internal static string GetTranslationKey(string category, string name) => $"Mods.Spellwright.{category}.{name}";
        internal static string GetTranslationKey(string category, string subcategory, string name) => $"Mods.Spellwright.{category}.{subcategory}.{name}";

        internal static string GetTranslation(string category, string key)
        {
            string translationKey = $"Mods.Spellwright.{category}.{key}";
            if (!translations.TryGetValue(translationKey, out var translation))
                return translationKey;
            return translation.GetTranslation(Language.ActiveCulture);
        }
        internal static string GetTranslation(string category, string subcategory, string key)
        {
            string translationKey = $"Mods.Spellwright.{category}.{subcategory}.{key}";
            if (!translations.TryGetValue(translationKey, out var translation))
                return translationKey;
            return translation.GetTranslation(Language.ActiveCulture);
        }

        public override void AddRecipeGroups()
        {
            //if (!Main.dedServ)
            //    try
            //    {
            //        spellInputState = new SpellInput();
            //        spellInputState.Visible = false;
            //    }
            //    catch (Exception e)
            //    {
            //        Logger.Error(e.ToString());
            //    }
        }

        public override object Call(params object[] args)
        {
            try
            {
                string message = args[0] as string;
                if (message == "Test")
                    Logger.Info("Test");
                {
                    Logger.Error("Call Error: Unknown Message: " + message);
                }
            }
            catch (Exception e)
            {
                Logger.Error("Call Error: " + e.StackTrace + e.Message);
            }
            return null;
        }
        public static bool IsPlayerServerOwner(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return Netplay.Connection.Socket.GetRemoteAddress().IsLocalHost();

            for (int plr = 0; plr < Main.maxPlayers; plr++)
                if (Netplay.Clients[plr].State == 10 && Main.player[plr] == player && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost())
                    return true;
            return false;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var msgType = (SpellwrightMessageType)reader.ReadByte();
            switch (msgType)
            {
                case SpellwrightMessageType.TODO_1:
                    int asd = reader.ReadInt32();
                    break;
                default:
                    Logger.Warn("Unknown message type: " + msgType);
                    break;
            }
        }
    }
}
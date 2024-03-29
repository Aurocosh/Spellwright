using Microsoft.Xna.Framework;
using Spellwright.Content.Spells;
using Spellwright.Core.Spells;
using Spellwright.Integration;
using Spellwright.Network.NetworkActions;
using Spellwright.UI.Components;
using Spellwright.UI.States;
using System;
using System.IO;
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
        internal static Spellwright Instance { get; private set; }

        internal UISpellInputState spellInputState { get; private set; }
        internal UIMessageState uiMessageState { get; private set; }
        internal UserInterface userInterface { get; private set; }

        internal AndroLibIntegration IntegAndroLib { get; private set; }

        public Spellwright()
        {
        }

        public override void Load()
        {
            // Since we are using hooks not in older versions, and since ItemID.Count changed, we need to do this.
            if (BuildInfo.tMLVersion < new Version(0, 11, 5))
                throw new Exception("\nThis mod uses functionality only present in the latest tModLoader. Please update tModLoader to use this mod\n\n");
            Instance = this;

            if (Main.rand == null)
                Main.rand = new UnifiedRandom();

            spellInputState = new UISpellInputState();
            uiMessageState = new UIMessageState();
            userInterface = new UserInterface();
            userInterface.IsVisible = true;
        }

        public override void PostSetupContent()
        {
            NetworkAction.Load(this);
            SpellLibrary.Refresh();
            SpellModifiersProcessor.Initialize();
            IntegAndroLib = new AndroLibIntegration();
        }

        public override void Unload()
        {
            Instance = null;

            UITextBox.textboxBackground = null;
            spellInputState = null;
            uiMessageState = null;
            userInterface = null;
            IntegAndroLib = null;

            NetworkAction.Unload();
            SpellLoader.Unload();
            SpellLibrary.Unload();
            SpellModifiersProcessor.Unload();
        }

        public void UpdateUI(GameTime gameTime)
        {
            userInterface?.Update(gameTime);
        }
        internal static string GetTranslationKey(string category, string name) => $"Mods.Spellwright.{category}.{name}";
        internal static string GetTranslationKey(string category, string subcategory, string name) => $"Mods.Spellwright.{category}.{subcategory}.{name}";

        internal static LocalizedText GetTranslation(string category, string key)
        {
            string translationKey = $"Mods.Spellwright.{category}.{key}";
            return Language.GetText(translationKey);
        }
        internal static LocalizedText GetTranslation(string category, string subcategory, string key)
        {
            string translationKey = $"Mods.Spellwright.{category}.{subcategory}.{key}";
            return Language.GetText(translationKey);
        }
        internal static LocalizedText GetTranslation(string category, string subcategory, string sub2category, string key)
        {
            string translationKey = $"Mods.Spellwright.{category}.{subcategory}.{sub2category}.{key}";
            return Language.GetText(translationKey);
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
            NetworkAction.HandlePacket(reader, whoAmI);
        }
    }
}
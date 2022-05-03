//using System;
//using Terraria;

//namespace Spellwright.Core.Links
//{
//    internal class HomePageHandler : PageHandler
//    {
//        public HomePageHandler()
//        {
//        }

//        public override string ProcessLink(string link, Player player)
//        {
//            var output = string.Join(
//                Environment.NewLine,
//                "Menu",
//                $" {GetTranslation("Guide")}",
//                $"  {GetLink("SpellTypes")}",
//                $"  {GetLink("SpellModifiers")}",
//                $"  {GetLink("SpellUnlock")}",
//                $"  {GetLink("SpellReagents")}",
//                $"  {GetLink("SpellLevels")}",
//                $" {GetTranslation("Spells")}",
//                $"  {GetLink("FavoriteSpells")}",
//                $"  {GetLink("UsableSpells")}",
//                $"  {GetLink("KnownSpells")}",
//                $"  {GetLink("LockedSpells")}",
//                $"  {GetLink("SpellCosts")}",
//                $" {GetTranslation("Other")}",
//                $"  {GetLink("PlayerStatus")}");
//            return output;
//        }
//    }
//}



////**Spell types
////** Spell modifiers
////** Leveling up
////** Unlocking spells
////** Spell costs and reagents
////** Spell levels


////* Spells
////** Favorite spells
////** Usable spells
////** All known spells
////** Locked spells
////** Spell costs


////* Other info
////** My status
////                            clrColor = DColorTranslator.FromHtml("#FFCC66");

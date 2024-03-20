using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Integration
{
    internal class AndroLibIntegration
    {
        public const string LibName = "androLib";
        public Mod ModObject { get; }
        public bool IsEnabled { get; }

        private readonly Dictionary<int, int> _itemIdToStorageMap = new();

        public AndroLibIntegration()
        {
            IsEnabled = ModLoader.TryGetMod(LibName, out var androLib);
            ModObject = androLib;

            if (IsEnabled)
                ReloadContainerIds();
        }

        public bool TryGetStorageByItemId(int itemId, out Item[] storageItems)
        {
            if (!_itemIdToStorageMap.TryGetValue(itemId, out int storageId))
            {
                storageItems = null;
                return false;
            }

            storageItems = GetItems(storageId);
            return true;
        }

        public void ReloadContainerIds()
        {
            if (!IsEnabled)
                return;

            _itemIdToStorageMap.Clear();
            RegisterStorage("AmmoBag", "VacuumBags");
            RegisterStorage("BannerBag", "VacuumBags");
            RegisterStorage("BossBag", "VacuumBags");
            RegisterStorage("BuildersBox", "VacuumBags");
            RegisterStorage("ExquisitePotionFlask", "VacuumBags", "PotionFlask");
            RegisterStorage("FishingBelt", "VacuumBags");
            RegisterStorage("HerbSatchel", "VacuumBags");
            RegisterStorage("JarOfDirt", "VacuumBags");
            RegisterStorage("MechanicsToolbelt", "VacuumBags");
            RegisterStorage("PaintBucket", "VacuumBags");
            RegisterStorage("PortableStation", "VacuumBags");
            RegisterStorage("PotionFlask", "VacuumBags");
            RegisterStorage("SlayersSack", "VacuumBags");
            RegisterStorage("TrashCan", "VacuumBags");
            RegisterStorage("WallEr", "VacuumBags");

            RegisterStorage("BagBlack", "VacuumBags");
            RegisterStorage("BagBlue", "VacuumBags");
            RegisterStorage("BagBrown", "VacuumBags");
            RegisterStorage("BagGray", "VacuumBags");
            RegisterStorage("BagGreen", "VacuumBags");
            RegisterStorage("BagOrange", "VacuumBags");
            RegisterStorage("BagPink", "VacuumBags");
            RegisterStorage("BagPurple", "VacuumBags");
            RegisterStorage("BagRed", "VacuumBags");
            RegisterStorage("BagWhite", "VacuumBags");
            RegisterStorage("BagYellow", "VacuumBags");

            RegisterStorage("PackBlack", "VacuumBags", "BagBlack");
            RegisterStorage("PackBlue", "VacuumBags", "BagBlue");
            RegisterStorage("PackBrown", "VacuumBags", "BagBrown");
            RegisterStorage("PackGray", "VacuumBags", "BagGray");
            RegisterStorage("PackGreen", "VacuumBags", "BagGreen");
            RegisterStorage("PackOrange", "VacuumBags", "BagOrange");
            RegisterStorage("PackPink", "VacuumBags", "BagPink");
            RegisterStorage("PackPurple", "VacuumBags", "BagPurple");
            RegisterStorage("PackRed", "VacuumBags", "BagRed");
            RegisterStorage("PackWhite", "VacuumBags", "BagWhite");
            RegisterStorage("PackYellow", "VacuumBags", "BagYellow");

            RegisterStorage("CalamitousCauldron", "VacuumBags");
            RegisterStorage("EarthenPyramid", "VacuumBags");
            RegisterStorage("EssenceOfGathering", "VacuumBags");
            RegisterStorage("FargosMementos", "VacuumBags");
            RegisterStorage("HoiPoiCapsule", "VacuumBags");
            RegisterStorage("LokisTesseract", "VacuumBags");
            RegisterStorage("SpookyGourd", "VacuumBags");

            RegisterStorage("OreBag", "VacuumBags", "OreBag", "VacuumOreBag");
        }

        private void RegisterStorage(string itemName, string modName, string storageName = null, string storageModName = null)
        {
            storageName ??= itemName;
            storageModName ??= modName;

            if (ModContent.TryFind<ModItem>(modName, itemName, out var item))
            {
                int storageId = GetStorageId(storageName, storageModName);
                if (storageId >= 0)
                    _itemIdToStorageMap.Add(item.Item.type, storageId);
                else
                    Spellwright.Instance.Logger.Info($"Unable to get a storage id for: {modName} - {itemName}.");
            }
            else
            {
                Spellwright.Instance.Logger.Info($"Unable to find storage item: {modName} - {itemName}.");
            }
        }

        private int GetStorageId(string itemName, string modName)
        {
            return (int)ModObject.Call("GetStorageID", modName, itemName);
        }

        private Item[] GetItems(int storageId)
        {
            return (Item[])ModObject.Call("GetItems", storageId);
        }
    }
}

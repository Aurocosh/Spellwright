using NetSerializer;
using Spellwright.Network.CustomSerializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace Spellwright.Network.NetworkActions
{
    [Serializable]
    internal abstract class NetworkAction
    {
        //[field: NonSerialized]
        //public byte FromWho { get; private set; }

        private static Serializer serializer;

        public NetworkAction()
        {
            //FromWho = (byte)Main.myPlayer;
        }

        public void Execute()
        {
            HandleRouting((byte)Main.myPlayer, false);
        }
        public abstract void DoAction();
        protected abstract void HandleRouting(byte fromWho, bool fromServer);
        protected void Send(int toWho, int fromWho)
        {
            ModPacket packet = Spellwright.Instance.GetPacket();
            serializer.Serialize(packet.BaseStream, this);
            packet.Send(toWho, fromWho);
        }

        public static void HandlePacket(BinaryReader reader, int fromWho)
        {
            var packet = (NetworkAction)serializer.Deserialize(reader.BaseStream);
            bool fromServer = fromWho == 256;
            packet.HandleRouting((byte)fromWho, fromServer);
        }

        internal static void Load(Mod mod)
        {
            var settings = new Settings()
            {
                CustomTypeSerializers = new ITypeSerializer[] { new PlayerCustomSerializer(), new PointCustomSerializer() },
            };
            var types = GetAllPacketTypes(mod, settings).ToList();
            serializer = new Serializer(types, settings);
        }

        private static IEnumerable<Type> GetAllPacketTypes(Mod mod, Settings settings)
        {
            foreach (var type in mod.Code.GetTypes().Where(t => t.IsSubclassOf(typeof(NetworkAction)) && !t.IsAbstract))
            {
                if (!type.IsSerializable)
                    throw new Exception($"All packets must have the SerializableAttribute. Add the SerializableAttribute to the type." + type.Name);
                foreach (var field in type.GetFields())
                {
                    if (settings.CustomTypeSerializers.Any(x => x.Handles(field.FieldType)))
                        continue;
                    if (!field.FieldType.IsSerializable && !field.IsNotSerialized)
                        throw new Exception($"The member {field.Name} is not serializable. Add the NonSerializedAttribute to it.");
                }
                yield return type;
            }
        }

        internal static void Unload()
        {
            serializer = null;
        }
    }
}

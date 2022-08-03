using Microsoft.Xna.Framework;
using NetSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Spellwright.Network.CustomSerializers
{
    internal class PointCustomSerializer : IStaticTypeSerializer
    {
        public bool Handles(Type type)
        {
            return type == typeof(Point);
        }

        public IEnumerable<Type> GetSubtypes(Type type)
        {
            yield break;
        }

        public MethodInfo GetStaticWriter(Type type)
        {
            return typeof(PointCustomSerializer).GetMethod("WritePrimitive",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.ExactBinding, null,
                new Type[] { typeof(Stream), type }, null);

        }

        public MethodInfo GetStaticReader(Type type)
        {
            return typeof(PointCustomSerializer).GetMethod("ReadPrimitive",
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.ExactBinding, null,
                new Type[] { typeof(Stream), type.MakeByRefType() }, null);
        }

        static void WritePrimitive(Stream stream, Point value)
        {
            Primitives.WritePrimitive(stream, value.X);
            Primitives.WritePrimitive(stream, value.Y);
        }

        static void ReadPrimitive(Stream stream, out Point value)
        {
            Primitives.ReadPrimitive(stream, out int x);
            Primitives.ReadPrimitive(stream, out int y);
            value = new Point(x, y);
        }
    }
}

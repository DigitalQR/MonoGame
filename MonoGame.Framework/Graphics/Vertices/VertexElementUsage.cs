// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    internal class VertexElementUsageData
    {
        internal readonly string _name;
        internal readonly string _semantic;

        internal VertexElementUsageData(string name, string semantic)
        {
            _name = name;
            _semantic = semantic;
        }
    }


    /// <summary>
    /// Defines usage for vertex elements.
    /// </summary>
    public struct VertexElementUsage
    {
        private static List<VertexElementUsageData> _registeredUsages = new List<VertexElementUsageData>();

        public static readonly VertexElementUsage Position;
        public static readonly VertexElementUsage Color;
        public static readonly VertexElementUsage TextureCoordinate;
        public static readonly VertexElementUsage Normal;
        public static readonly VertexElementUsage Binormal;
        public static readonly VertexElementUsage Tangent;
        public static readonly VertexElementUsage BlendIndices;
        public static readonly VertexElementUsage BlendWeight;
        public static readonly VertexElementUsage Fog;
        public static readonly VertexElementUsage PointSize;
        public static readonly VertexElementUsage Sample;
        public static readonly VertexElementUsage TessellateFactor;

        static VertexElementUsage()
        {
            CreateNew("null", "null");
            Position = CreateNew("Position", "POSITION");
            Color = CreateNew("Color", "COLOR");
            TextureCoordinate = CreateNew("TextureCoordinate", "TEXCOORD");
            Normal = CreateNew("Normal", "NORMAL");
            Binormal = CreateNew("Binormal", "BINORMAL");
            Tangent = CreateNew("Tangent", "TANGENT");
            BlendIndices = CreateNew("BlendIndices", "BLENDINDICES");
            BlendWeight = CreateNew("BlendWeight", "BLENDWEIGHT");
            Fog = CreateNew("Fog", "Fog");
            PointSize = CreateNew("PointSize", "PSIZE");
            Sample = CreateNew("Sample", "Sample");
            TessellateFactor = CreateNew("TessellateFactor", "TessellateFactor");
        }

        public static VertexElementUsage CreateNew(string name, string semantic = null)
        {
            if (string.IsNullOrEmpty(semantic))
                semantic = name;

            int index;
            lock (_registeredUsages)
            {
                _registeredUsages.Add(new VertexElementUsageData(name, semantic));
                index = (_registeredUsages.Count - 1);
            }

            return new VertexElementUsage(index);
        }

        public static VertexElementUsage FindExisting(string name)
        {
            lock (_registeredUsages)
            {
                for (int i = 0; i < _registeredUsages.Count; ++i)
                {
                    if (_registeredUsages[i]._name == name)
                        return new VertexElementUsage(i);
                }
            }

            return Null;
        }

        private int _index;

        public VertexElementUsage(int i)
        {
            _index = i;
        }

        public VertexElementUsage(VertexElementUsage other)
        {
            _index = other._index;
        }

        public string Name
        {
            get { return _registeredUsages[_index]._name; }
        }

        public string Semantic
        {
            get { return _registeredUsages[_index]._semantic; }
        }

        public static VertexElementUsage Null
        {
            get { return new VertexElementUsage(0); }
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexElementUsage)
                return ((VertexElementUsage)obj)._index == _index;

            return false;
        }

        public override int GetHashCode()
        {
            return _index.GetHashCode();
        }

        public override string ToString()
        {
            return Name + ":" + Semantic;
        }

        public static bool operator ==(VertexElementUsage a, VertexElementUsage b)
        {
            return a._index == b._index;
        }

        public static bool operator !=(VertexElementUsage a, VertexElementUsage b)
        {
            return a._index != b._index;
        }
    }
}

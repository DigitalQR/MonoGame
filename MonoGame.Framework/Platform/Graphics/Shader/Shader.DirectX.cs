// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.IO;
using SharpDX.Direct3D11;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class Shader
    {
        private VertexShader _vertexShader;
        private PixelShader _pixelShader;
        private ComputeShader _computeShader;
        private GeometryShader _geometryShader;
        private DomainShader _domainShader;
        private byte[] _shaderBytecode;

        // Caches the DirectX input layouts for this vertex shader.
        private InputLayoutCache _inputLayouts;

        internal byte[] Bytecode
        {
            get { return _shaderBytecode; }
        }

        internal InputLayoutCache InputLayouts
        {
            get { return _inputLayouts; }
        }

        internal VertexShader VertexShader
        {
            get
            {
                if (_vertexShader == null)
                    CreateVertexShader();
                return _vertexShader;
            }
        }

        internal PixelShader PixelShader
        {
            get
            {
                if (_pixelShader == null)
                    CreatePixelShader();
                return _pixelShader;
            }
        }

        internal ComputeShader ComputeShader
        {
            get
            {
                if (_computeShader == null)
                    CreateComputeShader();
                return _computeShader;
            }
        }

        internal GeometryShader GeometryShader
        {
            get
            {
                if (_geometryShader == null)
                    CreateGeometryShader();
                return _geometryShader;
            }
        }

        internal DomainShader DomainShader
        {
            get
            {
                if (_domainShader == null)
                    CreateDomainShader();
                return _domainShader;
            }
        }

        private static int PlatformProfile()
        {
            return 1;
        }

        private void PlatformConstruct(ShaderStage stage, byte[] shaderBytecode)
        {
            // We need the bytecode later for allocating the
            // input layout from the vertex declaration.
            _shaderBytecode = shaderBytecode;

            HashKey = MonoGame.Framework.Utilities.Hash.ComputeHash(Bytecode);

            switch(stage)
            {
                case ShaderStage.Vertex:
                    CreateVertexShader();
                    break;

                case ShaderStage.Pixel:
                    CreatePixelShader();
                    break;

                case ShaderStage.Compute:
                    CreateComputeShader();
                    break;

                case ShaderStage.Geometry:
                    CreateGeometryShader();
                    break;

                case ShaderStage.Domain:
                    CreateDomainShader();
                    break;

                default:
                    System.Diagnostics.Debug.Assert(false, "Unsupported ShaderStage");
                    break;
            }
        }

        private void PlatformGraphicsDeviceResetting()
        {
            SharpDX.Utilities.Dispose(ref _vertexShader);
            SharpDX.Utilities.Dispose(ref _pixelShader);
            SharpDX.Utilities.Dispose(ref _computeShader);
            SharpDX.Utilities.Dispose(ref _geometryShader);
            SharpDX.Utilities.Dispose(ref _domainShader);
            SharpDX.Utilities.Dispose(ref _inputLayouts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SharpDX.Utilities.Dispose(ref _vertexShader);
                SharpDX.Utilities.Dispose(ref _pixelShader);
                SharpDX.Utilities.Dispose(ref _inputLayouts);
                SharpDX.Utilities.Dispose(ref _computeShader);
                SharpDX.Utilities.Dispose(ref _geometryShader);
                SharpDX.Utilities.Dispose(ref _domainShader);
            }

            base.Dispose(disposing);
        }

        private void CreatePixelShader()
        {
            System.Diagnostics.Debug.Assert(Stage == ShaderStage.Pixel);
            _pixelShader = new PixelShader(GraphicsDevice._d3dDevice, _shaderBytecode);
        }

        private void CreateVertexShader()
        {
            System.Diagnostics.Debug.Assert(Stage == ShaderStage.Vertex);
            _vertexShader = new VertexShader(GraphicsDevice._d3dDevice, _shaderBytecode, null);
            _inputLayouts = new InputLayoutCache(GraphicsDevice, Bytecode);
        }

        private void CreateComputeShader()
        {
            System.Diagnostics.Debug.Assert(Stage == ShaderStage.Compute);
            _computeShader = new ComputeShader(GraphicsDevice._d3dDevice, _shaderBytecode, null);
        }

        private void CreateGeometryShader()
        {
            System.Diagnostics.Debug.Assert(Stage == ShaderStage.Geometry);
            _geometryShader = new GeometryShader(GraphicsDevice._d3dDevice, _shaderBytecode, null);
        }

        private void CreateDomainShader()
        {
            System.Diagnostics.Debug.Assert(Stage == ShaderStage.Domain);
            _domainShader = new DomainShader(GraphicsDevice._d3dDevice, _shaderBytecode, null);
        }
    }
}

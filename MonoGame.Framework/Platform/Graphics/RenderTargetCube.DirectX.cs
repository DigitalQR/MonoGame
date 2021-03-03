// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using SharpDX.DXGI;
using SharpDX.Direct3D11;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class RenderTargetCube
    {
        private RenderTargetView[] _renderTargetViews;

        private void PlatformConstruct(GraphicsDevice graphicsDevice, bool mipMap, int preferredMultiSampleCount, RenderTargetUsage usage)
        {
            System.Diagnostics.Debug.Assert(IsValidSurface, "RenderTargetCube must have a valid surface format");

            // Create one render target view per cube map face.
            _renderTargetViews = new RenderTargetView[6];
            for (int i = 0; i < _renderTargetViews.Length; i++)
            {
                var renderTargetViewDescription = new RenderTargetViewDescription
                {
                    Dimension = RenderTargetViewDimension.Texture2DArray,
                    Format = SharpDXHelper.ToResourceFormat(this),
                    Texture2DArray =
                    {
                        ArraySize = 1,
                        FirstArraySlice = i,
                        MipSlice = 0
                    }
                };

                _renderTargetViews[i] = new RenderTargetView(graphicsDevice._d3dDevice, GetTexture(), renderTargetViewDescription);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_renderTargetViews != null)
                {
                    for (var i = 0; i < _renderTargetViews.Length; i++)
                        _renderTargetViews[i].Dispose();

                    _renderTargetViews = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        public RenderTargetView GetRenderTargetView(int arraySlice)
        {
            return _renderTargetViews[arraySlice];
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        public DepthStencilView GetDepthStencilView()
        {
            return null;
        }
    }
}

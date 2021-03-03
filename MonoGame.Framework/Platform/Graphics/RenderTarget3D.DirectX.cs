// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using SharpDX.Direct3D11;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class RenderTarget3D
    {
        private int _currentSlice;
        private RenderTargetView _renderTargetView;
        private DepthStencilView _depthStencilView;

        private void PlatformConstruct(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, int preferredMultiSampleCount, RenderTargetUsage usage)
        {
            // Setup the multisampling description.
            var multisampleDesc = new SharpDX.DXGI.SampleDescription(1, 0);
            if (preferredMultiSampleCount > 1)
            {
                multisampleDesc.Count = preferredMultiSampleCount;
                multisampleDesc.Quality = (int)StandardMultisampleQualityLevels.StandardMultisamplePattern;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SharpDX.Utilities.Dispose(ref _renderTargetView);
                SharpDX.Utilities.Dispose(ref _depthStencilView);
            }

            base.Dispose(disposing);
        }

	    RenderTargetView IRenderTarget.GetRenderTargetView(int arraySlice)
	    {
            if (arraySlice >= Depth)
                throw new ArgumentOutOfRangeException("The arraySlice is out of range for this Texture3D.");

            if (!IsValidSurface)
                throw new InvalidOperationException("This render target does not have a valid surface type");

            // Dispose the previous target.
	        if (_currentSlice != arraySlice && _renderTargetView != null)
	        {
	            _renderTargetView.Dispose();
	            _renderTargetView = null;
	        }

            // Create the new target view interface.
	        if (_renderTargetView == null)
	        {
	            _currentSlice = arraySlice;

	            var desc = new RenderTargetViewDescription
	            {
	                Format = SharpDXHelper.ToResourceFormat(this),
	                Dimension = RenderTargetViewDimension.Texture3D,
	                Texture3D =
	                    {
	                        DepthSliceCount = -1,
	                        FirstDepthSlice = arraySlice,
	                        MipSlice = 0,
	                    }
	            };

	            _renderTargetView = new RenderTargetView(GraphicsDevice._d3dDevice, GetTexture(), desc);
	        }

	        return _renderTargetView;
	    }

	    DepthStencilView IRenderTarget.GetDepthStencilView()
	    {
            if (!IsValidDepthStencil)
                throw new InvalidOperationException("This render target does not have a valid surface type");

            // Create the new target view interface.
            if (_depthStencilView == null)
            {
                _currentSlice = 0;

                _depthStencilView = new DepthStencilView(GraphicsDevice._d3dDevice, GetTexture(), new DepthStencilViewDescription()
                {
                    Format = SharpDXHelper.ToResourceFormat(this),
                    Dimension = DepthStencilViewDimension.Texture2D,
                });
            }

            return _depthStencilView;
	    }
    }
}

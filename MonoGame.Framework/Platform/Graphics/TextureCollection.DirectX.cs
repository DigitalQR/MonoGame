// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Graphics
{
    public sealed partial class TextureCollection
    {
        void PlatformInit()
        {
        }

        internal void ClearTargets(GraphicsDevice device, RenderTargetBinding[] targets)
        {
            ClearTargets(targets, device._d3dContext.VertexShader);
            ClearTargets(targets, device._d3dContext.PixelShader);
            ClearTargets(targets, device._d3dContext.ComputeShader);
            ClearTargets(targets, device._d3dContext.GeometryShader);
            ClearTargets(targets, device._d3dContext.DomainShader);
        }

        private void ClearTargets(RenderTargetBinding[] targets, SharpDX.Direct3D11.CommonShaderStage shaderStage)
        {
            if (shaderStage == null)
                return;

            var computeStage = shaderStage as SharpDX.Direct3D11.ComputeShaderStage;
            if (_writableBinding && computeStage == null)
                return;

            // NOTE: We make the assumption here that the caller has
            // locked the d3dContext for us to use.

            // Make one pass across all the texture slots.
            for (var i = 0; i < _textures.Length; i++)
            {
                if (_textures[i] == null)
                    continue;

                bool shouldClear = false;
                for (int ii = 0; ii < targets.Length; ii++)
                {
                    if (_textures[i] == targets[ii].RenderTarget)
                    {
                        shouldClear = true;
                        break;
                    }
                }

                if (!shouldClear)
                    continue;

                // Immediately clear the texture from the device.
                _dirty &= ~(1 << i);
                _textures[i] = null;

                if (_writableBinding)
                {
                    computeStage.SetUnorderedAccessView(i, null);
                }
                else
                {
                    shaderStage.SetShaderResource(i, null);
                }
            }
        }

        void PlatformClear()
        {
        }

        void PlatformSetTextures(GraphicsDevice device)
        {
            // Skip out if nothing has changed.
            if (_dirty == 0)
                return;

            // NOTE: We make the assumption here that the caller has
            // locked the d3dContext for us to use.
            for (var i = 0; i < _textures.Length; i++)
            {
                var mask = 1 << i;
                if ((_dirty & mask) == 0)
                    continue;

                var tex = _textures[i];

                if (_writableBinding)
                {
                    SetWritableTexture(device._d3dContext.ComputeShader, i, tex);
                }
                else
                {
                    SetTexture(device._d3dContext.VertexShader, i, tex);
                    SetTexture(device._d3dContext.PixelShader, i, tex);
                    SetTexture(device._d3dContext.ComputeShader, i, tex);
                    SetTexture(device._d3dContext.GeometryShader, i, tex);
                    SetTexture(device._d3dContext.DomainShader, i, tex);
                }

                _dirty &= ~mask;
                if (_dirty == 0)
                    break;
            }

            _dirty = 0;
        }

        private void SetTexture(SharpDX.Direct3D11.CommonShaderStage shaderStage, int slot, Texture texture)
        {
            if (shaderStage == null)
                return;

            if (texture == null || texture.IsDisposed)
                shaderStage.SetShaderResource(slot, null);
            else
            {
                shaderStage.SetShaderResource(slot, texture.GetShaderResourceView());
                unchecked
                {
                    _graphicsDevice._graphicsMetrics._textureCount++;
                }
            }
        }

        private void SetWritableTexture(SharpDX.Direct3D11.ComputeShaderStage shaderStage, int slot, Texture texture)
        {
            if (shaderStage == null)
                return;

            if (texture == null || texture.IsDisposed)
                shaderStage.SetUnorderedAccessView(slot, null);
            else
            {
                shaderStage.SetUnorderedAccessView(slot, texture.GetUnorderedResourceView());
                unchecked
                {
                    // TODO - Separate metrics?
                    _graphicsDevice._graphicsMetrics._targetCount++;
                }
            }
        }
    }
}

// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public partial class RenderTarget3D : Texture3D, IRenderTarget
	{		
		public int MultiSampleCount { get; private set; }
		
		public RenderTargetUsage RenderTargetUsage { get; private set; }
		
		public bool IsContentLost { get { return false; } }
		
		public event EventHandler<EventArgs> ContentLost;

        private bool SuppressEventHandlerWarningsUntilEventsAreProperlyImplemented()
        {
            return ContentLost != null;
        }

        public RenderTarget3D(GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat preferredFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
            : base(graphicsDevice, width, height, depth, mipMap, QuerySelectedFormat(graphicsDevice, preferredFormat), true)
        {
            MultiSampleCount = preferredMultiSampleCount;
            RenderTargetUsage = usage;

            PlatformConstruct(graphicsDevice, width, height, mipMap, preferredMultiSampleCount, usage);
        }

        protected static SurfaceFormat QuerySelectedFormat(GraphicsDevice graphicsDevice, SurfaceFormat preferredFormat)
        {
            SurfaceFormat selectedFormat = preferredFormat;
            DepthFormat selectedDepthFormat;
            int selectedMultiSampleCount;

            if (graphicsDevice != null)
            {
                graphicsDevice.Adapter.QueryRenderTargetFormat(graphicsDevice.GraphicsProfile, preferredFormat, DepthFormat.None, 0,
                   out selectedFormat, out selectedDepthFormat, out selectedMultiSampleCount);
            }

            return selectedFormat;
        }

        public RenderTarget3D(GraphicsDevice graphicsDevice, int width, int height, int depth, bool mipMap, SurfaceFormat preferredFormat)
            : this(graphicsDevice, width, height, depth, mipMap, preferredFormat, 0, RenderTargetUsage.DiscardContents)
        { }

        public RenderTarget3D(GraphicsDevice graphicsDevice, int width, int height, int depth)
            : this(graphicsDevice, width, height, depth, false, SurfaceFormat.Color, 0, RenderTargetUsage.DiscardContents)
        { }
    }
}

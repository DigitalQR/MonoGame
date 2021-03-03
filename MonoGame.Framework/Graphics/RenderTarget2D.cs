// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
	public partial class RenderTarget2D : Texture2D, IRenderTarget
	{		
		public int MultiSampleCount { get; private set; }
		
		public RenderTargetUsage RenderTargetUsage { get; private set; }
		
		public bool IsContentLost { get { return false; } }
		
		public event EventHandler<EventArgs> ContentLost;
		
        private bool SuppressEventHandlerWarningsUntilEventsAreProperlyImplemented()
        {
            return ContentLost != null;
        }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared, int arraySize)
            : base(graphicsDevice, width, height, mipMap, QuerySelectedFormat(graphicsDevice, preferredFormat), SurfaceType.RenderTarget, shared, arraySize)
        {
            MultiSampleCount = graphicsDevice.GetClampedMultisampleCount(preferredMultiSampleCount);
            RenderTargetUsage = usage;

            PlatformConstruct(graphicsDevice, width, height, mipMap, preferredMultiSampleCount, usage, shared);
        }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared, int arraySize)
            : base(graphicsDevice, width, height, mipMap, QuerySelectedFormat(graphicsDevice, preferredDepthFormat), SurfaceType.RenderTarget, shared, arraySize)
        {
            MultiSampleCount = graphicsDevice.GetClampedMultisampleCount(preferredMultiSampleCount);
            RenderTargetUsage = usage;

            PlatformConstruct(graphicsDevice, width, height, mipMap, preferredMultiSampleCount, usage, shared);
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

        protected static DepthFormat QuerySelectedFormat(GraphicsDevice graphicsDevice, DepthFormat preferredFormat)
        {
            SurfaceFormat selectedFormat;
            DepthFormat selectedDepthFormat = preferredFormat;
            int selectedMultiSampleCount;

            if (graphicsDevice != null)
            {
                graphicsDevice.Adapter.QueryRenderTargetFormat(graphicsDevice.GraphicsProfile, SurfaceFormat.None, preferredFormat, 0,
                    out selectedFormat, out selectedDepthFormat, out selectedMultiSampleCount);
            }

            return selectedDepthFormat;
        }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared)
            : this(graphicsDevice, width, height, mipMap, preferredFormat, preferredMultiSampleCount, usage, shared, 1)
        {

        }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage, bool shared)
            : this(graphicsDevice, width, height, mipMap, preferredDepthFormat, preferredMultiSampleCount, usage, shared, 1)
        {

        }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
            : this(graphicsDevice, width, height, mipMap, preferredFormat, preferredMultiSampleCount, usage, false)
        { }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, DepthFormat preferredDepthFormat, int preferredMultiSampleCount, RenderTargetUsage usage)
            : this(graphicsDevice, width, height, mipMap, preferredDepthFormat, preferredMultiSampleCount, usage, false)
        { }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, SurfaceFormat preferredFormat)
            : this(graphicsDevice, width, height, mipMap, preferredFormat, 0, RenderTargetUsage.DiscardContents)
        { }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height, bool mipMap, DepthFormat preferredDepthFormat)
            : this(graphicsDevice, width, height, mipMap, preferredDepthFormat, 0, RenderTargetUsage.DiscardContents)
        { }

        public RenderTarget2D(GraphicsDevice graphicsDevice, int width, int height)
			: this(graphicsDevice, width, height, false, SurfaceFormat.Color, 0, RenderTargetUsage.DiscardContents) 
		{}

        /// <summary>
        /// Allows child class to specify the surface type, eg: a swap chain.
        /// </summary>
        protected RenderTarget2D(GraphicsDevice graphicsDevice,
                        int width,
                        int height,
                        bool mipMap,
                        SurfaceFormat format,
                        int preferredMultiSampleCount,
                        RenderTargetUsage usage,
                        SurfaceType surfaceType)
            : base(graphicsDevice, width, height, mipMap, format, surfaceType)
        {
            MultiSampleCount = graphicsDevice.GetClampedMultisampleCount(preferredMultiSampleCount);
            RenderTargetUsage = usage;
		}

        protected internal override void GraphicsDeviceResetting()
        {
            PlatformGraphicsDeviceResetting();
            base.GraphicsDeviceResetting();
        }        
	}
}

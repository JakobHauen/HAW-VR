using System.Runtime.InteropServices;
using System;
using AOT;

namespace agora_gaming_rtc
{
    public abstract class IVideoRawDataManager : IRtcEngineNative
    {

        public abstract int SetOnCaptureVideoFrameCallback(VideoRawDataManager.OnCaptureVideoFrameHandler action);

        public abstract int SetOnRenderVideoFrameCallback(VideoRawDataManager.OnRenderVideoFrameHandler action);

        public abstract int RegisterVideoRawDataObserver();

        public abstract int UnRegisterVideoRawDataObserver();
    }

    /** The definition of VideoRawDataManager. */
    public sealed class VideoRawDataManager : IVideoRawDataManager
    {
        private static IRtcEngine _irtcEngine;
        private static VideoRawDataManager _videoRawDataManagerInstance;
        /** Occurs each time the SDK receives a video frame captured by the local camera.
         * 
         * After you successfully register the video frame observer, the SDK triggers this callback each time a video frame is received. In this callback, you can get the video data captured by the local camera. You can then pre-process the data according to your scenarios.
         * 
         * @param videoFrame See VideoFrame.
         */
        public delegate void OnCaptureVideoFrameHandler(VideoFrame videoFrame);
        private OnCaptureVideoFrameHandler OnCaptureVideoFrame;
        /** Occurs each time the SDK receives a video frame sent by the remote user.
         * 
         * After you successfully register the video frame observer, the SDK triggers this callback each time a video frame is received. In this callback, you can get the video data sent by the remote user. You can then post-process the data according to your scenarios.
         * 
         * @param uid ID of the remote user who sends the current video frame.
         * @param videoFrame See VideoFrame.
         */
        public delegate void OnRenderVideoFrameHandler(uint uid, VideoFrame videoFrame);
        private OnRenderVideoFrameHandler OnRenderVideoFrame;

        private VideoRawDataManager(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        public static VideoRawDataManager GetInstance(IRtcEngine irtcEngine)
        {
            if (_videoRawDataManagerInstance == null)
            {
                _videoRawDataManagerInstance = new VideoRawDataManager(irtcEngine);
            }
            return _videoRawDataManagerInstance;
        }

        public static void ReleaseInstance()
        {
            _videoRawDataManagerInstance = null;
        }

        public void SetEngine(IRtcEngine irtcEngine)
        {
            _irtcEngine = irtcEngine;
        }

        /** Listens for the {@link agora_gaming_rtc.VideoRawDataManager.OnCaptureVideoFrameHandler OnCaptureVideoFrameHandler} delegate.
         *
         * @param action The implementation of the `OnCaptureVideoFrameHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnCaptureVideoFrameCallback(OnCaptureVideoFrameHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (action == null)
            {
                OnCaptureVideoFrame = null;
                IRtcEngineNative.initEventOnCaptureVideoFrame(null);
            }
            else
            {
                OnCaptureVideoFrame = action;
                IRtcEngineNative.initEventOnCaptureVideoFrame(OnCaptureVideoFrameCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

        /** Listens for the {@link agora_gaming_rtc.VideoRawDataManager.OnRenderVideoFrameHandler OnRenderVideoFrameHandler} delegate.
         *
         * @param action The implementation of the `OnRenderVideoFrameHandler` delegate.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int SetOnRenderVideoFrameCallback(OnRenderVideoFrameHandler action)
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_NOT_INIT_ENGINE;

            if (action == null)
            {
                OnRenderVideoFrame = null;
                IRtcEngineNative.initEventOnRenderVideoFrame(null);
            }
            else
            {
                OnRenderVideoFrame = action;
                IRtcEngineNative.initEventOnRenderVideoFrame(OnRenderVideoFrameCallback);
            }
            return (int)ERROR_CODE.ERROR_OK;
        }

        /** Registers a video raw data observer.
         * 
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int RegisterVideoRawDataObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_OK;

            return IRtcEngineNative.registerVideoRawDataObserver();
        }

        /** UnRegisters the video raw data observer.
         *
         * @return
         * - 0: Success.
         * - < 0: Failure.
         */
        public override int UnRegisterVideoRawDataObserver()
        {
            if (_irtcEngine == null)
                return (int)ERROR_CODE.ERROR_OK;

            return IRtcEngineNative.unRegisterVideoRawDataObserver();
        }

        [MonoPInvokeCallback(typeof(EngineEventOnCaptureVideoFrame))]
        private static void OnCaptureVideoFrameCallback(int videoFrameType, int width, int height, int yStride, IntPtr buffer, int rotation, long renderTimeMs)
        {
            if (_irtcEngine != null && _videoRawDataManagerInstance != null && _videoRawDataManagerInstance.OnCaptureVideoFrame != null)
            {
                VideoFrame videoFrame = new VideoFrame();
                videoFrame.type = (VIDEO_FRAME_TYPE)videoFrameType;
                videoFrame.width = width;
                videoFrame.height = height;
                videoFrame.yStride = yStride;
                byte[] yB = new byte[yStride * height];
                Marshal.Copy(buffer, yB, 0, yStride * height);
                videoFrame.buffer = yB;
                videoFrame.rotation = rotation;
                videoFrame.renderTimeMs = renderTimeMs;
                _videoRawDataManagerInstance.OnCaptureVideoFrame(videoFrame);
            }
        }

        [MonoPInvokeCallback(typeof(EngineEventOnRenderVideoFrame))]
        private static void OnRenderVideoFrameCallback(uint uid, int videoFrameType, int width, int height, int yStride, IntPtr yBuffer, int rotation, long renderTimeMs)
        {
            if (_irtcEngine != null && _videoRawDataManagerInstance != null && _videoRawDataManagerInstance.OnRenderVideoFrame != null)
            {
                VideoFrame videoFrame = new VideoFrame();
                videoFrame.type = (VIDEO_FRAME_TYPE)videoFrameType;
                videoFrame.width = width;
                videoFrame.height = height;
                videoFrame.yStride = yStride;
                byte[] yB = new byte[yStride * height];
                Marshal.Copy(yBuffer, yB, 0, yStride * height);
                videoFrame.buffer = yB;
                videoFrame.rotation = rotation;
                videoFrame.renderTimeMs = renderTimeMs;
                _videoRawDataManagerInstance.OnRenderVideoFrame(uid, videoFrame);
            }
        }
    }
};
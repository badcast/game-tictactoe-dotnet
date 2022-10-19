using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimationEditor
{
    public static class Time
    {
        private const float fpsMeasurePeriod = 1f;
        private static float fpsNextPeriod;
        private static bool _start;
        private static DateTime _startTime;
        private static float _time;
        private static float _deltaTime;
        private static int _frameCount;
        private static float _fps;
        private static float _lastTime;
        /// <summary>
        /// Текущая время 
        /// </summary>
        public static float time
        {
            get
            {
                return _time;
            }
        }

        public static float realTimeOnSince
        {
            get
            {
                return (float)(DateTime.Now - _startTime).TotalSeconds;
            }
        }

        public static float fps { get { return _fps; } }

        /// <summary>
        /// Прошедшее время в кадрах
        /// </summary>
        public static float deltaTime { get => _deltaTime; }

        /// <summary>
        /// Количество показанных кадров
        /// </summary>
        public static int frameCount { get => _frameCount; }
        public static void InitTime()
        {
            if (_start)
                return;
            _start = true;
            _startTime = DateTime.Now;
            _time = 0;
            _lastTime = 0;
            fpsNextPeriod = realTimeOnSince + fpsMeasurePeriod;
        }

        /// <summary>
        /// Вычисляет время 
        /// </summary>
        public static void Calculate()
        {
            float _newTime = (float)(DateTime.Now - _startTime).TotalSeconds;

            float elapsed = time - _lastTime;


            _deltaTime = elapsed;
            if (elapsed >= 1f)
            {
                _lastTime = time;
            }
            _time = _newTime;
        }

        public static void CalculateFps()
        {
            _frameCount++;
            if (realTimeOnSince > fpsNextPeriod)
            {
                _fps = (float)Math.Round(_frameCount / fpsMeasurePeriod, 1);
                _frameCount = 0;
                fpsNextPeriod += fpsMeasurePeriod;
            }
        }
    }

}

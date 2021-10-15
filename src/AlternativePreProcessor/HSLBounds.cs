using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrConsoleAppFx
{
    /// <summary>
    /// Hue, Saturation & Luminosity bounds
    /// Used to determine a HSL color falls in a given Color range
    /// </summary>
    public class HSLBounds
    {
        private readonly float _hueMin = 0;
        private readonly float _hueMax = 360;
        private readonly float _saturationMin = 0.0f;
        private readonly float _saturationMax = 1.0f;
        private readonly float _luminosityMin = 0.0f;
        private readonly float _luminosityMax = 1.0f;

        public HSLBounds(float HMin, float HMax, float SMin, float SMax, float LMin, float LMax)
        {
            _hueMin = HMin;
            _hueMax = HMax;
            _saturationMin = SMin;
            _saturationMax = SMax;
            _luminosityMin = LMin;
            _luminosityMax = LMax;
    }

        public float HMin => _hueMin;
        public float HMax => _hueMax;
        public float SMin => _saturationMin;
        public float SMax => _saturationMax;
        public float LMin => _luminosityMin;
        public float LMax => _luminosityMax;
        
    }
}

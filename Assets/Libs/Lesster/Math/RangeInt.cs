using System;

namespace Lesstergy.Math {

    [Serializable]
    public class RangeInt {
        public int min;
        public int max;

        public RangeInt(int min, int max) {
            this.min = min;
            this.max = max;
        }

        public bool IsInRange(int value) {
            return (value >= min && value <= max);
        }
    }

}

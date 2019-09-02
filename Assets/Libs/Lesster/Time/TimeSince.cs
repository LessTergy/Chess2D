namespace Lesstergy.Time
{

    public class TimeSince
    {

        public float lastValue { get; private set; }

        public float delta => UnityEngine.Time.timeSinceLevelLoad - lastValue;

        public void Fixate()
        {
            lastValue = UnityEngine.Time.timeSinceLevelLoad;
        }

        public void Increase()
        {
            lastValue += UnityEngine.Time.deltaTime;
        }

        public void Reset()
        {
            lastValue = 0f;
        }
    }

}
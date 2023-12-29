using System;

namespace AdaptiveBpm
{
    public interface IBPMGenerator
    {
        public float BPM { get; }
        public Action<float> BPMUpdated { get; set; }
    }
}

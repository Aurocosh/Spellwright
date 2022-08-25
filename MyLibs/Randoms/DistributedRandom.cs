using System.Collections.Generic;

namespace Spellwright.MyLibs.Randoms
{
    public class DistributedRandom<T>
    {
        private double distributionsSum = 0;
        private readonly Dictionary<T, double> distributions = new();

        public T GetRandomItem(double randomValue)
        {
            var ratio = 1.0f / distributionsSum;
            var tempDist = 0.0;
            foreach (var (key, value) in distributions)
            {
                tempDist += value;
                if (randomValue / ratio <= tempDist)
                    return key;
            }
            return default;
        }

        public void Add(T value, double distribution)
        {
            if (distributions.TryGetValue(value, out double currentDistribution))
                distributionsSum -= currentDistribution;
            distributions[value] = distribution;
            distributionsSum += distribution;
        }

        public void Remove(T value)
        {
            if (distributions.TryGetValue(value, out double currentDistribution))
                distributionsSum -= currentDistribution;
            distributions.Remove(value);
        }

        public void Clear()
        {
            distributions.Clear();
            distributionsSum = 0;
        }
    }
}

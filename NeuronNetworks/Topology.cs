using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetworks
{
    public class Topology
    {
        public int InputCount { get; }
        public int OutputCount { get; }
        public double LearningSpeed { get; }

        public List<int> HiddenLayer;
        public Topology(int inputCount, int outputCount, double learningSpeed, params int[] laysers)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            LearningSpeed = learningSpeed;
            HiddenLayer = new List<int>();
            HiddenLayer.AddRange(laysers);
        }
        

    }
}

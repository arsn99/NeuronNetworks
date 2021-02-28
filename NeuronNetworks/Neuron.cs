using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetworks
{
    public class Neuron
    {
        static public int sec = DateTime.UtcNow.Millisecond;// 993
        public List<double> Weights { get; }
        public List<double> Inputs { get; }
        public NeuronType neuronType { get; }
        public double Output { get; private set; }
        public double Delta { get; private set; }
        const double angle1 = 2.0;
        const double angle2 = 1.0;
        public Neuron(int inputCount,NeuronType type = NeuronType.Normal)
        {
            neuronType = type;
            Weights = new List<double>();
            Inputs = new List<double>();
            InitWeightsRandomValues(inputCount);
        }

        private void InitWeightsRandomValues(int inputCount)
        {
            var rnd = new Random(sec);
            
            for (int i = 0; i < inputCount; i++)
            {
                if (neuronType == NeuronType.Input)
                {
                    Weights.Add(1);
                }
                else
                {
                    Weights.Add(rnd.NextDouble());
                }
                Inputs.Add(0);
            }
        }

        public double FeedForward(List<double> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                Inputs[i] = inputs[i];
            }
            var sum = 0.0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }
            if (neuronType != NeuronType.Input)
            {
                Output = Sigmoid(sum);
            }
            else
            {
                Output = sum;
            }
            
            return Output;
        }
        public void Learn(double error,double learnSpeed)
        {
            if (neuronType!=NeuronType.Input)
            {
                Delta = error * SigmoidDx(Output);

                for (int i = 0; i < Weights.Count; i++)
                {
                    var weight = Weights[i];
                    var input = Inputs[i];

                    var newWeight = weight - input * Delta * learnSpeed;
                    Weights[i] = newWeight;
                }
            }
        }
        public override string ToString()
        {
            return Output.ToString();
        }
        private double Sigmoid(double x)
        {
            if (neuronType==NeuronType.Output)
            {
                return 1.0 / (1.0 + Math.Exp(-1.0*x));
            }
            else
                return 1.0 / (1.0 + Math.Exp(-x));

        }
        private double SigmoidDx(double x)
        {

            //return Sigmoid(x)/(1-Sigmoid(x));
            //Console.WriteLine($"{Sigmoid(x) * (1 - Sigmoid(x))} {Math.Exp(-x) / Math.Pow(1 + Math.Exp(-x), 2)}");
            if (neuronType == NeuronType.Output)
            {
                return 1.0 * x * (1.0 - x);
            }
            else
                return  x * (1.0 - x);

        }
    }
}

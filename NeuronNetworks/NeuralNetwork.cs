using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetworks
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; }
        public Topology Topology { get; set; }
        public NeuralNetwork(Topology topology)
        {
            Topology = topology;
            Layers = new List<Layer>();
            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }
        
        public List<Neuron> FeedForward(params double[] inputSignal)
        {
            SendSignalsToInputNeurons(inputSignal);
            FeedForwardAllLayersAfterInput();
            if (Topology.OutputCount==1)
            {
                return new List<Neuron> { Layers.Last().Neurons[0] };
            }
            else
            {
                return Layers.Last().Neurons;
            }
        }
        private double BackPropagation(double[] expected, double[] input)
        {
            var actual = FeedForward(input);
            double[] diff = new double[actual.Count];

            for (int i = 0; i < diff.Length; i++)
            {
                diff[i] = actual[i].Output - expected[i];
            }

            for (int i = 0; i < Layers.Last().NeuronCount; i++)
            {
                Layers.Last().Neurons[i].Learn(diff[i], Topology.LearningSpeed);
            }
            for (int i = Layers.Count-2; i > 0; i--)
            {
                var layer = Layers[i];
                var previousLayer = Layers[i + 1];

                for (int j = 0; j < layer.NeuronCount; j++)
                {
                    var neuron = layer.Neurons[j];
                    double diff2 = 0.0;
                    for (int k = 0; k < previousLayer.NeuronCount; k++)
                    {
                        var previousNeuron = previousLayer.Neurons[k];
                        diff2+= previousNeuron.Weights[j] * previousNeuron.Delta;     
                    }
                    neuron.Learn(diff2,Topology.LearningSpeed);
                }
            }

            return diff.Sum(x => x * x/4.0);
        }
        public double[] Learn(List<Tuple<double[],double[]>> dataset, int epoch)
        {
            var error = new double[epoch];
            for (int i = 0; i < epoch; i++)
            {
                
                foreach (var date in dataset)
                {
                    error[i]+= BackPropagation(date.Item1, date.Item2);
                    
                    Console.WriteLine(error);
                }
                error[i] /= 4.0;
            }
            return error;
        }
        private void FeedForwardAllLayersAfterInput()
        {
            for (int i = 1; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                var previousLayerSignals = Layers[i - 1].GetSignals();
                foreach (var neuron in layer.Neurons)
                {
                    neuron.FeedForward(previousLayerSignals);
                }
            }
        }

        private void SendSignalsToInputNeurons(params double[] inputSignal)
        {
            for (int i = 0; i < inputSignal.Length; i++)
            {
                var signal = new List<double>() { inputSignal[i] };
                var neuron = Layers[0].Neurons[i];

                neuron.FeedForward(signal);
            }
        }

        private void CreateOutputLayer()
        {
            var outputNeurons = new List<Neuron>();
            var lastLayer = Layers.Last();
            for (int i = 0; i < Topology.OutputCount; i++)
            {
                var neuron = new Neuron(lastLayer.NeuronCount, NeuronType.Output);
                outputNeurons.Add(neuron);
            }
            var outputLayer = new Layer(outputNeurons, NeuronType.Output);
            Layers.Add(outputLayer);
        }

        private void CreateHiddenLayers()
        {
            for (int j = 0; j < Topology.HiddenLayer.Count; j++)
            {
                var hiddenNeurons = new List<Neuron>();
                var lastLayer = Layers.Last();
                for (int i = 0; i < Topology.HiddenLayer[j]; i++)
                {
                    var neuron = new Neuron(lastLayer.NeuronCount);
                    hiddenNeurons.Add(neuron);
                }
                var hiddenLayer = new Layer(hiddenNeurons);
                Layers.Add(hiddenLayer);
            }
            
        }

        private void CreateInputLayer()
        {
            var inputNeurons = new List<Neuron>();
            for (int i = 0; i < Topology.InputCount; i++)
            {
                var neuron = new Neuron(1,NeuronType.Input);
                inputNeurons.Add(neuron);   
            }
            var inputLayer = new Layer(inputNeurons, NeuronType.Input);
            Layers.Add(inputLayer);
        }
    }
}

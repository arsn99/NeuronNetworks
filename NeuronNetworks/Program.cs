using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace NeuronNetworks
{
    class Program
    {
        public static double StrToDouble(params int[] mas)
        {
            int sum=0;
            for (int i = 0; i < mas.Length; i++)
            {
                sum += mas[i] * 1 << (mas.Length - 1 - i);
            }
            return sum/63.0;
        }

        public static double[] ImageToDouble(Bitmap image)
        {
            //var help = Math.Pow(2, 64);
            var mas = new double[image.Width];
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    mas[i] += (image.GetPixel(j, i).ToArgb()== Color.Black.ToArgb()?1.0:0.0)*Math.Pow(2.0,j);
                }
                //mas[i] =(mas[i] / help);
            }
            return mas; 
        }

        public static Mutex mutex = new Mutex();
        public static double minDif = double.MaxValue;
        public static double step = 0.1;
        public class Exempl
        {
            public double start, end;
            public List<Tuple<double[], double[]>> dataset;
            public Exempl(double s,double e, List<Tuple<double[], double[]>> dt )
            {
                s = start;
                end = e;
                dataset = dt;
            }
        }
        /*public static void Go(object exempl1)
        {
            Exempl exempl = (Exempl)exempl1;
            for (double i = exempl.start; i < exempl.end; i += 0.1)
            {
                // for (int j = 6; j < 60; j++)
                //{
                Topology topology = new Topology(64, 4, i, 35);
                NeuralNetwork neuralNetwork = new NeuralNetwork(topology);

                var diff1 = neuralNetwork.Learn(exempl.dataset, 100000);
                mutex.WaitOne();
                if (diff1 < minDif)
                {
                    step = i;
                    //count = j;
                    minDif = diff1;
                }
                mutex.ReleaseMutex();

                //neuralNetwork.FeedForward(new double[] { StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1) });

                // }
            }
        }*/
        static void Main(string[] args)
        {
            

            Bitmap bitmap = new Bitmap(Properties.Resources.Image1);
            
            //Console.WriteLine(bitmap.GetPixel(0,0));
            var dataset = new List<Tuple<double[], double[]>>
            {
               /* new Tuple<double[],double[]>(new double[]{1,0,0,0 },new double[]{StrToDouble(0,0,1,0,0),StrToDouble(0,1,1,0,0),StrToDouble(0,0,1,0,0),StrToDouble(0, 0, 1, 0, 0),StrToDouble(0, 0, 1, 0, 0),StrToDouble(0,1,1,1,0)}),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },new double[]{StrToDouble(0,1,1,1,0), StrToDouble(0, 1, 0, 1, 0), StrToDouble(0, 0, 0, 1, 0), StrToDouble(0, 0, 1, 0, 0), StrToDouble(0,1,0,0,0),StrToDouble(0,1,1,1,0)}),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },new double[]{StrToDouble(1,0,0,1,0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1,1,1,1,1),StrToDouble(0,0,0,0,1)}),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },new double[]{StrToDouble(1,0,0,1,0), StrToDouble(1, 0,1, 0, 1), StrToDouble(1, 1, 1, 0, 1), StrToDouble(1, 1, 1, 0, 1), StrToDouble(1, 0, 1, 0, 1),StrToDouble(1, 0, 0, 1, 0) }),
                */
                new Tuple<double[],double[]>(new double[]{1,0,0,0 },ImageToDouble(Properties.Resources.Image1)),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },ImageToDouble(Properties.Resources.Image2)),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },ImageToDouble(Properties.Resources.Image3)),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },ImageToDouble(Properties.Resources.Image4))
               
                
                /*new Tuple<double[],double[]>(new double[]{0,0,1,0 },new double[]{StrToDouble(1,1,1,1,1), StrToDouble(1, 0, 0, 0, 1), StrToDouble(0, 0, 0, 0, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(0,0,0,0,1),StrToDouble(1,1,1,1,1)}),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },new double[]{StrToDouble(1,0,1,0,1), StrToDouble(1, 0, 1, 0, 1), StrToDouble(1, 0, 1, 0, 1), StrToDouble(1, 0, 1, 0, 1), StrToDouble(1, 0, 1, 0, 1), StrToDouble(1,1,1,1,1)}),
                */
                

            };
            /*var ex = new Exempl[4];
            double start = 0.0;
            double end = 2.0;
            double st = (end - start) / ex.Length;
            for (int i = 0; i < ex.Length; i++)
            {
                ex[i] = new Exempl(start, start + step, dataset);
                start += step;
            }
            Thread[] threads = new Thread[4];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(Go));
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(ex[i]);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }*/
            NeuralNetwork neuralNetwork;
            Topology topology;
             
             int count = 60;
             

             /*for (double i = 0.01; i < 2.0; i+=0.1)
             {
                // for (int j = 6; j < 60; j++)
                 //{
                     topology = new Topology(64, 4, step, 35);
                     neuralNetwork = new NeuralNetwork(topology);

                     var diff1 = neuralNetwork.Learn(dataset, 100000);
                     if (diff1<minDif)
                     {
                         step = i;
                         //count = j;
                         minDif = diff1;
                     }

                     //neuralNetwork.FeedForward(new double[] { StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1) });

                // }
             }*/
            topology = new Topology(64, 4, 0.1,35);
            neuralNetwork = new NeuralNetwork(topology);
            int size = 10000;
            var diff = neuralNetwork.Learn(dataset, size);
            //------------------------------------------
            MLApp.MLApp matlab = new MLApp.MLApp();
            object result = null;
            double[] mas = new double[size];

            for (int i = 0; i < size; i++)
            {
                mas[i] = i;
                
            }
            matlab.Execute(@"cd C:\Users\Arseniy\source\repos\NeuronNetworks\NeuronNetworks");
            matlab.Feval("myfunc", 1,out result,mas,diff);

            
            //--------------------------------------------
            foreach (var data in dataset)
            {
                neuralNetwork.FeedForward(data.Item2);
            }
            
            
           /* var topology = new Topology(6, 4,0.15,5);
            var neuralNetwork = new NeuralNetwork(topology);
            
            var diff = neuralNetwork.Learn(dataset,500);
            var r = new List<double>();
            foreach (var data in dataset)
            {
                neuralNetwork.FeedForward(data.Item2);
            }
            neuralNetwork.FeedForward(new double[] { StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 1, 1) });
            *//*for (int i = 0; i < r.Count; i++)
            {
                var expected = Math.Round( dataset[i].Item1,3);
                var actual = Math.Round(r[i],3);

            }*/
            

        }
    }
}

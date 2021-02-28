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
        public static int count = 4;
        public static int size = 10000;
        public class Exempl
        {
            public double start, end;
            public List<Tuple<double[], double[]>> dataset;
            public List<Tuple<double[], double[]>> dataset2;
            public Exempl(double s,double e, List<Tuple<double[], double[]>> dt, List<Tuple<double[], double[]>> dt2)
            {
                start = s;
                end = e;
                dataset = dt;
                dataset2 = dt2;
            }
        }
        public static void Go(object exempl1)
        {
            Exempl exempl = (Exempl)exempl1;
            for (double i = exempl.start; i < exempl.end; i += 0.01)
            {
                    for (int j = 1; j < 7; j++)
                    {
                    Topology topology = new Topology(6, 4, i, j);
                    NeuralNetwork neuralNetwork = new NeuralNetwork(topology);

                    var diff1 = neuralNetwork.Learn(exempl.dataset,exempl.dataset2, size);
                    mutex.WaitOne();
                    if (diff1[0].Last() * diff1[0].Last() + (diff1[1].Last() - diff1[0].Last()) * (diff1[1].Last() - diff1[0].Last()) < minDif)
                    {
                        step = i;
                        count = j;
                        minDif = diff1[0].Last() * diff1[0].Last() +( diff1[1].Last() - diff1[0].Last())* (diff1[1].Last() - diff1[0].Last());
                    }
                    mutex.ReleaseMutex();
                }
            }
        }
        static void Main(string[] args)
        {
            

            Bitmap bitmap = new Bitmap(Properties.Resources.Image1);

            var dataset = new List<Tuple<double[], double[]>>
            {
                new Tuple<double[],double[]>(new double[]{1,0,0,0 },new double[]{StrToDouble(0,0,1,0,0),StrToDouble(0,1,1,0,0),StrToDouble(0,0,1,0,0),StrToDouble(0, 0, 1, 0, 0),StrToDouble(0, 0, 1, 0, 0),StrToDouble(0,1,1,1,0)}),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },new double[]{StrToDouble(0,1,1,1,0), StrToDouble(0, 1, 0, 1, 0), StrToDouble(0, 0, 0, 1, 0), StrToDouble(0, 0, 1, 0, 0), StrToDouble(0,1,0,0,0),StrToDouble(0,1,1,1,0)}),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },new double[]{StrToDouble(1,0,0,1,0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1,1,1,1,1),StrToDouble(0,0,0,0,1)}),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },new double[]{StrToDouble(1,0,0,1,0), StrToDouble(1, 0,1, 0, 1), StrToDouble(1, 1, 1, 0, 1), StrToDouble(1, 1, 1, 0, 1), StrToDouble(1, 0, 1, 0, 1),StrToDouble(1, 0, 0, 1, 0) }),   
                /*new Tuple<double[],double[]>(new double[]{1,0,0,0 },ImageToDouble(Properties.Resources.Image1)),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },ImageToDouble(Properties.Resources.Image2)),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },ImageToDouble(Properties.Resources.Image3)),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },ImageToDouble(Properties.Resources.Image4))
               */
            };
            var dataset2 = new List<Tuple<double[], double[]>>
            {
                new Tuple<double[],double[]>(new double[]{1,0,0,0 },new double[]{StrToDouble(1,0,1,0,0),StrToDouble(0,1,1,0,1),StrToDouble(0,0,1,0,0),StrToDouble(0, 0, 0, 0, 0),StrToDouble(0, 0, 1, 1, 0),StrToDouble(0,1,1,0,0)}),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },new double[]{StrToDouble(0,0,1,1,0), StrToDouble(1, 1, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(0, 0, 1, 1, 0), StrToDouble(0,1,0,0,1),StrToDouble(0,0,1,1,0)}),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },new double[]{StrToDouble(1,0,1,1,0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(0, 0, 0, 1, 0), StrToDouble(1, 0, 0, 1, 0), StrToDouble(1,1,0,1,1),StrToDouble(1,0,0,0,1)}),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },new double[]{StrToDouble(1,0,0,1,1), StrToDouble(1, 0,1, 1, 1), StrToDouble(1, 1, 0, 0, 1), StrToDouble(1, 1, 1, 1, 1), StrToDouble(1, 1, 1, 0, 1),StrToDouble(0, 0, 0, 1, 0) }),   
                 /*new Tuple<double[],double[]>(new double[]{1,0,0,0 },ImageToDouble(Properties.Resources.Image1)),
                new Tuple<double[],double[]>(new double[]{0,1,0,0 },ImageToDouble(Properties.Resources.Image2)),
                new Tuple<double[],double[]>(new double[]{0,0,1,0 },ImageToDouble(Properties.Resources.Image3)),
                new Tuple<double[],double[]>(new double[]{0,0,0,1 },ImageToDouble(Properties.Resources.Image4))
               */
            };
            var ex = new Exempl[4];
            double start = 0.0;
            double end = 1.0;
            double st = (end - start) / ex.Length;
            for (int i = 0; i < ex.Length; i++)
            {
                ex[i] = new Exempl(start, start + st, dataset,dataset2);
                start += st;
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
            }
            NeuralNetwork neuralNetwork;
            Topology topology;

            topology = new Topology(6, 4, step,count);
            neuralNetwork = new NeuralNetwork(topology);
            
            var diff = neuralNetwork.Learn(dataset,dataset2, size);
            //-------------------------------------
            MLApp.MLApp matlab = new MLApp.MLApp();
            object result;
            double[] mas = new double[size];
            for (int i = 0; i < size; i++)
            {
                mas[i] = i; 
            }
            matlab.Execute(@"cd C:\Users\Arseniy\source\repos\NeuronNetworks\NeuronNetworks");
            matlab.Feval("myfunc", 1,out result,mas,diff[0],diff[1]);

            foreach (var data in dataset)
            {
                neuralNetwork.FeedForward(data.Item2);
            }
            foreach (var data in dataset2)
            {
                neuralNetwork.FeedForward(data.Item2);
            }

        }
    }
}

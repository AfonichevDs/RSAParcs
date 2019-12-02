using CustomModule.RSA;
using Parcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomModule
{
    public class MainRSAModule : IModule
    {
        public static void Main(string[] args)
        {

            var job = new Job();
            if (!job.AddFile(Assembly.GetExecutingAssembly().Location/*"MyFirstModule.exe"*/))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }

            (new MainRSAModule()).Run(new ModuleInfo(job, null));
            Console.ReadKey();
        }

        public void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            string text = @"Auroras are occasionally seen in latitudes below the auroral zone, 
                            when a geomagnetic storm temporarily enlarges the auroral oval.Red: At the highest altitudes, 
                           excited atomic oxygen emits at 630 nm (red); low concentration of atoms and lower sensitivity of 
                           eyes at this wavelength make this color visible only under more intense solar activity. The 
                           low number of oxygen atoms and their gradually diminishing concentration is responsible for 
                           the faint appearance of the top parts of the curtains. 
                           Scarlet, crimson, and carmine are the most often-seen hues of red for the auroras.";
            string[] texts = new string[]
            {
                text.Substring(0, text.Length/2),
                text.Substring(text.Length/2, text.Length/2 - 1)
            };
            RSAProvider rsa = new RSAProvider(Constants.KeySize.Bit64);

            const int pointsNum = 2;
            var points = new IPoint[pointsNum];
            var channels = new IChannel[pointsNum];
            for (int i = 0; i < pointsNum; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("CustomModule.RSAModule");
            }

            for (int i = 0; i < pointsNum; ++i)
            {
                channels[i].WriteData(rsa.PublicKey.KeyOne.ToString());
                channels[i].WriteData(rsa.PublicKey.KeyTwo.ToString());
                channels[i].WriteData(texts[i]);
            }
            DateTime time = DateTime.Now;
            Console.WriteLine("Waiting for result...");

            string res = "";
            for (int i = pointsNum - 1; i >= 0; --i)
            {
                res += channels[i].ReadString();
            }

            Console.WriteLine("Result found: res = {0}, time = {1}", res, System.Math.Round((DateTime.Now - time).TotalSeconds, 3));

        }
    }
}

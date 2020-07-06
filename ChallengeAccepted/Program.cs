using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;
using Console = Colorful.Console;
using Random = System.Random;
using MlkPwgen;
using System.Linq;

namespace ChallengeAccepted
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileFormats = new List<string>()
            {
                "png",
                "gif",
                "jpg",
                "mp4",
                "txt",
                "zip",
                "rar"
            };


            Console.Title = "GayZoon Screenshot Scraper  -  (By BabiKoqi & iLinked)";
            Console.Write(@"
                                
                                  ▄▀  ██   ▀▄    ▄      ▄▄▄▄▄▄   ████▄ ████▄    ▄   
                                 ▄▀    █ █    █  █      ▀   ▄▄▀   █   █ █   █     █  
                                 █ ▀▄  █▄▄█    ▀█        ▄▀▀   ▄▀ █   █ █   █ ██   █ 
                                 █   █ █  █    █         ▀▀▀▀▀▀   ▀████ ▀████ █ █  █ 
                                  ███     █  ▄▀                               █  █ █ 
                                         █       By BabiKoqi & iLinked        █   ██ 
                                        ▀                                            
                                                 

", Color.Purple);

            Directory.CreateDirectory("Media");

            Console.Write("What is Your Desired Domain: ", Color.White);
            string domain = Console.ReadLine();


            Directory.CreateDirectory($"Media/{domain}");

            foreach (var format in fileFormats)
                Directory.CreateDirectory($"Media/{domain}/{format}");


            Console.Write("Character Length: ", Color.White);
            int charLength = int.Parse(Console.ReadLine());


            Console.Write("Amount of Threads: ", Color.White);
            int threadAmount = int.Parse(Console.ReadLine());


            Console.Write("Use MlkPwgen.dll For Generating Combinations? (Y/N): ", Color.White);
            bool useMlk = Console.ReadLine().ToLower()[0] == 'y';


            Console.Write("Do You Want To Save The Media? (Y/N): ", Color.White);
            bool saveMedia = Console.ReadLine().ToLower()[0] == 'y';

            ThreadPool.SetMinThreads(threadAmount, threadAmount);
            Task.Run(() =>
            {
                while (true)
                {
                    Console.Title = $"GayZoon Screenshot Scraper - Hits: {Hits}  CPM: {CPM * 60}  Bads: {Bads}  Checked: {Checks}  (By BabiKoqi & iLinked)";
                    CPM = 0;
                    Thread.Sleep(1000);
                }
            });

            Random rnd = new Random();

            while (true)
            {

                Parallel.ForEach(GetCombinations(rnd, charLength, useMlk), new ParallelOptions() { MaxDegreeOfParallelism = threadAmount }, combination =>
                {
                    bool valid = false;

                    foreach (var format in fileFormats)
                    {
                        string url = $"https://{domain}/{combination}.{format}";
                        using (HttpRequest httpRequest = new HttpRequest())
                        {
                            try
                            {
                                var resp = httpRequest.Get(url);

                                Hits++;
                                Checks++;
                                Console.WriteLine($"[HIT] {url}", Color.LimeGreen);
                                valid = true;
                                File.AppendAllText($@"Media\{domain}\URLs.txt", $"{url}\r\n");

                                if (saveMedia)
                                    File.WriteAllBytes($@"Media\{domain}\{format}\{domain.Replace(".", "-")}-{combination}.{format}", resp.ToBytes());

                                break;
                            }
                            catch { Checks++; }

                            CPM++;
                        }

                        if (!valid)
                        {
                            Console.WriteLine($"[BAD] https://{domain}/{combination}", Color.Red);
                            Bads++;
                            CPM++;
                        }
                    }
                });
            }
        }


        private static List<string> GetCombinations(Random rnd, int length, bool useMlk, int amount = 1000)
        {
            string charSet = "abcdefghijklmnopqrstuvwxyz0123456789";

            List<string> combs = new List<string>();

            for (int i = 0; i < amount; i++)
            {
                if (useMlk)
                    combs.Add(PasswordGenerator.Generate(length, charSet));
                else
                {
                    combs.Add(new string(Enumerable.Repeat(charSet, length)
                                .Select(s => s[rnd.Next(s.Length)]).ToArray()));
                }
            }

            return combs;
        }


        public static int Hits = 0;
        public static int Bads = 0;
        public static int Checks = 0;
        public static int CPM = 0;
    }
}

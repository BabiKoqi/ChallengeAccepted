using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Leaf.xNet;
using Console = Colorful.Console;

namespace ChallengeAccepted
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileFormats = new List<string>()
            {
                "jpg",
                "png",
                "gif",
                "mp4"
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
            foreach (var format in fileFormats)
                Directory.CreateDirectory("Media/" + format);


            Console.Write("Whats Your Desied Domain: ", Color.White);
            string domain = Console.ReadLine();

            Console.Write("Character Length: ", Color.White);
            int charLength = int.Parse(Console.ReadLine());


            Console.Write("Amount of Threads: ", Color.White);
            int threadAmount = int.Parse(Console.ReadLine());


            Console.Write("Do You Want To Save The Media? (Y/N): ", Color.White);
            bool saveMedia = Console.ReadLine().ToLower()[0] == 'y';
            Random rnd = new Random();
            bool isChecking = true;
            Task.Factory.StartNew(delegate ()
            {
                while (isChecking)
                {
                    CPM_aux = CPM * 60;
                    CPM = 0;
                    Thread.Sleep(1000);
                    Console.Title = $"GayZoon Screenshot Scraper  - Hits: {Hits}  CPM: {CPM_aux}  Bads: {Bads}  Checked: {Checkeds}  (By BabiKoqi & iLinked)";

                }
            });
            while (true)
            {

                Parallel.ForEach(GetCombinations(rnd, charLength), new ParallelOptions() { MaxDegreeOfParallelism = threadAmount }, combination =>
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

                                if (resp.StatusCode == HttpStatusCode.OK)
                                {
                                    Hits++;
                                    Checkeds++;
                                    Console.WriteLine($"[HIT] {url}", Color.LimeGreen);
                                    valid = true;
                                    File.AppendAllText("Valid-URLs.txt", $"{url}\r\n");

                                    if (saveMedia)
                                        File.WriteAllBytes($@"Media\{format}\{domain.Replace(".", "-")}-{combination}.{format}", resp.ToBytes());
                                }
                                Bads++;
                                break;
                            }
                            catch { }

                            CPM++;
                        }
                    }

                    if (!valid)
                    {
                        Console.WriteLine($"[BAD] https://{domain}/{combination}", Color.Red);
                        Bads++;
                        CPM++;
                        Checkeds++;
                    }
                });
            }
        }


        private static List<string> GetCombinations(Random rnd, int length)
        {
            List<string> combs = new List<string>();

            for (int i = 0; i < 1000; i++)
                combs.Add(GetCombination(rnd, length));

            return combs;
        }


        public static int Hits = 0;

        public static int Bads = 0;

        public static int Checkeds = 0;

        public static int CPM = 0;

        public static int CPM_aux = 0;


        private static string GetCombination(Random rnd, int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}

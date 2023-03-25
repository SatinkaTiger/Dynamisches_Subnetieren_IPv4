namespace Dynamisches_Subnettieren_IPv4
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        static void Main(string[] args)
        {
            int AnzahlNetzwerke;
            string Eingabe;
            string Temp = "";
            bool Converting;
            bool Schleife = true;
            int OktettIndex = 0;
            ProgrammKopf();
            Console.Write("Bitte geben Sie die Startadresse mit Präfix Ihres Netzwerks ein: ");
            Eingabe = Console.ReadLine();
            Console.Write("\nBitte geben Sie die Anzahl ihrer Teilnetzwerke ein: ");
            Converting = int.TryParse(Console.ReadLine(), out AnzahlNetzwerke);
            Console.Clear();
            string[] NetzwerkNamen = new string[AnzahlNetzwerke];
            int[] AnzahlHosts = new int[AnzahlNetzwerke];
            int[] AdressOktettenPräfix = new int[5];
            if (Converting)
            {
                for (int i = 0; i < AnzahlNetzwerke; i++)
                {
                    ProgrammKopf();
                    Console.Write($"Bitte geben Sie den Namen Ihres {i + 1}. Teilnetzwerks ein: ");
                    NetzwerkNamen[i] = Console.ReadLine();
                    Console.Write($"Bitte geben Sie die Anzahl Ihrer Hosts in {NetzwerkNamen[i]} ein: ");
                    Converting = int.TryParse(Console.ReadLine(), out AnzahlHosts[i]);
                    Console.Clear();
                    if (!Converting)
                    {
                        Fehlerbehandlung();
                    }
                }
            }
            else
            {
                Fehlerbehandlung();
            }
            while (Schleife)
            {
                for (int i = 0; i < Eingabe.Length; i++)
                {
                    if (Eingabe[i] == '.' || Eingabe[i] == '/')
                    {
                        try
                        {
                            AdressOktettenPräfix[OktettIndex] = Convert.ToInt32(Temp);
                            Temp = "";
                            OktettIndex++;
                        }
                        catch
                        {
                            Fehlerbehandlung();
                        }
                    }
                    else
                    {
                        Temp += Eingabe[i];
                    }
                    if (OktettIndex == 4 && i == Eingabe.Length - 1)
                    {
                        try
                        {
                            AdressOktettenPräfix[OktettIndex] = Convert.ToInt32(Temp);
                            Schleife = false;
                        }
                        catch
                        {
                            Fehlerbehandlung();
                        }
                    }
                }

            }
            for (int i = 0; i < AnzahlHosts.Length; i++)
            {
                for (int j = 0; j < AnzahlHosts.Length; j++)
                {
                    Eingabe = NetzwerkNamen[i];
                    OktettIndex = AnzahlHosts[i];
                    if (AnzahlHosts[j] < AnzahlHosts[i])
                    {
                        AnzahlHosts[i] = AnzahlHosts[j];
                        NetzwerkNamen[i] = NetzwerkNamen[j];
                        AnzahlHosts[j] = OktettIndex;
                        NetzwerkNamen[j] = Eingabe;
                    }
                }
            }
            AnzahlNetzwerke += AnzahlNetzwerke - 1;
            int[] BitsOktett = new int[] { 128, 64, 32, 16, 8, 4, 2 , 1,};
            int WelchesOktett;
            int Restwert;
            int[] Präfix = new int[AnzahlNetzwerke];
            for (int i = 0; i < AnzahlNetzwerke; i++)
            {
                if (i < AnzahlHosts.Length)
                {
                    Präfix[i] = PräfixErmitteln(AnzahlHosts[i]);
                }
                else
                {
                    Präfix[i] = PräfixErmitteln(2);
                }
            }
            int VB = 1;
            for (int i = 0; i < AnzahlNetzwerke; i++)
            {
                WelchesOktett = Präfix[i] / 8;
                Restwert = (Präfix[i] % 8);
                if (i < AnzahlHosts.Length)
                {
                    Console.WriteLine($"{NetzwerkNamen[i]} Adresse: {AdressOktettenPräfix[0]}.{AdressOktettenPräfix[1]}.{AdressOktettenPräfix[2]}.{AdressOktettenPräfix[3]}/{Präfix[i]}");
                }
                else
                {
                    Console.WriteLine($"VN{VB} Adresse: {AdressOktettenPräfix[0]}.{AdressOktettenPräfix[1]}.{AdressOktettenPräfix[2]}.{AdressOktettenPräfix[3]}/{Präfix[i]}");
                }
                if (Restwert == 0)
                {
                    AdressOktettenPräfix[WelchesOktett - 1] += 1;
                }
                else
                {
                    if ((AdressOktettenPräfix[WelchesOktett] += BitsOktett[Restwert]) >= 256)
                    {
                        AdressOktettenPräfix[WelchesOktett - 1] += 1;
                    }
                    else
                    {
                        AdressOktettenPräfix[WelchesOktett] += BitsOktett[Restwert];
                    }
                }
                VB++;
            }
            Console.ReadKey();
        }
        static void Fehlerbehandlung()
        {
            ProgrammKopf();
            Console.WriteLine("Fehlerhafte Eingabe\n\nBeenden mit beliebiger Taste");
            Console.ReadKey();
            Environment.Exit(0);
        }
        static void ProgrammKopf()
        {
            Console.WriteLine("Dynamisches Subnettieren in IPv4\n\nLizensiert für Whiteboxtest, nicht für den privaten und gewerblichen Gebrauch bestimmt\nAlle Rechte Rick Kummer\n\n");
        }
        static int PräfixErmitteln(int AnzahlHosts)
            {
            int Präfix = 0;
            bool Gefunden = false;
            for (int j = 0 ; j <= 32; j++)
            {
                if (Math.Pow(2, Convert.ToDouble(j)) >= Convert.ToDouble(AnzahlHosts + 2) && !Gefunden)
                {
                    Präfix = 32 - j;
                    Gefunden = true;
                }
            }
            return Präfix;
        }
    }
}

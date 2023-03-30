namespace Dynamisches_Subnettieren_IPv4
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Media;

    internal class Program
    {
        static void Main(string[] args)
        {
            /*Dynamisches Subnettieren IPv4
              Copyright (C) 2023  Rick Kummer

              Contact Email: kummer_rick@gmx.de

              This program is free software; you can redistribute it and/or modify
              it under the terms of the GNU General Public License as published by
              the Free Software Foundation; either version 2 of the License, or
              (at your option) any later version.

              This program is distributed in the hope that it will be useful,
              but WITHOUT ANY WARRANTY; without even the implied warranty of
              MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
              GNU General Public License for more details.

              You should have received a copy of the GNU General Public License along
              with this program; if not, write to the Free Software Foundation, Inc.,
              51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.*/

            int AnzahlNetzwerke = 0; //Variablendeklaration
            string Eingabe = "";
            string Temp = "";
            bool Converting = false;
            bool Schleife = true;
            bool SchleifeEingabe = true;
            int OktettIndex = 0;
            int[] AdressOktettenPräfix = new int[5];
            while (SchleifeEingabe)
            {
                ProgrammKopf(); //Beginn der Eingabeaufforderung
                Console.Write("Bitte geben Sie die Startadresse mit Präfix [x.x.x.x/x] Ihres Netzwerks ein: ");
                Eingabe = Console.ReadLine();
                Console.Write("\nBitte geben Sie die Anzahl ihrer Teilnetzwerke ein: ");
                Converting = int.TryParse(Console.ReadLine(), out AnzahlNetzwerke);
                Console.Clear();
                Schleife = true;
                if (Converting)
                {
                    while (Schleife) //Einpflegen der Startadresse in Array AdressOktettenPräfix
                    {
                        for (int i = 0; i < Eingabe.Length; i++)
                        {
                            if (Eingabe[i] == '.' || Eingabe[i] == '/') // Wenn ein Oktett zuende ist dann speichern in AdressOktettenPräfix
                            {
                                try
                                {
                                    if (Convert.ToInt32(Temp) < 256) // Wenn der Wert in String Temp Integer kleiner ist als 256 dann
                                    {
                                        AdressOktettenPräfix[OktettIndex] = Convert.ToInt32(Temp); //Convertiert und Speichert Temp in AdressOktettenPräfix Array
                                        Temp = ""; //Leeren der Temp Variablen für neues Oktett
                                        OktettIndex++; 
                                    }
                                    else
                                    {
                                        Fehlerbehandlung(1); //Anfangsadresse wurde falsch angegeben
                                        Schleife = false;
                                        break; //Beenden der Schleifen zum einpflehen Der Startadresse in Array AdressOktettenPräfix
                                    }
                                }
                                catch
                                {
                                    Fehlerbehandlung(1); //Anfangsadresse wurde falsch angegeben
                                    Schleife = false;
                                    break; //Beenden der Schleifen zum einpflehen Der Startadresse in Array AdressOktettenPräfix
                                }
                            }
                            else //Wenn in der Oktette
                            {
                                if (Temp.Length < 3) //Wenn die Oktette richtig angegeben ist 
                                {
                                    Temp += Eingabe[i]; //Temp um aktuellen Char erweitern
                                }
                                else //Wenn sie Oktette zu lang ist
                                {
                                    Fehlerbehandlung(1); //Anfangsadresse wurde falsch angegeben
                                    Schleife = false;
                                    break; //Beenden der Schleifen zum einpflehen Der Startadresse in Array AdressOktettenPräfix
                                }
                            }
                            if (OktettIndex == 4 && i == Eingabe.Length - 1) //Wenn der Präfix erfasst ist 
                            {
                                try
                                {
                                    AdressOktettenPräfix[OktettIndex] = Convert.ToInt32(Temp); //Convertiert ind speichert Präfix in 
                                    Schleife = false; 
                                    SchleifeEingabe = false; //Beenden beider Schleifen
                                }
                                catch
                                {
                                    Fehlerbehandlung(2); //Präfix falsch angegeben Bchstaben in der Eingabe
                                    Schleife = false;
                                    break; //Beenden der Schleifen zum einpflehen Der Startadresse in Array AdressOktettenPräfix
                                }
                                if (AdressOktettenPräfix[4] > 32) //Präfix falsch angegeben ungültiger Präfix (zu groß)
                                {
                                    Fehlerbehandlung(2); //Präfix falsch angegeben Bchstaben in der Eingabe
                                    Schleife = false;
                                    break; //Beenden der Schleifen zum einpflehen Der Startadresse in Array AdressOktettenPräfix
                                }
                            }
                        }
                    }
                }
                else
                {
                    Fehlerbehandlung(0); //Anzah der Teilnetzwerke falsch (Buchstaben verwendet)
                }
            }  
            string[] NetzwerkNamen = new string[AnzahlNetzwerke]; //Arrays erzeugen anhand der Anzahl der Teilnetzwerke
            int[] AnzahlHosts = new int[AnzahlNetzwerke];
            Schleife = true;
            while (Schleife)
            {
                for (int i = 0; i < AnzahlNetzwerke; i++) //Abfrage der Namen der Teilnetzwerke und deren Hostanzahl
                {
                    ProgrammKopf();
                    Console.Write($"Bitte geben Sie den Namen Ihres {i + 1}. Teilnetzwerks ein: ");
                    NetzwerkNamen[i] = Console.ReadLine();
                    Console.Write($"Bitte geben Sie die Anzahl Ihrer Hosts in {NetzwerkNamen[i]} ein: ");
                    Converting = int.TryParse(Console.ReadLine(), out AnzahlHosts[i]);
                    Console.Clear();
                    Schleife = false;
                    if (!Converting) //Prüfung auf Richtigkeit der Eingabe
                    {
                        Schleife = true;
                        Fehlerbehandlung(4); //Anzahl der Hosts falsch angegeben (Buchstaben verwendet)
                        i--;
                    }
                }
            }
            for (int i = 0; i < AnzahlHosts.Length; i++) //Sortierung der Arrays Netzwernamen und AnzahHosts nach Hostanzahl
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
            AnzahlNetzwerke += AnzahlNetzwerke - 1; //Berechnung der Verbindungsnetzwerke
            int[] BitsOktett = new int[] { 128, 64, 32, 16, 8, 4, 2 , 1,};
            int WelchesOktett;
            int Restwert;
            int[] Präfix = new int[AnzahlNetzwerke];
            for (int i = 0; i < AnzahlNetzwerke; i++) //Berechnung des Präfix für die Netzwerke
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
            WelchesOktett = AdressOktettenPräfix[4] / 8; //Ermittlung der ursprünglichen Subnetzmaske
            Restwert = AdressOktettenPräfix[4] % 8;
            string UrsprünglicheSubnetzmaske = "255";
            int RestwertSubnetzmaske = 0;
            Schleife = true;
            int OktettenSubnetzmaske = WelchesOktett;
            for (int i = 1; i < 4; i++)
            {
                if (i < WelchesOktett)
                {
                    UrsprünglicheSubnetzmaske += ".255";
                }
                else if (Schleife)
                {
                    for (int j = 0; j < Restwert; j++)
                    {
                        RestwertSubnetzmaske += BitsOktett[j];
                    }
                    UrsprünglicheSubnetzmaske += "." + Convert.ToString(RestwertSubnetzmaske);
                    Schleife = false;
                }
                else
                {
                    UrsprünglicheSubnetzmaske += ".0";
                }
            }
            int VB = 1;
            ProgrammKopf();
            string NeueSubnetzmaske = "255";
            int NeueSubnetzmaskeRest = 0;
            for (int i = 0; i < AnzahlNetzwerke; i++) //Verarbeiten und Ausgeben der Daten
            {
                Schleife = true;
                WelchesOktett = Präfix[i] / 8;
                Restwert = Präfix[i] % 8;
                for (int j = 1; j < 4; j++) //Ermittlung neuer Subnetzmaske für Teilnetzwerk
                {
                    if (j < WelchesOktett)
                    {
                        NeueSubnetzmaske += ".255";
                    }
                    else if ((OktettenSubnetzmaske == j) && !(WelchesOktett == OktettenSubnetzmaske))
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if (NeueSubnetzmaskeRest < RestwertSubnetzmaske)
                            {
                                NeueSubnetzmaskeRest += BitsOktett[k];
                            }
                        }
                        NeueSubnetzmaske += "." + Convert.ToString(NeueSubnetzmaskeRest);
                        NeueSubnetzmaskeRest = 0;
                    }
                    else if (WelchesOktett == j)
                    {
                        for (int k = 0; k < Restwert; k++)
                        {
                            NeueSubnetzmaskeRest += BitsOktett[k]; 
                        }
                        NeueSubnetzmaske += "." + Convert.ToString(NeueSubnetzmaskeRest);
                        NeueSubnetzmaskeRest = 0;
                    }
                    else
                    {
                        NeueSubnetzmaske += ".0";
                        NeueSubnetzmaskeRest = 0;
                    }
                }
                if (AdressOktettenPräfix[4] <= Präfix[i]) 
                {
                    if (i < AnzahlHosts.Length) //Ausgabe der Ergebnisse
                    {
                        Console.WriteLine($"{NetzwerkNamen[i]} Adresse: {AdressOktettenPräfix[0]}.{AdressOktettenPräfix[1]}.{AdressOktettenPräfix[2]}.{AdressOktettenPräfix[3]}/{Präfix[i]} Subnetzmaske: {NeueSubnetzmaske}");
                        NeueSubnetzmaske = "255";
                    }
                    else
                    {
                        Console.WriteLine($"Verbindungsnetzwerk {VB} Adresse: {AdressOktettenPräfix[0]}.{AdressOktettenPräfix[1]}.{AdressOktettenPräfix[2]}.{AdressOktettenPräfix[3]}/{Präfix[i]} Subnetzmaske: {NeueSubnetzmaske}");
                        NeueSubnetzmaske = "255";
                        VB++;
                    }
                    if (Restwert == 0) //Ermittlung neuer Netzadressen
                    {
                        AdressOktettenPräfix[WelchesOktett - 1] += 1;
                    }
                    else
                    {
                        if ((AdressOktettenPräfix[WelchesOktett] += BitsOktett[Restwert]) >= 256)
                        {
                            if ((AdressOktettenPräfix[WelchesOktett - 1] += +1) >= 256)
                            {
                                if ((AdressOktettenPräfix[WelchesOktett - 2] += 1) >= 256)
                                {
                                    Fehlerbehandlung(30);
                                }
                                else
                                {
                                    AdressOktettenPräfix[WelchesOktett - 2] += 1;
                                }

                            }
                            else
                            {
                                AdressOktettenPräfix[WelchesOktett - 1] += 1;
                            }
                        }
                        else
                        {
                            AdressOktettenPräfix[WelchesOktett] += BitsOktett[Restwert];
                        }
                    }
                }
                else //Fehlerbehandlung bei fehlerhaftem Präfix
                {
                    if (i < NetzwerkNamen.Length)
                    {
                        Console.WriteLine($"Das Netzwerk {NetzwerkNamen[i]} liegt außerhalb des angegebenen Netzbereiches");
                    }
                    else
                    {
                        Console.WriteLine($"Verbindungsnetzwerk {VB} liegt außerhalb des angegebenen Netzbereiches");
                    }
                    Fehlerbehandlung(3); //Anzahl Hosts zu groß
                }
            }
            Console.ReadKey();
        }
        static void Fehlerbehandlung(int Index) //Fehlerbehandlung
        {
            string[] Fehlerliste = new string[] { "Buchstaben verwendet bei der Eingabe der Anzahl der Teilnetzwerke verwendet", "Startadresse wurde nicht korrekt angegeben", "Präfix der Startadresse wurde nicht korrekt angegeben", "Anzahl der Hosts zu groß", "Buchstaben Verwendet bei der Angabe der Anzahl der Hosts" };
            SoundPlayer Sound = new SoundPlayer("Computer sagt nein.wav");
            try
            {
                Sound.Play();
            }
            catch
            {

            }
            if (Index < 3 || Index == 4)
            {
                ProgrammKopf();
            }
            Console.WriteLine($"Es ist ein Fehler aufgetreten: {Fehlerliste[Index]}\n\nWeiter mit beliebiger Taste");
            Console.ReadKey();
            Console.Clear();
        }
        static void ProgrammKopf() //Progammkopf
        {
            Console.WriteLine("Dynamisches Subnettieren IPv4\r\nCopyright (C) 2023  Rick Kummer\r\n");
        }
        static int PräfixErmitteln(int AnzahlHosts) //Präfix Funktion
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

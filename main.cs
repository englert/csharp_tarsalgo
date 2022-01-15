/*
9 1 2 be
9 1 9 be
ora perc id irany
*/

using System;                       // Console
using System.IO;                    // StreamReader() StreamWriter()
using System.Collections.Generic;   // List<>
using System.Linq;                  // from where select

class Tarsalgo
{
    public int      ora         {set; get;}
    public int      perc        {set; get;}
    public int      id          {set; get;}
    public string   irany       {set; get;}
    public int      ido_percben {set; get;}
    public int      bent        {set; get;}
    static int szamlalo  {set; get;}

    public Tarsalgo(string sor)
    {
        var s = sor.Split(" ");
        ora     = int.Parse(s[0]);
        perc    = int.Parse(s[1]);
        id      = int.Parse(s[2]);
        irany   =           s[3];
        ido_percben = perc + 60*ora;

        if (irany == "be") szamlalo += +1;
        else               szamlalo += -1;
        bent = szamlalo;  
    }
}

class Program 
{
  public static void Main (string[] args) 
  {
   var lista = new List<Tarsalgo>();
   var sr = new StreamReader("ajto.txt");
   
   while(!sr.EndOfStream)
   {
       lista.Add(new Tarsalgo(sr.ReadLine()));
   }
   sr.Close();

    //2. Írja a képernyőre annak a személynek az azonosítóját, aki a vizsgált időszakon belül először
    //lépett be az ajtón, és azét, aki utoljára távozott a megfigyelési időszakban! 

    var belepok = 
    (
        from sor in lista
        where sor.irany == "be"
        orderby sor.ido_percben
        select sor.id
    );
          
    var kilepok = 
    (
        from sor in lista
        where sor.irany == "ki"
        orderby sor.ido_percben
        select sor.id
    );
         
    Console.WriteLine($"2. feladat");
    Console.WriteLine($"Az első belépő: {belepok.First()}");
    Console.WriteLine($"Az utolsó kilépő: {kilepok.Last()}");
    Console.WriteLine();
  
    /*3. Határozza meg a fájlban szereplő személyek közül, ki hányszor haladt át a társalgó ajtaján!
    A meghatározott értékeket azonosító szerint növekvő sorrendben írja az athaladas.txt
    fájlba! Soronként egy személy azonosítója, és tőle egy szóközzel elválasztva az
    áthaladások száma szerepeljen!*/

    var sw = new StreamWriter("athaladas.txt");

    var mozgasok = 
    (
        from sor in lista
        group sor by sor.id
    );

    foreach(var sor in mozgasok)
    {
        sw.WriteLine($"{sor.Key}  {sor.Count()}");
    }
    sw.Close();

    /*4. Írja a képernyőre azon személyek azonosítóját, akik a vizsgált időszak végén a társalgóban
    tartózkodtak! */

    var szemelyek = 
    (
        from sor in lista
        orderby sor.id
        group sor by sor.id
    );

    Console.WriteLine("4. feladat");
    Console.Write("A végén a társalgóban voltak:");
    foreach(var sor in szemelyek)
    {   
        if(sor.Count() % 2 != 0) Console.Write($" {sor.Key}");
    }
    Console.WriteLine(" ");
    
    /*5. Hányan voltak legtöbben egyszerre a társalgóban? Írjon a képernyőre egy olyan időpontot
    (óra:perc), amikor a legtöbben voltak bent!*/
    
    var bent = 
    (
        from sor in lista
        orderby sor.bent
        select sor
    ).Last();

    Console.WriteLine($"5. feladat");
    Console.WriteLine($"Például {bent.ora}:{bent.perc}-kor voltak a legtöbben a társalgóban.");
     
    /*
    6. Kérje be a felhasználótól egy személy azonosítóját! 
    A további feladatok megoldásánál ezt használja fel!
    */
    Console.WriteLine($"6. feladat");
    Console.Write($"Adja meg a személy azonosítóját! "); 
    var id = int.Parse(Console.ReadLine());

    /*
    7. Írja a képernyőre, hogy a beolvasott azonosítóhoz tartozó személy mettől meddig
    tartózkodott a társalgóban!
    A kiírást az alábbi, 22-es személyhez tartozó példának megfelelően alakítsa ki:
    11:22-11:27
    13:45-13:47
    13:53-13:58
    14:17-14:20
    14:57-
    */
    var id_mozgasok =
    (
        from sor in lista
        where sor.id == id
        select sor
    );
    Console.WriteLine($"7. feladat");
    foreach (var item in id_mozgasok)
    {
        if (item.irany =="be") Console.Write($"{item.ora}:{item.perc}-");
        else                   Console.WriteLine($"{item.ora}:{item.perc}");
    }
    Console.WriteLine();
    /*
    8. Határozza meg, hogy a megfigyelt időszakban a beolvasott azonosítójú személy összesen
    hány percet töltött a társalgóban! Az előző feladatban példaként szereplő 22-es személy 5
    alkalommal járt bent, a megfigyelés végén még bent volt. Róla azt tudjuk, hogy 18 percet
    töltött bent a megfigyelés végéig. A 39-es személy 6 alkalommal járt bent, a vizsgált
    időszaanyodott a helyiségben. Róla azt tudjuk, hogy 39 percet töltött ott.
    Írja ki, hogy a beolvasott azonosítójú személy mennyi időt volt a társalgóban, és a
    megfigyelési időszak végén bent volt-e még!
    Minta:
    8. feladat
    A(z) 22. személy összesen 18 percet volt bent, a megfigyelés
    végén a társalgóban volt.
    */
    var osszes_perc = 
    (
        from sor in lista
        where sor.id == id
        let time = sor.irany == "be" ? -sor.ido_percben : +sor.ido_percben
        select time
    ).Sum();
    
    int bent_toltott_perc = 0;
    bool bent_maradt;

    if (osszes_perc < 0) bent_maradt = true;
    else                 bent_maradt = false;

    if (bent_maradt) bent_toltott_perc = osszes_perc + 15*60;
    else             bent_toltott_perc = osszes_perc;
    
    Console.WriteLine($"8. feladat");
    Console.WriteLine($"A(z) {id}. személy összesen {bent_toltott_perc} percet volt bent, a megfigyelés");
    if (bent_maradt) Console.WriteLine($"végén a társalgóban volt.");
    else         Console.WriteLine($"végén nem volt a társalgóban.");
  }
}
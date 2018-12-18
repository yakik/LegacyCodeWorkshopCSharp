using System;
using System.Collections.Generic;
using System.IO;

namespace GildedRose
{
    public class GildedRoseManager
    {
        
        static readonly System.String[] Options = new string[20];
        static int _numberOfOptions;
        public void Run()
        {
           
            var ResponseTitle = string.Empty;

            _numberOfOptions = 0;

            ResponseTitle = "\n\n\n-----Gilded Rose Management System-----\n\n" +
                             "OUR STOCK:\n";


            //read from configuration file
            reloadConfig:

            Options[0] = string.Empty;
            int j1;
            try
            {
                StreamReader streamReader = new StreamReader("gildedRoseConfig.txt");
                j1 = -1;
                while (streamReader.Peek() >= 0)
                {
                    char charRead1 = (char)streamReader.Read();
                    if (charRead1 == '@')
                    {
                        j1++;
                        _numberOfOptions++;
                        Options[j1] = string.Empty;


                    }
                    else
                        Options[j1] += char.ToString(charRead1);
                }
                streamReader.Close();

            }
            catch (IOException ex)
            {
                try
                {
                    StreamWriter streamWriter1 = new StreamWriter("gildedRoseConfig.txt");

                    streamWriter1.Write("@/Tick!");
                    streamWriter1.Write("@/Quit!");
                    streamWriter1.Write("@/addItem:Aged_Brie/quality:12/expiresInDays:10/");
                    streamWriter1.Write("@/addItem:+5_Dexterity_Vest/quality:20/expiresInDays:10/");
                    streamWriter1.Write("@/addItem:Elixir_of_the_Mongoose/quality:7/expiresInDays:5/");
                    streamWriter1.Write("@/addItem:Sulfuras,_Hand_of_Ragnaros/quality:80/expiresInDays:5/");
                    streamWriter1.Write("@/addItem:Backstage_passes_to_a_TAFKAL80ETC_concert/quality:20/expiresInDays:15/");


                    streamWriter1.Close();
                    goto reloadConfig;
                }
                catch (IOException ex2) { Console.WriteLine(ex2.Message); }

            }


            // load inventory from disk
            string responseBody = String.Empty;
            IList<Item> Items = new List<Item> { };

            string[] currentInventory = new string[40];
            currentInventory[0] = string.Empty;
            int j = -1;
            try
            {
                StreamReader fileReader = new StreamReader("inventory.txt");

                while (fileReader.Peek() >= 0)
                {
                    char charRead = (char)fileReader.Read();
                    if (charRead == '@')
                    {
                        j++;
                        currentInventory[j] = string.Empty;


                    }
                    else
                        currentInventory[j] += char.ToString(charRead);
                }

                fileReader.Close();
                for (int i1 = 0;
                    i1 < j + 1;
                    i1++)
                    Items.Add(new Item
                    {
                        Name = GetStringBetweenStrings(currentInventory[i1],
                            "addItem:",
                            "/"),
                        SellIn = Int32.Parse(GetStringBetweenStrings(currentInventory[i1],
                            "expiresInDays:",
                            "/")),
                        Quality = Int32.Parse(GetStringBetweenStrings(currentInventory[i1],
                            "quality:",
                            "/"))
                    });

            }
            catch (IOException ex)
            {

            }

            // Enter the listening loop.
            string ResponseBody1;
            while (true)
            {
                ResponseBody1 = "\n";
                int i;
                for (i = 0;
                    i < Items.Count;
                    i++)
                    ResponseBody1 += Items[i]
                                     .Name +
                                     "-> Quality:" +
                                     Items[i]
                                     .Quality +
                                     ", SellIn: " +
                                     Items[i]
                                     .SellIn +
                                     " days \n";
                // string response = responseTitle + responseBody + responseOptions;

                Console.Write(ResponseTitle);
                Console.Write(ResponseBody1);
                Console.Write("\nMENU OPTIONS:\n\n");
                for (i = 0; i < _numberOfOptions; i++)
                    Console.Write(i.ToString() + ": " + Options[i] + "\n");

                string UserInput = Console.ReadLine();
                int selectedOption = Int32.Parse(UserInput);
                string myRequest = Options[selectedOption];

                Console.WriteLine("Received: {0}",
                    myRequest);


                if (myRequest.IndexOf("/addItem",
                        0,
                        StringComparison.Ordinal) >=
                    0)
                    Items.Add(new Item
                    {
                        //Defect 157 Yaki
                        //Fixed!
                        Name = GetStringBetweenStrings(myRequest,
                            "/addItem:",
                            "/"),
                        SellIn = Int32.Parse(GetStringBetweenStrings(myRequest,
                            "expiresInDays:",
                            "/")),
                        Quality = Int32.Parse(GetStringBetweenStrings(myRequest,
                            "quality:",
                            "/"))
                    });
                if (myRequest.IndexOf("Quit!",
                        0,
                        StringComparison.Ordinal) >
                    0)
                    break;

                if (myRequest.IndexOf("Tick!",
                        0,
                        StringComparison.Ordinal) >
                    0)

                    for (i = 0;
                        i < Items.Count;
                        i++)
                    {
                        //Big IF, very proud of it, wanted to document it down but I just don't have the time
                        if (Items[i]
                            .Name !=
                            "Aged_Brie" &&
                            Items[i]
                            .Name !=
                            "Backstage_passes_to_a_TAFKAL80ETC_concert")
                        {
                            if (Items[i]
                                .Quality >
                                0)
                            {
                                if (Items[i]
                                    .Name !=
                                    "Sulfuras,_Hand_of_Ragnaros")
                                {
                                    Items[i]
                                        .Quality = Items[i]
                                                   .Quality -
                                                   1;
                                }
                            }
                        }
                        else
                        {
                            if (Items[i]
                                .Quality <
                                50)
                            {
                                Items[i]
                                    .Quality = Items[i]
                                               .Quality +
                                               1;
                                //To Do in future, need to make code clearer. Very Important!

                                if (Items[i]
                                    .Name ==
                                    "Backstage_passes_to_a_TAFKAL80ETC_concert")
                                {
                                    if (Items[i]
                                        .SellIn <
                                        11)
                                    {
                                        if (Items[i]
                                            .Quality <
                                            50)
                                        {
                                            Items[i]
                                                .Quality = Items[i]
                                                           .Quality +
                                                           1;
                                        }
                                    }
                                    /***********************************
                         * To handle if sellIn <6
                         * Special consideration
                         * Need to put good attention
                         * 
                         * 
                         * **********************************/

                                    if (Items[i]
                                        .SellIn <
                                        6)
                                    {
                                        if (Items[i]
                                            .Quality <
                                            50)
                                        {
                                            Items[i]
                                                .Quality = Items[i]
                                                           .Quality +
                                                           1;
                                        }
                                    }
                                }
                            }
                        }

                        if (Items[i]
                            .Name !=
                            "Sulfuras,_Hand_of_Ragnaros")
                        {
                            Items[i]
                                .SellIn = Items[i]
                                          .SellIn -
                                          1;
                        }

                        if (Items[i]
                            .SellIn <
                            0)
                        {
                            if (Items[i]
                                .Name !=
                                "Aged_Brie")
                            {
                                if (Items[i]
                                    .Name !=
                                    "Backstage_passes_to_a_TAFKAL80ETC_concert")
                                {
                                    if (Items[i]
                                        .Quality >
                                        0)
                                    {
                                        if (Items[i]
                                            .Name !=
                                            "Sulfuras,_Hand_of_Ragnaros")
                                        {
                                            Items[i]
                                                .Quality = Items[i]
                                                           .Quality -
                                                           1;
                                        }
                                    }
                                }
                                else
                                {
                                    Items[i]
                                        .Quality = Items[i]
                                                   .Quality -
                                                   Items[i]
                                                   .Quality;
                                }
                            }
                            else
                            {
                                if (Items[i]
                                    .Quality <
                                    50)
                                {
                                    Items[i]
                                        .Quality = Items[i]
                                                   .Quality +
                                                   1;
                                }
                            }
                        }
                    }

                try
                {
                    StreamWriter streamWriter = new StreamWriter("inventory.txt",
                        false);
                    for (int index = 0;
                        index < Items.Count;
                        index++)

                        streamWriter.Write("@addItem:" +
                                           Items[index]
                                           .Name +
                                           "/quality:" +
                                           Items[index]
                                           .Quality +
                                           "/expiresInDays:" +
                                           Items[index]
                                           .SellIn +
                                           "/");
                    streamWriter.Close();
                }
                catch (IOException ex2)
                {
                    Console.WriteLine(ex2.Message);
                }

                //Console.WriteLine("Sent: {0}", data);
            }
        }

        public static string GetStringBetweenStrings(string source,
            string from,
            string to)
        {
            int i,
                j;
            i = source.IndexOf(from,
                    0,
                    StringComparison.Ordinal) +
                from.Length;
            j = source.IndexOf(to,
                i,
                StringComparison.Ordinal);
            return source.Substring(i,
                j - i);
        }

    }
}
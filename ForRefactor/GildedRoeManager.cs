using System;
using System;
using System.Collections.Generic;
using System.IO;

namespace GildedRose
{
    public class GildedRoeManager
    {
        static private string[] options = new string[20];
        static private int numberOfOptions;
        public void run()
        {
           
            var responseTitle = string.Empty;

            numberOfOptions = 0;

            responseTitle = "\n\n\n-----Gilded Rose Management System-----\n\n" +
                             "OUR STOCK:\n";

            loadConfiguration();


            // load inventory from disk
            string responseBody = String.Empty;
            IList<Item> Items = new List<Item> { };

            loadPersistence(Items);

            // Enter the listening loop.
            while (true)
            {

                responseBody = "\n";
                int i;
                for (i = 0;
                    i < Items.Count;
                    i++)
                    responseBody += Items[i]
                                        .Name +
                                    "-> Quality:" +
                                    Items[i]
                                        .Quality +
                                    ", SellIn: " +
                                    Items[i]
                                        .SellIn +
                                    " days \n";
                // string response = responseTitle + responseBody + responseOptions;

                Console.Write(responseTitle);
                Console.Write(responseBody);
                Console.Write("\nMENU OPTIONS:\n\n");
                for (i = 0; i < numberOfOptions; i++)
                    Console.Write(i.ToString() + ": " + options[i] + "\n");

                string userInput = Console.ReadLine();
                int selectedOption = Int32.Parse(userInput);
                string myRequest = options[selectedOption];

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
                        processItems(Items, i);

                writeToDisk(Items);

                //Console.WriteLine("Sent: {0}", data);
            }






        }

        private static void writeToDisk(IList<Item> Items)
        {
            try
            {
                StreamWriter c_fileWriter = new StreamWriter("inventory.txt",
                    false);
                for (int index = 0;
                    index < Items.Count;
                    index++)

                    c_fileWriter.Write("@addItem:" +
                                       Items[index]
                                           .Name +
                                       "/quality:" +
                                       Items[index]
                                           .Quality +
                                       "/expiresInDays:" +
                                       Items[index]
                                           .SellIn +
                                       "/");
                c_fileWriter.Close();
            }
            catch (IOException ex2)
            {
                Console.WriteLine(ex2.Message);
            }
        }

        private static void processItems(IList<Item> Items, int i)
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

        private static void loadPersistence(IList<Item> Items)
        {
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
                for (int i = 0;
                    i < j + 1;
                    i++)
                    Items.Add(new Item
                    {
                        Name = GetStringBetweenStrings(currentInventory[i],
                            "addItem:",
                            "/"),
                        SellIn = Int32.Parse(GetStringBetweenStrings(currentInventory[i],
                            "expiresInDays:",
                            "/")),
                        Quality = Int32.Parse(GetStringBetweenStrings(currentInventory[i],
                            "quality:",
                            "/"))
                    });




            }
            catch (IOException ex)
            {
                ex.ToString();


            }
        }

        private static void loadConfiguration()
        {
            //read from configuration file
            reloadConfig:



            options[0] = string.Empty;
            int j;
            try
            {
                StreamReader c_fileReader = new StreamReader("gildedRoseConfig.txt");
                j = -1;
                while (c_fileReader.Peek() >= 0)
                {
                    char charRead = (char)c_fileReader.Read();
                    if (charRead == '@')
                    {
                        j++;
                        numberOfOptions++;
                        options[j] = string.Empty;


                    }
                    else
                        options[j] += char.ToString(charRead);
                }
                c_fileReader.Close();

            }
            catch (IOException ex)
            {
                ex.ToString();
                try
                {
                    StreamWriter c_fileWriter = new StreamWriter("gildedRoseConfig.txt");

                    c_fileWriter.Write("@/Tick!");
                    c_fileWriter.Write("@/Quit!");
                    c_fileWriter.Write("@/addItem:Aged_Brie/quality:12/expiresInDays:10/");
                    c_fileWriter.Write("@/addItem:+5_Dexterity_Vest/quality:20/expiresInDays:10/");
                    c_fileWriter.Write("@/addItem:Elixir_of_the_Mongoose/quality:7/expiresInDays:5/");
                    c_fileWriter.Write("@/addItem:Sulfuras,_Hand_of_Ragnaros/quality:80/expiresInDays:5/");
                    c_fileWriter.Write("@/addItem:Backstage_passes_to_a_TAFKAL80ETC_concert/quality:20/expiresInDays:15/");


                    c_fileWriter.Close();
                    goto reloadConfig;
                }
                catch (IOException ex2) { Console.WriteLine(ex2.Message); }

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

        public static string GetHTTPResponse(string content)
        {
            string myResponse;
            myResponse =
                "HTTP/1.1 200 OK\r\n" +
                "Date: Mon, 27 Jul 2009 12:28:53 GMT\r\n" +
                "Server: YAKIKOREN\r\n" +
                "Last-Modified: Wed, 1 Jan 2018 19:15:56 GMT\r\n" +
                "Content-Length:" +
                content.Length.ToString() +
                "\r\n" +
                "Content-Type: text/html\r\n" +
                "Connection: Closed\r\n" +
                "\r\n" +
                content;
            return myResponse;

        }
    }
}
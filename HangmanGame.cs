using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Hangman
{
    public class HangmanGame
    {
        public List<string> WordsStockRus = new List<string>();

        public object[] Conditions = new object[3];

        public void Start()
        {
            //reading the dictionary from the file;
            DictionaryCreate();

            //getting a random word from a dictionary;
            string sample = GetRandomizedWord();

            //making logics of moves;
            int attempts = 0;

            //storing the input into variable of char;
            char input;

            //storing the encrypted string full of *;
            string encrypted = Encryption(sample);

            //creating a mistakes storage as an empty string;
            string mistakes = String.Empty;

            while (attempts < 6)
            {
                Console.WriteLine("Укажите букву, которая должна быть в слове:");

                string inputString = Console.ReadLine().ToLower();

                input = inputString[0];
                // creating a defence from the entering more than 1 symbol;
                while (inputString.Length < 1 || inputString.Length > 1)
                {
                    if (inputString.Length == 1)
                    {
                        input = inputString[0];
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Вставьте только 1 букву!");
                        inputString = Console.ReadLine();
                    }
                }

                //making a move; reassigning encrypted to show the chars, that was found previously;
                Turn(sample, encrypted, input);
                encrypted = (string)Conditions[2];

                //converting the data on [i] to clause and checking it;
                bool clause = (bool)Conditions[0];

                //storing and converting all the mistakes user made;
                mistakes += Conditions[1] + " ";
                Console.WriteLine($"Общие ошибки: {mistakes} ");

                //checking the condition of 6 attempts and incrementing it if it is false;
                if (!clause)
                {
                    attempts++;
                }

                if (attempts >= 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ты проиграл!");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (encrypted == sample)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Ты побeдил! Слово: {sample}");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
            }
        }

        public object[] Turn(string word, string encrypted, char input)
        {
            string result = String.Empty;
            string mistakes = String.Empty;
            int counter = 0;

            bool clause = false;

            char[] charArr = word.ToCharArray();
            char[] encryptedCharArr = encrypted.ToCharArray();

            for (int i = 0; i < word.Length; i++)
            {
                if (input == charArr[i])
                {
                    encryptedCharArr[i] = input;
                    clause = true;
                }
                while (counter < 1)
                {
                    if (!(word.Contains(input)))
                    {
                        mistakes += input.ToString();
                    }

                    counter++;
                }

                result += encryptedCharArr[i].ToString();
            }

            Conditions[0] = clause;
            Conditions[1] = mistakes;
            Conditions[2] = result;

            Console.WriteLine(result);

            return Conditions;
        }

        public string Encryption(string str)
        {
            char[] arrChars = str.ToCharArray();
            string result = String.Empty;

            for (int i = 0; i < arrChars.Length; i++)
            {
                //replacing the letters with *;
                arrChars[i] = '*';
                //creating a string of str.Length length from *;
                result += arrChars[i].ToString();
            }

            return result;
        }

        public string GetRandomizedWord()
        {
            Random index = new Random();
            int value = index.Next(0, WordsStockRus.Count);

            return WordsStockRus[value];
        }

        public List<string> DictionaryCreate()
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(@"WordsStockRus.txt", FileMode.Open, FileAccess.Read);

                byte[] tempBuffer = new byte[fs.Length];

                int bytesToRead = (int)fs.Length;
                int bytesRead = 0;

                while (bytesToRead > 0)
                {
                    int n = fs.Read(tempBuffer, bytesRead, bytesToRead);

                    if (n == 0) break;

                    bytesRead += n;
                    bytesToRead -= n;
                }

                //creating a full string array with all of the words divided as a values;
                string[] bufStr = Encoding.Default.GetString(tempBuffer).Split("\r\n");

                foreach (var words in bufStr)
                {
                    WordsStockRus.Add(words);
                }

                /*Console.WriteLine(bufStr.Length);
                Console.WriteLine(WordsStockRus.Count);*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                fs.Close();
            }

            return WordsStockRus;
        }
    }
}
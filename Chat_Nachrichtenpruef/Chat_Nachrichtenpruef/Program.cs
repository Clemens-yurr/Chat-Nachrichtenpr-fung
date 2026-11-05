using Chat_Nachrichtenpruef;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat_Nachrichtenpruef
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var emptyHandler = new EmptyMessageHandler();
            var badWordsHandler = new BadWordsHandler();
            var shoutingHandler = new ShoutingHandler();
            var lengthHandler = new LengthLimitHandler();

            emptyHandler.SetNext(badWordsHandler)
                        .SetNext(shoutingHandler)
                        .SetNext(lengthHandler);

            Console.WriteLine("Chat-Moderation (Chain of Responsibility)");
            Console.WriteLine("Gib eine Nachricht ein (leer = Ende):\n");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                // Testfälle für die Eingabe:
                // "HALLO ZUSAMMEN ICH BIN SO LAUT!!!"
                // "Du bist voll blöd"
                // "Hi, wie geht's?"

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Programm beendet");
                    break;
                }

                var trace = new List<string>();
                var result = emptyHandler.Handle(input, trace);

                if (result.isValid)
                {
                    Console.WriteLine($"✅ Erlaubt - OK");
                }
                else
                {
                    Console.WriteLine($"❌ Blockiert - {result.reason}");
                }

                Console.WriteLine("Trace: " + string.Join(" → ", trace) + "\n");
            }
        }
    }
}
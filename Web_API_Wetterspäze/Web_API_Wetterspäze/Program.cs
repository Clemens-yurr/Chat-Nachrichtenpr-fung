using System;
using System.Collections.Generic;
using System.Text;

namespace _4AHINF_1POS_PLF_Aufgabe2_Deutinger
{
    public class ModerationResult
    {
        public bool IsAllowed { get; set; } = true;
        public string Reason { get; set; } = "OK";
        public List<string> Trace { get; } = new List<string>();
    }


    public abstract class MessageHandler
    {
        protected MessageHandler? _nextHandler;

        public MessageHandler SetNext(MessageHandler nextHandler)
        {
            _nextHandler = nextHandler;
            return nextHandler;
        }

        public virtual void Handle(string message, ModerationResult result)
        {

            result.Trace.Add(this.GetType().Name);

            if (!Check(message, out string failureReason))
            {
                result.IsAllowed = false;
                result.Reason = failureReason;
                return;
            }

            _nextHandler?.Handle(message, result);
        }

        protected abstract bool Check(string message, out string reason);
    }

    public class EmptyMessageHandler : MessageHandler
    {
        protected override bool Check(string message, out string reason)
        {
            bool onlyWhitespace = true;
            foreach (char c in message)
            {
                if (!char.IsWhiteSpace(c))
                {
                    onlyWhitespace = false;
                    break;
                }
            }

            if (message.Length == 0 || onlyWhitespace)
            {
                reason = "Leere Nachricht";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    public class BadWordsHandler : MessageHandler
    {

        private readonly HashSet<string> _badWords = new HashSet<string>
        {
            "blöd", "dumm", "idiot", "depp"
        };

        protected override bool Check(string message, out string reason)
        {
            string lowerMessage = message.ToLower();

            foreach (string badWord in _badWords)
            {
                if (lowerMessage.Contains(badWord))
                {
                    reason = "Unangemessene Sprache";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }
    }

    public class ShoutingHandler : MessageHandler
    {
        protected override bool Check(string message, out string reason)
        {
            int letterCount = 0;
            int upperCount = 0;

            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    letterCount++;
                    if (char.IsUpper(c))
                    {
                        upperCount++;
                    }
                }
            }

            if (letterCount > 0)
            {
                double upperPercentage = (double)upperCount / letterCount;
                if (upperPercentage >= 0.80)
                {
                    reason = "Bitte nicht schreien";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }
    }

    public class LengthLimitHandler : MessageHandler
    {
        protected override bool Check(string message, out string reason)
        {
            if (message.Length > 120)
            {
                reason = "Nachricht zu lang";
                return false;
            }

            reason = string.Empty;
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            MessageHandler emptyHandler = new EmptyMessageHandler();
            MessageHandler badWordsHandler = new BadWordsHandler();
            MessageHandler shoutingHandler = new ShoutingHandler();
            MessageHandler lengthHandler = new LengthLimitHandler();

            emptyHandler
                .SetNext(badWordsHandler)
                .SetNext(shoutingHandler)
                .SetNext(lengthHandler);

            Console.WriteLine("Chat-Moderation (Chain of Responsibility)");
            Console.WriteLine("Gib eine Nachricht ein (leer = Ende):\n");

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Programm beendet");
                    break;
                }

                ModerationResult result = new ModerationResult();
                emptyHandler.Handle(input, result);

                if (result.IsAllowed)
                {
                    Console.WriteLine($"Erlaubt - {result.Reason}");
                }
                else
                {
                    Console.WriteLine($"Blockiert - {result.Reason}");
                }

                Console.WriteLine("Trace: " + string.Join(" → ", result.Trace));
                Console.WriteLine();
            }
        }
    }
}
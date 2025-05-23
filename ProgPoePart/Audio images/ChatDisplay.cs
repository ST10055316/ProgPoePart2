using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class ChatDisplay
    {
        
            public static void DisplayMainMenu()
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nCYBER AWARENESS MENU");
                Console.WriteLine("1. Password Security");
                Console.WriteLine("2. Phishing Threats");
                Console.WriteLine("3. Secure Browsing");
                Console.WriteLine("4. Social Media Safety");
                Console.WriteLine("5. About This Hub");
                Console.WriteLine("(Type 'exit' to quit)");
                Console.ResetColor();
                Console.Write("\n🛡️  What cyber topic interests you today? ");
            }

            public static void DisplayFarewell(string userName)
            {
                UIHelper.ColorTypeWrite($"\n👋 Goodbye, {userName}!\n", ConsoleColor.Cyan);
                UIHelper.ColorTypeWrite("Remember to practice what you've learned today.\n", ConsoleColor.White);
                UIHelper.ColorTypeWrite("Your cybersecurity is in your hands!\n\n", ConsoleColor.White);
                Thread.Sleep(2000);
            }

            
        }
    }


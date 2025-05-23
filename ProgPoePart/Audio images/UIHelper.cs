using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class UIHelper
    {
        
            public static void ColorTypeWrite(string message, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                foreach (char c in message)
                {
                    Console.Write(c);
                    Thread.Sleep(20);
                }
                Console.ResetColor();
            }

            public static string GetUserName()
            {
                string userName = "";
                while (string.IsNullOrWhiteSpace(userName))
                {
                    ColorTypeWrite("\n🔒 Welcome to Cyber Awareness Hub!\n", ConsoleColor.Yellow);
                    ColorTypeWrite("Before we begin, what should I call you? ", ConsoleColor.White);
                    userName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        ColorTypeWrite("⚠️  I didn't catch that. Please enter your name: ", ConsoleColor.Red);
                    }
                }

                Console.WriteLine();
                ColorTypeWrite($"🛡️  Welcome, {userName}! I'm your Cyber Awareness Assistant.\n", ConsoleColor.Green);
                ColorTypeWrite("I'm here to help you stay safe in the digital world.\n\n", ConsoleColor.Cyan);
                Thread.Sleep(800);

                return userName;
            }
        }
    }


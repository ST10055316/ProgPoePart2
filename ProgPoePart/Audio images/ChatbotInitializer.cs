using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class ChatbotInitializer
    {
        
            public static void Run()
            {
                Console.Title = "Cyber Awareness Hub";
                ArtAssets.DisplayCyberShieldArt();
                AudioManager.PlayWelcomeAudio();
                UIHelper.ColorTypeWrite("Initializing Cyber Awareness Hub...\n", ConsoleColor.Cyan);
                Thread.Sleep(1000);

                string userName = UIHelper.GetUserName();
                ChatSession.Run(userName);
            }
        
        static class ArtAssets
        {
            public static void DisplayCyberShieldArt()
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"...ASCII Art...");
                Console.ResetColor();
            }
        }

        static class AudioManager
        {
            public static void PlayWelcomeAudio()
            {
                try
                {
                    string filePath = Path.Combine("Audio images", "greetings.wav");
                    SoundPlayer player = new SoundPlayer(filePath);
                    player.Play();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("[Audio greeting not found]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error playing audio]: {ex.Message}");
                }
            }
        }

        static class UIHelper
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
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class ChatLogic
    {
        
        
            public static void ProcessInput(string input, string userName)
            {
                Console.WriteLine();

                switch (input.ToLower())
                {
                    case "1":
                    case "password":
                    case "password security":
                        Topics.DisplayPasswordSecurity(userName);
                        break;
                    case "2":
                    case "phishing":
                    case "phishing threats":
                        Topics.DisplayPhishingThreats(userName);
                        break;
                    case "3":
                    case "browsing":
                    case "secure browsing":
                        Topics.DisplaySecureBrowsing(userName);
                        break;
                    case "4":
                    case "social media":
                    case "social media safety":
                        Topics.DisplaySocialMediaSafety(userName);
                        break;
                    case "5":
                    case "about":
                        Topics.DisplayAboutHub();
                        break;
                    default:
                        UIHelper.ColorTypeWrite("⚠️  I'm not sure about that topic. ", ConsoleColor.Red);
                        break;
                }
            }
        }
    }


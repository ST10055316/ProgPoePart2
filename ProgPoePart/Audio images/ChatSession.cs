using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class ChatSession
    {
       
        
            public static void Run(string userName)
            {
                bool continueChat = true;
                while (continueChat)
                {
                    ChatDisplay.DisplayMainMenu();
                    string userInput = InputManager.GetUserInput();

                    if (userInput.ToLower() == "exit")
                    {
                        UIHelper.ColorTypeWrite("Are you sure you want to exit? (yes/no): ", ConsoleColor.Yellow);
                        string confirm = Console.ReadLine()?.Trim().ToLower();
                        if (confirm == "yes" || confirm == "y")
                        {
                            continueChat = false;
                            ChatDisplay.DisplayFarewell(userName);
                        }
                    }
                    else
                    {
                        ChatLogic.ProcessInput(userInput, userName);
                    }
                }
            }
        }
    }


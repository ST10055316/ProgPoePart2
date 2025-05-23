using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgPoePart.Audio_images
{
    internal class InputManager
    {
        
        
            public static string GetUserInput()
            {
                string input = Console.ReadLine()?.Trim() ?? "";
                while (string.IsNullOrWhiteSpace(input))
                {
                    UIHelper.ColorTypeWrite("⚠️  Please enter a valid choice: ", ConsoleColor.Red);
                    input = Console.ReadLine()?.Trim() ?? "";
                }
                return input;
            }
        }
    }


using System;
using System.Media;
using System.IO;

namespace ProgPoePart
{
    internal class AudioPlayer
    {
         string filePath = "Audio images/greetings.wav"; // Adjust path if needed

        public void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.PlaySync(); // or player.Play() for async
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[Audio greeting not found]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[An error occurred while playing audio]: {ex.Message}");
            }
        }
    }
}

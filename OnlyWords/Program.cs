using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;

namespace OnlyWords
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @".\words.txt";
            var words = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var reader = File.OpenText(filePath))
            {
                string line = null;
                while((line = reader.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }

            var mistakes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var correct = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var random = new Random();
            
            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesizer synth = new SpeechSynthesizer();
            // Configure the audio output. 
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Child);

            while (words.Count > 0)
            {
                int index = random.Next(0, words.Count - 1);
                string response = null;
                var word = words.ElementAt(index);
                do
                {
                    synth.Speak(word);
                    response = Console.ReadLine();
                } while (string.IsNullOrEmpty(response));
                if (string.Equals(word, response, StringComparison.OrdinalIgnoreCase))
                {
                    synth.Speak("Good job!");
                    words.Remove(word);
                    mistakes.Remove(word);
                }
                else
                {
                    synth.Speak("Oops, the correct way to spell it is ");
                    Console.WriteLine(word);
                    mistakes.Add(word);
                }

                if (mistakes.Count > 0)
                {
                    Console.WriteLine($"Your mistakes so far {string.Join(",", mistakes)}");
                }
            }

        }
    }
}

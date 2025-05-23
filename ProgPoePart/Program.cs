using System;
using System.Collections.Generic;
using System.Linq;
using System.Media; // Required for SoundPlayer
using System.Threading;
using System.IO;

class CyberAwarenessChatbot
{
    // --- Chatbot Core Logic Fields ---
    private Dictionary<string, string> keywordResponses = new Dictionary<string, string>();
    private Dictionary<string, List<string>> randomResponses = new Dictionary<string, List<string>>();
    private Dictionary<string, List<string>> sentimentResponses = new Dictionary<string, List<string>>();

    // Memory and Recall
    private Dictionary<string, string> userData = new Dictionary<string, string>();
    private string currentTopic = string.Empty; // For conversation flow

    private Random random = new Random();
    // --- End Chatbot Core Logic Fields ---

    // Constructor to initialize chatbot components
    public CyberAwarenessChatbot()
    {
        InitializeKeywordResponses();
        InitializeRandomResponses();
        InitializeSentimentResponses();
    }

    // --- Initialization Methods for Chatbot Data ---
    private void InitializeKeywordResponses()
    {
        // Keyword Recognition: At least three keywords related to cybersecurity
        keywordResponses.Add("password", "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.");
        keywordResponses.Add("scam", "Be cautious of emails, messages, or calls asking for personal information or money, especially if they create a sense of urgency.");
        keywordResponses.Add("privacy", "Review the security settings on your accounts and consider using privacy-focused browsers and tools to protect your personal information online.");
        keywordResponses.Add("phishing", "Phishing attempts often involve deceptive emails or websites designed to steal your credentials. Always verify the sender and the URL before clicking any links.");
        keywordResponses.Add("malware", "Malware includes viruses, ransomware, and spyware. Keep your antivirus software updated and be careful about downloading files from untrusted sources.");
        keywordResponses.Add("security", "Cybersecurity is about protecting your digital life. It involves practices like strong passwords, being wary of scams, and understanding privacy settings.");
        keywordResponses.Add("browsing", "Secure browsing involves using HTTPS, keeping your browser updated, and being mindful of suspicious websites.");
        keywordResponses.Add("social media", "Social media safety means managing your privacy settings, being aware of what you share, and recognizing social engineering tactics.");
    }

    private void InitializeRandomResponses()
    {
        // Random Responses: Multiple predefined responses for common cybersecurity queries
        randomResponses.Add("phishing tips", new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organizations.",
            "Always verify the sender's email address and look for suspicious links before clicking.",
            "If an email seems too good to be true, it probably is. Be wary of unsolicited offers.",
            "Never share your passwords or sensitive information in response to an email."
        });

        randomResponses.Add("malware prevention", new List<string>
        {
            "Keep your operating system and software updated to patch known vulnerabilities.",
            "Use reputable antivirus software and perform regular scans.",
            "Be careful about what you download, especially from unofficial app stores or websites.",
            "Avoid clicking on suspicious links or opening attachments from unknown senders."
        });

        randomResponses.Add("password tips", new List<string>
        {
            "A strong password should be at least 12 characters long and include a mix of uppercase, lowercase, numbers, and symbols.",
            "Consider using a passphrase – a sequence of random words – which is easier to remember but hard to guess.",
            "Never reuse passwords across different accounts. Use a password manager if remembering many unique passwords is difficult."
        });

        randomResponses.Add("privacy tips", new List<string>
        {
            "Regularly review privacy settings on all your online accounts and apps.",
            "Be mindful of the personal information you share online, even on social media.",
            "Consider using a VPN to encrypt your internet traffic and protect your online privacy, especially on public Wi-Fi."
        });
    }

    private void InitializeSentimentResponses()
    {
        // Sentiment Detection: Responses based on user sentiment
        sentimentResponses.Add("worried", new List<string>
        {
            "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.",
            "I understand your concern. Cybersecurity can be complex, but I'm here to help you navigate it.",
            "Don't worry, we can work through this together. What specifically is making you feel worried?"
        });

        sentimentResponses.Add("curious", new List<string>
        {
            "That's a great question! I'm happy to provide more information on that topic.",
            "Curiosity is key to learning about cybersecurity. What specifically are you curious about?",
            "Excellent! Let's explore that topic further."
        });

        sentimentResponses.Add("frustrated", new List<string>
        {
            "I can see why you might be frustrated. Cybersecurity issues can be challenging. How can I help clarify things for you?",
            "It's okay to feel frustrated. Let's break down the problem and find a solution together.",
            "I understand. Sometimes these topics can be overwhelming. What's causing you frustration?"
        });
        sentimentResponses.Add("confused", new List<string>
        {
            "I apologize if that was unclear. Could you tell me what part you're finding confusing?",
            "It's common to feel confused with new topics. Let me try to explain it differently.",
            "Let's clarify that. What specifically is unclear to you?"
        });
    }
    // --- End Initialization Methods ---

    // --- Chatbot Response Generation Logic ---
    public string GetChatbotResponse(string userInput)
    {
        userInput = userInput.ToLower().Trim(); // Normalize input

        // 5. Sentiment Detection (simple keyword-based)
        string sentimentResponse = DetectAndRespondToSentiment(userInput);
        if (!string.IsNullOrEmpty(sentimentResponse))
        {
            // If sentiment is detected, reset topic to avoid immediate follow-up on sentiment
            currentTopic = string.Empty;
            return sentimentResponse;
        }

        // 4. Memory and Recall (checking for user's name or favorite topic)
        string personalizedResponse = HandleMemoryAndRecall(userInput);
        if (!string.IsNullOrEmpty(personalizedResponse))
        {
            return personalizedResponse;
        }

        // 3. Conversation Flow (handling follow-up questions or confusion about current topic)
        string conversationFlowResponse = HandleConversationFlow(userInput);
        if (!string.IsNullOrEmpty(conversationFlowResponse))
        {
            return conversationFlowResponse;
        }

        // 1. Keyword Recognition
        foreach (var entry in keywordResponses)
        {
            if (userInput.Contains(entry.Key))
            {
                currentTopic = entry.Key; // Set current topic for potential follow-up questions
                return entry.Value;
            }
        }

        // 2. Random Responses (for specific topics like "phishing tips")
        foreach (var entry in randomResponses)
        {
            if (userInput.Contains(entry.Key))
            {
                currentTopic = entry.Key; // Set current topic for potential follow-up questions
                return GetRandomResponse(entry.Key);
            }
        }

        // Handle specific menu-like inputs if they don't match keywords
        if (userInput == "1" || userInput == "password security") return GetRandomResponse("password tips");
        if (userInput == "2" || userInput == "phishing threats") return GetRandomResponse("phishing tips");
        if (userInput == "3" || userInput == "secure browsing") return keywordResponses["browsing"];
        if (userInput == "4" || userInput == "social media safety") return keywordResponses["social media"];
        if (userInput == "5" || userInput == "about this hub") return DisplayAboutHubText(); // Return text from about method

        // 6. Error Handling and Edge Cases (default response for unknown inputs)
        currentTopic = string.Empty; // Reset current topic if input is not recognized
        return "I'm not sure I understand. Can you try rephrasing? You can also type 'menu' to see options or 'exit' to quit.";
    }

    private string GetRandomResponse(string topic)
    {
        if (randomResponses.ContainsKey(topic))
        {
            List<string> responses = randomResponses[topic];
            int index = random.Next(responses.Count);
            return responses[index];
        }
        return string.Empty; // Should not happen if called correctly
    }

    private string DetectAndRespondToSentiment(string userInput)
    {
        foreach (var sentiment in sentimentResponses)
        {
            if (userInput.Contains(sentiment.Key))
            {
                return GetRandomResponseFromList(sentiment.Value);
            }
        }
        return string.Empty;
    }

    private string GetRandomResponseFromList(List<string> responses)
    {
        if (responses != null && responses.Any())
        {
            return responses[random.Next(responses.Count)];
        }
        return string.Empty;
    }

    private string HandleMemoryAndRecall(string userInput)
    {
        // Storing user's name
        if ((userInput.Contains("my name is") || userInput.Contains("i am")) && !userData.ContainsKey("name"))
        {
            string name = "";
            if (userInput.Contains("my name is"))
            {
                name = userInput.Split(new[] { "my name is" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();
            }
            else if (userInput.Contains("i am"))
            {
                name = userInput.Split(new[] { "i am" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                // Capitalize the first letter of the name for better presentation
                name = char.ToUpper(name[0]) + name.Substring(1);
                userData["name"] = name;
                return $"Nice to meet you, {name}! How can I assist you with cybersecurity today?";
            }
        }

        // Recalling user's name
        if (userData.ContainsKey("name") && (userInput.Contains("who am i") || userInput.Contains("do you remember my name")))
        {
            return $"Yes, if I remember correctly, your name is {userData["name"]}.";
        }

        // Storing favorite cybersecurity topic
        if (userInput.Contains("my favorite cybersecurity topic is") && !userData.ContainsKey("favorite_topic"))
        {
            string topic = userInput.Split(new[] { "my favorite cybersecurity topic is" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault()?.Trim();
            if (!string.IsNullOrWhiteSpace(topic))
            {
                userData["favorite_topic"] = topic;
                return $"That's interesting! So your favorite topic is {topic}. Let's discuss {topic} then!";
            }
        }
        // Recalling favorite topic later in conversation
        if (userData.ContainsKey("favorite_topic") && (userInput.Contains("tell me more about my favorite topic") || userInput.Contains("what about my favorite topic")))
        {
            return $"As someone interested in {userData["favorite_topic"]}, you might want to review some advanced tips related to it.";
        }

        return string.Empty;
    }

    private string HandleConversationFlow(string userInput)
    {
        // Simple example for follow-up questions related to the current topic
        if (!string.IsNullOrEmpty(currentTopic))
        {
            if (userInput.Contains("tell me more") || userInput.Contains("can you elaborate") || userInput.Contains("what else"))
            {
                // Try to provide a more detailed response or another random response related to the current topic
                if (keywordResponses.ContainsKey(currentTopic))
                {
                    return $"Regarding {currentTopic}, it's crucial to understand: {keywordResponses[currentTopic]} What else would you like to know?";
                }
                else if (randomResponses.ContainsKey(currentTopic))
                {
                    return $"Continuing on {currentTopic}: {GetRandomResponse(currentTopic)}";
                }
            }
            else if (userInput.Contains("i'm confused") || userInput.Contains("i don't understand") || userInput.Contains("clarify"))
            {
                // This is also handled by sentiment detection, but can be a fallback
                return $"I apologize if that was unclear. Could you tell me what part about {currentTopic} you're finding confusing?";
            }
        }
        return string.Empty;
    }
    // --- End Chatbot Response Generation Logic ---


    // --- Existing Console UI Methods (Modified for integration) ---
    static void Main(string[] args)
    {
        Console.Title = "Cyber Awareness Hub";
        DisplayCyberShieldArt();
        PlayWelcomeAudio();
        ColorTypeWrite("Initializing Cyber Awareness Hub...\n", ConsoleColor.Cyan);
        Thread.Sleep(1000);

        // Instantiate the chatbot
        CyberAwarenessChatbot chatbot = new CyberAwarenessChatbot();

        // Get user name and store it in chatbot's memory
        string userName = chatbot.GetUserNameAndStore();

        chatbot.RunChatLoop(userName);
    }

    static void DisplayCyberShieldArt()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(@"
  _____       _             _____                   
 / ____|     | |           / ____|                  
| |   _   _| |__   ___ _ __| (___   ___ __ _ _ __  _ __   ___ _ __  
| |  | | | | '_ \ / _ \ '__|\___ \ / __/ _` | '_ \| '_ \ / _ \ '__| 
| |___| |_| | |_) |  __/ |  ____) | (_| (_| | | | | | | |  __/ |    
 \_____\__,_|_.__/ \___|_| |_____/ \___\__,_|_| |_|_| |_|\___|_|    
                                                                     
");
        Console.ResetColor();
    }

    static void PlayWelcomeAudio()
    {
        try
        {
            // Ensure this path is correct relative to your executable or use an absolute path
            // For example, if 'Audio images' is a folder next to your .exe, this is fine.
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio images", "greetings.wav");
            if (File.Exists(filePath))
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.Play();
            }
            else
            {
                Console.WriteLine("[Audio greeting file not found at: " + filePath + "]");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("[Audio greeting not found - check file path]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error playing audio]: {ex.Message}");
        }
    }

    static void ColorTypeWrite(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(20); // Adjust for typing speed
        }
        Console.ResetColor();
    }

    // Modified GetUserName to store in chatbot's memory
    private string GetUserNameAndStore()
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

        // Store the name in the chatbot's memory
        userData["name"] = char.ToUpper(userName[0]) + userName.Substring(1); // Capitalize for storage

        Console.WriteLine();
        ColorTypeWrite($"🛡️  Welcome, {userData["name"]}! I'm your Cyber Awareness Assistant.\n", ConsoleColor.Green);
        ColorTypeWrite("I'm here to help you stay safe in the digital world.\n\n", ConsoleColor.Cyan);
        Thread.Sleep(800);

        return userData["name"];
    }

    private void RunChatLoop(string userName)
    {
        bool continueChat = true;
        ColorTypeWrite("Type 'menu' to see topics, or ask me anything about cybersecurity!\n", ConsoleColor.Magenta);
        while (continueChat)
        {
            Console.Write($"\n{userName}: "); // Prompt with user's name
            string userInput = Console.ReadLine()?.Trim() ?? "";

            if (userInput.ToLower() == "exit")
            {
                ColorTypeWrite("Are you sure you want to exit? (yes/no): ", ConsoleColor.Yellow);
                string confirm = Console.ReadLine()?.Trim().ToLower();
                if (confirm == "yes" || confirm == "y")
                {
                    continueChat = false;
                    DisplayFarewell(userName);
                }
                else
                {
                    ColorTypeWrite("Chatbot: Okay, let's continue!", ConsoleColor.Green);
                }
            }
            else if (userInput.ToLower() == "menu")
            {
                DisplayMainMenu();
            }
            else
            {
                string chatbotResponse = GetChatbotResponse(userInput);
                ColorTypeWrite("Chatbot: " + chatbotResponse + "\n", ConsoleColor.Green);
            }
        }
    }

    static void DisplayMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n--- CYBER AWARENESS TOPICS ---");
        Console.WriteLine("• Password Security (e.g., 'tell me about passwords')");
        Console.WriteLine("• Phishing Threats (e.g., 'what is phishing?')");
        Console.WriteLine("• Secure Browsing (e.g., 'how to browse safely?')");
        Console.WriteLine("• Social Media Safety (e.g., 'social media privacy')");
        Console.WriteLine("• Malware Prevention (e.g., 'how to prevent malware?')");
        Console.WriteLine("• Privacy (e.g., 'tell me about online privacy')");
        Console.WriteLine("• About This Hub (e.g., 'about')");
        Console.WriteLine("------------------------------");
        Console.WriteLine("(You can also express sentiments like 'I'm worried' or ask follow-up questions like 'tell me more')");
        Console.WriteLine("(Type 'exit' to quit)");
        Console.ResetColor();
    }

    // This method is now used internally by GetChatbotResponse when "about" is asked
    private string DisplayAboutHubText()
    {
        return "Cyber Awareness Hub v2.0\n\nMission: Empower users with essential cybersecurity knowledge\nDeveloped for educational purposes\n© 2025 Cyber Security Education Initiative\n\nFeatures:\n• Interactive cyber safety guidance\n• Up-to-date threat awareness\n• Practical protection strategies";
    }

    static void DisplayFarewell(string userName)
    {
        ColorTypeWrite($"\n👋 Goodbye, {userName}!\n", ConsoleColor.Cyan);
        ColorTypeWrite("Remember to practice what you've learned today.\n", ConsoleColor.White);
        ColorTypeWrite("Your cybersecurity is in your hands!\n\n", ConsoleColor.White);
        Thread.Sleep(2000);
    }
}
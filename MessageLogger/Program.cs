// See https://aka.ms/new-console-template for more information
using MessageLogger.Models;
using MessageLogger.Data;
using Microsoft.EntityFrameworkCore;

using (var context = new MessageLoggerContext())
{
    User user = null;
    string userInput = "";
    if (!context.Users.Any())
    {
        user = NewUser(context);
        userInput = Console.ReadLine();
    }
    else
    {
        userInput = "log out";
    }

    while (userInput.ToLower() != "quit")
    {
        while (userInput.ToLower() != "log out")
        {

            if (userInput.ToLower() == "edit" || userInput.ToLower() == "delete")
            {
                if (userInput.ToLower() == "edit")
                {
                    EditMessage(user, context);
                }
                else
                {
                    DeleteMessage(user, context);
                }
            }
            else
            {
                user.Messages.Add(new Message(userInput));
                context.SaveChanges();
            }
            foreach (var message in user.Messages)
            {
                Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
            }

            Console.Write("Add a message: ");

            userInput = Console.ReadLine();
            Console.WriteLine();
        }

        Console.Write("Would you like to log in a `new` or `existing` user?, `quit' to exit the program, or 'query' to see the query options ");
        userInput = Console.ReadLine();
        if (userInput.ToLower() == "new")
        {
            user = NewUser(context);

            userInput = Console.ReadLine();

        }
        else if (userInput.ToLower() == "existing")
        {
            Console.Write("What is your username? ");
            string username = Console.ReadLine();
            user = ExistingUser(context, username);

            if (user != null)
            {
                Console.WriteLine("To edit a message enter 'edit' or to delete amessage enter 'delete'");
                Console.Write("Add a message: ");
                userInput = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("could not find user");
                userInput = "quit";

            }
        }
        else if (userInput.ToLower() == "query")
        {
            Console.WriteLine(string.Join("\n",MessageStatistics(context)));
        }

    }

    Console.WriteLine("Thanks for using Message Logger!");
    foreach (var u in context.Users.Include(u => u.Messages))
    {
        Console.WriteLine($"{u.Name} wrote {u.Messages.Count} messages.");
    }
}

User NewUser(MessageLoggerContext context)
{

    Console.WriteLine("Welcome to Message Logger!");
    Console.WriteLine();
    Console.WriteLine("Let's create a user pofile for you.");
    Console.Write("What is your name? ");
    string name = Console.ReadLine();
    Console.Write("What is your username? (one word, no spaces!) ");
    string username = Console.ReadLine();
    User user = new User(name, username);

    context.Users.Add(user);
    context.SaveChanges();

    Console.WriteLine();
    Console.WriteLine("To log out of your user profile, enter `log out`.");

    Console.WriteLine("To edit a message enter 'edit' or to delete amessage enter 'delete'");
    Console.Write("Add a message (or `quit` to exit): ");
    return user;
}
User ExistingUser(MessageLoggerContext context, string username)
{
    
    User user = null;
    foreach (var existingUser in context.Users.Include(user => user.Messages))
    {
        if (existingUser.Username == username)
        {
            user = existingUser;
        }
    }
    return user;
}


List<string> MessageStatistics(MessageLoggerContext context)
{
    var returnList = new List<string>();
    Console.WriteLine("(1) all users ordered by how many messages they've created" +
        "\n(2) the 10 most common words used by all users or a specific user" +
        "\n(3) what hour had the most messages made" +
        "\nenter the number of the summary you would like to see");
    var userInput = Console.ReadLine();
    if (userInput == "1")
    {
        returnList = UsersByNumberOfMessages(context);
    }
    else if (userInput == "2")
    {
        returnList = MostCommonWords(context);
    }
    else if (userInput == "3")
    {
        returnList = BusiestHour(context);
    }
    return returnList;
}

List<string> UsersByNumberOfMessages(MessageLoggerContext context)
{
    var ListOfUsers = context.Users.Include(user => user.Messages);
    var orderedListOfUsers = ListOfUsers.OrderByDescending(user => user.Messages.Count());
    var orderedListOfNames = orderedListOfUsers.Select(user => user.Name).ToList();
    return orderedListOfNames;
}
List<string> MostCommonWords(MessageLoggerContext context)
{
    var returnList = new List<string>();
    var wordList = new List<string>();
    Console.WriteLine("enter the username of the account you'd like to see their top 10 words" +
        "\nenter anything else to see the top 10 words of all users");
    string userInput = Console.ReadLine();
    User user = ExistingUser(context, userInput);
    if (user != null)
    {
        foreach (Message message in user.Messages)
        {
            wordList.AddRange((message.Content.ToLower()).Split(" "));
        }
    }
    else
    {
        var listOfMessages = new List<Message>();
        foreach (User mUser in context.Users.Include(user => user.Messages))
        {
            listOfMessages.AddRange(mUser.Messages);
        }
        foreach (Message message in listOfMessages)
        {
            wordList.AddRange((message.Content.ToLower()).Split(" "));
        }
    }
    returnList = TenMostFrequent(wordList);
    return returnList;
}

List<string> TenMostFrequent(List<string> wordList)
{
    var returnList = new List<string>();
    var countedWords = new Dictionary<string, int>();
    foreach (string word in wordList)
    {
        if (countedWords.ContainsKey(word))
        {
            countedWords[word] += 1;
        }
        else
        {
            countedWords.Add(word, 1);
        }
    }
    var orderedCountedWords = countedWords.OrderByDescending(d => d.Value);
    foreach (KeyValuePair<string, int> word in orderedCountedWords)
    {
        if (returnList.Count() >= 10) break;
        string w = "";
        w += word.Key;
        w += " : ";
        w += word.Value.ToString();
        returnList.Add(w);
    }
    return returnList;
}
List<string> BusiestHour(MessageLoggerContext context)
{
    var returnList = new List<string>();
    var allMessages = new List<Message>();
    foreach(User user in context.Users.Include(u => u.Messages))
    {
        allMessages.AddRange(user.Messages);
    }
    var countedHours = new Dictionary<DateTime, int>();
    foreach(Message message in allMessages)
    {
        DateTime hour = new DateTime(message.CreatedAt.Year,
            message.CreatedAt.Month, message.CreatedAt.Day, message.CreatedAt.Hour, 0, 0);
        if (countedHours.ContainsKey(hour))
        {
            countedHours[hour] += 1;
        }
        else
        {
            countedHours.Add(hour, 1);
        }
    }
    var orderedCountedHours = countedHours.OrderByDescending(d => d.Value);
    foreach(KeyValuePair<DateTime, int> hour in orderedCountedHours)
    {
        string h = "";
        if (hour.Key.Hour > 12)
        {
            h = (hour.Key.Hour - 12).ToString() + " PM";
        }
        else if (hour.Key.Hour == 12)
        {
            h = (hour.Key.Hour).ToString() + " PM";
        }
        else if (hour.Key.Hour > 0)
        {
            h = (hour.Key.Hour).ToString() + " AM";
        }
        else
        {
            h = "12 AM";
        }
        returnList.Add($"the hour with the most posts is {h} on {hour.Key.Month}/{hour.Key.Day}/{hour.Key.Year}");
        break;
    }
    return returnList;
}

void EditMessage(User user, MessageLoggerContext context)
{
    int i = 1;
    foreach(Message message in user.Messages)
    {
        Console.WriteLine($"({i}) {message.Content}");
        i++;
    }
    Console.WriteLine("Enter the number of the message you'd like to edit");
    var userInput = Console.ReadLine();
    var inputId = Convert.ToInt32(userInput) - 1;
    var chosenMessage = user.Messages[inputId];
    Console.WriteLine(chosenMessage.Content);
    Console.WriteLine("What would you like it to say instead");
    var newContent = Console.ReadLine();
    chosenMessage.Content = newContent;
    context.SaveChanges();
    Console.WriteLine("Message edited successfully");
}
void DeleteMessage(User user, MessageLoggerContext context)
{
    int i = 1;
    foreach (Message message in user.Messages)
    {
        Console.WriteLine($"({i}) {message.Content}");
        i++;
    }
    Console.WriteLine("Enter the number of the message you'd like to delete");
    var userInput = Console.ReadLine();
    var inputId = Convert.ToInt32(userInput) - 1;
    var chosenMessage = user.Messages[inputId];
    user.Messages.Remove(chosenMessage);
    context.Messages.Remove(chosenMessage);
    context.SaveChanges();
    Console.WriteLine("Message deleted successfully");

}
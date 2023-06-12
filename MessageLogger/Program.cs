﻿// See https://aka.ms/new-console-template for more information
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

            user.Messages.Add(new Message(userInput));
            context.SaveChanges();

            foreach (var message in user.Messages)
            {
                Console.WriteLine($"{user.Name} {message.CreatedAt:t}: {message.Content}");
            }

            Console.Write("Add a message: ");

            userInput = Console.ReadLine();
            Console.WriteLine();
        }

        Console.Write("Would you like to log in a `new` or `existing` user? Or, `quit`? ");
        userInput = Console.ReadLine();
        if (userInput.ToLower() == "new")
        {
            user = NewUser(context);

            userInput = Console.ReadLine();

        }
        else if (userInput.ToLower() == "existing")
        {
            user = ExistingUser(context);

            if (user != null)
            {
                Console.Write("Add a message: ");
                userInput = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("could not find user");
                userInput = "quit";

            }
        }

    }

    Console.WriteLine("Thanks for using Message Logger!");
    foreach (var u in context.Users)
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

    Console.WriteLine();
    Console.Write("Add a message (or `quit` to exit): ");
    return user;
}
User ExistingUser(MessageLoggerContext context)
{
    Console.Write("What is your username? ");
    string username = Console.ReadLine();
    User user = null;
    foreach (var existingUser in context.Users)
    {
        if (existingUser.Username == username)
        {
            user = existingUser;
        }
    }
    return user;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Models;


namespace MessageLogger.Data
{
    public class DataSeeder
    {
        public static void SeedUsersAndMessages(MessageLoggerContext context)
        {
            if (!context.Users.Any())
            {
                var swipa = new User("Swipa", "SwipaCam");

                var swipasMessages = new List<Message>()
                {
                    new Message("303!! THE DENVER NUGGETS ARE THR NBA CHAMPS!!!!"),
                    new Message("Are the Denver Nuggets the only team in NBA hisory to win an NBA Championship without an NBA Top 75 Player?"),
                    new Message("Currently listening to Doc Rivers (former 76ers Head Coach) call Nikola Jokic the BEST PLAYER IN THE NBA"),
                    new Message("I am so proud of Jamal Murray. He deserves all of his flowers.\n1 of 4 players to ever average 20 PTS and 10 AST in an NBA Finals series" +
                    "\nMagic Johnson\nMichael Jordan\nLeBron James\nJamal Murray")
                };
                swipa.Messages.AddRange(swipasMessages);

                var harrison = new User("Harrison Wind", "HarrisonWind");

                var harrisonsMessages = new List<Message>()
                {
                    new Message("Jamal Murray's been parading around the Nuggets locker room with the Larry-OB for the last 10 minutes. He's not giving it up."),
                    new Message("We Don't Skip Steps."),
                    new Message("Welcome to the Golden Era"),
                    new Message("Nuggets' championship afterparty lasted well into this morning. The location? Aaron Gordon's warehouse."),
                    new Message("We started the season in Sombor, and we ended it watching the Nuggets win the championship.")
                };
                harrison.Messages.AddRange(harrisonsMessages);

                var ryan = new User("Ryan Blackburn", "NBABlackburn");

                var ryansMessages = new List<Message>()
                {
                    new Message("Currently grinning ear to ear, listening to The Bill Simmons Pod with Doc Rivers calling Nikola Jokic, the best player in the league."),
                    new Message("Players in NBA playoff history to average 26 points, 7 assists on at least 58% True Shooting in 15+ games:" +
                    "\nLebron James (x4)\nMichael Jordan (x2)\nJames Harden\n...and now both Jamal Murray and Nikola Jokic this playoff run.\nGreatness."),
                    new Message("Really Cool. Denver did the thing ,and they did it together.")
                };

                ryan.Messages.AddRange(ryansMessages);

                var users = new List<User>() { swipa, harrison, ryan };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}

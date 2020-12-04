using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace RockPaperScissors
{
    class RPS
    {

        private int player1Score;
        private int player2Score;
        private int matchCounter = 0;
        private IDictionary<string, int> choiceCount = new Dictionary<string, int>();

        public bool isValidChoice(string choice)
        {

            if (choice.Equals("rock") || choice.Equals("paper") || choice.Equals("scissors"))
            {
                return true;
            } else
            {
                return false;
            }

        }

        public void fight(string choice1, string choice2, string play1Name, string opponentsName)
        {
            choiceCount[choice1]++;
            choiceCount[choice2]++;

            if (choice1 == choice2)
            {
                Console.WriteLine("A tie! No points awarded\n");
            } else if ((choice1 == "rock" && choice2 == "scissors") || (choice1 == "paper" && choice2 == "rock") || (choice1 == "scissors" && choice2 == "paper"))
            {
                player1Score++;
                if (player1Score == 3)
                {
                    Console.WriteLine(play1Name + " won the game!\n");
                    Console.WriteLine("FINAL SCORE: " + play1Name + " " + player1Score + " - " + player2Score + " " + opponentsName + "\n");
                    Console.WriteLine("Total Rounds: " + matchCounter + ", Choice count; Rock: " + choiceCount["rock"] + ", Paper: " + choiceCount["paper"] + ", Scissors: " + choiceCount["scissors"] + "\n");
                } else
                {
                    Console.WriteLine(play1Name + " won this round!\n");
                    Console.WriteLine("SCORE: " + play1Name + " " + player1Score + " - " + player2Score + " " + opponentsName + "\n");
                }
            } else if ((choice2 == "rock" && choice1 == "scissors") || (choice2 == "paper" && choice1 == "rock") || (choice2 == "scissors" && choice1 == "paper"))
            {
                player2Score++;
                if (player2Score == 3)
                {
                    Console.WriteLine(opponentsName + " won the game!\n");
                    Console.WriteLine("FINAL SCORE: " + play1Name + " " + player1Score + " - " + player2Score + " " + opponentsName + "\n");
                    Console.WriteLine("Total Rounds: " + matchCounter + ", Choice count; Rock: " + choiceCount["rock"] + ", Paper: " + choiceCount["paper"] + ", Scissors: " + choiceCount["scissors"] + "\n");
                }
                else
                {
                    Console.WriteLine(opponentsName + " won this round!\n");
                    Console.WriteLine("SCORE: " + play1Name + " " + player1Score + " - " + player2Score + " " + opponentsName + "\n");
                }
            } else
            {
                Console.WriteLine("Error");
            }
        }

        static async Task Main(string[] args)
        {
            RPS rps = new RPS();

            string player1Name = "";
            string player2Name = "";
            string player1Choice = "";

            string opponentOption = "";

            string playAgain = "yes";

            Dictionary<string, string> postDict = new Dictionary<string, string>();

            var urlPlayer = "http://localhost/RPS/index.php?id=playersave";
            var urlMatch = "http://localhost/RPS/index.php?id=matchsave";

            using var client = new HttpClient();

            FormUrlEncodedContent content;
            HttpResponseMessage response;
            string responseString;

            while (playAgain == "yes")
            {

            rps.choiceCount.Add("rock", 0);
            rps.choiceCount.Add("paper", 0);
            rps.choiceCount.Add("scissors", 0);

            Console.WriteLine("Please enter your name:\n");
            player1Name = Console.ReadLine();
            postDict.Add("player1", player1Name);
            Console.WriteLine("Welcome " + player1Name + " to Rock Paper Scissors!\n");
            Console.WriteLine("Please enter an option; 1 (to play against another player), 2 (to play me... if you dare...)\n");
            opponentOption = Console.ReadLine().Trim();

                if (opponentOption == "1")
                {
                    string player2Choice = "";

                    Console.WriteLine("\nWise choice " + player1Name + "\n");
                    Console.WriteLine("Please enter your opponents name:\n");
                    player2Name = Console.ReadLine();

                    while (postDict.ContainsValue(player2Name)) {

                        Console.WriteLine("That name has already been taken!\n");
                        Console.WriteLine("Please enter your opponents name:\n");
                        player2Name = Console.ReadLine();
                    }

                    postDict.Add("player2", player2Name);

                    content = new FormUrlEncodedContent(postDict);
                    response = await client.PostAsync(urlPlayer, content);
                    responseString = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("\nWelcome " + player2Name + " to a best of 3 match of Rock Paper Scissors against " + player1Name + "\n");

                    while (rps.player1Score < 3 && rps.player2Score < 3)
                    {
                        rps.matchCounter++;
                        Console.WriteLine("Match: " + rps.matchCounter + "\n");
                        Console.WriteLine(player1Name + ", make your choice:\n");
                        player1Choice = Console.ReadLine().ToLower().Trim();
                        
                        while (!rps.isValidChoice(player1Choice))
                        {
                            Console.WriteLine("Invalid choice! " + player1Name + ", make your choice:\n");
                            player1Choice = Console.ReadLine().ToLower().Trim();
                        }

                        Console.WriteLine(player2Name + ", make your choice:\n");
                        player2Choice = Console.ReadLine().ToLower().Trim();
                        
                        while (!rps.isValidChoice(player2Choice))
                        {
                            Console.WriteLine("Invalid choice! " + player2Name + ", make your choice:\n");
                            player2Choice = Console.ReadLine().ToLower().Trim();
                        }
                        rps.fight(player1Choice, player2Choice, player1Name, player2Name);
                    }

                    Console.WriteLine("Would you like to play again? (yes/no)\n");
                    playAgain = Console.ReadLine();

                    while (!playAgain.Equals("yes") && !playAgain.Equals("no"))
                    {
                        Console.WriteLine("That isn't an option! Please enter 'yes' or 'no'\n");
                        playAgain = Console.ReadLine();
                    }

                }
                else if (opponentOption == "2")
                {

                    Console.WriteLine("\nYou fool! I will destroy you!\n");
                    Console.WriteLine("This is a best of 3 match of Rock Paper Scissors, you against me\n");

                    player2Name = "Computer";

                    content = new FormUrlEncodedContent(postDict);
                    response = await client.PostAsync(urlPlayer, content);
                    responseString = await response.Content.ReadAsStringAsync();

                    while (rps.player1Score < 3 && rps.player2Score < 3)
                    {
                        rps.matchCounter++;
                        Console.WriteLine("Match: " + rps.matchCounter + "\n");
                        Console.WriteLine(player1Name + ", make your choice:\n");
                        player1Choice = Console.ReadLine().ToLower().Trim();
                        
                        while (!rps.isValidChoice(player1Choice))
                        {
                            Console.WriteLine("Invalid choice! " + player1Name + ", make your choice:\n");
                            player1Choice = Console.ReadLine().ToLower().Trim();
                        }

                        Random rdm = new Random();
                        var random = rdm.Next(0, 3);
                        string[] computerChoices = { "rock", "paper", "scissors" };
                        string computerChoice = computerChoices[random];

                        rps.fight(player1Choice, computerChoice, player1Name, player2Name);
                    }

                    if (rps.player1Score == 3)
                    {
                        Console.WriteLine("NOOOO.... I will have my revenge!\n");
                    }
                    else
                    {
                        Console.WriteLine("HA! Too easy.\n");
                    }

                    Console.WriteLine("Would you like to play again? (yes/no)\n");
                    playAgain = Console.ReadLine();

                    while (!playAgain.Equals("yes") && !playAgain.Equals("no"))
                    {
                        Console.WriteLine("That isn't an option! Please enter 'yes' or 'no'\n");
                        playAgain = Console.ReadLine();
                    }

                }
                else
                {

                    Console.WriteLine("That wasn't an option!\n");
                    
                }

                if (opponentOption == "1" || opponentOption == "2")
                {

                    postDict.Clear();
                    postDict.Add("player1", player1Name);
                    postDict.Add("player2", player2Name);
                    postDict.Add("player1Score", rps.player1Score.ToString());
                    postDict.Add("player2Score", rps.player2Score.ToString());
                    postDict.Add("mostCommonChoice", "Rock: " + rps.choiceCount["rock"] + ", Paper: " + rps.choiceCount["paper"] + ", Scissors: " + rps.choiceCount["scissors"]);

                    content = new FormUrlEncodedContent(postDict);
                    response = await client.PostAsync(urlMatch, content);
                    responseString = await response.Content.ReadAsStringAsync();
                }

                rps.matchCounter = 0;
                rps.player1Score = 0;
                rps.player2Score = 0;
                rps.choiceCount.Clear();
                postDict.Clear();
            }

            Console.WriteLine("Thank you for playing. Goodbye.\n");
        }
    }
}

using System;

namespace MonteCarloConnectFour
{
    class Program
    {
        static MonteCarloTreeSearch mcts = new MonteCarloTreeSearch();
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Play against AI? (y/n)");
                String action = Console.ReadLine();

                if (action.ToLower() == "y")
                    PlayMatchesAgainstAI();
                else if (action.ToLower() == "n")
                    PlayMatchesBetweenAI();
                else if (action.ToLower() == "s")
                    PlayMatchesBetweenHumans();
                Console.ReadLine();
            }
        }

        static void PlayMatchesBetweenAI()
        {
            Game game = new Game();
            Console.WriteLine(game.GameState);

            while (!game.GameState.Done)
            {
                game = mcts.FindNextMove(game);
            }
            game.GameState.PrintStatus();
        }

        static void PlayMatchesAgainstAI()
        {
            Console.WriteLine("Do you want to go first (y/n)?");
            String action = Console.ReadLine();

            Game game = new Game();
            Console.WriteLine(game.GameState);

            if (action.ToLower() == "y")
                game = HumanAction(game);
            while (!game.GameState.Done)
            {
                game = mcts.FindNextMove(game);
                if (game.GameState.Done) 
                    break;
                game = HumanAction(game);
            }
            game.GameState.PrintStatus();
        }

        static void PlayMatchesBetweenHumans()
        {
            Game game = new Game();
            Console.WriteLine(game.GameState);
            while (!game.GameState.Done)
            {
                game = HumanAction(game);
            }
            game.GameState.PrintStatus();
        }

        static Game HumanAction(Game game)
        {
            Boolean enableAI = false;
            int actionIndex = 0;
            while (true)
            {
                Console.WriteLine("Enter your chosen action (type 'help' to find index): ");
                String action = Console.ReadLine();
                if (action.ToLower() == "ai")
                {
                    enableAI = true;
                    break;
                }
                else if (action.ToLower() == "help")
                {
                    GameState.DisplayHelp();
                }
                else if (Int32.TryParse(action, out actionIndex))
                {
                    if (game.GameState.AllowedActions.Contains(actionIndex))
                        break;
                }
            }
            if (enableAI)
            {
                game = mcts.FindNextMove(game);
            }
            else
            {
                game.Step(actionIndex);
                Console.WriteLine(game.GameState);
            }
            return game;
        }

    }
}

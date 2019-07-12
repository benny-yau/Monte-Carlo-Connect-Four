using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonteCarloConnectFour
{
    public class MonteCarloTreeSearch
    {
        private const int WIN_SCORE = 1;
        private const int iterations = 10000;

        public Game FindNextMove(Game game)
        {
            Game winGame = CheckWinMove(game);
            if (winGame != null)
            {
                Console.WriteLine(winGame.GameState);
                return winGame;
            }
            Console.WriteLine("Running mcts...");
            Tree tree = new Tree();
            Node rootNode = tree.Root;
            rootNode.State.Game = game;
            int count = 0;
            while (count < iterations)
            {
                count++;
                // Phase 1 - Selection
                Node promisingNode = SelectPromisingNode(rootNode);

                // Phase 2 - Expansion
                if (promisingNode.State.Game.GameState.Done == false && (promisingNode == tree.Root || promisingNode.State.VisitCount >= 10))
                {
                    ExpandNode(promisingNode);
                }
                
                // Phase 3 - Simulation
                Node nodeToExplore = promisingNode;
                if (promisingNode.ChildArray.Count() > 0)
                {
                    Node answerNode = promisingNode.ChildArray.Where(m => m.State.Game.GameState.Done).FirstOrDefault();
                    if (answerNode != null)
                        nodeToExplore = answerNode;
                    else
                        nodeToExplore = promisingNode.RandomChildNode;
                }
                int playoutResult = SimulateRandomPlayout(nodeToExplore, rootNode);

                // Phase 4 - Update
                BackPropagation(nodeToExplore, playoutResult);

                if (count % (iterations * 0.2) == 0)
                    Console.WriteLine("Iterations : " + count + " out of " + iterations);
            }

            Node winnerNode = rootNode.ChildWithMaxScore;
            List<Node> resultNodes = rootNode.ChildArray.OrderByDescending(m => UCT.UCT_Value(m)).ToList();
            for (int i = 0; i <= resultNodes.Count - 1; i++)
            {
                Console.WriteLine(resultNodes[i]);
                Debug.WriteLine(resultNodes[i]);
            }
            Console.WriteLine(winnerNode.State.Game.GameState);
            Debug.WriteLine(winnerNode.State.Game.GameState);
            Debug.WriteLine("Last moves: " + winnerNode.State.Game);
            return winnerNode.State.Game;
        }

        private Game CheckWinMove(Game game)
        {
            foreach (int p in game.GameState.AllowedActions)
            {
                Game newGame = new Game(game);
                newGame.Step(p);
                if (newGame.GameState.Done)
                {
                    game.Step(p);
                    return game;
                }
            }

            foreach (int p in game.GameState.AllowedActions)
            {
                Game newGame = new Game(game);
                newGame.GameState.PlayerTurn = -newGame.GameState.PlayerTurn;
                newGame.Step(p);
                if (newGame.GameState.Done)
                {
                    game.Step(p);
                    return game;
                }
            }
            return null;
        }

        private Node SelectPromisingNode(Node rootNode)
        {
            Node node = rootNode;
            while (node.ChildArray.Count() != 0)
            {
                node = UCT.FindBestNodeWithUCT(node);
            }
            return node;
        }

        private void ExpandNode(Node node)
        {
            Game game = node.State.Game;
            foreach (int p in game.GameState.AllowedActions)
            {
                Game newGame = new Game(game);
                newGame.Step(p);
                State newState = new State(newGame);
                Node newNode = new Node(newState);
                newNode.Parent = node;
                node.ChildArray.Add(newNode);
            }
        }

        private void BackPropagation(Node nodeToExplore, int playerNo)
        {
            Node tempNode = nodeToExplore;
            while (tempNode != null)
            {
                tempNode.State.IncrementVisit();
                if (tempNode.State.Game.GameState.PlayerTurn == -playerNo)
                {
                    tempNode.State.AddScore(WIN_SCORE);
                }
                tempNode = tempNode.Parent;
            }
        }

        private int SimulateRandomPlayout(Node nodeToExplore, Node rootNode)
        {
            State tempState = new State(nodeToExplore.State);
            GameState gameState = tempState.Game.GameState;

            if (gameState.Done)
            {
                int opponent = -rootNode.State.Game.GameState.PlayerTurn;
                if (gameState.Value == opponent)
                    nodeToExplore.Parent.State.WinScore = int.MinValue;
                return gameState.Value;
            }

            while (!gameState.Done)
            {
                gameState = tempState.RandomPlay();
            }
            return gameState.Value;
        }

    }
}

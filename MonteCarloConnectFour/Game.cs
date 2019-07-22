using System;
using System.Collections.Generic;
using System.Linq;

namespace MonteCarloConnectFour
{
    public class Game
    {
        public static int SizeX = 7;
        public static int SizeY = 6;
        private GameState gameState;
        private List<int> lastMoves;

        public GameState GameState
        {
            get
            {
                return gameState;
            }
        }

        public List<int> LastMoves
        {
            get
            {
                if (lastMoves == null)
                    lastMoves = new List<int>();
                return lastMoves;
            }
            set
            {
                lastMoves = value;
            }
        }

        public Game()
        {
            int[] board = Enumerable.Repeat(0, SizeX * SizeY).ToArray();
            gameState = new GameState(board, 1);
        }

        public Game(Game game)
        {
            gameState = new GameState(game.GameState.Board, game.GameState.PlayerTurn);
            gameState.Done = game.GameState.Done;
            gameState.Value = game.gameState.Value;
            this.LastMoves.AddRange(game.LastMoves);
        }

        public GameState Reset()
        {
            int[] board = Enumerable.Repeat(0, SizeX * SizeY).ToArray();
            this.gameState = new GameState(board, 1);
            this.LastMoves = null;
            return this.gameState;
        }

        public void Step(int action)
        {
            gameState.TakeAction(action);
            this.LastMoves.Add(action);
        }

        public override string ToString()
        {
            String msg = "";
            for (int i = 0; i <= this.LastMoves.Count - 1; i++)
            {
                int p = this.LastMoves[i];
                msg += p.ToString();
                if (i < this.LastMoves.Count - 1)
                    msg += ", ";
            }
            return msg;
        }

    }
}
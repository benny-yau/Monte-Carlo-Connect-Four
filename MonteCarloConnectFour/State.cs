using System.Collections.Generic;

namespace MonteCarloConnectFour
{
    public class State
    {
        private Game game;
        private int visitCount;
        private double winScore;

        public State()
        {
        }

        public State(Game game)
        {
            this.game = new Game(game);
        }

        public State(State state)
        {
            this.game = new Game(state.Game);
            this.visitCount = state.VisitCount;
            this.winScore = state.WinScore;
        }

        public Game Game
        {
            get
            {
                return game;
            }
            set
            {
                this.game = value;
            }
        }

        public int VisitCount
        {
            get
            {
                return visitCount;
            }
            set
            {
                this.visitCount = value;
            }
        }


        public double WinScore
        {
            get
            {
                return winScore;
            }
            set
            {
                this.winScore = value;
            }
        }

        public void IncrementVisit()
        {
            this.visitCount++;
        }

        public void AddScore(double score)
        {
            if (this.winScore != int.MinValue)
            {
                this.winScore += score;
            }
        }

        public GameState RandomPlay()
        {
            List<int> availablePositions = this.game.GameState.AllowedActions;
            if (availablePositions.Count == 0) return this.game.GameState;

            for (int i = 0; i <= availablePositions.Count - 1; i++)
            {
                Game verifyGame = new Game(this.game);
                verifyGame.Step(availablePositions[i]);
                if (verifyGame.GameState.Done)
                    return verifyGame.GameState;
            }

            int selectRandom = GlobalRandom.NextInt(availablePositions.Count);
            this.game.Step(availablePositions[selectRandom]);
            return this.game.GameState;
        }

    }
}

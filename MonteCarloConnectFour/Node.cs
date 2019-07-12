using System;
using System.Collections.Generic;

namespace MonteCarloConnectFour
{

    public class Node
    {
        private State state;
        private Node parent;
        private List<Node> childArray;

        public Node()
		{
			this.state = new State();
            childArray = new List<Node>();
		}

        public Node(State state)
		{
			this.state = state;
            childArray = new List<Node>();
		}

        public State State
        {
            get
            {
                return state;
            }
            set
            {
                this.state = value;
            }
        }

        public Node Parent
        {
            get
            {
                return parent;
            }
            set
            {
                this.parent = value;
            }
        }

        public List<Node> ChildArray
        {
            get
            {
                return childArray;
            }
            set
            {
                this.childArray = value;
            }
        }

        public Node RandomChildNode
        {
            get
            {
                int noOfPossibleMoves = this.childArray.Count;
                int selectRandom = GlobalRandom.NextInt(noOfPossibleMoves);
                return this.childArray[selectRandom];
            }
        }

        public Node ChildWithMaxScore //either choose node with highest UCT or visit count
        {
            get
            {
                //Node node = this.childArray.MaxObject(m => m.State.VisitCount);
                Node node = UCT.FindBestNodeWithUCT(this);
                return node;
            }
        }

        public override String ToString()
        {
            List<int> lastMoves = this.State.Game.LastMoves;
            String rc = "Move: " + ((lastMoves.Count == 0) ? "-" : lastMoves[lastMoves.Count - 1].ToString()) + " | ";
            rc += "Score: " + this.State.WinScore + " | ";
            rc += "VisitCount: " + this.State.VisitCount + " | ";
            rc += "UCT:" + UCT.UCT_Value(this).ToString("0.000");

            return rc;
        }
    }
}

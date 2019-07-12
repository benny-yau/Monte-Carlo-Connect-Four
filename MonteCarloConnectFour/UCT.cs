using System;

namespace MonteCarloConnectFour
{
    public class UCT
    {
        public static double UCT_Value(Node node)
        {
            if (node.State.VisitCount == 0)
                return int.MaxValue;

            return (node.State.WinScore / (double)node.State.VisitCount) + 0.44 * Math.Sqrt(Math.Log(node.Parent.State.VisitCount) / (double)node.State.VisitCount);
        }

        public static Node FindBestNodeWithUCT(Node node)
        {
            Node bestNode = node.ChildArray.MaxObject(m => UCT_Value(m));
            return bestNode;
        }
    }
}


namespace MonteCarloConnectFour
{
    public class Tree
    {
        private Node root;

        public Tree()
        {
            root = new Node();
        }

        public Tree(Node root)
        {
            this.root = root;
        }

        public Node Root
        {
            get
            {
                return root;
            }
            set
            {
                this.root = value;
            }
        }

        public void AddChild(Node parent, Node child)
        {
            parent.ChildArray.Add(child);
        }

    }
}

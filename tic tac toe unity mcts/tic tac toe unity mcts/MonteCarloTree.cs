namespace tic_tac_toe_unity_mcts
{
	public class MonteCarloTree
	{
        MonteCarloTreeNode root;

        public MonteCarloTree() {
            root = new MonteCarloTreeNode();
        }

        public MonteCarloTree(MonteCarloTreeNode root) {
            this.root = root;
        }

        public void AddNode(MonteCarloTreeNode parent, MonteCarloTreeNode child) {
            parent.Children.Add(child);
        }


        public MonteCarloTreeNode Root => root;
    }
}


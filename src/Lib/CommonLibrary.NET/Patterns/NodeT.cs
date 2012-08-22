using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace ComLib.Patterns
{
    /// <summary>
    /// Interface designating a node with id, parent id and name.
    /// </summary>
    public interface INodeWithIds
    {
        /// <summary>
        /// Id of the node.
        /// </summary>
        int Id { get; }


        /// <summary>
        /// Name of the node.
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Id of the parent of this node.
        /// </summary>
        int ParentId { get; }
    }


    /// <summary>
    /// Node to represent a root node.
    /// </summary>
    /// <typeparam name="T">Type of elements.</typeparam>
    public class NodeRoot<T> : Node<T>
    {   
        IDictionary<int, Node<T>> _nodesById = new Dictionary<int, Node<T>>();
        IDictionary<string, Node<T>> _nodesByName = new Dictionary<string, Node<T>>();


        /// <summary>
        /// Default initialization.
        /// </summary>
        public NodeRoot()
        {
            _parent = this;
            _root = this;
        }
    

        /// <summary>
        /// Get node by it's id.
        /// </summary>
        /// <param name="id">Node id.</param>
        /// <returns>Node designated by id.</returns>
        public Node<T> this[ int id]
        {
            get
            {
                if(_nodesById == null || !_nodesById.ContainsKey(id))
                    return null;

                return _nodesById[id];
            }
        }


        /// <summary>
        /// Get the node by it's name.
        /// </summary>
        /// <param name="id">Node name.</param>
        /// <returns>Node designated by name.</returns>
        public Node<T> this[string id]
        {
            get
            {
                if (_nodesByName == null || !_nodesByName.ContainsKey(id))
                    return null;
                return _nodesByName[id];
            }
        }


        internal void SetNodesById(IDictionary<int, Node<T>> nodesById)
        {
            _nodesById = nodesById;
        }


        internal void SetNodesByName(IDictionary<string, Node<T>> nodesByName)
        {
            _nodesByName = nodesByName;
        }
    }



    /// <summary>
    /// Class to represent a single node in a tree.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public class Node<T>
    {

        /// <summary>
        /// Convert the list of items to nodes via the interface.
        /// </summary>
        /// <typeparam name="TNode">Type of node to convert to.</typeparam>
        /// <param name="items">List of node items.</param>
        /// <returns>Converted node.</returns>
        public static Node<T> ToNodes<TNode>(IList<TNode> items) where TNode : INodeWithIds
        {
            NodeRoot<T> root = new NodeRoot<T>(); 
            IDictionary<int, Node<T>> nodesById = new Dictionary<int, Node<T>>();
            IDictionary<string, Node<T>> nodesByName = new Dictionary<string, Node<T>>();

            // 1. Store all the items as nodes by their id as Node<T>
            foreach (INodeWithIds item in items)
            {
                var node = new Node<T>((T)item);
                node._root = root;
                nodesById[item.Id] = node;
            }
            
            // 2. Build up the children.
            foreach(INodeWithIds item in items)
            {
                // Get existing one from step 1.
                Node<T> node = nodesById[item.Id];

                // Node w/ no parent.
                if (item.ParentId <= 0 )
                {
                    root.Add(node);
                    nodesByName[item.Name] = node;
                }
                else
                {
                    if (nodesById.ContainsKey(item.ParentId))
                    {
                        Node<T> parent = nodesById[item.ParentId];
                        parent.Add(node);
                        INodeWithIds parentItem = parent.Item as INodeWithIds;
                        nodesByName[parentItem.Name + "," + item.Name] = node;
                    }
                }
            }

            // Set the nodesmap on the root.
            root.SetNodesById(nodesById);
            root.SetNodesByName(nodesByName);
            return root;
        }


        /// <summary>
        /// Root node.
        /// </summary>
        protected NodeRoot<T> _root;


        /// <summary>
        /// Parent node.
        /// </summary>
        protected Node<T> _parent;


        /// <summary>
        /// Children nodes.
        /// </summary>
        protected IList<Node<T>> _children;
        

        /// <summary>
        /// Allow default initialization.
        /// </summary>
        public Node() { }


        /// <summary>
        /// Initialize w/ item
        /// </summary>
        /// <param name="item">Node item to initialize with.</param>
        public Node(T item)
        {
            Item = item;
        }


        /// <summary>
        /// The item representing this node.
        /// </summary>
        public T Item;


        /// <summary>
        /// Root node.
        /// </summary>
        public NodeRoot<T> Root { get { return _root; } set { _root = value; } }


        /// <summary>
        /// Parent of this item.
        /// </summary>
        public Node<T> Parent { get { return _parent; } set { _parent = value; } }


        /// <summary>
        /// The children of this item.
        /// </summary>
        public IList<Node<T>> Children { get { return _children; } set { _children = value; } }


        /// <summary>
        /// Add a child node.
        /// </summary>
        /// <param name="node">Child node to add.</param>
        public virtual void Add(T node)
        {
            Node<T> child = new Node<T>(){ Item = node };
            Add(child);
        }


        /// <summary>
        /// Add a child node.
        /// </summary>
        /// <param name="child">Child node to add.</param>
        public virtual void Add(Node<T> child)
        {
            if (_children == null)
                _children = new List<Node<T>>();

            child._parent = this;
            _children.Add(child);
        }


        /// <summary>
        /// Number of children.
        /// </summary>
        public int Count
        {
            get { return _children == null ? 0 : _children.Count; }
        }


        /// <summary>
        /// Remove the last node.
        /// </summary>
        public virtual void Remove()
        {
            if (_children == null || _children.Count == 0 ) 
                return;
            
            _children.RemoveAt(_children.Count - 1);
        }


        /// <summary>
        /// Remove at the specific index.
        /// </summary>
        /// <param name="ndx">Index to remove at.</param>
        public virtual void RemoveAt(int ndx)
        {
            if (_children == null || _children.Count == 0 )
                return;

            if (ndx < 0 || ndx >= _children.Count)
                throw new ArgumentOutOfRangeException("Can not remove node at " + ndx + ", index out of range.");

            _children.RemoveAt(ndx);
        }


        /// <summary>
        /// Remove all the elements.
        /// </summary>
        public virtual void Clear()
        {
            if (_children == null) 
                return;

            _children.Clear();
        }


        /// <summary>
        /// Determine if this has children.
        /// </summary>
        public bool HasChildren
        {
            get { return _children == null ? false : _children.Count > 0; }

        }
    }
}

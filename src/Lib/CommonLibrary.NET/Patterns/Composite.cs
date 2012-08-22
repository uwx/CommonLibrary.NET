/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using ComLib;


namespace ComLib.Patterns
{
    /// <summary>
    /// Compositie object with id and name.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public class CompositeWithIdAndName<T> : Composite<T> where T : CompositeWithIdAndName<T>
    {
        private Dictionary<string, T> _childrenByName = new Dictionary<string, T>();
        private Dictionary<int, T> _childrenById = new Dictionary<int, T>();


        /// <summary>
        /// Default name to empty.
        /// </summary>
        public CompositeWithIdAndName()
        {
            Name = string.Empty;
            Id = RootCategoryParentCategoryId;
        }


        /// <summary>
        /// Id of the node.
        /// </summary>
        public int Id { get; set; }

        
        /// <summary>
        /// Parent id.
        /// </summary>
        public int ParentId { get; set; }
        

        /// <summary>
        /// Name of the node.
        /// </summary>
        public string Name { get; set; }
        

        /// <summary>
        /// Return whether or not this is a root category.
        /// Which does not have a parent.
        /// </summary>
        public bool IsRoot
        {
            get { return _parent == null; }
        }


        /// <summary>
        /// Add node.
        /// </summary>
        /// <param name="node">Node to add.</param>
        public override void Add(T node)
        {
            base.Add(node);
            node.ParentId = this.Id;
            _childrenById[node.Id] = node;
            _childrenByName[node.Name] = node;
        }


        /// <summary>
        /// Remove the last node.
        /// </summary>
        public override void Remove()
        {
            if(_childrenList.Count == 0)
                return;

            InternalRemove(_childrenList.Count - 1);
        }


        /// <summary>
        /// Remove at the specific index.
        /// </summary>
        /// <param name="ndx">Node index.</param>
        public override void RemoveAt(int ndx)
        {
            Guard.IsTrue(ndx <= _childrenList.Count - 1, "Index : " + ndx + " is out of range.");

            // Remove from lookups.
            InternalRemove(ndx);
        }


        /// <summary>
        /// Remove all the elements.
        /// </summary>
        public override void Clear()
        {            
            _childrenByName.Clear();
            _childrenById.Clear();
            base.Clear();
        }


        /// <summary>
        /// Return node by name.
        /// </summary>
        /// <param name="name">Node name.</param>
        /// <returns>Node designated by the name.</returns>
        public T this[string name]
        {
            get
            {
                return _childrenByName[name];
            }
        }


        /// <summary>
        /// Return by id.
        /// </summary>
        /// <param name="id">Node index.</param>
        /// <returns>Node designated by the index.</returns>
        public T this[int id]
        {
            get
            {
                return _childrenById[id];
            }
        }


        /// <summary>
        /// Remove from lookups.
        /// </summary>
        /// <param name="ndx">Node index.</param>
        private void InternalRemove(int ndx)
        {
            // Get node and remove from lookups.
            T node = _childrenList[ndx];
            _childrenById.Remove(node.Id);
            _childrenByName.Remove(node.Name);

            // Now remove from list.
            base.RemoveAt(ndx);
        }
    }

    
    /// <summary>
    /// Composite object.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public class Composite<T> where T : Composite<T>
    {
        /// <summary>
        /// Id of root parent.
        /// </summary>
        public const int RootCategoryParentCategoryId = 0;


        /// <summary>
        /// List with children elements.
        /// </summary>
        protected List<T> _childrenList;


        /// <summary>
        /// Parent element.
        /// </summary>
        protected Composite<T> _parent;


        /// <summary>
        /// Creates a new instance of this class
        /// with an empty list of children.
        /// </summary>
        public Composite()
        {
            _childrenList = new List<T>();
        }


        /// <summary>
        /// Add a child node.
        /// </summary>
        /// <param name="node">Child node to add.</param>
        public virtual void Add(T node)
        {
            node.Parent = this;
            _childrenList.Add(node);  
        }


        /// <summary>
        /// Number of children.
        /// </summary>
        public int Count
        {
            get { return _childrenList.Count; }
        }


        /// <summary>
        /// Remove the last node.
        /// </summary>
        public virtual void Remove()
        {
            if(_childrenList.Count == 0)
                return;

            _childrenList.RemoveAt(_childrenList.Count - 1);
        }


        /// <summary>
        /// Remove at the specific index.
        /// </summary>
        /// <param name="ndx">Node index.</param>
        public virtual void RemoveAt(int ndx)
        {
            _childrenList.RemoveAt(ndx);
        }


        /// <summary>
        /// Remove all the elements.
        /// </summary>
        public virtual void Clear()
        {
            _childrenList.Clear();
        }


        /// <summary>
        /// Determine if this has children.
        /// </summary>
        public bool HasChildren
        {
            get { return _childrenList.Count > 0; }
        }


        /// <summary>
        /// Children.
        /// </summary>
        public ReadOnlyCollection<T> Children
        {
            get { return new ReadOnlyCollection<T>(_childrenList); }
        }


        /// <summary>
        /// Parent node.
        /// </summary>
        public Composite<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
    }




    /// <summary>
    /// Category look up class.
    /// This contains all the categories / subcategories available.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public class CompositeLookup<T> where T : CompositeWithIdAndName<T>
    {
        private string _separator = ",";
        private IDictionary<int, T> _nodesById;
        private ReadOnlyCollection<T> _rootNodesList;
        private IDictionary<string, T> _nodesByName;
        private bool _isCaseSensitive = false;


        /// <summary>
        /// Initialzie the lookup
        /// </summary>
        /// <param name="allCategories">List with all categories.</param>
        public CompositeLookup(IList<T> allCategories)
        {
            _nodesById = new Dictionary<int, T>();
            _nodesByName = new Dictionary<string, T>();
            Initilaize(allCategories);
        }


        /// <summary>
        /// Returns the category associated with the id.
        /// </summary>
        /// <param name="id">Category id.</param>
        /// <returns>Category designated by id.</returns>
        public T this[int id]
        {
            get
            {
                if (_nodesById.ContainsKey(id))
                    return _nodesById[id];

                return default(T);
            }
        }


        /// <summary>
        /// Get by fully qualified name.
        /// e.g. "Art.Painting.WaterColor"
        /// </summary>
        /// <param name="fullyQualifiedName">Qualified name.</param>
        /// <returns>Node designated by name.</returns>
        public T this[string fullyQualifiedName]
        {
            get
            {
                string key = BuildKey(fullyQualifiedName);
                if (!_nodesByName.ContainsKey(key))
                    return default(T);

                return _nodesByName[key];
            }
        }


        /// <summary>
        /// Get category by parent,child name.
        /// </summary>
        /// <param name="parentName">Name of parent.</param>
        /// <param name="childName">Name of child.</param>
        /// <returns>Found node.</returns>
        public T GetByName(string parentName, string childName)
        {
            // Validate
            if (string.IsNullOrEmpty(parentName)) { return null; }
            if (string.IsNullOrEmpty(childName)) { return null; }

            string key = BuildKey(parentName, childName);
            if (!_nodesByName.ContainsKey(key)) { return null; }
            return _nodesByName[key];
        }


        /// <summary>
        /// Returns a readonly collection of the root categories.
        /// </summary>
        public ReadOnlyCollection<T> RootNodes
        {
            get { return _rootNodesList; }
        }


        /// <summary>
        /// Get children given the parent id.
        /// </summary>
        /// <param name="id">Id of parent.</param>
        /// <returns>Collection of parent's children.</returns>
        public ReadOnlyCollection<T> Children(int id)
        {
            T category = this[id];
            if (category == null || !category.HasChildren)
                return null;

            return category.Children;
        }


        /// <summary>
        /// Store the categories appropriately.
        /// </summary>
        /// <param name="allNodes">List with all nodes.</param>
        private void Initilaize(IList<T> allNodes)
        {
            IList<T> rootNodes = new List<T>();
            IDictionary<int, IList<T>> rootCatChildren = new Dictionary<int, IList<T>>();

            // First store every node by id.
            foreach (T node in allNodes)
            {
                // Store by id and name.
                StoreById(node);
            }

            // Iterate again and store heirarchy.
            foreach (T node in allNodes)
            {
                // Store Root category.
                if (node.ParentId == Composite<T>.RootCategoryParentCategoryId)
                {
                    rootNodes.Add(node);
                }
                else // Child
                {
                    // Get the parent.
                    T parent = _nodesById[node.ParentId];
                    parent.Add(node);
                }
            }

            // Final loop
            foreach (T node in allNodes)
            {
                StoreByName(node);
            }
            _rootNodesList = new ReadOnlyCollection<T>(rootNodes);
        }


        /// <summary>
        /// Store by the id.
        /// [2] = Painting
        /// </summary>
        /// <param name="node">Node to store.</param>
        private void StoreById(T node)
        {
            // Store each category.
            _nodesById.Add(node.Id, node);
        }


        /// <summary>
        /// Builds a fully qualified name.
        /// Parent.Child.GrandChild etc.
        /// Art.Painting.Watercolor
        /// </summary>
        /// <param name="node">Node to store.</param>
        private void StoreByName(T node)
        {
            string key = _isCaseSensitive ? node.Name : node.Name.Trim().ToLower();
            T nodeToIterate = node;

            if (nodeToIterate.Parent != null)
            {
                // Art.Painting.Oil
                // Art.Painting.Water
                while (nodeToIterate.Parent != null)
                {
                    string name = ((CompositeWithIdAndName<T>)nodeToIterate.Parent).Name;
                    if (!_isCaseSensitive)
                        name = name.Trim().ToLower();

                    key =  name + _separator + key;
                    nodeToIterate = nodeToIterate.Parent as T;
                }
            }
            _nodesByName[key] = node;
        }


        private string BuildKey(string key)
        {
            if (_isCaseSensitive)
                return key;

            return key.Trim().ToLower();
        }


        /// <summary>
        /// Build category key.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        private string BuildKey(string parent, string child)
        {
            return parent.Trim().ToLower() + _separator + child.Trim().ToLower();
        }
    }

}

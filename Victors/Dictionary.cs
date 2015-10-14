using System.Collections.Generic;

namespace Victors
{
    public class Dictionary
    {
        protected DictionaryCollection Items { get; set; }
        public List<string> Names { get; protected set; }
        public int Count => Items.Count;
        public bool isEmpty => !(Items.Count>0);

        public Dictionary() : this(new DictionaryCollection())
        {

        }
        public Dictionary(string name, dynamic value) : this()
        {
            Add(name, value);
        }
        public Dictionary(List<string> names, List<dynamic> values) : this()
        {
            Add(names, values);
        }
        public Dictionary(List<dynamic> nvalues) : this()
        {
            Add(nvalues);
        }

        protected Dictionary(DictionaryCollection items)
        {
            Items = items;
            Names = items.GetNames();
        }

        public dynamic this[int index]
        {
            get
            {
                return Items[index].Value;
            }
            set
            {
                Items[index].Value = value;
            }
        }
        public dynamic this[string name]
        {
            get
            {
                foreach (DictItem item in Items)
                {
                    if (item.Name.ToLower() == name.ToLower())
                    {
                        return item.Value;
                    }
                }
                throw new System.IndexOutOfRangeException("[" + name + "]" + " didn't find");
            }
            set
            {
                foreach (DictItem item in Items)
                {
                    if (item.Name.ToLower() == name.ToLower())
                    {
                        item.Value = value;
                    }
                }
            }
        }

        protected void Add(DictItem item)
        {
            Items.Add(item);
            Names.Add(item.Name);
        }
        public void Add(string name, dynamic value)
        {
            Add(new DictItem(name, value));
        }
        public void Add(List<string> names, List<dynamic> values)
        {
            int i = 0;
            foreach (string name in names)
            {
                try
                {
                    Add(new DictItem(name, values[i++]));
                }
                catch { throw new System.IndexOutOfRangeException(); }
            }
        }
        public void Add(List<dynamic> nvalues)
        {
            int i = 0;
            foreach (dynamic nvalue in nvalues)
            {
                try
                {
                    Add(new DictItem((string)nvalues[i++], nvalues[i++]));
                }
                catch { throw new System.IndexOutOfRangeException(); }
            }
        }

        protected class DictionaryCollection : List<DictItem>
        {
            public DictionaryCollection(List<DictItem> items) : base(items)
            {

            }
            public DictionaryCollection() : base()
            {

            }
            public List<string> GetNames()
            {
                List<string> names = new List<string>();
                foreach (DictItem item in this)
                {
                    names.Add(item.Name);
                }
                return names;
            }
        }
        protected class DictItem
        {
            public string Name { get; set; }
            public dynamic Value { get; set; }
            public DictItem(string name, dynamic value)
            {
                Name = name;
                Value = value;
            }
            public DictItem() : this(null, null)
            {

            }
        }
    }
    public class DictionaryList : List<Dictionary>
    {

    }
}

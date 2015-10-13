using System.Collections.Generic;

namespace Victors.VFBClientClasses
{
    public class Fields
    {
        public FieldItems Items { get; set; }
        public Fields(FieldItems items)
        {
            Items = items;
        }
        public Fields() : this(new FieldItems())
        {

        }
        public dynamic this[int index]
        {
            get
            {
                return Items[index].Value;
            }
        }
        public dynamic this[string caption]
        {
            get
            {
                foreach(FieldItem item in Items)
                {
                    if (item.Name.ToLower() == caption.ToLower())
                    {
                        return item.Value;
                    }
                }
                throw new System.IndexOutOfRangeException("[" + caption + "]" + " didn't find");
            }
            set
            { 
                foreach (FieldItem item in Items)
                {
                    if (item.Name.ToLower() == caption.ToLower())
                    {
                        item.Value = value;
                    }
                }
            }
        }
    }
    public class FieldItems : List<FieldItem>
    {
        public FieldItems(List<FieldItem> items) : base(items)
        {
            
        }
        public FieldItems() : base(new List<FieldItem>())
        {

        }
    }
    public class FieldItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public FieldItem(string caption, string field)
        {
            Name = caption;
            Value = field;
        }
        public FieldItem() : this(null, null)
        {

        }
        public FieldItem(string field) : this(field, field)
        {

        }
    }

    public class Filter
    {
        public FilterItems Items { get; set; }
        public Filter(FilterItems items)
        {
            Items = items;
        }
        public Filter() : this(new FilterItems())
        {

        }
        public dynamic this[int index]
        {
            get
            {
                return Items[index].Value;
            }
        }
        public dynamic this[string caption]
        {
            get
            {
                foreach (FilterItem item in Items)
                {
                    if (item.Name.ToLower() == caption.ToLower())
                    {
                        return item.Value;
                    }
                }
                throw new System.IndexOutOfRangeException("[" + caption + "]" + " didn't find");
            }
            set
            {
                foreach (FilterItem item in Items)
                {
                    if (item.Name.ToLower() == caption.ToLower())
                    {
                        item.Value = value;
                    }
                }
            }
        }

    }
    public class FilterItems : List<FilterItem>
    {
        public FilterItems(List<FilterItem> items) : base(items)
        {

        }
        public FilterItems() : base()
        {

        }
    }
    public class FilterItem
    {
        public string Name { get; set; }
        public dynamic Value { get; set; }
        public FilterItem(string caption, dynamic value)
        {
            Name = caption;
            Value = value;
        }
        public FilterItem() : this(null, null)
        {

        }
    }

}

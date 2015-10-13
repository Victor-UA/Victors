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
        public int IndexOfCaption(string caption)
        {
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Caption == caption)
                    {
                        return i;
                    }
                }
            }
            catch { }
            return -1;
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
        public string Caption { get; set; }
        public string Field { get; set; }
        public FieldItem(string caption, string field)
        {
            Caption = caption;
            Field = field;
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
        public string Caption { get; set; }
        public dynamic Value { get; set; }
        public FilterItem(string caption, dynamic value)
        {
            Caption = caption;
            Value = value;
        }
        public FilterItem() : this(null, null)
        {

        }
    }

}

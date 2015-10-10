using System.Collections.Generic;

namespace Victors.VFBClientClasses
{
    public class Fields
    {
        public List<FieldItem> Items { get; set; }
        public Fields(List<FieldItem> items)
        {
            Items = items;
        }
        public Fields() : this(new List<FieldItem>())
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
        public List<FilterItem> Items { get; set; }
        public Filter(List<FilterItem> items)
        {
            Items = items;
        }
        public Filter() : this(new List<FilterItem>())
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

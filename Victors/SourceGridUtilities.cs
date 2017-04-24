using System.Data;
using System;
using Victors;
using System.Windows.Forms;

namespace SourceGridUtilities
{
    public class RowTag : object
    {
        public int Index { get; set; }
        public dynamic Key { get; set; }
        public RowTag(int index, dynamic key)
        {
            Index = index;
            Key = key;
        }
        public RowTag() : this(-1, null) { }
    }
    public static class Grid
    {
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key, Dictionary fields, Dictionary filter, string filtertype, bool casesensitive)
        {
            grid.Columns.Clear();
            grid.Rows.Clear();

            //Columns filling
            grid.ColumnsCount = fields.isEmpty ? dt.Columns.Count : fields.Count;
            grid.FixedRows = 1;
            grid.Rows.Insert(0);
            for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
            {
                grid[0, i] = new SourceGrid.Cells.ColumnHeader(fields.isEmpty ? dt.Columns[i].Caption : fields.Names[i]);
            }

            string Source = "";
            string Filter = "";

            //Data filling
            for (int r = 0; r < dt.Rows.Count; r++)
            { 
                bool RowChecked = filter.isEmpty | fields.isEmpty;
                if (!RowChecked)
                {                    
                    for (int i = 0; i < filter.Count & !RowChecked; i++)
                    {
                        //int index = fields.IndexOfCaption(filter.Items[i].Caption);
                        try
                        {
                            switch (filtertype.ToLower())
                            {
                                case "full match":
                                    if (casesensitive)
                                    {
                                        RowChecked = dt.Rows[r][fields[filter.Names[i]]] == filter[i];
                                    }
                                    else
                                    {
                                        Source = dt.Rows[r][fields[filter.Names[i]]] is DBNull ? "" :
                                        dt.Rows[r][fields[filter.Names[i]]] is DateTime ?
                                            string.Format("{0:dd.MM.yyyy HH:mm:ss}", dt.Rows[r][fields[filter.Names[i]]]) :
                                            (string)dt.Rows[r][fields[filter.Names[i]]];
                                        Filter = filter[i] is DBNull ? "" :
                                            filter[i] is DateTime ?
                                            string.Format("{0:dd.MM.yyyy HH:mm:ss}", filter[i]) :
                                            (string)filter[i];
                                        RowChecked = Source.ToLower() == Filter.ToLower();
                                    }                                    
                                    break;
                                case "partial match":
                                    Source = dt.Rows[r][fields[filter.Names[i]]] is DBNull ? "" :
                                        dt.Rows[r][fields[filter.Names[i]]] is DateTime ?
                                            string.Format("{0:dd.MM.yyyy HH:mm:ss}", dt.Rows[r][fields[filter.Names[i]]]) : 
                                            (string)dt.Rows[r][fields[filter.Names[i]]];
                                    Filter = filter[i] is DBNull ? "" :
                                        filter[i] is DateTime ?
                                        string.Format("{0:dd.MM.yyyy HH:mm:ss}", filter[i]) :
                                        (string)filter[i];
                                    if (casesensitive)
                                    {
                                        RowChecked = Source.Contains(Filter);
                                    }
                                    else
                                    {

                                        RowChecked = Source.ToLower().Contains(Filter.ToLower());
                                    }
                                    break;
                                default :
                                    Source = dt.Rows[r][fields[filter.Names[i]]] is DBNull ? "" :
                                        dt.Rows[r][fields[filter.Names[i]]] is DateTime ?
                                            string.Format("{0:dd.MM.yyyy HH:mm:ss}", dt.Rows[r][fields[filter.Names[i]]]) :
                                            (string)dt.Rows[r][fields[filter.Names[i]]];

                                    Filter = filter[i] is DBNull ? "" :
                                        filter[i] is DateTime ?
                                        string.Format("{0:dd.MM.yyyy HH:mm:ss}", filter[i]) :
                                        (string)filter[i];
                                    if (casesensitive)
                                    {
                                        RowChecked = Source.Contains(Filter);
                                    }
                                    else
                                    {
                                        RowChecked = Source.ToLower().Contains(Filter.ToLower());
                                    }
                                    break;
                            }
                            
                        }
                        catch
                        {
                            RowChecked = true;
                        }
                    }
                }
                if (!RowChecked)
                {
                    continue;
                }
                grid.Rows.Insert(grid.RowsCount);
                try
                {
                    grid.Rows[grid.RowsCount - 1].Tag = new RowTag(r, key == "" ? null : dt.Rows[r][key]);
                }
                catch
                {
                    grid.Rows[grid.RowsCount - 1].Tag = new RowTag(r, null);
                }
                for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
                {
                    if (fields.isEmpty)
                    {
                        grid[grid.RowsCount - 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][i] is DBNull ? 
                            "" : dt.Rows[r][i]);
                    }
                    else
                    {
                        grid[grid.RowsCount - 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][fields[i]] is DBNull ? 
                            "" : dt.Rows[r][fields[i]]);
                    }
                }
            }
            grid.AutoSizeCells();
        }
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key, Dictionary fields, Dictionary filter, string filtertype)
        {
            Fill(grid, dt, key, fields, filter, filtertype, false);
        }
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key, Dictionary fields, Dictionary filter)
        {
            Fill(grid, dt, key, fields, filter, "partial match");
        }
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key, Dictionary fields)
        {
            Fill(grid, dt, key, fields, new Dictionary());
        }
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key)
        {
            Fill(grid, dt, key, new Dictionary(), new Dictionary());
        }
        public static void Fill(SourceGrid.Grid grid, DataTable dt)
        {
            Fill(grid, dt, "", new Dictionary(), new Dictionary());
        }
    }
}

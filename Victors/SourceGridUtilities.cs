﻿using System.Data;
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
        public RowTag()
        {
            Index = -1;
            Key = null;
        }
    }
    public static class Grid
    {
        
        public static void Fill(SourceGrid.Grid grid, DataTable dt, string key, Dictionary fields, Dictionary filter)
        {
            //Columns filling
            grid.ColumnsCount = fields.isEmpty ? dt.Columns.Count : fields.Count;
            grid.FixedRows = 1;
            grid.Rows.Insert(0);
            for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
            {
                grid[0, i] = new SourceGrid.Cells.ColumnHeader(fields.isEmpty ? dt.Columns[i].Caption : fields.Names[i]);
            }

            //Data filling
            for (int r = 0; r < dt.Rows.Count; r++)
            { 
                bool RowChecked = filter.isEmpty || fields.isEmpty;
                if (!RowChecked)
                {
                    RowChecked = true;
                    for (int i = 0; i < filter.Count || !RowChecked; i++)
                    {
                        //int index = fields.IndexOfCaption(filter.Items[i].Caption);
                        try
                        {
                            RowChecked = dt.Rows[r][fields[filter.Names[i]]] == filter[i];
                        }
                        catch { }
                    }
                }
                if (!RowChecked)
                {
                    continue;
                }
                grid.Rows.Insert(r + 1);
                try
                {
                    grid.Rows[r + 1].Tag = new RowTag(r, key == "" ? null : dt.Rows[r][key]);
                }
                catch
                {
                    grid.Rows[r + 1].Tag = new RowTag(r, null);
                }
                for (int i = 0; i < (fields.isEmpty ? dt.Columns.Count : fields.Count); i++)
                {
                    if (fields.isEmpty)
                    {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][i] is DBNull ? "" : dt.Rows[r][i]);
                    }
                    else
                    {
                        grid[r + 1, i] = new SourceGrid.Cells.Cell(dt.Rows[r][fields[i]] is DBNull ? "" : dt.Rows[r][fields[i]]);
                    }
                }
            }
            grid.AutoSizeCells();
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

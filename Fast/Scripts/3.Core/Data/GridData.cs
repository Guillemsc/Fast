using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Data
{
    public class GridData
    {
        private readonly List<List<object>> data_list = new List<List<object>>();

        public GridData()
        {

        }

        public GridData(List<List<object>> data)
        {
            data_list = new List<List<object>>(data);
        }

        public int RowsCount
        {
            get { return data_list.Count; }
        }

        public List<object> GetRowData(int index)
        {
            List<object> ret = new List<object>();

            if(index < data_list.Count)
            {
                ret = new List<object>(data_list[index]);
            }

            return ret;
        }

        public T GetData<T>(int row_index, int column_index)
        {
            T ret = default(T);

            List<object> row = GetRowData(row_index);

            if(column_index < row.Count)
            {
                ret = (T)row[column_index];
            }

            return ret;
        }
    }
}

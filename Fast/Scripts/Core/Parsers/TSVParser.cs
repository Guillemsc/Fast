using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Fast.Parsers
{
    class TSVParser
    {
        public static Data.GridData Parse(List<string> lines)
        {
            Data.GridData ret = null;

            List<List<object>> data = new List<List<object>>();

            for(int i = 0; i < lines.Count; ++i)
            {
                string curr_line = lines[i];

                List<object> column_data = new List<object>();

                List<string> sliced_data = curr_line.Split('\t').ToList();

                for(int y = 0; y < sliced_data.Count; ++y)
                {
                    string final_str = sliced_data[y].Replace("\t", "");

                    column_data.Add(final_str);
                }

                data.Add(column_data);
            }

            ret = new Data.GridData(data);

            return ret;
        }

        public static string Compose(Data.GridData data)
        {
            string ret = "";

            if (data != null)
            {
                for (int i = 0; i < data.RowsCount; ++i)
                {
                    List<object> row_data = data.GetRowData(i);

                    for (int y = 0; y < row_data.Count; ++y)
                    {
                        object column_object = row_data[y];

                        ret += column_object.ToString() + "\t";
                    }

                    ret += "\n";
                }
            }
            else
            {
                Debug.LogError("[Fast.Parsers.TSVParser] GridData is null and cannot be parsed");
            }

            return ret;
        }
    }
}

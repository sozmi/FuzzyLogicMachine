using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp.Converters
{
    public class MatrixToDataViewConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var myDataTable = new DataTable();
            var colums = values[0] as string[];
            var rows = values[1] as string[];
            var vals = values[2] as double[][];
            myDataTable.Columns.Add("---");    //The blanc corner column
            if(colums!=null && rows!=null && vals != null)
            {
                foreach (var value in colums)
                {
                    myDataTable.Columns.Add(value);
                }
                int index = 0;

                foreach (string row in rows)
                {
                    var tmp = new string[1 + vals[index].Length];
                    tmp[0] = row.ToString();
                    for (int i = 0; i < vals[index].Length; i++)
                        tmp[i+1] = vals[index][i].ToString(); 
                    myDataTable.Rows.Add(tmp);
                    index++;
                }
            }
            return myDataTable.DefaultView;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

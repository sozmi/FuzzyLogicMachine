using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Tables
{
    public class Matrix
    {
        public double[][] matrix;
        public string[] columns;
        public string[] rows;

        public Matrix()
        {
        }

        public Matrix(string[] columns_, string[] rows_)
        {
            this.columns = columns_;
            this.rows = rows_;
            this.matrix = new double[this.rows.Length][];
            for (int i = 0; i < matrix.Length; i++)
                matrix[i] = new double[this.columns.Length];
        }
        public Matrix(List<List<double>> m_, string columns_, string rows_)
        {
            this.columns = columns_.Split(";");
            this.rows = rows_.Split(";");
            this.matrix = new double[rows.Length][];
            for (int i = 0; i < m_.Count; i++)
                this.matrix[i] = [.. m_[i]];
        }

        public static Matrix Compose(Matrix t, Matrix s)
        {
            Matrix m = new(t.columns, s.rows);

            for (int k = 0; k < t.columns.Length; k++)
                for (int i = 0; i < s.rows.Length; i++)
                    for (int j = 0; j < s.columns.Length; j++)
                        m.matrix[i][k] = Math.Max(Math.Min(s.matrix[i][j], t.matrix[j][k]), m.matrix[i][k]);
            

            return m;
        }
        public static Matrix ComposeProd(Matrix t, Matrix s)
        {
            Matrix m = new(t.columns, s.rows);

            for (int k = 0; k < t.columns.Length; k++)
                for (int i = 0; i < s.rows.Length; i++)
                    for (int j = 0; j < s.columns.Length; j++)
                        m.matrix[i][k] = Math.Max(s.matrix[i][j] * t.matrix[j][k], m.matrix[i][k]);

            return m;
        }
    }

    public class MatrixViewModel : BaseViewModel
    {
        public Matrix matrix;
        public MatrixViewModel(Matrix matrix) { 
        this.matrix = matrix;
        }
        public string[] RowHeaders { get => matrix.rows; set => SetProperty(matrix.rows, value, v => matrix.rows = v); }

        public string[] ColumnHeaders { get => matrix.columns; set => SetProperty(matrix.columns, value, v => matrix.columns = v); }

        public double[][] Values { get => matrix.matrix; set => SetProperty(matrix.matrix, value, v => matrix.matrix = v); }
    }
    public class TablesViewModel : BaseViewModel
    {
        public MatrixViewModel S { get => s; set => SetProperty(ref s, value); } 
        private MatrixViewModel s;

        public MatrixViewModel T { get => t; set => SetProperty(ref t, value); }
        private MatrixViewModel t;

        public MatrixViewModel ST { get => st; set => SetProperty(ref st, value); }
        private MatrixViewModel st;
        public string X { get => x; set => SetProperty(ref x, value); }
        private string x;
        public string Y { get => y; set => SetProperty(ref y, value); }
        private string y;
        public string Z { get => z; set => SetProperty(ref z, value); }
        private string z;

        public TablesViewModel()
        {
            ActionAddDefault = new(AddDefault);
            ActionMaxMin = new(MaxMin);
            ActionProd = new(Prod);
        }

        private void Prod()
        {
            Matrix stm = Matrix.ComposeProd(T.matrix, S.matrix);
            ST = new(stm);
        }

        private void MaxMin()
        {
            Matrix stm = Matrix.Compose(T.matrix, S.matrix);
            ST = new(stm);
        }

        public RelayAction ActionAddDefault { get; set; }
        public RelayAction ActionMaxMin { get; set; }
        public RelayAction ActionProd {  get; set; }

        private void AddDefault()
        {
            X = "Менеджер;Программист;Водитель;Секретарь;Переводчик";
            Y = "Быстрота и гибкость мышления;Умение быстро принимать решения;Устойчивость и концентрация внимания;Зрительная nпамять;Быстрота nреакции;" +
                    "Двигательная память;Физическая выносливость;Координация движений;Эмоционально-волевая устойчивость;Ответственность";
            Z = "Петров;Иванов;Сидоров;Васильева;Григорьева";

            List<List<double>> s = [
                [0.9, 0.9, 0.8, 0.4, 0.5, 0.3, 0.6, 0.2, 0.9, 0.8],
                [0.8, 0.5, 0.9, 0.3, 0.1, 0.2, 0.2, 0.2, 0.5, 0.5],
                [0.3, 0.9, 0.6, 0.5, 0.9, 0.8, 0.9, 0.8, 0.6, 0.3],
                [0.5, 0.4, 0.5, 0.5, 0.2, 0.2, 0.3, 0.3, 0.9, 0.8],
                [0.7, 0.8, 0.8, 0.2, 0.6, 0.2, 0.2, 0.3, 0.3, 0.2]
            ];
            Matrix sm = new(s, Y, X);
            S = new(sm);

            List<List<double>> t = [
                [0.9, 0.8, 0.7, 0.9, 1.0],
                [0.6, 0.4, 0.8, 0.5, 0.6],
                [0.5, 0.2, 0.3, 0.8, 0.7],
                [0.5, 0.9, 0.5, 0.8, 0.4],
                [1.0, 0.6, 0.5, 0.7, 0.4],
                [0.4, 0.5, 1.0, 0.7, 0.8],
                [0.5, 0.8, 0.9, 0.5, 0.4],
                [0.5, 0.6, 0.7, 0.6, 0.5],
                [0.8, 1.0, 0.2, 0.5, 0.6],
                [0.3, 0.5, 0.9, 0.6, 0.8]
            ];
            Matrix tm = new(t, Z, Y);
            T = new(tm);
        }
        
    }

}

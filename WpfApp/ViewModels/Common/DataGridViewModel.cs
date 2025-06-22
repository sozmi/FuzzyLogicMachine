using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{
    public class DataGridViewModel<TViewModel> : CollectionViewModel where TViewModel : new()
    {
        public delegate void FuncHandler(ParamsInfo info);
        public event FuncHandler? OnAdd, OnRemove, OnEdit;
        public ObservableCollection<TViewModel> Items { get; set; } = [];
        public DataGridViewModel()
        {
            Items = [];
            Items.CollectionChanged += DataCollectionChanged;
        }
        public DataGridViewModel(List<TViewModel> views)
        {
            Items = [.. views];
            foreach (var item in views)
                if (item is BaseViewModel b)
                    b.PropertyChanged += ItemChanged;
            Items.CollectionChanged += DataCollectionChanged;
        }

        private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ParamsInfo info = new()
                    {
                        Index = Items.Count
                    };
                    if (item is TViewModel i)
                        info.Item = i;
                    if (item is BaseViewModel b)
                        b.PropertyChanged += ItemChanged;
                    OnAdd?.Invoke(info);
                }

                Debug.WriteLine("Добавленных элементов: {0}", e.NewItems.Count);
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ParamsInfo info = new()
                    {
                        Index = e.OldStartingIndex,
                    };
                    if (item is TViewModel i)
                        info.Item = i;
                    OnRemove?.Invoke(info);
                }
                Debug.WriteLine("Удалённых элементов: {0}", e.OldItems.Count);
            }
        }

        private void ItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TViewModel f)
            {
                int index = Items.IndexOf(f);
                ParamsInfo info = new()
                {
                    Index = index,
                    Item = f
                };
                OnEdit?.Invoke(info);
            }
        }
    }
}
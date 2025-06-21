using ClassLibraryFLM.Functions;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    public class ParamViewModel : BaseViewModel
    {
        private readonly string _name;
        public readonly Guid Id;
        public string Name { get => _name; }
        private double _value;
        public double Value { get => _value; set => SetProperty(ref _value, value); }
        public ParamViewModel() { }

        public ParamViewModel(KeyValuePair<Guid, ParamValue> p)
        {
            _value = p.Value.Value;
            _name = p.Value.Name;
            Id = p.Key;
        }
    }
}
using ClassLibraryFLM.Base;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{
    public class PairViewModel<A, B>(Pair<A, B> pair) : BaseViewModel
    {
        public Pair<A, B> item = pair;
        public A Item1 { get => item.Item1; set => SetProperty(item.Item1, value, v => { item.Item1 = v; }); }
        public B Item2 { get => item.Item2; set => SetProperty(item.Item2, value, v => { item.Item2 = v; }); }
    }
}
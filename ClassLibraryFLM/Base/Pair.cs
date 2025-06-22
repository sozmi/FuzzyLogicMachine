namespace ClassLibraryFLM.Base
{
    public class Pair<A,B>(A a, B b)
    {
        public A Item1 { get; set; } = a;
        public B Item2 { get; set; } = b;
    }
}

namespace ClassLibraryFLM.Base
{
    public class Pair<A,B>
    {
        public A Item1 { get; set; }
        public B Item2 { get; set; }

        public Pair(A a, B b)
        {
            Item1 = a;
            Item2 = b;
        }
    }
}

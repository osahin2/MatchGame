namespace Pool
{
    public interface IPooler<T> where T : class
    {
        T GetPooled();
        void Free(T obj);

        void Clear();
    }
}
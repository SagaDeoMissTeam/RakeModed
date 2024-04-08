namespace RakeModed.assetLoader__Experemental_.data
{
    public class SDMAsset<T>
    {
        public string name { get; }
        public T obj { get; }

        public SDMAsset(string name, T obj)
        {
            this.name = name;
            this.obj = obj;
        }

        public T getObj()
        {
            return obj;
        }

        public string getName()
        {
            return name;
        }
    }
}
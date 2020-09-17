using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RvaJustin.AjaxGenerator.Repositories
{
    public abstract class Repository<TValue>
    {
        private readonly ConcurrentDictionary<string, TValue> data = new ConcurrentDictionary<string, TValue>();

        protected Repository()
        {
            
        }

        public bool IsEmpty => data.Count == 0;

        public virtual bool TryGet(string key, out TValue value)
        {
            value = default(TValue);
            if (!data.ContainsKey(key))
            {
                return false;
            }
            
            value = data[key];
            return true;
        }

        public virtual void Set(string key, TValue value)
        {
            data[key] = value;
        }

        protected void ReplaceValues(IDictionary<string,TValue> dictionary)
        {
            data.Clear();
            
            foreach (var key in dictionary.Keys)
            {
                data[key] = dictionary[key];
            }
        }
    }
}
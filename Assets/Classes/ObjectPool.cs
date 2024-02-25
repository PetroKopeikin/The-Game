using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ObjectPool<T> where T : GameItem
    {
        private List<T> items = new();
        GameObject objectsParent;

        public int Size
        {
            get { return items.Capacity; }
        }
      
        public ObjectPool(int capacity, string id) 
        {
            items.Capacity = capacity;
            objectsParent = new GameObject(id + "Parent");
        }
        
        public void AddToPool(T item) 
        {
            if(item)
            {
                var itemCopy = GameObject.Instantiate(item.gameObject);
                itemCopy.transform.SetParent(objectsParent.transform);
                item.Deactivate();
            }
        }

        public void ReturnToPool(T item)
        {
            if(item && items.Contains(item))
            {
                item.Deactivate();
            }
        }

        public T GetFromPool(string name)
        {
            T result = null;    
            if(items.Count == 0)
            {
                return result;
            }

            foreach(T item in items)
            {
                if(item.gameObject.name == name)
                {
                    result = item;
                    result.Activate();
                }
            }
            return result;
        }

        public void Prefill(T prefab)
        {
            if(Size == 0)
            {
                return;
            }

            for (int i = 0; i < Size; i++)
            {
                if (prefab)
                {
                    AddToPool(prefab);
                }
            }
        }

    }
}

using UnityEngine;
using Utils;
using Core;
using Core.Interfaces;

namespace Game
{
    public class Main : MonoBehaviour
    {

        private PoolManager<GameItem> poolManager;
        private EventChannels<IListener> eventChannel;

        public PoolManager<GameItem> PoolManager
        { 
            get 
            {
                if(poolManager == null)
                {
                    poolManager = new PoolManager<GameItem>();
                }
                
                return poolManager; 
            } 
        }

        public EventChannels<IListener> EventChannel
        {
            get
            {
                if (eventChannel == null)
                {
                    eventChannel = new EventChannels<IListener>();
                }

                return eventChannel;
            }
        }

        void Awake()
        {
            InitGame();
        }

        void InitGame()
        {
            var path = "Prefabs/Enemy";
            var patternItem = Resources.Load(path) as GameObject;
            
            if (!patternItem)
            {
                Debug.LogError("Prefab " + path + " not found");
                return;
            }

            PoolInfo<GameItem> info = new();
            info.capacity = 20;

            var prefab = patternItem.GetComponent<GameItem>();
            info.prefab = prefab;

            info.prefabName = patternItem.name;

            var pool = PoolManager.GetOrCreatePool(info);
        }
    }
}

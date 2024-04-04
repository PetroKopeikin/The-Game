using UnityEngine;
using Utils;
using Core;
using Core.Interfaces;

namespace Game
{
    public class Main : MonoBehaviour
    {
        public static Main Instance;

        public SimpleDI container;

        private PoolManager<GameItem> poolManager;

        private EventChannels<IListener> eventChannel;

        private GameManager gameManager;


        public PoolManager<GameItem> PoolManager
        {
            get
            {
                if (poolManager == null)
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

        public GameManager GameManager 
        {
            get 
            {
                if(gameManager == null)
                {
                    gameManager = new GameManager();
                }
                return gameManager; 
            } 
        }

        void Awake()
        {

            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            { 
                Destroy(gameObject);
            }

            this.GetContainer().BuildDependencies();


            InitGame();
        }

        void InitGame()
        {

            GameManager.Init();
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

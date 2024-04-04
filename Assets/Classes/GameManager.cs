using Core;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Game
{
    [Dependency(typeof(GameManager))]
    public class GameManager
    {
        private SimpleActionProvider<SaveGameData> actionProvider;
        private SaveGameData saveGameData;
        public void Init()
        {
            saveGameData = new SaveGameData();
            this.GetContainer().AddDependencySingle<SimpleActionProvider<SaveGameData>>();
            actionProvider = (SimpleActionProvider<SaveGameData>)this.GetContainer().Resolve(typeof(SimpleActionProvider<SaveGameData>));

            actionProvider.AddAction("OnGameChanged", OnGameChanged);

            LoadGame();
        }

        public void LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/MySaveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
                SaveGameData data = (SaveGameData)bf.Deserialize(file);
                file.Close();

                saveGameData.jarrowsNumber = data.jarrowsNumber;
                saveGameData.fruits = data.fruits;

                Debug.LogError("Game Data Loaded!");
            }
            else
            {
                Debug.LogError("There is no save data!");
            }
                
        }

        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
            SaveGameData data = new SaveGameData();

            data.jarrowsNumber = saveGameData.jarrowsNumber;
            data.fruits = saveGameData.fruits;

            bf.Serialize(file, data);
            file.Close();
        }

        public void OnGameChanged(SaveGameData gameData)
        {
            var data = gameData as SaveGameData;
            data.fruits = saveGameData.fruits;
            data.jarrowsNumber = saveGameData.jarrowsNumber;
        }


    }
}

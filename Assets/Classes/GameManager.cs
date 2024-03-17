using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Game
{
    public class GameManager
    {

        private SaveGameData saveGameData;
        private SimpleActionProvider<SaveGameData> actionsProvider;
        public void Init()
        {
            saveGameData = new SaveGameData();
            actionsProvider = new SimpleActionProvider<SaveGameData>();
            LoadGame();
            this.GetActionProvider<SaveGameData>().AddAction("OnGameChanged", OnGameChanged);
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

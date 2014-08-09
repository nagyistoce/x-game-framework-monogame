using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    class GameObjectManager
    {
        // Singleton Instance
        static GameObjectManager instance;

        // GameObject List
        List<GameObject> lGameObjects;

        #region Public Attributes

        // Singleton Instance
        public static GameObjectManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObjectManager();
                }

                return instance;
            }
        }

        // GameObject List
        public List<GameObject> ObjectList
        {
            get { return lGameObjects; }
        }

        // Number of objects
        public int NumberOfObjects
        {
            get { return lGameObjects.Count; }
        }

        #endregion

        public GameObjectManager()
        {
            lGameObjects = new List<GameObject>();
        }

        // Add a gameobject to the list
        public void AddObject(GameObject gameObject)
        {
            lGameObjects.Add(gameObject);
        }

        // Remove a gameobject with a certain id
        public void RemoveObject(int id)
        {
            if (lGameObjects.Count > id)
            {
                lGameObjects.RemoveAt(id);
            }
        }

        // Removes a gameobject that matches the one given
        public void RemoveObject(GameObject gameObject)
        {
            if (lGameObjects.Contains(gameObject))
            {
                lGameObjects.Remove(gameObject);
            }
        }

        // Removes all objects from the list
        public void RemoveAllObjects()
        {
            if (lGameObjects.Count > 0)
            {
                for (int i = lGameObjects.Count - 1; i >= 0; i--)
                {
                    lGameObjects.RemoveAt(i);
                }
            }
        }

        // Gets an object at an id
        public GameObject GetObject(int id)
        {
            if (lGameObjects.Count > id)
            {
                return lGameObjects[id];
            }

            return null;
        }

        // Removes objects that are due to be deleted
        public void RemoveDeletedObjects()
        {
            if (lGameObjects.Count > 0)
            {
                for (int i = lGameObjects.Count - 1; i >= 0; i--)
                {
                    if (lGameObjects[i].ToBeDeleted)
                    {
                        lGameObjects.RemoveAt(i);
                    }
                }
            }
        }

        // Loads all gameobjects
        public void Load()
        {
            for (int i = lGameObjects.Count - 1; i >= 0; i--)
            {
                lGameObjects[i].Load();
            }
        }

        // Update
        public void Update()
        {
            for (int i = lGameObjects.Count - 1; i >= 0; i--)
            {
                if (lGameObjects[i].Active)
                {
                    lGameObjects[i].Update();
                }
            }

            // Remove any objects that need to be deleted
            RemoveDeletedObjects();
        }

        // Draw
        public void Draw()
        {
            for (int i = lGameObjects.Count - 1; i >= 0; i--)
            {
                lGameObjects[i].Draw();
            }
        }
    }
}

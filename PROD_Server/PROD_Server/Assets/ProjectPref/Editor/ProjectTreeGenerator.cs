using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;



namespace ProjectTreeGenerator {
    class Folder {
        public string DirPath { get; private set; }
        public string ParentPath { get; private set; }
        public string Name;
        public List<Folder> SubFolders;

        public Folder Add(string name) {

            Folder subFolder = null;
            if (ParentPath.Length > 0)
                subFolder = new Folder(name, ParentPath + Path.DirectorySeparatorChar + Name);
            else
                subFolder = new Folder(name, Name);


            SubFolders.Add(subFolder);
            return subFolder;

        }


        public Folder(string name, string dirPath) {


            Name = name;
            ParentPath = dirPath;
            DirPath = ParentPath + Path.DirectorySeparatorChar + Name;
            SubFolders = new List<Folder>();

        }
    }


    public class CreateProjectTree {

        [MenuItem("Tools/Project/Generate Project Tree")]
        public static void Execute() {
            var assets = GenerateFolderStructure();
            CreateFolders(assets);
        }



        private static void CreateFolders(Folder rootFolder) {

            if (!AssetDatabase.IsValidFolder(rootFolder.DirPath)) {

                Debug.Log("Creating: <b>" + rootFolder.DirPath + "</b>");
                AssetDatabase.CreateFolder(rootFolder.ParentPath, rootFolder.Name);
                File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".keep");

            } else {

                if (Directory.GetFiles(Directory.GetCurrentDirectory() + Path.AltDirectorySeparatorChar + rootFolder.DirPath).Length < 1) {

                    File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".keep");
                    Debug.Log("Creating '.keep' file in: <b>" + rootFolder.DirPath + "</b>");

                } else {
                    Debug.Log("Directory <b>" + rootFolder.DirPath + "</b> already exists");
                }
            }

            foreach (var folder in rootFolder.SubFolders) {
                CreateFolders(folder);
            }
        }

        private static Folder GenerateFolderStructure() {
            Folder rootFolder = new Folder("Assets", "");

            rootFolder.Add("Plugins");
            rootFolder.Add("Data");

            var ScriptAssets = rootFolder.Add("Scripts");
            ScriptAssets.Add("Attributes");
            ScriptAssets.Add("GameLogic");
            ScriptAssets.Add("Enums");
            ScriptAssets.Add("ScriptableObjects");
            ScriptAssets.Add("Utility");

            rootFolder.Add("Scenes");

            var Prefabs = rootFolder.Add("Prefabs");
            Prefabs.Add("Resources");

            var staticAssets = rootFolder.Add("Art");
            staticAssets.Add("Models");
            staticAssets.Add("Animations");
            staticAssets.Add("Animators");
            staticAssets.Add("Effects");
            staticAssets.Add("Fonts");
            staticAssets.Add("Materials");
            staticAssets.Add("Shaders");
            staticAssets.Add("Audio");
            staticAssets.Add("Sprites");
            staticAssets.Add("Textures");
            staticAssets.Add("Videos");

            staticAssets.SubFolders.Find(f => f.Name == "Audio").Add("Sounds");
            staticAssets.SubFolders.Find(f => f.Name == "Audio").Add("Music");
            staticAssets.SubFolders.Find(f => f.Name == "Audio").Add("Ambients");

            return rootFolder;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace ProjectTreeGenerator
{
    class Folder
    {
        public string DirPath { get; private set; }
        public string ParentPath { get; private set; }
        public string Name;
        public List<Folder> Subfolders;

        public Folder Add(string name)
        {
            Folder subfolder = null;
            if (ParentPath.Length > 0)
                subfolder = new Folder(name, ParentPath + Path.DirectorySeparatorChar + Name);
            else
                subfolder = new Folder(name, Name);

            Subfolders.Add(subfolder);
            return subfolder;
        }

        public Folder(string name, string dirPath)
        {
            Name = name;
            ParentPath = dirPath;
            DirPath = ParentPath + Path.DirectorySeparatorChar + Name;
            Subfolders = new List<Folder>();
        }
    }

    public class CreateProjectTree
    {

        [MenuItem("Tools/Generate Project Tree")]
        public static void Execute()
        {
            var assets = GenerateFolderStructure();
            CreateFolders(assets);
        }

        private static void CreateFolders(Folder rootFolder)
        {
            if (!AssetDatabase.IsValidFolder(rootFolder.DirPath))
            {
                Debug.Log("Creating: <b>" + rootFolder.DirPath + "</b>");
                AssetDatabase.CreateFolder(rootFolder.ParentPath, rootFolder.Name);
                File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".keep");
            }
            else
            {
                if (Directory.GetFiles(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath).Length < 1)
                {
                    File.Create(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + rootFolder.DirPath + Path.DirectorySeparatorChar + ".keep");
                    Debug.Log("Creating '.keep' file in: <b>" + rootFolder.DirPath + "</b>");
                }
                else
                {
                    Debug.Log("Directory <b>" + rootFolder.DirPath + "</b> already exists");
                }
            }

            foreach (var folder in rootFolder.Subfolders)
            {
                CreateFolders(folder);
            }
        }

        private static Folder GenerateFolderStructure()
        {
            Folder rootFolder = new Folder("Assets", "");
            rootFolder.Add("Scripts");
            rootFolder.Add("Scenes");
            rootFolder.Add("Extensions");
            rootFolder.Add("Resources");
            rootFolder.Add("Plugins");

            var staticAssets = rootFolder.Add("StaticAssets");
            staticAssets.Add("Animations");
            staticAssets.Add("Animators");
            staticAssets.Add("Effects");
            staticAssets.Add("Fonts");
            staticAssets.Add("Materials");
            staticAssets.Add("Models");
            staticAssets.Add("Prefabs");
            staticAssets.Add("Shaders");
            staticAssets.Add("Sounds");
            staticAssets.Add("Sprites");
            staticAssets.Add("Textures");
            staticAssets.Add("Videos");

            return rootFolder;
        }
    }
}
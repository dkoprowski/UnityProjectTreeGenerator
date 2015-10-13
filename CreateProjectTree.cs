using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
class MyFolderStruct
{
    public string Name;
    public List<MyFolderStruct> Children;
    public MyFolderStruct Parent;
    public string Path { get { return GeneratePath(); } }
    public MyFolderStruct(string name, MyFolderStruct parent)
    {
        Name = name;
        Children = new List<MyFolderStruct>();
        Parent = parent;
    }

    public MyFolderStruct()
    {
        Name = "";
        Children = new List<MyFolderStruct>();
    }

    private string GeneratePath()
    {
        if (Parent != null)
        {
            return Parent.Path + "/" + Name;
        }

        return Name;
    }
}

public class CreateProjectTree {

    [MenuItem("Tools/Generate Project Tree")]
    public static void Execute()
    {
        var assets = GenerateDirectory();
        CreateFolders(assets);
    }
    //TODO: Secure against cycles. 
    private static void CreateFolders(MyFolderStruct folders)
    {
        foreach (var folder in folders.Children)
        {
            if (!AssetDatabase.IsValidFolder(folder.Path) && folder.Parent != null)
            {
                Debug.Log("Creating: <b>" + folder.Path + "</b>");
                AssetDatabase.CreateFolder(folder.Parent.Path, folder.Name);
                File.Create(Directory.GetCurrentDirectory() + "\\" + folder.Path + "\\.keep");
            }
            if (AssetDatabase.IsValidFolder(folder.Path))
            {
                if (Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" + folder.Path).Length < 1)
                {
                    File.Create(Directory.GetCurrentDirectory() + "\\" + folder.Path + "\\.keep");
                    Debug.Log("Creating '.keep' file in: <b>" + folder.Path + "</b>");

                }
            }
            else
            {
                Debug.Log("Creating aborted: " + folder.Path);
            }

            if (folder.Children.Count > 0)
            {
                CreateFolders(folder);
            }
        }
    }
    private static MyFolderStruct GenerateDirectory()
    {
        MyFolderStruct assets = new MyFolderStruct() { Name = "Assets"};
        assets.Children.Add(new MyFolderStruct("Scripts", assets));
        assets.Children.Add(new MyFolderStruct("Scenes", assets));
        assets.Children.Add(new MyFolderStruct("Extensions", assets));
        assets.Children.Add(new MyFolderStruct("Resources", assets));
        
        var staticAssets = new MyFolderStruct("StaticAssets", assets);
        staticAssets.Children.Add(new MyFolderStruct("Animations", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Animators", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Effects", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Fonts", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Materials", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Models", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Prefabs", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Shaders", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Sounds", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Sprites", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Textures", staticAssets));
        staticAssets.Children.Add(new MyFolderStruct("Videos", staticAssets));

        assets.Children.Add(staticAssets);
        return assets;
    }
}

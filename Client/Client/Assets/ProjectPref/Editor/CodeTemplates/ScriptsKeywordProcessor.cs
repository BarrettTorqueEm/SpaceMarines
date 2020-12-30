// - New script keyword processor for Unity 3D by Sarper Soher       -
// - http://www.sarpersoher.com                                      -
// -------------------------------------------------------------------
// - This script changes the keywords in a newly created script with -
// - the values below                                                -
// - Special thanks to hpjohn - http://bit.ly/1N4dd1C      
// -------------------------------------------------------------------
// - Extended by Dimitry "Pixeye" Mitrofanov
// - http://www.hbrew.store/
// -------------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor {
    // UNITY DOCS: This is called by Unity when it is about to create an asset not imported by the user, eg. ".meta" files.
    public static void OnWillCreateAsset(string path) {
        // The path looks like this when created "Assets/ExampleScript.cs.meta"
        // So our first job is to remove the ".meta " part from the path
        path = path.Replace(".meta", "");

        // Find the index of '.' before extension, in what index the extension starts?
        var index = path.LastIndexOf(".");
        // If it does not contain a '.' character after removing the ".meta", return, it's not what we are looking for
        if (index == -1) return;

        // Get the substring after '.' using the above extension index (get file extension)
        var file = path.Substring(index);

        // Now check the extension we have to determine if it's a script file, if not, do nothing
        if (file != ".cs" && file != ".js" && file != ".boo") return;

        // "Application.dataPath" gives us "<path to project folder>/Assets"
        // We find the start index of the "Assets" folder, we will use it to get the full name of the script file we've created
        index = Application.dataPath.LastIndexOf("Assets");

        // Get the absolute path to the created script so we can feed it into a ReadAllText (see the next code line)
        // Before this, the path is "Assets/ExampleScript.cs"
        // It becomes, "DRIVE LETTER:/Projects/YourProject/src/Assets/ExampleScript.cs" in my case, i.e. becomes absolute
        path = Application.dataPath.Substring(0, index) + path;

        // Read all the text the script contains into a string
        // MSDN: Opens a text file, reads all lines of the file, and then closes the file.
        file = System.IO.File.ReadAllText(path);


        string codeKeysPath = Path.GetFullPath(@"Assets/ProjectPref/Editor/CodeTemplates/Settings/TemplateKeys.txt");
        List<CodeKeys> codeKeys = new List<CodeKeys>();
        if (File.Exists(codeKeysPath)) {
            using (var t = new StreamReader(codeKeysPath)) {
                int i = 0;

                string id = string.Empty;
                string contents = string.Empty;

                while (!t.EndOfStream) {
                    string raw = t.ReadLine();

                    i++;
                    if (i % 2 == 1) {
                        id = raw;
                    } else {
                        contents = raw;
                        codeKeys.Add(new CodeKeys(id, contents));

                    }
                }
            }
        }


        for (int i = 0; i < codeKeys.Count; i++) {
            file = UpdateFile(file, codeKeys[i]);
        }
        System.IO.File.WriteAllText(path, file);

        // Refresh the unity asset database to trigger a compilation of our changes on the script
        AssetDatabase.Refresh();
    }

    // You may add other special tags 
    public static string UpdateFile(string file, CodeKeys k) {

        string contentRaw = string.Empty;
        contentRaw = k.content.Replace("[Time]", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        contentRaw = contentRaw.Replace("[PlayerSettings.productName]", PlayerSettings.productName);
        contentRaw = contentRaw.Replace("[PlayerSettings.companyName]", PlayerSettings.companyName);
        contentRaw = contentRaw.Replace("[PlayerSettings.version]", Application.version);

        file = file.Replace(k.id, contentRaw);
        return file;
    }


    public struct CodeKeys {
        public string id;
        public string content;
        public CodeKeys(string id, string content) {
            this.id = id;
            this.content = content;
        }
    }


}

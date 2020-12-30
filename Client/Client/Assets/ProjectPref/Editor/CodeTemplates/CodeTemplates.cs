using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Homebrew
{

  public class CreateCodeFile: EndNameEditAction
  {
    public override void Action(int instanceId,string pathName,string resourceFile)
    {
      Object o = CodeTemplates.CreateScript (pathName,resourceFile);
      ProjectWindowUtil.ShowCreatedAsset (o);
    }
  }



  public class CodeTemplates
  {

    private static Texture2D scriptIcon = (EditorGUIUtility.IconContent ("cs Script Icon").image as Texture2D);

    

    internal static UnityEngine.Object CreateScript(string pathName, string templatePath)
    {
      string filePath = Path.GetFullPath (pathName);
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension (pathName);
      string className = fileNameWithoutExtension.Replace (" ",string.Empty);
      string templateContents = string.Empty;

      if (File.Exists (templatePath))
      {
        using (var t = new StreamReader (templatePath))
        {
          t.ReadLine ();
          templateContents=t.ReadToEnd ();
        }

        templateContents=templateContents.Replace ("##NAME##",className);


       

        UTF8Encoding encoding = new UTF8Encoding (true,false);

        using (var tc = new StreamWriter (filePath,false,encoding))
        {
          tc.Write (templateContents);
        }

        AssetDatabase.ImportAsset (pathName);

        

      }
      return AssetDatabase.LoadAssetAtPath (pathName,typeof (Object));
    }

    public static void CreateFromTemplate(string initialName,string templatePath)
    {
      ProjectWindowUtil.StartNameEditingIfProjectWindowExists (
        0,
        ScriptableObject.CreateInstance<CreateCodeFile> (),
        initialName,
        scriptIcon,
        templatePath);
    }

  }

}

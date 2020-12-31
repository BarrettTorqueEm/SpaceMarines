using UnityEditor;
using UnityEngine;


namespace Homebrew
{

  public class CodeTemplatesMenuItems
  {

    private const string MENU_ITEM_PATH = "Assets/Create/";
    private const int MENU_ITEM_PRIORITY = 70;

      [MenuItem (MENU_ITEM_PATH+"Scripts/Class",false,MENU_ITEM_PRIORITY)]
    private static void CreateClass()
    {
      CodeTemplates.CreateFromTemplate (
          "Class.cs",
  @"Assets/ProjectPref/Editor/CodeTemplates/Templates/Class.txt");

    }  [MenuItem (MENU_ITEM_PATH+"Scripts/MonoBehaviour",false,MENU_ITEM_PRIORITY)]
    private static void CreateMonoBehaviour()
    {
      CodeTemplates.CreateFromTemplate (
          "MonoBehaviour.cs",
  @"Assets/ProjectPref/Editor/CodeTemplates/Templates/MonoBehaviour.txt");

    }

  }

}
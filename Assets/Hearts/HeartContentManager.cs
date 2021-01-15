using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Wrapper for named redirectors to assets in the editor
[System.Serializable]
public class NamedAsset<AssetType>
{
  public string Name;
  public AssetType Asset;
}
// Wrapper for searchable lists of redirectors
[System.Serializable]
public class SearchableList<ItemType, AssetType> where ItemType : NamedAsset<AssetType>
{
  public List<ItemType> List;

  public AssetType Find(string AssetName)
  {
    // Linear search
    foreach (var item in List)
      if (item.Name == AssetName)
        return item.Asset;
    // Not found
    return default(AssetType);
  }
}
[System.Serializable]
public class NamedFont : NamedAsset<Font> { }
[System.Serializable]
public class NamedSprite : NamedAsset<Sprite> { }
[System.Serializable]
public class NamedFontList : SearchableList<NamedFont, Font> { }
[System.Serializable]
public class NamedSpriteList : SearchableList<NamedSprite, Sprite> { }

public class HeartContentManager : MonoBehaviour
{
  // The source for all the blurbs
  [SerializeField]
  TextAsset BlurbsFile;
  
  // The various fonts allowed for the blurbs
  public NamedFontList Fonts = new NamedFontList();

  // The various images allowed for the blurbs
  public NamedSpriteList Sprites = new NamedSpriteList();

  // The blurbs loaded from the file
  List<string> Blurbs = new List<string>();

  public string GetRandomBlurb()
  {
    CheckInitialized();
    if (Blurbs.Count < 1) return "";
    return Blurbs[Random.Range(0, Blurbs.Count)];
  }

  // Use this for initialization
  void Start()
  {
    CheckInitialized();
  }

  bool Initialized = false;
  void CheckInitialized()
  {
    if (Initialized) return;
    Initialized = true;

    // Check if we have a file
    if (BlurbsFile == null) return;

    // Get the blurbs from the file
    string text = BlurbsFile.text;
    while (text.Length > 0)
    {
      // Pop a line from the text
      int lineLength = text.IndexOf('\n');
      int removeLength = lineLength + 1; // If not found, removeLength will be 0
      if (lineLength == -1) removeLength = lineLength = text.Length;
      string line = text.Substring(0, lineLength);
      text = text.Remove(0, removeLength);

      // Check if the line is a comment
      if (line.StartsWith("//")) continue;
      Blurbs.Add(line);
    }
  }
}

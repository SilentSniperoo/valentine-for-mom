using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HeartContent : MonoBehaviour
{
  HeartContentManager Manager;

  public List<Font> RandomizedFonts = new List<Font>();
  public List<Sprite> RandomizedForegrounds = new List<Sprite>();

  // Use this for initialization
  void Start()
  {
    Manager = FindObjectOfType<HeartContentManager>();
    if (Manager == null) return;

    // Randomize the font
    if (RandomizedFonts.Count > 0)
    {
      int randomIndex = Random.Range(0, RandomizedFonts.Count);
      Font randomFont = RandomizedFonts[randomIndex];
      if (randomFont != null) SetFont(randomFont);
    }
    // Randomize the foreground
    if (RandomizedForegrounds.Count > 0)
    {
      int randomIndex = Random.Range(0, RandomizedForegrounds.Count);
      Sprite randomForeground = RandomizedForegrounds[randomIndex];
      if (randomForeground != null) SetForeground(randomForeground);
    }

    // Get a random set of parameters (may or may not set font and background)
    string blurb = Manager.GetRandomBlurb();
    ParseBlurb(blurb);
  }

  // Render images and set text based on the blurb
  void ParseBlurb(string blurb)
  {
    // Find any tags
    int tagStart = blurb.IndexOf('<');
    while (tagStart != -1)
    {
      // Find the end of the tag
      int tagEnd = blurb.IndexOf('>');
      if (tagEnd == -1) return; // Error in formatting of blurb

      // Use the tag
      ParseTag(blurb.Substring(tagStart + 1, tagEnd - tagStart - 1));

      // Remove the tag from the blurb
      blurb = blurb.Remove(tagStart, tagEnd - tagStart + 1);

      // Refresh whether we have a tag
      tagStart = blurb.IndexOf('<');
    }

    Text text = GetComponent<Text>();
    text.text = blurb;
  }
  // Render an image or set the font
  void ParseTag(string tag)
  {
    List<string> list = ParseList(tag);
    // <Font,Courier>
    //  Forces the text to use the font named "Courier"
    if (tag.StartsWith("Font") && list.Count == 2)
    {
      // Find the font from the redirector list
      string name = list[1];
      Font font = Manager.Fonts.Find(name);
      if (font == null) return;

      // Set the font
      SetFont(font);
    }
    // <Background,Heart>
    //  Renders the image named "Heart" as the background
    else if (tag.StartsWith("Background") && list.Count == 2)
    {
      // Find the texture from the redirector list
      string name = list[1];
      Sprite sprite = Manager.Sprites.Find(name);
      if (sprite == null) return;

      // Set the background
      SetBackground(sprite);
    }
    // <Image,"Heart",23,45,67,89>
    //  Renders the image named "Heart" with min x=23, y=45 and max x=67, y=89
    else if (tag.StartsWith("Image") && list.Count == 6)
    {
      // Find the texture from the redirector list
      string name = list[1];
      Sprite sprite = Manager.Sprites.Find(name);
      if (sprite == null) return;

      // Find the transform data
      float minx = float.Parse(list[2]);
      float miny = float.Parse(list[3]);
      float maxx = float.Parse(list[4]);
      float maxy = float.Parse(list[5]);

      // Render the image on top of us
      GameObject obj = new GameObject("BlurbImage");
      obj.transform.SetParent(transform);
      obj.transform.SetAsFirstSibling();
      Image image = obj.AddComponent<Image>();
      image.sprite = sprite;
      image.rectTransform.anchorMin = new Vector2(minx, miny);
      image.rectTransform.anchorMax = new Vector2(maxx, maxy);
      image.rectTransform.anchoredPosition = Vector2.zero;
      image.rectTransform.offsetMin = Vector2.zero;
      image.rectTransform.offsetMax = Vector2.zero;
      image.preserveAspect = true;
    }
  }
  List<string> ParseList(string list)
  {
    // Create a list to return
    List<string> result = new List<string>();

    // Separate all the elements by commas
    int comma = list.IndexOf(',');
    while (comma != -1)
    {
      // Add the element to the temporary list
      result.Add(list.Substring(0, comma));

      // Remove the element
      list = list.Remove(0, comma + 1);

      // Refresh whether we have another element
      comma = list.IndexOf(',');
    }

    // Add the final element (no comma after it)
    result.Add(list);
    return result;
  }
  void SetFont(Font font)
  {
    Text text = GetComponent<Text>();
    text.font = font;
  }
  void SetForeground(Sprite sprite)
  {
    Image image = GetComponentInChildren<Image>();
    if (image == null) return;
    image.sprite = sprite;
  }
  void SetBackground(Sprite sprite)
  {
    if (transform.parent == null) return;
    Image image = transform.parent.GetComponent<Image>();
    if (image == null) return;
    image.sprite = sprite;
  }
}

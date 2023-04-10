using UnityEngine;


public enum ColorMode
{
    Single,
    HorizontalGradient,
    VerticalGradient,
    FourCornersGradient
}

//[CreateAssetMenu(fileName = "ColorGradient", menuName = "Setting/ColorGradient", order = 2)]
[System.Serializable][ExcludeFromPresetAttribute]
public class ColorGradient : ScriptableObject
{
    public ColorMode colorMode = ColorMode.FourCornersGradient;

    public Color topLeft;
    public Color topRight;
    public Color bottomLeft;
    public Color bottomRight;

    const ColorMode k_DefaultColorMode = ColorMode.FourCornersGradient;
    static readonly Color k_DefaultColor = Color.white;

    public ColorGradient()
    {
        colorMode = k_DefaultColorMode;
        topLeft = k_DefaultColor;
        topRight = k_DefaultColor;
        bottomLeft = k_DefaultColor;
        bottomRight = k_DefaultColor;
    }

    public ColorGradient(Color color)
    {
        colorMode = k_DefaultColorMode;
        topLeft = color;
        topRight = color;
        bottomLeft = color;
        bottomRight = color;
    }

    public ColorGradient(Color color0, Color color1, Color color2, Color color3)
    {
        colorMode = k_DefaultColorMode;
        topLeft = color0;
        topRight = color1;
        bottomLeft = color2;
        bottomRight = color3;
    }
}

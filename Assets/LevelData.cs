using UnityEngine;

[CreateAssetMenu(fileName = "NovaTema", menuName = "Idiomy/TemaLevelov")]
public class LevelData : ScriptableObject
{
    public string nazovTemy; // Názov, ktorý sa zobrazí v Dropdowne

    [Header("Ink Súbory")]
    public TextAsset stage1_Ink;
    public TextAsset stage2_Ink;
    public TextAsset stage3_Ink;
    public TextAsset stage4_Ink;
    public TextAsset stage5_Ink;
    public TextAsset stage6_Ink;
    public TextAsset stage7_Ink;
    public TextAsset stage8_Ink;
    public TextAsset stage9_Ink;
}

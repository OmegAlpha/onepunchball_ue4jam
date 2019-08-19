using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinsData", menuName = "OPB/SkinsData", order = 1)]
public class OPB_PlayerSkins_Data : ScriptableObject
{
    public List<Mesh> Meshes_ArmorHead;
    public List<Mesh> Meshes_ArmorHips;

    public List<Color> ColorsPalette;

    public List<Material> SkinMaterialColors;
}

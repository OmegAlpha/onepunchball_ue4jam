
//TODO: do a proper class, encapsulating the playerpref or remote save/load, etc etc 
public static class OPB_LocalUserInfo
{
    public static string UserName;
    public static int index_Mesh_ArmorHead;
    public static int index_Mesh_ArmorHead_Color;
    
    public static int index_Mesh_ArmorHips;
    public static int index_Mesh_ArmorHips_Color;
    
    public static int index_BodySkin_Color;

    public static string GetSkinString()
    {
        return index_Mesh_ArmorHead + "|" + index_Mesh_ArmorHead_Color + "|" +
               index_Mesh_ArmorHips + "|" + index_Mesh_ArmorHips_Color + "|" +
               index_BodySkin_Color;

    }

}
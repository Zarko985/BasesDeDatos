using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    //Datos publicos que se van a guardar en la Base de datos del JSON 
    public string Name = "ammoName";
    public int dmg = 7;
    public string Velociad = "15 M/S";
   

    public JObject Serialize()
    {
        string jsonString = JsonUtility.ToJson(this);
        JObject retVal = JObject.Parse(jsonString);
        return retVal;
    }

    public void Deserialize(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
    }
}

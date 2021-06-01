using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    //Creación del Array
    public Ammo[] municion;
    void Update()
    {

        #region Guardado
        //Al pulsar "S", se guardan los datos
        if (Input.GetKeyDown(KeyCode.S))
        {
            JObject jSaveGame = new JObject();

           //Lee y guarda la informacion en el Array
            for (int i = 0; i < municion.Length; i++)
            {
                Ammo addAmmo = municion[i];
                JObject serializedEnemy = addAmmo.Serialize();
                jSaveGame.Add(addAmmo.name, serializedEnemy);
                

            }
            Debug.Log("Saving file");

            //Direccion de guardado
            string filePath = Application.persistentDataPath + "/saveAmmo.sav";

            //Al guardar la informacion la encripta
            byte[] encryptedMessage = Encrypt(jSaveGame.ToString());
            File.WriteAllBytes(filePath, encryptedMessage);
        }
        #endregion

        #region Carga
        //Al pulsar "L", carga la informacion
        if (Input.GetKeyDown(KeyCode.L))
        {
            //Direccion de donde Carga la informacion
            string filePath = Application.persistentDataPath + "/saveAmmo.sav";
            Debug.Log("Loading from: " + filePath);

            //Desencripta la informacion
            byte[] decryptedMessage = File.ReadAllBytes(filePath);
            string jsonString = Decrypt(decryptedMessage);


            JObject jSaveGame = JObject.Parse(jsonString);

            //Lee la informacion y la extrae
            for (int i = 0; i < municion.Length; i++)
            {
                Ammo addAmmo = municion[i];
                string AmmoJsonString = jSaveGame[addAmmo.name].ToString();
                addAmmo.Deserialize(AmmoJsonString);
            }
            //Muestra la informacion guardada
            Debug.Log(jSaveGame);


        }
        #endregion
    }

    #region Encriptado
    //Encriptacion de Seguridad
    byte[] _key = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
    byte[] _inicializationVector = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

    //Encripta la informacion guardada
    byte[] Encrypt(string message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform encryptor = aes.CreateEncryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        StreamWriter streamWriter = new StreamWriter(cryptoStream);

        streamWriter.WriteLine(message);

        streamWriter.Close();
        cryptoStream.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    //Desencripta la informacion para su extraccion
    string Decrypt(byte[] message)
    {
        AesManaged aes = new AesManaged();
        ICryptoTransform decrypter = aes.CreateDecryptor(_key, _inicializationVector);

        MemoryStream memoryStream = new MemoryStream(message);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decrypter, CryptoStreamMode.Read);
        StreamReader streamReader = new StreamReader(cryptoStream);

        string decryptedMessage = streamReader.ReadToEnd();

        memoryStream.Close();
        cryptoStream.Close();
        streamReader.Close();

        return decryptedMessage;
    }
    #endregion
}

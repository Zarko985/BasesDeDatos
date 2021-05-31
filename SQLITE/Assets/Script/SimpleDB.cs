using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;


public class SimpleDB : MonoBehaviour
{
    // Start is called before the first frame update
    private string dbName = "URI=file:Inventory.db";

    public Text ammoText;
    void Start()
    {
        CreateDB();

       
        DisplayAmmo();

        ammoText.text = "";

    }

    public void CreateDB()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Municion (Nombre VARCHAR(20), Cantidad INT);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public void AddAmmo(string AmmoName, int AmmoAmount)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Municion (Nombre, Cantidad) VALUES ('" + AmmoName + "', '" + AmmoAmount + "');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        DisplayAmmo();
    }

    public void DisplayAmmo()
    {
        ammoText.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Municion;";

                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    
                        ammoText.text += ("Name: " + reader["Nombre"] + "\t\t"+ "Amount: " + reader["Cantidad"] + "\n");
                    
                    reader.Close();
                }

                 
            }

            connection.Close();
        }
    }

    public void Perfrotante()
    {
        AddAmmo("Perforante", 20);
    }

    public void Explosiva()
    {
        AddAmmo("Explosiva", 10);
    }

    public void Incendiaria()
    {
        AddAmmo("Incendiaria", 5);
    }

}

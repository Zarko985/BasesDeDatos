using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;


public class SimpleDB : MonoBehaviour
{
    // Start is called before the first frame update

    //Nombre de la base de datos
    private string dbName = "URI=file:Inventory.db";

    //Texto para mostrar los datos guardados
    public Text ammoText;

    //Activacion de tabla y datos
    void Start()
    {
        CreateDB();

       
        DisplayAmmo();

        ammoText.text = "";

    }

    #region Base de datos
    //Creacion de la base de datos
    public void CreateDB()
    {
        //Conexion con la base de datos
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            //Crea una tabla llamada Municion en caso de que no exista
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS Municion (Nombre VARCHAR(20), Cantidad INT);";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
    #endregion

    #region Añadir municion
    //Metodo para añadir municion
    public void AddAmmo(string AmmoName, int AmmoAmount)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            //Activa un objeto para controlar la Base de datos
            using (var command = connection.CreateCommand())
            {
                //Comando para insertar los valores en la base de datos
                command.CommandText = "INSERT INTO Municion (Nombre, Cantidad) VALUES ('" + AmmoName + "', '" + AmmoAmount + "');";
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
        //Actualiza el inventario
        DisplayAmmo();
    }
    #endregion

    #region Mostrar Municion

    //Muestra las municiones
    public void DisplayAmmo()
    {
        //Limpia el texto
        ammoText.text = "";

        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                //Selecciona los parametros que van a mostarse
                command.CommandText = "SELECT * FROM Municion;";


                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    
                        //Muestra en pantalla los campos nombrados
                        //Muestra tantos como hay guardados
                        ammoText.text += ("Name: " + reader["Nombre"] + "\t\t"+ "Amount: " + reader["Cantidad"] + "\n");
                    
                    reader.Close();
                }

                 
            }

            connection.Close();
        }
    }
    #endregion 

    #region munciones
    //Metodos para la activacion de los botones 
    //para guardar los datos en la base de datos
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
    #endregion
}

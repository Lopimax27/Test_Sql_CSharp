using MySql.Data.MySqlClient;
using System;

public class User
{
    private string _username;

    public string Username
    {
        get { return _username; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Devi inserire un username.");
            }
            _username = value;
        }
    }

    private string _password;
    public string Password
    {
        get { return _password; }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                Console.WriteLine("Devi inserire una password.");
            }
            _password = value;
        }
    }

    private float _saldo;
    public float Saldo
    {
        get { return _saldo; }
        set
        {
            if (value < 0)
            {
                Console.WriteLine("Il saldo non puÃ² essere negativo.");
            }
            _saldo = value;
        }
    }
}
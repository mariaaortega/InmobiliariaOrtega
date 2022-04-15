﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaOrtega.Models
{
    public class RepositorioPago : RepositorioBase
    {
        RepositorioContrato repContrato;

        public RepositorioPago(IConfiguration config) : base(config)
        {
            repContrato = new RepositorioContrato(configuration);
            this.tabla = "Pagos";
            this.columnas = new string[3] { "ContratoId", "Fecha", "Importe" };
        }

        new public Pago ObtenerPorId<T>(int id)
        {
            Pago e = base.ObtenerPorId<Pago>(id);
            e.Contrato = repContrato.ObtenerPorId<Contrato>(e.ContratoId);

            return e;
        }

        new public List<Pago> ObtenerTodos<T>()
        {
            List<Pago> lista = base.ObtenerTodos<Pago>();

            foreach (var e in lista)
            {
                e.Contrato = repContrato.ObtenerPorId<Contrato>(e.ContratoId);
            }

            return lista;
        }

        public List<Pago> ObtenerPorContrato(int ContratoId)
        {
            List<Pago> res = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, ";
                for (int i = 0; i < columnas.Length; i++)
                {
                    if (i == columnas.Length - 1)
                        sql += columnas[i];
                    else
                        sql += $"{columnas[i]}, ";
                }
                sql += $" FROM {tabla} WHERE ContratoId = {ContratoId} ORDER BY Fecha DESC;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago item = new Pago();
                        item.Id = reader.GetInt32(0);
                        item.ContratoId = reader.GetInt32(1);
                        item.Fecha = reader.GetDateTime(2);
                        item.Importe = reader.GetInt32(3);
                        item.Contrato = repContrato.ObtenerPorId_v2(item.ContratoId);

                        res.Add(item);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Dictionary<string, long> ObtenerDatosContrato(int ContratoId)
        {
            Dictionary<string, long> res = new Dictionary<string, long>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT COUNT(p.Id) as CantidadPagos, MAX(p.Fecha) as UltimoPago FROM Contratos c JOIN Pagos p ON p.ContratoId = c.Id WHERE c.Id = {ContratoId}";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add("CantidadPagos", (int)reader["CantidadPagos"]);
                        if (res["CantidadPagos"] != 0)
                        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo para el parámetro "s" en "DateTime DateTime.Parse(string s)".
                            res.Add("UltimoPago", DateTime.Parse(reader["UltimoPago"].ToString()).Ticks);
#pragma warning restore CS8604 // Posible argumento de referencia nulo para el parámetro "s" en "DateTime DateTime.Parse(string s)".
                        } else
                        {
                            res.Add("UltimoPago", 0);
                        }
                        
                    }
                    connection.Close();
                }
            }
            return res;
        }
    }
}

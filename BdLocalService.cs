﻿using ControlSalud.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlSalud
{
    public class BdLocalService
    {
        private const string DB_NAME = "bd.db";
        private readonly SQLiteAsyncConnection _connection;
        private bool _inicializado;

        public BdLocalService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, DB_NAME);
            _connection = new SQLiteAsyncConnection(dbPath);
            _inicializado = false;
        }

        private async Task InitializeAsync()
        {
            if (!_inicializado)
            {
                await _connection.CreateTableAsync<Paciente>();
                _inicializado = true;
            }
        }

        public async Task<List<Paciente>> ObtenerPacientes()
        {
            await InitializeAsync();
            return await _connection.Table<Paciente>().ToListAsync();
        }

        public async Task AgregarPaciente(Paciente paciente)
        {
            await InitializeAsync();
            await _connection.InsertAsync(paciente);
        }

        public async Task ActualizarPaciente(Paciente paciente)
        {
            await InitializeAsync();
            await _connection.UpdateAsync(paciente);
        }
    }
}

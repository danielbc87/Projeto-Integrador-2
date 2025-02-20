using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Health_Organizer
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "health_organizer.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Medication>().Wait();
            _database.CreateTableAsync<Appointment>().Wait();
        }

        // CRUD - Medicamentos
        public Task<int> SaveMedicationAsync(Medication medication) => _database.InsertAsync(medication);
        public Task<List<Medication>> GetMedicationsAsync() => _database.Table<Medication>().ToListAsync();

        // CRUD - Consultas
        public Task<int> SaveAppointmentAsync(Appointment appointment) => _database.InsertAsync(appointment);
        public Task<List<Appointment>> GetAppointmentsAsync() => _database.Table<Appointment>().ToListAsync();

        // Método para excluir um medicamento
        public Task<int> DeleteMedicationAsync(Medication medication)
        {
            return _database.DeleteAsync(medication);
        }

        // Método para excluir uma consulta
        public Task<int> DeleteAppointmentAsync(Appointment appointment)
        {
            return _database.DeleteAsync(appointment);
        }
        public Task<int> UpdateMedicationAsync(Medication medication)
        {
            return _database.UpdateAsync(medication);
        }
        public Task<int> UpdateAppointmentAsync(Appointment appointment)
        {
            return _database.UpdateAsync(appointment);
        }


    }

    // Modelo para Medicamentos
    public class Medication
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
    }

    // Modelo para Consultas
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Doctor { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}

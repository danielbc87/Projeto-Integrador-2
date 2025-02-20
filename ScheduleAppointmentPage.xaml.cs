using Microsoft.Maui.Controls;
using System;

namespace Health_Organizer
{
    public partial class ScheduleAppointmentPage : ContentPage
    {
        private Appointment _currentAppointment;

        // Construtor modificado para aceitar uma consulta existente (opcional)
        public ScheduleAppointmentPage(Appointment appointment = null)
        {
            InitializeComponent();
            _currentAppointment = appointment;

            if (_currentAppointment != null)
            {
                docNameEntry.Text = _currentAppointment.Doctor;
                dateEntry.Text = _currentAppointment.Date;
                timeEntry.Text = _currentAppointment.Time;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validação: verificar se os campos estão preenchidos
            if (string.IsNullOrWhiteSpace(docNameEntry.Text) ||
                string.IsNullOrWhiteSpace(dateEntry.Text) ||
                string.IsNullOrWhiteSpace(timeEntry.Text))
            {
                await DisplayAlert("Erro", "Todos os campos devem ser preenchidos.", "OK");

                // Limpar campos vazios
                docNameEntry.Text = string.IsNullOrWhiteSpace(docNameEntry.Text) ? "" : docNameEntry.Text;
                dateEntry.Text = string.IsNullOrWhiteSpace(dateEntry.Text) ? "" : dateEntry.Text;
                timeEntry.Text = string.IsNullOrWhiteSpace(timeEntry.Text) ? "" : timeEntry.Text;

                return;
            }

            // Validação: verificar se a data inserida é válida
            if (!DateTime.TryParse(dateEntry.Text, out DateTime selectedDate))
            {
                await DisplayAlert("Erro", "Formato de data inválido. Use DD/MM/AAAA.", "OK");

                // Limpar campo incorreto
                dateEntry.Text = "";
                return;
            }

            // Validação: verificar se a data não é no passado
            if (selectedDate < DateTime.Today)
            {
                await DisplayAlert("Erro", "A data da consulta não pode ser no passado.", "OK");

                // Limpar campo incorreto
                dateEntry.Text = "";
                return;
            }

            // Validação: verificar se o horário está no formato HH:MM
            if (!TimeSpan.TryParse(timeEntry.Text, out TimeSpan selectedTime))
            {
                await DisplayAlert("Erro", "Formato de horário inválido. Use HH:MM.", "OK");

                // Limpar campo incorreto
                timeEntry.Text = "";
                return;
            }

            // Criar ou atualizar a consulta
            if (_currentAppointment == null)
            {
                var appointment = new Appointment
                {
                    Doctor = docNameEntry.Text,
                    Date = selectedDate.ToString("dd/MM/yyyy"),
                    Time = selectedTime.ToString(@"hh\:mm")
                };
                await App.Database.SaveAppointmentAsync(appointment);
            }
            else
            {
                _currentAppointment.Doctor = docNameEntry.Text;
                _currentAppointment.Date = selectedDate.ToString("dd/MM/yyyy");
                _currentAppointment.Time = selectedTime.ToString(@"hh\:mm");
                await App.Database.UpdateAppointmentAsync(_currentAppointment);
            }

            await DisplayAlert("Sucesso", "Consulta salva com sucesso!", "OK");
            await Navigation.PopAsync();
        }


    }

}

using Microsoft.Maui.Controls;
using System;

namespace Health_Organizer
{
    public partial class AddMedicationPage : ContentPage
    {
        private Medication _currentMedication;

        public AddMedicationPage(Medication medication = null)
        {
            InitializeComponent();
            _currentMedication = medication;

            if (_currentMedication != null)
            {
                medNameEntry.Text = _currentMedication.Name;
                medTimeEntry.Text = _currentMedication.Time;
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validação: verificar se os campos estão preenchidos
            if (string.IsNullOrWhiteSpace(medNameEntry.Text) ||
                string.IsNullOrWhiteSpace(medTimeEntry.Text))
            {
                await DisplayAlert("Erro", "Todos os campos devem ser preenchidos.", "OK");

                // Limpar campos com erro
                medNameEntry.Text = string.IsNullOrWhiteSpace(medNameEntry.Text) ? "" : medNameEntry.Text;
                medTimeEntry.Text = string.IsNullOrWhiteSpace(medTimeEntry.Text) ? "" : medTimeEntry.Text;

                return;
            }

            // Validação: verificar se o horário está no formato HH:MM
            if (!TimeSpan.TryParse(medTimeEntry.Text, out TimeSpan selectedTime))
            {
                await DisplayAlert("Erro", "Formato de horário inválido. Use HH:MM.", "OK");

                // Limpar campo incorreto
                medTimeEntry.Text = "";
                return;
            }

            // Criar ou atualizar o medicamento
            if (_currentMedication == null)
            {
                var medication = new Medication
                {
                    Name = medNameEntry.Text,
                    Time = selectedTime.ToString(@"hh\:mm") // Formatar corretamente
                };
                await App.Database.SaveMedicationAsync(medication);
            }
            else
            {
                _currentMedication.Name = medNameEntry.Text;
                _currentMedication.Time = selectedTime.ToString(@"hh\:mm");
                await App.Database.UpdateMedicationAsync(_currentMedication);
            }

            await DisplayAlert("Sucesso", "Medicamento salvo com sucesso!", "OK");
            await Navigation.PopAsync();
        }

    }

}


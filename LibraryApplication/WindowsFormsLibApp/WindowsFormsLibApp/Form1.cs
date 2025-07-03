using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LibraryBusiness;
using LibraryConfigUtilities;

namespace LibraryPenaltyUI
{
    public partial class Form1 : Form
    {
        private PenaltyFeeCalculator calculator;
        private List<Country> countries;

        public Form1()
        {
            InitializeComponent();
            calculator = new PenaltyFeeCalculator();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            countries = calculator.GetCountryList();
            comboBoxCountries.DataSource = countries;
            comboBoxCountries.DisplayMember = "CountryCode";
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (comboBoxCountries.SelectedItem is Country selectedCountry)
            {
                DateTime startDate = dateTimePickerStart.Value;
                DateTime endDate = dateTimePickerEnd.Value;

                string result = calculator.Calculate(startDate, endDate, selectedCountry.CountryCode);
                labelResult.Text = $"Ceza Tutarı: {result}";
            }
            else
            {
                labelResult.Text = "Lütfen bir ülke seçin.";
            }
        }
    }
}

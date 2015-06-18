using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace WagerGame
{
    public partial class MainForm1 : Form
    {
        [Serializable()]
        public class Result
        {
            [System.Xml.Serialization.XmlElement("name")]
            public string Name { get; set; }

            [System.Xml.Serialization.XmlElement("value")]
            public string Value { get; set; }

        }

        [Serializable()]
        [System.Xml.Serialization.XmlRoot("ResultsCollection")]
        public class ResultsList
        {
            [XmlArray("Results")]
            [XmlArrayItem("Result", typeof(Result))]
            public Result[] resultsArray { get; set; }
        }
        public MainForm1()
        {
            InitializeComponent();
            ResultsList results = null;
            string path = "PossibleResults.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(ResultsList));

            StreamReader reader = new StreamReader(path);
            results = (ResultsList)serializer.Deserialize(reader);
            reader.Close();
            var resultsList = results.resultsArray.ToList();

            textBox1.Text = textBox1.Text + "Odds List" + Environment.NewLine;
            foreach (var result in resultsList)
            {

                double resultValue = double.Parse(result.Value, NumberStyles.Any, CultureInfo.InvariantCulture);


                double usOdd = 0;
                textBox1.Text = textBox1.Text + "Name: " + result.Name + Environment.NewLine;
                if (resultValue > 50)
                {

                    usOdd = (resultValue / (100 - resultValue)) * 100;
                    textBox1.Text = textBox1.Text + "US: " + "-" + Math.Round(usOdd, 2) + Environment.NewLine;


                }
                else
                {

                    usOdd = ((100 - resultValue) / resultValue) * 100;
                    textBox1.Text = textBox1.Text + "US: " + "+" + Math.Round(usOdd, 1) + Environment.NewLine;

                }
                textBox1.Text = textBox1.Text + "Decimal: " + Math.Round((100 / resultValue), 2) + Environment.NewLine;
                cmbOptions.Items.Add(new ComboboxItem(result.Name, Math.Round((100 / resultValue), 2)));




            }
            cmbOptions.SelectedIndex = 1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                double bet = double.Parse(textBox2.Text);
                if (bet >= 1)
                {
                    ComboboxItem selectedOption = (ComboboxItem)cmbOptions.SelectedItem;
                    MessageBox.Show("Your expected prize is: " + "$" + bet * double.Parse(selectedOption.Value.ToString()));

                }
                else
                { MessageBox.Show("You need to make a bigger bet"); }

            }
            catch
            {

                MessageBox.Show("Please enter a correct number, without signs");

            }
        }
    }
}

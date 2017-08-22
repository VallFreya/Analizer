using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Analizer.Exception;
using Analizer.Models;

namespace Analizer
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            BtnEpsilon.ToolTip = "Эпсилон может быть только один";
        }

        private void AddPr_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstRule.Text) || string.IsNullOrWhiteSpace(SecondRule.Text))
            {
                MessageBox.Show("Недопустиммый ввод, проверьте правильнось ввода правил"); // если поля пустые
            }
            else
            {
                ListRules.Items.Add(FirstRule.Text + "->" + SecondRule.Text);
                FirstRule.Clear();
                SecondRule.Clear();
            }
        }
        
        private void Analizer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Grammar grammar = new Grammar(ListRules.Items.Cast<string>().ToList());
                GrammarType.Content = grammar.Type;
                CountRules.Content = ListRules.Items.Count;
            }
            catch (EmptyRulesException)
            {
                MessageBox.Show("Нет правил");
            }
            catch (EqualsRulesException ex)
            {
                MessageBox.Show($"Правило {ex.Message} имеет одинаковую правую и левую часть");
            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void ClearAllRulesButton_Click(object sender, RoutedEventArgs e)
        {
            ListRules.Items.Clear();
        }

        private void ClearRuleButton_Click(object sender, RoutedEventArgs e)
        {
            int select = ListRules.SelectedIndex;
            if (select == -1)
            {
                MessageBox.Show("Не выбран элемент для удаления", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                ListRules.Items.RemoveAt(ListRules.SelectedIndex);
            }
        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItemAuthor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Серёгина Диана\nГруппа 3Б(121131)", "Автор", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonEpsilon_Click(object sender, RoutedEventArgs e)
        {
            SecondRule.Text = Convert.ToString(BtnEpsilon.Content);
        }

        private void LeftRuleInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = "12345667890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".IndexOf(e.Text, StringComparison.Ordinal) < 0;
        }

        private void RightRuleInput(object sender, TextCompositionEventArgs e)
        {

            e.Handled = "12345667890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".IndexOf(e.Text, StringComparison.Ordinal) < 0;
        }

        private void AddSimbolRight(object sender, RoutedEventArgs e)
        {
            char epsilon = Convert.ToChar(BtnEpsilon.Content);
            string pEpsilon = Convert.ToString(SecondRule.Text);
            int leng = pEpsilon.Length;
            bool bEpsilon = false;
            for (int l = 0; l < leng; l++)
            {
                if (pEpsilon[l] == epsilon)
                {
                    bEpsilon = true;
                }
            }

            if (bEpsilon == false)
            {
                var button = sender as Button;
                if (button != null)
                {
                    String symbol = Convert.ToString(button.Content);
                    String text = SecondRule.Text;
                    text += symbol;
                    SecondRule.Text = text;
                }
            }
        }

        private void AddSimbolLeft(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string symbol = Convert.ToString(button.Content);
                string text = FirstRule.Text;
                text += symbol;
                FirstRule.Text = text;
            }
        }
    }
}
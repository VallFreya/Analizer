using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            int result = 0;   // результат конечный и 4 промежуточных
            if (ListRules.Items.Count == 0) // проверка на наличие правил
            {
                MessageBox.Show("Не найдены правила", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (string stringRule in ListRules.Items)
            {
                /* проверка на А-грамматику */
                (string firstUnitRule, string secontUnitRule) rule = (stringRule.Substring(0, stringRule.IndexOf('-')), stringRule.Substring(stringRule.IndexOf('-') + 2)); //разделяем правило на левое и правое
                var aGrammar = CheckAGrammar(rule);
                if (aGrammar)
                {
                    var resultAGrammar = 1;   //результат конечный и 4 промежуточных
                    if (resultAGrammar > result)
                    {
                        result = resultAGrammar;
                    }

                    // если правило есть A-гр то нет смысла проверять дальше идем к следующему правилу 
                    // TODO добавить вывод результата в конце итерации
                    continue;
                }

                /*
                * проверка на КС-грамматику
                */
                var contextFreeGrammar = CheckContextFreeGrammar(rule);
                if (contextFreeGrammar)
                {
                    var resultContextFreeGrammar = 2;   //результат конечный и 4 промежуточных
                    if (resultContextFreeGrammar > result)
                    {
                        result = resultContextFreeGrammar;
                    }

                    continue;
                }

                /*
                * проверка на КЗ-грамматику
                */
                if (rule.firstUnitRule == rule.secontUnitRule)
                {
                    // выдавать ошибку
                    MessageBox.Show("1 часть правил и 2 одинаковы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                var contextSensitiveGrammar = CheckContextSensitiveGrammar(rule);
                if (contextSensitiveGrammar)
                {
                    var resultContextSensitiveGrammar = 3; //результат конечный и 4 промежуточных
                    if (resultContextSensitiveGrammar > result)
                    {
                        result = resultContextSensitiveGrammar;
                    }
                }
                else
                {
                    var resultZeroGrammar = 4;   //результат конечный и 4 промежуточных
                    if (resultZeroGrammar > result)
                    {
                        result = resultZeroGrammar;
                    }
                }
            }

            label6.Content = "Тип грамматики:";
            label4.Content = "Всего правил: ";
            if (result == 1)
            {
                label14.Content = "A-гр.";
            }

            if (result == 2)
            {
                label14.Content = "КС-гр.";
            }

            if (result == 3)
            {
                label14.Content = "КЗ-гр.";
            }

            if (result == 4)
            {
                label14.Content = "0-гр.";
            }
            CountRules.Content = ListRules.Items.Count;
        }

        private void button_clearAll_Click(object sender, RoutedEventArgs e)
        {
            ListRules.Items.Clear();
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Серёгина Диана\nГруппа 3Б(121131)", "Автор", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SecondRule.Text = Convert.ToString(BtnEpsilon.Content);
        }

        private void Text_input(object sender, TextCompositionEventArgs e)
        {

            e.Handled = "12345667890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".IndexOf(e.Text, StringComparison.Ordinal) < 0;
        }

        private void Text_input_2(object sender, TextCompositionEventArgs e)
        {

            e.Handled = "12345667890QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".IndexOf(e.Text, StringComparison.Ordinal) < 0;
        }

        private void AddSimbol(object sender, RoutedEventArgs e)
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

        private bool CheckAGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length == 1 || rule.secontUnitRule.Length <= 2)
            {
                // размер левой части в А гр всегда равен 1, а второй может быть как 1 так и 2
                if (new Regex(@"[A-Z]").Match(rule.firstUnitRule).Length == 0)
                {
                    // левая часть правила должна быть нетерминальный символом
                    return false;
                }

                if (rule.secontUnitRule.Length == 1)
                {
                    if (new Regex(@"[A-Z]").Match(rule.secontUnitRule).Length != 0)
                    {
                        // если в правой части один символ то он должен быть терминалом
                        return false;
                    }

                    return true; // если размер подходит и это терминал, то это A-гр
                }

                if (new Regex(@"[A-Z]").Matches(rule.secontUnitRule).Count == 1)
                {
                    //обязательно должен быть нетерминал если размер правой части равен 2
                    return true;
                }
            }

            return false;
        }

        private bool CheckContextFreeGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length == 1 && new Regex(@"[A-Z]").Match(rule.firstUnitRule).Length == 1)
            {
                // правая часть всегда равна 1, и там обязательно должен быть нетерминал
                return true;
            }

            return false;
        }

        private bool CheckContextSensitiveGrammar((string firstUnitRule, string secontUnitRule) rule)
        {
            if (rule.firstUnitRule.Length > rule.secontUnitRule.Length) // правая часть должна быть больше или равна левой
            {
                return false;
            }
            
            rule = DeleteContext(rule); // обрезаем контекст у правила

            return CheckContextFreeGrammar(rule); // должно выглядеть как правило КС граммматики
        }

        /// <summary>
        /// Удалить контекст из правила
        /// </summary>
        /// <param name="rule">Правило</param>
        /// <returns>Правило</returns>
        private (string firstUnitRule, string secontUnitRule) DeleteContext((string firstUnitRule, string secontUnitRule) rule)
        {
            for (int i = 0; ; i++) // обрезаем левый контекст
            {
                if (rule.firstUnitRule[i] != rule.secontUnitRule[i])
                {
                    break;
                }

                rule.firstUnitRule = rule.firstUnitRule.Substring(1); // обрезать первый символ
                rule.secontUnitRule = rule.secontUnitRule.Substring(1);
            }

            for (int i = rule.firstUnitRule.Length - 1, k = rule.secontUnitRule.Length - 1; ; i--, k--)
            {
                if (rule.firstUnitRule[i] != rule.secontUnitRule[k])
                {
                    break;
                }

                rule.firstUnitRule = rule.firstUnitRule.Substring(0, rule.firstUnitRule.Length - 1); // обрезать последний символ
                rule.secontUnitRule = rule.secontUnitRule.Substring(0, rule.secontUnitRule.Length - 1);
            }

            return rule;
        }

        private void AddSimbol2(object sender, RoutedEventArgs e)
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
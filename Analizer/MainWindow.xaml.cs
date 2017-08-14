using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections;

namespace Analizer
{
    public partial class MainWindow
    {
        readonly string[] _all = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public MainWindow()
        {
            InitializeComponent();
            BTep.ToolTip = "Эпсилон может быть только один";
        }

        private void AddPr_Click(object sender, RoutedEventArgs e)
        {
            char epsilon = Convert.ToChar(BTep.Content);
            string pEpsilon = Convert.ToString(textBox2.Text);
            int leng = pEpsilon.Length;
            bool bEpsilon = false;
            for (int l = 0; l < leng; l++)
            {
                if (pEpsilon[l] == epsilon)
                {
                    bEpsilon = true;
                }
            }

            if (textBox1.Text == "" || textBox2.Text == "" || bEpsilon && leng > 1)
            {
                MessageBox.Show("Недопустиммый ввод, проверьте правильнось ввода правил");              //если поля пустые
            }
            else
            {
                listBox1.Items.Add(textBox1.Text + "->" + textBox2.Text);
                textBox1.Clear();
                textBox2.Clear();
            }
        }

        private void Analizer_Click(object sender, RoutedEventArgs e)
        {
            var anArrayOfPairedRules = new ArrayList();
            int result = 0;   // результат конечный и 4 промежуточных
            int theNumberOfRules = listBox1.Items.Count; // всего правил в грамматике
            if (theNumberOfRules == 0) // проверка на наличие правил
            {
                MessageBox.Show("Не найдены правила", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            label5.Content = theNumberOfRules; // массив строк для правил
            for (int index = 0; index < theNumberOfRules; index++) // перебираем все правила до конца
            {
                string rule = Convert.ToString(listBox1.Items[index]);
                int separator = 0; //и ндекс разделителя 1 и 2 частей правила 
                int legthRule = rule.Length; // ищем длину строки 
                string theInterimRule = rule; // временная строка для правила
                for (int i = 0; i < legthRule; i++) // находим символ отвечающий за то что 1 часть правила закончилась
                {
                    if (theInterimRule[i] == '-')
                    {
                        separator = i;
                    }
                }

                string firstUnitRule = "";
                string secontUnitRule = "";
                for (int i = 0; i < separator; i++) // создаем 1 часть правила
                {
                    firstUnitRule += theInterimRule[i];
                }

                for (int i = separator + 2; i < legthRule; i++) // создаем 2 часть правила
                {
                    secontUnitRule += theInterimRule[i];
                }

                anArrayOfPairedRules.Add(firstUnitRule);  // добавляем в массив 1 и 2 части правила
                anArrayOfPairedRules.Add(secontUnitRule);
                var leftContext = 0; // обнуляем левый контекст
                var rightContext = 0; // обнуляем правый контекст
                int indexLeftContext = 0; // индексы правого и левого контекстов
                int indexRightContext = 0; // начинаем с 1 если i=0, так как при умножении выйдем за пределы массива
                var legthFirstUnitRule = firstUnitRule.Length - 1; // размер 1 части правила
                var legthSecontUnitRule = secontUnitRule.Length - 1; // размер 2 части правила
                var theLeftEndOfContext = false; // индекс конца контекста слева
                var theRihtEndOfContext = false; // индекс конца контекса справа
                bool errorRule = false;
                var aGrammar = false;
                var contextFreeGrammar = false;
                var contextSensitiveGrammar = false;
                /*
                * проверка на А-грамматику
                 */
                int resultAGrammar;   //результат конечный и 4 промежуточных
                if (legthFirstUnitRule == 0 && legthSecontUnitRule == 1)
                {
                    bool firstSymbol = false;
                    bool secontSymbolFirstNonTerm = false;
                    bool secontSymbolFirstTerm = false;
                    for (int y = 0; y < 26; y++)
                    {
                        char symbol = Convert.ToChar(_all[y]);
                        char symbolGrammar = Convert.ToChar(firstUnitRule[0]);

                        if (symbol == symbolGrammar)
                        {
                            firstSymbol = true;
                        }
                        if (symbol == Convert.ToChar(secontUnitRule[0]))
                        {
                            secontSymbolFirstNonTerm = true;
                        }
                        if (symbol == Convert.ToChar(secontUnitRule[1]))
                        {
                            secontSymbolFirstTerm = true;
                        }
                    }
                    if (firstSymbol && secontSymbolFirstNonTerm && secontSymbolFirstTerm == false || firstSymbol && secontSymbolFirstNonTerm == false && secontSymbolFirstTerm)
                    {
                        aGrammar = true;
                        resultAGrammar = 1;
                        if (resultAGrammar > result)
                        {
                            result = resultAGrammar;
                        }
                    }
                }

                if (legthFirstUnitRule == 0 && legthSecontUnitRule == 0)
                {
                    for (int y = 0; y < 26; y++)
                    {
                        char symbol = Convert.ToChar(_all[y]);
                        char symbolGrammar = Convert.ToChar(firstUnitRule[0]);

                        if (symbol == symbolGrammar)
                        {
                            aGrammar = true;
                            resultAGrammar = 1;
                            if (resultAGrammar > result)
                            {
                                result = resultAGrammar;
                            }
                            break;
                        }

                    }

                }
                /*
                    * проверка на КС-грамматику
                    */
                if (aGrammar == false)
                //если не А грамматика то проверяем дальше
                {
                    if (legthFirstUnitRule == 0 && legthSecontUnitRule >= 1)
                    {
                        for (int y = 0; y < 26; y++)
                        {
                            char symbol = Convert.ToChar(_all[y]);
                            char symbolGrammar = Convert.ToChar(firstUnitRule[0]);

                            if (symbol == symbolGrammar)
                            {
                                contextFreeGrammar = true;
                                var resultContextFreeGrammar = 2;   //результат конечный и 4 промежуточных
                                if (resultContextFreeGrammar > result)
                                {
                                    result = resultContextFreeGrammar;
                                }
                                break;
                            }
                        }


                    }
                }
                /*
                    * проверка на КЗ-грамматику
                    */
                if (contextFreeGrammar == false && aGrammar == false)
                //если не А- и не КС-грамматики, то проверяем на КЗ-грамматику
                {
                    index++;
                    var k = 0;
                    /*
                        * проверка на одиннаковые 1 и 2 части
                        */
                    if (legthFirstUnitRule == legthSecontUnitRule)
                    {
                        for (int i = 0; i <= legthFirstUnitRule; i++)
                        {
                            if (k == legthFirstUnitRule)
                            {
                                errorRule = true;
                                break;
                            }
                            if (firstUnitRule[k] == secontUnitRule[k])
                            {
                                k++;
                            }
                        }
                    }

                    k = 0;

                    if (errorRule)
                    {
                        MessageBox.Show("1 часть правил и 2 одинаковы!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    if (legthSecontUnitRule >= legthFirstUnitRule - 1 && errorRule == false)
                    // если размер 2 части правила больше размера 1 части правила и нет одиннаковой 1 и 2 части
                    {
                        while (theLeftEndOfContext == false) // нахождение левого контекста
                        {
                            if (k > legthFirstUnitRule || k > legthSecontUnitRule)
                            {
                                indexLeftContext = k - 1;
                                break;
                            }
                            if (firstUnitRule[k] == secontUnitRule[k])
                            {
                                leftContext += 1;
                                // накапливаем индекс левого контекста
                                k++;
                            }
                            else
                            {
                                indexLeftContext = k;
                                theLeftEndOfContext = true;
                            }
                        }

                        --k;
                        int index1 = legthFirstUnitRule;  // начинаем с конца каждого правила
                        int index2 = legthSecontUnitRule;
                        while (theRihtEndOfContext == false) // нахождение правого контекста
                        {
                            if (index1 < 0 || index2 < 0)
                            {
                                index1 += 1;
                                break;
                            }

                            if (firstUnitRule[index1] == secontUnitRule[index2])
                            {
                                rightContext += 1;
                                index1 -= 1;
                                index2 -= 1;
                            }
                            else
                            {
                                indexRightContext = index2;
                                theRihtEndOfContext = true;
                            }
                        }

                        index--;
                        legthFirstUnitRule++;
                        legthSecontUnitRule++;
                        if (legthFirstUnitRule < legthSecontUnitRule)
                        {
                            if (indexLeftContext > indexRightContext)
                            {
                                index1 += 1;
                            }
                        }

                        if (leftContext > 0 || rightContext > 0) // когда контекст находится и справа и слева
                        {
                            for (int y = 0; y < 26; y++)
                            {
                                char symbol = Convert.ToChar(_all[y]);
                                char symbolGrammar = Convert.ToChar(firstUnitRule[index1]); 
                                if (symbol == symbolGrammar)
                                {
                                    contextSensitiveGrammar = true;
                                    var resultContextSensitiveGrammar = 3;   //результат конечный и 4 промежуточных
                                    if (resultContextSensitiveGrammar > result)
                                    {
                                        result = resultContextSensitiveGrammar;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                    
                if (contextSensitiveGrammar == false && contextFreeGrammar == false && aGrammar == false)
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
        }

        private void button_clearAll_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            int select = listBox1.SelectedIndex;
            if (select == -1)
            {
                MessageBox.Show("Не выбран элемент для удаления", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
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
            textBox2.Text = Convert.ToString(BTep.Content);
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
            char epsilon = Convert.ToChar(BTep.Content);
            string pEpsilon = Convert.ToString(textBox2.Text);
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
                    String text = textBox2.Text;
                    text += symbol;
                    textBox2.Text = text;
                }
            }
        }



        private void AddSimbol2(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                String symbol = Convert.ToString(button.Content);
                String text = textBox1.Text;
                text += symbol;
                textBox1.Text = text;
            }
        }
    }
}
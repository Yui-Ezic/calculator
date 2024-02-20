using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        double memory = 0;
        bool func = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "0";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = Convert.ToString(Calc(richTextBox1.Text));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button24_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "sin(";
            
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            memory = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "2";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "3";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "4";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "5";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "6";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "7";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "8";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "9";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += ",";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(richTextBox1.Text);
            double y = Math.Sqrt(x);
            richTextBox1.Text = Convert.ToString(y);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(richTextBox1.Text);
            double y = x * x;
            richTextBox1.Text = Convert.ToString(y);
        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (func == false)
            {
                richTextBox1.Text += "+";
            }
            else
            {
                richTextBox1.Text += ")+";
                func = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (func == false)
            {
                richTextBox1.Text += "-";
            }
            else
            {
                richTextBox1.Text += ")-";
                func = false;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (func == false)
            {
                richTextBox1.Text += "*";
            }
            else
            {
                richTextBox1.Text += ")*";
                func = false;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (func == false)
            {
                richTextBox1.Text += "";
            }
            else
            {
                richTextBox1.Text += ")/";
                func = false;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
           richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
        }

        public static double Calc(string s)
        {
            s = '(' + s + ')';
            Stack<double> Operands = new Stack<double>();
            Stack<char> Functions = new Stack<char>();
            int pos = 0;
            object token;
            object prevToken = 'Ы';

            do
            {
                token = getToken(s, ref pos);
                // разрливаем унарный + и -
                if (token is char && prevToken is char &&
                    // если эту сточку заменить на (char)prevToken != ')' &&
                    // то можно будет писать (2 * -5) или даже (2 - -4)
                    // но нужно будет ввести ещё одну проверку так, как запись 2 + -+-+-+2 тоже будет работать :)
                    (char)prevToken == '(' &&
                    ((char)token == '+' || (char)token == '-'))
                    Operands.Push(0); // Добавляем нулевой элемент

                if (token is double) // Если операнд
                {
                    Operands.Push((double)token); // то просто кидаем в стек
                }
                // в данном случае у нас только числа и операции. но можно добавить функции, переменные и т.д. и т.п.
                else if (token is char) // Если операция
                {
                    if ((char)token == ')')
                    {
                        // Скобка - исключение из правил. выталкивает все операции до первой открывающейся
                        while (Functions.Count > 0 && Functions.Peek() != '(')
                            popFunction(Operands, Functions);
                        Functions.Pop(); // Удаляем саму скобку "("
                    }
                    else
                    {
                        while (canPop((char)token, Functions)) // Если можно вытолкнуть
                            popFunction(Operands, Functions); // то выталкиваем

                        Functions.Push((char)token); // Кидаем новую операцию в стек
                    }
                }
                prevToken = token;
            }
            while (token != null);

            if (Operands.Count > 1 || Functions.Count > 0)
                throw new Exception("Ошибка в разборе выражения");

            return Operands.Pop();
        }

        private static void popFunction(Stack<double> Operands, Stack<char> Functions)
        {
            double B = Operands.Pop();
            double A = Operands.Pop();
            switch (Functions.Pop())
            {
                case '+':
                    Operands.Push(A + B);
                    break;
                case '-':
                    Operands.Push(A - B);
                    break;
                case '*':
                    Operands.Push(A * B);
                    break;
                case '/':
                    Operands.Push(A / B);
                    break;
            }
        }

        private static bool canPop(char op1, Stack<char> Functions)
        {
            if (Functions.Count == 0)
                return false;
            int p1 = getPriority(op1);
            int p2 = getPriority(Functions.Peek());

            return p1 >= 0 && p2 >= 0 && p1 >= p2;
        }

        private static int getPriority(char op)
        {
            switch (op)
            {
                case '(':
                    return -1; // не выталкивает сам и не дает вытолкнуть себя другим
                case '*':
                case '/':
                    return 1;
                case '+':
                case '-':
                    return 2;
                default:
                    throw new Exception("недопустимая операция");
            }
        }

        private static object getToken(string s, ref int pos)
        {
            readWhiteSpace(s, ref pos);

            if (pos == s.Length) // конец строки
                return null;
            if (char.IsDigit(s[pos]))
                return Convert.ToDouble(readDouble(s, ref pos));
            else if (FunctionDefinition(s, ref pos))
                return getFunction(s, ref pos);
            else
                return readFunction(s, ref pos);
        }

        private static char readFunction(string s, ref int pos)
        {
            // в данном случае все операции состоят из одного символа
            // но мы можем усложнить код добавив == && || mod div и ещё чегонить
            return s[pos++];
        }

        private static string readDouble(string s, ref int pos)
        {
            string res = "";
            while (pos < s.Length && (char.IsDigit(s[pos]) || s[pos] == ','))
                res += s[pos++];

            return res;
        }

        // Считывает все проблемы и прочие левые символы.
        private static void readWhiteSpace(string s, ref int pos)
        {
            while (pos < s.Length && char.IsWhiteSpace(s[pos]))
                pos++;
        }

        private static bool FunctionDefinition(string s, ref int pos)
        {
            if (s[pos] == 's' && s[pos + 1] == 'q' && s[pos + 2] == 'r' && s[pos + 3] == 't')
            {
                return true;
            }
            else if (s[pos] == 's' && s[pos + 1] == 'i' && s[pos + 2] == 'n')
            {
                return true;
            }
            else if (s[pos] == 'c' && s[pos + 1] == 'o' && s[pos + 2] == 's')
            {
                return true;
            }
            else if (s[pos] == 't' && s[pos + 1] == 'a' && s[pos + 2] == 'n')
            {
                return true;
            }
            else if (s[pos] == 'a' && s[pos + 1] == 'b' && s[pos + 2] == 's')
            {
                return true;
            }
            else if (s[pos] == 'l' && s[pos + 1] == 'n')
            {
                return true;
            }
            else if (s[pos] == 'l' && s[pos + 1] == 'g')
            {
                return true;
            }
            else return false;
        }

        private static double getFunction(string s, ref int pos)
        {
            double rez = 0;
            if (s[pos] == 's' && s[pos + 1] == 'q' && s[pos + 2] == 'r' && s[pos + 3] == 't')
            {
                pos += 5; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Sqrt(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Sqrt(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 's' && s[pos + 1] == 'i' && s[pos + 2] == 'n')
            {
                pos += 4; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Sin(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Sin(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 'c' && s[pos + 1] == 'o' && s[pos + 2] == 's')
            {
                pos += 4; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Cos(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Cos(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 't' && s[pos + 1] == 'a' && s[pos + 2] == 'n')
            {
                pos += 4; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Tan(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Tan(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 'a' && s[pos + 1] == 'b' && s[pos + 2] == 's')
            {
                pos += 4; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Abs(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Abs(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 'l' && s[pos + 1] == 'n')
            {
                pos += 3; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Log(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Log(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else if (s[pos] == 'l' && s[pos + 1] == 'g')
            {
                pos += 3; // с учитыванием скобки
                if (s[pos] == '-')
                {
                    pos++;
                    rez = Math.Log10(Convert.ToDouble(readDouble(s, ref pos)) * -1);
                }
                else
                    rez = Math.Log10(Convert.ToDouble(readDouble(s, ref pos)));
                pos++; // скобка после числа
                return rez;
            }
            else return rez;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "cos(";
        }

        private void button23_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "tan(";
        }

        private void button31_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "abs(";
        }

        private void button35_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "ln(";
        }

        private void button37_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "lg(";
        }

        private void button20_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += ")";
            func = false;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            func = true;
            richTextBox1.Text += "sin(";
        }
    }
}

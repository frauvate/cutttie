using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private int gridSize;
        private bool isTwoPlayer;
        private Button[,] buttons;
        private bool isPlayerXTurn = true;
        private int moveCount = 0;

        // Constructor
        public Form1(int gridSize, bool isTwoPlayer)
        {
            InitializeComponent();
            this.gridSize = gridSize;
            this.isTwoPlayer = isTwoPlayer;
            buttons = new Button[gridSize, gridSize];
            InitializeBoard();
        }

        // Board Initialization
        private void InitializeBoard()
        {
            // Buton boyutunu form genişliğine göre ayarlama
            int buttonSize = this.ClientSize.Width / gridSize;

            // Grid boyutunu, her bir buton için bir döngüyle yerleştirme
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(i * buttonSize, j * buttonSize),
                        Font = new Font("Arial", 24),
                        Text = ""
                    };

                    buttons[i, j].Click += Button_Click;
                    this.Controls.Add(buttons[i, j]);
                }
            }
        }

        // Button Click Event
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            // Eğer buton boşsa, oyun işaretini yerleştir
            if (clickedButton.Text == "")
            {
                clickedButton.Text = isPlayerXTurn ? "X" : "O";
                moveCount++;

                // Kazanma durumunu kontrol et
                GameOver();

                // Beraberlik durumu kontrolü
                if (moveCount == gridSize * gridSize && !CheckForWin("X") && !CheckForWin("O"))
                {
                    MessageBox.Show("Berabere!");
                    RestartGame();
                }
                else
                {
                    // Sıra değişiyor
                    isPlayerXTurn = !isPlayerXTurn;

                    // Eğer bilgisayara karşı oynanıyorsa ve sıradaki oyuncu 'O' ise, bilgisayarın hamlesini yap
                    if (!isTwoPlayer && !isPlayerXTurn)
                    {
                        ComputerMove();
                    }
                }
            }
        }

        // Bilgisayar Hamlesi (Basit Yapay Zeka)
        private void ComputerMove()
        {
            // Basit bir yapay zeka algoritması: Boş bir alanı 'O' ile doldur
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (buttons[i, j].Text == "")
                    {
                        buttons[i, j].Text = "O";
                        moveCount++;
                        if (CheckForWin("O"))
                        {
                            MessageBox.Show("Bilgisayar kazandı!");
                            RestartGame();
                        }
                        isPlayerXTurn = true; // Bilgisayar hamlesi sonrası X oyuncusuna sıra geçiyor
                        return;
                    }
                }
            }
        }

        // Kazanma Durumu Kontrolü
        private bool CheckForWin(string symbol)
        {
            // Satır, sütun ve çapraz kontrolü
            for (int i = 0; i < gridSize; i++)
            {
                if (CheckRow(i, symbol) || CheckColumn(i, symbol)) return true;
            }
            return CheckDiagonals(symbol);
        }


        // Satır Kontrolü
        private bool CheckRow(int row, string symbol)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (buttons[row, i].Text != symbol) return false;
            }
            return true;
        }

        // Sütun Kontrolü
        private bool CheckColumn(int col, string symbol)
        {
            for (int i = 0; i < gridSize; i++)
            {
                if (buttons[i, col].Text != symbol) return false;
            }
            return true;
        }

        // Çapraz Kontrolü
        private bool CheckDiagonals(string symbol)
        {
            bool diagonal1 = true, diagonal2 = true;
            for (int i = 0; i < gridSize; i++)
            {
                if (buttons[i, i].Text != symbol) diagonal1 = false;
                if (buttons[i, gridSize - i - 1].Text != symbol) diagonal2 = false;
            }
            return diagonal1 || diagonal2;
        }

        private void GameOver()
        {
            string winner = "";

            // Kazanan oyuncu kontrolü
            if (CheckForWin("X"))
            {
                winner = "X";
            }
            else if (CheckForWin("O"))
            {
                winner = "O";
            }
            // Beraberlik durumu
            else if (moveCount == gridSize * gridSize)
            {
                winner = "Draw"; // Beraberlik
            }

            // Eğer bir kazanan varsa ya da beraberlik durumu oluştuysa, sonucu göstermek için MessageBox kullanıyoruz
            if (winner != "")
            {
                string message = winner == "Draw" ? "Beraberlik! Yeni oyun başlatmak ister misiniz?" : winner + " kazandı! Yeni oyun başlatmak ister misiniz?";
                DialogResult result = MessageBox.Show(message, "Oyun Bitti", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    // Yeniden başlatmak için, oyunu sıfırlıyoruz
                    RestartGame();
                }
                else if (result == DialogResult.No)
                {
                    // Ana menüye dönüyoruz
                    AnaMenu mainMenu = new AnaMenu();
                    mainMenu.ShowDialog();
                    this.Hide();
                }
            }
        }


        // Oyunu Yeniden Başlatma
        private void RestartGame()
        {
            // Tüm butonları sıfırla
            foreach (Button button in buttons)
            {
                button.Text = "";
            }
            isPlayerXTurn = true; // Oyunun başında X ile başla
            moveCount = 0; // Hamle sayısını sıfırla
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Uygulamayı tamamen kapatır.
        }
    }
}

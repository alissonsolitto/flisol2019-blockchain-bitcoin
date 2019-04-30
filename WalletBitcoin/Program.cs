using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using WalletBitcoin.Bitcoin;

namespace WalletBitcoin
{
    class Program
    {
        static void Main(string[] args)
        {

            int menuOption = 0;

            do
            {
                menuOption = DisplayMenu();

                switch (menuOption)
                {

                    case 1: //Gerando uma chave privada
                        {
                            try
                            {
                                Console.WriteLine("Informe a senha para criar sua chave privada: ");
                                var senha = Console.ReadLine();

                                var generatekey = new GenerateKey();

                                var wordList = generatekey.GetWordList(); //Gerando lista de palavras
                                var pkey = generatekey.GetPrivateKey(wordList.ToString(), senha); //Gerando chave com senha

                                Console.WriteLine($"\nAnote a lista de palavras: {wordList.ToString()}");
                                Console.WriteLine($"\nAnote o seu endereço na rede: {pkey.PrivateKey.PubKey.GetAddress(Network.TestNet)}");
                                Console.WriteLine("\nSalve essas informações para recuperar a sua chave!");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                            }

                            break;
                        }

                    case 2: //Recuperando chave privada
                        {
                            try
                            {
                                Console.WriteLine("Informe o conjunto de 12 palavras: ");
                                var words = Console.ReadLine();

                                Console.WriteLine("Informe a senha da chave privada: ");
                                var senha = Console.ReadLine();

                                var generatekey = new GenerateKey();
                                var pkey = generatekey.GetPrivateKey(words, senha); //Gerando chave com senha

                                Console.WriteLine($"\nSeu endereço na rede é: {pkey.PrivateKey.PubKey.GetAddress(Network.TestNet)}");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                            }

                            break;
                        }

                    case 3: //Obtendo saldo do endereço
                        {
                            try
                            {
                                Console.WriteLine("Informe o endereço na rede: ");
                                var address = Console.ReadLine();

                                var account = new AccountBitcoin();
                                account.GetBalance(address);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                            }

                            break;
                        }
                    case 4: //Obtendo histórico de transações do endereço
                        {
                            try
                            {
                                Console.WriteLine("Informe o endereço na rede: ");
                                var address = Console.ReadLine();

                                var account = new AccountBitcoin();
                                account.GetHistory(address);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                            }

                            break;
                        }
                    case 5: //Criando uma transação na rede
                        {
                            try
                            {
                                Console.WriteLine("Informe o conjunto de 12 palavras: ");
                                var words = Console.ReadLine();

                                Console.WriteLine("Informe a senha da chave privada: ");
                                var senha = Console.ReadLine();

                                Console.WriteLine("Informe o endereço para envio: ");
                                var addressSend = Console.ReadLine();

                                Console.WriteLine("Informe a quantidade de SATOSHIS para envio: ");
                                var moneySend = Console.ReadLine();

                                Console.WriteLine("Informe a quantidade de SATOSHIS para a taxa: ");
                                var fee = Console.ReadLine();


                                //Recuperando chave privada
                                var generatekey = new GenerateKey();
                                var pkey = generatekey.GetPrivateKey(words, senha); //Gerando chave com senha

                                //Criando a transação
                                var transaction = new TransactionBitcoin();
                                transaction.SendTransaction(
                                    pkey,
                                    addressSend,
                                    new Money(Convert.ToInt64(moneySend)),
                                    new Money(Convert.ToInt64(fee))
                                    );

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                            }

                            break;
                        }

                    default:
                        break;
                }


            } while (menuOption != 6);
        }

        static public int DisplayMenu()
        {
            try
            {
                Console.WriteLine("========================================");
                Console.WriteLine("Wallet Bitcoin");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Criar um endereço");
                Console.WriteLine("2. Recuperar um endereço");
                Console.WriteLine("3. Obter o saldo");
                Console.WriteLine("4. Visualizar histórico de transações");
                Console.WriteLine("5. Criar um transação");
                Console.WriteLine("6. Encerrar o programa");

                var result = Console.ReadLine();
                Console.Clear();
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
                return 0;
            }
        }
    }
}

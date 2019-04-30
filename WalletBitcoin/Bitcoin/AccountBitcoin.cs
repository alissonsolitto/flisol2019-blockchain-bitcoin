using NBitcoin;
using QBitNinja.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBitcoin.Bitcoin
{
    public class AccountBitcoin
    {
        public void GetBalance(string address)
        {
            Money SpentCoins = 0;
            Money ReceivedCoins = 0;

            //Criando address BTC
            BitcoinAddress btcAddress = BitcoinAddress.Create(address, Network.TestNet);

            //Criando Client QBitNinjaClient
            var client = new QBitNinjaClient(Network.TestNet);

            //Percorre todas as transações do endereço
            client.GetBalance(btcAddress).Result.Operations.ToList().ForEach(x =>
            {
                if (x.Amount > 0) //Recebido
                    ReceivedCoins += x.Amount;
                else //Enviado
                    SpentCoins += x.Amount;
            });

            Console.WriteLine("Recebidos: " + ReceivedCoins.ToUnit(MoneyUnit.BTC));
            Console.WriteLine("Enviados: " + SpentCoins.ToUnit(MoneyUnit.BTC));
            Console.WriteLine("Total: " + (ReceivedCoins + SpentCoins).ToUnit(MoneyUnit.BTC));
        }

        public void GetHistory(string address)
        {
            //Criando address BTC
            BitcoinAddress btcAddress = BitcoinAddress.Create(address, Network.TestNet);

            //Criando Client QBitNinjaClient
            var client = new QBitNinjaClient(Network.TestNet);

            //Percorre todas as transações do endereço
            client.GetBalance(btcAddress).Result.Operations.ToList().ForEach(x =>
            {
                //Detalhes da transação
                var tran = client.GetTransaction(x.TransactionId).Result;

                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("TransactionId: " + x.TransactionId.ToString());
                Console.WriteLine("Amount: " + x.Amount.ToUnit(MoneyUnit.BTC));
                Console.WriteLine("BlockId: " + x.BlockId.ToString());
                Console.WriteLine("Height: " + x.Height);
                Console.WriteLine("Confirmations: " + x.Confirmations);
                Console.WriteLine("Fees: " + tran.Fees.ToUnit(MoneyUnit.BTC));                
            });            
        }
    }
}

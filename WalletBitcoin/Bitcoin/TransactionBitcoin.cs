using NBitcoin;
using NBitcoin.Policy;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletBitcoin.Bitcoin
{
    public class TransactionBitcoin
    {
        public void SendTransaction(ExtKey pkey, string addressSend, Money moneySend, Money fee)
        {
            var btcSecret = pkey.PrivateKey.GetBitcoinSecret(Network.TestNet);

            //Enviando para...
            BitcoinAddress btcAddress = BitcoinAddress.Create(addressSend, Network.TestNet);

            List<ICoin> lstCoin = new List<ICoin>();

            //Buscando TODAS saidas validas para gastar
            var client = new QBitNinjaClient(Network.TestNet);
            client.GetBalance(btcSecret.GetAddress(), true).Result.Operations.ToList().ForEach(x =>
            {
                if (x.Confirmations > 0)
                {
                    x.ReceivedCoins.ForEach(r =>
                    {
                        if (r.TxOut.Value > Money.Satoshis(0))
                        {
                            lstCoin.Add(new Coin()
                            {
                                Outpoint = r.Outpoint,
                                TxOut = r.TxOut
                            });
                        }
                    });
                }
            });

            //Convertendo as saidas em ICOIN
            Coin[] coin = lstCoin.Select((o, i) => new Coin(o.Outpoint, o.TxOut)).ToArray();

            //TransactionBuilder montando a transação complexa
            TransactionBuilder txBuilder = new TransactionBuilder();

            //Politica que define a taxa minima de transacao
            txBuilder.StandardTransactionPolicy.MinRelayTxFee = FeeRate.Zero;
            //False para permitir transações com poucos satoshis
            txBuilder.DustPrevention = false;

            Transaction tx = txBuilder
                .AddKeys(btcSecret.PrivateKey)
                .AddCoins(coin)                
                .Send(btcAddress.ScriptPubKey, moneySend)
                .SetChange(btcSecret.ScriptPubKey)
                .SendFees(fee)
                .BuildTransaction(true);

            BroadcastResponse broadcastResponse = client.Broadcast(tx).Result;

            if (!broadcastResponse.Success)
            {
                Console.WriteLine(string.Format("Code: {0}", broadcastResponse.Error.ErrorCode));
                Console.WriteLine("Mensagem: " + broadcastResponse.Error.Reason);
            }
            else
            {
                Console.WriteLine("Sucesso ====> https://live.blockcypher.com/btc-testnet/tx/" + tx.GetHash().ToString());
            }
        }
    }
}

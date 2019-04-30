using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;

namespace WalletBitcoin.Bitcoin
{
    public class GenerateKey
    {
        public Mnemonic GetWordList()
        {
            Mnemonic mnemo = new Mnemonic(Wordlist.PortugueseBrazil, WordCount.Twelve);
            return mnemo;
        }

        public ExtKey GetPrivateKey(string mnemoic, string senha)
        {
            var mnemo = new Mnemonic(mnemoic, Wordlist.PortugueseBrazil);
            ExtKey pkey = mnemo.DeriveExtKey(senha);

            return pkey;
        }        
    }
}

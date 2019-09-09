using AuthorizeTransaction.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizeTransaction
{
    public class Program
    {
        public static Account Account;
        public static List<Input> Inputs;

        public Program()
        {
            Account = new Account();
            Inputs = new List<Input>();
        }

        public static void Main(string[] args)
        {
            try
            {
                ReadInput();
                AuthorizeOperations();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void ReadInput()
        {
            Inputs = new List<Input>();
            Console.WriteLine("Cat operations");
            do
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line)) { break; }
                Inputs.Add(JsonConvert.DeserializeObject<Input>(line));
            } while (true);
        }

        private static void AuthorizeOperations()
        {
            var array = new Output[Inputs.Count];

            Console.WriteLine("Authorize < operations");
            for (int i = 0; i < Inputs.Count; i++)
            {
                Input line = Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = AccountCreation(line.Account);
                }

                array[i].Account = Account;
                Console.WriteLine(JsonConvert.SerializeObject(array[i]));
            }
        }

        public static Output TransactionAuthorization(Transaction transaction)
        {
            var output = new Output() { Violations = new string[0] };

            if (!Account.ActiveCard)
            {
                output.Violations = new string[] { "card-not-active" };
                return output;
            }

            if (HighFrequency(transaction))
            {
                output.Violations = new string[] { "high-frequency-small-interval" };
                return output;
            }

            if (DoubledTransaction(transaction))
            {
                output.Violations = new string[] { "doubled-transaction" };
                return output;
            }

            if (Account.AvailableLimit - transaction.Amount < 0)
            {
                output.Violations = new string[] { "insufficient-limit" };
            }
            else
            {
                Account.AvailableLimit -= transaction.Amount;
            }

            return output;
        }

        public static Output AccountCreation(Account account)
        {
            var output = new Output() { Violations = new string[0] };

            if (Account.ActiveCard)
            {
                output.Violations = new string[] { "account-already-initialized" };
            }
            else
            {
                Account.ActiveCard = account.ActiveCard;
                Account.AvailableLimit = account.AvailableLimit;
            }

            return output;
        }

        public static bool HighFrequency(Transaction current)
        {
            return TransactionsOnTwoMinutes(current) > 3;
        }

        public static int TransactionsOnTwoMinutes(Transaction current)
        {
            DateTime minutes = current.Time.AddMinutes(-2);
            int transactions = Inputs.Where(c => c.Transaction.Time >= minutes && c.Transaction.Time <= current.Time).Count();
            return transactions;
        }

        public static bool DoubledTransaction(Transaction current)
        {
            int transactions = TransactionsOnTwoMinutes(current);

            if (transactions > 2)
            {
                return Inputs.Where(c => c.Transaction.Merchant == current.Merchant && c.Transaction.Amount == current.Amount).Count() > 2;
            }
            return false;
        }
    }
}

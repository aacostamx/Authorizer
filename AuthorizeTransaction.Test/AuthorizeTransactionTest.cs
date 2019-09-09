using AuthorizeTransaction.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace AuthorizeTransaction.Test
{
    public class AuthorizeTransactionTest
    {
        [Fact]
        public void Transactions()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 100 } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Burger King\", \"amount\": 20, \"time\": \"2019-02-13T10:00:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Habbib's\", \"amount\": 60, \"time\": \"2019-02-13T11:00:00.000Z\" } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(3, array.Length);
            Assert.True(Program.Account.ActiveCard);
            Assert.Equal(20, Program.Account.AvailableLimit);
        }

        [Fact]
        public void Account_Already_Initialized()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 100 } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Burger King\", \"amount\": 20, \"time\": \"2019-02-13T10:00:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 100 } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(3, array.Length);
            Assert.True(Program.Account.ActiveCard);
            Assert.Equal(80, Program.Account.AvailableLimit);
            Assert.Equal("account-already-initialized", array[2].Violations[0]);
        }

        [Fact]
        public void Account_Insufficient_Limit()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 100 } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Burger King\", \"amount\": 90, \"time\": \"2019-02-13T10:00:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Habbib's\", \"amount\": 90, \"time\": \"2019-02-13T11:00:00.000Z\" } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(3, array.Length);
            Assert.True(Program.Account.ActiveCard);
            Assert.Equal(10, Program.Account.AvailableLimit);
            Assert.Equal("insufficient-limit", array[2].Violations[0]);
        }

        [Fact]
        public void Card_Not_Active()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Burger King\", \"amount\": 20, \"time\": \"2019-02-13T10:00:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Habbib's\", \"amount\": 90, \"time\": \"2019-02-13T11:00:00.000Z\" } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(2, array.Length);
            Assert.False(Program.Account.ActiveCard);
            Assert.Equal(0, Program.Account.AvailableLimit);
            Assert.Equal("card-not-active", array[1].Violations[0]);
        }

        [Fact]
        public void High_Frequency_Small_Interval()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 1000 } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Burger King\", \"amount\": 150, \"time\": \"2019-02-13T10:00:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Rapi\", \"amount\": 200, \"time\": \"2019-02-13T10:01:00.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Uber Eats\", \"amount\": 200, \"time\": \"2019-02-13T10:01:25.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Uber\", \"amount\": 40, \"time\": \"2019-02-13T10:01:26.000Z\" } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(5, array.Length);
            Assert.True(Program.Account.ActiveCard);
            Assert.Equal(450, Program.Account.AvailableLimit);
            Assert.Equal("high-frequency-small-interval", array[4].Violations[0]);
        }

        [Fact]
        public void Doubled_Transaction()
        {
            // Arrange
            Program.Account = new Account();
            Program.Inputs = new List<Input>();
            Program.Inputs = new List<Input>
            {
                JsonConvert.DeserializeObject<Input>("{ \"account\": { \"activeCard\": true, \"availableLimit\": 1000 } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Uber\", \"amount\": 40, \"time\": \"2019-02-13T10:01:26.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Uber\", \"amount\": 40, \"time\": \"2019-02-13T10:01:27.000Z\" } }"),
                JsonConvert.DeserializeObject<Input>("{ \"transaction\": { \"merchant\": \"Uber\", \"amount\": 40, \"time\": \"2019-02-13T10:01:28.000Z\" } }")
            };
            var array = new Output[Program.Inputs.Count];

            // Act
            for (int i = 0; i < Program.Inputs.Count; i++)
            {
                Input line = Program.Inputs[i];

                if (line.Transaction.Amount > 0
                    && !string.IsNullOrEmpty(line.Transaction.Merchant))
                {
                    array[i] = Program.TransactionAuthorization(line.Transaction);
                }
                else
                {
                    array[i] = Program.AccountCreation(line.Account);
                }

                array[i].Account = Program.Account;
            }

            // Assert
            Assert.NotNull(array);
            Assert.Equal(4, array.Length);
            Assert.True(Program.Account.ActiveCard);
            Assert.Equal(920, Program.Account.AvailableLimit);
            Assert.Equal("doubled-transaction", array[3].Violations[0]);
        }

    }
}

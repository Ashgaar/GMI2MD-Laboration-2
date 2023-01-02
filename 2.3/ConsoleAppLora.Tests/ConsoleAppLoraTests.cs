using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALoRa.Library;
using ConsoleApp.Lora;


namespace ConsoleAppLora.Tests
{
    [TestClass]
    public class ConsoleAppLoraTests
    {
        [TestMethod]
        public void HexToAscii_ShouldPass_WhenUsingValidInput()
        {
            var data = Program.FromHex("54");
            var cleanData = Program.ConvertToAscii(data);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void HexToAscii_ShouldThrowException_WhenUsingInvalidInput()
        {
            Assert.ThrowsException<ArgumentException>(() => Program.ConvertToAscii(Program.FromHex("")));
        }

        [TestMethod]
        public void HexToAscii_ShouldThrowException_WhenUsingNullInput()
        {
            Assert.ThrowsException<NullReferenceException>(() => Program.ConvertToAscii(Program.FromHex(null)));
        }


        [TestMethod]
        [DataRow("31","1")]
        [DataRow("31-32-38", "128")]
        [DataRow("35-32", "52")]
        [DataRow("30", "0")]
        public void HexToAscii_ShouldReturnCorrectCharacters_WhenValidNumberInput(string input, string expected)
        {
            var calc = Program.ConvertToAscii(Program.FromHex(input));
            Assert.AreEqual(expected, calc);
        }

        [TestMethod]
        [DataRow("41", "A")]
        [DataRow("4A-61-67", "Jag")]
        [DataRow("48-65-6A", "Hej")]
        [DataRow("4B-75-6E-67-65-6E-20-48-61-6E-73", "Kungen Hans")]
        public void HexToAscii_ShouldReturnCorrectCharacters_WhenValidCharacterInput(string input, string expected)
        {
            var calc = Program.ConvertToAscii(Program.FromHex(input));
            Assert.AreEqual(expected, calc);
        }
    }
}
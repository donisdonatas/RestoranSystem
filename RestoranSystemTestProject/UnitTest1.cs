using RestoranSystem.Utilities;

namespace RestoranSystemTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void ConvertToBool_InputInteger_OutputBool()
        {
            int num = 1;
            int NumToBeTrue = 1;

            bool Answer = Converter.ConvertToBool(num, NumToBeTrue);
            Assert.True(Answer, "Skaičius paverstas į Loginę reikšmę.");
        }

        [Fact]
        public void ConvertDecimalToReal_InputDecimal_OutputString()
        {
            decimal NumDecimal = 10.5m;
            string Result = Converter.ConvertDecimalToReal(NumDecimal);
            Assert.Equal("10.5", Result);
        }
    }
}
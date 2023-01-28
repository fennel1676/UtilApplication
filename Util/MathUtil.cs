namespace AKI.Util
{
    public class MathUtil
    {
        public static int Factorial(int nNum)
        {
            return (nNum <= 1) ? 1 : nNum * Factorial(nNum - 1);
        }
    }
}

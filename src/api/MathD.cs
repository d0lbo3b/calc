namespace calculator.api;

public static class MathD {
    public static long Factorial(long a) {
        if (a <= 0) return 1;

        long fact = 1;
        for (var i = 1; i < a+1; i++) {
            fact *= i;
        }
        return fact;
    }

    public static double Sin(double degrees) {
        degrees %= 360;
        return (float)Math.Sin(Math.PI*degrees/180.0);
    }
    
    public static double Cos(double degrees) {
        degrees %= 360;
        if (degrees is 90 or -90) return 0;
        return (float)Math.Cos(Math.PI*degrees/180.0);
    }
    
    public static double Tan(double degrees) {
        degrees %= 360;
        if (degrees is 0 or 180) return 0;
        return (float)Math.Tan(Math.PI*degrees/180.0);
    }
}
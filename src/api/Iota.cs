namespace calculator.api;

public static class Iota {
    private static int _iota;


    public static int GetIota(bool reset = false) {
        var iota = _iota++;
        if (reset) {
            IotaReset();
        }
        return iota;
    }

    public static void IotaReset() {
        _iota = 0;
    }
}
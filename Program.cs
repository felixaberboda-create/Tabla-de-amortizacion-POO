using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        decimal monto = CapturarMonto();
        decimal interesAnual = CapturarInteres();
        int meses = CapturarMeses();

        decimal cuota = CalcularCuota(monto, interesAnual, meses);
        GenerarTabla(monto, interesAnual, meses, cuota);

        Console.ReadKey();
    }

    // ------------------ CAPTURAR MONTO ------------------
    static decimal CapturarMonto()
    {
        decimal monto;
        string input;

        do
        {
            input = AnsiConsole.Ask<string>("Ingrese el monto del préstamo:");

            if (!decimal.TryParse(input, out monto) || monto <= 0)
            {
                AnsiConsole.MarkupLine("[red]⚠ Error: Ingrese un monto válido mayor que 0[/]");
            }

        } while (monto <= 0);

        return monto;
    }

    // ------------------ CAPTURAR INTERES ------------------
    static decimal CapturarInteres()
    {
        decimal interes;
        string input;

        do
        {
            input = AnsiConsole.Ask<string>("Ingrese la tasa de interés anual (%):");

            if (!decimal.TryParse(input, out interes) || interes <= 0)
            {
                AnsiConsole.MarkupLine("[red]⚠ Error: Ingrese una tasa válida mayor que 0[/]");
            }

        } while (interes <= 0);

        return interes;
    }

    // ------------------ CAPTURAR MESES ------------------
    static int CapturarMeses()
    {
        int meses;
        string input;

        do
        {
            input = AnsiConsole.Ask<string>("Ingrese el plazo en meses:");

            if (!int.TryParse(input, out meses) || meses <= 0)
            {
                AnsiConsole.MarkupLine("[red]⚠ Error: Ingrese un número entero mayor que 0[/]");
            }

        } while (meses <= 0);

        return meses;
    }

    // ------------------ CALCULAR CUOTA ------------------
    static decimal CalcularCuota(decimal monto, decimal interesAnual, int meses)
    {
        decimal tasaMensual = (interesAnual / 12) / 100;

        decimal potencia = (decimal)Math.Pow((double)(1 + tasaMensual), meses);

        decimal cuota = monto * (tasaMensual * potencia) / (potencia - 1);

        return Math.Round(cuota, 2);
    }

    // ------------------ GENERAR TABLA ------------------
    static void GenerarTabla(decimal monto, decimal interesAnual, int meses, decimal cuota)
    {
        decimal tasaMensual = (interesAnual / 12) / 100;
        decimal saldo = monto;

        var tabla = new Table();

        tabla.AddColumn("No. Cuota");
        tabla.AddColumn("Pago de Cuota");
        tabla.AddColumn("Interés");
        tabla.AddColumn("Abono Capital");
        tabla.AddColumn("Saldo");

        for (int i = 1; i <= meses; i++)
        {
            decimal interes = Math.Round(saldo * tasaMensual, 2);
            decimal abonoCapital = Math.Round(cuota - interes, 2);
            saldo = Math.Round(saldo - abonoCapital, 2);

            tabla.AddRow(
                i.ToString(),
                cuota.ToString("F2"),
                interes.ToString("F2"),
                abonoCapital.ToString("F2"),
                saldo.ToString("F2")
            );
        }

        AnsiConsole.Write(tabla);
    }
}